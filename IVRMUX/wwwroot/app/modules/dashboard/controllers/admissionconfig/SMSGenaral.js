
(function () {
    'use strict';
    angular
        .module('app')
        .controller('SMSGenaralController', SMSGenaralController)
    SMSGenaralController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    function SMSGenaralController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {
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
            //hard code
            //emailotp = 1;
            //mobileotp = 1;
            //$scope.emailotp =1;
         //$scope.mobileotp = 0;

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


            var mobilenofull = configsettings[0].ivrmgC_OTPMobileNo.toString();
        
            if (mobilenofull != '0') {
                var otpmobile = mobilenofull.substring(6, 10);
                $scope.mobileno = otpmobile;
            }
            var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
           //  emailidforotp = 'praveend114@gmail.com';
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

        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.changeradio1 = function () {
            $scope.stdmsg = 'Hi';
            if ($scope.selradioval == 'exam') {

                $scope.stdmsg = 'Hi';
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    // "ASMS_Id": $scope.asmS_Id
                }
                if ($scope.count > 0) {
                    apiService.create("SMSGenaral/Getexam/", data).
                        then(function (promise) {

                            $scope.exmstdlist = promise.exmstdlist;

                        });
                }



            }
        }

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
                })
            }
        }

        $scope.resendotp = function () {
            $('#myModalswal').modal('show');
            //  $('#myModalotp').modal('show');
        }

        $scope.stfsearchValue = '';
        $scope.sendotpsms = function (forgetEmail) {

            $("#myModalswal").modal({ backdrop: false });
            $scope.resendotpbutton = false;
            $scope.forgetEmailOTP = "";

            if (emailidforotp == null || emailidforotp == undefined) {
                emailidforotp = "test@mail";
            }
            // $('#myModalotp').modal('hide');
            var mobno = {
                "clickedlinkname": forgetEmail,
                "Mobile": mobilenofull.toString(),
                "Email": emailidforotp.toString(),
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("Login/ForgotOTPForEmailval_new", mobno).then(function (promise) {
                if ($scope.radioval == 'Student') {
                    $scope.clslst2 = $scope.albumNameArray;
                    $("#myModalswal").modal({ backdrop: false });
                }
                if ($scope.radioval == 'Staff') {
                    $scope.clslst21 = $scope.albumNameArray;
                    $("#myModalswal").modal({ backdrop: false });
                }

                console.log($scope.clslst2);
                $('#myModalswal').modal('show');
                $("#myModalswal").modal({ backdrop: false });
            })

        }


        $scope.submitted = true;
        $scope.email = true;
        $scope.sms = true;
        $scope.saveUseronce = function () {
            debugger;
            //alert($scope.template2)
            $scope.otpmobile = false;
            $scope.otpemail = false;
            $scope.buttonotp = false;
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
                angular.forEach($scope.studentList, function (user) {
                    if (!!user.selected) {
                        user.msg = $scope.stdmsg;
                        $scope.albumNameArray.push(user);
                    }
                })
                if ($scope.albumNameArray.length > 0 && $scope.template1 != undefined && $scope.template1!='') {
                    $("#myModalotp").modal({ backdrop: false });
                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
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
                    swal('Kindly select atleast one student..! and Enter Template Name');
                    return;
                }
            }

            if ($scope.radioval == 'General') {
                if ($scope.myForm.$valid) {

                    $("#myModalotp").modal({ backdrop: false });
                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
                        // emailidforotp = 'praveend114@gmail.com';
                        if (emailidforotp != null || emailidforotp != undefined) {
                            $scope.emailid = emailidforotp.substring(0, 4);
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }

                    }
                    else
                        if ((mobileotp != 0 && ($scope.email == true || $scope.sms == true))) {

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
                    $scope.submitted = true;
                }
            }


            if ($scope.radioval == 'Staff') {
                debugger;
                $scope.albumNameArray = [];
                angular.forEach($scope.employeelst, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                })

                if ($scope.albumNameArray.length > 0 && $scope.template2 != undefined && $scope.template2 != '') {
                    $("#myModalotp").modal({ backdrop: false });
                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
                        // emailidforotp = 'praveend114@gmail.com';

                        if (emailidforotp != null || emailidforotp != undefined) {

                            $scope.emailid = emailidforotp.substring(0, 4);
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }

                    }
                    else if ((mobileotp != 0 && ($scope.email == true || $scope.sms == true))) {
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
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.changeradio = function () {
            //alert('a')
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            if ($scope.loadradioval == true) {
                $scope.radioval = 'General';
                $scope.loadradioval = false;
            }

            if ($scope.radioval == 'General') {
                $scope.mobno = "";
                $scope.mes = "";
            }

            if ($scope.radioval == 'Student') {


                $scope.snd_email = true;
                $scope.snd_sms = true;
                $scope.selradioval = '';
                $scope.emE_Id = '';
                $scope.exmstdlist = [];


                $scope.stdmsg = "";
                var data = {
                    "selectedRadiobtn": $scope.radioval
                }

                $scope.studentList = [];
                apiService.create("SMSGenaral/Getdetails/", data).
                    then(function (promise) {

                        $scope.fillacademiyear = promise.yearlist;
                        $scope.currentYear = promise.currentYear;
                        for (var i = 0; i < $scope.fillacademiyear.length; i++) {
                            if ($scope.currentYear[0].asmaY_Id == $scope.fillacademiyear[i].asmaY_Id) {
                                $scope.ASMAY_Id = $scope.currentYear[0].asmaY_Id;
                            }
                            $scope.classlist = promise.classlist;
                            $scope.ASMCL_Id = $scope.classlist[0].asmcL_Id;
                            $scope.sectionlist = promise.sectionlist;
                            $scope.asmS_Id = $scope.sectionlist[0].asmS_Id;

                            if (promise.studentCount > 0) {
                                $scope.count = promise.studentCount;
                                $scope.studentList = promise.studentlist;
                                //  console.log($scope.studentList);
                            }
                            else {
                                swal("No records found for selected academicYear,class and section");
                                $scope.count = 0;
                            }
                        }
                    });
            }
            if ($scope.radioval == 'Staff') {
                var id = 1;
                $scope.departmentdropdown = [];
                $scope.Designation_types = [];
                $scope.stfsnd_email = true;
                $scope.stfsnd_sms = true;
                $scope.deptcheck = false;

                $scope.employeelst = [];
                $scope.stfmsg = "";
                apiService.getURI("SMSGenaral/Getdepartment", id).
                    then(function (promise) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        if (promise.departmentdropdown != null) {
                            $scope.deptcheck = true;

                            $scope.all_checkdep();
                        }
                    })
            }
        }

        $scope.all_checkdep = function () {

            $scope.employeelst = [];
            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_designation();
        }

        $scope.get_designation = function () {
            $scope.employeelst = [];
            $scope.deptcheck = $scope.departmentdropdown.every(function (options) {
                return options.selected;
            });

            $scope.get_designationnew();
        }
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
                    "multipledep": groupidss,
                }
                apiService.create("SMSGenaral/get_designation", data).
                    then(function (promise) {

                        $scope.Designation_types = promise.designationdropdown;
                        if (promise.designationdropdown != null) {
                            $scope.desgcheck = true;
                            $scope.all_checkdesg();
                        }
                    })
            }
            else {
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        }

        $scope.all_checkdesg = function () {

            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        }

        //fill desg end
        //fill employee start
        $scope.get_employee = function () {
            $scope.employeelst = [];
            $scope.desgcheck = $scope.Designation_types.every(function (options) {

                return options.selected;
            });

            $scope.get_employeenew();
        }
        $scope.get_employeenew = function () {
            $scope.stf = false;
            $scope.Stflist = false;
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
                }
                apiService.create("SMSGenaral/get_employee", data).
                    then(function (promise) {

                        $scope.employeelst = promise.stafflist;
                        if ($scope.employeelst.length > 0) {
                            $scope.stf = true;
                        }
                    })
            }
            else {
                $scope.employeelst = "";
            }
        }

        $scope.GetStudentDetails = function () {
            $scope.selradioval = '';
            $scope.emE_Id = '';
            $scope.all = false;
            $scope.exmstdlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.asmS_Id
            }
            apiService.create("SMSGenaral/GetStudentDetails/", data).
                then(function (promise) {

                    if (promise.studentCount > 0) {

                        if ($scope.selradioval == 'exam') {

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
        }

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
                    }
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
                        "crnt_date": $scope.curnt_date,
                    }
                }

                apiService.create("SMSGenaral/searchstddetails/", data).
                    then(function (promise) {

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


                                        })
                                        var finaltotal = '';
                                        var finalgrade = '';
                                        angular.forEach($scope.stumarkdetails, function (fnl) {
                                            if (stu.amsT_Id == fnl.amsT_Id) {
                                                finaltotal = fnl.totalMarks;
                                                finalgrade = fnl.totalGrade;


                                            }


                                        })
                                        grade = grade + ',' + finalgrade;
                                        marks = marks + ',' + finaltotal;

                                        $scope.temparray.push({ amsT_Id: stu.amsT_Id, studentName: stu.studentName, amsT_AdmNo: stu.amsT_AdmNo, amsT_MobileNo: stu.amsT_MobileNo, amsT_emailId: stu.amsT_emailId, marksDetails: marks, gradeDetails: grade });

                                    })

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

                                        })
                                        $scope.temparray1.push({ amsT_Id: stu1.amsT_Id, studentName: stu1.studentName, amsT_AdmNo: stu1.amsT_AdmNo, amsT_MobileNo: stu1.amsT_MobileNo, amsT_emailId: stu1.amsT_emailId, atndetails: attt });

                                    })
                                    $scope.studentList = $scope.temparray1;
                                }
                            }

                        } else {
                            swal("No records found for selected academicYear,class and section");
                            $scope.studentList = "";
                            $scope.count = 0;
                        }
                    });

            }
            else {
                $scope.submitted1 = true;
            }

        }


        $scope.order1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }


        $scope.order = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };
        // hard code 
        $scope.otpapproveflag = true;
        $scope.approveflag = false

        $scope.submitted = false;
        $scope.SendData = function () {
           
            if ($scope.myForm.$valid) {
                //    var Send = {
                //        "Mobno": $scope.mobno,
                //        "mes": $scope.mes,
                //        "radiotype": $scope.radioval
                //    }
                //    apiService.create("SMSGenaral/savedetail", Send).then(function (promise) {
                //        if (promise.smsStatus == 'sent') {
                //            swal("SMS Sent Successfully.");
                //            $state.reload();
                //        }
                //        else {
                //            swal("SMS Sending Cancelled.");
                //        }
                //    });

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
                                "template": $scope.template,
                                "otpapproveflag": $scope.otpapproveflag,
                                "approveflag": $scope.approveflag,
                            }

                            apiService.create("SMSGenaral/savedetail", Send).then(function (promise) {
                                if (promise.smsStatus == 'sent') {
                                    swal("SMS Sent Successfully.");
                                    $state.reload();
                                }
                                else if (promise.smsStatus=='user') {
                                    swal("Approval User is not Mapped .");
                                }
                                else if (promise.smsStatus=='approve') {
                                    swal("Waiting For Approval.");
                                    $state.reload();
                                }
                                else {
                                    swal("SMS Sending Cancelled.");
                                }
                            });



                        }
                        else {
                        }
                    });

            }
            else {
                $scope.submitted = true;
            }
        }




        $scope.SendMSG = function (stdmsg) {
            //alert($scope.radioval)

            $('#myModalswal').modal('hide');
            $scope.printstudents = [];
            angular.forEach($scope.studentList, function (user) {
                if (!!user.selected) $scope.printstudents.push(user);
            })

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

                                })
                            }
                            debugger;
                            var data = {
                                studentlistdto: $scope.printstudents,
                                "SmsMailText": stdmsg,
                                "radiotype": $scope.radioval,
                                "snd_email": $scope.snd_email,
                                "snd_sms": $scope.snd_sms,
                                "template": $scope.template1,
                                "otpapproveflag": $scope.otpapproveflag,
                                "approveflag": $scope.approveflag,
                            };
                            apiService.create("SMSGenaral/savedetail", data).then(function (promise) {
                                if (promise.emailStatus == 'sent') {
                                    swal("SMS And Mail Sent Successfully.");
                                    $state.reload();
                                }
                                else if (promise.smsStatus == 'user') {
                                    swal("Approval User is not Mapped .");
                                }
                                else if (promise.smsStatus == 'approve') {
                                    swal("Waiting For Approval.");
                                    $state.reload();
                                }
                                else {
                                    swal("SMS Sending Cancelled.");
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
        }


        $scope.SendStaffData = function () {
            //alert($scope.radioval)

            $scope.printstudents = [];
            angular.forEach($scope.employeelst, function (employee) {
                if (employee.hrmE_MobileNo != null) {
                    if (!!employee.selected) $scope.printstudents.push(employee);
                }

            })

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
                                "template": $scope.template2,
                                "otpapproveflag": $scope.otpapproveflag,
                                "approveflag": $scope.approveflag,

                            };
                            apiService.create("SMSGenaral/savedetail", data)
                                .then(function (promise) {
                                    if (promise.smsStatus == 'sent') {
                                        swal("SMS Sent Successfully.");
                                       $state.reload();
                                    }
                                    else if (promise.smsStatus == 'user') {
                                        swal("Approval User is not Mapped .");
                                    }
                                    else if (promise.smsStatus == 'approve') {
                                        swal("Waiting For Approval.");
                                        $state.reload();
                                    }
                                    else {
                                        swal("SMS Sending Cancelled.");
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
        }

        $scope.onSelectclass = function (classId) {



            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.asmS_Id > 0) {
                $scope.GetStudentDetails();
            }
        }
        $scope.onSelectyear = function () {


            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.asmS_Id > 0) {
                $scope.GetStudentDetails();
            }
        }
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
        }

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
        }



        $scope.optionToggled1 = function (SelectedStudentRecord, index) {

            $scope.all = $scope.studentList.every(function (options) {

                return options.selected;
            });

            //$scope.all = $scope.employeelst.every(function (itm)
            //{ return itm.selected; });
            //if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
            //    $scope.printstudents.push(SelectedStudentRecord);
            //}
            //else {
            //    $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            //}
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.Stflist = $scope.employeelst.every(function (options) {

                return options.selected;
            });

            //$scope.all = $scope.employeelst.every(function (itm)
            //{ return itm.selected; });
            //if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
            //    $scope.printstudents.push(SelectedStudentRecord);
            //}
            //else {
            //    $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            //}
        }

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
        }
        $scope.cancel1 = function () {

            $scope.stdmsg = "";

            $scope.all = false;
            angular.forEach($scope.studentList, function (user) {
                user.selected = false;
            })
        }
        $scope.cancel2 = function () {
            $scope.stfmsg = "";
            $scope.Stflist = false;
            angular.forEach($scope.departmentdropdown, function (user) {
                user.selected = false;
            })
            $scope.employeelst = [];
            $scope.desgcheck = false;
            $scope.stf = false;
            $scope.Designation_types = [];
        }
    }
})();