(function () {
    'use strict';
    angular
        .module('app')
        .controller('OverallDailyAttendanceAbsentSMSController', OverallDailyAttendanceAbsentSMSController)

    OverallDailyAttendanceAbsentSMSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function OverallDailyAttendanceAbsentSMSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

        $scope.objj = {};
        $scope.catreport_btn = true;
        $scope.catreport = false;
        $scope.printdatatable = [];
        $scope.printdatatable_model = [];
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.catreport1_btn = true;
        //$scope.itemsPerPage = 10;
        //$scope.itemsPerPage_model = 10;
        $scope.submitted = false;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
                copty = "";
            }
        } else {
            paginationformasters = 10;
            copty = "";
        }


        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage_model = paginationformasters;
        $scope.coptyright = copty;

        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}

        //$scope.imgname = logopath;

        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (configsettingsnew !== null) {


            if (configsettingsnew.length > 0) {
                var emailotp = configsettingsnew[0].ivrmgC_emailValOTPFlag;
                var mobileotp = configsettingsnew[0].ivrmgC_MobileValOTPFlag;

                $scope.emailotp = configsettingsnew[0].ivrmgC_emailValOTPFlag;
                $scope.mobileotp = configsettingsnew[0].ivrmgC_MobileValOTPFlag;
                //hard code
                //emailotp = 1;
                //mobileotp = 1;
                //$scope.emailotp =1;
                //$scope.mobileotp = 1;

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
                //  mobilenofull = '9591081840';
                if (mobilenofull != '0') {
                    var otpmobile = mobilenofull.substring(6, 10);
                    $scope.mobileno = otpmobile;
                }
                var emailidforotp = configsettingsnew[0].ivrmgC_OTPMailId;
                // emailidforotp = 'praveend114@gmail.com';
                if (emailidforotp != null || emailidforotp != undefined) {
                    $scope.emailid = emailidforotp.substring(0, 4);
                }
                if ($scope.emailotp == 1 || $scope.mobileotp == 1) {
                    $scope.otpcheck = true;
                }
                else {
                    $scope.otpcheck = false;
                }
            }
            else {
                var emailotp = 0;
                var mobileotp = 0;
            }
        } else {
            var emailotp = 0;
            var mobileotp = 0;
        }


        $scope.propertyName = 'ASMCL_ClassName';
        $scope.order = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //load
        $scope.StuAttRptDropdownList = function () {
            apiService.get("OverallDailyAttendanceAbsentSMS/getdetails/").then(function (promise) {
                $scope.yearDropdown = promise.academicList;
                $scope.allAcademicYear1 = promise.academicListdefault;
                for (var i = 0; i < $scope.yearDropdown.length; i++) {
                    name = $scope.yearDropdown[i].asmaY_Id;
                    for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                        if (name == $scope.allAcademicYear1[j].asmaY_Id) {
                            $scope.yearDropdown[i].Selected = true;
                            $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                        }
                    }
                }
                $scope.fromdate = new Date();
                $scope.fromdate = new Date();
                $scope.maxDatedof = new Date();
                $scope.minDatedof = new Date();
                $scope.categoryDropdown = promise.category_list;
                $scope.categoryflag = promise.categoryflag;
            });
           
        }

        //Report
        $scope.submitted = false;
        $scope.showReport = function () {
            $scope.printdatatable = [];
            $scope.searchValue = "";
            $scope.searchValue123 = "";

            var AMC_Id = 0
            if ($scope.objj.amC_Id > 0) {
                AMC_Id = $scope.objj.amC_Id
            }
            var att_type = "";
            if ($scope.AttendanceType == 0) {
                att_type='D'
            }
            else {
                att_type = 'H'
            }
                
            if ($scope.myForm.$valid) {
                var fromdate = new Date($scope.fromdate).toDateString();

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "fromdate": fromdate,
                    "AMC_Id": AMC_Id,
                    "attType": att_type
                }
                apiService.create("OverallDailyAttendanceAbsentSMS/getAttendetails", data)
                    .then(function (promise) {
                        $scope.students = promise.studentAttendanceList;
                        $scope.student_teacherList = promise.student_teacherList;

                        $scope.presentCountgrid = $scope.students.length;
                        $scope.presentCountgrid1 = $scope.student_teacherList.length;

                        if ($scope.students != null && $scope.students.length > 0) {
                            $scope.catreport_btn = false;
                            $scope.catreport = true;
                        }
                        if ($scope.student_teacherList != null && $scope.student_teacherList.length > 0) {
                            $scope.catreport1_btn = false;
                            $scope.catreport = true;
                        }
                        else {
                            swal("No Records Found!");
                            $scope.catreport1_btn = true;
                            $scope.catreport1_btn = true;
                            $scope.catreport = false;
                        }

                        $scope.searchValue1234 = "";
                    });
            } else {
                $scope.submitted = true;
            }
        };


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.student_teacherList, function (itm) {
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

            $scope.all2 = $scope.student_teacherList.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return ($filter('date')(obj.AMST_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.AMST_DOB, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || (obj.AMST_FirstName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_AdmNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMAY_RollNo).indexOf($scope.searchValue) >= 0 || (obj.AMST_DOB_Words).indexOf($scope.searchValue) >= 0 || (obj.AMST_FatherName).indexOf($scope.searchValue) >= 0 || (obj.AMST_MotherName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_FatherMobleNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_MobileNo).indexOf($scope.searchValue) >= 0 || (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0 || (obj.AMST_emailId).indexOf($scope.searchValue) >= 0 || (obj.AMST_BloodGroup).indexOf($scope.searchValue) >= 0 || (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0;
        }

        $scope.searchValue123 = '';
        $scope.filterValue11 = function (obj) {

            return (obj.classsection).indexOf($scope.searchValue123) >= 0 || (obj.studentname).indexOf($scope.searchValue123) >= 0;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clear = function () {
            $scope.asmaY_Id = "";
            $scope.fromdate = "";
            $scope.submitted = false;
            $scope.catreport = false;
            $scope.catreport_btn = true;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.searchValue = '';
            $scope.searchValue123 = '';
        }

        //-Sending sms-//
        $scope.sendsms = function () {
            //var fromdate = new Date($scope.fromdate).toDateString();
            var fromdate = new Date($scope.fromdate).toDateString();
            var att_type = "";
            if ($scope.AttendanceType == 0) {
                att_type = 'D'
            }
            else {
                att_type = 'H'
            }
            var data = {
                "absentlist": $scope.printdatatable,
                "ASMAY_Id": $scope.asmaY_Id,
                "fromdate": fromdate,
                "attType": att_type
            }
            apiService.create("OverallDailyAttendanceAbsentSMS/sendsms", data).then(function (promise) {
                if (promise != null) {
                    swal(promise.message);
                    $state.reload();
                }
            })
        }

        //-Sending Email-//
        $scope.sendemail = function () {
            var fromdate = new Date($scope.fromdate).toDateString();
            var data = {
                "absentlist": $scope.printdatatable,
                "ASMAY_Id": $scope.asmaY_Id,
                "fromdate": fromdate
            }
            apiService.create("OverallDailyAttendanceAbsentSMS/sendemail", data).then(function (promise) {
                if (promise != null) {
                    swal(promise.message);
                    $state.reload();
                }
            })
        }

        //--Smart Card Attendance Transfer--//
        $scope.smartcardatt = function () {
            var fromdate = new Date($scope.fromdate).toDateString();
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "fromdate": fromdate
            }
            apiService.create("OverallDailyAttendanceAbsentSMS/smartcardatt", data).then(function (promise) {
                if (promise != null) {
                    swal(promise.message);
                    $state.reload();
                }
            })
        }

        $scope.createuser = function () {
            var fromdate = new Date($scope.fromdate).toDateString();
            var data = {
                "fromdate": fromdate,
                "ASMAY_Id": $scope.asmaY_Id,

            }
            apiService.create("OverallDailyAttendanceAbsentSMS/createuser", data).then(function (promise) {
                if (promise != null) {
                    swal(promise.message);
                }
            })
        }

     

        // Sending OTP 
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
        }


        $scope.radioval = 'Student';
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
                angular.forEach($scope.student_teacherList, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                })
                if ($scope.albumNameArray.length > 0) {

                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        $("#myModalotp").modal({ backdrop: false });
                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
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


        $scope.saveUseronceemail = function () {

            $scope.otpmobile = false;
            $scope.otpemail = false;
            $scope.buttonotp = false;

            $scope.smsdata = false;
            $scope.emaildata = true;

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
                angular.forEach($scope.student_teacherList, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                })
                if ($scope.albumNameArray.length > 0) {

                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        $("#myModalotp").modal({ backdrop: false });
                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
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

        $scope.stfsearchValue = '';

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
    }
})();
