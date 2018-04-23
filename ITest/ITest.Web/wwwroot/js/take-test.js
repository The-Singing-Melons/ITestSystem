
$(function () {
    $('#test-submit-btn').on('click', function () {
        $('.radio-btn-to-change')
            .toArray()
            .forEach(function (rButton) {
                var inputId = $(rButton).closest('.funkyradio').find('> input')[0].id;

                var separators = ['__', '_'];
                var params = inputId.split(new RegExp(separators.join('|'), 'g'));

                var questionId = params[1];
                var answerNumber = params[3];

                $(rButton).attr('id', `Questions_${questionId}__Answers_${answerNumber}_SelectedAnswerId`)
                $(rButton).attr('name', `Questions[${questionId}].Answers[${answerNumber}].SelectedAnswerId`)
            })
    })
})