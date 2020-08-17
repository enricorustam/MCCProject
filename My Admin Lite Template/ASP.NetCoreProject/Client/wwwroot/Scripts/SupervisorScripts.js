var table = null;

$(document).ready(function () {
    //debugger;
    table = $("#supervisor").DataTable({
        "processing": true,
        "ajax": {
            url: "/Supervisors/LoadSupervisor",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columns": [
            { "data": "name" },
            { "data": "password" },
            {
                "render": function (data, type, row) {
                    //console.log(row);
                    return '<button class="btn btn-info fa fa-pencil-square-o" data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetById(' + row.id + ')" ></button>'
                        + '&nbsp;'
                        + '<button class="btn btn-danger fa fa-trash-o" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')" ></button>'
                }
            }
        ],
        "columnDefs": [{
            "targets": [2],
            "orderable": false
        }]
    });
});

function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    $('#Password').val('');
    $('#Update').hide();
    $('#Save').show();
}

function GetById(id) {
    $.ajax({
        url: "/Supervisors/GetById/",
        data: { id: id }
    }).then((result) => {
        debugger;
        $('#Id').val(result.id);
        $('#Name').val(result.name);
        $('#Password').val(result.password);
        $('#Save').hide();
        $('#Update').show();
        $('#mymodal').modal('show');
    })
}

function Save() {
    debugger;
    var Supervisor = new Object();
    Supervisor.name = $('#Name').val();
    Supervisor.password = $('#Password').val();
    $.ajax({
        type: 'POST',
        url: "/Supervisors/Insert/",
        data: Supervisor
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Supervisor inserted Successfully'
            });
            table.ajax.reload();
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}

function Update() {
    var Supervisor = new Object();
    Supervisor.id = $('#Id').val();
    Supervisor.name = $('#Name').val();
    Supervisor.password = $('#Password').val();
    $.ajax({
        type: 'POST',
        url: "/Supervisors/Update/",
        data: Supervisor
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Supervisor Updated Successfully'
            });
            table.ajax.reload();
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}

function Delete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            //debugger;
            $.ajax({
                url: "/Supervisors/Delete/",
                data: { id: id }
            }).then((result) => {
                debugger;
                if (result.statusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully'
                    });
                    table.ajax.reload();
                } else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            })
        };
    });
}