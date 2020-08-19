var table = null;
var arrDepart = [];
var arrSuperv = [];
var arrEmp = [];
var idtest = parseInt(document.getElementById("SesId").value);
var role = document.getElementById("SesRole").value;

if (role == "Supervisor") {
    am4core.useTheme(am4themes_animated);

    // =================================================================
    // Membuat chart (id/classchart, jenis chart)
    var chart = am4core.create("linechart", am4charts.XYChart);

    // Increase contrast by taking evey second color
    chart.colors.step = 2;

    // Add data
    // Mengambil data dari url (database)
    chart.dataSource.url = "/forms/LoadForm";
    chart.dateFormatter.inputDateFormat = "yyyy-MM-dd";

    // Create axes
    var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
    dateAxis.renderer.minGridDistance = 50;

    // Setinng chart (field y, notasi di data (x,y), .., notasi data berbentuk apa)
    function createAxisAndSeries(field, name, opposite, bullet) {
        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        //if (chart.yAxes.indexOf(valueAxis) != 0) {
        //    valueAxis.syncWithAxis = chart.yAxes.getIndex(0);
        //    //console.log(chart.yAxes)
        //}

        var series = chart.series.push(new am4charts.LineSeries());
        series.dataFields.valueY = field;
        series.dataFields.dateX = "endDate";
        series.strokeWidth = 2;
        series.yAxis = valueAxis;
        series.name = name;
        series.tooltipText = "{name}: [bold]{valueY}[/]";
        series.tensionX = 0.8;
        series.showOnInit = true;

        var interfaceColors = new am4core.InterfaceColorSet();

        switch (bullet) {
            case "triangle":
                var bullet = series.bullets.push(new am4charts.Bullet());
                bullet.width = 12;
                bullet.height = 12;
                bullet.horizontalCenter = "middle";
                bullet.verticalCenter = "middle";

                var triangle = bullet.createChild(am4core.Triangle);
                triangle.stroke = interfaceColors.getFor("background");
                triangle.strokeWidth = 2;
                triangle.direction = "top";
                triangle.width = 12;
                triangle.height = 12;
                break;
            case "rectangle":
                var bullet = series.bullets.push(new am4charts.Bullet());
                bullet.width = 10;
                bullet.height = 10;
                bullet.horizontalCenter = "middle";
                bullet.verticalCenter = "middle";

                var rectangle = bullet.createChild(am4core.Rectangle);
                rectangle.stroke = interfaceColors.getFor("background");
                rectangle.strokeWidth = 2;
                rectangle.width = 10;
                rectangle.height = 10;
                break;
            default:
                var bullet = series.bullets.push(new am4charts.CircleBullet());
                bullet.circle.stroke = interfaceColors.getFor("background");
                bullet.circle.strokeWidth = 2;
                break;
        }

        valueAxis.renderer.line.strokeOpacity = 1;
        valueAxis.renderer.line.strokeWidth = 2;
        valueAxis.renderer.line.stroke = series.stroke;
        valueAxis.renderer.labels.template.fill = series.stroke;
        valueAxis.renderer.opposite = opposite;

        // Create vertical scrollbar and place it before the value axis
        chart.scrollbarY = new am4core.Scrollbar();
        chart.scrollbarY.parent = chart.leftAxesContainer;
        chart.scrollbarY.toBack();
        // Create a horizontal scrollbar with previe and place it underneath the date axis
        chart.scrollbarX = new am4charts.XYChartScrollbar();
        chart.scrollbarX.series.push(series);
        chart.scrollbarX.parent = chart.bottomAxesContainer;

        //dateAxis.start = 0.79;
        dateAxis.keepSelection = true;
    }

    // Add legend
    chart.legend = new am4charts.Legend();

    createAxisAndSeries("duration", "Overtime Duration", false, "circle");

    // Add cursor agar data bisa dilihat per titik
    chart.cursor = new am4charts.XYCursor();
}

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
    if (role == "Employee") {
        table = $("#form").DataTable({
            "processing": true,
            "responsive": true,
            "pagination": true,
            "stateSave": true,
            "ajax": {
                url: "/Forms/LoadForm/",
                type: "GET",
                //data: { id: idtest},
                dataType: "json",
                dataSrc: "",
            },
            "columns": [
                { "data": "id" },
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
                        if (row.employeeId == idtest) {
                            return '<button class="btn btn-info fa fa-pencil-square-o" data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetById(' + row.id + ')" ></button>'
                        } else {
                            return ' '
                        }
                    }
                }
            ]
            //,
            //'buttons': ['excel', 'pdf']
        });
    }
    else {
        table = $("#form").DataTable({
            "processing": true,
            "responsive": true,
            "pagination": true,
            "stateSave": true,
            "ajax": {
                url: "/Forms/LoadForm/",
                type: "GET",
                //data: { id: idtest},
                dataType: "json",
                dataSrc: "",
            },
            "columns": [
                { "data": "id" },
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
                { "data": "departmentName" }
            ]
            //,
            //'buttons': ['excel', 'pdf']
        });
    }
});

function ClearScreen() {
    $('#Id').val('');
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
