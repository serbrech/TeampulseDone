$(function () {
    $(".description-link").click(function () {
        $(this).closest("tr").nextAll(".description:first").toggle();
    });

    $.datepicker.setDefaults({ dateFormat: $("#date-pattern").val() });
    $('input.datepicker').datepicker();
});