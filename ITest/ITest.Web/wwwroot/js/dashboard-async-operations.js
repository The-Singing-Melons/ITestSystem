$(function () {
    var publishTestSubmitEvent = $('.created-tests-table').on('submit', '.publish-test', function (event) {
        event.preventDefault();

        var form = $(this);

        var id = form.children('#id').val();

        var isPublishedCol = form.closest('tr').children('.is-published');
        var actionsCol = form.closest('.actions-col');

        var url = this.action;
        var data = form.serialize();

        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            success: function (response) {
                if (response.value.isPublished === true) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/Manage/GetPublishedTestPartial',
                        data: data,
                        success: function (partial) {
                            isPublishedCol.text('Published');
                            actionsCol.html(partial);
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            console.log(xhr);
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
                alert(xhr.status);
                alert(thrownError);
            }
        });

    });

    var deleteTestSubmitEvent = $('.created-tests-table').on('submit', '.delete-test', function (event) {
        event.preventDefault();

        var form = $(this);

        var table = form.closest('.created-tests-table').DataTable();

        var id = form.children('#Id').val();

        var testRow = form.closest('tr');

        var url = this.action;
        var data = form.serialize();

        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            success: function (isDeleted) {
                if (isDeleted === true) {
                    table
                        .row(testRow)
                        .remove()
                        .draw();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });

    var disableTestSubmitEvent = $('.created-tests-table').on('submit', '.disable-test', function (event) {
        event.preventDefault();

        var form = $(this);

        var id = form.children('#Id').val();

        var isPublishedCol = form.closest('tr').children('.is-published');
        var actionsCol = form.closest('.actions-col');

        var url = this.action;
        var data = form.serialize();

        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            success: function (response, status, headers) {
                if (response.value.isDisabled === true) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/Manage/GetDisabledTestPartial',
                        data: data,
                        success: function (partial) {
                            isPublishedCol.text('Draft');
                            actionsCol.html(partial);
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            console.log(xhr);
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
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