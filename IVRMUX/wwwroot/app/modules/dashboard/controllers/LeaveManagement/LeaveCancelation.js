
(function () {
    'use strict';
    angular
.module('app')
        .controller('LeaveCancelationController', LeaveCancelationController)

    LeaveCancelationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants']
    function LeaveCancelationController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants) {

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }

        $scope.currentPage = 1;
        $scope.coptyright = copty;

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrelT_Status)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

        //if ($scope.gridApi.selection.selectRow) {
        //    $scope.gridApi.selection.selectRow($scope.gridleavestatus.data[0]);
        //}

        //$scope.toggleFullRowSelection = function () {
        //    $scope.gridleavestatus.enableFullRowSelection = !$scope.gridleavestatus.enableFullRowSelection;
        //    $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.OPTIONS);
        //};

        $scope.gridleavestatus = {
            enableCellEditOnFocus: true,
            enableRowSelection: true,
            enableSelectAll: true,
            enableFiltering: true,
            enableCellEdit: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 10,

            columnDefs: [
                { name: 'hrelaP_ApplicationID', displayName: 'App. ID', width: '10%', enableFiltering: false },
                { name: 'hrmE_EmployeeFirstName', displayName: 'Name', width: '20%' },
                { name: 'hrmL_LeaveType', displayName: 'Leave Type', enableCellEdit: false, width: '15%' },
                { name: 'hrelaP_FromDate', displayName: 'From Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy"', width: '10%', enableCellEdit: true, },
                { name: 'hrelaP_ToDate', displayName: 'To Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy"', width: '10%', enableCellEdit: true, },
                { name: 'hrelaP_TotalDays', displayName: 'No. of Days', width: '10%', enableFiltering: false },
                { name: 'createdDate', displayName: 'Applied Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy HH:mm"', width: '15%', enableFiltering: false },
                { name: 'hrelaP_ReportingDate', displayName: 'Reporting', type: 'date', cellFilter: 'date:"dd-MM-yyyy"', width: '10%', enableFiltering: false }
            ]
        };
       
        $scope.gridleavestatus.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;

            gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();
            });
            gridApi.selection.on.rowSelectionChangedBatch($scope, function (row) {
                $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();
            });
        };

        $scope.loadApprovedData = function () {
            var id = 2;
            apiService.getURI("LeaveApproval/getApprovedLeave", id).
                then(function (promise) {
                    $scope.gridleavestatus.data = promise.get_leavestatus;
                    $scope.activityIds = promise.activityIds;
                    if ($scope.count == 0) {
                        swal("Data not Found !!");
                        $scope.ckdept = false;
                    }
                });
        };

        $scope.get_status = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    get_leave_status: $scope.gridApi.selection.getSelectedRows(),
                    "HRELAPA_Remarks": $scope.remarkstxta
                };
                apiService.create("LeaveApproval/get_status", data).
                    then(function (promise) {
                        $scope.gridleavestatus.data = promise.get_leavestatus;
                        swal("Approved...");
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancelationstatus = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    get_leave_status: $scope.gridApi.selection.getSelectedRows(),
                    "HRELAPA_Remarks": $scope.remarkstxta
                };
                apiService.create("LeaveApproval/reject_status", data).
                    then(function (promise) {
                        $scope.gridleavestatus.data = promise.get_leavestatus;
                        swal("Cancelled...");
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    }
})();