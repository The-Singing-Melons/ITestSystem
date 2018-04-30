$(function () {
    var publishedTestActionsFrame =
        `<a class='btn btn-success btn-xs disabled'>
            <span class="glyphicon glyphicon-ok"></span>
            Published
        </a>
        <a class='btn btn-warning btn-xs disabled'>
            <span class="glyphicon glyphicon-edit"></span>
            Edit
        </a>

        <form action="/Admin/Manage/DisableTest" method="post" class="disable-test">
            <input id="testName" name="testName" type="hidden" value="{{e_name}}" />
            <input id="categoryName" name="categoryName" type="hidden" value="{{a_name}}" />

            <button type="submit" class="btn btn-danger btn-xs">
                <span class="glyphicon glyphicon-ban-circle"></span>
                Disable
            </button>
        </form>`

    var disabledTestActionsFrame =
        `<form action="/Admin/Manage/PublishTest" method="post" class="publish-test">
            <input id="testName" name="testName" type="hidden" value="{{e_name}}" />
            <input id="categoryName" name="categoryName" type="hidden" value="{{a_name}}" />

            <button type="submit" class="btn btn-success btn-xs">
                <span class="glyphicon glyphicon-ok"></span>
                Publish
            </button>
        </form>

        <a class='btn btn-warning btn-xs disabled'>
            <span class="glyphicon glyphicon-edit"></span>
            Edit
        </a>

        <a class='btn btn-danger btn-xs disabled'>
            <span class="glyphicon glyphicon-remove"></span>
            Delete
        </a>`;

    var token = '';

    var publishTestSubmitEvent = $('.created-tests-table').on('submit', '.publish-test', function (event) {
        event.preventDefault();

        var testName = $(this).children('#testName').val();
        var categoryName = $(this).children('#categoryName').val();

        var isPublishedCol = $(this).closest('tr').children('.is-published');
        var actionsCol = $(this).closest('td');

        var url = this.action;
        var data = $(this).serialize();

        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            headers: { 'X-CSRF-TOKEN': token },
            success: function (response, status, headers) {
                //token = response.value.token;

                if (response.value.isPublished === true) {
                    isPublishedCol.text('Published');

                    var actionsHtml = publishedTestActionsFrame
                        .replace(/\{\{\e_name\}\}/g, testName)
                        .replace(/\{\{\a_name\}\}/g, categoryName);

                    actionsCol.html(actionsHtml);
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

        var table = $(this).closest('.created-tests-table').DataTable();

        var testName = $(this).children('#testName').val();
        var categoryName = $(this).children('#categoryName').val();

        var testRow = $(this).closest('tr');

        var url = this.action;
        var data = $(this).serialize();

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

        var testName = $(this).children('#testName').val();
        var categoryName = $(this).children('#categoryName').val();

        var isPublishedCol = $(this).closest('tr').children('.is-published');
        var actionsCol = $(this).closest('td');

        var url = this.action;
        var data = $(this).serialize();

        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            headers: { 'X-CSRF-TOKEN': token },
            success: function (response, status, headers) {
                //token = response.value.token;

                if (response.value.isDisabled === true) {
                    isPublishedCol.text('Draft');

                    var actionsHtml = disabledTestActionsFrame
                        .replace(/\{\{\e_name\}\}/g, testName)
                        .replace(/\{\{\a_name\}\}/g, categoryName);

                    actionsCol.html(actionsHtml);
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