(function () {
    'use strict';
    angular.module('app').controller('MarksEntryHHSController', MarksEntryHHSController);
    MarksEntryHHSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter'];

    function MarksEntryHHSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter) {

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
        $scope.MarkCalculation = false;
        $scope.BindData = function () {
            apiService.getDATA("MarksEntryHHS/Getdetails").then(function (promise) {
                $scope.year_list = promise.yearlist;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.class_list = promise.classlist;
            });
        };
        $scope.update = 0;
        $scope.get_classes = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            };

            apiService.create("MarksEntryHHS/get_classes", data).then(function (promise) {
                $scope.class_list = promise.classlist;
                $scope.ASMCL_Id = "";
                $scope.ASMS_Id = "";
                $scope.EME_Id = "";
                $scope.ISMS_Id = "";
                $scope.section_list = [];
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.temp_student_list_S = [];
                $scope.temp_student_list_SSSE = [];
                $scope.temp_student_list_SS = [];
                $scope.temp_student_list_SE = [];
            });
        };

        $scope.get_sections = function (ASMCL_Id, ASMAY_Id) {
            $scope.display = false;
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "ASMCL_Id": ASMCL_Id
            };

            apiService.create("MarksEntryHHS/get_sections", data).then(function (promise) {
                $scope.section_list = promise.sectionlist;
                $scope.ASMS_Id = "";
                $scope.EME_Id = "";
                $scope.ISMS_Id = "";
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.temp_student_list_S = [];
                $scope.temp_student_list_SSSE = [];
                $scope.temp_student_list_SS = [];
                $scope.temp_student_list_SE = [];
            });
        };

        $scope.get_exams = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
            $scope.display = false;
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("MarksEntryHHS/get_exams", data).then(function (promise) {
                $scope.EME_Id = "";
                $scope.ISMS_Id = "";
                $scope.exam_list = promise.examlist;
                $scope.subject_list = [];
                $scope.temp_student_list_S = [];
                $scope.temp_student_list_SSSE = [];
                $scope.temp_student_list_SS = [];
                $scope.temp_student_list_SE = [];
            });
        };

        $scope.get_subjects = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id) {
            $scope.display = false;
            $scope.temp_student_list_S = [];
            $scope.temp_student_list_SSSE = [];
            $scope.temp_student_list_SS = [];
            $scope.temp_student_list_SE = [];

            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EME_Id": EME_Id
            };

            apiService.create("MarksEntryHHS/get_subjects", data).then(function (promise) {
                $scope.subject_list = promise.subjectlist;
                $scope.ISMS_Id = "";
                $scope.temp_student_list_S = [];
                $scope.temp_student_list_SSSE = [];
                $scope.temp_student_list_SS = [];
                $scope.temp_student_list_SE = [];
            });
        };

        $scope.onselectSubject = function () {
            $scope.display = false;             
            $scope.temp_student_list_S = [];
            $scope.temp_student_list_SSSE = [];
            $scope.temp_student_list_SS = [];
            $scope.temp_student_list_SE = [];
        };

        $scope.onsearch = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.display = false;             
            $scope.submitted = true;
            Temp_subs_marks_list = [];
            $scope.temp_student_list_S = [];
            $scope.temp_student_list_SSSE = [];
            $scope.temp_student_list_SS = [];
            $scope.temp_student_list_SE = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMS_Id": ASMS_Id,
                    "ASMCL_Id": ASMCL_Id,
                    "ASMAY_Id": ASMAY_Id,
                    "EME_Id": EME_Id,
                    "ISMS_Id": ISMS_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("MarksEntryHHS/onsearch", data).then(function (promise) {

                    if (promise.studentList !== null && promise.studentList.length > 0) {
                        var count = 0;
                        $scope.configurationsettings = promise.configuration;

                        if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "Name") {
                            $scope.sortKey = "amsT_FirstName";
                        }

                        else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "AdmNo") {
                            $scope.sortKey = "amsT_AdmNo";
                        }

                        else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "RollNo") {
                            $scope.sortKey = "amaY_RollNo";
                        }

                        else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "RegNo") {
                            $scope.sortKey = "amsT_RegistrationNo";
                        }

                        else {
                            $scope.sortKey = "amsT_FirstName";
                        }
                        $scope.display = true;

                        if ($scope.configurationsettings !== null && $scope.configurationsettings.length > 0) {
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

                        $scope.get_student_wise_papertype_list = promise.get_student_wise_papertype_list;
                        $scope.get_papertype_grade_details = promise.get_papertype_grade_details;

                        // when sub subject and sub exam not there
                        if (!promise.eyceS_SubSubjectFlg && !promise.eyceS_SubExamFlg) {

                            $scope.update = promise.saveupdatecount;
                            $scope.eyceidlastdateentry = promise.lastdateentry;
                            $scope.savemarksbutton = promise.lastdateentryflag;
                            $scope.lastdateexamflag = promise.lastdateexamflag;

                            if ($scope.lastdateexamflag === false) {
                                $scope.display = false;
                                $scope.marksentrystatedate = promise.marksentrystatedate;
                                var dated = $filter('date')(new Date($scope.marksentrystatedate), 'MMM dd yyyy');
                                swal("From " + dated + " You Can Start Enter The Marks");
                            } else {
                                if ($scope.savemarksbutton === false) {
                                    swal("Marks Entry Last Date Completed You Can Not Enter Marks");
                                }
                            }

                            $scope.temp_student_list_S = promise.studentList;

                            angular.forEach($scope.temp_student_list_S, function (stu) {
                                angular.forEach($scope.get_student_wise_papertype_list, function (stu_pt) {
                                    if (stu.amsT_Id === stu_pt.amsT_Id) {
                                        stu.papertype = stu_pt.empatY_PaperTypeName;
                                        stu.papertypeid = stu_pt.empatY_Id;
                                    }
                                });
                            });

                            $scope.marksdeleteflag = promise.marksdeleteflag;
                            $scope.subject_details = promise.subject_details;
                            $scope.eyceS_SubSubjectFlg = promise.eyceS_SubSubjectFlg;
                            $scope.eyceS_SubExamFlg = promise.eyceS_SubExamFlg;
                            $scope.eyceS_MarksGradeEntryFlg = promise.eyceS_MarksGradeEntryFlg;

                            if (promise.eyceS_MarksGradeEntryFlg === 'M') {
                                // $scope.ngpattern = /^[0-9]{0,4}\.?[0-9]{1,2}?$/;
                                $scope.allowpattern = "[0-9A-Z.]";
                                $scope.placeholder = "Enter Marks...";
                            }
                            else if (promise.eyceS_MarksGradeEntryFlg === 'G') {
                                // $scope.ngpattern = "";
                                $scope.allowpattern = "[A-Z0-9+-]";
                                $scope.placeholder = "Enter Grade...";
                            }

                            var final_subject_array = [];
                            $scope.row_span = 0;
                            $scope.col_span = 0;
                            angular.forEach(promise.subject_details, function (sub_deta) {
                                angular.forEach($scope.subject_list, function (sub_m) {
                                    if (sub_deta.ismS_Id == sub_m.ismS_Id) {
                                        sub_deta.ismS_SubjectName = sub_m.ismS_SubjectName;
                                    }
                                });
                                $scope.row_span += 1;
                                $scope.col_span += 1;
                                if (promise.eyceS_SubSubjectFlg) {
                                    sub_deta.subsubjects = promise.subject_subsubjects;
                                    $scope.row_span += 1;
                                    $scope.col_span += promise.subject_subsubjects.length;
                                }
                                if (promise.eyceS_SubExamFlg) {
                                    sub_deta.subexams = promise.subject_subexams;
                                    $scope.row_span += 1;
                                    $scope.col_span += promise.subject_subexams.length;
                                }
                            });

                            if (promise.eyceS_SubSubjectFlg && promise.eyceS_SubExamFlg) {
                                $scope.temp_sub_subjs_exams = [];

                                angular.forEach(promise.subject_subsubjects, function (sub_s) {
                                    angular.forEach(promise.subject_subexams, function (sub_e) {
                                        $scope.temp_sub_subjs_exams.push({ sub_subject: sub_s, sub_exam: sub_e });
                                    });
                                });
                            }

                            console.log($scope.col_span);
                            if (promise.eyceS_SubSubjectFlg) {
                                $scope.subject_subsubjects_details = promise.subject_subsubjects;
                            }
                            if (promise.eyceS_SubExamFlg) {
                                $scope.subject_subexams_details = promise.subject_subexams;
                            }
                            if (promise.eyceS_MarksGradeEntryFlg === "G") {
                                $scope.grade_details = promise.grade_details;
                                if (promise.eyceS_SubSubjectFlg) {
                                    $scope.subject_subsubject_grade_details = promise.subsubject_gradedetails;
                                }
                                if (promise.eyceS_SubExamFlg) {
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
                        else if (promise.eyceS_SubSubjectFlg && promise.eyceS_SubExamFlg) {
                            $scope.update = promise.saveupdatecount;
                            $scope.eyceidlastdateentry = promise.lastdateentry;
                            $scope.savemarksbutton = promise.lastdateentryflag;

                            $scope.lastdateexamflag = promise.lastdateexamflag;
                            if ($scope.lastdateexamflag === false) {
                                $scope.display = false;
                                $scope.marksentrystatedate = promise.marksentrystatedate;
                                var datedd = $filter('date')(new Date($scope.marksentrystatedate), 'MMM dd yyyy');
                                swal("From " + datedd + " You Can Start Enter The Marks");
                            } else {
                                if ($scope.savemarksbutton === false) {
                                    swal("Marks Entry Last Date Completed You Can Not Enter Marks");
                                }
                            }

                            $scope.temp_student_list_SSSE = promise.studentList;
                            $scope.marksdeleteflag = promise.marksdeleteflag;
                            $scope.subject_details = promise.subject_details;
                            $scope.eyceS_SubSubjectFlg = promise.eyceS_SubSubjectFlg;
                            $scope.eyceS_SubExamFlg = promise.eyceS_SubExamFlg;
                            $scope.eyceS_MarksGradeEntryFlg = promise.eyceS_MarksGradeEntryFlg;

                            var final_subject_array = [];

                            $scope.row_span = 0;
                            $scope.col_span = 0;

                            angular.forEach(promise.subject_details, function (sub_deta) {
                                angular.forEach($scope.subject_list, function (sub_m) {
                                    if (sub_deta.ismS_Id == sub_m.ismS_Id) {
                                        sub_deta.ismS_SubjectName = sub_m.ismS_SubjectName;
                                    }
                                });

                                $scope.row_span += 1;

                                if (promise.eyceS_SubSubjectFlg && promise.eyceS_SubExamFlg) {
                                    sub_deta.subsubjects = promise.subject_subsubjects;
                                    sub_deta.subexams = promise.subject_subexams;
                                    $scope.row_span += 2;
                                    $scope.col_span += ((promise.subsubjectlist.length));
                                }

                                else {
                                    if (promise.eyceS_SubSubjectFlg) {
                                        sub_deta.subsubjects = promise.subject_subsubjects;
                                        $scope.row_span += 1;
                                        $scope.col_span += promise.subject_subsubjects.length;
                                    }
                                    if (promise.eyceS_SubExamFlg) {
                                        sub_deta.subexams = promise.subject_subexams;
                                        $scope.row_span += 1;
                                        $scope.col_span += promise.subject_subexams.length;
                                    }
                                }
                            });

                            if (promise.eyceS_SubSubjectFlg && promise.eyceS_SubExamFlg) {
                                $scope.temp_sub_subjs_exams = [];
                                angular.forEach(promise.subject_subsubjects, function (sub_s) {
                                    angular.forEach(promise.subject_subexams, function (sub_e) {
                                        if (sub_e.emsS_Id == sub_s.emsS_Id) {
                                            $scope.temp_sub_subjs_exams.push({ sub_subject: sub_s, sub_exam: sub_e });
                                        }
                                    });
                                });
                            }

                            console.log($scope.col_span);
                            $scope.subject_subsubjects_details = [];
                            $scope.temp_sub_subjs_exams = [];

                            if (promise.eyceS_SubSubjectFlg) {
                                angular.forEach(promise.subsubjectlist, function (ddd) {
                                    $scope.temp = [];
                                    angular.forEach(promise.subject_subsubjects, function (dd) {
                                        if (ddd.emsS_Id == dd.emsS_Id) {
                                            $scope.temp.push(dd);
                                            $scope.temp_sub_subjs_exams.push(dd);
                                        }
                                    });

                                    $scope.subject_subsubjects_details.push({ subsubject: ddd, subexamlist: $scope.temp });

                                });
                                $scope.col_span += $scope.temp_sub_subjs_exams.length;
                            }

                            if (promise.eyceS_SubExamFlg) {
                                $scope.subject_subexams_details = promise.subexamlist;
                            }

                            if (promise.eyceS_MarksGradeEntryFlg === "G") {
                                $scope.grade_details = promise.grade_details;
                                if (promise.eyceS_SubSubjectFlg) {
                                    $scope.subject_subsubject_grade_details = promise.subsubject_gradedetails;
                                }
                                if (promise.eyceS_SubExamFlg) {
                                    $scope.subject_subexam_grade_details = promise.subexam_gradedetails;
                                }
                            }


                            if (promise.eyceS_MarksGradeEntryFlg === 'M') {
                                // $scope.ngpattern = /^[0-9]{0,4}\.?[0-9]{1,2}?$/;
                                $scope.allowpattern = "[0-9A-Z.]";
                                $scope.placeholder = "Enter Marks...";
                            }
                            else if (promise.eyceS_MarksGradeEntryFlg === 'G') {
                                // $scope.ngpattern = "";
                                $scope.allowpattern = "[A-Z0-9+-]";
                                $scope.placeholder = "Enter Grade...";
                            }

                            $scope.submitted = false;
                            $scope.myForm.$setPristine();
                            $scope.myForm.$setUntouched();
                            $scope.subMorGFlag = promise.subMorGFlag;

                            //$scope.gradname = promise.gradname;

                            $scope.gradname = $scope.subject_subsubject_grade_details;

                            if (promise.saved_studentList !== null && promise.saved_studentList.length > 0 && promise.saved_ssse_list !== null
                                && promise.saved_ssse_list.length > 0) {
                                $scope.map_marks_SSSE(promise.saved_studentList, promise.saved_ssse_list);
                            }
                        }

                        // when sub subject is there 
                        else if (promise.eyceS_SubSubjectFlg && !promise.eyceS_SubExamFlg) {
                            if (promise.subject_subsubjects !== null && promise.subject_subsubjects.length > 0) {

                                $scope.update = promise.saveupdatecount;

                                $scope.eyceidlastdateentry = promise.lastdateentry;
                                $scope.savemarksbutton = promise.lastdateentryflag;

                                $scope.gradname = promise.subsubject_gradedetails;

                                $scope.lastdateexamflag = promise.lastdateexamflag;
                                if ($scope.lastdateexamflag === false) {
                                    $scope.display = false;
                                    $scope.marksentrystatedate = promise.marksentrystatedate;
                                    var datedd1 = $filter('date')(new Date($scope.marksentrystatedate), 'MMM dd yyyy');
                                    swal("From " + datedd1 + " You Can Start Enter The Marks");
                                } else {
                                    if ($scope.savemarksbutton === false) {
                                        swal("Marks Entry Last Date Completed You Can Not Enter Marks");
                                    }
                                }

                                $scope.temp_student_list_SS = promise.studentList;
                                $scope.marksdeleteflag = promise.marksdeleteflag;
                                $scope.subject_details = promise.subject_details;
                                $scope.eyceS_SubSubjectFlg = promise.eyceS_SubSubjectFlg;
                                $scope.eyceS_SubExamFlg = promise.eyceS_SubExamFlg;
                                $scope.eyceS_MarksGradeEntryFlg = promise.eyceS_MarksGradeEntryFlg;
                                var final_subject_array = [];
                                $scope.row_span = 0;
                                $scope.col_span = 0;
                                angular.forEach(promise.subject_details, function (sub_deta) {
                                    angular.forEach($scope.subject_list, function (sub_m) {
                                        if (sub_deta.ismS_Id == sub_m.ismS_Id) {
                                            sub_deta.ismS_SubjectName = sub_m.ismS_SubjectName;
                                        }
                                    });
                                    $scope.row_span += 1;
                                    $scope.col_span += 1;
                                    if (promise.eyceS_SubSubjectFlg) {
                                        sub_deta.subsubjects = promise.subject_subsubjects;
                                        $scope.row_span += 1;
                                        $scope.col_span += promise.subject_subsubjects.length;
                                    }
                                });

                                console.log($scope.col_span);
                                if (promise.eyceS_SubSubjectFlg) {
                                    $scope.subject_subsubjects_details = promise.subject_subsubjects;
                                }

                                if (promise.eyceS_MarksGradeEntryFlg === "G") {
                                    $scope.grade_details = promise.grade_details;
                                    if (promise.eyceS_SubSubjectFlg) {
                                        $scope.subject_subsubject_grade_details = promise.subsubject_gradedetails;
                                    }
                                }

                                if (promise.eyceS_MarksGradeEntryFlg === 'M') {
                                    // $scope.ngpattern = /^[0-9]{0,4}\.?[0-9]{1,2}?$/;
                                    $scope.allowpattern = "[0-9A-Z.]";
                                    $scope.placeholder = "Enter Marks...";
                                }
                                else if (promise.eyceS_MarksGradeEntryFlg === 'G') {
                                    // $scope.ngpattern = "";
                                    $scope.allowpattern = "[A-Z0-9+-]";
                                    $scope.placeholder = "Enter Grade...";
                                }

                                $scope.submitted = false;
                                $scope.myForm.$setPristine();
                                $scope.myForm.$setUntouched();
                                $scope.subMorGFlag = promise.subMorGFlag;

                                //$scope.gradname = promise.gradname;

                                if (promise.saved_studentList !== null && promise.saved_studentList.length > 0 && promise.saved_ss_list !== null
                                    && promise.saved_ss_list.length > 0) {
                                    $scope.map_marks_SS(promise.saved_studentList, promise.saved_ss_list);
                                }
                            } else {
                                swal("For This User Sub Subject Is Not Mapped For Selected Subject, Kinldy Contact Administrator");
                            }
                        }

                        // when sub exam is there 
                        else if (promise.eyceS_SubExamFlg && !promise.eyceS_SubSubjectFlg) {
                            if (promise.subject_subexams !== null && promise.subject_subexams.length > 0) {

                                $scope.update = promise.saveupdatecount;

                                $scope.eyceidlastdateentry = promise.lastdateentry;

                                $scope.savemarksbutton = promise.lastdateentryflag;

                                $scope.lastdateexamflag = promise.lastdateexamflag;

                                $scope.gradname = promise.subexam_gradedetails;

                                if ($scope.lastdateexamflag === false) {
                                    $scope.display = false;
                                    $scope.marksentrystatedate = promise.marksentrystatedate;
                                    var datedd = $filter('date')(new Date($scope.marksentrystatedate), 'MMM dd yyyy');
                                    swal("From " + datedd + " You Can Start Enter The Marks");
                                } else {
                                    if ($scope.savemarksbutton === false) {
                                        swal("Marks Entry Last Date Completed You Can Not Enter Marks");
                                    }
                                }

                                $scope.temp_student_list_SE = promise.studentList;
                                $scope.marksdeleteflag = promise.marksdeleteflag;
                                $scope.subject_details = promise.subject_details;
                                $scope.eyceS_SubSubjectFlg = promise.eyceS_SubSubjectFlg;
                                $scope.eyceS_SubExamFlg = promise.eyceS_SubExamFlg;
                                $scope.eyceS_MarksGradeEntryFlg = promise.eyceS_MarksGradeEntryFlg;
                                var final_subject_array = [];
                                $scope.row_span = 0;
                                $scope.col_span = 0;
                                angular.forEach(promise.subject_details, function (sub_deta) {
                                    angular.forEach($scope.subject_list, function (sub_m) {
                                        if (sub_deta.ismS_Id == sub_m.ismS_Id) {
                                            sub_deta.ismS_SubjectName = sub_m.ismS_SubjectName;
                                        }
                                    });
                                    $scope.row_span += 1;
                                    $scope.col_span += 1;
                                    if (promise.eyceS_SubExamFlg) {
                                        sub_deta.subexams = promise.subject_subexams;
                                        $scope.row_span += 1;
                                        $scope.col_span += promise.subject_subexams.length;
                                    }
                                });
                                console.log($scope.col_span);
                                if (promise.eyceS_SubExamFlg) {
                                    $scope.subject_subexams_details = promise.subject_subexams;
                                }
                                if (promise.eyceS_MarksGradeEntryFlg === "G") {
                                    $scope.grade_details = promise.grade_details;
                                    if (promise.eyceS_SubExamFlg) {
                                        $scope.subject_subexam_grade_details = promise.subexam_gradedetails;
                                    }
                                }

                                if (promise.eyceS_MarksGradeEntryFlg === 'M') {
                                    // $scope.ngpattern = /^[0-9]{0,4}\.?[0-9]{1,2}?$/;
                                    $scope.allowpattern = "[0-9A-Z.]";
                                    $scope.placeholder = "Enter Marks...";
                                }
                                else if (promise.eyceS_MarksGradeEntryFlg === 'G') {
                                    // $scope.ngpattern = "";
                                    $scope.allowpattern = "[A-Z0-9+-]";
                                    $scope.placeholder = "Enter Grade...";
                                }

                                $scope.submitted = false;
                                $scope.myForm.$setPristine();
                                $scope.myForm.$setUntouched();
                                $scope.subMorGFlag = promise.subMorGFlag;
                                $scope.gradname = promise.gradname;
                                if (promise.saved_studentList != null && promise.saved_studentList.length > 0 && promise.saved_se_list !== null
                                    && promise.saved_se_list.length > 0) {
                                    $scope.map_marks_SE(promise.saved_studentList, promise.saved_se_list);
                                }
                            } else {
                                swal("For This User Sub Exam Is Not Mapped For Selected Subject, Kinldy Contact Administrator");
                            }
                        }
                    }
                    else {
                        swal("This Subject Is Not Mapped With Students");
                        $scope.ISMS_Id = "";
                    }
                });
            }
        };

        var Temp_subs_marks_list = [];

        // ONLY FOR SUBJECT APPLICABLE
        $scope.map_marks_S = function (saved_studentList) {
            angular.forEach(saved_studentList, function (stu_s) {
                angular.forEach($scope.temp_student_list_S, function (stu_m) {
                    if (stu_m.amsT_Id == stu_s.amsT_Id) {
                        stu_m.selected_s = true;
                        $scope.optionToggled_S();

                        angular.forEach($scope.subject_details, function (sub) {
                            if (stu_m.ismS_Id == stu_s.ismS_Id && stu_m.ismS_Id == sub.ismS_Id) {
                                if ($scope.eyceS_MarksGradeEntryFlg === 'M') {
                                    if (stu_s.estM_Flg === '' || stu_s.estM_Flg === null) {
                                        var flags = 0;
                                        var mraks = '' + stu_s.estM_Marks + '';
                                        if (mraks.match(/[.]/i)) {
                                            var sliptmarks = mraks.split('.');
                                            if (sliptmarks[0] >= 1 && sliptmarks[0] < 10) {
                                                flags = 1;
                                            } else {
                                                flags = 0;
                                            }
                                        } else {
                                            if (stu_s.estM_Marks >= 1 && stu_s.estM_Marks < 10) {
                                                flags = 1;
                                            } else {
                                                flags = 0;
                                            }
                                        }
                                        if (flags === 1) {
                                            stu_m.ESTM_Marks = '0' + stu_s.estM_Marks.toString();
                                        } else {
                                            stu_m.ESTM_Marks = stu_s.estM_Marks.toString();
                                        }

                                        //stu_m.ESTM_Marks = stu_s.estM_Marks.toString();
                                    }
                                    else {
                                        stu_m.ESTM_Marks = stu_s.estM_Flg.toString();
                                    }
                                }
                                else if ($scope.eyceS_MarksGradeEntryFlg === 'G') {
                                    if (stu_s.estM_Flg === '' || stu_s.estM_Flg === null) {
                                        stu_m.ESTM_Marks = stu_s.estM_Grade.toString();
                                    }
                                    else {
                                        stu_m.ESTM_Marks = stu_s.estM_Flg.toString();
                                    }
                                }

                                $scope.valid_marks_S(sub, sub.ismS_Id, stu_m.ESTM_Marks, stu_m.amsT_Id, stu_m);
                            }
                        });

                    }
                });
            });
        };

        $scope.toggleAll_S = function (stas) {
            //var toggleStatus = $scope.all_s;
            var toggleStatus = stas;
            angular.forEach($scope.temp_student_list_S, function (itm) {
                itm.selected_s = toggleStatus;
            });
        };

        $scope.optionToggled_S = function () {
            $scope.all_s = $scope.temp_student_list_S.every(function (itm) { return itm.selected_s; });
        };

        $scope.valid_marks_S = function (obj_s, subj_id, values, stu_id, obj_student) {
            if ($scope.eyceS_MarksGradeEntryFlg === "G") {
                var flag = "false";
                var obtainmarks = values;
                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                    if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M"
                        || obtainmarks.toUpperCase() === "OD") {
                        flag = "true";
                        if (Temp_subs_marks_list.length > 0) {
                            var al_cnt = 0;
                            angular.forEach(Temp_subs_marks_list, function (yet) {
                                if (yet.AMST_Id == stu_id && yet.ISMS_Id == subj_id) {
                                    yet.ESTM_Flg = obtainmarks;
                                    yet.ESTM_Grade = null;
                                    al_cnt += 1;
                                }
                            });

                            if (al_cnt === 0) {
                                Temp_subs_marks_list.push({
                                    AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg,
                                    ESTM_Flg: obtainmarks, ESTM_Marks: 0, ESTM_Grade: null
                                });
                            }
                        }
                        else if (Temp_subs_marks_list.length == 0) {
                            Temp_subs_marks_list.push({
                                AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg,
                                ESTM_Flg: obtainmarks, ESTM_Marks: 0, ESTM_Grade: null
                            });
                        }
                    }
                    if (flag === "false") {
                        //Checking Grade Validation For Paper Based Exam Type
                        if ($scope.get_student_wise_papertype_list !== null && $scope.get_student_wise_papertype_list.length > 0) {
                            angular.forEach($scope.get_papertype_grade_details, function (grade_details) {
                                if (grade_details.empatY_Id === obj_student.papertypeid && grade_details.emgD_Name.toUpperCase() === obtainmarks.toUpperCase()) {
                                    flag = "true";
                                }
                            });
                        } else {
                            for (var i = 0; i < $scope.grade_details.length; i++) {
                                if ($scope.grade_details[i].toUpperCase() == obtainmarks.toUpperCase()) {
                                    flag = "true";
                                }
                            }
                        }
                        if (flag === "false") {
                            obj_student.ESTM_Marks = "";
                            swal('Entered Grade cant be out of master setting...!');
                        } else {
                            if (Temp_subs_marks_list.length > 0) {
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == subj_id) {
                                        yet.ESTM_Grade = obtainmarks;
                                        yet.ESTM_Flg = "";
                                        al_cnt += 1;
                                    }
                                });

                                if (al_cnt == 0) {
                                    Temp_subs_marks_list.push({
                                        AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg,
                                        ESTM_Marks: 0, ESTM_Grade: obtainmarks, ESTM_Flg: ""
                                    });
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({
                                    AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Marks: 0,
                                    ESTM_Grade: obtainmarks, ESTM_Flg: ""
                                });
                            }
                        }
                    }
                }
            }
            else {
                var flag = "false";
                var obtainmarks = values;
                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                    if (obtainmarks.match(/[a-z]/i)) {
                        if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                            flag = "true";
                            if (Temp_subs_marks_list.length > 0) {
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == subj_id) {
                                        yet.ESTM_Flg = obtainmarks;
                                        yet.ESTM_Marks = 0;
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt == 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Flg: obtainmarks, ESTM_Marks: 0, ESTM_Grade: null });
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Flg: obtainmarks, ESTM_Marks: 0, ESTM_Grade: null });
                            }

                        }
                        if (flag === "false") {
                            obj_student.ESTM_Marks = "";
                            swal('Entered value cant be out of master setting...!');
                        }
                    }
                    else {
                        var totalMarks = 0;

                        //if (Number(obj_student.eyceS_MaxMarks) > Number(obj_student.eyceS_MarksEntryMax)) {
                        //    totalMarks = Number(obj_student.eyceS_MarksEntryMax);
                        //}
                        //else {
                        totalMarks = Number(obj_student.eyceS_MarksEntryMax);
                        //}

                        obtainmarks = Number(obtainmarks);
                        if (totalMarks < obtainmarks) {
                            obj_student.ESTM_Marks = "";
                            swal('Entered marks cant be more than Max Marks ...!' + totalMarks);
                        }
                        else if (obtainmarks < 0) {
                            obj_student.ESTM_Marks = "";
                            swal('Entered marks cant be in nagative values...!');
                        }
                        else {
                            if (Temp_subs_marks_list.length > 0) {
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == subj_id) {
                                        yet.ESTM_Marks = obtainmarks;
                                        yet.ESTM_Flg = "";
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt == 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Marks: obtainmarks, ESTM_Grade: null, ESTM_Flg: "" });
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Marks: obtainmarks, ESTM_Grade: null, ESTM_Flg: "" });
                            }
                        }
                    }
                }
            }
        };

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
                        var ESTM_Marks = 0;
                        var ESTM_Flg = "";
                        var ESTM_Grade = "";
                        angular.forEach(Temp_subs_marks_list, function (stu_t) {
                            if (stu_t.AMST_Id == stu.amsT_Id && stu_t.ISMS_Id == stu.ismS_Id) {
                                ESTM_Marks = stu_t.ESTM_Marks;
                                ESTM_Flg = stu_t.ESTM_Flg;
                                ESTM_Grade = stu_t.ESTM_Grade;
                            }
                        });
                        main_save_list.push({ AMST_Id: stu.amsT_Id, AMST_FirstName: stu.amsT_FirstName, AMST_AdmNo: stu.amsT_AdmNo, AMAY_RollNo: stu.amaY_RollNo, ISMS_SubjectName: stu.ismS_SubjectName, ISMS_Id: stu.ismS_Id, EYCES_MaxMarks: stu.eyceS_MaxMarks, EYCES_MarksEntryMax: stu.eyceS_MarksEntryMax, EYCES_MinMarks: stu.eyceS_MinMarks, ESTM_Marks: ESTM_Marks, ESTM_Flg: ESTM_Flg, ESTM_Grade: ESTM_Grade })
                    }
                });
                if (main_save_list.length > 0) {
                    var data = {
                        main_save_list: main_save_list,
                        "ASMS_Id": ASMS_Id,
                        "ASMCL_Id": ASMCL_Id,
                        "ASMAY_Id": ASMAY_Id,
                        "EME_Id": EME_Id,
                        "ISMS_Id": ISMS_Id,
                        "EYCES_MarksGradeEntryFlg": $scope.eyceS_MarksGradeEntryFlg,
                        "EYCES_SubSubjectFlg": $scope.eyceS_SubSubjectFlg,
                        "EYCES_SubExamFlg": $scope.eyceS_SubExamFlg,
                        "IVRMMAP_AddFlg": $scope.MarkCalculation,
                        "Pagename": "School"
                    };
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    };
                    apiService.create("MarksEntryHHS/SaveMarks", data).then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Data Saved Successfully');
                        }
                        else if (promise.returnval == false) {
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


        // ONLY FOR SUBSUBJECT AND SUBEXAM APPLICABLE
        $scope.map_marks_SSSE = function (saved_studentList, saved_ssse_list) {
            angular.forEach(saved_studentList, function (stu_s) {
                angular.forEach($scope.temp_student_list_SSSE, function (stu_m) {
                    if (stu_m.amsT_Id == stu_s.amsT_Id) {
                        stu_m.selected_ssse = true;
                        $scope.optionToggled_SSSE();
                        stu_m.ESTMSS_Marks = [];

                        if ($scope.eyceS_MarksGradeEntryFlg === "M") {
                            angular.forEach($scope.temp_sub_subjs_exams, function (y) {
                                angular.forEach($scope.subject_subsubjects_details, function (ss) {
                                    if (ss.subsubject.emsS_Id == y.emsS_Id) {
                                        if (stu_m.ESTMSS_Marks[ss.subsubject.emsS_Id] === undefined) {
                                            stu_m.ESTMSS_Marks[ss.subsubject.emsS_Id] = [];
                                        }

                                        angular.forEach($scope.subject_subexams_details, function (se) {
                                            if (se.emsE_Id == y.emsE_Id) {
                                                stu_m.ESTMSS_Marks[ss.subsubject.emsS_Id][se.emsE_Id] = [];
                                                angular.forEach(saved_ssse_list, function (ssse_s) {
                                                    if (ssse_s.emsS_Id == y.emsS_Id && ssse_s.emsE_Id == y.emsE_Id) {
                                                        if (ssse_s.estM_Id == stu_s.estM_Id) {
                                                            if ((ssse_s.estmsS_Flg !== null && ssse_s.estmsS_Flg !== "") &&
                                                                (ssse_s.estmsS_Marks === null || ssse_s.estmsS_Marks == 0.00)) {

                                                                stu_m.ESTMSS_Marks[ssse_s.emsS_Id][ssse_s.emsE_Id][stu_m.amsT_Id] = ssse_s.estmsS_Flg.toString();
                                                            }
                                                            else if ((ssse_s.estmsS_Flg === null || ssse_s.estmsS_Flg === "") && ssse_s.estmsS_Marks !== null) {
                                                                var flagsse = 0;
                                                                var markssse = '' + ssse_s.estmsS_Marks + '';
                                                                if (markssse.match(/[.]/i)) {
                                                                    var sliptmarks = mraks.split('.');
                                                                    if (sliptmarks[0] >= 1 && sliptmarks[0] < 10) {
                                                                        flagsse = 1;
                                                                    } else {
                                                                        flagsse = 0;
                                                                    }
                                                                } else {
                                                                    if (ssse_s.estmsS_Marks >= 1 && ssse_s.estmsS_Marks < 10) {
                                                                        flagsse = 1;
                                                                    } else {
                                                                        flagsse = 0;
                                                                    }
                                                                }
                                                                if (flagsse === 1) {
                                                                    stu_m.ESTMSS_Marks[ssse_s.emsS_Id][ssse_s.emsE_Id][stu_m.amsT_Id] = '0' + ssse_s.estmsS_Marks.toString();
                                                                } else {
                                                                    stu_m.ESTMSS_Marks[ssse_s.emsS_Id][ssse_s.emsE_Id][stu_m.amsT_Id] = ssse_s.estmsS_Marks.toString();
                                                                }
                                                            }

                                                            $scope.valid_marks_SSSE(y, y.emsS_Id, y.emsE_Id, stu_m.ESTMSS_Marks, stu_m.amsT_Id, stu_m);
                                                        }
                                                    }
                                                });
                                            }
                                        });
                                    }
                                });
                            });
                        } else {
                            angular.forEach($scope.temp_sub_subjs_exams, function (y) {
                                angular.forEach($scope.subject_subsubjects_details, function (ss) {
                                    if (ss.subsubject.emsS_Id == y.emsS_Id) {
                                        if (stu_m.ESTMSS_Marks[ss.subsubject.emsS_Id] === undefined) {
                                            stu_m.ESTMSS_Marks[ss.subsubject.emsS_Id] = [];
                                        }

                                        angular.forEach($scope.subject_subexams_details, function (se) {
                                            if (se.emsE_Id == y.emsE_Id) {
                                                stu_m.ESTMSS_Marks[ss.subsubject.emsS_Id][se.emsE_Id] = [];
                                                angular.forEach(saved_ssse_list, function (ssse_s) {
                                                    if (ssse_s.emsS_Id == y.emsS_Id && ssse_s.emsE_Id == y.emsE_Id) {
                                                        if (ssse_s.estM_Id == stu_s.estM_Id) {
                                                            if ((ssse_s.estmsS_Flg !== null && ssse_s.estmsS_Flg !== "") &&
                                                                (ssse_s.estmsS_Grade === null || ssse_s.estmsS_Grade == '')) {

                                                                stu_m.ESTMSS_Marks[ssse_s.emsS_Id][ssse_s.emsE_Id][stu_m.amsT_Id] = ssse_s.estmsS_Flg.toString();
                                                            }
                                                            else if ((ssse_s.estmsS_Flg === null || ssse_s.estmsS_Flg === "")
                                                                && ssse_s.estmsS_Grade !== null) {
                                                                stu_m.ESTMSS_Marks[ssse_s.emsS_Id][ssse_s.emsE_Id][stu_m.amsT_Id] = ssse_s.estmsS_Grade;
                                                            }
                                                            $scope.valid_marks_SSSE(y, y.emsS_Id, y.emsE_Id, stu_m.ESTMSS_Marks, stu_m.amsT_Id, stu_m);
                                                        }
                                                    }
                                                });
                                            }
                                        });
                                    }
                                });
                            });
                        }
                    }
                });
            });
        };

        $scope.toggleAll_SSSE = function (stas) {
            var toggleStatus = stas;
            angular.forEach($scope.temp_student_list_SSSE, function (itm) {
                itm.selected_ssse = toggleStatus;
            });
        };

        $scope.optionToggled_SSSE = function () {
            $scope.all_ssse = $scope.temp_student_list_SSSE.every(function (itm) { return itm.selected_ssse; });
        };

        $scope.valid_marks_SSSE = function (obj_ssse, s_subj_id, s_exm_id, values, stu_id, obj_student) {
            if ($scope.eyceS_MarksGradeEntryFlg === "G") {
                var flag = "false";
                var obtainmarks = values[s_subj_id][s_exm_id][stu_id];

                for (var i = 0; i < $scope.gradname.length; i++) {
                    if ($scope.gradname[i].emgD_Name.toUpperCase() == obtainmarks.toUpperCase()) {
                        flag = "true";
                    }
                }

                if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L"
                    || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() == "OD") {
                    flag = "true";
                }

                if (flag === "false") {
                    values[s_subj_id][s_exm_id][stu_id] = "";
                    swal('Entered Grade cant be out of master setting...!');
                }

                else {
                    if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                        if (obtainmarks.match(/[a-z]/i)) {
                            if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                                flag = "true";
                                if (Temp_subs_marks_list.length > 0) {
                                    var al_cnt = 0;
                                    angular.forEach(Temp_subs_marks_list, function (yet) {
                                        if (yet.AMST_Id == stu_id && yet.ISMS_Id == obj_student.ismS_Id && yet.EMSS_Id == s_subj_id && yet.EMSE_Id == s_exm_id) {
                                            yet.ESTMSS_Flg = obtainmarks;
                                            yet.ESTMSS_Marks = null;
                                            al_cnt += 1;
                                        }
                                    });

                                    if (al_cnt === 0) {
                                        Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: obtainmarks, ESTMSS_Marks: null, ESTMSS_Grade: null });
                                    }
                                }
                                else if (Temp_subs_marks_list.length == 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: obtainmarks, ESTMSS_Marks: null, ESTMSS_Grade: null });
                                }

                            }
                            if (flag === "false") {
                                values[s_subj_id][s_exm_id][stu_id] = "";
                                swal('Entered value cant be out of master setting...!');
                            }
                            else {
                                if (Temp_subs_marks_list.length > 0) {
                                    var al_cnt = 0;
                                    angular.forEach(Temp_subs_marks_list, function (yet) {
                                        if (yet.AMST_Id == stu_id && yet.ISMS_Id == obj_student.ismS_Id && yet.EMSS_Id == s_subj_id && yet.EMSE_Id == s_exm_id) {
                                            yet.ESTMSS_Grade = obtainmarks;
                                            yet.ESTMSS_Flg = null;
                                            yet.ESTMSS_Marks = null;
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Marks: null, ESTMSS_Grade: obtainmarks, ESTMSS_Flg: null });
                                    }
                                }
                                else if (Temp_subs_marks_list.length == 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Marks: null, ESTMSS_Grade: obtainmarks, ESTMSS_Flg: null });
                                }
                            }
                        }
                    }
                }
            }
            else {
                var flag = "false";
                var obtainmarks = values[s_subj_id][s_exm_id][stu_id];

                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                    if (obtainmarks.match(/[a-z]/i)) {
                        if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                            flag = "true";
                            if (Temp_subs_marks_list.length > 0) {
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == obj_student.ismS_Id && yet.EMSS_Id == s_subj_id && yet.EMSE_Id == s_exm_id) {
                                        yet.ESTMSS_Flg = obtainmarks;
                                        yet.ESTMSS_Marks = null;
                                        al_cnt += 1;
                                    }
                                });

                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: obtainmarks, ESTMSS_Marks: null, ESTMSS_Grade: null });
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: obtainmarks, ESTMSS_Marks: null, ESTMSS_Grade: null });
                            }

                        }
                        if (flag === "false") {
                            values[s_subj_id][s_exm_id][stu_id] = "";
                            swal('Entered value cant be out of master setting...!');
                        }
                    }
                    else {
                        var totalMarks = 0;
                        totalMarks = Number(obj_ssse.eycessS_MarksEntryMax);
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
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == obj_student.ismS_Id && yet.EMSS_Id == s_subj_id && yet.EMSE_Id == s_exm_id) {
                                        yet.ESTMSS_Marks = obtainmarks;
                                        yet.ESTMSS_Flg = null;
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Marks: obtainmarks, ESTMSS_Grade: null, ESTMSS_Flg: null });
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({
                                    AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, EMSE_Id: s_exm_id,
                                    ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Marks: obtainmarks, ESTMSS_Grade: null, ESTMSS_Flg: null
                                });
                            }
                        }
                    }
                }
            }
        };

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
                            if (ddddd.AMST_Id == stu.amsT_Id) {
                                var marksflagnew = ddddd.ESTMSS_Flg;
                                if (marksflagnew != undefined && marksflagnew != null && marksflagnew != "") {
                                    if (marksflagnew.match(/[a-z]/i)) {
                                        marksflag = ddddd.ESTMSS_Flg;
                                        ddddd.ESTMSS_Marks = 0;
                                    } else {
                                        // marks += ddddd.ESTMSS_Marks;
                                    }
                                }
                                marks += ddddd.ESTMSS_Marks;
                            }
                        });

                        stu.estM_Marks = marks;
                        stu.estM_Flg = marksflag;

                        main_save_list.push({ AMST_Id: stu.amsT_Id, AMST_FirstName: stu.amsT_FirstName, AMST_AdmNo: stu.amsT_AdmNo, AMAY_RollNo: stu.amaY_RollNo, ISMS_SubjectName: stu.ismS_SubjectName, ISMS_Id: stu.ismS_Id, EYCES_MaxMarks: stu.eyceS_MaxMarks, EYCES_MarksEntryMax: stu.eyceS_MarksEntryMax, EYCES_MinMarks: stu.eyceS_MinMarks, ESTM_Marks: stu.estM_Marks, ESTM_Flg: stu.estM_Flg });
                    }
                });

                if (main_save_list.length > 0) {
                    var data = {
                        main_save_list: main_save_list,
                        Temp_subs_marks_list: Temp_subs_marks_list,
                        "ASMS_Id": ASMS_Id,
                        "ASMCL_Id": ASMCL_Id,
                        "ASMAY_Id": ASMAY_Id,
                        "EME_Id": EME_Id,
                        "ISMS_Id": ISMS_Id,
                        "EYCES_MarksGradeEntryFlg": $scope.eyceS_MarksGradeEntryFlg,
                        "EYCES_SubSubjectFlg": $scope.eyceS_SubSubjectFlg,
                        "EYCES_SubExamFlg": $scope.eyceS_SubExamFlg,
                        "IVRMMAP_AddFlg": $scope.MarkCalculation,
                        "Pagename": "School"
                    };
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    };
                    apiService.create("MarksEntryHHS/SaveMarks", data).then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Data Saved Successfully');
                        }
                        else if (promise.returnval == false) {
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


        //ONLY FOR SUBSUBJECT APPLICABLE
        $scope.map_marks_SS = function (saved_studentList, saved_ss_list) {
            angular.forEach(saved_studentList, function (stu_s) {
                angular.forEach($scope.temp_student_list_SS, function (stu_m) {
                    if (stu_m.amsT_Id == stu_s.amsT_Id) {
                        stu_m.selected_ss = true;
                        $scope.optionToggled_SS();
                        stu_m.ESTMSS_Marks = [];
                        angular.forEach($scope.subject_subsubjects_details, function (ss) {

                            if (stu_m.ESTMSS_Marks[ss.emsS_Id] == undefined) {
                                stu_m.ESTMSS_Marks[ss.emsS_Id] = [];
                            }
                            angular.forEach(saved_ss_list, function (ss_s) {
                                if (ss_s.emsS_Id == ss.emsS_Id) {
                                    if (ss_s.estM_Id == stu_s.estM_Id) {
                                        if ($scope.eyceS_MarksGradeEntryFlg === 'M') {

                                            if ((ss_s.estmsS_Flg !== null && ss_s.estmsS_Flg !== "") && (ss_s.estmsS_Marks === null || ss_s.estmsS_Marks === 0)) {
                                                stu_m.ESTMSS_Marks[ss_s.emsS_Id][stu_m.amsT_Id] = ss_s.estmsS_Flg.toString();
                                            }
                                            else if ((ss_s.estmsS_Flg === null || ss_s.estmsS_Flg === "") && (ss_s.estmsS_Marks !== null)) {
                                                var flagss = 0;
                                                var mraks = '' + ss_s.estmsS_Marks + '';
                                                if (mraks.match(/[.]/i)) {
                                                    var sliptmarks = mraks.split('.');
                                                    if (sliptmarks[0] >= 1 && sliptmarks[0] < 10) {
                                                        flagss = 1;
                                                    } else {
                                                        flagss = 0;
                                                    }
                                                } else {
                                                    if (ss_s.estmsS_Marks >= 1 && ss_s.estmsS_Marks < 10) {
                                                        flagss = 1;
                                                    } else {
                                                        flagss = 0;
                                                    }
                                                }
                                                if (flagss === 1) {
                                                    stu_m.ESTMSS_Marks[ss_s.emsS_Id][stu_m.amsT_Id] = '0' + ss_s.estmsS_Marks.toString();
                                                } else {
                                                    stu_m.ESTMSS_Marks[ss_s.emsS_Id][stu_m.amsT_Id] = ss_s.estmsS_Marks.toString();
                                                }
                                                //stu_m.ESTMSS_Marks[ss_s.emsS_Id][stu_m.amsT_Id] = ss_s.estmsS_Marks.toString();
                                            }
                                        } else {
                                            if ((ss_s.estmsS_Flg !== null && ss_s.estmsS_Flg !== "") && (ss_s.estmsS_Marks === null || ss_s.estmsS_Marks === 0)) {
                                                stu_m.ESTMSS_Marks[ss_s.emsS_Id][stu_m.amsT_Id] = ss_s.estmsS_Flg.toString();
                                            } else {
                                                stu_m.ESTMSS_Marks[ss_s.emsS_Id][stu_m.amsT_Id] = ss_s.estmsS_Grade.toString();
                                            }
                                        }

                                        $scope.valid_marks_SS(ss, ss.emsS_Id, stu_m.ESTMSS_Marks, stu_m.amsT_Id, stu_m);
                                    }
                                }
                            });
                        });
                    }
                });
            });
        };

        $scope.toggleAll_SS = function (stas) {
            var toggleStatus = stas;
            angular.forEach($scope.temp_student_list_SS, function (itm) {
                itm.selected_ss = toggleStatus;
            });
        };

        $scope.optionToggled_SS = function () {
            $scope.all_ss = $scope.temp_student_list_SS.every(function (itm) { return itm.selected_ss; });
        };

        $scope.valid_marks_SS = function (obj_ss, s_subj_id, values, stu_id, obj_student) {
            var obtainmarks = values[s_subj_id][stu_id];
            if ($scope.eyceS_MarksGradeEntryFlg === "G") {

                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                    var flag = "false";
                    if (obtainmarks.match(/[a-z]/i)) {
                        for (var i = 0; i < $scope.gradname.length; i++) {
                            if ($scope.gradname[i].emgD_Name.toUpperCase() === obtainmarks.toUpperCase()) {
                                flag = "true";

                                if (Temp_subs_marks_list.length > 0) {
                                    var al_cnt = 0;
                                    angular.forEach(Temp_subs_marks_list, function (yet) {
                                        if (yet.AMST_Id == stu_id && yet.ISMS_Id == obj_student.ismS_Id && yet.EMSS_Id == s_subj_id) {
                                            yet.ESTMSS_Grade = obtainmarks;
                                            yet.ESTMSS_Marks = null;
                                            yet.ESTMSS_Flg = null;
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: null, ESTMSS_Marks: null, ESTMSS_Grade: obtainmarks });
                                    }
                                }
                                else if (Temp_subs_marks_list.length == 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: null, ESTMSS_Marks: null, ESTMSS_Grade: obtainmarks });
                                }
                            }
                        }
                        if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                            flag = "true";

                            if (Temp_subs_marks_list.length > 0) {
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == obj_student.ismS_Id && yet.EMSS_Id == s_subj_id) {
                                        yet.ESTMSS_Flg = obtainmarks;
                                        yet.ESTMSS_Marks = null;
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: obtainmarks, ESTMSS_Marks: null, ESTMSS_Grade: null });
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: obtainmarks, ESTMSS_Marks: null, ESTMSS_Grade: null });
                            }
                        }
                    }
                    if (flag === "false") {
                        values[s_subj_id][stu_id] = "";
                        swal('Entered Grade cant be out of master setting...!');
                    }
                }
            }
            else {
                var flag = "false";
                var obtainmarks = values[s_subj_id][stu_id];
                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                    if (obtainmarks.match(/[a-z]/i)) {
                        if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                            flag = "true";
                            if (Temp_subs_marks_list.length > 0) {
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == obj_student.ismS_Id && yet.EMSS_Id == s_subj_id) {
                                        yet.ESTMSS_Flg = obtainmarks;
                                        yet.ESTMSS_Marks = null;
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: obtainmarks, ESTMSS_Marks: null, ESTMSS_Grade: null });
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: obtainmarks, ESTMSS_Marks: null, ESTMSS_Grade: null });
                            }

                        }
                        if (flag == "false") {
                            values[s_subj_id][stu_id] = "";
                            swal('Entered value cant be out of master setting...!');
                        }
                    }
                    else {
                        var totalMarks = 0;
                        totalMarks = Number(obj_ss.eycessS_MarksEntryMax);
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
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == obj_student.ismS_Id && yet.EMSS_Id == s_subj_id) {
                                        yet.ESTMSS_Marks = obtainmarks;
                                        yet.ESTMSS_Flg = null;
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Marks: obtainmarks, ESTMSS_Grade: null, ESTMSS_Flg: null });
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSS_Id: s_subj_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Marks: obtainmarks, ESTMSS_Grade: null, ESTMSS_Flg: null });
                            }
                        }
                    }
                }
            }
        };

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
                        if ($scope.eyceS_SubSubjectFlg) {
                            var others_cnt = 0;
                            var ESTM_Flg = "";
                            var ESTM_Grade = "";
                            var subject_max_marks = stu.eyceS_MaxMarks;
                            var subject_marks_entry_for = stu.eyceS_MarksEntryMax;
                            var sum_s_sub_max_marks = 0;
                            var sum_s_sub_obt_marks = 0;
                            var ESTM_Marks = 0;
                            var sum_s_exm_obt_marks = 0;

                            angular.forEach($scope.subject_subsubjects_details, function (s_ss) {
                                sum_s_sub_max_marks += s_ss.eycessS_MaxMarks;
                                if (stu.ESTMSS_Marks[s_ss.emsS_Id][stu.amsT_Id].match(/[a-z]/i)) {
                                    others_cnt += 1;

                                    if (stu.ESTMSS_Marks[s_ss.emsS_Id][stu.amsT_Id] === "AB" || stu.ESTMSS_Marks[s_ss.emsS_Id][stu.amsT_Id] === "M"
                                        || stu.ESTMSS_Marks[s_ss.emsS_Id][stu.amsT_Id] === "L" || stu.ESTMSS_Marks[s_ss.emsS_Id][stu.amsT_Id] === "OD") {
                                        ESTM_Flg = stu.ESTMSS_Marks[s_ss.emsS_Id][stu.amsT_Id];
                                    } else {
                                        ESTM_Grade = stu.ESTMSS_Marks[s_ss.emsS_Id][stu.amsT_Id];
                                    }
                                }
                                else {
                                    sum_s_sub_obt_marks += Number(stu.ESTMSS_Marks[s_ss.emsS_Id][stu.amsT_Id]);
                                }
                            });

                            //if (stu.eyceS_MaxMarks == stu.eyceS_MarksEntryMax) {
                            //    var ratio_s_sub = (sum_s_sub_max_marks / subject_max_marks);
                            //    var ESTM_Marks = sum_s_sub_obt_marks;
                            //    stu.estM_Marks = ESTM_Marks;
                            //    stu.estM_Flg = ESTM_Flg;
                            //    stu.estM_Grade = ESTM_Grade;
                            //}
                            //else if (stu.eyceS_MaxMarks > stu.eyceS_MarksEntryMax) {

                            //    var ratio_s_sub = (sum_s_sub_max_marks / subject_marks_entry_for);
                            //    var ESTM_Marks = sum_s_sub_obt_marks;
                            //    stu.estM_Marks = ESTM_Marks;
                            //    stu.estM_Flg = ESTM_Flg;
                            //    stu.estM_Grade = ESTM_Grade;
                            //}

                            stu.estM_Marks = sum_s_sub_obt_marks;
                            stu.estM_Flg = ESTM_Flg;
                            stu.estM_Grade = ESTM_Grade;
                        }

                        main_save_list.push({ AMST_Id: stu.amsT_Id, AMST_FirstName: stu.amsT_FirstName, AMST_AdmNo: stu.amsT_AdmNo, AMAY_RollNo: stu.amaY_RollNo, ISMS_SubjectName: stu.ismS_SubjectName, ISMS_Id: stu.ismS_Id, EYCES_MaxMarks: stu.eyceS_MaxMarks, EYCES_MarksEntryMax: stu.eyceS_MarksEntryMax, EYCES_MinMarks: stu.eyceS_MinMarks, ESTM_Marks: stu.estM_Marks, ESTM_Flg: stu.estM_Flg, ESTM_Grade: stu.estM_Grade });
                    }
                });

                if (main_save_list.length > 0) {
                    var data = {
                        main_save_list: main_save_list,
                        Temp_subs_marks_list: Temp_subs_marks_list,
                        "ASMS_Id": ASMS_Id,
                        "ASMCL_Id": ASMCL_Id,
                        "ASMAY_Id": ASMAY_Id,
                        "EME_Id": EME_Id,
                        "ISMS_Id": ISMS_Id,
                        "EYCES_MarksGradeEntryFlg": $scope.eyceS_MarksGradeEntryFlg,
                        "EYCES_SubSubjectFlg": $scope.eyceS_SubSubjectFlg,
                        "EYCES_SubExamFlg": $scope.eyceS_SubExamFlg,
                        "IVRMMAP_AddFlg": $scope.MarkCalculation,
                        "Pagename": "School"
                    };
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    };
                    apiService.create("MarksEntryHHS/SaveMarks", data).then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Data Saved Successfully');
                        }
                        else if (promise.returnval == false) {
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


        // ONLY FOR SUBEXAM APPLICABLE
        $scope.map_marks_SE = function (saved_studentList, saved_se_list) {
            angular.forEach(saved_studentList, function (stu_s) {
                angular.forEach($scope.temp_student_list_SE, function (stu_m) {
                    if (stu_m.amsT_Id == stu_s.amsT_Id) {
                        stu_m.selected_se = true;
                        $scope.optionToggled_SE();
                        stu_m.ESTMSS_Marks = [];

                        angular.forEach($scope.subject_subexams_details, function (se) {
                            if (stu_m.ESTMSS_Marks[se.emsE_Id] === undefined) {
                                stu_m.ESTMSS_Marks[se.emsE_Id] = [];
                            }
                            angular.forEach(saved_se_list, function (se_s) {
                                if (se_s.emsE_Id == se.emsE_Id) {
                                    if (se_s.estM_Id == stu_s.estM_Id) {
                                        if ((se_s.estmsS_Flg != null && se_s.estmsS_Flg != "") && (se_s.estmsS_Marks == null || se_s.estmsS_Marks === 0)) {
                                            stu_m.ESTMSS_Marks[se_s.emsE_Id][stu_m.amsT_Id] = se_s.estmsS_Flg.toString();
                                        }
                                        else if ((se_s.estmsS_Flg == null || se_s.estmsS_Flg == "") && se_s.estmsS_Marks != null) {
                                            var flagse = 0;
                                            var mraks = '' + se_s.estmsS_Marks + '';
                                            if (mraks.match(/[.]/i)) {
                                                var sliptmarks = mraks.split('.');
                                                if (sliptmarks[0] >= 1 && sliptmarks[0] < 10) {
                                                    flagse = 1;
                                                } else {
                                                    flagse = 0;
                                                }
                                            } else {
                                                if (se_s.estmsS_Marks >= 1 && se_s.estmsS_Marks < 10) {
                                                    flagse = 1;
                                                } else {
                                                    flagse = 0;
                                                }
                                            }
                                            if (flagse === 1) {
                                                stu_m.ESTMSS_Marks[se_s.emsE_Id][stu_m.amsT_Id] = '0' + se_s.estmsS_Marks.toString();
                                            } else {
                                                stu_m.ESTMSS_Marks[se_s.emsE_Id][stu_m.amsT_Id] = se_s.estmsS_Marks.toString();
                                            }
                                        }
                                        $scope.valid_marks_SE(se, se.emsE_Id, stu_m.ESTMSS_Marks, stu_m.amsT_Id, stu_m);
                                    }
                                }
                            });
                        });
                    }
                });
            });
        };

        $scope.toggleAll_SE = function (stas) {
            var toggleStatus = stas;
            angular.forEach($scope.temp_student_list_SE, function (itm) {
                itm.selected_se = toggleStatus;
            });
        };

        $scope.optionToggled_SE = function () {
            $scope.all_se = $scope.temp_student_list_SE.every(function (itm) { return itm.selected_se; });
        };

        $scope.valid_marks_SE = function (obj_se, s_exm_id, values, stu_id, obj_student) {
            if ($scope.eyceS_MarksGradeEntryFlg == "G") {
                var flag = "false";
                for (var i = 0; i < $scope.gradname.length; i++) {
                    if ($scope.gradname[i].emgD_Name.toUpperCase() == obtainmarks.toUpperCase()) {
                        flag = "true";
                    }
                }
                if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L"
                    || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                    flag = "true";
                }

                if (flag === "false") {
                    row.entity.obtainmarks = 0;
                    swal('Entered Grade cant be out of master setting...!');
                }
            }
            else {
                var flag = "false";
                var obtainmarks = values[s_exm_id][stu_id];
                if (obtainmarks !== undefined && obtainmarks !== null && obtainmarks !== "") {
                    if (obtainmarks.match(/[a-z]/i)) {
                        if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M" || obtainmarks.toUpperCase() === "OD") {
                            flag = "true";
                            if (Temp_subs_marks_list.length > 0) {
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == obj_student.ismS_Id && yet.EMSE_Id == s_exm_id) {
                                        yet.ESTMSS_Flg = obtainmarks;
                                        yet.ESTMSS_Marks = null;
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt == 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: obtainmarks, ESTMSS_Marks: null, ESTMSS_Grade: null });
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Flg: obtainmarks, ESTMSS_Marks: null, ESTMSS_Grade: null });
                            }

                        }
                        if (flag == "false") {
                            values[s_exm_id][stu_id] = "";
                            swal('Entered value cant be out of master setting...!');
                        }
                    }
                    else {
                        var totalMarks = 0;
                        totalMarks = obj_se.eycessS_MarksEntryMax;
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
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == obj_student.ismS_Id && yet.EMSE_Id == s_exm_id) {
                                        yet.ESTMSS_Marks = obtainmarks;
                                        yet.ESTMSS_Flg = null;
                                        al_cnt += 1;
                                    }
                                });
                                if (al_cnt === 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Marks: obtainmarks, ESTMSS_Grade: null, ESTMSS_Flg: null });
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: obj_student.ismS_Id, EMSE_Id: s_exm_id, ESTMSS_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTMSS_Marks: obtainmarks, ESTMSS_Grade: null, ESTMSS_Flg: null });
                            }
                        }
                    }
                }
            }
        };

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
                        if ($scope.eyceS_SubExamFlg) {
                            var others_cnt = 0;
                            var ESTM_Flg = "";
                            var ESTM_Grade = "";
                            var subject_max_marks = stu.eyceS_MaxMarks;
                            var subject_marks_entry_for = stu.eyceS_MarksEntryMax;
                            var sum_s_exm_max_marks = 0;
                            var sum_s_exm_obt_marks = 0;
                            var ESTM_Marks = 0;
                            angular.forEach($scope.subject_subexams_details, function (s_se) {
                                sum_s_exm_max_marks += s_se.eycessE_MaxMarks;
                                if (stu.ESTMSS_Marks[s_se.emsE_Id][stu.amsT_Id].match(/[a-z]/i)) {
                                    others_cnt += 1;
                                    //ESTM_Flg = stu.ESTMSS_Marks[s_se.emsE_Id][stu.amsT_Id];
                                    if (stu.ESTMSS_Marks[s_se.emsE_Id][stu.amsT_Id] === "AB" || stu.ESTMSS_Marks[s_se.emsE_Id][stu.amsT_Id] === "M"
                                        || stu.ESTMSS_Marks[s_se.emsE_Id][stu.amsT_Id] === "L" || stu.ESTMSS_Marks[s_se.emsE_Id][stu.amsT_Id] === "OD") {
                                        ESTM_Flg = stu.ESTMSS_Marks[s_se.emsE_Id][stu.amsT_Id];
                                    } else {
                                        ESTM_Grade = stu.ESTMSS_Marks[s_se.emsS_Id][stu.amsT_Id];
                                    }
                                }
                                else {
                                    sum_s_exm_obt_marks += Number(stu.ESTMSS_Marks[s_se.emsE_Id][stu.amsT_Id]);
                                }
                            });

                            //if (stu.eyceS_MaxMarks == stu.eyceS_MarksEntryMax) {
                            //    var ratio_s_sub = (sum_s_exm_max_marks / subject_max_marks);
                            //    var ESTM_Marks = sum_s_exm_obt_marks / ratio_s_sub;
                            //    stu.estM_Marks = ESTM_Marks;
                            //    stu.estM_Flg = ESTM_Flg;
                            //}
                            //else if (stu.eyceS_MaxMarks > stu.eyceS_MarksEntryMax) {
                            //    var ratio_s_sub = (sum_s_exm_max_marks / subject_marks_entry_for);
                            //    var ESTM_Marks = sum_s_exm_obt_marks / ratio_s_sub;
                            //    stu.estM_Marks = ESTM_Marks;
                            //    stu.estM_Flg = ESTM_Flg;
                            //}

                            stu.estM_Marks = sum_s_exm_obt_marks;
                            stu.estM_Flg = ESTM_Flg;
                            stu.estM_Grade = ESTM_Grade
                        }

                        main_save_list.push({
                            AMST_Id: stu.amsT_Id, AMST_FirstName: stu.amsT_FirstName, AMST_AdmNo: stu.amsT_AdmNo, AMAY_RollNo: stu.amaY_RollNo, ISMS_SubjectName: stu.ismS_SubjectName, ISMS_Id: stu.ismS_Id, EYCES_MaxMarks: stu.eyceS_MaxMarks, EYCES_MarksEntryMax: stu.eyceS_MarksEntryMax, EYCES_MinMarks: stu.eyceS_MinMarks, ESTM_Marks: stu.estM_Marks, ESTM_Flg: stu.estM_Flg,
                            ESTM_Grade: stu.estM_Grade
                        });
                    }
                });

                if (main_save_list.length > 0) {
                    var data = {
                        main_save_list: main_save_list,
                        Temp_subs_marks_list: Temp_subs_marks_list,
                        "ASMS_Id": ASMS_Id,
                        "ASMCL_Id": ASMCL_Id,
                        "ASMAY_Id": ASMAY_Id,
                        "EME_Id": EME_Id,
                        "ISMS_Id": ISMS_Id,
                        "EYCES_MarksGradeEntryFlg": $scope.eyceS_MarksGradeEntryFlg,
                        "EYCES_SubSubjectFlg": $scope.eyceS_SubSubjectFlg,
                        "EYCES_SubExamFlg": $scope.eyceS_SubExamFlg,
                        "IVRMMAP_AddFlg": $scope.MarkCalculation,
                        "Pagename": "School"
                    };
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    };
                    apiService.create("MarksEntryHHS/SaveMarks", data).then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Data Saved Successfully');
                        }
                        else if (promise.returnval == false) {
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


        // COMMON FUNCTIONS
        $scope.clear = function () {
            $state.reload();
            $scope.BindData();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;//|| field.$dirty
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.get_evalistt = function () {
            $scope.alltask = $scope.documentList.every(function (options) {
                return options.selectedd2;
            });
        };
    }
})();