
(function () {
    'use strict';
    angular
.module('app')
.controller('StaffnStudentReportController', StaffnStudentReportController)

    StaffnStudentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', 'Excel', '$stateParams', '$filter']
    function StaffnStudentReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, Excel, $stateParams, $filter) {
        $scope.editEmployee = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.grid_view = false;

       

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.status = "A";
      $scope.submitted = false;
       $scope.getreport = function () {
           
           $scope.submitted = true;
           if ($scope.myForm.$valid) {
               var data = {
                   "Status": $scope.status,
               }
               apiService.create("StaffnStudentReport/getrpt", data).
                           then(function (promise) {
                               if (promise.returnval === true) {
                                   swal('Staff Name successfully Replaced');
                                   $scope.BindData();
                               }
                               else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                                   swal('Records Doesnot Exist for the selected Staff !');
                               }
                               else {
                                   swal('Data Not Saved !');
                               }
                               $scope.BindData();
                           })
           }
       };
       

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            
            apiService.getDATA("StaffnStudentReport/getdetails").
       then(function (promise) {
           $scope.subject = promise.subdrp;
           $scope.class = promise.clsdrp;
           $scope.Academic = promise.year;
           
       })
        };


        //TO clear  data
        $scope.clearid = function () {
            
            $state.reload();
        };
        

    }

})();