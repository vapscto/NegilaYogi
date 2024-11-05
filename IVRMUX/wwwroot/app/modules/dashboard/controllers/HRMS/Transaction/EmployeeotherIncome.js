﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeotherIncomeController', EmployeeotherIncomeController)
    EmployeeotherIncomeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function EmployeeotherIncomeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object


        $scope.loan = {};

        $scope.LoanEMIDis = true;

        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                {
                    name: 'hrmE_EmployeeFirstName', displayName: 'Employee Name', enableHiding: false
                },
                {
                    name: 'hreoI_OtherIncome', displayName: 'Amount ', enableHiding: false },
                {
                    name: 'imfY_FinancialYear', displayName: 'Financial Year ', enableHiding: false
                },
                {
                    name: 'hrmoI_OtherIncomeName', displayName: 'OtherIncome Name ', enableHiding: false
                },

              

                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hreoI_ActiveFlg=== false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                        '<span ng-if="row.entity.hreoI_ActiveFlg=== true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
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
            apiService.getURI("HREmpOtherIncome/getalldetails", pageid).then(function (promise) {

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                   
                }

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                    
                }
                if (promise.allowance !== null && promise.allowance.length > 0) {
                    $scope.allowance = promise.allowance;
                }

                if (promise.emploanList !== null && promise.emploanList.length > 0) {
                    $scope.gridOptions.data = promise.emploanList;
                    $scope.gridOptions = promise.emploanList;

                }
            })
        }
        $scope.LoanDetailsDis = true;
        //Compare to VAlue
        $scope.CalculateEMI = function () {

            $scope.loan.hreL_LaonEMI = "";

            if ($scope.loan.hrmlN_Id != undefined && $scope.loan.hrmlN_Id != "") {

                var selectedLoan = $filter('filter')($scope.masterloandropdown, function (d) {
                    return d.hrmlN_Id === parseInt($scope.loan.hrmlN_Id);
                });

                var maxloan = selectedLoan[0].hrmL_Max;
                if (maxloan != "" && maxloan != null) {

                    var EmpMaxLoanCanApply = (parseFloat(maxloan) * parseFloat($scope.loan.empGrossSal));

                    if ($scope.loan.hreL_LoanAmount > EmpMaxLoanCanApply) {
                        $scope.loan.hreL_LoanAmount = "";
                        swal('Loan Amount should be less than your ' + maxloan + ' times of gross salary...!!');
                        return;
                    }
                    if (($scope.loan.hreL_LoanAmount != undefined && $scope.loan.hreL_LoanAmount != "" && $scope.loan.hreL_LoanAmount != null)
                        && ($scope.loan.hreL_LoanInsallments != undefined && $scope.loan.hreL_LoanInsallments != "" && $scope.loan.hreL_LoanInsallments != null)
                    ) {

                        var result = parseFloat($scope.loan.hreL_LoanAmount) / parseFloat($scope.loan.hreL_LoanInsallments);

                        if (result < $scope.loan.empGrossSal) {
                            $scope.loan.hreL_LaonEMI = Math.round(result);
                        } else {
                            swal('EMI Should be Less than Net Salary..!!!');
                            $scope.loan.hreL_LoanInsallments = "";
                        }


                    }

                }

            } else {

                $scope.loan.hreL_LoanAmount = "";
                swal('Kindly select the Loan Type First..!!');
                return;

            }

        };


        // clear form data
        $scope.cancel = function () {
            // $scope.search = "";
            $scope.loan = {};
            $scope.submitted = false;
            $scope.LoanDetailsDis = true;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
        }
        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data1 = $scope.loan;
                var data = {
                    "IMFY_Id": $scope.loan.imfY_Id,
                    "HRME_ID": $scope.loan.hrmE_Id,
                    "HRMOI_Id": $scope.loan.hrmaL_Id,
                    "HREOI_OtherIncome": $scope.loan.hreoI_OtherIncome,
                 "HREOI_Id": $scope.LoanDetailssddd
                }


                //var data = {
                //    hrmgT_IdList: $scope.empFinancialYear
                //}
                apiService.create("HREmpOtherIncome/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal("Employee Loan Id already exist..!!");
                                return;
                            }

                            else if (promise.retrunMsg == "false") {
                                swal("Max Amount Exceed", 'Fail');

                            }
                            else if (promise.retrunMsg == "Add") {
                                swal("Record Saved Successfully..");
                            }
                            else if (promise.retrunMsg == "Update") {
                                swal("Record Updated Successfully..");
                            }

                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                            if (promise.emploanList !== null && promise.emploanList.length > 0) {
                                $scope.gridOptions.data = promise.emploanList;
                                //angular.forEach($scope.gridOptions.data, function (value, key) {
                                //    var fdate = value.hretdS_DepositedDate.split('T');
                                //    value.hretdS_DepositedDate = fdate[0];

                                //});
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
            var id = record.hreoI_Id;
            $scope.LoanDetailssddd = id;
            //var data = {
            //    "IMFY_Id": $scope.abc,
            //    "HRME_ID": $scope.loan.hrmE_Id,
            //    "HRETDS_DepositedDate": $scope.loan.hreL_AppliedDate,
            //    "HRETDS_BSRCode": $scope.loan.hreL_LoanAmount,
            //    "HRETDS_ChallanNo": $scope.loan.hreL_ReferenceNo,
            //}
            apiService.getURI("HREmpOtherIncome/editRecord", id).
                then(function (promise) {
                    if (promise.emploanList != null && promise.emploanList.length > 0) {
                        $scope.loan = promise.emploanList[0];

                        $scope.loan = promise.emploanList[0];

                       // $scope.loan.hreL_AppliedDate = new Date(promise.emploanList[0].hretdS_DepositedDate);
                        $scope.loan.hreoI_OtherIncome = promise.emploanList[0].hreoI_OtherIncome;
                        $scope.loan.hrmaL_Id = promise.emploanList[0].hrmoI_Id;
                        $scope.loan.imfY_Id = promise.emploanList[0].imfY_Id;
                        $scope.loan.hrmE_Id = promise.emploanList[0].hrmE_Id;

                      //  $scope.loan.hreL_AppliedDate = new Date($scope.loan.hreL_AppliedDate);
                    // $scope.loan.empGrossSal = promise.empGrossSal;
                       // $scope.LoanDetailsDis = false;
                    }
                })
        }

        $scope.GetDetailsByEmployee = function () {

            $scope.loan.empGrossSal = "";
            $scope.LoanDetailsDis = true;
            if ($scope.loan.hrmE_Id != "") {
                var id = $scope.loan.hrmE_Id;
                apiService.getURI("HREmpOtherIncome/getDetailsByEmployee", id).
                    then(function (promise) {
                        if (promise.empGrossSal != 0 && promise.empGrossSal != null) {
                            $scope.loan.empGrossSal = promise.empGrossSal;
                            $scope.LoanDetailsDis = false;
                        }
                    })
            }

        }

        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hreoI_ActiveFlg
 == false) {
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
                        apiService.DeleteURI("HREmpOtherIncome/ActiveDeactiveRecord", data.hreoI_Id).
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
                                    if (promise.emploanList !== null && promise.emploanList.length > 0) {
                                        $scope.gridOptions.data = promise.emploanList;
                                        angular.forEach($scope.gridOptions.data, function (value, key) {
                                            var fdate = value.hretdS_DepositedDate.split('T');
                                            value.hretdS_DepositedDate = fdate[0];

                                        });
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