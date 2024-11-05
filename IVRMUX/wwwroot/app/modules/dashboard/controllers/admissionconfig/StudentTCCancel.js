(function () {
    'use strict';
    angular.module('app').controller('studentTCCancelController', studentTCCancelController)

    studentTCCancelController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', 'Excel', '$timeout','$stateParams']
    function studentTCCancelController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, Excel, $timeout, $stateParams) {

        $scope.details_flag = false;
        $scope.submitted = false;

        $scope.ddate = {};
        $scope.obj = {};
        $scope.ddate = new Date();

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.loadInitialData = function (user) {
            apiService.getURI("StudentTC/GetTCCancelDetails", 2).then(function (promise) {
                $scope.getstudenttcdetails = promise.getstudenttcdetails;
                $scope.academicList = promise.academicList;
                $scope.currentYear = promise.currentYear;
                $scope.TempYear = [];

                $scope.CurrentYearOrder = $scope.currentYear[0].asmaY_Order;
                $scope.prevorder = $scope.CurrentYearOrder - 1;                
                $scope.TempYear = $scope.currentYear;

                angular.forEach($scope.academicList, function (dd) {
                    if (dd.asmaY_Order === $scope.prevorder) {
                        $scope.TempYear.push(dd);
                    }
                });

                $scope.ASMAY_Id = $scope.currentYear[0].asmaY_Id;
                $scope.getdeletedtcdetails = promise.getdeletedtcdetails;
            });
        };

        $scope.OnChangeAcademicYear = function () {
            $scope.getstudenttcdetails = [];
            $scope.getdeletedtcdetail = [];
            $scope.AMST_Id = "";
            $scope.details_flag = false;
            $scope.submitted = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("StudentTC/OnChangeAcademicYear", data).then(function (promise) {
                $scope.getstudenttcdetails = promise.getstudenttcdetails;
                $scope.getdeletedtcdetails = promise.getdeletedtcdetails;

                angular.forEach($scope.academicList, function (dd) {
                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                        $scope.yearname = dd.asmaY_Year;
                    }
                });

            });
        };

        $scope.OnStudentNameChange = function () {
            $scope.details_flag = false;
            $scope.submitted = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMST_Id": $scope.AMST_Id.amsT_Id
            };

            apiService.create("StudentTC/OnStudentNameChange", data).then(function (promise) {
                $scope.gettcdetailsbystudentid = promise.gettcdetailsbystudentid;
                if ($scope.gettcdetailsbystudentid !== null && $scope.gettcdetailsbystudentid.length > 0) {
                    $scope.details_flag = true;

                    $scope.stdname = $scope.gettcdetailsbystudentid[0].amsT_FirstName;
                    $scope.ASTC_TCDate = $scope.gettcdetailsbystudentid[0].astC_TCDate;
                    $scope.TCNo = $scope.gettcdetailsbystudentid[0].astC_TCNO;
                    $scope.ASTC_TCApplicationDate = $scope.gettcdetailsbystudentid[0].astC_TCApplicationDate;
                    $scope.ASTC_TCIssueDate = $scope.gettcdetailsbystudentid[0].astC_TCIssueDate;

                    $scope.joinedclassname = $scope.gettcdetailsbystudentid[0].joinedclassname;
                    $scope.joined_year = $scope.gettcdetailsbystudentid[0].joinedyearname;

                    $scope.class_name = $scope.gettcdetailsbystudentid[0].leftclassname;
                    $scope.left_year = $scope.gettcdetailsbystudentid[0].leftyearname;
                    $scope.section_name = $scope.gettcdetailsbystudentid[0].leftsectionname;

                    $scope.Stu_Img = $scope.gettcdetailsbystudentid[0].studentphotopath;
                    $scope.Adm_No = $scope.gettcdetailsbystudentid[0].amsT_AdmNo;
                    $scope.Reg_No = $scope.gettcdetailsbystudentid[0].amsT_RegistrationNo;


                    $scope.studenttcdetails = promise.studenttcdetails;
                    $scope.getadm_m_student_details = promise.getadm_m_student_details;
                } else {
                    swal("Student Details Not Found");
                }
            });
        };

        $scope.SaveTCCancelDetails = function (objs) {
            $scope.submitted = false;
            if ($scope.myForm1.$valid) {

                swal({
                    title: "Are You Sure?",
                    text: "You Want To Delete TC For This Student",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "AMST_Id": $scope.AMST_Id.amsT_Id,
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "TC_CancelReason": objs.TC_CancelReason
                            };

                            apiService.create("StudentTC/SaveTCCancelDetails", data).then(function (promise) {
                                if (promise !== null) {
                                    if (promise.returnval === true) {
                                        swal("TC Deleted Successfully");
                                    } else {
                                        swal("Failed To Delete TC");
                                    }
                                    $state.reload();
                                }
                            });
                        }
                        else {
                            swal("Record Delection Cancelled");
                        }
                    });                
            } else {
                $scope.submitted = true;
            }
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.clear = function () {
            $state.reload();
        };

        $scope.exportToExceld = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 900);
        };

        $scope.printData = function () {
            $scope.date = new Date();
            var innerContents = document.getElementById("printSectionId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };
    }
})();