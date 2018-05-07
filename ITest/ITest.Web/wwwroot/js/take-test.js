
$(function () {
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
                $('#modal-submit-btn').click();
            }
        }
        );
})