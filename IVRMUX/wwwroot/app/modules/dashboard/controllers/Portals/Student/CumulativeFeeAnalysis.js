(function () {
    'use strict';
    angular
        .module('app')
        .controller('CumulativeFeeAnalysisController', CumulativeFeeAnalysisController)

    CumulativeFeeAnalysisController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function CumulativeFeeAnalysisController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.overallfee = false;
        $scope.detailsfee = false;
        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#3498DB",
                "#76D7C4",
                "#808B96",
                "#80DEEA",
                "#C5E1A5",
                "#AAB7B8"
            ]);
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;


            apiService.getDATA("CumulativeFeeAnalysis/getloaddata").
                then(function (promise) {
                    $scope.acdlist = promise.acdlist;
                    $scope.feeAnalysislst = promise.feeAnalysisList;
                    $scope.studentfeedetails = promise.studentfeedetails;

                 
                    if ((promise.feeAnalysisList.length > 0 && promise.studentfeedetails.length > 0) || (promise.feeAnalysisList.length > 0 || promise.studentfeedetails.length > 0)) {
                        $scope.head = ["RECEIVABLE", "CONCESSION", "COLLECTION", "ADJUSTED", "BALANCE"]
                        $scope.analysisgraph = [];
                        $scope.analysisgraph1 = [];
                        if ($scope.feeAnalysislst != null) {
                            for (var a = 0; a < $scope.feeAnalysislst.length; a++) {

                                $scope.analysisgraph.push({ label: $scope.head[0], y: $scope.feeAnalysislst[a].RECEIVABLE, name: "RECEIVABLE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                                $scope.analysisgraph1.push({ indexLabel: $scope.head[0], y: $scope.feeAnalysislst[a].RECEIVABLE, name: "RECEIVABLE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year })

                            }
                        }
                        //  $scope.analysisgraph1 = [];
                        if ($scope.feeAnalysislst != null) {
                            for (var a = 0; a < $scope.feeAnalysislst.length; a++) {

                                $scope.analysisgraph.push({ label: $scope.head[1], y: $scope.feeAnalysislst[a].CONCESSION, name: "CONCESSION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                                $scope.analysisgraph1.push({ indexLabel: $scope.head[1], y: $scope.feeAnalysislst[a].CONCESSION, name: "CONCESSION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });

                            }
                        }

                        if ($scope.feeAnalysislst != null) {
                            for (var a = 0; a < $scope.feeAnalysislst.length; a++) {

                                $scope.analysisgraph.push({ label: $scope.head[2], y: $scope.feeAnalysislst[a].COLLECTION, name: "COLLECTION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                                $scope.analysisgraph1.push({ indexLabel: $scope.head[2], y: $scope.feeAnalysislst[a].COLLECTION, name: "COLLECTION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            }
                        }

                        if ($scope.feeAnalysislst != null) {
                            for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                                $scope.analysisgraph.push({ label: $scope.head[3], y: $scope.feeAnalysislst[a].ADJUSTED, name: "ADJUSTMENT" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                                $scope.analysisgraph1.push({ indexLabel: $scope.head[3], y: $scope.feeAnalysislst[a].ADJUSTED, name: "ADJUSTMENT" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            }
                        }

                        if ($scope.feeAnalysislst != null) {
                            for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                                $scope.analysisgraph.push({ label: $scope.head[4], y: $scope.feeAnalysislst[a].BALANCE, name: "BALANCE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                                $scope.analysisgraph1.push({ indexLabel: $scope.head[4], y: $scope.feeAnalysislst[a].BALANCE, name: "BALANCE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            }
                        }

                        var chart = new CanvasJS.Chart("columnchart", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            colorSet: "graphcolor",
                            axisX: {
                                labelFontSize: 12,
                            },
                            axisY: {
                                labelFontSize: 12,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [
                                {
                                    type: "column",
                                    showInLegend: false,
                                    dataPoints: $scope.analysisgraph
                                }
                            ]
                        });
                        chart.render();

                        var chart = new CanvasJS.Chart("chartContainer", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            colorSet: "graphcolor",
                            axisX: {
                                interval: 1,
                                labelFontSize: 12,
                            },
                            axisY: {
                                labelFontSize: 12,
                            },

                            data: [
                                {
                                    type: "doughnut",
                                    innerRadius: 90,
                                    showInLegend: true,
                                    dataPoints: $scope.analysisgraph1
                                }
                            ]

                        });
                        chart.render();
                        function explodePie(e) {
                            if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
                                e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
                            } else {
                                e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
                            }
                        } e.chart.render();
                    }

                    else {
                        swal('No Data Found!!!')
                        $scope.feeAnalysislst = [];
                        $scope.studentfeedetails = [];
                    }
                   


                })
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.onreport = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,

            };
            apiService.create("CumulativeFeeAnalysis/onreport", data).
                then(function (promise) {
                    if ((promise.feeAnalysisList.length > 0 && promise.studentfeedetails.length > 0) || (promise.feeAnalysisList.length > 0 || promise.studentfeedetails.length > 0)) {
                        $scope.feeAnalysislst = promise.feeAnalysisList;
                        $scope.studentfeedetails = promise.studentfeedetails;

                        $scope.head = ["RECEIVABLE", "CONCESSION", "COLLECTION", "ADJUSTED", "BALANCE"]
                        $scope.analysisgraph = [];
                        $scope.analysisgraph1 = [];
                        if ($scope.feeAnalysislst != null) {
                            for (var a = 0; a < $scope.feeAnalysislst.length; a++) {

                                $scope.analysisgraph.push({ label: $scope.head[0], y: $scope.feeAnalysislst[a].RECEIVABLE, name: "RECEIVABLE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                                $scope.analysisgraph1.push({ indexLabel: $scope.head[0], y: $scope.feeAnalysislst[a].RECEIVABLE, name: "RECEIVABLE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year })

                            }
                        }
                        //  $scope.analysisgraph1 = [];
                        if ($scope.feeAnalysislst != null) {
                            for (var a = 0; a < $scope.feeAnalysislst.length; a++) {

                                $scope.analysisgraph.push({ label: $scope.head[1], y: $scope.feeAnalysislst[a].CONCESSION, name: "CONCESSION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                                $scope.analysisgraph1.push({ indexLabel: $scope.head[1], y: $scope.feeAnalysislst[a].CONCESSION, name: "CONCESSION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });

                            }
                        }

                        if ($scope.feeAnalysislst != null) {
                            for (var a = 0; a < $scope.feeAnalysislst.length; a++) {

                                $scope.analysisgraph.push({ label: $scope.head[2], y: $scope.feeAnalysislst[a].COLLECTION, name: "COLLECTION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                                $scope.analysisgraph1.push({ indexLabel: $scope.head[2], y: $scope.feeAnalysislst[a].COLLECTION, name: "COLLECTION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            }
                        }

                        if ($scope.feeAnalysislst != null) {
                            for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                                $scope.analysisgraph.push({ label: $scope.head[3], y: $scope.feeAnalysislst[a].ADJUSTED, name: "ADJUSTMENT" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                                $scope.analysisgraph1.push({ indexLabel: $scope.head[3], y: $scope.feeAnalysislst[a].ADJUSTED, name: "ADJUSTMENT" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            }
                        }

                        if ($scope.feeAnalysislst != null) {
                            for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                                $scope.analysisgraph.push({ label: $scope.head[4], y: $scope.feeAnalysislst[a].BALANCE, name: "BALANCE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                                $scope.analysisgraph1.push({ indexLabel: $scope.head[4], y: $scope.feeAnalysislst[a].BALANCE, name: "BALANCE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            }
                        }

                        var chart = new CanvasJS.Chart("columnchart", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            colorSet: "graphcolor",
                            axisX: {
                                labelFontSize: 12,
                            },
                            axisY: {
                                labelFontSize: 12,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [
                                {
                                    type: "column",
                                    showInLegend: false,
                                    dataPoints: $scope.analysisgraph
                                }
                            ]
                        });
                        chart.render();

                        var chart = new CanvasJS.Chart("chartContainer", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            colorSet: "graphcolor",
                            axisX: {
                                interval: 1,
                                labelFontSize: 12,
                            },
                            axisY: {
                                labelFontSize: 12,
                            },

                            data: [
                                {
                                    type: "doughnut",
                                    innerRadius: 90,
                                    showInLegend: true,
                                    dataPoints: $scope.analysisgraph1
                                }
                            ]

                        });
                        chart.render();
                        function explodePie(e) {
                            if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
                                e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
                            } else {
                                e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
                            }
                            e.chart.render();
                        }
                    }
                    else {
                        swal('No Data Found!!!')
                        $scope.feeAnalysislst = [];
                        $scope.studentfeedetails = [];
                    }
                });

        };


    };
})();