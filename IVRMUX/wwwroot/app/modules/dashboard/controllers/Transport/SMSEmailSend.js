(function () {
    'use strict';
    angular
        .module('app')
        .controller('SMSEmailSendController', SMSEmailSendController)

    SMSEmailSendController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function SMSEmailSendController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
        }
     

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.reporsmart = false;


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





        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.BindData = function () {
            apiService.getDATA("SMSEmailSend/getdata/2").then(function (promise) {
                if (promise != null) {
                    $scope.YearList = promise.yearList;
                    $scope.classlist = promise.classlist;
                    $scope.sectionlist = promise.sectionlist;
                }
                else {
                    swal("No Records Found")
                }
            })
        }
        //$scope.cnt12 = function ()
        //{
        //    
        //    if ($scope.count12 == true)
        //    {
        //        $scope.regorname_map = false;
        //    }
        //}

        $scope.onclickloaddata = function () {


            if ($scope.allorindiv == "All") {

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = true;
                $scope.reporsmart = false;

            }
            else if ($scope.allorindiv === "indi") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;

            }
            $scope.students = [];
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            $scope.printstudents = [];
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                //else {
                //    $scope.printstudents.splice(itm);
                //}
            });
            //if ($scope.students.length === 0 && $scope.printstudents.length > 0) {
            //    angular.forEach($scope.printstudents, function (itm) {
            //        $scope.printstudents.splice(itm);
            //    });
            //}
        }

        $scope.toggleAll1 = function () {
            var toggleStatus1 = $scope.all1;
            $scope.printstudents = [];
            angular.forEach($scope.countsts, function (itm) {
                itm.selected1 = toggleStatus1;
                if (itm.selected1 == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                //else {
                //    $scope.printstudents.splice(itm);
                //}
            });
            //if ($scope.countsts.length === 0 && $scope.printstudents.length > 0) {
            //    angular.forEach($scope.printstudents, function (itm) {
            //        $scope.printstudents.splice(itm);
            //    });
            //}
        }
        $scope.clearfields = function () {
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            //$scope.asmaY_Id = "";
            //$scope.asmcL_Id = "";
            //$scope.asmS_Id = "";
            $state.reload();
        }
      

        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.trmR_PickRouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.trmL_PickLocationName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.trmR_DropRouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.trmL_DropLocationName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.clear = function () {

            $scope.students = [];
        }
        $scope.getreport = function (obj) {

            $scope.all = "";
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
                var data =
                {
                    "asmaY_Id": $scope.asmaY_Id,
                    "asmcL_Id": $scope.asmcL_Id,
                    "asmS_Id": obj.asmS_Id,
                    "filterdata": $scope.filterdata
                }
                    apiService.create("SMSEmailSend/Getreportdetails", data).then(function (promise) {

                    if (promise.messagelist.length === 0) {
                        $scope.reporsmart = false;
                        swal("Record Not Found !!");
                        $state.reload();
                    }
                    else {

                        $scope.allandfalse = true;
                        $scope.counttrue = false;
                        $scope.students = promise.messagelist;
                        $scope.presentCountgrid = $scope.students.length;
                        $scope.exp_excel_flag = false;
                        $scope.print_flag = false;
                    }
                });
            }

            else {
                $scope.submitted = true;
            }
        };

     
        $scope.whatsAppsend = function () {
            $scope.albumNameArray = [];
            angular.forEach($scope.students, function (user) {
                if (!!user.selected) {
                    $scope.albumNameArray.push(user);
                }
            });
            if ($scope.albumNameArray.length > 0) {
                var datalist = {
                    "filterdata": $scope.filterdata,
                    data_array: $scope.albumNameArray
                };
                apiService.create("SMSEmailSend/whatsapp", datalist).then(function (promise) {

                    if (promise.success = "success") {
                        swal("Whatsapp Sent Successfully.");
                        $state.reload();
                    }
                    else {
                        swal("Something went wrong");
                        $state.reload();
                    }
                });
                //}
                //else {

                //    swal('Kindly select atleast one student..!');
                //    return;
                //}
            }
        };

        $scope.smssend = function () {
            $scope.albumNameArray = [];
            angular.forEach($scope.students, function (user) {
                if (!!user.selected) {
                    $scope.albumNameArray.push(user);
                }
            });
            if ($scope.albumNameArray.length > 0) {
                var datalist = {
                    "filterdata": $scope.filterdata,
                    data_array: $scope.albumNameArray
                };
                apiService.create("SMSEmailSend/smssend", datalist).then(function (promise) {

                    if (promise.success === "success") {
                        swal('SMS Sent  Successfully...!', 'success');
                        $state.reload();
                    }
                    else {
                        swal('Failed to Send SMS..!', 'Failure');
                        return;
                    }
                });
                //}
                //else {

                //    swal('Kindly select atleast one student..!');
                //    return;
                //}
            }
        };



        $scope.emailsend = function () {

            //if ($scope.myFormSM.$valid)
            //{
            $scope.albumNameArray1 = [];
            angular.forEach($scope.students, function (user) {
                if (!!user.selected) {
                    $scope.albumNameArray1.push(user);
                }
            });
            if ($scope.albumNameArray1.length > 0) {
                var datalist =
                {
                    "filterdata": $scope.filterdata,
                    data_array: $scope.albumNameArray1
                };
                apiService.create("SMSEmailSend/emailsend", datalist).then(function (promise) {

                    if (promise.success === "success") {
                        swal('Email Sent  Successfully...!', 'success');
                        $state.reload();
                    }
                    else {
                        swal('Failed to Send Email..!', 'Failure');
                        return;
                    }
                });
                //}
                //else {

                //    swal('Kindly select atleast one student..!');
                //    return;
                //}
            }
        };

        $scope.printstudents = [];
        $scope.printData = function () {

            if ($scope.printstudents !== null && $scope.students.length > 0) {
                var innerContents = "";
                innerContents = document.getElementById("printareaId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }

            else {
                swal("Please Select Records to be Printed");
            }
        };

        //$scope.exportToExcel = function () {

        //    var printSectionId = '#table1';
        //    if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
        //        var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');
        //        $timeout(function () { location.href = exportHref; }, 100);
        //        //$state.reload();
        //    }
        //    else {
        //        swal("Please Select Records to be Exported");
        //    }
        //};

        $scope.exportToExcel = function () {
            var excelnamemain = "Student Credentials";
            var printSectionId = '#table1';
            excelnamemain = excelnamemain + '-' + $scope.from_dateex + ' To ' + $scope.to_dateex + '.xls';
            var exportHref = Excel.tableToExcel(printSectionId, 'Student Credentials');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelnamemain;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };





        $scope.validreport = function () {
            if ($scope.regorname_map === "new") {

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = true;
                $scope.reporsmart = false;

            }
            else if ($scope.regorname_map === "regular") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;

            }
            else if ($scope.regorname_map === "both") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;

            }

            $scope.students = [];

        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
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
        $scope.saveUseroncewhatsapp = function () {

            $scope.otpmobile = false;
            $scope.otpemail = false;
            $scope.buttonotp = false;
            $scope.whatsAppdata = true;
            $scope.smsdata = false;
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
                angular.forEach($scope.students, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                })
                if ($scope.albumNameArray.length > 0) {

                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        $("#myModalotp").modal({ backdrop: false });
                        var emailidforotp = ivrmcofigsettings[0].ivrmgC_OTPMailId;
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






















        $scope.radioval = 'Student';
        $scope.saveUseronce = function () {

            $scope.otpmobile = false;
            $scope.otpemail = false;
            $scope.buttonotp = false;
            $scope.whatsAppdata = false;
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
                angular.forEach($scope.students, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                })
                if ($scope.albumNameArray.length > 0) {

                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        $("#myModalotp").modal({ backdrop: false });
                        var emailidforotp = ivrmcofigsettings[0].ivrmgC_OTPMailId;
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
                angular.forEach($scope.students, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                })
                if ($scope.albumNameArray.length > 0) {

                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        $("#myModalotp").modal({ backdrop: false });
                        var emailidforotp = ivrmcofigsettings[0].ivrmgC_OTPMailId;
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


    };

})();