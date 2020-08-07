var table = null;
var arrDepart = [];
var arrSuperv = [];
var arrEmp = [];

function convertToDate(data) {
    // The 6th+ positions contain the number of milliseconds in Universal Coordinated Time between the specified date and midnight January 1, 1970.
    var dtStart = new Date(parseInt(data.substr(6)));
    // Format using moment.js.
    var dtStartWrapper = moment(dtStart);
    return dtStartWrapper.format("MM/DD/YYYY HH:MM");
};

$(document).ready(function () {
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })

    debugger;
    table = $("#form").DataTable({
        "processing": true,
        "responsive": true,
        "pagination": true,
        "stateSave": true,
        "ajax": {
            url: "/Forms/LoadForm",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columns": [
            //{
            //    "data": "Id",
            //    "sortable": true,
            //    render: function (data, type, row, meta) {
            //        return meta.row;
            //    }
            //},
            { "data": "employeeName" },
            {
                "data": "startDate",
                //"render": function (data) { return convertToDate(data); }
            },
            {
                "data": "endDate",
                //"render": function (data) { return convertToDate(data); }
            },
            { "data": "duration" },
            { "data": "supervisorName" },
            { "data": "departmentName" },
            {
                "sortable": false,
                "render": function (data, type, row) {
                    //console.log(row);
                    return '<button class="btn btn-info fa fa-pencil-square-o" data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetById(' + row.id + ')" ></button>'
                        + '&nbsp;'
                        + '<button class="btn btn-danger fa fa-trash-o" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')" ></button>'
                }
            }
        ]
        //,
        //'buttons': ['excel', 'pdf']
    });
});

function ClearScreen() {
    $('#Id').val('');
    $('#EmployeeOption').val('');
    $('#StartDate').val('');
    $('#EndDate').val('');
    $('#Duration').val('');
    $('#SupervisorOption').val('');
    $('#DepartmentOption').val('');
    $('#Update').hide();
    $('#Save').show();
}

//============= Get data Department for Dropdown =================
function LoadDepartment(element) {
    //debugger;
    if (arrDepart.length === 0) {
        $.ajax({
            type: "Get",
            url: "/Departments/LoadDepartment",
            success: function (data) {
                arrDepart = data;
                renderDepartment(element);
            }
        });
    }
    else {
        renderDepartment(element);
    }
}

function renderDepartment(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Department').hide());
    $.each(arrDepart, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.name))
    });
}

LoadDepartment($('#DepartmentOption'))
//============= Get data Department for Dropdown =================

//============= Get data Supervisor for Dropdown =================
function LoadSupervisor(element) {
    //debugger;
    if (arrSuperv.length === 0) {
        $.ajax({
            type: "Get",
            url: "/Supervisors/LoadSupervisor",
            success: function (data) {
                arrSuperv = data;
                renderSupervisor(element);
            }
        });
    }
    else {
        renderSupervisor(element);
    }
}

function renderSupervisor(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Supervisor').hide());
    $.each(arrSuperv, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.name))
    });
}

LoadSupervisor($('#SupervisorOption'))
//============= Get data Supervisor for Dropdown =================

//============= Get data Employee for Dropdown =================
function LoadEmployee(element) {
    debugger;
    if (arrEmp.length === 0) {
        $.ajax({
            type: "Get",
            url: "/Employees/LoadEmployee",
            success: function (data) {
                arrEmp = data;
                renderEmployee(element);
            }
        });
    }
    else {
        renderEmployee(element);
    }
}

function renderEmployee(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Employee').hide());
    $.each(arrEmp, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.name))
    });
}

LoadEmployee($('#EmployeeOption'))
//============= Get data Employee for Dropdown =================

function GetById(id) {
    $.ajax({
        url: "/forms/GetById/",
        data: { id: id }
    }).then((result) => {
        debugger;
        $('#Id').val(result.id);
        $('#EmployeeOption').val(result.employeeId);
        $('#StartDate').val(result.startDate);
        $('#EndDate').val(result.endDate);
        $('#Duration').val(result.duration);
        $('#SupervisorOption').val(result.supervisorId);
        $('#DepartmentOption').val(result.departmentId);
        $('#Save').hide();
        $('#Update').show();
        $('#mymodal').modal('show');
    })
}

function Save() {
    debugger;
    var Form = new Object();
    Form.employeeId = $('#EmployeeOption').val();
    Form.StartDate = $('#StartDate').val();
    Form.EndDate = $('#EndDate').val();
    Form.Duration = $('#Duration').val();
    Form.supervisorId = $('#SupervisorOption').val();
    Form.departmentId = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: "/forms/InsertAndUpdate/",
        data: Form
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'inserted Successfully',
                showConfirmButton: false,
                timer: 1500,
            })
            table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}

function Update() {
    debugger;
    var Form = new Object();
    Form.id = $('#Id').val();
    Form.employeeId = $('#EmployeeOption').val();
    Form.StartDate = $('#StartDate').val();
    Form.EndDate = $('#EndDate').val();
    Form.Duration = $('#Duration').val();
    Form.supervisorId = $('#SupervisorOption').val();
    Form.departmentId = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: "/forms/InsertAndUpdate/",
        cache: false,
        dataType: "JSON",
        data: Form
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Updated Successfully',
                showConfirmButton: false,
                timer: 1500,
            });
            table.ajax.reload(null, false);
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
        confirmButtonText: 'Yes, delete it!',
    }).then((result) => {
        if (result.value) {
            //debugger;
            $.ajax({
                url: "/forms/Delete/",
                data: { id: id }
            }).then((result) => {
                //debugger;
                if (result.statusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Delete Successfully',
                        showConfirmButton: false,
                        timer: 1500,
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