(function () {
    'use strict';
    angular
        .module('app')
        .controller('AttendanceDetailsController', AttendanceDetailsController);

    AttendanceDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window'];
    function AttendanceDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.attdetails = false;

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

            apiService.getDATA("AttendanceDetails/getloaddata").
                then(function (promise) {

                    $scope.attyearlst = promise.attyearlist;
                    $scope.currentyar = promise.currentyear;

                    for (var t = 0; t < $scope.attyearlst.length; t++) {
                        if ($scope.attyearlst[t].asmaY_Id === $scope.currentyar[0].asmaY_Id) {
                            $scope.asmaY_Id = $scope.currentyar[t].asmaY_Id;
                        }
                    }
                    $scope.onyearchange();
                });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };


        //---------Academic Year Selection
        $scope.onyearchange = function (asmaY_Id) {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("AttendanceDetails/getAttdata", data).
                then(function (promise) {
                    var chart = {};
                    $scope.attlst = promise.attList;
                    $scope.attgraph = [];
                    $scope.attgraph1 = [];
                    $scope.attgraph2 = [];
                    if ($scope.attlst.length !== "0" && $scope.attlst !== null) {

                        $scope.newarytm = [];
                        $scope.clspersnt = [];
                        angular.forEach($scope.attlst, function (qw) {
                            $scope.newarytm = [];
                            $scope.newarytm = [qw.CLASS_HELD, qw.TOTAL_PRESENT, qw.score];
                            $scope.clspersnt = ["CLASS HELD", "TOTAL PRESENT", "PERCENTAGE"];
                            qw.na = $scope.newarytm;
                            qw.cp = $scope.clspersnt;
                        });
                        $scope.grddata = [];

                        angular.forEach($scope.attlst, function (ag) {
                            angular.forEach(ag.na, function (attg) {
                                $scope.grddata.push(attg);
                            });
                        });

                        $scope.clsheldpersnt = [];
                        angular.forEach($scope.attlst, function (cg) {
                            angular.forEach(cg.cp, function (cpg) {
                                $scope.clsheldpersnt.push(cpg);
                            });
                        });

                        $scope.month = [];
                        var found = false;
                        $scope.month.push($scope.attlst[0].MONTH_NAME);
                        for (var i = 0; i < $scope.attlst.length; i++) {
                            found = false;
                            for (var u = 0; u < $scope.month.length; u++) {
                                if ($scope.month[u] === $scope.attlst[i].MONTH_NAME) {
                                    found = true;
                                }
                            }
                            if (found === false) { $scope.month.push($scope.attlst[i].MONTH_NAME); }
                        }

                        $scope.monthcount = [];
                        var count = 1;
                        var monthName = $scope.attlst[0].MONTH_NAME;
                        for (var i = 1; i < $scope.attlst.length; i++) {
                            if (monthName === $scope.attlst[i].MONTH_NAME) {
                                count++;
                            }
                            else {
                                $scope.monthcount.push({ name: monthName, cnt: count });
                                monthName = $scope.attlst[i].MONTH_NAME;
                                count = 1;
                            }
                        }
                        $scope.monthcount.push({ name: monthName, cnt: count });
                        $scope.attdetails = true;
                        angular.forEach($scope.attlst, function (ag) {
                            $scope.attgraph.push({ label: ag.MONTH_NAME, "y": ag.TOTAL_PRESENT });
                            $scope.attgraph1.push({ "y": parseInt(ag.CLASS_HELD) });
                            $scope.attgraph2.push({ "y": parseInt(ag.score) });
                        });

                        chart = new CanvasJS.Chart("columnchart", {
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
                            data: [{
                                name: "TOTAL PRESENT",
                                showInLegend: true,
                                type: "column",
                                dataPoints: $scope.attgraph
                            },
                            {
                                name: "CLASS HELD",
                                showInLegend: true,
                                type: "column",
                                dataPoints: $scope.attgraph1
                            }
                           
                            ]
                        });
                        chart.render();

                        chart = new CanvasJS.Chart("linechart", {
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
                            data: [{
                                name: "TOTAL PRESENT",
                                showInLegend: true,
                                type: "area",
                                dataPoints: $scope.attgraph
                            },
                            {
                                name: "CLASS HELD",
                                showInLegend: true,
                                type: "area",
                                dataPoints: $scope.attgraph1
                            }
                            ]
                        });
                        chart.render();
                    }
                    else {
                        swal("No Record Found....");
                        $scope.attdetails = false;
                    }

                });
        };
    }
})();

