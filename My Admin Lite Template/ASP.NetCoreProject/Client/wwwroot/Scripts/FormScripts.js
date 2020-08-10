var table = null;
var arrDepart = [];
var arrSuperv = [];
var arrEmp = [];

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


//function chartAM4() {
//    debugger;
//    var LineChartManager = {
//        GetChartData: function () {
//            var obj = '';
//            var jsonParam = '';
//            var URL = '/forms/LoadForm'
//            LineChartManager.GetJsonResult(url, jsonParam, false, false, onSuccess, onFailed);

//            function onSuccess(jsonData) {
//                obj = jsonData;
//            }
//            function onFailed(error) {
//                alert(error.statusText);
//            }
//            return obj;
//        },
//        GetJsonResult: function (url, jsonparam, isAsync, isCache, successCallback, errorCalback) {
//            $.ajax({
//                type: 'GET',
//                async: isAsync,
//                cache: isCache,
//                url: url,
//                data: jsonparam,
//                contentType: 'application/json; charset=utf-8',
//                dataType: 'json',
//                success: successCallback,
//                error: errorCalback
//            });
//        }
//    };

//    var LineChartHelper = {
//        LoadLineChart: function () {
//            var data = LineChartManager.GetChartData();
//            am4core.ready(function () {
//                var chart = am4core.create("chartdiv", am4charts.XYChart);
//                chart.data = data;
//                chart.dateFormatter.inputDateFormat = "yyyy-mm-dd";
//                var dateAxis = chart.xAxes.push(new am4charts.dateAxis());
//                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
//                var series = chart.series.push(new am4charts.LineSeries());
//                series.dataFields.valueY = "Duration";
//                series.dataFields.dateX = "EndDate";
//                series.tooltipText = '{Duration}';
//                series.strokeWidth = 2;
//                series.minBulletDistance = 15;

//                series.tooltip.background.cornerRadius = 20;
//                series.tooltip.background.strokeOpacity = 0;
//                series.tooltip.pointerOrientation = "vertical";
//                series.tooltip.label.midWidth = 20;
//                series.tooltip.label.minHeight = 20;
//                series.tooltip.label.textAlign = 'middle';
//                series.tooltip.label.textValign = 'middle';

//                var bullet = series.bullets.push(new am4charts.CircleBullet());
//                bullet.circle.strokeWidth = 2;
//                bullet.circle.radius = 4;
//                bullet.circle.fill = am4core.color('#fff');

//                var bullethover = bullet.states.create('hover');
//                bullethover.properties.scale = 1.3;

//                chart.cursor = new am4charts.XYCursor();
//                chart.cursor.behavior = 'panXY';
//                chart.cursor.xAxis = dateAxis;
//                chart.cursor.snapToSeries = series;

//                chart.scrollbarY = new am4core.Scrollbar();
//                chart.scrollbarY.parent = chart.leftAxesContainer;
//                chart.scrollbarY.toBack();

//                chart.scrollbarX = new am4charts.XYChartsScrollbar();
//                chart.scrollbarX.series.push(series);
//                chart.scrollbarX.parent = chart.bottomAxesContainer;

//                chart.events.on('ready', function () {
//                    dateAxis.zoom({ start: 0.79, end: 1 });
//                });
//            });
//            debugger;
//        }
//    };
//    debugger;
//}