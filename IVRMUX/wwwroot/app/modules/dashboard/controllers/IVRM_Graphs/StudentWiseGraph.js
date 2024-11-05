(function () {    'use strict';    angular        .module('app')        .controller('DateWiseFeeCollectionGraph', DateWiseFeeCollectionGraph);    DateWiseFeeCollectionGraph.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache',]    function DateWiseFeeCollectionGraph($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, ) {        $scope.yearmodel = false;$scope.showmodel=false;        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));					$scope.asmaY_Id='2021-2022';			$scope.asmaY_Iddd ='2021-2022';				$scope.showmodel=true;				$scope.yearmodel = true;				        $scope.ShowReport = function () {			$scope.showmodel=true;
            $scope.yearmodel = true;
            $scope.asmaY_Iddd = $scope.asmaY_Id;
        }        function createChart1() {

            $("#chart1").kendoChart({
                title: {
                    text: "HEAD WISE COLLECTION"
                },
                legend: {
                    position: "top"
                },
                seriesDefaults: {
                    type: "column"
                },																								
                series: [{
                    name: "Total Charges",
                    data: [25000,35000,15000,65000]
                }, {
                    name: "Paid",
                    data: [10000,15000,12000,20000]

                }, 				{                    name: "Concession",                    data: [5000,15000,0,20000]                },				{
                    name: "Balance",
                    data: [10000,5000,3000,25000]
                }],
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
                    categories: ["Exam Fee", "Admission Fee", "Registration Fee", "Regular Fee"],
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


     
        }

        $(document).ready(createChart1);
        $(document).bind("kendo:skinChange", createChart1);
        var drawing = kendo.drawing;
        var geometry = kendo.geometry;
        function createChart2() {
            $("#chart2").kendoChart({
                title: {
                    text: "HEAD WISE COLLECTION"
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
                						                series: [{                    name: "Total Charges",                    data: [25000,35000,15000,65000]                }, {                    name: "Paid",                    data: [10000,15000,12000,20000]                }, 				{                    name: "Concession",                    data: [5000,15000,0,20000]                },				{                    name: "Balance",                    data: [10000,5000,3000,25000]                }],
                valueAxis: {
                    line: {
                        visible: false
                    }
                },
                categoryAxis: {
                    categories: ["Exam Fee", "Admission Fee", "Registration Fee", "Regular Fee"],
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

        $(document).ready(createChart2);
        $(document).bind("kendo:skinChange", createChart2);

    }})();