(function () {
    'use strict';
    angular.module('app').controller('GeneralSendSMSController', GeneralSendSMSController)

    GeneralSendSMSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', '$http', 'Flash', 'apiService', '$stateParams', '$filter', '$q', 'superCache', '$sce', '$window']
    function GeneralSendSMSController($rootScope, $scope, $state, $location, dashboardService, $http, Flash, apiService, $stateParams, $filter, $q, superCache, $sce, $window) {

        var vm = this;
        $scope.obj1 = {}
        vm.gridOptions = {};
        $scope.reg = {};
        $scope.searchValue = '';
        $scope.stsearch = '';
        $scope.sortReverse = true;
        $scope.snd_email = false;
        $scope.snd_sms = false;
        $scope.stfsnd_email = false;
        $scope.stfsnd_sms = false;

        $scope.whatsapp_flag = false;
        $scope.sms_flag = true;
        $scope.whatsappfile = "";
        $scope.errormessage = "";

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
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var configsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));



        $scope.mobilesstd = [{ id: 'mobilestd1' }];
        //  $scope.selmobsstd = [{ id: 'selmobilestd1' }];
        $scope.addNewMobile1std = function () {

            var newItemNostd = $scope.mobilesstd.length + 1;
            $scope.mobilesstd.push({ 'id': 'mobilestd' + newItemNostd });
            //if (newItemNostd <= 5) {
            //    $scope.mobilesstd.push({ 'id': 'mobilestd' + newItemNostd });
            //}


            //if (newItemNostd == 4) {
            //    $scope.myForm1.$setPristine();
            //}

        };

        $scope.removeNewMobile1std = function (index, curval1std) {
            var newItemNostd2 = $scope.mobilesstd.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd = $scope.mobilesstd.splice(index, 1);
            }
        };

        $scope.removeNewEmail1std = function (index, id) {
            var newItemNostd = $scope.emailsstd.length - 1;
            if (newItemNostd !== 0) {
                $scope.delmsrd = $scope.emailsstd.splice(index, 1);
            }
        }
        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };
        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;

            $scope.emailsstd.push({ 'id': 'emailsstd' + newItemNostd2 });

        };

        $scope.SelectedFileForUploadzdfemm = [];
        $scope.selectFileforUploademail = function (input) {

            $rootScope.selr = $scope.radioval;
            $scope.emailsstd = [];
            $scope.emailsstd = [{ id: 'emailsstd1' }];

            $scope.SelectedFileForUploadzdfemm = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {

                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;

                }
                else {
                    //swal("Please Upload the .xlsx file");
                    //return;
                    $scope.filename = input.files[0].name;
                }
            }
        };


        /// Bulk upload Ivrs

        $scope.ivrsmobile = [{ id: 'mobileivrs1' }];
        $scope.addNewMobileivrs = function () {
            var newItemNostd = $scope.ivrsmobile.length + 1;
            $scope.ivrsmobile.push({ 'id': 'mobileivrs' + newItemNostd });
        };
        $scope.removemobileivrs = function (index, curval1std) {
            var newItemNostd2 = $scope.ivrsmobile.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd = $scope.ivrsmobile.splice(index, 1);
            }
        };

        $scope.selectFileforUploadIvrs = [];
        $scope.selectFileforUploadIvrsf = function (input) {
            $rootScope.ivrscheck = true;
            $scope.ivrsmobile = [];
            $scope.ivrsmobile = [{ id: 'mobileivrs1' }];
            $scope.selectFileforUploadIvrs = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;
                    // alert($scope.filename);
                }
                else {
                    //swal("Please Upload the .xlsx file");
                    //return;
                    $scope.filename = input.files[0].name;
                }
            }
        };

        $scope.clearivrsmodal = function () {
            $scope.ivrsmobile = [];
            $scope.ivrsmobile = [{ id: 'mobileivrs1' }];
            $scope.addNewMobileivrs = function () {
                var newItemNostd = $scope.ivrsmobile.length + 1;
                $scope.ivrsmobile.push({ 'id': 'mobileivrs' + newItemNostd });
            };
            $scope.removemobileivrs = function (index, curval1std) {
                var newItemNostd2 = $scope.ivrsmobile.length - 1;
                if (newItemNostd2 !== 0) {
                    $scope.delmsrd = $scope.ivrsmobile.splice(index, 1);
                }
            };
            $scope.searchValue = "";
            $scope.ivrsmsg = "";
        };
        //Bulk Upload



        $scope.obj = {}



        $scope.SelectedFileForUploadzdf = [];
        $scope.selectFileforUploadzd = function (input) {
            $rootScope.selr = $scope.radioval;
            $scope.mobilesstd = [];
            $scope.mobilesstd = [{ id: 'mobilestd1' }];

            $scope.SelectedFileForUploadzdf = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {

                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;

                }
                else {
                    //swal("Please Upload the .xlsx file");
                    //return;
                    $scope.filename = input.files[0].name;
                }
            }

        }


        $rootScope.validateForm = function (sss, flg) {

            if ($rootScope.ivrscheck === true) {
                $scope.ivrsmobile = sss;
            } else {
                if (flg == 'S') {
                    $scope.mobilesstd = sss;
                }
                else {
                    $scope.emailsstd = sss;
                }
            }
        };

        if (configsettings !== null && configsettings.length > 0) {
            var emailotp = configsettings[0].ivrmgC_emailValOTPFlag;
            var mobileotp = configsettings[0].ivrmgC_MobileValOTPFlag;

            $scope.emailotp = configsettings[0].ivrmgC_emailValOTPFlag;
            $scope.mobileotp = configsettings[0].ivrmgC_MobileValOTPFlag;
            //hard code
            //emailotp = 1;
            //mobileotp = 1;
            //$scope.emailotp =0;
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
                };
                if ($scope.count > 0) {
                    apiService.create("General_SendSMS/Getexam/", data).
                        then(function (promise) {

                            $scope.exmstdlist = promise.exmstdlist;

                        });
                }
            }
            //else if ($scope.selradioval == 'StdTemplate') {
            //    $scope.GetStudentDetails();
            //}
        };

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



        $scope.resendotp = function () {
            $('#myModalswal').modal('show');
            //  $('#myModalotp').modal('show');
        };

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
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("Login/ForgotOTPForEmailval", mobno).then(function (promise) {
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
            });

        };


        $scope.submitted = true;
        $scope.email = true;
        $scope.sms = true;
        $scope.mbarray = [];
        $scope.ISES_Id = '';
        $scope.saveUseronce = function () {

            $scope.errormessage = "";
            $scope.mbarray = [];
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
                if ($scope.exm_radioval == 'mark' && $scope.selradioval == 'exam') {
                    $scope.examdd = 'examddd';
                } else if ($scope.exm_radioval == 'mark' && $scope.selradioval == 'grade') {
                    $scope.examdd = 'gradeddd';
                } else if ($scope.selradioval == 'attendance') {
                    $scope.examdd = 'attendanceddd';
                } else {
                    $scope.examdd = 'ggddd';
                }


                $scope.albumNameArray = [];
                angular.forEach($scope.studentList, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                });
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
                        $scope.stdmsg = $scope.stdmsg;
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

            if ($scope.radioval == 'Gemail') {
                if ($scope.myFormr.$valid) {

                    $scope.mbarray = [];
                    angular.forEach($scope.emailsstd, function (xx) {
                        if (xx.hrmeM_EmailId != undefined && xx.hrmeM_EmailId != '' && xx.hrmeM_EmailId != null && xx.hrmeM_EmailId != 0) {
                            $scope.mbarray.push(xx);
                        }
                    });

                    if ($scope.mbarray.length > 0) {
                        if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
                            $("#myModalotp").modal({ backdrop: false });
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
                        swal('Enter Email')
                    }


                }
                else {
                    $scope.submitted = true;
                }
            }

            if ($scope.radioval == 'General') {

                if ($scope.myForm.$valid) {

                    if ($scope.sms_flag === false && $scope.whatsapp_flag === false) {
                        swal("Select Atleast One Message Type");
                        $scope.errormessage = "Select Atleast One Message Type";
                    } else {
                        $scope.mbarray = [];
                        angular.forEach($scope.mobilesstd, function (xx) {
                            if (xx.hrmemnO_MobileNo != undefined && xx.hrmemnO_MobileNo != '' && xx.hrmemnO_MobileNo != null && xx.hrmemnO_MobileNo != 0) {
                                $scope.mbarray.push(xx);
                            }
                        });

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
                        else
                            if ((mobileotp != 0 && ($scope.email == true || $scope.sms == true))) {

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
                }
                else {
                    $scope.submitted = true;
                }
            }

            if ($scope.radioval == 'Staff') {

                $scope.albumNameArray = [];

                angular.forEach($scope.employeelst, function (user) {
                    if (user.selected === true) {
                        $scope.albumNameArray.push(user);
                    }
                });
                console.log($scope.albumNameArray);

                if ($scope.albumNameArray.length > 0) {

                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {

                        $("#myModalotp").modal({ backdrop: false });

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
                        $scope.clslst21 = $scope.albumNameArray;
                        console.log($scope.clslst2);
                        $('#myModalswal').modal('show');
                        $("#myModalswal").modal({ backdrop: false });
                    }
                }
                else {

                    swal('Kindly select atleast one Employee..!');
                    return;
                }
            }
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.changeradio = function () {

            $scope.currentPage = 1;
            $scope.currentPage3 = 1;
            $scope.currentPage4 = 1;
            $scope.itemsPerPage = 10;
            $scope.itemsPerPage3 = 10;
            $scope.itemsPerPage4 = 10;
            $scope.radiovalnewregular = "Regular";
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
                apiService.create("General_SendSMS/Getdetails/", data).
                    then(function (promise) {

                        $scope.templatelist = promise.templatelist;
                        $scope.fillacademiyear = promise.yearlist;
                        $scope.currentYear = promise.currentYear;
                        for (var i = 0; i < $scope.fillacademiyear.length; i++) {
                            if ($scope.currentYear[0].asmaY_Id == $scope.fillacademiyear[i].asmaY_Id) {
                                $scope.ASMAY_Id = $scope.currentYear[0].asmaY_Id;
                            }
                            $scope.studentroutelist = promise.studentroutelist;
                            $scope.routelist = promise.routelist;
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
                apiService.getURI("General_SendSMS/Getdepartment", id).
                    then(function (promise) {
                        $scope.templatelist = promise.templatelist;
                        $scope.departmentdropdown = promise.departmentdropdown;
                        if (promise.departmentdropdown != null) {
                            $scope.deptcheck = true;

                            $scope.all_checkdep();
                        }
                    });
            }
        };

        $scope.studentlistdetails = function (TRMR_Id) {

            $scope.studentList = [];
            angular.forEach($scope.studentroutelist, function (itm) {
                if (itm.trmR_Id == TRMR_Id) {
                    $scope.studentList.push({ studentName: itm.studentName, amsT_AdmNo: itm.amsT_AdmNo, amsT_MobileNo: itm.amsT_MobileNo, amsT_emailId: itm.amsT_emailId });
                }
            });

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
                    "multipledep": groupidss,
                };
                apiService.create("General_SendSMS/get_designation", data).
                    then(function (promise) {

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

            $scope.get_employeenew();
        };

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
                };
                apiService.create("General_SendSMS/get_employee", data).
                    then(function (promise) {

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

        $scope.GetStudentDetails = function () {
            $scope.selradioval = '';
            $scope.emE_Id = '';
            $scope.all = false;
            $scope.exmstdlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "neworregular": $scope.radiovalnewregular
            };
            apiService.create("General_SendSMS/GetStudentDetails/", data).
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
        };

        $scope.exname = '';
        $scope.stumarkdetails = [];
        $scope.at_details = [];
        $scope.searchstddetails = function () {
            $scope.studentList = [];

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

                    if ($scope.attend_radioval == 'between') {
                        $scope.curnt_date = new Date();
                    } else {
                        $scope.frm_date = new Date();
                        $scope.to_date = new Date();
                    }

                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "selradioval": $scope.selradioval,
                        "attend_radioval": $scope.attend_radioval,
                        "fr_date": new Date($scope.frm_date).toDateString(),
                        "to_date": new Date($scope.to_date).toDateString(),
                        "crnt_date": new Date($scope.curnt_date).toDateString(),
                    };
                }

                apiService.create("General_SendSMS/searchstddetails/", data).
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
                                    $scope.temparraynew = [];
                                    if ($scope.exm_radioval === 'grade') {
                                        angular.forEach($scope.studentList, function (grad) {
                                            if (grad.gradeDetails !== ",") {
                                                $scope.temparraynew.push(grad);
                                            }
                                        });
                                    }
                                    if ($scope.exm_radioval === 'mark') {
                                        angular.forEach($scope.studentList, function (grad) {
                                            if (grad.marksDetails !== ",") {
                                                $scope.temparraynew.push(grad);
                                            }
                                        });
                                    }
                                    $scope.studentList = $scope.temparraynew;
                                    console.log($scope.studentList);
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

                                        lb = 'Student Attendence From' + ' ' + ' ' + fromdate1 + ' ' + 'To' + ' ' + ' ' + todate1 + ' ' + ' ' + '-' + ' ';
                                    }
                                    if ($scope.attend_radioval == 'current') {
                                        var curdate = $filter('date')($scope.curnt_date, 'dd-MM-yyyy');
                                        // $scope.curnt_date = $filter('date')($scope.curnt_date, 'dd-MM-yyyy');
                                        lb = 'Student Attendence On' + ' ' + ' ' + curdate + ' ' + '-' + ' ';
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
                            swal("No records found for selected academicYear,class and section");
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



        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };
        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadsoundtrack", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    //$('#').attr('src', data.cfilepath);
                    var img = data.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }




        $scope.athflag = false;
        $scope.SendDatagmail = function () {

            if ($scope.myFormr.$valid) {
                $('#myModalswal').modal('hide');
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send Email ..!?",
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
                                //"Mobno": $scope.mobno,
                                emaillist: $scope.mbarray,
                                "esubject": $scope.esubject,
                                "mes": $scope.emes,
                                "footer": $scope.Footer,
                                "Header": $scope.emlhead,
                                "fhead": $scope.FHEAD,
                                "radiotype": $scope.radioval,
                                "atchflag": $scope.athflag,
                                filelist: $scope.materaldocuupload
                            };

                            apiService.create("General_SendSMS/savedetail", Send).then(function (promise) {
                                if (promise.smsStatus == 'sent') {
                                    swal("SMS Sent Successfully.");
                                    $state.reload();
                                }
                                else {
                                    swal("Email Sending Cancelled.");
                                }
                            });
                        }
                        else {
                            //dd
                        }
                    });

            }
            else {
                $scope.submitted = true;
            }
        };
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
                                //"Mobno": $scope.mobno,
                                mobilenolist: $scope.mbarray,
                                "mes": $scope.mes,
                                "radiotype": $scope.radioval,
                                "sms_flag": $scope.sms_flag,
                                "whatsapp_flag": $scope.whatsapp_flag,
                                "fileattachementforwhatsapp": $scope.fileattachementforwhatsapp,
                                "whatsapp_filetype": $scope.whatsapp_filetype
                            };

                            apiService.create("General_SendSMS/savedetail", Send).then(function (promise) {
                                if (promise.smsStatus == 'sent') {
                                    swal("SMS Sent Successfully.");
                                    $state.reload();
                                }
                                else {
                                    swal("SMS Sending Cancelled.");
                                }
                            });
                        }
                        else {
                            //dd
                        }
                    });

            }
            else {
                $scope.submitted = true;
            }
        };



        $scope.SendMSGtempl = function (stdmsg) {

            $('#myModalswal').modal('hide');
            $scope.printstudents = [];
            angular.forEach($scope.studentList, function (user) {
                if (!!user.selected) $scope.printstudents.push(user);
            });

            if ($scope.printstudents.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send SMS/EMAIL?",
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
                                "SmsMailText": stdmsg,
                                "radiotype": $scope.radioval,
                                "exm_radioval": $scope.exm_radioval,
                                "selradioval": $scope.selradioval,
                                "snd_email": $scope.snd_email,
                                "snd_sms": $scope.snd_sms,
                                "exmname": $scope.exname,
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "ASMCL_Id": $scope.ASMCL_Id,
                                "ASMS_Id": $scope.asmS_Id,
                                "ISES_Id": $scope.ISES_Id,
                            };
                            apiService.create("General_SendSMS/savedetail", data).then(function (promise) {
                                if (promise.emailStatus === 'sent') {

                                    if ($scope.snd_email === true && $scope.snd_sms === false) {
                                        swal("Email Sent Successfully.");
                                    } else if ($scope.snd_email === false && $scope.snd_sms === true) {
                                        swal("SMS Sent Successfully.");
                                    } else {
                                        swal("SMS And Mail Sent Successfully.");
                                    }
                                    $state.reload();
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




        $scope.SendMSG = function (stdmsg) {


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

                                $scope.curnt_date = new Date();
                                $scope.frm_date = new Date();
                                $scope.to_date = new Date();
                            }

                            else if ($scope.selradioval == 'attendance') {
                                $scope.emE_Id = 0;
                                if ($scope.attend_radioval == 'between') {
                                    $scope.curnt_date = new Date();
                                } else {
                                    $scope.frm_date = new Date();
                                    $scope.to_date = new Date();
                                }
                            }

                            else {
                                $scope.frm_date = new Date();
                                $scope.to_date = new Date();
                                $scope.curnt_date = new Date();
                                $scope.emE_Id = 0;
                            }
                            var data = {
                                studentlistdto: $scope.printstudents,
                                "SmsMailText": stdmsg,
                                "radiotype": $scope.radioval,
                                "exm_radioval": $scope.exm_radioval,
                                "selradioval": $scope.selradioval,
                                "snd_email": $scope.snd_email,
                                "snd_sms": $scope.snd_sms,
                                "exmname": $scope.exname,
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "ASMCL_Id": $scope.ASMCL_Id,
                                "ASMS_Id": $scope.asmS_Id,
                                "EME_Id": $scope.emE_Id,
                                "fr_date": new Date($scope.frm_date).toDateString(),
                                "to_date": new Date($scope.to_date).toDateString(),
                                "crnt_date": new Date($scope.curnt_date).toDateString(),
                                "attend_radioval": $scope.attend_radioval
                            };
                            apiService.create("General_SendSMS/savedetail", data).then(function (promise) {
                                if (promise.emailStatus === 'sent') {

                                    if ($scope.snd_email === true && $scope.snd_sms === false) {
                                        swal("Email Sent Successfully.");
                                    } else if ($scope.snd_email === false && $scope.snd_sms === true) {
                                        swal("SMS Sent Successfully.");
                                    } else {
                                        swal("SMS And Mail Sent Successfully.");
                                    }

                                }
                                else {
                                    swal("SMS And Mail Sent Successfully.");
                                }
                                $state.reload();
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


        $scope.SendStaffData = function () {


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
                                "stfsnd_sms": $scope.stfsnd_sms
                            };
                            apiService.create("General_SendSMS/savedetail", data)
                                .then(function (promise) {
                                    if (promise.smsStatus === 'sent') {

                                        if ($scope.stfsnd_email === true && $scope.stfsnd_sms === false) {
                                            swal("Email Sent Successfully.");
                                        } else if ($scope.stfsnd_email === false && $scope.stfsnd_sms === true) {
                                            swal("SMS Sent Successfully.");
                                        } else {
                                            swal("SMS And Email Sent Successfully.");
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

        $scope.onSelectclass = function (classId) {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id >= 0 && $scope.asmS_Id >= 0) {
                $scope.GetStudentDetails();
            }
        };
        $scope.onSelectyear = function () {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id >= 0 && $scope.asmS_Id >= 0) {
                $scope.GetStudentDetails();
            }
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

            //$scope.all = $scope.employeelst.every(function (itm)
            //{ return itm.selected; });
            //if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
            //    $scope.printstudents.push(SelectedStudentRecord);
            //}
            //else {
            //    $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            //}
        };

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
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //$scope.interacted1 = function (field1) {
        //    return $scope.submitted1;
        //};
        //$scope.interacted2 = function (field2) {
        //    return $scope.submitted2;
        //};


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


        //IVRS Code written by surya  
        $scope.objivr = {};
        $scope.IVRS_CHANGE_TYPES = function () {
            $scope.datagridviewIVR = true;
            $scope.ClearInellegence();
        };


        $scope.dataArray = [];
        $scope.BindData_ivr = function () {
            apiService.getDATA("IVRSOBD/getalldetails").
                then(function (promise) {
                    $scope.ivr_fillacademiyear = promise.acs_lst;
                    $scope.ivr_classlist = promise.clas_list;
                    $scope.ivr_sectionlist = promise.sect_list;
                    $scope.mi_id = promise.mI_ID;
                    $scope.schoolname = promise.schoolname;
                    $scope.datagridviewIVR = true;
                    $scope.gridOptions.data = promise.maindata;
                });

            $scope.ClearInellegence();

        };

        $scope.maketestcall_Kaleyra1 = function () {

            var data = {
                apikey: 'Afb8af1f8c1ee8fc9af6fff35fce864d2'
            };

            var config = {
                params: data,
                headers: { 'Accept': 'application/json' }
            };

            angular.forEach($scope.ivrsmobile, function (item) {
                var KaleyraURL = "https://api-voice.kaleyra.com/v1/?api_key=Afb8af1f8c1ee8fc9af6fff35fce864d2&method=voice.call&format=json&numbers=" + item.ivrS_MobileNo + "&play=ivr:152086";

                $http.get(KaleyraURL, config)
                    .success(function (data, message, status) {
                        var sttsus = data + message + Content;
                    })
            })
        }

        $scope.maketestcall_Kaleyra = function () {

            var data = {
                selected_list: $scope.ivrsmobile,
                "ivrid": "152086"
            }
            apiService.create("IVRSOBD/initiatecalls", data).
                then(function (promise) {
                    if (promise.returnMsg == 'sucess') {
                        swal("Calls sent Sucessfully")
                    }
                    else if (promise.returnMsg == 'Failure') {
                        swal("Kindly contact administrator")
                    }
                    $state.reload();
                })
        }


        $scope.maketestcall_mobiglitz = function () {

            var data = {
                selected_list: $scope.ivrsmobile,
                "ivrid": "263847"
            }
            apiService.create("IVRSOBD/initiatecallsmobiglitz", data).
                then(function (promise) {
                    if (promise.returnMsg == 'sucess') {
                        swal("Calls sent Sucessfully")
                    }
                    else if (promise.returnMsg == 'Failure') {
                        swal("Kindly contact administrator")
                    }
                    $state.reload();
                })
        }


        $scope.maketestcall = function (objivr) {
            var mobi = "";
            if ($scope.rndtype === '1000033778') {
                angular.forEach($scope.ivrsmobile, function (item) {
                    if (item.ivrS_MobileNo !== null && item.ivrS_MobileNo !== "") {
                        if (mobi === "") {
                            mobi = "+91" + item.ivrS_MobileNo + "," + $scope.mi_id + "," + $scope.reg.ivrsmsg +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                        else {
                            mobi += ";+91" + item.ivrS_MobileNo + "," + $scope.mi_id + "," + $scope.reg.ivrsmsg +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                    }
                });
            }
            else if ($scope.rndtype === '1000033928') {
                angular.forEach($scope.ivrsmobile, function (item) {
                    if (item.ivrS_MobileNo !== null && item.ivrS_MobileNo !== "") {
                        if (mobi === "") {
                            mobi = "+91" + item.ivrS_MobileNo + "," + $scope.mi_id + "," + $scope.objivr.urlimage +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                        else {
                            mobi += ";+91" + item.ivrS_MobileNo + "," + $scope.mi_id + "," + $scope.objivr.urlimage +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                    }
                });
            }
            var data = {
                "phonebookname": "phonebook",
                "numbers": mobi
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json charset=UTF-8',
                    'x-api-key': 'lF4vZUSwA8Jab0ABWsITtxwM1ZwL6h2jZDdCTX30',
                    'Authorization': '964c52d4-016a-4d31-9b0a-86daaf43bbc5',
                }
            };
            $http.post('https://kpi.knowlarity.com/Basic/v1/account/contacts/phonebook', data, config)
                .success(function (data, status, config) {
                    $scope.getMessage(data.id);
                })
                .error(function (data, status, config) {
                    $scope.PostDataResponse = "Data: " + data;
                });
            $('#myivrstestpop').modal('hide');
        };

        $scope.getMessage = function (id) {
            setTimeout(function () {
                $scope.$apply(function () {
                    //wrapped this within $apply
                    $scope.scheduleCall(id);
                });
            }, 2000);
        };

        $scope.scheduleCall = function (ph_id) {
            if ($scope.ph_id !== "") {
                var date = new Date();
                var year = date.getFullYear();
                var month = date.getMonth() + 1;
                var day = date.getDate();
                if (month.toString().length === 1) {
                    month = "0" + month;
                }
                if (day.toString().length === 1) {
                    day = "0" + day;
                }
                var value = year + '-' + month + '-' + day;
                var hrs_get = date.getHours();
                var min_get = date.getMinutes();
                var ff = parseInt(Number(min_get)) + parseInt(Number(1));
                if (ff === 60) {
                    ff = "01";
                }
                var advancetime = value + ' ' + hrs_get + ':' + ff;
                var IVR_ID = $scope.rndtype.toString();
                var data =
                {
                    "ivr_id": IVR_ID,
                    "phonebook": ph_id.toString(),
                    "timezone": "Asia/Kolkata",
                    "priority": "8",
                    "order_throttling": "10",
                    "retry_duration": "15",
                    "start_time": advancetime.toString(),
                    "end_time": "2021-12-12 19:04",
                    "max_retry": "1",
                    "call_scheduling": "[1, 1, 1, 1, 1, 1, 1]",
                    "call_scheduling_start_time": "09:00",
                    "call_scheduling_stop_time": "21:00",
                    //"k_number": "+911133035250",
                    "k_number": "+918043692464", //for billing purpose it can be any number
                    "is_transactional": "false",
                    //"caller_id":"+918048995956" // display in parents mobile
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json',
                        'x-api-key': 'lF4vZUSwA8Jab0ABWsITtxwM1ZwL6h2jZDdCTX30',
                        'Authorization': '964c52d4-016a-4d31-9b0a-86daaf43bbc5'
                    }
                };
                $http.post('https://kpi.knowlarity.com/Basic/v1/account/call/campaign', data, config)
                    .success(function (data, status, headers, config) {
                        $scope.PostDataResponse = data;
                        if (data.status_code === "1" || data.status_code === 1) {
                            swal("Call will initiate after a Minute!!");
                            $state.reload();
                            var abc = $scope.myTabIndex;
                            $scope.myTabIndex = abc + 1;
                            $scope.clearivrsmodal();
                        }
                        else {
                            swal('Please Try again Later !!!');
                        }
                    })
                    .error(function (data, status, header, config) {
                        $scope.ResponseDetails = "Data: " + data;
                    });
            }
        };

        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [10, 15, 20],
            paginationPageSize: 10,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'amsT_Id', displayName: 'AMST Id' },
                { name: 'studentName', displayName: 'Student Name' },
                { name: 'amsT_AdmNo', displayName: 'Admission No' },
                { name: 'amsT_emailId', displayName: 'E-Mail' },
                { name: 'amsT_MobileNo', displayName: 'MobileNo' }
            ]
        };
        $scope.Getstudentdetails_IVRS = function () {
            if ($scope.myFormnew1.$valid) {
                var data = {
                    "ASMAY_ID": $scope.ivrs_ASMAY_Id,
                    "ASMCL_ID": $scope.Ivrs_asmcL_Id,
                    "ASMS_ID": $scope.ivrs_asmS_Id
                };
                apiService.create("IVRSOBD/ivrgetstudetails", data).then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 10;
                    $scope.datagridviewIVR = false;
                    $scope.gridOptions.data = promise.maindata;
                    $scope.dataArray = promise.maindata;
                });
            }
        };

        $scope.maketestBulk_calls = function (objivr) {

        }

        $scope.maketestBulk_callsold = function (objivr) {
            var mobi = "";

            if ($scope.rndtype === '1000033778') {
                angular.forEach($scope.dataArray, function (item) {
                    if (!!item.amsT_MobileNo) {
                        if (mobi === "") {
                            mobi = "+91" + item.amsT_MobileNo + "," + $scope.mi_id + "," + $scope.objivr.ttscontent +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                        else {
                            mobi += ";+91" + item.amsT_MobileNo + "," + $scope.mi_id + "," + $scope.objivr.ttscontent +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                    }
                });
            }
            else if ($scope.rndtype === '1000033928') {
                angular.forEach($scope.dataArray, function (item) {
                    if (!!item.amsT_MobileNo) {
                        if (mobi === "") {
                            mobi = "+91" + item.amsT_MobileNo + "," + $scope.mi_id + "," + $scope.objivr.urlimage +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                        else {
                            mobi += ";+91" + item.amsT_MobileNo + "," + $scope.mi_id + "," + $scope.objivr.urlimage +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                    }
                });
            }
            var data = {
                "phonebookname": "phonebook",
                "numbers": mobi
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json charset=UTF-8',
                    'x-api-key': 'lF4vZUSwA8Jab0ABWsITtxwM1ZwL6h2jZDdCTX30',
                    'Authorization': '964c52d4-016a-4d31-9b0a-86daaf43bbc5',
                    //'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'POST',
                    'cache-control': 'no-cache'
                }
            };
            $http.post('https://kpi.knowlarity.com/Basic/v1/account/contacts/phonebook', data, config)
                .success(function (data, status, config) {
                    $scope.getMessage(data.id);
                })
                .error(function (data, status, config) {
                    $scope.PostDataResponse = "Data: " + data;
                });
        };


        $scope.UploadSoundfile = [];
        $scope.UploadSoundfile = function (input, document) {
            $scope.UploadSoundfile = input.files;
            if (input.files && input.files[0]) {
                if ((input.files[0].type == "audio/mp3" || input.files[0].type == "audio/mpeg") && input.files[0].size <= 922097152) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadsoundfiel();
                }
                else if (input.files[0].type != "audio/mp3" || input.files[0].type != "audio/mpeg") {
                    swal("Please Upload the sound file");
                    return;
                } else if (input.files[0].size > 922097152) {
                    swal("sound size should be less than 922MB");
                    return;
                }
            }
        };
        function Uploadsoundfiel() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadSoundfile.length; i++) {
                formData.append("File", $scope.UploadSoundfile[i]);
            }
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadsoundtrack", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    swal(d);
                    $scope.objivr.urlimage = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            Uploads1(miid);
        }


        $scope.changeradioregularnew = function () {
            $scope.count = 0;
            $scope.GetStudentDetails();


        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };


        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.onview = function (filepath, filename) {

            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    var pdfId = document.getElementById("pdfId");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);


                    $('#showpdf').modal('show');
                });
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_videos);
        };

        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };

        $scope.uploadPrincipalSignature = [];

        $scope.uploadPrincipalSignature_whatsapp = function (input) {

            $scope.uploadPrincipalSignature = input.files;
            $scope.whatsapp_filename = input.files[0].name;
            if (input.files && input.files[0]) {

                if (input.files[0].size < 2097152) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                    {
                        UploadPrincipalsign(document);
                    }
                    else if (input.files[0].type === "video/mp4") {
                        UploadPrincipalsign(document);
                    }
                    else if (input.files[0].type === "application/pdf") {
                        UploadPrincipalsign(document);
                    }
                    else if (input.files[0].type === "application/msword") {
                        UploadPrincipalsign(document);
                    }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                        UploadPrincipalsign(document);
                    }
                    else if (input.files[0].type === "application/vnd.ms-excel") {
                        UploadPrincipalsign(document);
                    }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                        UploadPrincipalsign(document);
                    }
                    else if (input.files[0].type === "audio/mp3" || input.files[0].type === "audio/mpeg") {
                        UploadPrincipalsign(document);
                    } else {
                        swal("Upload Audio , MP4, Pdf, Image Files Only");
                    }
                }
                else {
                    swal("File size should be less than 2MB");
                    return;
                }
            }
        }

        function UploadPrincipalsign() {
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadPrincipalSignature.length; i++) {
                formData.append("File", $scope.uploadPrincipalSignature[i]);
            }

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadsoundtrack", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.fileattachementforwhatsapp = d;

                    var img = $scope.fileattachementforwhatsapp;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.whatsapp_filetype = lastelement;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        var imagedownload = "";
        $scope.downloaddirectimage = function (data, idd, type) {

            var studentreg = idd;
            $scope.imagedownload = data;
            imagedownload = data;

            var img = data;
            var imagarr = img.split('.');
            var lastelement = imagarr[imagarr.length - 1];
            var filetype = lastelement;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg
                    })[0].click();
                });
        };

        $scope.cancel = function () {
            synth.cancel();
            $scope.mobno = "";
            $scope.mes = "";

            $state.reload();
        };

        const synth = window.speechSynthesis;
        // Function to stop speech --added by adarsh
        $scope.stopSpeech = function () {
            synth.cancel();
            document.getElementById("speechOutput").textContent = 'Speech stopped manually.';
        };
        //this function works when the page name was changed from the navigation bar and stops speech --added by adarsh
        window.addEventListener('popstate', function (event) {
            $scope.stopSpeech();
        });
        //this function works when page reloads and stops speech --added by adarsh
        window.addEventListener('beforeunload', function (event) {
            $scope.stopSpeech();
        });
        $scope.speakTexthree = function () {
            var textToSpeak = $scope.obj.ivrs_flashs;
            var speechOutput = document.getElementById("speechOutput");

            if ('speechSynthesis' in window) {
                const synth = window.speechSynthesis;
                const utterance = new SpeechSynthesisUtterance(textToSpeak);

                var beforeUnloadHandler = function () {
                    synth.cancel();
                };

                // Ensure that $window is injected and use it to add event listener
                $window.addEventListener('beforeunload', beforeUnloadHandler);

                var routeChangeHandler = function () {
                    synth.cancel();
                };

                // Ensure that $rootScope is injected and use it to add event listener
                $rootScope.$on('$routeChangeStart', routeChangeHandler);

                utterance.onend = function () {
                    speechOutput.textContent = `Speaking: ${textToSpeak}`;
                };

                synth.speak(utterance);
            } else {
                speechOutput.textContent = 'Text-to-speech not supported in this browser.';
            }
        };







        $scope.speakTexttranslation = function () {
            var textTolonguage = $scope.obj.ivrs_Translation;
            var speechOutput = document.getElementById("speechOutput");
            if ('speechSynthesis' in window) {
                const synth = window.speechSynthesis;
                const utterance = new SpeechSynthesisUtterance(textTolonguage);
                synth.speak(utterance);
                speechOutput.textContent = `Speaking: ${textTolonguage}`;
            }
            else {
                speechOutput.textContent = 'Text-to-speech not supported in this browser.';
            }
        }
        $scope.clearid1 = function () {
            synth.cancel();
            $state.reload();
        }
        $scope.onlanguagechange = function () {
            if ($scope.obj1.Sourcelong == $scope.obj1.Targetlong) {
                swal("Both Languages Are same..", "", "warning")
            }

        }
        //  $scope.obj1.Sourcelong.value = "default";

        $scope.OntextChage = function () {
            $scope.translatedtxt = 't'
            //$scope.TexttoSpeak = "";
            var apiKey = ""; var url = "";
            if ($scope.obj1.Sourcelong != null && $scope.obj1.Targetlong != null && $scope.obj1.Sourcelong != "" && $scope.obj1.Targetlong != "") {
                apiKey = 'AIzaSyCqYiUTcgHWAVn5brSRNtAOB1b1EGam-DQ';
                url = `https://translation.googleapis.com/language/translate/v2?key=${apiKey}`;
                $scope.data = {
                    q: $scope.obj1.fromtext,
                    source: $scope.obj1.Sourcelong,
                    target: $scope.obj1.Targetlong,
                };

                fetch(url, {
                    method: 'POST',
                    body: JSON.stringify($scope.data),

                    headers: {
                        'Content-Type': 'application/json',
                    },
                })
                    .then(response => response.json())
                    .then(data => {
                        $scope.obj1.translatedtext1 = data.data.translations[0].translatedText;
                    })
                    .catch(error => {
                        console.error(error);
                        $scope.obj1.translatedtext1 = "";
                        $scope.obj1.fromtext = "";

                    });
            }

        }

        $scope.copytxt = function () {
            var copyText = document.getElementById("translatedtxt");
            copyText.select();
            copyText.setSelectionRange(0, 99999);
            navigator.clipboard.writeText(copyText.value);
        }

        //download translated text
        //$scope.downloadtext = function () {
        //    const link = document.createElement("a");
        //    var dwntxt = document.getElementById("translatedtxt").innerHTML
        //    var file = new Blob([dwntxt], { type: 'text/plain' });
        //    link.href = URL.createObjectURL(file);
        //    link.download = "translatedtxt.txt";
        //    link.click();
        //    URL.revokeObjectURL(link.href);
        //};
        $scope.ClearInellegence = function () {
            $scope.obj.ivrs_flashs = "";
            $scope.obj.Sourcelong = "";
            $scope.obj.Targetlong = "";
            $scope.obj.fromremarks = "";
            $scope.obj.ivrs_Translation = "";
            $scope.obj.translatedtxt = "";

        }


    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });
})();

angular
    .module('app').filter('keys', function () {

        return function (input) {
            if (!input) {
                return [];
            }
            delete input.$$hashKey;
            return Object.keys(input);
        }

    })

angular
    .module('app').directive("fileread", ['$rootScope', 'apiService', function ($rootScope, apiService) {

        return {
            scope: {
                opts: '='

            },
            link: function ($scope, $elm, $attrs) {

                $elm.on('change', function (changeEvent) {

                    var reader = new FileReader();

                    reader.onload = function (evt) {
                        $scope.$apply(function () {
                            var data = evt.target.result;

                            var workbook = XLSX.read(data, { type: 'binary' });

                            var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];

                            var data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);
                            if (data.length == 0) {

                                swal("Excel Sheet is Empty");
                                $elm.val(null);
                                $scope.opts.data = null;
                                return;

                            } else {
                                $scope.opts = {};

                                $scope.opts.columnDefs = [];

                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });
                                });
                                //dfsd
                                $scope.opts.data = data;

                                $scope.array = [];

                                var aaa = $rootScope.eee;
                                var cnt = 0;
                                var cnt1 = 0;
                                if ($rootScope.ivrscheck === true) {
                                    angular.forEach($scope.opts.columnDefs, function (ww) {
                                        if (ww.field === 'MobileNo') {
                                            cnt += 1;
                                        }
                                    });
                                    if (cnt > 0) {
                                        angular.forEach($scope.opts.data, function (ee) {
                                            $scope.array.push({
                                                ivrS_MobileNo: ee.MobileNo
                                            });
                                        });
                                        $rootScope.validateForm($scope.array, 'A');
                                    }
                                    else {
                                        swal('Excel Column Header Name Is Miss Matching , Change the Column Header As--"MobileNo"');
                                    }
                                }
                                else {
                                    angular.forEach($scope.opts.columnDefs, function (ww) {
                                        if (ww.field == 'MobileNo') {
                                            cnt += 1;
                                        }
                                        if (ww.field == 'EmailId') {
                                            cnt1 += 1;
                                        }

                                    });

                                    if ($rootScope.selr == 'Gemail') {
                                        if (cnt1 > 0) {
                                            angular.forEach($scope.opts.data, function (ee) {

                                                $scope.array.push({
                                                    hrmeM_EmailId: ee.EmailId
                                                })
                                            })
                                            $rootScope.validateForm($scope.array, 'E');
                                        }
                                        else {
                                            swal('Excel Column Header Name Is Miss Matching , Change the Column Header As--"EmailId"')
                                        }
                                    }
                                    if ($rootScope.selr == 'General') {
                                        if (cnt > 0) {
                                            angular.forEach($scope.opts.data, function (ee) {

                                                $scope.array.push({
                                                    hrmemnO_MobileNo: ee.MobileNoe
                                                })
                                            })
                                            $rootScope.validateForm($scope.array, 'S');
                                        }
                                        else {
                                            swal('Excel Column Header Name Is Miss Matching , Change the Column Header As--"MobileNo"')
                                        }
                                    }
                                }

                                console.log($scope.array);
                                console.log($scope.opts.data);
                            }
                        });
                    };
                    reader.readAsBinaryString(changeEvent.target.files[0]);

                });
            }
        }
    }]);
