(function () {
    'use strict';

    angular
        .module('app')
        .controller('Baldwin_Final_P_C_ReportController', Baldwin_Final_P_C_ReportController);

    Baldwin_Final_P_C_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout'];

    function Baldwin_Final_P_C_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Baldwin_Final_P_C_Report';

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != undefined && admfigsettings != null) {
            var logopath = "";
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
            $scope.imgname = logopath;
        }


        $scope.BindData = function () {
            
            apiService.getDATA("Baldwin_Final_P_C_Report/Getdetails").
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
            apiService.create("Baldwin_Final_P_C_Report/get_classes", data).then(function (promise) {
                $scope.class_list = promise.classlist;
                $scope.ASMCL_Id = "";
                $scope.ASMS_Id = "";
                $scope.AMST_Id = "";
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
            apiService.create("Baldwin_Final_P_C_Report/get_sections", data).then(function (promise) {
                $scope.section_list = promise.sectionlist;
                $scope.ASMS_Id = "";
                $scope.AMST_Id = "";
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
            apiService.create("Baldwin_Final_P_C_Report/get_students", data).then(function (promise) {
                $scope.student_list = promise.studentlist;
                $scope.AMST_Id = "";
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
            // $scope.AMST_Id = "";
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
            $scope.subject_list = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    //"AMST_Id": $scope.AMST_Id
                }
                apiService.create("Baldwin_Final_P_C_Report/get_report", data).then(function (promise) {
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
                        angular.forEach($scope.student_list, function (s) {
                            if (s.amsT_Id == $scope.AMST_Id) {
                                $scope.Student_Name = s.amsT_FirstName;
                                $scope.Student_AdmNo = s.amsT_AdmNo;
                                $scope.Student_RollNo = s.amaY_RollNo;
                                $scope.Student_dob = s.amsT_DOB;
                            }
                        })

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
                                $scope.prom_cumulative_details = promise.prom_cumulativedetails;
                                console.log($scope.prom_cumulative_details);
                                angular.forEach($scope.student_marks, function (stu_marks) {
                                    if (stu_marks.estmpS_PassFailFlg == 'AB' || stu_marks.estmpS_PassFailFlg == 'M' || stu_marks.estmpS_PassFailFlg == 'L') {
                                        stu_marks.estmpS_PassFailFlg = stu_marks.estmpS_PassFailFlg;
                                    }
                                    else if (stu_marks.estmpS_PassFailFlg != 'AB' && stu_marks.estmpS_PassFailFlg != 'M' && stu_marks.estmpS_PassFailFlg != 'L') {
                                        stu_marks.estmpS_PassFailFlg = stu_marks.estmpS_ObtainedMarks;
                                    }
                                })
                                $scope.temp_headers = [];
                                var final_exam_grps = [];
                                var other_exam_grps = [];
                                var grp_ids_other = [];
                                var grp_ids_final = [];
                                angular.forEach($scope.prom_subj_grp_exms, function (e1) {
                                    angular.forEach($scope.exam_list, function (e2) {
                                        if (e2.emE_Id == e1.emE_Id) {
                                            if (e2.emE_FinalExamFlag) {
                                                final_exam_grps.push(e1);
                                                if (grp_ids_final.length == 0) {
                                                    grp_ids_final.push(e1.empsG_Id);
                                                }
                                                else if (grp_ids_final.length > 0) {
                                                    var al_ct = 0;
                                                    angular.forEach(grp_ids_final, function (grp_id) {
                                                        if (grp_id == e1.empsG_Id)
                                                            al_ct += 1;
                                                    })
                                                    if (al_ct == 0) {
                                                        grp_ids_final.push(e1.empsG_Id);
                                                    }
                                                }
                                                //grp_ids_final.push(e1.empsG_Id);
                                            }
                                            else {
                                                other_exam_grps.push(e1);
                                                if (grp_ids_other.length == 0) {
                                                    grp_ids_other.push(e1.empsG_Id);
                                                }
                                                else if (grp_ids_other.length > 0) {
                                                    var al_ct = 0;
                                                    angular.forEach(grp_ids_other, function (grp_id) {
                                                        if (grp_id == e1.empsG_Id)
                                                            al_ct += 1;
                                                    })
                                                    if (al_ct == 0) {
                                                        grp_ids_other.push(e1.empsG_Id);
                                                    }
                                                }
                                                //grp_ids_other.push(e1.empsG_Id);
                                            }
                                        }

                                    })

                                })
                                console.log(final_exam_grps);
                                console.log(other_exam_grps);


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
                                console.log($scope.exam_list);
                                console.log($scope.temp_headers);
                                //for colspan 
                                //angular.forEach($scope.temp_headers, function (th) {
                                //    if (th.flag == 'F' && th.equal_flag) {
                                //        $scope.colspan_e = $scope.temp_headers.length + $scope.exam_list.length;
                                //    }
                                //    else if (th.flag == 'F' && !th.equal_flag) {
                                //        $scope.colspan_e = $scope.temp_headers.length + $scope.exam_list.length + 1;
                                //    }
                                //})

                                //for subjetwise exms and groups
                                $scope.Final_temp_headers = [];
                                angular.forEach($scope.subject_list, function (subj) {

                                    angular.forEach($scope.exam_list, function (exm) {
                                        if (!exm.emE_FinalExamFlag) {
                                            $scope.Final_temp_headers.push({ emE_Id: exm.emE_Id, emE_ExamName: exm.emE_ExamName, emE_ExamCode: exm.emE_ExamCode, emE_FinalExamFlag: exm.emE_FinalExamFlag, Name: exm.emE_ExamCode, ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                                        }
                                    })
                                    angular.forEach(promise.promotion_subectdetails, function (pro_subj) {
                                        if (pro_subj.ismS_Id == subj.ismS_Id) {
                                            angular.forEach($scope.temp_headers, function (hdr) {
                                                if (hdr.flag != 'F') {
                                                    $scope.Final_temp_headers.push({ flag: hdr.flag, Per_val: hdr.Per_val, names: hdr.names, Name: hdr.Per_val + ' %', ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                                                }
                                            })
                                        }
                                    })
                                    angular.forEach($scope.exam_list, function (exm) {
                                        if (exm.emE_FinalExamFlag) {
                                            $scope.Final_temp_headers.push({ emE_Id: exm.emE_Id, emE_ExamName: exm.emE_ExamName, emE_ExamCode: exm.emE_ExamCode, emE_FinalExamFlag: exm.emE_FinalExamFlag, Name: exm.emE_ExamCode, ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                                        }
                                    })
                                    angular.forEach(promise.promotion_subectdetails, function (pro_subj) {
                                        if (pro_subj.ismS_Id == subj.ismS_Id) {
                                            angular.forEach($scope.temp_headers, function (hdr) {
                                                if (hdr.flag == 'F') {
                                                    $scope.Final_temp_headers.push({ flag: hdr.flag, Per_val: hdr.Per_val, names: hdr.names, equal_flag: hdr.equal_flag, Name: hdr.Per_val + ' %', ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                                                }
                                            })

                                            $scope.Final_temp_headers.push({ Name: '100 %', flag: 'Total', ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                                        }
                                    })
                                })
                                console.log($scope.Final_temp_headers);
                                //for colspan
                                angular.forEach($scope.subject_list, function (sub) {
                                    var col_span = 0;
                                    angular.forEach($scope.Final_temp_headers, function (header) {
                                        if (header.ismS_Id == sub.ismS_Id) {
                                            if (header.flag == 'F' && header.equal_flag) {
                                                col_span -= 1;
                                            }
                                            col_span += 1;
                                        }
                                    })
                                    sub.col_span = col_span;
                                    //angular.forEach($scope.temp_headers, function (th) {
                                    //    if (th.flag == 'F' && th.equal_flag) {
                                    //        $scope.colspan_e = $scope.temp_headers.length + $scope.exam_list.length;
                                    //    }
                                    //    else if (th.flag == 'F' && !th.equal_flag) {
                                    //        $scope.colspan_e = $scope.temp_headers.length + $scope.exam_list.length + 1;
                                    //    }
                                    //})
                                })

                                angular.forEach($scope.student_list, function (stud) {
                                    $scope.tmparrry = [];
                                    angular.forEach($scope.Final_temp_headers, function (header) {
                                        var count = 0;
                                        var obj = {};
                                        if (header.flag == undefined) {
                                            angular.forEach($scope.student_marks, function (marks) {
                                                if (marks.amsT_Id == stud.amsT_Id && marks.emE_Id == header.emE_Id && marks.ismS_Id == header.ismS_Id) {
                                                    count += 1;
                                                    if (marks.estmpS_PassFailFlg == 'AB' || marks.estmpS_PassFailFlg == 'M' || marks.estmpS_PassFailFlg == 'L') {
                                                        obj.estmpS_ObtainedMarks = marks.estmpS_PassFailFlg;
                                                    }
                                                    else if (marks.estmpS_PassFailFlg != 'AB' && marks.estmpS_PassFailFlg != 'M' && marks.estmpS_PassFailFlg != 'L') {
                                                        obj.estmpS_ObtainedMarks = marks.estmpS_ObtainedMarks;
                                                    }
                                                    obj.estmpS_ObtainedGrade = marks.estmpS_ObtainedGrade;
                                                }
                                            })
                                        }
                                        else {
                                            if (header.flag == 'O') {
                                                angular.forEach($scope.prom_cumulative_details, function (g_marks) {
                                                    if (g_marks.AMST_Id == stud.amsT_Id && g_marks.ISMS_Id == header.ismS_Id) {
                                                        count += 1;
                                                        obj.estmpS_ObtainedMarks = g_marks.Obtained_O;
                                                    }
                                                })
                                            }
                                            else if (header.flag == 'F' && !header.equal_flag) {
                                                angular.forEach($scope.prom_cumulative_details, function (g_marks) {
                                                    if (g_marks.AMST_Id == stud.amsT_Id && g_marks.ISMS_Id == header.ismS_Id) {
                                                        count += 1;
                                                        obj.estmpS_ObtainedMarks = g_marks.Obtained_F;
                                                    }
                                                })
                                            }
                                            else if (header.flag == 'Total') {
                                                angular.forEach($scope.prom_cumulative_details, function (prom_mks) {
                                                    if (prom_mks.AMST_Id == stud.amsT_Id && prom_mks.ISMS_Id == header.ismS_Id) {
                                                        count += 1;
                                                        obj.estmpS_ObtainedMarks = prom_mks.Obtained_T;
                                                    }
                                                })
                                            }
                                            obj.estmpS_ObtainedGrade = "";
                                        }
                                        if (count == 0) {
                                            obj.estmpS_ObtainedMarks = "";
                                            obj.estmpS_ObtainedGrade = "";
                                        }
                                        $scope.tmparrry.push(obj);
                                        if (header.flag == 'F' && header.equal_flag) {
                                            $scope.tmparrry.splice(($scope.tmparrry.length - 1), 1);
                                        }
                                    })
                                    stud.sub_list = $scope.tmparrry;
                                })
                                console.log($scope.student_list);
                            }
                            //if ($scope.EMP_MarksPerFlg == 'P') {
                            //    $scope.temp_headers = [];
                            //    var final_exam_grps = [];
                            //    var other_exam_grps = [];
                            //    angular.forEach($scope.prom_subj_grp_exms, function (e1) {
                            //        angular.forEach($scope.exam_list, function (e2) {
                            //            if (e2.emE_Id == e1.emE_Id) {
                            //                if (e2.emE_FinalExamFlag) {
                            //                    final_exam_grps.push(e1);
                            //                }
                            //                else {
                            //                    other_exam_grps.push(e1);
                            //                }
                            //            }

                            //        })

                            //    })
                            //    console.log(final_exam_grps);
                            //    console.log(other_exam_grps);
                            //    var grp_ids_other = [];
                            //    var grp_ids_final = [];
                            //    angular.forEach(other_exam_grps, function (ot) {
                            //        if (grp_ids_other.length == 0) {
                            //            grp_ids_other.push(ot.empsG_Id);
                            //        }
                            //        else if (grp_ids_other.length > 0) {
                            //            var al_ct = 0;
                            //            angular.forEach(grp_ids_other, function (grp_id) {
                            //                if (grp_id == ot.empsG_Id)
                            //                    al_ct += 1;
                            //            })
                            //            if (al_ct == 0) {
                            //                grp_ids_other.push(ot.empsG_Id);
                            //            }
                            //        }
                            //    })
                            //    angular.forEach(final_exam_grps, function (fin) {
                            //        if (grp_ids_final.length == 0) {
                            //            grp_ids_final.push(fin.empsG_Id);
                            //        }
                            //        else if (grp_ids_final.length > 0) {
                            //            var al_ct = 0;
                            //            angular.forEach(grp_ids_final, function (grp_id) {
                            //                if (grp_id == fin.empsG_Id)
                            //                    al_ct += 1;
                            //            })
                            //            if (al_ct == 0) {
                            //                grp_ids_final.push(fin.empsG_Id);
                            //            }
                            //        }
                            //    })

                            //    var total_oth = 0;
                            //    var oth_group_names = [];
                            //    angular.forEach(grp_ids_other, function (grp) {
                            //        angular.forEach($scope.prom_subj_groupdetails, function (s_grp) {
                            //            if (s_grp.empsG_Id == grp) {
                            //                total_oth += Number(s_grp.empsG_PercentValue);
                            //                oth_group_names.push(s_grp);
                            //            }
                            //        })
                            //    })
                            //    if (total_oth > 0) {
                            //        $scope.temp_headers.push({ flag: 'O', Per_val: total_oth, names: oth_group_names });
                            //    }
                            //    var total_fin = 0;
                            //    var fin_group_names = [];
                            //    angular.forEach(grp_ids_final, function (grp) {
                            //        angular.forEach($scope.prom_subj_groupdetails, function (s_grp) {
                            //            if (s_grp.empsG_Id == grp) {
                            //                total_fin += Number(s_grp.empsG_PercentValue);
                            //                fin_group_names.push(s_grp);
                            //            }
                            //        })
                            //    })
                            //    if (total_fin > 0) {
                            //        var equal_flag = false;
                            //        angular.forEach($scope.exam_list, function (exm) {
                            //            if (exm.emE_FinalExamFlag) {
                            //                if (exm.marks == total_fin) {
                            //                    equal_flag = true;
                            //                }
                            //            }
                            //        })
                            //        $scope.temp_headers.push({ flag: 'F', Per_val: total_fin, names: fin_group_names, equal_flag: equal_flag });
                            //    }

                            //    //for colspan
                            //    angular.forEach($scope.temp_headers, function (th) {
                            //        if (th.flag == 'F' && th.equal_flag) {
                            //            $scope.colspan_e = $scope.temp_headers.length + $scope.exam_list.length;
                            //        }
                            //        else if (th.flag == 'F' && !th.equal_flag) {
                            //            $scope.colspan_e = $scope.temp_headers.length + $scope.exam_list.length + 1;
                            //        }
                            //    })
                            //    //for subjetwise exms and groups
                            //    $scope.Final_temp_headers = [];
                            //    angular.forEach($scope.subject_list, function (subj) {
                            //        angular.forEach($scope.exam_list, function (exm) {
                            //            if (!exm.emE_FinalExamFlag) {
                            //                //exm.Name = exm.emE_ExamCode;
                            //                //exm.Name = exm.emE_ExamName;
                            //                //exm.ismS_Id = subj.ismS_Id;
                            //                $scope.Final_temp_headers.push({ emE_Id: exm.emE_Id, emE_ExamName: exm.emE_ExamName, emE_ExamCode: exm.emE_ExamCode, emE_FinalExamFlag: exm.emE_FinalExamFlag, Name: exm.emE_ExamCode, ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                            //            }
                            //        })
                            //        angular.forEach($scope.temp_headers, function (hdr) {
                            //            if (hdr.flag != 'F') {
                            //                //hdr.Name = hdr.Per_val + ' %';
                            //                //hdr.ismS_Id = subj.ismS_Id;
                            //                $scope.Final_temp_headers.push({ flag: hdr.flag, Per_val: hdr.Per_val, names: hdr.names, Name: hdr.Per_val + ' %', ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                            //            }
                            //        })
                            //        angular.forEach($scope.exam_list, function (exm) {
                            //            if (exm.emE_FinalExamFlag) {
                            //                //exm.Name = exm.emE_ExamCode;
                            //                //exm.Name = exm.emE_ExamName;
                            //                //exm.ismS_Id = subj.ismS_Id;
                            //                //$scope.Final_temp_headers.push(exm);
                            //                $scope.Final_temp_headers.push({ emE_Id: exm.emE_Id, emE_ExamName: exm.emE_ExamName, emE_ExamCode: exm.emE_ExamCode, emE_FinalExamFlag: exm.emE_FinalExamFlag, Name: exm.emE_ExamCode, ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                            //            }
                            //        })
                            //        angular.forEach($scope.temp_headers, function (hdr) {
                            //            if (hdr.flag == 'F') {
                            //                //hdr.Name = hdr.Per_val + ' %';
                            //                //hdr.ismS_Id = subj.ismS_Id;
                            //                //$scope.Final_temp_headers.push(hdr);
                            //                $scope.Final_temp_headers.push({ flag: hdr.flag, Per_val: hdr.Per_val, names: hdr.names, equal_flag: hdr.equal_flag, Name: hdr.Per_val + ' %', ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                            //            }
                            //        })
                            //        $scope.Final_temp_headers.push({ Name: '100 %', type: 'Total', ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                            //    })
                            //    console.log($scope.Final_temp_headers);
                            //    console.log($scope.student_list);
                            //    console.log($scope.student_marks);
                            //    if (promise.promotion_stumarks != null && promise.promotion_stumarks != "" && promise.promotion_stumarks.length > 0 && promise.promotion_stumarks_grpwise != null && promise.promotion_stumarks_grpwise != "" && promise.promotion_stumarks_grpwise.length > 0) {
                            //        

                            //        var Temp_EMPSG_Ids = [];
                            //        angular.forEach(promise.promotion_subectdetails, function (sub) {
                            //            var oth_empsg_ids = [];
                            //            var fin_empsg_ids = [];
                            //            angular.forEach(promise.prom_subj_groupdetails_all, function (sub_grp) {
                            //                if (sub.empS_Id == sub_grp.empS_Id) {
                            //                    angular.forEach($scope.temp_headers, function (temp) {
                            //                        if (temp.flag == 'O') {
                            //                            angular.forEach(temp.names, function (te) {
                            //                                if (te.empsG_GroupName == sub_grp.empsG_GroupName)
                            //                                    oth_empsg_ids.push(sub_grp.empsG_Id);
                            //                            })
                            //                        }
                            //                        else if (temp.flag == 'F') {
                            //                            angular.forEach(temp.names, function (te) {
                            //                                if (te.empsG_GroupName == sub_grp.empsG_GroupName)
                            //                                    fin_empsg_ids.push(sub_grp.empsG_Id);
                            //                            })
                            //                        }
                            //                    })
                            //                }
                            //            })
                            //            Temp_EMPSG_Ids.push({ ismS_Id: sub.ismS_Id, O_Ids: oth_empsg_ids, F_Ids: fin_empsg_ids });
                            //        })

                            //        $scope.temp_student_marks = [];
                            //        angular.forEach(promise.promotion_stumarks, function (subj) {
                            //            var subject_marks_other = 0;
                            //            var subject_marks_final = 0;
                            //            angular.forEach(Temp_EMPSG_Ids, function (s_grp_id) {
                            //                if (s_grp_id.ismS_Id == subj.ismS_Id) {
                            //                    angular.forEach(s_grp_id.O_Ids, function (ot) {
                            //                        angular.forEach(promise.promotion_stumarks_grpwise, function (st_m_grp) {
                            //                            if (st_m_grp.empsG_Id == ot && subj.estmppS_Id == st_m_grp.estmppS_Id) {
                            //                                subject_marks_other += Number(st_m_grp.estmppsG_GroupObtMarks);
                            //                            }
                            //                        })
                            //                    })
                            //                    angular.forEach(s_grp_id.F_Ids, function (fin) {
                            //                        angular.forEach(promise.promotion_stumarks_grpwise, function (st_m_grp) {
                            //                            if (st_m_grp.empsG_Id == fin && subj.estmppS_Id == st_m_grp.estmppS_Id) {
                            //                                subject_marks_final += Number(st_m_grp.estmppsG_GroupObtMarks);
                            //                            }
                            //                        })
                            //                    })
                            //                }
                            //            })
                            //            $scope.temp_student_marks.push({ ismS_Id: subj.ismS_Id, grp_marks_o: subject_marks_other, grp_marks_f: subject_marks_final,amsT_Id:subj.amsT_Id });

                            //        })
                            //        console.log($scope.temp_student_marks);
                            //        var total_array_Grp = [];
                            //        var subj_total_grp_O = 0;
                            //        var subj_total_grp_F = 0;
                            //        angular.forEach($scope.temp_student_marks, function (stu_marks_g) {
                            //            subj_total_grp_O += Number(stu_marks_g.grp_marks_o);
                            //            subj_total_grp_F += Number(stu_marks_g.grp_marks_f);
                            //        })
                            //        total_array_Grp.push({ total_s_o: subj_total_grp_O, total_s_f: subj_total_grp_F });

                            //        $scope.Total_Array_Grp = total_array_Grp;
                            //        console.log($scope.Total_Array_Grp);
                            //    }
                            //}
                            //for subjetwise exms and groups
                            else if ($scope.EMP_MarksPerFlg == 'F' || $scope.EMP_MarksPerFlg == 'T') {
                                //for subjetwise exms and groups
                                $scope.Final_temp_headers = [];
                                angular.forEach($scope.subject_list, function (subj) {
                                    angular.forEach($scope.exam_list, function (exm) {
                                        if (!exm.emE_FinalExamFlag) {
                                            $scope.Final_temp_headers.push({ emE_Id: exm.emE_Id, emE_ExamName: exm.emE_ExamName, emE_ExamCode: exm.emE_ExamCode, emE_FinalExamFlag: exm.emE_FinalExamFlag, Name: exm.emE_ExamCode, ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                                        }
                                    })
                                    angular.forEach($scope.exam_list, function (exm) {
                                        if (exm.emE_FinalExamFlag) {
                                            $scope.Final_temp_headers.push({ emE_Id: exm.emE_Id, emE_ExamName: exm.emE_ExamName, emE_ExamCode: exm.emE_ExamCode, emE_FinalExamFlag: exm.emE_FinalExamFlag, Name: exm.emE_ExamCode, ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                                        }
                                    })

                                    $scope.Final_temp_headers.push({ Name: 'Total', type: 'Total', ismS_Id: subj.ismS_Id, subj_flag: subj.flag });
                                })
                                console.log($scope.Final_temp_headers);

                                angular.forEach($scope.student_list, function (stud) {
                                    $scope.tmparrry = [];
                                    angular.forEach($scope.Final_temp_headers, function (header) {
                                        var count = 0;
                                        var obj = {};
                                        if (header.type == undefined) {
                                            angular.forEach($scope.student_marks, function (marks) {
                                                if (marks.amsT_Id == stud.amsT_Id && marks.emE_Id == header.emE_Id && marks.ismS_Id == header.ismS_Id) {
                                                    count += 1;
                                                    if (marks.estmpS_PassFailFlg == 'AB' || marks.estmpS_PassFailFlg == 'M' || marks.estmpS_PassFailFlg == 'L') {
                                                        obj.estmpS_ObtainedMarks = marks.estmpS_PassFailFlg;
                                                    }
                                                    else if (marks.estmpS_PassFailFlg != 'AB' && marks.estmpS_PassFailFlg != 'M' && marks.estmpS_PassFailFlg != 'L') {
                                                        obj.estmpS_ObtainedMarks = marks.estmpS_ObtainedMarks;
                                                    }
                                                    obj.estmpS_ObtainedGrade = marks.estmpS_ObtainedGrade;
                                                }
                                            })
                                        }
                                        else {
                                            if (header.type == 'Total') {
                                                angular.forEach($scope.promotion_student_marks, function (prom_mks) {
                                                    if (prom_mks.amsT_Id == stud.amsT_Id && prom_mks.ismS_Id == header.ismS_Id) {
                                                        count += 1;
                                                        obj.estmpS_ObtainedMarks = prom_mks.estmppS_ObtainedMarks;
                                                    }
                                                })
                                            }
                                            obj.estmpS_ObtainedGrade = "";
                                        }
                                        if (count == 0) {
                                            obj.estmpS_ObtainedMarks = "";
                                            obj.estmpS_ObtainedGrade = "";
                                        }
                                        $scope.tmparrry.push(obj);
                                    })
                                    stud.sub_list = $scope.tmparrry;
                                })
                                console.log($scope.student_list);

                            }
                        }

                        //End
                    }
                    else {
                        swal("Selected Details Not Mapped with Subjects/Exams");
                        $scope.clear();
                    }
                    console.log($scope.subject_list);
                    //$scope.class_list = promise.classlist;
                    //$scope.ASMCL_Id = "";
                    //$scope.ASMS_Id = "";
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
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.exportToExcel = function (tableIds) {
            
            var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }
    }
})();
