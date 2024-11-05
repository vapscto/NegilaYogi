(function () {
    'use strict';
    angular
        .module('app')
        .controller('employeeSalaryDetailsController', employeeSalaryDetailsController);

    employeeSalaryDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q'];
    function employeeSalaryDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {

        $scope.EmployeeDis = false;
        $scope.Employee = {};

        $scope.Salary = {};
        $scope.Salary.hreeD_Percentage = '0.00';
        $scope.Salary.hreeD_Amount = '0.00';
        $scope.Salary.hrmeD_AppPercent = '0.00';
        $scope.Salary.hrmeD_Percent = '0.00';
        $scope.Salary.hrmeD_Details = '';
        $scope.Salary.hrmeD_Id = 0;
        $scope.Salary.hreeD_ActiveFlag = true;

        $scope.netSalary = 0;
        $scope.EarningTotal = 0;
        $scope.DeductionTotal = 0;
        $scope.ArrearTotal = 0;


        // Get form Details at onload 
        $scope.onLoadGetData = function () {

            $scope.netSalary = 0;
            $scope.EarningTotal = 0;
            $scope.DeductionTotal = 0;
            $scope.ArrearTotal = 0;

            var pageid = 2;
            apiService.getURI("EmployeeSalaryDetails/getalldetails", pageid).then(function (promise) {

                if (promise.employeedetailList !== null && promise.employeedetailList.length > 0) {
                    $scope.employeedropdown = promise.employeedetailList;

                }

                //dropdown list

                $scope.groupTypedropdown = promise.groupTypedropdownlist;
                $scope.departmentdropdown = promise.departmentdropdownlist;
                $scope.designationdropdown = promise.designationdropdownlist;


                if ($scope.groupTypedropdown !== null && $scope.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = $scope.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);
                }


                if ($scope.departmentdropdown !== null && $scope.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = $scope.departmentdropdown;
                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                }

                if ($scope.designationdropdown !== null && $scope.designationdropdown.length > 0) {
                    $scope.designationdropdown = $scope.designationdropdown;

                    $scope.designationselectedAll = true;
                    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);


                }

                $scope.mi_id = promise.mI_Id;


                //earning list
                $scope.earningList = promise.earningList;

                if ($scope.earningList !== null && $scope.earningList.length > 0) {

                    angular.forEach($scope.earningList, function (value, key) {
                        $scope.earningList[key].hreeD_ActiveFlag = true;

                        if (value.hrmeD_AmountPercentFlag === "Percentage") {
                            $scope.earningList[key].hreeD_Percentage = value.hrmeD_AmountPercent;
                            $scope.earningList[key].hreeD_Amount = '0.00';
                            $scope.earningList[key].hrmeD_Percent = value.hrmeD_AmountPercent;

                            $scope.earningList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                            //percentOff

                            $scope.earningList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;


                            $scope.earningList[key].AmountDis = true;
                            $scope.earningList[key].PercentDis = false;

                        }
                        else {
                            $scope.earningList[key].hreeD_Percentage = '0.00';
                            $scope.earningList[key].hreeD_Amount = value.hrmeD_AmountPercent;
                            $scope.earningList[key].hrmeD_AppPercent = '0.00';

                            $scope.earningList[key].hrmeD_Percent = '0.00';

                            $scope.earningList[key].hrmeD_Details = '';

                            $scope.earningList[key].AmountDis = false;
                            $scope.earningList[key].PercentDis = true;
                        }

                    });

                }

                //deduction list

                $scope.detectionList = promise.detectionList;
                if ($scope.detectionList !== null && $scope.detectionList.length > 0) {

                    angular.forEach($scope.detectionList, function (value, key) {

                        $scope.detectionList[key].hreeD_ActiveFlag = true;
                        if (value.hrmeD_AmountPercentFlag === "Percentage") {
                            $scope.detectionList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                            $scope.detectionList[key].hreeD_Amount = '0.00';
                            $scope.detectionList[key].hreeD_Percentage = value.hrmeD_AmountPercent;

                            $scope.detectionList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;

                            $scope.detectionList[key].AmountDis = true;
                            $scope.detectionList[key].PercentDis = false;

                        }
                        else {
                            $scope.detectionList[key].hrmeD_Percent = '0.00';

                            $scope.detectionList[key].hreeD_Amount = value.hrmeD_AmountPercent;
                            $scope.detectionList[key].hreeD_Percentage = '0.00';

                            $scope.detectionList[key].hrmeD_Details = '';

                            $scope.detectionList[key].AmountDis = false;
                            $scope.detectionList[key].PercentDis = true;
                        }

                    });


                }

                $scope.arrearList = promise.arrearList;

                if ($scope.arrearList !== null && $scope.arrearList.length > 0) {

                    angular.forEach($scope.arrearList, function (value, key) {
                        $scope.arrearList[key].hreeD_ActiveFlag = true;

                        if (value.hrmeD_AmountPercentFlag === "Percentage") {
                            $scope.arrearList[key].hreeD_Percentage = value.hrmeD_AmountPercent;
                            $scope.arrearList[key].hreeD_Amount = '0.00';
                            $scope.arrearList[key].hrmeD_Percent = value.hrmeD_AmountPercent;

                            $scope.arrearList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                            //percentOff

                            $scope.arrearList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;

                            $scope.arrearList[key].AmountDis = true;
                            $scope.arrearList[key].PercentDis = false;

                        }
                        else {
                            $scope.arrearList[key].hreeD_Percentage = '0.00';
                            $scope.arrearList[key].hreeD_Amount = value.hrmeD_AmountPercent;
                            $scope.arrearList[key].hrmeD_AppPercent = '0.00';

                            $scope.arrearList[key].hrmeD_Percent = '0.00';

                            $scope.arrearList[key].hrmeD_Details = '';

                            $scope.arrearList[key].AmountDis = false;
                            $scope.arrearList[key].PercentDis = true;
                        }

                    });

                }

                if (promise.grossList[0] !== null && promise.grossList[0] !== undefined) {
                    $scope.Salary = promise.grossList[0];
                    $scope.Salary.hreeD_ActiveFlag = true;
                    // 

                    $scope.Salary.hreeD_Percentage = '0.00';
                    $scope.Salary.hreeD_Amount = $scope.Salary.hrmeD_AmountPercent;
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';


                } else {
                    $scope.Salary.hreeD_Percentage = '0.00';
                    $scope.Salary.hreeD_Amount = '0.00';
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
                    $scope.Salary.hrmeD_Id = 0;
                    $scope.Salary.hreeD_ActiveFlag = true;
                }

                $scope.EarningTotal = $scope.getEarningTotal();
                $scope.DeductionTotal = $scope.getDeductionTotal();
                $scope.ArrearTotal = $scope.getArrearTotal();

                $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;


                if (promise.configurationDetails !== null) {

                    $scope.RetirementYrs = promise.configurationDetails.hrC_RetirementYrs;
                    $scope.hrC_AsPerEmpFlag = promise.configurationDetails.hrC_AsPerEmpFlag;

                    $scope.configurationDetails = promise.configurationDetails;


                }

            });
        };


        //By group Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;
            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_depts();
        };


        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });

            $scope.get_depts();
        };
        $scope.get_depts = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids
            };
            apiService.create("EmployeeSalaryDetails/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                });
        };



        //By Employee Type
        $scope.GetEmployeeByTypeAll = function (employeeTypeselectedAll) {

            var toggleStatus = $scope.employeeTypeselectedAll;
            angular.forEach($scope.employeeTypedropdown, function (itm) {
                itm.selected = toggleStatus;

            });
        };


        //single
        $scope.GetEmployeeByType = function (employeeType) {

            $scope.employeeTypeselectedAll = $scope.employeeTypedropdown.every(function (itm) {

                return itm.selected;
            });

        };
        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1
            };
            apiService.create("EmployeeSalaryDetails/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
        };


        //By Department
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;

            });
            $scope.get_desig();

        };


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        };



        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;

            });

        };


        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        };



        $scope.getEarningTotal = function () {

            var total = 0;
            for (var i = 0; i < $scope.earningList.length; i++) {

                if ($scope.earningList[i].hreeD_ActiveFlag) {

                    var product = $scope.earningList[i];
                    total += parseFloat(product.hreeD_Amount);
                }


            }
            return total;
        };

        $scope.getDeductionTotal = function () {

            var total = 0;
            for (var i = 0; i < $scope.detectionList.length; i++) {
                if ($scope.detectionList[i].hreeD_ActiveFlag) {
                    var product = $scope.detectionList[i];
                    total += parseFloat(product.hreeD_Amount);
                }

            }
            return total;
        };

        $scope.getArrearTotal = function () {

            var total = 0;
            for (var i = 0; i < $scope.arrearList.length; i++) {
                if ($scope.arrearList[i].hreeD_ActiveFlag) {
                    var product = $scope.arrearList[i];
                    total += parseFloat(product.hreeD_Amount);
                }

            }
            return total;
        };

        $scope.submitted = false;
        $scope.GetEmployeeList = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            $scope.employeedropdown = [];
            //  $scope.Emp = {};

            var groupTypeselected = [];
            angular.forEach($scope.groupTypedropdown, function (itm) {
                if (itm.selected) {
                    groupTypeselected.push(itm.hrmgT_Id);//
                }

            });

            var departmentselected = [];
            angular.forEach($scope.departmentdropdown, function (itm) {
                if (itm.selected) {
                    departmentselected.push(itm.hrmD_Id);
                }

            });


            var designationselected = [];
            angular.forEach($scope.designationdropdown, function (itm) {
                if (itm.selected) {
                    designationselected.push(itm.hrmdeS_Id);
                }

            });

            if (groupTypeselected.length === 0 && departmentselected.length === 0 && designationselected.length === 0) {
                swal('Kindly select atleast one record');
                return;
            }


            var data = {
                groupTypeIdList: groupTypeselected,
                hrmD_IdList: departmentselected,
                hrmdeS_IdList: designationselected
            };

            apiService.create("EmployeeSalaryDetails/GetEmployeeDetailsBySelected", data).
                then(function (promise) {

                    if (promise.employeedetailList !== null && promise.employeedetailList.length > 0) {
                        $scope.employeedropdown = promise.employeedetailList;
                        $scope.EmployeeDis = true;
                        //   $scope.getEarningTotal();
                    }
                    else {
                        $scope.employeedropdown = [];

                        swal("No Record Found to Display..");
                    }
                });
        };


        //Salary details form validation
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
                    HRME_Id: $scope.Employee.hrmE_Id,
                    EarningDTO: selectedEarning,
                    DeductionDTO: selectedDetection,
                    ArrearDTO: selectedArrear,
                    "TabName": "SalaryTab"
                };
                apiService.create("EmployeeSalaryDetails/", data).
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
                    });
            }

        };

        
        $scope.interacted7 = function (field) {
            //if (field.$dirty==undefined) {
            //    console.log(field.$error);
            //}


            return $scope.submittedSalarydetails;
        };


        //

       
        // clear form data
        $scope.clear_Salarydetails_tab = function (data) {

            $scope.Salary.hreeD_Percentage = '0.00';
            $scope.Salary.hreeD_Amount = '0.00';
            $scope.Salary.hrmeD_AppPercent = '0.00';
            $scope.Salary.hrmeD_Percent = '0.00';
            $scope.Salary.hrmeD_Details = '';
            //$scope.Salary.hreeD_ActiveFlag = true;

            angular.forEach($scope.earningList, function (value, key) {

                $scope.earningList[key].hreeD_ActiveFlag = false;
                $scope.earningList[key].hreeD_Amount = '0.00';
                $scope.earningList[key].hreeD_Percentage = '0.00';

            });
            angular.forEach($scope.detectionList, function (value, key) {

                $scope.detectionList[key].hreeD_ActiveFlag = false;
                $scope.detectionList[key].hreeD_Amount = '0.00';
                $scope.detectionList[key].hreeD_Percentage = '0.00';

            });
            angular.forEach($scope.arrearList, function (value, key) {

                $scope.arrearList[key].hreeD_ActiveFlag = false;
                $scope.arrearList[key].hreeD_Amount = '0.00';
                $scope.arrearList[key].hreeD_Percentage = '0.00';

            });

            $scope.netSalary = 0;
            $scope.EarningTotal = 0;
            $scope.DeductionTotal = 0;
            $scope.ArrearTotal = 0;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            //$scope.detectionList =[];
            //$scope.detectionList = [];
            $scope.submittedSalarydetails = false;
            $scope.myFormSalarydetails.$setPristine();
            $scope.myFormSalarydetails.$setUntouched();
            $scope.EmployeeDis = false;
            $scope.onLoadGetData();
        };


        $scope.configurationDetails = {};

      
        // Onload Get Employee Salary Details

        $scope.getSalaryDetails = function () {
            $scope.submittedSalarydetails = true;
            if ($scope.myFormSalarydetails.$valid) {
                $scope.netSalary = 0;
                $scope.EarningTotal = 0;
                $scope.DeductionTotal = 0;
                $scope.ArrearTotal = 0;


                angular.forEach($scope.earningList, function (value, key) {

                    // $scope.earningList[key].Selected = false;

                    $scope.earningList[key].hreeD_ActiveFlag = false;

                    $scope.earningList[key].hreeD_Amount = '0.00';
                    $scope.earningList[key].hreeD_Percentage = '0.00';
                });



                angular.forEach($scope.detectionList, function (value, key) {

                    // $scope.detectionList[key].Selected = false;
                    $scope.detectionList[key].hreeD_ActiveFlag = false;

                    $scope.detectionList[key].hreeD_Amount = '0.00';
                    $scope.detectionList[key].hreeD_Percentage = '0.00';
                });


                angular.forEach($scope.arrearList, function (value, key) {

                    // $scope.arrearList[key].Selected = false;
                    $scope.arrearList[key].hreeD_ActiveFlag = false;

                    $scope.arrearList[key].hreeD_Amount = '0.00';
                    $scope.arrearList[key].hreeD_Percentage = '0.00';
                });

                var data = $scope.Employee;
                apiService.create("EmployeeSalaryDetails/getEmployeeSalaryDetails", data).
                    then(function (promise) {
                        //earning & deduction details

                        if (promise.employeeEarningsDeductionsDetails !== null && promise.employeeEarningsDeductionsDetails.length > 0) {

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

                            if (GrossDetails !== null && GrossDetails.length > 0) {
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

                                    if (value.hrmeD_Id === value1.hrmeD_Id) {

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

                                    if (value.hrmeD_Id === value1.hrmeD_Id) {

                                        //  $scope.detectionList[key].Selected = true;
                                        $scope.detectionList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.detectionList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.detectionList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.detectionList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.detectionList[key].hrmE_Id = value1.hrmE_Id;
                                        if (value.hrmeD_EDTypeFlag === 'PF') {
                                            if ($scope.Employee.hrmE_PFFixedFlag) {
                                                $scope.detectionList[key].AmountDis = false;
                                                $scope.detectionList[key].PercentDis = false;
                                            } else {
                                                if (value.hrmeD_AmountPercentFlag === "Percentage") {
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

                                    if (value.hrmeD_Id === value1.hrmeD_Id) {

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



        };


        $scope.getEmployeeSalaryDetailsByHead = function (data) {
            $scope.submittedSalarydetails = true;
            if ($scope.myFormSalarydetails.$valid) {

                $scope.netSalary = 0;
                $scope.EarningTotal = 0;
                $scope.DeductionTotal = 0;
                $scope.ArrearTotal = 0;

                apiService.create("EmployeeSalaryDetails/getEmployeeSalaryDetailsByHead", data).
                    then(function (promise) {
                        //earning & deduction details

                        if (promise.employeeEarningsDeductionsDetails !== null && promise.employeeEarningsDeductionsDetails.length > 0) {

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

                            if (GrossDetails !== null && GrossDetails.length > 0) {
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

                                    if (value.hrmeD_Id === value1.hrmeD_Id) {

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

                                    if (value.hrmeD_Id === value1.hrmeD_Id) {

                                        //  $scope.detectionList[key].Selected = true;
                                        $scope.detectionList[key].hreeD_ActiveFlag = value1.hreeD_ActiveFlag;
                                        $scope.detectionList[key].hreeD_Id = value1.hreeD_Id;
                                        $scope.detectionList[key].hreeD_Amount = value1.hreeD_Amount;
                                        $scope.detectionList[key].hreeD_Percentage = value1.hreeD_Percentage;
                                        $scope.detectionList[key].hrmE_Id = value1.hrmE_Id;
                                        if (value.hrmeD_EDTypeFlag === 'PF') {
                                            if ($scope.Employee.hrmE_PFFixedFlag) {
                                                $scope.detectionList[key].AmountDis = false;
                                                $scope.detectionList[key].PercentDis = false;
                                            } else {
                                                if (value.hrmeD_AmountPercentFlag === "Percentage") {
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

                                    if (value.hrmeD_Id === value1.hrmeD_Id) {

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

        };

    }
})();



