var table = null;

$(document).ready(function () {
    //debugger;
    table = $("#myTable").DataTable({
        "processing": true,
        "ajax": {
            url: "/Employees/LoadEmployee",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columns": [
            { "data": "name" },
            { "data": "nip" },
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
    $('#Nip').val('');
    $('#Update').hide();
    $('#Save').show();
}

function GetById(id) {
    $.ajax({
        url: "/Employees/GetById/",
        data: { id: id }
    }).then((result) => {
        debugger;
        $('#Id').val(result.id);
        $('#Name').val(result.name);
        $('#Nip').val(result.nip);
        $('#Save').hide();
        $('#Update').show();
        $('#mymodal').modal('show');
    })
}

function Save() {
    debugger;
    var Employee = new Object();
    Employee.name = $('#Name').val();
    Employee.nip = $('#Nip').val();
    $.ajax({
        type: 'POST',
        url: "/Employees/Insert/",
        data: Employee
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Employee inserted Successfully'
            });
            table.ajax.reload();
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}

function Update() {
    var Employee = new Object();
    Employee.id = $('#Id').val();
    Employee.name = $('#Name').val();
    Employee.nip = $('#Nip').val();
    $.ajax({
        type: 'POST',
        url: "/Employees/Update/",
        data: Employee
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Employee Updated Successfully'
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
                url: "/Employees/Delete/",
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