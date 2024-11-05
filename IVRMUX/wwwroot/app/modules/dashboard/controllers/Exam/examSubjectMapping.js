
(function () {
    'use strict';
    angular.module('app').controller('ExamSubjectMappingController', ExamSubjectMappingController)
    ExamSubjectMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter']
    function ExamSubjectMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter) {
        //$scope.SubWise_Selected_subexms_list = [];
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};

        $scope.EYCE_BestOfApplicableFlg = false;
        $scope.edit_exam_flag = true;
        $scope.get_Master_PT = [];
        //TO  GEt The Values iN Grid
        $scope.select_cat = false;
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', width: '6%', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', width: '11%', displayName: 'Year' },
                { name: 'emcA_CategoryName', width: '11%', displayName: 'Category' },
                { name: 'emE_ExamName', width: '11%', displayName: 'Exam' },
                { name: 'emgR_GradeName', width: '9%', displayName: 'Grade' },
                { name: 'eycE_AttendanceFromDate', width: '12%', displayName: 'Attendance From', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'eycE_AttendanceToDate', width: '10%', displayName: 'Attendance To', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                {
                    name: 'eycE_SubExamFlg', width: '9%', displayName: 'Sub-Exams', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.eycE_SubExamFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.eycE_SubExamFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'eycE_SubSubjectFlg', width: '10%', displayName: 'Sub-Subjects', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.eycE_SubSubjectFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.eycE_SubSubjectFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'eycE_BestOfApplicableFlg', width: '9%', displayName: 'Best Applicable', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.eycE_BestOfApplicableFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.eycE_BestOfApplicableFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                { name: 'eycE_BestOf', width: '9%', displayName: 'Best Off' },
                {
                    field: 'id', name: '', width: '10%',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.eycE_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.eycE_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };

        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        // Load Data 
        $scope.BindData = function () {
            apiService.getDATA("ExamSubjectMapping/Getdetails").then(function (promise) {
                $scope.year_list = promise.yearlist;
                $scope.category_list = promise.categorylist;
                $scope.tempcategorylist = promise.categorylist;
                $scope.grade_list = promise.gradelist;
                $scope.tempexamlist = promise.examlist;
                $scope.exam_list = promise.examlist;
                angular.forEach(promise.subexamlist, function (opt) {
                    opt.EYCESSE_SubExamOrder = opt.emsE_SubExamOrder;
                });

                $scope.subexam_list = promise.subexamlist;
                $scope.subject_list = promise.subjectlist;

                angular.forEach(promise.subsubjectlist, function (opt) {
                    opt.EYCESSS_SubSubjectOrder = opt.emsS_Order;
                });

                $scope.subsubject_list = promise.subsubjectlist;

                $scope.tempsubsubjectlist = promise.subsubjectlist;
                $scope.tempsubexamlist = promise.subexamlist;

                $scope.tempsubsubjectsubexamlist = promise.subsubjectsubexamlist;

                angular.forEach($scope.subject_list, function (opt) {
                    opt.ISMS_Id = opt.ismS_Id;
                    opt.EYCES_MarksDisplayFlg = true;
                    opt.EYCES_MarksGradeEntryFlg = 'M';
                    opt.EYCES_AplResultFlg = true;
                    opt.EYCES_SubjectOrder = opt.ismS_OrderFlag;
                });

                $scope.tempsubjectlist = promise.subjectlist;
                $scope.all = true;
                $scope.obj.all_SubExam = false;
                $scope.obj.all_SubSubject = false;
                $scope.toggleAll();
                $scope.gridOptions.data = promise.category_exams;
                $scope.category_exams_list = promise.category_exams;
            });
        };

        $scope.valid_from_date = function (from_date) {
            if ($scope.ASMAY_Id !== "" && $scope.ASMAY_Id !== null && $scope.ASMAY_Id !== undefined) {
                $scope.EYCE_AttendanceToDate = "";
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.EYCE_AttendanceFromDate = "";
            }
        };

        $scope.valid_to_date = function (to_date) {
            if ($scope.EYCE_AttendanceFromDate !== "" && $scope.EYCE_AttendanceFromDate !== null && $scope.EYCE_AttendanceFromDate !== undefined) {
                // $scope.EYCE_AttendanceToDate = "";
            }
            else {
                swal("First Select Attendance From Date !!!");
                $scope.EYCE_AttendanceToDate = "";
            }
        };

        $scope.get_category = function (yr_id) {
            $scope.get_Master_PT = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ExamSubjectMapping/get_category", data).then(function (promise) {
                $scope.category_list = promise.categorylist;
                $scope.EYC_Id = "";
                if ($scope.EYCE_Id !== "" && $scope.EYCE_Id !== 0 && $scope.EYCE_Id !== undefined) {
                    angular.forEach($scope.category_list, function (role) {
                        if (role.eyC_Id == $scope.temp_category) {
                            $scope.EYC_Id = role.eyC_Id;
                            role.Selected = true;
                        }
                    });
                }
                if (promise.categorylist === "" || promise.categorylist === null) {
                    swal("No Categories Are Mapped To Selected Academic Year");
                    $scope.ASMAY_Id = "";
                }
            });

            var iddata = yr_id;
            for (var k = 0; k < $scope.year_list.length; k++) {
                if ($scope.year_list[k].asmaY_Id == iddata) {
                    var data = $scope.year_list[k].asmaY_Year;
                }
            }

            if (data != null) {
                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                // For Academic From Date
                $scope.minDatemf = new Date(
                    $scope.fromDate,
                    $scope.frommon,
                    $scope.fromDay + 1);

                $scope.maxDatemf = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);
                $scope.EYCE_AttendanceFromDate = "";
            }
        };

        $scope.get_subjects = function (cat_id) {
            $scope.get_Master_PT = [];
            if ($scope.ASMAY_Id !== "" && $scope.ASMAY_Id !== null && $scope.ASMAY_Id !== undefined) {
                var data = {
                    "EYC_Id": $scope.EYC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id
                };
                apiService.create("ExamSubjectMapping/get_subjects", data).then(function (promise) {
                    $scope.subject_list = promise.subjectlist;
                    $scope.select_cat = true;
                    $scope.exam_list = promise.examlist;
                    angular.forEach($scope.subject_list, function (opt) {
                        opt.ISMS_Id = opt.ismS_Id;
                        opt.EYCES_MarksGradeEntryFlg = 'M';
                        opt.EYCES_MarksDisplayFlg = true;
                        opt.EYCES_AplResultFlg = true;
                        opt.EYCES_SubjectOrder = opt.ismS_OrderFlag;
                        opt.EMGR_Id = $scope.EMGR_Id;

                        for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) {
                            if (opt.ismS_Id == $scope.SubWise_Selected_subexms_list[i].ISMS_Id) {
                                opt.EYCES_SubExamFlg = true;
                                opt.EYCES_SubSubjectFlgb = true;
                            }
                        }
                        for (var i = 0; i < $scope.SubWise_Selected_subsubjs_list.length; i++) {
                            if (opt.ismS_Id == $scope.SubWise_Selected_subsubjs_list[i].ISMS_Id) {
                                opt.EYCES_SubSubjectFlg = true;
                                opt.EYCES_SubSubjectFlgb = true;
                            }
                        }
                    });

                    if ($scope.EYCE_Id > 0) {
                        angular.forEach(temp_saved_subs, function (sy) {
                            angular.forEach($scope.subject_list, function (sy1) {
                                if (sy.ismS_Id == sy1.ismS_Id) {
                                    sy1.ismS_OrderFlag = sy.eyceS_SubjectOrder;
                                    sy1.EYCES_SubjectOrder = sy1.ismS_OrderFlag;
                                    sy1.Exam_Subject_PT_GradeList = sy.Exam_Subject_PT_GradeList;
                                }
                            });
                        });
                    }

                    $scope.all = true;
                    $scope.toggleAll();

                    if ($scope.EYCE_Id != "" && $scope.EYCE_Id != 0 && $scope.EYCE_Id != undefined) {
                        if ($scope.EYC_Id == $scope.temp_category) {
                            angular.forEach($scope.tempexamlist, function (role) {
                                if (role.emE_Id == $scope.temp_exm) {
                                    $scope.exam_list.push(role);
                                }
                            });
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
                                        role.EYCES_MaxMarks = itm.eyceS_MaxMarks;
                                        role.EYCES_MinMarks = itm.eyceS_MinMarks;
                                        role.EYCES_MarksEntryMax = itm.eyceS_MarksEntryMax;
                                        role.EYCES_SubExamFlg = itm.eyceS_SubExamFlg;
                                        role.EYCES_SubSubjectFlg = itm.eyceS_SubSubjectFlg;

                                        if (role.EYCES_SubExamFlg == true || role.EYCES_SubSubjectFlg == true) {
                                            role.EYCES_SubSubjectFlgb = true;
                                        } else {
                                            role.EYCES_SubSubjectFlgb = false;
                                        }
                                        role.EYCES_MarksGradeEntryFlg = itm.eyceS_MarksGradeEntryFlg;
                                        role.EYCES_MarksDisplayFlg = itm.eyceS_MarksDisplayFlg;
                                        role.EYCES_GradeDisplayFlg = itm.eyceS_GradeDisplayFlg;
                                        role.EYCES_AplResultFlg = itm.eyceS_AplResultFlg;
                                        role.EMGR_Id = itm.emgR_Id;
                                        exm_subject_cnt += 1;
                                    }
                                });
                                if (exm_subject_cnt == 0) {
                                    role.checkedvalue = false;
                                    $scope.optionToggled();
                                }
                            });

                            $scope.optionToggledmarksdisplay();
                            $scope.optionToggledgradedisplay();
                        }
                    }
                    $scope.get_Master_PT = [];
                    if (promise.get_Master_PT !== undefined && promise.get_Master_PT !== null && promise.get_Master_PT.length > 0) {
                        $scope.get_Master_PT = promise.get_Master_PT;
                    }

                    if (promise.subjectlist === null || promise.subjectlist === "") {
                        swal("Subjects are Not Mapped To Selected Category!!!");
                        $scope.EYC_Id = "";
                        $scope.select_cat = false;
                    }
                    if (promise.examlist === null || promise.examlist === "") {
                        swal("All Exams are  Mapped To Selected Category !!!");
                        $scope.EYC_Id = "";
                    }
                });
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.EYC_Id = "";
            }

        };

        $scope.get_gradename = function (grd) {
            angular.forEach($scope.subject_list, function (itm) {
                itm.EMGR_Id = grd;
            });
            angular.forEach($scope.grade_list, function (itm1) {
                if (itm1.emgR_Id == grd) {
                    $scope.GroupName = itm1.emgR_GradeName;
                }
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.exam_list.some(function (options) {
                return options.checked;
            });
        };

        // Sub Subject Mapping For Subject  All Sub Subjects Funtionality
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
                                // itm = itm1;  
                                var itm = $scope.subsubject_list[b];
                                var itm1 = $scope.SubWise_Selected_subsubjs_list[a].sub_subjs_list[c];
                                subsubj_count += 1;
                                itm.checkedvalue = true;
                                itm.EYCESSS_MaxMarks = itm1.EYCESSS_MaxMarks;
                                itm.EYCESSS_MinMarks = itm1.EYCESSS_MinMarks;
                                itm.EYCESSS_MarksEntryMax = itm1.EYCESSS_MarksEntryMax;
                                itm.EMGR_Id = itm1.EMGR_Id;
                                itm.EYCESSS_ExemptedFlg = itm1.EYCESSS_ExemptedFlg;
                                itm.EYCESSS_ExemptedPer = itm1.EYCESSS_ExemptedPer;
                                itm.EYCESSS_GradesFlg = itm1.EYCESSS_GradesFlg;
                                itm.EYCESSS_AplResultFlg = itm1.EYCESSS_AplResultFlg;
                                itm.EYCESSS_MarksFlg = itm1.EYCESSS_MarksFlg;
                                $scope.optionToggled1();
                            }

                        }
                        if (subsubj_count == 0) {
                            var itm = $scope.subsubject_list[b];
                            itm.checkedvalue = false;
                            itm.EYCESSS_MaxMarks = "";
                            itm.EYCESSS_MinMarks = "";
                            itm.EYCESSS_MarksEntryMax = "";
                            itm.EMGR_Id = "";
                            itm.EYCESSS_GradesFlg = false;
                            itm.EYCESSS_AplResultFlg = false;
                            itm.EYCESSS_MarksFlg = false;
                            //itm.EMGR_Id = $scope.EMGR_Id;
                            itm.EYCESSS_ExemptedFlg = false;
                            itm.EYCESSS_ExemptedPer = "";
                            $scope.optionToggled1();
                        }
                    }
                }
            }

            $scope.Subject_Name = user.ismS_SubjectName;
            $scope.ismS_Id = user.ismS_Id;
            $scope.Subj_Max_Marks = user.EYCES_MaxMarks;
            $scope.Subj_Min_Marks = user.EYCES_MinMarks;
            $scope.Subj_Marks_Entry_Max = user.EYCES_MarksEntryMax;

            //  alert('#popup');
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
                    cancelButtonText: "  Change Selection!",
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
                                    itm.EYCES_SubSubjectFlgb = true;
                                    itm.EYCES_SubSubjectFlg = true;;
                                    $('#popup3').modal('show');
                                }
                            });
                        }
                    });
            }
        };

        //to save subsubjects
        $scope.submitted1 = false;

        $scope.SubWise_Selected_subsubjs_list = [];

        $scope.saveddata1 = function (subjs_subs) {
            $scope.submitted1 = true;

            if ($scope.myForm1.$valid) {

                var final_count = 0;
                angular.forEach(subjs_subs, function (itm) {
                    if (itm.checkedvalue) {
                        final_count += 1;
                    }
                });

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
                            opt786.EYCES_SubSubjectFlg = false;
                        }
                    });

                    $('#popup3').modal('hide');
                    $scope.subsubject_list = [];
                }

                else if (final_count > 0) {
                    var Subj_subsubjects_max_total = 0;
                    var Subj_subsubjects_min_total = 0;
                    var Subj_subsubjects_marksentrytotal_total = 0;

                    angular.forEach(subjs_subs, function (itm) {
                        if (itm.checkedvalue) {
                            Subj_subsubjects_max_total += Number(itm.EYCESSS_MaxMarks);
                            Subj_subsubjects_min_total += Number(itm.EYCESSS_MinMarks);
                            Subj_subsubjects_marksentrytotal_total += Number(itm.EYCESSS_MarksEntryMax);
                        }
                    });

                    Subj_subsubjects_min_total = Number(Subj_subsubjects_min_total.toFixed(2));

                    if (Subj_subsubjects_max_total == $scope.Subj_Max_Marks && Subj_subsubjects_min_total == $scope.Subj_Min_Marks
                        && Subj_subsubjects_marksentrytotal_total == $scope.Subj_Marks_Entry_Max) {
                        var Selected_subsubjs_list = [];
                        angular.forEach(subjs_subs, function (itm) {
                            var newCol = "";
                            if (itm.checkedvalue) {
                                if (itm.EYCESSS_GradesFlg !== true) {
                                    itm.EYCESSS_GradesFlg = false;
                                }
                                if (itm.EYCESSS_MarksFlg !== true) {
                                    itm.EYCESSS_MarksFlg = false;
                                }
                                if (itm.EYCESSS_AplResultFlg !== true) {
                                    itm.EYCESSS_AplResultFlg = false;
                                }
                                newCol = {
                                    EMSS_Id: itm.emsS_Id, EMGR_Id: itm.EMGR_Id, EYCESSS_MaxMarks: itm.EYCESSS_MaxMarks,
                                    EYCESSS_MinMarks: itm.EYCESSS_MinMarks, EYCESSS_ExemptedFlg: itm.EYCESSS_ExemptedFlg,
                                    EYCESSS_ExemptedPer: itm.EYCESSS_ExemptedPer, EYCESSS_SubSubjectOrder: itm.EYCESSS_SubSubjectOrder,
                                    EYCESSS_MarksFlg: itm.EYCESSS_MarksFlg, EYCESSS_GradesFlg: itm.EYCESSS_GradesFlg,
                                    EYCESSS_AplResultFlg: itm.EYCESSS_AplResultFlg, EYCESSS_MarksEntryMax: itm.EYCESSS_MarksEntryMax
                                };
                                Selected_subsubjs_list.push(newCol);
                            }
                        });

                        for (var i = 0; i < $scope.SubWise_Selected_subsubjs_list.length; i++) {
                            var already_count = 0;
                            if ($scope.ismS_Id == $scope.SubWise_Selected_subsubjs_list[i].ISMS_Id) {
                                already_count += 1;
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
                        swal("Total Of Selected Sub-Subjects Max , Min and Max Marks Entry Marks Must Equal To Subject Max ,Max Marks Entry And Min Marks");
                        angular.forEach(subjs_subs, function (itm) {
                            if (itm.checkedvalue) {
                                //itm.EYCESSS_MaxMarks = "";
                                //itm.EYCESSS_MinMarks = "";
                                //itm.EYCESSS_MarksEntryMax = "";
                                //itm.EYCESSS_MarksFlg = false;
                                //itm.EYCESSS_GradesFlg = false;
                                //itm.EYCESSS_AplResultFlg = false;
                            }
                        });
                    }
                }
            }
            else {
                $scope.submitted1 = true;
            }
        };

        // view of sub subject list

        $scope.viewrecordspopup_subsubjs = function (employee) {
            $scope.editEmployee = employee.eyceS_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ExamSubjectMapping/getalldetailsviewrecords_subsubjs", pageid).then(function (promise) {
                $scope.Exm_Subject = promise.view_exam_subjects_subsubjects[0].ismS_SubjectName;
                $scope.viewrecordspopupdisplay_subsubjs = promise.view_exam_subjects_subsubjects;
                $('#popup_subsubjs').modal('show');
                $('#popup_subsubjectsubexms').modal('hide');

            });
        };

        $scope.clearpopupgrid_subsubjs = function () {
            $scope.viewrecordspopupdisplay_subsubjs = "";
        };

        $scope.set_order_subsubj = function () {
            $('#myModal3').modal('hide');
            $('#popup3').modal('show');
        };

        $scope.subject_check = false;

        $scope.clearpopupgrid3 = function (subsubary) {

            if ($scope.subject_check == true) {
                angular.forEach($scope.subject_list, function (itm) {

                    if (itm.ismS_Id == $scope.ismS_Id) {

                        itm.EYCES_SubSubjectFlg = false;

                    }
                });

            }
            $('#popup3').modal('hide');
        };

        //End 

        // Sub Exam Mapping to Main subject All Sub Exam Funtionality

        $scope.select_subexms = function (sel_subexms, user) {

            if (user.EYCES_SubSubjectFlgb == true) {
                $scope.exam_check = true;
            }

            //$scope.submitted2 = true;
            $scope.subexam_list = $scope.tempsubexamlist;
            $scope.Subject_Name = user.ismS_SubjectName;
            $scope.ismS_Id = user.ismS_Id;
            $scope.Subj_Max_Marks = user.EYCES_MaxMarks;
            $scope.Subj_Min_Marks = user.EYCES_MinMarks;
            $scope.Subj_Marks_Entry_Max = user.EYCES_MarksEntryMax;
            //  alert('#popup');
            var count = 0;
            for (var a = 0; a < $scope.SubWise_Selected_subexms_list.length; a++) {
                if ($scope.SubWise_Selected_subexms_list[a].ISMS_Id == user.ismS_Id) {
                    count += 1;
                    for (var b = 0; b < $scope.subexam_list.length; b++) {
                        var subexam_count = 0;
                        for (var c = 0; c < $scope.SubWise_Selected_subexms_list[a].sub_exam_list.length; c++) {
                            if ($scope.subexam_list[b].emsE_Id == $scope.SubWise_Selected_subexms_list[a].sub_exam_list[c].EMSE_Id) {
                                // itm = itm1;  
                                var itm = $scope.subexam_list[b];
                                var itm1 = $scope.SubWise_Selected_subexms_list[a].sub_exam_list[c];
                                subexam_count += 1;
                                itm.checkedvalue = true;
                                itm.EYCESSE_MaxMarks = itm1.EYCESSE_MaxMarks;
                                itm.EYCESSE_MinMarks = itm1.EYCESSE_MinMarks;
                                itm.EYCESSS_MarksEntryMax = itm1.EYCESSS_MarksEntryMax;
                                itm.EMGR_Id = itm1.EMGR_Id;
                                itm.EYCESSE_ExemptedFlg = itm1.EYCESSE_ExemptedFlg;
                                itm.EYCESSE_ExemptedPer = itm1.EYCESSE_ExemptedPer;
                                itm.EYCESSS_GradesFlg = itm1.EYCESSS_GradesFlg;
                                itm.EYCESSS_AplResultFlg = itm1.EYCESSS_AplResultFlg;
                                itm.EYCESSS_MarksFlg = itm1.EYCESSS_MarksFlg;
                                $scope.optionToggled2();
                            }

                        }
                        if (subexam_count == 0) {
                            var itm = $scope.subexam_list[b];
                            itm.checkedvalue = false;
                            itm.EYCESSE_MaxMarks = "";
                            itm.EYCESSE_MinMarks = "";
                            itm.EYCESSS_MarksEntryMax = "";
                            itm.EMGR_Id = "";
                            //itm.EMGR_Id = $scope.EMGR_Id;
                            itm.EYCESSE_ExemptedFlg = false;
                            itm.EYCESSE_ExemptedPer = "";
                            itm.EYCESSS_GradesFlg = false;
                            itm.EYCESSS_AplResultFlg = false;
                            itm.EYCESSS_MarksFlg = false;
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
                                    itm.EYCES_SubExamFlg = true;
                                    itm.EYCES_SubSubjectFlgb = true;
                                    $('#popup5').modal('show');
                                }
                            });
                        }
                    });
            }
        };

        //to save subsexams

        $scope.submitted2 = false;

        $scope.SubWise_Selected_subexms_list = [];

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
                            opt786.EYCES_SubExamFlg = false;
                        }
                    });

                    $('#popup5').modal('hide');
                    $scope.subexam_list = [];
                }
                else if (final_count > 0) {
                    var Subj_subexams_max_total = 0;
                    var Subj_subexams_min_total = 0;
                    var Subj_subexams_maxentry_total = 0;
                    angular.forEach(exms_subs, function (itm) {
                        if (itm.checkedvalue) {

                            Subj_subexams_max_total += Number(itm.EYCESSE_MaxMarks);
                            Subj_subexams_min_total += Number(itm.EYCESSE_MinMarks);
                            Subj_subexams_maxentry_total += Number(itm.EYCESSS_MarksEntryMax);
                        }
                    });
                    Subj_subexams_min_total = Number(Subj_subexams_min_total.toFixed(2));
                    if (Subj_subexams_max_total == $scope.Subj_Max_Marks && Subj_subexams_min_total == $scope.Subj_Min_Marks
                        && Subj_subexams_maxentry_total == $scope.Subj_Marks_Entry_Max) {

                        var Selected_subexms_list = [];
                        angular.forEach(exms_subs, function (itm) {
                            var newCol = "";
                            if (itm.checkedvalue) {
                                if (itm.EYCESSS_GradesFlg !== true) {
                                    itm.EYCESSS_GradesFlg = false;
                                } if (itm.EYCESSS_MarksFlg !== true) {
                                    itm.EYCESSS_MarksFlg = false;
                                } if (itm.EYCESSS_AplResultFlg !== true) {
                                    itm.EYCESSS_AplResultFlg = false;
                                }
                                newCol = {
                                    EMSE_Id: itm.emsE_Id, EMGR_Id: itm.EMGR_Id, EYCESSE_MaxMarks: itm.EYCESSE_MaxMarks, EYCESSE_MinMarks: itm.EYCESSE_MinMarks,
                                    EYCESSE_ExemptedFlg: itm.EYCESSE_ExemptedFlg, EYCESSE_ExemptedPer: itm.EYCESSE_ExemptedPer,
                                    EYCESSE_SubExamOrder: itm.EYCESSE_SubExamOrder, EYCESSS_MarksFlg: itm.EYCESSS_MarksFlg,
                                    EYCESSS_GradesFlg: itm.EYCESSS_GradesFlg, EYCESSS_AplResultFlg: itm.EYCESSS_AplResultFlg,
                                    EYCESSS_MarksEntryMax: itm.EYCESSS_MarksEntryMax
                                };
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
                        swal("Total Of Selected Sub-Exams Max , Marks Entry Max And Min Marks Must Equal To Subject Max, Marks Entry Max And Min Marks");
                        angular.forEach(exms_subs, function (itm) {
                            if (itm.checkedvalue) {
                                //itm.EYCESSE_MaxMarks = "";
                                //itm.EYCESSE_MinMarks = "";
                                //itm.EYCESSS_MarksEntryMax = "";
                                //itm.EYCESSS_MarksFlg = false;
                                //itm.EYCESSS_GradesFlg = false;
                                //itm.EYCESSS_AplResultFlg = false;
                            }
                        });
                    }
                }
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.viewrecordspopup_subexms = function (employee) {

            $scope.editEmployee = employee.eyceS_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ExamSubjectMapping/getalldetailsviewrecords_subexms", pageid).then(function (promise) {
                $scope.Exm_Subject = promise.view_exam_subjects_subexams[0].ismS_SubjectName;
                $scope.viewrecordspopupdisplay_subexms = promise.view_exam_subjects_subexams;
                $('#popup_subexms').modal('show');
                $('#popup_subsubjectsubexms').modal('hide');
            });

        };

        $scope.clearpopupgrid_subexms = function () {
            $scope.viewrecordspopupdisplay_subexms = "";
        };

        $scope.exam_check = false;

        $scope.clearpopupgrid5 = function (subexary) {
            if ($scope.exam_check == true) {
                angular.forEach($scope.subject_list, function (itm) {
                    if (itm.ismS_Id == $scope.ismS_Id) {
                        itm.EYCES_SubExamFlg = false;
                        itm.EYCES_SubSubjectFlgb = false;
                    }
                });
            }
            $('#popup5').modal('hide');
        };

        // End

        $scope.valid_max = function (max, user) {
            if (user.EYCES_MaxMarks != null && user.EYCES_MaxMarks != undefined && user.EYCES_MaxMarks != "") {
                var num_max = Number(max);
                if (num_max < 1) {
                    swal("Max. Marks  Value Not Less Than 1");
                    angular.forEach($scope.subject_list, function (itm1) {
                        if (itm1.ismS_Id == user.ismS_Id) {
                            itm1.EYCES_MaxMarks = "";
                        }
                    });
                }
                if (num_max > 1000) {
                    swal("Max Value Is 1000");
                    angular.forEach($scope.subject_list, function (itm1) {
                        if (itm1.ismS_Id == user.ismS_Id) {
                            itm1.EYCES_MaxMarks = "";
                        }
                    });
                }
            }
        };

        $scope.valid_min = function (min, user) {
            if (user.EYCES_MinMarks != null && user.EYCES_MinMarks != undefined && user.EYCES_MinMarks != "") {
                if (user.EYCES_MaxMarks != null && user.EYCES_MaxMarks != undefined && user.EYCES_MaxMarks != "") {
                    var num_min = Number(min);

                    if (num_min > user.EYCES_MaxMarks) {
                        swal("Min.Marks  Value Not More Than Max.Marks " + user.EYCES_MaxMarks);
                        angular.forEach($scope.subject_list, function (itm1) {
                            if (itm1.ismS_Id == user.ismS_Id) {
                                itm1.EYCES_MinMarks = "";
                            }
                        });
                    }
                }
                else {
                    swal("First Set Max.Marks !!!");
                    user.EYCES_MinMarks = "";
                }
            }
        };


        $scope.valid_max_entry = function (max_entry, user) {

            if (user.EYCES_MarksEntryMax != null && user.EYCES_MarksEntryMax != undefined && user.EYCES_MarksEntryMax != "") {
                if (user.EYCES_MaxMarks != null && user.EYCES_MaxMarks != undefined && user.EYCES_MaxMarks != "") {
                    var num_max_entry = Number(max_entry);
                    if (num_max_entry > user.EYCES_MaxMarks) {
                        //swal("MaxEntry.Marks  Value Not More Than Max. Marks" + user.EYCES_MaxMarks);
                        //angular.forEach($scope.subject_list, function (itm1) {
                        //    if (itm1.ismS_Id == user.ismS_Id) {
                        //        itm1.EYCES_MarksEntryMax = "";
                        //    }
                        //});
                    }
                    if (num_max_entry < 1) {
                        swal("MaxEntry.Marks  Value Not Less Than 1");
                        angular.forEach($scope.subject_list, function (itm1) {
                            if (itm1.ismS_Id == user.ismS_Id) {
                                itm1.EYCES_MarksEntryMax = "";
                            }
                        });
                    }
                }
                else {
                    user.EYCES_MarksEntryMax = "";
                    swal("First Set Max.Marks");
                }
            }
        };

        $scope.valid_max1 = function (subsubj_max, user) {
            var num_subsubj_max = Number(subsubj_max);
            if (num_subsubj_max > Number($scope.Subj_Max_Marks)) {
                swal("Max.Marks Max Value Is Max.Marks Of Subject " + $scope.Subj_Max_Marks);
                user.EYCESSS_MaxMarks = "";
            }
            user.EYCESSS_MinMarks = "";
        };

        $scope.valid_min1 = function (subsubj_max, subsubj_min, user) {

            if (subsubj_min != null && subsubj_min != undefined && subsubj_min != "") {
                if (subsubj_max != null && subsubj_max != undefined && subsubj_max != "") {
                    var num_subsubj_max = Number(subsubj_max);
                    var num_subsubj_min = Number(subsubj_min);
                    if (num_subsubj_min >= num_subsubj_max) {
                        swal("Min.Marks Value  Should Be Less Than Max.Marks Value");

                        user.EYCESSS_MinMarks = "";
                    }
                    else if (num_subsubj_min > Number($scope.Subj_Min_Marks)) {
                        swal("Min.Marks Max Value Is Min.Marks Of Subject " + $scope.Subj_Min_Marks);
                        user.EYCESSS_MinMarks = "";
                    }
                }
                else {
                    swal("First Enter Max.Marks");
                    user.EYCESSS_MinMarks = "";
                }
            }
        };

        $scope.valid_exmpt_per1 = function (subsubj_expt_per, user) {

            var num_subsubj_expt_per = Number(subsubj_expt_per);
            if (num_subsubj_expt_per > 100 || num_subsubj_expt_per < 0) {
                swal("Max Value Is 100 and Min Value Is 0");

                user.EYCESSS_ExemptedPer = "";
            }
        };

        $scope.valid_max2 = function (subexm_max, user) {
            var num_subexm_max = Number(subexm_max);
            if (num_subexm_max > Number($scope.Subj_Max_Marks)) {
                swal("Max.Marks Max Value Is Max.Marks Of Subject " + $scope.Subj_Max_Marks);
                user.EYCESSE_MaxMarks = "";
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

                        user.EYCESSE_MinMarks = "";
                    }
                    else if (num_subexm_min > Number($scope.Subj_Min_Marks)) {
                        swal("Min.Marks Max Value Is Min.Marks Of Subject " + $scope.Subj_Min_Marks);
                        user.EYCESSE_MinMarks = "";
                    }
                }
                else {
                    swal("First Enter Max.Marks");
                    user.EYCESSE_MinMarks = "";
                }
            }
        }

        $scope.valid_exmpt_per2 = function (subexm_expt_per, user) {
            var num_subexm_expt_per = Number(subexm_expt_per);
            if (num_subexm_expt_per > 100 || num_subexm_expt_per < 0) {
                swal("Max Value Is 100 and Min Value Is 0");

                user.EYCESSE_ExemptedPer = "";
            }
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subject_list, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
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

        $scope.optionToggled2 = function () {
            $scope.all2 = $scope.subexam_list.every(function (itm) { return itm.checkedvalue; })
        };

        $scope.optionToggled123 = function () {
            $scope.all123 = $scope.tempsubsubjectsubexamlist.every(function (itm) { return itm.checkedvalue; })
        };

        $scope.toggleAll122 = function () {
            var toggleStatus = $scope.all123;
            angular.forEach($scope.subsubjectsubexam_list, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        };

        $scope.toggleAll_SubExam = function () {
            var toggleStatus = $scope.obj.all_SubExam;
            angular.forEach($scope.subject_list, function (itm) {
                itm.EYCES_SubExamFlg = toggleStatus;
            });
        };

        $scope.optionToggled_SubExam = function () {
            $scope.obj.all_SubExam = $scope.subject_list.every(function (itm) { return itm.EYCES_SubExamFlg; })
        };     

        $scope.toggleAll_SubSubject = function () {
            var toggleStatus = $scope.obj.all_SubSubject;
            angular.forEach($scope.subject_list, function (itm) {
                itm.EYCES_SubSubjectFlg = toggleStatus;
            });
        };

        $scope.optionToggled_SubSubject = function () {
            $scope.obj.all_SubSubject = $scope.subject_list.every(function (itm) { return itm.EYCES_SubSubjectFlg; })
        };


        $scope.disonedit = false;
        var HostName = location.host;

        $scope.Previous1 = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizard/';
        };

        $scope.Previous2 = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub2/';
        };

        $scope.Previous3 = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub3/';
        };

        $scope.Previous4 = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub4/';
        };

        //var self = this;
        $scope.Next1 = function () {

            if ($scope.myForm1.$valid) {
                $scope.get_category($scope.EMCA_Id);
                $scope.get_year($scope.ASMAY_Id);
                $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub2/';

            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.Next2 = function () {

            if ($scope.myForm2.$valid) {
                $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub3/';
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.Next3 = function () {

            if ($scope.myForm3.$valid) {
                $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub4/';
            }
            else {
                $scope.submitted3 = true;
            }
        };

        $scope.Next4 = function () {

            if ($scope.myForm4.$valid) {
                $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub5/';
            }
            else {
                $scope.submitted4 = true;
            }
        };

        $scope.Finish = function () {

            if ($scope.myForm5.$valid) {
                $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizard/';
            }
            else {
                $scope.submitted5 = true;
            }
        };


        //to deactive the data

        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (employee.eycE_ActiveFlg === true) {
                //  mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                //mgs = "Active";
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

                        apiService.create("ExamSubjectMapping/deactivate", employee).
                            then(function (promise) {
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
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.deactive_sub = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (employee.eyceS_ActiveFlg === true) {
                // mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                //mgs = "Active";
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

                        apiService.create("ExamSubjectMapping/deactivate_sub", employee).
                            then(function (promise) {
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
                                //$scope.clear();
                                $scope.viewrecordspopup(employee);
                                $scope.BindData();
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
            };
            if (employee.eycessE_ActiveFlg === true) {
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

                        apiService.create("ExamSubjectMapping/deactive_sub_exm", employee).
                            then(function (promise) {
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
            };
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

                        apiService.create("ExamSubjectMapping/deactive_sub_subj", employee).
                            then(function (promise) {
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

        $scope.deactive_sub_subj_subexam = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
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
                        apiService.create("ExamSubjectMapping/deactive_sub_subj_subexam", employee).then(function (promise) {
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

        // End of deactive the data
        $scope.interacted = function (field) {
            return $scope.submitted;//|| field.$dirty
        };

        $scope.interacted1 = function (field) {

            return $scope.submitted1;//|| field.$dirty
        };

        $scope.interacted2 = function (field) {

            return $scope.submitted2;//|| field.$dirty
        };

        $scope.interacted123 = function (field) {
            return $scope.submitted23;//|| field.$dirty
        };

        var temp_saved_subs = [];

        // to Edit Data
        $scope.getorgvalue = function (employee) {
            // $scope.clear();
            $scope.editEmployee = employee.eycE_Id;
            var pageid = $scope.editEmployee;
            $scope.editrecordflag = 1;
            apiService.getURI("ExamSubjectMapping/editdetails", pageid).then(function (promise) {
                $scope.EYCE_Id = promise.edit_cat_exm[0].eycE_Id;
                $scope.ASMAY_Id = promise.edit_cat_exm[0].asmaY_Id;
                $scope.selected_exm_subjects = promise.edit_cat_exm_subs;
                $scope.EYC_Id = promise.edit_cat_exm[0].eyC_Id;
                $scope.temp_category = promise.edit_cat_exm[0].eyC_Id;
                $scope.edit_exam_flag = promise.edit_exam_flag;

                if ($scope.ASMAY_Id != "") {
                    $scope.get_category($scope.ASMAY_Id);
                }

                $scope.temp_exm = promise.edit_cat_exm[0].emE_Id;
                if ($scope.EYC_Id != "") {
                    $scope.get_subjects($scope.EYC_Id);
                }

                $scope.EMGR_Id = promise.edit_cat_exm[0].emgR_Id;
                $scope.EYCE_AttendanceFromDate = new Date(promise.edit_cat_exm[0].eycE_AttendanceFromDate);
                $scope.EYCE_AttendanceToDate = new Date(promise.edit_cat_exm[0].eycE_AttendanceToDate);

                $scope.EYCE_SubExamFlg = promise.edit_cat_exm[0].eycE_SubExamFlg;
                $scope.EYCE_SubSubjectFlg = promise.edit_cat_exm[0].eycE_SubSubjectFlg;
                $scope.EYCE_BestOfApplicableFlg = promise.edit_cat_exm[0].eycE_BestOfApplicableFlg;
                $scope.EYCE_BestOf = promise.edit_cat_exm[0].eycE_BestOf;

                $scope.SubWise_Selected_subexms_list = [];

                $scope.SubWise_Selected_subsubjs_list = [];

                $scope.SubWise_Selected_subsubjs_subexam_list = [];

                $scope.edit_cat_exm_subs_grade_list = promise.edit_cat_exm_subs_grade_list;

                for (var z = 0; z < promise.edit_cat_exm_subs.length; z++) {

                    var EYCES_Id = promise.edit_cat_exm_subs[z].eyceS_Id;
                    var ISMS_Id = promise.edit_cat_exm_subs[z].ismS_Id;
                    $scope.Temp_PT_GradeList_Temp = [];

                    //Checking Cateogry Having Exam Based Paper Type
                    if (promise.get_Master_PT !== undefined && promise.get_Master_PT !== null && promise.get_Master_PT.length > 0) {
                        angular.forEach($scope.edit_cat_exm_subs_grade_list, function (subj_grade) {
                            if (subj_grade.eyceS_Id === EYCES_Id) {
                                $scope.Temp_PT_GradeList_Temp.push({
                                    EMGR_Id: subj_grade.emgR_Id, EMPATY_Id: subj_grade.empatY_Id, EYCESPT_Id: subj_grade.eycespT_Id,
                                    ISMS_Id: ISMS_Id
                                });
                            }
                        });

                        promise.edit_cat_exm_subs[z].Exam_Subject_PT_GradeList = $scope.Temp_PT_GradeList_Temp;
                    }

                    // Only SubExam Applicable
                    if (promise.edit_cat_exm_subs[z].eyceS_SubExamFlg == true && promise.edit_cat_exm_subs[z].eyceS_SubSubjectFlg == false) {

                        var Selected_subexms_list = [];

                        angular.forEach(promise.edit_cat_exm_subs_sub_subjs, function (itm) {

                            var newCol = "";
                            if (EYCES_Id == itm.eyceS_Id && itm.emsS_Id == 0) {

                                newCol = {
                                    EMSE_Id: itm.emsE_Id, EMGR_Id: itm.emgR_Id, EYCESSE_MaxMarks: itm.eycessS_MaxMarks,
                                    EYCESSE_MinMarks: itm.eycessS_MinMarks, EYCESSE_ExemptedFlg: itm.eycessS_ExemptedFlg,
                                    EYCESSE_ExemptedPer: itm.eycessS_ExemptedPer, EYCESSE_SubExamOrder: itm.eycessS_SubExamOrder,
                                    EYCESSS_MarksFlg: itm.eycessS_MarksFlg, EYCESSS_GradesFlg: itm.eycessS_GradesFlg,
                                    EYCESSS_AplResultFlg: itm.eycessS_AplResultFlg, EYCESSS_MarksEntryMax: itm.eycessS_MarksEntryMax
                                };

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

                    // Only SubSubject Applicable
                    else if (promise.edit_cat_exm_subs[z].eyceS_SubExamFlg == false && promise.edit_cat_exm_subs[z].eyceS_SubSubjectFlg == true) {

                        var Selected_subsubjs_list = [];

                        angular.forEach(promise.edit_cat_exm_subs_sub_subjs, function (itm) {
                            var newCol = "";
                            if (EYCES_Id == itm.eyceS_Id) {

                                newCol = {
                                    EMSS_Id: itm.emsS_Id, EMGR_Id: itm.emgR_Id, EYCESSS_MaxMarks: itm.eycessS_MaxMarks,
                                    EYCESSS_MinMarks: itm.eycessS_MinMarks, EYCESSS_ExemptedFlg: itm.eycessS_ExemptedFlg,
                                    EYCESSS_ExemptedPer: itm.eycessS_ExemptedPer, EYCESSS_SubSubjectOrder: itm.eycessS_SubSubjectOrder,
                                    EYCESSS_MarksFlg: itm.eycessS_MarksFlg, EYCESSS_GradesFlg: itm.eycessS_GradesFlg,
                                    EYCESSS_AplResultFlg: itm.eycessS_AplResultFlg, EYCESSS_MarksEntryMax: itm.eycessS_MarksEntryMax
                                };
                                Selected_subsubjs_list.push(newCol);
                                promise.edit_cat_exm_subs[z].EYCES_SubSubjectFlgb = true;
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

                    //SubSubject Subexam Applicable
                    else {
                        var Selected_subsubjs_subexam_list = [];

                        angular.forEach(promise.edit_cat_exm_subs_sub_subjs, function (itm) {
                            var newCol = "";
                            if (EYCES_Id == itm.eyceS_Id) {
                                newCol = {
                                    EMSS_Id: itm.emsS_Id, EMSE_Id: itm.emsE_Id, EMGR_Id: itm.emgR_Id, EYCESSS_MaxMarks: itm.eycessS_MaxMarks,
                                    EYCESSS_MinMarks: itm.eycessS_MinMarks, EYCESSS_ExemptedFlg: itm.eycessS_ExemptedFlg,
                                    EYCESSS_ExemptedPer: itm.eycessS_ExemptedPer, EYCESSS_SubSubjectOrder: itm.eycessS_SubSubjectOrder,
                                    EYCESSS_MarksFlg: itm.eycessS_MarksFlg, EYCESSS_GradesFlg: itm.eycessS_GradesFlg,
                                    EYCESSS_AplResultFlg: itm.eycessS_AplResultFlg, EYCESSS_MarksEntryMax: itm.eycessS_MarksEntryMax
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

                temp_saved_subs = promise.edit_cat_exm_subs;

                $scope.optionToggled_SubExam();
                $scope.optionToggled_SubSubject();
                $scope.scroll();
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.resetLists = function () {
            $scope.configA = {
                onUpdate: function (evt) {
                    var itemEl = evt.item;
                }
            };
        };

        $scope.init = function () {
            $scope.resetLists();
        };

        $scope.sortKey = 'ismS_OrderFlag';
        $scope.sortReverse = false;

        // Order functions 

        $scope.order = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.init();

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.subject_list) {
                    $scope.subject_list[index].ismS_OrderFlag = Number(index) + 1;
                    $scope.subject_list[index].EYCES_SubjectOrder = Number(index) + 1;
                }
            }
        };

        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.ismS_Id !== 0) {
                    orderarray[key].ismS_OrderFlag = key + 1;
                    orderarray[key].EYCES_SubjectOrder = key + 1;
                    // opt.EYCES_SubjectOrder = opt.ismS_OrderFlag;
                }
            });
            $('#myModal').modal('hide');
        };

        $scope.sortableOptions3 = {
            stop: function (e, ui) {
                for (var index in $scope.subsubject_list) {
                    $scope.subsubject_list[index].emsS_Order = Number(index) + 1;
                    $scope.subsubject_list[index].EYCESSS_SubSubjectOrder = Number(index) + 1;
                }
            }
        };

        $scope.getOrder3 = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.emsS_Id !== 0) {
                    orderarray[key].emsS_Order = key + 1;
                    orderarray[key].EYCESSS_SubSubjectOrder = key + 1;
                    //  opt.EYCESSS_SubSubjectOrder = opt.emsS_Order;

                }
            });
        };

        $scope.sortableOptions5 = {
            stop: function (e, ui) {
                for (var index in $scope.subexam_list) {
                    $scope.subexam_list[index].emsE_SubExamOrder = Number(index) + 1;
                    $scope.subexam_list[index].EYCESSE_SubExamOrder = Number(index) + 1;
                }
            }
        };


        $scope.getOrder5 = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.emsE_Id !== 0) {
                    orderarray[key].emsE_SubExamOrder = key + 1;
                    orderarray[key].EYCESSE_SubExamOrder = key + 1;
                    // opt.EYCESSE_SubExamOrder = opt.emsE_SubExamOrder;
                }
            });
        };

        $scope.sortableOptions35 = {
            stop: function (e, ui) {
                for (var index in $scope.tempsubsubjectsubexamlist) {
                    $scope.tempsubsubjectsubexamlist[index].emsS_Order = Number(index) + 1;
                    $scope.tempsubsubjectsubexamlist[index].EYCESSS_SubSubjectOrder = Number(index) + 1;
                }
            }
        };


        $scope.getOrder35 = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.emsS_Id !== 0) {
                    orderarray[key].emsS_Order = key + 1;
                    orderarray[key].EYCESSS_SubSubjectOrder = key + 1;
                    //  opt.EYCESSS_SubSubjectOrder = opt.emsS_Order;
                }
            });
        };

        // End of the order functions

        // TO Save The Data
        $scope.submitted = false;

        $scope.saveddata = function () {
            $scope.submitted = true;
            $scope.EYCE_AttendanceFromDate = new Date($scope.EYCE_AttendanceFromDate).toDateString();
            $scope.EYCE_AttendanceToDate = new Date($scope.EYCE_AttendanceToDate).toDateString();

            $scope.exam_list_saved = [];
            $scope.subject_list_saved = [];

            angular.forEach($scope.exam_list, function (opt123) {
                if (opt123.checked) {
                    $scope.exam_list_saved.push(opt123);
                }
            });

            angular.forEach($scope.subject_list, function (opt123) {
                if (opt123.checkedvalue) {
                    //MB For New
                    if (!$scope.EYCE_SubExamFlg) {
                        opt123.EYCES_SubExamFlg = false;
                    }
                    if (!$scope.EYCE_SubSubjectFlg) {
                        opt123.EYCES_SubSubjectFlg = false;
                    }
                    //MB For New
                    $scope.subject_list_saved.push(opt123);
                }
            });
            var EYCE_BestOf = 0;
            if ($scope.EYCE_BestOfApplicableFlg === true) {
                EYCE_BestOf = $scope.EYCE_BestOf;
                var applytoresult = 0;
                angular.forEach($scope.subject_list_saved, function (dd) {
                    if (dd.EYCES_AplResultFlg === true) {
                        applytoresult += 1;
                    }
                });
                if (parseInt(applytoresult) <= parseInt(EYCE_BestOf)) {
                    swal("Best Of Count Should Be Less Than Equal To Apply To Result Subject Count");
                    return;
                }
            } else {
                EYCE_BestOf = null;
            }

            if ($scope.myForm.$valid) {

                var checkcount_gradelist = 0;
                var string_subjectname = "0";

                if ($scope.get_Master_PT !== null && $scope.get_Master_PT.length > 0) {
                    angular.forEach($scope.subject_list_saved, function (sub) {
                        if (sub.Exam_Subject_PT_GradeList === undefined || sub.Exam_Subject_PT_GradeList === null || sub.Exam_Subject_PT_GradeList.length === 0) {
                            checkcount_gradelist += 1;
                            string_subjectname += "," + sub.ismS_SubjectName;
                        } else {
                            angular.forEach(sub.Exam_Subject_PT_GradeList, function (dd, i) {
                                if (dd.EMPATY_Id === undefined || dd.EMPATY_Id === null || dd.EMPATY_Id === ""
                                    || dd.EMGR_Id === undefined || dd.EMGR_Id === null || dd.EMGR_Id === "") {
                                    sub.Exam_Subject_PT_GradeList.splice(i, 1);
                                }
                            });
                        }
                    });
                };

                if (checkcount_gradelist === 0) {

                    swal({
                        title: "Marks Entry Is Done For This Selection",
                        text: "Do You Want To Update The Record?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update The Record!",
                       // cancelButtonText: "Cancel Update!",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                var data = {
                                    "EYCE_Id": $scope.EYCE_Id,
                                    "EYCES_Id": $scope.EYCES_Id,
                                    "EYCESSS_Id": $scope.EYCESSS_Id,
                                    "EYCESSE_Id": $scope.EYCESSE_Id,
                                    "EYC_Id": $scope.EYC_Id,
                                    exams_list: $scope.exam_list_saved,
                                    "EMGR_Id": $scope.EMGR_Id,
                                    "EYCE_AttendanceFromDate": $scope.EYCE_AttendanceFromDate,
                                    "EYCE_AttendanceToDate": $scope.EYCE_AttendanceToDate,
                                    "EYCE_SubExamFlg": $scope.EYCE_SubExamFlg,
                                    "EYCE_SubSubjectFlg": $scope.EYCE_SubSubjectFlg,
                                    "EYCE_BestOfApplicableFlg": $scope.EYCE_BestOfApplicableFlg,
                                    "EYCE_BestOf": EYCE_BestOf,
                                    exm_subjects_list: $scope.subject_list_saved,
                                    exm_subject_subexams_list: $scope.SubWise_Selected_subexms_list,
                                    exm_subject_subsubjects_list: $scope.SubWise_Selected_subsubjs_list,
                                    exm_subject_subsubjects_subexam: $scope.SubWise_Selected_subsubject_subexms_list
                                };
                                apiService.create("ExamSubjectMapping/savedetails", data).then(function (promise) {

                                    $scope.gridOptions.data = promise.category_exams;
                                    $scope.category_exams_list = promise.category_exams;

                                    if (promise.returnval === true) {
                                        if (promise.eycE_Id === 0 || promise.eycE_Id < 0) {
                                            swal('Record saved successfully');
                                        }
                                        else if (promise.eycE_Id > 0) {
                                            swal('Record updated successfully');
                                        }
                                    }
                                    else if (promise.returnduplicatestatus === 'Duplicate') {
                                        swal('Record already exist');
                                    }
                                    else {
                                        if (promise.eycE_Id === 0 || promise.eycE_Id < 0) {
                                            swal('Failed to save, please contact administrator');
                                        }
                                        else if (promise.eycE_Id > 0) {
                                            swal('Failed to update, please contact administrator');
                                        }
                                    }
                                    $scope.BindData();
                                    $scope.clear();
                                });
                            }
                            else {
                                swal("Updation Cancel");
                            }
                        });
                }
                else {
                    string_subjectname = string_subjectname.replace("0,", '');
                    swal("Map The Grade To Following Subjects : " + string_subjectname);
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.valid_from = function (att_from) {

            var num_att_from = Number(att_from);
            if (num_att_from > 100) {
                $scope.ATT_From = "";
                swal("Max Value Is 100");
            }
        };

        $scope.valid_to = function (att_to) {

            var num_att_to = Number(att_to);
            if (num_att_to > 100) {
                $scope.ATT_To = "";
                swal("Max Value Is 100");
            }
        };


        // Clear Function

        $scope.clear = function () {
            $state.reload();
        };

        $scope.clear1 = function () {
            $scope.subsubject_list = $scope.tempsubsubjectlist;
            angular.forEach($scope.subsubject_list, function (itm1) {
                itm1.EYCESSS_MaxMarks = "";
                itm1.EYCESSS_MinMarks = "";
                itm1.EMGR_Id = "";
                itm1.EYCESSS_ExemptedFlg = false;
                itm1.EYCESSS_ExemptedPer = "";
            });
            $scope.all1 = true;
            $scope.toggleAll1();
            $scope.EYCESSS_Id = 0;
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";
        };

        $scope.clear2 = function () {
            $scope.subexam_list = $scope.tempsubexamlist;
            angular.forEach($scope.subexam_list, function (itm1) {
                itm1.EYCESSE_MaxMarks = "";
                itm1.EYCESSE_MinMarks = "";
                itm1.EMGR_Id = "";
                itm1.EYCESSE_ExemptedFlg = false;
                itm1.EYCESSE_ExemptedPer = "";
            });
            $scope.all2 = true;
            $scope.toggleAll2();
            $scope.EYCESSE_Id = 0;
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
        };

        $scope.clear3 = function () {
            $scope.subexam_list = $scope.tempsubsubjectsubexamlist;
            angular.forEach($scope.subexam_list, function (itm1) {
                itm1.EYCESSS_MaxMarks = "";
                itm1.EYCESSS_MinMarks = "";
                itm1.EMGR_Id = "";
                itm1.EYCESSS_ExemptedFlg = false;
                itm1.EYCESSS_ExemptedPer = "";
            });
            $scope.all23 = true;
            $scope.toggleAll2();
            $scope.EYCESSS_Id = 0;
            $scope.submitted23 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
        };

        // End Of Clear Function
        $scope.viewrecordspopup = function (employee, SweetAlert) {
            $scope.editEmployee = employee.eycE_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("ExamSubjectMapping/getalldetailsviewrecords", pageid).then(function (promise) {
                $scope.Category_Name = promise.view_exam_subjects[0].emcA_CategoryName;
                $scope.Exam_Name = promise.view_exam_subjects[0].emE_ExamName;
                $scope.viewrecordspopupdisplay = promise.view_exam_subjects;
                $scope.view_exam_subjects_grade_list = promise.view_exam_subjects_grade_list;
            });
        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };

        // view of sub subject or sub exam or subsubject with subexam

        $scope.viewrecordspopup_subsubjssubexam = function (employee) {

            if (employee.eyceS_SubSubjectFlg == true && employee.eyceS_SubExamFlg == false) {
                $scope.viewrecordspopup_subsubjs(employee);
            }

            else if (employee.eyceS_SubSubjectFlg == false && employee.eyceS_SubExamFlg == true) {
                $scope.viewrecordspopup_subexms(employee);
            }

            else if (employee.eyceS_SubSubjectFlg == true && employee.eyceS_SubExamFlg == true) {

                $scope.editEmployee = employee.eyceS_Id;
                var pageid = $scope.editEmployee;

                apiService.getURI("ExamSubjectMapping/getalldetailsviewrecords_subsubjssunexam", pageid).
                    then(function (promise) {

                        $scope.Exm_Subject = promise.view_exam_subjects_subsubjects[0].ismS_SubjectName;
                        $scope.viewrecordspopupdisplay_subsubjectsubexms = promise.view_exam_subjects_subsubjects;
                        $('#popup_subsubjectsubexms').modal('show');
                        $('#popup_subsubjs').modal('hide');
                        $('#popup_subexms').modal('hide');

                    });
            }

        };

        // Both sub subject with sub exam All Sub Subjects with sub exam Funtionality
        $scope.select_subsubjs1 = function (sel_subsubjs, user) {

            if (user.EYCES_SubSubjectFlg == undefined) {
                user.EYCES_SubSubjectFlg = false;
            }
            if (user.EYCES_SubExamFlg == undefined) {
                user.EYCES_SubExamFlg = false;
            }


            if (user.EYCES_SubSubjectFlg == true && user.EYCES_SubExamFlg == false) {
                $scope.sel_subsubjs = sel_subsubjs;
                $scope.select_subsubjs($scope.sel_subsubjs, user);

            } else if (user.EYCES_SubExamFlg == true && user.EYCES_SubSubjectFlg == false) {
                $scope.sel_subexms = sel_subsubjs;
                $scope.select_subexms($scope.sel_subexms, user);

            } else if (user.EYCES_SubSubjectFlg == true && user.EYCES_SubExamFlg == true) {

                if (user.EYCES_SubSubjectFlgb == true) {
                    $scope.subject_check = true;
                }


                $scope.subsubjectsubexam_list = $scope.tempsubsubjectsubexamlist;
                var count = 0;
                for (var a = 0; a < $scope.SubWise_Selected_subsubject_subexms_list.length; a++) {
                    if ($scope.SubWise_Selected_subsubject_subexms_list[a].ISMS_Id == user.ismS_Id) {
                        count += 1;
                        for (var b = 0; b < $scope.subsubjectsubexam_list.length; b++) {
                            var subsubj_count = 0;
                            for (var c = 0; c < $scope.SubWise_Selected_subsubject_subexms_list[a].sub_subject_sub_exam_list.length; c++) {
                                if (($scope.subsubjectsubexam_list[b].emsS_Id == $scope.SubWise_Selected_subsubject_subexms_list[a].sub_subject_sub_exam_list[c].EMSS_Id)
                                    && ($scope.subsubjectsubexam_list[b].emsE_Id == $scope.SubWise_Selected_subsubject_subexms_list[a].sub_subject_sub_exam_list[c].EMSE_Id)) {
                                    var itm = $scope.subsubjectsubexam_list[b];
                                    var itm1 = $scope.SubWise_Selected_subsubject_subexms_list[a].sub_subject_sub_exam_list[c];
                                    subsubj_count += 1;
                                    itm.checkedvalue = true;
                                    itm.EYCESSS_MaxMarks = itm1.EYCESSS_MaxMarks;
                                    itm.EYCESSS_MinMarks = itm1.EYCESSS_MinMarks;
                                    itm.EYCESSS_MarksEntryMax = itm1.EYCESSS_MarksEntryMax;
                                    itm.EMGR_Id = itm1.EMGR_Id;
                                    itm.EYCESSS_ExemptedFlg = itm1.EYCESSS_ExemptedFlg;
                                    itm.EYCESSS_ExemptedPer = itm1.EYCESSS_ExemptedPer;
                                    itm.EYCESSS_GradesFlg = itm1.EYCESSS_GradesFlg;
                                    itm.EYCESSS_AplResultFlg = itm1.EYCESSS_AplResultFlg;
                                    itm.EYCESSS_MarksFlg = itm1.EYCESSS_MarksFlg;
                                    $scope.optionToggled123();
                                }

                            }
                            if (subsubj_count == 0) {
                                var itm = $scope.subsubjectsubexam_list[b];
                                itm.checkedvalue = false;
                                itm.EYCESSS_MaxMarks = "";
                                itm.EYCESSS_MinMarks = "";
                                itm.EYCESSS_MarksEntryMax = "";
                                itm.EMGR_Id = "";
                                itm.EYCESSS_GradesFlg = false;
                                itm.EYCESSS_AplResultFlg = false;
                                itm.EYCESSS_MarksFlg = false;
                                //itm.EMGR_Id = $scope.EMGR_Id;
                                itm.EYCESSS_ExemptedFlg = false;
                                itm.EYCESSS_ExemptedPer = "";
                                $scope.optionToggled123();
                            }
                        }
                    }
                }

                $scope.Subject_Name = user.ismS_SubjectName;
                $scope.ismS_Id = user.ismS_Id;
                $scope.Subj_Max_Marks = user.EYCES_MaxMarks;
                $scope.Subj_Min_Marks = user.EYCES_MinMarks;
                $scope.Subj_MarksEntry_Max_Marks = user.EYCES_MarksEntryMax;

                //  alert('#popup');
                if (sel_subsubjs == true) {
                    $scope.subject_check = true;
                    if (count == 0) {
                        $scope.clear3();
                    }
                    $('#popup4546').modal('show');
                } else if (sel_subsubjs == false) {
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
                                    if ($scope.SubWise_Selected_subsubject_subexms_list[i].ISMS_Id == user.ismS_Id) {
                                        $scope.SubWise_Selected_subsubject_subexms_list.splice(i, 1);
                                    }
                                }
                                swal('Deleted Successfully');
                            }
                            else {
                                swal("Now You Can Change Selection");
                                angular.forEach($scope.subject_list, function (itm) {
                                    if (itm.ismS_Id == user.ismS_Id) {
                                        itm.EYCES_SubSubjectFlg = true;
                                        itm.EYCE_SubExamFlg = true;
                                        itm.EYCES_SubSubjectFlgb = true;
                                        $('#popup4546').modal('show');
                                    }
                                });
                            }
                        });
                }
            }
        };

        $scope.clearpopupgrid345 = function (subsubary) {
            if ($scope.subject_check == true) {
                angular.forEach($scope.subject_list, function (itm) {

                    if (itm.ismS_Id == $scope.ismS_Id) {
                        itm.EYCES_SubSubjectFlg = false;
                        itm.EYCES_SubExamFlg = false;
                        itm.EYCES_SubSubjectFlgb = false;
                    }
                });
            }
            $('#popup4546').modal('hide');
        };

        $scope.SubWise_Selected_subsubject_subexms_list = [];

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
                                    // $scope.SubWise_Selected_subsubject_subexms_list.splice(i, 1);
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
                    var Subj_subsubj_subexams_marksentrymax_total = 0;

                    angular.forEach(exms_subs, function (itm) {
                        if (itm.checkedvalue) {

                            Subj_subsubj_subexams_max_total += Number(itm.EYCESSS_MaxMarks);
                            Subj_subsubj_subexams_min_total += Number(itm.EYCESSS_MinMarks);
                            Subj_subsubj_subexams_marksentrymax_total += Number(itm.EYCESSS_MarksEntryMax);
                        }
                    });
                    Subj_subsubj_subexams_min_total = Number(Subj_subsubj_subexams_min_total.toFixed(2));
                    if (Subj_subsubj_subexams_max_total === Number($scope.Subj_Max_Marks)
                        && Subj_subsubj_subexams_min_total === Number($scope.Subj_Min_Marks)
                        && Subj_subsubj_subexams_marksentrymax_total === Number($scope.Subj_MarksEntry_Max_Marks)) {

                        var Selected_subsubject_subexms_list = [];

                        angular.forEach(exms_subs, function (itm) {
                            var newCol = "";
                            if (itm.checkedvalue) {

                                if (itm.EYCESSS_GradesFlg !== true) {
                                    itm.EYCESSS_GradesFlg = false;
                                } if (itm.EYCESSS_MarksFlg !== true) {
                                    itm.EYCESSS_MarksFlg = false;
                                } if (itm.EYCESSS_AplResultFlg !== true) {
                                    itm.EYCESSS_AplResultFlg = false;
                                }

                                newCol = {
                                    EMSS_Id: itm.emsS_Id, EMSE_Id: itm.emsE_Id, EMGR_Id: itm.EMGR_Id, EYCESSS_MaxMarks: itm.EYCESSS_MaxMarks,
                                    EYCESSS_MinMarks: itm.EYCESSS_MinMarks, EYCESSS_ExemptedFlg: itm.EYCESSS_ExemptedFlg,
                                    EYCESSS_ExemptedPer: itm.EYCESSS_ExemptedPer, EYCESSS_SubSubjectOrder: itm.EYCESSS_SubSubjectOrder,
                                    EYCESSS_MarksFlg: itm.EYCESSS_MarksFlg, EYCESSS_GradesFlg: itm.EYCESSS_GradesFlg,
                                    EYCESSS_AplResultFlg: itm.EYCESSS_AplResultFlg, EYCESSS_MarksEntryMax: itm.EYCESSS_MarksEntryMax
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
                        swal("Total Of Selected Sub-Subject Sub-Exam Max , Marks Entry Max And Min Marks Must Equal To Subject Max , Marks Entry Max And Min Marks");
                        angular.forEach(exms_subs, function (itm) {
                            if (itm.checkedvalue) {
                                //itm.EYCESSS_MaxMarks = "";
                                //itm.EYCESSS_MinMarks = "";
                                //itm.EYCESSS_MarksEntryMax = "";
                                //itm.EYCESSS_MarksFlg = false;
                                //itm.EYCESSS_GradesFlg = false;
                                //itm.EYCESSS_AplResultFlg = false;
                            }
                        });
                    }
                }
            }
            else {
                $scope.submitted23 = true;
            }
        };

        // End


        $scope.clearpopupgrid_subsubjectsubexms = function () {
            $scope.viewrecordspopupdisplay_subsubjectsubexms = "";
        };


        $scope.SetSubjectOrder = function (dd) {
            $scope.eycE_Id_temp = dd.eycE_Id;
            var data = {
                "EYCE_Id": dd.eycE_Id
            };

            apiService.create("ExamSubjectMapping/SetSubjectOrder", data).then(function (promise) {
                if (promise !== null) {
                    $scope.view_exam_subjects_temp = promise.view_exam_subjects;
                }
            });
        };


        $scope.SaveSubjectOrder = function (dd) {

            var data = {
                "EYCE_Id": $scope.eycE_Id_temp,
                "Temp_Subject_Order": $scope.view_exam_subjects_temp
            };

            apiService.create("ExamSubjectMapping/SaveSubjectOrder", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Subject Order Updated Successfully");
                    } else {
                        swal("Failed To Update Subject Order");
                    }
                    $scope.view_exam_subjects_temp = promise.view_exam_subjects;
                }
            });
        };

        $scope.onchangemaxmarks = function (maxmarks_temp) {
            angular.forEach($scope.subject_list, function (dd) {
                dd.EYCES_MaxMarks = maxmarks_temp;
            });
        };

        $scope.onchangeminmarks = function (minmarks_temp) {

            if (Number(minmarks_temp) < Number($scope.maxmarks)) {
                angular.forEach($scope.subject_list, function (dd) {
                    dd.EYCES_MinMarks = minmarks_temp;
                    //$scope.valid_min(dd.EYCES_MinMarks, dd);
                });
            } else {
                swal("Minimum Marks Should Be Less Than Maximum Marks");
                angular.forEach($scope.subject_list, function (dd) {
                    dd.EYCES_MinMarks = "";
                });
            }
        };

        $scope.onchangemaxentrymarks = function (maxentrymarks_temp) {
            angular.forEach($scope.subject_list, function (dd) {
                dd.EYCES_MarksEntryMax = maxentrymarks_temp;
                $scope.valid_max_entry(dd.EYCES_MarksEntryMax, dd);
            });
        };

        $scope.togglemarksdisplayAll = function () {
            var toggleStatus = $scope.marksdisplayall;
            angular.forEach($scope.subject_list, function (itm) {
                itm.EYCES_MarksDisplayFlg = toggleStatus;
            });
        };

        $scope.optionToggledmarksdisplay = function () {
            $scope.marksdisplayall = $scope.subject_list.every(function (itm) { return itm.EYCES_MarksDisplayFlg; })
        };

        $scope.togglegradedisplayAll = function () {
            var toggleStatus = $scope.gradedisplayall;
            angular.forEach($scope.subject_list, function (itm) {
                itm.EYCES_GradeDisplayFlg = toggleStatus;
            });
        };

        $scope.optionToggledgradedisplay = function () {
            $scope.gradedisplayall = $scope.subject_list.every(function (itm) { return itm.EYCES_GradeDisplayFlg; })
        };


        // Subject Wise Multiple Grades For Question Paper Based Exam Category
        $scope.OnClickAdd_SubjectWise_Grade = function (user_subject) {
            $scope.ISMS_Id_PT_Temp = user_subject.ismS_Id;
            $scope.ISMS_SubjectName_PT_Temp = user_subject.ismS_SubjectName;
            $scope.Temp_PT_GradeList = [];
            if (user_subject.Exam_Subject_PT_GradeList !== undefined && user_subject.Exam_Subject_PT_GradeList !== null
                && user_subject.Exam_Subject_PT_GradeList.length > 0) {
                // $scope.Temp_PT_GradeList = user_subject.Exam_Subject_PT_GradeList;

                angular.forEach(user_subject.Exam_Subject_PT_GradeList, function (dd) {
                    if (dd.EMPATY_Id !== undefined && dd.EMPATY_Id !== null && dd.EMPATY_Id !== ''
                        && dd.EMGR_Id !== undefined && dd.EMGR_Id !== null && dd.EMGR_Id !== '') {
                        $scope.EYCESPT_Id = dd.EYCESPT_Id === undefined || dd.EYCESPT_Id === null || dd.EYCESPT_Id === "" ? 0 : dd.EYCESPT_Id;
                        $scope.Temp_PT_GradeList.push({
                            EMGR_Id: dd.EMGR_Id, EMPATY_Id: dd.EMPATY_Id, EYCESPT_Id: $scope.EYCESPT_Id, ISMS_Id: $scope.ISMS_Id_PT_Temp
                        });
                    }
                });
            } else {
                $scope.Temp_PT_GradeList = [{
                    id: 'Subj_GradePT0',
                    EMGR_Id: $scope.EMGR_Id == null || $scope.EMGR_Id == "" ? "" : $scope.EMGR_Id,
                    EMPATY_Id: '', EYCESPT_Id: 0
                }];
            }
            $('#Mymodal_PT_GradeList').modal('show');
        };

        $scope.addNewMobile1std = function () {
            var newItemNostd = $scope.Temp_PT_GradeList.length + 1;
            if (newItemNostd <= 20) {
                $scope.Temp_PT_GradeList.push({
                    'id': 'Subj_GradePT' + newItemNostd,
                    EMGR_Id: $scope.EMGR_Id == null || $scope.EMGR_Id == "" ? "" : $scope.EMGR_Id,
                    EMPATY_Id: ''
                });
            }
        };

        $scope.removeNewMobile1std = function (index, curval1std) {
            $scope.delmsrd = $scope.Temp_PT_GradeList.splice(index, 1);
        };

        $scope.interacted_PT = function () {
            return $scope.submitted_pt;
        };

        $scope.OnChangeQuesPT = function (obj_pt, index) {
            angular.forEach($scope.Temp_PT_GradeList, function (dd, i) {
                if (i !== index) {
                    if (Number(dd.EMPATY_Id) === Number(obj_pt.EMPATY_Id)) {
                        swal("Paper Type Already Selected , You Can Not Select Same Paper Type");
                        obj_pt.EMPATY_Id = "";
                    }
                }
            });
        };

        $scope.Add_PT_Grade_ToSubjects = function (obj_myForm_PT_GradeDetails) {
            if (obj_myForm_PT_GradeDetails.$valid) {
                $scope.Exam_Subject_PT_GradeList_Temp = [];

                angular.forEach($scope.Temp_PT_GradeList, function (dd) {
                    if (dd.EMPATY_Id !== undefined && dd.EMPATY_Id !== null && dd.EMPATY_Id !== ''
                        && dd.EMGR_Id !== undefined && dd.EMGR_Id !== null && dd.EMGR_Id !== '') {
                        $scope.EYCESPT_Id = dd.EYCESPT_Id === undefined || dd.EYCESPT_Id === null || dd.EYCESPT_Id === "" ? 0 : dd.EYCESPT_Id;
                        $scope.Exam_Subject_PT_GradeList_Temp.push({
                            EMGR_Id: dd.EMGR_Id, EMPATY_Id: dd.EMPATY_Id, EYCESPT_Id: $scope.EYCESPT_Id, ISMS_Id: $scope.ISMS_Id_PT_Temp
                        });
                    }
                });

                angular.forEach($scope.subject_list, function (sub) {
                    if (sub.ismS_Id === $scope.ISMS_Id_PT_Temp) {
                        sub.EMGR_Id = $scope.Exam_Subject_PT_GradeList_Temp[0].EMGR_Id;
                        sub.Exam_Subject_PT_GradeList = $scope.Exam_Subject_PT_GradeList_Temp;
                    }
                });

                $('#Mymodal_PT_GradeList').modal('hide');
            } else {
                $scope.submitted_pt = true;
            }
        };


        $scope.OnClickViewSubjectGradeList = function (obj_sub) {
            $scope.View_Subject_GradeList = [];
            $scope.subjectname_temp = obj_sub.ismS_SubjectName;
            if ($scope.view_exam_subjects_grade_list !== undefined && $scope.view_exam_subjects_grade_list !== null
                && $scope.view_exam_subjects_grade_list.length > 0) {
                angular.forEach($scope.view_exam_subjects_grade_list, function (sub_grade) {
                    if (obj_sub.eyceS_Id === sub_grade.eyceS_Id) {
                        $scope.View_Subject_GradeList.push({
                            EMGR_Id: sub_grade.emgR_Id,
                            EMPATY_Id: sub_grade.empatY_Id,
                            EYCES_Id: sub_grade.eyceS_Id,
                            EYCESPT_Id: sub_grade.eycespT_Id,
                            EMPATY_PaperTypeName: sub_grade.empatY_PaperTypeName,
                            EMGR_GradeName: sub_grade.emgR_GradeName,
                            EYCESPT_ActiveFlg: sub_grade.eycespT_ActiveFlg
                        });
                    }
                });

                $('#myModal2_sub_gradelist').modal('show');
            }
        };


        $scope.deactive_subj_GradeList = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (employee.EYCESPT_ActiveFlg === true) {               
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {               
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "EYCES_Id": employee.EYCES_Id,
                            "EYCESPT_Id": employee.EYCESPT_Id,
                        };

                        apiService.create("ExamSubjectMapping/deactive_subj_GradeList", data).then(function (promise) {
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

                            $scope.view_exam_subjects_grade_list = promise.view_exam_subjects_grade_list;
                            $scope.View_Subject_GradeList = [];

                            angular.forEach($scope.view_exam_subjects_grade_list, function (sub_grade) {
                                $scope.View_Subject_GradeList.push({
                                    EMGR_Id: sub_grade.emgR_Id,
                                    EMPATY_Id: sub_grade.empatY_Id,
                                    EYCES_Id: sub_grade.eyceS_Id,
                                    EYCESPT_Id: sub_grade.eycespT_Id,
                                    EMPATY_PaperTypeName: sub_grade.empatY_PaperTypeName,
                                    EMGR_GradeName: sub_grade.emgR_GradeName,
                                    EYCESPT_ActiveFlg: sub_grade.eycespT_ActiveFlg
                                });
                            });                        
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };
    }
})();