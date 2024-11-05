(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgFeeDetailsController', ClgFeeDetailsController)

    ClgFeeDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ClgFeeDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        
      //  $scope.overallfee = false;
       // $scope.detailsfee = false;
        $scope.feedetialsGrid = false;
        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#34495E",
                "#85C1E9",
                "#DAF7A6",
                "#FFC300",
                "#FF5733",
            ]);

        $scope.cfg = {};

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("ClgFeeDetails/getloaddata").
                then(function (promise) {
                    $scope.yearlist = promise.yearlist;
                    $scope.currentyear = promise.currentyear;

                    $scope.cfg.ASMAY_Id = $scope.currentyear[0].asmaY_Id;

                    //angular.forEach($scope.yearlist, function (y) {
                    //    angular.forEach($scope.currentyear, function (c) {
                    //        if (y.asmaY_Id == c.asmaY_Id) {
                    //            $scope.asmaY_Id = c.asmaY_Id;
                    //        }
                    //    })
                    //})

                    $scope.onclickradio();
                    $scope.onacadyearchange();
                })
        };

        // ========================== Radio Change
        $scope.onclickradio = function () {
            if ($scope.feedetailsrdo == "overall") {
              //  $scope.overallfee = true;
              //  $scope.detailsfee = false;
                $scope.onacadyearchange();
            }
            else if ($scope.feedetailsrdo == "detailed") {
              //  $scope.overallfee = false;
              //  $scope.detailsfee = true;
                $scope.onacadyearchange();
            }           
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   
            $scope.reverse = !$scope.reverse; 
        }
        //===========================Academic Year Selection
        $scope.onacadyearchange = function () {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "type": $scope.feedetailsrdo,
            }
            apiService.create("ClgFeeDetails/Getdetails", data).
                then(function (promise) {

                    $scope.getfeedata = promise.getfeedetails;
                    if ($scope.getfeedata.length > 0 && $scope.getfeedata != null) {

                       // $scope.Fee_grid = true;
                        $scope.feedetialsGrid = true;
                        if ($scope.feedetailsrdo == "overall") {
                            if (($scope.getfeedata[0].Receivable != null && $scope.getfeedata[0].Receivable != "NULL")) {

                                $scope.overallfee = true;
                                $scope.detailsfee = false;

                                //==============================Overall Graph

                                $scope.head = ["RECEIVABLE", "CONCESSION", "COLLECTION", "ADJUSTED", "BALANCE"]
                                $scope.overallgraph = [];
                                $scope.overallgraph1 = [];

                                angular.forEach($scope.getfeedata, function (o) {
                                    $scope.overallgraph.push({ label: $scope.head[0], y: o.Receivable, name: "RECEIVABLE" });
                                    $scope.overallgraph1.push({ indexLabel: $scope.head[0], y: o.Receivable, name: "RECEIVABLE" })

                                    $scope.overallgraph.push({ label: $scope.head[1], y: o.Concession, name: "CONCESSION" });
                                    $scope.overallgraph1.push({ indexLabel: $scope.head[1], y: o.Concession, name: "CONCESSION" });

                                    $scope.overallgraph.push({ label: $scope.head[2], y: o.Collectionamount, name: "COLLECTION" });
                                    $scope.overallgraph1.push({ indexLabel: $scope.head[2], y: o.Collectionamount, name: "COLLECTION" });

                                    $scope.overallgraph.push({ label: $scope.head[3], y: o.Adjusted, name: "ADJUSTMENT" });
                                    $scope.overallgraph1.push({ indexLabel: $scope.head[3], y: o.Adjusted, name: "ADJUSTMENT" });

                                    $scope.overallgraph.push({ label: $scope.head[4], y: o.Balance, name: "BALANCE" });
                                    $scope.overallgraph1.push({ indexLabel: $scope.head[4], y: o.Balance, name: "BALANCE" });
                                })                          

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
                                swal("No Record found....!!")
                                $scope.feedetialsGrid = false;                            
                            }

                        }
                        //=========================================== Detailed Graph
                        else if ($scope.feedetailsrdo == "detailed") {
                            if ($scope.getfeedata.length != "0" && $scope.getfeedata != null) {
                                $scope.overallfee = false;
                                $scope.detailsfee = true;

                                $scope.detailedgraph = [];
                                $scope.detailedgraph1 = [];
                                $scope.detailedgraph2 = [];
                                $scope.detailedgraph3 = [];
                                $scope.detailedgraph4 = [];

                                angular.forEach($scope.getfeedata, function (d) {
                                    $scope.detailedgraph.push({ label: d.FEE_HEAD, "y": d.RECEIVABLE })

                                    $scope.detailedgraph1.push({ label: d.FEE_HEAD, "y": d.CONCESSION })

                                    $scope.detailedgraph2.push({ label: d.FEE_HEAD, "y": d.COLLECTION })

                                    $scope.detailedgraph3.push({ label: d.FEE_HEAD, "y": d.ADJUSTMENT })

                                    $scope.detailedgraph4.push({ label: d.FEE_HEAD, "y": d.BALANCE })
                                })

                                var chart = new CanvasJS.Chart("columnchartFee", {
                                    animationEnabled: true,
                                    animationDuration: 3000,
                                    colorSet: "graphcolor",
                                    height: 400,
                                    width: 1086,
                                    axisX: {
                                        interval: 1,
                                        labelFontSize: 12,
                                        labelAngle: -20,
                                    },
                                    axisY: {
                                        labelFontSize: 12,
                                    },
                                    
                                    data: [{
                                        name: "RECEIVABLE",
                                        showInLegend: true,
                                        type: "column",                                       
                                        dataPoints: $scope.detailedgraph
                                    },
                                    {
                                        name: "CONCESSION",
                                        showInLegend: true,
                                        type: "column",                                      
                                        dataPoints: $scope.detailedgraph1
                                    },
                                    {
                                        name: "COLLECTION",
                                        showInLegend: true,
                                        type: "column",                                      
                                        dataPoints: $scope.detailedgraph2
                                    },
                                    {
                                        name: "ADJUSTMENT",
                                        showInLegend: true,
                                        type: "column",                                       
                                        dataPoints: $scope.detailedgraph3
                                    },
                                    {
                                        name: "BALANCE",
                                        showInLegend: true,
                                        type: "column",                                       
                                        dataPoints: $scope.detailedgraph4
                                    }
                                    ]
                                });
                                chart.render();

                                var chart = new CanvasJS.Chart("chartContainerFee", {
                                    animationEnabled: true,
                                    animationDuration: 3000,
                                    colorSet: "graphcolor",
                                    height: 400,
                                    width: 1086,
                                    axisX: {
                                        interval: 1,
                                        labelFontSize: 10,
                                        labelAngle: -20,
                                    },
                                    axisY: {
                                        labelFontSize: 12,
                                    },
                                  
                                    data: [{
                                        name: "RECEIVABLE",
                                        showInLegend: true,
                                        type: "area",                                       
                                        dataPoints: $scope.detailedgraph
                                    },
                                    {
                                        name: "CONCESSION",
                                        showInLegend: true,
                                        type: "area",                                      
                                        dataPoints: $scope.detailedgraph1
                                    },
                                    {
                                        name: "COLLECTION",
                                        showInLegend: true,
                                        type: "area",                                       
                                        dataPoints: $scope.detailedgraph2
                                    },
                                    {
                                        name: "ADJUSTMENT",
                                        showInLegend: true,
                                        type: "area",                                       
                                        dataPoints: $scope.detailedgraph3
                                    },
                                    {
                                        name: "BALANCE",
                                        showInLegend: true,
                                        type: "area",                                        
                                        dataPoints: $scope.detailedgraph4
                                    }
                                    ]
                                });
                                chart.render();
                            }
                            else {
                                swal("No Record found")
                                $scope.feedetialsGrid = false;
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