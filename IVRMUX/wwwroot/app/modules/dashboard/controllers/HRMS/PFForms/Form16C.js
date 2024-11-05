(function () {
    'use strict';
    angular
        .module('app')
        .controller('form16CController', form16CController)

    form16CController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function form16CController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object


        $scope.EmployeeDis = false;
        $scope.Emp = {};

        $scope.employeeLeaveDetails = [];

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("Form16/getalldetails", pageid).then(function (promise) {

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

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;

                }

            })
        }


        $scope.totalAmount = function () {
            var total = 0;
            for (count = 0; count < $scope.tdsheads.length; count++) {
                total += parseInt($scope.data[count].total_amount, 10);
            }
            return total;
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
            $scope.GetEmployeeList();
        }


        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.GetEmployeeList();

        }


        //By Department
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;

            });

            $scope.GetEmployeeList();

        }


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });

            $scope.GetEmployeeList();
        }

        $scope.onselectyear = function () {
            var year = $scope.Employee.imfY_Id;
            $scope.empFinancialYear = year;

            angular.forEach($scope.leaveyeardropdown, function (value, key) {


                if (value.imfY_Id == $scope.empFinancialYear) {
                    var fromdates = value.imfY_FromDate.split('T');
                    value.imfY_FromDate = fromdates;
                    var tdate = value.imfY_ToDate.split('T');
                    value.imfY_ToDate = tdate;


                    $scope.fromdatesd = fromdates[0];


                    $scope.todatesd = tdate[0];
                    var assessment = value.imfY_AssessmentYear;
                    $scope.leaveyeardropdownss = assessment;
                }

            });

        }

        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;

            });
            $scope.GetEmployeeList();

        }


        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.GetEmployeeList();
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

                apiService.create("Form16/GetEmployeeDetailsByLeaveYearAndMonth", data).
                    then(function (promise) {

                        if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                            $scope.employeedropdown = promise.employeedropdown;
                        }
                        else {
                            $scope.employeedropdown = [];

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
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            $scope.esary = [];
            $scope.dsary = [];

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.institutionDetails = {};
                $scope.empdetails = {};
                $scope.EmployeeDis = false;
                if ($scope.EmployeeDis) {
                    $scope.EmployeeDis = false;
                }
                $scope.earningDetails = [];
                $scope.deductionDetails = [];
                $scope.esary = [];
                $scope.dsary = [];
                $scope.items = [];

                $scope.employeeSalaryslipDetails = [];
                $scope.employeeLeaveDetails = [];

                //  $scope.onselectyear();
                var data1 = $scope.Employee;

                var data =
                    {
                        "HRME_Id": $scope.Employee.hrmE_Id,
                        "IMFY_Id": $scope.Employee.imfY_Id,
                        "IMFY_FromDate": $scope.fromdatesd,
                        "IMFY_ToDate": $scope.todatesd,
                    }

                apiService.create("Form16/GenerateEmployeeSalarySlip", data).
                    then(function (promise) {
                        $scope.EmployeeDis = true;
                        //Earning Deduction Details
                        //if (promise.employeeSalaryslipDetails != null && promise.employeeSalaryslipDetails.length != 0) {
                        $scope.EmployeeDis = true;

                        var items = promise.employeeSalaryslipDetails;


                        //Institute Details
                        if (promise.institutionDetails != null) {


                            $scope.selectedMonth = promise.hreS_Month;
                            $scope.selectedYear = promise.hreS_Year;

                            $scope.institutionDetails = promise.institutionDetails;

                            if ($scope.institutionDetails.mI_Logo == "" || $scope.institutionDetails.mI_Logo == null) {
                                $scope.institutionDetails.mI_Logo = "../../../../../../images/uploads/studentDocumnets/2/36ed82c1-b373-4358-af12-4397bacda396.jpg";
                            }


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


                            $scope.tdsheads = promise.tdsheads;
                          

                           
                            $scope.masterch = promise.masterch;
                            $scope.quarterheads = $scope.quarterheads1;
                            $scope.professionaltaxamount = promise.professionaltaxamount;
                            $scope.allowancelist = promise.allowancelist;
                            $scope.masterallowance = promise.masterallowance;
                            //Employee Details
                            $scope.receit = promise.receit;
                            //  $scope.receipt
                            $scope.chapterlist = promise.chapterlist;
                            $scope.otherincomelist = promise.otherincomelist;
                            $scope.empdetails = promise.currentemployeeDetails;

                            $scope.empdetails.hrmE_DOJ = new Date($scope.empdetails.hrmE_DOJ);
                            $scope.empdetails.hrmE_DOB = new Date($scope.empdetails.hrmE_DOB);
                            $scope.tdsheads = promise.tdsheads;
                            $scope.designationName = promise.designationName;
                            $scope.departmentName = promise.departmentName;

                            $scope.empGrossSal = promise.empGrossSal;
                            var totalgrp = 0;
                            angular.forEach($scope.tdsheads, function (cls) {
                                totalgrp += cls.hretdS_TaxDeposited;
                            })
                            $scope.hretdS_TaxDeposited_total = totalgrp;



                            var totalallowance = 0;
                            angular.forEach($scope.allowancelist, function (cls) {
                                totalallowance += cls.hreaL_Allowance;
                            })
                            $scope.totalallowance = totalallowance;



                            var totalotherincome = 0;
                            angular.forEach($scope.otherincomelist, function (cls) {
                                totalotherincome += cls.hreoI_OtherIncome;
                            })
                            $scope.totalincome = totalotherincome;


                            var totalvalue = 0;
                            angular.forEach($scope.chapterlist, function (cls) {
                                totalvalue += cls.hrecviA_Amount;
                            })
                            $scope.totalabc = totalvalue;

                              $scope.totalabc1 = $scope.totalabc + $scope.pfvalue + $scope.licvalue;



                            $scope.hretdS_TaxDeposited_totalwords = toWords($scope.hretdS_TaxDeposited_total)



                         
                            var subtraction = $scope.empGrossSal - $scope.totalallowance;
                            $scope.balance = subtraction;
                            var afterprofessional = $scope.balance - $scope.professionaltaxamount;
                            $scope.balanceaferprofessional = afterprofessional;
                            var percent = promise.hrC_EducationCess;
                            var valueforeight = $scope.balanceaferprofessional + $scope.totalincome;
                            $scope.valueforeighttotal = valueforeight;

                            //
                            var valueforten = $scope.valueforeighttotal - $scope.totalabc1;
                            $scope.valuebindten = valueforten;
                        

                            if (promise.employeeLeaveDetails != null && promise.employeeLeaveDetails.length > 0) {


                                $scope.employeeLeaveDetails = promise.employeeLeaveDetails;
                            } else {
                                $scope.employeeLeaveDetails = [];
                            }


                           





                            $scope.tdsheads = promise.tdsheads;
                            var Q1sum = 0;
                            var Q2sum = 0;
                            var Q3sum = 0;
                            var Q4sum = 0;
                          
                            //}

                            var quarterheads1 = [];
                            $scope.quarterheads2 = promise.quarterheads;
                            $scope.quarterheads1 = promise.receit;
                            for (var j = 0; j < $scope.tdsheads.length; j++) {
                                var c_Date = new Date($scope.tdsheads[j].hretdS_DepositedDate).toDateString();
                                for (var i = 0; i < $scope.quarterheads1.length; i++) {
                                    var f_Date = new Date($scope.quarterheads1[i].hrmQ_FromDay).toDateString();
                                    var t_Date = new Date($scope.quarterheads1[i].hrmQ_ToDay).toDateString();

                                    var dateFrom1 = new Date($scope.quarterheads1[i].hrmQ_FromDay).getDate();
                                    var dateTo1 = new Date($scope.quarterheads1[i].hrmQ_ToDay).getDate();
                                    var dateCheck1 = new Date($scope.tdsheads[j].hretdS_DepositedDate).getDate();

                                    var dateFrom2 = new Date($scope.quarterheads1[i].hrmQ_FromDay).getMonth();
                                    var dateTo2 = new Date($scope.quarterheads1[i].hrmQ_ToDay).getMonth();
                                    var dateCheck2 = new Date($scope.tdsheads[j].hretdS_DepositedDate).getMonth();

                                    var dateFrom3 = new Date($scope.quarterheads1[i].hrmQ_FromDay).getFullYear();
                                    var dateTo3 = new Date($scope.quarterheads1[i].hrmQ_ToDay).getFullYear();
                                    var dateCheck3 = new Date($scope.tdsheads[j].hretdS_DepositedDate).getFullYear();

                                    var dateFrom = dateFrom1 + '/' + dateFrom2 + '/' + dateFrom3;
                                    var dateTo = dateTo1 + '/' + dateTo2 + '/' + dateTo3;
                                    var dateCheck = dateCheck1 + '/' + dateCheck2 + '/' + dateCheck3;

                                    var d1 = dateFrom.split("/");
                                    var d2 = dateTo.split("/");
                                    var c = dateCheck.split("/");

                                    var from = new Date(d1[2], parseInt(d1[1]) - 1, d1[0]);  // -1 because months are from 0 to 11
                                    var to = new Date(d2[2], parseInt(d2[1]) - 1, d2[0]);
                                    var check = new Date(c[2], parseInt(c[1]) - 1, c[0]);

                                    if (check >= from && check < to) {
                                        if (i == 0) {
                                            if ($scope.tdsheads.length > j) {
                                                Q1sum += $scope.tdsheads[j].hretdS_TaxDeposited;
                                                $scope.quarterheads1[i].sumvalue = Q1sum;
                                            }
                                        }
                                        else if (i == 1) {
                                            if ($scope.tdsheads.length > j) {
                                                Q2sum += $scope.tdsheads[j].hretdS_TaxDeposited;
                                                $scope.quarterheads1[i].sumvalue = Q2sum;
                                            }
                                        }
                                        else if (i == 2) {
                                            if ($scope.tdsheads.length > j) {
                                                Q3sum += $scope.tdsheads[j].hretdS_TaxDeposited;
                                                $scope.quarterheads1[i].sumvalue = Q3sum;
                                            }
                                        }
                                        else if (i == 3) {
                                            if ($scope.tdsheads.length > j) {
                                                Q4sum += $scope.tdsheads[j].hretdS_TaxDeposited;
                                                $scope.quarterheads1[i].sumvalue = Q4sum;
                                            }
                                        }

                                    }
                                }
                            }
                            $scope.quarterheads = $scope.quarterheads1;
                            $scope.professionaltaxamount = promise.professionaltaxamount;
                            $scope.allowancelist = promise.allowancelist;
                            $scope.masterallowance = promise.masterallowance;
                            //Employee Details

                            $scope.chapterlist = promise.chapterlist;
                            $scope.otherincomelist = promise.otherincomelist;
                            $scope.empdetails = promise.currentemployeeDetails;
                            $scope.pfvalue = promise.pfvalue;
                            $scope.licvalue = promise.licvalue;
                            $scope.empdetails.hrmE_DOJ = new Date($scope.empdetails.hrmE_DOJ);

                            $scope.designationName = promise.designationName;
                            $scope.departmentName = promise.departmentName;

                            //debugger;
                            $scope.empGrossSal = promise.empGrossSal;
                            var totalgrp = 0;
                            angular.forEach($scope.tdsheads, function (cls) {
                                totalgrp += cls.hretdS_TaxDeposited;
                            })
                            $scope.hretdS_TaxDeposited_total = totalgrp;
                            $scope.hretdS_TaxDeposited_totalwords = toWords($scope.hretdS_TaxDeposited_total) + 'Only.';

                            $scope.totalabc1 = $scope.totalabc + $scope.pfvalue + $scope.licvalue;
                            var valueforten = $scope.valueforeighttotal - $scope.totalabc1;
                            $scope.valuebindtens = valueforten;

                            var calulation1 = 0.000;
                            $scope.calulation = promise.calculation;

                            $scope.agediff = promise.birthyear;
                            angular.forEach($scope.calulation, function (cls1) {
                               // $scope.ageee = cls1.hrmE_DOJ;
                                if (cls1.hrmitD_AmountFrom < valueforten && cls1.hrmitD_AmountTo > valueforten && $scope.agediff > cls1.hrmiT_FromAge && $scope.agediff < cls1.hrmiT_ToAge) {
                                    calulation1 = cls1.hrmitD_IncomeTax;
                                }
                            })
                            var taxonincomecal = calulation1 / 100;
                            $scope.acd = taxonincomecal;
                            var valuebindten = $scope.valuebindtens * taxonincomecal;

                            $scope.valuebindten = valuebindten;


                            var percent = promise.hrC_EducationCess;

                              var percentage = percent / 100;
                            var valuefinal = percentage * $scope.valuebindten;
                           $scope.dollarvalue = valuefinal;
                        }

                        if (promise.employeeLeaveDetails != null && promise.employeeLeaveDetails.length > 0) {

                            $scope.employeeLeaveDetails = promise.employeeLeaveDetails;
                        } else {
                            $scope.employeeLeaveDetails = [];
                        }

                       
                        $scope.onselectyear()

                      
                    })
            }

        }

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


        $scope.printData = function () {
            var divToPrint = document.getElementById("Table");
            var newWin = window.open();
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        }
        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }



    }

})();

