
(function () {
    'use strict';
    angular
.module('app')
.controller('SalaryAdvanceApprovalController', SalaryAdvanceApprovalController)

    SalaryAdvanceApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter']
    function SalaryAdvanceApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
   
        $scope.select_cat = false;
        $scope.BindData = function () {
            apiService.getDATA("SalaryApprovalFacade/getalldetails").
       then(function (promise) {
           


           $scope.gridOptions.data = promise.griddisplay;

       })
     };
  
    }

})();