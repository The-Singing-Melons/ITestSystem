$(function () {
    $('#test-start-btn').on('click', function (event) {
        event.preventDefault();

        var category = $('.active a').data('category').replace(/\s+/g, '+');

        console.log(category);
        var url = '/user/home/GetRandomTest/' + category;

        $.ajax({
            type: "GET",
            url: url,
            success: function (response) {
                console.log(response);
            }
        });
    })
})