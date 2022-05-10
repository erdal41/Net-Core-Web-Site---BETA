"use strict";
var KTDatatablesExtensionsKeytable = function () {

    var topluSecim = false;
    var initTable = function () {
        const table = $('#commentsTable').DataTable({
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
                selector: 'td:first-child .checkable',
            },
            headerCallback: function (thead, data, start, end, display) {
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
                    render: function (data, type, full, meta) {
                        return `
                        <label class="checkbox checkbox-single checkbox-primary mb-0">
                            <input type="checkbox" value="" class="checkable"/>
                            <span></span>
                        </label>`;
                    },
                    "order": []
                },
                { orderable: false, targets: [6] },
            ],
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

        table.on('change', '.checkable', function (e) {
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

        $('#btnYenile').click(function () {
            $('#kt_datatable_group_action_form').collapse('hide');
            $.ajax({
                type: 'GET',
                url: '/Admin/Yorum/TumYorumlar/',
                contentType: "application/json",
                beforeSend: function () {
                    $('#commentsTable').hide();
                    $('.spinner-border').show();
                },
                success: function (data) {
                    const commentResult = jQuery.parseJSON(data);
                    table.clear();
                    if (commentResult.Data) {
                        const articlesArray = [];
                        $.each(commentResult.Data.Comments.$values,
                            function (index, comment) {
                                const newComment = getJsonNetObject(comment, commentResult.Data.Comments.$values);
                                let newArticle = getJsonNetObject(newComment.Article, newComment);
                                if (newArticle !== null) {
                                    articlesArray.push(newArticle);
                                }
                                if (newArticle === null) {
                                    newArticle = articlesArray.find((article) => {
                                        return article.$id === newComment.Article.$ref;
                                    });
                                }
                                const newTableRow = table.row.add([
                                    newComment.Id,
                                    newArticle.Title,
                                    newComment.Text.length > 75 ? newComment.Text.substring(0, 75) : newComment.Text,
                                    `${newComment.IsActive ? "Onaylandý" : "Onay Bekliyor"}`,
                                    `${convertToShortDate(newComment.ModifiedDate)}`,
                                    newComment.ModifiedByName,
                                    getButtonsForDataTable(newComment)
                                ]).node();
                                const jqueryTableRow = $(newTableRow);
                                jqueryTableRow.attr('name', `${newComment.Id}`);
                            });
                        table.draw();
                        $('.spinner-border').hide();
                        $('#commentsTable').fadeIn(1400);
                    } else {
                        toastr.error(`${commentResult.Message}`, 'Ýþlem Baþarýsýz!');
                    }
                },
                error: function (err) {
                    $('.spinner-border').hide();
                    $('#commentsTable').fadeIn(1000);
                    toastr.error(`${err.responseText}`, 'Hata!');
                }
            });
        });

        $(document).on('click',
            '#btnSil',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                const tableRow = $(`[name="${id}"]`);
                let commentText = tableRow.find('td:eq(2)').text();
                commentText = commentText.length > 75 ? commentText.substring(0, 75) : commentText;
                Swal.fire({
                    title: 'Silmek istediðinize emin misiniz?',
                    text: `${commentText} içerikli yorum silinicektir!`,
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
                            data: { commentId: id },
                            url: '/Admin/Yorum/Sil/',
                            success: function (data) {
                                const commentResult = jQuery.parseJSON(data);
                                if (commentResult.Data) {
                                    Swal.fire(
                                        'Silindi!',
                                        `${commentText} içerikli yorum baþarýyla silinmiþtir.`,
                                        'success'
                                    ).then(function () {
                                        location.reload();
                                    }); 
                                } else {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Baþarýsýz Ýþlem!',
                                        text: `Beklenmedik bir hata oluþtu.`,
                                    });
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
                    text: `Seçilen ${seciliDosyalar.length} yorumu kalýcý olarak silmek istediðinizden emin misiniz?`,
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
                            url: '/Admin/Yorum/TopluSil',
                            data: { commentIds: ids },
                            success: function (data) {
                                const commentResult = jQuery.parseJSON(data);
                                if (commentResult.Data) {
                                    Swal.fire(
                                        'Silindi!',
                                        `${seciliDosyalar.length} yorum baþarýyla silinmiþtir.`,
                                        'success'
                                    ).then(function () {
                                        location.reload();
                                    });   
                                } else {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Baþarýsýz Ýþlem!',
                                        text: `${commentResult.Message}`,
                                    });
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

        /* Ajax GET / Getting the _CommentUpdatePartial as Modal Form starts from here. */

        $(function () {
            const url = '/Admin/Yorum/Duzenle/';
            const placeHolderDiv = $('#modalPlaceHolder');
            $(document).on('click',
                '#btnDuzenle',
                function (event) {
                    event.preventDefault();
                    const id = $(this).attr('data-id');
                    $.get(url, { commentId: id }).done(function (data) {
                        placeHolderDiv.html(data);
                        placeHolderDiv.find('.modal').modal('show');
                    }).fail(function (err) {
                        toastr.error(`${err.responseText}`, 'Hata!');
                    });
                });

            /* Ajax POST / Updating a Comment starts from here */

            placeHolderDiv.on('click',
                '#btnKaydet',
                function (event) {
                    event.preventDefault();
                    const form = $('#form-comment-update');
                    const actionUrl = form.attr('action');
                    const dataToSend = new FormData(form.get(0));
                    $.ajax({
                        url: actionUrl,
                        type: 'POST',
                        data: dataToSend,
                        processData: false,
                        contentType: false,
                        success: function (data) {
                            const commentUpdateAjaxModel = jQuery.parseJSON(data);
                            const newFormBody = $('.modal-body', commentUpdateAjaxModel.CommentUpdatePartial);
                            placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                            const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                            if (isValid) {
                                const id = commentUpdateAjaxModel.CommentDto.Comment.Id;
                                const tableRow = $(`[name="${id}"]`);
                                placeHolderDiv.find('.modal').modal('hide');
                                table.row(tableRow).data([
                                    commentUpdateAjaxModel.CommentDto.Comment.Id,
                                    commentUpdateAjaxModel.CommentDto.Comment.Article.Title,
                                    commentUpdateAjaxModel.CommentDto.Comment.Text.length > 75 ? commentUpdateAjaxModel.CommentDto.Comment.Text.substring(0, 75) : commentUpdateAjaxModel.CommentDto.Comment.Text,
                                    `${commentUpdateAjaxModel.CommentDto.Comment.IsActive ? "Yayýnda" : "Taslak"}`,
                                    `${convertToShortDate(commentUpdateAjaxModel.CommentDto.Comment.ModifiedDate)}`,
                                    commentUpdateAjaxModel.CommentDto.Comment.ModifiedByName,
                                    getButtonsForDataTable(commentUpdateAjaxModel.CommentDto.Comment)
                                ]);
                                tableRow.attr("name", `${id}`);
                                table.row(tableRow).invalidate();
                                toastr.success(`${commentUpdateAjaxModel.CommentDto.Comment.Id} no'lu yorum baþarýyla güncellenmiþtir`, "Baþarýlý Ýþlem!");
                            } else {
                                let summaryText = "";
                                $('#validation-summary > ul > li').each(function () {
                                    let text = $(this).text();
                                    summaryText = `*${text}\n`;
                                });
                                toastr.warning(summaryText);
                            }
                        },
                        error: function (error) {
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }
                    });
                });

        });

        // Get Detail Ajax Operation

        $(function () {

            const url = '/Admin/Yorum/YorumDetayi/';
            const placeHolderDiv = $('#modalPlaceHolder');
            $(document).on('click',
                '#btnDetay',
                function (event) {
                    event.preventDefault();
                    const id = $(this).attr('data-id');
                    $.get(url, { commentId: id }).done(function (data) {
                        placeHolderDiv.html(data);
                        placeHolderDiv.find('.modal').modal('show');
                    }).fail(function (err) {
                        toastr.error(`${err.responseText}`, 'Hata!');
                    });
                });

        });

        /* Ajax POST / Deleting a Comment starts from here */

        $(document).on('click',
            '#btnOnay',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                const tableRow = $(`[name="${id}"]`);
                let commentText = tableRow.find('td:eq(2)').text();
                commentText = commentText.length > 75 ? commentText.substring(0, 75) : commentText;
                Swal.fire({
                    title: 'Onaylamak istediðinize emin misiniz?',
                    text: `${commentText} içerikli yorum onaylanacaktýr!`,
                    icon: 'info',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Evet, onaylamak istiyorum.',
                    cancelButtonText: 'Hayýr, onaylamak istemiyorum.'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            data: { commentId: id },
                            url: '/Admin/Yorum/Onay/',
                            success: function (data) {
                                const commentResult = jQuery.parseJSON(data);
                                if (commentResult.Data) {
                                    table.row(tableRow).data([
                                        commentResult.Data.Comment.Id,
                                        commentResult.Data.Comment.Article.Title,
                                        commentResult.Data.Comment.Text.length > 75 ? commentResult.Data.Comment.Text.substring(0, 75) : commentResult.Data.Comment.Text,
                                        `${commentResult.Data.Comment.IsActive ? "Yayýnda" : "Taslak"}`,
                                        `${convertToShortDate(commentResult.Data.Comment.ModifiedDate)}`,
                                        commentResult.Data.Comment.ModifiedByName,
                                        getButtonsForDataTable(commentResult.Data.Comment)
                                    ]);
                                    tableRow.attr("name", `${id}`);
                                    table.row(tableRow).invalidate();
                                    Swal.fire(
                                        'Onaylandý!',
                                        `${commentResult.Data.Comment.Id} no'lu yorum baþarýyla onaylanmýþtýr.`,
                                        'success'
                                    );

                                } else {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Baþarýsýz Ýþlem!',
                                        text: `Beklenmedik bir hata ile karþýlaþýldý.`,
                                    });
                                }
                            },
                            error: function (err) {
                                toastr.error(`${err.responseText}`, "Hata!");
                            }
                        });
                    }
                });
            });

        function getButtonsForDataTable(comment) {
            if (!comment.IsActive) {
                return `

                                    <button id="btnOnay" class="btn btn-sm btn-clean btn-icon mr-2" title="Yorumu Onayla" data-id="${comment.Id}">
                                            <span class="svg-icon svg-icon-md">
                                                <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-03-11-144509/theme/html/demo1/dist/../src/media/svg/icons/Communication/Chat-check.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                        <rect x="0" y="0" width="24" height="24" />
                                                        <path d="M4.875,20.75 C4.63541667,20.75 4.39583333,20.6541667 4.20416667,20.4625 L2.2875,18.5458333 C1.90416667,18.1625 1.90416667,17.5875 2.2875,17.2041667 C2.67083333,16.8208333 3.29375,16.8208333 3.62916667,17.2041667 L4.875,18.45 L8.0375,15.2875 C8.42083333,14.9041667 8.99583333,14.9041667 9.37916667,15.2875 C9.7625,15.6708333 9.7625,16.2458333 9.37916667,16.6291667 L5.54583333,20.4625 C5.35416667,20.6541667 5.11458333,20.75 4.875,20.75 Z" fill="#000000" fill-rule="nonzero" opacity="0.3" />
                                                        <path d="M2,11.8650466 L2,6 C2,4.34314575 3.34314575,3 5,3 L19,3 C20.6568542,3 22,4.34314575 22,6 L22,15 C22,15.0032706 21.9999948,15.0065399 21.9999843,15.009808 L22.0249378,15 L22.0249378,19.5857864 C22.0249378,20.1380712 21.5772226,20.5857864 21.0249378,20.5857864 C20.7597213,20.5857864 20.5053674,20.4804296 20.317831,20.2928932 L18.0249378,18 L12.9835977,18 C12.7263047,14.0909841 9.47412135,11 5.5,11 C4.23590829,11 3.04485894,11.3127315 2,11.8650466 Z M6,7 C5.44771525,7 5,7.44771525 5,8 C5,8.55228475 5.44771525,9 6,9 L15,9 C15.5522847,9 16,8.55228475 16,8 C16,7.44771525 15.5522847,7 15,7 L6,7 Z" fill="#000000" />
                                                    </g>
                                                </svg><!--end::Svg Icon-->
                                            </span>
                                        </button>
                                    <button id="btnDetay" class="btn btn-sm btn-clean btn-icon mr-2" title="Yorum Detayý" data-id="${comment.Id}">
                                        <span class="svg-icon svg-icon-md">
                                            <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-03-11-144509/theme/html/demo1/dist/../src/media/svg/icons/Communication/Chat6.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                    <rect x="0" y="0" width="24" height="24" />
                                                    <path opacity="0.3" fill-rule="evenodd" clip-rule="evenodd" d="M14.4862 18L12.7975 21.0566C12.5304 21.54 11.922 21.7153 11.4386 21.4483C11.2977 21.3704 11.1777 21.2597 11.0887 21.1255L9.01653 18H5C3.34315 18 2 16.6569 2 15V6C2 4.34315 3.34315 3 5 3H19C20.6569 3 22 4.34315 22 6V15C22 16.6569 20.6569 18 19 18H14.4862Z" fill="black" />
                                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M6 7H15C15.5523 7 16 7.44772 16 8C16 8.55228 15.5523 9 15 9H6C5.44772 9 5 8.55228 5 8C5 7.44772 5.44772 7 6 7ZM6 11H11C11.5523 11 12 11.4477 12 12C12 12.5523 11.5523 13 11 13H6C5.44772 13 5 12.5523 5 12C5 11.4477 5.44772 11 6 11Z" fill="black" />
                                                </g>
                                            </svg><!--end::Svg Icon-->
                                        </span>
                                    </button>
                                        <button id="btnDuzenle" class="btn btn-sm btn-clean btn-icon mr-2" title="Düzenle" data-id="${comment.Id}">
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
                                        <button id="btnSil" class="btn btn-sm btn-clean btn-icon" title="Sil" data-id="${comment.Id}">
                                            <span class="svg-icon svg-icon-md">
                                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                        <rect x="0" y="0" width="24" height="24" />
                                                        <path d="M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z" fill="#000000" fill-rule="nonzero" />
                                                        <path d="M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="#000000" opacity="0.3" />
                                                    </g>
                                                </svg>
                                            </span>
                                        </button>
                                            `;
            }
            return `            <button id="btnDetay" class="btn btn-sm btn-clean btn-icon mr-2" title="Yorum Detayý" data-id="${comment.Id}">
                                        <span class="svg-icon svg-icon-md">
                                            <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-03-11-144509/theme/html/demo1/dist/../src/media/svg/icons/Communication/Chat6.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                    <rect x="0" y="0" width="24" height="24" />
                                                    <path opacity="0.3" fill-rule="evenodd" clip-rule="evenodd" d="M14.4862 18L12.7975 21.0566C12.5304 21.54 11.922 21.7153 11.4386 21.4483C11.2977 21.3704 11.1777 21.2597 11.0887 21.1255L9.01653 18H5C3.34315 18 2 16.6569 2 15V6C2 4.34315 3.34315 3 5 3H19C20.6569 3 22 4.34315 22 6V15C22 16.6569 20.6569 18 19 18H14.4862Z" fill="black" />
                                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M6 7H15C15.5523 7 16 7.44772 16 8C16 8.55228 15.5523 9 15 9H6C5.44772 9 5 8.55228 5 8C5 7.44772 5.44772 7 6 7ZM6 11H11C11.5523 11 12 11.4477 12 12C12 12.5523 11.5523 13 11 13H6C5.44772 13 5 12.5523 5 12C5 11.4477 5.44772 11 6 11Z" fill="black" />
                                                </g>
                                            </svg><!--end::Svg Icon-->
                                        </span>
                                    </button>
                                        <button id="btnDuzenle" class="btn btn-sm btn-clean btn-icon mr-2" title="Düzenle" data-id="${comment.Id}">
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
                                        <button id="btnSil" class="btn btn-sm btn-clean btn-icon" title="Sil" data-id="${comment.Id}">
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


        }
    };

    return {

        //main function to initiate the module
        init: function () {
            initTable();
        },
    };
}();

jQuery(document).ready(function () {
    KTDatatablesExtensionsKeytable.init();
});