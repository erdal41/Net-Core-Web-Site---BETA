$(document).ready(function () {

    $('#categoryList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen bir kategori seçiniz...",
        allowClear: true
    });

    $('#filterByList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen bir filtre türü seçiniz...",
        allowClear: true
    });

    $('#orderByList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen bir sıralama türü seçiniz...",
        allowClear: true
    });

    $('#isAscendingList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen bir sıralama tipi seçiniz...",
        allowClear: true
    });
});