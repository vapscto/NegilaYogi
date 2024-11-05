(function () {
    'use strict';
    angular
.module('app')
.controller('SalaryApprovalController', SalaryApprovalController)
    SalaryApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function SalaryApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.select_cat = false;
        // Get form Details at onload 
        $scope.onLoadGetData = function () {

            var pageid = 2;
            apiService.getURI("SalaryApproval/getalldetails", pageid).then(function (promise) {
                
                 if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                $scope.gridOptions.data = promise.monthdropdown;
                   // $scope.gridOptions.data = promise.monthdropdown;
                 }

                 if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                     $scope.gridOptions.data = promise.leaveyeardropdown;
                 }

                 if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                     $scope.gridOptions.data = promise.employeedropdown;
                 }
            })
        }


        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'hrmlY_LeaveYear', displayName: 'Year', enableHiding: false },
              { name: 'ivrM_Month_Name', displayName: 'Month', enableHiding: false },
               //{
               //    field: 'id', name: '',
               //    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
               //  '<div class="grid-action-cell">' +
               //  '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
               //  //'<a ng-if="row.entity.hreL_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
               //  // //'<span ng-if="row.entity.hreL_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
               //  '</div>'
               //}
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

    }
})();

