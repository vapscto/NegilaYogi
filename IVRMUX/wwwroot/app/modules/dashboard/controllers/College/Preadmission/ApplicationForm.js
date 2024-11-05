(function () {
    'use strict';

    angular.module('app').controller('ApplicationFormController', ApplicationFormController);


    ApplicationFormController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile', 'Excel', '$timeout']
    function ApplicationFormController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile, Excel, $timeout) {


        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.zoomin = function () {


            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        }

        //$scope.scroll = function () {
        //    //$window.scrollTo(0, angular.element(document.getElementById('topdiv')).offsetTop);
        //     $window.scrollTo(0, 0);  
        //};

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        }

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        }
        var searchObject = $location.search();

        if (searchObject.status == "failure" || searchObject.status == "TXN_FAILURE") {
            swal("Payment Unsuccessfull");
            //  Request.QueryString.Remove("status");
            //$location.url($location.path)
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "success" || searchObject.status == "TXN_SUCCESS") {
            //swal("Payment successful and Please submit the filled application to the School office");
            swal("Payment Successfull..!!");
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Networkfailure") {
            swal('Network failure..!!', 'Try again after some time');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }

        var monthname;
        var datename;
        var yearname;

        $scope.editflg = false;
        $scope.search = "";
        $scope.PACA_Date = new Date();
        $scope.sortKey = 'PACA_Id';
        $scope.sortReverse = true;
        $scope.showpay = true;

        $scope.address = true;
        $scope.Parents = true;
        $scope.Others = true;
        $scope.DocumentUpload = true;
        $scope.PaymentMode = false;
        $scope.editflggard == false;

        var maxageeyear;
        var maxageemonth;
        var maxageedays;
        var minageeyear;
        var minageemonth;
        var minageedays;

        $scope.studentGuardianDetails = {};
        $scope.studentReferenceDetails = [];
        $scope.studentSourceDetails = [];
        $scope.studentActivityDetails = [];

        $scope.qwe = {};

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {

            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
            else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));
        $scope.configurationsettings = configsettings[0];

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag == "OnlineC") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

        $scope.calcageDOB = function (dateString) {

            var dobwords = new Date(dateString);
            var monthid = dobwords.getMonth();
            var dateid = dobwords.getDate();
            var yearid = dobwords.getFullYear();
            $scope.getmontnames(monthid);
            $scope.getdatenames(dateid);
            $scope.getyearname(yearid);

            var DOBWords = datename + ' ' + monthname + ' ' + yearname;
            $scope.obj.PACA_DOB_inwords = DOBWords;


            var now = new Date();
            var today = new Date(now.getYear(), now.getMonth(), now.getDate());

            var yearNow = now.getYear();
            var monthNow = now.getMonth();
            var dateNow = now.getDate();
            var doob = new Date(dateString);
            var dob = new Date(doob.getYear(6, 10),
                doob.getMonth(0, 2) - 1,
                doob.getDate(3, 5)
            );

            var yearDob = doob.getYear();
            var monthDob = doob.getMonth();
            var dateDob = doob.getDate();
            var age = {};
            var ageString = "";
            var yearString = "";
            var monthString = "";
            var dayString = "";

            var yearAge = "";
            yearAge = yearNow - yearDob;

            if (monthNow >= monthDob)
                var monthAge = monthNow - monthDob;
            else {
                yearAge--;
                var monthAge = 12 + monthNow - monthDob;
            }

            if (dateNow >= dateDob)
                var dateAge = dateNow - dateDob;
            else {
                monthAge--;
                var dateAge = 31 + dateNow - dateDob;

                if (monthAge < 0) {
                    monthAge = 11;
                    yearAge--;
                }
            }

            age = {
                years: yearAge

                // years: yearAge+"."+monthAge
                //months: monthAge,
                //days: dateAge
            };

            $scope.aggg = yearAge;
            $scope.obj.PACA_Age = $scope.aggg;
        }


        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;


        $scope.myDate = new Date();
        //calculating age , confirmation of age if there is any lessthan and greather than, and converting DOB in words
        $scope.calcage = function (dateString, SweetAlert) {

            angular.forEach($scope.allClass, function (c) {
                if (c.asmcL_Id == $scope.obj.asmcL_Id) {
                    $scope.asmcL_ClassName = c.asmcL_ClassName;
                }
            })

            $scope.classname11 = $scope.asmcL_ClassName;
            //converting date into words			
            var dobwords = new Date(dateString);
            var monthid = dobwords.getMonth();
            var dateid = dobwords.getDate();
            var yearid = dobwords.getFullYear();
            $scope.getmontnames(monthid);
            $scope.getdatenames(dateid);
            $scope.getyearname(yearid);
            $scope.obj.PACA_DOBin_words = datename + ' ' + monthname + ' ' + yearname;
            // $scope.dobwords = false;

            var now = new Date();
            var today = new Date(now.getYear(), now.getMonth(), now.getDate());

            var yearNow = now.getYear();
            var monthNow = now.getMonth();
            var dateNow = now.getDate();
            var doob = new Date(dateString);
            var dob = new Date(doob.getYear(6, 10),
                doob.getMonth(0, 2) - 1,
                doob.getDate(3, 5)
            );

            var yearDob = doob.getYear();
            var monthDob = doob.getMonth();
            var dateDob = doob.getDate();
            var age = {};
            var ageString = "";
            var yearString = "";
            var monthString = "";
            var dayString = "";

            var yearAge = "";
            yearAge = yearNow - yearDob;

            if (monthNow >= monthDob)
                var monthAge = monthNow - monthDob;
            else {
                yearAge--;
                var monthAge = 12 + monthNow - monthDob;
            }

            if (dateNow >= dateDob)
                var dateAge = dateNow - dateDob;
            else {
                monthAge--;
                var dateAge = 31 + dateNow - dateDob;

                if (monthAge < 0) {
                    monthAge = 11;
                    yearAge--;
                }
            }

            age = {
                years: yearAge,
                months: monthAge,
                days: dateAge
            };

            // swal(ageString);
            $scope.obj.PACA_Age = age.years;

            if ($scope.minage == 1) {
                if (age.years > minageeyear) {
                }
                else if (age.years == minageeyear) {
                    if (age.months > minageemonth) {

                    }
                    else if (age.months == minageemonth) {
                        if (age.days >= minageedays) {

                        }
                        else {

                            swal({
                                title: "Less than " + minageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                                text: "Do you want to continue !!",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                                cancelButtonText: "Cancel!!",
                                closeOnConfirm: true,
                                closeOnCancel: true
                            }, function (isConfirm) {
                                if (isConfirm) {

                                }
                                else {
                                    $scope.$apply(function () {
                                        $scope.obj.pasR_Age = "";
                                        $scope.obj.amsT_DOB = "";
                                        $scope.obj.amsT_DOB_Words = "";
                                    });
                                }
                            });
                        }
                    }
                    else {

                        swal({
                            title: "Less than " + minageeyear + " year ,You are not eligible for " + $scope.classname11 + " class",
                            text: "Do you want to continue !!",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                            cancelButtonText: "Cancel!!",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        }, function (isConfirm) {
                            if (isConfirm) {

                            }
                            else {
                                $scope.$apply(function () {
                                    $scope.obj.pasR_Age = "";
                                    $scope.obj.amsT_DOB = "";
                                    $scope.obj.amsT_DOB_Words = "";
                                });
                            }
                        });
                    }
                }
                else {

                    swal({
                        title: "Less than " + minageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                        text: "Do you want to continue !!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                        cancelButtonText: "Cancel!!",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {

                        }
                        else {
                            $scope.$apply(function () {
                                $scope.obj.pasR_Age = "";
                                $scope.obj.amsT_DOB = "";
                                $scope.obj.amsT_DOB_Words = "";
                            });
                        }
                    });
                }
            }

            if ($scope.maxage == 1) {
                if (age.years < maxageeyear) {
                }
                else if (age.years == maxageeyear) {
                    if (age.months < maxageemonth) {

                    }
                    else if (age.months == maxageemonth) {
                        if (age.days <= maxageedays) {

                        }
                        else {


                            swal({
                                title: "Greater than " + maxageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                                text: "Do you want to continue !!",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                                cancelButtonText: "Cancel!!",
                                closeOnConfirm: true,
                                closeOnCancel: true
                            }, function (isConfirm) {
                                if (isConfirm) {

                                }
                                else {
                                    $scope.$apply(function () {
                                        $scope.obj.pasR_Age = "";
                                        $scope.obj.amsT_DOB = "";
                                        $scope.obj.amsT_DOB_Words = "";
                                    });
                                }
                            });
                        }
                    }
                    else {


                        swal({
                            title: "Greater than " + maxageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                            text: "Do you want to continue !!",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                            cancelButtonText: "Cancel!!",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        }, function (isConfirm) {
                            if (isConfirm) {

                            }
                            else {
                                $scope.$apply(function () {
                                    $scope.obj.pasR_Age = "";
                                    $scope.obj.amsT_DOB = "";
                                    $scope.obj.amsT_DOB_Words = "";

                                });
                            }
                        });
                    }
                }
                else {


                    swal({
                        title: "Greater than " + maxageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                        text: "Do you want to continue !!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                        cancelButtonText: "Cancel!!",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {

                        }
                        else {
                            $scope.$apply(function () {
                                $scope.obj.pasR_Age = "";
                                $scope.obj.amsT_DOB = "";
                                $scope.obj.amsT_DOB_Words = "";
                            });
                        }
                    });
                }
            }
        };
        $scope.DOB = true;
        this.minDate = new Date();
        $scope.onYearCahnge = function (acdYId) {
            var data = {
                "ASMAY_Id": acdYId,
                "status_type": $scope.status_type
            };
            apiService.create("ApplicationForm/getCourse/", data).then(function (promise) {
                if (promise.courses !== null) {
                    $scope.courses = promise.courses;
                }
                else {
                    swal("No Course Is Mapped To Selected Academic Year");
                    $scope.courses = "";
                }
            });
        };
        //checking min and max age details
        $scope.onBranchchange = function (branchId) {

            var selectedData = $filter('filter')($scope.branches, { 'amB_Id': branchId });
            if (branchId != "") {
                var data = {
                    "AMB_Id": branchId,
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "ACAYCB_Id": selectedData[0].acaycB_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getSemester/", data).then(function (promise) {

                    if (promise.message == "MaxCapacity") {
                        swal("Sorry,Branch Capacity is Full");
                        // $scope.applastdatedisable = true;
                    }
                    else {
                        //  $scope.applastdatedisable = false;
                        //  if (promise.message != "" && promise.message != null) {
                        //      swal(promise.message);
                        //  }
                        //  else {
                        if (promise.semesters != null) {
                            $scope.semesters = promise.semesters;
                        }
                        else {
                            swal("No Semester Is Mapped To Selected Branch");
                            $scope.semesters = "";
                        }

                        // }
                    }
                    //if (flg == 0) {
                    $scope.DOB = false;
                    //}
                    //$scope.obj.AMCOC_Id = "";
                    //if (promise.studentCategory.length > 0) {
                    //    $scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                    //}
                    //else {
                    //    swal("To get Student Category.Please Map Selected Class to Some category");
                    //}


                    //maxageeyear = promise.fillclass[0].asmcL_MaxAgeYear;
                    //maxageemonth = promise.fillclass[0].asmcL_MaxAgeMonth;
                    //maxageedays = promise.fillclass[0].asmcL_MaxAgeDays;
                    //minageeyear = promise.fillclass[0].asmcL_MinAgeYear;
                    //minageemonth = promise.fillclass[0].asmcL_MinAgeMonth;
                    //minageedays = promise.fillclass[0].asmcL_MinAgeDays;
                    //if (flg == 0) {
                    //    if ($scope.maxage == 0 && $scope.minage == 0) {
                    //        $scope.maxDate1 = new Date(
                    //                    $scope.myDate.getFullYear(),
                    //                    $scope.myDate.getMonth(),
                    //                    $scope.myDate.getDate());
                    //    }
                    //    else if ($scope.maxage == 1 && $scope.minage == 0) {
                    //        $scope.maxDate1 = new Date(
                    //                    $scope.myDate.getFullYear(),
                    //                    $scope.myDate.getMonth(),
                    //                    $scope.myDate.getDate());
                    //    }
                    //    else if ($scope.maxage == 0 && $scope.minage == 1) {
                    //        $scope.maxDate1 = new Date(
                    //                    $scope.myDate.getFullYear(),
                    //                    $scope.myDate.getMonth(),
                    //                    $scope.myDate.getDate());
                    //    }
                    //    else if ($scope.maxage == 1 && $scope.minage == 1) {

                    //        $scope.maxDate1 = new Date(
                    //                    $scope.myDate.getFullYear(),
                    //                    $scope.myDate.getMonth(),
                    //                    $scope.myDate.getDate());
                    //    }
                    //}

                })

            }
        }

        $scope.onCourseChange = function (courseId) {

            var selectedData = $filter('filter')($scope.courses, { 'amcO_Id': courseId });
            if (selectedData != "") {
                var data = {
                    "AMCO_Id": courseId,
                    "ASMAY_Id": $scope.obj.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getBranch/", data).then(function (promise) {

                    if (promise.branches != null) {
                        $scope.branches = promise.branches;
                        $scope.obj.AMCOC_Id = "";
                        if (promise.studentCategory != null) {
                            $scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                        }
                        else {
                            swal("To get Student Category.Please Map Selected Course to Some category");
                        }
                    }
                    else {
                        swal("No Branch Is Mapped To Selected Course");
                        $scope.branches = "";
                    }
                })
            }
        }
        $scope.getQuotaCategory = function (quotaId) {
            if (quotaId > 0) {
                apiService.getURI("ApplicationForm/getQuotaCategory/", quotaId).then(function (promise) {
                    if (promise.quotaCategory != null) {
                        $scope.quotaCategory = promise.quotaCategory;
                    }
                    else {
                        swal("No Quota Category Is Mapped To Selected Quota");
                    }
                });
            }
        }
        $scope.myTabIndex = 0;

        $scope.obj = {};
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //student form validation
        $scope.compexammarksdetails = [];

        $scope.submitted1 = false;
        $scope.isduplicate = false;

        //address form validation
        $scope.submitted2 = false;
        $scope.previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }
        //parents form validation
        $scope.submitted3 = false;
        $scope.Parents_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }

        //others form validation
        $scope.submitted4 = false;
        $scope.validateOtherPrevious = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }

        $scope.previous_document = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {

            return $scope.submitted1;
        };

        $scope.interacted2 = function (field) {

            return $scope.submitted2;
        };
        $scope.interacted3 = function (field) {

            return $scope.submitted3;
        };
        $scope.interacted4 = function (field) {

            return $scope.submitted4;
        };
        $scope.interacted5 = function (field) {

            return $scope.submitted5;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.studentSiblingDetails = [{ id: 'sibling1' }];
        $scope.addNewsibling = function () {
            var newItemNo = $scope.studentSiblingDetails.length + 1;

            if (newItemNo <= 5) {
                $scope.studentSiblingDetails.push({ 'id': 'sibling' + newItemNo });
            }
        };

        $scope.removeNewsibling = function (index) {
            var newItemNo = $scope.studentSiblingDetails.length - 1;
            $scope.studentSiblingDetails.splice(index, 1);

            if ($scope.studentSiblingDetails.length == 0) {
            }
        };

        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.studentGuardianDetails.length + 1;

            if (newItemNo <= 5) {
                $scope.studentGuardianDetails.push({ 'id': 'Guardian' + newItemNo });
            }
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.studentGuardianDetails.length - 1;
            $scope.studentGuardianDetails.splice(index, 1);

            if ($scope.studentGuardianDetails.length == 0) {
            }
        };

        $scope.prevSchoolDetails = [{ id: 'prevSchool1' }];
        $scope.addNewsiblingprevsch = function () {
            var newItemNo = $scope.prevSchoolDetails.length + 1;

            if (newItemNo <= 5) {
                $scope.prevSchoolDetails.push({ 'id': 'prevSchool' + newItemNo });
            }
        };

        $scope.removeNewsiblingprevsch = function (index, data) {
            var newItemNo = $scope.prevSchoolDetails.length - 1;
            $scope.prevSchoolDetails.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.DeletePrevSchoolData(data);
            }


            if ($scope.prevSchoolDetails.length == 0) {
            }
        };

        $scope.TERESIANAPP = function () {

            var innerContents = document.getElementById("TeresianID1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Teresian/PreAdmission/Teresian_Admission_Pdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.paynowbefore = function (pacA_Id) {

            angular.forEach($scope.StudentList, function (item) {
                if (item.pacA_Id == pacA_Id) {
                    $scope.reg.PASR_FirstName = item.name;
                    $scope.reg.PASR_emailId = item.pacA_emailId;
                    $scope.reg.PASR_MobileNo = item.pacA_MobileNo;
                    $scope.pacA_Id = pacA_Id;
                }
            })
            $scope.PaymentMode = true;
            $scope.ProspectuseScreen = false;
            $scope.DocumentUpload = false;
            $scope.Others = false;
            $scope.Parents = false;
            $scope.address = false;
            //if ($scope.myTabIndex == 1) {
            $scope.myTabIndex = 5;
            //}
            swal.close();
            showConfirmButton: false
            $scope.scroll();
        }
        $scope.paynowbeforedit = function (pacA_Id) {

            angular.forEach($scope.StudentList, function (item) {
                if (item.pacA_Id == pacA_Id) {
                    $scope.reg.PASR_FirstName = item.name;
                    $scope.reg.PASR_emailId = item.pacA_emailId;
                    $scope.reg.PASR_MobileNo = item.pacA_MobileNo;
                    $scope.pacA_Id = pacA_Id;
                }
            })
        }

        //$scope.pamentsave = function () {

        //    var payu_URL = $scope.payu_URL;
        //    var url = payu_URL;
        //    var method = 'POST';
        //    var params = {
        //        "key": $scope.key,
        //        "txnid": $scope.txnid,
        //        "amount": $scope.amount,
        //        "productinfo": $scope.productinfo,
        //        "firstname": $scope.firstname,
        //        "email": $scope.email,
        //        "phone": $scope.phone,
        //        "udf1": $scope.udf1,
        //        "udf2": $scope.udf2,
        //        "udf3": $scope.udf3,
        //        "udf4": $scope.udf4,
        //        "udf5": $scope.udf5,
        //        "udf6": $scope.udf6,
        //        "service_provider": $scope.service_provider,
        //        "hash": $scope.hash,
        //        "surl": "http://localhost:57606/api/ApplicationForm/paymentresponse/",
        //        "furl": "http://localhost:57606/api/ApplicationForm/paymentresponse/"
        //    }
        //    FormSubmitter.submit(url, method, params);
        //}

        $scope.pamentsave = function (pacA_Id, offon) {
            var data = {
                "pacA_Id": $scope.pacA_Id,
                "offon": offon,
                configurationsettings: $scope.configurationsettings,
                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                "onlinepaygteway": pacA_Id.paygtw
            };
            apiService.create("ApplicationForm/paynow", data).then(function (promise) {

                if (promise.paydet != null) {
                    if (pacA_Id.paygtw === 'PAYTM') {
                        $scope.MID = promise.paydet[0].mid;
                        $scope.ORDER_ID = promise.paydet[0].ordeR_ID;
                        $scope.CUST_ID = promise.paydet[0].cusT_ID;
                        $scope.TXN_AMOUNT = promise.paydet[0].txN_AMOUNT;
                        $scope.CHANNEL_ID = promise.paydet[0].channeL_ID;
                        $scope.EMAIL = promise.paydet[0].email;
                        $scope.MOBILE_NO = promise.paydet[0].mobilE_NO;
                        $scope.INDUSTRY_TYPE_ID = promise.paydet[0].industrY_TYPE_ID;
                        $scope.WEBSITE = promise.paydet[0].website;
                        $scope.CHECKSUMHASH = promise.paydet[0].checksumhash;
                        $scope.MERC_UNQ_REF = promise.paydet[0].merC_UNQ_REF;

                        $scope.payu_URL = promise.paydet[0].payu_URL;
                        var payu_URL = $scope.payu_URL;
                        var url = payu_URL;
                        var method = 'POST';
                        var params = {
                            "MID": $scope.MID,
                            "ORDER_ID": $scope.ORDER_ID,
                            "CUST_ID": $scope.CUST_ID,
                            "TXN_AMOUNT": $scope.TXN_AMOUNT,
                            "CHANNEL_ID": $scope.CHANNEL_ID,
                            "EMAIL": $scope.EMAIL,
                            "MOBILE_NO": $scope.MOBILE_NO,
                            "INDUSTRY_TYPE_ID": $scope.INDUSTRY_TYPE_ID,
                            "WEBSITE": $scope.WEBSITE,
                            "CHECKSUMHASH": $scope.CHECKSUMHASH,
                            "MERC_UNQ_REF": $scope.MERC_UNQ_REF,
                            //"CALLBACK_URL": "https://secure.paytm.in/oltp-web/invoiceResponse",
                            // "CALLBACK_URL": "http://localhost:57606/api/ApplicationForm/paymentresponsepaytm/",
                            "CALLBACK_URL": "https://jnujaipur.vapssmartecampus.com/api/ApplicationForm/paymentresponsepaytm/",
                        }
                        FormSubmitter.submit($scope.payu_URL, method, params);
                    }
                    else if (pacA_Id.paygtw == "RAZORPAY") {
                        $scope.txnamt = Number(promise.paydet[0].fmA_Amount) * 100;
                        $scope.SaltKey = promise.paydet[0].ivrmoP_MERCHANT_KEY;
                        $scope.orderid = promise.paydet[0].trans_id;

                        $scope.institutioname = promise.paydet[0].institutioname;
                        $scope.institulogo = promise.paydet[0].institulogo;

                        $scope.stuname = promise.paydet[0].firstname;
                        $scope.stuemailid = promise.paydet[0].email;
                        $scope.stuaddress = promise.paydet[0].stuaddress;
                        $scope.stumobileno = promise.paydet[0].mobile;
                        $scope.stuadmno = promise.paydet[0].pacA_RegistrationNo;
                        $scope.splitpayinfor = promise.paydet[0].splitpayinformation;

                        $scope.mI_Id = promise.mI_Id;
                        $scope.asmaY_Id = promise.asmaY_Id;
                        $scope.pacA_Id = promise.paydet[0].pacA_ID;



                    }
                    if ($scope.myTabIndex == 1) {
                        $scope.myTabIndex = $scope.myTabIndex - 1;
                    }
                    swal.close();
                    showConfirmButton: false
                }
                else {
                    swal('Registered Successfully,But Payment gateway details are not mapped to institute', 'Contact Administrator..!!');
                    $state.reload();
                }

            });
        }

        $scope.paynowedit = function (pacA_Id) {

            var data = {
                "pacA_Id": pacA_Id,
                configurationsettings: $scope.configurationsettings,
                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
            }
            apiService.create("ApplicationForm/paynow", data).then(function (promise) {

                if (promise.paydet != null) {

                    $scope.reg.PASR_FirstName = promise.paydet[0].firstname;
                    $scope.reg.PASR_emailId = promise.paydet[0].email;
                    $scope.reg.PASR_MobileNo = promise.paydet[0].phone;
                    $scope.reg.Pasr_amount = promise.paydet[0].amount;

                    $scope.key = promise.paydet[0].marchanT_ID;
                    $scope.txnid = promise.paydet[0].trans_id;
                    $scope.amount = promise.paydet[0].amount;
                    $scope.productinfo = promise.paydet[0].productinfo;
                    $scope.firstname = promise.paydet[0].firstname;
                    $scope.email = promise.paydet[0].email;
                    $scope.phone = promise.paydet[0].phone;
                    $scope.surl = promise.paydet[0].surl;
                    $scope.furl = promise.paydet[0].furl;
                    $scope.hash = promise.paydet[0].hash;
                    $scope.udf1 = promise.paydet[0].udf1;
                    $scope.udf2 = promise.paydet[0].udf2;
                    $scope.udf3 = promise.paydet[0].udf3;
                    $scope.udf4 = promise.paydet[0].udf4;
                    $scope.udf5 = promise.paydet[0].udf5;
                    $scope.udf6 = promise.paydet[0].udf6;
                    $scope.service_provider = promise.paydet[0].service_provider;

                    $scope.hash_string = promise.paydet[0].hash_string;
                    $scope.payu_URL = promise.paydet[0].payu_URL;





                    $scope.PaymentMode = true;
                    $scope.ProspectuseScreen = false;
                    //if ($scope.myTabIndex == 1) {
                    $scope.myTabIndex = 0;
                    //}
                    swal.close();
                    showConfirmButton: false
                }
                else {
                    $scope.PaymentMode = false;
                    swal('Registered Successfully,But Payment gateway details are not mapped to institute', 'Contact Administrator..!!');
                    //$state.reload();
                }

            });
        }

        $scope.paynowsave = function (pacA_Id) {

            var data = {
                "pacA_Id": pacA_Id,
                configurationsettings: $scope.configurationsettings,
                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
            }
            apiService.create("ApplicationForm/paynow", data).then(function (promise) {

                if (promise.paydet != null) {

                    $scope.reg.PASR_FirstName = promise.paydet[0].firstname;
                    $scope.reg.PASR_emailId = promise.paydet[0].email;
                    $scope.reg.PASR_MobileNo = promise.paydet[0].phone;
                    $scope.reg.Pasr_amount = promise.paydet[0].amount;

                    $scope.key = promise.paydet[0].marchanT_ID;
                    $scope.txnid = promise.paydet[0].trans_id;
                    $scope.amount = promise.paydet[0].amount;
                    $scope.productinfo = promise.paydet[0].productinfo;
                    $scope.firstname = promise.paydet[0].firstname;
                    $scope.email = promise.paydet[0].email;
                    $scope.phone = promise.paydet[0].phone;
                    $scope.surl = promise.paydet[0].surl;
                    $scope.furl = promise.paydet[0].furl;
                    $scope.hash = promise.paydet[0].hash;
                    $scope.udf1 = promise.paydet[0].udf1;
                    $scope.udf2 = promise.paydet[0].udf2;
                    $scope.udf3 = promise.paydet[0].udf3;
                    $scope.udf4 = promise.paydet[0].udf4;
                    $scope.udf5 = promise.paydet[0].udf5;
                    $scope.udf6 = promise.paydet[0].udf6;
                    $scope.service_provider = promise.paydet[0].service_provider;

                    $scope.hash_string = promise.paydet[0].hash_string;
                    $scope.payu_URL = promise.paydet[0].payu_URL;

                    $scope.PaymentMode = true;
                    $scope.ProspectuseScreen = false;
                    //if ($scope.myTabIndex == 1) {
                    $scope.myTabIndex = 5;
                    //}
                    swal.close();
                    showConfirmButton: false
                }
                else {
                    $scope.PaymentMode = false;
                    swal('Registered Successfully,But Payment gateway details are not mapped to institute', 'Contact Administrator..!!');
                    //$state.reload();
                }

            });
        }

        //payment gateway
        $scope.clickimage = function (radioimage) {
            angular.forEach($scope.paymenttest, function (objectt) {
                if (objectt.fpgD_Image == radioimage) {
                    $scope.qwe.paygtw = objectt.fpgD_PGName;
                    $scope.onclickloaddata(objectt.fpgD_PGName, $scope.result);
                    return;
                }
            });
        };

        $scope.onclickloaddata = function (paymentgetway, result) {

            $scope.nextpayment = false;
            //if ($scope.reg.PASR_UndertakingFlag == 1 || $scope.reg.PASR_UndertakingFlag == true) {
            //    $scope.applastdatedisable = false;
            //}
            //else {
            //    $scope.applastdatedisable = true;
            //}

            if (paymentgetway == "RAZORPAY") {
                $scope.pamentsave($scope.qwe, result);
            }
            if (paymentgetway == "PAYU") {
                $scope.pamentsave($scope.qwe, result);
            }
            if (paymentgetway == "PAYTM") {
                $scope.pamentsave($scope.qwe, result);
            }
        }
        //

        $scope.Back = function () {

            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;

            // $scope.cancel();
            $state.reload();


        }


        $scope.allActivity = [];
        $scope.allSources = [];
        $scope.allRefrence = [];
        $scope.RegistrationNumbering = [];
        $scope.AdmissionNumbering = [];
        $scope.govtBondList = [];
        //Get data Initially
        $scope.status_type = "";
        $scope.currentPage2 = 1;
        $scope.BindData = function () {
            var data = {
                "status_type": $scope.status_type
            }
            apiService.create("ApplicationForm/Getdetails/", data).
                then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.admin = promise.countrole;
                    $scope.itemsPerPage = paginationformasters;
                    $scope.statelabel = true;
                    $scope.statelabel2 = true;
                    $scope.gender = "Female";
                    // $scope.obj.PACA_Sex = "Female";
                    $scope.allAcademicYear = promise.allAcademicYear;
                    $scope.allAcademicYear1 = promise.allAcademicYear;

                    $scope.allAcademicYearsearch = promise.academicYearOnLoad;
                    $scope.allAcademicYear1search = promise.allAcademicYear;

                    //previous  last year
                    $scope.previouspassyear = promise.academicYearOnLoad;

                    $scope.configurationsettings = configsettings[0];



                    for (var i = 0; i < $scope.allAcademicYear.length; i++) {
                        name = $scope.allAcademicYear[i].asmaY_Id;
                        for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                            if (name == $scope.allAcademicYear1[j].asmaY_Id) {
                                $scope.allAcademicYear[i].Selected = true;
                                $scope.obj.ASMAY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                                $scope.yearId = $scope.allAcademicYear1[j].asmaY_Id;
                            }
                        }
                    }
                    //  $scope.yearre = promise.academicyearforreadmit;

                    //for (var i = 0; i < $scope.allAcademicYearsearch.length; i++) {
                    //    name = $scope.allAcademicYearsearch[i].acmaY_Id;
                    //    for (var j = 0; j < $scope.allAcademicYear1search.length; j++) {
                    //        if (name == $scope.allAcademicYear1search[j].acmaY_Id) {
                    //            $scope.allAcademicYearsearch[i].Selected = true;
                    //            $scope.obj.asmaY_Idsearch = $scope.allAcademicYear1search[j].acmaY_Id;

                    //        }
                    //    }
                    //}
                    //$scope.branches = promise.branches;
                    //$scope.semesters = promise.semesters;
                    $scope.courses = promise.courses;




                    //previous class
                    // $scope.branches = promise.branches;
                    // $scope.allSection = promise.allSection;
                    $scope.allcountry = promise.allCountry;
                    //country code
                    $scope.allcountrycode = promise.allCountrycode;
                    //previous country
                    $scope.allcountry1 = promise.allCountry;
                    $scope.allReligion = promise.allReligion;
                    $scope.allReligionfather = promise.allReligion;
                    $scope.allReligionmother = promise.allReligion;
                    $scope.allcasteCategory = promise.allcasteCategory;
                    $scope.allCaste = promise.allCaste;
                    $scope.allCastefather = promise.allCaste;
                    $scope.allCastemother = promise.allCaste;
                    $scope.allRefrence = promise.allRefrence;
                    $scope.allSources = promise.allSources;
                    $scope.batches = promise.batches;
                    $scope.quotas = promise.quotas;
                    $scope.quotaCategory = promise.quotaCategory;
                    $scope.subjectScheme = promise.subjectScheme;
                    $scope.schemeType = promise.schemeType;
                    $scope.StudentList = promise.studentList;
                    $scope.presentCountgrid = $scope.StudentList.length;
                    if ($scope.StudentList != null || $scope.StudentList.length > 0) {
                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < $scope.StudentList.length; i++) {
                            if ($scope.StudentList[i].pacA_FirstName != '') {
                                if ($scope.StudentList[i].pacA_MiddleName !== null) {
                                    if ($scope.StudentList[i].pacA_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: $scope.StudentList[i].pacA_FirstName + " " + $scope.StudentList[i].pacA_MiddleName + " " + $scope.StudentList[i].pacA_LastName, pacA_Id: $scope.StudentList[i].pacA_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.StudentList[i].pacA_FirstName + " " + $scope.StudentList[i].pacA_MiddleName, pacA_Id: $scope.StudentList[i].pacA_Id });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: $scope.StudentList[i].pacA_FirstName, pacA_Id: $scope.StudentList[i].pacA_Id });
                                }

                                $scope.StudentList[i].name = $scope.albumNameArray1[i].name;
                            }
                        }
                    }
                    //$scope.allActivity = promise.allActivity;
                    $scope.studentCategory = promise.studentCategory;

                    $scope.allState = promise.allState_new;


                    $scope.siblingCourse = promise.courses;


                    $scope.compExam = promise.compExamarray;
                    if ($scope.compExam != null && $scope.compExam.length > 0) {
                        $scope.compexamrequired = $scope.compExam[0].pamcexM_CompulsoryFlg;
                    }


                    //document list

                    //$scope.documentList = promise.documentList;
                    //angular.forEach($scope.documentList, function (value1, i) {
                    //    $scope.documentList[i].document_Path = "";
                    //});

                    $scope.documentList = promise.documentList;
                    angular.forEach($scope.documentList, function (value1, i1) {
                        $scope.documentList[i1].document_Path = "";
                    });


                    var transnumconfig = promise.admTransNumSetting;
                    localStorage.setItem("transnumconfigsettings", JSON.stringify(transnumconfig));
                    var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

                    for (var i = 0; i < transnumconfigsettings.length; i++) {
                        if (transnumconfigsettings.length > 0) {
                            $scope.transnumbconfigurationsettings = transnumconfigsettings[i];

                            if (transnumconfigsettings[i].imN_Flag == "FormC") {
                                $scope.RegistrationNumbering = transnumconfigsettings[i];
                                //if (transnumconfigsettings[i].imN_AutoManualFlag == "Manual") {
                                //    $scope.reg_ = true;
                                //    $scope.regpreventduplicateflag = transnumconfigsettings[i].imN_DuplicatesFlag;
                                //}
                                //else {
                                //    $scope.reg_ = false;
                                //}
                            }
                            if (transnumconfigsettings[i].imN_Flag == "OnlineC") {
                                $scope.AdmissionNumbering = transnumconfigsettings[i];
                                //if (transnumconfigsettings[i].imN_AutoManualFlag == "Manual") {
                                //    $scope.Adm_ = true;
                                //    $scope.admpreventduplicateflag = transnumconfigsettings[i].imN_DuplicatesFlag;
                                //}
                                //else {
                                //    $scope.Adm_ = false;
                                //}
                            }
                        }
                    }



                    if (promise.boardList != null && promise.boardList != "") {
                        $scope.boardList = promise.boardList;
                    }


                    $scope.Schooltypelist = promise.schooltypelist;

                    $scope.prevSchlCls = promise.courses;

                    $scope.prevSchlCls = promise.combinationlist;

                    //Previous School Year.
                    $scope.prevYr = promise.academicYearOnLoad;

                    //Previous School Country.
                    $scope.prevCountry = promise.allCountry;

                    //payment gateway 
                    if (promise.fillpaymentgateway != null && promise.fillpaymentgateway.length > 0) {
                        $scope.paymenttest = promise.fillpaymentgateway;
                    }
                    //


                    if ($scope.configurationsettings.ispaC_ApplFeeFlag === 1) {

                        $scope.ispaC_ApplFeeFlag = $scope.configurationsettings.ispaC_ApplFeeFlag;
                        $scope.prosH = true;
                        $scope.prosL = true;
                    }
                    else {
                        $scope.ispaC_ApplFeeFlag = 0;
                        $scope.prosH = true;
                        $scope.prosL = true;
                    }
                    $scope.prospectusPaymentlist = promise.prospectusPaymentlist;

                    if ($scope.configurationsettings.ispaC_ApplFeeFlag === 1) {
                        if ($scope.prospectusPaymentlist.length > 0) {
                            for (var i = 0; i < $scope.StudentList.length; i++) {
                                for (var j = 0; j < $scope.prospectusPaymentlist.length; j++) {
                                    if ($scope.StudentList[i].pacA_Id == $scope.prospectusPaymentlist[j].pacA_Id && $scope.ispaC_ApplFeeFlag == 1) {
                                        $scope.StudentList[i].viewflag = true;
                                        $scope.StudentList[i].download = false;
                                        break;
                                    }
                                    else if ($scope.StudentList[i].pacA_Id != $scope.prospectusPaymentlist[j].pacA_Id && $scope.ispaC_ApplFeeFlag == 1) {
                                        $scope.StudentList[i].viewflag = false;
                                        $scope.StudentList[i].download = true;

                                    }
                                }
                            }

                        }

                        else if ($scope.prospectusPaymentlist.length == 0) {

                            for (var i = 0; i < $scope.StudentList.length; i++) {
                                if ($scope.ispaC_ApplFeeFlag == 1) {
                                    $scope.StudentList[i].viewflag = false;
                                    $scope.StudentList[i].download = true;

                                }
                                else if ($scope.ispaC_ApplFeeFlag == 0) {


                                    $scope.StudentList[i].viewflag = true;
                                    $scope.StudentList[i].download = false;

                                }
                            }
                        }
                    }
                    else {
                        for (var i = 0; i < $scope.StudentList.length; i++) {
                            if ($scope.ispaC_ApplFeeFlag == 1) {
                                $scope.StudentList[i].viewflag = false;
                                $scope.StudentList[i].download = true;

                            }
                            else if ($scope.ispaC_ApplFeeFlag == 0) {


                                $scope.StudentList[i].viewflag = true;
                                $scope.StudentList[i].download = false;

                            }
                        }
                    }


                    var admissioncongiguration = promise.studentcurrenrtbranch;
                    localStorage.setItem("admissionconfigsettings", JSON.stringify(admissioncongiguration));

                    var admissionSettings = JSON.parse(localStorage.getItem("admissionconfigsettings"));

                    if (admissionSettings !== null && admissionSettings !== "") {
                        if (admissionSettings.length > 0) {

                            $scope.photo = admissionSettings[0].asC_DefaultPhotoUpload;
                        }
                    }
                    else {

                        $scope.photo = 0;
                    }
                    if (parseInt($scope.photo) === 1) {
                        $scope.profile_photo = 1;
                    }
                    else {
                        $scope.profile_photo = 2;
                    }


                })
        };

        $scope.exportToExcel = function () {
            var printSectionId = '#table1';
            if ($scope.StudentList !== null && $scope.StudentList.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'APPLICATIONS LIST');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = "APPLICATIONS LIST.xls";
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };


        //get Student Category list
        $scope.GetStudentCategory = function (classId) {
            if (classId != "" && classId != null && classId != undefined) {
            }
        }

        $scope.DeleteBondData = function (DeleteRecord, SweetAlert) {

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.deleteId = DeleteRecord.amstB_Id;
                        var MdeleteId = $scope.deleteId;
                        apiService.DeleteURI("StudentAdmission/DeleteBondEntry", MdeleteId)

                        swal("Record Deleted Successfully");
                        $state.reload();
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }

        $scope.DeletePrevSchoolData = function (DeleteRecord) {
            var confirmPopup = confirm('Are you sure you want to Delete this item?');
            if (confirmPopup === true) {

                if (DeleteRecord.amstB_Id > 0) {
                    $scope.deleteId = DeleteRecord.amstB_Id;
                    var MdeleteId = $scope.deleteId;
                    apiService.DeleteURI("StudentAdmission/DeletePrevSchoolEntry", MdeleteId)
                    $scope.$apply();
                    swal("Record Deleted Successfully");

                    $state.reload();
                }
                else {
                    $scope.bondList.splice($scope.bondList.indexOf(DeleteRecord), 1);
                }

            }

        };


        // Previous Qualify Exam Marks details

        $scope.prevexammarksdetails = [{ id: 'prevExamdetails1' }];
        $scope.addNewsiblingprevschexamdetails = function () {
            var newItemNo = $scope.prevexammarksdetails.length + 1;

            if (newItemNo <= 20) {
                $scope.prevexammarksdetails.push({ 'id': 'prevExamdetails' + newItemNo });
            }
        };

        $scope.removeNewsiblingprevschexamdetails = function (index, data) {
            var newItemNo = $scope.prevexammarksdetails.length - 1;
            $scope.prevexammarksdetails.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.DeletePrevSchoolData(data);
            }


            if ($scope.prevexammarksdetails.length == 0) {
            }
        };

        // End 

        // Competitive Exam Marks details
        $scope.maxmarkschange = function (compExamStudetails) {
            if (PACSTCEM_ObtdMarks.PACSTCEM_TotalMaxMarks <= compExamStudetails.PACSTCEM_ObtdMarks) {

            }
            else {
                swal("Obtained marks should be less than or equal to Total marks..")
            }
        };
        $scope.maxmarksvalidate = function (compExamdetails) {

            $scope.maxmars = [];
            $scope.maxmars = compExamdetails.compSubList;

            angular.forEach($scope.maxmars, function (dd) {

                $scope.maxmarksrows = dd.pamcexmsuB_MaxMarks;
            })

            if (compExamdetails.pacstcemS_SubjectMarks <= $scope.maxmarksrows) {

            }
            else {
                swal("Subject marks should be less than or equal to Maximum marks..");
            }
        }

        $scope.tempidlist = [];
        $scope.compexammarksdetails = [{ id: 'compExamdetails1' }];
        $scope.addNewsiblingcompexamdetails = function (compExamdetails) {
            $scope.maxmar = [];
            $scope.maxmar = compExamdetails.compSubList;

            angular.forEach($scope.maxmar, function (dd) {

                $scope.maxmarksrow = dd.pamcexmsuB_MaxMarks;
            })

            if (compExamdetails.pacstcemS_SubjectMarks <= $scope.maxmarksrow) {
                var newItemNo = $scope.compexammarksdetails.length + 1;
                //

                if (newItemNo <= 20) {
                    $scope.compexammarksdetails.push({ 'id': 'compExamdetails' + newItemNo });
                }
                $scope.sublisttemp = [];

                if ($scope.sublisttemp.length == 1) {
                    if ($scope.compexammarksdetails[0].pamcexmsuB_Id != null) {
                        // $scope.tempidlist = $scope.compexammarksdetails[0].pamcexmsuB_Id;
                        $scope.tempidlist.push({
                            pamcexmsuB_Id: compExamdetails.pamcexmsuB_Id
                        })
                        angular.forEach($scope.compexammarksdetails, function (dd) {
                            if ($scope.compexammarksdetails[0].pamcexmsuB_Id == dd.pamcexmsuB_Id) {
                                $scope.sublisttemp.push({
                                    pamcexmsuB_Id: $scope.compexammarksdetails[0].pamcexmsuB_Id
                                });
                            }

                        })

                    }
                }
                else {
                    angular.forEach($scope.sublisttemp, function (dd) {
                        if ($scope.compexammarksdetails[0].pamcexmsuB_Id == dd.pamcexmsuB_Id) {
                            swal("Already selected this subject");
                        }
                        else {
                            if ($scope.compexammarksdetails[0].pamcexmsuB_Id != null) {

                                angular.forEach($scope.compexammarksdetails, function (dd) {
                                    if ($scope.compexammarksdetails[0].pamcexmsuB_Id == dd.pamcexmsuB_Id) {
                                        $scope.sublisttemp.push({
                                            pamcexmsuB_Id: $scope.compexammarksdetails[0].pamcexmsuB_Id
                                        });
                                    }

                                })

                            }
                        }
                    })

                }

                // $scope.obj.tempidlist = $scope.compexammarksdetails[0].pamcexmsuB_Id;
                $scope.tempidlist.push({
                    pamcexmsuB_Id: compExamdetails.pamcexmsuB_Id
                })
            }
            else {
                swal("Subject marks should be less than or equal to Maximum marks..")
            }

        };

        $scope.removeNewsiblingcompexamdetails = function (index, data) {
            var newItemNo = $scope.compexammarksdetails.length - 1;
            $scope.compexammarksdetails.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.DeletePrevSchoolData(data);
            }


            if ($scope.compexammarksdetails.length == 0) {

            }
        };


        $scope.validttlmarks = function (PACSTCEM_ObtdMarks, PACSTCEM_TotalMaxMarks) {

            if (Number(PACSTCEM_TotalMaxMarks) <= Number(PACSTCEM_ObtdMarks)) {
            }
            else {
                swal("Obtained marks should be less than or equal to Total marks..")
            }
        };
        $scope.compexamstudetails = [{ id: 'compExamStudetails1' }];
        $scope.addNewcompexamstudetails = function (compExamStudetails) {
            if (compExamStudetails.PACSTCEM_TotalMaxMarks <= compExamStudetails.PACSTCEM_ObtdMarks) {
                var newItemNo = $scope.compexammarksdetails.length + 1;

                if (newItemNo <= 20) {
                    $scope.compexamstudetails.push({ 'id': 'compExamStudetails' + newItemNo });
                }
            }
            else {
                swal("Obtained marks should be less than or equal to Total marks..")
            }


        };

        $scope.removeNewcompexamstudetails = function (index, data) {
            var newItemNo = $scope.compexamstudetails.length - 1;
            $scope.compexamstudetails.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.DeletePrevSchoolData(data);
            }


            if ($scope.compexammarksdetails.length == 0) {
            }
        };

        // End 


        $scope.Deletedata = function (DeleteRecord, SweetAlert) {

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.deleteId = DeleteRecord.PACA_Id;
                        var MdeleteId = $scope.deleteId;

                        var data = {
                            "PACA_Id": MdeleteId
                        };

                        apiService.create("ApplicationForm/DeleteEntry/", data).then(function (promise) {
                            if (promise.message != "" && promise.message != null) {
                                return swal(promise.message);
                            }
                            else {
                                swal("Failed To Delete Record");
                            }

                        });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }







        $scope.EditRecord = [];


        $scope.Editdata = function (EditRecord, allRefrence, allSources, allActivity) {

            $scope.disableinstname = true;
            $scope.DocumentUpload = true;
            $scope.Others = true;
            $scope.Parents = true;
            $scope.address = true;
            $scope.disablecourse = false;
            $scope.EditId = EditRecord.pacA_Id;
            var MEditId = $scope.EditId;
            $scope.castedisble = false;
            $scope.obj.AMCOC_Id = "";


            apiService.getURI("ApplicationForm/Edit/", MEditId).
                then(function (promise) {
                    $scope.PaymentMode = true;
                    $scope.DocumentUpload = true;
                    $scope.Others = true;
                    $scope.Parents = true;
                    $scope.address = true;
                    $scope.myTabIndex = 0;

                    if (promise.studentList != null && promise.studentList.length > 0) {


                        //  $scope.compExam = promise.compExamarray;

                        $scope.roleName = promise.roleName;
                        $scope.branches = promise.branches;
                        $scope.semesters = promise.semesters;
                        $scope.baranchid = promise.studentList[0].amB_Id;
                        $scope.semid = promise.studentList[0].amsE_Id;

                        if (promise.studentList[0].pacA_CompleteFillflag == 1) {
                            $scope.address = false;
                            $scope.PaymentMode = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            swal("Enter Permanent Address And Communication Address!!");
                            $scope.scroll();
                        }
                        if (promise.studentList[0].pacA_CompleteFillflag == 2) {
                            $scope.Parents = false;
                            $scope.address = false;
                            $scope.PaymentMode = false;
                            $scope.myTabIndex = $scope.myTabIndex + 2;
                            swal("Enter Father's Details And Mother's Details!!", "Else Skip & Continue");
                            $scope.scroll();
                        }
                        if (promise.studentList[0].pacA_CompleteFillflag == 3) {
                            $scope.Others = false;
                            $scope.Parents = false;
                            $scope.address = false;
                            $scope.PaymentMode = false;
                            $scope.myTabIndex = $scope.myTabIndex + 3;
                            swal("Enter Previous college details!!", "Else Skip & Continue");
                            $scope.scroll();
                        }
                        if (promise.studentList[0].pacA_CompleteFillflag == 4) {
                            $scope.DocumentUpload = false;
                            $scope.Others = false;
                            $scope.Parents = false;
                            $scope.address = false;
                            $scope.PaymentMode = false;
                            $scope.myTabIndex = $scope.myTabIndex + 4;
                            swal("Enter Guardian details & Passport details!!", "Else Submit Application");
                            $scope.scroll();
                        }


                        if (promise.studentList[0].pacA_CompleteFillflag == 5 && promise.pay != null) {
                            $scope.PaymentMode = true;
                            $scope.DocumentUpload = false;
                            $scope.Others = false;
                            $scope.Parents = false;
                            $scope.address = false;
                            $scope.paynowbeforedit(promise.studentList[0].pacA_Id);
                            $scope.myTabIndex = $scope.myTabIndex + 5;
                            swal("Pay Application fee!!");
                            $scope.scroll();
                        }
                        else if (promise.studentList[0].pacA_CompleteFillflag == 5) {
                            $scope.PaymentMode = false;
                            $scope.DocumentUpload = false;
                            $scope.Others = false;
                            $scope.Parents = false;
                            $scope.address = false;
                            $scope.myTabIndex = 0;
                        }

                        if (promise.pay == null && promise.roleName == 'OnlinePreadmissionUser') {
                            $scope.disablecourse = true;
                        }

                        $scope.documentList = promise.documentList;
                        $scope.DOB = false;
                        $scope.mi_id = promise.mI_Id;


                        if (promise.prevSchoolDetails != null && promise.prevSchoolDetails.length > 0) {
                            if (promise.prevSchoolDetails.length > 0) {

                                $scope.prevSchoolDetails = promise.prevSchoolDetails;
                                $scope.prevschlcount = promise.prevSchoolDetails.length;

                                for (var i = 0; i < promise.prevSchoolDetails.length; i++) {
                                    $scope.prevSchoolDetails[i].pacstpS_PreSchoolBoard = promise.prevSchoolDetails[i].pacstpS_PreSchoolBoard;
                                    $scope.prevSchoolDetails[i].pacstpS_PreSchoolType = promise.prevSchoolDetails[i].pacstpS_PreSchoolType;
                                    $scope.prevSchoolDetails[i].pacstpS_PreviousClass = promise.prevSchoolDetails[i].pacstpS_PreviousClass;
                                    $scope.prevSchoolDetails[i].pacstpS_PreviousTCNo = promise.prevSchoolDetails[i].pacstpS_PreviousTCNo;
                                    $scope.prevSchoolDetails[i].pacstpS_PreviousRegNo = promise.prevSchoolDetails[i].pacstpS_PreviousRegNo;
                                    $scope.prevSchoolDetails[i].pacstpS_PreviousBranch = promise.prevSchoolDetails[i].pacstpS_PreviousBranch;
                                    $scope.prevSchoolDetails[i].pacstpS_MediumOfInst = promise.prevSchoolDetails[i].pacstpS_MediumOfInst;
                                    $scope.prevSchoolDetails[i].pacstpS_PasssedMonthYear = promise.prevSchoolDetails[i].pacstpS_PasssedMonthYear;
                                    $scope.prevSchoolDetails[i].pacstpS_LanguagesTaken = promise.prevSchoolDetails[i].pacstpS_LanguagesTaken;
                                    $scope.prevSchoolDetails[i].pacstpS_TCDate = new Date(promise.prevSchoolDetails[i].pacstpS_TCDate);
                                    $scope.prevSchoolDetails[i].pacstpS_LeftYear = promise.prevSchoolDetails[i].pacstpS_LeftYear;
                                    $scope.prevSchoolDetails[i].pacstpS_PreSchoolCountry = promise.prevSchoolDetails[i].pacstpS_PreSchoolCountry;
                                    //  getPrevGetState(promise.prevSchoolDetails[i].pacstpS_PreSchoolCountry, promise.prevSchoolDetails[i].pacstpS_PreSchoolState);
                                    $scope.onselectprevCountry($scope.prevSchoolDetails[i].pacstpS_PreSchoolCountry);
                                    $scope.prevSchoolDetails[i].pacstpS_PreSchoolState = promise.prevSchoolDetails[i].pacstpS_PreSchoolState;
                                }
                            }
                        }
                        else {
                            $scope.prevSchoolDetails = {};
                            $scope.prevSchoolDetails = [{ id: 'prevSchool1' }];
                            $scope.prevschlcount = 0;
                        }
                        if (promise.studentGuardianDetails != null && promise.studentGuardianDetails.length > 0) {
                            $scope.editflggard = true;
                            if (promise.studentGuardianDetails.length > 0) {
                                $scope.studentGuardianDetails = promise.studentGuardianDetails;
                                $scope.grddetcount = promise.studentGuardianDetails.length;
                            }
                        }

                        else {
                            $scope.studentGuardianDetails = {};
                            $scope.studentGuardianDetails = [{ id: 'Guardian1' }];
                            $scope.grddetcount = 0;
                        }

                        //documnets
                        if (promise.documentList != null && promise.documentList.length > 0) {
                            if (promise.documentList.length > 0) {
                                $scope.document = {};
                                $scope.documentList = promise.documentList;
                                angular.forEach(promise.documentList, function (value, key) {
                                    $('#' + value.amsmD_Id).attr('src', value.document_Path);
                                })
                            }
                        }

                        if (promise.adm_College_Student_SubjectMarksDTO != null && promise.adm_College_Student_SubjectMarksDTO.length > 0) {
                            if (promise.adm_College_Student_SubjectMarksDTO.length > 0) {

                                $scope.prevexammarksdetails = promise.adm_College_Student_SubjectMarksDTO;
                                $scope.prevexammarksdetailscount = promise.adm_College_Student_SubjectMarksDTO.length;
                            }
                        }
                        else {
                            $scope.prevexammarksdetails = {};
                            $scope.prevexammarksdetails = [{ id: 'prevExamdetails1' }];
                            $scope.prevexammarksdetailscount = 0;
                        }


                        $('#blah').attr('src', promise.studentList[0].pacA_StudentPhoto);



                        $scope.fatherphoto = promise.studentList[0].pacA_FatherPhoto;
                        $scope.fatherSign = promise.studentList[0].pacA_FatherSign;
                        $scope.fatherFingerprint = promise.studentList[0].pacA_FatherFingerprint;
                        $scope.motherphoto = promise.studentList[0].pacA_MotherPhoto;
                        $scope.mothersign = promise.studentList[0].pacA_MotherSign;
                        $scope.motherfingerprint = promise.studentList[0].pacA_MotherFingerprint;
                        $scope.obj.image = promise.studentList[0].pacA_StudentPhoto;

                        $scope.obj.PACA_FirstName = promise.studentList[0].pacA_FirstName;


                        $scope.obj.PACA_MiddleName = promise.studentList[0].pacA_MiddleName;
                        $scope.obj.PACA_LastName = promise.studentList[0].pacA_LastName;
                        $scope.obj.PACA_Date = new Date(promise.studentList[0].pacA_Date);
                        $scope.obj.PACA_RegistrationNo = promise.studentList[0].pacA_RegistrationNo;
                        $scope.obj.PACA_AdmNo = promise.studentList[0].pacA_AdmNo;
                        $scope.obj.ASMAY_Id = promise.studentList[0].asmaY_Id;
                        $scope.obj.AMCO_Id = promise.studentList[0].amcO_Id;
                        $scope.obj.AMB_Id = promise.studentList[0].amB_Id;
                        $scope.obj.AMSE_Id = promise.studentList[0].amsE_Id;
                        $scope.obj.ACMB_Id = promise.studentList[0].acmB_Id;
                        $scope.obj.ACSS_Id = promise.studentList[0].acsS_Id;
                        $scope.obj.ACST_Id = promise.studentList[0].acsT_Id;
                        if (promise.studentCategory.length > 0) {
                            $scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                        }
                        $scope.obj.PACA_Sex = promise.studentList[0].pacA_Sex;
                        $scope.obj.PACA_DOB = new Date(promise.studentList[0].pacA_DOB);
                        $scope.obj.PACA_DOB_inwords = promise.studentList[0].pacA_DOB_inwords;
                        $scope.obj.PACA_Age = promise.studentList[0].pacA_Age;
                        $scope.obj.PACA_BloodGroup = promise.studentList[0].pacA_BloodGroup;
                        $scope.obj.PACA_MotherTongue = promise.studentList[0].pacA_MotherTongue;
                        $scope.obj.PACA_BirthCertNo = promise.studentList[0].pacA_BirthCertNo;
                        $scope.obj.IVRMMR_Id = promise.studentList[0].ivrmmR_Id;
                        $scope.obj.PACA_StudentSubCaste = promise.studentList[0].pacA_StudentSubCaste;
                        $scope.obj.PACA_TPINNO = promise.studentList[0].pacA_TPINNO;
                        $scope.obj.PACA_AlterNativeEmialId = promise.studentList[0].pacA_AlterNativeEmialId;
                        $scope.obj.PACA_CoutryCode = promise.studentList[0].pacA_CoutryCode;

                        $scope.obj.PACA_Village = promise.studentList[0].pacA_Village;
                        $scope.obj.PACA_Taluk = promise.studentList[0].pacA_Taluk;
                        $scope.obj.PACA_District = promise.studentList[0].pacA_District;
                        $scope.obj.PACA_Urban_Rural = promise.studentList[0].pacA_Urban_Rural;

                        $scope.obj.IMCC_Id = promise.studentList[0].imcC_Id;
                        for (var i = 0; i < $scope.allCaste.length; i++) {
                            $scope.allCaste[i].Selected = false;
                            $scope.obj.IMC_Id = "";
                        }


                        if (promise.allCaste.length > 0) {
                            for (var i = 0; i < promise.allCaste.length; i++) {
                                if (promise.studentList[0].imC_Id == promise.allCaste[i].imC_Id) {
                                    $scope.allCaste[i].Selected = true;
                                    $scope.obj.IMC_Id = promise.studentList[0].imC_Id;
                                }
                            }
                        }
                        else {
                            swal("Something has gone wrong.Please check Master Caste Category and Master Caste");
                        }




                        $scope.obj.PACA_Nationality = promise.studentList[0].pacA_Nationality;

                        $scope.obj.IVRMMC_Id = promise.studentList[0].ivrmmC_Id;

                        $scope.obj.ACQ_Id = promise.studentList[0].acQ_Id;
                        $scope.obj.ACQC_Id = promise.studentList[0].acqC_Id;

                        getSelectGetState($scope.obj.IVRMMC_Id, promise.studentList[0].pacA_PerState);

                        $scope.obj.PACA_PerState = promise.studentList[0].pacA_PerState;


                        $scope.obj.PACA_PerStreet = promise.studentList[0].pacA_PerStreet;
                        $scope.obj.PACA_PerArea = promise.studentList[0].pacA_PerArea;
                        $scope.obj.PACA_PerCity = promise.studentList[0].pacA_PerCity;

                        $scope.obj.PACA_PerPincode = promise.studentList[0].pacA_PerPincode;


                        $scope.obj.PACA_StuBankAccNo = promise.studentList[0].pacA_StuBankAccNo;
                        $scope.obj.PACA_StuBankIFSCCode = promise.studentList[0].pacA_StuBankIFSCCode;
                        $scope.obj.PACA_AadharNo = promise.studentList[0].pacA_AadharNo;
                        $scope.obj.PACA_BirthPlace = promise.studentList[0].pacA_BirthPlace;
                        $scope.obj.PACA_StuCasteCertiNo = promise.studentList[0].pacA_StuCasteCertiNo;
                        $scope.obj.PACA_MobileNo = promise.studentList[0].pacA_MobileNo;
                        $scope.obj.PACA_emailId = promise.studentList[0].pacA_emailId;

                        $scope.obj.PACA_PerStreet = promise.studentList[0].pacA_PerStreet;
                        $scope.obj.PACA_ConPincode = promise.studentList[0].pacA_ConPincode;
                        $scope.obj.PACA_ConArea = promise.studentList[0].pacA_ConArea;
                        $scope.obj.PACA_ConStreet = promise.studentList[0].pacA_ConStreet;
                        $scope.obj.PACA_ConCity = promise.studentList[0].pacA_ConCity;
                        $scope.obj.PACA_ConCountryId = promise.studentList[0].pacA_ConCountryId;

                        getSelectGetState2($scope.obj.PACA_ConCountryId, promise.studentList[0].pacA_ConState);

                        $scope.obj.PACA_ConState = promise.studentList[0].pacA_ConState;

                        $scope.obj.PACA_FatherAliveFlag = promise.studentList[0].pacA_FatherAliveFlag;

                        $scope.obj.PACA_FatherName = promise.studentList[0].pacA_FatherName;
                        $scope.obj.PACA_FatherSurname = promise.studentList[0].pacA_FatherSurname;
                        //   $scope.obj.PACA_FatherAadharNo = promise.studentList[0].pacA_FatherAadharNo;

                        if (promise.studentList[0].pacA_FatherAadharNo != '0' && promise.studentList[0].pacA_FatherAadharNo != null) {
                            $scope.obj.PACA_FatherAadharNo = promise.studentList[0].pacA_FatherAadharNo;
                        }
                        else {
                            $scope.obj.PACA_FatherAadharNo = "";
                        }

                        $scope.obj.PACA_FatherEducation = promise.studentList[0].pacA_FatherEducation;
                        $scope.obj.PACA_FatherOfficeAdd = promise.studentList[0].pacA_FatherOfficeAdd;
                        $scope.obj.PACA_FatherOccupation = promise.studentList[0].pacA_FatherOccupation;
                        $scope.obj.PACA_FatherDesignation = promise.studentList[0].pacA_FatherDesignation;
                        $scope.obj.PACA_FatherBankAccNo = promise.studentList[0].pacA_FatherBankAccNo;
                        $scope.obj.PACA_FatherBankIFSCCode = promise.studentList[0].pacA_FatherBankIFSCCode;
                        $scope.obj.PACA_FatherCasteCertiNo = promise.studentList[0].pacA_FatherCasteCertiNo;
                        $scope.obj.PACA_FatherNationality = promise.studentList[0].pacA_FatherNationality;
                        //   $scope.obj.PACA_FatherMonIncome = promise.studentList[0].pacA_FatherMonIncome;
                        //  $scope.obj.PACA_FatherAnnIncome = promise.studentList[0].pacA_FatherAnnIncome;
                        if (promise.studentList[0].pacA_FatherMonIncome != '0' && promise.studentList[0].pacA_FatherMonIncome != null) {
                            $scope.obj.PACA_FatherMonIncome = promise.studentList[0].pacA_FatherMonIncome;
                        }
                        else {
                            $scope.obj.PACA_FatherMonIncome = "";
                        }
                        if (promise.studentList[0].pacA_FatherAnnIncome != '0' && promise.studentList[0].pacA_FatherAnnIncome != null) {
                            $scope.obj.PACA_FatherAnnIncome = promise.studentList[0].pacA_FatherAnnIncome;
                        }
                        else {
                            $scope.obj.PACA_FatherAnnIncome = "";
                        }
                        //   $scope.obj.PACA_FatherMobleNo = promise.studentList[0].pacA_FatherMobleNo;

                        if (promise.studentList[0].pacA_FatherMobleNo != '0' && promise.studentList[0].pacA_FatherMobleNo != null) {
                            $scope.obj.PACA_FatherMobleNo = promise.studentList[0].pacA_FatherMobleNo;
                        }
                        else {
                            $scope.obj.PACA_FatherMobleNo = "";
                        }
                        $scope.obj.PACA_FatherEmailId = promise.studentList[0].pacA_FatherEmailId;
                        $scope.obj.PACA_FatherReligion = promise.studentList[0].pacA_FatherReligion;
                        $scope.obj.PACA_FatherCaste = promise.studentList[0].pacA_FatherCaste;
                        $scope.obj.PACA_FatherSubCaste = promise.studentList[0].pacA_FatherSubCaste;
                        $scope.obj.PACA_FatherCountryCode = promise.studentList[0].pacA_FatherCountryCode;


                        $scope.obj.PACA_MotherAliveFlag = promise.studentList[0].pacA_MotherAliveFlag;

                        $scope.obj.PACA_MotherName = promise.studentList[0].pacA_MotherName;
                        $scope.obj.PACA_MotherSurname = promise.studentList[0].pacA_MotherSurname;
                        $scope.obj.PACA_MotherCountryCode = promise.studentList[0].pacA_MotherCountryCode;

                        if (promise.studentList[0].pacA_MotherAadharNo != '0' && promise.studentList[0].pacA_MotherAadharNo != null) {
                            $scope.obj.PACA_MotherAadharNo = promise.studentList[0].pacA_MotherAadharNo;
                        }
                        else {
                            $scope.obj.PACA_MotherAadharNo = "";
                        }
                        $scope.obj.PACA_MotherEducation = promise.studentList[0].pacA_MotherEducation;
                        $scope.obj.PACA_MotherOfficeAdd = promise.studentList[0].pacA_MotherOfficeAdd;
                        $scope.obj.PACA_MotherOccupation = promise.studentList[0].pacA_MotherOccupation;
                        $scope.obj.PACA_MotherDesignation = promise.studentList[0].pacA_MotherDesignation;
                        $scope.obj.PACA_MotherBankAccNo = promise.studentList[0].pacA_MotherBankAccNo;
                        $scope.obj.PACA_MotherBankIFSCCode = promise.studentList[0].pacA_MotherBankIFSCCode;
                        $scope.obj.PACA_MotherCasteCertiNo = promise.studentList[0].pacA_MotherCasteCertiNo;
                        $scope.obj.PACA_MotherNationality = promise.studentList[0].pacA_MotherNationality;
                        $scope.obj.PACA_MotherNationality = promise.studentList[0].pacA_MotherNationality;
                        // $scope.obj.PACA_MotherMonIncome = promise.studentList[0].pacA_MotherMonIncome;

                        if (promise.studentList[0].pacA_MotherMonIncome != '0' && promise.studentList[0].pacA_MotherMonIncome != null) {
                            $scope.obj.PACA_MotherMonIncome = promise.studentList[0].pacA_MotherMonIncome;
                        }
                        else {
                            $scope.obj.PACA_MotherMonIncome = "";
                        }
                        // $scope.obj.PACA_MotherAnnIncome = promise.studentList[0].pacA_MotherAnnIncome;

                        if (promise.studentList[0].pacA_MotherAnnIncome != '0' && promise.studentList[0].pacA_MotherAnnIncome != null) {
                            $scope.obj.PACA_MotherAnnIncome = promise.studentList[0].pacA_MotherAnnIncome;
                        }
                        else {
                            $scope.obj.PACA_MotherAnnIncome = "";
                        }
                        //  $scope.obj.PACA_MotherMobleNo = promise.studentList[0].pacA_MotherMobleNo;

                        if (promise.studentList[0].pacA_MotherMobleNo != '0' && promise.studentList[0].pacA_MotherMobleNo != null) {
                            $scope.obj.PACA_MotherMobleNo = promise.studentList[0].pacA_MotherMobleNo;
                        }
                        else {
                            $scope.obj.PACA_MotherMobleNo = "";
                        }
                        $scope.obj.PACA_MotherEmailId = promise.studentList[0].pacA_MotherEmailId;

                        $scope.obj.PACA_MotherReligion = promise.studentList[0].pacA_MotherReligion;
                        $scope.obj.PACA_MotherCaste = promise.studentList[0].pacA_MotherCaste;
                        $scope.obj.PACA_MotherSubCaste = promise.studentList[0].pacA_MotherSubCaste;


                        //comp marks and exam details
                        $scope.compexamstudetails = [];
                        $scope.compexammarksdetails = [];
                        $scope.compexammarksdetails = promise.studentCompSubDetails;
                        $scope.compexamstudetails = promise.studentCompDetails;

                        if ($scope.compexamstudetails.length > 0) {
                            $scope.editflg = true;
                            for (var i = 0; i < $scope.compexamstudetails.length; i++) {

                                $scope.compexamstudetails[i].pamcexM_Id = $scope.compexamstudetails[0].pamcexM_Id;
                                $scope.compexamstudetails[i].pacstceM_RollNo = $scope.compexamstudetails[0].pacstceM_RollNo;
                                $scope.compexamstudetails[i].pacstceM_MeritNo = $scope.compexamstudetails[0].pacstceM_MeritNo;
                                $scope.compexamstudetails[i].PACSTCEM_RegistrationId = $scope.compexamstudetails[0].pacstceM_RegistrationId;
                                $scope.compexamstudetails[i].PACSTCEM_TotalMaxMarks = $scope.compexamstudetails[0].pacstceM_TotalMaxMarks;
                                $scope.compexamstudetails[i].PACSTCEM_ObtdMarks = $scope.compexamstudetails[0].pacstceM_ObtdMarks;
                                $scope.compexamstudetails[i].PACSTCEM_ALLIndiaRank = $scope.compexamstudetails[0].pacstceM_ALLIndiaRank;
                                $scope.compexamstudetails[i].PACSTCEM_CategoryRank = $scope.compexamstudetails[0].pacstceM_CategoryRank;
                                $scope.compexamstudetails[i].PACSTCEM_Percentage = $scope.compexamstudetails[0].pacstceM_Percentage;
                                $scope.compexamstudetails[i].PACSTCEM_Percentile = $scope.compexamstudetails[0].pacstceM_Percentile;
                                $scope.compexamstudetails[i].PAMCEXM_CompetitiveExams = $scope.compexamstudetails[0].pamcexM_CompetitiveExams;

                            }


                        }
                        else {

                            $scope.compexamstudetails = {};
                            $scope.compexamstudetails = [{ id: 'compExamStudetails1' }];
                            $scope.compexamstudetailscount = 0;
                            $scope.editflg = false;
                        }

                        if ($scope.compexammarksdetails.length > 0) {
                            $scope.editflg = true;
                            for (var i = 0; i < $scope.compexammarksdetails.length; i++) {
                                $scope.compexammarksdetails[i].pamcexmsuB_Id = $scope.compexammarksdetails[0].pamcexmsuB_Id;
                                $scope.compexammarksdetails[i].pamcexmsuB_MaxMarks = $scope.compexammarksdetails[0].pamcexmsuB_MaxMarks;
                                $scope.compexammarksdetails[i].pacstcemS_SubjectMarks = $scope.compexammarksdetails[0].pacstcemS_SubjectMarks;
                                $scope.compexammarksdetails[i].PAMCEXM_CompetitiveExams = $scope.compexammarksdetails[0].pamcexM_CompetitiveExams;
                                $scope.compexammarksdetails[i].PAMCEXMSUB_SubjectName = $scope.compexammarksdetails[0].pamcexmsuB_SubjectName;

                            }
                        }
                        else {

                            $scope.compexammarksdetails = {};
                            $scope.compexammarksdetails = [{ id: 'compExamdetails1' }];
                            $scope.compexammarksdetailscount = 0;
                            $scope.editflg = false;
                        }






                        //


                        $scope.obj.PACA_BPLCardFlag = promise.studentList[0].pacA_BPLCardFlag;


                        $scope.obj.PACA_BPLCardNo = promise.studentList[0].pacA_BPLCardNo;
                        $scope.obj.PACA_HostelReqdFlag = promise.studentList[0].pacA_HostelReqdFlag;

                        $scope.obj.PACA_TransportReqdFlag = promise.studentList[0].pacA_TransportReqdFlag;

                        $scope.obj.PACA_GymReqdFlag = promise.studentList[0].pacA_GymReqdFlag;

                        $scope.obj.PACA_ECSFlag = promise.studentList[0].pacA_ECSFlag;

                        $scope.obj.PACA_PassportNo = promise.studentList[0].pacA_PassportNo;
                        if ($scope.obj.PACA_PassportNo != "" && $scope.obj.PACA_PassportNo != null) {
                            $scope.obj.PACA_PassportIssuedAt = promise.studentList[0].pacA_PassportIssuedAt;
                            $scope.obj.PACA_PassportIssueDate = new Date(promise.studentList[0].pacA_PassportIssueDate);
                            $scope.obj.PACA_PassportIssuedCounrty = promise.studentList[0].pacA_PassportIssuedCounrty;
                            $scope.obj.PACA_PassportIssuedPlace = promise.studentList[0].pacA_PassportIssuedPlace;
                            $scope.obj.PACA_PassportExpiryDate = new Date(promise.studentList[0].pacA_PassportExpiryDate);
                        }
                        $scope.obj.PACA_VISAIssuedBy = promise.studentList[0].pacA_VISAIssuedBy;
                        if ($scope.obj.PACA_VISAIssuedBy != "" && $scope.obj.PACA_VISAIssuedBy != null) {

                            $scope.obj.PACA_VISAValidFrom = new Date(promise.studentList[0].pacA_VISAValidFrom);
                            $scope.obj.PACA_VISAValidTo = new Date(promise.studentList[0].pacA_VISAValidTo);
                        }

                        $scope.obj.chkbox_address = 0;
                        if ($scope.obj.PACA_PerStreet == $scope.obj.PACA_ConStreet && $scope.obj.PACA_PerArea == $scope.obj.PACA_ConArea && $scope.obj.IVRMMC_Id == $scope.obj.PACA_ConCountryId && $scope.obj.PACA_PerState == $scope.obj.PACA_ConState && $scope.obj.PACA_PerCity == $scope.obj.PACA_ConCity && $scope.obj.PACA_PerPincode == $scope.obj.PACA_ConPincode) {
                            $scope.obj.chkbox_address = 1;
                        }
                        $scope.obj.feeconcession = promise.studentList[0].amsT_Concession_Type;
                        if (promise.classsection != null && promise.classsection != "") {
                            $scope.lblclasssection = promise.classsection[0].classname + '-' + promise.classsection[0].sectionname;
                        }
                        else {
                            $scope.lblclasssection = "N/A";
                        }

                        if (promise.pA_College_Student_CBPreference.length > 0) {
                            $scope.mobiles = promise.pA_College_Student_CBPreference;
                        }

                        $scope.branchespref = promise.branches;
                    }
                    $scope.scroll();
                })
        };

        $scope.showprintdata = function (studentid) {

            apiService.getURI("ApplicationForm/getprintdata/", studentid).then(function (promise) {


                if (promise.studentList != null || promise.studentList.length > 0) {

                    //$scope.documentList = promise.documentList;
                    //$scope.DOB = false;
                    //$scope.mi_id = promise.mI_Id;


                    if (promise.prevSchoolDetails != null) {
                        if (promise.prevSchoolDetails.length > 0) {

                            $scope.prevSchoolDetails = promise.prevSchoolDetails;
                            $scope.prevschlcount = promise.prevSchoolDetails.length;

                            for (var i = 0; i < promise.prevSchoolDetails.length; i++) {
                                $scope.pacstpS_PreSchoolBoard = promise.prevSchoolDetails[i].pacstpS_PreSchoolBoard;
                                $scope.pacstpS_PreviousExamPassed = promise.prevSchoolDetails[i].pacstpS_PreviousExamPassed;
                                $scope.pacstpS_PreSchoolType = promise.prevSchoolDetails[i].pacstpS_PreSchoolType;

                                $scope.pacstpS_PreviousMarks = promise.prevSchoolDetails[i].pacstpS_PreviousMarks;
                                $scope.pacstpS_PreviousMarksObtained = promise.prevSchoolDetails[i].pacstpS_PreviousMarksObtained;
                                $scope.pacstpS_PreviousGrade = promise.prevSchoolDetails[i].pacstpS_PreviousGrade;

                                $scope.pacstpS_PrvSchoolName = promise.prevSchoolDetails[i].pacstpS_PrvSchoolName;
                                $scope.pacstpS_PreviousExamPassed = promise.prevSchoolDetails[i].pacstpS_PreviousExamPassed;
                                $scope.pacstpS_Urbanrural = promise.prevSchoolDetails[i].pacstpS_Urbanrural;
                                $scope.pacstpS_Attempts = promise.prevSchoolDetails[i].pacstpS_Attempts;
                                $scope.pacstpS_PreviousClass = promise.prevSchoolDetails[i].pacstpS_PreviousClass;
                                $scope.pacstpS_PreviousTCNo = promise.prevSchoolDetails[i].pacstpS_PreviousTCNo;
                                $scope.pacstpS_PreviousRegNo = promise.prevSchoolDetails[i].pacstpS_PreviousRegNo;
                                $scope.pacstpS_PreviousBranch = promise.prevSchoolDetails[i].pacstpS_PreviousBranch;
                                $scope.pacstpS_MediumOfInst = promise.prevSchoolDetails[i].pacstpS_MediumOfInst;
                                $scope.pacstpS_PasssedMonthYear = promise.prevSchoolDetails[i].pacstpS_PasssedMonthYear;
                                $scope.pacstpS_LanguagesTaken = promise.prevSchoolDetails[i].pacstpS_LanguagesTaken;
                                $scope.pacstpS_TCDate = new Date(promise.prevSchoolDetails[i].pacstpS_TCDate);
                                $scope.pacstpS_LeftYear = promise.prevSchoolDetails[i].pacstpS_LeftYear;
                                $scope.studentpreviousstate = promise.studentpreviousstate.length > 0 ? promise.studentpreviousstate[0].studpreviousstate : "";

                                var doobTc = promise.prevSchoolDetails[i].pacstpS_TCDate;

                                var doobyrtc = doobTc.substring(0, 4);
                                var doobmnthtc = doobTc.substring(5, 7);
                                var doobdaystc = doobTc.substring(8, 10);

                                $scope.b1tc = doobdaystc.substring(0, 1);
                                $scope.b2tc = doobdaystc.substring(1, 2);
                                $scope.BTC1 = $scope.b1tc + $scope.b2tc;

                                $scope.b3tc = doobmnthtc.substring(0, 1);
                                $scope.b4tc = doobmnthtc.substring(1, 2);
                                $scope.BTC2 = $scope.b3tc + $scope.b4tc;

                                $scope.b5tc = doobyrtc.substring(0, 1);
                                $scope.b6tc = doobyrtc.substring(1, 2);
                                $scope.b7tc = doobyrtc.substring(2, 3);
                                $scope.b8tc = doobyrtc.substring(3, 4);
                                $scope.BTC3 = $scope.b5tc + $scope.b6tc + $scope.b7tc + $scope.b8tc;

                                $scope.pacstpS_PreSchoolCountry = promise.prevSchoolDetails[i].pacstpS_PreSchoolCountry;
                                //  getPrevGetState(promise.prevSchoolDetails[i].pacstpS_PreSchoolCountry, promise.prevSchoolDetails[i].pacstpS_PreSchoolState);
                                $scope.onselectprevCountry($scope.pacstpS_PreSchoolCountry);
                                $scope.pacstpS_PreSchoolState = promise.prevSchoolDetails[i].pacstpS_PreSchoolState;
                            }
                        }
                    }
                    else {
                        $scope.prevSchoolDetails = [{ id: 'prevSchool1' }];
                        $scope.prevschlcount = 0;
                        $scope.pacstpS_Attempts = 1;
                    }
                    if (promise.studentGuardianDetails != null) {
                        if (promise.studentGuardianDetails.length > 0) {
                            $scope.studentGuardianDetails = promise.studentGuardianDetails;
                            $scope.grddetcount = promise.studentGuardianDetails.length;
                        }
                    }

                    else {
                        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];
                        $scope.grddetcount = 0;
                    }

                    if (promise.studentsubjectmarksarry != null) {
                        if (promise.studentsubjectmarksarry.length > 0) {

                            $scope.prevexammarksdetailsprint = promise.studentsubjectmarksarry;
                            $scope.prevexammarksdetailscountprint = promise.studentsubjectmarksarry.length;
                        }
                    }

                    //documnets
                    if (promise.documentList != null) {
                        if (promise.documentList.length > 0) {
                            $scope.document = {};
                            $scope.documentList = promise.documentList;
                            angular.forEach(promise.documentList, function (value, key) {
                                $('#' + value.amsmD_Id).attr('src', value.document_Path);
                            })
                        }
                    }


                    $scope.stuimage = promise.studentList[0].pacA_StudentPhoto;

                    $('#blah').attr('src', promise.studentList[0].pacA_StudentPhoto);



                    $scope.fatherphoto = promise.studentList[0].pacA_FatherPhoto;
                    $scope.fatherSign = promise.studentList[0].pacA_FatherSign;
                    $scope.fatherFingerprint = promise.studentList[0].pacA_FatherFingerprint;
                    $scope.motherphoto = promise.studentList[0].pacA_MotherPhoto;
                    $scope.mothersign = promise.studentList[0].pacA_MotherSign;
                    $scope.motherfingerprint = promise.studentList[0].pacA_MotherFingerprint;
                    $scope.image = promise.studentList[0].pacA_StudentPhoto;
                    $('#blahnewa').attr('src', promise.studentList[0].pacA_StudentPhoto);



                    if (promise.studentList != null || promise.studentList.length > 0) {
                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < promise.studentList.length; i++) {
                            if (promise.studentList[i].pacA_FirstName != '') {
                                if (promise.studentList[i].pacA_MiddleName !== null) {
                                    if (promise.studentList[i].pacA_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: promise.studentList[i].pacA_FirstName + " " + promise.studentList[i].pacA_MiddleName + " " + promise.studentList[i].pacA_LastName, pacA_Id: promise.studentList[i].pacA_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: promise.studentList[i].pacA_FirstName + " " + promise.studentList[i].pacA_MiddleName, pacA_Id: promise.studentList[i].pacA_Id });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studentList[i].pacA_FirstName, pacA_Id: promise.studentList[i].pacA_Id });
                                }

                                promise.studentList[i].pacA_FirstName = $scope.albumNameArray1[i].name;
                            }
                        }
                    }


                    $scope.PACA_FirstName = promise.studentList[0].pacA_FirstName;
                    $scope.PACA_MiddleName = promise.studentList[0].pacA_MiddleName;
                    $scope.PACA_LastName = promise.studentList[0].pacA_LastName;
                    $scope.PACA_Date = new Date(promise.studentList[0].pacA_Date);
                    $scope.PACA_RegistrationNo = promise.studentList[0].pacA_RegistrationNo;
                    $scope.PACA_AdmNo = promise.studentList[0].pacA_AdmNo;
                    $scope.PACA_StudentSubCaste = promise.studentList[0].pacA_StudentSubCaste;
                    $scope.ASMAY_Id = promise.studentList[0].asmaY_Id;
                    $scope.AMCO_Id = promise.studentList[0].amcO_Id;
                    $scope.AMB_Id = promise.studentList[0].amB_Id;
                    $scope.AMSE_Id = promise.studentList[0].amsE_Id;
                    $scope.ACMB_Id = promise.studentList[0].acmB_Id;
                    $scope.ACSS_Id = promise.studentList[0].acsS_Id;
                    $scope.ACST_Id = promise.studentList[0].acsT_Id;
                    if (promise.studentCategory.length > 0) {
                        $scope.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                    }
                    $scope.PACA_Sex = promise.studentList[0].pacA_Sex;
                    $scope.PACA_DOB = new Date(promise.studentList[0].pacA_DOB);
                    var doob = promise.studentList[0].pacA_DOB;

                    var doobyr = doob.substring(0, 4);
                    var doobmnth = doob.substring(5, 7);
                    var doobdays = doob.substring(8, 10);

                    $scope.b1 = doobdays.substring(0, 1);
                    $scope.b2 = doobdays.substring(1, 2);
                    $scope.BB1 = $scope.b1 + $scope.b2;

                    $scope.b3 = doobmnth.substring(0, 1);
                    $scope.b4 = doobmnth.substring(1, 2);
                    $scope.BB2 = $scope.b3 + $scope.b4;

                    $scope.b5 = doobyr.substring(0, 1);
                    $scope.b6 = doobyr.substring(1, 2);
                    $scope.b7 = doobyr.substring(2, 3);
                    $scope.b8 = doobyr.substring(3, 4);
                    $scope.BB3 = $scope.b5 + $scope.b6 + $scope.b7 + $scope.b8;

                    $scope.PACA_DOB_inwords = promise.studentList[0].pacA_DOB_inwords;
                    $scope.PACA_Age = promise.studentList[0].pacA_Age;
                    $scope.PACA_BloodGroup = promise.studentList[0].pacA_BloodGroup;
                    $scope.PACA_MotherTongue = promise.studentList[0].pacA_MotherTongue;
                    $scope.PACA_BirthPlace = promise.studentList[0].pacA_BirthPlace;
                    $scope.PACA_BirthCertNo = promise.studentList[0].pacA_BirthCertNo;
                    $scope.IVRMMR_Id = promise.studentList[0].ivrmmR_Id;
                    $scope.PACA_StudentSubCaste = promise.studentList[0].pacA_StudentSubCaste;
                    $scope.PACA_TPINNO = promise.studentList[0].pacA_TPINNO;

                    $scope.PACA_Village = promise.studentList[0].pacA_Village;
                    $scope.PACA_Taluk = promise.studentList[0].pacA_Taluk;
                    $scope.PACA_District = promise.studentList[0].pacA_District;
                    $scope.PACA_Urban_Rural = promise.studentList[0].pacA_Urban_Rural;

                    $scope.IMCC_Id = promise.studentList[0].imcC_Id;
                    for (var i = 0; i < $scope.allCaste.length; i++) {
                        $scope.allCaste[i].Selected = false;
                        $scope.IMC_Id = "";
                    }


                    if (promise.allCaste.length > 0) {
                        for (var i = 0; i < promise.allCaste.length; i++) {
                            if (promise.studentList[0].imC_Id == promise.allCaste[i].imC_Id) {
                                $scope.allCaste[i].Selected = true;
                                $scope.IMC_Id = promise.studentList[0].imC_Id;
                            }
                        }
                    }
                    else {
                        swal("Something has gone wrong.Please check Master Caste Category and Master Caste");
                    }




                    $scope.PACA_Nationality = promise.studentList[0].pacA_Nationality;

                    $scope.IVRMMC_Id = promise.studentList[0].ivrmmC_Id;



                    getSelectGetState($scope.IVRMMC_Id, promise.studentList[0].pacA_PerState);

                    $scope.PACA_PerState = promise.studentList[0].pacA_PerState;


                    $scope.PACA_PerStreet = promise.studentList[0].pacA_PerStreet;
                    $scope.PACA_PerArea = promise.studentList[0].pacA_PerArea;
                    $scope.PACA_PerCity = promise.studentList[0].pacA_PerCity;

                    $scope.PACA_PerPincode = promise.studentList[0].pacA_PerPincode;


                    $scope.PACA_StuBankAccNo = promise.studentList[0].pacA_StuBankAccNo;
                    $scope.PACA_StuBankIFSCCode = promise.studentList[0].pacA_StuBankIFSCCode;
                    $scope.PACA_AadharNo = promise.studentList[0].pacA_AadharNo;
                    $scope.PACA_BirthPlace = promise.studentList[0].pacA_BirthPlace;
                    $scope.PACA_StuCasteCertiNo = promise.studentList[0].pacA_StuCasteCertiNo;
                    $scope.PACA_MobileNo = promise.studentList[0].pacA_MobileNo;
                    $scope.PACA_emailId = promise.studentList[0].pacA_emailId;

                    $scope.PACA_PerStreet = promise.studentList[0].pacA_PerStreet;
                    $scope.PACA_ConPincode = promise.studentList[0].pacA_ConPincode;
                    $scope.PACA_ConArea = promise.studentList[0].pacA_ConArea;
                    $scope.PACA_ConStreet = promise.studentList[0].pacA_ConStreet;
                    $scope.PACA_ConCity = promise.studentList[0].pacA_ConCity;
                    $scope.PACA_ConCountryId = promise.studentList[0].pacA_ConCountryId;


                    $scope.studcourse = promise.studentcourse.length > 0 ? promise.studentcourse[0].amcO_CourseName : "";
                    $scope.studReligion = promise.studentReligion.length > 0 ? promise.studentReligion[0].ivrmmR_Name : "";
                    $scope.CasteName = promise.studentcastecate.length > 0 ? promise.studentcastecate[0].imC_CasteName : "";
                    $scope.studperstate = promise.studentperstate.length > 0 ? promise.studentperstate[0].studperstate : "";
                    $scope.studconstate = promise.studentconstate.length > 0 ? promise.studentconstate[0].studconstate : "";
                    $scope.studconcountry = promise.studentconcountry.length > 0 ? promise.studentconcountry[0].studconcountry : "";
                    $scope.studpercountry = promise.studentpercountry.length > 0 ? promise.studentpercountry[0].studpercountry : "";

                    $scope.countrycode = promise.studentpercountry.length > 0 ? promise.studentpercountry[0].countrycode : "";
                    $scope.statecode = promise.studentperstate.length > 0 ? promise.studentperstate[0].statecode : "";
                    $scope.CategoryName = promise.casteCategoryName.length > 0 ? promise.casteCategoryName[0].categoryName : "";


                    getSelectGetState2($scope.PACA_ConCountryId, promise.studentList[0].pacA_ConState);

                    $scope.PACA_ConState = promise.studentList[0].pacA_ConState;

                    $scope.PACA_FatherAliveFlag = promise.studentList[0].pacA_FatherAliveFlag;
                    if (promise.studentList[0].pacA_FatherSurname != null) {
                        $scope.PACA_FatherName = promise.studentList[0].pacA_FatherName + ' ' + promise.studentList[0].pacA_FatherSurname;
                    }
                    else {
                        $scope.PACA_FatherName = promise.studentList[0].pacA_FatherName;
                    }

                    $scope.PACA_FatherSurname = promise.studentList[0].pacA_FatherSurname;
                    $scope.PACA_FatherAadharNo = promise.studentList[0].pacA_FatherAadharNo;
                    $scope.PACA_FatherEducation = promise.studentList[0].pacA_FatherEducation;
                    $scope.PACA_FatherOfficeAdd = promise.studentList[0].pacA_FatherOfficeAdd;
                    $scope.PACA_FatherOccupation = promise.studentList[0].pacA_FatherOccupation;
                    $scope.PACA_FatherDesignation = promise.studentList[0].pacA_FatherDesignation;
                    $scope.PACA_FatherBankAccNo = promise.studentList[0].pacA_FatherBankAccNo;
                    $scope.PACA_FatherBankIFSCCode = promise.studentList[0].pacA_FatherBankIFSCCode;
                    $scope.PACA_FatherCasteCertiNo = promise.studentList[0].pacA_FatherCasteCertiNo;
                    $scope.PACA_FatherNationality = promise.studentList[0].pacA_FatherNationality;
                    $scope.PACA_FatherMonIncome = promise.studentList[0].pacA_FatherMonIncome;
                    $scope.PACA_FatherAnnIncome = promise.studentList[0].pacA_FatherAnnIncome;
                    $scope.PACA_FatherMobleNo = promise.studentList[0].pacA_FatherMobleNo;
                    $scope.PACA_FatherEmailId = promise.studentList[0].pacA_FatherEmailId;
                    $scope.PACA_FatherReligion = promise.studentList[0].pacA_FatherReligion;
                    $scope.PACA_FatherCaste = promise.studentList[0].pacA_FatherCaste;
                    $scope.PACA_FatherSubCaste = promise.studentList[0].pacA_FatherSubCaste;

                    $scope.PACA_MotherAliveFlag = promise.studentList[0].pacA_MotherAliveFlag;
                    if (promise.studentList[0].pacA_MotherSurname != null) {
                        $scope.PACA_MotherName = promise.studentList[0].pacA_MotherName + ' ' + promise.studentList[0].pacA_MotherSurname;
                    }
                    else {
                        $scope.PACA_MotherName = promise.studentList[0].pacA_MotherName;
                    }

                    $scope.PACA_MotherSurname = promise.studentList[0].pacA_MotherSurname;
                    $scope.PACA_MotherAadharNo = promise.studentList[0].pacA_MotherAadharNo;
                    $scope.PACA_MotherEducation = promise.studentList[0].pacA_MotherEducation;
                    $scope.PACA_MotherOfficeAdd = promise.studentList[0].pacA_MotherOfficeAdd;
                    $scope.PACA_MotherOccupation = promise.studentList[0].pacA_MotherOccupation;
                    $scope.PACA_MotherDesignation = promise.studentList[0].pacA_MotherDesignation;
                    $scope.PACA_MotherBankAccNo = promise.studentList[0].pacA_MotherBankAccNo;
                    $scope.PACA_MotherBankIFSCCode = promise.studentList[0].pacA_MotherBankIFSCCode;
                    $scope.PACA_MotherCasteCertiNo = promise.studentList[0].pacA_MotherCasteCertiNo;
                    $scope.PACA_MotherNationality = promise.studentList[0].pacA_MotherNationality;
                    $scope.PACA_MotherMonIncome = promise.studentList[0].pacA_MotherMonIncome;
                    $scope.PACA_MotherAnnIncome = promise.studentList[0].pacA_MotherAnnIncome;
                    $scope.PACA_MotherMobleNo = promise.studentList[0].pacA_MotherMobleNo;
                    $scope.PACA_MotherEmailId = promise.studentList[0].pacA_MotherEmailId;

                    $scope.PACA_MotherReligion = promise.studentList[0].pacA_MotherReligion;
                    $scope.PACA_MotherCaste = promise.studentList[0].pacA_MotherCaste;
                    $scope.PACA_MotherSubCaste = promise.studentList[0].pacA_MotherSubCaste;

                    $scope.PACA_PassportNo = promise.studentList[0].pacA_PassportNo;

                    $scope.PACA_PassportIssuedAt = promise.studentList[0].pacA_PassportIssuedAt;
                    if (promise.studentList[0].pacA_PassportIssueDate != null) {


                        $scope.PACA_PassportIssueDate = new Date(promise.studentList[0].pacA_PassportIssueDate);

                        var do0ob = promise.studentList[0].pacA_PassportIssueDate;

                        var doobyrv = do0ob.substring(0, 4);
                        var doobmnthv = do0ob.substring(5, 7);
                        var doobdaysv = do0ob.substring(8, 10);

                        $scope.b1v = doobdaysv.substring(0, 1);
                        $scope.b2v = doobdaysv.substring(1, 2);
                        $scope.BV1 = $scope.b1v + $scope.b2v;

                        $scope.b3v = doobmnthv.substring(0, 1);
                        $scope.b4v = doobmnthv.substring(1, 2);
                        $scope.BV2 = $scope.b3v + $scope.b4v;

                        $scope.b5v = doobyrv.substring(0, 1);
                        $scope.b6v = doobyrv.substring(1, 2);
                        $scope.b7v = doobyrv.substring(2, 3);
                        $scope.b8v = doobyrv.substring(3, 4);
                        $scope.BV3 = $scope.b5v + $scope.b6v + $scope.b7v + $scope.b8v;
                    }
                    $scope.PACA_PassportIssuedCounrty = promise.studentList[0].pacA_PassportIssuedCounrty;
                    $scope.PACA_PassportIssuedPlace = promise.studentList[0].pacA_PassportIssuedPlace;
                    $scope.PACA_PassportExpiryDate = promise.studentList[0].pacA_PassportExpiryDate;

                    $scope.PACA_VISAIssuedBy = promise.studentList[0].pacA_VISAIssuedBy;
                    $scope.PACA_VISAValidFrom = promise.studentList[0].pacA_VISAValidFrom;
                    $scope.PACA_VISAValidTo = promise.studentList[0].pacA_VISAValidTo;

                    if (promise.studentpreffredbranch != null && promise.studentpreffredbranch.length > 0) {
                        $scope.studentpreffredbranch = promise.studentpreffredbranch;
                    }

                    if (promise.studentcurrenrtbranch != null && promise.studentcurrenrtbranch.length > 0) {
                        $scope.studentcurrenrtbranch = promise.studentcurrenrtbranch[0].studentbranchname;
                    }



                    //comp marks and exam details
                    $scope.compexamstudetails = [];
                    $scope.compexammarksdetails = [];
                    $scope.compexammarksdetails = promise.studentCompSubDetails;
                    $scope.compexamstudetails = promise.studentCompDetails;

                    if ($scope.compexamstudetails.length > 0) {
                        $scope.editflg = true;
                        for (var i = 0; i < $scope.compexamstudetails.length; i++) {

                            $scope.compexamstudetails[i].pamcexM_Id = $scope.compexamstudetails[0].pamcexM_Id;
                            $scope.compexamstudetails[i].pacstceM_RollNo = $scope.compexamstudetails[0].pacstceM_RollNo;
                            $scope.compexamstudetails[i].pacstceM_MeritNo = $scope.compexamstudetails[0].pacstceM_MeritNo;
                            $scope.compexamstudetails[i].PACSTCEM_RegistrationId = $scope.compexamstudetails[0].pacstceM_RegistrationId;
                            $scope.compexamstudetails[i].PACSTCEM_TotalMaxMarks = $scope.compexamstudetails[0].pacstceM_TotalMaxMarks;
                            $scope.compexamstudetails[i].PACSTCEM_ObtdMarks = $scope.compexamstudetails[0].pacstceM_ObtdMarks;
                            $scope.compexamstudetails[i].PACSTCEM_ALLIndiaRank = $scope.compexamstudetails[0].pacstceM_ALLIndiaRank;
                            $scope.compexamstudetails[i].PACSTCEM_CategoryRank = $scope.compexamstudetails[0].pacstceM_CategoryRank;
                            $scope.compexamstudetails[i].PACSTCEM_Percentage = $scope.compexamstudetails[0].pacstceM_Percentage;
                            $scope.compexamstudetails[i].PACSTCEM_Percentile = $scope.compexamstudetails[0].pacstceM_Percentile;
                            $scope.compexamstudetails[i].PAMCEXM_CompetitiveExams = $scope.compexamstudetails[0].pamcexM_CompetitiveExams;

                        }


                    }
                    //else {

                    //    $scope.compexamstudetails = {};

                    //    $scope.compexamstudetails = [{ id: 'compExamStudetails1' }];
                    //    $scope.compexamstudetailscount = 0;

                    //    $scope.editflg = false;
                    //}

                    if ($scope.compexammarksdetails.length > 0) {
                        $scope.editflg = true;
                        for (var i = 0; i < $scope.compexammarksdetails.length; i++) {
                            $scope.compexammarksdetails[i].pamcexmsuB_Id = $scope.compexammarksdetails[0].pamcexmsuB_Id;
                            $scope.compexammarksdetails[i].pamcexmsuB_MaxMarks = $scope.compexammarksdetails[0].pamcexmsuB_MaxMarks;
                            $scope.compexammarksdetails[i].pacstcemS_SubjectMarks = $scope.compexammarksdetails[0].pacstcemS_SubjectMarks;
                            $scope.compexammarksdetails[i].PAMCEXM_CompetitiveExams = $scope.compexammarksdetails[0].pamcexM_CompetitiveExams;
                            $scope.compexammarksdetails[i].PAMCEXMSUB_SubjectName = $scope.compexammarksdetails[0].pamcexmsuB_SubjectName;

                        }
                    }
                    //else {

                    //    $scope.compexammarksdetails = {};
                    //    $scope.compexammarksdetails = [{ id: 'compExamdetails1' }];
                    //    $scope.compexammarksdetailscount = 0;

                    //}






                    //


                }
            });
        }


        //get permenent address state while editing
        function getSelectGetState(countryidd, stateid) {
            apiService.getURI("ApplicationForm/getdpstate", countryidd).then(function (promise) {
                $scope.allState = [{ "ivrmmS_Id": "", "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.obj.PACA_PerState = sts;
                $scope.data = promise.allState;
                $scope.allState.push.apply($scope.allState, $scope.data);
                $scope.statelabel = false;
            })
        }
        //get Previous School State while editing.
        function getPrevGetState(countryidd, stateid) {
            var data = {
                countryName: countryidd
            }

            apiService.create("ApplicationForm/StateByCountryName", data).then(function (promise) {
                $scope.prevState = [{ "ivrmmS_Name": "", "ivrmmS_Name": "--Select--" }];
                var sts = stateid;
                $scope.prevSchool.pacstpS_PreSchoolState = sts;
                $scope.data = promise.prevStateList;
                $scope.prevState.push.apply($scope.prevState, $scope.data);
            })
        }
        //get Residential address state while editing
        function getSelectGetState2(countryidd, stateid) {
            apiService.getURI("ApplicationForm/getdpstate", countryidd).then(function (promise) {
                $scope.allState1 = [{ "ivrmmS_Id": "", "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.obj.PACA_ConState = sts;
                $scope.data2 = promise.allState;
                $scope.allState1.push.apply($scope.allState1, $scope.data2);
                $scope.statelabel2 = false;
            })
        }

        $scope.Activitycheckboxchcked = [];

        $scope.CheckedActivityName = function (data) {

            if ($scope.Activitycheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allActivity.length; i++) {
                    if ($scope.allActivity[i].Selected == true) {
                        $scope.Activitycheckboxchcked.push($scope.allActivity[i]);
                    }
                }
            }
            else {
                $scope.Activitycheckboxchcked.splice($scope.Activitycheckboxchcked.indexOf(data), 1);
            }
        }





        $scope.Refrencecheckboxchcked = [];

        $scope.CheckedRefrenceName = function (data) {

            if ($scope.Refrencecheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allRefrence.length; i++) {
                    if ($scope.allRefrence[i].Selected == true) {
                        $scope.Refrencecheckboxchcked.push($scope.allRefrence[i]);
                    }
                }
            }
            else {
                $scope.Refrencecheckboxchcked.splice($scope.Refrencecheckboxchcked.indexOf(data), 1);
            }
        }





        $scope.Sourcescheckboxchcked = [];

        $scope.CheckedSourcesName = function (data) {

            if ($scope.Sourcescheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allSources.length; i++) {
                    if ($scope.allSources[i].Selected == true) {
                        $scope.Sourcescheckboxchcked.push($scope.allSources[i]);
                    }
                }
            }
            else {
                $scope.Sourcescheckboxchcked.splice($scope.Sourcescheckboxchcked.indexOf(data), 1);
            }
        }

        $scope.onSelectGetmobvalid = function (IVRMMC_Id) {

            var countryidd = IVRMMC_Id;
            if (countryidd != null) {
                $scope.obj.PACA_MobileNo = "";

            }

            //apiService.getURI("ApplicationForm/getdpmobvalid", countryidd).then(function (promise) {
            //    $scope.allState = promise.allState;
            //    $scope.obj.PACA_PerState = promise.allState[0].pacA_PerState;
            //    $scope.statelabel = true;
            //})
        }
        $scope.onSelectGetmobvalidmother = function (IVRMMC_Id, obj) {

            var countryidd = IVRMMC_Id;
            if (countryidd != null) {
                $scope.obj.PACA_MotherMobleNo = "";
            }
        }
        $scope.onSelectGetmobvalidfather = function (IVRMMC_Id) {

            var countryidd = IVRMMC_Id;
            if (countryidd != null) {

                $scope.obj.PACA_FatherMobleNo = "";
            }
        }

        $scope.onSelectGetmobvalidgard = function (IVRMMC_Id) {

            var countryidd = IVRMMC_Id;
            if (countryidd != null) {
                $scope.obj.pacstG_GuardianPhoneNo = "";
            }
        }

        //Get Satate by country
        $scope.onSelectGetState = function (IVRMMC_Id) {
            var countryidd = IVRMMC_Id;
            apiService.getURI("ApplicationForm/getdpstate", countryidd).then(function (promise) {
                $scope.allState = promise.allState;
                $scope.obj.PACA_PerState = promise.allState[0].pacA_PerState;
                $scope.statelabel = true;
            })
        }

        //get the state name by country  pre school
        $scope.onSelectGetStatepre = function (IVRMMC_Id5) {

            var countryidd = IVRMMC_Id5;
            apiService.getURI("ApplicationForm/getdpstate", countryidd).then(function (promise) {
                $scope.allState12 = promise.allState;
                $scope.statelabel = true;
            })
        }

        // Get city by state
        $scope.onSelectGetCity = function (ivrmmS_Id) {
            var stateId = ivrmmS_Id;
            apiService.getURI("StudentAdmission/getdpcities", stateId).then(function (promise) {
                $scope.allCity = promise.allCity;
            })
        }

        //Get Satate by country
        $scope.onSelectGetState1 = function (IVRMMC_Id2) {
            var countryidd = IVRMMC_Id2;
            apiService.getURI("ApplicationForm/getdpstate", countryidd).then(function (promise) {
                $scope.allState1 = promise.allState;
                $scope.obj.PACA_ConState = promise.allState[0].pacA_ConState;
                $scope.statelabel2 = true;
            })
        }

        // Get city by state
        $scope.onSelectGetCity1 = function (ivrmmS_Id2) {
            var stateId = ivrmmS_Id2;
            apiService.getURI("StudentAdmission/getdpcities", stateId).then(function (promise) {
                $scope.allCity1 = promise.allCity;
            })
        }



        $scope.address_copy = function () {

            if ($scope.obj.chkbox_address == 1) {
                $scope.obj.PACA_ConStreet = $scope.obj.PACA_PerStreet;
                $scope.obj.PACA_ConArea = $scope.obj.PACA_PerArea;
                $scope.obj.PACA_ConCountryId = $scope.obj.IVRMMC_Id;

                $scope.allState1 = [{ "ivrmmS_Id": "", "ivrmmS_Name": "Select State" }];
                var sts = Number($scope.obj.PACA_PerState);
                $scope.obj.PACA_ConState = sts;

                $scope.data2 = $scope.allState;
                $scope.allState1.push.apply($scope.allState1, $scope.data2);
                $scope.statelabel2 = false;

                $scope.obj.PACA_ConCity = $scope.obj.PACA_PerCity;
                $scope.obj.PACA_ConPincode = $scope.obj.PACA_PerPincode;

            }
            if ($scope.obj.chkbox_address == 0) {
                $scope.obj.PACA_ConStreet = "";
                $scope.obj.PACA_ConArea = "";
                $scope.obj.PACA_ConCountryId = "";
                $scope.obj.PACA_ConState = "";
                $scope.obj.PACA_ConCity = "";
                $scope.obj.PACA_ConPincode = "";

            }

        }

        $scope.SelectedGovtBonds = [];


        //------------------save method---------------------------------//
        $scope.savedata = function () {


            if ($scope.myForm1.$valid && $scope.myForm2.$valid && $scope.myForm3.$valid && $scope.myForm4.$valid && $scope.myForm5.$valid) {

                $scope.disableSaveButton = true;

                if ($scope.allActivity != "" && $scope.allActivity != null) {
                    if ($scope.allActivity.length > 0) {
                        for (var i = 0; i < $scope.allActivity.length; i++) {
                            if ($scope.allActivity[i].Selected == true) {
                                $scope.Activitycheckboxchcked.push($scope.allActivity[i]);
                            }
                        }
                    }
                }

                if ($scope.allRefrence != "" && $scope.allRefrence != null) {
                    if ($scope.allRefrence.length > 0) {
                        for (var i = 0; i < $scope.allRefrence.length; i++) {
                            if ($scope.allRefrence[i].Selected == true) {
                                $scope.Refrencecheckboxchcked.push($scope.allRefrence[i]);
                            }
                        }
                    }
                }
                if ($scope.allSources != "" && $scope.allSources != null) {
                    if ($scope.allSources.length > 0) {
                        for (var i = 0; i < $scope.allSources.length; i++) {
                            if ($scope.allSources[i].Selected == true) {
                                $scope.Sourcescheckboxchcked.push($scope.allSources[i]);
                            }
                        }
                    }
                }

                if ($scope.govtBondList != "" && $scope.govtBondList != null) {
                    if ($scope.govtBondList.length > 0) {
                        for (var i = 0; i < $scope.govtBondList.length; i++) {
                            if ($scope.govtBondList[i].Selected == true) {
                                $scope.SelectedGovtBonds.push($scope.govtBondList[i]);
                            }
                        }
                    }
                }



                var ActivityIDs = $scope.Activitycheckboxchcked;
                var RefrenceIds = $scope.Refrencecheckboxchcked;
                var SourcesIds = $scope.Sourcescheckboxchcked;

                if ($scope.prevSchoolDetails.length > 0) {
                    angular.forEach($scope.prevSchoolDetails, function (pr) {
                        pr.pacstpS_TCDate = new Date(pr.pacstpS_TCDate).toDateString();
                    })
                }

                var PrevSchoolDet = $scope.prevSchoolDetails;

                var StuGuardianDet = $scope.studentGuardianDetails;
                var StuSiblingDetails = $scope.studentSiblingDetails;

                if ($scope.obj.amsT_FatherAliveFlag === true) {
                    $scope.obj.amsT_FatherAliveFlag = "true";
                }
                else {
                    $scope.obj.amsT_FatherAliveFlag = "false";
                }

                if ($scope.obj.amsT_MotherAliveFlag === true) {
                    $scope.obj.amsT_MotherAliveFlag = "true";
                }
                else {
                    $scope.obj.amsT_MotherAliveFlag = "false";
                }

                if ($scope.obj.amsT_BPLCardFlag === true) {
                    $scope.obj.amsT_BPLCardFlag = 1;
                }
                else {
                    $scope.obj.amsT_BPLCardFlag = 0;
                }

                if ($scope.obj.amsT_HostelReqdFlag === true) {
                    $scope.obj.amsT_HostelReqdFlag = 1;
                }
                else {
                    $scope.obj.amsT_HostelReqdFlag = 0;
                }

                if ($scope.obj.amsT_TransportReqdFlag === true) {
                    $scope.obj.amsT_TransportReqdFlag = 1;
                }
                else {
                    $scope.obj.amsT_TransportReqdFlag = 0;
                }

                if ($scope.obj.amsT_GymReqdFlag === true) {
                    $scope.obj.amsT_GymReqdFlag = 1;
                }
                else {
                    $scope.obj.amsT_GymReqdFlag = 0;
                }

                if ($scope.obj.amsT_ECSFlag === true) {
                    $scope.obj.amsT_ECSFlag = 1;
                }
                else {
                    $scope.obj.amsT_ECSFlag = 0;
                }

                var amsT_Date = new Date($scope.amsT_Date).toDateString();
                var amsT_DOB = new Date($scope.obj.amsT_DOB).toDateString();

                var fathermobileno = $scope.mobiles;
                var fatheremailid = $scope.emails;
                var mothermobileno = $scope.mobiles1;
                var motheremailid = $scope.emails1;

                if ($scope.obj.amsT_FirstName == null) {
                    $scope.obj.amsT_FirstName = '';
                } else {
                    $scope.obj.amsT_FirstName = $scope.obj.amsT_FirstName;
                }

                if ($scope.obj.amsT_MiddleName == null) {
                    $scope.obj.amsT_MiddleName = '';
                } else {
                    $scope.obj.amsT_MiddleName = $scope.obj.amsT_MiddleName;
                }

                if ($scope.obj.amsT_LastName == null) {
                    $scope.obj.amsT_LastName = '';
                } else {
                    $scope.obj.amsT_LastName = $scope.obj.amsT_LastName;
                }

                //Student Sub caste
                if ($scope.obj.amsT_SubCasteIMC_Id == null) {
                    $scope.obj.amsT_SubCasteIMC_Id = '';
                }
                else {
                    $scope.obj.amsT_SubCasteIMC_Id = $scope.obj.amsT_SubCasteIMC_Id;
                }

                //Father Sub caste
                if ($scope.obj.amsT_FatherSubCaste == null) {
                    $scope.obj.amsT_FatherSubCaste = '';
                }
                else {
                    $scope.obj.amsT_FatherSubCaste = $scope.obj.amsT_FatherSubCaste;
                }

                //Mother Sub caste
                if ($scope.obj.amsT_MotherSubCaste == null) {
                    $scope.obj.amsT_MotherSubCaste = '';
                }
                else {
                    $scope.obj.amsT_MotherSubCaste = $scope.obj.amsT_MotherSubCaste;
                }






                var data = {
                    "AMST_Id": $scope.EditId,
                    "ASMAY_Id": $scope.obj.asmaY_Id,
                    "AMST_FirstName": $scope.obj.amsT_FirstName,
                    "AMST_MiddleName": $scope.obj.amsT_MiddleName,
                    "AMST_LastName": $scope.obj.amsT_LastName,
                    "AMST_Date": amsT_Date,
                    "AMST_RegistrationNo": $scope.obj.amsT_RegistrationNo,
                    "AMST_AdmNo": $scope.obj.amsT_AdmNo,
                    "AMC_Id": $scope.obj.stucategory,
                    "AMST_Sex": $scope.amsT_Sex,
                    "AMST_DOB": amsT_DOB,
                    "AMST_DOB_Words": $scope.obj.amsT_DOB_Words,
                    "PASR_Age": $scope.obj.pasR_Age,
                    "ASMCL_Id": $scope.obj.asmcL_Id,
                    "AMST_BloodGroup": $scope.obj.amsT_BloodGroup,
                    "AMST_MotherTongue": $scope.obj.amsT_MotherTongue,
                    "AMST_BirthCertNO": $scope.obj.amsT_BirthCertNO,
                    "IVRMMR_Id": $scope.obj.ivrmmR_Id,
                    "IMCC_Id": $scope.obj.imcC_Id,
                    "IMC_Id": $scope.obj.iC_Id,
                    "AMST_PerCountry": $scope.obj.IVRMMC_Id,
                    "AMST_PerStreet": $scope.obj.amsT_PerStreet,
                    "AMST_PerArea": $scope.obj.amsT_PerArea,
                    "AMST_PerCity": $scope.obj.amsT_PerCity,
                    "AMST_PerState": $scope.obj.amsT_PerState,
                    "AMST_PerPincode": $scope.obj.amsT_PerPincode,
                    "AMST_ConStreet": $scope.obj.amsT_ConStreet,
                    "AMST_ConArea": $scope.obj.amsT_ConArea,
                    "AMST_ConCity": $scope.obj.amsT_ConCity,
                    "AMST_ConState": $scope.obj.amsT_ConState,
                    "AMST_ConCountry": $scope.obj.amsT_ConCountry,
                    "AMST_ConPincode": $scope.obj.amsT_ConPincode,
                    "AMST_AadharNo": $scope.obj.amsT_AadharNo,
                    "AMST_StuBankAccNo": $scope.obj.amsT_StuBankAccNo,
                    "AMST_StuBankIFSC_Code": $scope.obj.amsT_StuBankIFSC_Code,
                    "AMST_StuCasteCertiNo": $scope.obj.amsT_StuCasteCertiNo,
                    "AMST_MobileNo": $scope.obj.amsT_MobileNo,
                    "AMST_emailId": $scope.obj.amsT_emailId,
                    "AMST_Photoname": $scope.obj.image,
                    "AMST_BirthPlace": $scope.obj.amsT_BirthPlace,
                    "AMST_FatherAliveFlag": $scope.obj.amsT_FatherAliveFlag,
                    "AMST_FatherName": $scope.obj.amsT_FatherName,
                    "AMST_FatherAadharNo": $scope.obj.amsT_FatherAadharNo,
                    "AMST_FatherSurname": $scope.obj.amsT_FatherSurname,
                    "AMST_FatherEducation": $scope.obj.amsT_FatherEducation,
                    "AMST_FatherOccupation": $scope.obj.amsT_FatherOccupation,
                    "AMST_FatherOfficeAdd": $scope.obj.amsT_FatherOfficeAdd,
                    "AMST_FatherDesignation": $scope.obj.amsT_FatherDesignation,
                    "AMST_FatherMonIncome": $scope.obj.amsT_FatherMonIncome,
                    "AMST_FatherAnnIncome": $scope.obj.amsT_FatherAnnIncome,
                    "AMST_FatherNationality": $scope.obj.IVRMMC_Id3,
                    "AMST_FatherMobleNo": $scope.obj.amsT_FatherMobleNo,
                    "AMST_FatheremailId": $scope.obj.amsT_FatheremailId,
                    "AMST_FatherBankAccNo": $scope.obj.amsT_FatherBankAccNo,
                    "AMST_FatherBankIFSC_Code": $scope.obj.amsT_FatherBankIFSC_Code,
                    "AMST_FatherCasteCertiNo": $scope.obj.amsT_FatherCasteCertiNo,
                    "ANST_FatherPhoto": $scope.fatherphoto,
                    "AMST_MotherAliveFlag": $scope.obj.amsT_MotherAliveFlag,
                    "AMST_MotherName": $scope.obj.amsT_MotherName,
                    "ANST_MotherPhoto": $scope.motherphoto,
                    "AMST_MotherAadharNo": $scope.obj.amsT_MotherAadharNo,
                    "AMST_MotherSurname": $scope.obj.amsT_MotherSurname,
                    "AMST_MotherEducation": $scope.obj.amsT_MotherEducation,
                    "AMST_MotherOccupation": $scope.obj.amsT_MotherOccupation,
                    "AMST_MotherOfficeAdd": $scope.obj.amsT_MotherOfficeAdd,
                    "AMST_MotherDesignation": $scope.obj.amsT_MotherDesignation,
                    "AMST_MotherMonIncome": $scope.obj.amsT_MotherMonIncome,
                    "AMST_MotherAnnIncome": $scope.obj.amsT_MotherAnnIncome,
                    "AMST_MotherNationality": $scope.obj.IVRMMC_Id4,
                    "AMST_MotherMobileNo": $scope.obj.amsT_MotherMobileNo,
                    "PACA_MotherEmailId": $scope.obj.amsT_MotherEmailId,
                    "AMST_MotherBankAccNo": $scope.obj.amsT_MotherBankAccNo,
                    "AMST_MotherBankIFSC_Code": $scope.obj.amsT_MotherBankIFSC_Code,
                    "AMST_MotherCasteCertiNo": $scope.obj.amsT_MotherCasteCertiNo,
                    "AMST_Nationality": $scope.obj.IVRMMC_Id,
                    "AMST_BPLCardFlag": $scope.obj.amsT_BPLCardFlag,
                    "AMST_BPLCardNo": $scope.obj.amsT_BPLCardNo,
                    "AMST_HostelReqdFlag": $scope.obj.amsT_HostelReqdFlag,
                    "AMST_TransportReqdFlag": $scope.obj.amsT_TransportReqdFlag,
                    "AMST_GymReqdFlag": $scope.obj.amsT_GymReqdFlag,
                    "AMST_ECSFlag": $scope.obj.amsT_ECSFlag,
                    "SelectedActivityDetails": ActivityIDs,
                    "SelectedRefrenceDetails": RefrenceIds,
                    "SelectedSourceDetails": SourcesIds,
                    "SelectedAchivementDetails": $scope.obj.amsteC_Extracurricular,
                    "SelectedBondDetails": $scope.SelectedGovtBonds,
                    "SelectedPrevSchoolDetails": PrevSchoolDet,
                    "SelectedSiblingDetails": StuSiblingDetails,
                    "SelectedGuardianDetails": StuGuardianDet,
                    "transnumconfigsettings": $scope.RegistrationNumbering,
                    "admissionNumbering": $scope.AdmissionNumbering,
                    "AMST_Father_Signature": $scope.fatherSign,
                    "AMST_Father_FingerPrint": $scope.fatherFingerprint,
                    "AMST_Mother_Signature": $scope.mothersign,
                    "AMST_Mother_FingerPrint": $scope.motherfingerprint,
                    "AMST_Concession_Type": $scope.obj.feeconcession,
                    "multiplemobileno": fathermobileno,
                    "multipleemailid": fatheremailid,
                    "multiplemobilenomother": mothermobileno,
                    "multipleemailidmother": motheremailid,
                    "Adm_M_Student_MobileNoDTO": $scope.mobilesstd,
                    "Adm_M_Student_EmailIdDTO": $scope.emailsstd,
                    "AMST_SubCasteIMC_Id": $scope.obj.amsT_SubCasteIMC_Id,
                    "AMST_MotherSubCaste": $scope.obj.amsT_MotherSubCaste,
                    "AMST_MotherCaste": $scope.obj.amsT_MotherCaste,
                    "AMST_MotherReligion": $scope.obj.amsT_MotherReligion,
                    "AMST_FatherSubCaste": $scope.obj.amsT_FatherSubCaste,
                    "AMST_FatherCaste": $scope.obj.amsT_FatherCaste,
                    "AMST_FatherReligion": $scope.obj.amsT_FatherReligion,
                    Uploaded_documentList: $scope.documentList,
                    guardianPhoto: $scope.studentGuardianDet,
                    guardianSign: $scope.studentGuardianDet,
                    guardianFingerprint: $scope.studentGuardianDet
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    },
                }
                apiService.create("StudentAdmission/", data).then(function (promise) {


                    if (promise.returnval == true && promise.message == "saved") {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval == true && promise.message == "updated") {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval == false) {
                        swal("Failed to Save/Update");

                    }

                })


            } else {
                $scope.submitted = true;
                $scope.submitted1 = true;
                $scope.submitted2 = true;
                $scope.submitted3 = true;
                $scope.submitted4 = true;
                $scope.disableSaveButton = false;
            }

        };









        $scope.SelectedFileForUploadzd = [];

        $scope.selectFileforUploadzd = function (input, document) {

            $scope.SelectedFileForUploadzd = input.files;
            //input.files[0].type == "image/jpeg" &&
            if (input.files && input.files[0]) {

                if (input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#' + document.amsmD_Id)
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }



        function Uploadprofiled(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", data.amsmD_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadStudentDocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.document_Path = d;

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });


        }

        //webcamera
        $scope.photoupload_flag = 'Default';
        $scope.WebcamGenrate = function () {
            $scope.photoupload_flag = 'Webcam';
            $scope.ImagePath = "";
            $scope.wbcamurl = "";
            Webcam.snap;
            Webcam.snap(function (data_uri) {
                $scope.wbcamurl = data_uri;
            });


            if ($scope.wbcamurl != "") {
                $scope.obj.image = $scope.wbcamurl;
            }
            $scope.uploadStudentProfilePic($scope.wbcamurl);
        }

        $scope.UploadStudentProfilePic = [];

        $scope.uploadStudentProfilePic = function (input, document) {

            $scope.wbcamurl = input;
            if ($scope.photoupload_flag == 'Default') {

            $scope.UploadStudentProfilePic = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blah')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

                }

            }
            else {
                // Uploadprofile($scope.wbcamurl);
                $scope.obj.image = $scope.wbcamurl;
            }
        }
        function Uploadprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadStudentProfilePic.length; i++) {
                formData.append("File", $scope.UploadStudentProfilePic[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.obj.image = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadGuardianPhoto = [];

        $scope.uploadGuardianPhoto = function (input, document) {

            $scope.UploadGuardianPhoto = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }
        function UploaddianPhoto(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianPhoto.length; i++) {
                formData.append("File", $scope.UploadGuardianPhoto[i]);
            }
            // We can send more data to server using append         
            //  formData.append("Id", data.amsmD_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadGuardianDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.pacstG_GuardianPhoto = d;

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });


        }

        $scope.UploadGuardianSign = [];

        $scope.uploadGuardianSign = function (input, document) {

            $scope.UploadGuardianSign = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {


                    UploaddianSign(document);

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }
        function UploaddianSign(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianSign.length; i++) {
                formData.append("File", $scope.UploadGuardianSign[i]);
            }
            // We can send more data to server using append         
            //  formData.append("Id", data.amsmD_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadGuardianDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.pacstG_GuardianSign = d;

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });


        }

        $scope.UploadGuardianFingerprint = [];

        $scope.uploadGuardianFingerprint = function (input, document) {

            $scope.UploadGuardianFingerprint = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {


                    UploaddianFingerprint(document);

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }



        function UploaddianFingerprint(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianFingerprint.length; i++) {
                formData.append("File", $scope.UploadGuardianFingerprint[i]);
            }
            // We can send more data to server using append         
            //  formData.append("Id", data.amsmD_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadGuardianDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.pacstG_Fingerprint = d;

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });


        }
        $scope.UploadFatherPhoto = [];

        $scope.uploadFatherPhoto = function (input) {

            $scope.UploadFatherPhoto = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadFatherProfile();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }
        function UploadFatherProfile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFatherPhoto.length; i++) {
                formData.append("File", $scope.UploadFatherPhoto[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.fatherphoto = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadFatherSignature = [];

        $scope.uploadFatherSignature = function (input) {

            $scope.UploadFatherSignature = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadFathersign();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function UploadFathersign() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFatherSignature.length; i++) {
                formData.append("File", $scope.UploadFatherSignature[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.fatherSign = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadFatherFingerprints = [];

        $scope.uploadFatherFingerprints = function (input) {

            $scope.UploadFatherFingerprints = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadFatherFingerprnts();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function UploadFatherFingerprnts() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFatherFingerprints.length; i++) {
                formData.append("File", $scope.UploadFatherFingerprints[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.fatherFingerprint = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadMotherphoto = [];

        $scope.uploadMotherphoto = function (input) {

            $scope.UploadMotherphoto = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadMotherProfilepic();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function UploadMotherProfilepic() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadMotherphoto.length; i++) {
                formData.append("File", $scope.UploadMotherphoto[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.motherphoto = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadMotherSign = [];

        $scope.uploadMotherSign = function (input) {

            $scope.UploadMotherSign = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadMothersignature();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function UploadMothersignature() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadMotherSign.length; i++) {
                formData.append("File", $scope.UploadMotherSign[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.mothersign = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        $scope.UploadMotherFingerprints = [];

        $scope.uploadMotherFingerprints = function (input) {

            $scope.UploadMotherFingerprints = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadMotherFing();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function UploadMotherFing() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadMotherFingerprints.length; i++) {
                formData.append("File", $scope.UploadMotherFingerprints[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.motherfingerprint = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }











        $scope.reload = function () {
            $state.reload();
        }


        //priview document
        $scope.showmodaldetails = function (data) {

            $('#preview').attr('src', data.document_Path);
        }

        $scope.showGuardianPhoto = function (data) {

            $('#preview').attr('src', data.amstG_GuardianPhoto);
        }

        $scope.showGuardianSign = function (data) {

            $('#preview').attr('src', data.amstG_GuardianSign);
        }

        $scope.showGuardianFingerprint = function (data) {

            $('#preview').attr('src', data.amstG_Fingerprint);
        }

        $scope.showfatherPhoto = function (fatherphoto) {
            $('#preview').attr('src', fatherphoto);
        }

        $scope.showfathersign = function (fatherSign) {

            $('#preview').attr('src', fatherSign);
        }

        $scope.showfatherfingerprint = function (fatherFingerprint) {

            $('#preview').attr('src', fatherFingerprint);
        }

        $scope.showmotherprofilepic = function (motherphoto) {

            $('#preview').attr('src', motherphoto);
        }

        $scope.showmothersign = function (mothersign) {

            $('#preview').attr('src', mothersign);
        }

        $scope.showmotherfingerprint = function (motherfingerprint) {
            $('#preview').attr('src', motherfingerprint);
        }






        //Clear functionality
        $scope.clear_first_tab = function (data) {

            $scope.obj.ACQ_Id = "";
            $scope.obj.ACQC_Id = "";

            $scope.obj.amsT_StuBankAccNo = "";
            $scope.lblclasssection = "";

            $scope.obj.amsT_BPLCardFlag = "";
            if ($scope.EditId > 0) {

            }
            else {
                $scope.obj.stucategory = "";
                $scope.obj.amcO_Id = "";
                $scope.obj.pacA_Age = "";
                $scope.obj.amsT_BPLCardNo = "";
            }

            $scope.obj.PACA_FirstName = "";
            $scope.obj.PACA_MiddleName = "";
            $scope.obj.PACA_LastName = "";
            $scope.obj.PACA_AadharNo = "";

            $scope.obj.AMCO_Id = "";
            $scope.obj.AMB_Id = "";
            $scope.obj.AMSE_Id = "";
            $scope.obj.PACA_DOB = "";
            $scope.obj.IMCC_Id = "";
            $scope.obj.IMC_Id = "";
            $scope.obj.PACA_StudentSubCaste = "";
            $scope.obj.PACA_Age = "";
            $scope.obj.PACA_MotherTongue = "";
            $scope.obj.PACA_Nationality = "";
            $scope.obj.PACA_CoutryCode = "";
            $scope.obj.PACA_MobileNo = "";
            $scope.obj.PACA_MobileNo = "";
            $scope.obj.PACA_emailId = "";
            $scope.obj.PACA_AlterNativeEmialId = "";

            $scope.PACA_AadharNo = "";
            $scope.PACA_BloodGroup = "";
            $scope.PACA_BirthPlace = "";
            $scope.ACQ_Id = "";
            $scope.ACQC_Id = "";
            $scope.PACA_District = "";
            $scope.PACA_Urban_Rural = "";
            $scope.obj.IVRMMR_Id = "";



            for (var i = 0; i < $scope.mobiles.length; i++) {
                $scope.obj.amB_Id = "";
            }



            $scope.emailsstd = [];
            $scope.emailstd = {};
            $scope.emailsstd = [{ id: 'emailsstd' }];
            $scope.emailsstd[0].pacA_emailId = "";

            $scope.mobilesstd = {};
            $scope.mobilesstd = [{ id: 'mobilesstd' }];
            $scope.mobilesstd[0].pacA_MobileNo = "";


            $scope.search = "";
            angular.forEach(
                angular.element("input[type='file']"),
                function (inputElem) {
                    angular.element(inputElem).val(null);
                });
            $scope.obj.image = "";
            $('#blah').removeAttr('src');
            $scope.obj.feeconcession = "";
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();

        }

        $scope.clear_second_tab = function (data) {
            //  $state.reload();

            $scope.obj.PACA_PerStreet = "";
            $scope.obj.PACA_PerArea = "";
            $scope.obj.IVRMMC_Id = "";
            $scope.obj.PACA_PerState = "";
            $scope.obj.PACA_PerCity = "";
            $scope.obj.PACA_PerPincode = "";
            $scope.obj.PACA_ConStreet = "";
            $scope.obj.PACA_ConCountryId = "";
            $scope.obj.PACA_ConState = "";
            $scope.obj.PACA_ConCity = "";
            $scope.obj.PACA_ConPincode = "";
            $scope.obj.chkbox_address = "";
            $scope.search = "";
            $scope.obj.chkbox_address = false;
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();


        }

        $scope.clear_third_tab = function (data) {
            $scope.obj.PACA_FatherName = "";
            $scope.obj.PACA_FatherSurname = "";
            $scope.obj.PACA_FatherAadharNo = "";
            $scope.obj.PACA_FatherEducation = "";
            $scope.obj.PACA_FatherOccupation = "";
            $scope.obj.PACA_FatherMonIncome = "";
            $scope.obj.PACA_FatherAnnIncome = "";
            $scope.obj.PACA_FatherEmailId = "";

            $scope.obj.PACA_FatherCountryCode = "";
            $scope.obj.PACA_FatherMobleNo = "";
            $scope.obj.PACA_MotherName = "";
            $scope.obj.PACA_MotherSurname = "";
            $scope.obj.PACA_MotherAadharNo = "";
            $scope.obj.PACA_MotherEducation = "";
            $scope.obj.PACA_MotherOccupation = "";
            $scope.obj.PACA_MotherMonIncome = "";
            $scope.obj.PACA_MotherAnnIncome = "";
            $scope.obj.PACA_MotherCountryCode = "";
            $scope.obj.PACA_MotherMobleNo = "";
            $scope.obj.PACA_MotherEmailId = "";


            //$scope.obj.amsT_MotherAadharNo = "";
            //$scope.obj.amsT_MotherEducation = "";
            //$scope.obj.amsT_MotherOfficeAdd = "";
            //$scope.obj.amsT_MotherOccupation = "";
            //$scope.obj.amsT_MotherDesignation = "";
            //$scope.obj.amsT_MotherBankAccNo = "";
            //$scope.obj.amsT_MotherBankIFSC_Code = "";
            //$scope.obj.amsT_MotherCasteCertiNo = "";
            //$scope.obj.IVRMMC_Id4 = "";
            //$scope.obj.amsT_MotherMonIncome = "";
            //$scope.obj.amsT_MotherAnnIncome = "";
            //$scope.obj.amsT_MotherMobileNo = "";
            //$scope.obj.amsT_MotherEmailId = "";
            //$scope.search = "";
            //$scope.submitted3 = false;
            //$scope.myForm3.$setPristine();
            //$scope.myForm3.$setUntouched();

            //$scope.obj.amsT_MotherSubCaste = "";
            //$scope.obj.amsT_MotherCaste = "";
            //$scope.obj.amsT_MotherReligion = "";

            //$scope.obj.amsT_FatherSubCaste = "";
            //$scope.obj.amsT_FatherCaste = "";
            //$scope.obj.amsT_FatherReligion = "";


            $scope.mobiles = {};
            $scope.mobiles = [{ id: 'mobile1' }];
            $scope.mobiles[0].pacA_FatherMobleNo = "";

            $scope.emails = {};
            $scope.emails = [{ id: 'email1' }];
            $scope.emails[0].pacA_FatheremailId = "";

            $scope.mobiles1 = {};
            $scope.mobiles1 = [{ id: 'mobile2' }];
            $scope.mobiles1[0].pacA_MotherMobleNo = "";

            $scope.emails1 = {};
            $scope.emails1 = [{ id: 'email2' }];
            $scope.emails1[0].pacA_MotheremailId = "";

        }

        var name = "";

        $scope.clear_fourth_tab = function () {
            $scope.obj.amsteC_Extracurricular = "";
            $scope.search = "";

            for (var i = 0; i < $scope.allActivity.length; i++) {
                $scope.allActivity[i].Selected = false;
            }


            //for (var i = 0; i < $scope.allRefrence.length; i++) {
            //    $scope.allRefrence[i].Selected = false;
            //}



            //for (var i = 0; i < $scope.allSources.length; i++) {
            //    $scope.allSources[i].Selected = false;
            //}

            //for (var i = 0; i < $scope.govtBondList.length; i++) {
            //    $scope.govtBondList[i].Selected = false;
            //    $scope.govtBondList[i].amstB_BandNo = "";
            //}


            for (var i = 0; i < $scope.prevSchoolDetails.length; i++) {

                $scope.prevSchoolDetails[i].pacstpS_PrvSchoolName = "";
                $scope.prevSchoolDetails[i].pacstpS_PreviousExamPassed = "";
                $scope.prevSchoolDetails[i].pacstpS_Urbanrural = "";
                $scope.prevSchoolDetails[i].pacstpS_Attempts = "";
                $scope.prevSchoolDetails[i].pacstpS_PreSchoolType = "";
                $scope.prevSchoolDetails[i].pacstpS_PreviousClass = "";
                $scope.prevSchoolDetails[i].pacstpS_PreviousPer = "";
                $scope.prevSchoolDetails[i].pacstpS_PreviousGrade = "";
                $scope.prevSchoolDetails[i].pacstpS_LeftYear = "";
                $scope.prevSchoolDetails[i].pacstpS_PreviousMarks = "";
                $scope.prevSchoolDetails[i].pacstpS_PreviousMarksObtained = "";
                $scope.prevSchoolDetails[i].pacstpS_PreviousTCNo = "";
                $scope.prevSchoolDetails[i].pacstpS_TCDate = "";
                $scope.prevSchoolDetails[i].pacstpS_PreSchoolBoard = "";
                $scope.prevSchoolDetails[i].pacstpS_PreSchoolCountry = "";
                $scope.prevSchoolDetails[i].pacstpS_PreSchoolState = "";
                $scope.prevSchoolDetails[i].pacstpS_Address = "";
                $scope.prevSchoolDetails[i].pacstpS_LeftReason = "";
                $scope.prevSchoolDetails[i].pacstpS_MediumOfInst = "";

                $scope.prevSchoolDetails[i].pacstpS_PreviousBranch = "";
                $scope.prevSchoolDetails[i].pacstpS_PasssedMonthYear = "";
                $scope.prevSchoolDetails[i].pacstpS_LanguagesTaken = "";
                $scope.prevSchoolDetails[i].pacstpS_Regno = "";
                $scope.prevSchoolDetails[i].pacstpS_PreviousGrade = "";



            }


            for (var i = 0; i < $scope.compexamstudetails.length; i++) {
                $scope.prevexammarksdetails[i].pamcexM_Id = "";
                $scope.prevexammarksdetails[i].pacstceM_RollNo = "";
                $scope.prevexammarksdetails[i].PACSTCEM_RegistrationId = "";
                $scope.prevexammarksdetails[i].pacstceM_MeritNo = "";
                $scope.prevexammarksdetails[i].PACSTCEM_TotalMaxMarks = "";
                $scope.prevexammarksdetails[i].pacstsuM_SubjectMarks = "";
                $scope.prevexammarksdetails[i].PACSTCEM_CategoryRank = "";
                $scope.prevexammarksdetails[i].PACSTCEM_Percentage = "";
                $scope.prevexammarksdetails[i].PACSTCEM_Percentile = "";
            }

            for (var i = 0; i < $scope.compexammarksdetails.length; i++) {
                $scope.prevexammarksdetails[i].pamcexM_Id = "";
                $scope.prevexammarksdetails[i].pamcexmsuB_Id = "";
                $scope.prevexammarksdetails[i].pamcexmsuB_MaxMarks = "";
                $scope.prevexammarksdetails[i].pacstsuM_SubjectMarks = "";

            }

            //
            for (var i = 0; i < $scope.prevexammarksdetails.length; i++) {
                $scope.prevexammarksdetails[i].pacstsuM_SubjectName = "";
                $scope.prevexammarksdetails[i].pacstsuM_MaxMarks = "";
                $scope.prevexammarksdetails[i].pacstsuM_SubjectMarks = "";
                $scope.prevexammarksdetails[i].pacstsuM_Percentage = "";
                $scope.prevexammarksdetails[i].pacstsuM_LangFlg = "";
            }


            for (var i = 0; i < $scope.prevexammarksdetails.length; i++) {
                $scope.prevexammarksdetails[i].pacstsuM_SubjectName = "";
                $scope.prevexammarksdetails[i].pacstsuM_MaxMarks = "";
                $scope.prevexammarksdetails[i].pacstsuM_SubjectMarks = "";
                $scope.prevexammarksdetails[i].pacstsuM_Percentage = "";
                $scope.prevexammarksdetails[i].pacstsuM_LangFlg = "";
            }



            for (var i = 0; i < $scope.studentGuardianDetails.length; i++) {

                $scope.studentGuardianDetails[i].pacstG_GuardianName = "";
                $scope.studentGuardianDetails[i].pacstG_GuardianAddress = "";
                $scope.studentGuardianDetails[i].pacstG_emailid = "";
                $scope.studentGuardianDetails[i].pacstG_GuardianPhoneNo = "";
                $scope.studentGuardianDetails[i].pacstG_GuardianPhoto = null;
                $scope.studentGuardianDetails[i].pacstG_GuardianSign = null;
                $scope.studentGuardianDetails[i].pacstG_Fingerprint = null;



            }

            for (var i = 0; i < $scope.studentSiblingDetails.length; i++) {

                $scope.studentSiblingDetails[i].acstS_SiblingsName = "";
                $scope.studentSiblingDetails[i].amcO_Id = "";
                $scope.studentSiblingDetails[i].acstS_SiblingsRelation = "";

            }

            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();




        }

        $scope.clear_fifth_tab = function () {

            for (var i = 0; i < $scope.studentGuardianDetails.length; i++) {
                $scope.studentGuardianDetails[i].pacstG_GuardianName = "";
                $scope.studentGuardianDetails[i].pacstG_GuardianAddress = "";
                $scope.studentGuardianDetails[i].pacstG_emailid = "";
                $scope.studentGuardianDetails[i].pacstG_GuardianPhoneNo = "";
                $scope.studentGuardianDetails[i].PACSTG_CoutryCode = "";
            }

            for (var i = 0; i < $scope.prevSchoolDetails.length; i++) {
                $scope.obj.PACA_PassportNo = "";
                $scope.obj.PACA_PassportIssuedAt = "";
                $scope.obj.PACA_PassportIssueDate = "";
                $scope.obj.PACA_PassportIssuedPlace = "";
                $scope.obj.PACA_PassportIssuedCounrty = "";

                $scope.obj.PACA_VISAIssuedBy = "";
                $scope.obj.PACA_VISAValidFrom = "";


                $scope.obj.PACA_VISAValidTo = "";

            }



            $scope.search = "";

            angular.forEach(
                angular.element("input[type='file']"),
                function (inputElem) {
                    angular.element(inputElem).val(null);
                });


            for (var i = 0; i < $scope.documentList.length; i++) {


                $scope.documentList[i].document_Path = '';
            }

            $scope.fatherphoto = null;
            $scope.UploadFatherPhoto.fatherSign = null;


            $scope.fatherSign = null;
            $scope.fatherFingerprint = null;
            $scope.motherphoto = null;
            $scope.mothersign = null;
            $scope.motherfingerprint = null;




            $scope.submitted = false;
            $scope.submitted1 = false;
            $scope.submitted2 = false;
            $scope.submitted3 = false;
            $scope.submitted4 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
        };

        $scope.clear_sixth_tab = function () {
            studentGuardianDetails
        }

        $scope.getcaste = function (imcC_Id) {
            if (imcC_Id != "") {
                var data = {
                    "IMCC_Id": imcC_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getCaste", data).then(function (promise) {

                    if (promise.allCaste.length > 0) {
                        $scope.allCaste = promise.allCaste;
                        $scope.castedisble = false;
                    }
                    else {
                        swal("No Caste is mapped to selected Caste Category");
                        $scope.castedisble = true;
                        $scope.obj.IMC_Id = "";
                    }

                })
            }
        }

        $scope.clearAll = function () {
            $state.reload();
            $scope.scroll();
        }


        //added by vishnu
        $scope.headinstallmentid = function (totalgrid, index) {

            $scope.fineslb = [{ value: "Per Day", FTFS_FineType: "Per Day" },
            { value: "Day Slab", FTFS_FineType: "Day Slab" }
            ];


            $scope.fineslbecs = [{ value: "Per Day", FTFSE_FineType: "Per Day" },
            { value: "Day Slab", FTFSE_FineType: "Day Slab" }
            ];

            $scope.typeofstudentregular = false;

            headid = totalgrid[index].fmH_Id;
            ftiid = totalgrid[index].ftI_Id;

            var data = {
                "FMG_Id": $scope.FMG_Id,
                "FMCC_Id": $scope.FMCC_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeAmountEntry/getgroupmappedheads", data).
                then(function (promise) {

                    $scope.savedatatransregular = [];
                    $scope.savedatatransecs = [];
                    var r = 0;
                    var e = 0;
                    if (promise.fineslabdetails.length > 0) {
                        $scope.slab1 = true;
                        $scope.slab2 = true;

                        var lenofarray = promise.fineslabdetails.length;

                        for (var i = 0; i < lenofarray; i++) {
                            if (promise.fineslabdetails[i].fmfS_ECSFlag == "R" && promise.fineslabdetails[i].fmH_Id == headid && promise.fineslabdetails[i].ftI_Id == ftiid) {
                                $scope.savedatatransregular[r] = promise.fineslabdetails[i]
                                r = r + 1;
                            }
                        }

                        var lenofarrayecs = promise.fineslabdetailsecs.length;
                        for (var i = 0; i < lenofarrayecs; i++) {
                            if (promise.fineslabdetailsecs[i].fmfS_ECSFlag == "E" && promise.fineslabdetailsecs[i].fmH_Id == headid && promise.fineslabdetailsecs[i].ftI_Id == ftiid) {
                                $scope.savedatatransecs[e] = promise.fineslabdetailsecs[i]
                                e = e + 1;
                            }
                        }


                        if ($scope.savedatatransregular.length > 0) {
                            $scope.typeofstudentregular = true;
                            $scope.slab1 = true;
                            $scope.slab2 = false;
                            $scope.finslab1 = $scope.savedatatransregular;

                        }
                        if ($scope.savedatatransecs.length > 0) {
                            $scope.typeofstudentecs = true;
                            $scope.slab2 = true;
                            $scope.slab1 = true;
                            $scope.finslab2 = $scope.savedatatransecs;
                        }
                    }
                    else {
                        $scope.slab1 = false;
                        $scope.slab2 = false;
                    }
                })

        }


        $scope.getstudentlistre = function () {
            var data = {
                "ASMAY_Id": $scope.yearid1,
            }
            apiService.create("StudentAdmission/yearwisetcstd/", data).then
                (function (promise) {
                    if (promise != null) {
                        $scope.stdname = promise.studentList1;
                    }
                    else {
                        swal("No Records Found From Selected Year..")
                    }
                });
        }


        $scope.addtocart = function () {

            var data = {
                "ASMAY_Id": $scope.yearid1,
                "AMST_Id": $scope.amsT_Id,
            }
            apiService.create("StudentAdmission/addtocart/", data).then
                (function (promise) {

                    $scope.documentList = promise.documentList;
                    $scope.DOB = false;
                    $scope.mi_id = promise.mI_Id;



                    if (promise.bondList.length > 0 && promise.bondList != null) {
                        $scope.bondList = promise.bondList;
                    }
                    if (promise.prevSchoolDetails.length > 0 && promise.prevSchoolDetails != null) {
                        $scope.prevSchoolDetails = promise.prevSchoolDetails;
                    }
                    if (promise.studentGuardianDetails.length > 0 && promise.studentGuardianDetails != null) {
                        $scope.studentGuardianDetails = promise.studentGuardianDetails;
                    }

                    if (promise.studentSiblingDetails.length > 0 && promise.studentSiblingDetails != null) {

                        $scope.studentSiblingDetails = promise.studentSiblingDetails;
                    }

                    if (promise.Adm_College_Student_SubjectMarksDTO.length > 0 && promise.Adm_College_Student_SubjectMarksDTO != null) {
                        $scope.prevexammarksdetails = promise.Adm_College_Student_SubjectMarksDTO;
                    }

                    $scope.studentReferenceDetails = promise.studentReferenceDetails;
                    $scope.studentSourceDetails = promise.studentSourceDetails;
                    $scope.studentActivityDetails = promise.studentActivityDetails;


                    for (var i = 0; i < $scope.allRefrence.length; i++) {
                        $scope.allRefrence[i].Selected = false;
                    }
                    for (var i = 0; i < $scope.allRefrence.length; i++) {
                        name = $scope.allRefrence[i].pamR_Id;
                        for (var j = 0; j < promise.studentReferenceDetails.length; j++) {
                            if (name == promise.studentReferenceDetails[j].pamR_Id) {
                                $scope.allRefrence[i].Selected = true;
                            }
                        }
                    }
                    for (var i = 0; i < $scope.allSources.length; i++) {
                        $scope.allSources[i].Selected = false;
                    }
                    for (var i = 0; i < $scope.allSources.length; i++) {
                        name = $scope.allSources[i].pamS_Id;
                        for (var j = 0; j < promise.studentSourceDetails.length; j++) {
                            if (name == promise.studentSourceDetails[j].pamS_Id) {
                                $scope.allSources[i].Selected = true;
                            }
                        }
                    }
                    for (var i = 0; i < $scope.allActivity.length; i++) {
                        $scope.allActivity[i].Selected = false;
                    }
                    for (var i = 0; i < $scope.allActivity.length; i++) {
                        name = $scope.allActivity[i].amA_Id;
                        for (var j = 0; j < promise.studentActivityDetails.length; j++) {
                            if (name == promise.studentActivityDetails[j].amA_Id) {
                                $scope.allActivity[i].Selected = true;
                            }
                        }
                    }
                    //documnets
                    if (promise.documentList.length > 0) {
                        $scope.document = {};
                        $scope.documentList = promise.documentList;
                        angular.forEach(promise.documentList, function (value, key) {
                            $('#' + value.amsmD_Id).attr('src', value.document_Path);
                        })
                    }


                    $('#blah').attr('src', promise.amsT_Photoname);

                    $scope.Editfatherphoto = promise.ansT_FatherPhoto;
                    $scope.EditfatherSign = promise.amsT_Father_Signature;
                    $scope.EditfatherFingerprint = promise.amsT_Father_FingerPrint;
                    $scope.Editmotherphoto = promise.ansT_MotherPhoto;
                    $scope.Editmothersign = promise.amsT_Mother_Signature;
                    $scope.Editmotherfingerprint = promise.amsT_Mother_FingerPrint;

                    $scope.fatherphoto = promise.studentList[0].ansT_FatherPhoto;
                    $scope.fatherSign = promise.studentList[0].amsT_Father_Signature;
                    $scope.fatherFingerprint = promise.studentList[0].amsT_Father_FingerPrint;
                    $scope.motherphoto = promise.studentList[0].ansT_MotherPhoto;
                    $scope.mothersign = promise.studentList[0].amsT_Mother_Signature;
                    $scope.motherfingerprint = promise.studentList[0].amsT_Mother_FingerPrint;
                    $scope.obj.image = promise.studentList[0].amsT_Photoname;

                    $scope.obj.amsT_FirstName = promise.studentList[0].amsT_FirstName;

                    $scope.obj.amsT_MiddleName = promise.studentList[0].amsT_MiddleName;
                    $scope.obj.amsT_LastName = promise.studentList[0].amsT_LastName;
                    $scope.amsT_Date = new Date(promise.studentList[0].amsT_Date);

                    $scope.obj.asmcL_Id = promise.studentList[0].asmcL_Id;
                    if (promise.stud_catg_edit.length > 0) {
                        $scope.obj.stucategory = promise.stud_catg_edit[0].asmcC_Id;
                    }
                    $scope.amsT_Sex = promise.studentList[0].amsT_Sex;
                    $scope.obj.amsT_DOB = new Date(promise.studentList[0].amsT_DOB);
                    $scope.obj.amsT_DOB_Words = promise.studentList[0].amsT_DOB_Words;
                    $scope.obj.pasR_Age = promise.studentList[0].pasR_Age;
                    $scope.obj.amsT_BloodGroup = promise.studentList[0].amsT_BloodGroup;
                    $scope.obj.amsT_MotherTongue = promise.studentList[0].amsT_MotherTongue;
                    $scope.obj.amsT_BirthCertNO = promise.studentList[0].amsT_BirthCertNO;
                    $scope.obj.ivrmmR_Id = promise.studentList[0].ivrmmR_Id;


                    $scope.obj.imcC_Id = promise.studentList[0].imcC_Id;
                    for (var i = 0; i < $scope.allCaste.length; i++) {
                        $scope.allCaste[i].Selected = false;
                        $scope.obj.iC_Id = "";
                    }


                    if (promise.allCaste.length > 0) {
                        for (var i = 0; i < promise.allCaste.length; i++) {
                            if (promise.studentList[0].iC_Id == promise.allCaste[i].imC_Id) {
                                $scope.allCaste[i].Selected = true;
                                $scope.obj.iC_Id = promise.studentList[0].iC_Id;
                            }
                        }
                    }
                    else {
                        swal("Something has gone wrong.Please check Master Caste Category and Master Caste");
                    }

                    $scope.obj.IVRMMC_Id = promise.studentList[0].amsT_Nationality;

                    $scope.obj.IVRMMC_Id5 = promise.studentList[0].amsT_PerCountry;


                    getSelectGetState($scope.obj.IVRMMC_Id5, promise.studentList[0].amsT_PerState);

                    $scope.obj.amsT_PerState = promise.studentList[0].amsT_PerState;

                    $scope.obj.amsT_PerStreet = promise.studentList[0].amsT_PerStreet;
                    $scope.obj.amsT_PerArea = promise.studentList[0].amsT_PerArea;
                    $scope.obj.amsT_PerCity = promise.studentList[0].amsT_PerCity;

                    $scope.obj.amsT_PerPincode = promise.studentList[0].amsT_PerPincode;

                    $scope.obj.amsT_StuBankAccNo = promise.studentList[0].amsT_StuBankAccNo;
                    $scope.obj.amsT_StuBankIFSC_Code = promise.studentList[0].amsT_StuBankIFSC_Code;
                    $scope.obj.amsT_AadharNo = promise.studentList[0].amsT_AadharNo;
                    $scope.obj.amsT_BirthPlace = promise.studentList[0].amsT_BirthPlace;
                    $scope.obj.amsT_StuCasteCertiNo = promise.studentList[0].amsT_StuCasteCertiNo;
                    $scope.obj.amsT_MobileNo = promise.studentList[0].amsT_MobileNo;
                    $scope.obj.amsT_emailId = promise.studentList[0].amsT_emailId;

                    $scope.obj.amsT_PerStreet = promise.studentList[0].amsT_PerStreet;
                    $scope.obj.amsT_ConPincode = promise.studentList[0].amsT_ConPincode;
                    $scope.obj.amsT_ConArea = promise.studentList[0].amsT_ConArea;
                    $scope.obj.amsT_ConStreet = promise.studentList[0].amsT_ConStreet;
                    $scope.obj.amsT_ConCity = promise.studentList[0].amsT_ConCity;
                    $scope.obj.amsT_ConCountry = promise.studentList[0].amsT_ConCountry;

                    getSelectGetState2($scope.obj.amsT_ConCountry, promise.studentList[0].amsT_ConState);

                    $scope.obj.amsT_ConState = promise.studentList[0].amsT_ConState;




                    if (promise.studentList[0].amsT_FatherAliveFlag === "true") {
                        $scope.obj.amsT_FatherAliveFlag = true;
                    }
                    else {
                        $scope.obj.amsT_FatherAliveFlag = false;
                    }

                    $scope.obj.amsT_FatherName = promise.studentList[0].amsT_FatherName;
                    $scope.obj.amsT_FatherSurname = promise.studentList[0].amsT_FatherSurname;
                    $scope.obj.amsT_FatherAadharNo = promise.studentList[0].amsT_FatherAadharNo;
                    $scope.obj.amsT_FatherEducation = promise.studentList[0].amsT_FatherEducation;
                    $scope.obj.amsT_FatherOfficeAdd = promise.studentList[0].amsT_FatherOfficeAdd;
                    $scope.obj.amsT_FatherOccupation = promise.studentList[0].amsT_FatherOccupation;
                    $scope.obj.amsT_FatherDesignation = promise.studentList[0].amsT_FatherDesignation;
                    $scope.obj.amsT_FatherBankAccNo = promise.studentList[0].amsT_FatherBankAccNo;
                    $scope.obj.amsT_FatherBankIFSC_Code = promise.studentList[0].amsT_FatherBankIFSC_Code;
                    $scope.obj.amsT_FatherCasteCertiNo = promise.studentList[0].amsT_FatherCasteCertiNo;
                    $scope.obj.IVRMMC_Id3 = promise.studentList[0].amsT_FatherNationality;
                    $scope.obj.amsT_FatherMonIncome = promise.studentList[0].amsT_FatherMonIncome;
                    $scope.obj.amsT_FatherAnnIncome = promise.studentList[0].amsT_FatherAnnIncome;
                    $scope.obj.amsT_FatherMobleNo = promise.studentList[0].amsT_FatherMobleNo;
                    $scope.obj.amsT_FatheremailId = promise.studentList[0].amsT_FatheremailId;



                    if (promise.studentList[0].amsT_MotherAliveFlag === "true") {
                        $scope.obj.amsT_MotherAliveFlag = true;
                    }
                    else {
                        $scope.obj.amsT_MotherAliveFlag = false;
                    }

                    $scope.obj.amsT_MotherName = promise.studentList[0].amsT_MotherName;
                    $scope.obj.amsT_MotherSurname = promise.studentList[0].amsT_MotherSurname;
                    $scope.obj.amsT_MotherAadharNo = promise.studentList[0].amsT_MotherAadharNo;
                    $scope.obj.amsT_MotherEducation = promise.studentList[0].amsT_MotherEducation;
                    $scope.obj.amsT_MotherOfficeAdd = promise.studentList[0].amsT_MotherOfficeAdd;
                    $scope.obj.amsT_MotherOccupation = promise.studentList[0].amsT_MotherOccupation;
                    $scope.obj.amsT_MotherDesignation = promise.studentList[0].amsT_MotherDesignation;
                    $scope.obj.amsT_MotherBankAccNo = promise.studentList[0].amsT_MotherBankAccNo;
                    $scope.obj.amsT_MotherBankIFSC_Code = promise.studentList[0].amsT_MotherBankIFSC_Code;
                    $scope.obj.amsT_MotherCasteCertiNo = promise.studentList[0].amsT_MotherCasteCertiNo;
                    $scope.obj.IVRMMC_Id4 = promise.studentList[0].amsT_MotherNationality;
                    $scope.obj.amsT_MotherMonIncome = promise.studentList[0].amsT_MotherMonIncome;
                    $scope.obj.amsT_MotherAnnIncome = promise.studentList[0].amsT_MotherAnnIncome;
                    $scope.obj.amsT_MotherMobileNo = promise.studentList[0].amsT_MotherMobileNo;
                    $scope.obj.amsT_MotherEmailId = promise.studentList[0].amsT_MotherEmailId;

                    if (promise.studentAchivementDetails.length > 0) {
                        $scope.obj.amsteC_Extracurricular = promise.studentAchivementDetails[0].amsteC_Extracurricular;
                    }

                    $scope.obj.amsT_BPLCardFlag = promise.studentList[0].amsT_BPLCardFlag;
                    if (promise.studentList[0].amsT_BPLCardFlag == 1) {

                        $scope.obj.amsT_BPLCardFlag = true;
                    }
                    else {
                        $scope.obj.amsT_BPLCardFlag = false;
                    }

                    $scope.obj.amsT_BPLCardNo = promise.studentList[0].amsT_BPLCardNo;
                    $scope.obj.amsT_HostelReqdFlag = promise.studentList[0].amsT_HostelReqdFlag;
                    if ($scope.obj.amsT_HostelReqdFlag === 1) {
                        $scope.obj.amsT_HostelReqdFlag = true;
                    }
                    else {
                        $scope.obj.amsT_HostelReqdFlag = false;
                    }
                    $scope.obj.amsT_TransportReqdFlag = promise.studentList[0].amsT_TransportReqdFlag;
                    if ($scope.obj.amsT_TransportReqdFlag === 1) {
                        $scope.obj.amsT_TransportReqdFlag = true;
                    }
                    else {
                        $scope.obj.amsT_TransportReqdFlag = false;
                    }
                    $scope.obj.amsT_GymReqdFlag = promise.studentList[0].amsT_GymReqdFlag;
                    if ($scope.obj.amsT_GymReqdFlag === 1) {
                        $scope.obj.amsT_GymReqdFlag = true;
                    }
                    else {
                        $scope.obj.amsT_GymReqdFlag = false;
                    }
                    $scope.obj.amsT_ECSFlag = promise.studentList[0].amsT_ECSFlag;
                    if ($scope.obj.amsT_ECSFlag === 1) {
                        $scope.obj.amsT_ECSFlag = true;
                    }
                    else {
                        $scope.obj.amsT_ECSFlag = false;
                    }
                    $('#myModalreadmit').modal('hide');
                    $scope.readmit = false;
                    $scope.readmitreload();
                    $scope.amsT_Id = "";
                });
        }

        $scope.prevSchoolsubjectEntered = function (index) {
            if ($scope.EditId > 0) {
                if ($scope.prevexammarksdetailscount == 0) {

                    if ($scope.prevexammarksdetails[index].pacstsuM_SubjectName == "" || $scope.prevexammarksdetails[index].pacstsuM_SubjectName == null || $scope.prevexammarksdetails[index].pacstsuM_SubjectName == undefined) {

                        $scope.prevexammarksdetails[index].pacstsuM_MaxMarks = "";
                        $scope.prevexammarksdetails[index].pacstsuM_SubjectMarks = "";
                        $scope.prevexammarksdetails[index].pacstsuM_Percentage = "";
                        $scope.prevexammarksdetails[index].pacstsuM_LangFlg = "";
                    }
                }
                else {
                    if ($scope.prevexammarksdetails[index].pacstsuM_SubjectName == "" || $scope.prevexammarksdetails[index].pacstsuM_SubjectName == null || $scope.prevexammarksdetails[index].pacstsuM_SubjectName == undefined) {
                        $scope.errorMessage2e = 'Subject Name Required';
                    }
                    else {
                        $scope.errorMessage2e = '';
                    }
                }
            }
            else {
                if ($scope.prevexammarksdetails[index].pacstsuM_SubjectName == "" || $scope.prevexammarksdetails[index].pacstsuM_SubjectName == null || $scope.prevexammarksdetails[index].pacstsuM_SubjectName == undefined) {
                    $scope.prevexammarksdetails[index].pacstsuM_MaxMarks = "";
                    $scope.prevexammarksdetails[index].pacstsuM_SubjectMarks = "";
                    $scope.prevexammarksdetails[index].pacstsuM_Percentage = "";
                    $scope.prevexammarksdetails[index].pacstsuM_LangFlg = "";
                }
            }
        };

        $scope.removeall = function () {
            $('#myModalreadmit').modal('hide');
            $scope.readmit = false;
            $scope.readmitreload();
            $scope.amsT_Id = "";
        }
        $scope.readmitreload = function () {
            apiService.getDATA("StudentAdmission/Getdetails").then(
                function (promise) {
                    $scope.yearre = promise.academicyearforreadmit;

                });
        }

        $scope.searchByColumn = function (search, searchColumn, searchyear) {

            if (search != "" && search != null && search != undefined) {
                if (searchyear == null || searchyear == "") {
                    searchyear = 0;
                }
                var data = {
                    "EnteredData": search,
                    "SearchColumn": searchColumn,
                    "ASMAY_Id": searchyear
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/SearchByColumn", data)

                    .then(function (promise) {

                        if (promise.count == 0) {
                            swal("No Records Found");
                            $state.reload();
                        }
                        if (promise.message != "" && promise.message != null) {
                            swal(promise.message);
                            if (promise.studentList != null && promise.studentList != "") {
                                for (var i = 0; i < promise.studentList.length; i++) {

                                    if (promise.studentList[i].PACA_MiddleName == null || promise.studentList[i].PACA_MiddleName == '' || promise.studentList[i].PACA_MiddleName == "") {
                                        promise.studentList[i].PACA_MiddleName = "Not Available";
                                    }

                                    if (promise.studentList[i].PACA_LastName == null || promise.studentList[i].PACA_LastName == '' || promise.studentList[i].PACA_LastName == "") {
                                        promise.studentList[i].PACA_LastName = "Not Available";
                                    }

                                    if (promise.studentList[i].PACA_SOL == "S") {
                                        promise.studentList[i].PACA_SOL = "Active";
                                    }
                                    else if (promise.studentList[i].PACA_SOL == "D") {
                                        promise.studentList[i].PACA_SOL = "Deactive";
                                    }
                                    else if (promise.studentList[i].PACA_SOL == "L") {
                                        promise.studentList[i].PACA_SOL = "Left";
                                    }

                                    //if (promise.studentList[i].amsT_SOL == "S") {
                                    //    promise.studentList[i].amsT_SOL = "Active";
                                    //}
                                    //else if (promise.studentList[i].amsT_SOL == "D") {
                                    //    promise.studentList[i].amsT_SOL = "Deactive";
                                    //}
                                    //else if (promise.studentList[i].amsT_SOL == "L") {
                                    //    promise.studentList[i].amsT_SOL = "Left";
                                    //}
                                }
                            }
                            $scope.StudentList = promise.studentList;
                            $scope.presentCountgrid = $scope.StudentList.length;

                        }
                        else {
                            $scope.search = "";
                            if (promise.studentList != null && promise.studentList != "") {
                                for (var i = 0; i < promise.studentList.length; i++) {
                                    if (promise.studentList[i].PACA_MiddleName == null || promise.studentList[i].PACA_MiddleName == '' || promise.studentList[i].PACA_MiddleName == "") {
                                        promise.studentList[i].PACA_MiddleName = "";
                                    }

                                    if (promise.studentList[i].PACA_LastName == null || promise.studentList[i].PACA_LastName == '' || promise.studentList[i].PACA_LastName == "") {
                                        promise.studentList[i].PACA_LastName = "";
                                    }

                                    if (promise.studentList[i].PACA_SOL == "S") {
                                        promise.studentList[i].PACA_SOL = "Active";
                                    }
                                    else if (promise.studentList[i].PACA_SOL == "D") {
                                        promise.studentList[i].PACA_SOL = "Deactive";
                                    }
                                    else if (promise.studentList[i].PACA_SOL == "L") {
                                        promise.studentList[i].PACA_SOL = "Left";
                                    }

                                    //if (promise.studentList[i].amsT_SOL == "S") {
                                    //    promise.studentList[i].amsT_SOL = "Active";
                                    //}
                                    //else if (promise.studentList[i].amsT_SOL == "D") {
                                    //    promise.studentList[i].amsT_SOL = "Deactive";
                                    //}
                                    //else if (promise.studentList[i].amsT_SOL == "L") {
                                    //    promise.studentList[i].amsT_SOL = "Left";
                                    //}
                                }
                            }
                            $scope.StudentList = promise.studentList;
                            $scope.presentCountgrid = $scope.StudentList.length;
                        }

                    })
            }
            else {
                swal("Please Enter Value To Be Searched In Search here.....Text Box  And Then Click On Search Icon");
            }
        }

        $scope.getcolumnId = function (ColumnId) {
            if (ColumnId == "14") {
                swal("Enter data in for ex: 2015-2016 format");
            }
        }
        $scope.guardianNameEntered = function (guardianDet, index) {

            if ($scope.EditId > 0) {
                if ($scope.grddetcount == 0) {
                    if ($scope.studentGuardianDetails[index].pacstG_GuardianName == "" || $scope.studentGuardianDetails[index].pacstG_GuardianName == null || $scope.studentGuardianDetails[index].pacstG_GuardianName == undefined) {
                        $scope.studentGuardianDetails[index].pacstG_GuardianAddress = "";
                        $scope.studentGuardianDetails[index].pacstG_emailid = "";
                        $scope.studentGuardianDetails[index].pacstG_GuardianPhoneNo = "";
                    }
                }
                else {
                    if ($scope.studentGuardianDetails[index].pacstG_GuardianName == "" || $scope.studentGuardianDetails[index].pacstG_GuardianName == null || $scope.studentGuardianDetails[index].pacstG_GuardianName == undefined) {
                        $scope.errorMessage = 'Guardian Name Required';
                    }
                    else {
                        $scope.errorMessage = '';
                    }
                }
            }
            else {
                if ($scope.studentGuardianDetails[index].pacstG_GuardianName == "" || $scope.studentGuardianDetails[index].pacstG_GuardianName == null || $scope.studentGuardianDetails[index].pacstG_GuardianName == undefined) {
                    $scope.studentGuardianDetails[index].pacstG_GuardianAddress = "";
                    $scope.studentGuardianDetails[index].pacstG_emailid = "";
                    $scope.studentGuardianDetails[index].pacstG_GuardianPhoneNo = "";
                }
            }
        }
        $scope.siblingNameEntered = function (index) {
            if ($scope.EditId > 0) {
                if ($scope.sibcount == 0) {
                    if ($scope.studentSiblingDetails[index].acstS_SiblingsName == "" || $scope.studentSiblingDetails[index].acstS_SiblingsName == null || $scope.studentSiblingDetails[index].acstS_SiblingsName == undefined) {
                        $scope.studentSiblingDetails[index].amcO_Id = "";
                        $scope.studentSiblingDetails[index].acstS_SiblingsRelation = "";
                    }
                }
                else {
                    if ($scope.studentSiblingDetails[index].acstS_SiblingsName == "" || $scope.studentSiblingDetails[index].acstS_SiblingsName == null || $scope.studentSiblingDetails[index].acstS_SiblingsName == undefined) {
                        $scope.errorMessage1 = 'Sibling Name Required';
                    }
                    else {
                        $scope.errorMessage1 = '';
                    }
                }
            }
            else {
                if ($scope.studentSiblingDetails[index].acstS_SiblingsName == "" || $scope.studentSiblingDetails[index].acstS_SiblingsName == null || $scope.studentSiblingDetails[index].acstS_SiblingsName == undefined) {
                    $scope.studentSiblingDetails[index].amcO_Id = "";
                    $scope.studentSiblingDetails[index].acstS_SiblingsRelation = "";

                }
            }
        }
        $scope.prevSchoolNameEntered = function (index) {
            if ($scope.EditId > 0) {
                if ($scope.prevschlcount == 0) {
                    if ($scope.prevSchoolDetails[index].pacstpS_PrvSchoolName == "" || $scope.prevSchoolDetails[index].pacstpS_PrvSchoolName == null || $scope.prevSchoolDetails[index].pacstpS_PrvSchoolName == undefined) {
                        $scope.prevSchoolDetails[index].pacstpS_PreSchoolType = "";
                        $scope.prevSchoolDetails[index].pacstpS_PreviousClass = "";
                        $scope.prevSchoolDetails[index].pacstpS_PreviousPer = "";
                        $scope.prevSchoolDetails[index].pacstpS_PreviousExamPassed = "";
                        $scope.prevSchoolDetails[index].pacstpS_PreviousGrade = "";
                        $scope.prevSchoolDetails[index].pacstpS_PreviousTCNo = "";
                        $scope.prevSchoolDetails[index].pacstpS_TCDate = "";
                        $scope.prevSchoolDetails[index].pacstpS_LeftYear = "";
                        $scope.prevSchoolDetails[index].pacstpS_PreviousMarks = "";
                        $scope.prevSchoolDetails[index].pacstpS_PreviousMarksObtained = "";
                        $scope.prevSchoolDetails[index].pacstpS_PreSchoolBoard = "";
                        $scope.prevSchoolDetails[index].pacstpS_PreSchoolCountry = "";
                        $scope.prevSchoolDetails[index].pacstpS_PreSchoolState = "";
                        $scope.prevSchoolDetails[index].pacstpS_Address = "";
                        $scope.prevSchoolDetails[index].pacstpS_LeftReason = "";
                        $scope.prevSchoolDetails[index].pacstpS_MediumOfInst = "";
                        $scope.prevSchoolDetails[i].pacstpS_PreviousBranch = "";
                        $scope.prevSchoolDetails[i].pacstpS_PasssedMonthYear = "";
                        $scope.prevSchoolDetails[i].pacstpS_LanguagesTaken = "";
                        $scope.prevSchoolDetails[i].pacstpS_Regno = "";
                        $scope.prevSchoolDetails[i].pacstpS_PreviousGrade = "";
                        $scope.prevSchoolDetails[i].pacstpS_Attempts = "";
                        $scope.prevSchoolDetails[i].pacstpS_Urbanrural = "";

                    }
                }
                else {
                    if ($scope.prevSchoolDetails[index].pacstpS_PrvSchoolName == "" || $scope.prevSchoolDetails[index].pacstpS_PrvSchoolName == null || $scope.prevSchoolDetails[index].pacstpS_PrvSchoolName == undefined) {
                        $scope.errorMessage2 = 'School Name Required';
                    }
                    else {
                        $scope.errorMessage2 = '';
                    }
                }
            }
            else {
                if ($scope.prevSchoolDetails[index].pacstpS_PrvSchoolName == "" || $scope.prevSchoolDetails[index].pacstpS_PrvSchoolName == null || $scope.prevSchoolDetails[index].pacstpS_PrvSchoolName == undefined) {
                    $scope.prevSchoolDetails[index].pacstpS_PreSchoolType = "";
                    $scope.prevSchoolDetails[index].pacstpS_PreviousClass = "";
                    $scope.prevSchoolDetails[index].pacstpS_PreviousPer = "";
                    $scope.prevSchoolDetails[index].pacstpS_PreviousExamPassed = "";
                    $scope.prevSchoolDetails[index].pacstpS_PreviousGrade = "";
                    $scope.prevSchoolDetails[index].pacstpS_PreviousTCNo = "";
                    $scope.prevSchoolDetails[index].pacstpS_LeftYear = "";
                    $scope.prevSchoolDetails[index].pacstpS_PreviousMarks = "";
                    $scope.prevSchoolDetails[index].pacstpS_PreviousMarksObtained = "";
                    $scope.prevSchoolDetails[index].pacstpS_TCDate = "";

                    $scope.prevSchoolDetails[index].pacstpS_PreSchoolBoard = "";
                    $scope.prevSchoolDetails[index].pacstpS_PreSchoolCountry = "";
                    $scope.prevSchoolDetails[index].pacstpS_PreSchoolState = "";
                    $scope.prevSchoolDetails[index].pacstpS_Address = "";
                    $scope.prevSchoolDetails[index].pacstpS_LeftReason = "";
                    $scope.prevSchoolDetails[index].pacstpS_MediumOfInst = "";
                    $scope.prevSchoolDetails[i].pacstpS_Attempts = "";
                    $scope.prevSchoolDetails[i].pacstpS_Urbanrural = "";

                }
            }
        }

        $scope.onselectprevCountry = function (pacstpS_PreSchoolCountry) {
            var countryname = {
                "countryName": pacstpS_PreSchoolCountry
            }

            apiService.create("ApplicationForm/StateByCountryName/", countryname).then(function (promise) {

                $scope.prevState = promise.prevStateList;

            })
        }

        $scope.onselectcompExam = function (pamcexM_Id, obj) {
            var examname = {
                "PAMCEXM_Id": pamcexM_Id,
                "subflg": false,
                "tempidlist": $scope.tempidlist
            }
            apiService.create("ApplicationForm/compExamName/", examname).then(function (promise) {
                $scope.compExamList = promise.compExamList;

                if ($scope.compExamList !== null && $scope.compExamList.length > 0) {
                    obj.compExamList = $scope.compExamList;


                }
                //if (promise.compSubList !== null && promise.compSubList.length > 0) {
                //    obj.compSubList = promise.compSubList;
                //}
                obj.pamcexmsuB_Id = "";
                //  $scope.compexammarksdetails = promise.compExamarray;          
            })
        }

        $scope.onselectsub = function (pamcexmsuB_Id, objj) {
            var subname = {
                "PAMCEXMSUB_Id": pamcexmsuB_Id,

                "subflg": true
            }
            apiService.create("ApplicationForm/compExamName/", subname).then(function (promise) {
                // $scope.compSubList = promise.compSubList;
                $scope.compSubList = promise.compSubList;

                if ($scope.compSubList !== null && $scope.compSubList.length > 0) {
                    objj.compSubList = $scope.compSubList;
                    // obj.pamcexmsuB_Id = "";
                }
            })
        }

        //get month names
        //month names
        $scope.getmontnames = function (monthid) {

            monthname = "";
            switch (monthid) {

                case 0:
                    monthname = "JANUARY"
                    break;
                case 1:
                    monthname = "FEBRUARY"
                    break;
                case 2:
                    monthname = "MARCH"
                    break;
                case 3:
                    monthname = "APRIL"
                    break;
                case 4:
                    monthname = "MAY"
                    break;
                case 5:
                    monthname = "JUNE"
                    break;
                case 6:
                    monthname = "JULY"
                    break;
                case 7:
                    monthname = "AUGUST"
                    break;
                case 8:
                    monthname = "SEPTEMBER"
                    break;
                case 9:
                    monthname = "OCTOBER"
                    break;
                case 10:
                    monthname = "NOVEMBER"
                    break;
                case 11:
                    monthname = "DECEMBER"
                    break;
                default:
                    monthname = ""
                    break;
            }
            return monthname;
        }

        //get datename

        $scope.getdatenames = function (dateid) {

            datename = "";
            switch (dateid) {

                case 1:
                    datename = "FIRST"
                    break;
                case 2:
                    datename = "SECOND"
                    break;
                case 3:
                    datename = "THIRD"
                    break;
                case 4:
                    datename = "FOURTH"
                    break;
                case 5:
                    datename = "FIFTH"
                    break;
                case 6:
                    datename = "SIXTH"
                    break;
                case 7:
                    datename = "SEVENTH"
                    break;
                case 8:
                    datename = "EIGHTH"
                    break;
                case 9:
                    datename = "NINTH"
                    break;
                case 10:
                    datename = "TENTH"
                    break;
                case 11:
                    datename = "ELEVENTH"
                    break;
                case 12:
                    datename = "TWELFTH"
                    break;
                case 13:
                    datename = "THIRTEENTH"
                    break;
                case 14:
                    datename = "FOURTEENTH"
                    break;
                case 15:
                    datename = "FIFTEENTH"
                    break;
                case 16:
                    datename = "SIXTEENTH"
                    break;
                case 17:
                    datename = "SEVENTEENTH"
                    break;
                case 18:
                    datename = "EIGHTEENTH"
                    break;
                case 19:
                    datename = "NINTEENTH"
                    break;
                case 20:
                    datename = "TWENTY"
                    break;
                case 21:
                    datename = "TWENTY FIRST"
                    break;
                case 22:
                    datename = "TWENTY SECOND"
                    break;
                case 23:
                    datename = "TWENTY THIRD"
                    break;
                case 24:
                    datename = "TWENTY FOURTH"
                    break;
                case 25:
                    datename = "TWENTY FIFTH"
                    break;
                case 26:
                    datename = "TWENTY SIXTH"
                    break;
                case 27:
                    datename = "TWENTY SEVENTH"
                    break;
                case 28:
                    datename = "TWENTY EIGHTH"
                    break;
                case 29:
                    datename = "TWENTY NINTH"
                    break;
                case 30:
                    datename = "THIRTY"
                    break;
                case 31:
                    datename = "THIRTY FIRST"
                    break;

                default:
                    datename = ""
                    break;
            }
            return datename;

        }


        //getting year 1
        $scope.getyearname = function (yearid) {

            var yearid = yearid.toString();

            var yearsplit = yearid.substring(0, 2)
            var dob = parseInt(yearsplit);

            var yearsplit1 = yearid.substring(2, 4)
            var dob1 = parseInt(yearsplit1);

            yearname = "";
            var datad = yearid;
            var yearname1 = "";





            if (dob >= 20) {
                yearname = "TWO THOUSAND";
            }
            else {
                yearname = "NINTEEN";
            }
            switch (dob1) {

                case 1:
                    yearname1 = "ONE"
                    break;
                case 2:
                    yearname1 = "TWO"
                    break;
                case 3:
                    yearname1 = "THREE"
                    break;
                case 4:
                    yearname1 = "FOUR"
                    break;
                case 5:
                    yearname1 = "FIVE"
                    break;
                case 6:
                    yearname1 = "SIX"
                    break;
                case 7:
                    yearname1 = "SEVEN"
                    break;
                case 8:
                    yearname1 = "EIGHT"
                    break;
                case 9:
                    yearname1 = "NINE"
                    break;
                case 10:
                    yearname1 = "TEN"
                    break;
                case 11:
                    yearname1 = "ELEVEN"
                    break;
                case 12:
                    yearname1 = "TWELVE"
                    break;
                case 13:
                    yearname1 = "THIRTEEN"
                    break;
                case 14:
                    yearname1 = "FOURTEEN"
                    break;
                case 15:
                    yearname1 = "FIFTEEN"
                    break;
                case 16:
                    yearname1 = "SIXTEEN"
                    break;
                case 17:
                    yearname1 = "SEVENTEEN"
                    break;
                case 18:
                    yearname1 = "EIGHTEEN"
                    break;
                case 19:
                    yearname1 = "NINTEEN"
                    break;
                case 20:
                    yearname1 = "TWENTY"
                    break;
                case 21:
                    yearname1 = "TWENTY ONE"
                    break;
                case 22:
                    yearname1 = "TWENTY TWO"
                    break;
                case 23:
                    yearname1 = "TWENTY THREE"
                    break;
                case 24:
                    yearname1 = "TWENTY FOUR"
                    break;
                case 25:
                    yearname1 = "TWENTY FIVE"
                    break;
                case 26:
                    yearname1 = "TWENTY SIX"
                    break;
                case 27:
                    yearname1 = "TWENTY SEVEN"
                    break;
                case 28:
                    yearname1 = "TWENTY EIGHT"
                    break;
                case 29:
                    yearname1 = "TWENTY NINE"
                    break;
                case 30:
                    yearname1 = "THIRTY"
                    break;
                case 31:
                    yearname1 = "THIRTY ONE"
                    break;
                case 32:
                    yearname1 = "THIRTY TWO"
                    break;
                case 33:
                    yearname1 = "THIRTY THREE"
                    break;
                case 34:
                    yearname1 = "THIRTY FOUR"
                    break;
                case 35:
                    yearname1 = "THIRTY FIVE"
                    break;
                case 36:
                    yearname1 = "THIRTY SIX"
                    break;
                case 37:
                    yearname1 = "THIRTY SEVEN"
                    break;
                case 38:
                    yearname1 = "THIRTY EIGHT"
                    break;
                case 39:
                    yearname1 = "THIRTY NINE"
                    break;
                case 40:
                    yearname1 = "FOURTY"
                    break;
                case 41:
                    yearname1 = "FOURTY ONE"
                    break;
                case 42:
                    yearname1 = "FOURTY TWO"
                    break;
                case 43:
                    yearname1 = "FOURTY THREE"
                    break;
                case 44:
                    yearname1 = "FOURTY FOUR"
                    break;
                case 45:
                    yearname1 = "FOURTY FIVE"
                    break;
                case 46:
                    yearname1 = "FOURTY SIX"
                    break;
                case 47:
                    yearname1 = "FOURTY SEVEN"
                    break;
                case 48:
                    yearname1 = "FOURTY EIGHT"
                    break;
                case 49:
                    yearname1 = "FOURTY NINE"
                    break;
                case 50:
                    yearname1 = "FIFTY"
                    break;
                case 51:
                    yearname1 = "FIFTY ONE"
                    break;
                case 52:
                    yearname1 = "FIFTY TWO"
                    break;
                case 53:
                    yearname1 = "FIFTY THREE"
                    break;
                case 54:
                    yearname1 = "FIFTY FOUR"
                    break;
                case 55:
                    yearname1 = "FIFTY FIVE"
                    break;
                case 56:
                    yearname1 = "FIFTY SIX"
                    break;
                case 57:
                    yearname1 = "FIFTY SEVEN"
                    break;
                case 58:
                    yearname1 = "FIFTY EIGHT"
                    break;
                case 59:
                    yearname1 = "FIFTY NINE"
                    break;
                case 60:
                    yearname1 = "SIXTY"
                    break;

                case 61:
                    yearname1 = "SIXTY ONE"
                    break;
                case 62:
                    yearname1 = "SIXTY TWO"
                    break;
                case 63:
                    yearname1 = "SIXTY THREE"
                    break;
                case 64:
                    yearname1 = "SIXTY FOUR"
                    break;
                case 65:
                    yearname1 = "SIXTY FIVE"
                    break;
                case 66:
                    yearname1 = "SIXTY SIX"
                    break;
                case 67:
                    yearname1 = "SIXTY SEVEN"
                    break;
                case 68:
                    yearname1 = "SIXTY EIGHT"
                    break;
                case 69:
                    yearname1 = "SIXTY NINE"
                    break;
                case 70:
                    yearname1 = "SEVENTY"
                    break;

                case 71:
                    yearname1 = "SEVENTY ONE"
                    break;
                case 72:
                    yearname1 = "SEVENTY TWO"
                    break;
                case 73:
                    yearname1 = "SEVENTY THREE"
                    break;
                case 74:
                    yearname1 = "SEVENTY FOUR"
                    break;
                case 75:
                    yearname1 = "SEVENTY FIVE"
                    break;
                case 76:
                    yearname1 = "SEVENTY SIX"
                    break;
                case 77:
                    yearname1 = "SEVENTY SEVEN"
                    break;
                case 78:
                    yearname1 = "SEVENTY EIGHT"
                    break;
                case 79:
                    yearname1 = "SEVENTY NINE"
                    break;
                case 80:
                    yearname1 = "EIGHTY"
                    break;

                case 81:
                    yearname1 = "EIGHTY ONE"
                    break;
                case 82:
                    yearname1 = "EIGHTY TWO"
                    break;
                case 83:
                    yearname1 = "EIGHTY THREE"
                    break;
                case 84:
                    yearname1 = "EIGHTY FOUR"
                    break;
                case 85:
                    yearname1 = "EIGHTY FIVE"
                    break;
                case 86:
                    yearname1 = "EIGHTY SIX"
                    break;
                case 87:
                    yearname1 = "EIGHTY SEVEN"
                    break;
                case 88:
                    yearname1 = "EIGHTY EIGHT"
                    break;
                case 89:
                    yearname1 = "EIGHTY NINE"
                    break;
                case 90:
                    yearname1 = "NINTY"
                    break;

                case 91:
                    yearname1 = "NINTY ONE"
                    break;
                case 92:
                    yearname1 = "NINTY TWO"
                    break;
                case 93:
                    yearname1 = "NINTY THREE"
                    break;
                case 94:
                    yearname1 = "NINTY FOUR"
                    break;
                case 95:
                    yearname1 = "NINTY FIVE"
                    break;
                case 96:
                    yearname1 = "NINTY SIX"
                    break;
                case 97:
                    yearname1 = "NINTY SEVEN"
                    break;
                case 98:
                    yearname1 = "NINTY EIGHT"
                    break;
                case 99:
                    yearname1 = "NINTY NINE"
                    break;
                case 0:
                    yearname1 = "ZERO"
                    break;

                default:
                    yearname1 = ""
                    break;
            }
            yearname = yearname + ' ' + yearname1;
            return yearname;
        }

        //adding multiple mobile number father

        $scope.mobiles = [{ id: 'mobile1' }];
        $scope.selmobs = [{ id: 'selmobile1' }];

        $scope.addNewMobile = function () {
            var newItemNo = $scope.mobiles.length + 1;
            if (newItemNo <= 3) {
                $scope.mobiles.push({ 'id': 'mobile' + newItemNo });
            }
            var newItemNo1 = $scope.selmobs.length + 1;
            if (newItemNo1 <= 3) {
                $scope.selmobs.push({ 'id': 'selmobile' + newItemNo1 });
            }

        };

        //removing mobile number father
        $scope.delm = [];
        $scope.removeNewMobile = function (index, curval1) {


            var newItemNo = $scope.mobiles.length - 1;
            if (newItemNo !== 0) {
                $scope.delm = $scope.mobiles.splice(index, 1);


            }
        };

        //adding email id  father

        $scope.emails = [{ id: 'email1' }];
        $scope.addNewEmail = function () {
            var newItemNo = $scope.emails.length + 1;
            if (newItemNo <= 5) {
                $scope.emails.push({ 'id': 'email1' + newItemNo });
            }
        };

        $scope.removeNewEmail = function (index) {


            var newItemNo = $scope.emails.length - 1;
            if (newItemNo !== 0) {
                $scope.delm = $scope.emails.splice(index, 1);

            }
        };

        $scope.showAddEmail = function (email) {
            return email.id === $scope.emails[$scope.emails.length - 1].id;
        };

        //adding mobile number mother
        $scope.mobiles1 = [{ id: 'mobile2' }];
        $scope.selmobs1 = [{ id: 'selmobile2' }];

        $scope.addNewMobile1 = function () {
            var newItemNo2 = $scope.mobiles1.length + 1;
            if (newItemNo2 <= 5) {
                $scope.mobiles1.push({ 'id': 'mobile2' + newItemNo2 });
            }
            var newItemNo3 = $scope.selmobs1.length + 1;
            if (newItemNo3 <= 5) {
                $scope.selmobs1.push({ 'id': 'selmobile2' + newItemNo3 });
            }
        };

        //removing mobile number mother
        $scope.delm1 = [];
        $scope.removeNewMobile1 = function (index, curval11) {

            var newItemNo2 = $scope.mobiles1.length - 1;
            if (newItemNo2 !== 0) {
                $scope.delm1 = $scope.mobiles1.splice(index, 1);


            }

        };


        //adding email id mother

        $scope.emails1 = [{ id: 'email2' }];
        $scope.addNewEmail1 = function () {
            var newItemNo2 = $scope.emails1.length + 1;
            if (newItemNo2 <= 5) {
                $scope.emails1.push({ 'id': 'email' + newItemNo2 });
            }
        };

        $scope.removeNewEmail1 = function (index) {


            var newItemNo = $scope.emails1.length - 1;
            if (newItemNo !== 0) {
                $scope.delm1 = $scope.emails1.splice(index, 1);

            }
        };

        $scope.showAddEmail1 = function (email) {
            return email.id === $scope.emails1[$scope.emails1.length - 1].id;
        };

        //adding mobile number student
        $scope.mobilesstd = [{ id: 'mobilestd1' }];
        $scope.selmobsstd = [{ id: 'selmobilestd1' }];

        $scope.addNewMobile1std = function () {
            var newItemNostd = $scope.mobilesstd.length + 1;
            if (newItemNostd <= 5) {
                $scope.mobilesstd.push({ 'id': 'mobilestd1' + newItemNostd });
            }

            if (newItemNostd == 4) {
                $scope.myForm1.$setPristine();
            }



        };


        $scope.addtopdays = function (user, index) {

            for (var k = 0; k < $scope.mobilesstd.length; k++) {
                var roll = parseInt(user.PACA_MobileNo);
                var arryind = $scope.mobilesstd.indexOf($scope.mobilesstd[k]);
                console.log(arryind);
                if (arryind != index) {
                    if ($scope.mobilesstd[k].PACA_MobileNo == roll) {
                        swal("Already Exist");
                        $scope.mobilesstd[index].PACA_MobileNo = "";


                        break;
                    }

                }


            }
        };

        //removing mobile number student
        $scope.delmsrd = [];
        $scope.removeNewMobile1std = function (index, curval1std) {

            var newItemNostd2 = $scope.mobilesstd.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd = $scope.mobilesstd.splice(index, 1);

            }

        };


        //adding email id student

        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;
            if (newItemNostd2 <= 5) {
                $scope.emailsstd.push({ 'id': 'emailsstd1' + newItemNostd2 });
            }
        };

        $scope.removeNewEmail1std = function (index) {


            var newItemNostd = $scope.emailsstd.length - 1;
            if (newItemNostd !== 0) {
                $scope.delmsrd = $scope.emailsstd.splice(index, 1);


            }

        };

        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };


        //checking reg no duplicate
        $scope.checkregnoduplicate = function () {
            var id = $scope.EditId;
            var data = {
                "PACA_Id": id,
                "admRegManualFlag": $scope.reg_,
                "admAdmManualFlag": $scope.Adm_,
                "preventdupRegNo": $scope.regpreventduplicateflag,
                "preventdupAdmNo": $scope.admpreventduplicateflag,
                "PACA_RegistrationNo": $scope.obj.PACA_RegistrationNo
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ApplicationForm/checkDuplicate", data).then(function (promise) {


                //Check Duplicate Adm.No Or Reg.No.If set to prevent duplicate in Transaction Numbering.
                if (promise.duplicateRegNo > 0) {
                    $scope.obj.PACA_RegistrationNo = "";
                    swal("Student Reg.No. Already Exists");
                    return;
                }
            });
        }

        $scope.checkadmnoduplicate = function () {
            var id = $scope.EditId;
            var data = {
                "PACA_Id": id,
                "admRegManualFlag": $scope.reg_,
                "admAdmManualFlag": $scope.Adm_,
                "preventdupRegNo": $scope.regpreventduplicateflag,
                "preventdupAdmNo": $scope.admpreventduplicateflag,
                "PACA_AdmNo": $scope.obj.PACA_AdmNo
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ApplicationForm/checkDuplicate", data).then(function (promise) {


                //Check Duplicate Adm.No Or Reg.No.If set to prevent duplicate in Transaction Numbering.                
                if (promise.duplicateAdmNo > 0) {
                    $scope.obj.PACA_AdmNo = "";
                    swal("Student Admission No. Already Exists");
                    return;
                }
            });
        }

        $scope.removeall1 = function () {
            $('#myModalecs').modal('hide');
            $scope.submittedecs = false;
            $scope.myFormecs.$setPristine();
            $scope.myFormecs.$setUntouched();
        }

        $scope.objee = [];
        $scope.objj = [];
        $scope.ecsdetailscheck = function (objj) {
            if (objj == false) {
                swal({
                    title: "Do You Want To Delete The ECS Details Of This Student",
                    text: "Do you want to continue !!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                    cancelButtonText: "Cancel!!",
                    closeOnConfirm: true,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        $('#myModalecs').modal('hide');
                        $scope.ecsdetailslist = [];
                        $scope.submittedecs = false;
                        $scope.myFormecs.$setPristine();
                        $scope.myFormecs.$setUntouched();

                    }
                    else {
                        $('#myModalecs').modal('show');
                        $scope.obj.amsT_ECSFlag = true;
                    }
                });

            } else {

            }

        }
        $scope.interactedecs = function (field) {
            return $scope.submittedecs;
        };
        $scope.submittedecs = false;
        $scope.ecsdetailslist = [];

        $scope.addtocartecs = function (objee) {
            if ($scope.myFormecs.$valid) {
                if (objee.asecS_Id == undefined) {
                    objee.asecS_Id = 0;
                }
                $scope.ecsdetailslist = objee;
                $('#myModalecs').modal('hide');
            }
            else {
                $scope.submittedecs = true;
            }
        }



        //----Saving Tab Wise---------//

        //First Tab
        $scope.savefirsttab = function () {

            if ($scope.myForm1.$valid) {

                var id = 0;

                if ($scope.obj.PACA_MiddleName == null || $scope.obj.PACA_MiddleName == "" || $scope.obj.PACA_MiddleName == undefined) {
                    $scope.obj.PACA_MiddleName = '';
                }
                if ($scope.obj.PACA_LastName == null || $scope.obj.PACA_LastName == "" || $scope.obj.PACA_LastName == undefined) {
                    $scope.obj.PACA_LastName = '';
                }
                if ($scope.obj.PACA_District == null) {
                    $scope.obj.PACA_District = '';
                } else {
                    $scope.obj.PACA_District = $scope.obj.PACA_District;
                }
                if ($scope.obj.PACA_Village == null) {
                    $scope.obj.PACA_Village = '';
                } else {
                    $scope.obj.PACA_Village = $scope.obj.PACA_Village;
                }
                if ($scope.obj.PACA_Taluk == null) {
                    $scope.obj.PACA_Taluk = '';
                } else {
                    $scope.obj.PACA_Taluk = $scope.obj.PACA_Taluk;
                }


                var PACA_DOB = new Date($scope.obj.PACA_DOB).toDateString();

                if ($scope.EditId == undefined) {
                    id = 0;
                } else {
                    id = $scope.EditId;
                }
                if ($scope.PACA_AadharNo == undefined) {
                    $scope.PACA_AadharNo = 0;
                }
                if ($scope.PACA_Age == undefined) {
                    $scope.PACA_Age = 0;
                }
                var data = {
                    "PACA_Id": id,
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "PACA_FirstName": $scope.obj.PACA_FirstName,
                    "PACA_MiddleName": $scope.obj.PACA_MiddleName,
                    "PACA_LastName": $scope.obj.PACA_LastName,
                    "AMCO_Id": $scope.obj.AMCO_Id,
                    "AMB_Id": $scope.obj.AMB_Id,
                    "AMSE_Id": $scope.obj.AMSE_Id,
                    "PACA_DOB": PACA_DOB,
                    "PACA_DOB_inwords": $scope.obj.PACA_DOB_inwords,
                    "IMCC_Id": $scope.obj.IMCC_Id,
                    "IMC_Id": $scope.obj.IMC_Id,
                    "PACA_StudentSubCaste": $scope.obj.PACA_StudentSubCaste,
                    "PACA_BirthPlace": $scope.obj.PACA_BirthPlace,
                    "PACA_Age": $scope.obj.PACA_Age,
                    "PACA_Sex": $scope.obj.PACA_Sex,
                    "PACA_MotherTongue": $scope.obj.PACA_MotherTongue,
                    "IVRMMR_Id": $scope.obj.IVRMMR_Id,
                    "PACA_StudentPhoto": $scope.obj.image,
                    "PACA_Nationality": $scope.obj.PACA_Nationality,
                    "PACA_MobileNo": $scope.obj.PACA_MobileNo,
                    "PACA_emailId": $scope.obj.PACA_emailId,
                    "PACA_AadharNo": $scope.obj.PACA_AadharNo,
                    "PACA_BloodGroup": $scope.obj.PACA_BloodGroup,
                    "transnumconfigsettings": $scope.RegistrationNumbering,
                   // "admissionNumbering": $scope.AdmissionNumbering,
                    "PACA_Taluk": $scope.obj.PACA_Taluk,
                    "PACA_Village": $scope.obj.PACA_Village,
                    "PACA_District": $scope.obj.PACA_District,
                    "PA_College_Student_CBPreference": $scope.mobiles,
                    "PACA_Urban_Rural": $scope.obj.PACA_Urban_Rural,
                    // "PACA_ApplStatus": 787928,
                    "PACA_ApplStatus": "787926",
                    "ACQ_Id": $scope.obj.ACQ_Id,
                    "ACQC_Id": $scope.obj.ACQC_Id,
                    "PACA_AlterNativeEmialId": $scope.obj.PACA_AlterNativeEmialId,
                    "PACA_CoutryCode": $scope.obj.PACA_CoutryCode,

                    //  "AMST_Concession_Type": $scope.obj.feeconcession,

                    //  "AMST_SubCasteIMC_Id": $scope.obj.amsT_SubCasteIMC_Id,
                };


                apiService.create("ApplicationForm/saveStudentDetails", data).then(function (promise) {
                    if (promise.message == "Add") {
                        swal("Record Saved Successfully");
                        $scope.BindData();
                        $scope.EditId = promise.pacA_Id;
                        $scope.address = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;
                    }
                    else if (promise.message == "Update") {
                        swal("Record Updated Successfully");
                        $scope.BindData();
                        $scope.EditId = promise.pacA_Id;
                        $scope.address = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        swal("Failed to Save/Update");
                        $scope.submitted1 = true;
                        $scope.address = true;

                    }
                });
            }
            else {
                $scope.submitted1 = true;
                $scope.address = true;
            }
        };

        //view docs//
        $scope.docs = [];
        $scope.Clgapplicationstudocs = function (studentid) {

            var data = {
                "AMSMD_Id": studentid

            }
            apiService.create("ApplicationForm/Clgapplicationstudocs/", data).then(function (promise) {

                if (promise.docdownload.length > 0 && promise.docdownload != null) {
                    $scope.pages = promise.docdownload;
                    if (promise.docdownload.length > 0) {
                        angular.forEach($scope.pages, function (obj1) {
                            $('#').attr('src', obj1.amsmD_FilePath);
                            var img = obj1.amsmD_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            obj1.filetype = lastelement;
                        });
                    }
                    $('#myModaldoc').modal('show');
                }
                else {
                    swal("No Record Found!");
                }

            });
        }



        var studentreg = "";
        var imagedownload = "";
        var docname = "";
        $scope.downloaddirect = function (data) {
            //if ($scope.pages.length > 0) {
            //    for (var i = 0; i < $scope.pages.length; i++) {
            //        if ($scope.pages[i].paca_id == idd) {
            //            studentreg = $scope.pages[i].PACA_RegistrationNo;
            //        }
            //    }
            //}

            $scope.imagedownload = data.amsmD_FilePath;
            imagedownload = data.amsmD_FilePath;
            docname = data.amsmD_FileName;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '-' + docname + '.jpg'
                    })[0].click();
                })
        }

        $scope.downloadpdf = function (data) {

            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = data;

            $http.get(imagedownload1, { responseType: 'arraybuffer' })
                .success(function (response) {
                    var fileURL = "";
                    var file = "";
                    var embed = "";
                    var pdfId = "";
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);

                    pdfId = document.getElementById("showpdf");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        }
        //Second Tab
        $scope.savesecondtab = function () {
            if ($scope.myForm2.$valid) {
                var id = $scope.EditId;
                var data = {
                    "PACA_Id": $scope.EditId,
                    "PACA_PerStreet": $scope.obj.PACA_PerStreet,
                    "PACA_PerArea": $scope.obj.PACA_PerArea,
                    "IVRMMC_Id": $scope.obj.IVRMMC_Id,
                    "PACA_PerState": $scope.obj.PACA_PerState,
                    "PACA_PerCity": $scope.obj.PACA_PerCity,
                    "PACA_PerPincode": $scope.obj.PACA_PerPincode,
                    "PACA_ConStreet": $scope.obj.PACA_ConStreet,
                    "PACA_ConArea": $scope.obj.PACA_ConArea,
                    "PACA_ConCountryId": $scope.obj.PACA_ConCountryId,
                    "PACA_ConState": $scope.obj.PACA_ConState,
                    "PACA_ConCity": $scope.obj.PACA_ConCity,
                    "PACA_ConPincode": $scope.obj.PACA_ConPincode,

                }
                apiService.create("ApplicationForm/saveAddress", data).then(function (promise) {
                    if (promise.message == "Add") {
                        swal("Record Saved Successfully");
                        $scope.EditId = promise.pacA_Id;
                        $scope.Parents = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;
                    }
                    else if (promise.message == "Update") {
                        swal("Record Updated Successfully");
                        $scope.EditId = promise.pacA_Id;
                        $scope.Parents = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;
                    }

                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists")
                    }
                    else {
                        swal("Failed to Save/Update");
                        $scope.submitted2 = true;
                        $scope.Parents = true;
                    }
                })
            }
            else {
                $scope.submitted2 = true;
                $scope.Parents = true;
            }
        }

        //Third Tab
        $scope.savethirdtab = function () {
            if ($scope.myForm3.$valid) {

                var id = $scope.EditId;

                if ($scope.obj.PACA_FatherBankAccNo == 0) {
                    $scope.obj.PACA_FatherBankAccNo = undefined;
                }
                if ($scope.obj.PACA_FatherBankIFSCCode == 0) {
                    $scope.obj.PACA_FatherBankIFSCCode = undefined;
                }
                if ($scope.obj.PACA_FatherReligion == 0) {
                    $scope.obj.PACA_FatherReligion = undefined;
                }
                if ($scope.obj.PACA_FatherCaste == 0) {
                    $scope.obj.PACA_FatherCaste = undefined;
                }
                if ($scope.obj.PACA_FatherSubCaste == 0) {
                    $scope.obj.PACA_FatherSubCaste = undefined;
                }
                if ($scope.obj.PACA_MotherMobleNo == "") {
                    $scope.obj.PACA_MotherMobleNo = undefined;
                }

                if ($scope.obj.PACA_FatherMobleNo == "") {
                    $scope.obj.PACA_FatherMobleNo = undefined;
                }
                if ($scope.obj.PACA_FatherAadharNo == "") {
                    $scope.obj.PACA_FatherAadharNo = undefined;
                }


                if ($scope.obj.PACA_MotherBankAccNo == 0) {
                    $scope.obj.PACA_MotherBankAccNo = undefined;
                }
                if ($scope.obj.PACA_MotherBankIFSCCode == 0) {
                    $scope.obj.PACA_MotherBankIFSCCode = undefined;
                }
                if ($scope.obj.PACA_MotherReligion == 0) {
                    $scope.obj.PACA_MotherReligion = undefined;
                }
                if ($scope.obj.PACA_MotherCaste == 0) {
                    $scope.obj.PACA_MotherCaste = undefined;
                }
                if ($scope.obj.PACA_MotherSubCaste == 0) {
                    $scope.obj.PACA_MotherSubCaste = undefined;
                }
                if ($scope.obj.PACA_MotherAadharNo == 0) {
                    $scope.obj.PACA_MotherAadharNo = undefined;
                }
                if ($scope.obj.PACA_FatherMonIncome == 0) {
                    $scope.obj.PACA_FatherMonIncome = undefined;
                }
                if ($scope.obj.PACA_FatherAnnIncome == 0) {
                    $scope.obj.PACA_FatherAnnIncome = undefined;
                }

                if ($scope.obj.PACA_MotherMonIncome == 0) {
                    $scope.obj.PACA_MotherMonIncome = undefined;
                }
                if ($scope.obj.PACA_MotherAnnIncome == 0) {
                    $scope.obj.PACA_MotherAnnIncome = undefined;
                }
                if ($scope.obj.PACA_MotherAadharNo == 0) {
                    $scope.obj.PACA_MotherAadharNo = undefined;
                }



                var data = {
                    "PACA_Id": $scope.EditId,
                    "PACA_FatherAliveFlag": $scope.obj.PACA_FatherAliveFlag,
                    "PACA_FatherName": $scope.obj.PACA_FatherName,
                    "PACA_FatherSurname": $scope.obj.PACA_FatherSurname,
                    "PACA_FatherAadharNo": $scope.obj.PACA_FatherAadharNo,
                    "PACA_FatherEducation": $scope.obj.PACA_FatherEducation,
                    "PACA_FatherOfficeAdd": $scope.obj.PACA_FatherOfficeAdd,
                    "PACA_FatherOccupation": $scope.obj.PACA_FatherOccupation,
                    "PACA_FatherDesignation": $scope.obj.PACA_FatherDesignation,
                    "PACA_FatherBankAccNo": $scope.obj.PACA_FatherBankAccNo,
                    "PACA_FatherBankIFSCCode": $scope.obj.PACA_FatherBankIFSCCode,
                    "PACA_FatherCasteCertiNo": $scope.obj.PACA_FatherCasteCertiNo,
                    "PACA_FatherNationality": $scope.obj.PACA_FatherNationality,
                    "PACA_FatherReligion": $scope.obj.PACA_FatherReligion,
                    "PACA_FatherCaste": $scope.obj.PACA_FatherCaste,
                    "PACA_FatherSubCaste": $scope.obj.PACA_FatherSubCaste,
                    "PACA_FatherMonIncome": $scope.obj.PACA_FatherMonIncome,
                    "PACA_FatherAnnIncome": $scope.obj.PACA_FatherAnnIncome,
                    "PACA_FatherMobleNo": $scope.obj.PACA_FatherMobleNo,
                    "PACA_FatherEmailId": $scope.obj.PACA_FatherEmailId,

                    //Mother details
                    "PACA_MotherAliveFlag": $scope.obj.PACA_MotherAliveFlag,
                    "PACA_MotherName": $scope.obj.PACA_MotherName,
                    "PACA_MotherSurname": $scope.obj.PACA_MotherSurname,
                    "PACA_MotherAadharNo": $scope.obj.PACA_MotherAadharNo,
                    "PACA_MotherEducation": $scope.obj.PACA_MotherEducation,
                    "PACA_MotherOfficeAdd": $scope.obj.PACA_MotherOfficeAdd,
                    "PACA_MotherOccupation": $scope.obj.PACA_MotherOccupation,
                    "PACA_MotherDesignation": $scope.obj.PACA_MotherDesignation,
                    "PACA_MotherBankAccNo": $scope.obj.PACA_MotherBankAccNo,
                    "PACA_MotherBankIFSCCode": $scope.obj.PACA_MotherBankIFSCCode,
                    "PACA_MotherCasteCertiNo": $scope.obj.PACA_MotherCasteCertiNo,
                    "PACA_MotherNationality": $scope.obj.PACA_MotherNationality,
                    "PACA_MotherReligion": $scope.obj.PACA_MotherReligion,
                    "PACA_MotherCaste": $scope.obj.PACA_MotherCaste,
                    "PACA_MotherSubCaste": $scope.obj.PACA_MotherSubCaste,
                    "PACA_MotherMonIncome": $scope.obj.PACA_MotherMonIncome,
                    "PACA_MotherAnnIncome": $scope.obj.PACA_MotherAnnIncome,
                    "PACA_MotherMobleNo": $scope.obj.PACA_MotherMobleNo,
                    "PACA_MotherEmailId": $scope.obj.PACA_MotherEmailId,
                    "PACA_MotherCountryCode": $scope.obj.PACA_MotherCountryCode,
                    "PACA_FatherCountryCode": $scope.obj.PACA_FatherCountryCode
                }
                apiService.create("ApplicationForm/saveParentsDetails", data).then(function (promise) {
                    if (promise.message == "Add") {

                        swal("Record Saved Successfully");
                        $scope.EditId = promise.pacA_Id;
                        $scope.Others = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;

                    }
                    else if (promise.message == "Update") {
                        swal("Record Update Successfully");
                        $scope.EditId = promise.pacA_Id;
                        $scope.Others = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists")
                    }
                    else {
                        $scope.submitted3 = true;
                        $scope.Others = true;

                    }
                })
            }

            else {
                $scope.submitted3 = true;
                $scope.Others = true;
            }
        }



        //Fourth Tab
        $scope.savefourthtab = function () {
            if ($scope.myForm4.$valid) {

                var id = $scope.EditId;
                if ($scope.prevSchoolDetails.length > 0) {
                    angular.forEach($scope.prevSchoolDetails, function (pr) {
                        if (pr.pacstpS_PrvSchoolName != "" && pr.pacstpS_PrvSchoolName != null
                            && pr.pacstpS_PrvSchoolName != undefined) {
                            pr.pacstpS_TCDate = new Date(pr.pacstpS_TCDate).toDateString();
                        }

                    })
                }

                var PrevSchoolDet = $scope.prevSchoolDetails;

                var studentpreviousexammarksdetails = $scope.prevexammarksdetails;

                //added 
                var studentcompexamdetails = $scope.compexamstudetails;

                var studentcompsubmarks = $scope.compexammarksdetails;

                var data = {
                    "PACA_Id": $scope.EditId,
                    "SelectedPrevSchoolDetails": PrevSchoolDet,
                    "Adm_College_Student_SubjectMarksTempDTO": studentpreviousexammarksdetails,
                    "PA_College_Student_CEMarksClgDTO": studentcompexamdetails,
                    "PA_College_Student_CEMarks_SubjectClgDTO": studentcompsubmarks
                }
                apiService.create("ApplicationForm/saveOthersDetails", data).then(function (promise) {
                    if (promise.message == "Add") {
                        swal("Record Saved Successfully");
                        $scope.EditId = promise.pacA_Id;
                        $scope.DocumentUpload = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;

                    }
                    else if (promise.message == "Update") {

                        swal("Record Update Successfully");
                        $scope.EditId = promise.pacA_Id;
                        $scope.DocumentUpload = false;
                        $scope.myTabIndex = $scope.myTabIndex + 1;

                    }

                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists")
                    }
                    else {
                        $scope.submitted4 = true;
                        $scope.DocumentUpload = true;
                        swal("Failed to Save/Update Record");

                    }
                })
            }

            else {
                $scope.submitted4 = true;
                $scope.DocumentUpload = true;
            }
        }

        //Fifth Tab
        $scope.savefinaltab = function () {
            if ($scope.myForm5.$valid) {

                var StuGuardianDet = $scope.studentGuardianDetails;

                var PACA_PassportIssueDate = "";
                var PACA_PassportExpiryDate = "";
                var PACA_VISAValidFrom = "";
                var PACA_VISAValidTo = "";

                if ($scope.obj.PACA_PassportNo != '' && $scope.obj.PACA_PassportNo != null && $scope.obj.PACA_PassportNo != undefined) {

                    PACA_PassportIssueDate = $scope.obj.PACA_PassportIssueDate;
                    PACA_PassportExpiryDate = $scope.obj.PACA_PassportExpiryDate;
                } else {
                    PACA_PassportIssueDate = new Date();
                    PACA_PassportExpiryDate = new Date();
                }
                if ($scope.obj.PACA_VISAIssuedBy != '' && $scope.obj.PACA_VISAIssuedBy != null && $scope.obj.PACA_VISAIssuedBy != undefined) {
                    PACA_VISAValidFrom = $scope.obj.PACA_VISAValidFrom;
                    PACA_VISAValidTo = $scope.obj.PACA_VISAValidTo;
                } else {
                    PACA_VISAValidFrom = new Date();
                    PACA_VISAValidTo = new Date();
                    $scope.obj.PACA_VISAIssuedBy = "";
                }

                var data = {
                    "PACA_Id": $scope.EditId,
                    "PACA_FatherSign": $scope.fatherSign,
                    "PACA_FatherFingerprint": $scope.fatherFingerprint,
                    "PACA_MotherSign": $scope.mothersign,
                    "PACA_MotherFingerprint": $scope.motherfingerprint,
                    "PACA_FatherPhoto": $scope.fatherphoto,
                    "PACA_MotherPhoto": $scope.motherphoto,
                    "PACA_HostelReqdFlag": $scope.obj.PACA_HostelReqdFlag,
                    "PACA_TransportReqdFlag": $scope.obj.PACA_TransportReqdFlag,
                    "PACA_GymReqdFlag": $scope.obj.PACA_GymReqdFlag,
                    "SelectedGuardianDetails": StuGuardianDet,
                    "pacstG_GuardianPhoto": $scope.pacstG_GuardianPhoto,
                    "pacstG_GuardianSign": $scope.pacstG_GuardianSign,
                    "pacstG_Fingerprint": $scope.pacstG_Fingerprint,
                    "PACA_PassportNo": $scope.obj.PACA_PassportNo,
                    "PACA_PassportIssuedAt": $scope.obj.PACA_PassportIssuedAt,
                    "PACA_PassportIssueDate": new Date(PACA_PassportIssueDate).toDateString(),
                    "PACA_PassportIssuedCounrty": $scope.obj.PACA_PassportIssuedCounrty,
                    "PACA_PassportIssuedPlace": $scope.obj.PACA_PassportIssuedPlace,
                    "PACA_PassportExpiryDate": new Date(PACA_PassportExpiryDate).toDateString(),
                    "PACA_VISAIssuedBy": $scope.obj.PACA_VISAIssuedBy,
                    "PACA_VISAValidFrom": new Date(PACA_VISAValidFrom).toDateString(),
                    "PACA_VISAValidTo": new Date(PACA_VISAValidTo).toDateString(),
                    Uploaded_documentList: $scope.documentList
                }
                apiService.create("ApplicationForm/saveDocuments", data).then(function (promise) {
                    if (promise.message == "Add") {
                        swal("Record Saved Successfully");
                        $scope.BindData();
                        if (promise.pay == "Pay") {
                            $scope.paynowbefore(promise.pacA_Id);
                            $scope.PaymentMode = true;
                        }
                        else {
                            $state.reload();
                        }
                    }
                    else if (promise.message == "Update") {

                        swal("Record Update Successfully");
                        $scope.BindData();
                        if (promise.pay == "Pay") {
                            $scope.paynowbefore(promise.pacA_Id);
                            $scope.PaymentMode = true;
                        }
                        else {
                            $state.reload();
                        }

                    }
                    else {
                        $scope.submitted5 = true;
                        $scope.disableSaveButton = false;
                        swal("Failed to Save/Update Record");
                    }
                })
            }
            else {
                $scope.submitted5 = true;
                $scope.disableSaveButton = false;
            }
        }


        //Clear Total Marks
        $scope.cleartotalmarks = function (prevSchool) {
            prevSchool.pacstpS_PreviousMarksObtained = "";
            prevSchool.pacstpS_PreviousPer = "";
        }

        //Percentage Calculation
        $scope.percentage_cal = function (prevSchool) {
            var total_marks = prevSchool.pacstpS_PreviousMarks;
            var total_marksobtained = prevSchool.pacstpS_PreviousMarksObtained;
            var percentage = (total_marksobtained / total_marks) * 100;

            prevSchool.pacstpS_PreviousPer = parseFloat(percentage).toFixed(2);
        }

        //Percentage Calculation
        $scope.percentage_calnew = function (prevExamdetails) {
            var total_marks = prevExamdetails.pacstsuM_MaxMarks;
            var total_marksobtained = prevExamdetails.pacstsuM_SubjectMarks;
            var percentage = (total_marksobtained / total_marks) * 100;

            if (percentage !== null && percentage !== "NaN") {
                prevExamdetails.pacstsuM_Percentage = parseFloat(percentage).toFixed(2);
            }
        };

    }
})();
