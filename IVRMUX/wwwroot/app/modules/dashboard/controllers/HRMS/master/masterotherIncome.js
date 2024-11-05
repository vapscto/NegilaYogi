(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterotherIncomeController', masterotherIncomeController)

    masterotherIncomeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterotherIncomeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.Bank = {};
        var hrmoi_idvalue = 0;

        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmoI_OtherIncomeName', displayName: 'OtherIncome Name', enableHiding: false },
                { name: 'hrmoI_MaxLimit', displayName: 'MaxLimit', enableHiding: false },
                //{ name: 'hrmbD_BankAddress', displayName: 'Bank Address', enableHiding: false },
                //{ name: 'hrmbD_BranchName', displayName: 'Branch Name', enableHiding: false },
                //{ name: 'hrmbD_IFSCCode', displayName: 'IFSC Code', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"  data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmoI_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmoI_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
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
            apiService.getURI("MasterOtherIncome/getalldetails", pageid).then(function (promise) {
                if (promise.bankdetailList !== null && promise.bankdetailList.length > 0) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 10;
                    $scope.gridOptions = promise.bankdetailList;
                   // $scope.gridOptions.data = promise.bankdetailList;
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
               var data = $scope.Bank;
                var data = {
                    // "IMFY_Id": $scope.abc,
                    "HRMOI_OtherIncomeName": $scope.Bank.hrmoI_OtherIncomeName,
                    "HRMOI_OtherIncomeFlg": $scope.Bank.hrmoI_OtherIncomeFlg,
                    "HRMOI_MaxLimit": $scope.Bank.hrmoI_MaxLimit,
                    "HRMOI_MaxLimitAplFlg": $scope.Bank.hrmoI_MaxLimitAplFlg,
                    "HRMOI_Id": hrmoi_idvalue
                }
                apiService.create("MasterOtherIncome/", data).
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
                            else if (promise.retrunMsg == "ifsc") {
                                swal("IFSC Code is already exist..");
                                return;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
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

            var id = record.hrmoI_Id;
            hrmoi_idvalue = record.hrmoI_Id;
            apiService.getURI("MasterOtherIncome/editRecord", id).
                then(function (promise) {

                    if (promise.bankdetailList != null && promise.bankdetailList.length > 0) {
                        $scope.Bank = promise.bankdetailList[0];

                        $scope.Bank.hrmoI_OtherIncomeName = promise.bankdetailList[0].hrmoI_OtherIncomeName;
                        $scope.Bank.hrmoI_OtherIncomeFlg = promise.bankdetailList[0].hrmoI_OtherIncomeFlg;
                        $scope.Bank.hrmoI_MaxLimit = promise.bankdetailList[0].hrmoI_MaxLimit;
                        $scope.Bank.hrmoI_MaxLimitAplFlg = promise.bankdetailList[0].hrmoI_MaxLimitAplFlg;
                    }


                })
        }


        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmoI_ActiveFlg== false) {
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
                        apiService.DeleteURI("MasterOtherIncome/ActiveDeactiveRecord", data.hrmoI_Id).
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

                                    if (promise.bankdetailList !== null && promise.bankdetailList.length > 0) {
                                        // $scope.currentPage = 1;
                                        // $scope.itemsPerPage = 10;

                                        // $scope.employeeTypeList = promise.employeeTypeList;
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