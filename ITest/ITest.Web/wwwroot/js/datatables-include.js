$(function () {
    $('.dashboard-table').DataTable({
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