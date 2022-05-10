var ul = $("#listImages");
var li = $("#listImages li");
var span = $('#listImages li label span');
var check = $('#listImages li .checkbox > input');
var topluSecim = false;
const placeHolderDiv = $('#modalPlaceHolder');

$('#listImages li').on('change', '.checkable', function (e) {
    var checkedCount = $('#listImages').find('input[type=checkbox]:checked').length;
    $('#kt_datatable_selected_records').html(checkedCount);

    if ($(this).prop('checked') == true) {
        $(this).closest('li').find('img').removeClass("opacity-60");
    } else {
        $(this).closest('li').find('img').addClass("opacity-60");
    }
});


function refresh() {
    $('#kt_datatable_group_action_form').collapse('hide');
    $.ajax({
        type: 'GET',
        url: '/Admin/Medya/TumDosyalar/',
        contentType: "application/json",
        beforeSend: function () {
            $(ul).hide();
            $('.spinner-border').show();
        },
        success: function (data) {
            ul.find('li').remove();
            const uploadResult = jQuery.parseJSON(data);
            if (uploadResult.Data.ResultStatus === 0) {
                $.each(uploadResult.Data.Uploads.$values,
                    function (index, upload) {
                        const newUpload = getJsonNetObject(upload, uploadResult.Data.Uploads.$values);
                        ul.append(
                            `<li class="symbol symbol-lg-120 mr-7 mb-7 float-left symbol-label">
                                <label class="checkbox checkbox-single checkbox-primary mb-0 symbol-label media-file-size">
                                    <input type="checkbox" class="check-image checkable" disabled="disabled" value="${newUpload.Id}"/>
                                    <span class="item-hide"></span>
                                    <img class="image-size" src="/assets/img/${newUpload.FileName}" alt="${newUpload.AltText}" title="${newUpload.Title}" data-id="${newUpload.Id}">
                                </label>
                            </li>`);
                    });
                $('.spinner-border').hide();
                $(ul).fadeIn(1400);
            } else {
                toastr.error(`${uploadResult.Data.Message}`, 'İşlem Başarısız!');
            }
        },
        error: function (err) {
            $('.spinner-border').hide();
            $(ul).fadeIn(1000);
            toastr.error(`${err.responseText}`, 'Hata!');
        }
    });
};

$('#btnYenile').click(function () {
    refresh();
});

$('#btnTopluSecim').click(function () {
    topluSecim = true;
    $('#listImages li label img').addClass("opacity-60");
    $('#btnTopluSecim').addClass("item-hide");
    $('#secilenDosyalar').removeClass('item-hide');
    $('#btnTopluSil').removeClass('item-hide');
    $('#btnVazgec').removeClass('item-hide');
    $(span).removeClass("item-hide");
    $(check).removeAttr("disabled");
});

$('#btnVazgec').click(function () {
    topluSecim = false;
    $('#listImages li label img').removeClass("opacity-60");
    $('#btnTopluSecim').removeClass("item-hide");
    $('#secilenDosyalar').addClass('item-hide');
    $('#btnTopluSil').addClass('item-hide');
    $('#btnVazgec').addClass('item-hide');
    $(span).addClass("item-hide");
    $(check).attr("disabled", "disabled");
    $('#listImages').find('input[type=checkbox]:checked').prop("checked", false);
    $('#kt_datatable_selected_records').html("0");
});

$(document).on('change', '#uploadFile', function (event) {
    event.preventDefault();
    var input = document.getElementById('uploadFile');
    var files = input.files;
    var formData = new FormData();

    for (var i = 0; i != files.length; i++) {
        formData.append("files", files[i]);
    }

    $.ajax({
        url: "/Admin/Medya/Ekle",
        data: formData,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            refresh();
        },
        error: function (err) {
            toastr.error(`${err.responseText}`, "Hata!");
        }
    });
});

$(function () {
    const url = '/Admin/Medya/Duzenle';
    $(document).on('click',
        '#listImages li label img',
        function () {
            if (topluSecim === false) {
                const id = $(this).attr('data-id');
                $.get(url, { uploadId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function () {
                    toastr.error("Bir hata oluştu.");
                });
            }
        });

    placeHolderDiv.on('click',
        '#btnKaydet',
        function (event) {
            event.preventDefault();

            const form = $('#form-upload-update');
            const actionUrl = form.attr('action');
            const dataToSend = form.serialize();
            $.post(actionUrl, dataToSend).done(function (data) {
                const uploadUpdateAjaxModel = jQuery.parseJSON(data);
                const newFormBody = $('.modal-body', uploadUpdateAjaxModel.UploadUpdatePartial);
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    toastr.success(`${uploadUpdateAjaxModel.UploadDto.Message}`, "Başarılı İşlem!");
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
        const imageTitle = $('#AltText').val();
        Swal.fire({
            title: 'Silmek istediğinize emin misiniz?',
            text: `${imageTitle} adlı  dosya silinecektir!`,
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
                    data: { uploadId: id },
                    url: '/Admin/Medya/Sil/',
                    success: function (data) {
                        const uploadResult = jQuery.parseJSON(data);
                        if (uploadResult.ResultStatus === 0) {
                            Swal.fire(
                                'Silindi!',
                                `${uploadResult.Message}`,
                                'success'
                            );
                            placeHolderDiv.find('.modal').modal('hide');
                            var deletedLi = $('#listImages').find(`img[data-id=${id}]`).closest('li');
                            deletedLi.fadeOut(1500, function () {
                                deletedLi.remove();
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Başarısız İşlem!',
                                text: `${uploadResult.Message}`
                            });
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

$('#btnTopluSil').click(function () {
    var seciliDosyalar = $('#listImages li .checkbox > input[type=checkbox]:checked');
    if (seciliDosyalar.length > 0 && topluSecim === true) {
        var ids = [];
        seciliDosyalar.each(function () {
            var id = parseInt($(this).val());
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
                    url: '/Admin/Medya/TopluSil',
                    data: { uploadIds: ids },
                    success: function (data) {
                        const uploadResult = jQuery.parseJSON(data);
                        if (uploadResult.ResultStatus === 0) {
                            seciliDosyalar.each(function () {
                                var deletedLi = $(this).closest("li");
                                deletedLi.fadeOut(1500, function () {
                                    deletedLi.remove();
                                });
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Başarısız İşlem!',
                                text: `${uploadResult.Message}`,
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

$('#uploadSearch').keyup(function () {
    var filter = $(this).val().toLowerCase();
    if (filter.length >= 3) {
        console.log($('#listImages li').find('img').length);
        $.each($('#listImages li').find('img'), function () {
            var name = $(this).attr('src');

            if (name.indexOf(filter) == -1) {
                $(this).closest('li').addClass('d-none');
            }
            else {
                $(this).closest('li').removeClass('d-none');
            }
        });
    }
    else {
        $('li').removeClass('d-none');
    }
});