(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgAttendanceDetailsController', ClgAttendanceDetailsController)

    ClgAttendanceDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ClgAttendanceDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.attdetails = false;

        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#2471A3",
                "#76D7C4",
                "#DAF7A6",
                "#FFC300",
                "#FF5733",
            ]);
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getDATA("ClgAttendanceDetails/getloaddata").
                then(function (promise) {
                    $scope.yearlist = promise.yearlist;
                    $scope.currentyear = promise.currentyear;

                    angular.forEach($scope.yearlist, function (y) {
                        angular.forEach($scope.currentyear, function (c) {
                            if (y.asmaY_Id == c.asmaY_Id) {
                                $scope.asmaY_Id = c.asmaY_Id;
                            }
                        })
                    })
                    //$scope.onselectAcdYear();
                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }

        //====================Academic Year Selection
        $scope.onselectAcdYear = function (asmaY_Id) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("ClgAttendanceDetails/getAttdata", data).
                then(function (promise) {

                    $scope.attlst = promise.attList;

                    $scope.attmonth = [];
                    $scope.attsubj = [];
                    $scope.attpersent = [];
                    $scope.attclassheld = [];

                    if ($scope.attlst.length != "0" && $scope.attlst != null) {
                        $scope.attdetails = true;

                        angular.forEach($scope.attlst, function (att) {
                            $scope.attmonth.push({ label: att.MONTH_NAME })
                            $scope.attsubj.push({ label: att.ISMS_SubjectName })
                            $scope.attpersent.push({ label: att.ISMS_SubjectName, "y": parseInt(att.TOTAL_PRESENT) })
                            $scope.attclassheld.push({ label: att.ISMS_SubjectName, "y": parseInt(att.CLASS_HELD) })
                        })
                        $scope.newarytm = [];
                        $scope.clspersnt = [];
                        angular.forEach($scope.attlst, function (qw) {
                            $scope.newarytm = [];
                            $scope.newarytm = [qw.TOTAL_PRESENT, qw.CLASS_HELD];
                            $scope.clspersnt = ["TOTAL PRESENT", "CLASS HELD"];
                            qw.na = $scope.newarytm;
                            qw.cp = $scope.clspersnt;
                        })
                        $scope.grddata = [];

                        angular.forEach($scope.attlst, function (ag) {
                            angular.forEach(ag.na, function (attg) {
                                $scope.grddata.push(attg);
                            })
                        })

                        $scope.clsheldpersnt = [];
                        angular.forEach($scope.attlst, function (cg) {
                            angular.forEach(cg.cp, function (cpg) {
                                $scope.clsheldpersnt.push(cpg);
                            })
                        })

                        $scope.clgsubject = [];
                        angular.forEach($scope.attlst, function (cg) {
                            $scope.clgsubject.push(cg.ISMS_SubjectName);
                        })

                        $scope.month = [];
                        var found = false;
                        $scope.month.push($scope.attlst[0].MONTH_NAME);
                        for (var i = 0; i < $scope.attlst.length; i++) {
                            found = false;
                            for (var u = 0; u < $scope.month.length; u++) {
                                if ($scope.month[u] == $scope.attlst[i].MONTH_NAME) {
                                    found = true;}}
                            if (found == false) { $scope.month.push($scope.attlst[i].MONTH_NAME); }
                        }

                        $scope.monthcount = [];
                        var count = 1;
                        var monthName = $scope.attlst[0].MONTH_NAME;
                        for (var i = 1; i < $scope.attlst.length; i++) {
                            if (monthName == $scope.attlst[i].MONTH_NAME) {
                                count++;
                            }
                            else {
                                $scope.monthcount.push({ name: monthName, cnt: count });
                                monthName = $scope.attlst[i].MONTH_NAME;
                                count = 1;
                            }
                        }
                        $scope.monthcount.push({ name: monthName, cnt: count });
                        
                        //===================GRAPH 1
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
                            data: [{
                                name: "TOTAL PERSENT",
                                showInLegend: true,
                                type: "column",
                                dataPoints: $scope.attpersent
                            },
                            {
                                name: "CLASS HELD",
                                showInLegend: true,
                                type: "column",
                                dataPoints: $scope.attclassheld
                            }
                            ]
                        });
                        chart.render();
                        //===================GRAPH 2
                        var chart = new CanvasJS.Chart("linechart", {
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
                            data: [{
                                name: "TOTAL PERSENT",
                                showInLegend: true,
                                type: "area",
                                dataPoints: $scope.attpersent
                            },
                            {
                                name: "CLASS HELD",
                                showInLegend: true,
                                type: "area",
                                dataPoints: $scope.attclassheld
                            }
                            ]
                        });
                        chart.render();
                    }
                    else {
                        swal("No Record Found....!!")
                        $scope.attdetails = false;
                    }

                })
        }
    };
})();

