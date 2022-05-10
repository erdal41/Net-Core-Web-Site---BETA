$(document).ready(function () {

    // Trumbowyg

    $('#text-editor').trumbowyg({
        lang: 'tr',
        btns: [
            ['viewHTML'],
            ['undo', 'redo'], // Only supported in Blink browsers
            ['formatting'],
            ['strong', 'em', 'del'],
            ['superscript', 'subscript'],
            ['link'],
            ['insertImage'],
            ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
            ['unorderedList', 'orderedList'],
            ['horizontalRule'],
            ['removeformat'],
            ['fullscreen'],
            ['foreColor', 'backColor'],
            ['emoji'],
            ['fontfamily'],
            ['fontsize']
        ],
        plugins: {
            colors: {
                foreColorList: [
                    'ff0000', '00ff00', '0000ff', '54e346'
                ],
                backColorList: [
                    '000', '333', '555'
                ],
                displayAsList: false
            }
        }
    });

    // Trumbowyg

    // Select2

    $('#categoryList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen bir kategori seçiniz...",
        allowClear: true
    });

    // Select2

    var url = '/Admin/Medya/GetAllUploadsPartial/';
    var modalUploadList = $('#modalUploadList');
    $(document).on('click',
        '#uploadChoose',
        function (event) {
            event.preventDefault();
            $.get(url).done(function (data) {
                modalUploadList.html(data);
                modalUploadList.find('.modal').modal('show');
            }).fail(function (err) {
                toastr.error(`${err.responseText}`, 'Hata!');
            });
        });

    $('#uploadRemove').click(function (event) {
        event.preventDefault();
        imageDiv.css('background-image', "");
    });
});