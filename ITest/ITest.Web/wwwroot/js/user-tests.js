$(function () {
    $('.panel-heading ul li a').first().click();

    $('.test-start-btn').on('click', function (event) {
        event.preventDefault();

        var category = $('.active a').data('category').replace(/\s+/g, '+');

        var url = '/user/home/GetRandomTest/' + category;

        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                if (data.isSuccessful) {
                    window.location.href = data.url;
                }
                else {
                    alert('Currently there are no Tests for this Category');
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
});