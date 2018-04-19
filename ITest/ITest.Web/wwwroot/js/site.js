var testOperationsAsynch = (function () {
    var _questionNumber = 1;

    var addQuestionClickEvent = $('#questions-container .add-question').on('click', function () {
        var url = '/Admin/Manage/LoadQuestion/';

        $.get(url, function (questionPartial) {
            var questionHtml = $(questionPartial);
            questionHtml.find('a').attr('href', `#collapse-${_questionNumber}`);
            questionHtml.find('.panel-collapse').attr('id', `collapse-${_questionNumber}`);
            questionHtml.find('.add-answer').attr('name', `collapse-${_questionNumber}`);

            $('#questions-container .questions-body').append(questionHtml);

            $('.summernote').summernote({
                placeholder: 'Add your description here...',
                height: 200,

            });

            _questionNumber++;
        });
    });

    var deleteQuestionClickEvent = $('#questions-container').on('click', '.delete-question', function () {
        console.log(this);
    });

    var addAnswerClickEvent = $('#questions-container .questions-body').on('click', '.add-answer', function () {
        var questionNumber = this.name.split('-')[1];

        var url = '/Admin/Manage/LoadAnswer/';

        $.get(url, function (answerPartial) {
            var answerHtml = $(answerPartial);

            answerHtml.attr('class', `answer-container-${questionNumber}`);

            answerHtml.find('input').attr('name', `radio-${questionNumber}`);

            $(`#questions-container #collapse-${questionNumber} .answers-container`).append(answerHtml);

            $('.summernote').summernote({
                height: 200
            });
        });
    });

    var createTestClickEvent = $('#create-test').on('click', function (event) {
        event.preventDefault();
        var testJson = $(this.parentNode).serialize();

        console.log(testJson.split('&'));
    });

    return {
        addQuestionClickEvent: addQuestionClickEvent,
        deleteQuestionClickEvent: deleteQuestionClickEvent,
        addAnswerClickEvent: addAnswerClickEvent,
        createTestClickEvent: createTestClickEvent
    };
})();