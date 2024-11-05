(function () {
    'use strict';
    angular
.module('app')
.controller('masterIncomeTaxDetailsController', masterIncomeTaxDetailsController)

    masterIncomeTaxDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterIncomeTaxDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.IncTaxDetail = {};

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
                            name: 'hrmitD_AmountFrom', displayName: 'Amount From', enableHiding: false
                        },
                        {
                            name: 'hrmitD_AmountTo', displayName: 'Amount To', enableHiding: false
                        },
                        {
                            name: 'hrmitD_IncomeTax', displayName: 'IncomeTax Detail', enableHiding: false
                        },
                        {
                            field: 'id', name: '',
                            displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                            '<div class="grid-action-cell">' +
                            '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                            '<a ng-if="row.entity.hrmitD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                            '<span ng-if="row.entity.hrmitD_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
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
            apiService.getURI("MasterIncomeTaxDetails/getalldetails", pageid).then(function (promise) {

                if (promise.incomeTaxDetailsList !== null && promise.incomeTaxDetailsList.length > 0) {
                    //$scope.gridOptions.data = promise.incomeTaxDetailsList;
                    $scope.gridOptions = promise.incomeTaxDetailsList;
                }
                if (promise.incomeTaxList !== null && promise.incomeTaxList.length > 0) {
                    $scope.incomeTaxList = promise.incomeTaxList;
                }


            })
        }

        //compare both Amount
        $scope.checkErr = function (FromAmount, ToAmount) {
            if (parseFloat(FromAmount) > parseFloat(ToAmount)) {
                swal("Amount From should not be greater than Amount To", 'Please Change Your amount!');
                $scope.IncTaxDetail.HRMITD_AmountFrom = "";
                return false;
            }
        };

        $scope.checkErr1 = function (FromAmount, ToAmount) {
            if (parseFloat(FromAmount) > parseFloat(ToAmount)) {
                swal("Amount To should be greater than Amount From", 'Please Change Your amount!');
                $scope.IncTaxDetail.HRMITD_AmountTo = "";
                return false;
            }
        };

        // clear form data
        $scope.cancel = function () {
            $scope.IncTaxDetail = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
        }
        //saving/updating Record       incometaxDetails
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMIT_Id": $scope.IncTaxDetail.HRMIT_Id,
                    "HRMITD_AmountFrom": $scope.IncTaxDetail.HRMITD_AmountFrom,
                    "HRMITD_AmountTo": $scope.IncTaxDetail.HRMITD_AmountTo,
                    "HRMITD_IncomeTax": $scope.IncTaxDetail.HRMITD_IncomeTax,
                   incTaxDetail: $scope.IncTaxDetail,
                    "HRMITD_Id": $scope.idss
                }
                apiService.create("MasterIncomeTaxDetails/", data).
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

                        if (promise.incomeTaxDetailsList !== null && promise.incomeTaxDetailsList.length > 0) {

                            $scope.gridOptions.data = promise.incomeTaxDetailsList;
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
            $scope.LoanDetailsDis = true;
            var id = record.hrmitD_Id;
            $scope.idss = id;
            apiService.getURI("MasterIncomeTaxDetails/editRecord", id).
                then(function (promise) {
                    angular.forEach(promise.incomeTaxDetailsList, function (value, key) {
                        if (value.hrmiT_Id !== 0) {
                            $scope.IncTaxDetail.IncTaxDetail = value.IncTaxDetail;
                            $scope.IncTaxDetail.HRMITD_AmountFrom = value.hrmitD_AmountFrom;
                            $scope.IncTaxDetail.HRMITD_AmountTo = value.hrmitD_AmountTo;
                            $scope.IncTaxDetail.HRMITD_IncomeTax = value.hrmitD_IncomeTax;
                            $scope.IncTaxDetail.HRMIT_Id = value.hrmiT_Id;
                            //$scope.IncTaxDetail.HRMIT_Id = value.hrmiT_AgeFlag;
                            $scope.LoanDetailsDis = true;
                        }
                    });
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
                   apiService.DeleteURI("MasterIncomeTaxDetails/ActiveDeactiveRecord", data.hrmitD_Id).
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

                           if (promise.incomeTaxDetailsList !== null && promise.incomeTaxDetailsList.length > 0) {

                               $scope.gridOptions.data = promise.incomeTaxDetailsList;
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