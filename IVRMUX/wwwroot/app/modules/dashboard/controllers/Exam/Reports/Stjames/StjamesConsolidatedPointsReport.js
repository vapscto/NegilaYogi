(function () {
    'use strict';
    angular.module('app').controller('StjamesConsolidatedPointsReportController', StjamesConsolidatedPointsReportController)
    StjamesConsolidatedPointsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window', 'Excel', '$timeout']
    function StjamesConsolidatedPointsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window, Excel, $timeout) {

        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.getexamlist = [];
        $scope.obj = {};
        $scope.generateddate = new Date();
        $scope.reporttype = 'indi';
        $scope.StjamesPoints = true;

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.class_list = [];
            $scope.ASMCL_Id = "";
            $scope.section_list = [];
            $scope.ASMS_Id = "";
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            $scope.section_list = [];
            $scope.ASMS_Id = "";
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_sections", data).then(function (promise) {
                $scope.section_list = promise.getsectionlist;
            });
        };

        //-----------section Selection
        $scope.onsectionchange = function () {
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
            $scope.getgrouplist = [];

            //var data = {
            //    "ASMAY_Id": $scope.ASMAY_Id,
            //    "ASMCL_Id": $scope.ASMCL_Id,
            //    "ASMS_Id": $scope.ASMS_Id,
            //    "reporttype": $scope.reporttype
            //};

            //apiService.create("JSHSExamReports/get_Exam_group", data).then(function (promise) {
            //    if ($scope.reporttype === "groupwise") {
            //        $scope.getgrouplist = promise.getgroupdetails;
            //    } else if ($scope.reporttype === "indi") {
            //        $scope.getexamlist = promise.getexam;
            //    }
            //});
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getexamlist.some(function (options) {
                return options.EME_Id;
            });
        };

        $scope.isOptionsRequired2 = function () {
            return !$scope.getgrouplist.some(function (options) {
                return options.EMPS_Id;
            });
        };

        $scope.onclickdates = function () {
            $scope.ASMS_Id = "";
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];

        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {

            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];

            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                var data = "";

                data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id
                };

                apiService.create("JSHSExamReports/stjamesexamconsolidatedreport", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {
                            $scope.JSHSReport = true;

                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlistnew;
                            console.log($scope.getstudentwisesubjectlist);

                            $scope.tempsubjectdetails = [];

                            angular.forEach($scope.getstudentwisesubjectlist, function (dd) {
                                $scope.tempsubjectdetails.push({
                                    subid: dd.subid, subjectname: dd.subjectname, subjectorder_new: dd.subjectorder_new, subjectorder: dd.subjectorder,
                                    EYCES_AplResultFlg: dd.EYCES_AplResultFlg, ISMS_SubjectCode: dd.ISMS_SubjectCode, complusoryflag: dd.complusoryflag
                                });
                                if (dd.complusoryflag !== 'C' && dd.EYCES_AplResultFlg === true) {
                                    $scope.tempsubjectdetails.push({
                                        subid: dd.subid, subjectname: 'Points', subjectorder_new: dd.subjectorder_new, subjectorder: dd.subjectorder,
                                        EYCES_AplResultFlg: dd.EYCES_AplResultFlg, ISMS_SubjectCode: dd.ISMS_SubjectCode, complusoryflag: dd.complusoryflag
                                    });
                                }
                            });

                            console.log($scope.tempsubjectdetails);
                            $scope.getstudentdetails = promise.getstudentdetails;
                            $scope.getexamdetails = promise.getexamdetails;
                            $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;
                            $scope.getexamwisetotaldetails = promise.getexamwisetotaldetails;

                            $scope.getgroupexamdetails = promise.getgroupexamdetails;

                            $scope.getexamdetails.push({ empG_GroupName: "Final Average", empsG_Order: 10001, empG_DistplayName: "FI" });

                            $scope.getgroupexamdetails.push({
                                empG_GroupName: "Final Average", emE_Id: 50001, emE_ExamName: "Final Average", empG_DistplayName: "FI"
                            });

                            $scope.groupexam = [];
                            angular.forEach($scope.getexamdetails, function (dd) {
                                $scope.groupexam = [];
                                angular.forEach($scope.getgroupexamdetails, function (d) {
                                    if (d.empG_GroupName === dd.empG_GroupName) {
                                        $scope.groupexam.push(d);
                                    }
                                });
                                dd.groupexamdetails = $scope.groupexam;
                            });
                            console.log('rrr');
                            console.log($scope.getexamdetails);


                            // Student Subject Wise
                            $scope.subjectdetails = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.subjectdetails = [];
                                angular.forEach($scope.getstudentwisesubjectlist, function (sub) {
                                    if (stu.AMST_Id === parseInt(sub.AMST_Id)) {
                                        $scope.subjectdetails.push({
                                            ISMS_SubjectName: sub.subjectname, ISMS_Id: sub.subid,
                                            EYCES_AplResultFlg: sub.EYCES_AplResultFlg, subjectorder: sub.subjectorder
                                        });
                                    }
                                });
                                stu.subjects = $scope.subjectdetails;
                            });

                            // Student Marks Wise
                            $scope.marksdetails = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.marksdetails = [];
                                angular.forEach($scope.getstudentmarksdetails, function (subd) {
                                    if (stu.AMST_Id === parseInt(subd.AMST_Id)) {
                                        $scope.marksdetails.push({
                                            AMST_Id: subd.AMST_Id, ISMS_Id_Old: subd.ISMS_Id_Old, ISMS_SubjectName_Old: subd.ISMS_SubjectName_Old,
                                            EMPSG_GroupName: subd.EMPSG_GroupName, ESTMPPSG_GroupMaxMarks: subd.ESTMPPSG_GroupMaxMarks,
                                            ESTMPPSG_GroupObtMarks_Old: subd.ESTMPPSG_GroupObtMarks_Old, EMPS_SubjOrder: subd.EMPS_SubjOrder,
                                            EMPS_AppToResultFlg: subd.EMPS_AppToResultFlg, ESTMPPSG_GroupObtGrade: subd.ESTMPPSG_GroupObtGrade,
                                            ESTMPPSG_GradePoints: subd.ESTMPPSG_GradePoints, grporder: subd.grporder,
                                            ESTMPPS_PassFailFlg: subd.ESTMPPS_PassFailFlg, EMPSG_DisplayName: subd.EMPSG_DisplayName,
                                            ESG_Id: subd.ESG_Id, subjectgrporder: subd.subjectgrporder, complusoryflag: subd.complusoryflag, ISMS_Id: subd.ISMS_Id,
                                            ISMS_SubjectName: subd.ISMS_SubjectName, ESTMPPSG_GroupObtMarks: subd.ESTMPPSG_GroupObtMarks, Marksdispaly: subd.Marksdispaly, Gradedisplay: subd.Gradedisplay
                                        });
                                        if (subd.EMPS_AppToResultFlg === true && subd.complusoryflag !== 'C') {
                                            $scope.marksdetails.push({
                                                AMST_Id: subd.AMST_Id, ISMS_Id_Old: subd.ISMS_Id_Old, ISMS_SubjectName_Old: 'Points',
                                                EMPSG_GroupName: subd.EMPSG_GroupName, ESTMPPSG_GroupMaxMarks: subd.ESTMPPSG_GroupMaxMarks,
                                                ESTMPPSG_GroupObtMarks_Old: subd.ESTMPPSG_GroupObtMarks_Old, EMPS_SubjOrder: subd.EMPS_SubjOrder,
                                                EMPS_AppToResultFlg: subd.EMPS_AppToResultFlg, ESTMPPSG_GroupObtGrade: subd.ESTMPPSG_GroupObtGrade,
                                                ESTMPPSG_GradePoints: subd.ESTMPPSG_GradePoints, grporder: subd.grporder,
                                                ESTMPPS_PassFailFlg: subd.ESTMPPS_PassFailFlg, EMPSG_DisplayName: subd.EMPSG_DisplayName,
                                                ESG_Id: subd.ESG_Id, subjectgrporder: subd.subjectgrporder, complusoryflag: subd.complusoryflag, ISMS_Id: subd.ISMS_Id,
                                                ISMS_SubjectName: 'Points', ESTMPPSG_GroupObtMarks: subd.ESTMPPSG_GroupObtMarks, Marksdispaly: subd.Marksdispaly, Gradedisplay: subd.Gradedisplay
                                            });
                                        }
                                    }
                                });
                                stu.marks = $scope.marksdetails;
                            });

                            // Over All Total                           
                            $scope.totalgrade = [];
                            angular.forEach($scope.getstudentdetails, function (st) {
                                $scope.totalgrade = [];
                                angular.forEach($scope.getexamwisetotaldetails, function (su) {
                                    if (parseInt(st.AMST_Id) === parseInt(su.AMST_Id)) {
                                        $scope.totalgrade.push(su);
                                    }
                                });
                                st.markstotal = $scope.totalgrade;
                            });

                            // Student Attendance Wise
                            $scope.attendancedetails = [];
                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.attendancedetails = [];
                                angular.forEach($scope.getstudentwiseattendancedetails, function (d) {
                                    if (dd.AMST_Id === d.AMST_Id) {
                                        $scope.attendancedetails.push(d);
                                    }
                                });
                                dd.attendance = $scope.attendancedetails;
                            });



                            console.log($scope.getstudentdetails);

                            angular.forEach($scope.year_list, function (dd) {
                                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.year = dd.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.class_list, function (dd) {
                                if (dd.asmcL_Id === parseInt($scope.ASMCL_Id)) {
                                    $scope.cla = dd.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.section_list, function (dd) {
                                if (dd.asmS_Id === parseInt($scope.ASMS_Id)) {
                                    $scope.sec = dd.asmC_SectionName;
                                }
                            });

                        } else {
                            swal("No Records Found");
                        }
                    }

                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.printToCartd = function () {
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.reporttype === "indi") {
                innerContents = document.getElementById("Baldwin").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="/css/print/StJames/progresscardpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            } else if ($scope.reporttype === "promotion") {
                innerContents = document.getElementById("Baldwin1").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="/css/print/StJames/consolidatedreportpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' + '<style type="text/css">        @media print {    table {    page-break-inside: auto; }         tbody {   page-break-inside: avoid;     page-break-after: auto;   }  }    </style>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIdProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.Excel_HHS02 = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var excelname = "CONSOLIDATED MARKS SHEET REPORT.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
    }
})();