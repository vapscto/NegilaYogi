
(function () {
    'use strict';
    angular.module('app').controller('ClgExamSubjectWizardController', ClgExamSubjectWizardController)

    ClgExamSubjectWizardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter']
    function ClgExamSubjectWizardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.select_cat = false;
        var temp_saved_subs = [];
        $scope.submitted1 = false;
        var temp_saved_subs = [];
        $scope.sortKey = 'ismS_OrderFlag';
        $scope.sortReverse = false;
        $scope.submitted2 = false;        
        $scope.SubWise_Selected_subexms_list = [];
        $scope.submitted = false;
        $scope.exam_check = false;
        $scope.subject_check = false;

        $scope.BindData = function () {
            apiService.getDATA("ClgExamSubjectWizard/Getdetails").then(function (promise) {
                $scope.course_listdd = promise.courseslist;
                $scope.subjectschema_list = promise.subjectshemalist;
                $scope.subjectgrp_list = promise.subjectgrplist;
                $scope.branch_list = promise.branchlist;
                $scope.schmetype_list = promise.schmetypelist;
                $scope.semisters_list = promise.semisters;
                $scope.grade_list = promise.gradelist;
                $scope.exam_list = promise.examlist;
                $scope.gridOptions.data = promise.scheme_exams;
                $scope.ECYSE_Id = 0;
                $scope.ECYSES_Id = 0;
                $scope.tempexamlist = promise.examlist;
                angular.forEach(promise.subexamlist, function (opt) {
                    opt.ECYCESSS_SubExamOrder = opt.emsE_SubExamOrder;
                });
                $scope.subexam_list = promise.subexamlist;
                angular.forEach(promise.subsubjectlist, function (opt) {
                    opt.ECYSESSS_SubSubjectOrder = opt.emsS_Order;
                });
                $scope.tempsubsubjectlist = promise.subsubjectlist;
                $scope.tempsubexamlist = promise.subexamlist;

                $scope.tempsubsubjectsubexamlist = promise.subsubjectsubexamlist;

                angular.forEach($scope.subject_list, function (opt) {
                    opt.ISMS_Id = opt.ismS_Id;
                    opt.ECYSES_MarksGradeEntryFlg = 'M';
                    opt.ECYSES_MarksDisplayFlg = true;
                    opt.ECYSES_AplResultFlg = true;
                    opt.ECYSES_SubjectOrder = opt.ismS_OrderFlag;
                });
                $scope.tempsubjectlist = promise.subjectlist;
                $scope.all = true;
                $scope.toggleAll();
            });
        };

        $scope.getbranch = function () {
            $scope.AMB_Id = "";
            var data = {
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("ClgExamSubjectWizard/getbranch", data).then(function (promise) {
                $scope.branch_list = promise.branchlist;
            });
        };

        $scope.getsemester = function () {
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("ClgExamSubjectWizard/getsemester", data).then(function (promise) {
                $scope.semisters_list = promise.semisters;
            });
        };

        $scope.getsubjectscheme = function () {
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("ClgExamSubjectWizard/getsubjectscheme", data).then(function (promise) {
                $scope.subjectschema_list = promise.subjectshemalist;
            });
        };

        $scope.getsubjectschemetype = function () {
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id
            };
            apiService.create("ClgExamSubjectWizard/getsubjectschemetype", data).then(function (promise) {
                $scope.schmetype_list = promise.schmetypelist;
            });
        };

        $scope.getsubjectgroup = function () {
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "ACST_Id": $scope.ACST_Id
            };
            apiService.create("ClgExamSubjectWizard/getsubjectgroup", data).then(function (promise) {
                $scope.subjectgrp_list = promise.subjectgrplist;
            });
        };

        $scope.get_subjects = function () {
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "ACST_Id": $scope.ACST_Id
                //"EMG_Id": $scope.EMG_Id,
            }
            apiService.create("ClgExamSubjectWizard/get_subjects", data).then(function (promise) {
                $scope.subject_list = promise.subjectgroups;
                $scope.select_cat = true;
                $scope.exam_list = promise.examlist;

                angular.forEach($scope.subject_list, function (opt) {
                    opt.ISMS_Id = opt.ismS_Id;
                    opt.ECYSES_MarksGradeEntryFlg = 'M';
                    opt.ECYSES_MarksDisplayFlg = true;
                    opt.ECYSES_AplResultFlg = true;
                    opt.ECYSES_SubjectOrder = opt.ismS_OrderFlag;
                    opt.EMGR_Id = $scope.EMGR_Id;
                    for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) {
                        if (opt.ismS_Id == $scope.SubWise_Selected_subexms_list[i].ISMS_Id) {
                            opt.ECYSES_SubExamFlg = true;
                            opt.EYCES_SubSubjectFlgb = true;
                        }
                    }
                    for (var i = 0; i < $scope.SubWise_Selected_subsubjs_list.length; i++) {
                        if (opt.ismS_Id == $scope.SubWise_Selected_subsubjs_list[i].ISMS_Id) {
                            opt.ECYSES_SubSubjectFlg = true;
                            opt.EYCES_SubSubjectFlgb = true;
                        }
                    } for (var i = 0; i < $scope.SubWise_Selected_subsubject_subexms_list.length; i++) {
                        if (opt.ismS_Id == $scope.SubWise_Selected_subsubject_subexms_list[i].ISMS_Id) {
                            opt.ECYSES_SubSubjectFlg = true;
                            opt.EYCES_SubSubjectFlgb = true;
                            opt.ECYSES_SubExamFlg = true;
                        }
                    }
                });
                if ($scope.ECYSE_Id > 0) {
                    angular.forEach(temp_saved_subs, function (sy) {
                        angular.forEach($scope.subject_list, function (sy1) {
                            if (sy.ismS_Id == sy1.ismS_Id) {
                                sy1.ismS_OrderFlag = sy.ecyseS_SubjectOrder;
                                sy1.ECYSES_SubjectOrder = sy1.ismS_OrderFlag;
                            }
                        })
                    });
                }
                $scope.all = true;
                $scope.toggleAll();
                if ($scope.ECYSE_Id != "" && $scope.ECYSE_Id != 0 && $scope.ECYSE_Id != undefined) {
                    //angular.forEach($scope.tempexamlist, function (role) {
                    //    if (role.emE_Id == $scope.temp_exm) {
                    //        $scope.exam_list.push(role);
                    //    }
                    //})
                    angular.forEach($scope.exam_list, function (role) {
                        if (role.emE_Id == $scope.temp_exm) {
                            role.checked = true;
                        }
                    });

                    angular.forEach($scope.subject_list, function (role) {
                        var exm_subject_cnt = 0;
                        angular.forEach($scope.selected_exm_subjects, function (itm) {
                            if (role.ismS_Id == itm.ismS_Id) {
                                role.checkedvalue = true;
                                role.ECYSES_MaxMarks = itm.ecyseS_MaxMarks;
                                role.ECYSES_MinMarks = itm.ecyseS_MinMarks;
                                role.ECYSES_MarksEntryMax = itm.ecyseS_MarksEntryMax;
                                role.ECYSES_SubExamFlg = itm.ecyseS_SubExamFlg;
                                role.ECYSES_SubSubjectFlg = itm.ecyseS_SubSubjectFlg;
                                role.ECYSES_MarksGradeEntryFlg = itm.ecyseS_MarksGradeEntryFlg;
                                role.ECYSES_MarksDisplayFlg = itm.ecyseS_MarksDisplayFlg;
                                role.ECYSES_GradeDisplayFlg = itm.ecyseS_GradeDisplayFlg;
                                role.ECYSES_AplResultFlg = itm.ecyseS_AplResultFlg;
                                role.EMGR_Id = itm.emgR_Id;
                                exm_subject_cnt += 1;
                            }
                        });
                        if (exm_subject_cnt == 0) {
                            role.checkedvalue = false;
                            $scope.optionToggled();
                        }
                    });
                    console.log($scope.subject_list);
                }
                if (promise.subjectgroups == null || promise.subjectgroups == "") {
                    swal("Subjects are Not Mapped To Selected Group!!!");
                    $scope.select_cat = false;
                }
                if (promise.examlist == null || promise.examlist == "") {
                    swal("All Exams are  Mapped To Selected Group !!!");
                }
            });
        };

        $scope.get_gradename = function (grd) {
            angular.forEach($scope.subject_list, function (itm) {
                itm.EMGR_Id = grd;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.exam_list.some(function (options) {
                return options.checked;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subject_list, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        };

        $scope.valid_from_date = function (from_date) {
            if ($scope.EMGR_Id != "" && $scope.EMGR_Id != null && $scope.EMGR_Id != undefined) {
                $scope.ECYSE_AttendanceToDate = "";
            }
            else {
                swal("First Select Exam Master Group !!!");
                $scope.ECYSE_AttendanceFromDate = "";
            }
        };

        $scope.valid_to_date = function (to_date) {
            if ($scope.ECYSE_AttendanceFromDate != "" && $scope.ECYSE_AttendanceFromDate != null && $scope.ECYSE_AttendanceFromDate != undefined) {
                // $scope.EYCE_AttendanceToDate = "";
            }
            else {
                swal("First Select Attendance From Date !!!");
                $scope.ECYSE_AttendanceToDate = "";
            }
        };
        
        $scope.saveddata = function () {
            $scope.submitted = true;
            $scope.ECYSE_AttendanceFromDate = new Date($scope.ECYSE_AttendanceFromDate).toDateString();
            $scope.ECYSE_AttendanceToDate = new Date($scope.ECYSE_AttendanceToDate).toDateString();
            $scope.exam_list_saved = [];

            $scope.subject_list_saved = [];
            if ($scope.ECYSE_SubExamFlg == undefined)
                $scope.ECYSE_SubExamFlg = false;
            if ($scope.ECYSE_SubSubjectFlg == undefined)
                $scope.ECYSE_SubSubjectFlg = false;

            angular.forEach($scope.exam_list, function (opt123) {
                if (opt123.checked) {
                    $scope.exam_list_saved.push(opt123);
                }
            });

            angular.forEach($scope.subject_list, function (opt123) {
                if (opt123.checkedvalue) {
                    //MB For New
                    if (!opt123.ECYSES_SubExamFlg) {
                        opt123.ECYSES_SubExamFlg = false;
                    }
                    if (!opt123.ECYSES_SubSubjectFlg) {
                        opt123.ECYSES_SubSubjectFlg = false;
                    }
                    //MB For New
                    $scope.subject_list_saved.push(opt123);
                }
            });

            if ($scope.myForm.$valid) {
                var data = {
                    "AMCO_Id": $scope.AMCO_Id,
                    "ACSS_Id": $scope.ACSS_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "ACST_Id": $scope.ACST_Id,
                    //"EMG_Id": $scope.EMG_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ECYSE_Id": $scope.ECYSE_Id,
                    "ECYSES_Id": $scope.ECYSES_Id,
                    "exams_list": $scope.exam_list_saved,
                    "EMGR_Id": $scope.EMGR_Id,
                    "ECYSE_AttendanceFromDate": $scope.ECYSE_AttendanceFromDate,
                    "ECYSE_AttendanceToDate": $scope.ECYSE_AttendanceToDate,
                    "ECYSE_SubExamFlg": $scope.ECYSE_SubExamFlg,
                    "ECYSE_SubSubjectFlg": $scope.ECYSE_SubSubjectFlg,
                    "exm_subjects_list": $scope.subject_list_saved,
                    "exm_subject_subexams_list": $scope.SubWise_Selected_subexms_list,
                    "exm_subject_subsubjects_list": $scope.SubWise_Selected_subsubjs_list,
                    "exm_subject_subsubjects_subexam": $scope.SubWise_Selected_subsubject_subexms_list
                }
                apiService.create("ClgExamSubjectWizard/savedetails", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal('Record saved/Updated successfully');
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        swal('Record Failed To save/Update');
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clear = function () {
            $scope.AMCO_Id = "";
            $scope.ACSS_Id = "";
            $scope.AMB_Id = "";
            $scope.ACST_Id = "";
            $scope.EMG_Id = "";
            $scope.AMSE_Id = "";
            $scope.ECYSE_Id = "";
            $scope.ECYSES_Id = "";
            $scope.exam_list = $scope.tempexamlist;
            $scope.select_cat = false;
            $scope.subject_list = $scope.tempsubjectlist;
            $scope.subexam_list = $scope.tempsubexamlist;
            $scope.subsubject_list = $scope.tempsubsubjectlist;
            angular.forEach($scope.exam_list, function (itm1) {
                itm1.checked = false;
            })
            $scope.EMGR_Id = "";
            $scope.ECYSE_AttendanceFromDate = "";
            $scope.ECYSE_AttendanceToDate = "";
            $scope.ECYSE_SubExamFlg = false;
            $scope.ECYSE_SubSubjectFlg = false;
            angular.forEach($scope.subject_list, function (itm1) {
                itm1.ECYCES_SubExamFlg = false;
                itm1.ECYCES_SubSubjectFlg = false;
                itm1.ECYCES_MarksGradeEntryFlg = 'M';
                itm1.ECYCES_MarksDisplayFlg = true;
                itm1.ECYCES_GradeDisplayFlg = false;
                itm1.ECYCES_AplResultFlg = true;
                itm1.EMGR_Id = "";
            })
            $scope.all = true;
            $scope.toggleAll();
            $scope.ECYSE_Id = 0;
            $scope.ECYSES_Id = 0;
            $scope.SubWise_Selected_subexms_list = [];
            $scope.SubWise_Selected_subsubjs_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $scope.search = "";
            $scope.clear1();
            $scope.clear2();
        };

        $scope.select_subexms = function (sel_subexms, user) {
            $scope.subexam_list = $scope.tempsubexamlist;
            $scope.Subject_Name = user.ismS_SubjectName;
            $scope.ismS_Id = user.ismS_Id;
            $scope.Subj_Max_Marks = user.ECYSES_MaxMarks;
            $scope.Subj_Min_Marks = user.ECYSES_MinMarks;
            var count = 0;
            for (var a = 0; a < $scope.SubWise_Selected_subexms_list.length; a++) {
                var f = $scope.SubWise_Selected_subexms_list[a].ISMS_Id;
                if ($scope.SubWise_Selected_subexms_list[a].ISMS_Id == user.ismS_Id) {
                    count += 1;
                    for (var b = 0; b < $scope.subexam_list.length; b++) {
                        var subexam_count = 0;
                        for (var c = 0; c < $scope.SubWise_Selected_subexms_list[a].sub_exam_list.length; c++) {
                            if ($scope.subexam_list[b].emsE_Id == $scope.SubWise_Selected_subexms_list[a].sub_exam_list[c].EMSE_Id) {
                                var itm = $scope.subexam_list[b];
                                var itm1 = $scope.SubWise_Selected_subexms_list[a].sub_exam_list[c];
                                subexam_count += 1;
                                itm.checkedvalue = true;
                                itm.EYCESSE_MaxMarks = itm1.EYCESSE_MaxMarks;
                                itm.EYCESSE_MinMarks = itm1.EYCESSE_MinMarks;
                                itm.EMGR_Id = itm1.EMGR_Id;
                                itm.EYCESSE_ExemptedFlg = itm1.EYCESSE_ExemptedFlg;
                                itm.EYCESSE_ExemptedPer = itm1.EYCESSE_ExemptedPer;
                                $scope.optionToggled2();
                            }
                        }
                        if (subexam_count == 0) {
                            var itm = $scope.subexam_list[b];
                            itm.checkedvalue = false;
                            itm.EYCESSE_MaxMarks = "";
                            itm.EYCESSE_MinMarks = "";
                            itm.EMGR_Id = "";
                            itm.EYCESSE_ExemptedFlg = false;
                            itm.EYCESSE_ExemptedPer = "";
                            $scope.optionToggled2();
                        }
                    }
                }
            }
            if (sel_subexms == true) {
                $scope.exam_check = true;
                if (count == 0) {
                    $scope.clear2();
                }
                $('#popup5').modal('show');
            } else if (sel_subexms == false) {
                $scope.exam_check = false;
                swal({
                    title: "Are you sure",
                    text: "Do you want to Delete SubExams Mapped to " + $scope.Subject_Name + " Subject???",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete Selection!",
                    cancelButtonText: "No, Change Selection!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) {
                                if ($scope.SubWise_Selected_subexms_list[i].ISMS_Id == user.ismS_Id) {
                                    $scope.SubWise_Selected_subexms_list.splice(i, 1);
                                }
                            }
                            swal('Deleted Successfully');
                        }
                        else {
                            swal("Now You Can Change Selection");
                            angular.forEach($scope.subject_list, function (itm) {
                                if (itm.ismS_Id == user.ismS_Id) {
                                    itm.ECYSES_SubExamFlg = true;
                                    itm.EYCES_SubSubjectFlgb = true;
                                    $('#popup5').modal('show');
                                }
                            })
                        }
                    });
            }
        };

        $scope.select_subsubjs = function (sel_subsubjs, user) {

            $scope.subsubject_list = $scope.tempsubsubjectlist;
            var count = 0;
            for (var a = 0; a < $scope.SubWise_Selected_subsubjs_list.length; a++) {
                if ($scope.SubWise_Selected_subsubjs_list[a].ISMS_Id == user.ismS_Id) {
                    count += 1;
                    for (var b = 0; b < $scope.subsubject_list.length; b++) {
                        var subsubj_count = 0;
                        for (var c = 0; c < $scope.SubWise_Selected_subsubjs_list[a].sub_subjs_list.length; c++) {
                            if ($scope.subsubject_list[b].emsS_Id == $scope.SubWise_Selected_subsubjs_list[a].sub_subjs_list[c].EMSS_Id) {
                                var itm = $scope.subsubject_list[b];
                                var itm1 = $scope.SubWise_Selected_subsubjs_list[a].sub_subjs_list[c];
                                subsubj_count += 1;
                                itm.checkedvalue = true;
                                itm.ECYSESSS_MaxMarks = itm1.ECYSESSS_MaxMarks;
                                itm.ECYSESSS_MinMarks = itm1.ECYSESSS_MinMarks;
                                itm.EMGR_Id = itm1.EMGR_Id;
                                itm.ECYSESSS_ExemptedFlg = itm1.ECYSESSS_ExemptedFlg;
                                itm.ECYSESSS_ExemptedPer = itm1.ECYSESSS_ExemptedPer;
                                $scope.optionToggled1();
                            }
                        }
                        if (subsubj_count == 0) {
                            var itm = $scope.subsubject_list[b];
                            itm.checkedvalue = false;
                            itm.ECYSESSS_MaxMarks = "";
                            itm.ECYSESSS_MinMarks = "";
                            itm.EMGR_Id = "";
                            itm.ECYSESSS_ExemptedFlg = false;
                            itm.ECYSESSS_ExemptedPer = "";
                            $scope.optionToggled1();
                        }
                    }
                }
            };

            $scope.Subject_Name = user.ismS_SubjectName;
            $scope.ismS_Id = user.ismS_Id;
            $scope.Subj_Max_Marks = user.ECYSES_MaxMarks;
            $scope.Subj_Min_Marks = user.ECYSES_MinMarks;

            if (sel_subsubjs == true) {
                $scope.subject_check = true;
                if (count == 0) {
                    $scope.clear1();
                }
                $('#popup3').modal('show');
            } else if (sel_subsubjs == false) {
                $scope.subject_check = false;
                swal({
                    title: "Are you sure",
                    text: "Do you want to Delete SubSubjects Mapped to " + $scope.Subject_Name + " Subject???",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete Selection!",
                    cancelButtonText: " cat, Change Selection!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            for (var i = 0; i < $scope.SubWise_Selected_subsubjs_list.length; i++) {
                                if ($scope.SubWise_Selected_subsubjs_list[i].ISMS_Id == user.ismS_Id) {
                                    $scope.SubWise_Selected_subsubjs_list.splice(i, 1);
                                }
                            }
                            swal('Deleted Successfully');
                        }
                        else {
                            swal("Now You Can Change Selection");
                            angular.forEach($scope.subject_list, function (itm) {
                                if (itm.ismS_Id == user.ismS_Id) {
                                    itm.ECYSES_SubSubjectFlg = true;
                                    itm.EYCES_SubSubjectFlgb = true;
                                    $('#popup3').modal('show');
                                }
                            })
                        }
                    });
            }
        };
        
        $scope.clearpopupgrid5 = function (subexary) {
            if ($scope.exam_check == true) {
                angular.forEach($scope.subject_list, function (itm) {
                    if (itm.ismS_Id == $scope.ismS_Id) {

                        itm.ECYSES_SubExamFlg = false;
                    }
                })
            }
            $('#popup5').modal('hide');
        };

        $scope.optionToggled2 = function () {
            $scope.all2 = $scope.subexam_list.every(function (itm) { return itm.checkedvalue; })
        };

        $scope.saveddata2 = function (exms_subs) {
            $scope.submitted2 = true;
            if ($scope.myForm2.$valid) {
                var final_count = 0;
                angular.forEach(exms_subs, function (itm) {
                    if (itm.checkedvalue) {
                        final_count += 1;
                    }
                });

                if (final_count == 0) {
                    angular.forEach($scope.subject_list, function (opt786) {
                        if ($scope.ismS_Id == opt786.ismS_Id) {
                            for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) {
                                var already_count = 0;
                                if ($scope.ismS_Id == $scope.SubWise_Selected_subexms_list[i].ISMS_Id) {
                                    already_count += 1;
                                    if (already_count > 0) {
                                        $scope.SubWise_Selected_subexms_list.splice(i, 1);
                                    }
                                }
                            }
                            opt786.ECYSES_SubExamFlg = false;
                        }
                    });

                    $('#popup5').modal('hide');
                    $scope.subexam_list = [];
                }
                else if (final_count > 0) {
                    var Subj_subexams_max_total = 0;
                    var Subj_subexams_min_total = 0;
                    angular.forEach(exms_subs, function (itm) {
                        if (itm.checkedvalue) {
                            Subj_subexams_max_total += Number(itm.ECYSESSS_MaxMarks);
                            Subj_subexams_min_total += Number(itm.ECYSESSS_MinMarks);
                        }
                    });

                    if (Subj_subexams_max_total == $scope.Subj_Max_Marks && Subj_subexams_min_total == $scope.Subj_Min_Marks) {
                        var Selected_subexms_list = [];
                        angular.forEach(exms_subs, function (itm) {
                            var newCol = "";
                            if (itm.checkedvalue) {
                                newCol = { EMSE_Id: itm.emsE_Id, EMGR_Id: itm.EMGR_Id, ECYSESSS_MaxMarks: itm.ECYSESSS_MaxMarks, ECYSESSS_MinMarks: itm.ECYSESSS_MinMarks, ECYSESSS_ExemptedFlg: itm.ECYSESSS_ExemptedFlg, ECYSESSS_ExemptedPer: itm.ECYSESSS_ExemptedPer, ECYSESSS_SubExamOrder: itm.ECYSESSS_SubExamOrder }
                                Selected_subexms_list.push(newCol);
                            }
                        });

                        for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) {
                            var already_count = 0;
                            if ($scope.ismS_Id == $scope.SubWise_Selected_subexms_list[i].ISMS_Id) {
                                already_count += 1;
                                if (already_count > 0) {
                                    $scope.SubWise_Selected_subexms_list.splice(i, 1);
                                }
                            }
                        }
                        $scope.SubWise_Selected_subexms_list.push({ ISMS_Id: $scope.ismS_Id, sub_exam_list: Selected_subexms_list });
                        $('#popup5').modal('hide');
                        $scope.subexam_list = [];
                    }
                    else {
                        swal("Total Of Selected Sub-Exams Max And Min Marks Must Equal To Subject Max And Min Marks");
                        angular.forEach(exms_subs, function (itm) {
                            if (itm.checkedvalue) {

                                itm.ECYSESSS_MaxMarks = "";
                                itm.ECYSESSS_MinMarks = "";
                            }
                        });
                    }
                }
            }
            else {
                $scope.submitted2 = true;
            }
        };       

        $scope.clearpopupgrid3 = function (subsubary) {
            if ($scope.subject_check == true) {
                angular.forEach($scope.subject_list, function (itm) {
                    if (itm.ismS_Id == $scope.ismS_Id) {
                        itm.ECYSES_SubSubjectFlg = false;
                    }
                });
            }
            $('#popup3').modal('hide');
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.subject_list.every(function (itm) { return itm.checkedvalue; })
        };

        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.all1;
            angular.forEach($scope.subsubject_list, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        };

        $scope.optionToggled1 = function () {
            $scope.all1 = $scope.subsubject_list.every(function (itm) { return itm.checkedvalue; })
        };

        $scope.toggleAll2 = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.subexam_list, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        };

        $scope.clear1 = function () {
            $scope.subsubject_list = $scope.tempsubsubjectlist;
            angular.forEach($scope.subsubject_list, function (itm1) {
                itm1.ECYSESSS_MaxMarks = "";
                itm1.ECYSESSS_MinMarks = "";
                itm1.EMGR_Id = "";
                itm1.ECYSESSS_ExemptedFlg = false;
                itm1.ECYSESSS_ExemptedPer = "";
            });

            $scope.all1 = true;
            $scope.toggleAll1();
            $scope.ECYSESSS_Id = 0;
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";
        };

        $scope.clear2 = function () {
            $scope.subexam_list = $scope.tempsubexamlist;
            angular.forEach($scope.subexam_list, function (itm1) {
                itm1.ECYSESSS_MaxMarks = "";
                itm1.ECYSESSS_MinMarks = "";
                itm1.EMGR_Id = "";
                itm1.ECYSESSS_ExemptedFlg = false;
                itm1.ECYSESSS_ExemptedPer = "";
            });

            $scope.all2 = true;
            $scope.toggleAll2();
            $scope.EYCESSE_Id = 0;
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.subject_list) {
                    $scope.subject_list[index].ismS_OrderFlag = Number(index) + 1;
                    $scope.subject_list[index].ECYSES_SubjectOrder = Number(index) + 1;
                }
            }
        };

        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.ismS_Id !== 0) {
                    orderarray[key].ismS_OrderFlag = key + 1;
                    orderarray[key].ECYSES_SubjectOrder = key + 1;
                }
            });
            $('#myModal').modal('hide');
        };

        $scope.sortableOptions3 = {
            stop: function (e, ui) {
                for (var index in $scope.subsubject_list) {
                    $scope.subsubject_list[index].emsS_Order = Number(index) + 1;
                    $scope.subsubject_list[index].ECYSESSS_SubSubjectOrder = Number(index) + 1;
                }
            }
        };

        $scope.getOrder3 = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.emsS_Id !== 0) {
                    orderarray[key].emsS_Order = key + 1;
                    orderarray[key].ECYSESSS_SubSubjectOrder = key + 1;

                }
            });
        };

        $scope.sortableOptions5 = {
            stop: function (e, ui) {
                for (var index in $scope.subexam_list) {
                    $scope.subexam_list[index].emsE_SubExamOrder = Number(index) + 1;
                    $scope.subexam_list[index].ECYSESSS_SubExamOrder = Number(index) + 1;
                }
            }
        };

        $scope.getOrder5 = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.emsE_Id !== 0) {
                    orderarray[key].emsE_SubExamOrder = key + 1;
                    orderarray[key].ECYSESSS_SubExamOrder = key + 1;
                }
            });
        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.order = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.clearpopupgrid_subsubjs = function () {
            $scope.viewrecordspopupdisplay_subsubjs = "";
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', width: '6%', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'amcO_CourseName', displayName: 'Course', width: '10%' },
                { name: 'amB_BranchName', displayName: 'Branch', width: '10%' },
                { name: 'amsE_SEMName', displayName: 'Semester', width: '10%' },
                { name: 'schemetype', displayName: 'Scheme Type', width: '10%' },
                { name: 'subjectscheme', displayName: 'Subject Scheme', width: '10%' },
                { name: 'emE_ExamName', displayName: 'Exam Name', width: '10%' },
                //{ name: 'subjectgrpname', displayName: 'SubjectGroup Name', width: '10%' },
                { name: 'emgR_GradeName', width: '9%', displayName: 'Grade Name' },
                { name: 'ecysE_AttendanceFromDate', width: '12%', displayName: 'Attendance From', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'ecysE_AttendanceToDate', width: '10%', displayName: 'Attendance To', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                {
                    name: 'ecysE_SubExamFlg', width: '9%', displayName: 'Sub-Exams', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.ecysE_SubExamFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.ecysE_SubExamFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'ecysE_SubSubjectFlg', width: '10%', displayName: 'Sub-Subjects', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.ecysE_SubSubjectFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.ecysE_SubSubjectFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    field: 'id', name: '', width: '10%',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ecysE_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ecysE_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.viewrecordspopup = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ecysE_Id;
            var pageid = $scope.editEmployee;

            $scope.coursename = employee.amcO_CourseName;
            $scope.branchname = employee.amB_BranchName;
            $scope.semester = employee.amsE_SEMName;
            $scope.schemetype = employee.schemetype;
            $scope.subjectscheme = employee.subjectscheme;
            $scope.emE_ExamName = employee.emE_ExamName;

            apiService.getURI("ClgExamSubjectWizard/getalldetailsviewrecords", pageid).then(function (promise) {
                $scope.Category_Name = promise.view_exam_subjects[0].emcA_CategoryName;
                $scope.Exam_Name = promise.view_exam_subjects[0].emE_ExamName;
                $scope.viewrecordspopupdisplay = promise.view_exam_subjects;
            })
        };

        $scope.viewrecordspopup_subexms = function (employee) {
            $scope.editEmployee = employee.ecyseS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("ClgExamSubjectWizard/getalldetailsviewrecords_subexms", pageid).then(function (promise) {
                $scope.Exm_Subject = promise.view_exam_subjects_subexams[0].ismS_SubjectName;
                $scope.viewrecordspopupdisplay_subexms = promise.view_exam_subjects_subexams;
                $('#popup_subexms').modal('show');
                $('#popup_subsubjectsubexms').modal('hide');
            });
        };

        $scope.viewrecordspopup_subsubjs = function (employee) {
            $scope.editEmployee = employee.ecyseS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("ClgExamSubjectWizard/getalldetailsviewrecords_subsubjs", pageid).then(function (promise) {
                $scope.Exm_Subject = promise.view_exam_subjects_subsubjects[0].ismS_SubjectName;
                $scope.viewrecordspopupdisplay_subsubjs = promise.view_exam_subjects_subsubjects;
                $('#popup_subsubjs').modal('show');
                $('#popup_subsubjectsubexms').modal('hide');
            });
        };

        $scope.deactive_sub = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ecyseS_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you Want To " + mgs + " record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgExamSubjectWizard/deactivate_sub", employee).then(function (promise) {
                            if (promise.already_cnt == true) {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval == true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.clear();
                            $scope.viewrecordspopup(employee);
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.deactive_sub_exm = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ecysessS_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgExamSubjectWizard/deactive_sub_exm", employee).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.clear();
                            $scope.viewrecordspopup_subexms(employee);
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.deactive_sub_subj = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.eycessS_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgExamSubjectWizard/deactive_sub_subj", employee).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.clear();
                            $scope.viewrecordspopup_subsubjs(employee);
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        // to Edit Data
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ecysE_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("ClgExamSubjectWizard/editdetails", pageid).then(function (promise) {
                $scope.AMCO_Id = promise.edit_cat_exm[0].amcO_Id;
                $scope.ACSS_Id = promise.edit_cat_exm[0].acsS_Id;
                $scope.AMSE_Id = promise.edit_cat_exm[0].amsE_Id;
                $scope.AMB_Id = promise.edit_cat_exm[0].amB_Id;
                $scope.ACST_Id = promise.edit_cat_exm[0].acsT_Id;
                $scope.EMG_Id = promise.emG_ID;
                $scope.ECYSE_Id = promise.edit_cat_exm[0].ecysE_Id;
                $scope.selected_exm_subjects = promise.edit_cat_exm_subs;
                $scope.temp_exm = promise.edit_cat_exm[0].emE_Id;
                $scope.SubWise_Selected_subexms_list = [];
                $scope.SubWise_Selected_subsubjs_list = [];
                $scope.SubWise_Selected_subsubject_subexms_list = [];

                $scope.EMGR_Id = promise.edit_cat_exm[0].emgR_Id;
                $scope.ECYSE_AttendanceFromDate = new Date(promise.edit_cat_exm[0].ecysE_AttendanceFromDate);
                $scope.ECYSE_AttendanceToDate = new Date(promise.edit_cat_exm[0].ecysE_AttendanceToDate);
                $scope.ECYSE_SubExamFlg = promise.edit_cat_exm[0].ecysE_SubExamFlg;
                $scope.ECYSE_SubSubjectFlg = promise.edit_cat_exm[0].ecysE_SubSubjectFlg;

                temp_saved_subs = promise.edit_cat_exm_subs;

                for (var z = 0; z < promise.edit_cat_exm_subs.length; z++) {
                    var ECYSES_Id = promise.edit_cat_exm_subs[z].ecyseS_Id;
                    var ISMS_Id = promise.edit_cat_exm_subs[z].ismS_Id;

                    if (promise.edit_cat_exm_subs[z].ecyseS_SubExamFlg === true && promise.edit_cat_exm_subs[z].ecyseS_SubSubjectFlg === false) {
                        var Selected_subexms_list = [];
                        angular.forEach(promise.edit_cat_exm_subs_sub_subjs, function (itm) {
                            var newCol = "";
                            if (ECYSES_Id == itm.ecyseS_Id) {
                                newCol = { EMSE_Id: itm.emsE_Id, EMGR_Id: itm.emgR_Id, ECYSESSS_MaxMarks: itm.ecysessS_MaxMarks, ECYSESSS_MinMarks: itm.ecysessS_MinMarks, ECYSESSS_ExemptedFlg: itm.ecysessS_ExemptedFlg, ECYSESSS_ExemptedPer: itm.ecysessS_ExemptedPer, ECYSESSS_SubExamOrder: itm.ecysessS_SubExamOrder };
                                Selected_subexms_list.push(newCol);
                                itm.EYCES_SubSubjectFlgb = true;
                            }
                        });
                        for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) {
                            var already_count = 0;
                            if (ISMS_Id == $scope.SubWise_Selected_subexms_list[i].ISMS_Id) {
                                already_count += 1;
                                if (already_count > 0) {
                                    $scope.SubWise_Selected_subexms_list.splice(i, 1);
                                }
                            }
                        }
                        $scope.SubWise_Selected_subexms_list.push({ ISMS_Id: ISMS_Id, sub_exam_list: Selected_subexms_list });
                    }

                    else if (promise.edit_cat_exm_subs[z].ecyseS_SubExamFlg === false && promise.edit_cat_exm_subs[z].ecyseS_SubSubjectFlg === true) {

                        var Selected_subsubjs_list = [];
                        angular.forEach(promise.edit_cat_exm_subs_sub_subjs, function (itm) {
                            var newCol = "";
                            if (ECYSES_Id == itm.ecyseS_Id) {
                                newCol = { EMSS_Id: itm.emsS_Id, EMGR_Id: itm.emgR_Id, ECYSESSS_MaxMarks: itm.ecysessS_MaxMarks, ECYSESSS_MinMarks: itm.ecysessS_MinMarks, ECYSESSS_ExemptedFlg: itm.ecysessS_ExemptedFlg, ECYSESSS_ExemptedPer: itm.ecysessS_ExemptedPer, ECYSESSS_SubSubjectOrder: itm.ecysessS_SubSubjectOrder };
                                Selected_subsubjs_list.push(newCol);
                                itm.EYCES_SubSubjectFlgb = true;
                            }
                        });
                        for (var i = 0; i < $scope.SubWise_Selected_subsubjs_list.length; i++) {
                            var already_count = 0;
                            if (ISMS_Id == $scope.SubWise_Selected_subsubjs_list[i].ISMS_Id) {
                                already_count += 1;
                                if (already_count > 0) {
                                    $scope.SubWise_Selected_subsubjs_list.splice(i, 1);
                                }
                            }
                        }
                        $scope.SubWise_Selected_subsubjs_list.push({ ISMS_Id: ISMS_Id, sub_subjs_list: Selected_subsubjs_list });
                    }

                    else if (promise.edit_cat_exm_subs[z].ecyseS_SubExamFlg === true && promise.edit_cat_exm_subs[z].ecyseS_SubSubjectFlg === true) {

                        var Selected_subsubjs_subexam_list = [];

                        angular.forEach(promise.edit_cat_exm_subs_sub_subjs, function (itm) {
                            var newCol = "";
                            if (ECYSES_Id == itm.ecyseS_Id) {

                                newCol =
                                    {
                                        EMSS_Id: itm.emsS_Id, EMSE_Id: itm.emsE_Id, EMGR_Id: itm.emgR_Id,
                                        ECYSESSS_MaxMarks: itm.ecysessS_MaxMarks, ECYSESSS_MinMarks: itm.ecysessS_MinMarks,
                                        ECYSESSS_ExemptedFlg: itm.ecysessS_ExemptedFlg,
                                        ECYSESSS_ExemptedPer: itm.ecysessS_ExemptedPer,
                                        ECYSESSS_SubSubjectOrder: itm.ecysessS_SubSubjectOrder,
                                        ECYSESSS_ProgressCardFlag: itm.ecysessS_ProgressCardFlag,
                                        ECYSESSS_SubjectDisplayName: itm.ecysessS_SubjectDisplayName,
                                        ECYSESSS_SubjectDisplayCode: itm.ecysessS_SubjectDisplayCode,
                                    };

                                Selected_subsubjs_subexam_list.push(newCol);
                                itm.EYCES_SubSubjectFlgb = true;
                            }
                        });

                        for (var i = 0; i < $scope.SubWise_Selected_subsubject_subexms_list.length; i++) {
                            var already_count = 0;
                            if (ISMS_Id == $scope.SubWise_Selected_subsubject_subexms_list[i].ISMS_Id) {
                                already_count += 1;
                                if (already_count > 0) {
                                    $scope.SubWise_Selected_subsubject_subexms_list.splice(i, 1);
                                }
                            }
                        }

                        $scope.SubWise_Selected_subsubject_subexms_list.push({ ISMS_Id: ISMS_Id, sub_subject_sub_exam_list: Selected_subsubjs_subexam_list });
                    }
                }
                //if ($scope.EMG_Id != "") {
                $scope.get_subjects();
                //}
            });
        };

        //to deactive the data
        $scope.deactive = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ecysE_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("ClgExamSubjectWizard/deactivate", employee).then(function (promise) {
                            if (promise.already_cnt == true) {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval == true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.clear();
                            $scope.BindData();
                        })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.valid_max = function (max, user) {
            if (user.ECYSES_MaxMarks != null && user.ECYSES_MaxMarks != undefined && user.ECYSES_MaxMarks != "") {
                var num_max = Number(max);
                if (num_max < 1) {
                    swal("Max. Marks  Value Not Less Than 1");
                    angular.forEach($scope.subject_list, function (itm1) {
                        if (itm1.ismS_Id == user.ismS_Id) {
                            itm1.ECYSES_MaxMarks = "";
                        }
                    })
                }
                if (num_max > 1000) {
                    swal("Max Value Is 1000");
                    angular.forEach($scope.subject_list, function (itm1) {
                        if (itm1.ismS_Id == user.ismS_Id) {
                            itm1.ECYSES_MaxMarks = "";
                        }
                    });
                }
            }
        };

        $scope.valid_min = function (min, user) {
            if (user.ECYSES_MinMarks != null && user.ECYSES_MinMarks != undefined && user.ECYSES_MinMarks != "") {
                if (user.ECYSES_MaxMarks != null && user.ECYSES_MaxMarks != undefined && user.ECYSES_MaxMarks != "") {
                    var num_min = Number(min);
                    if (num_min > user.ECYSES_MaxMarks) {
                        swal("Min.Marks  Value Not More Than Max.Marks " + user.ECYSES_MaxMarks);
                        angular.forEach($scope.subject_list, function (itm1) {
                            if (itm1.ismS_Id == user.ismS_Id) {
                                itm1.ECYSES_MinMarks = "";
                            }
                        });
                    }
                }
                else {
                    swal("First Set Max.Marks !!!");
                    user.ECYSES_MinMarks = "";
                }
            }
        };

        $scope.valid_max_entry = function (max_entry, user) {

            if (user.ECYSES_MarksEntryMax != null && user.ECYSES_MarksEntryMax != undefined && user.ECYSES_MarksEntryMax != "") {
                if (user.ECYSES_MaxMarks != null && user.ECYSES_MaxMarks != undefined && user.ECYSES_MaxMarks != "") {
                    var num_max_entry = Number(max_entry);
                    if (num_max_entry > user.ECYSES_MaxMarks) {
                        swal("MaxEntry.Marks  Value Not More Than Max. Marks" + user.ECYSES_MaxMarks);
                        angular.forEach($scope.subject_list, function (itm1) {
                            if (itm1.ismS_Id == user.ismS_Id) {
                                itm1.ECYSES_MarksEntryMax = "";
                            }
                        })
                    }
                    if (num_max_entry < 1) {
                        swal("MaxEntry.Marks  Value Not Less Than 1");
                        angular.forEach($scope.subject_list, function (itm1) {
                            if (itm1.ismS_Id == user.ismS_Id) {
                                itm1.ECYSES_MarksEntryMax = "";
                            }
                        });
                    }
                }
                else {
                    swal("First Set Max.Marks");
                    user.ECYSES_MarksEntryMax = "";
                }
            }
        };

        $scope.valid_max1 = function (subsubj_max, user) {
            var num_subsubj_max = Number(subsubj_max);
            if (num_subsubj_max > Number($scope.Subj_Max_Marks)) {
                swal("Max.Marks Max Value Is Max.Marks Of Subject " + $scope.Subj_Max_Marks);
                user.ECYSESSS_MaxMarks = "";
            }
            user.ECYSESSS_MinMarks = "";
        };

        $scope.valid_min1 = function (subsubj_max, subsubj_min, user) {
            if (subsubj_min != null && subsubj_min != undefined && subsubj_min != "") {
                if (subsubj_max != null && subsubj_max != undefined && subsubj_max != "") {
                    var num_subsubj_max = Number(subsubj_max);
                    var num_subsubj_min = Number(subsubj_min);
                    if (num_subsubj_min >= num_subsubj_max) {
                        swal("Min.Marks Value  Should Be Less Than Max.Marks Value");
                        user.ECYSESSS_MinMarks = "";
                    }
                    else if (num_subsubj_min > Number($scope.Subj_Min_Marks)) {
                        swal("Min.Marks Max Value Is Min.Marks Of Subject " + $scope.Subj_Min_Marks);
                        user.ECYSESSS_MinMarks = "";
                    }
                }
                else {
                    swal("First Enter Max.Marks");
                    user.ECYSESSS_MinMarks = "";
                }
            }
        };

        $scope.valid_exmpt_per1 = function (subsubj_expt_per, user) {
            var num_subsubj_expt_per = Number(subsubj_expt_per);
            if (num_subsubj_expt_per > 100 || num_subsubj_expt_per < 0) {
                swal("Max Value Is 100 and Min Value Is 0");

                user.ECYSESSS_ExemptedPer = "";
            }
        };

        $scope.valid_max2 = function (subexm_max, user) {
            var num_subexm_max = Number(subexm_max);
            if (num_subexm_max > Number($scope.Subj_Max_Marks)) {
                swal("Max.Marks Max Value Is Max.Marks Of Subject " + $scope.Subj_Max_Marks);
                user.ECYSESSS_MaxMarks = "";
            }
            user.EYCESSE_MinMarks = "";
        };

        $scope.valid_min2 = function (subexm_max, subexm_min, user) {
            if (subexm_min != null && subexm_min != undefined && subexm_min != "") {
                if (subexm_max != null && subexm_max != undefined && subexm_max != "") {
                    var num_subexm_max = Number(subexm_max);
                    var num_subexm_min = Number(subexm_min);
                    if (num_subexm_min >= num_subexm_max) {
                        swal("Min.Marks Value  Should Be Less Than Max.Marks Value");

                        user.ECYSESSS_MinMarks = "";
                    }
                    else if (num_subexm_min > Number($scope.Subj_Min_Marks)) {
                        swal("Min.Marks Max Value Is Min.Marks Of Subject " + $scope.Subj_Min_Marks);
                        user.ECYSESSS_MinMarks = "";
                    }
                }
                else {
                    swal("First Enter Max.Marks");
                    user.ECYSESSS_MinMarks = "";
                }
            }
        };

        $scope.valid_exmpt_per2 = function (subexm_expt_per, user) {
            var num_subexm_expt_per = Number(subexm_expt_per);
            if (num_subexm_expt_per > 100 || num_subexm_expt_per < 0) {
                swal("Max Value Is 100 and Min Value Is 0");
                user.ECYSESSS_ExemptedPer = "";
            }
        };

        $scope.set_order_subsubj = function () {
            $('#myModal3').modal('hide');
            $('#popup3').modal('show');
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.SubWise_Selected_subsubjs_list = [];

        $scope.saveddata1 = function (subjs_subs) {
            $scope.submitted1 = true;

            if ($scope.myForm1.$valid) {

                var final_count = 0;
                angular.forEach(subjs_subs, function (itm) {
                    if (itm.checkedvalue) {
                        final_count += 1;
                    }
                })
                if (final_count == 0) {
                    angular.forEach($scope.subject_list, function (opt786) {
                        if ($scope.ismS_Id == opt786.ismS_Id) {
                            for (var i = 0; i < $scope.SubWise_Selected_subsubjs_list.length; i++) {
                                var already_count = 0;
                                if ($scope.ismS_Id == $scope.SubWise_Selected_subsubjs_list[i].ISMS_Id) {
                                    already_count += 1;
                                    if (already_count > 0) {
                                        $scope.SubWise_Selected_subsubjs_list.splice(i, 1);
                                    }
                                }
                            }

                            opt786.ECYSES_SubSubjectFlg = false;
                        }
                    })
                    $('#popup3').modal('hide');
                    $scope.subsubject_list = [];

                }
                else if (final_count > 0) {
                    var Subj_subsubjects_max_total = 0;
                    var Subj_subsubjects_min_total = 0;
                    angular.forEach(subjs_subs, function (itm) {
                        if (itm.checkedvalue) {

                            Subj_subsubjects_max_total += Number(itm.ECYSESSS_MaxMarks);
                            Subj_subsubjects_min_total += Number(itm.ECYSESSS_MinMarks);
                        }
                    })
                    if (Subj_subsubjects_max_total == $scope.Subj_Max_Marks && Subj_subsubjects_min_total == $scope.Subj_Min_Marks) {
                        var Selected_subsubjs_list = [];
                        angular.forEach(subjs_subs, function (itm) {
                            var newCol = "";
                            if (itm.checkedvalue) {

                                //$scope.Selected_subsubjs_list.push({ ismS_Id: $scope.ismS_Id, sub_exam_list: itm });
                                newCol = { EMSS_Id: itm.emsS_Id, EMGR_Id: itm.EMGR_Id, ECYSESSS_MaxMarks: itm.ECYSESSS_MaxMarks, ECYSESSS_MinMarks: itm.ECYSESSS_MinMarks, ECYSESSS_ExemptedFlg: itm.ECYSESSS_ExemptedFlg, ECYSESSS_ExemptedPer: itm.ECYSESSS_ExemptedPer, ECYSESSS_SubSubjectOrder: itm.ECYSESSS_SubSubjectOrder }
                                Selected_subsubjs_list.push(newCol);
                            }
                        })

                        for (var i = 0; i < $scope.SubWise_Selected_subsubjs_list.length; i++) {
                            var already_count = 0;
                            if ($scope.ismS_Id == $scope.SubWise_Selected_subsubjs_list[i].ISMS_Id) {
                                already_count += 1;
                                // $scope.SubWise_Selected_subsubjs_list.splice(i, 1);
                                if (already_count > 0) {
                                    $scope.SubWise_Selected_subsubjs_list.splice(i, 1);
                                }
                            }
                        }

                        $scope.SubWise_Selected_subsubjs_list.push({ ISMS_Id: $scope.ismS_Id, sub_subjs_list: Selected_subsubjs_list });
                        $('#popup3').modal('hide');
                        $scope.subsubject_list = [];
                    }
                    else {
                        swal("Total Of Selected Sub-Subjects Max And Min Marks Must Equal To Subject Max And Min Marks");
                        angular.forEach(subjs_subs, function (itm) {
                            if (itm.checkedvalue) {
                                itm.ECYSESSS_MaxMarks = "";
                                itm.ECYSESSS_MinMarks = "";
                            }
                        });
                    }
                }
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };

        $scope.init = function () {
            $scope.resetLists();
        };

        $scope.init();

        $scope.valid_from = function (att_from) {
            var num_att_from = Number(att_from);
            if (num_att_from > 100) {
                swal("Max Value Is 100");
                $scope.ATT_From = "";
            }
        };

        $scope.valid_to = function (att_to) {
            var num_att_to = Number(att_to);
            if (num_att_to > 100) {
                swal("Max Value Is 100");
                $scope.ATT_To = "";
            }
        };

        $scope.clearpopupgrid_subexms = function () {
            $scope.viewrecordspopupdisplay_subexms = "";
        };

        // Sub subject and sub exam mapping 
        $scope.select_subsubjs1 = function (sel_subsubjs, user) {

            if (user.ECYSES_SubSubjectFlg === undefined) {
                user.ECYSES_SubSubjectFlg = false;
            }
            if (user.ECYSES_SubExamFlg === undefined) {
                user.ECYSES_SubExamFlg = false;
            }

            // Only For Sub Subjects
            if (user.ECYSES_SubSubjectFlg === true && user.ECYSES_SubExamFlg === false) {
                $scope.sel_subsubjs = sel_subsubjs;
                $scope.select_subsubjs(sel_subsubjs, user);
            }

            // Only For Sub Exams 
            else if (user.ECYSES_SubExamFlg === true && user.ECYSES_SubSubjectFlg === false) {
                $scope.sel_subexms = sel_subsubjs;
                $scope.select_subexms(sel_subsubjs, user);
            }

            // Both Sub subject and subexam
            else if (user.ECYSES_SubExamFlg === true && user.ECYSES_SubSubjectFlg === true) {
                //dd
                if (user.ECYSES_SubSubjectFlg === true) {
                    $scope.subject_check = true;
                }

                //if (user.EYCES_SubSubjectFlgb === true) {
                //    $scope.subject_check = true;
                //}

                $scope.subsubjectsubexam_list = $scope.tempsubsubjectsubexamlist;
                var count = 0;
                for (var a = 0; a < $scope.SubWise_Selected_subsubject_subexms_list.length; a++) {
                    if ($scope.SubWise_Selected_subsubject_subexms_list[a].ISMS_Id == user.ismS_Id) {
                        count += 1;
                        for (var b = 0; b < $scope.subsubjectsubexam_list.length; b++) {
                            var subsubj_count = 0;
                            for (var c = 0; c < $scope.SubWise_Selected_subsubject_subexms_list[a].sub_subject_sub_exam_list.length; c++) {
                                if ((parseInt($scope.subsubjectsubexam_list[b].emsS_Id) === parseInt($scope.SubWise_Selected_subsubject_subexms_list[a].sub_subject_sub_exam_list[c].EMSS_Id)) && (parseInt($scope.subsubjectsubexam_list[b].emsE_Id) === parseInt($scope.SubWise_Selected_subsubject_subexms_list[a].sub_subject_sub_exam_list[c].EMSE_Id))) {
                                    var itm = $scope.subsubjectsubexam_list[b];
                                    var itm1 = $scope.SubWise_Selected_subsubject_subexms_list[a].sub_subject_sub_exam_list[c];
                                    subsubj_count += 1;
                                    itm.checkedvalue = true;
                                    itm.ECYSESSS_MaxMarks = itm1.ECYSESSS_MaxMarks;
                                    itm.ECYSESSS_MinMarks = itm1.ECYSESSS_MinMarks;
                                    itm.EMGR_Id = itm1.EMGR_Id;
                                    itm.ECYSESSS_ExemptedFlg = itm1.ECYSESSS_ExemptedFlg;
                                    itm.ECYSESSS_ExemptedPer = itm1.ECYSESSS_ExemptedPer;

                                    itm.ECYSESSS_ProgressCardFlag = itm1.ECYSESSS_ProgressCardFlag;
                                    itm.ECYSESSS_SubjectDisplayName = itm1.ECYSESSS_SubjectDisplayName;
                                    itm.ECYSESSS_SubjectDisplayCode = itm1.ECYSESSS_SubjectDisplayCode;
                                    $scope.optionToggled123();
                                }
                            }

                            if (subsubj_count === 0) {
                                var itm = $scope.subsubjectsubexam_list[b];
                                itm.checkedvalue = false;
                                itm.ECYSESSS_MaxMarks = "";
                                itm.ECYSESSS_MinMarks = "";
                                itm.EMGR_Id = "";
                                itm.ECYSESSS_ExemptedFlg = false;
                                itm.ECYSESSS_ExemptedPer = "";
                                $scope.optionToggled123();
                            }
                        }
                    }
                }

                $scope.Subject_Name = user.ismS_SubjectName;
                $scope.ismS_Id = user.ismS_Id;
                $scope.Subj_Max_Marks = user.ECYSES_MaxMarks;
                $scope.Subj_Min_Marks = user.ECYSES_MinMarks;

                //  alert('#popup');
                if (sel_subsubjs === true) {
                    $scope.subject_check = true;
                    if (count === 0) {
                        $scope.clear3();
                    }
                    $('#popup4546').modal('show');
                } else if (sel_subsubjs === false) {
                    $scope.subject_check = false;
                    swal({
                        title: "Are you sure",
                        text: "Do you want to Delete SubSubjects Mapped to " + $scope.Subject_Name + " Subject???",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete Selection!",
                        cancelButtonText: "Change Selection!",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                for (var i = 0; i < $scope.SubWise_Selected_subsubject_subexms_list.length; i++) {
                                    if (parseInt($scope.SubWise_Selected_subsubject_subexms_list[i].ISMS_Id) === parseInt(user.ismS_Id)) {
                                        $scope.SubWise_Selected_subsubject_subexms_list.splice(i, 1);
                                    }
                                }
                                swal('Deleted Successfully');
                            }
                            else {
                                swal("Now You Can Change Selection");
                                angular.forEach($scope.subject_list, function (itm) {
                                    if (parseInt(itm.ismS_Id) === parseInt(user.ismS_Id)) {
                                        itm.ECYSES_SubSubjectFlg = true;
                                        itm.ECYSES_SubExamFlg = true;
                                        itm.ECYSES_SubSubjectFlgb = true;
                                        itm.EYCES_SubSubjectFlgb = true;
                                        $('#popup4546').modal('show');
                                    }
                                });
                            }
                        });
                }
            }
        };

        $scope.SubWise_Selected_subsubject_subexms_list = [];

        // Add and Close For SUBSUBJECT AND SUBEXAM
        $scope.saveddata23 = function (exms_subs) {

            $scope.submitted23 = true;

            if ($scope.myForm123.$valid) {

                var final_count = 0;
                angular.forEach(exms_subs, function (itm) {
                    if (itm.checkedvalue) {
                        final_count += 1;
                    }
                });

                if (final_count === 0) {
                    angular.forEach($scope.subject_list, function (opt786) {
                        if ($scope.ismS_Id === opt786.ismS_Id) {
                            for (var i = 0; i < $scope.SubWise_Selected_subsubject_subexms_list.length; i++) {
                                var already_count = 0;
                                if ($scope.ismS_Id === $scope.SubWise_Selected_subsubject_subexms_list[i].ISMS_Id) {
                                    already_count += 1;
                                    if (already_count > 0) {
                                        $scope.SubWise_Selected_subsubject_subexms_list.splice(i, 1);
                                    }
                                }
                            }
                            opt786.EYCES_SubExamFlg = false;
                        }
                    });

                    $('#popup4546').modal('hide');

                    $scope.subsubjectsubexam_list = [];
                }
                else if (final_count > 0) {
                    var Subj_subsubj_subexams_max_total = 0;
                    var Subj_subsubj_subexams_min_total = 0;
                    angular.forEach(exms_subs, function (itm) {
                        if (itm.checkedvalue) {
                            Subj_subsubj_subexams_max_total += Number(itm.ECYSESSS_MaxMarks);
                            Subj_subsubj_subexams_min_total += Number(itm.ECYSESSS_MinMarks);
                        }
                    });

                    if (Subj_subsubj_subexams_max_total === parseInt($scope.Subj_Max_Marks) && Subj_subsubj_subexams_min_total === parseInt($scope.Subj_Min_Marks)) {

                        var Selected_subsubject_subexms_list = [];

                        angular.forEach(exms_subs, function (itm) {
                            var newCol = "";
                            if (itm.checkedvalue) {
                                newCol = {
                                    EMSS_Id: itm.emsS_Id, EMSE_Id: itm.emsE_Id, EMGR_Id: itm.EMGR_Id, ECYSESSS_MaxMarks: itm.ECYSESSS_MaxMarks,
                                    ECYSESSS_MinMarks: itm.ECYSESSS_MinMarks, ECYSESSS_ExemptedFlg: itm.ECYSESSS_ExemptedFlg,
                                    ECYSESSS_ExemptedPer: itm.ECYSESSS_ExemptedPer,
                                    ECYSESSS_SubSubjectOrder: itm.ECYSESSS_SubSubjectOrder,
                                    ECYSESSS_ProgressCardFlag: itm.ECYSESSS_ProgressCardFlag,
                                    ECYSESSS_SubjectDisplayName: itm.ECYSESSS_SubjectDisplayName,
                                    ECYSESSS_SubjectDisplayCode: itm.ECYSESSS_SubjectDisplayCode,
                                };
                                Selected_subsubject_subexms_list.push(newCol);
                            }
                        });

                        for (var i = 0; i < $scope.SubWise_Selected_subsubject_subexms_list.length; i++) {
                            var already_count = 0;
                            if ($scope.ismS_Id === $scope.SubWise_Selected_subsubject_subexms_list[i].ISMS_Id) {
                                already_count += 1;
                                if (already_count > 0) {
                                    $scope.SubWise_Selected_subsubject_subexms_list.splice(i, 1);
                                }
                            }
                        }

                        $scope.SubWise_Selected_subsubject_subexms_list.push({ ISMS_Id: $scope.ismS_Id, sub_subject_sub_exam_list: Selected_subsubject_subexms_list });
                        $('#popup4546').modal('hide');
                        $scope.subsubjectsubexam_list = [];
                    }

                    else {
                        swal("Total Of Selected Sub-Subject Sub-Exam Max And Min Marks Must Equal To Subject Max And Min Marks");
                        angular.forEach(exms_subs, function (itm) {
                            if (itm.checkedvalue) {
                                itm.ECYCESSS_MaxMarks = "";
                                itm.ECYCESSS_MinMarks = "";
                            }
                        });
                    }
                }
            }
            else {
                $scope.submitted23 = true;
            }
        };

        $scope.sortableOptions35 = {
            stop: function (e, ui) {
                for (var index in $scope.tempsubsubjectsubexamlist) {
                    $scope.tempsubsubjectsubexamlist[index].emsS_Order = Number(index) + 1;
                    $scope.tempsubsubjectsubexamlist[index].ECYSESSS_SubSubjectOrder = Number(index) + 1;
                }
            }
        };

        $scope.getOrder35 = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.emsS_Id !== 0) {
                    orderarray[key].emsS_Order = key + 1;
                    orderarray[key].ECYSESSS_SubSubjectOrder = key + 1;
                    //  opt.EYCESSS_SubSubjectOrder = opt.emsS_Order;

                }
            });
        };

        $scope.clear3 = function () {
            $scope.subsubjectsubexam_list = $scope.tempsubsubjectsubexamlist;
            angular.forEach($scope.subsubjectsubexam_list, function (itm1) {
                itm1.ECYSESSS_MaxMarks = "";
                itm1.ECYSESSS_MinMarks = "";
                itm1.EMGR_Id = "";
                //itm1.EMGR_Id = $scope.EMGR_Id;
                itm1.ECYSESSSS_ExemptedFlg = false;
                itm1.ECYSESSS_ExemptedPer = "";
                itm1.ECYSESSS_ProgressCardFlag = "";
                itm1.ECYSESSS_SubjectDisplayName = "";
                itm1.ECYSESSS_SubjectDisplayCode = "";
            });

            $scope.all123 = true;
            $scope.toggleAll123();
            $scope.EYCESSS_Id = 0;
            $scope.submitted23 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
        };

        $scope.optionToggled123 = function () {
            $scope.all123 = $scope.tempsubsubjectsubexamlist.every(function (itm) { return itm.checkedvalue; })
        };

        $scope.toggleAll123 = function () {
            var toggleStatus = $scope.all123;
            angular.forEach($scope.subsubjectsubexam_list, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        };

        $scope.clearpopupgrid345 = function (subsubary) {
            if ($scope.subject_check === true) {
                //angular.forEach($scope.subject_list, function (itm) {
                //    if (parseInt(itm.ismS_Id) === parseInt($scope.ismS_Id)) {
                //        itm.ECYSE_SubSubjectFlg = false;
                //        itm.ECYSES_SubExamFlg = false;
                //        itm.EYCES_SubSubjectFlgb = false;
                //    }
                //});
            }
            $('#popup4546').modal('hide');
        };

        $scope.viewrecordspopup_subsubjssubexam = function (employee) {

            if (employee.ecyseS_SubSubjectFlg === true && employee.ecyseS_SubExamFlg === false) {
                $scope.viewrecordspopup_subsubjs(employee);
            }

            else if (employee.ecyseS_SubSubjectFlg === false && employee.ecyseS_SubExamFlg === true) {
                $scope.viewrecordspopup_subexms(employee);
            }

            else if (employee.ecyseS_SubSubjectFlg === true && employee.ecyseS_SubExamFlg === true) {

                $scope.editEmployee = employee.ecyceS_Id;
                $scope.editEmployee_Temp = employee.ecyceS_Id;
                var pageid = $scope.editEmployee;

                apiService.create("ClgExamSubjectWizard/getalldetailsviewrecords_subsubjssunexam", employee).then(function (promise) {
                    $scope.Exm_Subject = promise.view_exam_subjects_subsubjects[0].ismS_SubjectName;
                    $scope.viewrecordspopupdisplay_subsubjectsubexms = promise.view_exam_subjects_subsubjects;
                    $('#popup_subsubjectsubexms').modal('show');
                    $('#popup_subsubjs').modal('hide');
                    $('#popup_subexms').modal('hide');
                });
            }
        };

        $scope.deactive_sub_subj_subexam = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";

            if (employee.ecysessS_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + "Rrecord ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("ClgExamSubjectWizard/deactive_sub_subj_subexam", employee).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.clear();
                            $scope.viewrecordspopup_subsubjssubexam(employee);
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.interacted123 = function (field) {
            return $scope.submitted23;//|| field.$dirty
        };

        $scope.SetOrder_SubSubject = function (obj_order, flag) {
            var data = {
                "Set_SubSubject_Order_DTO": obj_order,
                "ECYSES_Id": $scope.editEmployee_Temp,
            };
            apiService.create("ClgExamSubjectWizard/SetOrder_SubSubject", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal("Order Updated Successfully");
                    $('#popup_subsubjectsubexms').modal('hide');
                    $('#popup_subsubjs').modal('hide');
                    $('#popup_subexms').modal('hide');
                    // $scope.viewrecordspopup_subsubjssubexam(obj_order);
                }
            });
        };
    }
})();