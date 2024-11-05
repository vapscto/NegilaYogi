////////////////"column

(function () {
    'use strict';
    angular
        .module('app')
        .controller('CategorygraphReportController', CategorygraphReportController)

    CategorygraphReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function CategorygraphReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {
        $("#chart123").kendoChart({
            title: {
                text: "Preadmission Status Report"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{

                name: "ACCEPTED",
                color: "light green",
                data: [100, 100, 50, 50, 50]
            }, {
                name: "INPROGRESS",
                color: "green",
                data: [10, 10, 80, 80, 80]
            }, {
                name: "SELECTED",
                color: "#b8cd8e",
                data: [20, 200, 200, 200, 200]

            }, {
                name: "WAITING",
                color: "#e1d766",
                data: [10, 10, 42, 42, 107]
            },
            {
                name: "REJECTED",
                color: "yellow",
                data: [55, 80, 23, 15, 56]
            },

                //{
                //    name: " Tamil Nadu",
                //    data: [1.988, 2.733, 3.994, 3.464, 4.001, 3.939, 1.333, -2.245, 4.339, 2.727]
                //}
            ],

            valueAxis: {
                labels: {
                    format: "{0}%"
                },
                line: {
                    visible: false
                },
                axisCrossingValue: 0
            },
            categoryAxis: {
                categories: ["PREKG", "LKG", "UKG", "I CLASS ", "II CLASS "],
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

        $scope.test = false;
        $scope.Report = function () {
            $scope.test = true;
        }


        //////////////////////////////////////////////////// line
        $("#chart1").kendoChart({
            title: {
                text: "Caste Categorywise Admission Count"
            },
            legend: {
                position: "bottom"
            },
            seriesDefaults: {
                type: "column",
                area: {
                    line: {
                        style: "smooth"
                    }
                }
            },
            series: [{
                name: "SC",

                data: [5, 15, 22,13,19,11],
                color: "#2269cb",

            }, {
                name: "OBC",

                    data: [12, 55, 45,12,63,39],
                color: "#d3752e",
            }, {
                    name: "General",

                    data: [21, 10, 16,16,12,71],
                color: "#b5b5b1",
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
            categoryAxis: [
                {
                    categories: ["PREKG", "LKG", "UKG", "PREKG", "LKG", "UKG"],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                },
                {
                    categories: ["2021-2022", "2020-2021"],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                }
            ],
            tooltip: {
                visible: true,
                format: "{0}%",
                template: "#= series.name #: #= value #"
            }
        });

        //function createChart1() {
        //    $("#chart1").kendoChart({
        //        title: {
        //            text: "Preadmission Status Report"
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
        //            name: "ACCEPTED",
        //            data: [100, 100, 50, 50, 50],
        //            dashType: "dash",
        //            color: "blue"
        //        }, {
        //            name: "INPROGRESS",
        //            data: [10, 10, 80, 80, 80],
        //            color: "skyblue"
        //        }, {
        //            name: "SELECTED",
        //            data: [20, 200, 200, 200, 200],
        //            color: "yellow"
        //        }, {
        //            name: "WAITING",
        //            data: [10, 10, 42, 42, 107],
        //            color: "red"
        //        },
        //        {
        //            name: "REJECTED",
        //            data: [55, 80, 23, 15, 56],
        //            color: "gray"
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
        //            categories: ["PREKG", "UKG", "LKG", "I CLASS"],
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
                    position: "bottom",
                    text: "Share of Internet Population Growth, 2007 - 2012"
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
                    }, {
                        category: "II CLASS",
                        value: 800,
                        color: "#004d38"
                    }, {
                        category: "North America",
                        value: 600,
                        color: "#033939"
                    }]
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


