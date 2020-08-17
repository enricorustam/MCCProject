am4core.useTheme(am4themes_animated);

//Creating Pie Chart =====================
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


// =================================================================
// Membuat Line chart (id/classchart, jenis chart)
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