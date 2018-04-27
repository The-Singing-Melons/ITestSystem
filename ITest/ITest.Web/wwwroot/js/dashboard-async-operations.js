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
            <input id="testName" name="testName" type="hidden" value="{{t_name}}" />
            <input id="categoryName" name="categoryName" type="hidden" value="{{c_name}}" />

            <button type="submit" class="btn btn-danger btn-xs">
                <span class="glyphicon glyphicon-ban-circle"></span>
                Disable
            </button>
        </form>`

    var draftTestActionsFrame =
        `<form action="/Admin/Manage/PublishTest" method="post" class="publish-test">
            <input id="testName" name="testName" type="hidden" value="{{t_name}}" />
            <input id="categoryName" name="categoryName" type="hidden" value="{{c_name}}" />

            <button type="submit" class="btn btn-success btn-xs">
                <span class="glyphicon glyphicon-ok"></span>
                Publish
            </button>
        </form>

        <form action="/Admin/Manage/EditTest" method="get">
            <input id="testName" name="testName" type="hidden" value="{{t_name}}" />
            <input id="categoryName" name="categoryName" type="hidden" value="{{c_name}}" />

            <button type="submit" class='btn btn-warning btn-xs'>
                <span class="glyphicon glyphicon-edit"></span>
                Edit
            </button>
        </form>

        <form action="/Admin/Manage/DeleteTest" method="post" class="delete-test">
            <input id="testName" name="testName" type="hidden" value="{{t_name}}" />
            <input id="categoryName" name="categoryName" type="hidden" value="{{c_name}}" />

            <button type="submit" class="btn btn-danger btn-xs">
                <span class="glyphicon glyphicon-remove"></span>
                Delete
            </button>
        </form>`;

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
            success: function (isPublished) {
                if (isPublished === true) {
                    isPublishedCol.text('Published');

                    var actionsHtml = publishedTestActionsFrame
                        .replace(/\{\{\t_name\}\}/g, testName)
                        .replace(/\{\{\c_name\}\}/g, categoryName);

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
});