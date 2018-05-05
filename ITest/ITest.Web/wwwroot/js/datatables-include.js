$(function () {
    $('.created-tests-table').DataTable({
        "language": {
            "emptyTable": "No Test available. Click 'New Test' to create new Test"
        },
        "order": [[3, "desc"]],
        searching: false,
        "lengthChange": false,
        "info": false,
        "pagingType": "full_numbers",
        "columnDefs": [{
            "targets": 3,
            "orderable": false
        }]
    });

    $('.attended-tests').DataTable({
        "language": {
            "emptyTable": "No one has taken any tests yet"
        },
        "order": [[3, "desc"]],
        searching: false,
        "lengthChange": false,
        "info": false,
        "pagingType": "full_numbers",
        "columnDefs": [{
            "targets": 3,
            "orderable": false
        }]
    });
});