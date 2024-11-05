
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegegeneralsmsController', CollegegeneralsmsController)

    CollegegeneralsmsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    function CollegegeneralsmsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {
        $scope.searchValue = '';
        $scope.stsearch = '';
        $scope.sortReverse = true;
        $scope.snd_email = false;
        $scope.snd_sms = false;
        $scope.stfsnd_email = false;
        $scope.stfsnd_sms = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.loadradioval = true;
        $scope.exm_radioval = 'mark';
        $scope.attend_radioval = 'between';
        $scope.stdmsg = '';
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        var configsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (configsettings.length > 0) {
            var emailotp = configsettings[0].ivrmgC_emailValOTPFlag;
            var mobileotp = configsettings[0].ivrmgC_MobileValOTPFlag;

            $scope.emailotp = configsettings[0].ivrmgC_emailValOTPFlag;
            $scope.mobileotp = configsettings[0].ivrmgC_MobileValOTPFlag;

            if ($scope.emailotp == true) {
                $scope.emailotpshow = true;
            }
            else {
                $scope.emailotpshow = false;
            }

            if ($scope.mobileotp == true) {
                $scope.mobileotpshow = true;
            }
            else {
                $scope.mobileotpshow = false;
            }


            var mobilenofull = configsettings[0].ivrmgC_OTPMobileNo.toString();
            //  mobilenofull = '9591081840';
            if (mobilenofull !== '0') {
                var otpmobile = mobilenofull.substring(6, 10);
                $scope.mobileno = otpmobile;
            }
            var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
            // emailidforotp = 'praveend114@gmail.com';
            if (emailidforotp !== null || emailidforotp !== undefined) {
                $scope.emailid = emailidforotp.substring(0, 4);
            }


            if ($scope.emailotp === true || $scope.mobileotp === true) {
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


        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;

        $scope.changeradio1 = function () {
            $scope.stdmsg = 'Hi';
            if ($scope.selradioval === 'exam') {

                $scope.stdmsg = 'Hi';
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    // "ASMS_Id": $scope.asmS_Id
                }
                if ($scope.count > 0) {
                    apiService.create("Collegegeneralsms/Getexam/", data).
                        then(function (promise) {

                            $scope.exmstdlist = promise.exmstdlist;

                        });
                }
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
        }


        // ******* Verify  Otp ********* // 
        $scope.VerifyforgetEmailOtp = function (data) {

            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {
                var mobno = {
                    "EMAILOTP": data
                }
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

        $scope.resendotp = function () {
            $('#myModalswal').modal('show');
            //  $('#myModalotp').modal('show');
        };

        $scope.stfsearchValue = '';


        // ***** Get OTP Function **** //

        $scope.sendotpsms = function (forgetEmail) {

            $("#myModalswal").modal({ backdrop: false });
            $scope.resendotpbutton = false;
            $scope.forgetEmailOTP = "";

            if (emailidforotp === null || emailidforotp === undefined) {
                emailidforotp = "test@mail";
            }
            // $('#myModalotp').modal('hide');
            var mobno = {
                "clickedlinkname": forgetEmail,
                "Mobile": mobilenofull.toString(),
                "Email": emailidforotp.toString(),
            };

            apiService.create("Login/ForgotOTPForEmailval_new", mobno).then(function (promise) {

                if (promise !== "Something Went Wrong Please Contact Administrator") {

                    if ($scope.radioval === 'Student' || $scope.radioval === 'Father' || $scope.radioval === 'Mother' || $scope.radioval === 'Guardian') {
                        $scope.clslst2 = $scope.albumNameArray;
                        $("#myModalswal").modal({ backdrop: false });
                    }

                    if ($scope.radioval === 'Staff') {
                        $scope.clslst21 = $scope.albumNameArray;
                        $("#myModalswal").modal({ backdrop: false });
                    }

                    console.log($scope.clslst2);
                    $('#myModalswal').modal('show');
                    $("#myModalswal").modal({ backdrop: false });

                } else {
                    $('#myModalswal').modal('hide');
                    swal(promise);
                }
            });
        };


        $scope.submitted = true;
        $scope.email = true;
        $scope.sms = true;

        // ** SEND Button after selecting the details ** // 

        $scope.saveUseronce = function () {

            //$scope.otpmobile = false;
            //$scope.otpemail = false;
            //$scope.buttonotp = false;            

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

            if ($scope.radioval === 'Student' || $scope.radioval === 'Father' || $scope.radioval === 'Mother' || $scope.radioval === 'Guardian') {

                if ($scope.template1 === undefined || $scope.template1 === null || $scope.template1 === "") {
                    swal("Please Enter The Template Name");
                    return;
                }

                $scope.albumNameArray = [];
                angular.forEach($scope.studentList, function (user) {
                    if (!!user.selected) {
                        user.msg = $scope.stdmsg;
                        $scope.albumNameArray.push(user);
                    }
                });

                if ($scope.albumNameArray.length > 0 && $scope.template1 !== undefined && $scope.template1 !== '') {

                    if ((emailotp === true && ($scope.email === true || $scope.sms === true))) {
                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
                        if (emailidforotp !== null || emailidforotp !== undefined) {
                            $scope.emailid = emailidforotp.substring(0, 4);
                            $('#myModalotp').modal('show');
                            $("#myModalotp").modal({ backdrop: false });
                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }
                    }

                    else if ((mobileotp === true && ($scope.email === true || $scope.sms === true))) {

                        if (mobilenofull !== '0') {
                            var otpmobile = mobilenofull.substring(6, 10);
                            $scope.mobileno = otpmobile;
                            $('#myModalotp').modal('show');

                            $("#myModalotp").modal({ backdrop: false });
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
                    swal('Kindly select atleast one student..! and Enter Template Name');
                    return;
                }
            }

            if ($scope.radioval === 'General') {

                if ($scope.template === undefined || $scope.template === null || $scope.temparray === "") {
                    swal("Please Enter The Template Name");
                    return;
                }

                if ($scope.myForm.$valid) {
                    if ((emailotp === true && ($scope.email === true || $scope.sms === true))) {
                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
                        // emailidforotp = 'praveend114@gmail.com';
                        if (emailidforotp !== null || emailidforotp !== undefined) {
                            $scope.emailid = emailidforotp.substring(0, 4);
                            $('#myModalotp').modal('show');
                            $("#myModalotp").modal({ backdrop: false });
                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }
                    }

                    else
                        if ((mobileotp === true && ($scope.email === true || $scope.sms === true))) {

                            if (mobilenofull !== '0') {
                                var otpmobile = mobilenofull.substring(6, 10);
                                $scope.mobileno = otpmobile;
                                $('#myModalotp').modal('show');
                                $("#myModalotp").modal({ backdrop: false });
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
                    $scope.submitted = true;
                }
            }

            if ($scope.radioval === 'Staff') {
                if ($scope.template2 === undefined || $scope.template2 === null || $scope.template2 === "") {
                    swal("Please Enter The Template Name");
                    return;
                }

                $scope.albumNameArray = [];
                angular.forEach($scope.employeelst, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                });

                if ($scope.albumNameArray.length > 0 && $scope.template2 !== undefined && $scope.template2 !== '') {

                    if ((emailotp === true && ($scope.email === true || $scope.sms === true))) {
                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
                        // emailidforotp = 'praveend114@gmail.com';
                        if (emailidforotp !== null || emailidforotp !== undefined) {
                            $scope.emailid = emailidforotp.substring(0, 4);
                            $('#myModalotp').modal('show');
                            $("#myModalotp").modal({ backdrop: false });
                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }

                    }
                    else if ((mobileotp === true && ($scope.email === true || $scope.sms === true))) {
                        if (mobilenofull !== '0') {
                            var otpmobile = mobilenofull.substring(6, 10);
                            $scope.mobileno = otpmobile;
                            $('#myModalotp').modal('show');
                            $("#myModalotp").modal({ backdrop: false });
                        }
                        else {
                            swal("Authorized Mobile Number Not Found!!");
                        }
                    }
                    else {
                        $scope.clslst21 = $scope.albumNameArray;
                        $('#myModalswal').modal('show');
                        $("#myModalswal").modal({ backdrop: false });
                    }
                }
                else {

                    swal('Kindly select atleast one Employee..! and Template Name');
                    return;
                }
            }
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.changeradio = function () {
            //alert('a')
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.ASMAY_Id = "";
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.stdmsg = "";
            $scope.template1 = "";
            $scope.template = "";

            if ($scope.loadradioval == true) {
                $scope.radioval = 'General';
                $scope.loadradioval = false;
            }

            if ($scope.radioval === 'General') {
                $scope.mobno = "";
                $scope.mes = "";
            }

            if ($scope.radioval === 'Student') {
                $scope.snd_email = true;
                $scope.snd_sms = true;
                $scope.selradioval = '';
                $scope.emE_Id = '';
                $scope.exmstdlist = [];
                $scope.stdmsg = "";

                var data = {
                    "selectedRadiobtn": $scope.radioval
                };

                $scope.studentList = [];
                apiService.create("Collegegeneralsms/Getdetails/", data).then(function (promise) {

                    $scope.fillacademiyear = promise.yearlist;
                    $scope.currentYear = promise.currentYear;
                    $scope.courselist = promise.courselist;
                    $scope.branchlist = promise.branchlist;
                    $scope.semisterlist = promise.semisterlist;
                    $scope.sectionlist = promise.sectionlist;

                });
            }

            if ($scope.radioval === 'Father') {

                $scope.snd_email = true;
                $scope.snd_sms = true;
                $scope.selradioval = '';
                $scope.emE_Id = '';
                $scope.exmstdlist = [];
                $scope.stdmsg = "";
                var data = {
                    "selectedRadiobtn": $scope.radioval
                };

                $scope.studentList = [];
                apiService.create("Collegegeneralsms/Getdetails/", data).then(function (promise) {

                    $scope.fillacademiyear = promise.yearlist;
                    $scope.currentYear = promise.currentYear;
                    $scope.courselist = promise.courselist;
                    $scope.branchlist = promise.branchlist;
                    $scope.semisterlist = promise.semisterlist;
                    $scope.sectionlist = promise.sectionlist;

                });
            }

            if ($scope.radioval === 'Mother') {


                $scope.snd_email = true;
                $scope.snd_sms = true;
                $scope.selradioval = '';
                $scope.emE_Id = '';
                $scope.exmstdlist = [];


                $scope.stdmsg = "";

                var data = {
                    "selectedRadiobtn": $scope.radioval
                };

                $scope.studentList = [];
                apiService.create("Collegegeneralsms/Getdetails/", data).then(function (promise) {
                    $scope.fillacademiyear = promise.yearlist;
                    $scope.currentYear = promise.currentYear;
                    $scope.courselist = promise.courselist;
                    $scope.branchlist = promise.branchlist;
                    $scope.semisterlist = promise.semisterlist;
                    $scope.sectionlist = promise.sectionlist;

                });
            }

            if ($scope.radioval === 'Guardian') {
                $scope.snd_email = true;
                $scope.snd_sms = true;
                $scope.selradioval = '';
                $scope.emE_Id = '';
                $scope.exmstdlist = [];
                $scope.stdmsg = "";
                var data = {
                    "selectedRadiobtn": $scope.radioval
                };

                $scope.studentList = [];
                apiService.create("Collegegeneralsms/Getdetails/", data).then(function (promise) {
                    $scope.fillacademiyear = promise.yearlist;
                    $scope.currentYear = promise.currentYear;
                    $scope.courselist = promise.courselist;
                    $scope.branchlist = promise.branchlist;
                    $scope.semisterlist = promise.semisterlist;
                    $scope.sectionlist = promise.sectionlist;

                });
            }

            if ($scope.radioval === 'Staff') {
                var id = 1;
                $scope.departmentdropdown = [];
                $scope.Designation_types = [];
                $scope.stfsnd_email = true;
                $scope.stfsnd_sms = true;
                $scope.deptcheck = false;

                $scope.employeelst = [];
                $scope.stfmsg = "";
                apiService.getURI("Collegegeneralsms/Getdepartment", id).then(function (promise) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    if (promise.departmentdropdown != null) {
                        $scope.deptcheck = true;
                        $scope.all_checkdep();
                    }
                });
            }
        };

        $scope.all_checkdep = function () {
            $scope.employeelst = [];
            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_designation();
        };

        $scope.get_designation = function () {
            $scope.employeelst = [];
            $scope.deptcheck = $scope.departmentdropdown.every(function (options) {
                return options.selected;
            });
            $scope.get_designationnew();
        };

        $scope.get_designationnew = function () {
            $scope.desgcheck = "";
            var groupidss;
            for (var i = 0; i < $scope.departmentdropdown.length; i++) {
                if ($scope.departmentdropdown[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.departmentdropdown[i].hrmD_Id;
                    else
                        groupidss = groupidss + "," + $scope.departmentdropdown[i].hrmD_Id;
                }
            }

            if (groupidss != undefined) {
                var data = {
                    "multipledep": groupidss
                };
                apiService.create("Collegegeneralsms/get_designation", data).then(function (promise) {

                    $scope.Designation_types = promise.designationdropdown;
                    if (promise.designationdropdown != null) {
                        $scope.desgcheck = true;
                        $scope.all_checkdesg();
                    }
                });
            }
            else {
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        };

        $scope.all_checkdesg = function () {
            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.selected = toggleStatus;
            });

            $scope.get_employee();
        };

        //fill desg end
        //fill employee start
        $scope.get_employee = function () {
            $scope.employeelst = [];
            $scope.desgcheck = $scope.Designation_types.every(function (options) {
                return options.selected;
            });
            //$scope.get_employeenew();
        };

        $scope.get_employeenew = function () {
            $scope.stf = false;
            $scope.Stflist = false;
            $scope.employeelst = [];
            var deptIds;
            for (var i = 0; i < $scope.departmentdropdown.length; i++) {
                if ($scope.departmentdropdown[i].selected == true) {

                    if (deptIds == undefined)
                        deptIds = $scope.departmentdropdown[i].hrmD_Id;
                    else
                        deptIds = deptIds + "," + $scope.departmentdropdown[i].hrmD_Id;
                }
            }
            var groupidss;
            for (var i = 0; i < $scope.Designation_types.length; i++) {
                if ($scope.Designation_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.Designation_types[i].hrmdeS_Id;
                    else
                        groupidss = groupidss + "," + $scope.Designation_types[i].hrmdeS_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledes": groupidss,
                    "multipledep": deptIds
                };
                apiService.create("Collegegeneralsms/get_employee", data).then(function (promise) {
                    $scope.employeelst = promise.stafflist;
                    if ($scope.employeelst.length > 0) {
                        $scope.stf = true;
                    }
                });
            }
            else {
                $scope.employeelst = "";
            }
        };

        $scope.get_employeenew_clear = function () {
            $scope.stfmsg = "";
            $scope.template2 = "";
            $scope.employeelst = [];
        };



        //************* Get Student List *************//

        $scope.GetStudentDetails = function () {
            $scope.selradioval = '';
            $scope.emE_Id = '';
            $scope.all = false;
            $scope.exmstdlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id,
                "radiotype": $scope.radioval
            };

            apiService.create("Collegegeneralsms/GetStudentDetails/", data).then(function (promise) {
                if (promise.studentCount > 0) {

                    if ($scope.selradioval === 'exam') {
                        $scope.changeradio1();
                    }
                    $scope.count = promise.studentCount;
                    $scope.studentList = promise.studentlist;

                } else {
                    swal("No records found for selected academicYear,class and section");
                    $scope.studentList = "";
                    $scope.count = 0;
                }
            });
        };

        $scope.exname = '';
        $scope.stumarkdetails = [];
        $scope.at_details = [];
        $scope.searchstddetails = function () {
            $scope.all = false;
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                var data = {};
                if ($scope.selradioval == 'exam') {
                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "EME_Id": $scope.emE_Id,
                        "selradioval": $scope.selradioval
                    };
                }
                if ($scope.selradioval == 'attendance') {
                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "selradioval": $scope.selradioval,
                        "attend_radioval": $scope.attend_radioval,
                        "fr_date": $scope.frm_date,
                        "to_date": $scope.to_date,
                        "crnt_date": $scope.curnt_date
                    };
                }

                apiService.create("Collegegeneralsms/searchstddetails/", data).then(function (promise) {
                    if (promise.studentCount > 0) {
                        $scope.count = promise.studentCount;
                        $scope.studentList = promise.studentlist;
                        if ($scope.selradioval == 'exam') {
                            $scope.stumarkdetails = promise.stumarkdetails;
                            if (promise.stumarkdetails != null) {
                                $scope.temparray = [];
                                angular.forEach($scope.studentList, function (stu) {
                                    var marks = '';
                                    var grade = '';
                                    angular.forEach($scope.stumarkdetails, function (mrk) {
                                        if (stu.amsT_Id == mrk.amsT_Id) {
                                            if (marks == '') {
                                                if (mrk.estmpS_PassFailFlg == 'AB' || mrk.estmpS_PassFailFlg == 'M' || mrk.estmpS_PassFailFlg == 'L') {
                                                    marks = mrk.result;
                                                }
                                                else {
                                                    marks = mrk.marksDetails;
                                                }
                                            }
                                            else {
                                                if (mrk.estmpS_PassFailFlg == 'AB' || mrk.estmpS_PassFailFlg == 'M' || mrk.estmpS_PassFailFlg == 'L') {
                                                    marks = marks + ',' + mrk.result;
                                                }
                                                else {
                                                    marks = marks + ',' + mrk.marksDetails;
                                                }
                                            }
                                            if (grade == '') {
                                                if (mrk.estmpS_PassFailFlg == 'AB' || mrk.estmpS_PassFailFlg == 'M' || mrk.estmpS_PassFailFlg == 'L') {
                                                    grade = mrk.result;
                                                }
                                                else {
                                                    grade = mrk.gradeDetails;
                                                }
                                            }
                                            else {
                                                if (mrk.estmpS_PassFailFlg == 'AB' || mrk.estmpS_PassFailFlg == 'M' || mrk.estmpS_PassFailFlg == 'L') {
                                                    grade = grade + ',' + mrk.result;
                                                }
                                                else {
                                                    grade = grade + ',' + mrk.gradeDetails;
                                                }
                                            }
                                        }
                                    });
                                    var finaltotal = '';
                                    var finalgrade = '';
                                    angular.forEach($scope.stumarkdetails, function (fnl) {
                                        if (stu.amsT_Id == fnl.amsT_Id) {
                                            finaltotal = fnl.totalMarks;
                                            finalgrade = fnl.totalGrade;
                                        }
                                    });
                                    grade = grade + ',' + finalgrade;
                                    marks = marks + ',' + finaltotal;

                                    $scope.temparray.push({ amsT_Id: stu.amsT_Id, studentName: stu.studentName, amsT_AdmNo: stu.amsT_AdmNo, amsT_MobileNo: stu.amsT_MobileNo, amsT_emailId: stu.amsT_emailId, marksDetails: marks, gradeDetails: grade });
                                });

                                $scope.studentList = $scope.temparray;

                                //   console.log($scope.temparray)
                            }
                        }

                        if ($scope.selradioval == 'attendance') {

                            $scope.at_details = promise.attdetails;
                            if ($scope.at_details.length != 0) {

                                $scope.temparray1 = [];
                                var lb = '';
                                if ($scope.attend_radioval == 'between') {
                                    var fromdate1 = $filter('date')($scope.frm_date, 'dd-MM-yyyy');
                                    var todate1 = $filter('date')($scope.to_date, 'dd-MM-yyyy');
                                    //$scope.frm_date = $filter('date')($scope.frm_date, 'dd-MM-yyyy');
                                    //  $scope.to_date = $filter('date')($scope.to_date, 'dd-MM-yyyy');



                                    lb = 'Student Attendence From' + ' ' + ' ' + fromdate1 + ' ' + 'To' + ' ' + ' ' + todate1 + ' ' + ' ' + '-' + ' '
                                }
                                if ($scope.attend_radioval == 'current') {
                                    var curdate = $filter('date')($scope.curnt_date, 'dd-MM-yyyy');
                                    // $scope.curnt_date = $filter('date')($scope.curnt_date, 'dd-MM-yyyy');
                                    lb = 'Student Attendence On' + ' ' + ' ' + curdate + ' ' + '-' + ' '
                                }

                                angular.forEach($scope.studentList, function (stu1) {
                                    var attt = '';
                                    //attt = lb + 'Total Class Held :' + 100 + '/' + 'Total Class Present :' + 80;
                                    angular.forEach($scope.at_details, function (at1) {
                                        if (stu1.amsT_Id == at1.amsT_Id) {
                                            attt = lb + ' ' + 'Total Class Held :' + at1.totalworkingday + ' ' + '/' + ' ' + 'Total Class Present :' + at1.totalpresentday + '   ' + 'Percentage :' + at1.attper;
                                        }
                                    });
                                    $scope.temparray1.push({ amsT_Id: stu1.amsT_Id, studentName: stu1.studentName, amsT_AdmNo: stu1.amsT_AdmNo, amsT_MobileNo: stu1.amsT_MobileNo, amsT_emailId: stu1.amsT_emailId, atndetails: attt });
                                });
                                $scope.studentList = $scope.temparray1;
                            }
                        }

                    } else {
                        swal("No Records Found");
                        $scope.studentList = "";
                        $scope.count = 0;
                    }
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.order1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.submitted = false;

        // **** Sending SMS and Email For General **** //

        $scope.SendData = function () {
            if ($scope.myForm.$valid) {
                $('#myModalswal').modal('hide');
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send SMS ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var Send = {
                                "Mobno": $scope.mobno,
                                "mes": $scope.mes,
                                "radiotype": $scope.radioval,
                                "template": $scope.template
                            };

                            apiService.create("Collegegeneralsms/savedetail", Send).then(function (promise) {
                                if (promise.smsStatus === 'sent') {
                                    swal("SMS Sent Successfully.");
                                    $state.reload();
                                }
                                else {
                                    swal("SMS Sending Cancelled.");
                                }
                            });
                        }
                        else {
                            var dataaa = 0;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        // ****** Sending SMS To Students ***** //

        $scope.SendMSG = function (stdmsg) {
            //alert($scope.radioval)

            $('#myModalswal').modal('hide');

            $scope.printstudents = [];
            angular.forEach($scope.studentList, function (user) {
                if (!!user.selected) $scope.printstudents.push(user);
            });

            if ($scope.printstudents.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send SMS?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            if ($scope.selradioval == 'exam') {
                                angular.forEach($scope.exmstdlist, function (ex) {
                                    if ($scope.emE_Id == ex.emE_Id) {
                                        $scope.exname = ex.emE_ExamName;
                                    }
                                });
                            }

                            var data = {
                                studentlistdto: $scope.printstudents,
                                "SmsMailText": stdmsg,
                                "radiotype": $scope.radioval,
                                "snd_email": $scope.snd_email,
                                "snd_sms": $scope.snd_sms,
                                "template": $scope.template1
                            };
                            apiService.create("Collegegeneralsms/savedetail", data).then(function (promise) {
                                if (promise.emailStatus === 'sent') {

                                    if ($scope.snd_email === true && $scope.snd_sms === true) {
                                        swal("SMS And Mail Sent Successfully.");
                                    } else if ($scope.snd_email === true && $scope.snd_sms === false) {
                                        swal("Mail Sent Successfully.");
                                    } else if ($scope.snd_email === false && $scope.snd_sms === true) {
                                        swal("SMS Sent Successfully.");
                                    } else {
                                        swal("Something Went Wrong Kindly Contact Administrator");
                                    }

                                    $state.reload();
                                } else {
                                    swal("Failed To Sent SMS And Email");
                                }

                            });
                        }
                        else {
                            swal("SMS And Mail Sending Cancelled.");
                        }
                    });

            } else {
                swal("Kindly select atleast a record to procced..!");
                return;
            }
        };

        // ***** Sending SMS and Email For Staff **** //

        $scope.SendStaffData = function () {
            //alert($scope.radioval)
            $scope.printstudents = [];
            angular.forEach($scope.employeelst, function (employee) {
                if (employee.hrmE_MobileNo != null) {
                    if (!!employee.selected) $scope.printstudents.push(employee);
                }
            });

            if ($scope.printstudents.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send SMS?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data = {
                                studentlistdto: $scope.printstudents,
                                "SmsMailText": $scope.stfmsg,
                                "radiotype": $scope.radioval,
                                "stfsnd_email": $scope.stfsnd_email,
                                "stfsnd_sms": $scope.stfsnd_sms,
                                "template": $scope.template2

                            };
                            apiService.create("Collegegeneralsms/savedetail", data)
                                .then(function (promise) {
                                    if (promise.smsStatus === 'sent') {
                                        if ($scope.stfsnd_email === true && $scope.stfsnd_sms === true) {
                                            swal("SMS And Mail Sent Successfully.");
                                        } else if ($scope.stfsnd_email === true && $scope.stfsnd_sms === false) {
                                            swal("Mail Sent Successfully.");
                                        } else if ($scope.stfsnd_email === false && $scope.stfsnd_sms === true) {
                                            swal("SMS Sent Successfully.");
                                        } else {
                                            swal("Something Went Wrong Kindly Contact Administrator");
                                        }
                                        $state.reload();
                                    }
                                    else {
                                        swal("SMS Not Sent for all selection.");
                                    }
                                });
                        }
                        else {
                            swal("SMS Sending Cancelled.");
                        }
                    });

            } else {
                swal("Kindly select atleast a record to having  mobile number to procced..!");
                return;
            }
        };


        $scope.onSelectyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMSE_Id = "";
            $scope.AMB_Id = "";
            $scope.ACMS_Id = "";
            $scope.studentList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("Collegegeneralsms/onSelectyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.courselist = promise.courselist;
                }
                else {
                    swal("No Course Is Mapped For Selected Year");
                }
            });
        };

        $scope.onselectedcourse = function () {
            $scope.AMSE_Id = "";
            $scope.AMB_Id = "";
            $scope.ACMS_Id = "";
            $scope.studentList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };

            apiService.create("Collegegeneralsms/onselectedcourse", data).then(function (promise) {
                if (promise !== null) {
                    $scope.branchlist = promise.branchlist;
                }
                else {
                    swal("No Branch Is Mapped For Selected Details");
                }
            });
        };

        $scope.onselectbranch = function () {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.studentList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("Collegegeneralsms/onselectbranch", data).then(function (promise) {
                if (promise !== null) {
                    $scope.semisterlist = promise.semisterlist;
                }
                else {
                    swal("No Semister Is Mapped For Selected Details");
                }
            });
        };

        $scope.onselectsemister = function () {
            $scope.ACMS_Id = "";
            $scope.studentList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };

            apiService.create("Collegegeneralsms/onselectsemister", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sectionlist = promise.sectionlist;
                }
                else {
                    swal("No Section Is Mapped For Selected Details");
                }
            });
        };

        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.studentList, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };

        $scope.printstudents = [];
        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.Stflist;
            angular.forEach($scope.employeelst, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.Stflist == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };

        $scope.optionToggled1 = function (SelectedStudentRecord, index) {
            $scope.all = $scope.studentList.every(function (options) {
                return options.selected;
            });
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.Stflist = $scope.employeelst.every(function (options) {
                return options.selected;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field1) {
            return $scope.submitted1;
        };

        $scope.interacted2 = function (field2) {
            return $scope.submitted2;
        };

        $scope.cancel = function () {
            $scope.mobno = "";
            $scope.mes = "";
            $state.reload();
        };

        $scope.cancel1 = function () {
            $scope.stdmsg = "";
            $scope.all = false;
            angular.forEach($scope.studentList, function (user) {
                user.selected = false;
            });
        };

        $scope.cancel2 = function () {
            $scope.stfmsg = "";
            $scope.Stflist = false;
            angular.forEach($scope.departmentdropdown, function (user) {
                user.selected = false;
            });
            $scope.employeelst = [];
            $scope.desgcheck = false;
            $scope.stf = false;
            $scope.Designation_types = [];
        };
    }
})();