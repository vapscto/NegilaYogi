(function () {
    'use strict';
    angular.module('app').controller('FeeDefaulterReportController', FeeDefaulterReportController)

    FeeDefaulterReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeeDefaulterReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        $scope.tadprint = false;
        $scope.class = false;
        $scope.route = false;
        $scope.Ismailsms = false;

        //by deepak
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        // $scope.obj = {};
        $scope.Format_I = false;
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        //End by 
        // $scope.IsHiddenup = true;
        $scope.printdatatable = [];
        $scope.printdatatablegrp = [];
        $scope.printdatatablehad = [];
        $scope.printdatatablecls = [];
        $scope.itemterm = [];
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        // var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;

        var configsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (configsettings !== null && configsettings.length > 0) {
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
            if (mobilenofull != '0') {
                var otpmobile = mobilenofull.substring(6, 10);
                $scope.mobileno = otpmobile;
            }
            var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
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
            var emailotp = false;
            var mobileotp = false;

        }



        if (configsettings !== null && configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }

        $scope.emailsending = function () {

        }

        $scope.smssending = function () {

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


        $scope.imgname = logopath;

        if (grouporterm == "T") {
            $scope.grouportername = "Term Name"
            $scope.term = true;
            $scope.groupterm = false;
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name"
            $scope.groupterm = true;
            $scope.term = false;
        }

        //Fee Group Check all
        $scope.hrdallcheckfee = function () {
            var toggleStatus = $scope.checkallhrdfee;
            angular.forEach($scope.group, function (itm) {
                itm.fmG_Id_chk = toggleStatus;
            });
        }

        $scope.binddatagrp3 = function () {
            $scope.checkallhrdfee = $scope.group.every(function (role) {
                return role.fmG_Id_chk;
            });
        };
        
        //Custom Group Check All
        $scope.hrdallcheckcustom = function () {
            var toggleStatus1 = $scope.checkallhrdcustom;
            angular.forEach($scope.custom, function (itm) {
                itm.fmgG_Id_chk = toggleStatus1;
            });
        }

        $scope.binddatagrp1 = function () {
            $scope.checkallhrdcustom = $scope.custom.every(function (role) {
                return role.fmgG_Id_chk;
            });
        };    
        //Ended

        $scope.togchkbxC = function () {

            $scope.usercheckC = $scope.groupcount.every(function (role) {
                return role.fmT_Id_chk;
            });
        };

        $scope.all_checkC = function () {
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.groupcount, function (role) {
                role.fmT_Id_chk = toggleStatus;
            });
        };

        $scope.allroute = function () {
            var toggleStatus = $scope.userchecker;
            angular.forEach($scope.installmentcount, function (role) {
                role.select = toggleStatus;
            });
        };

        $scope.togchkbxroute = function () {
            $scope.userchecker = $scope.installmentcount.every(function (role) {
                return role.select;
            });
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
                if ($scope.email == true || $scope.sms == true || $scope.notification == true) {
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
                if ($scope.Balance == true) {
                    angular.forEach($scope.students, function (user) {
                        if (!!user.stdselected) {
                            $scope.albumNameArray.push({
                                StudentName: user.id1, AMST_AdmNo: user.admno, ClassSection: user.ClassName, AMST_FatherName: user.FatherName, AMST_MobileNo: user.MobileNo, totalbalance: user.Total_Balance, AMST_emailId: user.Email
                            });
                        }
                    })
                }
                else {


                    angular.forEach($scope.students, function (user) {
                        if (!!user.stdselected) {
                            $scope.albumNameArray.push(user);
                        }
                    })
                }

                if ($scope.albumNameArray.length > 0) {


                    if ((emailotp != false && ($scope.email == true || $scope.sms == true))) {
                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
                        if (emailidforotp != null || emailidforotp != undefined) {

                            $scope.emailid = emailidforotp.substring(0, 4);
                            $("#myModalotp").modal({ backdrop: false });
                            $('#myModalotp').modal('show');

                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }

                    }
                    else if ((mobileotp != false && ($scope.email == true || $scope.sms == true ))) {

                        if (mobilenofull != '0') {
                            var otpmobile = mobilenofull.substring(6, 10);
                            $scope.mobileno = otpmobile;
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Mobile Number Not Found!!");
                        }


                    } else if ($scope.notification == true) {
                        $scope.studentlistt = $scope.albumNameArray;
                        $('#myModalNotification').modal('show');

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


        $scope.search = "";
        //for total
        $scope.get_total_class_print = function () {
            var totalgrp = 0;
            var totalhad = 0;
            var totalcls = 0;
            var totalstu = 0;

            angular.forEach($scope.printdatatable, function (cls) {
                totalgrp += cls.totalbalance;
            })
            angular.forEach($scope.printdatatablegrp, function (cls) {

                totalhad += cls.totalbalance;

            })
            angular.forEach($scope.printdatatablehad, function (cls) {

                totalcls += cls.totalbalance;

            })
            angular.forEach($scope.printdatatablecls, function (cls) {

                totalstu += cls.totalbalance;

            })

            $scope.totalgrp = totalgrp;
            $scope.totalhad = totalhad;
            $scope.totalcls = totalcls;
            $scope.totalstu = totalstu;
        }

        //end
        $scope.bulkdown = function () {
            if ($scope.bulk == false) {
                $scope.bulk1 = false;
                $scope.exe = false;
                $scope.bulk2 = false;
                $scope.showbutton = false;
                $scope.Grid_view = false;
                $scope.Balance1 = false;
            }
            else if ($scope.bulk == true) {

                $scope.showbutton = false;
                $scope.Grid_view = false;
                $scope.Balance1 = false;
                $scope.exe = false;

            }

        }
        $scope.balance_click = function () {
            if ($scope.Balance == false) {
                $scope.Balance1 = false;
                $scope.bulk2 = false;
                $scope.Grid_view = false;
                $scope.bulk1 = false;

            }
            else if ($scope.Balance == true) {
                $scope.Balance1 = false;
                $scope.bulk2 = false;
                $scope.Grid_view = false;
                $scope.bulk1 = false;

            }
        }

        $scope.toggleAllstd = function () {

            var toggleStatus = $scope.stdall;
            angular.forEach($scope.students, function (itm) {
                itm.stdselected = toggleStatus;
                if ($scope.stdall == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

            var balstd = 0;
            var totfineamount = 0;
            for (var q = 0; q < $scope.students.length; q++) {
                if ($scope.students[q].stdselected == true) {
                    balstd = balstd + $scope.students[q].totalbalance;
                }
            }

            for (var r = 0; r < $scope.students.length; r++) {
                if ($scope.students[r].stdselected == true) {
                    totfineamount = totfineamount + $scope.students[r].FineAmount;
                }
            }

            $scope.selectedbalstd = balstd;
            $scope.selectedbalstdfine = totfineamount;
            $scope.get_total_class_print();
        }
        //123456
        $scope.toggleAllstd = function () {

            var toggleStatus = $scope.stdall;
            angular.forEach($scope.students, function (itm) {
                itm.stdselected = toggleStatus;
                if ($scope.stdall == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

            var balstd = 0;
            var totfineamount = 0;
            for (var q = 0; q < $scope.students.length; q++) {
                if ($scope.students[q].stdselected == true) {
                    balstd = balstd + $scope.students[q].totalbalance;
                }
            }

            for (var r = 0; r < $scope.students.length; r++) {
                if ($scope.students[r].stdselected == true) {
                    totfineamount = totfineamount + $scope.students[r].FineAmount;
                }
            }

            $scope.selectedbalstd = balstd;
            $scope.selectedbalstdfine = totfineamount;
            $scope.get_total_class_print();
        }

        //123456
        $scope.toggleAllstdDDD = function () {

            var toggleStatus = $scope.stdalldd;
            angular.forEach($scope.students, function (itm) {
                itm.stdselected = toggleStatus;
                if ($scope.stdalldd == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

            var balstd = 0;
            var totfineamount = 0;
            for (var q = 0; q < $scope.stdalldd.length; q++) {
                if ($scope.employeeid[q].stdselected == true) {
                    balstd = balstd + $scope.employeeid[q].totalbalance;
                }
            }

            for (var r = 0; r < $scope.stdalldd.length; r++) {
                if ($scope.employeeid[r].stdselected == true) {
                    totfineamount = totfineamount + $scope.employeeid[r].FineAmount;
                }
            }

            $scope.selectedbalstd = balstd;
            $scope.selectedbalstdfine = totfineamount;
            $scope.get_total_class_print();
        }

        //123456
        $scope.IsRequired = function () {

            return !$scope.termlst.some(function (options) {
                return options.trmids;
            });
        }
        $scope.blc123 = function () {
            $scope.bulk = false;
            $scope.showbutton = false;
            $scope.students1 = [];
            $scope.header_list = [];
            $scope.header_list1 = [];
            $scope.Grid_list = false;
        }
        $scope.blk = function () {
            $scope.Balance = false;
            $scope.showbutton = false;
            $scope.students1 = [];
            $scope.header_list = [];
            $scope.header_list1 = [];
        }
        $scope.toggleAllstd_1 = function () {

            var aaa = $scope.stdall_1;
            angular.forEach($scope.students1, function (ee) {
                ee.stdselected = aaa;
            })
            $scope.optionToggledstd1();
        }
        $scope.optionToggledstd1 = function (SelectedStudentRecord, index) {

            $scope.printdatatable1 = [];

            angular.forEach($scope.students1, function (qq) {

                if (qq.stdselected == true) {
                    $scope.printdatatable1.push(qq)
                }

            })
            console.log($scope.printdatatable1);
            $scope.studentlist_new123 = [];

            if ($scope.printdatatable1 != null) {


                $scope.header_list_a = [];
                $scope.student_balance_list_a = [];



                var count = 0;
                $scope.student_balance_list_a = $scope.printdatatable1;

                if ($scope.student_balance_list_a.length > 0) {
                    for (var i = 0; i < $scope.student_balance_list_a.length; i++) {

                        if (i === 0) {
                            angular.forEach($scope.student_balance_list_a[i], function (key, r) {
                                $scope.header_list_a.push({ colmn: r, head: key });
                            });
                        }
                    }
                }
                var ttatt_a = [];
                angular.forEach($scope.header_list_a, function (dd) {
                    if (dd.colmn != 'AMST_Id' && dd.colmn != 'stdselected' && dd.colmn != '$$hashKey') {



                        ttatt_a.push(dd);
                    }

                })
                var ttatt1_a = [];
                angular.forEach($scope.student_balance_list_a, function (dd1) {
                    if (dd1.r != 'AMST_Id' && dd1.r != 'stdselected' && dd1.r != '$$hashKey') {


                        ttatt1_a.push(dd1);
                    }

                })

                $scope.header_list_a = ttatt_a;
                $scope.student_balance_list_a = ttatt1_a;

                $scope.header_list1_a = [];
                angular.forEach($scope.header_list_a, function (rr2) {
                    var cnnt = 0;
                    for (var i = 0; i < $scope.student_balance_list_a.length; i++) {

                        angular.forEach($scope.student_balance_list_a[i], function (key2, r2) {
                            if (r2 == rr2.colmn && rr2.colmn != 'MobileNo' && rr2.colmn != 'FatherName' && rr2.colmn != 'SectionName' && rr2.colmn != 'ClassName' && rr2.colmn != 'StudentName' && rr2.colmn != 'admno' && rr2.colmn != 'AMST_Id' && rr2.colmn != 'Email') {
                                cnnt += parseInt(key2);
                            }



                        });

                    }

                    $scope.header_list1_a.push({ head: rr2.colmn, cntt: cnnt });

                })


            }

            console.log($scope.student_balance_list_a);

            if ($scope.printdatatable1.length > 0) {
                $scope.showbutton = true;
                $scope.showbutton_ex = true;
            }
            else {
                $scope.showbutton = false;
            }

        };
        //123456
        $scope.optionToggledstdDDD = function (SelectedStudentRecord, index) {
            $scope.stdalldd = $scope.employeeid.every(function (itm) { return itm.stdselected; });
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

            var balstd = 0;
            var totfineamount = 0;
            for (var q = 0; q < $scope.employeeid.length; q++) {
                if ($scope.employeeid[q].stdselected == true) {
                    balstd = balstd + $scope.employeeid[q].totalbalance;
                    totfineamount = totfineamount + $scope.employeeid[q].FineAmount;
                }
            }
            $scope.selectedbalstd = balstd;
            $scope.selectedbalstdfine = totfineamount;
            $scope.get_total_class_print();
        };
        //123456
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

            var balstd = 0;
            var totfineamount = 0;
            for (var q = 0; q < $scope.students.length; q++) {
                if ($scope.students[q].stdselected == true) {
                    balstd = balstd + $scope.students[q].totalbalance;
                    totfineamount = totfineamount + $scope.students[q].FineAmount;
                }
            }
            $scope.selectedbalstd = balstd;
            $scope.selectedbalstdfine = totfineamount;
            $scope.get_total_class_print();
        }




        //$scope.optionToggledstd = function (SelectedStudentRecord, index) {
        //    $scope.printdatatable = [];
        //    angular.forEach($scope.students, function (qq) {

        //        if (qq.stdselected == true) {
        //            $scope.printdatatable.push(qq)
        //        }

        //    })
        //}
        //=======
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }




        $scope.toggleAllgrp = function () {

            var toggleStatus = $scope.grpall;
            angular.forEach($scope.groups, function (itm) {
                itm.grpselected = toggleStatus;
                if ($scope.grpall == true) {
                    $scope.printdatatablegrp.push(itm);
                }
                else {
                    $scope.printdatatablegrp.splice(itm);
                }
            });
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            var balgrp = 0;
            for (var q = 0; q < $scope.groups.length; q++) {
                if ($scope.groups[q].grpselected == true) {
                    balgrp = balgrp + $scope.groups[q].totalbalance;
                }
            }
            $scope.selectedbalgrp = balgrp;
            $scope.get_total_class_print();
        }
        $scope.optionToggledgrp = function (SelectedStudentRecord, index) {

            angular.forEach($scope.groups, function (aa) {
                if (aa.grpselected === false) {
                    aa.remarttext = "";
                }
            });


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

            var balgrp = 0;

            for (var q = 0; q < $scope.groups.length; q++) {
                if ($scope.groups[q].grpselected == true) {
                    balgrp = balgrp + $scope.groups[q].totalbalance;

                }
            }
            $scope.selectedbalgrp = balgrp;

            $scope.selectedbalamt = obj.selectedbal

            $scope.get_total_class_print();




        }

        $scope.toggleAllhad = function () {

            var toggleStatus = $scope.hadall;
            angular.forEach($scope.heads, function (itm) {
                itm.hadselected = toggleStatus;
                if ($scope.hadall == true) {
                    $scope.printdatatablehad.push(itm);
                }
                else {
                    $scope.printdatatablehad.splice(itm);
                }
            });
            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

            var balhad = 0;
            for (var q = 0; q < $scope.heads.length; q++) {
                if ($scope.heads[q].hadselected == true) {
                    balhad = balhad + $scope.heads[q].totalbalance;
                }
            }
            $scope.selectedbalhad = balhad;
            $scope.get_total_class_print();
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
            var balhad = 0;
            for (var q = 0; q < $scope.heads.length; q++) {
                if ($scope.heads[q].hadselected == true) {
                    balhad = balhad + $scope.heads[q].totalbalance;
                }
            }
            $scope.selectedbalhad = balhad;
            $scope.get_total_class_print();
        }
        $scope.toggleAllcls = function () {

            var toggleStatus = $scope.clsall;
            angular.forEach($scope.classes, function (itm) {
                itm.clsselected = toggleStatus;
                if ($scope.clsall == true) {
                    $scope.printdatatablecls.push(itm);
                }
                else {
                    $scope.printdatatablecls.splice(itm);
                }
            });
            if ($scope.printdatatablecls.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            var balcls = 0;
            for (var q = 0; q < $scope.classes.length; q++) {
                if ($scope.classes[q].clsselected == true) {
                    balcls = balcls + $scope.classes[q].totalbalance;
                }
            }
            $scope.selectedbalcls = balcls;
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
            var balcls = 0;
            for (var q = 0; q < $scope.classes.length; q++) {
                if ($scope.classes[q].clsselected == true) {
                    balcls = balcls + $scope.classes[q].totalbalance;
                }
            }
            $scope.selectedbalcls = balcls;
            $scope.get_total_class_print();
        }

        // $scope.status = "act";
        $scope.due = "duedate";
        //  var temp_grp_list = [];

        $scope.getterms = function () {
            var data = {
                "reporttype": grouporterm,
            }
            apiService.create("FeeDefaulterReport/getgrpterms", data).
                then(function (promise) {

                    $scope.groupcount = promise.fillmastergroup;
                    $scope.termlst = promise.fillterms;
                    $scope.custom = promise.customlist;

                    if ($scope.custom != null) {
                        $scope.customgroupname = $scope.custom[0].fmg_groupname;
                    }

                    if (grouporterm == "T") {
                        angular.forEach(promise.grouplist, function (tr) {
                            tr.fmG_Id_chk = false;
                        })
                    }
                    else if (grouporterm == "G") {
                        angular.forEach(promise.grouplist, function (tr) {
                            tr.fmG_Id_chk1 = false;
                        })
                    }


                    $scope.group = promise.grouplist;


                    if (grouporterm == "T") {
                        angular.forEach(promise.customlist, function (tr) {
                            tr.fmgG_Id_chk = false;
                        })
                    }
                    else if (grouporterm == "G") {
                        angular.forEach(promise.customlist, function (tr) {
                            tr.fmgG_Id_chk1 = false;
                        })
                    }


                })
        }


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            $scope.IsHiddenup = true;
            var pageid = 1;
            $scope.std = false;
            $scope.grp = false;
            $scope.cls = false;
            $scope.had = false;
            $scope.fmG_Class_Flag = true;
            $scope.fmG_Installment_Flag = true;
            $scope.active = true;
            $scope.showbutton = false;
            $scope.bl = false;
            //$scope.custom_check_flag = true
            //$scope.group_check_flag = true;
            //$scope.custom_check = "0";
            //$scope.group_check = "0";
            //$scope.load_group_check();
            //$scope.load_custom_check();
            $scope.Gdate = false;
            //var data = {
            //    "reporttype": grouporterm,
            //}
            var pageid = 1;

            apiService.getURI("FeeDefaulterReport/getalldetails", pageid).then(function (promise) {
                $scope.bl = false;
                $scope.configsettings = promise.accountdetails;

                if ($scope.configsettings !== null && $scope.configsettings.length > 0) {
                    grouporterm = $scope.configsettings[0].fmC_GroupOrTermFlg;
                    autoreceipt = $scope.configsettings[0].fmC_AutoReceiptFeeGroupFlag;
                    receiptformat = $scope.configsettings[0].fmC_Receipt_Format;
                    mergeinstallment = $scope.configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
                }

                if (promise.getfeedefaultertemplate !== null && promise.getfeedefaultertemplate.length > 0) {
                    $scope.getfeedefaultertemplate = promise.getfeedefaultertemplate
                }

                if (grouporterm == "T") {
                    $scope.grouportername = "Term Name"
                    $scope.term = true;
                    $scope.groupterm = false;
                }
                else if (grouporterm == "G") {
                    $scope.grouportername = "Group Name"
                    $scope.groupterm = true;
                    $scope.term = false;
                }
                $scope.getterms();
                $scope.arrlist6 = promise.adcyear;
                //$scope.groupcount = promise.fillmastergroup;
                //$scope.termlst = promise.fillterms;
                $scope.classcount = promise.fillclass;
                $scope.installmentcount = promise.fillinstallment;
                if ($scope.installmentcount != null && $scope.installmentcount.length > 0) {
                    angular.forEach($scope.installmentcount, function (tr) {
                        tr.select = false;
                    })
                }


                //  temp_grp_list = promise.grouplist;
            });
        }
        $scope.stu_class = false;
        $scope.class_drpdis = function () {

            if ($scope.class == "1") {
                $scope.fmG_Class_Flag = false;
                $scope.stu_class = true;
                $scope.section = true;

            }
            else if ($scope.class == "0") {
                $scope.fmG_Class_Flag = true;
                $scope.stu_class = false;
                $scope.section = false;
            }
        }

        $scope.route_id = function () {

            if ($scope.route == "1") {
                // $scope.trmR_Id = true;
                $scope.busrt = 1;
            }
            else if ($scope.route == "0") {
                //$scope.trmR_Id = false;
                $scope.busrt = 0;
            }
        }

        $scope.fee_instalmt = false;
        $scope.installment_drpdis = function () {

            if ($scope.installment == "1") {
                $scope.fmG_Installment_Flag = false;
                $scope.fee_instalmt = true;
            }
            else if ($scope.installment == "0") {
                $scope.fmG_Installment_Flag = true;
                $scope.fee_instalmt = false;
            }
        }


        //adding section 
        $scope.onselectclass = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.fmG_Class,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            apiService.create("FeeDefaulterReport/getsection", data).
                then(function (promise) {
                    $scope.sectioncount = promise.fillsection;
                    //  $scope.arrlstinst1 = promise.fillinstallment;
                })
        };

        $scope.student_install_wise = function () {


            $scope.std = false;
            $scope.grp = false;
            $scope.cls = false;
            $scope.had = false;
            $scope.Grid_view = false;
            $scope.printdatatable = [];
            $scope.printdatatablegrp = [];
            $scope.printdatatablehad = [];
            $scope.printdatatablecls = [];
            //  $scope.printdatatable = [];
            $scope.stdall = false;
            $scope.stdalldd = false;
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
            if ($scope.result == "FIW") {
                $scope.install_drdd = false;
                $scope.student_btns = false;
                $scope.Ismailsms = false;

            }
            else if ($scope.result == "FSW") {
                $scope.install_drdd = false;
                $scope.student_btns = false;
                $scope.Ismailsms = false;
            }
            else {
                $scope.install_drdd = false;
                $scope.student_btns = false;
                $scope.Ismailsms = false
            }

        };

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? true : false;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clear_feedef = function () {

            $state.reload();
            // $scope.loaddata();
            //if ($scope.rpttyp == "year") {
            //    $scope.rpttyp = "year";
            //    $scope.asmaY_Id = "";
            //    // $scope.asmC_Id = "";

            //}
            //else if ($scope.rpttyp == "date") {
            //    $scope.rpttyp == "date";
            //    $scope.fromDate = null;
            //    //$scope.todate = "";
            //    $scope.due = "duedate";
            //    //  $scope.asmC_Id = " ";
            //}
            //if ($scope.class == "1") {
            //    $scope.fmG_Class = "";
            //    $scope.section = false;
            //}
            //if ($scope.installment == "1") {
            //    $scope.fmG_Installment = "";
            //}

            ////$scope.fmG_Id = "";
            //$scope.class = false;
            //$scope.route = false;
            //$scope.trmids = "";
            //$scope.fmT_Id = "";
            //$scope.asmC_Id = "";
            //$scope.status = "act";
            //$scope.result = "FGW";
            //$scope.Grid_view = false;
            //$scope.print_flag = true;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            //// $scope.termlst = '';
            //$scope.loaddata();

        };

        $scope.Grid_view = false;
        $scope.print_flag = true;
        $scope.submitted = false;
        $scope.rpttyp = "year";
        $scope.adyr = true;
        $scope.result = "FGW";
        //$scope.status = "act";
        //$scope.due = "duedate";
        $scope.ShowReport = function (termlst, asmaY_Id, fromDate, rpttyp, result, due, asmcL_Id, trmR_Id) {
            $scope.Grid_list = false;
            $scope.selectedbalstd = '';
            $scope.stdall = [];
            $scope.stdalldd = [];
            $scope.grpall = [];
            $scope.hadall = [];
            $scope.clsall = [];
            $scope.printdatatable = [];
            $scope.printdatatablegrp = [];
            $scope.printdatatablehad = [];
            $scope.printdatatablecls = [];

            if ($scope.Balance == true) {
                $scope.blc_new = 1;
            }
            else {
                $scope.blc_new = 0;
            }



            $scope.selectedbalstdfine = "";
            $scope.selectedbalstd = "";

            if ($scope.myForm.$valid) {


                if ($scope.result == "FSW") {
                    $scope.Ismailsms = true;
                }
                else {
                    $scope.Ismailsms = false;
                }

                angular.forEach($scope.arrlist6, function (y) {
                    if (y.asmaY_Id == $scope.asmaY_Id) {
                        $scope.acdyr = y.asmaY_Year;
                    }
                })




                $scope.albumNameArraycolumn1 = [];
                angular.forEach($scope.custom, function (custom1) {
                    if (!!custom1.selected) $scope.albumNameArraycolumn1.push({
                        columnID1: custom1.fmgG_Id

                    });
                })

                $scope.albumNameArraycolumn2 = [];
                angular.forEach($scope.group, function (group) {
                    if (!!group.selected) $scope.albumNameArraycolumn2.push({
                        columnID2: group.fmG_Id

                    });
                })

                $scope.albumNameArraycolumn3 = [];
                angular.forEach($scope.groupcount, function (groupcount) {
                    if (!!groupcount.selected) $scope.albumNameArraycolumn3.push({
                        columnID3: groupcount.fmT_Id

                    });
                })




                var FMG_Ids = [];
                var FMT_Ids = [];
                var TRMR_Ids = [];
                if (grouporterm == "T") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                    angular.forEach($scope.groupcount, function (ty) {
                        if (ty.fmT_Id_chk) {
                            FMT_Ids.push(ty.fmT_Id);
                        }
                    })
                }
                else if (grouporterm == "G") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk1) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                }


                angular.forEach($scope.installmentcount, function (ty) {
                    //if (ty.trmR_Id == true) {
                    //    TRMR_Ids.push(ty.trmR_Id);
                    //}
                    if (ty.select == true) {
                        TRMR_Ids.push(ty.trmR_Id);
                    }
                })


                if ($scope.rpttyp == "year") {
                    if ($scope.class == "1") {

                        $scope.asmcL_Id = $scope.fmG_Class;
                    }
                    else {
                        $scope.asmcL_Id = 0;
                        $scope.asmC_Id = 0;
                    }

                    if ($scope.route == "1") {

                        $scope.trmR_Id = $scope.trmR_Id;
                        $scope.busrt = 1;
                    }
                    else {
                        $scope.trmR_Id = 0;
                        $scope.busrt = 0;
                    }

                    var data = {
                        "FMG_Id": $scope.fmG_Id === undefined ? 0 : $scope.fmG_Id,
                        "Balance_1": $scope.blc_new,
                        "FMGG_Id": $scope.fmgG_Id === undefined ? 0 : $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id === undefined ? 0 : $scope.fmT_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        //"studenttype": $scope.status,
                        "yearid": $scope.busrt,
                        "active": $scope.active === true ? 1 : 0,
                        "deactive": $scope.deactive === true ? 1 : 0,
                        "left": $scope.left === true ? 1 : 0,
                        "customflag": $scope.custom_check === undefined ? "" : $scope.custom_check,
                        "groupflag": $scope.group_check === undefined ? "" : $scope.group_check,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "AMSC_Id": $scope.asmC_Id,
                        //"TRMR_Id": $scope.trmR_Id,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        TRMR_Ids: TRMR_Ids,
                        "term_group": grouporterm,
                        "Select_Button": $scope.radtype,
                        "ASMAY_Id": $scope.asmaY_Id,
                    }
                }
                else if ($scope.rpttyp == "date") {
                    if ($scope.class == "1") {

                        $scope.asmcL_Id = $scope.fmG_Class;
                    }
                    else {
                        $scope.asmcL_Id = 0;
                        $scope.asmC_Id = 0;
                    }
                    if ($scope.route == "1") {

                        $scope.trmR_Id = $scope.trmR_Id;
                        $scope.busrt = 1;
                    }
                    else {
                        $scope.trmR_Id = 0;
                        $scope.busrt = 0;
                    }
                    $scope.fromDate = new Date($scope.fromDate).toDateString();
                    $scope.blc = "";


                    var data = {

                        "ASMAY_Id": $scope.asmaY_Id,
                        "Balance_1": $scope.blc_new,
                        "From_Date": $scope.fromDate,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        // "studenttype": $scope.status,
                        "yearid": $scope.busrt,
                        "active": $scope.active,
                        "deactive": $scope.deactive,
                        "left": $scope.left,
                        "duedate": $scope.due,
                        //"customflag": $scope.custom_check,
                        //"groupflag": $scope.group_check,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "AMSC_Id": $scope.asmC_Id,
                        //"TRMR_Id": $scope.trmR_Id,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        "term_group": grouporterm,
                        "Select_Button": $scope.radtype,
                        TRMR_Ids: TRMR_Ids,
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

                $scope.getTotalfinestd = function (int) {
                    var totfine = 0;
                    angular.forEach($scope.students, function (e) {
                        //totfine += e.FineAmount;
                        if (e.FineAmount == undefined) {
                            totfine += 0;
                        }
                        else {
                            totfine += e.FineAmount;
                        }
                    });
                    return totfine;
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
                $scope.Grid_list = false;


                apiService.create("FeeDefaulterReport/radiobtndata", data).
                    then(function (promise) {

                        if ($scope.bulk != true) {

                            if (promise.student_balance_list != null) {
                                $scope.Balance1 = true;
                                $scope.bulk2 = false;
                                $scope.bulk1 = false;
                                $scope.Grid_list = true;
                                $scope.Grid_view = false;
                                $scope.Cumureport = true;
                                $scope.screport = true;
                                $scope.export = true;
                                $scope.header_list = [];


                                var count = 0;
                                $scope.student_balance_list = promise.student_balance_list;

                                if ($scope.student_balance_list.length > 0) {
                                    for (var i = 0; i < $scope.student_balance_list.length; i++) {

                                        if (i === 0) {
                                            angular.forEach($scope.student_balance_list[i], function (key, r) {

                                                $scope.header_list.push({ colmn: r, head: key });



                                            });
                                        }
                                    }
                                }

                                var ttatt = [];
                                angular.forEach($scope.header_list, function (dd) {
                                    if (dd.colmn != 'AMST_Id') {

                                        ttatt.push(dd);
                                    }

                                })

                                var ttatt1 = [];
                                angular.forEach($scope.student_balance_list, function (dd1) {
                                    if (dd1.r != 'AMST_Id') {

                                        ttatt1.push(dd1);
                                    }

                                })
                                $scope.header_list = ttatt;
                                $scope.student_balance_list = ttatt1;

                                $scope.header_list.push({ colmn: 'Total_Balance', head: 0 });

                                for (var i = 0; i < $scope.student_balance_list.length; i++) {
                                    var cnnt1 = 0;
                                    angular.forEach($scope.student_balance_list[i], function (key, r) {

                                        angular.forEach($scope.header_list, function (rr) {

                                            if (r == rr.colmn && rr.colmn != 'MobileNo' && rr.colmn != 'FatherName' && rr.colmn != 'SectionName' && rr.colmn != 'ClassName' && rr.colmn != 'StudentName' && rr.colmn != 'admno' && rr.colmn != 'Email' && rr.colmn != 'AMST_Id') {
                                                cnnt1 += parseInt(key);
                                            }



                                        });

                                    });
                                    $scope.student_balance_list[i].Total_Balance = cnnt1;
                                }


                                $scope.header_list1 = [];
                                angular.forEach($scope.header_list, function (rr) {
                                    var cnnt = 0;
                                    for (var i = 0; i < $scope.student_balance_list.length; i++) {

                                        angular.forEach($scope.student_balance_list[i], function (key, r) {
                                            if (r == rr.colmn && rr.colmn != 'MobileNo' && rr.colmn != 'FatherName' && rr.colmn != 'SectionName' && rr.colmn != 'ClassName' && rr.colmn != 'StudentName' && rr.colmn != 'admno' && rr.colmn != 'Email' && rr.colmn != 'AMST_Id') {
                                                cnnt += parseInt(key);
                                            }



                                        });

                                    }

                                    $scope.header_list1.push({ head: rr.colmn, cntt: cnnt });

                                })



                                $scope.students1 = [];
                                $scope.students1 = $scope.student_balance_list;



                                console.log($scope.student_balance_list);

                                console.log($scope.student_balance_list);
                            }

                            else {
                                if (promise.smsemailsettings !== null && promise.smsemailsettings.length > 0) {
                                    $scope.smscontent = promise.smsemailsettings[0].iseS_SMSMessage;
                                    $scope.MailSubject = promise.smsemailsettings[0].iseS_MailSubject;
                                    $scope.MailHeader = promise.smsemailsettings[0].iseS_MailSubject;
                                    $scope.Parameter_email = promise.smsemailsettings[0].iseS_SMSMessage;
                                    $scope.MailFooter = promise.smsemailsettings[0].iseS_MailFooter;
                                }

                                $scope.mI_ID = promise.mI_ID;
                                $scope.reportdetails = promise.searchstudentDetails;
                                $scope.FromDate = new Date();
                                $scope.dueduration = promise.month;

                                if (promise.radioval == "FGW") {
                                    if (promise.groupalldata != null && promise.groupalldata != "") {
                                        $scope.groups = promise.groupalldata;
                                        $scope.Grid_view = true;
                                        $scope.Grid_list = false;
                                        $scope.print_flag = false;
                                        $scope.std = false;
                                        $scope.cls = false;
                                        $scope.had = false;
                                        $scope.grp = true;
                                        $scope.stdist = false;
                                        $scope.tot = $scope.getTotalgrp(promise.groupalldata);
                                        $scope.totcountfirst = promise.groupalldata.length;
                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                    }

                                }

                                else if (promise.radioval == "FIW") {
                                    if (promise.mI_ID == 5) {
                                        $scope.bdate = false;
                                    }
                                    else {
                                        $scope.bdate = true;
                                    }


                                    if (promise.studentalldata != null && promise.studentalldata != "") {

                                        $scope.students = promise.studentalldata;

                                        //$scope.duedate = promise.date

                                        if ($scope.route == "1") {
                                            //  $scope.routename = promise.studentalldata[0].TRMR_RouteName;

                                            angular.forEach($scope.installmentcount, function (rt) {
                                                if (rt.trmR_Id == $scope.trmR_Id) {
                                                    $scope.routename = rt.trmR_RouteNo + ' : ' + rt.trmR_RouteName;
                                                }
                                            });

                                            $scope.Gdate = true;
                                        }
                                        else {
                                            $scope.Gdate = false;
                                        }

                                        if ($scope.students.length != 0) {
                                            $scope.Grid_view = true;
                                            $scope.print_flag = false;
                                            $scope.std = false;
                                            $scope.cls = false;
                                            $scope.had = false;
                                            $scope.grp = false;
                                            $scope.stdist = true;
                                            $scope.tot = $scope.getTotalstd(promise.studentalldata);
                                            $scope.totcountfirst = promise.studentalldata.length;
                                        }
                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                    }

                                }


                                else if (promise.radioval == "FHW") {
                                    if (promise.headalldata != null && promise.headalldata != "") {
                                        $scope.heads = promise.headalldata;
                                        $scope.Grid_view = true;
                                        $scope.print_flag = false;
                                        $scope.std = false;
                                        $scope.cls = false;
                                        $scope.had = true;
                                        $scope.grp = false;
                                        $scope.stdist = false;
                                        $scope.tot = $scope.getTotalhd(promise.headalldata);
                                        $scope.totfineamount = $scope.getTotalfinestd(promise.studentalldata);
                                        $scope.totcountfirst = promise.headalldata.length;
                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                    }

                                }
                                else if (promise.radioval == "FCW") {
                                    if (promise.classalldata != null && promise.classalldata != "") {
                                        $scope.classes = promise.classalldata;
                                        $scope.Grid_view = true;
                                        $scope.print_flag = false;
                                        $scope.std = false;
                                        $scope.cls = true;
                                        $scope.had = false;
                                        $scope.grp = false;
                                        $scope.stdist = false;
                                        $scope.tot = $scope.getTotalcls(promise.classalldata);
                                        $scope.totcountfirst = promise.classalldata.length;
                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                    }

                                }

                                else if (promise.radioval == "FSW") {
                                    if (promise.mI_ID == 5) {
                                        $scope.bdate = false;
                                    }
                                    else {
                                        $scope.bdate = true;
                                    }


                                    if (promise.studentalldata != null && promise.studentalldata != "") {

                                        $scope.students = promise.studentalldata;
                                        //$scope.duedate = promise.date
                                        if ($scope.asmcL_Id != 0) {
                                            if (promise.feesummlist != null) {
                                                if (promise.feesummlist.length > 0) {
                                                    $scope.duedate = new Date(promise.feesummlist[0].DueDate);
                                                }
                                            }
                                        }
                                        if ($scope.route == "1") {
                                            //  $scope.routename = promise.studentalldata[0].TRMR_RouteName;

                                            angular.forEach($scope.installmentcount, function (rt) {
                                                if (rt.trmR_Id == $scope.trmR_Id) {
                                                    $scope.routename = rt.trmR_RouteNo + ' : ' + rt.trmR_RouteName;
                                                }
                                            });

                                            $scope.Gdate = true;
                                        }
                                        else {
                                            $scope.Gdate = false;
                                        }



                                        if ($scope.students.length != 0) {
                                            $scope.Grid_view = true;
                                            $scope.print_flag = false;
                                            $scope.std = true;
                                            $scope.cls = false;
                                            $scope.had = false;
                                            $scope.grp = false;
                                            $scope.stdist = false;
                                            $scope.tot = $scope.getTotalstd(promise.studentalldata);
                                            $scope.totfineamount = $scope.getTotalfinestd(promise.studentalldata);
                                            $scope.totcountfirst = promise.studentalldata.length;
                                        }
                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                    }

                                }
                                else if (promise.radioval == "FPW") {


                                    if (promise.studentalldata != null && promise.studentalldata != "") {

                                        $scope.students = promise.studentalldata;




                                        if ($scope.students.length != 0) {
                                            $scope.Grid_view = true;
                                            $scope.print_flag = false;
                                            $scope.std = false;
                                            $scope.fpwstd = true;
                                            $scope.cls = false;
                                            $scope.had = false;
                                            $scope.grp = false;
                                            $scope.stdist = false;
                                            $scope.tot = $scope.getTotalstd(promise.studentalldata);
                                            $scope.totfineamount = $scope.getTotalfinestd(promise.studentalldata);
                                            $scope.totcountfirst = promise.studentalldata.length;
                                        }
                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                    }

                                }


                            }

                        }

                        else {

                            if ($scope.bulk == true && $scope.Balance == true) {
                                $scope.bulk1 = true;
                                $scope.bulk2 = false;
                                $scope.Balance1 = false;
                                if (promise.student_balance_list != null) {

                                    $scope.studentlist_new = [];
                                    $scope.studentlist_new = promise.student_balance_list;
                                    $scope.Grid_list = true;
                                    $scope.Grid_view = false;
                                    $scope.Cumureport = true;
                                    $scope.screport = true;
                                    $scope.export = true;
                                    $scope.header_list = [];

                                    var count = 0;
                                    $scope.student_balance_list = promise.student_balance_list;

                                    $scope.student_balance_list_45 = [];
                                    $scope.student_balance_list_45 = $scope.student_balance_list;

                                    if ($scope.student_balance_list.length > 0) {
                                        for (var i = 0; i < $scope.student_balance_list.length; i++) {

                                            if (i === 0) {
                                                angular.forEach($scope.student_balance_list[i], function (key, r) {
                                                    $scope.header_list.push({ colmn: r, head: key });
                                                });
                                            }
                                        }
                                    }
                                    var ttatt = [];
                                    angular.forEach($scope.header_list, function (dd) {
                                        if (dd.colmn != 'AMST_Id') {

                                            ttatt.push(dd);
                                        }

                                    })
                                    var ttatt1 = [];
                                    angular.forEach($scope.student_balance_list, function (dd1) {
                                        if (dd1.r != 'AMST_Id') {

                                            ttatt1.push(dd1);
                                        }

                                    })

                                    $scope.header_list = ttatt;
                                    $scope.student_balance_list = ttatt1;

                                    $scope.header_list.push({ colmn: 'Total_Balance', head: 0 });

                                    for (var i = 0; i < $scope.student_balance_list.length; i++) {
                                        var cnnt1 = 0;
                                        angular.forEach($scope.student_balance_list[i], function (key, r) {

                                            angular.forEach($scope.header_list, function (rr) {

                                                if (r == rr.colmn && rr.colmn != 'MobileNo' && rr.colmn != 'FatherName' && rr.colmn != 'SectionName' && rr.colmn != 'ClassName' && rr.colmn != 'StudentName' && rr.colmn != 'admno' && rr.colmn != 'AMST_Id' && rr.colmn != 'Email') {
                                                    cnnt1 += parseInt(key);
                                                }



                                            });

                                        });
                                        $scope.student_balance_list[i].Total_Balance = cnnt1;
                                    }





                                    $scope.header_list1 = [];
                                    angular.forEach($scope.header_list, function (rr) {
                                        var cnnt = 0;
                                        for (var i = 0; i < $scope.student_balance_list.length; i++) {

                                            angular.forEach($scope.student_balance_list[i], function (key, r) {
                                                if (r == rr.colmn && rr.colmn != 'MobileNo' && rr.colmn != 'FatherName' && rr.colmn != 'SectionName' && rr.colmn != 'ClassName' && rr.colmn != 'StudentName' && rr.colmn != 'admno' && rr.colmn != 'AMST_Id' && rr.colmn != 'Email') {
                                                    cnnt += parseInt(key);
                                                }



                                            });

                                        }

                                        $scope.header_list1.push({ head: rr.colmn, cntt: cnnt });

                                    })

                                    var abc = 0;
                                    angular.forEach($scope.header_list1, function (ee) {
                                        abc += 1;
                                        ee.fld = 'id' + abc;


                                    })

                                    angular.forEach($scope.student_balance_list, function (key, ee) {

                                        angular.forEach(key, function (gg, ee) {

                                            angular.forEach($scope.header_list1, function (dd) {

                                                if (dd.head == ee) {

                                                    if (dd.head != 'MobileNo' && dd.head != 'FatherName' && dd.head != 'SectionName' && dd.head != 'ClassName' && dd.head != 'StudentName' && dd.head != 'admno' && dd.head != 'AMST_Id' && dd.head != 'Email') {


                                                        key[dd.fld] = parseInt(gg);
                                                    }
                                                    else {
                                                        key[dd.fld] = gg;
                                                    }
                                                }



                                            })



                                        })

                                    })

                                    $scope.colarrayall = [];
                                    $scope.tempaggary = [];


                                    $scope.colarrayall.push({
                                        title: "SLNO",
                                        template: "<span class='row-number' ></span>", width: "80px"
                                    })

                                    angular.forEach($scope.header_list1, function (ww) {
                                        if (ww.head != 'MobileNo' && ww.head != 'FatherName' && ww.head != 'SectionName' && ww.head != 'ClassName' && ww.head != 'StudentName' && ww.head != 'admno' && ww.head != 'Email') {
                                            $scope.tempaggary.push({ field: ww.fld, name: ww.fld, aggregate: "sum" });
                                            $scope.colarrayall.push({
                                                title: ww.head, field: ww.fld, width: 100, aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                                groupFooterTemplate: "Sum: #=sum#"
                                            });
                                        }
                                        else {

                                            $scope.colarrayall.push({ title: ww.head, field: ww.fld, width: 100 });
                                        }

                                    })

                                    angular.forEach($scope.student_balance_list, function (rr) {
                                        rr.Discontinued = false;

                                    })

                                    $scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname + ",";
                                    $scope.aaaa = [{
                                        title: $scope.txtdata,
                                        columns: $scope.colarrayall
                                    }]

                                    $(document).ready(function () {
                                        $('#kindogridhhs').empty();
                                        $("#kindogridhhs").kendoGrid({
                                            toolbar: ["excel"],

                                            dataSource: {
                                                data: $scope.student_balance_list,
                                                pageSize: 100,
                                                aggregate: $scope.tempaggary
                                            },
                                            excel: {
                                                fileName: "Student_Fee_Balance_List.xls",
                                                proxyURL: "",
                                                filterable: true,
                                                allPages: true
                                            },
                                            pdf: {
                                                avoidLinks: true,
                                                landscape: true,
                                                repeatHeaders: true,
                                                fileName: "Student_Fee_Balance_List.pdf",
                                                allPages: true
                                            },



                                            sortable: true,
                                            // pageable: true,
                                            pageable: true,
                                            groupable: false,
                                            filterable: true,
                                            columnMenu: true,
                                            reorderable: true,
                                            resizable: true,
                                            selectable: true,
                                            //change: onChange,
                                            columns: $scope.aaaa,
                                            dataBound: function (e) {
                                                var pagenum = this.dataSource.page();
                                                var pageitms = this.dataSource.pageSize();
                                                var rows = this.items();
                                                $(rows).each(function () {
                                                    var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                    var rowLabel = $(this).find(".row-number");
                                                    $(rowLabel).html(index);
                                                });


                                            }
                                        });
                                    });



                                    console.log($scope.student_balance_list)
                                }
                            }
                            else {

                                $scope.students_n = [];
                                $scope.students_n = promise.studentalldata;

                                for (var i = 0; i < $scope.students_n.length; i++) {
                                    var cnnt1 = 0;
                                    angular.forEach($scope.students_n, function (rr) {
                                        if (rr.FineAmount == undefined) {
                                            rr.FineAmount = 0;
                                        }
                                        rr.FinalTotal = rr.totalbalance + rr.FineAmount;

                                    });



                                }
                                $scope.students = $scope.students_n;

                                $scope.bulk2 = true;
                                $scope.std = false;
                                $scope.bulk1 = false;
                                $scope.Balance1 = false;
                                $scope.colarrayall5 = [{
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "50px"
                                },
                                {
                                    name: 'StudentName', field: 'StudentName', title: 'Name', aggregates: ["sum"]
                                }];

                                if ($scope.radtype == 'Student') {
                                    $scope.colarrayall5.push({ name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Admission No', width: "100px" });
                                    $scope.colarrayall5.push({ name: 'ClassSection', field: 'ClassSection', title: 'Class : Section', width: "100px" });
                                    $scope.regno = true;
                                    count = count + 1;
                                } else {
                                    $scope.regno = false;
                                }
                                if ($scope.radtype == 'Staff') {

                                    $scope.colarrayall5.push({ name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Employee Code', width: "100px" });
                                    $scope.colarrayall5.push({ name: 'ClassSection', field: 'ClassSection', title: 'Designation', width: "100px" });
                                    $scope.regno = true;
                                    count = count + 1;
                                } else {
                                    $scope.regno = false;
                                }
                                $scope.colarrayall5.push({ name: 'AMST_FatherName', field: 'AMST_FatherName', title: 'Father Name', aggregates: ["sum"] });
                                $scope.colarrayall5.push({ name: 'AMST_MobileNo', field: 'AMST_MobileNo', title: 'Mobile No', aggregates: ["sum"] });
                                $scope.colarrayall5.push({
                                    name: 'AMST_emailId', field: 'AMST_emailId', title: 'Email Id', footerTemplate: "Total:",
                                    groupFooterTemplate: "Total: "
                                });

                                $scope.colarrayall5.push({
                                    name: 'totalbalance', field: 'totalbalance', title: 'Balance Amount', aggregates: ["sum"], footerTemplate: "#=sum#",
                                    groupFooterTemplate: " #=sum#"
                                });
                                $scope.colarrayall5.push({
                                    name: 'FineAmount', field: 'FineAmount', title: 'Fine Amount', aggregates: ["sum"], footerTemplate: "#=sum#",
                                    groupFooterTemplate: " #=sum#"
                                });
                                $scope.colarrayall5.push({
                                    name: 'FinalTotal', field: 'FinalTotal', title: 'Total Total', aggregates: ["sum"], footerTemplate: "#=sum#",
                                    groupFooterTemplate: " #=sum#"
                                });

                                $scope.student_list5 = promise.studentalldata;


                                angular.forEach($scope.colarrayall5, function (widobj) {
                                    widobj.width = 100;
                                })

                                $scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname + "";

                                console.log($scope.txtdata);
                                $scope.aaaa = [{
                                    title: $scope.txtdata,
                                    columns: $scope.colarrayall5
                                }]
                                console.log($scope.student_list5);
                                var gridall;
                                $(document).ready(function () {
                                    initGridall();
                                });
                                function initGridall() {
                                    $('#grid5').empty();
                                    gridall = $("#grid5").kendoGrid({
                                        toolbar: ["excel", "pdf"],
                                        excel: {
                                            fileName: "Student_fee_Export.xlsx",
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                        },
                                        pdf: {
                                            fileName: "Kendo UI Grid Export.pdf",
                                            allPages: true
                                        },
                                        dataSource: {
                                            //type: "odata",
                                            //transport: {
                                            //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                            //},
                                            data: $scope.student_list5,
                                            pageSize: 10,
                                            aggregate: [
                                                { name: 'paid', field: 'paid', aggregate: "sum" },
                                                { name: 'totalbalance', field: 'totalbalance', aggregate: "sum" },
                                                { name: 'FineAmount', field: 'FineAmount', aggregate: "sum" },
                                                // { name: 'totalbalance', field: 'totalbalance', aggregate: "sum" },
                                                { name: 'FinalTotal', field: 'FinalTotal', aggregate: "sum" },
                                            ]
                                        },
                                        sortable: true,
                                        //pageable: false,
                                        pageable: true,
                                        groupable: false,
                                        filterable: true,
                                        columnMenu: true,
                                        reorderable: true,
                                        resizable: true,
                                        columns: $scope.aaaa,
                                        dataBound: function () {
                                            var pagenum = this.dataSource.page();
                                            var pageitms = this.dataSource.pageSize()
                                            var rows = this.items();
                                            $(rows).each(function () {
                                                var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                var rowLabel = $(this).find(".row-number");
                                                $(rowLabel).html(index);
                                            });
                                        }

                                    }).data("kendoGrid");
                                }
                            }



                            /////////////////////end/////////////////
                        }

                    })

            }
            else {
                $scope.submitted = true;

            }
        };







        // $scope.frmdt = true;
        $scope.frmdt = false;
        $scope.install_drdd = false;
        $scope.student_btns = false;
        //$scope.install_drdd = true;


        $scope.onclickloaddata = function () {

            if ($scope.rpttyp == "year") {

                $scope.frmdt = false;

                $scope.adyr = true;

                //  $scope.clear_feedef();
            }
            else if ($scope.rpttyp == "date") {

                $scope.frmdt = true;

                $scope.adyr = true;

                //  $scope.clear_feedef();

            }
        };


        //$scope.SendMSG = function (Text) {

        //    $scope.albumNameArray = [];
        //    angular.forEach($scope.reportdetails, function (user) {
        //        if (!!user.selected) $scope.albumNameArray.push(user);
        //    })

        //    var data = {
        //        "SmsMailStudentDetails": $scope.albumNameArray,
        //        "SmsMailText": Text
        //    };
        //    apiService.create("FeeDefaulterReport/SendSms", data)
        //    $scope.$apply();
        //    $scope.PostDataResponse = data;
        //    alert('SMS Sent Successfully')
        //    $scope.saved = "SMS Sent Successfully";
        //};

        //$scope.SendMAIL = function (Text) {

        //    $scope.albumNameArray = [];
        //    angular.forEach($scope.reportdetails, function (user) {
        //        if (!!user.selected) $scope.albumNameArray.push(user);
        //    })

        //    var data = {
        //        "SmsMailStudentDetails": $scope.albumNameArray,
        //        "SmsMailText": Text
        //    };
        //    apiService.create("FeeDefaulterReport/SendMail", data)
        //    $scope.$apply();
        //    $scope.PostDataResponse = data;
        //    alert('MAIL Sent Successfully')
        //    $scope.saved = "MAIL Sent Successfully";
        //};



        //$scope.printData = function () {
        //    

        //    var divToPrint = document.getElementById("table2");
        //    var newWin = window.open("");
        //    newWin.document.write(divToPrint.outerHTML);
        //    newWin.print();
        //    newWin.close();
        //}
        //$("#btnExport").click(function (e) {
        //    window.open('data:application/vnd.ms-excel,' + $('#Table').html());
        //    e.preventDefault();
        //});



        $scope.printData = function () {
            if ($scope.result == "FGW") {
                if ($scope.mI_ID == 5) {
                    $scope.bdate = false;
                }
                else {
                    $scope.bdate = true;
                }
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
                    // $state.reload();
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
                    // $state.reload();
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
                    // $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }

            else if ($scope.result == "FSW") {
                var pdss = "";

                if ($scope.route == "1") {
                    $scope.Gdate = true;
                    pdss = 'printSectionIdstd1'
                }
                else {
                    $scope.Gdate = false;
                    pdss = 'printSectionIdstd'
                }
                if ($scope.Balance != true) {
                    if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                        var innerContents = document.getElementById(pdss).innerHTML;
                        var popupWinindow = window.open('');
                        popupWinindow.document.open();
                        popupWinindow.document.write('<html><head>' +
                            '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                            '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                            '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                        popupWinindow.document.close();
                        // $state.reload();
                    }
                    else {
                        swal("Please Select Records to be Printed");
                    }
                }
                else {
                    if ($scope.printdatatable1 !== null && $scope.printdatatable1.length > 0) {
                        var innerContents = document.getElementById('StudentBalance').innerHTML;
                        var popupWinindow = window.open('');
                        popupWinindow.document.open();
                        popupWinindow.document.write('<html><head>' +
                            '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                            '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                            '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                        popupWinindow.document.close();
                        // $state.reload();
                    }
                    else {
                        swal("Please Select Records to be Printed");
                    }

                }



            }

            else if ($scope.result == "FIW") {

                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    //ovetrallprint
                    if ($scope.Format_I === true) {
                        var innerContents = document.getElementById('ovetrallprint').innerHTML;
                        var popupWinindow = window.open('');
                        popupWinindow.document.open();
                        popupWinindow.document.write('<html><head>' +
                            '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                            '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                            '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                        popupWinindow.document.close();
                    }
                    else {
                        var innerContents = document.getElementById('printSectionIdstdinst').innerHTML;
                        var popupWinindow = window.open('');
                        popupWinindow.document.open();
                        popupWinindow.document.write('<html><head>' +
                            '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                            '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                            '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                        popupWinindow.document.close();
                    }


                }
                else {
                    swal("Please Select Records to be Printed");
                }

            }

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
            else if ($scope.result == "FCW") {
                if ($scope.printdatatablecls !== null && $scope.printdatatablecls.length > 0) {
                    var exportHref = Excel.tableToExcel(tablecls, 'sheet name');
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
            else if ($scope.result == "FHW") {
                if ($scope.printdatatablehad !== null && $scope.printdatatablehad.length > 0) {
                    var exportHref = Excel.tableToExcel(tablehad, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            else if ($scope.result == "FIW") {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    if ($scope.Format_I == true) {

                        var exportHref = Excel.tableToExcel(tablestdinsttt, 'sheet name');
                        $timeout(function () { location.href = exportHref; }, 100);
                    }
                    else {
                        var exportHref = Excel.tableToExcel(tablestdinst, 'sheet name');
                        $timeout(function () { location.href = exportHref; }, 100);
                    }

                }
                else {
                    swal("Please select records to be Exported");
                }

            }
            else if ($scope.result == "FSW") {
                if ($scope.Balance == true) {

                    //var exportHref = Excel.tableToExcel(tablestd123, 'sheet name');
                    //$timeout(function () { location.href = exportHref; }, 100);

                    var excelnamemain = "Fee Defaulter Report";
                    var printSectionId = '#tablestd123';
                    $scope.start_Date = new Date();
                    $scope.end_Date = new Date();
                    $scope.from_dateex = $filter('date')($scope.start_Date, 'yyyy-MM-dd');
                    $scope.to_dateex = $filter('date')($scope.end_Date, 'yyyy-MM-dd');
                    var exportHref = Excel.tableToExcel(printSectionId, 'Defaulter Report');
                    $timeout(function () {
                        var a = document.createElement('a');
                        a.href = exportHref;
                        a.download = excelnamemain;
                        document.body.appendChild(a);
                        a.click();
                        a.remove();
                    }, 100);

                }
                else {
                    if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                        //var exportHref = Excel.tableToExcel(tablestd, 'sheet name');
                        //$timeout(function () { location.href = exportHref; }, 200);

                        var excelnamemain = "Fee Defaulter Report";
                        var printSectionId = '#tablestd';
                        $scope.start_Date = new Date();
                        $scope.end_Date = new Date();
                        $scope.from_dateex = $filter('date')($scope.start_Date, 'yyyy-MM-dd');
                        $scope.to_dateex = $filter('date')($scope.end_Date, 'yyyy-MM-dd');
                        var exportHref = Excel.tableToExcel(printSectionId, 'Defaulter Report');
                        $timeout(function () {
                            var a = document.createElement('a');
                            a.href = exportHref;
                            a.download = excelnamemain;
                            document.body.appendChild(a);
                            a.click();
                            a.remove();
                        }, 100);

                    }
                    else {
                        swal("Please select records to be Exported");
                    }

                }
            }

        }




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
                if (grouporterm == "T") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                    angular.forEach($scope.groupcount, function (ty) {
                        if (ty.fmT_Id_chk) {
                            FMT_Ids.push(ty.fmT_Id);
                        }
                    })
                }
                else if (grouporterm == "G") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk1) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                }
                var data = {
                    FMG_Ids: FMG_Ids,
                    FMT_Ids: FMT_Ids,
                    "term_group": grouporterm,
                    "selectedStudentList": $scope.selectedStudentList,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id
                };
                apiService.create("FeeDefaulterReport/SendSms", data).then(function (promise) {
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
        $scope.selectedStudentListforemail = [];
        $scope.send_mail = function () {

            var data ;

            if ($scope.students != null && $scope.students != "") {
                for (var i = 0; i < $scope.students.length; i++) {
                    if ($scope.students[i].stdselected == true) {
                        $scope.selectedStudentListforemail.push($scope.students[i]);
                    }
                }
            }
            $scope.fmtdeatls = [];
            angular.forEach($scope.termlst, function (itm) {
                if (itm.trmids == true) {
                    $scope.fmtdeatls.push(itm);
                }
            });

            if ($scope.selectedStudentListforemail.length == 0) {
                swal("Please select records to send email");
            }
            else {
                var FMG_Ids = [];
                var FMT_Ids = [];
                if (grouporterm == "T") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                    angular.forEach($scope.groupcount, function (ty) {
                        if (ty.fmT_Id_chk) {
                            FMT_Ids.push(ty.fmT_Id);
                        }
                    })
                }
                else if (grouporterm == "G") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk1) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                }

                if ($scope.notification == true) {


                    data = {
   
                        "term_group": grouporterm,
                        "selectedStudentListforemail": $scope.selectedStudentListforemail,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "TemplateName": $scope.obj1.defaulter,
                        "category": $scope.notification

                    };

                } else {

                    data = {
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        "term_group": grouporterm,
                        "TempTerm": $scope.fmtdeatls,
                        "selectedStudentListforemail": $scope.selectedStudentListforemail,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "TemplateName": $scope.obj1.defaulter


                    };

                }

             
                apiService.create("FeeDefaulterReport/Sendmail", data).then(function (promise) {

                    $('#myModalNotification').modal('hide');
                    if (promise.msg == "success") {
                        swal("Email Sent Successfully");
                        //$state.reload();
                    }
                    else if (promise.msg == "notice") {
                       
                        swal("Notice Sent Successfully");
                        
                    }
                    else if (promise.msg == "failed") {
                        swal("Email Not Sent");
                        //  $state.reload();
                    }

                })
            }
        }
        $scope.is_optionrequired_trm_cg = function () {

            return !$scope.custom.some(function (options) {
                return options.fmgG_Id_chk;
            });
        }
        $scope.is_optionrequired_trm_grp = function () {

            return !$scope.group.some(function (options) {
                return options.fmG_Id_chk;
            });
        }
        $scope.is_optionrequired_trm_trm = function () {

            return !$scope.groupcount.some(function (options) {
                return options.fmT_Id_chk;
            });
        }
        $scope.is_optionrequired_groupterm_cg = function () {

            //if ($scope.custom_check == true) {
            return !$scope.custom.some(function (options) {
                return options.fmgG_Id_chk1;
            });
            // }

        }
        $scope.is_optionrequired_groupterm_grp = function () {

            // if ($scope.group_check==true) {
            return !$scope.group.some(function (options) {
                return options.fmG_Id_chk1;
            });
            // }

        }

        $scope.rdoChange = function () {
            $scope.students = [];
            $scope.employee = [];
            $scope.templist = [];
        };
        $scope.get_groups = function () {
            var FMGG_Ids = [];
            if (grouporterm == "T") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk) {
                        FMGG_Ids.push(ty.fmgG_Id);
                    }
                })
            }
            else if (grouporterm == "G") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk1) {
                        FMGG_Ids.push(ty.fmgG_Id);
                    }
                })
            }

            if (FMGG_Ids.length > 0) {
                var data = {
                    "reporttype": grouporterm,
                    FMGG_Ids: FMGG_Ids
                }



                apiService.create("FeeDefaulterReport/get_groups", data).
                    then(function (promise) {

                        //$scope.groupcount = promise.fillmastergroup;
                        //$scope.custom = promise.customlist;
                        if (grouporterm == "T") {
                            angular.forEach(promise.grouplist, function (tr) {
                                tr.fmG_Id_chk = true;
                            })
                        }
                        else if (grouporterm == "G") {
                            angular.forEach(promise.grouplist, function (tr) {
                                tr.fmG_Id_chk1 = true;
                            })
                        }
                        $scope.group = promise.grouplist;
                    });
            }
            else if (FMGG_Ids.length == 0) {
                //$scope.group = temp_grp_list;
                $scope.group = [];
            }


        }

        $scope.I_Formt_I = function () {
            //123456
            //123456
            $scope.employee = [];
            $scope.employeeid = [];
            if ($scope.Format_I === true) {
                $scope.employee = $scope.students[0].StudentName;
                angular.forEach($scope.students, function (dev) {
                    if ($scope.employeeid.length === 0) {
                        $scope.employeeid.push({ AMST_Id: dev.AMST_Id, StudentName: dev.StudentName, AMST_AdmNo: dev.AMST_AdmNo, ClassSection: dev.ClassSection, HRMDES_DesignationName: dev.HRMDES_DesignationName, AMST_FatherName: dev.AMST_FatherName, AMST_MobileNo: dev.AMST_MobileNo, AMST_emailId: dev.AMST_emailId, FMT_Name: dev.FMT_Name, totalbalance: dev.totalbalance });

                    }
                    else if ($scope.employeeid.length > 0) {
                        var intcount = 0;
                        angular.forEach($scope.employeeid, function (emp) {
                            if (emp.AMST_Id === dev.AMST_Id) {
                                intcount += 1;
                            }
                        });
                        if (intcount === 0) {
                            $scope.employeeid.push({ AMST_Id: dev.AMST_Id, StudentName: dev.StudentName, AMST_AdmNo: dev.AMST_AdmNo, ClassSection: dev.ClassSection, HRMDES_DesignationName: dev.HRMDES_DesignationName, AMST_FatherName: dev.AMST_FatherName, AMST_MobileNo: dev.AMST_MobileNo, AMST_emailId: dev.AMST_emailId, FMT_Name: dev.FMT_Name, totalbalance: dev.totalbalance });
                        }
                    }
                });

                console.log($scope.employeeid);
                angular.forEach($scope.employeeid, function (ddd) {
                    $scope.templist = [];
                    var studentwisetotal = 0;
                    angular.forEach($scope.students, function (dd) {
                        if (dd.AMST_Id === ddd.AMST_Id) {
                            studentwisetotal += dd.totalbalance;
                            $scope.templist.push(dd);
                        }
                    });


                    ddd.totalbalance = studentwisetotal;
                    ddd.totaltermsum = studentwisetotal;
                    ddd.studentdetailstwo = $scope.templist;
                });

                console.log($scope.employeeid);

                //123456
                //123456
            }

        };

    }
})();
