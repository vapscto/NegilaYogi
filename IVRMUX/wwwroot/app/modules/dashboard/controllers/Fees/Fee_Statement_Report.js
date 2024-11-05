(function () {
    'use strict';
    angular
        .module('app')
        .controller('Fee_Statement_ReportController', Fee_Statement_ReportController)

    Fee_Statement_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function Fee_Statement_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object


        $scope.EmployeeDis = false;
        $scope.Emp = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.employeeLeaveDetails = [];
        //======================
        //========================Tag
       
        $scope.togchkbxG = function () {
            $scope.feeall = $scope.feeheads.every(function (rr) {
                return rr.feeck;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.feeheads.some(function (options) {
                return options.feeck;
                });
          
        };
        $scope.all_check = function (tg) {
            $scope.feeall = tg;
            var toggleStatus = $scope.feeall;
            angular.forEach($scope.feeheads, function (tg) {
                tg.feeck = toggleStatus;
            });
        };
        //=========================
        $scope.typerdochange = function () {
            $scope.Studying = false;
            $scope.Left = false;
            $scope.DeActive = false;
            $scope.getfeedheadlist = [];
            angular.forEach($scope.feeheads, function (qq) {
                qq.feeck = false;
            })
        }
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("FeeStatusReport/getalldetails", pageid).then(function (promise) {
                $scope.adcyear = promise.adcyear;
                $scope.get_studentlist = promise.get_studentlist;
                $scope.feeheads = promise.feeheads;
                $scope.Studying = false;
                $scope.Left = false;
          
               
              
            });
        };

        $scope.get_feeheadandstu_details = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("FeeStatusReport/get_fee_and_stu", data).then(function (promise) {
               
                
                $scope.get_studentlist = promise.get_studentlist;
                $scope.feeheads = promise.feeheads;
                $scope.class_list = promise.class_list;
              
            });
        };

        $scope.get_section = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("FeeStatusReport/getsection", data).then(function (promise) {
                if (promise.section_list.length > 0) {
                    $scope.section_list = promise.section_list;
                }
                else {
                    swal('No Data Found!!!')
                }
              
            });
        };

        $scope.onreport = function () {
            if ($scope.myForm.$valid) {
                $scope.feeheadarray = [];
                angular.forEach($scope.feeheads, function (qq) {
                    if (qq.feeck == true) {
                        $scope.feeheadarray.push({ FMH_Id: qq.FMH_Id })
                    }
                })
                $scope.Studying1 = 0;
                $scope.Left1 = 0;
               
                if ($scope.Studying == true) {
                    $scope.Studying1 = 1;
                }
                else {
                    $scope.Studying1 = 0;
                }

                if ($scope.Left == true) {
                    $scope.Left1 = 1;
                }
                else {
                    $scope.Left1 = 0;
                }
               
                if ($scope.typeflag == 'All') {
                   
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "Amst_Id": 0,
                        feeheadarray: $scope.feeheadarray,
                        "studying": $scope.Studying1,
                        "left": $scope.Left1,
                         "ASMCL_Id": 0,
                        "ASMS_Id":0,
                        "typeflag": $scope.typeflag

                    }
                }
                if ($scope.typeflag == 'CS') {

                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "Amst_Id": 0,
                        feeheadarray: $scope.feeheadarray,
                        "Studying": $scope.Studying1,
                        "Left": $scope.Left1,
                     
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id,
                        "typeflag": $scope.typeflag

                    }
                }
                else if ($scope.typeflag == 'Individual') {

                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "Amst_Id": $scope.AMST_Id.AMST_Id,
                        feeheadarray: $scope.feeheadarray,
                        "Studying": $scope.Studying1,
                        "Left1": $scope.Left1,
                       
                        "ASMCL_Id": 0,
                        "ASMS_Id": 0,
                        "typeflag": $scope.typeflag
                    }
                }

                apiService.create("FeeStatusReport/getfeehead_statement_report", data).then(function (promise) {
                    $scope.ddate = "";
                    $scope.NetAmountInwords = "";
                    $scope.institutionDetails = promise.institutionDetails[0].mI_Logo;
                    $scope.getfeedheadlist = promise.getfeedheadlist;
                    $scope.ddate = new Date();
                    var totalamount1 = 0;
                    $scope.totalamount = 0;
                    angular.forEach($scope.getfeedheadlist, function (qq) {
                        totalamount1 += qq.FSS_NetAmount;

                    });
                    $scope.totalamount = totalamount1
                    $scope.NetAmountInwords = toWords($scope.totalamount) + 'Rupees only.';
                    $scope.feeheadarray1 = [];
                    angular.forEach($scope.feeheads, function (qq) {
                        if (qq.feeck == true) {
                            $scope.feeheadarray1.push({ FMH_FeeName: qq.FMH_FeeName })
                        }
                    })

                    var ttstring = "";
                    $scope.ttstring1 = "";
                    angular.forEach($scope.feeheadarray1, function (qq) {

                        ttstring += qq.FMH_FeeName + ' ' + ' ,';

                    });
                    $scope.ttstring1 = ttstring;
                });
            }
              else {
                    $scope.submitted = true;
                }
            };
        $scope.cancel = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

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

        $scope.printData = function (Employee) {
         
                var innerContents = document.getElementById("statusprint").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/EmpPaySlipPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
           
        };

        $scope.export = function () {
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
            });
        };

        $scope.SendMail = function (all_employees) {
            $scope.tmplt = [];
            angular.forEach($scope.all_employees, function (obj) {
                var Template = document.getElementById(obj.currentemployeeDetails.hrmE_EmployeeCode).innerHTML;
                $scope.tmplt.push({ hrmE_EmployeeCode: obj.currentemployeeDetails.hrmE_EmployeeCode, TemplateString: Template });
            });

            $scope.temp_list = [];
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
            });
            if (cnt == 0) {
                $scope.Employee.Template = $scope.tmplt;
                $scope.Employee.EmailSMS = "Email";

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
                    });
            }
            else if (cnt < 0) {
                swal("Attach file for selected Employees");
            }
        };


        $scope.SendSMS = function () {
            $scope.Employee.EmailSMS = "SMS";
            $scope.tmplt = [];
            angular.forEach($scope.all_employees, function (obj) {
                var Template = document.getElementById(obj.currentemployeeDetails.hrmE_EmployeeCode).innerHTML;
                $scope.tmplt.push({ hrmE_EmployeeCode: obj.currentemployeeDetails.hrmE_EmployeeCode, TemplateString: Template });
            });
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
            });
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
                });
        };

      


        
    }


})();