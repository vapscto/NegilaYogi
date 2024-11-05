﻿(function () {
    'use strict';
    angular.module('app').controller('DPS_Exam_reportController', StjamesI_IIIProgressCardController)
    StjamesI_IIIProgressCardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window', 'Excel', '$timeout']
    function StjamesI_IIIProgressCardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window, Excel, $timeout) {
        $scope.readmit = false;
        $scope.DisplayMarks = false;
        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.Displayattendance = true;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.getexamlist = [];
        $scope.obj = {};
        $scope.generateddate = new Date();
        $scope.reporttype = 'indi';

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
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "reporttype": $scope.reporttype
            };

            apiService.create("JSHSExamReports/get_Exam_group", data).then(function (promise) {
                $scope.getexamlist = promise.getexam;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
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
        $scope.GetStjamesNurReport = function (obj) {

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
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id
                };

                apiService.create("JSHSExamReports/GetStjamesNurReport", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {

                            $scope.JSHSReport = true;

                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.getstudentdetails = promise.getstudentdetails;
                            $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                            $scope.getexamwisesubsubjectlist = promise.getexamwisesubsubjectlist;
                            $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;

                            $scope.examwiseremarks = promise.examwiseremarks;
                            $scope.studentwisemarks = promise.studentwisemarks;

                            // STUDENT WISE SUBJECT
                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.subjecttemplist = [];
                                angular.forEach($scope.getstudentwisesubjectlist, function (d) {
                                    if (dd.AMST_Id === d.AMST_Id) {
                                        $scope.subjecttemplist.push({
                                            ISMS_Id: d.ISMS_Id, ISMS_SubjectName: d.ISMS_SubjectName, EYCES_SubjectOrder: d.EYCES_SubjectOrder,
                                            EYCES_AplResultFlg: d.EYCES_AplResultFlg, EYCES_SubSubjectFlg: d.EYCES_SubSubjectFlg, EYCES_SubExamFlg: d.EYCES_SubExamFlg
                                        });
                                    }
                                });
                                dd.subjectlist = $scope.subjecttemplist;
                            });

                            // STUDENT WISE MARKS
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.studentmarks = [];
                                angular.forEach($scope.getstudentmarksdetails, function (stumrks) {
                                    if (stu.AMST_Id === stumrks.AMST_Id) {
                                        $scope.studentmarks.push(stumrks);
                                    }
                                });
                                stu.stdmarks = $scope.studentmarks;
                            });

                            // STUDENT WISE ATTENDANCE DETAILS
                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.subjecttemplist = [];
                                angular.forEach($scope.getstudentwiseattendancedetails, function (d) {
                                    if (dd.AMST_Id === d.AMST_Id) {
                                        dd.TOTALWORKINGDAYS = d.TOTALWORKINGDAYS;
                                        dd.PRESENTDAYS = d.PRESENTDAYS;
                                        dd.ATTENDANCEPERCENTAGE = d.ATTENDANCEPERCENTAGE;
                                    }
                                });
                            });


                            //STUDNET WISE REMARKS
                            if ($scope.examwiseremarks !== null && $scope.examwiseremarks.length > 0) {
                                angular.forEach($scope.getstudentdetails, function (d) {
                                    angular.forEach($scope.examwiseremarks, function (dd) {
                                        if (d.AMST_Id === dd.amsT_Id) {
                                            d.remaks = dd.emeR_Remarks;
                                        }
                                    });
                                });
                            }

                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.subjecttemplist = [];
                                angular.forEach($scope.studentwisemarks, function (d) {
                                    if (dd.AMST_Id === d.AMST_Id) {
                                        dd.ESTMP_TotalMaxMarks = d.estmP_TotalMaxMarks;
                                        dd.ESTMP_TotalConverionMaxMarks = d.estmP_TotalConverionMaxMarks;

                                        dd.ESTMP_TotalObtMarks = d.estmP_TotalObtMarks;
                                        dd.ESTMP_TotalConvertedMarks = d.estmP_TotalConvertedMarks;

                                        dd.ESTMP_TotalGrade = d.estmP_TotalGrade;
                                        dd.ESTMP_GradePoints = d.estmP_GradePoints;
                                    }
                                });

                            });




                            $scope.clstchname = promise.clstchname;
                            $scope.clastechname = "";
                            if ($scope.clstchname !== null && $scope.clstchname.length > 0) {
                                $scope.clastechname = $scope.clstchname[0].hrmE_EmployeeFirstName;
                            }

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

                            angular.forEach($scope.getexamlist, function (dd) {
                                if (dd.emE_Id === parseInt($scope.EME_Id)) {
                                    $scope.exam = dd.emE_ExamName;
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

        $scope.printToCart = function () {
            var innerContents = "";
            var popupWinindow = "";

            innerContents = document.getElementById("Baldwin").innerHTML;
            popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Stjames/Stjamesprogresscardpdf.css" />' +
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