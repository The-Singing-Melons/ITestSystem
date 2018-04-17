var counter = 1;

$('#questions-container .add-question').on('click', function () {
    var url = '/Admin/Manage/LoadQuestion/';

    $.get(url, function (questionPartial) {
        var questionHtml = $(questionPartial);
        $(questionHtml[0].childNodes[3]).attr('href', `#collapse${counter}`);
        $(questionHtml[0].childNodes[5]).attr('id', `collapse${counter}`);

        $('#questions-container .questions-body').append(questionHtml);

        $('.summernote').summernote({
            placeholder: 'Add your description here...'
        });

        counter++;
    });
});

$('#questions-container .questions-body').on('click', '.add-answer',function () {
    console.log('tuk');

    var url = '/Admin/Manage/LoadAnswer/';

    $.get(url, function (answerPartial) {
        var answerHtml = $(answerPartial);

        $('#questions-container .answers-container').append(answerHtml);

        $('.summernote').summernote();
    });
});

$('#questions-container').on('click', '.delete-question',function () {
    console.log(this);
});