"use strict";
var KTDatatablesExtensionsKeytable = function () {

    var topluSecim = false;
    var initTable = function () {
        // begin first table
        var table = $('#deletedArticlesTable').DataTable({
            language: {
                "sDecimal": ",",
                "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
                "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
                "sInfoEmpty": "Kayıt yok",
                "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
                "sInfoPostFix": "",
                "sInfoThousands": ".",
                "sLengthMenu": "Sayfada _MENU_ kayıt göster",
                "sLoadingRecords": "Yükleniyor...",
                "sProcessing": "İşleniyor...",
                "sSearch": "Ara:",
                "sZeroRecords": "Eşleşen kayıt bulunamadı",
                "oPaginate": {
                    "sFirst": "İlk",
                    "sLast": "Son",
                    "sNext": "Sonraki",
                    "sPrevious": "Önceki"
                },
                "oAria": {
                    "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                    "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
                },
                "select": {
                    "rows": {
                        "_": "%d kayıt seçildi",
                        "0": "",
                        "1": "1 kayıt seçildi"
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
                { orderable: false, targets: [1, 6] }
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

        function resetSelection() {
            topluSecim = false;
            $('#kt_datatable_selected_records').html("0");
            $('#kt_datatable_group_action_form').collapse('hide');
            $('.group-checkable').prop('checked', false);
        }

        $('#btnRefresh').click(function () {
            $('#kt_datatable_group_action_form').collapse('hide');
            $.ajax({
                type: 'GET',
                url: '/Admin/Makale/TumSilinmisMakaleler/',
                contentType: "application/json",
                beforeSend: function () {
                    $('#deletedArticlesTable').hide();
                    $('.spinner-border').show();
                },
                success: function (data) {
                    const articleResult = jQuery.parseJSON(data);
                    table.clear();
                    if (articleResult.Data.ResultStatus === 0) {
                        let categoriesArray = [];
                        $.each(articleResult.Data.Articles.$values,
                            function (index, article) {
                                const newArticle = getJsonNetObject(article, articleResult.Data.Articles.$values);
                                let newCategory = getJsonNetObject(newArticle.Category, newArticle);
                                if (newCategory !== null) {
                                    categoriesArray.push(newCategory);
                                }
                                if (newCategory === null) {
                                    newCategory = categoriesArray.find((category) => {
                                        return category.$id === newArticle.Category.$ref;
                                    });
                                }
                                const newTableRow = table.row.add([
                                    newArticle.Id,
                                    `<div class="symbol symbol-50">
                                        <div class="symbol-label" style="background-image: url('/admin/assets/img/${newArticle.Thumbnail}"></div>
                                    </div>`,
                                    newArticle.Title,
                                    newCategory.Name,
                                    `${convertToShortDate(newArticle.ModifiedDate)}`,
                                    newArticle.ModifiedByName,
                                    `
	                        <button id="btnUndo" class="btn btn-sm btn-clean btn-icon mr-2" title="Geri Al" data-id="${newArticle.Id}">
                                <span class="svg-icon svg-icon-md">
                                    <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-04-03-014522/theme/html/demo2/dist/../src/media/svg/icons/Text/Undo.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                            <rect x="0" y="0" width="24" height="24" />
                                            <path d="M21.4451171,17.7910156 C21.4451171,16.9707031 21.6208984,13.7333984 19.0671874,11.1650391 C17.3484374,9.43652344 14.7761718,9.13671875 11.6999999,9 L11.6999999,4.69307548 C11.6999999,4.27886191 11.3642135,3.94307548 10.9499999,3.94307548 C10.7636897,3.94307548 10.584049,4.01242035 10.4460626,4.13760526 L3.30599678,10.6152626 C2.99921905,10.8935795 2.976147,11.3678924 3.2544639,11.6746702 C3.26907199,11.6907721 3.28437331,11.7062312 3.30032452,11.7210037 L10.4403903,18.333467 C10.7442966,18.6149166 11.2188212,18.596712 11.5002708,18.2928057 C11.628669,18.1541628 11.6999999,17.9721616 11.6999999,17.7831961 L11.6999999,13.5 C13.6531249,13.5537109 15.0443703,13.6779456 16.3083984,14.0800781 C18.1284272,14.6590944 19.5349747,16.3018455 20.5280411,19.0083314 L20.5280247,19.0083374 C20.6363903,19.3036749 20.9175496,19.5 21.2321404,19.5 L21.4499999,19.5 C21.4499999,19.0068359 21.4451171,18.2255859 21.4451171,17.7910156 Z" fill="#000000" fill-rule="nonzero" />
                                        </g>
                                    </svg><!--end::Svg Icon-->
                                </span>
                            </button>
                            <button id="btnDelete" class="btn btn-sm btn-clean btn-icon" title="Kalıcı Olarak Sil" data-id="${newArticle.Id}">
                                <span class="svg-icon svg-icon-md">
                                    <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-04-03-014522/theme/html/demo2/dist/../src/media/svg/icons/Navigation/Close.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                            <g transform="translate(12.000000, 12.000000) rotate(-45.000000) translate(-12.000000, -12.000000) translate(4.000000, 4.000000)" fill="#000000">
                                                <rect x="0" y="7" width="16" height="2" rx="1" />
                                                <rect opacity="0.3" transform="translate(8.000000, 8.000000) rotate(-270.000000) translate(-8.000000, -8.000000) " x="0" y="7" width="16" height="2" rx="1" />
                                            </g>
                                        </g>
                                    </svg><!--end::Svg Icon-->
                                </span>
                            </button>`
                                ]).node();
                                const jqueryTableRow = $(newTableRow);
                                jqueryTableRow.attr('name', `${newArticle.Id}`);
                            });
                        table.draw();
                        $('.spinner-border').hide();
                        $('#deletedArticlesTable').fadeIn(1400);
                    } else {
                        toastr.error(`${articleResult.Data.Message}`, 'İşlem Başarısız!');
                    }
                },
                error: function (err) {
                    $('.spinner-border').hide();
                    $('#deletedArticlesTable').fadeIn(1000);
                    toastr.error(`${err.responseText}`, 'Hata!');
                }
            });
        });

        $(document).on('click',
            '#btnDelete',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                const tableRow = $(`[name="${id}"]`);
                const articleTitle = tableRow.find('td:eq(2)').text();
                Swal.fire({
                    title: 'Kalıcı olarak silmek istediğinize emin misiniz?',
                    text: `${articleTitle} başlıklı makale kalıcı olarak silinicektir!`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Evet, kalıcı olarak silmek istiyorum.',
                    cancelButtonText: 'Hayır, istemiyorum.'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            data: { articleId: id },
                            url: '/Admin/Makale/KaliciSil/',
                            success: function (data) {
                                const articleResult = jQuery.parseJSON(data);
                                if (articleResult.ResultStatus === 0) {
                                    Swal.fire(
                                        'Silindi!',
                                        `${articleResult.Message}`,
                                        'success'
                                    ).then(function () {
                                        tableRow.fadeOut(1500, function () {
                                            table.row(tableRow).remove().draw();
                                            resetSelection();
                                        });
                                    });
                                } else {
                                    Swal.fire(
                                        'error',
                                        'Başarısız İşlem!',
                                        `${articleResult.Message}`,
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

        $('#btnMultiDelete').click(function () {
            var seciliDosyalar = $('tr td label input[type=checkbox]:checked');
            if (seciliDosyalar.length > 0 && topluSecim === true) {
                var ids = [];
                seciliDosyalar.each(function () {
                    var id = parseInt($(this).closest('tr').attr('name'));
                    ids.push(id);
                });

                Swal.fire({
                    title: 'Kayıt Silme Onayı',
                    text: `Seçilen ${seciliDosyalar.length} adet makaleyi kalıcı olarak silmek istediğinizden emin misiniz?`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Evet, silmek istiyorum.',
                    cancelButtonText: 'Hayır, silmek istemiyorum.'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            url: '/Admin/Makale/TopluKaliciSil',
                            data: { articleIds: ids },
                            success: function (data) {
                                const articleResult = jQuery.parseJSON(data);
                                if (articleResult.ResultStatus === 0) {
                                    Swal.fire(
                                        'Silindi!',
                                        `Seçilen ${seciliDosyalar.length} adet makale kalıcı olarak silindi.`,
                                        'success'
                                    ).then(function () {
                                        seciliDosyalar.each(function () {
                                            var deletedRow = $(this).closest("tr");
                                            deletedRow.fadeOut(1500, function () {
                                                table.row(deletedRow).remove().draw();
                                            });
                                        });
                                        resetSelection();
                                    });
                                } else {
                                    Swal.fire(
                                        'error',
                                        'Başarısız İşlem!',
                                        `${articleResult.Message}`,
                                    );
                                }
                            },
                            error: function (err) {
                                toastr.error(`${err.responseText}`, "Hata!");
                            }
                        });
                    }
                });
            }
        });

        $(document).on('click',
            '#btnUndo',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                const tableRow = $(`[name="${id}"]`);
                let articleTitle = tableRow.find('td:eq(2)').text();
                Swal.fire({
                    title: 'Çöp kutusundan geri getirmek istediğinize emin misiniz?',
                    text: `${articleTitle} başlıklı makaleyi çöp kutusundan geri getirilecektir!`,
                    icon: 'question',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Evet, istiyorum.',
                    cancelButtonText: 'Hayır, istemiyorum.'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            data: { articleId: id },
                            url: '/Admin/Makale/GeriAl/',
                            success: function (data) {
                                const articleUndoDeleteResult = jQuery.parseJSON(data);
                                if (articleUndoDeleteResult.ResultStatus === 0) {
                                    Swal.fire(
                                        'Çöp kutusundan Geri Getirildi!',
                                        `${articleUndoDeleteResult.Message}`,
                                        'success'
                                    ).then(function () {
                                        tableRow.fadeOut(1500, function () {
                                            table.row(tableRow).remove().draw();
                                            resetSelection();
                                        });
                                    });
                                } else {
                                    Swal.fire(
                                        'error',
                                        'Başarısız İşlem!',
                                        `${articleUndoDeleteResult.Message}`,
                                    );
                                }
                            },
                            error: function (err) {
                                console.log(err);
                                toastr.error(`${err.responseText}`, "Hata!");
                            }
                        });
                    }
                });
            });

        $('#btnMultiUndo').click(function () {
            var seciliDosyalar = $('tr td label input[type=checkbox]:checked');
            if (seciliDosyalar.length > 0 && topluSecim === true) {
                var ids = [];
                seciliDosyalar.each(function () {
                    var id = parseInt($(this).closest('tr').attr('name'));
                    ids.push(id);
                });

                Swal.fire({
                    title: 'Geri Alma',
                    text: `Seçilen ${seciliDosyalar.length} adet makaleyi çöp kutusundan geri almak istediğinizden emin misiniz?`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Evet, istiyorum.',
                    cancelButtonText: 'Hayır, istemiyorum.'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            url: '/Admin/Makale/TopluGeriAl',
                            data: { articleIds: ids },
                            success: function (data) {
                                const articleResult = jQuery.parseJSON(data);
                                if (articleResult.ResultStatus === 0) {
                                    Swal.fire(
                                        'Silindi!',
                                        `Seçilen ${seciliDosyalar.length} adet makale çöp kutusundan geri getirildi.`,
                                        'success'
                                    ).then(function () {
                                        seciliDosyalar.each(function () {
                                            var deletedRow = $(this).closest("tr");
                                            deletedRow.fadeOut(1500, function () {
                                                table.row(deletedRow).remove().draw();
                                            });
                                        });
                                        resetSelection();
                                    });
                                } else {
                                    Swal.fire(
                                        'error',
                                        'Başarısız İşlem!',
                                        `${articleResult.Message}`,
                                    );
                                }
                            },
                            error: function (err) {
                                toastr.error(`${err.responseText}`, "Hata!");
                            }
                        });
                    }
                });
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