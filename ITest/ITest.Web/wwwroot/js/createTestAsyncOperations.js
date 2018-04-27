$(function () {
    var questionFrame =
        `<div id="question-{{q_id}}" class="question-container panel panel-default">
            <button class="delete-question btn btn-danger btn-xs pull-right" type="button">
                <span class="glyphicon glyphicon-remove"></span>
            </button>
            <a href="#collapse-{{q_id}}" data-toggle="collapse" data-parent="#accordion">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        Question {{q_number}}
                    </h4>
                </div>
            </a>
            <div id="collapse-{{q_id}}" class="panel-collapse collapse">
                <div class="panel-body">
                    <div class="question-description">
                        <h3>Description</h3>
                        <input type="text" id="Questions_{{q_id}}__Body" name="Questions[{{q_id}}].Body" class="summernote form-control" value=""
                    </div>
                    <div class="answers-container">
                    </div>
                </div>
                <div class="panel-body">
                    <button class="add-answer btn btn-default pull-right" name="collapse-{{q_id}}" type="button">Add Answer</button>
                </div>
            </div>
        </div>`;

    var answerFrame =
        `<div id="question-{{q_id}}-answer-{{a_id}}" class="answer-container">
            <h3 class="form-inline">Answer {{a_number}}</h3>
            <button class="delete-answer btn btn-danger btn-xs pull-right" type="button">
                <span class="glyphicon glyphicon-remove"></span>
            </button>
            <label class="btn btn-success pull-right">
                <input id="Questions_{{q_id}}__Answers_{{a_id}}__IsCorrect" name="radio-{{q_id}}" class="answer-is-correct" type="radio" value="true" autocomplete="off"
                                data-val="true"
                                data-val-length="Answers content's length must be maximum 500 symbols!"
                                data-val-length-max="500"
                                data-val-required="Please enter the Answer's content!"/>
                <span class="glyphicon glyphicon-ok"></span>
            </label>
            <input id="Questions_{{q_id}}__Answers_{{a_id}}__Content" name="Questions[{{q_id}}].Answers[{{a_id}}].Content" class="answer-content summernote form-control"/>
        </div>`;

    var noQuestionFrame =
        `<div>
            <h4 class="w-100 p-3">You need to add Questions to your Test</h4>
        </div>`;

    var noAnswersFrame =
        `<div class="no-answers">
            <h4 class="w-100 p-3">You need to add Answers to your Question</h4>
        </div>`;

    var radioButtonClick = function (questionNumber) {
        var answerRadioButtons = $(`#question-${questionNumber} .answers-container .answer-container input`)
            .filter(function () {
                return this.type === 'radio';
            });

        var hasCheckedRadioButton = answerRadioButtons
            .is(function () {
                return $(this).prop('checked') === true;
            });

        if (!hasCheckedRadioButton) {
            answerRadioButtons.first().prop('checked', true);
        }
    };
    var collapseQuestions = function () {
        $('#questions-container .question-container > a')
            .filter(function () {
                var isCollapsed = $(`#${this.parentNode.id} > div`)[0]
                    .classList
                    .contains('in');

                return isCollapsed;
            })
            .click();
    }

    var collapseQuestionsClickEvent = $('#questions-container #collapse-questions').on('click', collapseQuestions);

    var addQuestionClickEvent = $('#questions-container #add-question').on('click', function () {
        collapseQuestions();

        var newQuestionId = $('#questions-body .question-container').length;
        var questionHtml = questionFrame
            .replace(/\{\{\q_id\}\}/g, newQuestionId)
            .replace(/\{\{\q_number\}\}/g, newQuestionId + 1);

        if (newQuestionId === 0) {
            $('#questions-container #questions-body')
                .html(questionHtml);
        }
        else {
            $('#questions-container #questions-body')
                .append(questionHtml);
        }

        $(`#questions-container #questions-body #question-${newQuestionId} a`).click();

        var addAnswerButton = $(`#questions-container #questions-body #question-${newQuestionId} .add-answer`);
        addAnswerButton.click();
        addAnswerButton.click();
        addAnswerButton.click();
    });
    var deleteQuestionClickEvent = $('#questions-container #questions-body').on('click', '.delete-question', function () {
        var questionId = parseInt(this.parentNode.id.split('-')[1]);

        $(this.parentNode).remove();

        var nextQuestions = $('#questions-body .question-container')
            .filter(function () {
                var nextQuestionId = parseInt(this.id.split('-')[1]);
                return nextQuestionId > questionId;
            })
            .toArray();

        if ($('#questions-body .question-container').length === 0) {
            $('#questions-body').html(noQuestionFrame);
        }
        else {
            nextQuestions.forEach(function (question) {
                var newQuestionId = parseInt(question.id.split('-')[1]) - 1;

                $(question).attr('id', `question-${newQuestionId}`);
                $(`#question-${newQuestionId} a`).attr('href', `#collapse-${newQuestionId}`);
                $(`#question-${newQuestionId} a h4`).text(`Question ${newQuestionId + 1}`);
                $(`#question-${newQuestionId} .panel-collapse`).attr('id', `collapse-${newQuestionId}`);
                $(`#question-${newQuestionId} .question-description input`).attr('id', `Questions_${newQuestionId}__Body`);
                $(`#question-${newQuestionId} .question-description input`).attr('name', `Questions[${newQuestionId}].Body`);

                $(`#question-${newQuestionId} .add-answer`).attr('name', `collapse-${newQuestionId}`);

                var answers = $(`#Question-${newQuestionId} .answer-container`).toArray();

                answers.forEach(function (answer) {
                    var answerId = parseInt(answer.id.split('-')[3]) - 1;
                    $(`#question-${newQuestionId} .answer-container .answer-content`).attr('id', `Questions_${newQuestionId}__Answers_${answerId}__Content`);

                    $(`#question-${newQuestionId} .answer-container .answer-content`).attr('id', `Questions_${newQuestionId}__Answers_${answerId}__Content`);
                    $(`#question-${newQuestionId} .answer-container .answer-content`).attr('name', `Questions[${newQuestionId}].Answers[${answerId}].Content`);
                });
            });
        }
    });

    var addAnswerClickEvent = $('#questions-container #questions-body').on('click', '.add-answer', function () {
        var questionId = this.name;
        var questionNumber = questionId.split('-')[1];
        var newAnswerNumber = $(`#${questionId} .answers-container .answer-container`).length;

        var answerHtml = answerFrame
            .replace(/\{\{\q_id\}\}/g, questionNumber)
            .replace(/\{\{\a_id\}\}/g, newAnswerNumber)
            .replace(/\{\{\a_number\}\}/g, newAnswerNumber + 1);

        if (newAnswerNumber === 0) {
            $(`#${questionId} .answers-container`)
                .html(answerHtml);
        }
        else {
            $(`#${questionId} .answers-container`)
                .append(answerHtml);
        }

        radioButtonClick(questionNumber);
    });
    var deleteAnswerClickEvent = $('#questions-container #questions-body').on('click', '.delete-answer', function () {
        var answerId = this.parentNode.id.split('-');
        var questionId = parseInt(answerId[1]);
        var answerNumber = parseInt(answerId[3]);

        $(this.parentNode).remove();

        var nextAnswers = $(`#questions-body #question-${questionId} .answer-container`)
            .filter(function () {
                var nextAnswerNumber = parseInt(this.id.split('-')[3]);
                return nextAnswerNumber > answerNumber;
            })
            .toArray();

        if ($(`#questions-body #question-${questionId} .answer-container`).length === 0) {
            $(`#questions-body #question-${questionId} .answers-container`).html(noAnswersFrame);
        }
        else {
            nextAnswers.forEach(function (a) {
                var newAnswerNumber = parseInt(a.id.split('-')[3]) - 1;

                $(a).attr('id', `question-${questionId}-answer-${newAnswerNumber}`);
                $(`#question-${questionId}-answer-${newAnswerNumber} h3`).text(`Answer ${newAnswerNumber + 1}`);
                $(`#question-${questionId}-answer-${newAnswerNumber} .answer-content`).attr('id', `Questions_${questionId}__Answers_${newAnswerNumber}__Content`);
                $(`#question-${questionId}-answer-${newAnswerNumber} .answer-content`).attr('name', `Questions[${questionId}].Answers[${newAnswerNumber}].Content`);
            });

            radioButtonClick(questionId);
        }
    });

    var createTestClickEvent = $('.create-test').on('click', function () {
        $('#questions-container #questions-body .answer-is-correct')
            .toArray()
            .forEach(function (rButton) {
                var params = $(rButton).closest('.answer-container')[0].id.split('-');
                var questionId = params[1];
                var answerNumber = params[3];

                $(rButton).attr('name', `Questions[${questionId}].Answers[${answerNumber}].IsCorrect`);
            });
    });
});