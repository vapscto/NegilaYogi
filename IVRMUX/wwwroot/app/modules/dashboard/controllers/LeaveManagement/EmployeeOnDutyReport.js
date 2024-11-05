(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeOnDutyReportController', EmployeeOnDutyReportController)

    EmployeeOnDutyReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function EmployeeOnDutyReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

       

        $scope.onLoadGetData = function () {
            var pageid = 2;

            $scope.all_check_empl = function () {
                var checkStatus = $scope.empl;
                var count = 0;
                angular.forEach($scope.employeedropdown, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });
            }
            $scope.isOptionsRequired3 = function () {
                return !$scope.employeedropdown.some(function (options) {
                    return options.selected;
                });
            }

            $scope.addColumn4 = function () {

                $scope.empl = $scope.employeedropdown.every(function (options) {
                    return options.selected;
                });
            }


            apiService.getURI("EmployeeOnDutyReport/getalldetails", pageid).then(function (promise) {

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                } Empreport

            })
        }




        $scope.Onchangefromdate = function () {
            var fromyear = $scope.myDate_from.getFullYear();
            $scope.todatemax = new Date(
                fromyear,
                11,
                31);
        };

        $scope.compdate = function () {
            var a = new Date($scope.myDate_from);
            var b = new Date($scope.myDate_to);

            if (a <= b) {
                return true;
            }
            else {
                swal("To Date must be greater then From selected Date !!!!");
            }
        };
       


        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            $scope.employeelist = [];
            if ($scope.allind == 'Indi') {
                angular.forEach($scope.employeedropdown, function (em) {
                    if (em.selected === true) {
                        $scope.employeelist.push(em);
                    }
                });
            } else {
                $scope.employeelist = $scope.employeedropdown;
            }


            if ($scope.myForm.$valid) {

                var data = {
                    "employeelist": $scope.employeelist,
                    "HRELAP_FromDate": new Date($scope.myDate_from).toDateString(),
                    "HRELAP_ToDate": new Date($scope.myDate_to).toDateString(),
                }
                apiService.create("EmployeeOnDutyReport/getEmployeedetailsBySelection", data).then(function (promise) {

                    if (promise.empreport.length > 0 && promise.empreport !== null) {
                        $scope.empreport = promise.empreport;
                    }
                    else if ($scope.empreport == null || $scope.empreport.length == 0) {
                        swal("No Data Found !!");
                       
                    }
                    else {
                        $scope.submitted = true;
                    }
                })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
           
        /////////////// Clear
        $scope.cleardata = function () {
            $state.reload();

        };               
    }
})();