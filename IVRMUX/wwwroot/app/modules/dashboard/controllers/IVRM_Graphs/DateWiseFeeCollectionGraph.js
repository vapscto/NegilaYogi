﻿
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
                $scope.yearmodel = true;

            }
            else if ($scope.rpttyp == "date") {
                $scope.datemodel = true;
                $scope.yearmodel = false;
                $scope.monthname = $scope.obj.monthid;

            }
            else if ($scope.rpttyp == "month") {
                $scope.monthmodel = true;
                $scope.yearmodel = false;
            }
        }
                    ]
            title: {
                text: "Mode Wise Fee Collection"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                ]
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
                categories: ["1", "2", "3", "4", "5", "6", "7", "8"
                    , "9"
                    , "10"

                    , "11"
                    , "12"
                    , "13"
                    , "14"
                    , "15"
                    , "16"
                    , "17"
                    , "18"
                    , " 19"
                    , "20"
                    , " 21"
                    , "22"
                    , "23"
                    , "24"
                    , "25"
                    , "26"
                    , "27"
                    , "28"
                    , "29"
                    , "30"
                    , "31"],
                line: {
                    visible: false
                },
                labels: {
                    padding: { top: 35 }
                }
            },
            tooltip: {
                visible: true,
                format: "{0}",
                template: "#= series.name #: #= value #"
            }
        });

        var geometry = kendo.geometry;
        function createChart() {
            $("#chart4").kendoChart({
                title: {
                    text: "Month Wise Fee Collection"
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
                    name: "Online",
                                   data: [25396774, 13622151, 12548385, 21207537, 5511303, 25396774, 13622151, 12548385, 21207537, 5511303, 13622151, 12548385]
                }, {
                    name: "Cash",
                    data: [15295992, 11831328, 36124400, 19752824, 7204160,15295992, 11831328, 36124400, 19752824, 7204160, 11831328, 36124400]
                    },
                    {
                        name: "Bank",
                          data: [
                              21224178	,18892284	,44115884	,28672712	,11232446	,21224178	,18892284	,44115884	,28672712	,23947909,18892284	,44115884]
                    },
                    {
                        name: "Total",
                        data: [61916944, 44345763, 92788669, 69633073, 23947909, 61916944, 44345763, 92788669, 69633073, 23947909, 44345763, 92788669]
                    }
                ],
                panes: [{
                    clip: false
                }],
                valueAxis: {
                    line: {
                        visible: false
                    }
                },
                categoryAxis: {
                    categories: ["January	", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
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
            title: {
                text: "Month Wise Collection Report"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
                 
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
                categories: ["January	", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
                    line: {
                    visible: false
                },
                labels: {
                    padding: { top: 35 }
                }
            },
            tooltip: {
                visible: true,
                format: "{0}",
                template: "#= series.name #: #= value #"
            }
        });
            $("#chart6").kendoChart({
                title: {
                    text: "Year Wise Collection"
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
                   
                   
                    
                    
                   
                    name: "Online",
                    data: [25396774, 13622151, 12548385, 21207537, 5511303]
                }, {
                    name: "Cash",
                        data: [15295992, 11831328, 36124400, 19752824, 7204160]
                }, {
                    name: "Bank",
                        data: [21224178, 18892284, 44115884, 28672712, 11232446]
                }, {
                    name: "Total",
                        data: [61916944, 44345763, 92788669, 69633073, 23947909]
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
                    categories: ["2017-2018", "2018-2019", "2019-2020	", "2020-2021", "2021-2022"],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}",
                    template: "#= series.name #: #= value #"
                }
            });
        }

        $(document).ready(createCharttest);
        $(document).bind("kendo:skinChange", createCharttest);

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





        //        name: "Online",
        //        data: [25396774, 13622151, 12548385, 21207537, 5511303]
        //    }, {
        //        name: "Cash",
        //        data: [15295992, 11831328, 36124400, 19752824, 7204160]
        //    }, {
        //        name: "Bank",
        //        data: [21224178, 18892284, 44115884, 28672712, 11232446]
        //    }, {
        //        name: "Total",
        //        data: [61916944, 44345763, 92788669, 69633073, 23947909]
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
        //        categories: ["2017-2018", "2018-2019", "2019-2020	", "2020-2021", "2021-2022"],
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
            $("#chart3").kendoChart({
                title: {
                    position: "bottom",
                    text: "2017-2018"
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
                        template: "#= category #: \n #= value#"
                    }
                },
                series: [{
                    type: "pie",
                    startAngle: 150,
                    data: [{
                        category: "Online",
                        value: 25396774,
                        color: "#139ee1"
                    }, {
                        category: "Cash",
                            value: 15295992,
                        color: "#5eabd1"
                    }, {
                        category: "Bank",
                            value: 21224178,
                        color: "#a6d0e5"
                    }, {
                        category: "Total",
                            value: 61916944,
                        color: "#cadce5"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
     
            $("#chart7").kendoChart({
                title: {
                    position: "bottom",
                    text: "2019-2020"
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
                        template: "#= category #: \n #= value#"
                    }
                },
                series: [{
                    type: "pie",
                    startAngle: 150,
                    data: [{
                        category: "Online",
                        value: 12548385,
                        color: "#139ee1"
                    }, {
                        category: "Cash",
                            value: 36124400,
                        color: "#5eabd1"
                    }, {
                        category: "Bank",
                            value: 44115884,
                        color: "#a6d0e5"
                    },  {
                        category: "Total",
                            value: 92788669,
                        color: "#cadce5"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
       
            $("#chart8").kendoChart({
                title: {
                    position: "bottom",
                    text: "2018-2019"
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
                        template: "#= category #: \n #= value#"
                    }
                },
                series: [{
                    type: "pie",
                    startAngle: 150,
                    data: [{
                        category: "Online",
                        value: 13622151,
                        color: "#139ee1"
                    }, {
                        category: "Cash",
                            value: 11831328,
                        color: "#5eabd1"
                    }, {
                        category: "Bank",
                            value: 18892284,
                        color: "#a6d0e5"
                    }, {
                        category: "Total",
                            value: 44345763,
                        color: "#cadce5"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
       
            $("#chart9").kendoChart({
                title: {
                    position: "bottom",
                    text: "2019-2020"
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
                        template: "#= category #: \n #= value#"
                    }
                },
                series: [{
                    type: "pie",
                    startAngle: 150,
                    data: [{
                        category: "Online",
                        value: 12548385,
                        color: "#139ee1"
                    }, {
                        category: "Cash",
                            value: 36124400,
                        color: "#5eabd1"
                    }, {
                        category: "Bank",
                            value: 44115884,
                        color: "#a6d0e5"
                    }, {
                        category: "Total",
                            value: 92788669,
                        color: "#cadce5"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
       
            $("#chart9").kendoChart({
                title: {
                    position: "bottom",
                    text: "2020-2021"
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
                        template: "#= category #: \n #= value#"
                    }
                },
                series: [{
                    type: "pie",
                    startAngle: 150,
                    data: [{
                        category: "Online",
                        value: 21207537,
                        color: "#139ee1"
                    }, {
                        category: "Cash",
                            value: 19752824,
                        color: "#5eabd1"
                    }, {
                        category: "Bank",
                            value: 28672712,
                        color: "#a6d0e5"
                    },  {
                        category: "Total",
                            value: 69633073,
                        color: "#cadce5"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
   
            $("#chart10").kendoChart({
                title: {
                    position: "bottom",
                    text: "2021-2022"
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
                        template: "#= category #: \n #= value#"
                    }
                },
                series: [{
                    type: "pie",
                    startAngle: 150,
                    data: [{
                        category: "Online",
                        value: 5511303	,
                        color: "#139ee1"
                    }, {
                        category: "Cash",
                            value: 7204160,
                        color: "#5eabd1"
                    }, {
                        category: "Bank",
                            value: 11232446,
                        color: "#a6d0e5"
                    }, {
                        category: "Total",
                            value: 23947909,
                        color: "#cadce5"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        