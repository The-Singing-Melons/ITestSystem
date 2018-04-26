
$(function () {
    //    $('#test-submit-btn').on('click', function () {
    //        $('.radio-btn-to-change')
    //            .toArray()
    //            .forEach(function (rButton) {
    //                var inputId = $(rButton).closest('.funkyradio').find('> input')[0].id;

    //                var separators = ['__', '_'];
    //                var params = inputId.split(new RegExp(separators.join('|'), 'g'));

    //                var questionId = params[1];
    //                var answerNumber = params[3];

    //                $(rButton).attr('id', `Questions_${questionId}__Answers_${answerNumber}_SelectedAnswerId`)
    //                $(rButton).attr('name', `Questions[${questionId}].Answers[${answerNumber}].SelectedAnswerId`)
    //            })
    //    })

    //window.onbeforeunload = function () {
    //    return ""
    //}

    //if (window.performance) {
    //    console.info("window.performance works fine on this browser");
    //}
    //if (performance.navigation.type == 1) {
    //    alert("You refreshed the page and your test was automatically submited");
    //    $('#test-submit-btn').click();

    //} else {
    //    console.info("This page is not reloaded");
    //}

    $(".timer").TimeCircles({
        time: {
            Days: { color: "#FF7F50", show: false },
            Hours: { color: "#FF7F50" },
            Minutes: { color: "#FF7F50" },
            Seconds: { color: "#FF7F50" }
        }
    })
        .addListener(
        function (unit, value, total) {
            if (total === 60) {
                alert("1 minute left. When the time is up the test will be automatically submited!")
            }
            if (total === 0) {
                $('#test-submit-btn').click();
            }
        }
        );
})