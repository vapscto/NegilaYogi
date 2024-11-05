(function () {
    'use strict';
    angular
.module('app')
.controller('EmployeeStudentDetailsController', EmployeeStudentDetailsController)

    EmployeeStudentDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    //dashboard.controller("EmployeeDashboardController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache',
    function EmployeeStudentDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {



        $scope.LoadData = function () {
            
            apiService.getDATA("EmployeeStudentDetails/getalldetails")
              .then(function (promise) {
                   
                  //$scope.classDropdown = promise.classlist;
                  $scope.yearDropdown = promise.academicList;
                  //$scope.sectionDropdown = promise.sectionList;
                  //$scope.studentDropdown = promise.studentList;


              })
        };


        $scope.get_class = function () {
            
            // var sal_list = [];
            // var TT_list = [];
            var data={
                "ASMAY_Id":$scope.asmaY_Id
            }
            apiService.create("EmployeeStudentDetails/get_class", data)
                .then(function (promise) {
                    //$scope.Month_list = promise.monthName;
                    $scope.classDropdown = promise.classlist;
                    // $scope.yearDropdown = promise.academicList;



                })
        }
        $scope.get_section = function () {
            
            // var sal_list = [];
            // var TT_list = [];
            var data = {
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("EmployeeStudentDetails/get_section", data)
                .then(function (promise) {
                    //$scope.Month_list = promise.monthName;
                    //$scope.classDropdown = promise.classlist;
                    $scope.sectionDropdown = promise.sectionList;
                    // $scope.yearDropdown = promise.academicList;



                })
        }
        $scope.get_student = function () {
            
            
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("EmployeeStudentDetails/get_student", data)
                .then(function (promise) {
                    //$scope.Month_list = promise.monthName;
                    //$scope.classDropdown = promise.classlist;
                    //$scope.sectionDropdown = promise.sectionList;
                    $scope.studentDropdown = promise.studentList;
                    // $scope.yearDropdown = promise.academicList;



                })
        }

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
})();