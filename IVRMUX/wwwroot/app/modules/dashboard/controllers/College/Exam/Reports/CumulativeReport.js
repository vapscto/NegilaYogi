
(function () {
    'use strict';
    angular.module('app').controller('CollegeExamWiseCumulativeReportController', CollegeExamWiseCumulativeReportController)
    CollegeExamWiseCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function CollegeExamWiseCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.searchchkbx = "";

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
            if ($scope.myForm.$valid) {
                $scope.studentlist_temp = [];
                angular.forEach($scope.studentlist, function (d) {
                    $scope.studentlist_temp.push({ AMCST_Id: d.amcsT_Id });
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

                apiService.create("ClgCumulativeReport/GetCumulativeReportFormat2", data).then(function (promise) {
                    $scope.report = true;
                    if (promise.getStudentWiseSubjectMakrs !== null && promise.getStudentWiseSubjectMakrs.length > 0) {
                        $scope.colarrayall = [];
                        $scope.datagriv = true;
                        $scope.configuration = promise.configuration;
                        var count = 0;

                        $scope.GetStudentList = promise.getStudentList;
                        $scope.GetStudentWiseSubjectMakrs = promise.getStudentWiseSubjectMakrs;
                        $scope.GetStudentWiseMakrs = promise.getStudentWiseMakrs;

                        $scope.colarrayall = [
                            { title: "SLNO", template: "<span class='row-number'></span>", width: 100, locked: "true" },
                            { name: 'studentname', field: 'studentname', title: 'Student Name', width: 200, locked: "true" }];

                        if ($scope.configuration !== null && $scope.configuration.length > 0) {
                            if ($scope.configuration[0].exmConfig_RegnoColumnDisplay === true) {
                                $scope.colarrayall.push({ name: 'AMCST_RegistrationNo', field: 'AMCST_RegistrationNo', title: 'Reg.NO', width: 100, });
                                count = count + 1;
                            }
                            if ($scope.configuration[0].exmConfig_AdmnoColumnDisplay === true) {
                                $scope.colarrayall.push({ name: 'AMCST_AdmNo', field: 'AMCST_AdmNo', title: 'Adm.NO', width: 100 });
                                count = count + 1;
                            }
                            if ($scope.configuration[0].exmConfig_RollnoColumnDisplay === true) {
                                $scope.colarrayall.push({ name: 'ACYST_RollNo', field: 'ACYST_RollNo', title: 'Roll.NO', width: 100 });
                                count = count + 1;
                            }
                            if (count === 0) {
                                $scope.colarrayall.push({ name: 'AMCST_AdmNo', field: 'AMCST_AdmNo', title: 'Adm.NO', width: 100 });
                            }
                        } else {
                            $scope.colarrayall.push({ name: 'AMCST_AdmNo', field: 'AMCST_AdmNo', title: 'Adm.NO', width: 100 });
                        }


                        $scope.GetSubjectList = promise.getSubjectList;
                        $scope.distinct_subject = [];
                        var subjcount = 0;
                        angular.forEach($scope.GetSubjectList, function (dd) {
                            if ($scope.distinct_subject.length === 0) {
                                $scope.distinct_subject.push({
                                    ISMS_SubjectName: dd.ISMS_SubjectName, ISMS_Id: dd.ISMS_Id, Order: dd.ECYSES_SubjectOrder,
                                    ECYSES_SubExamFlg: dd.ECYSES_SubExamFlg, ECYSES_SubSubjectFlg: dd.ECYSES_SubSubjectFlg,
                                    ECYSES_MarksDisplayFlg: dd.ECYSES_MarksDisplayFlg, ECYSES_GradeDisplayFlg: dd.ECYSES_GradeDisplayFlg,
                                    ECYSES_AplResultFlg: dd.ECYSES_AplResultFlg
                                });
                            } else if ($scope.distinct_subject.length > 0) {
                                subjcount = 0;
                                angular.forEach($scope.distinct_subject, function (d) {
                                    if (dd.ISMS_Id === d.ISMS_Id) {
                                        subjcount += 1;
                                    }
                                });

                                if (subjcount === 0) {
                                    $scope.distinct_subject.push({
                                        ISMS_SubjectName: dd.ISMS_SubjectName, ISMS_Id: dd.ISMS_Id, Order: dd.ECYSES_SubjectOrder,
                                        ECYSES_SubExamFlg: dd.ECYSES_SubExamFlg, ECYSES_SubSubjectFlg: dd.ECYSES_SubSubjectFlg,
                                        ECYSES_MarksDisplayFlg: dd.ECYSES_MarksDisplayFlg, ECYSES_GradeDisplayFlg: dd.ECYSES_GradeDisplayFlg,
                                        ECYSES_AplResultFlg: dd.ECYSES_AplResultFlg
                                    });
                                }
                            }
                        });

                        angular.forEach($scope.distinct_subject, function (sub) {
                            $scope.get_sub_subject = [];
                            $scope.get_sub_exam = [];
                            angular.forEach($scope.GetSubjectList, function (sub_subj) {
                                if (sub.ISMS_Id === sub_subj.ISMS_Id) {
                                    //Sub Subject
                                    if (sub.ECYSES_SubSubjectFlg === true) {
                                        if ($scope.get_sub_subject.length === 0) {
                                            $scope.get_sub_subject.push({
                                                ISMS_Id: sub_subj.ISMS_Id, EMSS_Id: sub_subj.EMSS_Id,
                                                EMSS_SubSubjectName: sub_subj.EMSS_SubSubjectName
                                            });
                                        } else if ($scope.get_sub_subject.length > 0) {
                                            var count_sub_subj = 0;
                                            angular.forEach($scope.get_sub_subject, function (subsubj) {
                                                if (sub_subj.EMSS_Id === subsubj.EMSS_Id && subsubj.ISMS_Id === sub_subj.ISMS_Id) {
                                                    count_sub_subj += 1;
                                                }
                                            });
                                            if (count_sub_subj === 0) {
                                                $scope.get_sub_subject.push({
                                                    ISMS_Id: sub_subj.ISMS_Id, EMSS_Id: sub_subj.EMSS_Id,
                                                    EMSS_SubSubjectName: sub_subj.EMSS_SubSubjectName
                                                });
                                            }
                                        }
                                    }
                                    //Sub Exam
                                    if (sub.ECYSES_SubExamFlg === true && sub.ECYSES_SubSubjectFlg === false) {
                                        if ($scope.get_sub_exam.length === 0) {
                                            $scope.get_sub_exam.push({
                                                ISMS_Id: sub_subj.ISMS_Id, EMSE_Id: sub_subj.EMSE_Id,
                                                EMSE_SubExamName: sub_subj.EMSE_SubExamName
                                            });
                                        } else if ($scope.get_sub_exam.length > 0) {
                                            var count_sub_exam = 0;
                                            angular.forEach($scope.get_sub_exam, function (subsubj) {
                                                if (sub_subj.EMSE_Id === subsubj.EMSE_Id && subsubj.ISMS_Id === sub_subj.ISMS_Id) {
                                                    count_sub_exam += 1;
                                                }
                                            });
                                            if (count_sub_exam === 0) {
                                                $scope.get_sub_exam.push({
                                                    ISMS_Id: sub_subj.ISMS_Id, EMSE_Id: sub_subj.EMSE_Id,
                                                    EMSE_SubExamName: sub_subj.EMSE_SubExamName
                                                });
                                            }
                                        }
                                    }
                                }
                            });

                            // Sub Subject Sub Exam 
                            $scope.get_sub_subject_sub_exam = [];
                            angular.forEach($scope.get_sub_subject, function (sub_subj_exam) {
                                $scope.get_sub_subject_sub_exam = [];
                                angular.forEach($scope.GetSubjectList, function (dd) {
                                    if (sub_subj_exam.ISMS_Id === dd.ISMS_Id && sub_subj_exam.EMSS_Id === dd.EMSS_Id) {
                                        $scope.get_sub_subject_sub_exam.push({
                                            ISMS_Id: dd.ISMS_Id, EMSS_Id: dd.EMSS_Id, EMSE_Id: dd.EMSE_Id, EMSE_SubExamName: dd.EMSE_SubExamName
                                        });
                                    }
                                });
                                sub_subj_exam.get_sub_subject_sub_exam = $scope.get_sub_subject_sub_exam;
                            });

                            sub.sub_subject_list = $scope.get_sub_subject;
                            sub.sub_exam_list = $scope.get_sub_exam;
                        });
                        console.log($scope.distinct_subject);

                        //Creating Subject & Sub Subject & Sub Exam Wise Column list
                        angular.forEach($scope.distinct_subject, function (cols) {
                            if (cols.ECYSES_SubSubjectFlg === false && cols.ECYSES_SubExamFlg === false) {
                                $scope.colarrayall.push({ name: "unique_" + cols.ISMS_Id, field: "unique_" + cols.ISMS_Id, title: cols.ISMS_SubjectName, width: 100 });
                            }

                            else if (cols.ECYSES_SubSubjectFlg === true && cols.ECYSES_SubExamFlg === false) {
                                $scope.sub_subject_list = [];
                                angular.forEach(cols.sub_subject_list, function (cols_sub_subj) {
                                    $scope.sub_subject_list.push({
                                        name: "unique_" + cols_sub_subj.ISMS_Id + '_' + cols_sub_subj.EMSS_Id,
                                        field: "unique_" + cols_sub_subj.ISMS_Id + '_' + cols_sub_subj.EMSS_Id, title: cols_sub_subj.EMSS_SubSubjectName, width: 100
                                    });
                                });

                                $scope.sub_subject_list.push({
                                    name: "unique_" + cols.ISMS_Id + '_Sub_Subject_Total_0',
                                    field: "unique_" + cols.ISMS_Id + '_Sub_Subject_Total_0', title: 'Total', width: 100
                                });

                                $scope.colarrayall.push({
                                    name: "unique_" + cols.ISMS_Id, field: "unique_" + cols.ISMS_Id, title: cols.ISMS_SubjectName, width: 100,
                                    columns: $scope.sub_subject_list
                                });
                            }

                            else if (cols.ECYSES_SubSubjectFlg === false && cols.ECYSES_SubExamFlg === true) {
                                $scope.sub_exam_list = [];
                                angular.forEach(cols.sub_exam_list, function (cols_sub_subj) {
                                    $scope.sub_exam_list.push({
                                        name: "unique_" + cols_sub_subj.ISMS_Id + '_' + cols_sub_subj.EMSE_Id,
                                        field: "unique_" + cols_sub_subj.ISMS_Id + '_' + cols_sub_subj.EMSE_Id, title: cols_sub_subj.EMSE_SubExamName, width: 100
                                    });
                                });

                                $scope.sub_exam_list.push({
                                    name: "unique_" + cols.ISMS_Id + '_Sub_Exam_Total_0',
                                    field: "unique_" + cols.ISMS_Id + '_Sub_Exam_Total_0', title: 'Total', width: 100
                                });

                                $scope.colarrayall.push({
                                    name: "unique_" + cols.ISMS_Id, field: "unique_" + cols.ISMS_Id, title: cols.ISMS_SubjectName, width: 100,
                                    columns: $scope.sub_exam_list
                                });
                            }

                            else if (cols.ECYSES_SubSubjectFlg === true && cols.ECYSES_SubExamFlg === true) {
                                $scope.sub_subject_list = [];
                                angular.forEach(cols.sub_subject_list, function (cols_sub_subj) {
                                    $scope.sub_subject_subexam_list = [];
                                    angular.forEach(cols_sub_subj.get_sub_subject_sub_exam, function (cols_exam) {
                                        if (cols_exam.EMSS_Id === cols_sub_subj.EMSS_Id && cols_exam.ISMS_Id == cols_sub_subj.ISMS_Id) {
                                            $scope.sub_subject_subexam_list.push({
                                                name: "unique_" + cols_sub_subj.ISMS_Id + '_' + cols_sub_subj.EMSS_Id + '_' + cols_exam.EMSE_Id,
                                                field: "unique_" + cols_sub_subj.ISMS_Id + '_' + cols_sub_subj.EMSS_Id + '_' + cols_exam.EMSE_Id,
                                                title: cols_exam.EMSE_SubExamName, width: 100
                                            });
                                        }
                                    });

                                    $scope.sub_subject_list.push({
                                        name: "unique_" + cols_sub_subj.ISMS_Id + '_' + cols_sub_subj.EMSS_Id,
                                        field: "unique_" + cols_sub_subj.ISMS_Id + '_' + cols_sub_subj.EMSS_Id,
                                        title: cols_sub_subj.EMSS_SubSubjectName, width: 100,
                                        columns: $scope.sub_subject_subexam_list
                                    });
                                });

                                $scope.sub_subject_list.push({
                                    name: "unique_" + cols.ISMS_Id + '_Sub_Subj_Sub_Exam_Total_0',
                                    field: "unique_" + cols.ISMS_Id + '_Sub_Subj_Sub_Exam_Total_0',
                                    title: 'Total', width: 100
                                });

                                $scope.colarrayall.push({
                                    name: "unique_" + cols.ISMS_Id, field: "unique_" + cols.ISMS_Id, title: cols.ISMS_SubjectName, width: 100,
                                    columns: $scope.sub_subject_list
                                });
                            }
                        });

                        $scope.colarrayall.push({ name: 'FinalTotalObtainedMarks', field: 'FinalTotalObtainedMarks', title: 'Total Obtanied Marks', width: 100 });
                        $scope.colarrayall.push({ name: 'FinalResult', field: 'FinalResult', title: 'Result', width: 100 });

                        console.log($scope.colarrayall);
                        $scope.details = [];
                        angular.forEach($scope.GetStudentList, function (stu) {
                            $scope.details.push({
                                studentname: stu.studentname, AMCST_AdmNo: stu.AMCST_AdmNo, ACYST_RollNo: stu.ACYST_RollNo,
                                AMCST_RegistrationNo: stu.AMCST_RegistrationNo, AMCST_Id: stu.AMCST_Id
                            });
                        });


                        angular.forEach($scope.details, function (stu) {
                            $scope.student_wise_marks = [];
                            angular.forEach($scope.GetStudentWiseSubjectMakrs, function (stu_subj_marks) {
                                if (stu.AMCST_Id === stu_subj_marks.AMCST_Id) {
                                    if ($scope.student_wise_marks.length === 0) {
                                        $scope.student_wise_marks.push({
                                            AMCST_Id: stu_subj_marks.AMCST_Id, ISMS_Id: stu_subj_marks.ISMS_Id,
                                            ISMS_SubjectName: stu_subj_marks.ISMS_SubjectName,
                                            ECYSES_AplResultFlg: stu_subj_marks.ECYSES_AplResultFlg,
                                            ECSTMPS_MaxMarks: stu_subj_marks.ECSTMPS_MaxMarks,
                                            ECSTMPS_ObtainedMarks: stu_subj_marks.ECSTMPS_ObtainedMarks,
                                            ECSTMPS_ObtainedGrade: stu_subj_marks.ECSTMPS_ObtainedGrade,
                                            ECSTMPS_PassFailFlg: stu_subj_marks.ECSTMPS_PassFailFlg,
                                            ECYSES_SubExamFlg: stu_subj_marks.ECYSES_SubExamFlg,
                                            ECYSES_SubSubjectFlg: stu_subj_marks.ECYSES_SubSubjectFlg
                                        });
                                    } else if ($scope.student_wise_marks.length > 0) {
                                        var subj_count = 0;

                                        angular.forEach($scope.student_wise_marks, function (dd) {
                                            if (dd.AMCST_Id === stu.AMCST_Id && dd.ISMS_Id === stu_subj_marks.ISMS_Id) {
                                                subj_count += 1;
                                            }
                                        });

                                        if (subj_count === 0) {
                                            $scope.student_wise_marks.push({
                                                AMCST_Id: stu_subj_marks.AMCST_Id, ISMS_Id: stu_subj_marks.ISMS_Id,
                                                ISMS_SubjectName: stu_subj_marks.ISMS_SubjectName,
                                                ECYSES_AplResultFlg: stu_subj_marks.ECYSES_AplResultFlg,
                                                ECSTMPS_MaxMarks: stu_subj_marks.ECSTMPS_MaxMarks,
                                                ECSTMPS_ObtainedMarks: stu_subj_marks.ECSTMPS_ObtainedMarks,
                                                ECSTMPS_ObtainedGrade: stu_subj_marks.ECSTMPS_ObtainedGrade,
                                                ECSTMPS_PassFailFlg: stu_subj_marks.ECSTMPS_PassFailFlg,
                                                ECYSES_SubExamFlg: stu_subj_marks.ECYSES_SubExamFlg,
                                                ECYSES_SubSubjectFlg: stu_subj_marks.ECYSES_SubSubjectFlg
                                            });
                                        }
                                    }
                                }
                            });

                            //Subject Wise SubSubject && SubExam
                            angular.forEach($scope.student_wise_marks, function (stu_subject) {
                                $scope.stu_subj_subsubj = [];
                                $scope.stu_subj_subexam = [];
                                angular.forEach($scope.GetStudentWiseSubjectMakrs, function (stu_subj) {
                                    if (stu_subject.AMCST_Id === stu.AMCST_Id && stu_subj.AMCST_Id === stu_subject.AMCST_Id && stu_subject.ISMS_Id == stu_subj.ISMS_Id) {
                                        if (stu_subject.ECYSES_SubSubjectFlg) {
                                            if ($scope.stu_subj_subsubj.length === 0) {
                                                $scope.stu_subj_subsubj.push({
                                                    AMCST_Id: stu_subj.AMCST_Id, ISMS_Id: stu_subj.ISMS_Id, EMSS_Id: stu_subj.EMSS_Id,
                                                    ECSTMPSSS_MaxMarks: stu_subj.ECSTMPSSS_MaxMarks,
                                                    ECSTMPSSS_ObtainedMarks: stu_subj.ECSTMPSSS_ObtainedMarks,
                                                    ECSTMPSSS_ObtainedGrade: stu_subj.ECSTMPSSS_ObtainedGrade,
                                                    ECSTMPSSS_PassFailFlg: stu_subj.ECSTMPSSS_PassFailFlg,
                                                });
                                            } else if ($scope.stu_subj_subsubj.length > 0) {
                                                var stu_subj_subsubj_temp = 0;
                                                angular.forEach($scope.stu_subj_subsubj, function (d_sub_subj) {
                                                    if (d_sub_subj.EMSS_Id === stu_subj.EMSS_Id && d_sub_subj.ISMS_Id === stu_subj.ISMS_Id
                                                        && d_sub_subj.AMCST_Id === stu_subj.AMCST_Id) {
                                                        stu_subj_subsubj_temp += 1;
                                                    }
                                                });

                                                if (stu_subj_subsubj_temp === 0) {
                                                    $scope.stu_subj_subsubj.push({
                                                        AMCST_Id: stu_subj.AMCST_Id, ISMS_Id: stu_subj.ISMS_Id, EMSS_Id: stu_subj.EMSS_Id,
                                                        ECSTMPSSS_MaxMarks: stu_subj.ECSTMPSSS_MaxMarks,
                                                        ECSTMPSSS_ObtainedMarks: stu_subj.ECSTMPSSS_ObtainedMarks,
                                                        ECSTMPSSS_ObtainedGrade: stu_subj.ECSTMPSSS_ObtainedGrade,
                                                        ECSTMPSSS_PassFailFlg: stu_subj.ECSTMPSSS_PassFailFlg,
                                                    });
                                                }
                                            }
                                        }
                                        if (stu_subject.ECYSES_SubExamFlg && !stu_subject.ECYSES_SubSubjectFlg) {
                                            $scope.stu_subj_subexam.push({
                                                AMCST_Id: stu_subj.AMCST_Id, ISMS_Id: stu_subj.ISMS_Id, EMSE_Id: stu_subj.EMSE_Id,
                                                ECSTMPSSS_MaxMarks: stu_subj.ECSTMPSSS_MaxMarks,
                                                ECSTMPSSS_ObtainedMarks: stu_subj.ECSTMPSSS_ObtainedMarks,
                                                ECSTMPSSS_ObtainedGrade: stu_subj.ECSTMPSSS_ObtainedGrade,
                                                ECSTMPSSS_PassFailFlg: stu_subj.ECSTMPSSS_PassFailFlg,
                                            });
                                        }
                                    }
                                });

                                //Subject SubSubject Wise SubExam

                                angular.forEach($scope.stu_subj_subsubj, function (d_sub_subj_exam) {
                                    $scope.stu_subj_subsubj_subexam = [];
                                    angular.forEach($scope.GetStudentWiseSubjectMakrs, function (mrks) {
                                        if (d_sub_subj_exam.AMCST_Id === mrks.AMCST_Id && d_sub_subj_exam.ISMS_Id === mrks.ISMS_Id && d_sub_subj_exam.EMSS_Id === mrks.EMSS_Id) {
                                            $scope.stu_subj_subsubj_subexam.push({
                                                AMCST_Id: mrks.AMCST_Id, ISMS_Id: mrks.ISMS_Id, EMSE_Id: mrks.EMSE_Id, EMSS_Id: mrks.EMSS_Id,
                                                ECSTMPSSS_MaxMarks: mrks.ECSTMPSSS_MaxMarks,
                                                ECSTMPSSS_ObtainedMarks: mrks.ECSTMPSSS_ObtainedMarks,
                                                ECSTMPSSS_ObtainedGrade: mrks.ECSTMPSSS_ObtainedGrade,
                                                ECSTMPSSS_PassFailFlg: mrks.ECSTMPSSS_PassFailFlg,
                                            });
                                        }
                                    });
                                    d_sub_subj_exam.subsubj_subexam = $scope.stu_subj_subsubj_subexam;
                                });
                                stu_subject.sub_subject = $scope.stu_subj_subsubj;
                                stu_subject.sub_exam = $scope.stu_subj_subexam;
                            });

                            stu.subjectlist = $scope.student_wise_marks;
                        });


                        //Creating Columns With Marks
                        angular.forEach($scope.details, function (stu) {
                            angular.forEach(stu.subjectlist, function (stu_subj) {
                                var subjectname = "unique_" + stu_subj.ISMS_Id;
                                var subject_subsubject_name_total = "unique_" + stu_subj.ISMS_Id + '_Sub_Subject_Total_0';
                                var subject_subexam_name_total = "unique_" + stu_subj.ISMS_Id + '_Sub_Exam_Total_0';
                                var subject_subsubj_subexam_name_total = "unique_" + stu_subj.ISMS_Id + '_Sub_Subj_Sub_Exam_Total_0';

                                if (stu_subj.ECYSES_SubExamFlg === false && stu_subj.ECYSES_SubSubjectFlg === false) {
                                    stu[subjectname] = stu_subj.ECSTMPS_ObtainedMarks;
                                }
                                else if (stu_subj.ECYSES_SubSubjectFlg === true && stu_subj.ECYSES_SubExamFlg === false) {
                                    angular.forEach(stu_subj.sub_subject, function (stu_subj_subsubj) {
                                        var subsubjectname = "unique_" + stu_subj_subsubj.ISMS_Id + '_' + stu_subj_subsubj.EMSS_Id;
                                        stu[subsubjectname] = stu_subj_subsubj.ECSTMPSSS_ObtainedMarks;
                                    });
                                    stu[subject_subsubject_name_total] = stu_subj.ECSTMPS_ObtainedMarks;
                                }
                                else if (stu_subj.ECYSES_SubSubjectFlg === false && stu_subj.ECYSES_SubExamFlg === true) {
                                    angular.forEach(stu_subj.sub_exam, function (stu_subj_subexam) {
                                        var subexamname = "unique_" + stu_subj_subexam.ISMS_Id + '_' + stu_subj_subexam.EMSE_Id;
                                        stu[subexamname] = stu_subj_subexam.ECSTMPSSS_ObtainedMarks;
                                    });
                                    stu[subject_subexam_name_total] = stu_subj.ECSTMPS_ObtainedMarks;
                                }

                                else if (stu_subj.ECYSES_SubSubjectFlg === true && stu_subj.ECYSES_SubExamFlg === true) {
                                    angular.forEach(stu_subj.sub_subject, function (stu_subj_subsubj) {
                                        angular.forEach(stu_subj_subsubj.subsubj_subexam, function (dd_subsubj_subexam) {
                                            var subexamname = "unique_" + dd_subsubj_subexam.ISMS_Id + '_' + dd_subsubj_subexam.EMSS_Id + '_' + dd_subsubj_subexam.EMSE_Id;
                                            stu[subexamname] = dd_subsubj_subexam.ECSTMPSSS_ObtainedMarks;
                                        });
                                    });
                                    stu[subject_subsubj_subexam_name_total] = stu_subj.ECSTMPS_ObtainedMarks;
                                }
                            });
                        });


                        angular.forEach($scope.details, function (stu) {
                            angular.forEach($scope.getStudentWiseMakrs, function (stu_mrks) {
                                if (stu.AMCST_Id === stu_mrks.AMCST_Id) {
                                    stu["FinalTotalObtainedMarks"] = stu_mrks.ECSTMP_TotalObtMarks;
                                    stu["FinalResult"] = stu_mrks.ECSTMP_Result;
                                }
                            });
                        });


                        console.log($scope.details);

                        angular.forEach($scope.exam_list, function (dd) {
                            if (dd.emE_Id === parseInt($scope.EME_Id)) {
                                $scope.examname = dd.emE_ExamName;
                            }
                        });


                        $(document).ready(function () {
                            $('#gridhhs').empty();
                            $("#gridhhs").kendoGrid({
                                toolbar: ["excel"],
                                excel: {
                                    fileName: $scope.examname + " Cumulative Report.xlsx",
                                    proxyURL: "",
                                    filterable: false,
                                    allPages: true,
                                },

                                excelExport: function (e) {
                                    var sheet = e.workbook.sheets[0];
                                    sheet.name = $scope.examname + " Cumulative Report";
                                },

                                dataSource: {
                                    data: $scope.details,
                                    pageSize: 100
                                },
                                sortable: true,
                                pageable: true,
                                groupable: false,
                                filterable: true,
                                columnMenu: true,
                                reorderable: true,
                                resizable: true,


                                columns: $scope.colarrayall,
                                dataBound: function () {
                                    var pagenum = this.dataSource.page();
                                    var pageitms = this.dataSource.pageSize();
                                    var rows = this.items();
                                    $(rows).each(function () {
                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                        var rowLabel = $(this).find(".row-number");
                                        $(rowLabel).html(index);
                                    });
                                }
                            });
                        });
                    } else {
                        swal("No Record Found");
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.OnClickAll = function () {
            angular.forEach($scope.studentlist, function (dd) {
                dd.checkedsub = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.studentlist.every(function (itm) { return itm.checkedsub; });
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.studentlist.some(function (options) {
                return options.checkedsub;
            });
        };


        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
    }
})();