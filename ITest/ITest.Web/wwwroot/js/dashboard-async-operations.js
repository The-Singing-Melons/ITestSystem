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

        <form action="/Admin/Manage/DisableTest" method="get" class="disable-test">
            <input id="testName" name="testName" type="hidden" value="{{t_name}}" />
            <input id="categoryName" name="categoryName" type="hidden" value="{{c_name}}" />

            <button type="button" class="btn btn-danger btn-xs" data-toggle="modal" data-target="#disable-modal">
                <span class="glyphicon glyphicon-ban-circle"></span>
                Disable
            </button>

            <div class="modal fade" id="disable-modal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLongTitle">Modal title</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            Are you sure you want to Disable this Test?
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Nope</button>
                            <button type="submit" class="btn btn-primary">Yes</button>
                        </div>
                    </div>
                </div>
            </div>
        </form>`

    var publishTestSubmitEvent = $('.publish-test').on('submit', function (event) {
        event.preventDefault();

        var testName = $(this).children('#testName').val();
        var categoryName = $(this).children('#categoryName').val();

        var isPublishedCol = $(this).closest('tr').children('.is-published')
        var actionsCol = $(this).closest('td');

        var url = this.action;
        var data = $(this).serialize();

        $.ajax({
            type: 'GET',
            url: `${url}?${data}`,
            success: function (response) {
                if (response) {
                    isPublishedCol.text('Published');

                    var actionsHtml = publishedTestActionsFrame
                        .replace(/\{\{\t_name\}\}/g, testName)
                        .replace(/\{\{\c_name\}\}/g, categoryName);

                    actionsCol.html(actionsHtml);
                }
                else {
                    // already published?
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