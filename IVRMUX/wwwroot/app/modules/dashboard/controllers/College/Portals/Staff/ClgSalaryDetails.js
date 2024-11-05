(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgSalaryDetailsController', ClgSalaryDetailsController)

    ClgSalaryDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ClgSalaryDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {
      
        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#34495E",
                "#85C1E9",
                "#DAF7A6",
                "#FFC300",
                "#FF5733",
            ]);
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getDATA("ClgSalaryDetails/getloaddata").
                then(function (promise) {
                    $scope.yearlist = promise.yearlist;
                    $scope.onselectYear();
                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }

        $scope.submitted = false;
        //====================Academic Year Selection
        $scope.onselectYear = function (hrmlY_LeaveYear) {
            $scope.submitted = true;

            if ($scope.hrmlY_LeaveYear !== undefined) {
                var data = {
                    "HRMLY_LeaveYear": $scope.hrmlY_LeaveYear
                };
                apiService.create("ClgSalaryDetails/getSalary", data).
                    then(function (promise) {
                        $scope.sal_graph = [];
                        $scope.salary_list = promise.salarylist;
                        $scope.salaryEarningDlist = promise.salaryEarningDlist;

                        if ($scope.salary_list.length > 0) {
                            //======================= Net Salary
                            $scope.netSalaryarray = [];
                            var netSlry = "";

                            angular.forEach($scope.salary_list, function (er) {
                                angular.forEach($scope.salaryEarningDlist, function (de) {
                                    if (er.hres_id == de.hres_id) {
                                        netSlry = er.salary - de.salary;
                                        $scope.netSalaryarray.push({ hres_id: er.hres_id, salary: netSlry, month: er.monthName, hreS_Year: er.hreS_Year })
                                        netSlry = "";
                                    }
                                })
                            })

                            if ($scope.salary_list != null) {
                                for (var i = 0; i < $scope.salary_list.length; i++) {
                                    $scope.sal_graph.push({ label: $scope.salary_list[i].monthName, "y": $scope.salary_list[i].salary })
                                }
                            }

                            var chart = new CanvasJS.Chart("columnchart", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [
                                    {
                                        name: "SALARY",
                                        showInLegend: true,
                                        type: "column",
                                        dataPoints: $scope.sal_graph
                                    }
                                ]
                            });
                            chart.render();

                            var chart = new CanvasJS.Chart("linechart", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [
                                    {
                                        name: "SALARY",
                                        type: "pie",
                                        showInLegend: true,
                                        dataPoints: $scope.sal_graph
                                    }
                                ]
                            });
                            chart.render();
                            $scope.hreS_Year = promise.salarylist[0].hreS_Year;
                        }
                        else {
                            swal("No Record Found....!!");
                            $scope.salary_list = "";
                        }
                    });
            }
            else {
                swal("Please select Year!!");
                $scope.salary_list = "";
            }
        };

        //====================Salary Detail Model Selection
        $scope.showSalaryGrid = function (data) {

            var data = {
                "HRES_Id": data.hres_id
            };
            apiService.create("ClgSalaryDetails/getsalaryalldetails/", data)
                .then(function (promise) {

                    $scope.SalaryE = promise.salarylistE;
                    $scope.SalaryD = promise.salarylistD;
                    $scope.TotalEarning = promise.totalEarning[0].salary;
                    $scope.totalDeduction = promise.totalDeduction[0].salary;
                    $scope.NetSalary = $scope.TotalEarning - $scope.totalDeduction;
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    };
})();

