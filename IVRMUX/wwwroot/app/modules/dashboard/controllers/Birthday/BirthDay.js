
(function () {
    'use strict';
    angular
        .module('app')
        .controller('BirthDayController', BirthDayController)

    BirthDayController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function BirthDayController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.editEmployee = {};
        $scope.printstudents = [];
        $scope.albumNameArraycolumn = [];
        //$scope.SFName = "";
        //$scope.SMobileNo = "";
        //$scope.SemailId = "";

        $scope.LoadData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.hidegv2 = false;
            $scope.hidegv1 = false;
            var pageid = 1;
            apiService.getURI("BirthDay/", pageid).
                then(function (promise) {

                    $scope.classlist = promise.classDrpDwn;
                    $scope.sectionlist = promise.sectionDrpDwn;
                });
        }

        //$scope.Preview() = function () {

        //}


        $scope.fillstudentlist = function () {
            $scope.hidegv2 = false;
            var data = {
                "amst_dob": new Date($scope.Start_Date).toDateString(),
                "asmcL_Id": $scope.asmcL_Id,
                "sectionid": $scope.asmC_Id,
                //"rdbbutton": $scope.radioption,

            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("BirthDay/getS/", data).

                then(function (promise) {


                    if ($scope.asmC_Id == 1) {
                        $scope.hidegv2 = false;
                        $scope.hidegv1 = true;
                    }
                    else {
                        $scope.hidegv2 = true;
                        $scope.hidegv1 = false;
                    }

                    if (promise.count > 0) {
                        $scope.gridview1 = true;

                        $scope.birthdaylist = promise.studentDetails;
                        $scope.adtable = false;
                    } else {
                        swal("No records Found");
                        $scope.hidegv2 = false;
                        $scope.adtable = true;
                    }
                })


        }
        $scope.bindstaff = function () {

            var data = {

                "rdbbutton": $scope.radioption

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("BirthDay/BindStaff/", data).

                then(function (promise) {

                    if ($scope.radioption == "student" || $scope.radioption == "Alumni") {
                        if (promise.count > 0) {
                            $scope.hidegv1 = false;
                            $scope.hidegv2 = true;
                            $scope.studentlist = promise.studentlist;
                        }
                        else {
                            swal("No Records Found");
                            $scope.hidegv2 = false;
                            $scope.hidegv1 = false;
                        }

                    }
                    else if ($scope.radioption == "Staff") {
                        if (promise.count > 0) {
                            $scope.hidegv1 = true;
                            $scope.hidegv2 = false;
                            $scope.staffList = promise.staffList;
                        }
                        else {
                            swal("No Records Found");
                            $scope.hidegv1 = false;
                            $scope.hidegv2 = false;
                        }

                    }
                    //if ($scope.radioption == 1) {
                    //    $scope.hidegv1 = false;
                    //    $scope.hidegv2 = true;
                    //}
                    //else {
                    //    $scope.hidegv1 = true;
                    //    $scope.hidegv2 = false;
                    //}
                    //if (promise.count > 0) {

                    //    $scope.StafFlist = promise.staffList;
                    //    $scope.adtable = false;
                    //} else {

                    //    swal("No records Found");
                    //    $scope.hidegv1 = false;
                    //    $scope.adtable = true;
                    //}
                })


        }
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.selectedStudent = [];
        $scope.selectedEmployees = [];
        $scope.sendmsg = function () {
            if ($scope.radioption == "student" || $scope.radioption == "Alumni") {
                var whatsappstatus = "";
                var smsflag = "";
                var emailflag = "";
                var confirmmsg = "";
                if ($scope.sms != null && $scope.sms != "" && $scope.sms != undefined) {
                    smsflag = " SMS";
                }
                if ($scope.email != null && $scope.email != "" && $scope.email != undefined) {
                    emailflag = " EMAIL";
                }
                if ($scope.whatsappstatus != null && $scope.whatsappstatus != "" && $scope.whatsappstatus != undefined) {
                    whatsappstatus = " WHATSAPP";
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
                else if (whatsappstatus != "") {
                    confirmmsg = whatsappstatus;
                }

                $scope.selectedStudent.length = 0;

                for (var i = 0; i < $scope.studentlist.length; i++) {

                    if ($scope.studentlist[i].Selected1 == true) {
                        $scope.selectedStudent.push($scope.studentlist[i]);
                    }
                }

                if ($scope.selectedStudent.length == 0) {
                    return swal("Please Select Students Whom You Want To Send" + confirmmsg);
                }
                else {


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
                                var data = {
                                    "selectedStudent": $scope.selectedStudent,
                                    "selectedEmployees": $scope.selectedEmployees,
                                    "rdbbutton": $scope.radioption,
                                    "smsflag": $scope.sms,
                                    "emailflag": $scope.email,
                                    "whatsappflag": $scope.whatsappflag
                                }
                                apiService.create("BirthDay/Sendmsg", data).then(function (promise) {
                                    
                                    if (promise.smsStatus != "" && promise.smsStatus != null && promise.emailStatus != "" && promise.emailStatus != null) {
                                        swal("SMS And Email Sent Successfully.");
                                        $state.reload();
                                    }
                                    else if (promise.smsStatus != "" && promise.smsStatus != null) {
                                        swal("SMS Sent Successfully.");
                                        $state.reload();
                                    }
                                    else if (promise.emailStatus != "" && promise.emailStatus != null) {
                                        swal("EMAIL Sent Successfully.");
                                        $state.reload();
                                    }
                                  
                                    else {
                                        swal("Something went wrong");
                                        $state.reload();
                                    }
                                });

                                apiService.create("BirthDay/whatsapp", data).then(function (promise) {

                                    if (promise.whatsappstatus = "success" ) {
                                        swal("Whatsapp Sent Successfully.");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Something went wrong");
                                        $state.reload();
                                    }
                                });



                            }
                            else {
                                swal("Cancelled");
                            }
                        });

                }

            }
            else if ($scope.radioption == "Staff") {

                var smsflag = "";
                var emailflag = "";
                var confirmmsg = "";
                if ($scope.sms != null && $scope.sms != "" && $scope.sms != undefined) {
                    smsflag = " SMS";
                }
                if ($scope.email != null && $scope.email != "" && $scope.email != undefined) {
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


                $scope.selectedEmployees.length = 0;
                for (var i = 0; i < $scope.staffList.length; i++) {
                    if ($scope.staffList[i].Selected2 == true) {
                        $scope.selectedEmployees.push($scope.staffList[i]);
                    }
                }
                if ($scope.selectedEmployees.length == 0) {
                    return swal("Please Select Employees Whom You Want To Send " + confirmmsg);
                }
                else {
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

                                var data = {
                                    "selectedStudent": $scope.selectedStudent,
                                    "selectedEmployees": $scope.selectedEmployees,
                                    "rdbbutton": $scope.radioption,
                                    "smsflag": $scope.sms,
                                    "emailflag": $scope.email
                                }
                                apiService.create("BirthDay/Sendmsg", data).then(function (promise) {
                                    if (promise.smsStatus != "" && promise.smsStatus != null && promise.emailStatus != "" && promise.emailStatus != null) {
                                        swal("SMS And Email Sent Successfully.");
                                        $state.reload();
                                    }
                                    else if (promise.smsStatus != "" && promise.smsStatus != null) {
                                        swal("SMS Sent Successfully.");
                                        $state.reload();
                                    }
                                    else if (promise.emailStatus != "" && promise.emailStatus != null) {
                                        swal("EMAIL Sent Successfully.");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Something went wrong");
                                        $state.reload();
                                    }
                                });
                            }
                            else {
                                swal("Cancelled");
                            }
                        });

                }
            }
        }
        $scope.clear = function () {
            $scope.asmcL_Id = "";
            $scope.Start_Date = "";
            $scope.asmC_Id = "";
            $scope.submitted = false;
            $scope.IsHiddendown = false;
            //$scope.export_flag = true;

            //$scope.myform.$setPristine();
            //$scope.myform.$setUntouched();
        }
        $scope.datechange = function () {
            $scope.clear();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.toggleAll = function () {

            var toggleStatus1 = $scope.details1;
            var toggleStatus2 = $scope.details2;
            if ($scope.radioption == "student" || $scope.radioption == "Alumni") {
                angular.forEach($scope.studentlist, function (itm) { itm.Selected1 = toggleStatus1; });
            }
            else if ($scope.radioption == "Staff") {
                angular.forEach($scope.staffList, function (itm) { itm.Selected2 = toggleStatus2; });
            }

        }

        $scope.optionToggled = function () {


            if ($scope.radioption == "student" || $scope.radioption == "Alumni") {
                $scope.details1 = $scope.studentlist.every(function (itm) { return itm.Selected1; })
            }
            else if ($scope.radioption == "Staff") {
                $scope.details2 = $scope.staffList.every(function (itm) { return itm.Selected2; })
            }
        }

        $scope.Activitycheckboxchcked = [];
        $scope.Activitycheckboxchcked1 = [];




        $scope.sendsms = function () {

            if ($scope.radioption == "student" || $scope.radioption == "Alumni") {
                if ($scope.birthdaylist != "" && $scope.birthdaylist != null) {
                    if ($scope.birthdaylist.length > 0) {
                        for (var i = 0; i < $scope.birthdaylist.length; i++) {
                            if ($scope.birthdaylist[i].Selected1 == true) {

                                $scope.Activitycheckboxchcked.push($scope.birthdaylist[i]);
                            }
                        }
                    }
                }
            }
            else {
                if ($scope.StafFlist != "" && $scope.StafFlist != null) {
                    if ($scope.StafFlist.length > 0) {
                        for (var j = 0; j < $scope.StafFlist.length; j++) {
                            if ($scope.StafFlist[j].Selected2 == true) {

                                $scope.Activitycheckboxchcked1.push($scope.StafFlist[j]);
                            }
                        }
                    }
                }
            }


            var data = {
                SelectedActivityDetails_stu: $scope.Activitycheckboxchcked,
                SelectedActivityDetails1_stf: $scope.Activitycheckboxchcked1,
                "sms_text": $scope.smsbody,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("BirthDay/Sendsms", data).then(function (promise) {

            })

        }

        $scope.sendemail = function () {

            if ($scope.radioption == "student" || $scope.radioption == "Alumni") {
                if ($scope.birthdaylist != "" && $scope.birthdaylist != null) {
                    if ($scope.birthdaylist.length > 0) {
                        for (var i = 0; i < $scope.birthdaylist.length; i++) {
                            if ($scope.birthdaylist[i].Selected1 == true) {

                                $scope.Activitycheckboxchcked.push($scope.birthdaylist[i]);
                            }
                        }
                    }
                }
            }
            else if ($scope.radioption == "staff") {
                if ($scope.StafFlist != "" && $scope.StafFlist != null) {
                    if ($scope.StafFlist.length > 0) {
                        for (var j = 0; j < $scope.StafFlist.length; j++) {
                            if ($scope.StafFlist[j].Selected2 == true) {

                                $scope.Activitycheckboxchcked1.push($scope.StafFlist[j]);
                            }
                        }
                    }
                }

            }




            var data = {
                SelectedActivityDetails_stu: $scope.Activitycheckboxchcked,
                SelectedActivityDetails1_stf: $scope.Activitycheckboxchcked1,
                "email_header": $scope.mailHeader,
                "mailsubject": $scope.mailsubject,
                "mailbody": $scope.mailbody,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("BirthDay/Sendsms", data).then(function (promise) {


            })

        }
        $scope.search = "";
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.studentName)).indexOf(angular.lowercase($scope.search)) >= 0
        }

    }

})();