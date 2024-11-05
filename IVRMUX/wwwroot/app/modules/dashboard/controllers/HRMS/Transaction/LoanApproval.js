(function () {
    'use strict';
    angular
.module('app')
.controller('LoanApprovalController', LoanApprovalController)
    LoanApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function LoanApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.select_cat = false;
        // Get form Details at onload
        $scope.onLoadGetData = function () {

            var pageid = 2;
            apiService.getURI("LoanApproval/getalldetails", pageid).then(function (promise) {
                if (promise.griddisplay !== null && promise.griddisplay.length > 0) {
                    $scope.gridOptions.data = promise.griddisplay;
                }
            })
        }

        // Datatable display
        $scope.gridOptions = {

            enableCellEditOnFocus: true,
            enableRowSelection: true,
            enableSelectAll: true,
            //enableFiltering: true,
            enableCellEdit: false,
            //paginationPageSizes: [5, 10, 15],
            //paginationPageSize: 10,



            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
        
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, width: '15%', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmE_EmployeeFirstName', displayName: 'Employee Name', enableHiding: false, width: '40%' },
                { name: 'hreL_LoanAmount', displayName: 'Loan Amount', enableCellEdit: false, width: '20%' },
                { name: 'hreL_LoanInsallments', displayName: 'Loan Insallments', width: '20%', enableCellEdit: true, },
                //{
                //   field: 'id', name: '',
                //   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                // '<div class="grid-action-cell">' +
                // '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                // '<a ng-if="row.entity.hreL_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                //  '<span ng-if="row.entity.hreL_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                // '</div>'
                //} 
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;

            gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();
            });
            gridApi.selection.on.rowSelectionChangedBatch($scope, function (row) {
                $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();

            });
        }

        $scope.get_status = function () {
            swal("Loan Approved !!");
        }

        $scope.reject_status = function () {
            swal("Loan Rejected !!");
        }


    }
})();

