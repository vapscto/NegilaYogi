(function () {
    'use strict';
    angular
        .module('app')
        .controller('YearlySalaryReportController', YearlySalaryReportController);

    YearlySalaryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache'];
    function YearlySalaryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout, superCache) {
        //object

        //Employeee
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = 10;
        $scope.maxDateDOJ = new Date();

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("YearlySalaryReport/getBasicData", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                //if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                //    $scope.monthdropdown = promise.monthdropdown;


                //}

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
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

                if (promise.configurationDetails !== null) {

                    $scope.SalaryFromDay = promise.configurationDetails.hrC_SalaryFromDay;
                    $scope.SalaryToDay = promise.configurationDetails.hrC_SalaryToDay;

                }

            });
        };


        //

        $scope.getTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                var product = $scope.employeeSalaryslipDetails[i];
                total += product.totalEmployees;
            }
            return Math.round(total);
        };

        $scope.checkFromDSelected = function () {

        };



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
                        groupTypeselected.push(itm.hrmgT_Id);
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

                if ($scope.Employee.hrmE_Fromdate !== null && $scope.Employee.hrmE_Fromdate !== undefined) {
                    $scope.Employee.hrmE_Fromdate = new Date($scope.Employee.hrmE_Fromdate).toDateString();
                    $scope.fromYear = new Date($scope.Employee.hrmE_Fromdate).getFullYear(); 
                }

                if ($scope.Employee.hrmE_Todate !== null && $scope.Employee.hrmE_Todate !== undefined) {
                    $scope.Employee.hrmE_Todate = new Date($scope.Employee.hrmE_Todate).toDateString();
                    $scope.toYear = new Date($scope.Employee.hrmE_Todate).getFullYear(); 
                }

                var data = {
                    "HRME_Fromdate": $scope.Employee.hrmE_Fromdate,
                    "HRME_Todate": $scope.Employee.hrmE_Todate,
                    "HRME_Id": $scope.Employee.hrmE_Id,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected
                };

                apiService.create("YearlySalaryReport/reportBetweenDatesBySelection", data).
                    then(function (promise) {
                        if (promise.institutionDetails !== null) {
                            $scope.institutionDetails = promise.institutionDetails;
                            var instuteAddress = "";
                            if ($scope.institutionDetails.mI_Address1 !== null && $scope.institutionDetails.mI_Address1 !== "") {
                                instuteAddress = $scope.institutionDetails.mI_Address1;
                            }
                            if ($scope.institutionDetails.mI_Address2 !== null && $scope.institutionDetails.mI_Address2 !== "") {
                                instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address2;
                            }
                            if ($scope.institutionDetails.mI_Address3 !== null && $scope.institutionDetails.mI_Address3 !== "") {
                                instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address3;
                            }
                            $scope.CurrentInstuteAddress = instuteAddress;
                        }

                        //get current date
                        $scope.HRES_Year = promise.hreS_Year;
                        //get DOI
                        $scope.HRES_Month = promise.hreS_Month;
                        $scope.HRME_EmployeeFirstName = promise.hrmE_EmployeeFirstName;

                        if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {
                            $scope.EmployeeDis = true;
                            $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;
                            //console.log($scope.employeeSalaryslipDetails);
                            //$scope.earningheadlist = $scope.employeeSalaryslipDetails[0].earningresult;
                            angular.forEach($scope.employeeSalaryslipDetails, function (headresult) {
                                if (headresult.conveyanceAllowance == null) { headresult.conveyanceAllowance = 0; }
                                if (headresult.HRA == null) { headresult.HRA = 0; }
                                if (headresult.LeaveEncashment == null) { headresult.LeaveEncashment = 0; }
                                if (headresult.ESI == null) { headresult.ESI = 0; }
                                if (headresult.LIC == null) { headresult.LIC = 0; }
                                if (headresult.PF == null) { headresult.PF = 0; }
                                if (headresult.ProfessionalTax == null) { headresult.ProfessionalTax = 0; }
                                headresult.totalearning = parseInt(headresult.BasicPay) + parseInt(headresult.conveyanceAllowance) + parseInt(headresult.HRA) + parseInt(headresult.LeaveEncashment);
                                headresult.totaldeduction = parseInt(headresult.ESI) + parseInt(headresult.LIC) + parseInt(headresult.PF) + parseInt(headresult.ProfessionalTax);
                            });

                            //$scope.deductionheadlist = $scope.employeeSalaryslipDetails[0].deductionresult;
                            //angular.forEach($scope.deductionheadlist, function (headresult) {
                            //    headresult.netamount = 0;
                            //});

                            //angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                            //    itm.grossEarning = Math.round(itm.grossEarning);
                            //    itm.grossDeduction = Math.round(itm.grossDeduction);
                            //    itm.netSalary = Math.round(itm.netSalary);
                            //    angular.forEach(itm.earningresult, function (result) {
                            //        result.hresD_Amount = Math.round(result.hresD_Amount);
                            //        angular.forEach($scope.earningheadlist, function (headresult) {
                            //            if (headresult.hrmeD_Name == result.hrmeD_Name) {
                            //                headresult.netamount = Math.round(headresult.netamount + result.hresD_Amount);
                            //            }
                            //        });
                            //    });
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

                    });
            }

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

            angular.forEach($scope.designationdropdown, function (itm22) {
                itm22.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;
            });

            angular.forEach($scope.departmentdropdown, function (itm232) {
                itm232.selected = toggleStatus;
                $scope.departmentselectedAll = toggleStatus;
            });

            $scope.get_depts();
        };

        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            $scope.departmentdropdown = [];
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            $scope.departmentselectedAll = false;

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
            apiService.create("YearlySalaryReport/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
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
            angular.forEach($scope.designationdropdown, function (itm1) {
                itm1.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;

            });

            $scope.get_desig();

        };


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
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
            apiService.create("YearlySalaryReport/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
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

        };

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
        };

        //Total for per column

        $scope.TotalgrossEarning = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.grossEarning;
                }
            }

            return Math.round(total);
        };

        $scope.TotalgrossDeduction = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.grossDeduction;
                }
            }

            return Math.round(total);
        };

        $scope.TotalnetSalary = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.netSalary;
                }
            }

            return Math.round(total);
        };
        $scope.exportToExcel = function (tableId) {

            //var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var exportHref = Excel.tableToExcel(tableId, 'SalaryReport');

            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);



        };



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
        };

        $scope.TotalBASIC = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.BasicPay,10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalConveyance = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.conveyanceAllowance, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalLeave = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.LeaveEncashment, 10);
                }
            }
            return Math.round(total);
        };

        $scope.Totalearning = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.totalearning, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalESI = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.ESI, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalHRA = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.HRA, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalKEB = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.KEB, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalLIC = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.LIC, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalLOAN = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.loan, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalOTHER = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.Other, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalOTHERS = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.Others, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalOthersIT = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.OthersIT, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalPF = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.PF, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalPT = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.ProfessionalTax, 10);
                }
            }
            return Math.round(total);
        };

        $scope.Totaldeduction = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.totaldeduction, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalRefund = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.Refund, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalSalaryAdvance = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.SalaryAdvance, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalSchoolShare = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.SchoolShare, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalSFCont = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.SFCont, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalSplAllow = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.SplAllow, 10);
                }
            }
            return Math.round(total);
        };

        $scope.TotalVPF = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += parseInt(product.VPF, 10);
                }
            }
            return Math.round(total);
        };
    }


})();