(function () {
    'use strict';
    angular
.module('app')
.controller('EmployeeAdvanceReportController', EmployeeAdvanceReportController)

    EmployeeAdvanceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function EmployeeAdvanceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

     
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeAdvanceReport/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;


                }

               

                if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = promise.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);

                }


                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                }

                if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                    $scope.designationdropdown = promise.designationdropdown;

                    $scope.designationselectedAll = true;
                    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);


                }

                if (promise.configurationDetails != null) {

                    $scope.SalaryFromDay = promise.configurationDetails.hrC_SalaryFromDay;
                    $scope.SalaryToDay = promise.configurationDetails.hrC_SalaryToDay;

                }

            })
        }


        //

        //$scope.getTotal = function () {
        //    var total = 0;
        //    for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
        //        var product = $scope.employeeSalaryslipDetails[i];
        //        total += product.totalEmployees;
        //    }
        //    return Math.round(total);
        //}





        $scope.employeeSalaryslipDetails = [];
        $scope.institutionDetails = {};
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.EmployeeDis = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.institutionDetails = {};
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

                if (groupTypeselected.length == 0 && departmentselected.length == 0 && designationselected.length == 0) {
                    swal('Kindly select atleast one record');
                    return;
                }


                var data = {
                    "HRESA_AdvYear": $scope.Employee.hreS_Year,
                    "HRESA_AdvMonth": $scope.Employee.hreS_Month,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected
                }

                apiService.create("EmployeeAdvanceReport/getEmployeedetailsBySelection", data).
                            then(function (promise) {
                                if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {

                                    $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;
                                }


                                if (promise.institutionDetails != null) {
                                    $scope.institutionDetails = promise.institutionDetails;

                                    //  $('#blah').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.institutionDetails.mi_id + "/" + "EmployeeProfilePics" + "/" + $scope.institutionDetails.mI_Logo);

                                    var instuteAddress = "";
                                    if ($scope.institutionDetails.mI_Address1 != null && $scope.institutionDetails.mI_Address1 != "") {

                                        instuteAddress = $scope.institutionDetails.mI_Address1;

                                    }
                                    if ($scope.institutionDetails.mI_Address2 != null && $scope.institutionDetails.mI_Address2 != "") {

                                        instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address2;

                                    }

                                    if ($scope.institutionDetails.mI_Address3 != null && $scope.institutionDetails.mI_Address3 != "") {

                                        instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address3;

                                    }

                                    $scope.CurrentInstuteAddress = instuteAddress;

                                }

                                //get current date
                                $scope.hresA_AdvYear = promise.hresA_AdvYear;

                                //get DOI
                                $scope.hresA_AdvMonth = promise.hresA_AdvMonth;

                                if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {
                                    $scope.EmployeeDis = true;

                                    $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;
                                    // console.log($scope.employeeSalaryslipDetails);
                                    console.log($scope.employeeSalaryslipDetails);

                                    //$scope.earningheadlist = $scope.employeeSalaryslipDetails[0].earningresult;

                                    //angular.forEach($scope.earningheadlist, function (headresult) {

                                    //    headresult.netamount = 0;

                                    //});

                                    //$scope.deductionheadlist = $scope.employeeSalaryslipDetails[0].deductionresult;

                                    //angular.forEach($scope.deductionheadlist, function (headresult) {

                                    //    headresult.netamount = 0;

                                    //});


                                    //
                                    //angular.forEach($scope.employeeSalaryslipDetails, function (itm) {

                                    //    itm.grossEarning = Math.round(itm.grossEarning);
                                    //    itm.grossDeduction = Math.round(itm.grossDeduction);
                                    //    itm.netSalary = Math.round(itm.netSalary);

                                    //    //earning head totalAmount
                                    //    angular.forEach(itm.earningresult, function (result) {

                                    //        result.hresD_Amount = Math.round(result.hresD_Amount);
                                    //        angular.forEach($scope.earningheadlist, function (headresult) {

                                    //            if (headresult.hrmeD_Name == result.hrmeD_Name) {

                                    //                headresult.netamount = Math.round(headresult.netamount + result.hresD_Amount);
                                    //            }

                                    //        });

                                    //    });
                                    //    //deduction head totalAmount
                                    //    angular.forEach(itm.deductionresult, function (result) {

                                    //        result.hresD_Amount = Math.round(result.hresD_Amount);

                                    //        angular.forEach($scope.deductionheadlist, function (headresult) {

                                    //            if (headresult.hrmeD_Name == result.hrmeD_Name) {

                                    //                headresult.netamount = Math.round(headresult.netamount + result.hresD_Amount);
                                    //            }

                                    //        });

                                    //    });
                                    //});
                                    //$scope.earnlen = $scope.earningheadlist.length;
                                    //$scope.dedlen = $scope.deductionheadlist.length;
                                }
                                else {
                                    $scope.EmployeeDis = false;
                                    swal('No Record found to display .. !');
                                    return;
                                }

                            })
            }

        }



        //By group Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;

            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            angular.forEach($scope.designationdropdown, function (itm22) {
                itm22.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;
            });

            angular.forEach($scope.departmentdropdown, function (itm232) {
                itm232.selected = toggleStatus;
                $scope.departmentselectedAll = toggleStatus;
            });

            $scope.get_depts();
        }

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
            })
            var data = {
                hrmgT_IdList: ids
            }
            apiService.create("EmployeeAdvanceReport/get_depts", data).
                        then(function (promise) {

                            if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                                $scope.departmentdropdown = promise.departmentdropdown;
                                $scope.departmentselectedAll = true;
                                $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                            }
                        })
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
            angular.forEach($scope.designationdropdown, function (itm1) {
                itm1.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;

            })

            $scope.get_desig();

        }



        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        }
        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            })
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            })
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1
            }
            apiService.create("EmployeeAdvanceReport/get_desig", data).
                        then(function (promise) {
                            if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                                $scope.designationdropdown = promise.designationdropdown;
                                $scope.designationselectedAll = true;
                                $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                            }
                        })
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

        }


        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        }


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeSalaryslipDetails = [];
            $scope.submitted = false;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.employeeSelectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };



        $scope.printData = function () {
            var divToPrint = document.getElementById("Table");
            var newWin = window.open();
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
            // $state.reload();
        }

        //Total for per column

        //$scope.TotalgrossEarning = function () {
        //    var total = 0;
        //    if ($scope.employeeSalaryslipDetails != null) {
        //        for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


        //            var product = $scope.employeeSalaryslipDetails[i];
        //            total += product.grossEarning;
        //        }
        //    }

        //    return Math.round(total);
        //}

        //$scope.TotalgrossDeduction = function () {
        //    var total = 0;
        //    if ($scope.employeeSalaryslipDetails != null) {
        //        for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


        //            var product = $scope.employeeSalaryslipDetails[i];
        //            total += product.grossDeduction;
        //        }
        //    }

        //    return Math.round(total);
        //}

        $scope.TotalnetSalary = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.hresA_AppliedAmount;
                }
            }

            return Math.round(total);
        }



        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("table");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };

    }


})();