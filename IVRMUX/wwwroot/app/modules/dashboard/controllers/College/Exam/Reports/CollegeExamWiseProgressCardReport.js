
(function () {
    'use strict';
    angular.module('app').controller('CollegeExamWiseProgressCardReportController', CollegeExamWiseProgressCardReportController)
    CollegeExamWiseProgressCardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function CollegeExamWiseProgressCardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.searchchkbx = "";

        $scope.dateofissue = new Date();
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("ClgCumulativeReport/Getdetails").then(function (promise) {
                $scope.yearlist = promise.yearlist;
                $scope.datagriv = false;
            });
        };


        $scope.onchangeyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.searchchkbx = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ClgCumulativeReport/onchangeyear", data).then(function (promise) {
                $scope.course_list = promise.courseslist;
            });
        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.searchchkbx = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("ClgCumulativeReport/onchangecourse", data).then(function (promise) {
                $scope.branch_list = promise.branchlist;
            });
        };

        $scope.onchangebranch = function () {

            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.searchchkbx = "";
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("ClgCumulativeReport/onchangebranch", data).then(function (promise) {
                $scope.semisters_list = promise.semisters;
            });
        };

        $scope.onchangesemester = function () {
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.searchchkbx = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("ClgCumulativeReport/onchangesemester", data).then(function (promise) {
                $scope.seclist = promise.sections;
                $scope.subjectschema_list = promise.subjectshemalist;
            });
        };

        $scope.onchangesubjectscheme = function () {
            $scope.EME_Id = "";
            $scope.ACST_Id = "";
            $scope.searchchkbx = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id
            };
            apiService.create("ClgCumulativeReport/onchangesubjectscheme", data).then(function (promise) {
                $scope.schmetype_list = promise.schmetypelist;
            });
        };


        $scope.onchangeschemetype = function () {
            $scope.EME_Id = "";
            $scope.searchchkbx = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.exam_list = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "ACST_Id": $scope.ACST_Id,
                "ACMS_Id": $scope.ACMS_Id
            };
            apiService.create("ClgCumulativeReport/onchangeschemetype", data).then(function (promise) {
                $scope.exam_list = promise.exmstdlist;
                $scope.studentlist = promise.studentlist;
                $scope.all = true;
                $timeout(function () { $scope.OnClickAll(); }, 1000);
            });
        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.Getreport = function () {
            $scope.submitted = true;
            $scope.searchchkbx = "";
            $scope.studentslt1 = [];
            $scope.student_list = [];
            if ($scope.myForm.$valid) {
                $scope.studentlist_temp = [];
                angular.forEach($scope.studentlist, function (d) {
                    if (d.checkedsub) { $scope.studentlist_temp.push({ AMCST_Id: d.amcsT_Id }); }
                });

                var data = {
                    "EME_ID": $scope.EME_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "ACSS_Id": $scope.ACSS_Id,
                    "ACST_Id": $scope.ACST_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "Studentlist_temp": $scope.studentlist_temp
                };

                apiService.create("ClgCumulativeReport/GetProgresscardReport", data).then(function (promise) {
                    $scope.report = true;
                    if (promise.getProgressCardReportList !== null && promise.getProgressCardReportList.length > 0) {
                        $scope.getProgressCardReportList = promise.getProgressCardReportList;

                        $scope.student_list = [];
                           $scope.stud_work_attendence = promise.work_attendence;
                        $scope.stud_present_attendence = promise.present_attendence;
                        angular.forEach($scope.getProgressCardReportList, function (stu) {
                            if ($scope.student_list.length === 0) {
                                $scope.student_list.push({
                                    AMCST_Id: stu.AMCST_Id, StudentName: stu.StudentName, AMCST_RegistrationNo: stu.AMCST_RegistrationNo,
                                    AMCST_AdmNo: stu.AMCST_AdmNo, EME_ExamName: stu.EME_ExamName, AMCO_CourseName: stu.AMCO_CourseName,
                                    AMB_BranchName: stu.AMB_BranchName, AMSE_SEMName: stu.AMSE_SEMName, ACMS_SectionName: stu.ACMS_SectionName,
                                    ACMS_SectionName: stu.ACMS_SectionName, ECSTMP_TotalObtMarks: stu.ECSTMP_TotalObtMarks,
                                    ECSTMP_Percentage: stu.ECSTMP_Percentage, ECSTMP_TotalGrade: stu.ECSTMP_TotalGrade,
                                    ECSTMP_SectionRank: stu.ECSTMP_SectionRank, ECSTMP_SemRank: stu.ECSTMP_SemRank,
                                    ECSTMP_TotalMaxMarks: stu.ECSTMP_TotalMaxMarks, ECSTMP_Result: stu.ECSTMP_Result,
                                });
                            }
                            else if ($scope.student_list.length > 0) {
                                var stu_count = 0;

                                angular.forEach($scope.student_list, function (stu_temp) {
                                    if (stu.AMCST_Id === stu_temp.AMCST_Id) {
                                        stu_count += 1;
                                    }
                                });
                                if (stu_count === 0) {
                                    $scope.student_list.push({
                                        AMCST_Id: stu.AMCST_Id, StudentName: stu.StudentName, AMCST_RegistrationNo: stu.AMCST_RegistrationNo,
                                        AMCST_AdmNo: stu.AMCST_AdmNo, EME_ExamName: stu.EME_ExamName, AMCO_CourseName: stu.AMCO_CourseName,
                                        AMB_BranchName: stu.AMB_BranchName, AMSE_SEMName: stu.AMSE_SEMName, ACMS_SectionName: stu.ACMS_SectionName,
                                        ACMS_SectionName: stu.ACMS_SectionName, ECSTMP_TotalObtMarks: stu.ECSTMP_TotalObtMarks,
                                        ECSTMP_Percentage: stu.ECSTMP_Percentage, ECSTMP_TotalGrade: stu.ECSTMP_TotalGrade,
                                        ECSTMP_SectionRank: stu.ECSTMP_SectionRank, ECSTMP_SemRank: stu.ECSTMP_SemRank,
                                        ECSTMP_TotalMaxMarks: stu.ECSTMP_TotalMaxMarks, ECSTMP_Result: stu.ECSTMP_Result,
                                    });
                                }
                            }
                        });

                        angular.forEach($scope.student_list, function (stu) {
                            $scope.subject_list = [];
                            angular.forEach($scope.getProgressCardReportList, function (stu_subj) {
                                if (stu.AMCST_Id === stu_subj.AMCST_Id) {
                                    $scope.subject_list.push({
                                        AMCST_Id: stu_subj.AMCST_Id, ISMS_Id: stu_subj.ISMS_Id,
                                        ISMS_SubjectName: stu_subj.ISMS_SubjectName, ISMS_SubjectCode: stu_subj.ISMS_SubjectCode,
                                        ECYSES_AplResultFlg: stu_subj.ECYSES_AplResultFlg, ECSTMPS_MaxMarks: stu_subj.ECSTMPS_MaxMarks,
                                        ECSTMPS_ObtainedMarks: stu_subj.ECSTMPS_ObtainedMarks, ECSTMPS_ObtainedGrade: stu_subj.ECSTMPS_ObtainedGrade,
                                        ECSTMPS_SemAverage: stu_subj.ECSTMPS_SemAverage, ECSTMPS_SectionAverage: stu_subj.ECSTMPS_SectionAverage,
                                        ECSTMPS_SemHighest: stu_subj.ECSTMPS_SemHighest, ECSTMPS_SectionHighest: stu_subj.ECSTMPS_SectionHighest,
                                        ECSTMPS_PassFailFlg: stu_subj.ECSTMPS_PassFailFlg
                                    });
                                }
                            });
                            stu.subject = $scope.subject_list;
                        });

                        console.log($scope.student_list);

                        angular.forEach($scope.exam_list, function (dd) {
                            if (dd.emE_Id === parseInt($scope.EME_Id)) {
                                $scope.examname = dd.emE_ExamName;
                            };
                        });

                        $scope.stud_work_attendence = promise.work_attendence;
                    } else {
                        swal("No Records Found");
                    }
                });
            }
        };

        $scope.exportToExcel = function (tableIds) {
            var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.obj = {};
        $scope.studentlist = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BBIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.OnClickAll = function () {
            var checkStatus = $scope.all;
            var count = 0;
            angular.forEach($scope.studentlist, function (itm) {
                itm.checkedsub = checkStatus;
                if (itm.checkedsub == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }


        $scope.isOptionsRequired3 = function () {
            return !$scope.studentlist.some(function (options) {
                return options.checkedsub;
            });
        }

        $scope.individual = function () {

            $scope.all = $scope.studentlist.every(function (options) {
                return options.checkedsub;
            });
        }

        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
    }
})();