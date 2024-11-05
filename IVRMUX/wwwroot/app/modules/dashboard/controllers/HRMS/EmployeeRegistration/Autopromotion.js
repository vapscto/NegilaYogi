(function () {
    'use strict';
    angular
        .module('app')
        .controller('employeeAutopromotionController', employeeAutopromotionController)
    employeeAutopromotionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function employeeAutopromotionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object


      

        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },

                //{
                //    name: 'employeename', displayName: 'Employee Name', enableHiding: false },
                {
                    name: 'hrmE_EmployeeFirstName', displayName: 'Employee Allowance', enableHiding: false
                },
                {
                    name: 'hreaL_Allowance', displayName: 'Amount', enableHiding: false
                },

                {
                    name: 'imfY_FinancialYear', displayName: 'Financial Year ', enableHiding: false
                },
                {
                    name: 'hrmaL_AllowanceName', displayName: 'Allowance Name ', enableHiding: false
                },

                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hreaL_ActiveFlg=== false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                        '<span ng-if="row.entity.hreaL_ActiveFlg=== true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
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
            apiService.getURI("EmployeeAutopromotion/getalldetails", pageid).then(function (promise) {

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                   // $('#blah').attr('src', promise.employeedropdown.hrmE_PHOTO);
                }

            })
        }
        $scope.LoanDetailsDis = true;
        //Compare to VAlue
       

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
                    "HRMAL_Id": $scope.loan.hrmaL_Id,
                    "HREAL_Allowance": $scope.loan.hreaL_Allowance,
                    "HREAL_Id": $scope.LoanDetailsDisddd,
                }


                //var data = {
                //    hrmgT_IdList: $scope.empFinancialYear
                //}
                apiService.create("EmployeeAutopromotion/", data).
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
            var id = record.hreaL_Id;
            $scope.LoanDetailsDisddd = id;
            apiService.getURI("EmployeeAutopromotion/editRecord", id).
                then(function (promise) {
                    if (promise.emploanList != null && promise.emploanList.length > 0) {
                        $scope.loan = promise.emploanList[0];

                        $scope.loan.hreoI_OtherIncome = promise.emploanList[0].hreoI_OtherIncome;
                        $scope.loan.hrmaL_Id = promise.emploanList[0].hrmaL_Id;
                        $scope.loan.imfY_Id = promise.emploanList[0].imfY_Id;
                        $scope.loan.hrmE_Id = promise.emploanList[0].hrmE_Id;

                        // $scope.loan.hreL_AppliedDate = new Date($scope.loan.hreL_AppliedDate);
                        // $scope.loan.empGrossSal = promise.empGrossSal;
                        $scope.LoanDetailsDis = false;
                    }
                })
        }

        $scope.GetDetailsByEmployee = function () {

            $scope.loan.empGrossSal = "";
            $scope.LoanDetailsDis = true;
            if ($scope.loan.hrmE_Id != "") {
                var id = $scope.loan.hrmE_Id;
                apiService.getURI("EmployeeAutopromotion/getDetailsByEmployee", id).
                    then(function (promise) {
                        if (promise.dropdownvalus != 0 && promise.dropdownvalus != null) {
                            $scope.dropdownvalus = promise.dropdownvalus[0];
                            $('#blah').attr('src', promise.dropdownvalus[0].hrmE_PHOTO);
                        }

                        if (promise.employeegrade != 0 && promise.employeegrade != null) {
                            $scope.employeegrade = promise.employeegrade;
                         
                        }
                        if (promise.employeedesig != 0 && promise.employeedesig != null) {
                            $scope.employeedesig = promise.employeedesig;
                            
                        }
                        if (promise.employeedept != 0 && promise.employeedept != null) {
                            $scope.employeedept = promise.employeedept;
                          
                        }

                        if (promise.employeeemptype != 0 && promise.employeeemptype != null) {
                            $scope.employeeemptype = promise.employeeemptype;
                            
                        }

                        if (promise.employeeempgrouptype != 0 && promise.employeeempgrouptype != null) {
                            $scope.employeeempgrouptype = promise.employeeempgrouptype;
                           
                        }
                        $scope.getSalaryDetails();

                    })
            }

        }

        $scope.getSalaryDetails = function () {
            $scope.submittedSalarydetails = true;
            if ($scope.myFormSalarydetails.$valid) {
                $scope.netSalary = 0;
                $scope.EarningTotal = 0;
                $scope.DeductionTotal = 0;
                $scope.ArrearTotal = 0;

                var data = $scope.loan.hrmE_Id;
                apiService.create("EmployeeRegistration/getEmployeeSalaryDetails", data).
                    then(function (promise) {
                        //earning & deduction details

                        if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {

                            var employeeEarningsDeductionsDetails = promise.employeeEarningsDeductionsDetails;


                            var EarningDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Earning';
                            });

                            var DeductionDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Deduction';
                            });

                            var ArrearDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Arrear';
                            });

                            var GrossDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Gross';
                            });

                            if (GrossDetails != null && GrossDetails.length > 0) {
                                $scope.Salary = GrossDetails[0];
                                $scope.Salary.hreeD_ActiveFlag = GrossDetails[0].hreeD_ActiveFlag;

                            } else {
                                $scope.Salary.hreeD_Percentage = '0.00';
                                $scope.Salary.hreeD_Amount = '0.00';
                                $scope.Salary.hrmeD_AppPercent = '0.00';
                                $scope.Salary.hrmeD_Percent = '0.00';
                                $scope.Salary.hrmeD_Details = '';
                                $scope.Salary.hrmeD_Id = 0;
                                $scope.Salary.hreeD_ActiveFlag = true;
                            }

                            //Earning List
                            var totalEarning = 0;
                            angular.forEach($scope.earningList, function (value, key) {

                                angular.forEach(EarningDetails, function (value1, key1) {

                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        // $scope.earningList[key].Selected = true;
                                        $scope.earningList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.earningList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.earningList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.earningList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.earningList[key].hrmE_Id = value1.hrmE_Id;

                                        totalEarning = totalEarning + value1.hreeD_Amount;

                                    }

                                });


                            });


                            //deductionlist
                            var totalDeduction = 0;
                            angular.forEach($scope.detectionList, function (value, key) {




                                angular.forEach(DeductionDetails, function (value1, key1) {

                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        //  $scope.detectionList[key].Selected = true;
                                        $scope.detectionList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.detectionList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.detectionList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.detectionList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.detectionList[key].hrmE_Id = value1.hrmE_Id;
                                        if (value.hrmeD_EDTypeFlag == 'PF') {
                                            if ($scope.Employee.hrmE_PFFixedFlag) {
                                                $scope.detectionList[key].AmountDis = false;
                                                $scope.detectionList[key].PercentDis = false;
                                            } else {
                                                if (value.hrmeD_AmountPercentFlag == "Percentage") {
                                                    $scope.detectionList[key].AmountDis = true;
                                                    $scope.detectionList[key].PercentDis = false;

                                                } else {
                                                    $scope.detectionList[key].AmountDis = false;
                                                    $scope.detectionList[key].PercentDis = true;
                                                }
                                            }
                                        }

                                        totalDeduction = totalDeduction + value1.hreeD_Amount;
                                    }

                                });


                            });


                            //arrearlist
                            var totalArrear = 0;
                            angular.forEach($scope.arrearList, function (value, key) {

                                angular.forEach(ArrearDetails, function (value1, key1) {

                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        // $scope.arrearList[key].Selected = true;
                                        $scope.arrearList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.arrearList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.arrearList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.arrearList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.arrearList[key].hrmE_Id = value1.hrmE_Id;

                                        totalArrear = totalArrear + value1.hreeD_Amount;
                                    }

                                });

                            });

                            $scope.EarningTotal = $scope.getEarningTotal();
                            $scope.DeductionTotal = $scope.getDeductionTotal();
                            $scope.ArrearTotal = $scope.getArrearTotal();

                            $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;

                        }


                    });
            }



        }


        $scope.getEmployeeSalaryDetailsByHead = function (data) {
            $scope.submittedSalarydetails = true;
            if ($scope.myFormSalarydetails.$valid) {

                $scope.netSalary = 0;
                $scope.EarningTotal = 0;
                $scope.DeductionTotal = 0;
                $scope.ArrearTotal = 0;

                apiService.create("EmployeeRegistration/getEmployeeSalaryDetailsByHead", data).
                    then(function (promise) {
                        //earning & deduction details

                        if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {

                            var employeeEarningsDeductionsDetails = promise.employeeEarningsDeductionsDetails;


                            var EarningDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Earning';
                            });

                            var DeductionDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Deduction';
                            });

                            var ArrearDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Arrear';
                            });

                            var GrossDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Gross';
                            });

                            if (GrossDetails != null && GrossDetails.length > 0) {
                                $scope.Salary = GrossDetails[0];
                                $scope.Salary.hreeD_ActiveFlag = GrossDetails[0].hreeD_ActiveFlag;

                            } else {
                                $scope.Salary.hreeD_Percentage = '0.00';
                                $scope.Salary.hreeD_Amount = '0.00';
                                $scope.Salary.hrmeD_AppPercent = '0.00';
                                $scope.Salary.hrmeD_Percent = '0.00';
                                $scope.Salary.hrmeD_Details = '';
                                $scope.Salary.hrmeD_Id = 0;
                                $scope.Salary.hreeD_ActiveFlag = true;
                            }

                            var totalEarning = 0;
                            angular.forEach($scope.earningList, function (value, key) {

                                angular.forEach(EarningDetails, function (value1, key1) {

                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        // $scope.earningList[key].Selected = true;
                                        $scope.earningList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.earningList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.earningList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.earningList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.earningList[key].hrmE_Id = value1.hrmE_Id;

                                        totalEarning = totalEarning + value1.hreeD_Amount;
                                    }

                                });


                            });

                            $scope.submittedSalarydetails = false;
                            $scope.saveSalaryDetails = function () {
                                $scope.submittedSalarydetails = true;
                                if ($scope.myFormSalarydetails.$valid) {



                                    var selectedEarning = [];
                                    if ($scope.Salary.hrmeD_Id > 0) {
                                        selectedEarning.push($scope.Salary);
                                    }
                                    else {
                                        $scope.Salary.hreeD_Percentage = '0.00';
                                        $scope.Salary.hreeD_Amount = '0.00';
                                        $scope.Salary.hrmeD_AppPercent = '0.00';
                                        $scope.Salary.hrmeD_Percent = '0.00';
                                        $scope.Salary.hrmeD_Details = '';
                                        $scope.Salary.hrmeD_Id = 0;
                                        $scope.Salary.hreeD_ActiveFlag = true;
                                    }

                                    if ($scope.earningList !== null && $scope.earningList.length > 0) {

                                        angular.forEach($scope.earningList, function (earning) {
                                            if (earning.hreeD_ActiveFlag) {
                                                earning.hreeD_ActiveFlag = true;
                                            } else {
                                                earning.hreeD_ActiveFlag = false;
                                            }
                                            selectedEarning.push(earning);


                                        });

                                    }

                                    var selectedDetection = [];

                                    if ($scope.detectionList !== null && $scope.detectionList.length > 0) {

                                        angular.forEach($scope.detectionList, function (detection) {
                                            if (detection.hreeD_ActiveFlag) {
                                                detection.hreeD_ActiveFlag = true;

                                            } else {
                                                detection.hreeD_ActiveFlag = false;
                                            }

                                            selectedDetection.push(detection);
                                        });

                                    }

                                    var selectedArrear = [];

                                    if ($scope.arrearList !== null && $scope.arrearList.length > 0) {

                                        angular.forEach($scope.arrearList, function (arrear) {
                                            if (arrear.hreeD_ActiveFlag) {

                                                arrear.hreeD_ActiveFlag = true;
                                            } else {
                                                arrear.hreeD_ActiveFlag = false;
                                            }

                                            selectedArrear.push(arrear);

                                        });

                                    }

                                    if (selectedEarning.length === 0 && selectedDetection.length === 0) {
                                        swal('Kindly select atleast one record from Earning / Deduction');
                                        return;
                                    }

                                    var data = {
                                        Employeedto: $scope.Employee,
                                        EarningDTO: selectedEarning,
                                        DeductionDTO: selectedDetection,
                                        ArrearDTO: selectedArrear,
                                        
                                    }
                                    apiService.create("EmployeeRegistration/", data).
                                        then(function (promise) {

                                            if (promise.retrunMsg !== "") {

                                                if (promise.retrunMsg === "Duplicate") {
                                                    swal("Record already exist..!!");
                                                    return;
                                                } else if (promise.retrunMsg === "false") {
                                                    swal("Record Not saved / Updated..", 'Fail');
                                                } else if (promise.retrunMsg === "Add") {
                                                    swal("Record Saved Successfully..");
                                                } else if (promise.retrunMsg === "Update") {
                                                    swal("Record Updated Successfully..");
                                                } else {
                                                    swal("Something went wrong ..!", 'Kindly contact Administrator');
                                                    return;
                                                }

                                                $scope.cancel();

                                                $scope.onLoadGetData();
                                            }
                                        })
                                }

                            };
                            //deductionlist
                            var totalDeduction = 0;
                            angular.forEach($scope.detectionList, function (value, key) {

                                angular.forEach(DeductionDetails, function (value1, key1) {

                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        //  $scope.detectionList[key].Selected = true;
                                        $scope.detectionList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.detectionList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.detectionList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.detectionList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.detectionList[key].hrmE_Id = value1.hrmE_Id;
                                        if (value.hrmeD_EDTypeFlag == 'PF') {
                                            if ($scope.Employee.hrmE_PFFixedFlag) {
                                                $scope.detectionList[key].AmountDis = false;
                                                $scope.detectionList[key].PercentDis = false;
                                            } else {
                                                if (value.hrmeD_AmountPercentFlag == "Percentage") {
                                                    $scope.detectionList[key].AmountDis = true;
                                                    $scope.detectionList[key].PercentDis = false;

                                                } else {
                                                    $scope.detectionList[key].AmountDis = false;
                                                    $scope.detectionList[key].PercentDis = true;
                                                }
                                            }
                                        }

                                        totalDeduction = totalDeduction + value1.hreeD_Amount;
                                    }

                                });


                            });


                            //arrearlist
                            var totalArrear = 0;
                            angular.forEach($scope.arrearList, function (value, key) {

                                angular.forEach(ArrearDetails, function (value1, key1) {

                                    if (value.hrmeD_Id == value1.hrmeD_Id) {

                                        // $scope.arrearList[key].Selected = true;
                                        $scope.arrearList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.arrearList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.arrearList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.arrearList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.arrearList[key].hrmE_Id = value1.hrmE_Id;

                                        totalArrear = totalArrear + value1.hreeD_Amount;
                                    }

                                });

                            });


                            $scope.EarningTotal = $scope.getEarningTotal();
                            $scope.DeductionTotal = $scope.getDeductionTotal();
                            $scope.ArrearTotal = $scope.getArrearTotal();

                            $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;

                        }


                    });


            }

        }






        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hreaL_ActiveFlg
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
                        apiService.DeleteURI("EmployeeAutopromotion/ActiveDeactiveRecord", data.hreaL_Id).
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