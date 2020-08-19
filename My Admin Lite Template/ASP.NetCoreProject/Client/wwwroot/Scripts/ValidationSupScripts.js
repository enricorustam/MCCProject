var table = null;
var arrSuperv = [];
var arrForms = [];
var idtest = parseInt(document.getElementById("SesId").value);
var role = document.getElementById("SesRole").value;

$(document).ready(function () {
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    });

    debugger;
    if (role == "Employee") {
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
                { "data": "action" },
                { "data": "supervisorName" },
                { "data": "formId" }
            ]
        });
    }
    else {
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
                { "data": "action" },
                { "data": "supervisorName" },
                { "data": "formId" },
                {
                    "sortable": false,
                    "render": function (data, type, row) {
                        //console.log(row);
                        if (row.supervisorId == idtest) {
                            return '<button class="btn btn-info fa fa-pencil-square-o" data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetById(' + row.id + ')" ></button>'
                        } else {
                            return ' '
                        }
                    }
                }
            ]
        });
    }    
});

function ClearScreen() {
    $('#Id').val('');
    $('#Action').val('');
    //$('#SupervisorOption').val('');
    $('#FormOption').val('');
    $('#Update').hide();
    $('#Save').show();
}

//============= Get data Form for Dropdown =================
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
    $option.append($('<option/>').val('0').text('Select Form Id').hide());
    $.each(arrForms, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.id))
    });
}

LoadForm($('#FormOption'));
//============= Get data Form for Dropdown =================


function GetById(id) {
    $.ajax({
        url: "/validations/GetById/",
        data: { id: id }
    }).then((result) => {
        debugger;
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
    debugger;
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

if (role == "Supervisor") {
    am4core.useTheme(am4themes_animated);

    am4core.createFromConfig({
        // Set inner radius
        "innerRadius": "50%",

        // Set data
        "dataSource": {
            "url": "/Validations/LoadPieChart",
            "parser": {
                "type": "JSONParser",
            },
            "reloadFrequency": 5000,
        },

        // Create series
        "series": [{
            "type": "PieSeries",
            "dataFields": {
                "value": "total",
                "category": "action",
            },
            "slices": {
                "cornerRadius": 10,
                "innerCornerRadius": 7
            },
            "hiddenState": {
                "properties": {
                    // this creates initial animation
                    "opacity": 1,
                    "endAngle": -90,
                    "startAngle": -90
                }
            },
            "children": [{
                "type": "Label",
                "forceCreate": true,
                "text": "Status",
                "horizontalCenter": "middle",
                "verticalCenter": "middle",
                "fontSize": 40
            }]
        }],

        // Add legend
        "legend": {},
    }, "pieChart", am4charts.PieChart);

}