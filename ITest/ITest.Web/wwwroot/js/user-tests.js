$(function () {
    $('.test-start-btn').on('click', function (event) {
        event.preventDefault();

        var category = $('.active a').data('category').replace(/\s+/g, '+');

        var url = '/user/home/GetRandomTest/' + category;

        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                window.location.href = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
});