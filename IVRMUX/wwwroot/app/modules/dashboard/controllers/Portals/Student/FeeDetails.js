(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeDetailsController', FeeDetailsController);

    FeeDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window'];
    function FeeDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


        $scope.overallfee = false;
        $scope.detailsfee = false;
        $scope.feedetialsGrid = false;
        $scope.feedetailsrdo = "overall";
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
            apiService.getDATA("FeeDetails/getloaddata").
                then(function (promise) {
                    $scope.yearclslst = promise.yearclsList;
                    $scope.feecurrentyer = promise.feecurrentyear;

                    for (var t = 0; t < $scope.yearclslst.length; t++) {
                        if ($scope.yearclslst[t].ASMAY_Id === $scope.feecurrentyer[0].asmaY_Id) {
                            $scope.asmaY_Id = $scope.feecurrentyer[t].asmaY_Id;
                        }
                    }
                    $scope.onclickoverall();
                    $scope.onacadyearchange();
                });
        };

        // ===============================Radio Change
        $scope.onclickoverall = function () {
            if ($scope.feedetailsrdo === "overall") {
                $scope.overallfee = true;
                $scope.detailsfee = false;
                $scope.onacadyearchange();
            }
            else if ($scope.feedetailsrdo === "detailed") {
                $scope.overallfee = false;
                $scope.detailsfee = true;
                $scope.onacadyearchange();
            }
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };
        //=====================-Academic Year Selection
        $scope.onacadyearchange = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "type": $scope.feedetailsrdo
            };
            apiService.create("FeeDetails/Getdetails", data).
                then(function (promise) {
                    $scope.getfeedata = promise.getfeedetails;
                    if ($scope.getfeedata.length > 0 && $scope.getfeedata !== null) {
                        $scope.Fee_grid = true;
                        $scope.feedetialsGrid = true;
                        if ($scope.feedetailsrdo === "overall") {
                            if (($scope.getfeedata[0].Receivable !== null && $scope.getfeedata[0].Receivable !== "NULL")) {
                                $scope.overallfee = true;
                                $scope.detailsfee = false;
                                var cons = 0;
                                var adjs = 0;
                                for (var a = 0; a < $scope.getfeedata.length; a++) {
                                    cons += parseFloat($scope.getfeedata[a].Concession);
                                    adjs += parseFloat($scope.getfeedata[a].Adjusted);
                                }
                                if (cons > 0) {
                                    $scope.con = true;
                                }
                                else {
                                    $scope.con = false;
                                }
                                if (adjs > 0) {
                                    $scope.adj = true;
                                }
                                else {
                                    $scope.adj = false;
                                }
                                //=================Overall Graph
                                $scope.head = ["RECEIVABLE", "CONCESSION", "COLLECTION", "ADJUSTED", "BALANCE"];

                                $scope.overallgraph = [];
                                $scope.overallgraph1 = [];
                                $scope.overallgraph2 = [];
                                $scope.overallgraph3 = [];
                                $scope.overallgraph4 = [];
                                if ($scope.getfeedata !== null) {
                                    angular.forEach($scope.getfeedata, function (fdt) {
                                        $scope.overallgraph.push({ label: $scope.head[0], y: fdt.Receivable, name: "RECEIVABLE" });
                                        $scope.overallgraph1.push({ indexLabel: $scope.head[0], y: fdt.Receivable, name: "RECEIVABLE" });

                                        $scope.overallgraph.push({ label: $scope.head[1], y: fdt.Concession, name: "CONCESSION" });
                                        $scope.overallgraph1.push({ indexLabel: $scope.head[1], y: fdt.Concession, name: "CONCESSION" });

                                        $scope.overallgraph.push({ label: $scope.head[2], y: fdt.Collectionamount, name: "COLLECTION" });
                                        $scope.overallgraph1.push({ indexLabel: $scope.head[2], y: fdt.Collectionamount, name: "COLLECTION" });
                                        $scope.overallgraph.push({ label: $scope.head[3], y: fdt.Adjusted, name: "ADJUSTMENT" });
                                        $scope.overallgraph1.push({ indexLabel: $scope.head[3], y: fdt.Adjusted, name: "ADJUSTMENT" });

                                        $scope.overallgraph.push({ label: $scope.head[4], y: fdt.Balance, name: "BALANCE" });
                                        $scope.overallgraph1.push({ indexLabel: $scope.head[4], y: fdt.Balance, name: "BALANCE" });

                                    });
                                }

                                var chart = new CanvasJS.Chart("columnchart", {
                                    animationEnabled: true,
                                    animationDuration: 3000,
                                    height: 350,
                                    colorSet: "graphcolor",
                                    axisX: {
                                        labelFontSize: 12
                                    },
                                    axisY: {
                                        labelFontSize: 12
                                    },
                                    toolTip: {
                                        shared: true
                                    },
                                    data: [
                                        {
                                            type: "column",
                                            showInLegend: false,
                                            dataPoints: $scope.overallgraph
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
                                        labelFontSize: 12
                                    },
                                    axisY: {
                                        labelFontSize: 12
                                    },

                                    data: [
                                        {

                                            type: "doughnut",
                                            innerRadius: 90,
                                            showInLegend: true,
                                            dataPoints: $scope.overallgraph1
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
                                swal("No Record  found");
                                $scope.overallfee = false;
                            }

                        }
                        //-----------------Detailed Graph
                        else if ($scope.feedetailsrdo == "detailed") {
                            if ($scope.getfeedata.length != "0" && $scope.getfeedata != null) {
                                $scope.overallfee = false;
                                $scope.detailsfee = true;
                                $scope.detailedgraph = [];
                                $scope.detailedgraph1 = [];

                                var cons = 0;
                                var adjs = 0;
                                for (var a = 0; a < $scope.getfeedata.length; a++) {
                                    cons += parseFloat($scope.getfeedata[a].CONCESSION);
                                    adjs += parseFloat($scope.getfeedata[a].ADJUSTMENT);
                                }
                                if (cons == "0") {
                                    $scope.conD = false;
                                }
                                else {
                                    $scope.conD = true;
                                }
                                if (adjs == "0") {
                                    $scope.adjD = false;
                                }
                                else {
                                    $scope.adjD = true;
                                }


                                $scope.detailedgraph = [];
                                if ($scope.getfeedata != null) {
                                    for (var a = 0; a < $scope.getfeedata.length; a++) {
                                        $scope.detailedgraph.push({ label: $scope.getfeedata[a].FEE_HEAD, "y": $scope.getfeedata[a].RECEIVABLE })
                                    }
                                }
                                $scope.detailedgraph1 = [];
                                if ($scope.getfeedata != null) {
                                    for (var a = 0; a < $scope.getfeedata.length; a++) {
                                        $scope.detailedgraph1.push({ label: $scope.getfeedata[a].FEE_HEAD, "y": $scope.getfeedata[a].CONCESSION })
                                    }
                                }
                                $scope.detailedgraph2 = [];
                                if ($scope.getfeedata != null) {
                                    for (var a = 0; a < $scope.getfeedata.length; a++) {
                                        $scope.detailedgraph2.push({ label: $scope.getfeedata[a].FEE_HEAD, "y": $scope.getfeedata[a].COLLECTION })
                                    }
                                }
                                $scope.detailedgraph3 = [];
                                if ($scope.getfeedata != null) {
                                    for (var a = 0; a < $scope.getfeedata.length; a++) {
                                        $scope.detailedgraph3.push({ label: $scope.getfeedata[a].FEE_HEAD, "y": $scope.getfeedata[a].ADJUSTMENT })
                                    }
                                }
                                $scope.detailedgraph4 = [];
                                if ($scope.getfeedata != null) {
                                    for (var a = 0; a < $scope.getfeedata.length; a++) {
                                        $scope.detailedgraph4.push({ label: $scope.getfeedata[a].FEE_HEAD, "y": $scope.getfeedata[a].BALANCE })
                                    }
                                }

                                var chart = new CanvasJS.Chart("columnchartdetailed", {
                                    animationEnabled: true,
                                    animationDuration: 3000,
                                    colorSet: "graphcolor",
                                    height: 400,
                                    width: 1086,
                                    axisX: {
                                        interval: 1,
                                        labelFontSize: 12,
                                    },
                                    axisY: {
                                        labelFontSize: 12,
                                    },

                                    //toolTip: {
                                    //    shared: true
                                    //},
                                    data: [{
                                        name: "RECEIVABLE",
                                        showInLegend: true,
                                        type: "column",
                                        //color: "rgba(40,175,101,0.6)",
                                        dataPoints: $scope.detailedgraph
                                    },
                                    {
                                        name: "CONCESSION",
                                        showInLegend: true,
                                        type: "column",
                                        //color: "rgba(0,75,141,0.7)",
                                        dataPoints: $scope.detailedgraph1
                                    },
                                    {
                                        name: "COLLECTION",
                                        showInLegend: true,
                                        type: "column",
                                        //color: "rgba(255,255,0.7)",
                                        dataPoints: $scope.detailedgraph2
                                    },
                                    {
                                        name: "ADJUSTMENT",
                                        showInLegend: true,
                                        type: "column",
                                        //color: "rgba(255,0,0,0.4)",
                                        dataPoints: $scope.detailedgraph3
                                    },
                                    {
                                        name: "BALANCE",
                                        showInLegend: true,
                                        type: "column",
                                        //color: "rgba(0,0,255,0.5)",
                                        dataPoints: $scope.detailedgraph4
                                    }
                                    ]
                                });
                                chart.render();

                                var chart = new CanvasJS.Chart("rangeBarChatdetailed", {
                                    animationEnabled: true,
                                    animationDuration: 3000,
                                    colorSet: "graphcolor",
                                    height: 400,
                                    width: 1086,
                                    axisX: {
                                        interval: 1,
                                        labelFontSize: 10,
                                    },
                                    axisY: {
                                        labelFontSize: 12,
                                    },

                                    //toolTip: {
                                    //    shared: true
                                    //},
                                    data: [{
                                        name: "RECEIVABLE",
                                        showInLegend: true,
                                        type: "area",
                                        //color: "rgba(40,175,101,0.6)",
                                        dataPoints: $scope.detailedgraph
                                    },
                                    {
                                        name: "CONCESSION",
                                        showInLegend: true,
                                        type: "area",
                                        //color: "rgba(0,75,141,0.7)",
                                        dataPoints: $scope.detailedgraph1
                                    },
                                    {
                                        name: "COLLECTION",
                                        showInLegend: true,
                                        type: "area",
                                        //color: "rgba(0,75,141,0.7)",
                                        dataPoints: $scope.detailedgraph2
                                    },
                                    {
                                        name: "ADJUSTMENT",
                                        showInLegend: true,
                                        type: "area",
                                        //color: "rgba(255,0,0,0.4)",
                                        dataPoints: $scope.detailedgraph3
                                    },
                                    {
                                        name: "BALANCE",
                                        showInLegend: true,
                                        type: "area",
                                        //color: "rgba(0,0,255,0.5)",
                                        dataPoints: $scope.detailedgraph4
                                    }
                                    ]
                                });
                                chart.render();
                            }
                            else {
                                swal("No Record found")
                                $scope.detailsfee = false;
                            }
                        }
                    }
                    else {
                        swal("No Record found")
                        $scope.feedetialsGrid = false;
                    }


                })
        }
    };
})();