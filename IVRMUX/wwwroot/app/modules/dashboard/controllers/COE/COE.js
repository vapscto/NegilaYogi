(function () {
    'use strict';

    angular
        .module('app')
        .controller('COE', COE);

    COE.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache', '$filter', 'Excel', '$timeout', 'exportUiGridService']

    function COE($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, $filter, Excel, $timeout, exportUiGridService) {
        $scope.msgConfirmed = false;
        $scope.resendbtn = false;
        $scope.mobileOtpfield = false;
        $scope.verified = false;
        $scope.failedVerification = false;
        $scope.mobileOtpfield = false;
      
        $scope.loadData = function () {
            $scope.Oters_Mail_Id = "";
            apiService.getURI("COE/getData/", 1).then(function (promise) {
                if (promise.eventCount > 0) {
                    
                    $scope.Events = promise.eventsList;
                    $scope.otpmobileNo = promise.configuration[0];
                }
                else {
                    swal("No Events Found");
                }
            });
        }
        $scope.getEvents = function () {
            var Id = $scope.COEME_Id;
           
            apiService.getURI("COE/getEvents/", Id).then(function (promise) {
                
                $scope.EventsDetails = promise.eventsDetails;
                $scope.Others_list.length = 0;
                if ($scope.EventsDetails != null) {
                    $scope.stud_chk = promise.eventsDetails[0].coeE_StudentFlag;
                    $scope.oldstud_chk = promise.eventsDetails[0].coeE_AlumniFlag;
                    $scope.stf_chk = promise.eventsDetails[0].coeE_EmployeeFlag;
                    $scope.oth_chk = promise.eventsDetails[0].coeE_OtherFlag;
                    $scope.classList = promise.classList;
                    $scope.groupTypeList = promise.groupTypeList;
                    if ($scope.stud_chk == true) {
                        for (var i = 0; i < $scope.classList.length; i++) {
                            for (var j = 0; j < promise.coeEventsClassesList.length; j++) {
                                if ($scope.classList[i].asmcL_Id == promise.coeEventsClassesList[j]) {
                                    $scope.classList[i].class = true;
                                }
                            }
                        }
                    }
                    if ($scope.stf_chk == true) {
                        for (var i = 0; i < $scope.groupTypeList.length; i++) {
                            for (var j = 0; j < promise.coeEventsEmployees.length; j++) {
                                if ($scope.groupTypeList[i].hrmgT_Id == promise.coeEventsEmployees[j]) {
                                    $scope.groupTypeList[i].Selected = true;
                                }
                            }
                        }
                    }
                    
                    if ($scope.oth_chk == true) {
                       
                        for (var k = 0; k < promise.coeEventsOthers.length; k++) {
                            $scope.Others_list.push({ COEEO_MobileNo: promise.coeEventsOthers[k].coeeO_MobileNo, COEEO_Emailid: promise.coeEventsOthers[k].coeeO_Emailid, COEEO_Name: promise.coeEventsOthers[k].coeeO_Name });
                        }
                        $scope.mobilecount = $scope.Others_list.length;
                        }
                }
            });
        }
        $scope.all_check = function () {
            
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.classList, function (itm) {
                itm.class = toggleStatus;
            });
        }
        $scope.all_check1 = function () {
            var toggleStatus = $scope.usercheck1;
            angular.forEach($scope.groupTypeList, function (itm) {
                itm.Selected = toggleStatus;
            });
        }
        $scope.togchkbx = function () {
            
            $scope.usercheck = $scope.classList.every(function (options) {
                return options.class;
            });
        }
        $scope.togchkbx1 = function () {
            $scope.usercheck1 = $scope.groupTypeList.every(function (options) {
                return options.Selected;
            });
        }
        $scope.isOptionsRequired = function () {
            if ($scope.stud_chk == true) {
                return !$scope.classList.some(function (options) {
                    return options.class;
                });
            }
            else if ($scope.stud_chk == false) {
                return false;
            }

        }
        $scope.isOptionsRequired1 = function () {
            if ($scope.stf_chk == true) {
                return !$scope.groupTypeList.some(function (options) {
                    return options.Selected;
                });
            }
            else if ($scope.stf_chk == false) {
                return false;
            }

        }
        $scope.mobilenos_temp = [];
        $scope.mobilecount = 0;
        $scope.Others_list = [];
        $scope.add_mobile_nos = function () {
            
            var count = 0;

            //angular.forEach($scope.mobilenos_temp, function (mobileno123) {
            //    if (mobileno123 == $scope.Oters_Mob_No) {
            //        count_m += 1;
            //    }
            //});


            angular.forEach($scope.Others_list, function (other123) {
                if (other123.COEEO_MobileNo == $scope.Oters_Mob_No || other123.COEEO_Emailid == $scope.Oters_Mail_Id) {
                    count += 1;
                }
            });
            if (count == 0) {
                //$scope.mobilenos_temp.push($scope.Oters_Mob_No);
                //$scope.Oters_Mob_No = "";
                $scope.Others_list.push({ COEEO_MobileNo: $scope.Oters_Mob_No, COEEO_Emailid: $scope.Oters_Mail_Id, COEEO_Name: $scope.Oters_Name });
                $scope.Oters_Mob_No = "";
                $scope.Oters_Mail_Id = "";
                $scope.Oters_Name = "";

            }
            else if (count > 0) {
                swal("Entered Details are Already Added !!!");
            }



            //  $scope.mobilenos_temp.push($scope.Oters_Mob_No);
            $scope.mobilecount = $scope.Others_list.length;

            //$scope.Oters_Mob_No = "";
        }
        $scope.remove_mob_no = function (sel_mob_no_del) {
            
            for (var i = 0; i < $scope.Others_list.length; i++) {
                
                var mob123 = $scope.Others_list[i];
                if (mob123 == sel_mob_no_del) {
                    
                    $scope.Others_list.splice(i, 1);
                }
            }
            $scope.mobilecount = $scope.Others_list.length;

        }
        $scope.submitted = false;
        $scope.selectedClasses=[];
        $scope.selectedGroupTypeList=[];
        $scope.send = function () {
            
            if ($scope.myForm.$valid) {
                var smsflag = "";
                var emailflag = "";
                var confirmmsg = "";
                if ($scope.sms == true) {
                    smsflag = " SMS";
                }
                if ($scope.email == true) {
                    emailflag = " EMAIL";
                }
                if (smsflag != "" && emailflag != "") {
                    confirmmsg = smsflag + " " + " And" + emailflag;
                }
                else if (smsflag != "") {
                    confirmmsg = smsflag;
                }
                else if (emailflag != "") {
                    confirmmsg = emailflag;
                }
                swal({
                    title: "Are you sure?",
                    text: "Do you want to send" + confirmmsg,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: true,
                    closeOnCancel: false
                },
     function (isConfirm) {
         if (isConfirm) {
             apiService.getURI("COE/Confirmation/", 1).then(function (promise) {
                 $scope.msgConfirmed = true;
                 $scope.disablebtn = true;
             })
         }
         else {
             swal("Cancelled");
             $scope.msgConfirmed = false;
             $scope.disablebtn = false;
            
         }
     });

               
            }
            else {
                $scope.submitted = true;

            }
        }
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.sendOTP=function()
        {
            
            // var MobileNO = "9686061628";
            var MobileNO = $scope.otpmobileNo;
            if (MobileNO === "" || MobileNO === null|| MobileNO === undefined) {
                swal("Please Enter Mobile Number!");
                $scope.mobileOtpfield = false;
                }
                else {
                    var mobno = {
                        "mobileNo": MobileNO
                    }

                    apiService.create("Login/getOTPForMobile", mobno).then(function (promise) {
                        swal(promise);
                        $scope.mobileOtpfield = true;
                        $scope.disableotpbtn = true;
                    })
                }
        }
        $scope.verifyOTP = function () {
            if ($scope.otp === "" || $scope.otp === null  || $scope.otp === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {
            
                var mobno = {
                    "MOBILEOTP":$scope.otp
                }

                apiService.create("Login/VerifymobileOtp", mobno).then(function (promise) {

                    if (promise === "Success") {
                        $scope.verified = true;
                        $scope.failedVerification = false;
                        $scope.resendbtn = false;
                        $scope.selectedClasses.length = 0;
                        $scope.selectedGroupTypeList.length = 0;
                        if ($scope.stud_chk == true) {
                            angular.forEach($scope.classList, function (role) {
                                if (role.class) $scope.selectedClasses.push(role);
                            })
                        }
                        if ($scope.stf_chk == true) {
                            angular.forEach($scope.groupTypeList, function (role) {
                                if (role.Selected) $scope.selectedGroupTypeList.push(role);
                            })
                        }
                        var data = {
                            "COEME_Id": $scope.COEME_Id,
                            "Others_list": $scope.Others_list,
                            "selectedClasses": $scope.selectedClasses,
                            "selectedGroupTypeList": $scope.selectedGroupTypeList,
                            "COEE_StudentFlag": $scope.stud_chk,
                            "COEE_EmployeeFlag": $scope.stf_chk,
                            "COEE_OtherFlag": $scope.oth_chk,
                            "SMS_Flag": $scope.sms,
                            "Email_Flag": $scope.email
                        }
                        apiService.create("COE/Sendmessage/", data).then(function (promise) {
                            if (promise.msgStatus == 'success' && promise.mailStatus == 'success') {
                                swal("Sms and Email Sent Successfully.");
                                $state.reload();
                            }
                            else if (promise.msgStatus == 'success') {
                                swal("Sms Sent Successfully.");
                                $state.reload();
                            }
                            else if (promise.mailStatus == 'success') {
                                swal("Email Sent Successfully.");
                                $state.reload();
                            }
                            else {
                                swal("Failed to send message");
                            }
                        })
                    }
                    else if (promise === "Fail") {
                        swal("OTP Mismatch!!");
                        $scope.failedVerification = true;
                        $scope.verified = false;
                        $scope.resendbtn = false;
                    }
                    else {
                        swal("OTP Expired. Please Click On resend OTP");
                        $scope.resendbtn = true;
                        $scope.failedVerification = false;
                        $scope.verified = false;
                       
                    }
                })
           }
        }
    }
})();
