(function () {
    'use strict';
    angular.module('app').controller('CollegeDefaultersReportController', CollegeDefaultersReportController)
    CollegeDefaultersReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', 'uiGridConstants']
    function CollegeDefaultersReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, uiGridConstants) {

        $scope.colarrayaggre = [];
        $scope.usrname = localStorage.getItem('username');
    
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        if (configsettings !== null && configsettings.length > 0) {
            var grouporterm = configsettings[0].fmC_GroupOrTermFlg;
        }

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.checkall = true;
        $scope.check = function () {
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.arrlistchkgroup, function (itm) {
                itm.selected = toggleStatus;
            });
        }
        var configsetting = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (configsetting !== null && configsetting.length > 0) {
            var emailotp = configsetting[0].ivrmgC_emailValOTPFlag;
            var mobileotp = configsetting[0].ivrmgC_MobileValOTPFlag;

            $scope.emailotp = configsetting[0].ivrmgC_emailValOTPFlag;
            $scope.mobileotp = configsetting[0].ivrmgC_MobileValOTPFlag;

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


            var mobilenofull = configsetting[0].ivrmgC_OTPMobileNo.toString();
            if (mobilenofull != '0') {
                var otpmobile = mobilenofull.substring(6, 10);
                $scope.mobileno = otpmobile;
            }
            var emailidforotp = configsetting[0].ivrmgC_OTPMailId;
            if (emailidforotp != null || emailidforotp != undefined) {
                $scope.emailid = emailidforotp.substring(0, 4);
            }
            if ($scope.emailotp == true || $scope.mobileotp == true) {
                $scope.otpcheck = true;

            }
            else {
                $scope.otpcheck = false;
            }
        }
        else {
            emailotp = false;
            mobileotp = false;
        }

        $scope.smssending = function () {

        }

        $scope.emailsending = function () {

        }
      

        $scope.printdatatable = [];
        $scope.printdatatablegrp = [];
        $scope.printdatatablehad = [];
        $scope.printdatatablecls = [];
        $scope.student_install_wise = function () {

            $scope.std = false;
            $scope.grp = false;
            $scope.cls = false;
            $scope.had = false;
            $scope.catg = false;
            $scope.Grid_view = false;
            $scope.printdatatable = [];
            $scope.printdatatablegrp = [];
            $scope.printdatatablehad = [];
            $scope.printdatatablecls = [];
            $scope.printdatatablecat = [];
            $scope.stdall = false;
            angular.forEach($scope.students, function (obj) {
                obj.stdselected = false;
            });
            $scope.grpall = false;
            angular.forEach($scope.groups, function (obj) {
                obj.grpselected = false;
            });
            $scope.clsall = false;
            angular.forEach($scope.classes, function (obj) {
                obj.clsselected = false;
            });
            $scope.hadall = false;
            angular.forEach($scope.heads, function (obj) {
                obj.hadselected = false;
            });
            $scope.catall = false;
            angular.forEach($scope.category, function (obj) {
                obj.catselected = false;
            });

            if ($scope.printdatatablecls.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatablecat.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

        }
        $scope.get_total_student_print = function () {
            var s_total_FSS_PaidAmount_p = 0;
            var s_total_concession_p = 0;
            var s_total_balance_p = 0;

            var s_total_fine_p = 0;
            var s_total_rebate_p = 0;
            var s_total_waived_p = 0;
            angular.forEach($scope.printdatatable, function (stu) {
                s_total_FSS_PaidAmount_p += stu.FSS_PaidAmount;
                s_total_balance_p += stu.totalbalance;
                s_total_concession_p += stu.concession;
                s_total_fine_p += stu.fine;
                s_total_rebate_p += stu.rebate;
                s_total_waived_p += stu.waived;
            })
            $scope.s_total_FSS_PaidAmount_p = s_total_FSS_PaidAmount_p;
            $scope.s_total_balance_p = s_total_balance_p;
            $scope.s_total_concession_p = s_total_concession_p;
            $scope.s_total_fine_p = s_total_fine_p;
            $scope.s_total_rebate_p = s_total_rebate_p;
            $scope.s_total_waived_p = s_total_waived_p;
        }

        $scope.toggleAllstd = function () {

            var toggleStatus = $scope.stdall;
            $scope.printdatatable = [];
            angular.forEach($scope.students, function (itm) {
                itm.stdselected = toggleStatus;
                if ($scope.stdall == true) {
                    $scope.printdatatable.push(itm);
                }

            });
            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();
        }
        $scope.optionToggledstd = function (SelectedStudentRecord, index) {

            $scope.stdall = $scope.students.every(function (itm) { return itm.stdselected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();
        }
        $scope.get_total_group_print = function () {
            var g_total_FSS_PaidAmount_p = 0;
            var g_total_concession_p = 0;
            var g_total_balance_p = 0;
            var g_total_fine_p = 0;
            var g_total_rebate_p = 0;
            var g_total_waived_p = 0;
            angular.forEach($scope.printdatatablegrp, function (gp) {
                g_total_FSS_PaidAmount_p += gp.FSS_PaidAmount;
                g_total_balance_p += gp.totalbalance;
                g_total_concession_p += gp.concession;
                g_total_fine_p += gp.fine;
                g_total_rebate_p += gp.rebate;
                g_total_waived_p += gp.waived;
            })
            $scope.g_total_FSS_PaidAmount_p = g_total_FSS_PaidAmount_p;
            $scope.g_total_balance_p = g_total_balance_p;
            $scope.g_total_concession_p = g_total_concession_p;
            $scope.g_total_fine_p = g_total_fine_p;
            $scope.g_total_rebate_p = g_total_rebate_p;
            $scope.g_total_waived_p = g_total_waived_p;
        }
        $scope.toggleAllgrp = function () {

            $scope.printdatatablegrp = [];
            var toggleStatus = $scope.grpall;
            angular.forEach($scope.groups, function (itm) {
                itm.grpselected = toggleStatus;
                if ($scope.grpall == true) {
                    $scope.printdatatablegrp.push(itm);
                }

            });
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_group_print();
        }
        $scope.optionToggledgrp = function (SelectedStudentRecord, index) {

            $scope.grpall = $scope.groups.every(function (itm) { return itm.grpselected; });

            if ($scope.printdatatablegrp.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablegrp.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablegrp.splice($scope.printdatatablegrp.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_group_print();
        }
        $scope.get_total_head_print = function () {
            var h_total_FSS_PaidAmount_p = 0;
            var h_total_concession_p = 0;
            var h_total_balance_p = 0;
            var h_total_fine_p = 0;
            var h_total_rebate_p = 0;
            var h_total_waived_p = 0;
            angular.forEach($scope.printdatatablehad, function (hd) {
                h_total_FSS_PaidAmount_p += hd.FSS_PaidAmount;
                h_total_balance_p += hd.totalbalance;
                h_total_concession_p += hd.concession;
                h_total_fine_p += hd.fine;
                h_total_rebate_p += hd.rebate;
                h_total_waived_p += hd.waived;
            })
            $scope.h_total_FSS_PaidAmount_p = h_total_FSS_PaidAmount_p;
            $scope.h_total_concession_p = h_total_concession_p;
            $scope.h_total_balance_p = h_total_balance_p;
            $scope.h_total_fine_p = h_total_fine_p;
            $scope.h_total_rebate_p = h_total_rebate_p;
            $scope.h_total_waived_p = h_total_waived_p;
        }
        $scope.toggleAllhad = function () {

            var toggleStatus = $scope.hadall;
            $scope.printdatatablehad = [];
            angular.forEach($scope.heads, function (itm) {
                itm.hadselected = toggleStatus;
                if ($scope.hadall == true) {
                    $scope.printdatatablehad.push(itm);
                }
            });
            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_head_print();
        }
        $scope.toggleAllcat = function () {

            var toggleStatus = $scope.catall;
            $scope.printdatatablecat = [];
            angular.forEach($scope.category, function (itm) {
                itm.catselected = toggleStatus;
                if ($scope.catall == true) {
                    $scope.printdatatablecat.push(itm);
                }
            });
            if ($scope.printdatatablecat.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_cat_print();
        }
        $scope.optionToggledhad = function (SelectedStudentRecord, index) {

            $scope.hadall = $scope.heads.every(function (itm) { return itm.hadselected; });
            if ($scope.printdatatablehad.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablehad.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablehad.splice($scope.printdatatablehad.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_head_print();
        }
        $scope.optionToggledcat = function (SelectedStudentRecord, index) {

            $scope.catall = $scope.category.every(function (itm) { return itm.catselected; });
            if ($scope.printdatatablecat.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablecat.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablecat.splice($scope.printdatatablecat.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatablecat.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_cat_print();
        }
        $scope.get_total_class_print = function () {
            var c_total_FSS_PaidAmount_p = 0;
            var c_total_concession_p = 0;
            var c_total_balance_p = 0;
            var c_total_fine_p = 0;
            var c_total_rebate_p = 0;
            var c_total_waived_p = 0;
            angular.forEach($scope.printdatatablecls, function (cls) {
                c_total_FSS_PaidAmount_p += cls.FSS_PaidAmount;
                c_total_balance_p += cls.totalbalance;
                c_total_concession_p += cls.concession;
                c_total_fine_p += cls.fine;
                c_total_rebate_p += cls.rebate;
                c_total_waived_p += cls.waived;
            })
            $scope.c_total_FSS_PaidAmount_p = c_total_FSS_PaidAmount_p;
            $scope.c_total_balance_p = c_total_balance_p;
            $scope.c_total_concession_p = c_total_concession_p;
            $scope.c_total_fine_p = c_total_fine_p;
            $scope.c_total_rebate_p = c_total_rebate_p;
            $scope.c_total_waived_p = c_total_waived_p;
        }
        $scope.toggleAllcls = function () {

            var toggleStatus = $scope.clsall;
            $scope.printdatatablecls = [];
            angular.forEach($scope.classes, function (itm) {
                itm.clsselected = toggleStatus;
                if ($scope.clsall == true) {
                    $scope.printdatatablecls.push(itm);
                }
                //else {
                //    $scope.printdatatablecls.splice(itm);
                //}
            });
            if ($scope.printdatatablecls.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_print();
        }
        $scope.optionToggledcls = function (SelectedStudentRecord, index) {

            $scope.clsall = $scope.classes.every(function (itm) { return itm.clsselected; });
            if ($scope.printdatatablecls.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablecls.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablecls.splice($scope.printdatatablecls.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatablecls.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_print();
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


        $scope.sendotpsms = function (forgetEmail) {
            $scope.resendotpbutton = false;
            $scope.forgetEmailOTP = "";
            if (emailidforotp == null || emailidforotp == undefined) {
                emailidforotp = "test@mail";
            }

            $('#myModalotp').modal('hide');
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
            apiService.create("Login/ForgotOTPForEmailval", mobno).then(function (promise) {
                $scope.studentlistt = $scope.albumNameArray;
                $("#myModalswal").modal({ backdrop: false });
                $('#myModalswal').modal('show');
            })

        }











        $scope.onclickloaddata = function () {
            $scope.Grid_view = false;
            $scope.printdatatable = [];
            $scope.printdatatablegrp = [];
            $scope.printdatatablehad = [];
            $scope.printdatatablecls = [];
            $scope.printdatatablecat = [];

            $scope.stdall = false;
            angular.forEach($scope.students, function (obj) {
                obj.stdselected = false;
            });
            $scope.grpall = false;
            angular.forEach($scope.groups, function (obj) {
                obj.grpselected = false;
            });
            $scope.clsall = false;
            angular.forEach($scope.classes, function (obj) {
                obj.clsselected = false;
            });
            $scope.hadall = false;
            angular.forEach($scope.heads, function (obj) {
                obj.hadselected = false;
            });
            $scope.catall = false;
            angular.forEach($scope.category, function (obj) {
                obj.catselected = false;
            });

            if ($scope.printdatatablecls.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.rpttyp == "year") {
                $scope.frmdt = false;
                $scope.adyr = true;
            }
            else if ($scope.rpttyp == "date") {
                $scope.frmdt = true;
                $scope.adyr = false;
            }
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };




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
            $('#myModalswal').modal('hide');
            $('#myModalotp').modal('show');
        }


        $scope.submitted1 = false;
        $scope.saveUseronce = function () {

            $scope.otptype = false;
            $scope.otpmobile = false;
            $scope.otpemail = false;
            $scope.buttonotp = false;
            if ($scope.emailotp == true || $scope.mobileotp == true) {
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

            if ($scope.myForm.$valid) {
                $scope.albumNameArray = [];
                angular.forEach($scope.students, function (user) {
                    if (!user.stdselected) {
                        $scope.albumNameArray.push(user);
                    }
                })
                if ($scope.albumNameArray.length > 0) {


                    if ((emailotp != false && ($scope.email == true || $scope.sms == true))) {
                        var emailidforotp = configsetting[0].ivrmgC_OTPMailId;
                        if (emailidforotp != null || emailidforotp != undefined) {

                            $scope.emailid = emailidforotp.substring(0, 4);
                            $("#myModalotp").modal({ backdrop: false });
                            $('#myModalotp').modal('show');

                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }

                    }
                    else if ((mobileotp != false && ($scope.email == true || $scope.sms == true))) {

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
                        $scope.studentlistt = $scope.albumNameArray;
                        $('#myModalswal').modal('show');
                    }


                }
                else {

                    swal('Kindly select atleast one student..!');
                    return;
                }
            }
            else {
                $scope.submitted = true;
            }

        }


        $scope.selectedStudentListforemail = [];
        $scope.send_mail = function () {
            debugger;
            if ($scope.students != null && $scope.students != "") {
                for (var i = 0; i < $scope.students.length; i++) {
                    if ($scope.students[i].stdselected == true) {
                        $scope.selectedStudentListforemail.push($scope.students[i]);
                    }
                }
            }

            if ($scope.selectedStudentListforemail.length == 0) {
                swal("Please select records to send email");
            }
            else {
                var FMG_Ids = [];
                var FMT_Ids = [];

                angular.forEach($scope.group, function (ty) {
                    if (ty.fmG_Id_chk1) {
                        FMG_Ids.push(ty.fmG_Id);
                    }
                })

                var data = {
                    FMG_Ids: FMG_Ids,
                    //  FMT_Ids: FMT_Ids,
                    "term_group": grouporterm,
                    // "TempTerm": $scope.fmtdeatls,
                    "selectedStudentList": $scope.selectedStudentListforemail,
                    "ASMAY_Id": $scope.asmaY_Id,
                    // "ASMCL_Id": $scope.asmcL_Id

                };
                apiService.create("CollegeDefaultersReport/Sendmail", data).then(function (promise) {
                    if (promise.msg == "success") {
                        swal("Email Sent Successfully");
                        //$state.reload();
                    }
                    else if (promise.msg == "failed") {
                        swal("Email Not Sent");
                        //  $state.reload();
                    }

                })
            }
        }







        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.tabel12 = false;
            $scope.tabel123 = false;

            var data = {
                "configset": grouporterm,
            }

            apiService.create("CollegeDefaultersReport/getalldetails", data).
                then(function (promise) {
                    $scope.yearlst = promise.yearlst;
                    $scope.arrlistchkgroup = promise.grouplist;
                    $scope.sectioncount = promise.fillsection;
                    angular.forEach($scope.arrlistchkgroup, function (tr) {
                        tr.selected = true;
                    })
                    $scope.seclist = promise.fillfeehead;
                    $scope.colsemisterlist = promise.semisterlist;

                    $scope.checkallhrd = true;
                    //$scope.onclickloaddata();
                    // $scope.load_group_check();


                    $scope.sort = function (keyname) {
                        $scope.sortKey = keyname;   //set the sortKey to the param passed
                        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
                    }
                })
        }

        $scope.hrdallcheck = function () {
            var toggleStatus1 = $scope.checkallhrd;
            angular.forEach($scope.colsemisterlist, function (itm) {
                itm.selectedsem = toggleStatus1;
            });

        }



        $scope.checkallcourse = function () {
            var toggleStatus1 = $scope.checkcourse;
            angular.forEach($scope.coursecount, function (itm) {
                itm.selectedcourse = toggleStatus1;
            });
            $scope.get_branches();
            $scope.checkbranch = true;
        }

        $scope.checkallbranch = function () {
            var toggleStatus1 = $scope.checkbranch;
            angular.forEach($scope.branchcount, function (itm) {
                itm.selectedbranch = toggleStatus1;
            });
            $scope.get_semisters();
            $scope.checkallhrd = true;

        }

        
        $scope.hrgsinglecheck = function () {

            $scope.checkallhrd = $scope.colsemisterlist.every(function (itm) {

                return itm.selectedsem;
            });


        };
        $scope.onselectyear = function (obj) {

            var data = {
                "ASMAY_Id": obj.ASMAY,
            }


            apiService.create("CollegeDefaultersReport/get_courses", data).
                then(function (promise) {

                    $scope.coursecount = promise.courselist;
                    //   $scope.binddatagrp();

                })

        };

        $scope.get_branches = function (obj) {

            var AMCO_Ids = [];
            angular.forEach($scope.coursecount, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })
            var data = {
                "ASMAY_Id": $scope.obj.ASMAY,
                AMCO_Ids: AMCO_Ids,

            }

            apiService.create("CollegeDefaultersReport/get_branches", data).
                then(function (promise) {
                    $scope.branchcount = promise.branchlist;
                    angular.forEach($scope.branchcount, function (fy) {
                        fy.selectedbranch = true;
                        $scope.get_semisters();
                    })
                })

        };

        $scope.get_semisters = function () {

            var AMB_Ids = [];
            angular.forEach($scope.branchcount, function (ty) {
                if (ty.selectedbranch) {
                    AMB_Ids.push(ty.amB_Id);
                }
            })
            var AMCO_Ids = [];
            angular.forEach($scope.coursecount, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })

            var data = {
                "ASMAY_Id": $scope.obj.ASMAY,
                AMB_Ids: AMB_Ids,
                AMCO_Ids: AMCO_Ids

            }



            apiService.create("CollegeDefaultersReport/get_semisters", data).
                then(function (promise) {

                    $scope.colsemisterlist = promise.semisterlist;


                })

        };



        $scope.selectedStudentList = [];
        $scope.send_sms = function () {
            if ($scope.students != null && $scope.students != "") {
                for (var i = 0; i < $scope.students.length; i++) {
                    if ($scope.students[i].stdselected == true) {
                        $scope.selectedStudentList.push($scope.students[i]);
                    }
                }
            }
            if ($scope.selectedStudentList.length == 0) {
                swal("Please select records to send sms");
            }
            else {
                var FMG_Ids = [];
                var FMT_Ids = [];

                angular.forEach($scope.group, function (ty) {
                    if (ty.fmG_Id_chk1) {
                        FMG_Ids.push(ty.fmG_Id);
                    }
                })

                var data = {
                    FMG_Ids: FMG_Ids,
                    "term_group": grouporterm,
                    "selectedStudentList": $scope.selectedStudentList,
                    "ASMAY_Id": $scope.asmaY_Id,
                    //"ASMCL_Id": $scope.asmcL_Id
                };
                apiService.create("CollegeDefaultersReport/SendSms", data).then(function (promise) {
                    if (promise.msg == "successSMS") {
                        swal("SMS Sent Successfully");
                        //$state.reload();
                    }
                    else if (promise.msg == "failedSMS") {
                        swal("SMS Not Sent");
                        // $state.reload();
                    }
                })
            }
        }



        $scope.ShowReport = function (termlst, asmaY_Id, fromDate, rpttyp, result, status, due, asmcL_Id, trmR_Id) {

            var AMCO_Ids = [];
            var AMB_Ids = [];
            var FMG_Ids = [];
             var AMSE_Ids = [];

            angular.forEach($scope.arrlistchkgroup, function (ty) {
                if (ty.selected) {
                    FMG_Ids.push(ty.fmG_Id);
                }
            })
            angular.forEach($scope.coursecount, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })

            angular.forEach($scope.branchcount, function (ty) {
                if (ty.selectedbranch) {
                    AMB_Ids.push(ty.amB_Id);
                }
            })
            angular.forEach($scope.colsemisterlist, function (ty) {
                if (ty.selectedsem) {
                    AMSE_Ids.push(ty.AMSE_Id);
                }
            })

            //angular.foreach($scope.colsemisterlist, function (ty) {
            //    if (ty.selected) {
            //        amse_ids.push(ty.amsE_id);
            //    }
            //})
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
               


                if ($scope.rpttyp == "year") {
                    

                    var data = {
                        "FMG_Id": $scope.fmG_Id,
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        "studenttype": $scope.status,
                        //"customflag": $scope.custom_check,
                        //"groupflag": $scope.group_check,
                        //  "ASMCL_Id": $scope.asmcL_Id,
                        "AMSC_Id": $scope.asmC_Id,
                        // "TRMR_Id": $scope.trmR_Id,
                        //FMG_Ids: FMG_Ids,
                        //FMT_Ids: FMT_Ids,
                        // "term_group": grouporterm,
                        AMCO_Ids: AMCO_Ids,
                        AMB_Ids: AMB_Ids,
                        FMG_Ids: FMG_Ids,
                        AMSE_Ids: AMSE_Ids,
                    }
                }
                else {
                    if ($scope.class == "1") {

                        $scope.asmcL_Id = $scope.fmG_Class;
                    }
                    else {
                        $scope.asmcL_Id = 0;
                        $scope.asmC_Id = 0;
                    }
                    if ($scope.route == "1") {

                        $scope.trmR_Id = $scope.trmR_Id;
                    }
                    else {
                        $scope.trmR_Id = 0;
                    }
                    var data = {

                        "ASMAY_Id": $scope.obj.ASMAY,
                        "radioval": $scope.result,
                        //"studenttype": $scope.status,
                        "duedate": $scope.due,
                        "AMSC_Id": $scope.asmC_Id,
                        AMCO_Ids: AMCO_Ids,
                        AMB_Ids: AMB_Ids,
                        FMG_Ids: FMG_Ids,
                        AMSE_Ids: AMSE_Ids,
                        "active": $scope.active,
                        "deactive": $scope.deactive,
                        "left": $scope.left,
                    }
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                $scope.getTotalstd = function (int) {
                    var total = 0;
                    angular.forEach($scope.students, function (e) {
                        total += e.totalbalance;
                    });
                    return total;
                };

                $scope.getTotalgrp = function (int) {
                    var total = 0;
                    angular.forEach($scope.groups, function (e) {
                        total += e.totalbalance;
                    });
                    return total;
                };
                $scope.getTotalhd = function (int) {
                    var total = 0;
                    angular.forEach($scope.heads, function (e) {
                        total += e.totalbalance;
                    });
                    return total;
                };
                $scope.getTotalcls = function (int) {
                    var total = 0;
                    angular.forEach($scope.classes, function (e) {
                        total += e.totalbalance;
                    });
                    return total;
                };


                apiService.create("CollegeDefaultersReport/radiobtndata", data).
                    then(function (promise) {

                        if (promise.smsemailsettings.length > 0) {
                            $scope.smscontent = promise.smsemailsettings[0].iseS_SMSMessage;
                            $scope.MailSubject = promise.smsemailsettings[0].iseS_MailSubject;
                            $scope.MailHeader = promise.smsemailsettings[0].iseS_MailSubject;
                            $scope.Parameter_email = promise.smsemailsettings[0].iseS_SMSMessage;
                            $scope.MailFooter = promise.smsemailsettings[0].iseS_MailFooter;
                        }
                        $scope.mI_ID = promise.mI_ID;
                        $scope.reportdetails = promise.alldata;
                        $scope.FromDate = new Date();
                        $scope.dueduration = promise.month;

                        if (promise.radioval == "FGW") {
                            if (promise.alldata != null && promise.alldata != "") {
                                $scope.groups = promise.alldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = true;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = true;
                                $scope.tot = $scope.getTotalgrp(promise.alldata);
                                $scope.totcountfirst = promise.alldata.length;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = false;
                            }

                        }
                        else if (promise.radioval == "FHW") {
                            if (promise.alldata != null && promise.alldata != "") {
                                $scope.heads = promise.alldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = true;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = true;
                                $scope.grp = false;
                                $scope.tot = $scope.getTotalhd(promise.alldata);
                                $scope.totcountfirst = promise.alldata.length;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = false;
                            }

                        }
                        else if (promise.radioval == "FCW") {
                            if (promise.alldata != null && promise.alldata != "") {
                                $scope.classes = promise.alldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = true;
                                $scope.std = false;
                                $scope.cls = true;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.tot = $scope.getTotalcls(promise.alldata);
                                $scope.totcountfirst = promise.alldata.length;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = false;
                            }

                        }
                        else if (promise.radioval == "FBW") {
                            if (promise.alldata != null && promise.alldata != "") {
                                $scope.branch = promise.alldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = true;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.bh = true;
                                $scope.tot = $scope.getTotalcls(promise.alldata);
                                $scope.totcountfirst = promise.alldata.length;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = false;
                            }

                        }
                        else if (promise.radioval == "FSW") {
                            if (promise.mI_ID == 5) {
                                $scope.bdate = false;
                            }
                            else {
                                $scope.bdate = true;
                            }


                            if (promise.alldata != null && promise.alldata != "") {

                                $scope.students = promise.alldata;
                                $scope.duedate = promise.date
                                if ($scope.route == "1") {
                                    $scope.routename = promise.alldata[0].TRMR_RouteName;
                                    $scope.Gdate = true;
                                }
                                else {
                                    $scope.Gdate = false;
                                }



                                if ($scope.students.length != 0) {
                                    $scope.Grid_view = true;
                                    $scope.print_flag = true;
                                    $scope.std = true;
                                    $scope.cls = false;
                                    $scope.had = false;
                                    $scope.grp = false;
                                    $scope.tot = $scope.getTotalstd(promise.alldata);
                                    $scope.totcountfirst = promise.alldata.length;
                                }
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = false;
                            }

                        }
                    })
            }
            else {
                $scope.submitted = true;

            }
        };



        $scope.clear_feedef = function () {
            $state.reload();
        }

        $scope.exportToExcel = function () {

            if ($scope.result == "FGW") {
                if ($scope.printdatatablegrp !== null && $scope.printdatatablegrp.length > 0) {
                    var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }

            }
            else if ($scope.result == "FHW") {
                if ($scope.printdatatablehad !== null && $scope.printdatatablehad.length > 0) {
                    var exportHref = Excel.tableToExcel(tablehad, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            else if ($scope.result == "FCW") {
                if ($scope.printdatatablecls !== null && $scope.printdatatablecls.length > 0) {
                    var exportHref = Excel.tableToExcel(tablecls, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            else if ($scope.result == "FSW") {
                //    var table = "tablestd";
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var exportHref = Excel.tableToExcel(tablestd, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            else if ($scope.result == "CW") {
                if ($scope.printdatatablecat !== null && $scope.printdatatablecat.length > 0) {
                    var exportHref = Excel.tableToExcel(tablecat, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }

        }
        $scope.printData = function () {


            if ($scope.result == "FGW") {
                if ($scope.printdatatablegrp !== null && $scope.printdatatablegrp.length > 0) {
                    var innerContents = document.getElementById("printSectionIdgrp").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }
            else if ($scope.result == "FHW") {
                if ($scope.printdatatablehad !== null && $scope.printdatatablehad.length > 0) {
                    var innerContents = document.getElementById("printSectionIdhad").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }
            else if ($scope.result == "FCW") {
                if ($scope.printdatatablecls !== null && $scope.printdatatablecls.length > 0) {
                    var innerContents = document.getElementById("printSectionIdcls").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }
            else if ($scope.result == "FSW") {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var innerContents = document.getElementById("printSectionIdstd").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }
            else if ($scope.result == "CW") {
                if ($scope.printdatatablecat !== null && $scope.printdatatablecat.length > 0) {
                    var innerContents = document.getElementById("printSectionIdcat").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }

        }
    }
})();
