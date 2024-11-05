(function () {
    'use strict';
    angular
.module('app')
.controller('LoanLetterRequestController', LoanLetterRequestController)

    LoanLetterRequestController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function LoanLetterRequestController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {
        //$scope.editEmployee = {};
        //$scope.selected = {};
        //$scope.obj = {};

        $scope.loadData = function () {
            
            var id = 2;

            apiService.getURI("LoanLetterRequest/getalldetails", id).
       then(function (promise) {
           


           $scope.employeevalue = promise.employeedropdown;
           $scope.yearvalue = promise.leaveyeardropdown;
           $scope.monthvalue = promise.monthdropdown;
           $scope.loanvalue = promise.loandrop;
           //   $scope.gradelist = promise.gradedropdownlist;
           //  $scope.approve = promise.approveid;
           // $scope.Designation_types = promise.designation_types;
           
           $scope.gridAuth.data = promise.gridoption;
           console.log(promise.gridoption);
           // $scope.getauthdata();
           //console.log($scope.yearvalue);

       })


        };



        $scope.gridAuth = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'hrelT_Year', displayName: 'Year', enableFiltering: true },
                { name: 'hrelT_Month', displayName: 'Month', enableFiltering: true },
                { name: 'hrelT_Reason', displayName: 'Reason', enableFiltering: true },
                { name: 'hrmE_EmployeeFirstName', displayName: 'EmployeeName', enableFiltering: true },
           
                //{
                //    field: 'id', name: '', displayName: 'Actions', enableFiltering: false, enableSorting: false,
                //    //cellTemplate: '<div class="grid-action-cell">' +
                //    //              '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);" data-toggle="tooltip" title="Edit" > <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                //    //              '<a href="javascript:void(0)" ng-click="grid.appScope.deletedataY(row.entity);" data-placement="bottom" data-toggle="tooltip" title="Delete"> <i class="fa fa-trash"></i></a> &nbsp; &nbsp;' +
                //    //              '<a ng-if="row.entity.HRML_LeaveCreditFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                //    //              '<span ng-if="row.entity.HRML_LeaveCreditFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                //    //              '</div>'
                //}
            ]
        };



        $scope.saveauthdata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
                var HRELT_Year = "";
                var HRELT_Month = "";
                angular.forEach($scope.yearvalue, function (yt) {
                    if(yt.hrmlY_Id== $scope.HRMLY_Id)
                    {
                        HRELT_Year = yt.hrmlY_LeaveYear;
                    }
                })
                angular.forEach($scope.monthvalue, function (yt) {
                    if (yt.ivrM_Month_Id == $scope.IVRM_Month_Id) {
                        HRELT_Month = yt.ivrM_Month_Name;
                    }
                })
                var data = {
                    "HRELT_Id": $scope.HRELT_Id,
                    "HRME_Id": $scope.hrmE_Id,
                    "HRMLN_Id": $scope.hrmlN_Id,
                    "HRELT_Year": HRELT_Year,
                    "HRELT_Month": HRELT_Month,
                    "HRELT_Reason": $scope.HRELT_Reason
                }
                apiService.create("LoanLetterRequest/", data).then(function (promise) {

                    if (promise.returnval == true) {
                        swal('Record Saved/Updated Successfully');

                    }
                    else if (promise.retrunMsg == "Duplicate") {
                        swal("Records Already Exist!!!");
                    }
                    else {
                        swal('Failed to Save/Update record');
                    }
                    $scope.loadData();
                })
            }
            else {
                $scope.submitted = true;
            }

        };

        $scope.get_loans = function (stf_id) {
            
                var data = {
                    "HRME_Id": $scope.hrmE_Id
                }
                apiService.create("LoanLetterRequest/get_loans", data).then(function (promise) {
                    $scope.loanvalue = promise.loandrop;
                    $scope.hrmlN_Id = "";
                
                })
           

        };


        $scope.cleardata = function () {
            $scope.HRELT_Reason = '';
            $scope.loadData();

        }

    }
})();