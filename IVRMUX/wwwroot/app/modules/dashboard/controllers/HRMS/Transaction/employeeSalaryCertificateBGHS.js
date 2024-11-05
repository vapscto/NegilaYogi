(function () {
    'use strict';
    angular
.module('app')
.controller('EmployeeSalaryCertificateController', EmployeeSalaryCertificateController)

    EmployeeSalaryCertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function EmployeeSalaryCertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object


        $scope.EmployeeDis = false;
        $scope.Emp = {};

        $scope.employeeLeaveDetails = [];

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeSalaryCertificate/getalldetails", pageid).then(function (promise) {

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
                $scope.employeedropdown = [];
                $scope.employeedropdown = false;
                $scope.onLoadGetData();
                //$scope.departmentselectedAll = false;
                //$scope.designationselectedAll = false;
                //$scope.employeedropdown = false;
                //$scope.leaveyeardropdown = false;
                //$scope.monthdropdown = false;
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
            apiService.create("salaryUpdation/get_depts", data).
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
            apiService.create("salaryUpdation/get_desig", data).
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

                apiService.create("EmployeeSalaryCertificate/GetEmployeeDetailsByLeaveYearAndMonth", data).
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
            //  $scope.items = [];
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
                //   $scope.totals = "";
                //   $scope.empsaldetail = {};
                //   $scope.NetSalary = "";

                $scope.employeeSalaryslipDetails = [];
                $scope.employeeLeaveDetails = [];

                var data = $scope.Employee;
                
                apiService.create("EmployeeSalaryCertificate/GenerateEmployeeSalarySlip", data).
                            then(function (promise) {

                                //Earning Deduction Details
                                if (promise.employeeSalaryslipDetails != null && promise.employeeSalaryslipDetails.length != 0) {
                                    $scope.EmployeeDis = true;

                                    var items = promise.employeeSalaryslipDetails;

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
                                    $scope.esary = es;
                                    $scope.dsary = ds;
                                    if (es.out.length >= ds.out.length) {
                                        for(var item of ds.out)
                                      es.out[ds.out.indexOf(item)].ds = item;
                                        $scope.items = es.out;
                                    } else {

                                        for(var item of es.out)
                                     ds.out[es.out.indexOf(item)].ds = item;
                                        $scope.items = ds.out;
                                    }

                                    // console.log($scope.items);

                                    $scope.totals = [es.sum, ds.sum];
                                    console.log($scope.totals);

                                    $scope.totalearn = es.sum;
                                    $scope.totalded = ds.sum;
                                    var LopAmount = 0;
                                    if (promise.payrollStandard.hrC_PayMethodFlg == "Method1") {
                                        LopAmount = promise.empsaldetail.lopAmount;
                                    }
                                    else {
                                        LopAmount = 0;
                                    }
                                    //Employee Salary Details
                                    $scope.empsaldetail = promise.empsaldetail;

                                    $scope.NetSalary = Math.round((Math.ceil(parseFloat($scope.totals[0]) - parseFloat($scope.totals[1]))) - LopAmount);

                                    $scope.NetAmountInwords = toWords($scope.NetSalary) + 'Rupees only.';



                                    //Institute Details
                                    if (promise.institutionDetails != null) {


                                        $scope.selectedMonth = promise.hreS_Month;
                                        $scope.selectedYear = promise.hreS_Year;

                                        $scope.institutionDetails = promise.institutionDetails;

                                        if ($scope.institutionDetails.mI_Logo == "" || $scope.institutionDetails.mI_Logo == null) {
                                            $scope.institutionDetails.mI_Logo = "../../../../../../images/uploads/studentDocumnets/2/36ed82c1-b373-4358-af12-4397bacda396.jpg";
                                        }
                                        else {
                                            $scope.institutionDetails.mI_Logo = promise.institutionDetails.mI_Logo;
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

                                    }


                                    //Employee Details
                                    if (promise.currentemployeeDetails != null) {
                                        $scope.empdetails = promise.currentemployeeDetails;

                                        $scope.empdetails.hrmE_DOJ = new Date($scope.empdetails.hrmE_DOJ);

                                        $scope.DesignationName = promise.designationName;
                                        $scope.DepartmentName = promise.departmentName;
                                    }
                                    

                                    if (promise.employeeLeaveDetails != null && promise.employeeLeaveDetails.length > 0) {

                                        $scope.employeeLeaveDetails = promise.employeeLeaveDetails;
                                    } else {
                                        $scope.employeeLeaveDetails = [];
                                    }

                                }




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


        $scope.printData = function (Baldwin) {
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


        $scope.SendMail = function () {

            var innerContents = document.getElementById("Baldwin").innerHTML;
            var Template = '<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body>' + innerContents + '</body></html>';

            // var result = htmlToPDF(Template);
            

            $scope.Employee.Template = Template;
            $scope.Employee.EmailSMS = "Email"
            var data = $scope.Employee;

            apiService.create("EmployeeSalaryCertificate/SendEmailSMS", data).
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


        $scope.SendSMS = function () {

            var innerContents = document.getElementById("Baldwin").innerHTML;
            var Template = '<html><head>' +

            '</head><body>' + innerContents + '</body></html>';




            

            $scope.Employee.Template = Template;
            $scope.Employee.EmailSMS = "SMS"
            var data = $scope.Employee;

            apiService.create("EmployeeSalaryCertificate/SendEmailSMS", data).
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
        //        
        //        //prints pdf with headline Hello world
        //        console.log(resp.content.toString())
        //    });
        //}).catch(function (e) {
        //    console.log(e)
        //    
        //})



    }




})();