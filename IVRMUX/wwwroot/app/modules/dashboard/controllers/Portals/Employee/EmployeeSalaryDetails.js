(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeSalaryDetailsController', EmployeeSalaryDetailsController)

    EmployeeSalaryDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']

    function EmployeeSalaryDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {



        $scope.LoadData = function () {

            $scope.showweekly = false;
            $scope.showday_d = true;
            apiService.getDATA("EmployeeSalaryDetails/getalldetails")
                .then(function (promise) {

                    $scope.yearL = promise.yearlist;



                })


            $scope.onselectgroup = function () {

                var year = $scope.HRMLY_LeaveYear;
                var data = {
                    "HRMLY_LeaveYear": year
                }
                var sal_list = [];

                apiService.create("EmployeeSalaryDetails/getdaily_data", data).
                    then(function (promise) {
                      
                        $scope.salary_list = promise.salarylist;
                        $scope.salaryEarningDlist = promise.salaryEarningDlist;

                        if ($scope.salary_list.length > 0) {
                            $scope.salaryD = true;
                        } else {
                            $scope.salaryD = false;
                            swal("Employee did't have any Salary details....!!")
                        }
                        //=======================
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
                        //=================change part

                        if ($scope.netSalaryarray != null) {

                            for (var i = 0; i < $scope.netSalaryarray.length; i++) {
                                sal_list.push({ label: $scope.netSalaryarray[i].month, "y": $scope.netSalaryarray[i].salary })
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
                                    showInLegend: false,
                                    type: "pie",
                                    dataPoints: sal_list
                                }
                            ]
                        });

                        chart.render();

                        //-------------------Attendance Graph   
                        var chart = new CanvasJS.Chart("chartContainer", {
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
                                    type: "column",
                                    showInLegend: false,
                                    dataPoints: sal_list
                                }
                            ]
                        });
                        chart.render();

                        $scope.hreS_Year = promise.salarylist[0].hreS_Year;


                        //if ($scope.salary_list != null) {

                        //    for (var i = 0; i < $scope.salary_list.length; i++) {
                        //        sal_list.push({ label: $scope.salary_list[i].monthName, "y": $scope.salary_list[i].salary })
                        //    }
                        //}

                        //var chart = new CanvasJS.Chart("columnchart", {
                        //    animationEnabled: true,
                        //    animationDuration: 3000,
                        //    height: 350,
                        //    colorSet: "graphcolor",
                        //    axisX: {
                        //        labelFontSize: 13,
                        //    },
                        //    axisY: {
                        //        labelFontSize: 13,
                        //    },

                        //    toolTip: {
                        //        shared: true
                        //    },
                        //    data: [
                        //        {
                        //            showInLegend: true,
                        //            type: "pie",
                        //            dataPoints: sal_list
                        //        }
                        //    ]
                        //});

                        //chart.render();

                        ////-------------------Attendance Graph   
                        //var chart = new CanvasJS.Chart("chartContainer", {
                        //    animationEnabled: true,
                        //    animationDuration: 3000,
                        //    height: 350,
                        //    colorSet: "graphcolor",
                        //    axisX: {
                        //        labelFontSize: 13,
                        //    },
                        //    axisY: {
                        //        labelFontSize: 13,
                        //    },

                        //    toolTip: {
                        //        shared: true
                        //    },
                        //    data: [
                        //        {
                        //            type: "column",
                        //            showInLegend: true,
                        //            dataPoints: sal_list
                        //        }
                        //    ]
                        //});
                        //chart.render();

                        //$scope.hreS_Year = promise.salarylist[0].hreS_Year;
                    })

            };

            $scope.showSalaryGrid = function (data) {

                $scope.HRES_Id = data.hres_id;
                apiService.getURI("EmployeeSalaryDetails/getsalaryalldetails/", $scope.HRES_Id)
                    .then(function (promise) {

                        $scope.SalaryE = promise.salarylistE;
                        $scope.SalaryD = promise.salarylistD;
                        $scope.TotalEarning = promise.totalEarning[0].salary;
                        $scope.totalDeduction = promise.totalDeduction[0].salary;
                        $scope.NetSalary = $scope.TotalEarning - $scope.totalDeduction
                    })
            }
        };
    };
})();