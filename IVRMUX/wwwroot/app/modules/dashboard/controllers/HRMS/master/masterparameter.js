(function () {
    'use strict';
    angular
.module('app')
.controller('masterparameterController', masterparameterController)

    masterparameterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterparameterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.Bank = {};

       
        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'hR_Emp_As_parameter', displayName: 'Feedback Parameter', enableHiding: false },
              { name: 'hR_Emp_As_parameterdesc', displayName: 'Description', enableHiding: false },
             
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                 '<a ng-if="row.entity.hR_Emp_Assparameter_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                  '<span ng-if="row.entity.hR_Emp_Assparameter_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
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
            apiService.getURI("masterparameter/getalldetails", pageid).then(function (promise) {


                if (promise.bankdetailList !== null && promise.bankdetailList.length > 0) {
                    //$scope.currentPage = 1;
                    //$scope.itemsPerPage = 10;

                    $scope.gridOptions.data = promise.bankdetailList;
                }
            })
        }

        //// Sort table data
        //$scope.sort = function (keyname) {
        //    $scope.sortKey = keyname;   //set the sortKey to the param passed
        //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        //}

        // clear form data
        $scope.cancel = function () {
            // $scope.search = "";
            $scope.Bank = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
        }

        //saving/updating Record
        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HR_Emp_As_parameter": $scope.Bank.hembD_BankName,
                    "HR_Emp_As_parameterdesc": $scope.Bank.hrmbD_BranchName,
                    "HR_Emp_As_paraid": $scope.Bank.hR_Emp_As_paraid,
                }

                apiService.create("masterparameter/", data).
                then(function (promise) {
                    if (promise.retrunMsg !== "") {

                        
                        if (promise.retrunMsg == "AllDuplicate") {
                            swal("Record already exist..!!");
                            return;
                        } else if (promise.retrunMsg == "Duplicate") {
                            swal("Bank Name already exist..!!");
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
                        else if (promise.retrunMsg == "acc") {
                            swal("Account No. is already exist..");
                            return;
                        }
                        else if (promise.retrunMsg == "branch") {
                            swal("Branch Name already exist..!!");
                            return;
                        }
                      

                        if (promise.bankdetailList !== null && promise.bankdetailList.length > 0) {

                            $scope.gridOptions.data = promise.bankdetailList;
                        }
                        $scope.cancel();
                    }
                })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        // Edit Single Record
        $scope.EditData = function (record) {
            
            var id = record.hR_Emp_As_paraid;
            apiService.getURI("masterparameter/editRecord", id).
                then(function (promise) {

                    if (promise.bankdetailList != null && promise.bankdetailList.length > 0) {
                        $scope.Bank.hembD_BankName = promise.bankdetailList[0].hR_Emp_As_parameter;
                        $scope.Bank.hrmbD_BranchName = promise.bankdetailList[0].hR_Emp_As_parameterdesc;
                        $scope.Bank.hR_Emp_As_paraid = promise.bankdetailList[0].hR_Emp_As_paraid;
                    }
                      

                })
        }


        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.HR_Emp_Assparameter_ActiveFlag == false) {
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
                   apiService.DeleteURI("masterparameter/ActiveDeactiveRecord", data.hR_Emp_As_paraid).
                   then(function (promise) {


                       if (promise.retrunMsg !== "") {

                           if (promise.retrunMsg === "Activated") {
                               swal("Record Activated successfully");
                           }
                           else if (promise.retrunMsg === "Deactivated") {
                               swal("Record Deactivated successfully");
                           }
                           else {
                               swal("Record Not Activated/Deactivated", 'Fail');
                           }

                           if (promise.bankdetailList !== null && promise.bankdetailList.length > 0) {
                              
                               $scope.gridOptions.data = promise.bankdetailList;

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