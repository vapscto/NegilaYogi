////////////////"column

(function () {
    'use strict';
    angular
        .module('app')
        .controller('SourcegraphReportController', SourcegraphReportController)

    SourcegraphReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function SourcegraphReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {
        //$("#chart123").kendoChart({
        //    title: {
        //        text: "Gross domestic product growth /GDP annual %/"
        //    },
        //    legend: {
        //        position: "top"
        //    },
        //    seriesDefaults: {
        //        type: "column"
        //    },
        //    series: [{
        //        name: "India",
        //        data: [3.907, 7.943, 7.848, 9.284, 9.263, 9.801, 3.890, 8.238, 9.552, 6.855]
        //    }, {
        //        name: "Russian Federation",
        //        data: [4.743, 7.295, 7.175, 6.376, 8.153, 8.535, 5.247, -7.832, 4.3, 4.3]
        //    }, {
        //        name: "Germany",
        //        data: [0.010, -0.375, 1.161, 0.684, 3.7, 3.269, 1.083, -5.127, 3.690, 2.995]
        //    }, {
        //        name: "World",
        //        data: [1.988, 2.733, 3.994, 3.464, 4.001, 3.939, 1.333, -2.245, 4.339, 2.727]
        //    }],
        //    valueAxis: {
        //        labels: {
        //            format: "{0}%"
        //        },
        //        line: {
        //            visible: false
        //        },
        //        axisCrossingValue: 0
        //    },
        //    categoryAxis: {
        //        categories: [2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011],
        //        line: {
        //            visible: false
        //        },
        //        labels: {
        //            padding: { top: 135 }
        //        }
        //    },
        //    tooltip: {
        //        visible: true,
        //        format: "{0}%",
        //        template: "#= series.name #: #= value #"
        //    }
        //});


        //////////////////////////////bar
        //var drawing = kendo.drawing;
        //var geometry = kendo.geometry;
        //function createChart() {
        //    $("#chart").kendoChart({
        //        title: {
        //            text: "Site Visitors Stats /thousands/"
        //        },
        //        legend: {
        //            position: "bottom",
        //            item: {
        //                visual: createLegendItem
        //            }
        //        },
        //        seriesDefaults: {
        //            type: "column",
        //            stack: true,
        //            highlight: {
        //                toggle: function (e) {
        //                    // Don't create a highlight overlay,
        //                    // we'll modify the existing visual instead
        //                    e.preventDefault();

        //                    var visual = e.visual;
        //                    var opacity = e.show ? 0.8 : 1;

        //                    visual.opacity(opacity);
        //                }
        //            },
        //            visual: function (e) {
        //                return createColumn(e.rect, e.options.color);
        //            }
        //        },
        //        series: [{
        //            name: "Total Visits",
        //            data: [56000, 63000, 74000, 91000, 117000, 138000, 128000, 115000, 102000, 98000, 123000, 113000]
        //        }, {
        //            name: "Unique visitors",
        //            data: [52000, 34000, 23000, 48000, 67000, 83000, 40000, 50000, 64000, 72000, 75000, 81000]
        //        }],
        //        panes: [{
        //            clip: false
        //        }],
        //        valueAxis: {
        //            line: {
        //                visible: false
        //            }
        //        },
        //        categoryAxis: {
        //            categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
        //            majorGridLines: {
        //                visible: false
        //            },
        //            line: {
        //                visible: false
        //            }
        //        },
        //        tooltip: {
        //            visible: true
        //        }
        //    });
        //}

        //function createColumn(rect, color) {
        //    var origin = rect.origin;
        //    var center = rect.center();
        //    var bottomRight = rect.bottomRight();
        //    var radiusX = rect.width() / 2;
        //    var radiusY = radiusX / 3;
        //    var gradient = new drawing.LinearGradient({
        //        stops: [{
        //            offset: 0,
        //            color: color
        //        }, {
        //            offset: 0.5,
        //            color: color,
        //            opacity: 0.9
        //        }, {
        //            offset: 0.5,
        //            color: color,
        //            opacity: 0.9
        //        }, {
        //            offset: 1,
        //            color: color
        //        }]
        //    });

        //    var path = new drawing.Path({
        //        fill: gradient,
        //        stroke: {
        //            color: "none"
        //        }
        //    }).moveTo(origin.x, origin.y)
        //        .lineTo(origin.x, bottomRight.y)
        //        .arc(180, 0, radiusX, radiusY, true)
        //        .lineTo(bottomRight.x, origin.y)
        //        .arc(0, 180, radiusX, radiusY);

        //    var topArcGeometry = new geometry.Arc([center.x, origin.y], {
        //        startAngle: 0,
        //        endAngle: 360,
        //        radiusX: radiusX,
        //        radiusY: radiusY
        //    });

        //    var topArc = new drawing.Arc(topArcGeometry, {
        //        fill: {
        //            color: color
        //        },
        //        stroke: {
        //            color: "#ebebeb"
        //        }
        //    });
        //    var group = new drawing.Group();
        //    group.append(path, topArc);
        //    return group;
        //}

        //function createLegendItem(e) {
        //    var color = e.options.markers.background;
        //    var labelColor = e.options.labels.color;
        //    var rect = new geometry.Rect([0, 0], [120, 50]);
        //    var layout = new drawing.Layout(rect, {
        //        spacing: 5,
        //        alignItems: "center"
        //    });

        //    var overlay = drawing.Path.fromRect(rect, {
        //        fill: {
        //            color: "#fff",
        //            opacity: 0
        //        },
        //        stroke: {
        //            color: "none"
        //        },
        //        cursor: "pointer"
        //    });

        //    var column = createColumn(new geometry.Rect([0, 0], [15, 10]), color);
        //    var label = new drawing.Text(e.series.name, [0, 0], {
        //        fill: {
        //            color: labelColor
        //        }
        //    })

        //    layout.append(column, label);
        //    layout.reflow();

        //    var group = new drawing.Group().append(layout, overlay);

        //    return group;
        //}

        //$(document).ready(createChart);
        //$(document).bind("kendo:skinChange", createChart);



        //////////////////////////////////////////////////// line


        //function createChart1() {
        //    $("#chart1").kendoChart({
        //        title: {
        //            text: "SourceWise Student Count"
        //        },
        //        legend: {
        //            position: "bottom"
        //        },
        //        chartArea: {
        //            background: ""
        //        },
        //        seriesDefaults: {
        //            type: "line",
        //            style: "smooth"
        //        },
        //        series: [{
        //            name: "India",
        //            data: [3.907, 7.943, 7.848, 9.284, 9.263, 9.801, 3.890, 8.238, 9.552, 6.855]
        //        }, {
        //            name: "World",
        //            data: [1.988, 2.733, 3.994, 3.464, 4.001, 3.939, 1.333, -2.245, 4.339, 2.727]
        //        }, {
        //            name: "Russian Federation",
        //            data: [4.743, 7.295, 7.175, 6.376, 8.153, 8.535, 5.247, -7.832, 4.3, 4.3]
        //        }, {
        //            name: "Haiti",
        //            data: [-0.253, 0.362, -3.519, 1.799, 2.252, 3.343, 0.843, 2.877, -5.416, 5.590]
        //        }],
        //        valueAxis: {
        //            labels: {
        //                format: "{0}%"
        //            },
        //            line: {
        //                visible: false
        //            },
        //            axisCrossingValue: -10
        //        },
        //        categoryAxis: {
        //            categories: [2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011],
        //            majorGridLines: {
        //                visible: false
        //            },
        //            labels: {
        //                rotation: "auto"
        //            }
        //        },
        //        tooltip: {
        //            visible: true,
        //            format: "{0}%",
        //            template: "#= series.name #: #= value #"
        //        }
        //    });
        //}

        //$(document).ready(createChart1);
        //$(document).bind("kendo:skinChange", createChart1);



        //////////////////////////////////// pie
        function createChart2() {
            $("#chart2").kendoChart({
                title: {
                    position: "top",
                    text: "Source Wise Student Count"
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    labels: {
                        visible: true,
                        background: "transparent",
                        template: "#= category #: \n #= value#%"
                    }
                },
                series: [{
                    type: "pie",
                    startAngle: 150,
                    data: [{
                        category: "PREKG",
                        value: 25,
                        color: "#afd977"
                    }, {
                        category: "UKG",
                        value: 29,
                            color: "#6f9d33"
                    }, {
                        category: "LKG",
                        value: 20,
                            color: "#57c7c2"
                    }, {
                        category: "I Std",
                        value:14,
                            color: "#457406"
                    }, {
                        category: "II Std",
                        value: 12,
                            color: "#f3f562"
                        },
                    ]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}%"
                }
            });
        }

        $(document).ready(createChart2);
        $(document).bind("kendo:skinChange", createChart2);


        //////////////////////////////////////area

        $("#area").kendoChart({
            title: {
                text: "Source Wise Student Count"
            },
            legend: {
                position: "bottom"
            },
            seriesDefaults: {
                type: "area",
                area: {
                    line: {
                        style: "smooth"
                    }
                }
            },
            series: [{
                name: "Friends",
                data: [90, 85, 13, 50, 56],
                color:"#78b39d"
            }, {
                name: "Social Media",
                    data: [55, 33, 38,80, 60],
                    color: "sky blue"
            }, {
                name: "News Paper",
                    data: [44, 67, 90, 42,73],
                    color: "blue"
            }],
            valueAxis: {
                labels: {
                    format: "{0}"
                },
                line: {
                    visible: false
                },
                axisCrossingValue: -10
            },
            categoryAxis: {
                categories: ["PREKG","LKG","UKG","I Std","II Std"],
                majorGridLines: {
                    visible: false
                },
                labels: {
                    rotation: "auto"
                }
            },
            tooltip: {
                visible: true,
                format: "{0}%",
                template: "#= series.name #: #= value #"
            }
        });


        $(document).ready(createChart);
        $(document).bind("kendo:skinChange", createChart);



        /////////////////////////////////////////////
        //$("#Bubble").kendoChart({
        //    title: {
        //        text: "Job Growth for 2011"
        //    },
        //    legend: {
        //        visible: false
        //    },
        //    seriesDefaults: {
        //        type: "bubble"
        //    },
        //    series: [{
        //        data: [{
        //            x: -2500,
        //            y: 50000,
        //            size: 500000,
        //            category: "Microsoft"
        //        }, {
        //            x: 500,
        //            y: 110000,
        //            size: 7600000,
        //            category: "Starbucks"
        //        }, {
        //            x: 7000,
        //            y: 19000,
        //            size: 700000,
        //            category: "Google"
        //        }, {
        //            x: 1400,
        //            y: 150000,
        //            size: 700000,
        //            category: "Publix Super Markets"
        //        }, {
        //            x: 2400,
        //            y: 30000,
        //            size: 300000,
        //            category: "PricewaterhouseCoopers"
        //        }, {
        //            x: 2450,
        //            y: 34000,
        //            size: 90000,
        //            category: "Cisco"
        //        }, {
        //            x: 2700,
        //            y: 34000,
        //            size: 400000,
        //            category: "Accenture"
        //        }, {
        //            x: 2900,
        //            y: 40000,
        //            size: 450000,
        //            category: "Deloitte"
        //        }, {
        //            x: 3000,
        //            y: 55000,
        //            size: 900000,
        //            category: "Whole Foods Market"
        //        }]
        //    }],
        //    xAxis: {
        //        labels: {
        //            format: "{0:N0}",
        //            skip: 1,
        //            rotation: "auto"
        //        },
        //        axisCrossingValue: -5000,
        //        majorUnit: 2000,
        //        plotBands: [{
        //            from: -5000,
        //            to: 0,
        //            color: "#00f",
        //            opacity: 0.05
        //        }]
        //    },
        //    yAxis: {
        //        labels: {
        //            format: "{0:N0}"
        //        },
        //        line: {
        //            width: 0
        //        }
        //    },
        //    tooltip: {
        //        visible: true,
        //        format: "{3}: {2:N0} applications",
        //        opacity: 1
        //    }
        //});



        //$(document).ready(createChart);
        //$(document).bind("kendo:skinChange", createChart);



        ///////////////////////////////////Scatter 



        //$("#Scatter").kendoChart({
        //    title: {
        //        text: "Rainfall - Wind Speed"
        //    },
        //    legend: {
        //        position: "bottom"
        //    },
        //    seriesDefaults: {
        //        type: "scatter"
        //    },
        //    series: [{
        //        name: "January 2008",
        //        data: [
        //            [16.4, 5.4], [21.7, 2], [25.4, 3], [19, 2], [10.9, 1], [13.6, 3.2], [10.9, 7.4], [10.9, 0], [10.9, 8.2], [16.4, 0], [16.4, 1.8], [13.6, 0.3], [13.6, 0], [29.9, 0], [27.1, 2.3], [16.4, 0], [13.6, 3.7], [10.9, 5.2], [16.4, 6.5], [10.9, 0], [24.5, 7.1], [10.9, 0], [8.1, 4.7], [19, 0], [21.7, 1.8], [27.1, 0], [24.5, 0], [27.1, 0], [29.9, 1.5], [27.1, 0.8], [22.1, 2]]
        //    }, {
        //        name: "January 2009",
        //        data: [
        //            [6.4, 13.4], [1.7, 11], [5.4, 8], [9, 17], [1.9, 4], [3.6, 12.2], [1.9, 14.4], [1.9, 9], [1.9, 13.2], [1.4, 7], [6.4, 8.8], [3.6, 4.3], [1.6, 10], [9.9, 2], [7.1, 15], [1.4, 0], [3.6, 13.7], [1.9, 15.2], [6.4, 16.5], [0.9, 10], [4.5, 17.1], [10.9, 10], [0.1, 14.7], [9, 10], [2.7, 11.8], [2.1, 10], [2.5, 10], [27.1, 10], [2.9, 11.5], [7.1, 10.8], [2.1, 12]]
        //    }, {
        //        name: "January 2010",
        //        data: [
        //            [21.7, 3], [13.6, 3.5], [13.6, 3], [29.9, 3], [21.7, 20], [19, 2], [10.9, 3], [28, 4], [27.1, 0.3], [16.4, 4], [13.6, 0], [19, 5], [16.4, 3], [24.5, 3], [32.6, 3], [27.1, 4], [13.6, 6], [13.6, 8], [13.6, 5], [10.9, 4], [16.4, 0], [32.6, 10.3], [21.7, 20.8], [24.5, 0.8], [16.4, 0], [21.7, 6.9], [13.6, 7.7], [16.4, 0], [8.1, 0], [16.4, 0], [16.4, 0]]
        //    }],
        //    xAxis: {
        //        max: 35,
        //        title: {
        //            text: "Wind Speed [km/h]"
        //        },
        //        crosshair: {
        //            visible: true,
        //            tooltip: {
        //                visible: true,
        //                format: "n1"
        //            }
        //        }
        //    },
        //    yAxis: {
        //        min: -5,
        //        max: 25,
        //        title: {
        //            text: "Rainfall [mm]"
        //        },
        //        axisCrossingValue: -5,
        //        crosshair: {
        //            visible: true,
        //            tooltip: {
        //                visible: true,
        //                format: "n1"
        //            }
        //        }
        //    }
        //});


        //$(document).ready(createChart);
        //$(document).bind("kendo:skinChange", createChart);



        //////////////////////////////////////Bullet
        //$("#chart-mmHg").kendoChart({
        //    legend: {
        //        visible: false
        //    },
        //    series: [{
        //        type: "bullet",
        //        data: [[750, 762.5]]
        //    }],
        //    chartArea: {
        //        margin: {
        //            left: 0
        //        }
        //    },
        //    categoryAxis: {
        //        majorGridLines: {
        //            visible: false
        //        },
        //        majorTicks: {
        //            visible: false
        //        }
        //    },
        //    valueAxis: [{
        //        plotBands: [{
        //            from: 715, to: 752, color: "#ccc", opacity: .6
        //        }, {
        //            from: 752, to: 772, color: "#ccc", opacity: .3
        //        }],
        //        majorGridLines: {
        //            visible: false
        //        },
        //        min: 715,
        //        max: 795,
        //        minorTicks: {
        //            visible: true
        //        }
        //    }],
        //    tooltip: {
        //        visible: true,
        //        template: "Maximum: #= value.target # <br /> Average: #= value.current #"
        //    }
        //});

        //$("#chart-hPa").kendoChart({
        //    legend: {
        //        visible: false
        //    },
        //    series: [{
        //        type: "bullet",
        //        data: [[1001, 1017]]
        //    }],
        //    chartArea: {
        //        margin: {
        //            left: 0
        //        }
        //    },
        //    categoryAxis: {
        //        majorGridLines: {
        //            visible: false
        //        },
        //        majorTicks: {
        //            visible: false
        //        }
        //    },
        //    valueAxis: [{
        //        plotBands: [{
        //            from: 955, to: 1002, color: "#ccc", opacity: .6
        //        }, {
        //            from: 1002, to: 1027, color: "#ccc", opacity: .3
        //        }],
        //        majorGridLines: {
        //            visible: false
        //        },
        //        min: 955,
        //        max: 1055,
        //        minorTicks: {
        //            visible: true
        //        }
        //    }],
        //    tooltip: {
        //        visible: true,
        //        template: "Maximum: #= value.target # <br /> Average: #= value.current #"
        //    }
        //});

        //$("#chart-hum").kendoChart({
        //    legend: {
        //        visible: false
        //    },
        //    series: [{
        //        type: "bullet",
        //        data: [[45, 60]]
        //    }],
        //    categoryAxis: {
        //        majorGridLines: {
        //            visible: false
        //        },
        //        majorTicks: {
        //            visible: false
        //        }
        //    },
        //    valueAxis: [{
        //        plotBands: [{
        //            from: 0, to: 33, color: "#ccc", opacity: .6
        //        }, {
        //            from: 33, to: 66, color: "#ccc", opacity: .3
        //        }],
        //        majorGridLines: {
        //            visible: false
        //        },
        //        min: 0,
        //        max: 100,
        //        minorTicks: {
        //            visible: true
        //        }
        //    }],
        //    tooltip: {
        //        visible: true,
        //        template: "Maximum: #= value.target # <br /> Average: #= value.current #"
        //    }
        //});

        //$("#chart-temp").kendoChart({
        //    legend: {
        //        visible: false
        //    },
        //    series: [{
        //        type: "bullet",
        //        data: [[25, 22]]
        //    }],
        //    categoryAxis: {
        //        majorGridLines: {
        //            visible: false
        //        },
        //        majorTicks: {
        //            visible: false
        //        }
        //    },
        //    valueAxis: [{
        //        plotBands: [{
        //            from: 0, to: 10, color: "yellow", opacity: .3
        //        }, {
        //            from: 10, to: 20, color: "orange", opacity: .3
        //        }, {
        //            from: 20, to: 30, color: "red", opacity: .3
        //        }],
        //        majorGridLines: {
        //            visible: false
        //        },
        //        min: 0,
        //        max: 30,
        //        minorTicks: {
        //            visible: true
        //        }
        //    }],
        //    tooltip: {
        //        visible: true,
        //        template: "Maximum: #= value.target # <br /> Average: #= value.current #"
        //    }
        //});


        //$(document).ready(createChart);
        //$(document).bind("kendo:skinChange", createChart);


    }
})();


