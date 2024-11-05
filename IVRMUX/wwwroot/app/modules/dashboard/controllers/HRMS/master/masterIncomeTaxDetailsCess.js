(function () {
    'use strict';
    angular
.module('app')
.controller('masterIncomeTaxDetailsCessController', masterIncomeTaxDetailsCessController)

    masterIncomeTaxDetailsCessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterIncomeTaxDetailsCessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.IncTaxDetailCess = {};

        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                        {
                            name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>'
                        },
                        {
                            name: 'hrmiT_AgeFlag', displayName: 'Income Tax Name', enableHiding: false
                        },
                        {
                            name: 'hrmitC_CessName', displayName: 'Income Tax Cess', enableHiding: false
                        },
                        {
                            name: 'hrmitdC_Amount', displayName: 'Amount', enableHiding: false
                        },
                        {
                            field: 'id', name: '',
                            displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                            '<div class="grid-action-cell">' +
                            '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                            '<a ng-if="row.entity.hrmitdC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                            '<span ng-if="row.entity.hrmitdC_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                            '</div>'
                        }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };
        // Get form Details at onload 
        
        $scope.onloadGetData = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getURI("MasterIncomeTaxDetailsCess/getalldetails", pageid).then(function (promise) {
                if (promise.incomeTaxDetailsCessList!==null && promise.incomeTaxDetailsCessList.length > 0) {
                   // $scope.gridOptions.data = promise.incomeTaxDetailsCessList;
                    $scope.gridOptions = promise.incomeTaxDetailsCessList;
                }
                //DROP DOWN 
                $scope.cessnamedropdown = promise.cessnamedropdown;

                $scope.incomeTaxList = promise.incomeTaxList;
            })
        }
        // clear form data
        $scope.cancel = function () {
            $scope.IncTaxDetailCess = {};
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
                var data = {
                    "HRMITC_Id": $scope.IncTaxDetailCess.HRMITC_Id,
                   // "HRMITD_AmountFrom": $scope.IncTaxDetail.HRMITD_AmountFrom,
                 //   "HRMITD_AmountTo": $scope.IncTaxDetail.HRMITD_AmountTo,
                    "HRMITDC_Amount": $scope.IncTaxDetailCess.HRMITDC_Amount,
                    "HRMITD_Id": $scope.IncTaxDetailCess.HRMIT_Id
                }
                apiService.create("MasterIncomeTaxDetailsCess/", data).
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

                        if (promise.incomeTaxDetailsCessList!==null && promise.incomeTaxDetailsCessList.length > 0) {

                            $scope.gridOptions.data = promise.incomeTaxDetailsCessList;
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

            var id = record.hrmitdC_Id;
            apiService.getURI("MasterIncomeTaxDetailsCess/editRecord", id).
                then(function (promise) {
                  

                    if (promise.incomeTaxDetailsCessList != null && promise.incomeTaxDetailsCessList.length > 0) {
                        $scope.IncTaxDetailCess.HRMITC_Id = promise.incomeTaxDetailsCessList[0].hrmitC_Id;
                       
                        $scope.IncTaxDetailCess.HRMIT_Id = promise.incomeTaxDetailsCessList[0].hrmiT_Id;
                        $scope.IncTaxDetailCess.HRMITDC_Amount = promise.incomeTaxDetailsCessList[0].hrmitdC_Amount;
                      
                  
                    }

                })
        }
        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            if (data.hrmitD_ActiveFlag == false) {
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
                   apiService.DeleteURI("MasterIncomeTaxDetailsCess/ActiveDeactiveRecord", data.hrmiT_Id).
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

                           if (promise.incomeTaxDetailsCessList!==null && promise.incomeTaxDetailsCessList.length > 0) {

                               $scope.gridOptions.data = promise.incomeTaxDetailsCessList;
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