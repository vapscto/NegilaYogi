
(function () {
    'use strict';
    angular
        .module('app')
        .controller('PeriodWiseLeaveApprovalController', PeriodWiseLeaveApprovalController)

    PeriodWiseLeaveApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', '$sce']
    function PeriodWiseLeaveApprovalController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, $sce) {

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.currentPage = 1;
        $scope.coptyright = copty;

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrelT_Status)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

   



        $scope.toggleAll = function () {
            var checkStatus = $scope.all;
            angular.forEach($scope.gridleavestatus, function (itm) {
                itm.selected = checkStatus;
            });
        }



        $scope.isOptionsRequired = function () {
            return !$scope.gridleavestatus.some(function (options) {
                return options.selected;
            });
        };

        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("LeaveApproval/getperiodApprovalStatus", id).then(function (promise) {   
                $scope.gridleavestatus = promise.periodwiseApproval;
                $scope.temp_leave = promise.get_leavestatus;
                $scope.approvalstatus = promise.approvalstatus;
                if ($scope.count == 0) {
                    swal("Data not Found !!");
                    $scope.ckdept = false;
                }
            });
                    
        };
        
     
        $scope.get_status = function (status) {
            $scope.selected_Inst = [];
            if ($scope.myForm.$valid) {

                angular.forEach($scope.gridleavestatus, function (ty) {
                    if (ty.selected === true) {
                        $scope.selected_Inst.push(ty);
                    }
                })

                var data = {    
                    get_leave_status: $scope.selected_Inst,
                    "HRELAPDD_Remarks": $scope.remarkstxta,  
                    HRELAPDD_ApprovalFlg : status,
                };
                apiService.create("LeaveApproval/periodleavestatus", data). then(function (promise) {                     

                    if (promise.returnmsg === 'Approved') {
                            swal("Record Approved Successfully");
                            $state.reload();
                        }
                    else if (promise.returnmsg === 'Rejected') {
                            swal("Record Rejected Successfully");
                            $state.reload();
                        }
                        else {
                        swal("Record Not  "+ returnmsg +"  Successfully");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

    }
})();