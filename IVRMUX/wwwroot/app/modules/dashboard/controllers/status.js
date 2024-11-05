
(function () {
    'use strict';
    angular
        .module('app')
        .controller('statusController', statusController)

    statusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'Excel', '$compile', '$timeout', 'superCache']
    function statusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, Excel, $compile, $timeout, superCache) {

        // Load initial data

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (configsettings !=null && configsettings.length > 0) {
            var emailotp = configsettings[0].ivrmgC_emailValOTPFlag;
            var mobileotp = configsettings[0].ivrmgC_MobileValOTPFlag;

            $scope.emailotp = configsettings[0].ivrmgC_emailValOTPFlag;
            $scope.mobileotp = configsettings[0].ivrmgC_MobileValOTPFlag;

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
        $scope.printDataadd = function () {
            var innerContents = document.getElementById("SRKVSStudentAddressBook").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet"  href="css/print/SRKVSStudentAddressBook/SRKVSStudentAddressBookPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 300);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.otpmobile = false;
        $scope.otpemail = false;
        $scope.buttonotp = false;
        $scope.smsshow = false;
        $scope.emailshow = false;
        $scope.ondropdown = false;
        $scope.resendotpbutton = false;
        $scope.page2 = "pag2";
        $scope.page3 = "pag3";

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;

        $scope.user = {};
        $scope.remarks = [];
        $scope.rptStatus = false;
        $scope.angularData = {
            'nameList': []
        };

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "pasR_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.IsHidden = true;
        // $scope.IsHidden1 = false;

        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }
        //  $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }
        $scope.printdatatable = [];
        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all2;
            angular.forEach($scope.studentlist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    if ($scope.printdatatable.indexOf(itm) === -1) {
                        $scope.printdatatable.push(itm);
                    }
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });

        }

        $scope.selected = function (SelectedStudentRecord, index) {


            $scope.all2 = $scope.studentlist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        }

        $scope.loadbasicdata = function () {

            apiService.get("Status/getinitialdata/").then(function (promise) {
                if (promise.academicList != undefined && promise.academicList.length>0) {
                    $scope.academiclist = promise.academicList;
                    $scope.yearid = $scope.academiclist[0].asmaY_Id;
                    $scope.classlist = promise.classlist;
                    $scope.statuslist = promise.statuslist;

                    $scope.onclickloaddata();

                }
                else if (promise.isSessionExpired == true) {
                    $cookieStore.remove('pagetitle');
                    $cookieStore.put("pagetitle", "Home");
                    $cookieStore.remove('IsLogged');
                    apiService.getURI("Login/clearsession", 1).
                        then(function (promise) {
                            //$state.go("login");
                            if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                                window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                            }
                            else {
                                window.location.href = "http://localhost:57606/#/login/";
                            }

                        })
                    swal("Your Session has been Expired.....Please Re-login");

                }
            });
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
                $scope.clslst2 = $scope.albumNameArray;
                $("#myModalswal").modal({ backdrop: false });
                $('#myModalswal').modal('show');
            })

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
            $('#myModalswal').modal('hide');
            $('#myModalotp').modal('show');
        }


        $scope.search = '';
        $scope.filterOnLocation = function (user) {
            if ($scope.search != "") {
                $scope.ondropdown = true;
            }
            else {
                $scope.ondropdown = false;
            }
            //  (angular.lowercase(obj.User_Name)).indexOf(angular.lowercase($scope.searchValue))
            return (angular.lowercase(user.name)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(user.statusName)).indexOf(angular.lowercase($scope.search)) >= 0
                ||
                (angular.lowercase(user.pasR_RegistrationNo)).indexOf(angular.lowercase($scope.search)) >= 0
                ||
                (angular.lowercase(user.remark)).indexOf(angular.lowercase($scope.search)) >= 0
                ||
                (angular.lowercase(user.pasR_Sex)).indexOf(angular.lowercase($scope.search)) >= 0
                ||
                (angular.lowercase(user.className)).indexOf(angular.lowercase($scope.search)) >= 0;
            ;
        };

        // search student based on year, class and status
        $scope.submitted = false;

        $scope.searchuser = function () {
            $scope.all2 = "";
            $scope.search = "";
            $scope.yearid = $scope.academiclist[0].asmaY_Id;

            if ($scope.myForm.$valid) {
                $scope.currentPage = 1;
                $scope.itemsPerPage = paginationformasters;
                if ($scope.asmaY_Id == "") {
                    $scope.asmaY_Id = 0;
                }
                if ($scope.pamsT_Id == "" || $scope.pamsT_Id == undefined) {
                    $scope.pamsT_Id = 0;
                }
                if ($scope.asmcL_Id == "") {
                    $scope.asmcL_Id = 0;
                }

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "PAMST_Id": $scope.pamsT_Id,
                    "status_type": $scope.status_type
                }

                apiService.create("Status/SearchData", data).then(function (promise) {

                    if (promise.studentlist != null || promise.studentlist != undefined) {

                        $scope.studentlist = promise.studentlist;

                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < $scope.studentlist.length; i++) {
                            if ($scope.studentlist[i].pasR_FirstName != '') {
                                if ($scope.studentlist[i].pasR_MiddleName != null && $scope.studentlist[i].pasR_MiddleName != '' && $scope.studentlist[i].pasR_MiddleName != "" && $scope.studentlist[i].pasR_MiddleName!=="False" ) {
                                    if ($scope.studentlist[i].pasR_LastName != null && $scope.studentlist[i].pasR_LastName != '' && $scope.studentlist[i].pasR_LastName != "" && $scope.studentlist[i].pasR_LastName !== "False") {

                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pasR_FirstName + " " + $scope.studentlist[i].pasR_MiddleName + " " + $scope.studentlist[i].pasR_LastName, pasR_Id: $scope.studentlist[i].pasR_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pasR_FirstName + " " + $scope.studentlist[i].pasR_MiddleName, pasR_Id: $scope.studentlist[i].pasR_Id });
                                    }
                                }
                                else {
                                    if ($scope.studentlist[i].pasR_LastName != null && $scope.studentlist[i].pasR_LastName != '' && $scope.studentlist[i].pasR_LastName != "" && $scope.studentlist[i].pasR_LastName !== "False") {
                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pasR_FirstName + " " + $scope.studentlist[i].pasR_LastName, pasR_Id: $scope.studentlist[i].pasR_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pasR_FirstName, pasR_Id: $scope.studentlist[i].pasR_Id });
                                    }
                                }

                                $scope.studentlist[i].name = $scope.albumNameArray1[i].name;

                                var str = $scope.studentlist[i].pasR_ConStreet;
                                var strReplacedWith = " ";
                                if (str != null && str != '') {
                                    var currentIndex = str.lastIndexOf(",");
                                    str = str.substring(0, currentIndex) + strReplacedWith + str.substring(currentIndex + 1, str.length);
                                    $scope.studentlist[i].pasR_ConStreet = str;
                                }

                                var str1 = $scope.studentlist[i].pasR_ConArea;
                                var strReplacedWith1 = " ";
                                if (str1 != null && str1 != '') {
                                    var currentIndex1 = str1.lastIndexOf(",");
                                    str1 = str1.substring(0, currentIndex1) + strReplacedWith1 + str1.substring(currentIndex1 + 1, str1.length);
                                    $scope.studentlist[i].pasR_ConArea = str1;
                                }
                                
                            }
                        }

                        $scope.presentCountgrid = promise.studentlist.length;
                        $scope.showPrintB = true;
                        $scope.showExportB = true;
                        if ($scope.pamsT_Id == "0" && $scope.status_type === 'Appsts') {
                            $scope.abc = "787926";
                        }
                        else if ($scope.pamsT_Id == "0" && $scope.status_type === 'admsts') {
                            $scope.abc = "1";
                        }
                        else {
                            $scope.abc = $scope.pamsT_Id;
                        }
                        $scope.IsHidden1 = true;

                        //if (promise.studentlist[0].statusFlag == "RPT" && $scope.pamS_Id != "" && $scope.pamS_Id != undefined) {
                        //    $scope.rptStatus = true;
                        //}
                        //else {
                        //    $scope.rptStatus = false;
                        //}
                        if ($scope.status_type === 'Appsts') {
                            $scope.Student_status = 787926;
                            $scope.set_student_staus();
                            $scope.Student_status = 786;
                            $scope.set_student_staus();
                        }


                    }
                    else {
                        $scope.IsHidden1 = false;
                        $scope.showPrintB = false;
                        $scope.showExportB = false;
                        swal("No records Found");
                    }

                });
                $scope.sort = function (keyname) {

                    $scope.sortKey = keyname;   //set the sortKey to the param passed
                    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
                }
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interactedSM = function (field) {
            return $scope.submitted1 || field.$dirty1;
        };
        // Empty scope variable
        $scope.checkboxselected = [];


        $scope.clearfieldspage = function () {
          
          $state.reload();
        }

        $scope.clearfields = function () {
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.pamsT_Id = "";
            $scope.IsHidden1 = false;
            //  $state.reload();
            $scope.Student_status = "786";
            $scope.showPrintB = false;
            $scope.showExportB = false;
            $scope.search = "";
        }
        $scope.Clearid = function () {
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.changedegault = function () {
            if ($scope.default == true) {
                $scope.sms = false;
                $scope.email = false;
                $scope.smsshow = false;
                $scope.emailshow = false;
                $scope.MailSubject = "";
                $scope.MailHeader = "";
                $scope.Parameter_email = "";
                $scope.MailFooter = "";
                $scope.smscontent = "";
            }            
        };

        $scope.smssending = function () {
            if ($scope.sms == true) {
                $scope.smsshow = true;
                $scope.default = false;
            }
            else {
                $scope.smscontent = "";
                $scope.smsshow = false;
            }
        };

        $scope.emailsending = function () {
            if ($scope.email == true) {
                $scope.emailshow = true;
                $scope.default = false;
            }
            else {
                $scope.MailSubject = "";
                $scope.MailHeader = "";
                $scope.Parameter_email = "";
                $scope.MailFooter = "";
                $scope.emailshow = false;
            }
        };

        $scope.submitted1 = false;
        $scope.saveUseronce = function () {
            $scope.otpmobile = false;
            $scope.otpemail = false;
            $scope.buttonotp = false;
            if ($scope.emailotp == 1 || $scope.mobileotp == 1) {
                if ($scope.email == true || $scope.sms == true || $scope.default == true) {
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
                angular.forEach($scope.studentlist, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                });
                if ($scope.albumNameArray.length > 0) {
                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true || $scope.default == true))) {
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
                    else if ((mobileotp != 0 && ($scope.email == true || $scope.sms == true || $scope.default == true))) {
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
        };

        $scope.submitted1 = false;
        $scope.saveUser = function (data, data1) {

            $('#myModalswal').modal('hide');
            swal({
                title: "Are you sure?",
                text: "Do you want to Update Status ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                cancelButtonText: "Cancel!",
                closeOnConfirm: false,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {

                        if ($scope.myFormSM.$valid) {

                            var datalist = {
                                data_array: $scope.albumNameArray,
                                _type: $scope.status_type,
                                "smscontent": $scope.smscontent,
                                "header": $scope.MailHeader,
                                "Subject": $scope.MailSubject,
                                "Message": $scope.Parameter_email,
                                "Footer": $scope.MailFooter,
                                "smscheck": $scope.sms,
                                "emailcheck": $scope.email,
                                "defaultsmsemail": $scope.default
                            }
                            apiService.create("Status/", datalist).then(function (promise) {

                                if (promise.count == 0) {
                                    swal('Status Updated Successfully...!', 'success');
                                    $state.reload();
                                }
                                else {
                                    swal('Status Updated Successfully...!', 'success');
                                    $state.reload();
                                }
                            });



                        }

                        else {
                            $scope.submitted1 = true;
                        }
                    }
                    else {
                    }
                });
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


        //added by suryan 

        $scope.onclickloaddata = function () {

            $scope.statuslist = [];
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.pamsT_Id = "";
            $scope.studentlist = "";
            $scope.showPrintB = false;
            $scope.showExportB = false;
            $scope.search = "";
            //   $scope.Student_status = "786";
            if ($scope.status_type === 'Appsts') {
                $scope.statuslist = [{ pamsT_Id: 787926, pamsT_Status: "APP WAITING" }, { pamsT_Id: 787927, pamsT_Status: "APP REJECTED" }, { pamsT_Id: 787928, pamsT_Status: "APP ACCEPTED" }];
            }
            else if ($scope.status_type === 'admsts') {
                apiService.get("Status/getinitialdata/").then(function (promise) {
                    if (promise != "" && promise.statuslist.length>0) {
                        $scope.statuslist = promise.statuslist;
                    }
                });
            }
            $scope.IsHidden1 = false;
            $scope.Student_status = "786";
            $scope.set_student_staus();
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.testnoofstu = function (nostu) {

            angular.forEach($scope.studentlist, function (obj) {
                obj.checked = false;
            })
            // $scope.validate_time_noofstudent(nostu, NoOfMin, NoOfHr);
            if (nostu != undefined) {
                for (var k = 0; k < $scope.studentlist.length; k++) {
                    if (nostu < (k + 1)) {
                        break;
                    } else {
                        $scope.studentlist[k].checked = true;
                    }
                }
                //  $scope.makedisable = true;


                //$scope.sortKey = "pasR_RegistrationNo";   //set the sortKey to the param passed
                //$scope.reverse = false; //if true make it false and vice versa


            }



            // $scope.order

        }

        $scope.set_student_staus = function () {
            if ($scope.Student_status != "786") {
                angular.forEach($scope.studentlist, function (opq) {
                    opq.pamsT_Id = $scope.Student_status;
                    $scope.abc = opq.pamsT_Id;
                })
            }

        }

        //for Print
        $scope.printData = function (print_data) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();

            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        //for Export excel
        $scope.exportToExcel = function (tableId) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };

    }

})();