$(document).ready(function () {
    var ul = $("#listImages");
    var imageDiv = $('.image-input-wrapper');
    $('#modalWindow').on('click', '#btnUploadChoose', function (event) {
        event.preventDefault();
        var checkedRadio = $('#listImages').find('input[type=radio]:checked');
        var selectedUpload = "";
        imageDiv = $('.image-input-wrapper');
        if (checkedRadio.length == 1) {
            checkedRadio.each(function () {
                selectedUpload = $(this).closest('label').find('img').attr('src');
            });
            imageDiv.css('background-image', "url('" + selectedUpload + "')");
            imageDiv.css('background-size', "100% 100%");
            let imagePathArry = selectedUpload.split("/");
            let imageSource = imagePathArry[imagePathArry.length - 1]
            $('#Picture').val(imageSource);
            $('#modalUploadList').find('.modal').modal('hide');
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
                            <label class="radio radio-rounded mb-0 symbol-label media-file-size">
                                <input class="checkable" type="radio" name="radios15_1" value="${newUpload.Id}" />
                                <span></span>
                                    <img class="choose-image image-size" src="/assets/img/${newUpload.FileName}" alt="${newUpload.AltText}" title="${newUpload.Title}" data-id="${newUpload.Id}">
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

    $('#btnRefresh').click(function () {
        refresh();
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
});