﻿$(function () {
    var questionFrame =
        `<div id="question-{{q_id}}" class="question-container panel panel-default">
            <button class="delete-question btn btn-danger pull-right" type="button">
                <span class="glyphicon glyphicon-remove"></span>
            </button>
            <a href="#collapse-{{q_id}}" data-toggle="collapse" data-parent="#accordion">
                <div class="panel-heading bg-primary">
                    <h4 class="panel-title">
                        Question {{q_number}}
                    </h4>
                </div>
            </a>
            <div id="collapse-{{q_id}}" class="panel-collapse collapse">
                <div class="panel-body">
                    <div class="question-description">
                        <h3>Description</h3>

                        <input type="hidden" id="Questions_{{q_id}}__BodyPlaintext" name="Questions[{{q_id}}].BodyPlaintext" class="question-body-plaintext plaintext" value=""/>
                        <textarea id="Questions_{{q_id}}__Body" name="Questions[{{q_id}}].Body" class="question-body summernote form-control" ></textarea>
                    </div>
                    <div class="answers-container">
                        
                        <div id="question-{{q_id}}-answer-0" class="answer-container">                            
                            <div class="row answer-header">
                                <div class="col">
                                    <h3>Answer 1</h3>

                                    <button class="delete-answer btn btn-danger pull-right" type="button">
                                        <span class="glyphicon glyphicon-remove"></span>
                                    </button>

                                    <label class="btn btn-success pull-right">
                                        <input id="Questions_{{q_id}}__Answers_0__IsCorrect" name="radio-{{q_id}}" class="answer-is-correct" type="radio" value="true" autocomplete="off" checked/>
                                    </label>
                                </div>
                            </div>

                            <input type="hidden" id="Questions_{{q_id}}__Answers_0__ContentPlaintext" name="Questions[{{q_id}}].Answers[0].ContentPlaintext" class="answer-plaintext plaintext" value=""/>
                            <textarea id="Questions_{{q_id}}__Answers_0__Content" name="Questions[{{q_id}}].Answers[0].Content" class="answer-content summernote form-control"></textarea>
                        </div>

                        <div id="question-{{q_id}}-answer-1" class="answer-container">
                            <div class="row answer-header">
                                <div class="col">
                                    <h3>Answer 2</h3>
                                </div>

                                <div class="col">
                                    <button class="delete-answer btn btn-danger pull-right" type="button">
                                        <span class="glyphicon glyphicon-remove"></span>
                                    </button>

                                    <label class="btn btn-success pull-right">
                                        <input id="Questions_{{q_id}}__Answers_1__IsCorrect" name="radio-{{q_id}}" class="answer-is-correct" type="radio" value="true" autocomplete="off"/>
                                    </label>
                                </div>
                            </div>

                            <input type="hidden" id="Questions_{{q_id}}__Answers_1__ContentPlaintext" name="Questions[{{q_id}}].Answers[1].ContentPlaintext" class="answer-plaintext plaintext" value=""/>
                            <textarea id="Questions_{{q_id}}__Answers_1__Content" name="Questions[{{q_id}}].Answers[1].Content" class="answer-content summernote form-control"></textarea>
                        </div>

                        <div id="question-{{q_id}}-answer-2" class="answer-container">
                            <div class="row answer-header">
                                <div class="col">
                                    <h3>Answer 3</h3>
                                </div>

                                <div class="col">
                                    <button class="delete-answer btn btn-danger pull-right" type="button">
                                        <span class="glyphicon glyphicon-remove"></span>
                                    </button>

                                    <label class="btn btn-success pull-right">
                                        <input id="Questions_{{q_id}}__Answers_2__IsCorrect" name="radio-{{q_id}}" class="answer-is-correct" type="radio" value="true" autocomplete="off"/>
                                    </label>
                                </div>
                            </div>

                            <input type="hidden" id="Questions_{{q_id}}__Answers_2__ContentPlaintext" name="Questions[{{q_id}}].Answers[2].ContentPlaintext" class="answer-plaintext plaintext" value=""/>
                            <textarea id="Questions_{{q_id}}__Answers_2__Content" name="Questions[{{q_id}}].Answers[2].Content" class="answer-content summernote form-control"></textarea>
                        </div>

                    </div>
                </div>
                <div class="panel-body">
                    <button class="add-answer btn btn-default pull-right" name="collapse-{{q_id}}" type="button">Add Answer</button>
                </div>
            </div>
        </div>`;

    var answerFrame =
        `<div id="question-{{q_id}}-answer-{{a_id}}" class="answer-container">
            <div class="row answer-header">
                <div class="col">
                    <h3>Answer {{a_number}}</h3>
                </div>

                <div class="col">
                    <button class="delete-answer btn btn-danger pull-right" type="button">
                        <span class="glyphicon glyphicon-remove"></span>
                    </button>

                    <label class="btn btn-success pull-right">
                        <input id="Questions_{{q_id}}__Answers_{{a_id}}__IsCorrect" name="radio-{{q_id}}" class="answer-is-correct" type="radio" value="true" autocomplete="off"/>
                    </label>
                </div>
            </div>

            <input type="hidden" id="Questions_{{q_id}}__Answers_{{a_id}}__ContentPlaintext" name="Questions[{{q_id}}].Answers[{{a_id}}].ContentPlaintext" class="answer-plaintext plaintext" value=""/>
            <textarea id="Questions_{{q_id}}__Answers_{{a_id}}__Content" name="Questions[{{q_id}}].Answers[{{a_id}}].Content" class="answer-content summernote form-control"></textarea>
        </div>`;

    var noQuestionFrame =
        `<div>
            <h4 class="w-100 p-3">You need to add Questions to your Test</h4>
        </div>`;

    var noAnswersFrame =
        `<div class="no-answers">
            <h4 class="w-100 p-3">You need to add Answers to your Question</h4>
        </div>`;

    var questionSummernoteConfig = {
        height: 200,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['font', ['strikethrough', 'superscript', 'subscript']],
            ['fontsize', ['fontsize']],
            ['para', ['ul', 'ol', 'paragraph']]
        ],
        disableResizeEditor: true,
        disableDragAndDrop: true,
        placeholder: 'Add your description here...'
    };

    var answerSummernoteConfig = {
        height: 200,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['font', ['strikethrough', 'superscript', 'subscript']],
            ['fontsize', ['fontsize']],
            ['para', ['ul', 'ol', 'paragraph']]
        ],
        disableResizeEditor: true,
        disableDragAndDrop: true
    };

    //functions
    var initializeSummernote = function () {
        $('.summernote')
            .toArray()
            .forEach(function (textarea) {
                var text = textarea.textContent;
                $(textarea).summernote(answerSummernoteConfig);
                textarea.textContent = text;
            });
    };

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

    //event listeners
    var collapseQuestionsClickEvent = $('#questions-container #collapse-questions').on('click', collapseQuestions);

    var addQuestionClickEvent = $('#questions-container #add-question').on('click', function () {
        collapseQuestions();

        var newQuestionId = $('#questions-body .question-container').length;
        var questionHtml = questionFrame
            .replace(/\{\{\q_id\}\}/g, newQuestionId)
            .replace(/\{\{\q_number\}\}/g, newQuestionId + 1);

        var question = $(questionHtml);
        question.find(`#Questions_${newQuestionId}__Body`).summernote(questionSummernoteConfig);
        question.find(`.answer-content`).summernote(answerSummernoteConfig);
        $('.note-statusbar').hide();

        if (newQuestionId === 0) {
            $('#questions-container #questions-body')
                .html(question);
        }
        else {
            $('#questions-container #questions-body')
                .append(question);
        }

        $(`#questions-container #questions-body #question-${newQuestionId} a`).click();
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
                var nextQuestion = $(question);

                nextQuestion.attr('id', `question-${newQuestionId}`);
                nextQuestion.find('a').attr('href', `#collapse-${newQuestionId}`);
                nextQuestion.find('h4').text(`Question ${newQuestionId + 1}`);
                nextQuestion.find('.panel-collapse').attr('id', `collapse-${newQuestionId}`);
                nextQuestion.find('.question-description .question-body').attr('id', `Questions_${newQuestionId}__Body`);
                nextQuestion.find('.question-description .question-body').attr('name', `Questions[${newQuestionId}].Body`);

                nextQuestion.find('.question-description .question-body-plaintext').attr('id', `Questions_${newQuestionId}__BodyPlaintext`);
                nextQuestion.find('.question-description .question-body-plaintext').attr('name', `Questions[${newQuestionId}].BodyPlaintext`);

                nextQuestion.find('.add-answer').attr('name', `collapse-${newQuestionId}`);

                nextQuestion.find('.question-id').attr('id', `Questions_${newQuestionId}__Id`);
                nextQuestion.find('.question-id').attr('name', `Questions[${newQuestionId}].Id`);

                var answers = nextQuestion.find('.answer-container').toArray();

                answers.forEach(function (answer) {
                    var answerId = parseInt(answer.id.split('-')[3]);
                    var nextQuestionAnswer = $(answer);

                    nextQuestionAnswer.attr('id', `question-${newQuestionId}-answer-${answerId}`)

                    nextQuestionAnswer.find('.answer-is-correct').attr('id', `Questions_${newQuestionId}__Answers_${answerId}__IsCorrect`);

                    nextQuestionAnswer.find('.answer-content').attr('id', `Questions_${newQuestionId}__Answers_${answerId}__Content`);
                    nextQuestionAnswer.find('.answer-content').attr('name', `Questions[${newQuestionId}].Answers[${answerId}].Content`);

                    nextQuestionAnswer.find('.answer-plaintext').attr('id', `Questions_${newQuestionId}__Answers_${answerId}__ContentPlaintext`);
                    nextQuestionAnswer.find('.answer-plaintext').attr('name', `Questions[${newQuestionId}].Answers[${answerId}].ContentPlaintext`);

                    nextQuestionAnswer.find('.answer-id').attr('id', `Questions_${newQuestionId}__Answers_${answerId}__Id`);
                    nextQuestionAnswer.find('.answer-id').attr('name', `Questions[${newQuestionId}].Answers[${answerId}].Id`);
                });
            });
        }
    });


    var addAnswerClickEvent = $('#questions-container #questions-body').on('click', '.add-answer', function () {
        var questionId = this.name.split('-')[1];
        var newAnswerNumber = $(`#question-${questionId} .answers-container .answer-container`).length;

        var answerHtml = answerFrame
            .replace(/\{\{\q_id\}\}/g, questionId)
            .replace(/\{\{\a_id\}\}/g, newAnswerNumber)
            .replace(/\{\{\a_number\}\}/g, newAnswerNumber + 1);

        var answer = $(answerHtml);
        answer.find(`#Questions_${questionId}__Answers_${newAnswerNumber}__Content`).summernote(answerSummernoteConfig);

        if (newAnswerNumber === 0) {
            $(`#question-${questionId} .answers-container`)
                .html(answer);
        }
        else {
            $(`#question-${questionId} .answers-container`)
                .append(answer);
        }

        radioButtonClick(questionId);
    });
    var deleteAnswerClickEvent = $('#questions-container #questions-body').on('click', '.delete-answer', function () {
        var params = $(this).closest('.answer-container')[0].id.split('-');
        var questionId = parseInt(params[1]);
        var answerNumber = parseInt(params[3]);

        $(this).closest('.answer-container').remove();

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
            nextAnswers.forEach(function (answer) {
                var newAnswerNumber = parseInt(answer.id.split('-')[3]) - 1;

                var nextAnswer = $(answer);
                nextAnswer.attr('id', `question-${questionId}-answer-${newAnswerNumber}`);
                nextAnswer.find('h3').text(`Answer ${newAnswerNumber + 1}`);

                nextAnswer.find('.answer-content').attr('id', `Questions_${questionId}__Answers_${newAnswerNumber}__Content`);
                nextAnswer.find('.answer-content').attr('name', `Questions[${questionId}].Answers[${newAnswerNumber}].Content`);

                nextAnswer.find('.answer-plaintext').attr('id', `Questions_${questionId}__Answers_${newAnswerNumber}__ContentPlaintext`);
                nextAnswer.find('.answer-plaintext').attr('name', `Questions[${questionId}].Answers[${newAnswerNumber}].ContentPlaintext`);

                nextAnswer.find('.answer-id').attr('id', `Questions_${questionId}__Answers_${newAnswerNumber}__Id`);
                nextAnswer.find('.answer-id').attr('name', `Questions[${questionId}].Answers[${newAnswerNumber}].Id`);
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

        $('#questions-container .summernote')
            .toArray()
            .forEach(function (formatedTextarea) {
                var plainText = $(formatedTextarea)
                    .summernote('code')
                    .replace(/<\/?[^>]+(>|$)/g, "");

                $(formatedTextarea)
                    .siblings('.plaintext').val(plainText);
            });
    });

    initializeSummernote();
});