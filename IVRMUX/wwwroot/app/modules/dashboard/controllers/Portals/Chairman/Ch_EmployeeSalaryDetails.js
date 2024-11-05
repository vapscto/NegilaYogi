(function () {
    'use strict';
    angular
.module('app')
.controller('Ch_EmployeeSalaryDetailsController', Ch_EmployeeSalaryDetailsController)

    Ch_EmployeeSalaryDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    //dashboard.controller("EmployeeDashboardController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache',
    function Ch_EmployeeSalaryDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {

        $scope.salary_list = [];
        $scope.yearL = [];
        $scope.sal_list = [];
        $scope.searchValue = "";
        $scope.LoadData = function () {
           
            $scope.showweekly = false;
            $scope.showday_d = true;
            apiService.getDATA("Ch_EmployeeSalaryDetails/getalldetails")
                .then(function (promise) {
                    
                    $scope.yearL = promise.yearlist;
                    $scope.hrmlY_LeaveYear = promise.hrmlY_LeaveYear;
                    $scope.salary_list = promise.salarylist;
                    if ($scope.salary_list != null && $scope.salary_list != 0) {
                        angular.forEach($scope.salary_list, function (ee) {

                            ee.salary = Math.round(parseFloat(ee.salary), 2);

                        })
                        $scope.salaryD = true;
                        $scope.loadchart();

                    } else {
                        $scope.salaryD = false;

                        swal('No Salary details for Financial Year :!!' +  $scope.hrmlY_LeaveYear +'')
                    }

                })

          
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
            $scope.interacted = function (field) {
                return $scope.submitted;
            };

            $scope.onselectgroup = function (hrmlY_LeaveYear) {
                
                $scope.sal_list = [];
                $scope.salaryD = false;
                if ($scope.myForm.$valid) {
                var data = {
                    "HRMLY_LeaveYear": hrmlY_LeaveYear
                }
               
               
                apiService.create("Ch_EmployeeSalaryDetails/onyearchange", data).
           then(function (promise) {

               $scope.yearL = promise.yearlist;
               $scope.hrmlY_LeaveYear = promise.hrmlY_LeaveYear;
               $scope.salary_list = promise.salarylist;
               if ($scope.salary_list != null && $scope.salary_list != 0) {

                   angular.forEach($scope.salary_list, function (ee) {

                       ee.salary = Math.round(parseFloat(ee.salary),2);

                   })


                   $scope.salaryD = true;
                   $scope.loadchart();

               } else {
                   $scope.salaryD = false;

                   swal('No Salary details for Financial Year :!!' + $scope.hrmlY_LeaveYear + '')
               }

           

           })
                }
                else {
                    $scope.submitted = true;
                }
            };

            $scope.loadchart = function () {
                if ($scope.salary_list != null) {

                    for (var i = 0; i < $scope.salary_list.length; i++) {
                        $scope.sal_list.push({ label: $scope.salary_list[i].monthName, "y": $scope.salary_list[i].salary })
                    }
                }

                var chart = new CanvasJS.Chart("columnchart", {
                    height: 350,
                    width: 1075,
                    axisX: {
                        labelFontSize: 12,
                        labelAngle: -20 
                    },
                    axisY: {
                        labelFontSize: 12,
                    },

                    data: [
                    {
                        type: "column",
                        showInLegend: true,
                        dataPoints: $scope.sal_list
                    }
                    ]
                });

                chart.render();
            }


            $scope.showSalaryGrid = function (monthName) {
              
                var data = {
                    "HRMLY_LeaveYear": $scope.hrmlY_LeaveYear,
                    "monthName": monthName
                }

              
                apiService.create("Ch_EmployeeSalaryDetails/onmonth/", data)
               .then(function (promise) {

                   $scope.deptsalary = promise.deptsalary;

                   angular.forEach($scope.deptsalary, function (ee) {

                       ee.salary = Math.round(parseFloat(ee.salary), 2);

                   })


               })
            }

           
        };

        

    };
})();