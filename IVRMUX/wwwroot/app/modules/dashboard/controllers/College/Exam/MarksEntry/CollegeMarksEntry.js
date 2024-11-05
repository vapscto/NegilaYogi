(function () {
    'use strict';
    angular.module('app').controller('CollegeMarksEntryController', CollegeMarksEntryController)
    CollegeMarksEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams']
    function CollegeMarksEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams) {


        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (parseInt(privlgs[i].pageId) === parseInt(pageid)) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        $scope.MarkCalculation = false;
        $scope.BindData = function () {
            apiService.getDATA("CollegeMarksEntry/getalldetails").then(function (promise) {
                $scope.yearlist = promise.getyear;
                $scope.course_list = promise.courseslist;
                $scope.branch_list = promise.branchlist;
                $scope.semisters_list = promise.semisters;
                $scope.subject_list = promise.subjectlist;
                $scope.subjectgrp_list = promise.subjectgrplist;
                $scope.section_List = promise.sectionlist;
                $scope.exam_list = promise.examlist;
                $scope.gridOptions.data = "";
            });
        };

        $scope.onchangeyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("CollegeMarksEntry/onchangeyear", data).then(function (promise) {
                $scope.course_list = promise.courseslist;
            });

        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("CollegeMarksEntry/onchangecourse", data).then(function (promise) {
                $scope.branch_list = promise.branchlist;
            });
        };


        $scope.onchangebranch = function () {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("CollegeMarksEntry/onchangebranch", data).then(function (promise) {
                $scope.semisters_list = promise.semisters;
            });
        };

        $scope.get_exams = function () {
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            if ($scope.AMCO_Id !== "" && $scope.AMCO_Id !== undefined && $scope.AMSE_Id !== "" && $scope.AMSE_Id !== undefined && $scope.AMB_Id !== ""
                && $scope.AMB_Id !== undefined) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "AMB_Id": $scope.AMB_Id
                };
                apiService.create("CollegeMarksEntry/get_exams", data).then(function (promise) {
                    $scope.exam_list = promise.examlist;
                    $scope.section_List = promise.sectionlist;
                    $scope.subjectgrp_list = promise.subjectgrplist;
                });
            }
        };



        $scope.get_subjects = function () {
            $scope.ISMS_ID = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                //"EMG_ID": $scope.EMG_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "AMB_Id": $scope.AMB_Id,
                "ACMS_Id": $scope.ACMS_Id

            };
            apiService.create("CollegeMarksEntry/get_subjects", data).then(function (promise) {
                $scope.subject_list = promise.subjectgroups;
            });

        };

        $scope.getsubjectscheme = function () {
            $scope.ACSS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "AMB_Id": $scope.AMB_Id,
                "ACMS_Id": $scope.ACMS_Id,
                "ISMS_Id": $scope.ISMS_ID,
                "EME_Id": $scope.EME_Id

            };
            apiService.create("CollegeMarksEntry/getsubjectscheme", data).then(function (promise) {
                $scope.subjectsheme = promise.getsubjectschemetype;
            });
        };

        $scope.getsubjectschemetype = function () {
            $scope.ACST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "AMB_Id": $scope.AMB_Id,
                "ACMS_Id": $scope.ACMS_Id,
                "ISMS_Id": $scope.ISMS_ID,
                "ACSS_Id": $scope.ACSS_Id,
                "EME_Id": $scope.EME_Id
            };
            apiService.create("CollegeMarksEntry/getsubjectschemetype", data).then(function (promise) {
                $scope.subjectshemetype = promise.getschemetype;
            });
        };


        $scope.onsearch = function (ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": AMCO_Id,
                    "AMSE_Id": AMSE_Id,
                    "ACMS_Id": ACMS_Id,
                    "AMB_Id": AMB_Id,
                    "EME_Id": EME_Id,
                    "ISMS_Id": ISMS_Id,
                    "ACSS_Id": $scope.ACSS_Id,
                    "ACST_Id": $scope.ACST_Id,
                    "message" : ""
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("CollegeMarksEntry/onsearch", data).then(function (promise) {

                    console.log(promise.subject_subsubjects);
                    if (promise.studentList !== null && promise.studentList.length > 0) {
                        var count = 0;
                        $scope.configurationsettings = promise.configuration;

                        if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "Name") {
                            $scope.sortKey = "amcsT_FirstName";
                        }

                        else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "AdmNo") {
                            $scope.sortKey = "amcsT_AdmNo";
                        }

                        else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "RollNo") {
                            $scope.sortKey = "acysT_RollNo";
                        }

                        else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "RegNo") {
                            $scope.sortKey = "amcsT_RegistrationNo";
                        }

                        else {
                            $scope.sortKey = "amcsT_FirstName";
                        }

                        if ($scope.configurationsettings !== null) {
                            if ($scope.configurationsettings.length > 0) {
                                if ($scope.configurationsettings[0].exmConfig_RegnoColumnDisplay === true) {
                                    $scope.regno = true;
                                    count = count + 1;

                                } else {
                                    $scope.regno = false;
                                }

                                if ($scope.configurationsettings[0].exmConfig_AdmnoColumnDisplay === true) {
                                    $scope.admno = true;
                                    count = count + 1;
                                } else {
                                    $scope.admno = false;
                                }

                                if ($scope.configurationsettings[0].exmConfig_RollnoColumnDisplay === true) {
                                    $scope.rollno = true;
                                    count = count + 1;
                                } else {
                                    $scope.rollno = false;
                                }

                                if (count === 0) {
                                    $scope.admno = true;
                                    $scope.rollno = true;
                                }
                            } else {
                                $scope.admno = true;
                                $scope.rollno = true;
                            }

                        } else {
                            $scope.admno = true;
                            $scope.rollno = true;
                        }

                        // when sub subject and sub exam not there

                        if (!promise.ecyseS_SubSubjectFlg && !promise.ecyseS_SubExamFlg) {
                            $scope.temp_student_list_S = promise.studentList;
                            $scope.marksdeleteflag = promise.marksdeleteflag;
                            $scope.subject_details = promise.subject_details;
                            $scope.ecyseS_SubSubjectFlg = promise.ecyseS_SubSubjectFlg;
                            $scope.ecyseS_SubExamFlg = promise.ecyseS_SubExamFlg;
                            $scope.ecyseS_MarksGradeEntryFlg = promise.ecyseS_MarksGradeEntryFlg;
                            if (promise.ecyseS_MarksGradeEntryFlg === 'M') {
                                $scope.allowpattern = "[0-9A-Z.]";
                                $scope.placeholder = "Enter Marks...";
                            }
                            else if (promise.ecyseS_MarksGradeEntryFlg === 'G') {
                                $scope.allowpattern = "[A-Z0-9+-]";
                                $scope.placeholder = "Enter Grade...";
                            }
                            var final_subject_array = [];
                            $scope.row_span = 0;
                            $scope.col_span = 0;
                            angular.forEach(promise.subject_details, function (sub_deta) {
                                angular.forEach($scope.subject_list, function (sub_m) {
                                    if (parseInt(sub_deta.ismS_Id) === parseInt(sub_m.ismS_Id)) {
                                        sub_deta.ismS_SubjectName = sub_m.ismS_SubjectName;
                                    }
                                });
                                $scope.row_span += 1;
                                $scope.col_span += 1;
                                if (promise.ecyseS_SubSubjectFlg) {
                                    sub_deta.subsubjects = promise.subject_subsubjects;
                                    $scope.row_span += 1;
                                    $scope.col_span += promise.subject_subsubjects.length;
                                }
                                if (promise.ecyseS_SubExamFlg) {
                                    sub_deta.subexams = promise.subject_subexams;
                                    $scope.row_span += 1;
                                    $scope.col_span += promise.subject_subexams.length;
                                }
                            });

                            if (promise.ecyseS_SubSubjectFlg && promise.ecyseS_SubExamFlg) {
                                $scope.temp_sub_subjs_exams = [];
                                angular.forEach(promise.subject_subsubjects, function (sub_s) {
                                    angular.forEach(promise.subject_subexams, function (sub_e) {
                                        $scope.temp_sub_subjs_exams.push({ sub_subject: sub_s, sub_exam: sub_e });
                                    });
                                });
                            }

                            console.log($scope.col_span);
                            if (promise.ecyseS_SubSubjectFlg) {
                                $scope.subject_subsubjects_details = promise.subject_subsubjects;
                            }
                            if (promise.ecyseS_SubExamFlg) {
                                $scope.subject_subexams_details = promise.subject_subexams;
                            }
                            if (promise.ecyseS_MarksGradeEntryFlg === "G") {
                                $scope.grade_details = promise.grade_details;
                                if (promise.ecyseS_SubSubjectFlg) {
                                    $scope.subject_subsubject_grade_details = promise.subsubject_gradedetails;
                                }
                                if (promise.ecyseS_SubExamFlg) {
                                    $scope.subject_subexam_grade_details = promise.subexam_gradedetails;
                                }
                            }
                            $scope.submitted = false;
                            $scope.myForm.$setPristine();
                            $scope.myForm.$setUntouched();
                            if (promise.saved_studentList !== null && promise.saved_studentList.length > 0) {
                                $scope.map_marks_S(promise.saved_studentList);
                            }
                        }

                        // when sub subject and sub exam is there for subject

                        else if (promise.ecyseS_SubSubjectFlg && promise.ecyseS_SubExamFlg) {

                            $scope.temp_student_list_SSSE = promise.studentList;
                            $scope.marksdeleteflag = promise.marksdeleteflag;
                            $scope.subject_details = promise.subject_details;
                            $scope.ecyseS_SubSubjectFlg = promise.ecyseS_SubSubjectFlg;
                            $scope.ecyseS_SubExamFlg = promise.ecyseS_SubExamFlg;
                            $scope.ecyseS_MarksGradeEntryFlg = promise.ecyseS_MarksGradeEntryFlg;

                            if (promise.ecyseS_MarksGradeEntryFlg === 'M') {
                                $scope.allowpattern = "[0-9A-Z.]";
                                $scope.placeholder = "Enter Marks...";
                            }
                            else if (promise.ecyseS_MarksGradeEntryFlg === 'G') {
                                $scope.allowpattern = "[A-Z0-9+-]";
                                $scope.placeholder = "Enter Grade...";
                            }


                            var final_subject_array = [];

                            $scope.row_span = 0;
                            $scope.col_span = 0;

                            angular.forEach(promise.subject_details, function (sub_deta) {
                                angular.forEach($scope.subject_list, function (sub_m) {
                                    if (parseInt(sub_deta.ismS_Id) === parseInt(sub_m.ismS_Id)) {
                                        sub_deta.ismS_SubjectName = sub_m.ismS_SubjectName;
                                    }
                                });

                                $scope.row_span += 1;

                                if (promise.ecyseS_SubSubjectFlg && promise.ecyseS_SubExamFlg) {
                                    sub_deta.subsubjects = promise.subject_subsubjects;
                                    sub_deta.subexams = promise.subject_subexams;
                                    $scope.row_span += 2;
                                    $scope.col_span += (promise.subsubjectlist.length);
                                }

                                else {
                                    if (promise.ecyseS_SubSubjectFlg) {
                                        sub_deta.subsubjects = promise.subject_subsubjects;
                                        $scope.row_span += 1;
                                        $scope.col_span += promise.subject_subsubjects.length;
                                    }
                                    if (promise.ecyseS_SubExamFlg) {
                                        sub_deta.subexams = promise.subject_subexams;
                                        $scope.row_span += 1;
                                        $scope.col_span += promise.subject_subexams.length;
                                    }
                                }
                            });


                            if (promise.ecyseS_SubSubjectFlg && promise.ecyseS_SubExamFlg) {
                                $scope.temp_sub_subjs_exams = [];

                                angular.forEach(promise.subject_subsubjects, function (sub_s) {
                                    angular.forEach(promise.subject_subexams, function (sub_e) {

                                        if (parseInt(sub_e.emsS_Id) === parseInt(sub_s.emsS_Id)) {
                                            $scope.temp_sub_subjs_exams.push({ sub_subject: sub_s, sub_exam: sub_e });
                                        }
                                    });
                                });
                            }

                            console.log($scope.col_span);
                            $scope.subject_subsubjects_details = [];
                            $scope.temp_sub_subjs_exams = [];

                            if (promise.ecyseS_SubSubjectFlg) {
                                angular.forEach(promise.subsubjectlist, function (ddd) {
                                    $scope.temp = [];

                                    angular.forEach(promise.subject_subsubjects, function (dd) {

                                        if (parseInt(ddd.emsS_Id) === parseInt(dd.emsS_Id)) {
                                            $scope.temp.push(dd);
                                            $scope.temp_sub_subjs_exams.push(dd);
                                        }
                                    });

                                    $scope.subject_subsubjects_details.push({ subsubject: ddd, subexamlist: $scope.temp });

                                });
                                $scope.col_span += $scope.temp_sub_subjs_exams.length;
                                console.log($scope.temp_sub_subjs_exams);
                            }

                            if (promise.ecyseS_SubExamFlg) {
                                $scope.subject_subexams_details = promise.subexamlist;
                            }

                            if (promise.ecyseS_MarksGradeEntryFlg === "G") {
                                $scope.grade_details = promise.grade_details;
                                if (promise.ecyseS_SubSubjectFlg) {
                                    $scope.subject_subsubject_grade_details = promise.subsubject_gradedetails;
                                }
                                if (promise.ecyseS_SubExamFlg) {
                                    $scope.subject_subexam_grade_details = promise.subexam_gradedetails;
                                }
                            }

                            $scope.submitted = false;
                            $scope.myForm.$setPristine();
                            $scope.myForm.$setUntouched();
                            // $scope.gridOptions.data = promise.studentList;
                            $scope.subMorGFlag = promise.subMorGFlag;
                            $scope.gradname = promise.gradname;
                            if (promise.saved_studentList !== null && promise.saved_studentList.length > 0 && promise.saved_ssse_list !== null
                                && promise.saved_ssse_list.length > 0) {
                                $scope.map_marks_SSSE(promise.saved_studentList, promise.saved_ssse_list);
                            }
                        }

                        // When Sub Subject is there not sub exam is not there
                        else if (promise.ecyseS_SubSubjectFlg && promise.subject_subsubjects !== null && promise.subject_subsubjects.length > 0 && !promise.ecyseS_SubExamFlg) {

                            $scope.temp_student_list_SS = promise.studentList;
                            $scope.marksdeleteflag = promise.marksdeleteflag;
                            $scope.subject_details = promise.subject_details;
                            $scope.ecyseS_SubSubjectFlg = promise.ecyseS_SubSubjectFlg;
                            $scope.ecyseS_SubExamFlg = promise.ecyseS_SubExamFlg;
                            $scope.ecyseS_MarksGradeEntryFlg = promise.ecyseS_MarksGradeEntryFlg;
                            if (promise.ecyseS_MarksGradeEntryFlg === 'M') {
                                $scope.ngpattern = /^[0-9]{0,4}\.?[0-9]{1,2}?$/;
                                $scope.allowpattern = "[0-9A-Z.]";
                            }
                            else if (promise.ecyseS_MarksGradeEntryFlg === 'G') {
                                $scope.ngpattern = "";
                                $scope.allowpattern = "[a-zA-Z+-]";
                            }
                            var final_subject_array = [];

                            $scope.row_span = 0;
                            $scope.col_span = 0;
                            angular.forEach(promise.subject_details, function (sub_deta) {
                                angular.forEach($scope.subject_list, function (sub_m) {
                                    if (parseInt(sub_deta.ismS_Id) === parseInt(sub_m.ismS_Id)) {
                                        sub_deta.ismS_SubjectName = sub_m.ismS_SubjectName;
                                    }
                                });
                                $scope.row_span += 1;
                                $scope.col_span += 1;
                                if (promise.ecyseS_SubSubjectFlg) {
                                    sub_deta.subsubjects = promise.subject_subsubjects;
                                    $scope.row_span += 1;
                                    $scope.col_span += promise.subject_subsubjects.length;
                                }
                            });

                            console.log($scope.col_span);
                            if (promise.ecyseS_SubSubjectFlg) {
                                $scope.subject_subsubjects_details = promise.subject_subsubjects;
                                console.log("******************");
                                console.log($scope.subject_subsubjects_details);
                            }
                            if (promise.ecyseS_MarksGradeEntryFlg === "G") {
                                $scope.grade_details = promise.grade_details;
                                if (promise.ecyseS_SubSubjectFlg) {
                                    $scope.subject_subsubject_grade_details = promise.subsubject_gradedetails;
                                }
                            }
                            $scope.submitted = false;
                            $scope.myForm.$setPristine();
                            $scope.myForm.$setUntouched();
                            $scope.subMorGFlag = promise.subMorGFlag;
                            $scope.gradname = promise.gradname;
                            if (promise.saved_studentList !== null && promise.saved_studentList.length > 0 && promise.saved_ss_list !== null && promise.saved_ss_list.length > 0) {
                                $scope.map_marks_SS(promise.saved_studentList, promise.saved_ss_list);
                            }
                        }

                        // When Sub Exam Is there Not Sub Subject 
                        else if (promise.ecyseS_SubExamFlg && promise.subject_subexams != null && promise.subject_subexams.length > 0 && !promise.ecyseS_SubSubjectFlg) {
                            $scope.temp_student_list_SE = promise.studentList;
                            $scope.marksdeleteflag = promise.marksdeleteflag;
                            $scope.subject_details = promise.subject_details;
                            $scope.ecyseS_SubSubjectFlg = promise.ecyseS_SubSubjectFlg;
                            $scope.ecyseS_SubExamFlg = promise.ecyseS_SubExamFlg;
                            $scope.ecyseS_MarksGradeEntryFlg = promise.ecyseS_MarksGradeEntryFlg;

                            var final_subject_array = [];
                            $scope.row_span = 0;
                            $scope.col_span = 0;
                            angular.forEach(promise.subject_details, function (sub_deta) {
                                angular.forEach($scope.subject_list, function (sub_m) {
                                    if (parseInt(sub_deta.ismS_Id) === parseInt(sub_m.ismS_Id)) {
                                        sub_deta.ismS_SubjectName = sub_m.ismS_SubjectName;
                                    }
                                });
                                $scope.row_span += 1;
                                $scope.col_span += 1;
                                if (promise.ecyseS_SubExamFlg) {
                                    sub_deta.subexams = promise.subject_subexams;
                                    $scope.row_span += 1;
                                    $scope.col_span += promise.subject_subexams.length;
                                }
                            });
                            console.log($scope.col_span);
                            if (promise.ecyseS_SubExamFlg) {
                                $scope.subject_subexams_details = promise.subject_subexams;
                            }
                            if (promise.ecyseS_MarksGradeEntryFlg === "G") {
                                $scope.grade_details = promise.grade_details;
                                if (promise.ecyseS_SubExamFlg) {
                                    $scope.subject_subexam_grade_details = promise.subexam_gradedetails;
                                }
                            }
                            $scope.submitted = false;
                            $scope.myForm.$setPristine();
                            $scope.myForm.$setUntouched();
                            $scope.subMorGFlag = promise.subMorGFlag;
                            $scope.gradname = promise.gradname;
                            if (promise.saved_studentList !== null && promise.saved_studentList.length > 0 && promise.saved_se_list !== null && promise.saved_se_list.length > 0) {
                                $scope.map_marks_SE(promise.saved_studentList, promise.saved_se_list);
                            }
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
            }
        };


        // Mapping Marks  Subject Wise 

        $scope.map_marks_S = function (saved_studentList) {
            angular.forEach(saved_studentList, function (stu_s) {
                angular.forEach($scope.temp_student_list_S, function (stu_m) {
                    if (parseInt(stu_m.amcsT_Id) === parseInt(stu_s.amcsT_Id)) {
                        stu_m.selected_s = true;
                        $scope.optionToggled_S();

                        angular.forEach($scope.subject_details, function (sub) {
                            if (parseInt(stu_m.ismS_Id) === parseInt(stu_s.ismS_Id) && parseInt(stu_m.ismS_Id) === parseInt(sub.ismS_Id)) {
                                if ($scope.ecyseS_MarksGradeEntryFlg === 'M') {
                                    if (stu_s.ecstM_Flg === '' || stu_s.ecstM_Flg === null) {
                                        stu_m.ECSTM_Marks = stu_s.ecstM_Marks.toString();
                                    }
                                    else {
                                        stu_m.ECSTM_Marks = stu_s.ecstM_Flg.toString();
                                    }
                                }
                                else if ($scope.ecyseS_MarksGradeEntryFlg === 'G') {
                                    if (stu_s.ecstM_Flg === '' || stu_s.ecstM_Flg === null) {
                                        stu_m.ECSTM_Marks = stu_s.ecstM_Grade.toString();
                                    }
                                    else {
                                        stu_m.ECSTM_Marks = stu_s.ecstM_Flg.toString();
                                    }
                                }
                                $scope.valid_marks_S(sub, sub.ismS_Id, stu_m.ECSTM_Marks, stu_m.amcsT_Id, stu_m);
                            }
                        });
                    }
                });
            });
        };

        // Mapping Marks Subsubject and subexam wise
        $scope.map_marks_SSSE = function (saved_studentList, saved_ssse_list) {
            angular.forEach(saved_studentList, function (stu_s) {
                angular.forEach($scope.temp_student_list_SSSE, function (stu_m) {
                    if (parseInt(stu_m.amcsT_Id) === parseInt(stu_s.amcsT_Id)) {
                        stu_m.selected_ssse = true;
                        $scope.optionToggled_SSSE();
                        stu_m.ECSTMSS_Marks = [];
                        angular.forEach($scope.temp_sub_subjs_exams, function (y) {
                            angular.forEach($scope.subject_subsubjects_details, function (ss) {
                                if (parseInt(ss.subsubject.emsS_Id) === parseInt(y.emsS_Id)) {
                                    if (stu_m.ECSTMSS_Marks[ss.subsubject.emsS_Id] === undefined) {
                                        stu_m.ECSTMSS_Marks[ss.subsubject.emsS_Id] = [];
                                    }
                                    angular.forEach($scope.subject_subexams_details, function (se) {
                                        if (parseInt(se.emsE_Id) === parseInt(y.emsE_Id)) {
                                            stu_m.ECSTMSS_Marks[ss.subsubject.emsS_Id][se.emsE_Id] = [];
                                            angular.forEach(saved_ssse_list, function (ssse_s) {
                                                if (parseInt(ssse_s.emsS_Id) === parseInt(y.emsS_Id) && parseInt(ssse_s.emsE_Id) === parseInt(y.emsE_Id)) {
                                                    if (parseInt(ssse_s.ecstM_Id) === parseInt(stu_s.ecstM_Id)) {
                                                        if (ssse_s.ecstmsS_Flg !== null && (ssse_s.ecstmsS_Marks === null || ssse_s.ecstmsS_Marks === 0.00)) {
                                                            stu_m.ECSTMSS_Marks[ssse_s.emsS_Id][ssse_s.emsE_Id][stu_m.amcsT_Id] = ssse_s.ecstmsS_Flg.toString();
                                                        }
                                                        else if (ssse_s.ecstmsS_Flg === null && ssse_s.ecstmsS_Marks !== null) {
                                                            stu_m.ECSTMSS_Marks[ssse_s.emsS_Id][ssse_s.emsE_Id][stu_m.amcsT_Id] = ssse_s.ecstmsS_Marks.toString();
                                                        }
                                                        $scope.valid_marks_SSSE(y, y.emsS_Id, y.emsE_Id, stu_m.ECSTMSS_Marks, stu_m.amcsT_Id, stu_m);
                                                        //  }
                                                    }
                                                }
                                            });
                                        }
                                    });
                                }
                            });

                        });

                    }
                });
            });
        };

        // Mapping Marks only Subsubject
        $scope.map_marks_SS = function (saved_studentList, saved_ss_list) {
            angular.forEach(saved_studentList, function (stu_s) {
                angular.forEach($scope.temp_student_list_SS, function (stu_m) {
                    if (parseInt(stu_m.amcsT_Id) === parseInt(stu_s.amcsT_Id)) {
                        stu_m.selected_ss = true;
                        $scope.optionToggled_SS();
                        stu_m.ECSTMSS_Marks = [];
                        //  angular.forEach($scope.temp_sub_subjs_exams, function (y) {
                        angular.forEach($scope.subject_subsubjects_details, function (ss) {
                            //  if(ss.emsS_Id==y.sub_subject.emsS_Id)
                            //  {
                            if (stu_m.ECSTMSS_Marks[ss.emsS_Id] === undefined) {
                                stu_m.ECSTMSS_Marks[ss.emsS_Id] = [];
                            }
                            angular.forEach(saved_ss_list, function (ss_s) {
                                if (parseInt(ss_s.emsS_Id) === parseInt(ss.emsS_Id)) {
                                    if (parseInt(ss_s.ecstM_Id) === parseInt(stu_s.ecstM_Id)) {
                                        if (ss_s.ecstmsS_Flg !== null && ss_s.ecstmsS_Marks === null) {
                                            stu_m.ECSTMSS_Marks[ss_s.emsS_Id][stu_m.amcsT_Id] = ss_s.estmsS_Flg.toString();
                                        }
                                        else if (ss_s.ecstmsS_Flg === null && ss_s.ecstmsS_Marks !== null) {
                                            stu_m.ECSTMSS_Marks[ss_s.emsS_Id][stu_m.amcsT_Id] = ss_s.ecstmsS_Marks.toString();
                                        }
                                        $scope.valid_marks_SS(ss, ss.emsS_Id, stu_m.ECSTMSS_Marks, stu_m.amcsT_Id, stu_m);
                                    }
                                }
                            });
                        });
                    }
                });
            });
        };

        // Mapping Marks Only Subexam
        $scope.map_marks_SE = function (saved_studentList, saved_se_list) {
            angular.forEach(saved_studentList, function (stu_s) {
                angular.forEach($scope.temp_student_list_SE, function (stu_m) {
                    if (parseInt(stu_m.amcsT_Id) === parseInt(stu_s.amcsT_Id)) {
                        stu_m.selected_se = true;
                        $scope.optionToggled_SE();
                        stu_m.ECSTMSS_Marks = [];

                        angular.forEach($scope.subject_subexams_details, function (se) {
                            if (stu_m.ECSTMSS_Marks[se.emsE_Id] === undefined) {
                                stu_m.ECSTMSS_Marks[se.emsE_Id] = [];
                            }
                            angular.forEach(saved_se_list, function (se_s) {
                                if (se_s.emsE_Id === se.emsE_Id) {
                                    if (parseInt(se_s.ecstM_Id) === parseInt(stu_s.ecstM_Id)) {
                                        if (se_s.ecstmsS_Flg !== null && se_s.ecstmsS_Marks === null) {
                                            stu_m.ECSTMSS_Marks[se_s.emsE_Id][stu_m.amcsT_Id] = se_s.estmsS_Flg.toString();
                                        }
                                        else if (se_s.ecstmsS_Flg === null && se_s.ecstmsS_Marks !== null) {
                                            stu_m.ECSTMSS_Marks[se_s.emsE_Id][stu_m.amcsT_Id] = se_s.ecstmsS_Marks.toString();
                                        }
                                        $scope.valid_marks_SE(se, se.emsE_Id, stu_m.ECSTMSS_Marks, stu_m.amcsT_Id, stu_m);
                                    }
                                }

                            });
                        });
                    }
                });
            });
        };


        // Validating the marks Subject Wise

        var Temp_subs_marks_list = [];

        $scope.valid_marks_S = function (obj_s, subj_id, values, stu_id, obj_student) {//, totalMarks, obtainmarks, row
            var al_cnt = 0;
            var obtainmarks = 0;
            var flag = "false";
            if ($scope.ecyseS_MarksGradeEntryFlg === "G") {
                flag = "false";
                obtainmarks = values;
                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {

                    //if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                    if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                        flag = "true";
                        if (Temp_subs_marks_list.length > 0) {
                            al_cnt = 0;
                            angular.forEach(Temp_subs_marks_list, function (yet) {
                                if (parseInt(yet.AMCST_Id) === parseInt(stu_id) && parseInt(yet.ISMS_Id) === parseInt(subj_id)) {
                                    yet.ECSTM_Flg = obtainmarks;
                                    // yet.ESTM_Marks = 0;
                                    yet.ECSTM_Grade = null;
                                    al_cnt += 1;
                                }
                            });
                            if (al_cnt === 0) {
                                Temp_subs_marks_list.push({
                                    AMCST_Id: stu_id, ISMS_Id: subj_id, ECSTM_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg,
                                    ECSTM_Flg: obtainmarks, ECSTM_Marks: 0, ECSTM_Grade: null
                                });//ESTMSS_Marks: null,ESTMSS_Grade: null,
                            }
                        }
                        else if (Temp_subs_marks_list.length === 0) {
                            Temp_subs_marks_list.push({
                                AMCST_Id: stu_id, ISMS_Id: subj_id, ECSTM_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg,
                                ECSTM_Flg: obtainmarks, ECSTM_Marks: 0, ECSTM_Grade: null
                            });//ESTMSS_Marks: null,ESTMSS_Grade: null,
                        }

                    }
                    if (flag === "false") {
                        for (var i = 0; i < $scope.grade_details.length; i++) {
                            // if ($scope.grade_details[i] == obtainmarks) {
                            if ($scope.grade_details[i].toUpperCase() === obtainmarks.toUpperCase()) {
                                flag = "true";
                                if (Temp_subs_marks_list.length > 0) {
                                    al_cnt = 0;
                                    angular.forEach(Temp_subs_marks_list, function (yet) {
                                        if (parseInt(yet.AMCST_Id) === parseInt(stu_id) && parseInt(yet.ISMS_Id) === parseInt(subj_id)) {
                                            yet.ECSTM_Grade = obtainmarks;
                                            yet.ECSTM_Flg = "";

                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        Temp_subs_marks_list.push({
                                            AMCST_Id: stu_id, ISMS_Id: subj_id, ECSTM_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg,
                                            ECSTM_Marks: 0, ECSTM_Grade: obtainmarks, ECSTM_Flg: ""
                                        });//, ESTMSS_Grade: null, ESTMSS_Flg: null
                                    }
                                }
                                else if (Temp_subs_marks_list.length === 0) {
                                    Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: subj_id, ECSTM_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTM_Marks: 0, ECSTM_Grade: obtainmarks, ECSTM_Flg: "" });//, ESTMSS_Grade: null, ESTMSS_Flg: null
                                }
                            }
                        }
                    }
                    if (flag === "false") {
                        obj_student.ESTM_Marks = "";
                        swal('Entered Grade cant be out of master setting...!');
                    }
                }
            }
            else {
                flag = "false";
                obtainmarks = values;
                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                    if (obtainmarks.match(/[a-z]/i)) {
                        //if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                        if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                            flag = "true";
                            if (Temp_subs_marks_list.length > 0) {
                                al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (parseInt(yet.AMCST_Id) === parseInt(stu_id) && parseInt(yet.ISMS_Id) === parseInt(subj_id)) {
                                        yet.ECSTM_Flg = obtainmarks;
                                        yet.ECSTM_Marks = 0;
                                        al_cnt += 1;
                                    }
                                });

                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({
                                        AMCST_Id: stu_id, ISMS_Id: subj_id, ECSTM_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg,
                                        ECSTM_Flg: obtainmarks, ECSTM_Marks: 0, ECSTM_Grade: null
                                    });//ESTMSS_Marks: null,ESTMSS_Grade: null,
                                }
                            }
                            else if (Temp_subs_marks_list.length === 0) {
                                Temp_subs_marks_list.push({
                                    AMCST_Id: stu_id, ISMS_Id: subj_id, ECSTM_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg,
                                    ECSTM_Flg: obtainmarks, ECSTM_Marks: 0, ECSTM_Grade: null
                                });//ESTMSS_Marks: null,ESTMSS_Grade: null,
                            }

                        }
                        if (flag === "false") {
                            // values = "";
                            obj_student.ECSTM_Marks = "";
                            swal('Entered value cant be out of master setting...!');
                        }
                    }
                    else {
                        var totalMarks = 0;
                        if (Number(obj_student.ecyseS_MaxMarks) > Number(obj_student.ecyseS_MarksEntryMax)) {
                            totalMarks = Number(obj_student.ecyseS_MarksEntryMax);
                        }
                        else {
                            totalMarks = Number(obj_student.ecyseS_MaxMarks);
                        }

                        obtainmarks = Number(obtainmarks);
                        if (totalMarks < obtainmarks) {

                            obj_student.ECSTM_Marks = "";
                            swal('Entered marks cant be more than Max Marks ...!' + totalMarks);
                        }
                        else if (obtainmarks < 0) {
                            obj_student.ECSTM_Marks = "";
                            swal('Entered marks cant be in nagative values...!');
                        }
                        else {
                            if (Temp_subs_marks_list.length > 0) {
                                al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (parseInt(yet.AMCST_Id) === parseInt(stu_id) && parseInt(yet.ISMS_Id) === parseInt(subj_id)) {
                                        yet.ECSTM_Marks = obtainmarks;
                                        yet.ECSTM_Flg = "";
                                        al_cnt += 1;
                                    }

                                });
                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: subj_id, ECSTM_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTM_Marks: obtainmarks, ECSTM_Grade: null, ECSTM_Flg: "" });//, ESTMSS_Grade: null, ESTMSS_Flg: null
                                }
                            }
                            else if (Temp_subs_marks_list.length === 0) {
                                Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: subj_id, ECSTM_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTM_Marks: obtainmarks, ECSTM_Grade: null, ECSTM_Flg: "" });//, ESTMSS_Grade: null, ESTMSS_Flg: null
                            }
                        }
                    }
                }
            }
        };

        // validating the marks subsubject and subexam
        $scope.valid_marks_SSSE = function (obj_ssse, s_subj_id, s_exm_id, values, stu_id, obj_student) {//, totalMarks, obtainmarks, row
            var flag = "false";
            var al_cnt = 0;
            if ($scope.ecyseS_MarksGradeEntryFlg === "G") {
                for (var i = 0; i < $scope.gradname.length; i++) {
                    if ($scope.gradname[i].toUpperCase() === obtainmarks.toUpperCase()) {
                        flag = "true";
                    }
                }

                //if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                    flag = "true";
                }

                if (flag === "false") {
                    row.entity.obtainmarks = 0;
                    swal('Entered Grade cant be out of master setting...!');
                }
            }
            else {
                flag = "false";
                var obtainmarks = values[s_subj_id][s_exm_id][stu_id];
                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                    if (obtainmarks.match(/[A-Z]/i)) {
                        //if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                        if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                            flag = "true";
                            if (Temp_subs_marks_list.length > 0) {
                                al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (parseInt(yet.AMCST_Id) === parseInt(stu_id) && parseInt(yet.ISMS_Id) === parseInt(obj_student.ismS_Id)
                                        && parseInt(yet.EMSS_Id) === parseInt(s_subj_id) && parseInt(yet.EMSE_Id) === parseInt(s_exm_id)) {
                                        yet.ECSTMSS_Flg = obtainmarks;
                                        yet.ECSTMSS_Marks = null;
                                        al_cnt += 1;
                                    }

                                });
                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({
                                        AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id,
                                        ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Flg: obtainmarks, ECSTMSS_Marks: null, ECSTMSS_Grade: null
                                    });//ESTMSS_Marks: null,ESTMSS_Grade: null,
                                }
                            }
                            else if (Temp_subs_marks_list.length === 0) {
                                Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id, ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Flg: obtainmarks, ECSTMSS_Marks: null, ECSTMSS_Grade: null });//ESTMSS_Marks: null,ESTMSS_Grade: null,
                            }

                        }
                        if (flag === "false") {
                            values[s_subj_id][s_exm_id][stu_id] = "";
                            swal('Entered value cant be out of master setting...!');
                        }
                    }
                    else {
                        var totalMarks = 0;
                        totalMarks = Number(obj_ssse.ecysessS_MaxMarks);
                        obtainmarks = Number(obtainmarks);
                        if (totalMarks < obtainmarks) {
                            values[s_subj_id][s_exm_id][stu_id] = "";
                            swal('Entered marks cant be more than Max Marks ...!' + totalMarks);
                        }
                        else if (obtainmarks < 0) {
                            values[s_subj_id][s_exm_id][stu_id] = "";
                            swal('Entered marks cant be in nagative values...!');
                        }
                        else {
                            if (Temp_subs_marks_list.length > 0) {
                                al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (parseInt(yet.AMCST_Id) === parseInt(stu_id) && parseInt(yet.ISMS_Id) === parseInt(obj_student.ismS_Id)
                                        && parseInt(yet.EMSS_Id) === parseInt(s_subj_id) && parseInt(yet.EMSE_Id) === parseInt(s_exm_id)) {
                                        yet.ECSTMSS_Marks = obtainmarks;
                                        yet.ECSTMSS_Flg = null;
                                        al_cnt += 1;
                                    }

                                });
                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({
                                        AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id,
                                        ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Marks: obtainmarks, ECSTMSS_Grade: null, ECSTMSS_Flg: null
                                    });//, ESTMSS_Grade: null, ESTMSS_Flg: null
                                }
                            }
                            else if (Temp_subs_marks_list.length === 0) {
                                Temp_subs_marks_list.push({
                                    AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id,
                                    ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Marks: obtainmarks, ECSTMSS_Grade: null, ECSTMSS_Flg: null
                                });//, ESTMSS_Grade: null, ESTMSS_Flg: null
                            }

                        }
                    }
                }
            }
        };

        // Validating the marks subsubject only
        $scope.valid_marks_SS = function (obj_ss, s_subj_id, values, stu_id, obj_student) {//, totalMarks, obtainmarks, row
            var flag = "false";
            var al_cnt = 0;
            if ($scope.ecyseS_MarksGradeEntryFlg === "G") {

                flag = "false";

                for (var i = 0; i < $scope.gradname.length; i++) {
                    //if ($scope.gradname[i] == obtainmarks) {
                    if ($scope.gradname[i].toUpperCase() === obtainmarks.toUpperCase()) {
                        flag = "true";
                    }
                }

                //if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                    flag = "true";
                }

                if (flag === "false") {
                    row.entity.obtainmarks = 0;
                    swal('Entered Grade cant be out of master setting...!');
                }
            }
            else {

                var obtainmarks = values[s_subj_id][stu_id];
                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                    if (obtainmarks.match(/[A-Z]/i)) {
                        //if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                        if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                            flag = "true";
                            if (Temp_subs_marks_list.length > 0) {
                                al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (parseInt(yet.AMCST_Id) === parseInt(stu_id) && parseInt(yet.ISMS_Id) === parseInt(obj_student.ismS_Id)
                                        && parseInt(yet.EMSS_Id) === parseInt(s_subj_id))//&&  yet.EMSE_Id == s_exm_id
                                    {
                                        yet.ECSTMSS_Flg = obtainmarks;
                                        yet.ECSTMSS_Marks = null;
                                        al_cnt += 1;
                                    }
                                });

                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Flg: obtainmarks, ECSTMSS_Marks: null, ECSTMSS_Grade: null });//ESTMSS_Marks: null,ESTMSS_Grade: null, EMSE_Id: s_exm_id,
                                }
                            }
                            else if (Temp_subs_marks_list.length === 0) {
                                Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Flg: obtainmarks, ECSTMSS_Marks: null, ECSTMSS_Grade: null });//ESTMSS_Marks: null,ESTMSS_Grade: null,EMSE_Id: s_exm_id,
                            }

                        }
                        if (flag === "false") {
                            values[s_subj_id][stu_id] = "";
                            swal('Entered value cant be out of master setting...!');
                        }
                    }
                    else {
                        var totalMarks = 0;
                        totalMarks = Number(obj_ss.ecysessS_MaxMarks);
                        obtainmarks = Number(obtainmarks);
                        if (totalMarks < obtainmarks) {
                            values[s_subj_id][stu_id] = "";
                            swal('Entered marks cant be more than Max Marks ...!' + totalMarks);
                        }
                        else if (obtainmarks < 0) {
                            values[s_subj_id][stu_id] = "";
                            swal('Entered marks cant be in nagative values...!');
                        }
                        else {
                            if (Temp_subs_marks_list.length > 0) {
                                al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (parseInt(yet.AMCST_Id) === parseInt(stu_id) && parseInt(yet.ISMS_Id) === parseInt(obj_student.ismS_Id)
                                        && parseInt(yet.EMSS_Id) === parseInt(s_subj_id)) {//&& yet.EMSE_Id == s_exm_id
                                        yet.ECSTMSS_Marks = obtainmarks;
                                        yet.ECSTMSS_Flg = null;
                                        al_cnt += 1;
                                    }
                                });

                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Marks: obtainmarks, ECSTMSS_Grade: null, ECSTMSS_Flg: null });//,EMSE_Id: s_exm_id, ESTMSS_Grade: null, ESTMSS_Flg: null
                                }
                            }
                            else if (Temp_subs_marks_list.length === 0) {
                                Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Marks: obtainmarks, ECSTMSS_Grade: null, ECSTMSS_Flg: null });//,EMSE_Id: s_exm_id, ESTMSS_Grade: null, ESTMSS_Flg: null
                            }
                        }
                    }
                }
            }
        };

        // Validating The Marks Subexam only

        $scope.valid_marks_SE = function (obj_se, s_exm_id, values, stu_id, obj_student) {//, totalMarks, obtainmarks, row
            var flag = "false";
            var al_cnt = 0;
            if ($scope.ecyseS_MarksGradeEntryFlg === "G") {

                for (var i = 0; i < $scope.gradname.length; i++) {
                    //if ($scope.gradname[i] == obtainmarks) {
                    if ($scope.gradname[i].toUpperCase() === obtainmarks.toUpperCase()) {
                        flag = "true";
                    }
                }

                //if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                    flag = "true";
                }
                if (flag === "false") {
                    row.entity.obtainmarks = 0;
                    swal('Entered Grade cant be out of master setting...!');
                }
            }
            else {
                flag = "false";
                var obtainmarks = values[s_exm_id][stu_id];
                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                    if (obtainmarks.match(/[A-Z]/i)) {
                        //if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                        if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                            flag = "true";
                            if (Temp_subs_marks_list.length > 0) {
                                al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (parseInt(yet.AMCST_Id) === parseInt(stu_id) && parseInt(yet.ISMS_Id) === parseInt(obj_student.ismS_Id)
                                        && parseInt(yet.EMSE_Id) === parseInt(s_exm_id)) {
                                        yet.ECSTMSS_Flg = obtainmarks;
                                        yet.ECSTMSS_Marks = null;
                                        al_cnt += 1;
                                    }
                                });

                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSE_Id: s_exm_id, ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Flg: obtainmarks, ECSTMSS_Marks: null, ECSTMSS_Grade: null });//ESTMSS_Marks: null,ESTMSS_Grade: null, EMSS_Id: s_subj_id,
                                }
                            }
                            else if (Temp_subs_marks_list.length === 0) {
                                Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSE_Id: s_exm_id, ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Flg: obtainmarks, ECSTMSS_Marks: null, ECSTMSS_Grade: null });//ESTMSS_Marks: null,ESTMSS_Grade: null,EMSS_Id: s_subj_id,
                            }

                        }
                        if (flag === "false") {
                            values[s_exm_id][stu_id] = "";
                            swal('Entered value cant be out of master setting...!');
                        }
                    }
                    else {
                        var totalMarks = 0;

                        totalMarks = obj_se.ecysessS_MaxMarks;
                        obtainmarks = Number(obtainmarks);
                        if (totalMarks < obtainmarks) {
                            values[s_exm_id][stu_id] = "";
                            swal('Entered marks cant be more than Max Marks ...!' + totalMarks);
                        }
                        else if (obtainmarks < 0) {
                            values[s_exm_id][stu_id] = "";
                            swal('Entered marks cant be in nagative values...!');
                        }
                        else {
                            if (Temp_subs_marks_list.length > 0) {
                                al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (parseInt(yet.AMCST_Id) === parseInt(stu_id) && parseInt(yet.ISMS_Id) === parseInt(obj_student.ismS_Id)
                                        && parseInt(yet.EMSE_Id) === parseInt(s_exm_id)) {//&&yet.EMSS_Id == s_subj_id 
                                        yet.ECSTMSS_Marks = obtainmarks;
                                        yet.ECSTMSS_Flg = null;
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMCST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSE_Id: s_exm_id, ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Marks: obtainmarks, ESTMSS_Grade: null, ECSTMSS_Flg: null });//,EMSS_Id: s_subj_id,EMSE_Id: s_exm_id, ESTMSS_Grade: null, ECSTMSS_Flg: null
                                }
                            }
                            else if (Temp_subs_marks_list.length === 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSE_Id: s_exm_id, ECSTMSS_MarksGradeFlg: $scope.ecyseS_MarksGradeEntryFlg, ECSTMSS_Marks: obtainmarks, ESTMSS_Grade: null, ECSTMSS_Flg: null });//,EMSS_Id: s_subj_id, ESTMSS_Grade: null, ECSTMSS_Flg: null
                            }
                        }
                    }
                }
            }
        };



        // Saving Subject Wise Marks
        $scope.SaveMarks_S = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var flag = "false";

                if ($scope.marksdeleteflag) {
                    swal({
                        title: "Are You Sure?",
                        text: "Marks Already Calculated, If You Update The Marks You Need To Recalculate Marks Again..Do You Want To Continue ..!?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update It!",
                        cancelButtonText: "Cancel..!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.save_S(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                            }
                            else {
                                flag = "false";

                            }

                        });
                }
                else {
                    $scope.save_S(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                }
            }
            else {
                swal('Please select required field / Entered Marks are not in correct format....!');
            }
        };

        $scope.save_S = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            var main_save_list = [];
            if ($scope.myForm.$valid) {

                angular.forEach($scope.temp_student_list_S, function (stu) {
                    if (stu.selected_s) {
                        var ECSTM_Marks = 0;
                        var ECSTM_Flg = "";
                        var ECSTM_Grade = "";
                        angular.forEach(Temp_subs_marks_list, function (stu_t) {
                            if (parseInt(stu_t.AMCST_Id) === parseInt(stu.amcsT_Id) && parseInt(stu_t.ISMS_Id) === parseInt(stu.ismS_Id)) {
                                ECSTM_Marks = stu_t.ECSTM_Marks;
                                ECSTM_Flg = stu_t.ECSTM_Flg;
                                ECSTM_Grade = stu_t.ECSTM_Grade;
                            }
                        });
                        main_save_list.push({
                            AMCST_Id: stu.amcsT_Id,
                            AMCST_FirstName: stu.amcsT_FirstName,
                            AMCST_AdmNo: stu.amcsT_AdmNo,
                            ACYST_RollNo: stu.acysT_RollNo,
                            ISMS_SubjectName: stu.ismS_SubjectName,
                            ISMS_Id: stu.ismS_Id,
                            ECYSES_MaxMarks: stu.ecyseS_MaxMarks,
                            ECYSES_MarksEntryMax: stu.ecyseS_MarksEntryMax,
                            EYCES_MinMarks: stu.eyceS_MinMarks,
                            ECSTM_Marks: ECSTM_Marks,
                            ECSTM_Flg: ECSTM_Flg,
                            ECSTM_Grade: ECSTM_Grade
                        });
                    }
                });
                if (main_save_list.length > 0) {
                    var data = {
                        main_save_list: main_save_list,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        "EME_Id": $scope.EME_Id,
                        "ISMS_Id": $scope.ISMS_ID,
                        "ACMS_Id": $scope.ACMS_Id,
                        "ECYSES_MarksGradeEntryFlg": $scope.ecyseS_MarksGradeEntryFlg,
                        "ECYSES_SubSubjectFlg": $scope.ecyseS_SubSubjectFlg,
                        "ECYSES_SubExamFlg": $scope.ecyseS_SubExamFlg
                    };

                    apiService.create("CollegeMarksEntry/SaveMarks", data).
                        then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Data Saved Successfully');
                            }
                            else if (promise.returnval === false) {
                                swal('Failed to Save/Update Data');
                            }
                            $scope.clear();
                        });
                }
                else if (main_save_list.length === 0) {
                    swal("Select Students For Saving Marks...");
                }
            }
        };


        //for based on sub  subject sub exams  Max.Marks
        $scope.SaveMarks_SSSE = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var flag = "false";

                if ($scope.marksdeleteflag) {
                    swal({
                        title: "Are You Sure?",
                        text: "Marks Already Calculated, If You Update The Marks You Need To Recalculate Marks Again..Do You Want To Continue ..!?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update It!",
                        cancelButtonText: "Cancel..!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.save_SSSE(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                            }
                            else {
                                flag = "false";

                            }

                        });
                }
                else {
                    $scope.save_SSSE(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                }
            }
            else {
                swal('Please select required field / Entered Marks are not in correct format....!');
            }
        };

        $scope.save_SSSE = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            var main_save_list = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.temp_student_list_SSSE, function (stu) {
                    if (stu.selected_ssse) {
                        var marks = 0;
                        var marksflag = "";
                        angular.forEach(Temp_subs_marks_list, function (ddddd) {
                            if (parseInt(ddddd.AMCST_Id) === parseInt(stu.amcsT_Id)) {
                                var marksflagnew = ddddd.ECSTMSS_Flg;
                                if (marksflagnew !== undefined && marksflagnew !== null && marksflagnew !== "") {
                                    if (marksflagnew.match(/[A-Za-z]/i)) {
                                        marksflag = ddddd.ECSTMSS_Flg;
                                        ddddd.ECSTMSS_Marks = 0;
                                    } else {
                                        // marks += ddddd.ECSTMSS_Marks;
                                    }
                                }
                                marks += ddddd.ECSTMSS_Marks;
                            }
                        });

                        stu.ecstM_Marks = marks;
                        stu.ecstM_Flg = marksflag;


                        main_save_list.push({
                            AMCST_Id: stu.amcsT_Id,
                            AMCST_FirstName: stu.amcsT_FirstName,
                            AMCST_AdmNo: stu.amcsT_AdmNo,
                            ACYST_RollNo: stu.acysT_RollNo,
                            ISMS_SubjectName: stu.ismS_SubjectName,
                            ISMS_Id: stu.ismS_Id,
                            ECYSES_MaxMarks: stu.ecyseS_MaxMarks,
                            ECYSES_MarksEntryMax: stu.ecyseS_MarksEntryMax,
                            EYCES_MinMarks: stu.eyceS_MinMarks,
                            ECSTM_Marks: stu.ecstM_Marks,
                            ECSTM_Flg: stu.ecstM_Flg
                        });
                    }
                });

                if (main_save_list.length > 0) {
                    //  $scope.savedata = $scope.gridOptions.data;
                    var data = {
                        main_save_list: main_save_list,
                        Temp_subs_marks_list: Temp_subs_marks_list,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        "EME_Id": $scope.EME_Id,
                        "ISMS_Id": $scope.ISMS_ID,
                        "ACMS_Id": $scope.ACMS_Id,
                        "ECYSES_MarksGradeEntryFlg": $scope.ecyseS_MarksGradeEntryFlg,
                        "ECYSES_SubSubjectFlg": $scope.ecyseS_SubSubjectFlg,
                        "ECYSES_SubExamFlg": $scope.ecyseS_SubExamFlg
                    };

                    apiService.create("CollegeMarksEntry/SaveMarks", data).
                        then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Data Saved Successfully');
                            }
                            else if (promise.returnval === false) {
                                swal('Failed to Save/Update Data');
                            }
                            $scope.clear();
                        });
                }
                else if (main_save_list.length === 0) {
                    swal("Select Students For Saving Marks...");
                }
            }
        };

        // For based  on sub subject marks 

        $scope.SaveMarks_SS = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var flag = "false";

                if ($scope.marksdeleteflag) {
                    swal({
                        title: "Are You Sure?",
                        text: "Marks Already Calculated, If You Update The Marks You Need To Recalculate Marks Again..Do You Want To Continue ..!?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update It!",
                        cancelButtonText: "Cancel..!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.save_SS(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                            }
                            else {
                                flag = "false";

                            }

                        });
                }
                else {
                    $scope.save_SS(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                }


            }
            else {
                swal('Please select required field / Entered Marks are not in correct format....!');
            }
        };

        $scope.save_SS = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            var main_save_list = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.temp_student_list_SS, function (stu) {
                    if (stu.selected_ss) {
                        if ($scope.ecyseS_SubSubjectFlg) {
                            var others_cnt = 0;
                            var ECSTM_Flg = "";
                            var subject_max_marks = stu.ecyseS_MaxMarks;
                            var subject_marks_entry_for = stu.ecyseS_MarksEntryMax;
                            var sum_s_sub_max_marks = 0;
                            var sum_s_sub_obt_marks = 0;
                            angular.forEach($scope.subject_subsubjects_details, function (s_ss) {
                                sum_s_sub_max_marks += s_ss.eycessS_MaxMarks;
                                if (stu.ECSTMSS_Marks[s_ss.emsS_Id][stu.amcsT_Id].match(/[A-Z]/i)) {
                                    others_cnt += 1;
                                    ECSTM_Flg = stu.ECSTMSS_Marks[s_ss.emsS_Id][stu.amcsT_Id];
                                }
                                else {
                                    sum_s_sub_obt_marks += Number(stu.ECSTMSS_Marks[s_ss.emsS_Id][stu.amcsT_Id]);
                                }
                            });

                            if (stu.ecyseS_MaxMarks == stu.ecyseS_MarksEntryMax) {
                                var ratio_s_sub = (sum_s_sub_max_marks / subject_max_marks);
                                //var ESTM_Marks = sum_s_sub_obt_marks / ratio_s_sub;
                                var ECSTM_Marks = sum_s_sub_obt_marks;
                                stu.ecstM_Marks = ECSTM_Marks;
                                stu.ECSTM_Flg = ECSTM_Flg;
                            }
                            else if (stu.eyceS_MaxMarks > stu.eyceS_MarksEntryMax) {
                                var ratio_s_sub = (sum_s_sub_max_marks / subject_marks_entry_for);
                                var ECSTM_Marks = sum_s_sub_obt_marks;

                                stu.ecstM_Marks = ECSTM_Marks;
                                stu.ecstM_Flg = ECSTM_Flg;
                            }
                        }

                        main_save_list.push({ AMCST_Id: stu.amcsT_Id, AMCST_FirstName: stu.amcsT_FirstName, AMCST_AdmNo: stu.amcsT_AdmNo, ACYST_RollNo: stu.acysT_RollNo, ISMS_SubjectName: stu.ismS_SubjectName, ISMS_Id: stu.ismS_Id, ECYSES_MaxMarks: stu.ecyseS_MaxMarks, ECYSES_MarksEntryMax: stu.ecyseS_MarksEntryMax, ECYSES_MinMarks: stu.eyceS_MinMarks, ECSTM_Marks: stu.ecstM_Marks, ECSTM_Flg: stu.ECSTM_Flg });
                    }
                });

                if (main_save_list.length > 0) {
                    var data = {
                        main_save_list: main_save_list,
                        Temp_subs_marks_list: Temp_subs_marks_list,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        "EME_Id": $scope.EME_Id,
                        "ISMS_Id": $scope.ISMS_ID,
                        "ACMS_Id": $scope.ACMS_Id,
                        "ECYSES_MarksGradeEntryFlg": $scope.ecyseS_MarksGradeEntryFlg,
                        "ECYSES_SubSubjectFlg": $scope.ecyseS_SubSubjectFlg,
                        "ECYSES_SubExamFlg": $scope.ecyseS_SubExamFlg
                    };

                    apiService.create("CollegeMarksEntry/SaveMarks", data).
                        then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Data Saved Successfully');
                            }
                            else if (promise.returnval === false) {
                                swal('Failed to Save/Update Data');
                            }
                            $scope.clear();
                        });
                }
                else if (main_save_list.length === 0) {
                    swal("Select Students For Saving Marks...");
                }
            }
        };


        //for based on sub exams Max.Marks

        $scope.SaveMarks_SE = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var flag = "false";

                if ($scope.marksdeleteflag) {
                    swal({
                        title: "Are You Sure?",
                        text: "Marks Already Calculated, If You Update The Marks You Need To Recalculate Marks Again..Do You Want To Continue ..!?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update It!",
                        cancelButtonText: "Cancel..!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.save_SE(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                            }
                            else {
                                flag = "false";
                            }
                        });
                }
                else {
                    $scope.save_SE(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                }
            }
            else {
                swal('Please select required field / Entered Marks are not in correct format....!');
            }
        };

        $scope.save_SE = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            var main_save_list = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.temp_student_list_SE, function (stu) {
                    if (stu.selected_se) {
                        if ($scope.ecyseS_SubExamFlg) {

                            var others_cnt = 0;
                            var ECSTM_Flg = "";
                            var subject_max_marks = stu.ecyseS_MaxMarks;
                            var subject_marks_entry_for = stu.ecyseS_MarksEntryMax;
                            var sum_s_exm_max_marks = 0;
                            var sum_s_exm_obt_marks = 0;
                            angular.forEach($scope.subject_subexams_details, function (s_se) {
                                sum_s_exm_max_marks += s_se.ecysessS_MaxMarks;
                                if (stu.ECSTMSS_Marks[s_se.emsE_Id][stu.amcsT_Id].match(/[A-Z]/i)) {
                                    others_cnt += 1;
                                    ECSTM_Flg = stu.ECSTMSS_Marks[s_se.emsE_Id][stu.amcsT_Id];
                                }
                                else {
                                    sum_s_exm_obt_marks += Number(stu.ECSTMSS_Marks[s_se.emsE_Id][stu.amcsT_Id]);
                                }
                            });
                            if (stu.ecyseS_MaxMarks == stu.ecyseS_MarksEntryMax) {
                                var ratio_s_sub = (sum_s_exm_max_marks / subject_max_marks);
                                var ECSTM_Marks = sum_s_exm_obt_marks / ratio_s_sub;
                                stu.ecstM_Marks = ECSTM_Marks;
                                stu.ecstM_Flg = ECSTM_Flg;
                            }
                            else if (stu.ecyseS_MaxMarks > stu.eyceS_MarksEntryMax) {
                                var ratio_s_sub = (sum_s_exm_max_marks / subject_marks_entry_for);
                                var ECSTM_Marks = sum_s_exm_obt_marks / ratio_s_sub;
                                stu.ecstM_Marks = ECSTM_Marks;
                                stu.ecstM_Flg = ECSTM_Flg;
                            }
                        }

                        main_save_list.push({ AMCST_Id: stu.amcsT_Id, AMCST_FirstName: stu.amcsT_FirstName, AMCST_AdmNo: stu.amcsT_AdmNo, ACYST_RollNo: stu.acysT_RollNo, ISMS_SubjectName: stu.ismS_SubjectName, ISMS_Id: stu.ismS_Id, ECYSES_MaxMarks: stu.ecyseS_MaxMarks, ECYSES_MarksEntryMax: stu.ecyseS_MarksEntryMax, ECYSES_MinMarks: stu.ecyseS_MinMarks, ECSTM_Marks: stu.ecstM_Marks, ECSTM_Flg: stu.ecstM_Flg });
                    }
                });

                if (main_save_list.length > 0) {
                    var data = {
                        main_save_list: main_save_list,
                        Temp_subs_marks_list: Temp_subs_marks_list,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        "EME_Id": $scope.EME_Id,
                        "ISMS_Id": $scope.ISMS_ID,
                        "ACMS_Id": $scope.ACMS_Id,
                        "ECYSES_MarksGradeEntryFlg": $scope.ecyseS_MarksGradeEntryFlg,
                        "ECYSES_SubSubjectFlg": $scope.ecyseS_SubSubjectFlg,
                        "ECYSES_SubExamFlg": $scope.ecyseS_SubExamFlg
                    };

                    apiService.create("CollegeMarksEntry/SaveMarks", data).
                        then(function (promise) {


                            if (promise.returnval === true) {
                                swal('Data Saved Successfully');
                            }
                            else if (promise.returnval === false) {
                                swal('Failed to Save/Update Data');
                            }
                            $scope.clear();
                        });
                }
                else if (main_save_list.length == 0) {
                    swal("Select Students For Saving Marks...");
                }
            }

        };











        $scope.optionToggled_S = function () {
            $scope.all_s = $scope.temp_student_list_S.every(function (itm) { return itm.selected_s; });
            //  $scope.all_s = $scope.temp_student_list_S.every(function (itm) { return itm.selected_s; })
        };

        $scope.toggleAll_S = function (stas) {
            // var toggleStatus = $scope.all_s;
            var toggleStatus = $scope.all_s;
            angular.forEach($scope.temp_student_list_S, function (itm) {
                itm.selected_s = toggleStatus;

            });
        };

        $scope.optionToggled_SSSE = function () {
            $scope.all_ssse = $scope.temp_student_list_SSSE.every(function (itm) { return itm.selected_ssse; });
            //  $scope.all_s = $scope.temp_student_list_SSSE.every(function (itm) { return itm.selected_s; })
        };

        $scope.toggleAll_SSSE = function (stas) {
            // var toggleStatus = $scope.all_s;
            var toggleStatus = $scope.all_ssse;
            angular.forEach($scope.temp_student_list_SSSE, function (itm) {
                itm.selected_ssse = toggleStatus;
            });
        };

        $scope.optionToggled_SS = function () {
            $scope.all_ss = $scope.temp_student_list_SS.every(function (itm) { return itm.selected_ss; });
            //  $scope.all_s = $scope.temp_student_list_SS.every(function (itm) { return itm.selected_s; })
        };

        $scope.toggleAll_SS = function (stas) {
            // var toggleStatus = $scope.all_s;
            var toggleStatus = $scope.all_ss;
            angular.forEach($scope.temp_student_list_SS, function (itm) {
                itm.selected_ss = toggleStatus;
            });
        };

        $scope.optionToggled_SE = function () {
            $scope.all_se = $scope.temp_student_list_SE.every(function (itm) { return itm.selected_se; });
            //  $scope.all_s = $scope.temp_student_list_SE.every(function (itm) { return itm.selected_s; })
        };

        $scope.toggleAll_SE = function (stas) {
            // var toggleStatus = $scope.all_s;
            var toggleStatus = $scope.all_se;
            angular.forEach($scope.temp_student_list_SE, function (itm) {
                itm.selected_se = toggleStatus;

            });
        };


        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            columnDefs: [
                { name: 'SLNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'studentname', displayName: 'Student Name' },
                { name: 'amcsT_AdmNo', displayName: 'Admission No' },
                { name: 'acysT_RollNo', displayName: 'Roll No', type: 'number' },
                { name: 'totalMarks', displayName: 'Max Marks' },
                { name: 'minMarks', displayName: 'Min Marks' },
                { name: 'marksEnterFor', displayName: 'Marks Enter For' },
                { name: 'obtainmarks', displayName: 'Obtain Marks', cellTemplate: '<div class="ui-grid-cell-contents"><input type="text" ng-model="row.entity.obtainmarks"  style="text-align:center;" allow-pattern="[A-Z0-9+-]" ng-blur="grid.appScope.changemarks(row.entity.marksEnterFor,row.entity.obtainmarks,row)"  class="form-control" value="0"  min="0" </input></div>' }

            ]

        };

        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };
        $scope.submitted = false;

        $scope.clear = function () {
            $state.reload();
        };

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();