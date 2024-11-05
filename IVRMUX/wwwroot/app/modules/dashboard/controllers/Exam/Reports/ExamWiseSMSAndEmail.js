(function () {
    'use strict';
    angular.module('app').controller('ExamWiseSMSAndEmailDetails', ExamWiseSMSAndEmailDetails)
    ExamWiseSMSAndEmailDetails.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel', '$timeout']
    function ExamWiseSMSAndEmailDetails($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {
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

            if (mobilenofull !== '0') {
                var otpmobile = mobilenofull.substring(6, 10);
                $scope.mobileno = otpmobile;
            }

            var emailidforotp = configsettingsnew[0].ivrmgC_OTPMailId;
            //emailidforotp = "vishnureddy2503@gmail.com";

            if (emailidforotp !== null || emailidforotp !== undefined) {
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


        $scope.submitted = false;
        $scope.chkflag = false;

        $scope.typeonclickloaddata = function () {
            $scope.studentdetails = [];
            $scope.submitted = false;
        };

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ExamWiseSMSAndEmail/loaddata", pageid).then(function (promise) {
                $scope.allacademicyear = promise.allacademicyear;
            });
        };

        $scope.getclass = function () {
            $scope.ASMCL_Id = "";
            $scope.allclasslist = [];
            $scope.ASMS_Id = "";
            $scope.allsectionlist = [];
            $scope.EME_Id = "";
            $scope.allexamlist = [];
            $scope.studentdetails = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ExamWiseSMSAndEmail/getclass", data).then(function (promise) {
                $scope.allclasslist = promise.allclasslist;
            });
        };

        $scope.getsection = function () {
            $scope.ASMS_Id = "";
            $scope.allsectionlist = [];
            $scope.EME_Id = "";
            $scope.allexamlist = [];
            $scope.studentdetails = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("ExamWiseSMSAndEmail/getsection", data).then(function (promise) {
                $scope.allsectionlist = promise.allsectionlist;
            });
        };

        $scope.getexam = function () {
            $scope.EME_Id = "";
            $scope.allexamlist = [];
            $scope.studentdetails = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("ExamWiseSMSAndEmail/getexam", data).then(function (promise) {
                $scope.allexamlist = promise.allexamlist;
            });
        };

        $scope.searchDetails = function () {
            $scope.studentdetails = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "typeformat": $scope.typeformat
                };

                apiService.create("ExamWiseSMSAndEmail/searchDetails", data).then(function (promise) {
                    if (promise.studentdetails !== null && promise.studentdetails.length > 0) {
                        $scope.studentdetails = promise.studentdetails;
                    }
                    else {
                        swal("No Record Found...!!!");
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        // all checkbox
        $scope.toggleAll = function (usercheck) {
            var toggleStatus = false;
            if (usercheck === true) {
                toggleStatus = usercheck;
                angular.forEach($scope.studentdetails, function (itm) {
                    itm.isSelected22 = toggleStatus;
                });
            }
            else if (usercheck === false) {
                toggleStatus = usercheck;
                angular.forEach($scope.studentdetails, function (itm) {
                    itm.isSelected22 = toggleStatus;
                });
            }
        };

        //SelectedStudentRecord, index
        $scope.optionToggled = function () {
            $scope.usercheck = $scope.studentdetails.every(function (itm) { return itm.isSelected22; });
            if ($scope.usercheck === true) {
                $scope.usercheck = true;
            }
            if ($scope.usercheck === false) {
                $scope.usercheck = false;
            }
        };

        $scope.emailflag = false;
        $scope.smsflag = false;

        $scope.checkvalue1 = function (user) {
            if (user === true) {
                $scope.smsflag = true;
            }
            else if (user === false) {
                $scope.smsflag = false;
            }
        };

        $scope.checkvalue2 = function (user) {
            if (user === true) {
                $scope.emailflag = true;
            }
            else if (user === false) {
                $scope.emailflag = false;
            }
        };

        // send sms and email
        $scope.SendSmsEmail = function () {
            $scope.finalstudentlist = [];
            if ($scope.clslst2.length > 0) {
                if ($scope.smsflag === true || $scope.emailflag === true) {

                    var data = {
                        "sms": $scope.smsflag,
                        "email": $scope.emailflag,
                        finalstudentlist: $scope.clslst2,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "EME_Id": $scope.EME_Id,
                        "typeformat": $scope.typeformat
                    };
                    apiService.create("ExamWiseSMSAndEmail/SendSmsEmail", data).then(function (promise) {

                        if (promise !== null) {
                            if (promise.message === "true") {
                                if ($scope.smsflag === true && $scope.emailflag === false) {
                                    swal("SMS Sent Successfully");
                                } else if ($scope.smsflag === false && $scope.emailflag === true) {
                                    swal("Email Sent Successfully");
                                } else if ($scope.smsflag === true && $scope.emailflag === true) {
                                    swal("SMS And Email Sent Successfully");
                                }
                            } else {
                                if ($scope.smsflag === true && $scope.emailflag === false) {
                                    swal("SMS Sent Failed");
                                } else if ($scope.smsflag === false && $scope.emailflag === true) {
                                    swal("Email Sent Failed");
                                } else if ($scope.smsflag === true && $scope.emailflag === true) {
                                    swal("SMS And Email Sent Failed");
                                }
                            }
                            $('#myModalswal').modal('hide');
                            $scope.clearload();
                        }
                    });
                }
                else {
                    swal("Please Select SMS or EMail Checkbox");
                }
            }
            else {
                swal("Please Select Atleast One Student");
            }
        };

        $scope.clearload = function () {
            $scope.ASMAY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.EME_Id = "";
            $scope.clslst2 = [];
            $scope.smsflag = false;
            $scope.emailflag = false;
            $scope.submitted = false;
            $scope.typeformat = 'Marks';
            $scope.studentdetails = [];
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
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
                angular.forEach($scope.studentdetails, function (user) {
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
    }
})();