var testOperationsAsynch = (function () {
    var _questionNumber = 0;

    var questionFrame =
        `<div class="panel panel-default" id="Qiestion-{{q_id}}">
            <button type="button" class="btn btn-danger btn-xs pull-right delete-question">
                <span class="glyphicon glyphicon-remove"></span>
            </button>
            <a href="#collapse-{{q_id}}" data-toggle="collapse" data-parent="#accordion">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        Question {{q_number}}
                    </h4>
                </div>
            </a>
            <div class="panel-collapse collapse" id="collapse-{{q_id}}">
                <div class="panel-body">
                    <div class="question-description">
                        <h3>Description</h3>
                        <input type="text" id="Questions_{{q_id}}__Body" name="Questions[{{q_id}}].Body" value="" class="summernote form-control"></input>
                    </div>
                    <div class="answers-container">

                    </div>
                </div>
                <div class="panel-body">
                    <button type="button" name="collapse-{{q_id}}" class="btn btn-default pull-right add-answer">Add Answer</button>
                </div>
            </div>
        </div>`;

    var answerFrame =
        `<div class="answer-container">
            <h3 class="form-inline">Answer {{a_number}}</h3>
            <label class="btn btn-success pull-right">
                <input type="radio" id="Questions_{{q_id}}__Answers_{{a_id}}__IsCorrect" name="radio-{{q_id}}" value="true" autocomplete="off" chacked>
                <span class="glyphicon glyphicon-ok"></span>
            </label>
            <input id="Questions_{{q_id}}__Answers_{{a_id}}__Content" name="Questions[{{q_id}}].Answers[{{a_id}}].Content" class="summernote form-control"></input>
        </div>`;

    var addQuestionClickEvent = $('#questions-container #add-question').on('click', function () {
        $('#questions-container .questions-body').append(questionFrame.replace(/\{\{\q_id\}\}/g, _questionNumber).replace(/\{\{\q_number\}\}/g, _questionNumber + 1));

        _questionNumber++;
    });

    var deleteQuestionClickEvent = $('#questions-container').on('click', '.delete-question', function () {
        console.log(this);
    });

    var addAnswerClickEvent = $('#questions-container .questions-body').on('click', '.add-answer', function () {
        var questionId = this.name;
        var questionNumber = questionId.split('-')[1];

        var newAnswerNumber = $('#' + questionId + ' .answers-container .answer-container').length;

        $('#' + questionId + ' .answers-container').append(answerFrame.replace(/\{\{\q_id\}\}/g, questionNumber).replace(/\{\{\a_id\}\}/g, newAnswerNumber).replace(/\{\{\a_number\}\}/g, newAnswerNumber + 1));
    });

    var createTestClickEvent = $('#create-test').on('click', function (event) {
        //event.preventDefault();
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