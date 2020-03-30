$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    generales();
    removeMenuPanel();

    $(".Datetime1").mask("99/99/9999");
    $(".Datetime1").css("width", "100px");
    $(".Datetime1").attr("maxlength", 10);
    //$(".Datetime1").datepicker();
});