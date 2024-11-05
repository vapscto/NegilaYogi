(function () {
    'use strict';

    angular
        .module('app')
        .controller('MarksEntry_SController', MarksEntry_SController);

    MarksEntry_SController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window'];

    function MarksEntry_SController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window) {
        /* jshint validthis:true */
        //var vm = this;
        //vm.title = 'MarksEntry_SController';
        // $scope.all = {};
        //activate();
        // $scope.all_s = {};
        //function activate() { }

        //$scope.gridOptions = {

        //    enableColumnMenus: false,
        //    enableFiltering: true,
        //    columnDefs: [
        //           { name: 'SLNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
        //      { name: 'amsT_FirstName', displayName: 'Student Name' },
        //      { name: 'amsT_AdmNo', displayName: 'Admission No' },
        //      { name: 'amaY_RollNo', displayName: 'Roll No', type: 'number' },
        //        { name: 'eyceS_MaxMarks', displayName: 'Max Marks' },
        //        { name: 'eyceS_MinMarks', displayName: 'Min Marks' },
        //        { name: 'eyceS_MarksEntryMax', displayName: 'Marks Enter For' },
        //        { name: 'estM_Marks', displayName: 'Obtain Marks', cellTemplate: '<div class="ui-grid-cell-contents"><input type="text" ng-model="row.entity.obtainmarks"  style="text-align:center;" ng-blur="grid.appScope.changemarks(row.entity.marksEnterFor,row.entity.obtainmarks,row)"  class="form-control" value="0"  min="0" </input></div>' }

        //    ]

        //};
        //$scope.gridOptions.onRegisterApi = function (gridApi) {
        //    $scope.gridApi = gridApi;
        //};

        $scope.BindData = function () {
            apiService.getDATA("MarksEntry_S/Getdetails").
       then(function (promise) {
           $scope.year_list = promise.yearlist;
       })
        };
        $scope.get_classes = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("MarksEntry_S/get_classes", data).
       then(function (promise) {
           $scope.class_list = promise.classlist;
           $scope.ASMCL_Id = "";
           $scope.ASMS_Id = "";
           $scope.EME_Id = "";
           $scope.ISMS_Id = "";
           $scope.section_list = [];
           $scope.exam_list = [];
           $scope.subject_list = [];
           $scope.temp_student_list_S = [];

       })
        };
        $scope.get_sections = function (ASMCL_Id, ASMAY_Id) {

            var data = {
                "ASMAY_Id": ASMAY_Id,
                "ASMCL_Id": ASMCL_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("MarksEntry_S/get_sections", data).
       then(function (promise) {
           $scope.section_list = promise.sectionlist;
           $scope.ASMS_Id = "";
           $scope.EME_Id = "";
           $scope.ISMS_Id = "";
           $scope.exam_list = [];
           $scope.subject_list = [];
           $scope.temp_student_list_S = [];

       })
        };

        $scope.get_exams = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("MarksEntry_S/get_exams", data).
       then(function (promise) {

           $scope.EME_Id = "";
           $scope.ISMS_Id = "";
           $scope.exam_list = promise.examlist;
           $scope.subject_list = [];
           $scope.temp_student_list_S = [];
           //$scope.subjectlist = promise.subjectlist;

           //$scope.ISMS_Id = "";
       })
        };
        $scope.get_subjects = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id) {
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EME_Id": EME_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("MarksEntry_S/get_subjects", data).
       then(function (promise) {
           $scope.subject_list = promise.subjectlist;
           $scope.ISMS_Id = "";
           $scope.temp_student_list_S = [];
       })
        };

        $scope.onsearch = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.all_s = false;
            $scope.submitted = true;
            Temp_subs_marks_list = [];
            if ($scope.myForm.$valid) {
                //if ($scope.ASMCL_Id == 'classdefualt' || $scope.ASMS_Id == 'sectiondefualt' || $scope.EME_Id == 'examdefualt' || $scope.ISMS_Id == 'subjectdefualt') {

                //    if ($scope.ASMCL_Id == 'classdefualt') {
                //        swal('Please Select Class');
                //        return;
                //    }
                //    else if ($scope.ASMS_Id == 'sectiondefualt') {
                //        swal('Please Select Section');
                //        return;
                //    }
                //    else if ($scope.EME_Id == 'examdefualt') {
                //        swal('Please Select Exam');
                //        return;
                //    }
                //    else if ($scope.ISMS_Id == 'subjectdefualt') {
                //        swal('Please Select Subject');
                //        return;
                //    }

                //}
                // else {
                var data = {
                    "ASMS_Id": ASMS_Id,
                    "ASMCL_Id": ASMCL_Id,
                    "ASMAY_Id": ASMAY_Id,
                    "EME_Id": EME_Id,
                    "ISMS_Id": ISMS_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MarksEntry_S/onsearch", data).
               then(function (promise) {
                   if (promise.studentList != null && promise.studentList.length > 0) {
                       if ((!promise.eyceS_SubSubjectFlg && !promise.eyceS_SubExamFlg) || ((!promise.eyceS_SubSubjectFlg && (promise.eyceS_SubExamFlg && promise.subject_subexams != null && promise.subject_subexams.length == 0)) && (!promise.eyceS_SubExamFlg || (promise.eyceS_SubSubjectFlg && promise.subject_subsubjects != null && promise.subject_subsubjects.length == 0)))) {
                           $scope.temp_student_list_S = promise.studentList;
                           $scope.marksdeleteflag = promise.marksdeleteflag;
                           $scope.subject_details = promise.subject_details;
                           $scope.eyceS_SubSubjectFlg = promise.eyceS_SubSubjectFlg;
                           $scope.eyceS_SubExamFlg = promise.eyceS_SubExamFlg;
                           $scope.eyceS_MarksGradeEntryFlg = promise.eyceS_MarksGradeEntryFlg;

                           if (promise.eyceS_MarksGradeEntryFlg == 'M') {
                              // $scope.ngpattern = /^[0-9]{0,4}\.?[0-9]{1,2}?$/;
                              // $scope.allowpattern = "[0-9.]";
                               $scope.placeholder = "Enter Marks...";
                           }
                           else if (promise.eyceS_MarksGradeEntryFlg == 'G') {
                              // $scope.ngpattern = "";
                             //  $scope.allowpattern = "[a-zA-Z+-]";
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
                               })
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
                           })

                           if (promise.eyceS_SubSubjectFlg && promise.eyceS_SubExamFlg) {
                               $scope.temp_sub_subjs_exams = [];
                               angular.forEach(promise.subject_subsubjects, function (sub_s) {
                                   angular.forEach(promise.subject_subexams, function (sub_e) {

                                       $scope.temp_sub_subjs_exams.push({ sub_subject: sub_s, sub_exam: sub_e });
                                   })
                               })
                           }

                           console.log($scope.col_span);
                           if (promise.eyceS_SubSubjectFlg) {
                               $scope.subject_subsubjects_details = promise.subject_subsubjects;
                           }
                           if (promise.eyceS_SubExamFlg) {
                               $scope.subject_subexams_details = promise.subject_subexams;
                           }
                           if (promise.eyceS_MarksGradeEntryFlg == "G") {
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


                           // $scope.gridOptions.data = promise.studentList;
                           //$scope.subMorGFlag = promise.subMorGFlag;
                           //  $scope.gradname = promise.gradname;



                           if (promise.saved_studentList != null && promise.saved_studentList.length > 0) {
                               $scope.map_marks(promise.saved_studentList);
                           }
                       }
                       else {
                           swal("Select Correct Type of Subject");
                           $scope.clear();
                       }

                   }
                   else {
                       swal("This Subject Is Not Mapped With Students");
                       $scope.ISMS_Id = "";
                   }



               })
                //  }
            }
        };
        $scope.map_marks = function (saved_studentList) {
            angular.forEach(saved_studentList, function (stu_s) {
                angular.forEach($scope.temp_student_list_S, function (stu_m) {
                    if (stu_m.amsT_Id == stu_s.amsT_Id) {
                        stu_m.selected_s = true;
                        $scope.optionToggled();

                        angular.forEach($scope.subject_details, function (sub) {
                            if (stu_m.ismS_Id == stu_s.ismS_Id && stu_m.ismS_Id == sub.ismS_Id) {
                                if ($scope.eyceS_MarksGradeEntryFlg == 'M')
                                {
                                    if (stu_s.estM_Flg == '' || stu_s.estM_Flg == null) {
                                        stu_m.ESTM_Marks = stu_s.estM_Marks.toString();
                                    }
                                    else {
                                        stu_m.ESTM_Marks = stu_s.estM_Flg.toString();
                                    }
                                }
                                else if ($scope.eyceS_MarksGradeEntryFlg == 'G') {
                                    if (stu_s.estM_Flg == '' || stu_s.estM_Flg == null) {
                                        stu_m.ESTM_Marks = stu_s.estM_Grade.toString();
                                    }
                                    else {
                                        stu_m.ESTM_Marks = stu_s.estM_Flg.toString();
                                    }
                                }
                               
                                $scope.valid_marks(sub, sub.ismS_Id, stu_m.ESTM_Marks, stu_m.amsT_Id, stu_m);
                            }
                        })

                    }
                })
            })
        }

        $scope.clear = function () {
            $state.reload();
            $scope.BindData();
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.toggleAll_S = function (stas) {
            // var toggleStatus = $scope.all_s;
            var toggleStatus = $scope.all_s;
            angular.forEach($scope.temp_student_list_S, function (itm) {
                itm.selected_s = toggleStatus;

            });
        }
        $scope.optionToggled = function () {
            
            $scope.all_s = $scope.temp_student_list_S.every(function (itm)
            { return itm.selected_s; });

            //  $scope.all_s = $scope.temp_student_list_S.every(function (itm) { return itm.selected_s; })
        }
        var Temp_subs_marks_list = [];
        $scope.valid_marks = function (obj_s, subj_id, values, stu_id, obj_student) {//, totalMarks, obtainmarks, row
            
            if ($scope.eyceS_MarksGradeEntryFlg == "G") {
                
                var flag = "false";
                var obtainmarks = values;
                if (obtainmarks != undefined && obtainmarks != null && obtainmarks != "") {

                    //if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                    if (obtainmarks.toUpperCase() == "AB" || obtainmarks.toUpperCase() == "L" || obtainmarks.toUpperCase() == "M") {
                        flag = "true";
                        if (Temp_subs_marks_list.length > 0) {
                            var al_cnt = 0;
                            angular.forEach(Temp_subs_marks_list, function (yet) {
                                if (yet.AMST_Id == stu_id && yet.ISMS_Id == subj_id) {
                                    yet.ESTM_Flg = obtainmarks;
                                   // yet.ESTM_Marks = 0;
                                    yet.ESTM_Grade = null;
                                    al_cnt += 1;
                                }

                            })
                            if (al_cnt == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Flg: obtainmarks, ESTM_Marks: 0, ESTM_Grade: null });//ESTMSS_Marks: null,ESTMSS_Grade: null,
                            }
                        }
                        else if (Temp_subs_marks_list.length == 0) {
                            Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Flg: obtainmarks, ESTM_Marks: 0, ESTM_Grade: null });//ESTMSS_Marks: null,ESTMSS_Grade: null,
                        }

                    }
                    if (flag == "false") {
                        for (var i = 0; i < $scope.grade_details.length; i++) {
                            if ($scope.grade_details[i] == obtainmarks) {
                                flag = "true";
                                if (Temp_subs_marks_list.length > 0) {
                                    var al_cnt = 0;
                                    angular.forEach(Temp_subs_marks_list, function (yet) {
                                        if (yet.AMST_Id == stu_id && yet.ISMS_Id == subj_id) {
                                            yet.ESTM_Grade = obtainmarks;
                                            yet.ESTM_Flg = "";
                                           // yet.ESTM_Marks = 0;
                                            al_cnt += 1;
                                        }

                                    })
                                    if (al_cnt == 0) {
                                        Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Marks: 0, ESTM_Grade: obtainmarks, ESTM_Flg: "" });//, ESTMSS_Grade: null, ESTMSS_Flg: null
                                    }
                                }
                                else if (Temp_subs_marks_list.length == 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Marks: 0, ESTM_Grade: obtainmarks, ESTM_Flg: "" });//, ESTMSS_Grade: null, ESTMSS_Flg: null
                                }
                            }
                        }
                    }


                    if (flag == "false") {
                        obj_student.ESTM_Marks = "";
                        swal('Entered Grade cant be out of master setting...!');
                    }
                }
            }
            else {
                var flag = "false";
                var obtainmarks = values;
                if (obtainmarks != undefined && obtainmarks != null && obtainmarks != "") {
                    if (obtainmarks.match(/[a-z]/i)) {
                        //if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                        if (obtainmarks.toUpperCase() == "AB" || obtainmarks.toUpperCase() == "L" || obtainmarks.toUpperCase() == "M") {
                            flag = "true";
                            if (Temp_subs_marks_list.length > 0) {
                                var al_cnt = 0;
                                angular.forEach(Temp_subs_marks_list, function (yet) {
                                    if (yet.AMST_Id == stu_id && yet.ISMS_Id == subj_id) {
                                        yet.ESTM_Flg = obtainmarks;
                                        yet.ESTM_Marks = 0;
                                        al_cnt += 1;
                                    }

                                })
                                if (al_cnt == 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Flg: obtainmarks, ESTM_Marks: 0, ESTM_Grade: null });//ESTMSS_Marks: null,ESTMSS_Grade: null,
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Flg: obtainmarks, ESTM_Marks: 0, ESTM_Grade: null });//ESTMSS_Marks: null,ESTMSS_Grade: null,
                            }

                        }
                        if (flag == "false") {
                            // values = "";
                            obj_student.ESTM_Marks = "";
                            swal('Entered value cant be out of master setting...!');
                        }
                    }
                    else {
                        var totalMarks = 0;

                        if (Number(obj_student.eyceS_MaxMarks) > Number(obj_student.eyceS_MarksEntryMax)) {
                            totalMarks = Number(obj_student.eyceS_MarksEntryMax);
                        }
                        else {
                            totalMarks = Number(obj_student.eyceS_MaxMarks);
                        }

                        obtainmarks = Number(obtainmarks);
                        if (totalMarks < obtainmarks) {
                            //values = "";
                            obj_student.ESTM_Marks = "";
                            swal('Entered marks cant be more than Max Marks ...!' + totalMarks);
                        }
                        else if (obtainmarks < 0) {
                            //values = "";
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

                                })
                                if (al_cnt == 0) {
                                    Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Marks: obtainmarks, ESTM_Grade: null, ESTM_Flg: "" });//, ESTMSS_Grade: null, ESTMSS_Flg: null
                                }
                            }
                            else if (Temp_subs_marks_list.length == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: stu_id, ISMS_Id: subj_id, ESTM_MarksGradeFlg: $scope.eyceS_MarksGradeEntryFlg, ESTM_Marks: obtainmarks, ESTM_Grade: null, ESTM_Flg: "" });//, ESTMSS_Grade: null, ESTMSS_Flg: null
                            }

                        }
                    }
                }


            }
        };




        $scope.SaveMarks = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
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
                        $scope.save(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                    }
                    else {
                        flag = "false"

                    }

                });
                }
                else {
                    $scope.save(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                }


            }
            else {
                swal('Please select required field / Entered Marks are not in correct format....!');
            }
        };

        $scope.save = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            var main_save_list = [];
            if ($scope.myForm.$valid) {

                angular.forEach($scope.temp_student_list_S, function (stu) {
                    if (stu.selected_s) {
                        //if ($scope.eyceS_SubSubjectFlg)
                        //{
                        //    //var ESTM_Marks = 0;
                        //    var others_cnt = 0;
                        //    var ESTM_Flg="";
                        //    var subject_max_marks = stu.eyceS_MaxMarks;
                        //    var subject_marks_entry_for = stu.eyceS_MarksEntryMax;
                        //    var sum_s_sub_max_marks = 0;
                        //    var sum_s_sub_obt_marks = 0;
                        //    angular.forEach($scope.subject_subsubjects_details, function (s_ss) {
                        //        if ($scope.eyceS_SubExamFlg) {
                        //            var S_subs_max_marks = s_ss.eycessS_MaxMarks;
                        //            sum_s_sub_max_marks += S_subs_max_marks;
                        //            var subject_marks = 0;
                        //            var sum_s_exm_max_marks = 0;
                        //            var sum_s_exm_obt_marks = 0;
                        //            angular.forEach($scope.subject_subexams_details, function (s_se) {
                        //                if (stu.ESTMSS_Marks[s_ss.emsS_Id][s_se.emsE_Id][stu.amsT_Id].match(/[a-z]/i)) {
                        //                    others_cnt += 1;
                        //                    ESTM_Flg=stu.ESTMSS_Marks[s_ss.emsS_Id][s_se.emsE_Id][stu.amsT_Id];
                        //                }
                        //                else {
                        //                    //sum_s_exm_max_marks += s_se.eycessE_MaxMarks;
                        //                    sum_s_exm_obt_marks += Number(stu.ESTMSS_Marks[s_ss.emsS_Id][s_se.emsE_Id][stu.amsT_Id]);
                        //                }
                        //                sum_s_exm_max_marks += s_se.eycessE_MaxMarks;
                        //            })
                        //            var ratio_s_exm = (sum_s_exm_max_marks / S_subs_max_marks);
                        //            var S_subj_marks = sum_s_exm_obt_marks / ratio_s_exm;
                        //            sum_s_sub_obt_marks += S_subj_marks;
                        //        }
                        //    })
                        //    if (stu.eyceS_MaxMarks == stu.eyceS_MarksEntryMax) {
                        //        var ratio_s_sub = (sum_s_sub_max_marks / subject_max_marks);
                        //        var ESTM_Marks = sum_s_sub_obt_marks / ratio_s_sub;
                        //        stu.estM_Marks = ESTM_Marks;
                        //        stu.estM_Flg = ESTM_Flg;
                        //        //if (others_cnt == 0) {
                        //        //    stu.estM_Marks = ESTM_Marks;
                        //        //}
                        //        //else {

                        //        //    stu.estM_Flg = ESTM_Flg;
                        //        //}
                        //    }
                        //    else if (stu.eyceS_MaxMarks > stu.eyceS_MarksEntryMax)
                        //    {
                        //        var ratio_s_sub = (sum_s_sub_max_marks / subject_marks_entry_for);
                        //        var ESTM_Marks = sum_s_sub_obt_marks / ratio_s_sub;
                        //        stu.estM_Marks = ESTM_Marks;
                        //        stu.estM_Flg = ESTM_Flg;
                        //        //if (others_cnt == 0) {
                        //        //    stu.estM_Marks = ESTM_Marks;
                        //        //}
                        //        //else {

                        //        //    stu.estM_Flg = ESTM_Flg;
                        //        //}
                        //    }



                        //    //angular.forEach($scope.temp_sub_subjs_exams, function (s_ssse) {


                        //    //})

                        //}

                        var ESTM_Marks = 0;
                        var ESTM_Flg = "";
                        var ESTM_Grade = "";
                        angular.forEach(Temp_subs_marks_list, function (stu_t) {
                            if (stu_t.AMST_Id == stu.amsT_Id && stu_t.ISMS_Id == stu.ismS_Id) {
                                ESTM_Marks = stu_t.ESTM_Marks;
                                ESTM_Flg = stu_t.ESTM_Flg;
                                ESTM_Grade = stu_t.ESTM_Grade;
                            }

                        })


                        main_save_list.push({ AMST_Id: stu.amsT_Id, AMST_FirstName: stu.amsT_FirstName, AMST_AdmNo: stu.amsT_AdmNo, AMAY_RollNo: stu.amaY_RollNo, ISMS_SubjectName: stu.ismS_SubjectName, ISMS_Id: stu.ismS_Id, EYCES_MaxMarks: stu.eyceS_MaxMarks, EYCES_MarksEntryMax: stu.eyceS_MarksEntryMax, EYCES_MinMarks: stu.eyceS_MinMarks, ESTM_Marks: ESTM_Marks, ESTM_Flg: ESTM_Flg, ESTM_Grade: ESTM_Grade })
                    }
                })
                if (main_save_list.length > 0) {
                    //  $scope.savedata = $scope.gridOptions.data;
                    var data = {
                        main_save_list: main_save_list,
                        // Temp_subs_marks_list: Temp_subs_marks_list,
                        "ASMS_Id": ASMS_Id,
                        "ASMCL_Id": ASMCL_Id,
                        "ASMAY_Id": ASMAY_Id,
                        "EME_Id": EME_Id,
                        "ISMS_Id": ISMS_Id,
                        "EYCES_MarksGradeEntryFlg": $scope.eyceS_MarksGradeEntryFlg,
                        // "marksdeleteflag": $scope.marksdeleteflag,
                        // "detailsList": $scope.savedata
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("MarksEntry_S/SaveMarks", data).
               then(function (promise) {


                   if (promise.returnval == true) {
                       swal('Data Saved Successfully');
                   }
                   else if (promise.returnval == false) {
                       swal('Failed to Save/Update Data');
                   }
                   $scope.clear();
                   // $scope.BindData();
                   //if (promise.messagesaveupdate == "true") {
                   //    $scope.cancle();
                   //    //$scope.BindData();
                   //    swal('Data Saved Successfully');
                   //}
                   //else {
                   //    swal('Failed to Save/Update Data');
                   //}

               })
                }
                else if (main_save_list.length == 0) {
                    swal("Select Students For Saving Marks...");
                }


            }
        };
        var HostName = location.host;
        $scope.get_form = function (subj_type) {
            if (subj_type == 'SSSE') {
                $window.location.href = 'http://' + HostName + '/#/app/MarksEntry_SSSE/';
            }
            else if (subj_type == 'SS') {
                $window.location.href = 'http://' + HostName + '/#/app/MarksEntry_SS/';
            }
            else if (subj_type == 'SE') {
                $window.location.href = 'http://' + HostName + '/#/app/MarksEntry_SE/';
            }
            else if (subj_type == 'S') {
                $window.location.href = 'http://' + HostName + '/#/app/MarksEntry_S/';
            }

        };

    }
})();
