﻿(function () {
    'use strict';
    angular
.module('app')
.controller('masterMaritalStatusController', masterMaritalStatusController)

    masterMaritalStatusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterMaritalStatusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.Marital = {};


        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ivrmmmS_MaritalStatus', displayName: 'Marital Status', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.ivrmmmS_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                        '<span ng-if="row.entity.ivrmmmS_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getURI("MasterMaritalStatus/getalldetails", pageid).then(function (promise) {


                if (promise.maritalstatusList !== null && promise.maritalstatusList.length > 0) {

                    $scope.gridOptions.data = promise.maritalstatusList;
                    $scope.gridOptions = promise.maritalstatusList;
                }
            })
        }

       
        // clear form data
        $scope.cancel = function () {
            // $scope.search = "";
            $scope.Marital = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
        }

        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = $scope.Marital;

                apiService.create("MasterMaritalStatus/", data).
                then(function (promise) {
                    if (promise.retrunMsg !== "") {

                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }

                        else if (promise.retrunMsg == "false") {
                            swal("Record Not saved / Updated..", 'Fail');

                        }
                        else if (promise.retrunMsg == "Add") {
                            swal("Record Saved Successfully..");
                        }
                        else if (promise.retrunMsg == "Update") {
                            swal("Record Updated Successfully..");
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }

                        if (promise.maritalstatusList !== null && promise.maritalstatusList.length > 0) {

                            $scope.gridOptions.data = promise.maritalstatusList;
                        }
                        $state.reload();
                    }
                })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        // Edit Single Record
        $scope.EditData = function (record) {

            var id = record.ivrmmmS_Id;
            apiService.getURI("MasterMaritalStatus/editRecord", id).
                then(function (promise) {
                    if (promise.maritalstatusList != null && promise.maritalstatusList.length > 0) {
                        $scope.Marital = promise.maritalstatusList[0];
                    }
                   
                })
        }


        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.ivrmmmS_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.getURI("MasterMaritalStatus/ActiveDeactiveRecord", data.ivrmmmS_Id).
                   then(function (promise) {


                       if (promise.retrunMsg !== "") {

                           if (promise.retrunMsg === "Activated") {
                               swal("Record Activated successfully");
                               $state.reload();
                           }
                           else if (promise.retrunMsg === "Deactivated") {
                               swal("Record Deactivated successfully");
                               $state.reload();
                           }
                           else {
                               swal("Record Not Activated/Deactivated", 'Fail');
                           }

                          
                       }

                   })
               }
               else {
                   swal(" Cancelled", "Ok");
               }
           }

           );
        }
    }

})();