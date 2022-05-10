"use strict";
var KTDatatablesExtensionsKeytable = function () {

    var topluSecim = false;
    var initTable = function () {
        // begin first table
        var table = $('#tagsTable').DataTable({
            language: {
                "sDecimal": ",",
                "sEmptyTable": "Tabloda herhangi bir veri mevcut deðil",
                "sInfo": "_TOTAL_ kayýttan _START_ - _END_ arasýndaki kayýtlar gösteriliyor",
                "sInfoEmpty": "Kayýt yok",
                "sInfoFiltered": "(_MAX_ kayýt içerisinden bulunan)",
                "sInfoPostFix": "",
                "sInfoThousands": ".",
                "sLengthMenu": "Sayfada _MENU_ kayýt göster",
                "sLoadingRecords": "Yükleniyor...",
                "sProcessing": "Ýþleniyor...",
                "sSearch": "Ara:",
                "sZeroRecords": "Eþleþen kayýt bulunamadý",
                "oPaginate": {
                    "sFirst": "Ýlk",
                    "sLast": "Son",
                    "sNext": "Sonraki",
                    "sPrevious": "Önceki"
                },
                "oAria": {
                    "sSortAscending": ": artan sütun sýralamasýný aktifleþtir",
                    "sSortDescending": ": azalan sütun sýralamasýný aktifleþtir"
                },
                "select": {
                    "rows": {
                        "_": "%d kayýt seçildi",
                        "0": "",
                        "1": "1 kayýt seçildi"
                    }
                }
            },
            responsive: true,
            select: {
                style: 'multi',
                selector: 'td:first-child .checkable'
            },
            headerCallback: function (thead) {
                thead.getElementsByTagName('th')[0].innerHTML = `
                    <label class="checkbox checkbox-single checkbox-solid checkbox-primary mb-0">
                        <input type="checkbox" value="" class="group-checkable"/>
                        <span></span>
                    </label>`;
                thead.getElementsByTagName('th')[0]
            },
            columnDefs: [
                {
                    targets: 0,
                    sorting: false,
                    orderable: false,
                    render: function () {
                        return `
                        <label class="checkbox checkbox-single checkbox-primary mb-0">
                            <input type="checkbox" value="" class="checkable"/>
                            <span></span>
                        </label>`;
                    },
                    "order": []
                },
                { orderable: false, targets: [0, 5] }
            ]
        });

        table.on('change', '.group-checkable', function () {
            var set = $(this).closest('table').find('td:first-child .checkable');
            var checked = $(this).is(':checked');

            $(set).each(function () {
                if (checked) {
                    $(this).prop('checked', true);
                    table.rows($(this).closest('tr')).select();
                }
                else {
                    $(this).prop('checked', false);
                    table.rows($(this).closest('tr')).deselect();
                }
            });

            var checkedNodes = table.rows('.selected').nodes();
            var count = checkedNodes.length;
            $('#kt_datatable_selected_records').html(count);
            if (count > 0) {
                $('#kt_datatable_group_action_form').collapse('show');
                topluSecim = true;
            } else {
                $('#kt_datatable_group_action_form').collapse('hide');
                topluSecim = false;
            }
        });

        table.on('change', '.checkable', function () {
            var checkedNodes = table.rows('.selected').nodes();
            var count = checkedNodes.length;
            $('#kt_datatable_selected_records').html(count);
            if (count > 0) {
                $('#kt_datatable_group_action_form').collapse('show');
                topluSecim = true;
            } else {
                $('#kt_datatable_group_action_form').collapse('hide');
                topluSecim = false;
            }
        });

        function yenile() {
            $('#kt_datatable_group_action_form').collapse('hide');
            $.ajax({
                type: 'GET',
                url: '/Admin/Etiket/TumEtiketler/',
                contentType: "application/json",
                beforeSend: function () {
                    $('#tagsTable').hide();
                    $('.spinner-border').show();
                },
                success: function (data) {
                    const tagListDto = jQuery.parseJSON(data);
                    table.clear();
                    if (tagListDto.ResultStatus === 0) {
                        $.each(tagListDto.Tags.$values,
                            function (index, tag) {
                                const newTableRow = table.row.add([
                                    tag.Id,
                                    tag.Name,
                                    `${tag.IsActive ? "Yayýnda" : "Taslak"}`,
                                    `${convertToShortDate(tag.ModifiedDate)}`,
                                    tag.ModifiedByName,
                                    `
	                        <button id="btnEdit" class="btn btn-sm btn-clean btn-icon mr-2" data-id="${tag.Id}" title="Düzenle">
	                            <span class="svg-icon svg-icon-md">
	                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
	                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">\
	                                        <rect x="0" y="0" width="24" height="24"/>\
	                                        <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero"\ transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>\
	                                        <rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>\
	                                    </g>
	                                </svg>
	                            </span>
	                        </button>
	                        <button class="btn btn-sm btn-clean btn-icon" id="btnSil" data-id="${tag.Id}" title="Sil">
	                            <span class="svg-icon svg-icon-md">\
	                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
	                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
	                                        <rect x="0" y="0" width="24" height="24"/>
	                                        <path d="M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z" fill="#000000" fill-rule="nonzero"/>
	                                        <path d="M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="#000000" opacity="0.3"/>
	                                    </g>
	                                </svg>
	                            </span>
	                        </button>`
                                ]).node();
                                const jqueryTableRow = $(newTableRow);
                                jqueryTableRow.attr('name', `${tag.Id}`);
                            });
                        table.draw();
                        $('.spinner-border').hide();
                        $('#tagsTable').fadeIn(1400);
                    } else {
                        toastr.error(`${tagListDto.Message}`, 'Ýþlem Baþarýsýz!');
                    }
                },
                error: function (err) {
                    $('.spinner-border').hide();
                    $('#tagsTable').fadeIn(1000);
                    toastr.error(`${err.responseText}`, 'Hata!');
                }
            });
        }

        $('#btnYenile').click(function () {
            yenile();
        });

        $("#btnSave").click(function () {
            $.ajax(
                {
                    type: "POST",
                    url: "/Admin/Etiket/Ekle/",
                    data: { //Passing data  
                        Name: $("#Name").val(),
                        Note: $("#Note").val(),
                        IsActive: $("#IsActive").prop('checked')
                    },
                    success: function () {
                        yenile();
                    },
                    error: function (err) {
                        toastr.error(`${err.responseText}`, "Hata!");
                    }
                });
        });

        $(function () {
            const url = '/Admin/Etiket/Duzenle';
            const tagEditModal = $('#tagEditModal');
            $(document).on('click',
                '#btnEdit',
                function (event) {
                    event.preventDefault();
                    const id = $(this).attr('data-id');
                    $.get(url, { tagId: id }).done(function (data) {
                        tagEditModal.html(data);
                        tagEditModal.find('.modal').modal('show');
                    }).fail(function () {
                        toastr.error("Bir hata oluþtu.");
                    });
                });


            tagEditModal.on('click',
                '#btnSaveModal',
                function (event) {
                    event.preventDefault();

                    const form = $('#form-tag-update');
                    const actionUrl = form.attr('action');
                    const dataToSend = form.serialize();
                    $.post(actionUrl, dataToSend).done(function (data) {
                        const tagUpdateAjaxModel = jQuery.parseJSON(data);
                        const newFormBody = $('.modal-body', tagUpdateAjaxModel.TagUpdatePartial);
                        tagEditModal.find('.modal-body').replaceWith(newFormBody);
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            const id = tagUpdateAjaxModel.TagDto.Tag.Id;
                            const tableRow = $(`[name="${id}"]`);
                            tagEditModal.find('.modal').modal('hide');
                            table.row(tableRow).data([
                                tagUpdateAjaxModel.TagDto.Tag.Id,
                                tagUpdateAjaxModel.TagDto.Tag.Name,
                                tagUpdateAjaxModel.TagDto.Tag.IsActive ? "Yayýnda" : "Taslak",
                                convertToShortDate(tagUpdateAjaxModel.TagDto.Tag.ModifiedDate),
                                tagUpdateAjaxModel.TagDto.Tag.ModifiedByName,
                                `<button id="btnEdit" class="btn btn-sm btn-clean btn-icon mr-2" title="Düzenle" data-id="${tagUpdateAjaxModel.TagDto.Tag.Id}">
                                                <span class="svg-icon svg-icon-md">
                                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                            <rect x="0" y="0" width="24" height="24" />
                                                            <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) " />
                                                            <rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1" />
                                                        </g>
                                                    </svg>
                                                </span>
                                            </button>
                                            <button id="btnSil" class="btn btn-sm btn-clean btn-icon" title="Sil" data-id="${tagUpdateAjaxModel.TagDto.Tag.Id}">
                                                <span class="svg-icon svg-icon-md">
                                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                            <rect x="0" y="0" width="24" height="24" />
                                                            <path d="M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z" fill="#000000" fill-rule="nonzero" />
                                                            <path d="M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="#000000" opacity="0.3" />
                                                        </g>
                                                    </svg>
                                                </span>
                                            </button>`
                            ]);
                            tableRow.attr("name", `${id}`);
                            table.row(tableRow).invalidate();
                            toastr.success(`${tagUpdateAjaxModel.TagDto.Message}`, "Baþarýlý Ýþlem!");
                        } else {
                            let summaryText = "";
                            $('#validation-summary > ul > li').each(function () {
                                let text = $(this).text();
                                summaryText = `*${text}\n`;
                            });
                            toastr.warning(summaryText);
                        }
                    }).fail(function (response) {
                    });
                });

        });

        $(document).on('click',
            '#btnSil',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                const tableRow = $(`[name="${id}"]`);
                const title = tableRow.find('td:eq(1)').text();
                Swal.fire({
                    title: 'Silmek istediðinize emin misiniz?',
                    text: `${title} baþlýklý etiket silinicektir!`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Evet, silmek istiyorum.',
                    cancelButtonText: 'Hayýr, silmek istemiyorum.'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            data: { tagId: id },
                            url: '/Admin/Etiket/Sil/',
                            success: function (data) {
                                const tagResult = jQuery.parseJSON(data);
                                if (tagResult.ResultStatus === 0) {
                                    Swal.fire(
                                        'Silindi!',
                                        `${tagResult.Message}`,
                                        'success'
                                    ).then(function () {
                                        location.reload();
                                    });
                                } else {
                                    Swal.fire(
                                        'Baþarýsýz Ýþlem!',
                                        `${tagResult.Message}`,
                                        'error'
                                    );
                                }
                            },
                            error: function (err) {
                                toastr.error(`${err.responseText}`, "Hata!");
                            }
                        });
                    }
                });
            });

        $('#btnTopluSil').click(function () {
            var seciliDosyalar = $('tr td label input[type=checkbox]:checked');
            if (seciliDosyalar.length > 0 && topluSecim === true) {
                var ids = [];
                seciliDosyalar.each(function () {
                    var id = parseInt($(this).closest('tr').attr('name'));
                    ids.push(id);
                });

                Swal.fire({
                    title: 'Dosya Silme Onayý',
                    text: `Seçilen ${seciliDosyalar.length} adet etiketi kalýcý olarak silmek istediðinizden emin misiniz?`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Evet, silmek istiyorum.',
                    cancelButtonText: 'Hayýr, silmek istemiyorum.'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            url: '/Admin/Etiket/TopluSil',
                            data: { tagIds: ids },
                            success: function (data) {
                                const tagResult = jQuery.parseJSON(data);
                                if (tagResult.ResultStatus === 0) {
                                    Swal.fire(
                                        'Silindi!',
                                        `Seçilen ${seciliDosyalar.length} adet etiket kalýcý olarak silindi.`,
                                        'success'
                                    ).then(function () {
                                        location.reload();
                                    });
                                } else {
                                    Swal.fire(
                                        'Baþarýsýz Ýþlem!',
                                        `${tagResult.Message}`,
                                        'error'
                                    );
                                }
                            },
                            error: function (err) {
                                toastr.error(`${err.responseText}`, "Hata!");
                            }
                        });
                    }
                }), function () {
                    location.reload();
                };
            }
        });
    };

    return {
        init: function () {
            initTable();
        }
    };
}();

jQuery(document).ready(function () {
    KTDatatablesExtensionsKeytable.init();
});
