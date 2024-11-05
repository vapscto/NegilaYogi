(function () {    'use strict';    angular        .module('app')        .controller('DateWiseFeeCollectionGraph', DateWiseFeeCollectionGraph);    DateWiseFeeCollectionGraph.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache',]    function DateWiseFeeCollectionGraph($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, ) {        $scope.yearmodel = false;        $scope.datemodel = false;        $scope.monthmodel = false;        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));        $scope.obj = {};		$scope.showmodel=true;		 $scope.yearmodel = true;                $scope.datemodel = false;                $scope.monthmodel = false;        $scope.ShowReport = function () {			$scope.showmodel=true;            if ($scope.rpttyp == "year") {
                $scope.yearmodel = true;
                $scope.datemodel = false;                $scope.monthmodel = false;
            }
            else if ($scope.rpttyp == "date") {
                $scope.datemodel = true;
                $scope.yearmodel = false;                $scope.monthmodel = false;
                $scope.monthname = $scope.obj.monthid;

            }
            else if ($scope.rpttyp == "month") {
                $scope.monthmodel = true;
                $scope.yearmodel = false;                $scope.datemodel = false;
            }
        }        function createChart1() {            $("#chart1").kendoChart({                title: {                    text: "Mode Wise Fee Collection"                },                legend: {                    position: "bottom"                },                chartArea: {                    background: ""                },                seriesDefaults: {                    type: "line",                    style: "smooth"                },                series: [{                    name: "Online",                    data: [25396774, 13622151, 12548385, 21207537, 5511303, 16047874, 16802570, 11542293, 14710253, 7076362, 11847497, 6133453, 9956657, 3995992, 15001646, 2358788, 4490009, 6255740, 1155139, 6735781, 9625146, 3608925, 1203649, 1155139, 1155139, 7027852, 4766084, 8151880, 2810535, 16741087, 1708959]                },                {                    name: "Bank",                    data: [15295992, 11831328, 36124400, 19752824, 7204160, 15035160, 19708992, 10286496, 10132912, 9139664, 2336832, 2384832, 2288832, 12003496, 15780912, 2288832, 11693912, 8389496, 5857248, 3865664, 4673664, 3431832, 11412796, 12173744, 2288832, 0, 0, 14697328, 1144416, 10335912, 1144416
                    ]                }, {                    name: "Cash",                    data: [21224178, 18892284, 44115884, 28672712, 11232446, 21487290, 7851490, 14682160, 12775974, 13307148, 8721900, 7060148, 9533670, 6669602, 16206744, 4356888, 3433602, 2815484, 6862602, 2287298, 7672670, 6806782, 5072782, 8199962, 2572652, 0, 8759198, 9886198, 0, 14290682, 0]                }, {                    name: "Total",                    data: [61916944, 44345763, 92788669, 69633073, 23947909, 52570324, 44363052, 36510949, 37619139, 29523174, 22906229, 15578433, 21779159, 22669090, 46989302, 9004508, 19617523, 17460720, 13874989, 12888743, 21971480, 13847539, 17689227, 21528845, 7174460, 7027852, 16322119, 32735406, 3954951, 41367681, 2853375]                }],                valueAxis: {                    labels: {                        format: "{0}"                    },                    line: {                        visible: false                    },                    axisCrossingValue: -10                },                categoryAxis: {                 categories: ["1", "2", "3", "4", "5", "6", "7", "8"                    , "9"                    , "10"                    , "11"                    , "12"                    , "13"                    , "14"                    , "15"                    , "16"                    , "17"                    , "18"                    , " 19"                    , "20"                    , " 21"                    , "22"                    , "23"                    , "24"                    , "25"                    , "26"                    , "27"                    , "28"                    , "29"                    , "30"                    , "31"],                    majorGridLines: {                        visible: false                    },                    labels: {                        rotation: "auto"                    }                },                tooltip: {                    visible: true,                    format: "{0}",                    template: "#= series.name #: #= value #"                }            });        }        $(document).ready(createChart1);        $(document).bind("kendo:skinChange", createChart1);        $("#chart2").kendoChart({
            title: {
                text: "Mode Wise Fee Collection"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{                name: "Online",                data: [25396774, 13622151, 12548385, 21207537, 5511303, 16047874, 16802570, 11542293, 14710253, 7076362, 11847497, 6133453, 9956657, 3995992, 15001646, 2358788, 4490009, 6255740, 1155139, 6735781, 9625146, 3608925, 1203649, 1155139, 1155139, 7027852, 4766084, 8151880, 2810535, 16741087, 1708959]            },            {                name: "Bank",                data: [15295992, 11831328, 36124400, 19752824, 7204160, 15035160, 19708992, 10286496, 10132912, 9139664, 2336832, 2384832, 2288832, 12003496, 15780912, 2288832, 11693912, 8389496, 5857248, 3865664, 4673664, 3431832, 11412796, 12173744, 2288832, 0, 0, 14697328, 1144416, 10335912, 1144416
                ]            }, {                name: "Cash",                data: [21224178, 18892284, 44115884, 28672712, 11232446, 21487290, 7851490, 14682160, 12775974, 13307148, 8721900, 7060148, 9533670, 6669602, 16206744, 4356888, 3433602, 2815484, 6862602, 2287298, 7672670, 6806782, 5072782, 8199962, 2572652, 0, 8759198, 9886198, 0, 14290682, 0]            }, {                name: "Total",                data: [61916944, 44345763, 92788669, 69633073, 23947909, 52570324, 44363052, 36510949, 37619139, 29523174, 22906229, 15578433, 21779159, 22669090, 46989302, 9004508, 19617523, 17460720, 13874989, 12888743, 21971480, 13847539, 17689227, 21528845, 7174460, 7027852, 16322119, 32735406, 3954951, 41367681, 2853375]            }],
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
        var drawing = kendo.drawing;
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
        $(document).bind("kendo:skinChange", createChart);        $("#chart5").kendoChart({
            title: {
                text: "Month Wise Collection Report"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
                                            series: [{                    name: "Online",                                   data: [25396774, 13622151, 12548385, 21207537, 5511303, 25396774, 13622151, 12548385, 21207537, 5511303, 13622151, 12548385]                }, {                    name: "Cash",                    data: [15295992, 11831328, 36124400, 19752824, 7204160,15295992, 11831328, 36124400, 19752824, 7204160, 11831328, 36124400]                    },                    {                        name: "Bank",                          data: [                              21224178	,18892284	,44115884	,28672712	,11232446	,21224178	,18892284	,44115884	,28672712	,23947909,18892284	,44115884]                    },                    {                        name: "Total",                        data: [61916944, 44345763, 92788669, 69633073, 23947909, 61916944, 44345763, 92788669, 69633073, 23947909, 44345763, 92788669]                    }                ],
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
        });        function createCharttest() {
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
        //$("#chart3").kendoChart({
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
            }})();