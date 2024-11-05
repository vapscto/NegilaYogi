(function () {
    'use strict';
    angular
.module('app')
.controller('PDARefundableReportController', PDARefundableReportController)
    PDARefundableReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function PDARefundableReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("PDARefundableReport/getalldetails", pageid).
        then(function (promise) {
            $scope.acdyr = promise.fillyear;
            $scope.classcount = promise.classlist;
            $scope.sectioncount = promise.searcharray;
        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }






    }
})();
