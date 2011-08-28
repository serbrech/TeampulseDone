$(function () {
    $(".description-link").click(function () {
        $(this).closest("tr").nextAll(".description:first").toggle();
    });
});