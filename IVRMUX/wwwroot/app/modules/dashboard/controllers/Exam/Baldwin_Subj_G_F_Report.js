(function () {
    'use strict';

    angular.module('app').controller('Baldwin_Subj_G_F_ReportController', Baldwin_Subj_G_F_ReportController);

    Baldwin_Subj_G_F_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout'];

    function Baldwin_Subj_G_F_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Baldwin_Subj_G_F_Report';

        activate();

        function activate() { }

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != undefined && admfigsettings != null) {
            var logopath = "";
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
            $scope.imgname = logopath;
        }

        $scope.BindData = function () {            
            apiService.getDATA("Baldwin_Subj_G_F_Report/Getdetails").then(function (promise) {
                    $scope.year_list = promise.yearlist;
                });
        };
        $scope.get_classes = function () {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("Baldwin_Subj_G_F_Report/get_classes", data).then(function (promise) {
                $scope.class_list = promise.classlist;
                $scope.ASMCL_Id = "";
                $scope.ASMS_Id = "";
                $scope.AMST_Id = "";
                $scope.section_list = [];
                $scope.student_list = [];
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.student_marks = [];
            });

        };
        $scope.get_sections = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("Baldwin_Subj_G_F_Report/get_sections", data).then(function (promise) {
                $scope.section_list = promise.sectionlist;
                $scope.ASMS_Id = "";
                $scope.AMST_Id = "";
                $scope.student_list = [];
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.student_marks = [];
            });
        };
        $scope.get_students = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("Baldwin_Subj_G_F_Report/get_students", data).then(function (promise) {
                $scope.student_list = promise.studentlist;
                $scope.AMST_Id = "";
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.student_marks = [];
            });
        };
        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.get_report = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id
                    //  "AMST_Id": $scope.AMST_Id
                };
                apiService.create("Baldwin_Subj_G_F_Report/get_report", data).then(function (promise) {
                    if (promise.examlist != null && promise.subjectlist != null && promise.examlist.length > 0 && promise.subjectlist.length > 0) {
                        $scope.exam_list = promise.examlist;
                        $scope.subject_list = promise.subjectlist;
                        $scope.student_marks = promise.studentmarks;
                        angular.forEach($scope.year_list, function (y) {
                            if (y.asmaY_Id == $scope.ASMAY_Id) {
                                $scope.Year_Name = y.asmaY_Year;
                            }
                        });
                        angular.forEach($scope.class_list, function (c) {
                            if (c.asmcL_Id == $scope.ASMCL_Id) {
                                $scope.Class_Name = c.asmcL_ClassName;
                            }
                        });
                        angular.forEach($scope.section_list, function (s) {
                            if (s.asmS_Id == $scope.ASMS_Id) {
                                $scope.Section_Name = s.asmC_SectionName;
                            }
                        });
                        //angular.forEach($scope.student_list, function (s) {
                        //    if (s.amsT_Id == $scope.AMST_Id) {
                        //        $scope.Student_Name = s.amsT_FirstName;
                        //        $scope.Student_AdmNo = s.amsT_AdmNo;
                        //        $scope.Student_RollNo = s.amaY_RollNo;
                        //        $scope.Student_dob = s.amsT_DOB;
                        //    }
                        //})
                        if (promise.classteacher != null && promise.classteacher.length > 0) {
                            $scope.Class_Teacher_Name = promise.classteacher[0].hrmE_EmployeeFirstName;
                        }


                        $scope.exam_subjectwise_details = promise.examsubjectwise_details;
                        $scope.exam_process_details = promise.process_examdetails;
                        //$scope.total_Exams = 0;
                        //angular.forEach($scope.exam_process_details, function (ttotal) {
                        //    $scope.total_Exams += ttotal.estmP_TotalObtMarks;
                        //})
                        angular.forEach($scope.exam_list, function (exm) {
                            var temp_subdetails = [];
                            angular.forEach(promise.examsubjectwise_details, function (sd) {
                                if (sd.eycE_Id == exm.eycE_Id && sd.eyceS_AplResultFlg) {
                                    temp_subdetails.push(sd);
                                }
                            });
                            var max_sub_marks = Math.max.apply(Math, temp_subdetails.map(function (item) { return item.eyceS_MaxMarks; }));
                            exm.marks = max_sub_marks;
                        });
                        $scope.total_max_sub_marks = 0;
                        angular.forEach($scope.exam_list, function (ex) {
                            $scope.total_max_sub_marks += ex.marks;
                        });
                        angular.forEach($scope.subject_list, function (sub) {
                            var temp_subdetails = [];
                            angular.forEach(promise.examsubjectwise_details, function (sd) {
                                if (sd.ismS_Id == sub.ismS_Id) {
                                    temp_subdetails.push(sd);
                                }
                            });
                            var applicable_flag = Math.max.apply(Math, temp_subdetails.map(function (item) { return item.eyceS_AplResultFlg; }));
                            sub.flag = applicable_flag;
                        });
                        //for only applicable subjects
                        var Applicable_subjects = [];
                        angular.forEach($scope.subject_list, function (sub) {
                            if (sub.flag) {
                                Applicable_subjects.push(sub);
                            }
                        });
                        $scope.subject_list = Applicable_subjects;
                        console.log($scope.subject_list);
                        //for  Elective subject groups
                        $scope.elective_subj_grp_details = promise.elective_subj_grpdetails;
                        $scope.elective_subj_grp_subjects = promise.elective_subj_grpsubjects;
                        console.log($scope.elective_subj_grp_details);
                        console.log($scope.elective_subj_grp_subjects);
                        var temp_subjectsandgrps = [];
                        angular.forEach($scope.elective_subj_grp_details, function (s_grp) {
                            var subjects = [];
                            angular.forEach($scope.elective_subj_grp_subjects, function (grp_subjs) {
                                if (grp_subjs.emG_Id == s_grp.emG_Id) {
                                    subjects.push({ ismS_Id: grp_subjs.ismS_Id, emG_Id: s_grp.emG_Id });
                                }

                            });
                            s_grp.subjects = subjects;
                        });
                        console.log($scope.elective_subj_grp_details);
                        var temp_elective_grp_subjects = [];
                        angular.forEach($scope.subject_list, function (sub) {
                            var subj_cnt = 0;
                            angular.forEach($scope.elective_subj_grp_subjects, function (grp_subjs) {
                                if (sub.ismS_Id == grp_subjs.ismS_Id) {
                                    angular.forEach($scope.elective_subj_grp_details, function (grp_s) {
                                        if (grp_subjs.emG_Id == grp_s.emG_Id) {
                                            //sub.emG_Id = grp_s.emG_Id;
                                            //sub.emG_GroupName = grp_s.emG_GroupName;
                                            subj_cnt += 1;
                                            var al_grp_cnt = 0;
                                            angular.forEach(temp_elective_grp_subjects, function (sub_grp) {
                                                if (sub_grp.emG_Id == grp_s.emG_Id)
                                                    al_grp_cnt += 1;
                                            });
                                            if (al_grp_cnt == 0) {
                                                temp_elective_grp_subjects.push({ emG_Id: grp_s.emG_Id, Name: grp_s.emG_GroupName, subjects: grp_s.subjects, flag: sub.flag });
                                            }
                                        }
                                    });
                                }

                            });
                            if (subj_cnt == 0) {
                                temp_elective_grp_subjects.push(sub);
                            }
                        });
                        console.log(temp_elective_grp_subjects);
                        $scope.temp_subject_list_new = temp_elective_grp_subjects;
                        //var total_array_s = [];
                        //angular.forEach($scope.subject_list, function (subj) {
                        //    if (subj.flag) {
                        //        var subj_total = 0;
                        //        angular.forEach($scope.student_marks, function (stu_marks) {
                        //            if (stu_marks.ismS_Id == subj.ismS_Id) {
                        //                if (stu_marks.estmpS_PassFailFlg != 'AB' && stu_marks.estmpS_PassFailFlg != 'M' && stu_marks.estmpS_PassFailFlg != 'L') {
                        //                    subj_total += Number(stu_marks.estmpS_ObtainedMarks);
                        //                }
                        //            }

                        //        })
                        //        total_array_s.push({ ismS_Id: subj.ismS_Id, total_s: subj_total });
                        //    }
                        //})
                        //$scope.Total_Array_S = total_array_s;

                        //$scope.stud_work_attendence = promise.work_attendence;
                        //$scope.stud_present_attendence = promise.present_attendence;

                        //for Subject Grouping
                        $scope.subject_grps_details = promise.subject_grpsdetails;
                        $scope.subject_grps_exm_details = promise.subject_grps_exmdetails;
                        $scope.subject_grps_subj_details = promise.subject_grps_subjdetails;


                        angular.forEach(promise.subject_grpsdetails, function (s_grp) {
                            if (s_grp.esG_ExamPromotionFlag == 'IE') {
                                //for exams
                                angular.forEach(promise.subject_grps_exmdetails, function (s_grp_exm) {
                                    if (s_grp_exm.esG_Id == s_grp.esG_Id) {
                                        angular.forEach($scope.exam_list, function (e) {
                                            if (e.emE_Id == s_grp_exm.emE_Id)
                                                e.grp_flag = true;
                                        });
                                    }
                                });
                                //for subjects
                                angular.forEach(promise.subject_grps_subjdetails, function (s_grp_subj) {
                                    if (s_grp_subj.esG_Id == s_grp.esG_Id) {
                                        angular.forEach($scope.subject_list, function (subj) {
                                            if (subj.ismS_Id == s_grp_subj.ismS_Id) {
                                                subj.grp_flag = true;
                                                subj.esG_Id = s_grp.esG_Id;
                                                subj.esG_SubjectGroupName = s_grp.esG_SubjectGroupName;
                                            }

                                        });
                                    }
                                });
                            }
                        });

                        console.log($scope.exam_list);
                        console.log($scope.subject_list);
                        //for exams
                        var temp_examlist = [];
                        angular.forEach($scope.exam_list, function (e1) {
                            temp_examlist.push({ emE_Id: e1.emE_Id, emE_ExamName: e1.emE_ExamName, emE_FinalExamFlag: e1.emE_FinalExamFlag, grp_flag: e1.grp_flag, total_flag: false, eycE_Id: e1.eycE_Id });
                            if (e1.grp_flag) {
                                temp_examlist.push({ emE_Id: e1.emE_Id, emE_ExamName: '', emE_FinalExamFlag: e1.emE_FinalExamFlag, grp_flag: e1.grp_flag, total_flag: true, eycE_Id: e1.eycE_Id });//emE_ExamName: 'Group Total',
                            }
                        });
                        console.log(temp_examlist);
                        $scope.temp_examlist = temp_examlist;
                        //for subjects
                        var temp_subj_grp_Subjects = [];
                        //for applicable
                        angular.forEach($scope.subject_list, function (subj) {
                            if (subj.flag) {
                                if (subj.grp_flag == undefined || !subj.grp_flag) {
                                    temp_subj_grp_Subjects.push({ ismS_Id: subj.ismS_Id, ismS_SubjectName: subj.ismS_SubjectName, flag: subj.flag, grp_flag: false, esG_Id: 0, esG_SubjectGroupName: '' });
                                }
                                else if (subj.grp_flag) {
                                    var temp_grp_subjs = [];
                                    var al_g_s_cnt = 0;
                                    angular.forEach(temp_subj_grp_Subjects, function (s_g) {
                                        if (s_g.esG_Id == subj.esG_Id)
                                            al_g_s_cnt += 1;
                                    });
                                    if (al_g_s_cnt == 0) {
                                        angular.forEach($scope.subject_list, function (su1) {
                                            if (su1.esG_Id == subj.esG_Id) {
                                                temp_grp_subjs.push(su1);
                                            }
                                        })
                                        temp_subj_grp_Subjects.push({ flag: subj.flag, grp_flag: subj.grp_flag, esG_Id: subj.esG_Id, esG_SubjectGroupName: subj.esG_SubjectGroupName, grp_subj: temp_grp_subjs });//ismS_Id: subj.ismS_Id, ismS_SubjectName: subj.ismS_SubjectName, 
                                    }

                                }
                            }
                        });
                        //for non-applicable
                        angular.forEach($scope.subject_list, function (subj) {
                            if (!subj.flag) {
                                if (subj.grp_flag == undefined || !subj.grp_flag) {
                                    temp_subj_grp_Subjects.push({ ismS_Id: subj.ismS_Id, ismS_SubjectName: subj.ismS_SubjectName, flag: subj.flag, grp_flag: false, esG_Id: 0, esG_SubjectGroupName: '' });
                                }
                                else if (subj.grp_flag) {
                                    var temp_grp_subjs = [];
                                    var al_g_s_cnt = 0;
                                    angular.forEach(temp_subj_grp_Subjects, function (s_g) {
                                        if (s_g.esG_Id == subj.esG_Id)
                                            al_g_s_cnt += 1;
                                    });
                                    if (al_g_s_cnt == 0) {
                                        angular.forEach($scope.subject_list, function (su1) {
                                            if (su1.esG_Id == subj.esG_Id) {
                                                temp_grp_subjs.push(su1);
                                            }
                                        });
                                        temp_subj_grp_Subjects.push({ flag: subj.flag, grp_flag: subj.grp_flag, esG_Id: subj.esG_Id, esG_SubjectGroupName: subj.esG_SubjectGroupName, grp_subj: temp_grp_subjs });// ismS_SubjectName: subj.ismS_SubjectName, 
                                    }

                                }
                            }
                        });
                        console.log(temp_subj_grp_Subjects);
                        $scope.temp_subjectlist = temp_subj_grp_Subjects;
                        //for group total marks
                        var temp_student_marks_grpwise = [];
                        angular.forEach($scope.temp_subjectlist, function (s_g) {
                            //if(!s_g.grp_flag)
                            //{
                            //    angular.forEach($scope.student_marks, function (stu_mks) {
                            //        temp_student_marks_grpwise.push(stu_mks);
                            //    })
                            //}
                            if (s_g.grp_flag) {
                                angular.forEach($scope.temp_examlist, function (t_exm) {
                                    if (t_exm.total_flag) {
                                        angular.forEach($scope.student_list, function (stu) {
                                            var subj_grp_marks = 0;
                                            var stu_cnt = 0;
                                            angular.forEach($scope.student_marks, function (stu_mks) {
                                                if (t_exm.emE_Id == stu_mks.emE_Id && stu.amsT_Id == stu_mks.amsT_Id) {
                                                    stu_cnt += 1;
                                                    angular.forEach(s_g.grp_subj, function (grp_subj) {
                                                        if (grp_subj.ismS_Id == stu_mks.ismS_Id) {
                                                            //if(marks.estmpS_PassFailFlg=='AB' || marks.estmpS_PassFailFlg=='M' || marks.estmpS_PassFailFlg=='L')
                                                            //{
                                                            //    //subj_grp_marks += marks.estmpS_ObtainedMarks;
                                                            //} 
                                                            if (stu_mks.estmpS_PassFailFlg != 'AB' && stu_mks.estmpS_PassFailFlg != 'M' && stu_mks.estmpS_PassFailFlg != 'L') {
                                                                subj_grp_marks += stu_mks.estmpS_ObtainedMarks;
                                                            }
                                                        }

                                                    });

                                                }
                                            });
                                            var ESG_GroupMinMarks = 0;
                                            angular.forEach($scope.subject_grps_details, function (grp) {
                                                if (grp.esG_Id == s_g.esG_Id) {
                                                    ESG_GroupMinMarks = grp.esG_GroupMinMarks;
                                                }
                                            });
                                            var ESTMPS_PassFailFlg = "";
                                            subj_grp_marks = subj_grp_marks / s_g.grp_subj.length;
                                            if (ESG_GroupMinMarks > subj_grp_marks) {
                                                ESTMPS_PassFailFlg = "Fail";
                                            }
                                            else if (ESG_GroupMinMarks <= subj_grp_marks) {
                                                ESTMPS_PassFailFlg = "Pass";
                                            }
                                            if (stu_cnt > 0) {
                                                temp_student_marks_grpwise.push({ grp_flag: s_g.grp_flag, esG_Id: s_g.esG_Id, estmpS_ObtainedMarks: subj_grp_marks, estmpS_PassFailFlg: ESTMPS_PassFailFlg, amsT_Id: stu.amsT_Id, emE_Id: t_exm.emE_Id });
                                            }
                                        });

                                    }
                                });
                            }

                        });
                        console.log(temp_student_marks_grpwise);
                        $scope.temp_student_marks = temp_student_marks_grpwise;
                        //for fail count
                        var temp_fail_cnt = [];
                        angular.forEach($scope.student_list, function (stu) {
                            angular.forEach($scope.temp_examlist, function (exm) {
                                if (!exm.total_flag) {
                                    var cnt = 0;
                                    var stu_cnt = 0;
                                    angular.forEach($scope.student_marks, function (stu_mks) {
                                        if (stu_mks.amsT_Id == stu.amsT_Id && stu_mks.emE_Id == exm.emE_Id) {
                                            stu_cnt += 1;
                                            if (stu_mks.estmpS_PassFailFlg == 'Fail') {
                                                //for only applicable to result subjects
                                                angular.forEach($scope.exam_subjectwise_details, function (sd) {
                                                    if (sd.ismS_Id == stu_mks.ismS_Id && sd.eycE_Id == exm.eycE_Id && sd.eyceS_AplResultFlg) {
                                                        cnt += 1;
                                                    }
                                                });
                                            }
                                            //  cnt += 1;
                                        }
                                    });
                                }
                                if (stu_cnt > 0) {
                                    temp_fail_cnt.push({ amsT_Id: stu.amsT_Id, emE_Id: exm.emE_Id, F_Count: cnt });
                                }
                            });
                        });
                        console.log(temp_fail_cnt);
                        $scope.temp_fail_count = temp_fail_cnt;

                        angular.forEach($scope.student_list, function (stud) {

                            $scope.tmparrry = [];
                            //var obj_m = {};
                            //var count_m = 0;
                            angular.forEach($scope.temp_examlist, function (exm) {
                                var obj_m = {};
                                var count_m = 0;
                                if (!exm.total_flag) {
                                    var tmparrry_N = [];
                                    angular.forEach($scope.subject_list, function (sub) {
                                        if (sub.flag) {
                                            var count = 0;
                                            var obj = {};
                                            angular.forEach($scope.student_marks, function (marks) {
                                                if (marks.amsT_Id == stud.amsT_Id && marks.ismS_Id == sub.ismS_Id && marks.emE_Id == exm.emE_Id) {
                                                    count += 1;
                                                    if (marks.estmpS_PassFailFlg == 'AB' || marks.estmpS_PassFailFlg == 'M' || marks.estmpS_PassFailFlg == 'L') {
                                                        obj.estmpS_ObtainedMarks = marks.estmpS_PassFailFlg;
                                                        //obj.color = 'red';
                                                        obj.color = 'pink';
                                                    }
                                                    else if (marks.estmpS_PassFailFlg != 'AB' && marks.estmpS_PassFailFlg != 'M' && marks.estmpS_PassFailFlg != 'L' && marks.estmpS_PassFailFlg == 'Pass') {
                                                        obj.estmpS_ObtainedMarks = marks.estmpS_ObtainedMarks;
                                                    }
                                                    else if (marks.estmpS_PassFailFlg != 'AB' && marks.estmpS_PassFailFlg != 'M' && marks.estmpS_PassFailFlg != 'L' && marks.estmpS_PassFailFlg == 'Fail') {
                                                        obj.estmpS_ObtainedMarks = marks.estmpS_ObtainedMarks;
                                                        //obj.color = 'red';
                                                        obj.color = 'pink';
                                                    }
                                                    obj.estmpS_ObtainedGrade = marks.estmpS_ObtainedGrade;
                                                    //obj.color = 'red';
                                                    if (!exm.emE_FinalExamFlag) {
                                                        obj.color = '';
                                                    }
                                                }
                                            });
                                            if (count == 0) {
                                                obj.estmps_obtainedmarks = "";
                                                obj.estmps_obtainedgrade = "";
                                            }
                                            tmparrry_N.push(obj);
                                        }
                                    })
                                    obj_m.sub_list_NT = tmparrry_N;
                                    angular.forEach($scope.exam_process_details, function (pro_exm) {
                                        if (pro_exm.amsT_Id == stud.amsT_Id && pro_exm.emE_Id == exm.emE_Id) {
                                            obj_m.estmP_TotalObtMarks = pro_exm.estmP_TotalObtMarks;
                                        }
                                    });
                                    angular.forEach($scope.temp_fail_count, function (cnt) {
                                        if (cnt.amsT_Id == stud.amsT_Id && cnt.emE_Id == exm.emE_Id) {
                                            obj_m.F_Count = cnt.F_Count;
                                        }
                                    });
                                }
                                else if (exm.total_flag) {
                                    var tmparrry_T = [];
                                    angular.forEach($scope.temp_subjectlist, function (sub) {
                                        if (sub.flag) {
                                            var count = 0;
                                            var obj = {};
                                            obj.grp_flag = sub.grp_flag;
                                            //obj.col_span = sub.grp_subj.length;
                                            if (!sub.grp_flag) {
                                                angular.forEach($scope.student_marks, function (marks) {
                                                    if (marks.amsT_Id == stud.amsT_Id && marks.ismS_Id == sub.ismS_Id && marks.emE_Id == exm.emE_Id) {
                                                        count += 1;
                                                        if (marks.estmpS_PassFailFlg == 'AB' || marks.estmpS_PassFailFlg == 'M' || marks.estmpS_PassFailFlg == 'L') {
                                                            obj.estmpS_ObtainedMarks = marks.estmpS_PassFailFlg;
                                                            //obj.color = 'red';
                                                            obj.color = 'pink';
                                                        }
                                                        else if (marks.estmpS_PassFailFlg != 'AB' && marks.estmpS_PassFailFlg != 'M' && marks.estmpS_PassFailFlg != 'L' && marks.estmpS_PassFailFlg == 'Pass') {
                                                            obj.estmpS_ObtainedMarks = marks.estmpS_ObtainedMarks;
                                                        }
                                                        else if (marks.estmpS_PassFailFlg != 'AB' && marks.estmpS_PassFailFlg != 'M' && marks.estmpS_PassFailFlg != 'L' && marks.estmpS_PassFailFlg == 'Fail') {
                                                            obj.estmpS_ObtainedMarks = marks.estmpS_ObtainedMarks;
                                                            //obj.color = 'red';
                                                            obj.color = 'pink';
                                                        }
                                                        obj.estmpS_ObtainedGrade = marks.estmpS_ObtainedGrade;
                                                        //obj.color = 'red';
                                                    }
                                                });
                                            }
                                            else if (sub.grp_flag) {
                                                obj.col_span = sub.grp_subj.length;
                                                angular.forEach($scope.temp_student_marks, function (marks) {
                                                    if (marks.amsT_Id == stud.amsT_Id && marks.esG_Id == sub.esG_Id && marks.emE_Id == exm.emE_Id) {
                                                        count += 1;
                                                        if (marks.estmpS_PassFailFlg == 'AB' || marks.estmpS_PassFailFlg == 'M' || marks.estmpS_PassFailFlg == 'L') {
                                                            obj.estmpS_ObtainedMarks = marks.estmpS_PassFailFlg;
                                                            //obj.color = 'red';
                                                            obj.color = 'pink';
                                                        }
                                                        else if (marks.estmpS_PassFailFlg != 'AB' && marks.estmpS_PassFailFlg != 'M' && marks.estmpS_PassFailFlg != 'L' && marks.estmpS_PassFailFlg == 'Pass') {
                                                            obj.estmpS_ObtainedMarks = marks.estmpS_ObtainedMarks;
                                                        }
                                                        else if (marks.estmpS_PassFailFlg != 'AB' && marks.estmpS_PassFailFlg != 'M' && marks.estmpS_PassFailFlg != 'L' && marks.estmpS_PassFailFlg == 'Fail') {
                                                            obj.estmpS_ObtainedMarks = marks.estmpS_ObtainedMarks;
                                                            //obj.color = 'red';
                                                            obj.color = 'pink';
                                                        }
                                                        obj.estmpS_ObtainedGrade = marks.estmpS_ObtainedGrade;
                                                        // obj.color = 'red';
                                                    }
                                                });
                                            }

                                            if (count == 0) {
                                                obj.estmps_obtainedmarks = "";
                                                obj.estmps_obtainedgrade = "";
                                            }
                                            tmparrry_T.push(obj);
                                        }
                                    });
                                    obj_m.sub_list_T = tmparrry_T;
                                }
                                obj_m.emE_Id = exm.emE_Id;
                                obj_m.emE_ExamName = exm.emE_ExamName;
                                obj_m.total_flag = exm.total_flag;
                                $scope.tmparrry.push(obj_m);
                            });

                            stud.sub_list = $scope.tmparrry;
                        });
                        console.log($scope.student_list);
                    }
                    else {
                        swal("Selected Details Not Mapped with Subjects/Exams");
                        $scope.clear();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.Print = function () {
            var innerContents = document.getElementById("Final_Report").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' + '<style type="text/css">        @media print {    table {    page-break-inside: auto; }         tbody {   page-break-inside: avoid;     page-break-after: auto;   }  }    </style>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 300);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.clear = function () {
            $scope.ASMAY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            //  $scope.AMST_Id = "";
            $scope.class_list = [];
            $scope.section_list = [];
            $scope.student_list = [];
            $scope.exam_list = [];
            $scope.subject_list = [];
            $scope.student_marks = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
        $scope.exportToExcel = function (tableIds) {

            var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        };
    }
})();
