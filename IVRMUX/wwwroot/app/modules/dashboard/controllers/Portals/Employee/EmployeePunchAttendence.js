(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeePunchAttendenceController', EmployeePunchAttendenceController)

    EmployeePunchAttendenceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function EmployeePunchAttendenceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.maxDatemf = new Date();
        //loading start

        $scope.loadData = function () {

            var id = 2;
            apiService.getURI("EmployeePunchAttendence/getalldetails/", id).
                then(function (promise) {


                    $scope.empDetails = promise.filldepartment;
                    $scope.Fname = $scope.empDetails[0].empFname;
                    $scope.Mname = $scope.empDetails[0].empMname;
                    $scope.Lname = $scope.empDetails[0].empLname;
                    $scope.HRME_DOJ = $scope.empDetails[0].hrmE_DOJ
                    $scope.HRMD_DepartmentName = $scope.empDetails[0].hrmD_DepartmentName;
                    $scope.HRMDES_DesignationName = $scope.empDetails[0].hrmdeS_DesignationName;
                    $scope.grid_show = false;
                })

        };
        //loading end
        $scope.gettodate = function () {

            var fromdate1 = $scope.fromdate == null ? "" : $filter('date')($scope.fromdate, "yyyy-MM-dd");
            var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
            //var groupidss = $scope.hrmE_Id;
            var data = {
                "fromdate": fromdate1,
                "todate": todate1,
            }


            apiService.create("EmployeePunchAttendence/getrpt/", data).
                then(function (promise) {



                    if (promise.emp_punchDetails !== null && promise.emp_punchDetails.length > 0) {
                        $scope.filldata = promise.emp_punchDetails;
                        $scope.grid_show = true;

                    }
                    else {
                        $scope.grid_show = false;
                        swal("No Record Found")
                    }

                }
                )
        };


    }
})();