var table = null;
var arrSuperv = [];
var arrForms = [];

$(document).ready(function () {
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })

    //debugger;
    table = $("#validation").DataTable({
        "processing": true,
        "responsive": true,
        "pagination": true,
        "stateSave": true,
        "ajax": {
            url: "/Validations/LoadValidation",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columns": [
            //{
            //    "data": "id",
            //    "sortable": true,
            //    render: function (data, type, row, meta) {
            //        return meta.row + meta.settings._iDisplayStart + 1;
            //    }
            //},
            { "data": "action" },
            { "data": "supervisorName" },
            { "data": "formName" },

            {
                "sortable": false,
                "render": function (data, type, row) {
                    //console.log(row);
                    $('[data-toggle="tooltip"]').tooltip();
                    return '<button class="btn btn-md btn-outline-warning btn-circle" data-toggle="tooltip" data-placement="left"  title="Edit" onclick="return GetById(' + row.id + ')" ><i class="fa fa-lg fa-edit"></i></button>'
                        + '&nbsp;'
                        + '<button class="btn btn-md btn-outline-danger btn-circle" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')" ><i class="fa fa-lg fa-trash"></i></button>'
                }
            }
        ]
    });

});

function ClearScreen() {
    $('#Id').val('');
    $('#Action').val('');
    $('#SupervisorOption').val('');
    $('#FormOption').val('');
    $('#Update').hide();
    $('#Save').show();
}

//Supervisor
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


//Forms
function LoadForm(element) {
    //debugger;
    if (arrForms.length === 0) {
        $.ajax({
            type: "Get",
            url: "/Forms/LoadForm",
            success: function (data) {
                arrForms = data;
                renderForm(element);
            }
        });
    }
    else {
        renderForm(element);
    }
}

function renderForm(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Employee').hide());
    $.each(arrForms, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.name))
    });
}

LoadForm($('#FormOption'))


function GetById(id) {
    $.ajax({
        url: "/validations/GetById/",
        data: { id: id }
    }).then((result) => {
        //debugger;
        $('#Id').val(result.id);
        $('#Action').val(result.action);
        $('#SupervisorOption').val(result.supervisorId);
        $('#FormOption').val(result.formId);
        $('#Save').hide();
        $('#Update').show();
        $('#mymodal').modal('show');
    })
}

function Save() {
    debugger;
    var Validation = new Object();
    Validation.action = $('#Action').val();
    Validation.supervisorId = $('#SupervisorOption').val();
    Validation.formId = $('#FormOption').val();
    $.ajax({
        type: 'POST',
        url: "/validations/InsertAndUpdate/",
        data: Validation
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
    var Validation = new Object();
    Validation.id = $('#Id').val();
    Validation.Action = $('#Action').val();
    Validation.supervisorId = $('#SupervisorOption').val();
    Validation.formId = $('#FormOption').val();
    $.ajax({
        type: 'POST',
        url: "/validations/InsertAndUpdate/",
        cache: false,
        dataType: "JSON",
        data: Validation
    }).then((result) => {
        //debugger;
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
                url: "/validations/Delete/",
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