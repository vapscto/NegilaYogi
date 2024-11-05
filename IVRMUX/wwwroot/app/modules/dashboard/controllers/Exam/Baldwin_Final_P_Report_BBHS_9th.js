(function () {
    'use strict';

    angular
        .module('app')
        .controller('Baldwin_Final_P_Report_BBHS_9thController', Baldwin_Final_P_Report_BBHS_9thController);

    Baldwin_Final_P_Report_BBHS_9thController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter'];

    function Baldwin_Final_P_Report_BBHS_9thController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Baldwin_Final_P_Report_BBHS_9th';

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != undefined && admfigsettings != null) {
            var logopath = "";
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
            $scope.imgname = logopath;
        }


        $scope.BindData = function () {
            
            apiService.getDATA("Baldwin_Final_P_Report/Getdetails").
       then(function (promise) {

           $scope.year_list = promise.yearlist;
           //   $scope.class_list = promise.classlist;
           //  $scope.section_list = promise.sectionlist;
           //$scope.exsplt = promise.exmstdlist;
       })
        };
        $scope.get_classes = function () {
            
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Baldwin_Final_P_Report/get_classes", data).then(function (promise) {
                $scope.class_list = promise.classlist;
                $scope.ASMCL_Id = "";
                $scope.ASMS_Id = "";
                //$scope.AMST_Id = "";
                $scope.section_list = [];
                $scope.student_list = [];
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.student_marks = [];
            })

        }
        $scope.get_sections = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("Baldwin_Final_P_Report/get_sections", data).then(function (promise) {
                $scope.section_list = promise.sectionlist;
                $scope.ASMS_Id = "";
               // $scope.AMST_Id = "";
                $scope.student_list = [];
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.student_marks = [];
            })
        }
        $scope.get_students = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            }
            apiService.create("Baldwin_Final_P_Report/get_students", data).then(function (promise) {
                $scope.student_list = promise.studentlist;
               // $scope.AMST_Id = "";
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.student_marks = [];
            })
        }
        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        }
        $scope.clear = function () {
            $scope.ASMAY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            //$scope.AMST_Id = "";
            $scope.class_list = [];
            $scope.section_list = [];
            $scope.student_list = [];
            $scope.exam_list = [];
            $scope.subject_list = [];
            $scope.student_marks = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.get_report = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    //"AMST_Id": $scope.AMST_Id
                }
                apiService.create("Baldwin_Final_P_Report/get_report", data).then(function (promise) {
                    if (promise.examlist != null && promise.subjectlist != null && promise.examlist.length > 0 && promise.subjectlist.length > 0) {
                        $scope.exam_list = promise.examlist;
                        $scope.subject_list = promise.subjectlist;
                        $scope.student_marks = promise.studentmarks;
                        angular.forEach($scope.year_list, function (y) {
                            if (y.asmaY_Id == $scope.ASMAY_Id) {
                                $scope.Year_Name = y.asmaY_Year;
                            }
                        })
                        angular.forEach($scope.class_list, function (c) {
                            if (c.asmcL_Id == $scope.ASMCL_Id) {
                                $scope.Class_Name = c.asmcL_ClassName;
                            }
                        })
                        angular.forEach($scope.section_list, function (s) {
                            if (s.asmS_Id == $scope.ASMS_Id) {
                                $scope.Section_Name = s.asmC_SectionName;
                            }
                        })
                        //angular.forEach($scope.student_list, function (s) {
                        //    if (s.amsT_Id == $scope.AMST_Id) {
                        //        $scope.Student_Name = s.amsT_FirstName;
                        //        $scope.Student_AdmNo = s.amsT_AdmNo;
                        //        $scope.Student_RollNo = s.amaY_RollNo;
                        //        $scope.Student_dob = s.amsT_DOB;
                        //    }
                        //})
                        //var total_array_s = [];
                        //angular.forEach($scope.subject_list, function (subj) {
                        //    var subj_total = 0;
                        //    angular.forEach($scope.student_marks, function (stu_marks) {
                        //        if (stu_marks.ismS_Id == subj.ismS_Id)
                        //        {
                        //            if (stu_marks.estmpS_PassFailFlg != 'AB' && stu_marks.estmpS_PassFailFlg != 'M' && stu_marks.estmpS_PassFailFlg != 'L') {
                        //                subj_total += Number(stu_marks.estmpS_ObtainedMarks);
                        //            }
                        //        }

                        //    })
                        //    total_array_s.push({ismS_Id:subj.ismS_Id,total_s:subj_total});
                        //})
                        //$scope.Total_Array_S = total_array_s;

                        //var total_array_e = [];
                        //angular.forEach($scope.exam_list, function (exm) {
                        //    var exm_total = 0;
                        //    angular.forEach($scope.student_marks, function (stu_marks) {
                        //        if (stu_marks.emE_Id == exm.emE_Id) {
                        //            if (stu_marks.estmpS_PassFailFlg != 'AB' && stu_marks.estmpS_PassFailFlg != 'M' && stu_marks.estmpS_PassFailFlg != 'L') {
                        //                exm_total += Number(stu_marks.estmpS_ObtainedMarks);
                        //            }
                        //        }

                        //    })
                        //    total_array_e.push({ emE_Id: exm.emE_Id, total_e: exm_total });
                        //})
                        //$scope.Total_Array_E = total_array_e;
                        if (promise.classteacher != null && promise.classteacher.length > 0) {
                            $scope.Class_Teacher_Name = promise.classteacher[0].hrmE_EmployeeFirstName;
                        }


                        $scope.exam_subjectwise_details = promise.examsubjectwise_details;
                        $scope.exam_process_details = promise.process_examdetails;
                        $scope.total_Exams = 0;
                        angular.forEach($scope.exam_process_details, function (ttotal) {
                            $scope.total_Exams += ttotal.estmP_TotalObtMarks;
                        })
                        //  $scope.max = Math.max.apply(Math,$scope.data.map(function(item){return item.age;}));
                        angular.forEach($scope.exam_list, function (exm) {
                            var temp_subdetails = [];
                            angular.forEach(promise.examsubjectwise_details, function (sd) {
                                if (sd.eycE_Id == exm.eycE_Id && sd.eyceS_AplResultFlg) {
                                    temp_subdetails.push(sd);
                                }
                            })
                            var max_sub_marks = Math.max.apply(Math, temp_subdetails.map(function (item) { return item.eyceS_MaxMarks; }));
                            exm.marks = max_sub_marks;
                        })
                        $scope.total_max_sub_marks = 0;
                        angular.forEach($scope.exam_list, function (ex) {
                            $scope.total_max_sub_marks += ex.marks;
                        })
                        //$scope.exam_list.push({ emE_ExamName: "Total", marks: total_max_sub_marks });
                        //$scope.exam_list.push({ emE_ExamName: "Average", marks: (total_max_sub_marks / $scope.exams_length) });
                        angular.forEach($scope.subject_list, function (sub) {
                            var temp_subdetails = [];
                            angular.forEach(promise.examsubjectwise_details, function (sd) {
                                if (sd.ismS_Id == sub.ismS_Id) {
                                    temp_subdetails.push(sd);
                                }
                            })
                            var applicable_flag = Math.max.apply(Math, temp_subdetails.map(function (item) { return item.eyceS_AplResultFlg; }));
                            sub.flag = applicable_flag;
                        })
                        var total_array_s = [];
                        angular.forEach($scope.subject_list, function (subj) {
                            if (subj.flag) {
                                var subj_total = 0;
                                angular.forEach($scope.student_marks, function (stu_marks) {
                                    if (stu_marks.ismS_Id == subj.ismS_Id) {
                                        if (stu_marks.estmpS_PassFailFlg != 'AB' && stu_marks.estmpS_PassFailFlg != 'M' && stu_marks.estmpS_PassFailFlg != 'L') {
                                            subj_total += Number(stu_marks.estmpS_ObtainedMarks);
                                        }
                                    }

                                })
                                total_array_s.push({ ismS_Id: subj.ismS_Id, total_s: subj_total });
                            }
                        })
                        $scope.Total_Array_S = total_array_s;
                        //var total_array_e = [];
                        //angular.forEach($scope.exam_list, function (exm) {
                        //    var exm_total = 0;
                        //    angular.forEach($scope.student_marks, function (stu_marks) {
                        //        if (stu_marks.emE_Id == exm.emE_Id) {
                        //            angular.forEach($scope.subject_list, function (sub) {
                        //                if(sub.ismS_Id==stu_marks.ismS_Id && sub.flag)
                        //                {
                        //                    if (stu_marks.estmpS_PassFailFlg != 'AB' && stu_marks.estmpS_PassFailFlg != 'M' && stu_marks.estmpS_PassFailFlg != 'L') {
                        //                        exm_total += Number(stu_marks.estmpS_ObtainedMarks);
                        //                    }
                        //                }
                        //            })

                        //        }

                        //    })
                        //    total_array_e.push({ emE_Id: exm.emE_Id, total_e: exm_total });
                        //})
                        //$scope.Total_Array_E = total_array_e;
                        //$scope.Total_of_total = 0;
                        //angular.forEach(total_array_e, function (ttotal) {
                        //    $scope.Total_of_total += ttotal.total_e;
                        //})      
                        //for student wise subjects new
                        $scope.studentwise_subjects = promise.stu_subjects;
                        angular.forEach($scope.student_list, function (stud) {
                            var subjects = [];
                            angular.forEach($scope.studentwise_subjects, function (stud_subj) {
                                if (stud_subj.amsT_Id == stud.amsT_Id) {
                                    angular.forEach($scope.subject_list, function (subj) {
                                        if (subj.ismS_Id == stud_subj.ismS_Id) {
                                            subjects.push(subj);
                                        }
                                    })
                                }

                            })
                            stud.stu_subjects = subjects;
                        })
                        angular.forEach($scope.student_list, function (stud) {
                            var subjects_new = [];
                            angular.forEach($scope.subject_list, function (subj) {
                                angular.forEach(stud.stu_subjects, function (stud_subj) {
                                    if (subj.ismS_Id == stud_subj.ismS_Id) {
                                        subjects_new.push(subj);
                                    }
                                })
                            })

                            stud.stu_subjects = subjects_new;
                        })
                        //for student wise marks new
                        angular.forEach($scope.student_list, function (stud) {
                            var stu_marks = [];
                            angular.forEach($scope.student_marks, function (stud_marks) {
                                if (stud_marks.amsT_Id == stud.amsT_Id) {
                                    stu_marks.push(stud_marks);
                                }

                            })
                            stud.stu_student_marks = stu_marks;
                        })

                        $scope.stud_work_attendence = promise.work_attendence;
                        $scope.stud_present_attendence = promise.present_attendence;
                        //For Promotion
                        
                        $scope.ExmConfig_PromotionFlag = promise.exmConfig_PromotionFlag;
                        $scope.EMP_MarksPerFlg = promise.emP_MarksPerFlg;
                        $scope.promotion_subectdetails = promise.promotion_subectdetails;
                        $scope.prom_subj_groupdetails = promise.prom_subj_groupdetails;
                        $scope.prom_subj_grp_exms = promise.prom_subj_grp_exms;
                        $scope.promotion_student_marks = promise.promotion_stumarks;
                        $scope.promotion_main_marks = promise.promotion_mainmarks;
                        if ($scope.ExmConfig_PromotionFlag) {
                            if ($scope.EMP_MarksPerFlg == 'P') {
                                $scope.temp_headers = [];
                                var final_exam_grps = [];
                                var other_exam_grps = [];
                                angular.forEach($scope.prom_subj_grp_exms, function (e1) {
                                    angular.forEach($scope.exam_list, function (e2) {
                                        if (e2.emE_Id == e1.emE_Id) {
                                            if (e2.emE_FinalExamFlag) {
                                                final_exam_grps.push(e1);
                                            }
                                            else {
                                                other_exam_grps.push(e1);
                                            }
                                        }

                                    })

                                })
                                console.log(final_exam_grps);
                                console.log(other_exam_grps);
                                var grp_ids_other = [];
                                var grp_ids_final = [];
                                angular.forEach(other_exam_grps, function (ot) {
                                    if (grp_ids_other.length == 0) {
                                        grp_ids_other.push(ot.empsG_Id);
                                    }
                                    else if (grp_ids_other.length > 0) {
                                        var al_ct = 0;
                                        angular.forEach(grp_ids_other, function (grp_id) {
                                            if (grp_id == ot.empsG_Id)
                                                al_ct += 1;
                                        })
                                        if (al_ct == 0) {
                                            grp_ids_other.push(ot.empsG_Id);
                                        }
                                    }
                                })
                                angular.forEach(final_exam_grps, function (fin) {
                                    if (grp_ids_final.length == 0) {
                                        grp_ids_final.push(fin.empsG_Id);
                                    }
                                    else if (grp_ids_final.length > 0) {
                                        var al_ct = 0;
                                        angular.forEach(grp_ids_final, function (grp_id) {
                                            if (grp_id == fin.empsG_Id)
                                                al_ct += 1;
                                        })
                                        if (al_ct == 0) {
                                            grp_ids_final.push(fin.empsG_Id);
                                        }
                                    }
                                })

                                var total_oth = 0;
                                var oth_group_names = [];
                                angular.forEach(grp_ids_other, function (grp) {
                                    angular.forEach($scope.prom_subj_groupdetails, function (s_grp) {
                                        if (s_grp.empsG_Id == grp) {
                                            total_oth += Number(s_grp.empsG_PercentValue);
                                            oth_group_names.push(s_grp);
                                        }
                                    })
                                })
                                if (total_oth > 0) {
                                    $scope.temp_headers.push({ flag: 'O', Per_val: total_oth, names: oth_group_names });
                                }
                                var total_fin = 0;
                                var fin_group_names = [];
                                angular.forEach(grp_ids_final, function (grp) {
                                    angular.forEach($scope.prom_subj_groupdetails, function (s_grp) {
                                        if (s_grp.empsG_Id == grp) {
                                            total_fin += Number(s_grp.empsG_PercentValue);
                                            fin_group_names.push(s_grp);
                                        }
                                    })
                                })
                                if (total_fin > 0) {
                                    var equal_flag = false;
                                    angular.forEach($scope.exam_list, function (exm) {
                                        if (exm.emE_FinalExamFlag) {
                                            if (exm.marks == total_fin) {
                                                equal_flag = true;
                                            }
                                        }
                                    })
                                    $scope.temp_headers.push({ flag: 'F', Per_val: total_fin, names: fin_group_names, equal_flag: equal_flag });
                                }

                                if (promise.promotion_stumarks != null && promise.promotion_stumarks != "" && promise.promotion_stumarks.length > 0 && promise.promotion_stumarks_grpwise != null && promise.promotion_stumarks_grpwise != "" && promise.promotion_stumarks_grpwise.length > 0) {
                                    
                                   
                                    var Temp_EMPSG_Ids = [];
                                    angular.forEach(promise.promotion_subectdetails, function (sub) {
                                        var oth_empsg_ids = [];
                                        var fin_empsg_ids = [];
                                        angular.forEach(promise.prom_subj_groupdetails_all, function (sub_grp) {
                                            if (sub.empS_Id == sub_grp.empS_Id) {
                                                angular.forEach($scope.temp_headers, function (temp) {
                                                    if (temp.flag == 'O') {
                                                        angular.forEach(temp.names, function (te) {
                                                            if (te.empsG_GroupName == sub_grp.empsG_GroupName)
                                                                oth_empsg_ids.push(sub_grp.empsG_Id);
                                                        })
                                                    }
                                                    else if (temp.flag == 'F') {
                                                        angular.forEach(temp.names, function (te) {
                                                            if (te.empsG_GroupName == sub_grp.empsG_GroupName)
                                                                fin_empsg_ids.push(sub_grp.empsG_Id);
                                                        })
                                                    }
                                                })
                                            }
                                        })
                                        Temp_EMPSG_Ids.push({ ismS_Id: sub.ismS_Id, O_Ids: oth_empsg_ids, F_Ids: fin_empsg_ids });
                                    })
                                    angular.forEach($scope.student_list, function (stud) {
                                    $scope.temp_student_marks = [];
                                    angular.forEach(promise.promotion_stumarks, function (subj) {
                                        var subject_marks_other = 0;
                                        var subject_marks_final = 0;
                                        if (subj.amsT_Id == stud.amsT_Id) {
                                            angular.forEach(Temp_EMPSG_Ids, function (s_grp_id) {
                                                if (s_grp_id.ismS_Id == subj.ismS_Id) {
                                                    angular.forEach(s_grp_id.O_Ids, function (ot) {
                                                        angular.forEach(promise.promotion_stumarks_grpwise, function (st_m_grp) {
                                                            if (st_m_grp.empsG_Id == ot && subj.estmppS_Id == st_m_grp.estmppS_Id) {
                                                                subject_marks_other += Number(st_m_grp.estmppsG_GroupObtMarks);
                                                            }
                                                        })
                                                    })
                                                    angular.forEach(s_grp_id.F_Ids, function (fin) {
                                                        angular.forEach(promise.promotion_stumarks_grpwise, function (st_m_grp) {
                                                            if (st_m_grp.empsG_Id == fin && subj.estmppS_Id == st_m_grp.estmppS_Id) {
                                                                subject_marks_final += Number(st_m_grp.estmppsG_GroupObtMarks);
                                                            }
                                                        })
                                                    })
                                                }
                                            })

                                            //angular.forEach(grp_ids_other, function (ot) {
                                            //    angular.forEach(promise.promotion_stumarks_grpwise, function (st_m_grp) {
                                            //        if(st_m_grp.empsG_Id==ot && subj.estmppS_Id==st_m_grp.estmppS_Id)
                                            //        {
                                            //            subject_marks_other += Number(st_m_grp.estmppsG_GroupObtMarks);
                                            //        }
                                            //})
                                            //})
                                            //angular.forEach(grp_ids_final, function (fin) {
                                            //    angular.forEach(promise.promotion_stumarks_grpwise, function (st_m_grp) {
                                            //        if (st_m_grp.empsG_Id == fin && subj.estmppS_Id == st_m_grp.estmppS_Id) {
                                            //            subject_marks_final += Number(st_m_grp.estmppsG_GroupObtMarks);
                                            //        }
                                            //    })
                                            //})
                                            $scope.temp_student_marks.push({ ismS_Id: subj.ismS_Id, grp_marks_o: subject_marks_other, grp_marks_f: subject_marks_final, amsT_Id: stud.amsT_Id });
                                        }
                                    })
                                    stud.stu_temp_student_marks = $scope.temp_student_marks;
                                    })
                                    angular.forEach($scope.student_list, function (stud) {
                                        var total_array_Grp = [];
                                        var subj_total_grp_O = 0;
                                        var subj_total_grp_F = 0;
                                        angular.forEach(stud.stu_temp_student_marks, function (stu_marks_g) {
                                            //subj_total_grp_O += Number(stu_marks_g.grp_marks_o);
                                            //subj_total_grp_F += Number(stu_marks_g.grp_marks_f);
                                            subj_total_grp_O += Number($filter('number')(Number(stu_marks_g.grp_marks_o), 0));
                                            subj_total_grp_F += Number($filter('number')(Number(stu_marks_g.grp_marks_f), 0));
                                        })
                                        total_array_Grp.push({total_s_o: subj_total_grp_O, total_s_f: subj_total_grp_F });

                                        $scope.Total_Array_Grp = total_array_Grp;
                                        stud.stu_Total_Array_Grp = $scope.Total_Array_Grp;
                                        console.log($scope.Total_Array_Grp);
                                        $scope.Total_total_grp = 0;
                                        angular.forEach($scope.Total_Array_Grp, function (total_g) {
                                            $scope.Total_total_grp += Number($filter('number')(Number(total_g.total_s_o), 0));
                                            $scope.Total_total_grp += Number($filter('number')(Number(total_g.total_s_f), 0));
                                        })
                                        stud.stu_Total_total_grp = $scope.Total_total_grp;

                                        //for percentage new
                                        var Total_max_grp = 0;
                                        angular.forEach(stud.stu_subjects, function (subj) {
                                            angular.forEach(promise.promotion_subectdetails, function (pro_sub_max_marks) {

                                                if (pro_sub_max_marks.ismS_Id == subj.ismS_Id) {
                                                    Total_max_grp += Number($filter('number')(Number(pro_sub_max_marks.empS_MaxMarks), 0));
                                                }
                                            })
                                        })
                                        stud.stud_total_max_grp = Total_max_grp;
                                        var stu_total_percentage = 0;
                                        stu_total_percentage = (stud.stu_Total_total_grp / stud.stud_total_max_grp) * 100;
                                        stud.total_percentage = Number($filter('number')(Number(stu_total_percentage), 2));
                                        //
                                    })
                                }
                                //for all new
                                angular.forEach($scope.student_list, function (stud) {
                                    var stu_subj_marks = [];
                                    angular.forEach(stud.stu_subjects, function (sub) {
                                        var subj_marks = [];
                                        var subj_obj = {};
                                        //  if (sub.flag)
                                        //  {
                                        angular.forEach($scope.exam_list, function (exm) {
                                            if (!exm.emE_FinalExamFlag) {
                                                var obj = {};
                                                var cnt = 0;
                                                angular.forEach(stud.stu_student_marks, function (marks) {
                                                    if (marks.ismS_Id == sub.ismS_Id && marks.emE_Id == exm.emE_Id) {
                                                        cnt += 1;
                                                        obj.estmpS_ObtainedGrade = marks.estmpS_ObtainedGrade;
                                                        if (marks.estmpS_PassFailFlg == 'AB' || marks.estmpS_PassFailFlg == 'M' || marks.estmpS_PassFailFlg == 'L') {
                                                            obj.estmpS_ObtainedMarks = marks.estmpS_PassFailFlg;
                                                            obj.estmpS_ObtainedGrade = marks.estmpS_PassFailFlg;
                                                        }
                                                        else if (marks.estmpS_PassFailFlg != 'AB' && marks.estmpS_PassFailFlg != 'M' && marks.estmpS_PassFailFlg != 'L') {
                                                            obj.estmpS_ObtainedMarks = marks.estmpS_ObtainedMarks;
                                                        }
                                                        //obj.estmpS_ObtainedGrade = marks.estmpS_ObtainedGrade;
                                                    }
                                                })
                                                if (cnt == 0) {
                                                    obj.estmpS_ObtainedMarks = "";
                                                    obj.estmpS_ObtainedGrade = "";
                                                }
                                                subj_marks.push(obj);
                                            }
                                        })

                                        angular.forEach($scope.temp_headers, function (hdr) {
                                            if (hdr.flag != 'F') {
                                                var obj = {};
                                                var cnt = 0;
                                                angular.forEach(stud.stu_temp_student_marks, function (g_marks) {
                                                    if (g_marks.ismS_Id == sub.ismS_Id) {
                                                        cnt += 1;
                                                        obj.estmpS_ObtainedMarks = Number($filter('number')(Number(g_marks.grp_marks_o), 0));
                                                    }
                                                })
                                                if (cnt == 0) {
                                                    obj.estmpS_ObtainedMarks = "";
                                                }
                                                subj_marks.push(obj);
                                            }
                                        })
                                        angular.forEach($scope.exam_list, function (exm) {
                                            if (exm.emE_FinalExamFlag) {
                                                var obj = {};
                                                var cnt = 0;
                                                angular.forEach(stud.stu_student_marks, function (marks) {
                                                    if (marks.ismS_Id == sub.ismS_Id && marks.emE_Id == exm.emE_Id) {
                                                        cnt += 1;
                                                        obj.estmpS_ObtainedGrade = marks.estmpS_ObtainedGrade;
                                                        if (marks.estmpS_PassFailFlg == 'AB' || marks.estmpS_PassFailFlg == 'M' || marks.estmpS_PassFailFlg == 'L') {
                                                            obj.estmpS_ObtainedMarks = marks.estmpS_PassFailFlg;
                                                            obj.estmpS_ObtainedGrade = marks.estmpS_PassFailFlg;
                                                        }
                                                        else if (marks.estmpS_PassFailFlg != 'AB' && marks.estmpS_PassFailFlg != 'M' && marks.estmpS_PassFailFlg != 'L') {
                                                            obj.estmpS_ObtainedMarks = marks.estmpS_ObtainedMarks;
                                                        }
                                                        //obj.estmpS_ObtainedGrade = marks.estmpS_ObtainedGrade;
                                                    }
                                                })
                                                if (cnt == 0) {
                                                    obj.estmpS_ObtainedMarks = "";
                                                    obj.estmpS_ObtainedGrade = "";
                                                }
                                                subj_marks.push(obj);
                                            }
                                        })

                                        angular.forEach($scope.temp_headers, function (hdr) {
                                            if (hdr.flag == 'F' && !hdr.equal_flag) {
                                                var obj = {};
                                                var cnt = 0;
                                                angular.forEach(stud.stu_temp_student_marks, function (g_marks) {
                                                    if (g_marks.ismS_Id == sub.ismS_Id) {
                                                        cnt += 1;
                                                        obj.estmpS_ObtainedMarks = Number($filter('number')(Number(g_marks.grp_marks_f), 0));
                                                    }
                                                })
                                                if (cnt == 0) {
                                                    obj.estmpS_ObtainedMarks = "";
                                                }
                                                subj_marks.push(obj);
                                            }
                                        })
                                        angular.forEach($scope.promotion_student_marks, function (prom_mks) {
                                            if (prom_mks.amsT_Id == stud.amsT_Id && prom_mks.ismS_Id == sub.ismS_Id) {
                                                subj_obj.estmppS_ObtainedMarks = Number($filter('number')(Number(prom_mks.estmppS_ObtainedMarks), 0));
                                                subj_obj.estmppS_Remarks = prom_mks.estmppS_Remarks;
                                                subj_obj.estmppS_ObtainedGrade = prom_mks.estmppS_ObtainedGrade;
                                                subj_obj.estmppS_PassFailFlg = prom_mks.estmppS_PassFailFlg;
                                            }
                                        })
                                        //     }
                                        //  sub.subj_marks = subj_marks;
                                        subj_obj.flag = sub.flag;
                                        subj_obj.ismS_Id = sub.ismS_Id;
                                        subj_obj.ismS_SubjectName = sub.ismS_SubjectName;
                                        stu_subj_marks.push({ sub_details: subj_obj, subj_marks_details: subj_marks });
                                    })
                                    stud.final_subj_marks = stu_subj_marks;
                                })

                                console.log($scope.student_list);
                            }
                            else if($scope.EMP_MarksPerFlg == 'T' || $scope.EMP_MarksPerFlg == 'F')
                            {
                                angular.forEach($scope.student_list, function (stud) {
                                                angular.forEach(stud.stu_student_marks, function (marks) {
                                                 
                                                        if (marks.estmpS_PassFailFlg == 'AB' || marks.estmpS_PassFailFlg == 'M' || marks.estmpS_PassFailFlg == 'L') {
                                                            marks.estmpS_ObtainedMarks = marks.estmpS_PassFailFlg;
                                                            marks.estmpS_ObtainedGrade = marks.estmpS_PassFailFlg;
                                                        }
                                                })
                                     })

                                console.log($scope.student_list);
                            }

                        }

                        //End
                    }
                    else {
                        swal("Selected Student Not Mapped with Subjects/Exams");
                        $scope.clear();
                    }

                    //$scope.class_list = promise.classlist;
                    //$scope.ASMCL_Id = "";
                    //$scope.ASMS_Id = "";
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.Print = function () {
            var innerContents = document.getElementById("Final_Report").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/PromotionFinalReportPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
    }
})();
