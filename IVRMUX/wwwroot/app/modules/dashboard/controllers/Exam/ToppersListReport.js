(function () {
    'use strict';
    angular.module('app').controller('ToppersListReportController', ToppersListReportController)
    ToppersListReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function ToppersListReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null) {
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            } else {
                logopath = "";
            }

        } else {
            logopath = "";
        }


        $scope.radioval = 'Student';

        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        var emailotp = false;
        var mobileotp = false;

        $scope.sendsmsbtn = false;
        $scope.sendmailbtn = false;

        if (configsettingsnew !== null && configsettingsnew.length > 0) {
            emailotp = configsettingsnew[0].ivrmgC_emailValOTPFlag;
            mobileotp = configsettingsnew[0].ivrmgC_MobileValOTPFlag;

            $scope.emailotp = configsettingsnew[0].ivrmgC_emailValOTPFlag;
            $scope.mobileotp = configsettingsnew[0].ivrmgC_MobileValOTPFlag;

            if ($scope.emailotp === true) {
                $scope.emailotpshow = true;
            }
            else {
                $scope.emailotpshow = false;
            }

            if ($scope.mobileotp === true) {
                $scope.mobileotpshow = true;
            }
            else {
                $scope.mobileotpshow = false;
            }

            var mobilenofull = configsettingsnew[0].ivrmgC_OTPMobileNo.toString();

            //mobilenofull = "9483740804";

            if (mobilenofull !==null && mobilenofull !== '0') {
                var otpmobile = mobilenofull.substring(6, 10);
                $scope.mobileno = otpmobile;
            }

            var emailidforotp = configsettingsnew[0].ivrmgC_OTPMailId;
            //emailidforotp = "vishnureddy2503@gmail.com";

            if (emailidforotp !== undefined && emailidforotp !== null && emailidforotp !== "") {
                $scope.emailid = emailidforotp.substring(0, 4);
            }
            if ($scope.emailotp === true || $scope.mobileotp === true) {
                $scope.otpcheck = true;
            }
            else {
                $scope.otpcheck = false;
            }

        } else {
            emailotp = false;
            mobileotp = false;
        }

        $scope.obj = {};

        $scope.obj.smschecked = false;
        $scope.obj.emailchecked = false;
        $scope.notification = false;

        $scope.imgname = logopath;
        $scope.BindData = function () {
            apiService.getDATA("ToppersListReport/getdetails").then(function (promise) {
                $scope.acdlist = promise.acdlist;
                $scope.catlist = promise.catlist;
                $scope.ctlist = promise.ctlist;
                $scope.seclist = promise.seclist;
                $scope.sublist = promise.sublist;
                $scope.examlist = promise.examlist;
            });
        };

        $scope.onselectAcdYear = function () {
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.EME_Id = "";
            $scope.ISMS_Id = "";
        };

        $scope.get_sec_exam = function () {
            $scope.main_list = [];
            $scope.main_list1 = [];
             $scope.EME_Id = "";
            $scope.main_listsms = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("ToppersListReport/get_sec_exam", data).then(function (promise) {

                if (promise !== null) {
                    $scope.seclist = promise.seclist;

                    if ($scope.seclist.length === 0) {
                        $scope.ASMS_Id = "";
                    }

                    $scope.examlist = promise.examlist;
                    if ($scope.examlist.length === 0) {
                        $scope.EME_Id = "";
                    }

                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.onselectexam = function () {
            $scope.main_list = [];
            $scope.main_list1 = [];
            $scope.main_listsms = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("ToppersListReport/onselectexam", data).then(function (promise) {
                if (promise !== null) {

                    $scope.examlist = promise.examlist;
                    $scope.examlist = promise.examlist;
                    if ($scope.examlist.length === 0) {
                        $scope.EME_Id = "";
                    }

                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.get_subject = function () {
            $scope.main_list = [];
            $scope.main_listsms = [];
            $scope.main_list1 = [];
            var asms_id = 0;
            if ($scope.ASMS_Id === "") {
                asms_id = 0;
            } else {
                asms_id = $scope.ASMS_Id;
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": asms_id,
                "EME_Id": $scope.EME_Id
            };

            apiService.create("ToppersListReport/get_subject", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sublist = promise.sublist;
                    if ($scope.sublist.length === 0) {
                        $scope.ISMS_Id = "";
                    }
                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.submitted = false;

        $scope.onreport = function () {
            $scope.main_list = [];
            $scope.main_list1 = [];
            $scope.main_listsms = [];
            $scope.submitted = true;
            $scope.exm_checked = true;

            if ($scope.myForm.$valid) {
                var asmsid = 0;
                var subid = 0;
                if ($scope.qualification_type === '1') {
                    asmsid = 0;
                }
                var exm_check = 0;
                if ($scope.exm_checked === true) {
                    exm_check = 1;
                } else {
                    exm_check = 0;
                }

                var sub_check = 0;
                if ($scope.sub_checked === 1) {
                    sub_check = 1;
                } else {
                    sub_check = 0;
                }

                if ($scope.ASMS_Id === "") {
                    asmsid = 0;
                } else {
                    asmsid = $scope.ASMS_Id;
                }

                if ($scope.ISMS_Id === "") {
                    subid = 0;
                } else {
                    subid = $scope.ISMS_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": asmsid,
                    "EME_Id": $scope.EME_Id,
                    "ISMS_Id": subid,
                    "report_type": $scope.qualification_type,
                    "sub_check_type": sub_check,
                    "exm_check_type": exm_check,
                    "topper": $scope.topper
                };

                apiService.create("ToppersListReport/onreport", data).then(function (promise) {

                    angular.forEach($scope.acdlist, function (dd) {
                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.year = dd.asmaY_Year;
                        }
                    });

                    angular.forEach($scope.ctlist, function (ddd) {
                        if (ddd.asmcL_Id === parseInt($scope.ASMCL_Id)) {
                            $scope.class = ddd.asmcL_ClassName;
                        }
                    });

                    angular.forEach($scope.examlist, function (d) {
                        if (d.emE_Id === parseInt($scope.EME_Id)) {
                            $scope.exam = d.emE_ExamName;
                        }
                    });

                    if (parseInt($scope.qualification_type) === 1) {
                        if ($scope.sub_checked === 1) {
                            angular.forEach($scope.sublist, function (d1) {
                                if (d1.ismS_Id === $scope.ISMS_Id) {
                                    $scope.subjectname = d1.ismS_SubjectName;
                                }
                            });

                            $scope.details = "Year :" + $scope.year + ' Class :' + $scope.class + ' Exam : ' + $scope.exam + ' Subject : ' + $scope.subjectname;
                        }
                        else {
                            $scope.details = "Year :" + $scope.year + ' Class :' + $scope.class + ' Exam : ' + $scope.exam;
                        }
                    }

                    else if (parseInt($scope.qualification_type) === 2) {
                        if ($scope.sub_checked === 1) {
                            angular.forEach($scope.sublist, function (d1) {
                                if (d1.ismS_Id === parseInt($scope.ISMS_Id)) {
                                    $scope.subjectname = d1.ismS_SubjectName;
                                }
                            });

                            $scope.details = "Year :" + $scope.year + ' Class - Section :' + $scope.class + ' Exam : ' + $scope.exam + ' Subject : ' + $scope.subjectname;
                        }
                        else {
                            $scope.details = "Year :" + $scope.year + ' Class :' + $scope.class + ' Exam : ' + $scope.exam;
                        }
                    }

                    if ($scope.sub_checked === 1) {
                        $scope.main_list1 = promise.datareport;
                        if ($scope.qualification_type === 2) {
                            angular.forEach($scope.main_list1, function (dd2) {
                                dd2.ASMCL_ClassName = dd2.asmcL_ClassName;
                                dd2.ASMC_SectionName = dd2.asmC_SectionName;
                            });
                        }

                    } else {
                        $scope.main_list = promise.datareport;
                        $scope.main_listsms = promise.datareport;

                        angular.forEach($scope.main_listsms, function (dd) {
                            var rank = dd.estmP_SectionRank;
                            $scope.ordinal_suffix_of1(rank);
                            dd.estmP_SectionRanknew = dd.estmP_SectionRank + " " + $scope.datesuf1;
                            dd.exmaname = $scope.exam;
                        });
                    }
                });
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            var popupWinindow = "";
            if (parseInt($scope.qualification_type) === 1 || parseInt($scope.qualification_type) === 2) {
                var data1 = "";
                if ($scope.sub_checked === 1) {
                    data1 = "tablesub";
                } else {
                    data1 = "table";
                }

                innerContents = document.getElementById(data1).innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

            else if ($scope.qualification_type === 'toppers') {
                innerContents = document.getElementById("table1").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        };

        $scope.exportToExcel = function () {
            var data1 = "";
            if (parseInt($scope.qualification_type) === 1 || parseInt($scope.qualification_type) === 2) {

                if ($scope.sub_checked === 1) {
                    data1 = "#tablesub1";
                } else {
                    data1 = "#table1d";
                }
            } else if ($scope.qualification_type === 'toppers') {
                data1 = "#table1";
            }

            var exportHref = Excel.tableToExcel(data1, 'sheet name');
            var excelname = "Topper List Report.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.toggleAll = function (usercheck) {
            var toggleStatus = false;
            if (usercheck === true) {
                toggleStatus = usercheck;
                angular.forEach($scope.main_listsms, function (itm) {
                    itm.isSelected22 = toggleStatus;
                });
            }
            else if (usercheck === false) {
                toggleStatus = usercheck;
                angular.forEach($scope.main_listsms, function (itm) {
                    itm.isSelected22 = toggleStatus;
                });
            }
        };

        $scope.optionToggled = function () {
            $scope.usercheck = $scope.main_listsms.every(function (itm) { return itm.isSelected22; });
            if ($scope.usercheck === true) {
                $scope.usercheck = true;
            }
            if ($scope.usercheck === false) {
                $scope.usercheck = false;
            }
        };


        $scope.sendsms = function (obj) {

            $scope.selectedlist = [];
            angular.forEach($scope.main_listsms, function (dd) {
                if (dd.isSelected22) {
                    $scope.selectedlist.push(dd);
                }
            });

            if ($scope.selectedlist.length > 0) {
                var subid = 0;
                if ($scope.ISMS_Id === undefined || $scope.ISMS_Id === null || $scope.ISMS_Id === "") {
                    subid = 0;
                } else {
                    subid = $scope.ISMS_Id;
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "temp_topper_list_smsdetails": $scope.selectedlist,
                    "smschecked": obj.smschecked ,
                    "emailchecked": obj.emailchecked,
                    "EME_Id": $scope.EME_Id,
                    "ISMS_Id": subid
                };

                apiService.create("ToppersListReport/sendsms", data).then(function (promise) {
                    if (obj.smschecked === true && obj.emailchecked === false) {
                        swal("SMS Sent Successfully");
                    }
                    else if (obj.smschecked === false && obj.emailchecked === true) {
                        swal("Mail Sent Successfully");
                    }
                    else if (obj.smschecked === true && obj.emailchecked === true) {
                        swal("SMS / Mail Sent Successfully");
                    }
                    else {
                        swal("Failed To Send SMS / Email");
                    }
                });

            } else {
                swal("Select Alteast One Student To Send SMS / Email / Notification");
            }
        };


        $scope.email = true;
        $scope.sms = true;

        $scope.saveUseronce = function () {
            $scope.otpmobile = false;
            $scope.otpemail = false;
            $scope.buttonotp = false;

            $scope.smsdata = true;
            $scope.emaildata = false;

            if ($scope.emailotp === true || $scope.mobileotp === true) {
                if ($scope.email === true || $scope.sms === true) {
                    $scope.otpcheck = true;
                }
                else {
                    $scope.otpcheck = false;
                }
            }

            else {
                $scope.otpcheck = false;
            }

            if ($scope.radioval === 'Student') {
                $scope.albumNameArray = [];
                angular.forEach($scope.main_listsms, function (user) {
                    if (user.isSelected22 === true) {
                        $scope.albumNameArray.push(user);
                    }
                });

                if ($scope.albumNameArray.length > 0) {
                    if ((emailotp !== false && ($scope.email === true || $scope.sms === true))) {
                        $("#myModalotp").modal({ backdrop: 'static', keyboard: false });
                        var emailidforotp = configsettingsnew[0].ivrmgC_OTPMailId;
                        //emailidforotp = "vishnureddy2503@gmail.com";

                        if (emailidforotp !== null || emailidforotp !== undefined) {
                            $scope.emailid = emailidforotp.substring(0, 4);
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }

                    }
                    else if ((mobileotp !== false && ($scope.email === true || $scope.sms === true))) {
                        $("#myModalotp").modal({ backdrop: 'static', keyboard: false });
                        if (mobilenofull !== '0') {
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
                        $("#myModalswal").modal({ backdrop: 'static', keyboard: false });
                    }
                }
                else {
                    swal('Kindly select atleast one student..!');
                    return;
                }
            }
        };

        $scope.sendotpsms = function (forgetEmail) {
            $("#myModalswal").modal({ backdrop: 'static', keyboard: false });
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
                if ($scope.radioval === 'Student') {
                    $scope.clslst2 = $scope.albumNameArray;
                    $("#myModalswal").modal({ backdrop: 'static', keyboard: false });
                }

                console.log($scope.clslst2);
                $('#myModalswal').modal('show');
                $("#myModalswal").modal({ backdrop: 'static', keyboard: false });
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

        $scope.otponclickloaddata = function () {
            $scope.buttonotp = true;
            if ($scope.otptype === 'M') {
                $scope.otpmobile = true;
                $scope.otpemail = false;
            }
            else if ($scope.otptype === 'E') {
                $scope.otpmobile = false;
                $scope.otpemail = true;
            }
        };


        $scope.ordinal_suffix_of1 = function (datesufix1) {
            var j = datesufix1 % 10,
                k = datesufix1 % 100;

            console.log(j, k);
            if (j === 1 && k !== 11) {
                $scope.datesuf1 = datesufix1 + "st";
                $scope.index1 = "st";
                return $scope.datesuf1;
            }
            if (j === 2 && k !== 12) {
                $scope.datesuf1 = datesufix1 + "nd";
                $scope.index1 = "nd";
                return $scope.datesuf1;
            }
            if (j === 3 && k !== 13) {
                console.log("D");
                $scope.datesuf1 = datesufix1 + "rd";
                $scope.index1 = "rd";
                return $scope.datesuf1;
            }
            $scope.datesuf1 = datesufix1 + "th";
            $scope.index1 = "th";
            return $scope.datesuf1;
        };
    }

})();