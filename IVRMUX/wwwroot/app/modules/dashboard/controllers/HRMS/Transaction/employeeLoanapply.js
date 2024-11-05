(function () {
    'use strict';
    angular
.module('app')
        .controller('employeeLoanapplyController', employeeLoanapplyController)
    employeeLoanapplyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function employeeLoanapplyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
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
              { name: 'masterEmployee.hrmE_EmployeeFirstName', displayName: 'Employee Name', enableHiding: false },
                //{ name: 'hreL_ReferenceNo', displayName: 'Reference No', enableHiding: false },
              { name: 'hrMasterLoan.hrmL_LoanType', displayName: 'Loan Type', enableHiding: false },
              { name: 'hreL_LoanAmount', displayName: 'Loan Amount', enableHiding: false },
              { name: 'hreL_LoanInsallments', displayName: 'Loan Insallments', enableHiding: false },
               { name: 'hreL_LaonEMI', displayName: 'Laon EMI', enableHiding: false },
                { name: 'hreL_AppliedDate', displayName: 'Applied Date', enableHiding: false },
              //{ name: 'hreL_LoanStatus', displayName: 'Loan Status', enableHiding: false },
              //{ name: 'hreL_SanctionedAmount', displayName: 'Sanctioned Amount', enableHiding: false },
              //{ name: 'hreL_TotalPrincipalPaid', displayName: 'Total Principal Paid', enableHiding: false },
              //{ name: 'hreL_TotalInterestPaid', displayName: 'Total Interest Paid', enableHiding: false },
              { name: 'hreL_TotalPending', displayName: 'Total Pending', enableHiding: false },
              
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                 '<a ng-if="row.entity.hreL_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                  '<span ng-if="row.entity.hreL_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
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
            apiService.getURI("HREmpLoan/getalldetails", pageid).then(function (promise) {

                //if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                //    $scope.employeedropdown = promise.employeedropdown;
                //}

                if (promise.masterloandropdown !== null && promise.masterloandropdown.length > 0) {
                    $scope.masterloandropdown = promise.masterloandropdown;
                }
                if (promise.modeOfPaymentdropdown !== null && promise.modeOfPaymentdropdown.length > 0) {
                    $scope.modeOfPaymentdropdown = promise.modeOfPaymentdropdown;
                }

                //if (promise.emploanList !== null && promise.emploanList.length > 0) {


                //    $scope.gridOptions.data = promise.emploanList;

                //}

                angular.forEach($scope.gridOptions.data, function (value, key) {
                    var fdate = value.hreL_AppliedDate.split('T');
                    value.hreL_AppliedDate = fdate[0];
                });
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
                
                var data = $scope.loan;
                apiService.create("HREmpLoan/", data).
                then(function (promise) {
                    if (promise.retrunMsg !== "") {

                        if (promise.retrunMsg == "Duplicate") {
                            swal("Employee Loan Id already exist..!!");
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
                            return;
                        }
                        if (promise.emploanList !== null && promise.emploanList.length > 0) {
                            $scope.gridOptions = promise.emploanList;
                            angular.forEach($scope.gridOptions.data, function (value, key) {
                                var fdate = value.hreL_AppliedDate.split('T');
                                value.hreL_AppliedDate = fdate[0];

                            });
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
            var id = record.hreL_Id;
            apiService.getURI("HREmpLoan/editRecord", id).
                then(function (promise) {
                    if (promise.emploanList != null && promise.emploanList.length > 0) {
                        $scope.loan = promise.emploanList[0];
                        $scope.loan.hreL_AppliedDate = new Date($scope.loan.hreL_AppliedDate);
                        $scope.loan.empGrossSal = promise.empGrossSal;
                        $scope.LoanDetailsDis = false;
                    }
                })
        }

        $scope.GetDetailsByEmployee = function () {
            
            $scope.loan.empGrossSal = "";
            $scope.LoanDetailsDis = true;
            if ($scope.loan.hrmE_Id != "") {
                var id = $scope.loan.hrmE_Id;
                apiService.getURI("HREmpLoan/getDetailsByEmployee", id).
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
            if (data.hreL_ActiveFlag == false) {
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
                   apiService.DeleteURI("HREmpLoan/ActiveDeactiveRecord", data.hreL_Id).
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
                                   var fdate = value.hreL_AppliedDate.split('T');
                                   value.hreL_AppliedDate = fdate[0];

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