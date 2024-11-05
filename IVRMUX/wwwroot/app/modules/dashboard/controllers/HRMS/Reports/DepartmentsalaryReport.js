(function () {
    'use strict';
    angular
        .module('app')
        .controller('DepartmentsalaryReportController', DepartmentsalaryReportController)

    DepartmentsalaryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function DepartmentsalaryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = 10;

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("DepartmentSalaryReport/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;


                }

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

                if (promise.configurationDetails != null) {

                    $scope.SalaryFromDay = promise.configurationDetails.hrC_SalaryFromDay;
                    $scope.SalaryToDay = promise.configurationDetails.hrC_SalaryToDay;

                }

            })
        }


        //

        $scope.getTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                var product = $scope.employeeSalaryslipDetails[i];
                total += product.totalEmployees;
            }
            return Math.round(total);
        }





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
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected
                }

                apiService.create("DepartmentSalaryReport/getEmployeedetailsBySelection", data).
                    then(function (promise) {
                        if (promise.headList !== null && promise.headList.length > 0) {
                            $scope.headList = promise.headList;
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
                        $scope.HRES_Year = promise.hreS_Year;

                        //get DOI
                        $scope.HRES_Month = promise.hreS_Month;

                        if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {
                            $scope.EmployeeDis = true;

                            $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;
                            console.log($scope.employeeSalaryslipDetails);


                            $scope.earningheadlist = $scope.employeeSalaryslipDetails[0].earningresult;

                            angular.forEach($scope.earningheadlist, function (headresult) {

                                headresult.netamount = 0;

                            });

                            $scope.deductionheadlist = $scope.employeeSalaryslipDetails[0].deductionresult;

                            angular.forEach($scope.deductionheadlist, function (headresult) {

                                headresult.netamount = 0;

                            });

                            var earningamt = 0;

                            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {

                                itm.grossEarning = Math.round(itm.grossEarning);
                                itm.grossDeduction = Math.round(itm.grossDeduction);
                                itm.netSalary = Math.round(itm.netSalary);

                                //earning head totalAmount
                                angular.forEach(itm.earningresult, function (result) {

                                    //earning head



                                    result.hresD_Amount = Math.round(result.hresD_Amount);



                                    angular.forEach($scope.earningheadlist, function (headresult) {

                                        if (headresult.hrmeD_Name == result.hrmeD_Name) {

                                            headresult.netamount = Math.round(headresult.netamount + result.hresD_Amount);
                                        }





                                    });




                                });
                                //deduction head totalAmount
                                angular.forEach(itm.deductionresult, function (result) {

                                    result.hresD_Amount = Math.round(result.hresD_Amount);

                                    angular.forEach($scope.deductionheadlist, function (headresult) {
                                        //if (headresult.hrmeD_Name=="INCOME TAX") {
                                            if (headresult.hrmeD_Name == result.hrmeD_Name && result.hrmeD_Name != 'SENIOR SCHOOL' && result.hrmeD_Name != 'JUNIOR SCHOOL') {

                                                headresult.netamount = Math.round(headresult.netamount + result.hresD_Amount);
                                            }
                                        //}

                                        

                                    });

                                });
                            });
                            $scope.earnlen = $scope.earningheadlist.length;
                            $scope.dedlen = $scope.deductionheadlist.length;
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }
                        console.log($scope.employeeSalaryslipDetails);

                        // $scope.employeeSalaryslipDetails
                        var HRA = 0; var BasicPay = 0; var PERSONALPAY = 0; var DA = 0; var CLAMT = 0; var MEDICA = 0;
                        var COORDINATOR = 0; var COMPENSATORYALLOWANCE = 0; var TRANSPORT = 0; var RESPONSIBILITY = 0;
                        var INTERIMRELIEF = 0; var MISC = 0; var BasicManualAdj = 0; var GrossEarning = 0; var PF = 0;
                        var VPF = 0; var PTAX = 0; var INCOMETAX = 0; var ESI = 0; var ADVANCE = 0; var MISCDEDUCTION = 0;
                        var GrossEarning = 0; var grossDeduction = 0; var SPECIALALL = 0;
                        angular.forEach($scope.employeeSalaryslipDetails, function (employeeSalaryslipDetails) {
                            if (employeeSalaryslipDetails.hrmD_DepartmentName == 'SENIOR SCHOOL' || employeeSalaryslipDetails.hrmD_DepartmentName == 'JUNIOR SCHOOL') {
                                angular.forEach(employeeSalaryslipDetails.earningresult, function (earningresult) {
                                    if (earningresult.hrmeD_Name == "Basic Pay") {
                                        BasicPay += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = BasicPay;
                                    }
                                    //SPECIAL ALL
                                    if (earningresult.hrmeD_Name == "SPECIAL ALL") {
                                        SPECIALALL += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = SPECIALALL;
                                    }
                                    if (earningresult.hrmeD_Name == "HRA") {
                                        HRA += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = HRA;
                                    }
                                    if (earningresult.hrmeD_Name == "DA") {
                                        DA += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = DA;
                                    }
                                    if (earningresult.hrmeD_Name == "PERSONAL PAY") {
                                        PERSONALPAY += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = PERSONALPAY;
                                    }
                                    if (earningresult.hrmeD_Name == "CL AMT") {
                                        CLAMT += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = CLAMT;
                                    }
                                    if (earningresult.hrmeD_Name == "MEDICAL") {
                                        MEDICA += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = MEDICA;
                                    }
                                    if (earningresult.hrmeD_Name == "CO ORDINATOR") {
                                        COORDINATOR += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = COORDINATOR;
                                    }
                                    if (earningresult.hrmeD_Name == "COMPENSATORY ALLOWANCE") {
                                        COMPENSATORYALLOWANCE += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = COMPENSATORYALLOWANCE;
                                    }
                                    if (earningresult.hrmeD_Name == "TRANSPORT") {
                                        TRANSPORT += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = TRANSPORT;
                                    }
                                    if (earningresult.hrmeD_Name == "RESPONSIBILITY") {
                                        RESPONSIBILITY += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = RESPONSIBILITY;
                                    }
                                    if (earningresult.hrmeD_Name == "INTERIM RELIEF") {
                                        INTERIMRELIEF += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = INTERIMRELIEF;
                                    }
                                    if (earningresult.hrmeD_Name == "MISC") {
                                        MISC += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = MISC;
                                    }
                                    if (earningresult.hrmeD_Name == "MISC") {
                                        MISC += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = MISC;
                                    }
                                    if (earningresult.hrmeD_Name == "Basic Manual Adj") {
                                        BasicManualAdj += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = BasicManualAdj;
                                    }
                                    if (earningresult.hrmeD_Name == "Gross Earning") {
                                        GrossEarning += (Number(earningresult.hresD_Amount));
                                        earningresult.hresD_Amount = GrossEarning;
                                    }
                                    //Gross Earning
                                });
                                angular.forEach(employeeSalaryslipDetails.deductionresult, function (headresult) {

                                    if (headresult.hrmeD_Name == "P F") {
                                        PF += (Number(headresult.hresD_Amount));
                                        headresult.hresD_Amount = PF;
                                    }
                                    if (headresult.hrmeD_Name == "V PF") {
                                        VPF += (Number(headresult.hresD_Amount));
                                        headresult.hresD_Amount = VPF;
                                    }
                                    if (headresult.hrmeD_Name == "P TAX") {
                                        PTAX += (Number(headresult.hresD_Amount));
                                        headresult.hresD_Amount = PTAX;
                                    }
                                    if (headresult.hrmeD_Name == "INCOME TAX") {
                                        INCOMETAX += (Number(headresult.hresD_Amount));
                                        headresult.hresD_Amount = INCOMETAX;
                                    }
                                    if (headresult.hrmeD_Name == "ESI") {
                                        ESI += (Number(headresult.hresD_Amount));
                                        headresult.hresD_Amount = ESI;
                                    }
                                    if (headresult.hrmeD_Name == "ADVANCE") {
                                        ADVANCE += (Number(headresult.hresD_Amount));
                                        headresult.hresD_Amount = ADVANCE;
                                    }
                                    if (headresult.hrmeD_Name == "MISC DEDUCTION") {
                                        MISCDEDUCTION += (Number(headresult.hresD_Amount));
                                        headresult.hresD_Amount = MISCDEDUCTION;
                                    }
                                });
                                GrossEarning = (Number(employeeSalaryslipDetails.grossEarning) + (Number(GrossEarning)));
                                employeeSalaryslipDetails.grossEarning = GrossEarning;
                                grossDeduction = (Number(employeeSalaryslipDetails.grossDeduction) + (Number(grossDeduction)));
                                employeeSalaryslipDetails.grossDeduction = grossDeduction;

                            }

                        });
                    })
                //employeeSalaryslipDetails



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
            apiService.create("CumulativeSalaryReport/get_depts", data).
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
            apiService.create("CumulativeSalaryReport/get_desig", data).
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

        $scope.TotalgrossEarning = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    if ($scope.employeeSalaryslipDetails[i].hrmD_DepartmentName != 'SENIOR SCHOOL') {
                        var product = $scope.employeeSalaryslipDetails[i];
                        total += product.grossEarning;
                    }

                  
                }
            }

            return Math.round(total);
        }

        $scope.TotalgrossDeduction = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {

                    if ($scope.employeeSalaryslipDetails[i].hrmD_DepartmentName != 'SENIOR SCHOOL') {
                        var product = $scope.employeeSalaryslipDetails[i];
                        total += product.grossDeduction;
                    }

                   
                }
            }

            return Math.round(total);
        }

        $scope.TotalnetSalary = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    if ($scope.employeeSalaryslipDetails[i].hrmD_DepartmentName != 'SENIOR SCHOOL') {
                        var product = $scope.employeeSalaryslipDetails[i];
                        var asd = product.grossEarning - product.grossDeduction;
                        total += asd;
                    }
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


    }


})();