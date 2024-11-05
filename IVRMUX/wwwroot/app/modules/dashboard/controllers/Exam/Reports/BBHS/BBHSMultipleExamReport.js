
(function () {
    'use strict';
    angular.module('app').controller('BBHSMultipleExamReportController', BBHSMultipleExamReportController)

    BBHSMultipleExamReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function BBHSMultipleExamReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.readmit = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.getexamlist = [];
        $scope.obj = {};
        $scope.generateddate = new Date();

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
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("JSHSExamReports/get_Exam_grade", data).then(function (promise) {
                $scope.grade_list = promise.getgradelist;
                $scope.getexamlist = promise.getexam;

                if ($scope.getexamlist !== null && $scope.getexamlist.length > 0) {
                    $scope.examlist = $scope.getexamlist;
                } else {
                    swal("No Exam Is Mapped For Selected Details");
                    $scope.termlist = [];
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getexamlist.some(function (options) {
                return options.EME_Id;
            });
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
                angular.forEach($scope.getexamlist, function (term) {
                    if (term.EME_Id === true) {
                        $scope.termlisttemp.push({ EME_Id: term.emE_Id, EME_ExamName: term.emE_ExamName });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "examlist": $scope.termlisttemp,
                    "flagtype": "BBHS",
                };

                apiService.create("JSHSExamReports/getmultiple_exam_progress_report", data).then(function (promise) {

                    if (promise !== null) {

                        if (promise.getexamsubjectwisemarksdetails !== null && promise.getexamsubjectwisemarksdetails.length > 0) {

                            angular.forEach($scope.year_list, function (yr) {
                                if (yr.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.year = yr.asmaY_Year;
                                }
                            });

                            $scope.JSHSReport = true;
                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                            $scope.getstudentdetails = promise.getstudentdetails;
                            $scope.getexamdetails = promise.getexamdetails;
                            $scope.getexamsubjectwisemarksdetails = promise.getexamsubjectwisemarksdetails;
                            $scope.getexamwisetotaldetails = promise.getexamwisetotaldetails;
                            $scope.getstudentmarksindidetails = promise.getstudentmarksindidetails;
                            $scope.stud_work_attendences = promise.work_attendence;
                            $scope.stud_present_attendences = promise.present_attendence;

                            $scope.subcolumns = [];
                            angular.forEach($scope.getexamdetails, function (dd) {
                                $scope.subcolumns.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id + '_1', columnname: "MAX MARKS" });
                                $scope.subcolumns.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id + '_2', columnname: "MIN MARKS" });
                                $scope.subcolumns.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id + '_3', columnname: "MARKS" });
                                $scope.subcolumns.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id + '_4', columnname: "GRADE" });
                                $scope.subcolumns.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id + '_5', columnname: "SECTION HIGHEST" });
                                $scope.subcolumns.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id + '_6', columnname: "CLASS HIGHEST" });
                            });

                            //Student Wise Subject List
                            $scope.subjectdetails = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.subjectdetails = [];
                                angular.forEach($scope.getstudentwisesubjectlist, function (sub) {
                                    if (stu.AMST_Id === sub.AMST_Id) {
                                        if ($scope.subjectdetails.length === 0) {
                                            $scope.subjectdetails.push(sub);
                                        } else if ($scope.subjectdetails.length > 0) {
                                            var count = 0;
                                            angular.forEach($scope.subjectdetails, function (stu_sub) {
                                                if (stu_sub.ISMS_Id === sub.ISMS_Id) {
                                                    count += 1;
                                                }
                                            });

                                            if (count === 0) {
                                                $scope.subjectdetails.push(sub);
                                            }
                                        }
                                    }
                                });
                                stu.subjects = $scope.subjectdetails;
                            });

                            //Student Wise Subject Wise Marks List
                            $scope.marksdetails = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.marksdetails = [];
                                angular.forEach(stu.subjects, function (stu_subj) {
                                    $scope.marksdetails = [];
                                    angular.forEach($scope.getstudentwisesubjectlist, function (dd) {
                                        if (dd.ISMS_Id === stu_subj.ISMS_Id && stu.AMST_Id === dd.AMST_Id) {
                                            $scope.marksdetails.push({
                                                ISMS_Id: stu_subj.ISMS_Id, EME_Id: dd.EME_Id, columnid: dd.EME_Id + "_1",
                                                columnname: dd.EYCES_MaxMarks
                                            });
                                            $scope.marksdetails.push({
                                                ISMS_Id: stu_subj.ISMS_Id, EME_Id: dd.EME_Id, columnid: dd.EME_Id + "_2",
                                                columnname: dd.EYCES_MinMarks
                                            });
                                        }
                                    });

                                    angular.forEach($scope.getexamsubjectwisemarksdetails, function (sub) {
                                        if (stu.AMST_Id === sub.amsT_Id && stu_subj.ISMS_Id === sub.ismS_Id) {

                                           // if (stu_subj.EYCES_MarksDisplayFlg === true) {
                                                $scope.marksdetails.push({
                                                    ISMS_Id: stu_subj.ISMS_Id, EME_Id: sub.emE_Id, columnid: sub.emE_Id + "_3",
                                                    columnname: sub.estmpS_PassFailFlg !== 'AB' ? sub.estmpS_ObtainedMarks : sub.estmpS_PassFailFlg
                                                });
                                            //}
                                           
                                            //if (stu_subj.EYCES_GradeDisplayFlg === true) {
                                                $scope.marksdetails.push({
                                                    ISMS_Id: stu_subj.ISMS_Id, EME_Id: sub.emE_Id, columnid: sub.emE_Id + "_4",
                                                    columnname: sub.estmpS_PassFailFlg !== 'AB' ? sub.estmpS_ObtainedGrade : sub.estmpS_PassFailFlg
                                                });
                                           // }                                            

                                            $scope.marksdetails.push({
                                                ISMS_Id: stu_subj.ISMS_Id, EME_Id: sub.emE_Id, columnid: sub.emE_Id + "_5",
                                                columnname: sub.estmpS_SectionHighest
                                            });

                                            $scope.marksdetails.push({
                                                ISMS_Id: stu_subj.ISMS_Id, EME_Id: sub.emE_Id, columnid: sub.emE_Id + "_6",
                                                columnname: sub.estmpS_ClassHighest
                                            });
                                        }
                                    });
                                    stu_subj.marks = $scope.marksdetails;
                                });
                            });

                            //Percentage, Ranks
                            $scope.totalmarksdetails = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.totalmarksdetails = [];
                                angular.forEach($scope.getexamwisetotaldetails, function (sub) {
                                    if (stu.AMST_Id === sub.amsT_Id) {
                                        $scope.totalmarksdetails.push(sub);
                                    }
                                });
                                stu.totalmarks = $scope.totalmarksdetails;
                            });

                            //Grand Total Details
                            $scope.grandtotalmarksdetails = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.grandtotalmarksdetails = [];
                                angular.forEach($scope.getstudentmarksindidetails, function (stu_marks) {
                                    if (stu.AMST_Id === stu_marks.AMST_Id) {

                                        $scope.grandtotalmarksdetails.push({
                                            EME_Id: stu_marks.EME_Id, columnid: stu_marks.EME_Id + "_1",
                                            columnname: stu_marks.EYCES_MaxMarks
                                        });

                                        $scope.grandtotalmarksdetails.push({
                                            EME_Id: stu_marks.EME_Id, columnid: stu_marks.EME_Id + "_2",
                                            columnname: stu_marks.EYCES_MinMarks
                                        });

                                        $scope.grandtotalmarksdetails.push({
                                            EME_Id: stu_marks.EME_Id, columnid: stu_marks.EME_Id + "_3",
                                            columnname: stu_marks.ESTMP_TotalObtMarks
                                        });
                                    }
                                });

                                stu.grand_total_marks = $scope.grandtotalmarksdetails;

                            });

                            //Student Wise Working Days
                            $scope.studentwiseworkingdays = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.studentwiseworkingdays = [];
                                angular.forEach($scope.stud_work_attendences, function (stu_marks) {
                                    if (stu.AMST_Id === stu_marks.AMST_Id) {
                                        $scope.studentwiseworkingdays.push(stu_marks);                              
                                    }
                                });
                                stu.stud_work_attendence = $scope.studentwiseworkingdays;
                            });

                            //Student Wise Present Days
                            $scope.studentwisepresentdays = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.studentwisepresentdays = [];
                                angular.forEach($scope.stud_present_attendences, function (stu_marks) {
                                    if (stu.AMST_Id === stu_marks.AMST_Id) {
                                        $scope.studentwisepresentdays.push(stu_marks);
                                    }
                                });
                                stu.stud_present_attendence = $scope.studentwisepresentdays;
                            });

                            console.log($scope.subcolumns);
                            console.log($scope.getstudentdetails);
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
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BBIVProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();