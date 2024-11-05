(function () {
    'use strict';
    angular
        .module('app')
        .controller('SthomasPaySlipController', SthomasPaySlipController)

    SthomasPaySlipController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function SthomasPaySlipController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object


        $scope.EmployeeDis = false;
        $scope.Emp = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.employeeLeaveDetails = [];

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeSalarySlipGeneration/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                //if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                //    $scope.employeedropdown = promise.employeedropdown;
                //}

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

            })
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
            apiService.create("EmployeeSalarySlipGeneration/get_depts", data).
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
            apiService.create("EmployeeSalarySlipGeneration/get_desig", data).
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



        //GetEmployeeDetailsByLeaveYearAndMonth
        $scope.submitted = false;
        $scope.GetEmployeeList = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            $scope.employeedropdown = [];
            $scope.Emp = {};
            if ($scope.Employee.hreS_Year != "" && $scope.Employee.hreS_Year != undefined && $scope.Employee.hreS_Month != "" && $scope.Employee.hreS_Month != undefined) {

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

                apiService.create("EmployeeSalarySlipGeneration/GetEmployeeDetailsByLeaveYearAndMonth", data).
                    then(function (promise) {

                        if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                            $scope.employeedropdown = promise.employeedropdown;
                            $scope.employeedropdown_mail = promise.employeedropdown;
                        }
                        else {
                            $scope.employeedropdown = [];
                            $scope.employeedropdown_mail = [];

                            swal("Employees salary Not generated for " + $scope.Employee.hreS_Month + "-" + $scope.Employee.hreS_Year);
                        }
                    })
            }
        }



        $scope.employeeSalaryslipDetails = [];
        $scope.institutionDetails = {};
        $scope.empdetails = {};
        $scope.submitted = false;
        $scope.GenerateSalarySlip = function () {
            //debugger;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }


            var groupTypeselected = [];
            angular.forEach($scope.groupTypedropdown, function (itm) {
                if (itm.selected) {
                    groupTypeselected.push(itm.hrmgT_Id);//
                }

            });
            $scope.esary = [];
            $scope.dsary = [];
            //  $scope.items = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.institutionDetails = {};
                $scope.empdetails = {};
                $scope.EmployeeDis = false;
                if ($scope.EmployeeDis) {
                    $scope.EmployeeDis = false;
                }
                $scope.groupTypeselected = [];
                $scope.departmentselected = [];
                $scope.designationselected = [];
                $scope.earningDetails = [];
                $scope.deductionDetails = [];
                $scope.esary = [];
                $scope.dsary = [];
                $scope.items = [];
                //   $scope.totals = "";
                //   $scope.empsaldetail = {};
                //   $scope.NetSalary = "";

                $scope.employeeSalaryslipDetails = [];
                $scope.employeeLeaveDetails = [];
                var emp_ids = [];
                angular.forEach($scope.employeedropdown, function (emp) {
                    if (emp.selected) {
                        emp_ids.push(emp.hrmE_Id);
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
                var data =
                {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected,
                    empid: emp_ids


                };

                // var data = $scope.Employee;
                apiService.create("EmployeeSalarySlipGeneration/GenerateEmployeeSalarySlip", data).
                    then(function (promise) {
                        if (promise.main_list.length > 0 && promise.main_list != null) {
                            //Earning Deduction Details
                            $scope.EmployeeDis = true;
                            for (var i = 0; i < promise.main_list.length; i++) {
                                if (promise.main_list[i].employeeSalaryslipDetails != null && promise.main_list[i].employeeSalaryslipDetails.length != 0) {

                                    var items = promise.main_list[i].employeeSalaryslipDetails;

                                    var getPortion = function (label) {
                                        var sum = 0;
                                        var out = items.filter(function (x) {
                                            var match = x.HRMED_EarnDedFlag == label;
                                            if (match)
                                                sum += x.Amount;
                                            return match;
                                        });
                                        return { out, sum }
                                    };

                                    var es = getPortion('Earning');
                                    var ds = getPortion('Deduction');
                                    promise.main_list[i].esary = es;
                                    promise.main_list[i].dsary = ds;
                                    if (es.out.length >= ds.out.length) {
                                        for (var item of ds.out)
                                            es.out[ds.out.indexOf(item)].ds = item;
                                        promise.main_list[i].items = es.out;
                                    } else {

                                        for (var item of es.out)
                                            ds.out[es.out.indexOf(item)].ds = item;
                                        promise.main_list[i].items = ds.out;
                                    }
                                    //debugger;
                                    // console.log($scope.items);
                                    //$scope.main_list = promise.main_list.items;
                                    //console.log($scope.main_list);
                                    promise.main_list[i].totals = [es.sum, ds.sum];
                                    promise.main_list[i].earningtotal = [es.sum];
                                    promise.main_list[i].deducttotal = [ds.sum];

                                  
                                    
                                    var LopAmount = 0;
                                    if (promise.main_list[i].payrollStandard.hrC_PayMethodFlg == "Method1") {
                                        LopAmount = promise.main_list[i].lopAmount;
                                    }
                                    else {
                                        LopAmount = 0;
                                    }
                                    //Employee Salary Details
                                    promise.main_list[i].empsaldetail = promise.main_list[i].empsaldetail;

                                    //promise.main_list[i].NetSalary = Math.round((Math.ceil(parseFloat(promise.main_list[i].totals[0]) - parseFloat(promise.main_list[i].totals[1]))) - LopAmount,2);
                                    promise.main_list[i].NetSalary = Math.round((parseFloat(promise.main_list[i].totals[0]) - parseFloat(promise.main_list[i].totals[1])) - parseFloat(LopAmount), 2);

                                    promise.main_list[i].NetAmountInwords = toWords(promise.main_list[i].NetSalary) + 'Rupees only.';



                                    //Institute Details
                                    if (promise.main_list[i].institutionDetails != null) {


                                        promise.main_list[i].selectedMonth = promise.main_list[i].hreS_Month;
                                        promise.main_list[i].selectedYear = promise.main_list[i].hreS_Year;

                                        promise.main_list[i].institutionDetails = promise.main_list[i].institutionDetails;

                                        if (promise.main_list[i].institutionDetails.mI_Logo == "" || promise.main_list[i].institutionDetails.mI_Logo == null) {
                                            promise.main_list[i].institutionDetails.mI_Logo = "../../../../../../images/uploads/studentDocumnets/2/36ed82c1-b373-4358-af12-4397bacda396.jpg";
                                        }


                                        //  $('#blah').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.institutionDetails.mi_id + "/" + "EmployeeProfilePics" + "/" + $scope.institutionDetails.mI_Logo);

                                        var instuteAddress = "";
                                        if (promise.main_list[i].institutionDetails.mI_Address1 != null && promise.main_list[i].institutionDetails.mI_Address1 != "") {
                                            instuteAddress = promise.main_list[i].institutionDetails.mI_Address1;
                                        }
                                        if (promise.main_list[i].institutionDetails.mI_Address2 != null && promise.main_list[i].institutionDetails.mI_Address2 != "") {

                                            instuteAddress = instuteAddress + ',' + promise.main_list[i].institutionDetails.mI_Address2;
                                        }

                                        if (promise.main_list[i].institutionDetails.mI_Address3 != null && promise.main_list[i].institutionDetails.mI_Address3 != "") {

                                            instuteAddress = instuteAddress + ',' + promise.main_list[i].institutionDetails.mI_Address3;
                                        }

                                        promise.main_list[i].CurrentInstuteAddress = instuteAddress;

                                    }


                                    //Employee Details
                                    if (promise.main_list[i].currentemployeeDetails != null) {
                                        promise.main_list[i].empdetails = promise.main_list[i].currentemployeeDetails;

                                        promise.main_list[i].empdetails.hrmE_DOJ = new Date(promise.main_list[i].empdetails.hrmE_DOJ);

                                        promise.main_list[i].DesignationName = promise.main_list[i].designationName;
                                        promise.main_list[i].DepartmentName = promise.main_list[i].departmentName;
                                    }
                                    //debugger;

                                    if (promise.main_list[i].employeeLeaveDetails != null && promise.main_list[i].employeeLeaveDetails.length > 0) {

                                        promise.main_list[i].employeeLeaveDetails = promise.main_list[i].employeeLeaveDetails;
                                    } else {
                                        promise.main_list[i].employeeLeaveDetails = [];
                                    }

                                }

                            }
                        }
                       // debugger;
                        $scope.all_employees = promise.main_list;
                        console.log($scope.all_employees);

                    })
            }

        }

        $scope.onchangeEmployee = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.submitted = false;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";
            $scope.Emp = {};
            $scope.employeeLeaveDetails = [];
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //

        var th = ['', 'thousand', 'million', 'billion', 'trillion'];
        var dg = ['Zero', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine'];
        var tn = ['Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
        var tw = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];


        function toWords(s) {
            s = s.toString();
            s = s.replace(/[\, ]/g, '');
            if (s != parseFloat(s)) return 'not a number';
            var x = s.indexOf('.');
            if (x == -1) x = s.length;
            if (x > 15) return 'too big';
            var n = s.split('');
            var str = '';
            var sk = 0;
            for (var i = 0; i < x; i++) {
                if ((x - i) % 3 == 2) {
                    if (n[i] == '1') {
                        str += tn[Number(n[i + 1])] + ' ';
                        i++;
                        sk = 1;
                    }
                    else if (n[i] != 0) {
                        str += tw[n[i] - 2] + ' ';
                        sk = 1;
                    }
                }
                else if (n[i] != 0) {
                    str += dg[n[i]] + ' ';
                    if ((x - i) % 3 == 0) str += 'hundred ';
                    sk = 1;
                }


                if ((x - i) % 3 == 1) {
                    if (sk) str += th[(x - i - 1) / 3] + ' ';
                    sk = 0;
                }
            }
            if (x != s.length) {
                var y = s.length;
                str += 'point ';
                for (var i = x + 1; i < y; i++) str += dg[n[i]] + ' ';
            }
            return str.replace(/\s+/g, ' ');
        }




        //$scope.printData = function (Employee) {
        //    debugger;
        //    if (Employee.salcount == "1") {
        //        var innerContents = document.getElementById("EmpPaySlip").innerHTML;
        //        var popupWinindow = window.open('');
        //        popupWinindow.document.open();
        //        popupWinindow.document.write('<html><head>' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EmpPaySlipPdf.css" />' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        //            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //        popupWinindow.document.close();
        //    } else if (Employee.salcount == "2") {
        //        var innerContents = document.getElementById("EmpPaySlip").innerHTML;
        //        var popupWinindow = window.open('');
        //        popupWinindow.document.open();
        //        popupWinindow.document.write('<html><head>' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanpdf1.css" />' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        //            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //        popupWinindow.document.close();
        //    }

        //}


        $scope.printData = function (Employee) {
            //debugger;
            if (Employee.salcount == "1") {
                var innerContents = document.getElementById("EmpPaySlip").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    //'<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    //'<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/EmpPaySlipPdf.css" />' +
                    //'<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                    //'<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                  //  '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            } else if (Employee.salcount == "2") {
                var innerContents = document.getElementById("EmpPaySlip").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    //'<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    //'<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/EmpPaySlipPdf1.css" />' +
                    //'<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                    //'<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                   '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                   // '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        }

        $scope.export = function () {
            debugger;
            $scope.tableId11 = [];
            for (var x = 0; x < $scope.all_employees.length; x++) {
                $scope.tableId = 'table' + $scope.all_employees[x].hrmE_Id;
                $scope.tableId11.push({ sal: 'salaryslip' + $scope.all_employees[x].hrmE_Id, id: 'table' + $scope.all_employees[x].hrmE_Id, Name: 'Salary Slip Of Employee ' + $scope.all_employees[x].empdetails.hrmE_EmployeeCode });
                console.log($scope.tableId11);


            }
            angular.forEach($scope.tableId11, function (obj) {
                html2canvas(document.getElementById(obj.id), {
                    onrendered: function (canvas) {

                        var data = canvas.toDataURL();
                        var docDefinition = {
                            content: [{
                                image: data,
                                width: 500,
                            }],


                        };



                        pdfMake.createPdf(docDefinition).download(obj.Name);


                    }
                });
            })
        }




        $scope.SendMail = function (all_employees) {
            debugger;
            $scope.tmplt = [];
            angular.forEach($scope.all_employees, function (obj) {

                var Template = document.getElementById(obj.currentemployeeDetails.hrmE_EmployeeCode).innerHTML;

                $scope.tmplt.push({ hrmE_EmployeeCode: obj.currentemployeeDetails.hrmE_EmployeeCode, TemplateString: Template });
            })



            $scope.temp_list = [];
            debugger;
            var cnt = 0;
            var emp_ids = [];
            angular.forEach($scope.employeedropdown, function (emp) {
                if (emp.selected) {
                    emp_ids.push(emp.hrmE_Id);
                }
            });
            angular.forEach($scope.employeedropdown_mail, function (itm) {
                if (itm.selected_mail) {
                    if (itm.SalSlip != null || itm.SalSlip != undefined) {
                        cnt += 1;
                        $scope.temp_list.push(itm);
                    }
                }
            })
            if (cnt == 0) {
                $scope.Employee.Template = $scope.tmplt;
                $scope.Employee.EmailSMS = "Email"

                var data =
                {
                    "EmailSMS": $scope.Employee.EmailSMS,
                    empid: emp_ids,
                    Template: $scope.tmplt,
                    "HRES_Month": $scope.Employee.hreS_Month
                };

                apiService.create("EmployeeSalarySlipGeneration/SendEmailSMS", data).
                    then(function (promise) {
                        if (promise.retrunMsg == "success") {

                            swal("Email Sent..!!!");

                        }
                        else if (promise.retrunMsg == "notFound") {

                            swal("Email Not sent..!!!", 'Default Email-Id is Not Found.. !!!');
                        }
                        else if (promise.retrunMsg == "Error") {
                            swal("Something went wrong", 'Try After some time..!!');
                        } else {
                            swal("Something went wrong", 'Try After some time..!!');
                        }


                    })

            }
            else if (cnt < 0) {
                swal("Attach file for selected Employees");
            }

        }


        $scope.SendSMS = function () {

            //var innerContents = document.getElementById("Baldwin").innerHTML;
            //var Template = '<html><head>' +

            //'</head><body>' + innerContents + '</body></html>';




            debugger;

            // $scope.Employee.Template = Template;
            $scope.Employee.EmailSMS = "SMS"
            //var data = $scope.Employee;
            $scope.tmplt = [];
            angular.forEach($scope.all_employees, function (obj) {

                var Template = document.getElementById(obj.currentemployeeDetails.hrmE_EmployeeCode).innerHTML;

                $scope.tmplt.push({ hrmE_EmployeeCode: obj.currentemployeeDetails.hrmE_EmployeeCode, TemplateString: Template });
            })
            var emp_ids = [];
            angular.forEach($scope.employeedropdown, function (emp) {
                if (emp.selected) {
                    emp_ids.push(emp.hrmE_Id);
                }
            });
            angular.forEach($scope.employeedropdown_mail, function (itm) {
                if (itm.selected_mail) {
                    if (itm.SalSlip != null || itm.SalSlip != undefined) {
                        cnt += 1;
                        $scope.temp_list.push(itm);
                    }
                }
            })
            var data =
            {
                "EmailSMS": $scope.Employee.EmailSMS,
                "HRES_Month": $scope.Employee.hreS_Month,
                empid: emp_ids,
                Template: $scope.tmplt
            };

            apiService.create("EmployeeSalarySlipGeneration/SendEmailSMS", data).
                then(function (promise) {
                    if (promise.retrunMsg != "") {
                        if (promise.retrunMsg == "success") {

                            swal("SMS Sent..!!!");
                        } else if (promise.retrunMsg == "notFound") {

                            swal("SMS Not sent..!!!", 'Default Mobile No. is Not Found.. !!!');
                        }

                        else if (promise.retrunMsg == "Error") {
                            swal("Something went wrong", 'Try After some time..!!');
                        } else {
                            swal(promise.retrunMsg);
                        }
                    } else {
                        swal("Something went wrong", 'Try After some time..!!');
                    }

                })




        }

        //var jsreport = require('jsreport-core')()

        //jsreport.init().then(function () {
        //    return jsreport.render({
        //        template: {
        //            content: '<h1>Hello {{:foo}}</h1>',
        //            engine: 'jsrender',
        //            recipe: 'phantom-pdf'
        //        },
        //        data: {
        //            foo: "world"
        //        }

        //    }).then(function (resp) {
        //        debugger;
        //        //prints pdf with headline Hello world
        //        console.log(resp.content.toString())
        //    });
        //}).catch(function (e) {
        //    console.log(e)
        //    debugger;
        //})


        $scope.chk_employeeall = function (all_e) {
            angular.forEach($scope.employeedropdown, function (emp) {
                emp.selected = all_e;
            })
        }
        $scope.chk_employee = function (emp) {

            $scope.employeeSelectedAll = $scope.employeedropdown.every(function (itm) {

                return itm.selected;
            });
        }
        $scope.chk_employeeall_mail = function (all_e) {
            angular.forEach($scope.employeedropdown_mail, function (emp) {
                emp.selected_mail = all_e;
            })
        }
        $scope.chk_employee_mail = function (emp) {

            $scope.employeeSelectedAll_mail = $scope.employeedropdown_mail.every(function (itm) {

                return itm.selected_mail;
            });
        }




        $scope.UploadStudentProfilePic = [];
        $scope.images_temp = [];

        $scope.uploadStudentProfilePic = function (input, document) {
            debugger;
            $scope.UploadStudentProfilePic = input.files;

            for (var a = 0; a < $scope.UploadStudentProfilePic.length; a++) {

                if (input.files && input.files[a]) {

                    if (input.files[a].size <= 2097152) {//(input.files[a].type == "image/jpeg" || input.files[a].type == "image/png") &&

                        var count = 0;
                        angular.forEach($scope.images_temp, function (imgt123) {
                            if (imgt123.name == input.files[a].name) {
                                count += 1;
                            }
                        });

                        if (count == 0) {
                            $scope.images_temp.push(input.files[a]);
                        }
                        var reader = new FileReader();
                        // var id = input.files[a].name;
                        reader.onload = function (e) {
                            debugger;

                            $('#blah')
                                .attr('src', e.target.result)

                        };
                        reader.readAsDataURL(input.files[a]);
                        //$scope.images_temp.push(input.files[a]);
                    }
                    else if (input.files[a].size > 2097152) {
                        swal("Image size should be less than 2MB");
                        return;
                    }
                }

            }

        }
    }


})();