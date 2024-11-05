(function () {
    'use strict';
    angular
        .module('app')
        .controller('Transferred_Employee_Details', Transferred_Employee_Details);
    Transferred_Employee_Details.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache'];
    function Transferred_Employee_Details($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {


        $scope.showvalue = false;
        $scope.getreport = function () {
            var data = {
                "FormatType": $scope.allind
            };

            apiService.create("Transferred_Employee_Details/getvalue", data).then(function (promise) {
                if (promise.employeeDetails != undefined && promise.employeeDetails != null) {
                    $scope.employeeDetails = promise.employeeDetails;
                    if (promise.employeeDetails.length > 0) {
                        $scope.showvalue = true;
                    } else {
                        $scope.showvalue = false;
                    }
                }
            });
        };
    }
})();