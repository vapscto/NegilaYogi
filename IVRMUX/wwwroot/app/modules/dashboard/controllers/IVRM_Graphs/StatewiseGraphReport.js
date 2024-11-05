//////////////"column

(function () {
    'use strict';
    angular
        .module('app')
        .controller('StatewiseGraphReportController', StatewiseGraphReportController)

    StatewiseGraphReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function StatewiseGraphReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {
        $("#chart123").kendoChart({
            title: {
                text: "State Wise Admission Count"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "  Karnataka",
                data: [38,64, 81, 95, 113]
            }, {
                    name: "Andhra Pradesh ",
                    data: [3,13, 9, 12, 2]
            }, {
                    name: "Kerala ",
                    data: [2,0, 1, 0, 1]
            }, {
                    name: " Maharashtra",
                    data: [5, 3, 2, 4, 2]
                },
                //{
                //    name: " Tamil Nadu",
                //    data: [1.988, 2.733, 3.994, 3.464, 4.001, 3.939, 1.333, -2.245, 4.339, 2.727]
                //}
            ],
           
            valueAxis: {
                labels: {
                    format: "{0}"
                },
                line: {
                    visible: false
                },
                axisCrossingValue: 0
            },
            categoryAxis: {
                categories: ["PREKG", "LKG", "UKG", "I Std ","II Std"],
                line: {
                    visible: false
                },
                labels: {
                    padding: { top: 10 }
                }
            },
            tooltip: {
                visible: true,
                format: "{0}%",
                template: "#= series.name #: #= value #"
            }
        });
        $scope.test = false;
        $scope.Report = function () {
            $scope.test = true;
        }

        ////////////////////////////bar
        var drawing = kendo.drawing;
        var geometry = kendo.geometry;
        function createChart() {
            $("#chart").kendoChart({
                title: {
                    text: "Site Visitors Stats /thousands/"
                },
                legend: {
                    position: "bottom",
                    item: {
                        visual: createLegendItem
                    }
                },
                seriesDefaults: {
                    type: "column",
                    stack: true,
                    highlight: {
                        toggle: function (e) {
                            // Don't create a highlight overlay,
                            // we'll modify the existing visual instead
                            e.preventDefault();

                            var visual = e.visual;
                            var opacity = e.show ? 0.8 : 1;

                            visual.opacity(opacity);
                        }
                    },
                    visual: function (e) {
                        return createColumn(e.rect, e.options.color);
                    }
                },
                series: [{
                    name: "Total Visits",
                    data: [56000, 63000, 74000, 91000, 117000, 138000, 128000, 115000, 102000, 98000, 123000, 113000]
                }, {
                    name: "Unique visitors",
                    data: [52000, 34000, 23000, 48000, 67000, 83000, 40000, 50000, 64000, 72000, 75000, 81000]
                }],
                panes: [{
                    clip: false
                }],
                valueAxis: {
                    line: {
                        visible: false
                    }
                },
                categoryAxis: {
                    categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                    majorGridLines: {
                        visible: false
                    },
                    line: {
                        visible: false
                    }
                },
                tooltip: {
                    visible: true
                }
            });
        }

        function createColumn(rect, color) {
            var origin = rect.origin;
            var center = rect.center();
            var bottomRight = rect.bottomRight();
            var radiusX = rect.width() / 2;
            var radiusY = radiusX / 3;
            var gradient = new drawing.LinearGradient({
                stops: [{
                    offset: 0,
                    color: color
                }, {
                    offset: 0.5,
                    color: color,
                    opacity: 0.9
                }, {
                    offset: 0.5,
                    color: color,
                    opacity: 0.9
                }, {
                    offset: 1,
                    color: color
                }]
            });

            var path = new drawing.Path({
                fill: gradient,
                stroke: {
                    color: "none"
                }
            }).moveTo(origin.x, origin.y)
                .lineTo(origin.x, bottomRight.y)
                .arc(180, 0, radiusX, radiusY, true)
                .lineTo(bottomRight.x, origin.y)
                .arc(0, 180, radiusX, radiusY);

            var topArcGeometry = new geometry.Arc([center.x, origin.y], {
                startAngle: 0,
                endAngle: 360,
                radiusX: radiusX,
                radiusY: radiusY
            });

            var topArc = new drawing.Arc(topArcGeometry, {
                fill: {
                    color: color
                },
                stroke: {
                    color: "#ebebeb"
                }
            });
            var group = new drawing.Group();
            group.append(path, topArc);
            return group;
        }

        function createLegendItem(e) {
            var color = e.options.markers.background;
            var labelColor = e.options.labels.color;
            var rect = new geometry.Rect([0, 0], [120, 50]);
            var layout = new drawing.Layout(rect, {
                spacing: 5,
                alignItems: "center"
            });

            var overlay = drawing.Path.fromRect(rect, {
                fill: {
                    color: "#fff",
                    opacity: 0
                },
                stroke: {
                    color: "none"
                },
                cursor: "pointer"
            });

            var column = createColumn(new geometry.Rect([0, 0], [15, 10]), color);
            var label = new drawing.Text(e.series.name, [0, 0], {
                fill: {
                    color: labelColor
                }
            })

            layout.append(column, label);
            layout.reflow();

            var group = new drawing.Group().append(layout, overlay);

            return group;
        }

        $(document).ready(createChart);
        $(document).bind("kendo:skinChange", createChart);



        //////////////////////////////////////////////////// line


        function createChart1() {
            $("#chart1").kendoChart({
                title: {
                    text: "Gross domestic product growth \n /GDP annual %/"
                },
                legend: {
                    position: "bottom"
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: [{
                    name: "India",
                    data: [3.907, 7.943, 7.848, 9.284, 9.263, 9.801, 3.890, 8.238, 9.552, 6.855]
                }, {
                    name: "World",
                    data: [1.988, 2.733, 3.994, 3.464, 4.001, 3.939, 1.333, -2.245, 4.339, 2.727]
                }, {
                    name: "Russian Federation",
                    data: [4.743, 7.295, 7.175, 6.376, 8.153, 8.535, 5.247, -7.832, 4.3, 4.3]
                }, {
                    name: "Haiti",
                    data: [-0.253, 0.362, -3.519, 1.799, 2.252, 3.343, 0.843, 2.877, -5.416, 5.590]
                }],
                valueAxis: {
                    labels: {
                        format: "{0}%"
                    },
                    line: {
                        visible: false
                    },
                    axisCrossingValue: -10
                },
                categoryAxis: {
                    categories: [2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011],
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
        }

        $(document).ready(createChart1);
        $(document).bind("kendo:skinChange", createChart1);



        //////////////////////////////////// pie
        function createChart2() {
            $("#chart2").kendoChart({
                title: {
                    position: "bottom",
                    text: "StateWise Admission Count"
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
                        value: 450,
                        color: "#9de219"
                    }, {
                        category: "LKG",
                        value: 490,
                        color: "#90cc38"
                    }, {
                        category: "UKG",
                        value: 280,
                        color: "#068c35"
                    }, {
                        category: "I CLASS",
                        value: 2207,
                        color: "#006634"
                        }
                    //    , {
                    //        category: "II CLASS",
                    //    value: 800,
                    //    color: "#004d38"
                        //}
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


    }
})();