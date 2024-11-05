//By prashant latest file
(function () {
    'use strict';
    angular.module('app').controller('PercentageWiseAttendanceReportController', PercentageWiseAttendanceReportController)

    PercentageWiseAttendanceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout']
    function PercentageWiseAttendanceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout) {

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.obj = {};
        $scope.obj.smschecked = false;
        $scope.obj.emailchecked = false;
        $scope.obj.whatsappchecked = false;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var logopath = "";

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (configsettingsnew !== null && configsettingsnew.length > 0) {

            var emailotp = configsettingsnew[0].ivrmgC_emailValOTPFlag;
            var mobileotp = configsettingsnew[0].ivrmgC_MobileValOTPFlag;

            $scope.emailotp = configsettingsnew[0].ivrmgC_emailValOTPFlag;
            $scope.mobileotp = configsettingsnew[0].ivrmgC_MobileValOTPFlag;

            if ($scope.emailotp == 1) {
                $scope.emailotpshow = true;
            }
            else {
                $scope.emailotpshow = false;
            }

            if ($scope.mobileotp == 1) {
                $scope.mobileotpshow = true;
            }
            else {
                $scope.mobileotpshow = false;
            }

            var mobilenofull = configsettingsnew[0].ivrmgC_OTPMobileNo.toString();
            if (mobilenofull != '0') {
                var otpmobile = mobilenofull.substring(6, 10);
                $scope.mobileno = otpmobile;
            }
            var emailidforotp = configsettingsnew[0].ivrmgC_OTPMailId;
            if (emailidforotp != null || emailidforotp != undefined) {
                $scope.emailid = emailidforotp.substring(0, 4);
            }
            if ($scope.emailotp == 1 || $scope.mobileotp == 1) {
                $scope.otpcheck = true;
            }
            else {
                $scope.otpcheck = false;
            }

        } else {
            var emailotp = 0;
            var mobileotp = 0;
        }

        $scope.FromDate = new Date();
        $scope.imgname = logopath;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.pagination = false;
        $scope.currentPage = 1;
        $scope.export_flag = true;
        $scope.Print_flag = false;
        $scope.printdatatable = [];

        $scope.getDataByType = function () {
            $scope.export_flag = true;
            $scope.report = false;
        };

        $scope.percentagearray = [
            { id: '<=', value: 'Less Than Equal To' },
            { id: '<', value: 'Less Than' },
            { id: '>=', value: 'Greater Than Equal To' },
            { id: '>', value: 'Greater Than' },
            { id: '=', value: 'Equal' }
        ];

        $scope.getloaddata = function () {
            var pageid = 2;
            apiService.getURI("PercentageWiseAttendanceReport/getloaddata", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.yearlist = promise.getyear;
                }
            });
        };

        $scope.getclass = function () {
            $scope.getreportdatalist = [];
            $scope.report = false;
            $scope.export_flag = true;
            $scope.ASMS_Id = "";
            $scope.ASMCL_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("PercentageWiseAttendanceReport/getclass", data).then(function (promise) {
                $scope.classlist = promise.getclass;
                angular.forEach($scope.yearlist, function (dd) {
                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                        $scope.minDatef = new Date(dd.asmaY_From_Date);
                        $scope.maxDatef = new Date(dd.asmaY_To_Date);
                        $scope.maxDatet = new Date(dd.asmaY_To_Date);

                        $scope.obj.fromdate = new Date(dd.asmaY_From_Date);
                        $scope.obj.todate = new Date(dd.asmaY_To_Date);
                    }
                });
            });
        };

        $scope.getsection = function () {
            $scope.getreportdatalist = [];
            $scope.report = false;
            $scope.export_flag = true;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("PercentageWiseAttendanceReport/getsection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sectionlist = promise.getsection;
                }
            });
        };

        $scope.setTodate = function (data1) {
            $scope.obj.todate = null;
            $scope.todatef = false;
            $scope.todate1 = data1.fromdate;
            $scope.minDatet = new Date(
                $scope.todate1.getFullYear(),
                $scope.todate1.getMonth(),
                $scope.todate1.getDate() + 1);
        };

        $scope.showreport = function (obj) {
            if ($scope.myform.$valid) {
                $scope.getreportdatalist = [];
                $scope.searchValue = "";
                $scope.report = true;
                $scope.export_flag = false;
                $scope.reportdetails = [];

                var sectionid = 0;
                if ($scope.type23 === "All") {
                    sectionid = 0;
                } else {
                    sectionid = $scope.ASMS_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": sectionid,
                    "fromdate": new Date(obj.fromdate).toDateString(),
                    "todate": new Date(obj.todate).toDateString(),
                    "allorindi": $scope.type23,
                    "percentage": $scope.percentage,
                    "flag": $scope.flag
                };

                apiService.create("PercentageWiseAttendanceReport/showreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getreportdatalist = promise.getreportdetails;
                        console.log($scope.getreportdatalist);
                        if ($scope.getreportdatalist !== null && $scope.getreportdatalist.length > 0) {
                            $scope.reportdetails = $scope.getreportdatalist;
                            $scope.report = true;
                            $scope.export_flag = false;

                            angular.forEach($scope.yearlist, function (dd) {
                                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.year = dd.asmaY_Year;
                                }
                            });
                            $scope.percentagevalue = "";

                            angular.forEach($scope.percentagearray, function (d) {
                                if (d.id === $scope.flag) {
                                    $scope.percentagevalue = d.value;
                                }
                            });

                            $scope.reportdetailsdd = "";

                            $scope.reportdetailsdd = "Academic Year : " + $scope.year + ' ' + "Attendance Percentage : " + $scope.percentagevalue + ' ' + $scope.percentage + ' %';

                            $scope.getinstituion = promise.getinstituion;
                            $scope.inst_name = $scope.getinstituion[0].mI_Name;
                        } else {
                            swal("No Records Found");
                            $scope.report = false;
                            $scope.searchValue = "";
                            $scope.export_flag = true;
                        }
                    } else {
                        swal("No Records Found");
                        $scope.report = false;
                        $scope.export_flag = true;
                        $scope.searchValue = "";
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };     

        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                var excelname = "Attendance Percentage Wise Report.xlsx";
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            } else {
                swal("Select Student List To Print The Details")
            }
        };

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            } else {
                swal("Select Student List To Print The Details")
            }         
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clear = function () {
            $state.reload();
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.reportdetails, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all2 = $scope.reportdetails.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.radioval = 'Student';
        $scope.email = true;
        $scope.sms = true;

        $scope.otponclickloaddata = function () {
            $scope.buttonotp = true;
            if ($scope.otptype == 'M') {
                $scope.otpmobile = true;
                $scope.otpemail = false;
            }
            else if ($scope.otptype == 'E') {
                $scope.otpmobile = false;
                $scope.otpemail = true;
            }
        };

        $scope.saveUseronce = function () {

            $scope.otpmobile = false;
            $scope.otpemail = false;
            $scope.buttonotp = false;

            $scope.smsdata = true;
            $scope.emaildata = false;

            if ($scope.emailotp == 1 || $scope.mobileotp == 1) {
                if ($scope.email == true || $scope.sms == true) {
                    $scope.otpcheck = true;
                }
                else {
                    $scope.otpcheck = false;
                }
            }

            else {
                $scope.otpcheck = false;
            }

            if ($scope.radioval == 'Student') {

                $scope.albumNameArray = [];
                angular.forEach($scope.reportdetails, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                })
                if ($scope.albumNameArray.length > 0) {
                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        $("#myModalotp").modal({ backdrop: false });
                        var emailidforotp = configsettingsnew[0].ivrmgC_OTPMailId;
                        if (emailidforotp != null || emailidforotp != undefined) {
                            $scope.emailid = emailidforotp.substring(0, 4);
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }
                    }
                    else if ((mobileotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        $("#myModalotp").modal({ backdrop: false });
                        if (mobilenofull != '0') {
                            var otpmobile = mobilenofull.substring(6, 10);
                            $scope.mobileno = otpmobile;
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Mobile Number Not Found!!");
                        }
                    }
                    else {
                        $scope.clslst2 = $scope.albumNameArray;
                        $('#myModalswal').modal('show');
                        $("#myModalswal").modal({ backdrop: false });
                    }
                }
                else {
                    swal('Kindly select atleast one student..!');
                    return;
                }
            }
        };

        $scope.sendotpsms = function (forgetEmail) {

            $("#myModalswal").modal({ backdrop: false });
            $scope.resendotpbutton = false;
            $scope.forgetEmailOTP = "";

            if (emailidforotp === null || emailidforotp === undefined) {
                emailidforotp = "test@mail";
            }
            var mobno = {
                "clickedlinkname": forgetEmail,
                "Mobile": mobilenofull.toString(),
                "Email": emailidforotp.toString()
            };

            apiService.create("Login/ForgotOTPForEmailval", mobno).then(function (promise) {
                if ($scope.radioval == 'Student') {
                    $scope.clslst2 = $scope.albumNameArray;
                    $("#myModalswal").modal({ backdrop: false });
                }

                console.log($scope.clslst2);
                $('#myModalswal').modal('show');
                $("#myModalswal").modal({ backdrop: false });
            });
        };

        $scope.VerifyforgetEmailOtp = function (data) {

            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {

                var mobno = {
                    "EMAILOTP": data
                };

                apiService.create("Login/VerifyEmailOtpgen", mobno).then(function (promise) {
                    if (promise === "Success") {
                        $scope.otpcheck = false;
                    }

                    else if (promise === "Fail") {
                        $scope.otpcheck = true;
                        $scope.resendotpbutton = true;
                        swal("OTP Mismatch!");
                    }
                    else {
                        $('#myModalswal').modal('hide');
                        $('#myModalotp').modal('show');
                        swal("OTP Expired. Please resend OTP");

                    }
                });
            }
        };

        $scope.SendAttendanceSMS = function () {

            var data = {
                "Temp_studentlist": $scope.printdatatable,
                "ASMAY_Id": $scope.ASMAY_Id,
                "smschecked": $scope.obj.smschecked,
                "emailchecked": $scope.obj.emailchecked,
                "whatsappchecked": $scope.obj.whatsappchecked
            };

            apiService.create("PercentageWiseAttendanceReport/SendAttendanceSMS", data).then(function (promise) {
                if (promise != null) {
                    swal("Message Sent Successfully");
                    $('#myModalswal').modal('hide');
                    $('#myModalotp').modal('hide');
                }
            });
        };
    }
})();