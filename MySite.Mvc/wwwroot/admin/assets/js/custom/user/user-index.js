"use strict";
var KTDatatablesExtensionsKeytable = function () {

    var topluSecim = false;
    /* DataTables start here. */
    var initTable = function () {

        /* DataTables start here. */

        const table = $('#usersTable').DataTable({
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
            headerCallback: function (thead, data, start, end, display) {
                thead.getElementsByTagName('th')[0].innerHTML =
                    `<label class="checkbox checkbox-single checkbox-solid checkbox-primary mb-0">
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
                { orderable: false, targets: [6] }
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
                url: '/Admin/Kullanici/TumKullanicilar/',
                contentType: "application/json",
                beforeSend: function () {
                    $('#usersTable').hide();
                    $('.spinner-border').show();
                },
                success: function (data) {
                    const userListDto = jQuery.parseJSON(data);
                    table.clear();
                    if (userListDto.ResultStatus === 0) {
                        $.each(userListDto.Users.$values,
                            function (index, user) {
                                const newTableRow = table.row.add([
                                    user.Id,
                                    `<div class="symbol symbol-circle symbol-lg-60">
                                            <img src="/img/${user.Picture}" alt="${user.UserName}" class="my-image-table" />
                                        </div>`,
                                    user.UserName,
                                    user.FirstName,
                                    user.LastName,
                                    user.Email,
                                    `<button id="btnDetay" class="btn btn-sm btn-clean btn-icon mr-2" title="Kullanıcı Detayı" data-id="${user.Id}">
                                                <span class="svg-icon svg-icon-md">
                                                    <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-03-11-144509/theme/html/demo1/dist/../src/media/svg/icons/Communication/Chat6.svg-->
                                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                            <rect x="0" y="0" width="24" height="24" />
                                                            <path opacity="0.3" fill-rule="evenodd" clip-rule="evenodd" d="M14.4862 18L12.7975 21.0566C12.5304 21.54 11.922 21.7153 11.4386 21.4483C11.2977 21.3704 11.1777 21.2597 11.0887 21.1255L9.01653 18H5C3.34315 18 2 16.6569 2 15V6C2 4.34315 3.34315 3 5 3H19C20.6569 3 22 4.34315 22 6V15C22 16.6569 20.6569 18 19 18H14.4862Z" fill="black" />
                                                            <path fill-rule="evenodd" clip-rule="evenodd" d="M6 7H15C15.5523 7 16 7.44772 16 8C16 8.55228 15.5523 9 15 9H6C5.44772 9 5 8.55228 5 8C5 7.44772 5.44772 7 6 7ZM6 11H11C11.5523 11 12 11.4477 12 12C12 12.5523 11.5523 13 11 13H6C5.44772 13 5 12.5523 5 12C5 11.4477 5.44772 11 6 11Z" fill="black" />
                                                        </g>
                                                    </svg><!--end::Svg Icon-->
                                                </span>
                                            </button>
                                            <button id="btnRolAta" class="btn btn-sm btn-clean btn-icon mr-2" data-id="${user.Id}" title="Rol Ata">
                                                <span class="svg-icon svg-icon-md">
                                                    <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-03-11-144509/theme/html/demo1/dist/../src/media/svg/icons/Communication/Shield-user.svg-->
                                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                            <rect x="0" y="0" width="24" height="24" />
                                                            <path d="M4,4 L11.6314229,2.5691082 C11.8750185,2.52343403 12.1249815,2.52343403 12.3685771,2.5691082 L20,4 L20,13.2830094 C20,16.2173861 18.4883464,18.9447835 16,20.5 L12.5299989,22.6687507 C12.2057287,22.8714196 11.7942713,22.8714196 11.4700011,22.6687507 L8,20.5 C5.51165358,18.9447835 4,16.2173861 4,13.2830094 L4,4 Z" fill="#000000" opacity="0.3" />
                                                            <path d="M12,11 C10.8954305,11 10,10.1045695 10,9 C10,7.8954305 10.8954305,7 12,7 C13.1045695,7 14,7.8954305 14,9 C14,10.1045695 13.1045695,11 12,11 Z" fill="#000000" opacity="0.3" />
                                                            <path d="M7.00036205,16.4995035 C7.21569918,13.5165724 9.36772908,12 11.9907452,12 C14.6506758,12 16.8360465,13.4332455 16.9988413,16.5 C17.0053266,16.6221713 16.9988413,17 16.5815,17 C14.5228466,17 11.463736,17 7.4041679,17 C7.26484009,17 6.98863236,16.6619875 7.00036205,16.4995035 Z" fill="#000000" opacity="0.3" />
                                                        </g>
                                                    </svg><!--end::Svg Icon-->
                                                </span>
                                            </button>
                                            <button id="btnDuzenle" class="btn btn-sm btn-clean btn-icon mr-2" title="Düzenle" data-id="${user.Id}">
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
                                            <button id="btnSil" class="btn btn-sm btn-clean btn-icon" title="Sil" data-id="${user.Id}">
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
                                            `
                                ]).node();
                                const jqueryTableRow = $(newTableRow);
                                jqueryTableRow.attr('name', `${user.Id}`);
                            });
                        table.draw();
                        $('.spinner-border').hide();
                        $('#usersTable').fadeIn(1400);
                    } else {
                        toastr.error(`${userListDto.Message}`, 'İşlem Başarısız!');
                    }
                },
                error: function (err) {
                    $('.spinner-border').hide();
                    $('#usersTable').fadeIn(1000);
                    toastr.error(`${err.responseText}`, 'Hata!');
                }
            });
        });

        $(function () {
            const url = '/Admin/Kullanici/Ekle/';
            const placeHolderDiv = $('#modalPlaceHolder');
            $('#btnEkle').click(function () {
                $.get(url).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find(".modal").modal('show');
                });
            });

            /* Ajax GET / Getting the _UserAddPartial as Modal Form ends here. */

            /* Ajax POST / Posting the FormData as UserAddDto starts from here. */

            placeHolderDiv.on('click',
                '#btnKaydet',
                function (event) {
                    event.preventDefault();
                    const form = $('#form-user-add');
                    const actionUrl = form.attr('action');
                    const dataToSend = new FormData(form.get(0));
                    $.ajax({
                        url: actionUrl,
                        type: 'POST',
                        data: dataToSend,
                        processData: false,
                        contentType: false,
                        success: function (data) {
                            const userAddAjaxModel = jQuery.parseJSON(data);
                            const newFormBody = $('.modal-body', userAddAjaxModel.UserAddPartial);
                            placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                            const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                            if (isValid) {
                                placeHolderDiv.find('.modal').modal('hide');
                                const newTableRow = table.row.add([
                                    userAddAjaxModel.UserDto.User.Id,
                                    `<div class="symbol symbol-circle symbol-lg-60">
<img src="/img/${userAddAjaxModel.UserDto.User.Picture}" alt="${userAddAjaxModel.UserDto.User.UserName}" class="my-image-table" />
</div>`,
                                    userAddAjaxModel.UserDto.User.UserName,
                                    userAddAjaxModel.UserDto.User.FirstName,
                                    userAddAjaxModel.UserDto.User.LastName,
                                    userAddAjaxModel.UserDto.User.Email,
                                    `
                                <button id="btnDetay" class="btn btn-sm btn-clean btn-icon mr-2" title="Kullanıcı Detayı" data-id="${userAddAjaxModel.UserDto.User.Id}">
                                                <span class="svg-icon svg-icon-md">
                                                    <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-03-11-144509/theme/html/demo1/dist/../src/media/svg/icons/Communication/Chat6.svg-->
                                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                            <rect x="0" y="0" width="24" height="24" />
                                                            <path opacity="0.3" fill-rule="evenodd" clip-rule="evenodd" d="M14.4862 18L12.7975 21.0566C12.5304 21.54 11.922 21.7153 11.4386 21.4483C11.2977 21.3704 11.1777 21.2597 11.0887 21.1255L9.01653 18H5C3.34315 18 2 16.6569 2 15V6C2 4.34315 3.34315 3 5 3H19C20.6569 3 22 4.34315 22 6V15C22 16.6569 20.6569 18 19 18H14.4862Z" fill="black" />
                                                            <path fill-rule="evenodd" clip-rule="evenodd" d="M6 7H15C15.5523 7 16 7.44772 16 8C16 8.55228 15.5523 9 15 9H6C5.44772 9 5 8.55228 5 8C5 7.44772 5.44772 7 6 7ZM6 11H11C11.5523 11 12 11.4477 12 12C12 12.5523 11.5523 13 11 13H6C5.44772 13 5 12.5523 5 12C5 11.4477 5.44772 11 6 11Z" fill="black" />
                                                        </g>
                                                    </svg><!--end::Svg Icon-->
                                                </span>
                                            </button>
                                            <button id="btnRolAta" class="btn btn-sm btn-clean btn-icon mr-2" data-id="${userAddAjaxModel.UserDto.User.Id}" title="Rol Ata">
                                                <span class="svg-icon svg-icon-md">
                                                    <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-03-11-144509/theme/html/demo1/dist/../src/media/svg/icons/Communication/Shield-user.svg-->
                                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                            <rect x="0" y="0" width="24" height="24" />
                                                            <path d="M4,4 L11.6314229,2.5691082 C11.8750185,2.52343403 12.1249815,2.52343403 12.3685771,2.5691082 L20,4 L20,13.2830094 C20,16.2173861 18.4883464,18.9447835 16,20.5 L12.5299989,22.6687507 C12.2057287,22.8714196 11.7942713,22.8714196 11.4700011,22.6687507 L8,20.5 C5.51165358,18.9447835 4,16.2173861 4,13.2830094 L4,4 Z" fill="#000000" opacity="0.3" />
                                                            <path d="M12,11 C10.8954305,11 10,10.1045695 10,9 C10,7.8954305 10.8954305,7 12,7 C13.1045695,7 14,7.8954305 14,9 C14,10.1045695 13.1045695,11 12,11 Z" fill="#000000" opacity="0.3" />
                                                            <path d="M7.00036205,16.4995035 C7.21569918,13.5165724 9.36772908,12 11.9907452,12 C14.6506758,12 16.8360465,13.4332455 16.9988413,16.5 C17.0053266,16.6221713 16.9988413,17 16.5815,17 C14.5228466,17 11.463736,17 7.4041679,17 C7.26484009,17 6.98863236,16.6619875 7.00036205,16.4995035 Z" fill="#000000" opacity="0.3" />
                                                        </g>
                                                    </svg><!--end::Svg Icon-->
                                                </span>
                                            </button>
                                            <button id="btnDuzenle" class="btn btn-sm btn-clean btn-icon mr-2" title="Düzenle" data-id="${userAddAjaxModel.UserDto.User.Id}">
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
                                            <button id="btnSil" class="btn btn-sm btn-clean btn-icon" title="Sil" data-id="${userAddAjaxModel.UserDto.User.Id}">
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
                            `
                                ]).node();
                                const jqueryTableRow = $(newTableRow);
                                jqueryTableRow.attr('name', `${userAddAjaxModel.UserDto.User.Id}`);
                                table.row(newTableRow).draw();
                                toastr.success(`${userAddAjaxModel.UserDto.Message}`, 'Başarılı İşlem!');
                            } else {
                                let summaryText = "";
                                $('#validation-summary > ul > li').each(function () {
                                    let text = $(this).text();
                                    summaryText = `*${text}\n`;
                                });
                                toastr.warning(summaryText);
                            }
                        },
                        error: function (err) {
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }
                    });
                });
        });

        /* Ajax POST / Posting the FormData as UserAddDto ends here. */

        /* Ajax POST / Deleting a User starts from here */

        $(document).on('click',
            '#btnSil',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                const tableRow = $(`[name="${id}"]`);
                const userName = tableRow.find('td:eq(2)').text();
                Swal.fire({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: `${userName} adlı kullanıcı silinicektir!`,
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
                            dataType: 'json',
                            data: { userId: id },
                            url: '/Admin/Kullanici/Sil/',
                            success: function (data) {
                                const userDto = jQuery.parseJSON(data);
                                if (userDto.ResultStatus === 0) {
                                    Swal.fire(
                                        'Silindi!',
                                        `${userDto.User.UserName} adlı kullanıcı başarıyla silinmiştir.`,
                                        'success'
                                    );

                                    table.row(tableRow).remove().draw();
                                } else {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Başarısız İşlem!',
                                        text: `${userDto.Message}`,
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
                    title: 'Dosya Silme Onayı',
                    text: `Seçilen ${seciliDosyalar.length} dosyayı kalıcı olarak silmek istediğinizden emin misiniz?`,
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
                            url: '/Admin/Kullanici/TopluSil',
                            data: { userIds: ids },
                            success: function (data) {
                                const userResult = jQuery.parseJSON(data);
                                if (userResult.ResultStatus === 0) {
                                    seciliDosyalar.each(function () {
                                        var deletedRow = $(this).closest("tr");
                                        deletedRow.fadeOut(1500, function () {
                                            deletedRow.remove();
                                        });
                                    });
                                } else {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Başarısız İşlem!',
                                        text: `${userResult.Message}`,
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

        /* Ajax GET / Getting the _UserUpdatePartial as Modal Form starts from here. */

        $(function () {
            const url = '/Admin/Kullanici/Duzenle/';
            const placeHolderDiv = $('#modalPlaceHolder');
            $(document).on('click',
                '#btnDuzenle',
                function (event) {
                    event.preventDefault();
                    const id = $(this).attr('data-id');
                    $.get(url, { userId: id }).done(function (data) {
                        placeHolderDiv.html(data);
                        placeHolderDiv.find('.modal').modal('show');
                    }).fail(function (err) {
                        toastr.error(`${err.responseText}`, 'Hata!');
                    });
                });

            /* Ajax POST / Updating a User starts from here */

            placeHolderDiv.on('click',
                '#btnKaydet',
                function (event) {
                    event.preventDefault();

                    const form = $('#form-user-update');
                    const actionUrl = form.attr('action');
                    const dataToSend = new FormData(form.get(0));
                    $.ajax({
                        url: actionUrl,
                        type: 'POST',
                        data: dataToSend,
                        processData: false,
                        contentType: false,
                        success: function (data) {
                            const userUpdateAjaxModel = jQuery.parseJSON(data);
                            console.log(userUpdateAjaxModel);
                            const newFormBody = $('.modal-body', userUpdateAjaxModel.UserUpdatePartial);
                            placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                            const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                            if (isValid) {
                                const id = userUpdateAjaxModel.UserDto.User.Id;
                                const tableRow = $(`[name="${id}"]`);
                                placeHolderDiv.find('.modal').modal('hide');
                                table.row(tableRow).data([
                                    userUpdateAjaxModel.UserDto.User.Id,
                                    `<div class="symbol symbol-circle symbol-lg-60">
                                                <img src="/img/${userUpdateAjaxModel.UserDto.User.Picture}" alt="${userUpdateAjaxModel.UserDto.User.UserName}"/>
                                     </div>`,
                                    userUpdateAjaxModel.UserDto.User.UserName,
                                    userUpdateAjaxModel.UserDto.User.FirstName,
                                    userUpdateAjaxModel.UserDto.User.LastName,
                                    userUpdateAjaxModel.UserDto.User.Email,
                                    `
                                <button id="btnDetay" class="btn btn-sm btn-clean btn-icon mr-2" title="Kullanıcı Detayı" data-id="${userUpdateAjaxModel.UserDto.User.Id}">
                                                <span class="svg-icon svg-icon-md">
                                                    <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-03-11-144509/theme/html/demo1/dist/../src/media/svg/icons/Communication/Chat6.svg-->
                                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                            <rect x="0" y="0" width="24" height="24" />
                                                            <path opacity="0.3" fill-rule="evenodd" clip-rule="evenodd" d="M14.4862 18L12.7975 21.0566C12.5304 21.54 11.922 21.7153 11.4386 21.4483C11.2977 21.3704 11.1777 21.2597 11.0887 21.1255L9.01653 18H5C3.34315 18 2 16.6569 2 15V6C2 4.34315 3.34315 3 5 3H19C20.6569 3 22 4.34315 22 6V15C22 16.6569 20.6569 18 19 18H14.4862Z" fill="black" />
                                                            <path fill-rule="evenodd" clip-rule="evenodd" d="M6 7H15C15.5523 7 16 7.44772 16 8C16 8.55228 15.5523 9 15 9H6C5.44772 9 5 8.55228 5 8C5 7.44772 5.44772 7 6 7ZM6 11H11C11.5523 11 12 11.4477 12 12C12 12.5523 11.5523 13 11 13H6C5.44772 13 5 12.5523 5 12C5 11.4477 5.44772 11 6 11Z" fill="black" />
                                                        </g>
                                                    </svg><!--end::Svg Icon-->
                                                </span>
                                            </button>
                                            <button id="btnRolAta" class="btn btn-sm btn-clean btn-icon mr-2" data-id="${userUpdateAjaxModel.UserDto.User.Id}" title="Rol Ata">
                                                <span class="svg-icon svg-icon-md">
                                                    <!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-03-11-144509/theme/html/demo1/dist/../src/media/svg/icons/Communication/Shield-user.svg-->
                                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                            <rect x="0" y="0" width="24" height="24" />
                                                            <path d="M4,4 L11.6314229,2.5691082 C11.8750185,2.52343403 12.1249815,2.52343403 12.3685771,2.5691082 L20,4 L20,13.2830094 C20,16.2173861 18.4883464,18.9447835 16,20.5 L12.5299989,22.6687507 C12.2057287,22.8714196 11.7942713,22.8714196 11.4700011,22.6687507 L8,20.5 C5.51165358,18.9447835 4,16.2173861 4,13.2830094 L4,4 Z" fill="#000000" opacity="0.3" />
                                                            <path d="M12,11 C10.8954305,11 10,10.1045695 10,9 C10,7.8954305 10.8954305,7 12,7 C13.1045695,7 14,7.8954305 14,9 C14,10.1045695 13.1045695,11 12,11 Z" fill="#000000" opacity="0.3" />
                                                            <path d="M7.00036205,16.4995035 C7.21569918,13.5165724 9.36772908,12 11.9907452,12 C14.6506758,12 16.8360465,13.4332455 16.9988413,16.5 C17.0053266,16.6221713 16.9988413,17 16.5815,17 C14.5228466,17 11.463736,17 7.4041679,17 C7.26484009,17 6.98863236,16.6619875 7.00036205,16.4995035 Z" fill="#000000" opacity="0.3" />
                                                        </g>
                                                    </svg><!--end::Svg Icon-->
                                                </span>
                                            </button>
                                            <button id="btnDuzenle" class="btn btn-sm btn-clean btn-icon mr-2" title="Düzenle" data-id="${userUpdateAjaxModel.UserDto.User.Id}">
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
                                            <button id="btnSil" class="btn btn-sm btn-clean btn-icon" title="Sil" data-id="${userUpdateAjaxModel.UserDto.User.Id}">
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
                            `
                                ]);
                                tableRow.attr("name", `${id}`);
                                table.row(tableRow).invalidate();
                                toastr.success(`${userUpdateAjaxModel.UserDto.Message}`, "Başarılı İşlem!");
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
                            console.log(error);
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }
                    });
                });

        });
        // Get Detail Ajax Operation

        $(function () {

            const url = '/Admin/Kullanici/Detaylar/';
            const placeHolderDiv = $('#modalPlaceHolder');
            $(document).on('click',
                '#btnDetay',
                function (event) {
                    event.preventDefault();
                    const id = $(this).attr('data-id');
                    $.get(url, { userId: id }).done(function (data) {
                        placeHolderDiv.html(data);
                        placeHolderDiv.find('.modal').modal('show');
                    }).fail(function (err) {
                        toastr.error(`${err.responseText}`, 'Hata!');
                    });
                });

        });

        $(function () {
            const url = '/Admin/Rol/RolAta/';
            const placeHolderDiv = $('#modalPlaceHolder');
            $(document).on('click',
                '#btnRolAta',
                function (event) {
                    event.preventDefault();
                    const id = $(this).attr('data-id');
                    $.get(url, { userId: id }).done(function (data) {
                        placeHolderDiv.html(data);
                        placeHolderDiv.find('.modal').modal('show');
                    }).fail(function (err) {
                        toastr.error(`${err.responseText}`, 'Hata!');
                    });
                });

            /* Ajax POST / Updating a Role Assign starts from here */

            placeHolderDiv.on('click',
                '#btnKaydet',
                function (event) {
                    event.preventDefault();
                    const form = $('#form-role-assign');
                    const actionUrl = form.attr('action');
                    const dataToSend = new FormData(form.get(0));
                    $.ajax({
                        url: actionUrl,
                        type: 'POST',
                        data: dataToSend,
                        processData: false,
                        contentType: false,
                        success: function (data) {
                            const userRoleAssignAjaxModel = jQuery.parseJSON(data);
                            console.log(userRoleAssignAjaxModel);
                            const newFormBody = $('.modal-body', userRoleAssignAjaxModel.RoleAssignPartial);
                            placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                            const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                            if (isValid) {
                                const id = userRoleAssignAjaxModel.UserDto.User.Id;
                                const tableRow = $(`[name="${id}"]`);
                                //placeHolderDiv.find('.modal').modal('hide');
                                toastr.success(`${userRoleAssignAjaxModel.UserDto.Message}`, "Başarılı İşlem!");
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
                            console.log(error);
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }
                    });
                });

        });
    };

    return {

        //main function to initiate the module
        init: function () {
            initTable();
        }

    };
}();

jQuery(document).ready(function () {
    KTDatatablesExtensionsKeytable.init();
});