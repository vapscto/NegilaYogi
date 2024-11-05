(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterallowanceCController', masterallowanceCController)

    masterallowanceCController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterallowanceCController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.Bank = {};
        var hrmal_idvalue = 0;

        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmaL_AllowanceName', displayName: 'Allowance Name', enableHiding: false },
                { name: 'hrmaL_MaxLimit', displayName: 'Max Limit', enableHiding: false },
                //{ name: 'hrmbD_BankAddress', displayName: 'Bank Address', enableHiding: false },
                //{ name: 'hrmbD_BranchName', displayName: 'Branch Name', enableHiding: false },
                //{ name: 'hrmbD_IFSCCode', displayName: 'IFSC Code', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"  data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmaL_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmaL_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
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
            apiService.getURI("MasterAllowance/getalldetails", pageid).then(function (promise) {
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
                var data1 = $scope.Bank;
                var data = {
                    // "IMFY_Id": $scope.abc,
                    "HRMAL_AllowanceName": $scope.Bank.hrmaL_AllowanceName,
                    "HRMAL_AllowanceFlg": $scope.Bank.hrmaL_AllowanceFlg,
                    "HRMAL_MaxLimit": $scope.Bank.hrmaL_MaxLimit,
                    "HRMAL_MaxLimitAplFlg": $scope.Bank.hrmaL_MaxLimitAplFlg,
                    "HRMAL_Id": hrmal_idvalue
                }

                apiService.create("MasterAllowance/", data).
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

            var id = record.hrmaL_Id;
            hrmal_idvalue = record.hrmaL_Id;
            apiService.getURI("MasterAllowance/editRecord", id).
                then(function (promise) {

                    if (promise.bankdetailList != null && promise.bankdetailList.length > 0) {
                        //$scope.Bank = promise.bankdetailList[0];

                        $scope.Bank.hrmaL_AllowanceName = promise.bankdetailList[0].hrmaL_AllowanceName;
                        $scope.Bank.hrmaL_AllowanceFlg = promise.bankdetailList[0].hrmaL_AllowanceFlg;
                        $scope.Bank.hrmaL_MaxLimitAplFlg = promise.bankdetailList[0].hrmaL_MaxLimitAplFlg;
                        $scope.Bank.hrmaL_MaxLimit = promise.bankdetailList[0].hrmaL_MaxLimit;
                        //$scope.Bank.hrmQ_ToMonth = promise.bankdetailList[0].hrmQ_ToMonth;
                    }
                })
        }


        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmaL_ActiveFlg== false) {
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
                        apiService.DeleteURI("MasterAllowance/ActiveDeactiveRecord", data.hrmaL_Id).
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

                                    if (promise.bankdetailList !== null && promise.bankdetailList.length > 0) {
                                        //$scope.currentPage = 1;
                                        //$scope.itemsPerPage = 10;

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