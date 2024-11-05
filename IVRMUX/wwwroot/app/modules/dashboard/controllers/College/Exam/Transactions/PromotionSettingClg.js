(function () {
    'use strict';
    angular.module('app').controller('PromotionSettingController', PromotionSettingController)
    PromotionSettingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window']
    function PromotionSettingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.EMP_BestOfApplicableFlg = false;
        $scope.select_cat = false;
        var temp_exm_prom_groups_list = [];

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.BindData = function () {
            $scope.EMP_MarksPerFlg = 'P';
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.select_subjs();
            apiService.getDATA("PromotionSetting/Getdetails").then(function (promise) {
                $scope.exm_prom_groups_list = promise.exm_prom_groups;
                temp_exm_prom_groups_list = promise.exm_prom_groups;
                $scope.year_list = promise.yearlist;
                $scope.category_list = promise.categorylist;
                $scope.tempcategorylist = promise.categorylist;
                $scope.grade_list = promise.gradelist;
                $scope.exam_list = promise.examlist;
                $scope.tempexamlist = promise.examlist;
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

                angular.forEach($scope.subject_list, function (opt) {
                    opt.ISMS_Id = opt.ismS_Id;
                    opt.EMPS_AppToResultFlg = true;
                });

                $scope.tempsubjectlist = promise.subjectlist;

                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.temp_year_id = promise.asmaY_Id;
                angular.forEach($scope.year_list, function (opq) {
                    if (opq.asmaY_Id == $scope.ASMAY_Id) {
                        opq.Selected = true;
                    }
                });

                $scope.get_category($scope.ASMAY_Id);

                $scope.all = true;
                $scope.toggleAll();

                angular.forEach(promise.promotion_details, function (itms) {
                    if (itms.emP_MarksPerFlg == 'M') {
                        itms.emP_MarksPerFlg = 'Marks';
                    }
                    else if (itms.emP_MarksPerFlg == 'P') {
                        itms.emP_MarksPerFlg = 'Percentage';
                    }
                    else if (itms.emP_MarksPerFlg == 'T') {
                        itms.emP_MarksPerFlg = 'Total';
                    }
                    else if (itms.emP_MarksPerFlg == 'F') {
                        itms.emP_MarksPerFlg = 'Final Exam';
                    }
                });

                $scope.gridOptions.data = promise.promotion_details;
                $scope.promotion_details_list = promise.promotion_details;
            });
        };

        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.ismS_Id !== 0) {
                    orderarray[key].ismS_OrderFlag = key + 1;
                    orderarray[key].EMPS_SubjOrder = key + 1;
                }
            });
            $('#myModal').modal('hide');
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
                                // itm = itm1;  
                                var itm = $scope.subsubject_list[b];
                                var itm1 = $scope.SubWise_Selected_subsubjs_list[a].sub_subjs_list[c];
                                subsubj_count += 1;
                                itm.checkedvalue = true;
                                itm.EYCESSS_MaxMarks = itm1.EYCESSS_MaxMarks;
                                itm.EYCESSS_MinMarks = itm1.EYCESSS_MinMarks;
                                itm.EMGR_Id = itm1.EMGR_Id;
                                itm.EYCESSS_ExemptedFlg = itm1.EYCESSS_ExemptedFlg;
                                itm.EYCESSS_ExemptedPer = itm1.EYCESSS_ExemptedPer;
                                $scope.optionToggled1();
                            }

                        }
                        if (subsubj_count == 0) {
                            var itm = $scope.subsubject_list[b];
                            itm.checkedvalue = false;
                            itm.EYCESSS_MaxMarks = "";
                            itm.EYCESSS_MinMarks = "";
                            itm.EMGR_Id = "";
                            itm.EYCESSS_ExemptedFlg = false;
                            itm.EYCESSS_ExemptedPer = "";
                            $scope.optionToggled1();
                        }
                    }
                }

            }
            $scope.Subject_Name = user.ismS_SubjectName;
            $scope.ismS_Id = user.ismS_Id;

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
                    cancelButtonText: "No, Change Selection!",
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
                                    itm.EYCES_SubSubjectFlg = true;
                                    $('#popup3').modal('show');
                                }
                            })
                        }
                    });
            }
        };

        $scope.valid_max = function (max, user) {
            if (user.EMPS_MaxMarks != null && user.EMPS_MaxMarks != undefined && user.EMPS_MaxMarks != "") {
                var num_max = Number(max);
                if (num_max < 1) {
                    swal("Max. Marks  Value Not Less Than 1");
                    angular.forEach($scope.subject_list, function (itm1) {
                        if (itm1.ismS_Id == user.ismS_Id) {
                            itm1.EMPS_MaxMarks = "";
                        }
                    });
                }
                else if (num_max > 1000) {
                    swal("Max Value Is 1000");

                    angular.forEach($scope.subject_list, function (itm1) {
                        if (itm1.ismS_Id == user.ismS_Id) {
                            itm1.EMPS_MaxMarks = "";
                        }
                    });
                }
                //else {
                //    if ($scope.EMP_MarksPerFlg == 'M') {
                //        angular.forEach($scope.subject_list, function (ot) {                           

                //        });
                //        $scope.group_list_subwise = [];
                //        $scope.group_cnt = $scope.group_list_subwise.length;
                //    }
                //}
            }
        };

        $scope.valid_min = function (min, user) {
            if (user.EMPS_MinMarks != null && user.EMPS_MinMarks != undefined && user.EMPS_MinMarks != "") {
                if (user.EMPS_MaxMarks != null && user.EMPS_MaxMarks != undefined && user.EMPS_MaxMarks != "") {
                    var num_min = Number(min);

                    //if (num_min == 0) {
                    //    swal("Min.Marks  Not be Zero");
                    //    angular.forEach($scope.subject_list, function (itm1) {
                    //        if (itm1.ismS_Id == user.ismS_Id) {
                    //            itm1.EMPS_MinMarks = "";
                    //        }
                    //    });
                    //}
                    if (num_min > Number(user.EMPS_MaxMarks)) {
                        swal("Min.Marks  Value Not More Than Max.Marks " + user.EMPS_MaxMarks);
                        angular.forEach($scope.subject_list, function (itm1) {
                            if (itm1.ismS_Id == user.ismS_Id) {
                                itm1.EMPS_MinMarks = "";
                            }
                        });
                    }
                }
                else {
                    swal("First Set Max.Marks !!!");
                    user.EMPS_MinMarks = "";
                }
            }
        };

        $scope.valid_max_convert = function (max_convert, user) {
            if (user.EMPS_ConvertForMarks != null && user.EMPS_ConvertForMarks != undefined && user.EMPS_ConvertForMarks != "") {
                if (user.EMPS_MaxMarks != null && user.EMPS_MaxMarks != undefined && user.EMPS_MaxMarks != "") {
                    if (user.EMPS_MinMarks != null && user.EMPS_MinMarks != undefined && user.EMPS_MinMarks != "") {
                        var num_max_convert = Number(max_convert);
                        if (num_max_convert > 1000) {
                            swal("MaxEntry.Marks  Value Not More Than 1000 ");
                            angular.forEach($scope.subject_list, function (itm1) {
                                if (itm1.ismS_Id == user.ismS_Id) {
                                    //itm1.EYCES_MarksEntryMax = user.ismS_Max_Marks;
                                    itm1.EMPS_ConvertForMarks = "";
                                }
                            })
                            //$scope.EMGD_From = "";
                        }
                        else if (num_max_convert < Number(user.EMPS_MinMarks)) {// else if (num_max_convert < user.EMPS_MinMarks) {
                            swal("MaxEntry.Marks  Value Not Less Than Min. Marks " + user.EMPS_MinMarks);
                            angular.forEach($scope.subject_list, function (itm1) {
                                if (itm1.ismS_Id == user.ismS_Id) {
                                    // itm1.EYCES_MarksEntryMax = user.ismS_Min_Marks;
                                    itm1.EMPS_ConvertForMarks = "";
                                }
                            })
                            //$scope.EMGD_From = "";
                        } else if (num_max_convert < 1) {
                            swal("MaxEntry.Marks  Value Not Less Than 1");
                            angular.forEach($scope.subject_list, function (itm1) {
                                if (itm1.ismS_Id == user.ismS_Id) {
                                    itm1.EMPS_ConvertForMarks = "";
                                }
                            });
                        }
                    }
                    else {
                        swal("First Set Min.Marks");
                        user.EMPS_ConvertForMarks = user.EMPS_MaxMarks;
                    }
                }
                else {
                    swal("First Set Max.Marks");
                    user.EMPS_ConvertForMarks = user.EMPS_MaxMarks;
                }
                if (user.checkedvalue == true) {
                    if ($scope.EMP_MarksPerFlg == 'M') {
                        $scope.EMPSG_MarksValue = "";
                    }
                    //  $scope.EMPSG_MarksValue = "";
                    var selct_subject_count = 0;
                    angular.forEach($scope.subject_list, function (opte) {
                        if (opte.checkedvalue == true) {
                            selct_subject_count += 1;
                        }
                    })
                    if (selct_subject_count > 0) {
                        var minmum_value = 0;
                        angular.forEach($scope.subject_list, function (itm123) {
                            if (itm123.checkedvalue == true) {
                                if (minmum_value == 0) {
                                    minmum_value = Number(itm123.EMPS_ConvertForMarks);
                                }
                                else if (minmum_value > 0) {
                                    if (minmum_value > Number(itm123.EMPS_ConvertForMarks)) {
                                        minmum_value = Number(itm123.EMPS_ConvertForMarks);
                                    }
                                }

                            }
                        })

                        for (var i = 0; i < $scope.group_list_subwise.length; i++) {
                            var num_marks_tot = $scope.group_list_subwise[i].Exm_M_Prom_Subj_Group_Exams_master.length * $scope.group_list_subwise[i].EMPSG_MarksValue;
                            if (num_marks_tot > minmum_value) {
                                // swal("Marks Should Be Not More Than Of Minimum Of  Selected Subjects Converted Marks");
                                //$scope.EMPSG_MarksValue = "";
                                $scope.group_list_subwise.splice(i, 1);
                                $scope.group_cnt = $scope.group_list_subwise.length;
                            }
                        }
                    }
                }
            }
        }

        $scope.valid_max1 = function (subsubj_max, user) {
            var num_subsubj_max = Number(subsubj_max);
            if (num_subsubj_max > 100) {
                swal("Max Value Is 100");

                user.EYCESSS_MaxMarks = "";
            }
            user.EYCESSS_MinMarks = "";
        }

        $scope.valid_min1 = function (subsubj_max, subsubj_min, user) {
            if (subsubj_min != null && subsubj_min != undefined && subsubj_min != "") {
                if (subsubj_max != null && subsubj_max != undefined && subsubj_max != "") {
                    var num_subsubj_max = Number(subsubj_max);
                    var num_subsubj_min = Number(subsubj_min);
                    if (num_subsubj_min >= num_subsubj_max) {
                        swal("Min.Marks Value  Should Be Less Than Max.Marks Value");
                        user.EYCESSS_MinMarks = "";
                    }
                }
                else {
                    swal("First Enter Max.Marks");
                    user.EYCESSS_MinMarks = "";
                }
            }
        }

        $scope.valid_exmpt_per1 = function (subsubj_expt_per, user) {
            var num_subsubj_expt_per = Number(subsubj_expt_per);
            if (num_subsubj_expt_per > 100 || num_subsubj_expt_per < 0) {
                swal("Max Value Is 100 and Min Value Is 0");
                user.EYCESSS_ExemptedPer = "";
            }
        };

        $scope.valid_max2 = function (subexm_max, user) {
            var num_subexm_max = Number(subexm_max);
            if (num_subexm_max > 100) {
                swal("Max Value Is 100");

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
                }
                else {
                    swal("First Enter Max.Marks");
                    user.EYCESSE_MinMarks = "";
                }
            }
        };

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

            if ($scope.EMP_MarksPerFlg == 'M') {
                $scope.EMPSG_MarksValue = "";
            }

            var selct_subject_count = 0;
            angular.forEach($scope.subject_list, function (opte) {
                if (opte.checkedvalue == true) {
                    selct_subject_count += 1;
                }
            });

            if (selct_subject_count > 0) {
                var minmum_value = 0;
                angular.forEach($scope.subject_list, function (itm123) {
                    if (itm123.checkedvalue == true) {
                        if (minmum_value == 0) {
                            minmum_value = Number(itm123.EMPS_ConvertForMarks);
                        }
                        else if (minmum_value > 0) {
                            if (minmum_value > Number(itm123.EMPS_ConvertForMarks)) {
                                minmum_value = Number(itm123.EMPS_ConvertForMarks);
                            }
                        }
                    }
                });

                for (var i = 0; i < $scope.group_list_subwise.length; i++) {
                    var num_marks_tot = $scope.group_list_subwise[i].Exm_M_Prom_Subj_Group_Exams_master.length * $scope.group_list_subwise[i].EMPSG_MarksValue;
                    if (num_marks_tot > minmum_value) {
                        // swal("Marks Should Be Not More Than Of Minimum Of  Selected Subjects Converted Marks");
                        //$scope.EMPSG_MarksValue = "";
                        $scope.group_list_subwise.splice(i, 1);
                        $scope.group_cnt = $scope.group_list_subwise.length;
                    }
                }
            }
        }

        $scope.optionToggled = function () {
            $scope.all = $scope.subject_list.every(function (itm) { return itm.checkedvalue; })
            // $scope.EMPSG_MarksValue = "";
            if ($scope.EMP_MarksPerFlg == 'M') {
                $scope.EMPSG_MarksValue = "";
            }

            var selct_subject_count = 0;
            angular.forEach($scope.subject_list, function (opte) {
                if (opte.checkedvalue == true) {
                    selct_subject_count += 1;
                }
            })
            if (selct_subject_count > 0) {
                var minmum_value = 0;
                angular.forEach($scope.subject_list, function (itm123) {
                    if (itm123.checkedvalue == true) {
                        if (minmum_value == 0) {
                            minmum_value = Number(itm123.EMPS_ConvertForMarks);
                        }
                        else if (minmum_value > 0) {
                            if (minmum_value > Number(itm123.EMPS_ConvertForMarks)) {
                                minmum_value = Number(itm123.EMPS_ConvertForMarks);
                            }
                        }

                    }
                })

                for (var i = 0; i < $scope.group_list_subwise.length; i++) {
                    var num_marks_tot = $scope.group_list_subwise[i].Exm_M_Prom_Subj_Group_Exams_master.length * $scope.group_list_subwise[i].EMPSG_MarksValue;
                    if (num_marks_tot > minmum_value) {
                        // swal("Marks Should Be Not More Than Of Minimum Of  Selected Subjects Converted Marks");
                        //$scope.EMPSG_MarksValue = "";
                        $scope.group_list_subwise.splice(i, 1);
                        $scope.group_cnt = $scope.group_list_subwise.length;
                    }
                }
            }
        }

        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.all1;
            angular.forEach($scope.group_list_subwise, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }

        $scope.optionToggled1 = function () {
            $scope.all1 = $scope.group_list_subwise.every(function (itm) { return itm.checkedvalue; })
        }

        $scope.toggleAll2 = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.subexam_list, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        }

        $scope.optionToggled2 = function () {
            $scope.all2 = $scope.subexam_list.every(function (itm) { return itm.checkedvalue; })
        }

        $scope.subject_check = false;
        $scope.clearpopupgrid3 = function (subsubary) {
            if ($scope.subject_check == true) {
                angular.forEach($scope.subject_list, function (itm) {
                    if (itm.ismS_Id == $scope.ismS_Id) {
                        itm.EYCES_SubSubjectFlg = false;
                    }
                })
            }
            $('#popup3').modal('hide');
        }

        $scope.exam_check = false;
        $scope.clearpopupgrid5 = function (subexary) {
            if ($scope.exam_check == true) {
                angular.forEach($scope.subject_list, function (itm) {

                    if (itm.ismS_Id == $scope.ismS_Id) {
                        itm.EYCES_SubExamFlg = false;
                    }
                })
            }
            $('#popup5').modal('hide');
        }

        $scope.select_subexms = function (sel_subexms, user) {

            //$scope.submitted2 = true;
            $scope.subexam_list = $scope.tempsubexamlist;
            $scope.Subject_Name = user.ismS_SubjectName;
            $scope.ismS_Id = user.ismS_Id;
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
                                    itm.EYCES_SubExamFlg = true;
                                    $('#popup5').modal('show');
                                }
                            })
                        }
                    });
            }
        }


        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', width: '6%', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', width: '13%', displayName: 'Academic Year' },
                { name: 'emcA_CategoryName', width: '13%', displayName: 'Category Name' },

                { name: 'emgR_GradeName', width: '10%', displayName: 'Grade Name' },
                { name: 'emP_MarksPerFlg', displayName: 'Marks/Per/Total/FinalExam' },
                {
                    name: 'emP_PassToIndSubjectFlg', displayName: 'Pass to Individual Subjects', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.emP_PassToIndSubjectFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.emP_PassToIndSubjectFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'emP_PassToOverallFlag', displayName: 'Pass to OverAll Subjects', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.emP_PassToOverallFlag == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.emP_PassToOverallFlag == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'emP_BestOfApplicableFlg', displayName: 'Best Applicable', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.emP_BestOfApplicableFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.emP_BestOfApplicableFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                { name: 'emP_BestOf', displayName: 'Best Off' },
                {
                    field: 'id',
                    name: '',
                    displayName: 'Subject order', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<table class="table" style="border:0px"><tr>' +
                        '<td>' + '<a  ng-if="row.entity.emP_MarksPerFlg != \'Final Exam\'" href="javascript:void(0)" data-toggle="modal" data-target="#myModal" data-backdrop="static" ng-click="grid.appScope.SetSubjectOrder(row.entity);"> <i class="fa fa-eye text-purple" ></i></a> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;' + '</td></tr></table>' +
                        '</div>'
                },
                {
                    field: 'id', name: '', width: '10%',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<table class="table" style="border:0px"><tr>' +
                        '<td>' + '<a  ng-if="row.entity.emP_MarksPerFlg != \'Final Exam\'" href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;' + '</td>' +
                        '<td>' + '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>' + '</td>' +
                        '<td>' + '<a ng-if="row.entity.emP_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.emP_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' + '</td>' +
                        '</tr></table>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

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


        $scope.get_category = function (yr_id) {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EMP_Id": $scope.EMP_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("PromotionSetting/get_category", data).
                then(function (promise) {
                    $scope.category_list = promise.categorylist;
                    $scope.EYC_Id = "";
                    if ($scope.EMP_Id != "" && $scope.EMP_Id != 0 && $scope.EMP_Id != undefined) {
                        angular.forEach($scope.category_list, function (role) {

                            if (role.eyC_Id == $scope.temp_category) {
                                $scope.EYC_Id = role.eyC_Id;
                                role.Selected = true;
                            }
                        })
                    }

                    //if (promise.categorylist == "" || promise.categorylist == null) {
                    //    swal("No Categories Are Mapped To Selected Academic Year");
                    //}
                })

        }

        $scope.valid_to_date = function (from_date) {
            if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != null && $scope.ASMAY_Id != undefined) {
                $scope.EYCE_AttendanceToDate = "";
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.EYCE_AttendanceFromDate = "";
            }
        }
        $scope.get_subjects = function (cat_id) {

            if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != null && $scope.ASMAY_Id != undefined) {

                var data = {
                    "EYC_Id": $scope.EYC_Id,
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("PromotionSetting/get_subjects", data).then(function (promise) {
                    $scope.subject_list = promise.subjectlist;
                    $scope.select_cat = true;
                    $scope.exam_list = promise.examlist;

                    angular.forEach($scope.exam_list, function (d) {
                        d.ConvertionReqOrNot = false;
                        d.ForMaxMarkrs = "";
                    });

                    var countorder = 0;
                    angular.forEach($scope.subject_list, function (opt) {
                        countorder += 1;
                        opt.ISMS_Id = opt.ismS_Id;
                        opt.EMPS_AppToResultFlg = true;
                        opt.EMGR_Id = $scope.EMGR_Id;
                        opt.EMPS_SubjOrder = countorder;
                    });

                    $scope.all = true;
                    $scope.toggleAll();

                    if ($scope.EMP_Id != "" && $scope.EMP_Id != 0 && $scope.EMP_Id != undefined) {
                        if ($scope.selected_pro_subjects != undefined && $scope.selected_pro_subjects != null && $scope.selected_pro_subjects != "") {
                            angular.forEach($scope.subject_list, function (role) {
                                var exm_subject_cnt = 0;
                                angular.forEach($scope.selected_pro_subjects, function (itm) {
                                    if (role.ismS_Id == itm.ismS_Id) {
                                        role.checkedvalue = true;
                                        role.ISMS_Id = itm.ismS_Id;
                                        role.EMPS_MaxMarks = itm.empS_MaxMarks;
                                        role.EMPS_MinMarks = itm.empS_MinMarks;
                                        role.EMPS_ConvertForMarks = itm.empS_ConvertForMarks;
                                        role.EMPS_AppToResultFlg = itm.empS_AppToResultFlg;
                                        role.pro_exams_group_list = itm.pro_exams_group_list;
                                        exm_subject_cnt += 1;
                                    }
                                });
                                if (exm_subject_cnt == 0) {
                                    role.checkedvalue = false;
                                    $scope.optionToggled();
                                }
                            });
                        }
                    }


                    if ((promise.subjectlist == null || promise.subjectlist == "") && (promise.examlist == null || promise.examlist == "")) {
                        swal("Subjects and Exams are Not Mapped To Selected Category!!!");
                        $scope.EYC_Id = "";
                        $scope.select_cat = false;
                        $scope.subject_list = [];
                    }
                    else if (promise.subjectlist == null || promise.subjectlist == "") {
                        swal("Subjects are Not Mapped To Selected Category!!!");
                        $scope.EYC_Id = "";
                        $scope.select_cat = false;
                        $scope.subject_list = [];
                    }
                    else if (promise.examlist == null || promise.examlist == "") {
                        swal("Exams are Not Mapped To Selected Category!!!");
                        $scope.EYC_Id = "";
                        $scope.select_cat = false;
                        $scope.subject_list = [];
                    }

                })
                $scope.group_list_subwise = [];
                $scope.group_cnt = $scope.group_list_subwise.length;
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.EYC_Id = "";
            }
        };

        $scope.get_gradename = function (grd) {
            angular.forEach($scope.subject_list, function (itm) {
                itm.EMGR_Id = grd;
            })

            angular.forEach($scope.grade_list, function (itm1) {
                if (itm1.emgR_Id == grd) {
                    $scope.GroupName = itm1.emgR_GradeName;
                }
            });
        }

        $scope.isOptionsRequired = function () {
            if ($scope.EMP_MarksPerFlg != 'T' && $scope.EMP_MarksPerFlg != 'F') {
                return !$scope.exam_list.some(function (options) {
                    return options.checked;
                });
            }
            else if ($scope.EMP_MarksPerFlg == 'T' || $scope.EMP_MarksPerFlg == 'F') {
                return false;
            }
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
            // $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub2/';
        };
        $scope.Next2 = function () {

            if ($scope.myForm2.$valid) {
                $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub3/';
            }
            else {
                $scope.submitted2 = true;
            }
            // $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub3/';
        };
        $scope.Next3 = function () {

            if ($scope.myForm3.$valid) {
                $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub4/';
            }
            else {
                $scope.submitted3 = true;
            }
            // $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub4/';
        };
        $scope.Next4 = function () {

            if ($scope.myForm4.$valid) {
                $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub5/';
            }
            else {
                $scope.submitted4 = true;
            }
            //$window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizardSub5/';
        };

        $scope.Finish = function () {

            if ($scope.myForm5.$valid) {
                $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizard/';
            }
            else {
                $scope.submitted5 = true;
            }
            //$window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizard/';
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
            if (employee.emP_ActiveFlag === true) {
                //mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                // mgs = "Active";
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
                        apiService.create("PromotionSetting/deactivate", employee).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.clear();
                            //$scope.BindData();
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
            }
            if (employee.empS_ActiveFlag === true) {
                // mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                // mgs = "Active";
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

                        apiService.create("PromotionSetting/deactivate_sub", employee).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                // swal(confirmmgs + " " + " successfully");
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.clear();
                            $scope.viewrecordspopup(employee);
                        });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.deactive_sub_grp_exm = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.empsgE_ActiveFlg === true) {
                // mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                // mgs = "Active";
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

                        apiService.create("PromotionSetting/deactive_sub_grp_exm", employee).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {

                                swal("Record " + mgs + " Failed");
                            }
                            $scope.clear();
                            $scope.viewrecordspopup_sub_grp_exms(employee);
                        });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.deactive_sub_grp = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.empsG_ActiveFlag === true) {
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

                        apiService.create("PromotionSetting/deactive_sub_grp", employee).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                // swal(confirmmgs + " " + " successfully");
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.clear();
                            $scope.viewrecordspopup_subgrps(employee);
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };



        $scope.interacted = function (field) {

            return $scope.submitted;// || field.$dirty
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted1;// || field.$dirty
        };
        $scope.interacted2 = function (field) {

            return $scope.submitted2;// || field.$dirty
        };



        // to Edit Data
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.emP_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("PromotionSetting/editdetails", pageid).then(function (promise) {
                $scope.EMP_Id = promise.edit_m_pro[0].emP_Id;
                $scope.ASMAY_Id = promise.edit_m_pro[0].asmaY_Id;
                if (promise.edit_m_pro[0].emP_MarksPerFlg !== 'F') {
                    $scope.selected_pro_subjects = promise.edit_m_pro_subs;
                    if (promise.edit_m_pro[0].emP_BestOfApplicableFlg === null) {
                        $scope.EMP_BestOfApplicableFlg = false;
                    } else {
                        $scope.EMP_BestOfApplicableFlg = promise.edit_m_pro[0].emP_BestOfApplicableFlg;
                    }
                    $scope.EMP_BestOf = promise.edit_m_pro[0].emP_BestOf;
                }

                $scope.EYC_Id = promise.edit_m_pro[0].eyC_Id;
                $scope.temp_category = promise.edit_m_pro[0].eyC_Id;

                $scope.EMGR_Id = promise.edit_m_pro[0].emgR_Id;
                $scope.EMP_PassToIndSubjectFlg = promise.edit_m_pro[0].emP_PassToIndSubjectFlg;
                $scope.EMP_PassToOverallFlag = promise.edit_m_pro[0].emP_PassToOverallFlag;
                $scope.EMP_MarksPerFlg = promise.edit_m_pro[0].emP_MarksPerFlg;

                $scope.group_list_subwise = [];
                $scope.edit_m_pro_subs_grps = promise.edit_m_pro_subs_grps;

                angular.forEach($scope.selected_pro_subjects, function (dd) {
                    $scope.group_list_subwise = [];
                    angular.forEach($scope.edit_m_pro_subs_grps, function (d) {
                        if (dd.empS_Id === d.empS_Id) {
                            $scope.group_list_subwise.push({
                                EMPSG_GroupName: d.empsG_GroupName, EMPSG_DisplayName: d.empsG_DisplayName, EMPSG_PercentValue: Number(d.empsG_PercentValue),
                                EMPSG_MarksValue: Number(d.empsG_MarksValue), EMPSG_MaxOff: Number(d.empsG_MaxOff), EMPSG_BestOff: Number(d.empsG_BestOff),
                                EMPSG_Order: Number(d.empsG_Order), EMPSG_Id: d.empsG_Id, EMPSG_RoundOffFlag: d.empsG_RoundOffFlag,
                            });
                        }
                    });
                    dd.pro_exams_group_list = $scope.group_list_subwise;
                });

                var Selected_temp_examlist = [];
                $scope.edit_m_pro_subs_grps_exms = promise.edit_m_pro_subs_grps_exms;
                angular.forEach($scope.selected_pro_subjects, function (ddd) {
                    var Selected_temp_examlist = [];
                    angular.forEach(ddd.pro_exams_group_list, function (dd) {
                        var Selected_temp_examlist = [];
                        angular.forEach($scope.edit_m_pro_subs_grps_exms, function (d) {
                            if (d.empsG_Id === dd.EMPSG_Id) {
                                Selected_temp_examlist.push({
                                    emE_ExamName: d.emE_ExamName,
                                    EMPSGE_ConvertionReqOrNot: d.empsgE_ConvertionReqOrNot, EMPSGE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs, emE_Id: d.emE_Id
                                });
                            }
                        });
                        dd.Exm_M_Prom_Subj_Group_Exams_master = Selected_temp_examlist;
                    });
                });

                if ($scope.ASMAY_Id !== "") {
                    $scope.get_category($scope.ASMAY_Id);
                }
                if ($scope.EYC_Id !== "") {
                    $scope.get_subjects($scope.EYC_Id);
                }

                //for (var z = 0; z < promise.edit_m_pro_subs_grps.length; z++) {

                //    var EMPSG_Id = promise.edit_m_pro_subs_grps[z].empsG_Id;
                //    var Selected_temp_examlist = [];
                //    angular.forEach(promise.edit_m_pro_subs_grps_exms, function (opj) {
                //        if (opj.empsG_Id == EMPSG_Id) {
                //            angular.forEach($scope.exam_list, function (opj1) {
                //                if (opj1.emE_Id == opj.emE_Id) {
                //                    Selected_temp_examlist.push(opj1);
                //                }
                //            });
                //        }
                //    });

                //    if ($scope.group_list_subwise.length === 0) {
                //        $scope.group_list_subwise.push({
                //            EMPSG_GroupName: promise.edit_m_pro_subs_grps[z].empsG_GroupName, EMPSG_DisplayName: promise.edit_m_pro_subs_grps[z].empsG_DisplayName,
                //            EMPSG_PercentValue: Number(promise.edit_m_pro_subs_grps[z].empsG_PercentValue),
                //            EMPSG_MarksValue: Number(promise.edit_m_pro_subs_grps[z].empsG_MarksValue),
                //            EMPSG_MaxOff: Number(promise.edit_m_pro_subs_grps[z].empsG_MaxOff), EMPSG_BestOff: Number(promise.edit_m_pro_subs_grps[z].empsG_BestOff),
                //            Exm_M_Prom_Subj_Group_Exams_master: Selected_temp_examlist, EMPSG_Order: Number(promise.edit_m_pro_subs_grps[z].empsG_Order)
                //        });
                //        $scope.group_cnt = $scope.group_list_subwise.length;
                //    }
                //    else if ($scope.group_list_subwise.length > 0) {
                //        var dupli_count = 0;
                //        angular.forEach($scope.group_list_subwise, function (opti) {
                //            if (opti.EMPSG_GroupName.toUpperCase() == promise.edit_m_pro_subs_grps[z].empsG_GroupName.toUpperCase()
                //                && opti.EMPSG_DisplayName.toUpperCase() == promise.edit_m_pro_subs_grps[z].empsG_DisplayName.toUpperCase()
                //                && Number(opti.EMPSG_PercentValue) == Number(promise.edit_m_pro_subs_grps[z].empsG_PercentValue)
                //                && Number(opti.EMPSG_MarksValue) == Number(promise.edit_m_pro_subs_grps[z].empsG_MarksValue)
                //                && Number(opti.EMPSG_MaxOff) == Number(promise.edit_m_pro_subs_grps[z].empsG_MaxOff)
                //                && Number(opti.EMPSG_BestOff) == Number(promise.edit_m_pro_subs_grps[z].empsG_BestOff)
                //                && Number(opti.EMPSG_Order) == Number(promise.edit_m_pro_subs_grps[z].empsG_Order)) {
                //                if (opti.Exm_M_Prom_Subj_Group_Exams_master.length == Selected_temp_examlist.length) {
                //                    var exam_dupli_count = 0;
                //                    angular.forEach(opti.Exm_M_Prom_Subj_Group_Exams_master, function (s1) {
                //                        angular.forEach(Selected_temp_examlist, function (s2) {
                //                            if (s1.emE_Id == s2.emE_Id) {
                //                                exam_dupli_count += 1;
                //                            }
                //                        });
                //                    });
                //                    if (exam_dupli_count == Selected_temp_examlist.length) {
                //                        dupli_count += 1;
                //                    }
                //                }
                //            }
                //        });
                //        if (dupli_count === 0) {
                //            $scope.group_list_subwise.push({
                //                EMPSG_GroupName: promise.edit_m_pro_subs_grps[z].empsG_GroupName,
                //                EMPSG_DisplayName: promise.edit_m_pro_subs_grps[z].empsG_DisplayName,
                //                EMPSG_PercentValue: Number(promise.edit_m_pro_subs_grps[z].empsG_PercentValue),
                //                EMPSG_MarksValue: Number(promise.edit_m_pro_subs_grps[z].empsG_MarksValue),
                //                EMPSG_MaxOff: Number(promise.edit_m_pro_subs_grps[z].empsG_MaxOff),
                //                EMPSG_BestOff: Number(promise.edit_m_pro_subs_grps[z].empsG_BestOff),
                //                Exm_M_Prom_Subj_Group_Exams_master: Selected_temp_examlist, EMPSG_Order: Number(promise.edit_m_pro_subs_grps[z].empsG_Order)
                //            });
                //            $scope.group_cnt = $scope.group_list_subwise.length;
                //        }
                //    }
                //}

                //angular.forEach($scope.group_list_subwise, function (opti) {
                //    for (var l = 0; l < $scope.exm_prom_groups_list.length; l++) {
                //        if (opti.EMPSG_GroupName.toUpperCase() == $scope.exm_prom_groups_list[l].empsG_GroupName.toUpperCase()
                //            && opti.EMPSG_DisplayName.toUpperCase() == $scope.exm_prom_groups_list[l].empsG_DisplayName.toUpperCase()
                //            && Number(opti.EMPSG_PercentValue) == Number($scope.exm_prom_groups_list[l].empsG_PercentValue)
                //            && Number(opti.EMPSG_MarksValue) == Number($scope.exm_prom_groups_list[l].empsG_MarksValue)
                //            && Number(opti.EMPSG_MaxOff) == Number($scope.exm_prom_groups_list[l].empsG_MaxOff)
                //            && Number(opti.EMPSG_BestOff) == Number($scope.exm_prom_groups_list[l].empsG_BestOff)
                //            && Number(opti.EMPSG_Order) == Number($scope.exm_prom_groups_list[l].empsG_Order)) {
                //            $scope.exm_prom_groups_list.splice(l, 1);
                //            l--;
                //        }
                //    }
                //});
                console.log($scope.selected_pro_subjects);
                $scope.all1 = true;
                $scope.toggleAll1();

                $scope.promotion_calculation_flag = promise.calculated_Flag;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        //to save subsubjects
        $scope.submitted1 = false;
        $scope.group_list_subwise = [];
        $scope.group_cnt = $scope.group_list_subwise.length;

        $scope.saveddata1 = function (subjs_subs) {
            $scope.submitted1 = true;

            if ($scope.myForm1.$valid) {

                if ($scope.index_editgroupmapping !== undefined && $scope.index_editgroupmapping !== null && $scope.index_editgroupmapping !== ""
                    && $scope.index_editgroupmapping >= 0) {
                    $scope.group_list_subwise.splice($scope.index_editgroupmapping, 1);
                }

                $scope.temp_exam_list = [];
                angular.forEach($scope.exam_list, function (itm) {
                    if (itm.checked == true) {
                        $scope.temp_exam_list.push({
                            emE_ExamName: itm.emE_ExamName,
                            EMPSGE_ConvertionReqOrNot: itm.ConvertionReqOrNot, EMPSGE_ForMaxMarkrs: itm.ForMaxMarkrs, emE_Id: itm.emE_Id
                        });
                    }
                });

                if ($scope.group_list_subwise.length === 0) {
                    if ($scope.EMP_MarksPerFlg === 'P') {
                        var percentage = 0;
                        angular.forEach($scope.group_list_subwise, function (opti) {
                            percentage += Number(opti.EMPSG_PercentValue);
                        })
                        percentage += Number($scope.EMPSG_PercentValue);
                        if (percentage <= 100) {
                            $scope.group_list_subwise.push({
                                EMPSG_GroupName: $scope.EMPSG_GroupName, EMPSG_DisplayName: $scope.EMPSG_DisplayName,
                                EMPSG_PercentValue: Number($scope.EMPSG_PercentValue), EMPSG_MarksValue: Number($scope.EMPSG_MarksValue),
                                EMPSG_MaxOff: Number($scope.EMPSG_MaxOff), EMPSG_BestOff: Number($scope.EMPSG_BestOff),
                                EMPSG_Order: Number($scope.EMPSG_Order), EMPSG_RoundOffFlag: $scope.EMPSG_RoundOffFlag,
                                Exm_M_Prom_Subj_Group_Exams_master: $scope.temp_exam_list
                            });
                        }
                    }

                    else if ($scope.EMP_MarksPerFlg == 'M') {
                        var max_marks_value = 0;
                        angular.forEach($scope.subject_list, function (opte) {
                            if (opte.checkedvalue == true && opte.ismS_Id === $scope.subjectid) {
                                max_marks_value = Number(opte.EMPS_MaxMarks);
                            }
                        });
                        var chk_marksval_tot = 0;
                        angular.forEach($scope.group_list_subwise, function (opti) {
                            chk_marksval_tot += Number(opti.EMPSG_MarksValue);
                        });

                        if (chk_marksval_tot <= max_marks_value) {
                            $scope.group_list_subwise.push({
                                EMPSG_GroupName: $scope.EMPSG_GroupName, EMPSG_DisplayName: $scope.EMPSG_DisplayName,
                                EMPSG_PercentValue: Number($scope.EMPSG_PercentValue), EMPSG_MarksValue: Number($scope.EMPSG_MarksValue),
                                EMPSG_MaxOff: Number($scope.EMPSG_MaxOff), EMPSG_BestOff: Number($scope.EMPSG_BestOff),
                                EMPSG_Order: Number($scope.EMPSG_Order), EMPSG_RoundOffFlag: $scope.EMPSG_RoundOffFlag,
                                Exm_M_Prom_Subj_Group_Exams_master: $scope.temp_exam_list
                            });
                        }
                    }

                    $scope.group_cnt = $scope.group_list_subwise.length;
                }

                else if ($scope.group_list_subwise.length > 0) {
                    var dupli_count = 0;
                    angular.forEach($scope.group_list_subwise, function (opti) {
                        if (opti.EMPSG_GroupName.toUpperCase() == $scope.EMPSG_GroupName.toUpperCase() && opti.EMPSG_DisplayName.toUpperCase() == $scope.EMPSG_DisplayName.toUpperCase() && Number(opti.EMPSG_PercentValue) == Number($scope.EMPSG_PercentValue) && Number(opti.EMPSG_MarksValue) == Number($scope.EMPSG_MarksValue) && Number(opti.EMPSG_MaxOff) == Number($scope.EMPSG_MaxOff) && Number(opti.EMPSG_BestOff) == Number($scope.EMPSG_BestOff) && opti.EMPSG_Order == Number($scope.EMPSG_Order)) {
                            if (opti.Exm_M_Prom_Subj_Group_Exams_master.length == $scope.temp_exam_list.length) {
                                var exam_dupli_count = 0;
                                angular.forEach(opti.Exm_M_Prom_Subj_Group_Exams_master, function (s1) {
                                    angular.forEach($scope.temp_exam_list, function (s2) {
                                        if (s1.emE_Id == s2.emE_Id) {
                                            exam_dupli_count += 1;
                                        }
                                    });
                                });
                                if (exam_dupli_count == $scope.temp_exam_list.length) {
                                    dupli_count += 1;
                                }
                            }
                        }
                    });
                    if (dupli_count == 0) {
                        if ($scope.EMP_MarksPerFlg == 'P') {
                            var percentage = 0;
                            angular.forEach($scope.group_list_subwise, function (opti) {
                                percentage += Number(opti.EMPSG_PercentValue);
                            });
                            percentage += Number($scope.EMPSG_PercentValue);
                            if (percentage <= 100) {
                                $scope.group_list_subwise.push({ EMPSG_GroupName: $scope.EMPSG_GroupName, EMPSG_DisplayName: $scope.EMPSG_DisplayName, EMPSG_PercentValue: $scope.EMPSG_PercentValue, EMPSG_MarksValue: $scope.EMPSG_MarksValue, EMPSG_MaxOff: $scope.EMPSG_MaxOff, EMPSG_BestOff: $scope.EMPSG_BestOff, EMPSG_Order: $scope.EMPSG_Order, EMPSG_RoundOffFlag: $scope.EMPSG_RoundOffFlag, Exm_M_Prom_Subj_Group_Exams_master: $scope.temp_exam_list });
                            }
                            else {
                                swal("Total of Groups Percentage Value is Not More Than 100");
                                $scope.clear1();
                            }
                        }
                        else if ($scope.EMP_MarksPerFlg == 'M') {
                            var max_marks_value = 0;
                            angular.forEach($scope.subject_list, function (opte) {
                                if (opte.checkedvalue == true) {
                                    max_marks_value = Number(opte.EMPS_MaxMarks)
                                }
                            });
                            var chk_marksval_tot = 0;
                            angular.forEach($scope.group_list_subwise, function (opti) {
                                chk_marksval_tot += Number(opti.EMPSG_MarksValue);
                            });

                            $scope.group_list_subwise.push({
                                EMPSG_GroupName: $scope.EMPSG_GroupName, EMPSG_DisplayName: $scope.EMPSG_DisplayName,
                                EMPSG_PercentValue: Number($scope.EMPSG_PercentValue), EMPSG_MarksValue: Number($scope.EMPSG_MarksValue),
                                EMPSG_MaxOff: Number($scope.EMPSG_MaxOff), EMPSG_BestOff: Number($scope.EMPSG_BestOff),
                                EMPSG_Order: Number($scope.EMPSG_Order), EMPSG_RoundOffFlag: $scope.EMPSG_RoundOffFlag,
                                Exm_M_Prom_Subj_Group_Exams_master: $scope.temp_exam_list
                            });

                            //if (chk_marksval_tot <= max_marks_value) {
                            //    $scope.group_list_subwise.push({
                            //        EMPSG_GroupName: $scope.EMPSG_GroupName, EMPSG_DisplayName: $scope.EMPSG_DisplayName,
                            //        EMPSG_PercentValue: Number($scope.EMPSG_PercentValue), EMPSG_MarksValue: Number($scope.EMPSG_MarksValue),
                            //        EMPSG_MaxOff: Number($scope.EMPSG_MaxOff), EMPSG_BestOff: Number($scope.EMPSG_BestOff),
                            //        EMPSG_Order: Number($scope.EMPSG_Order), Exm_M_Prom_Subj_Group_Exams_master: $scope.temp_exam_list
                            //    });
                            //}
                            //else {
                            //    swal("Total of Groups Marks Value is Not More Than Subjects Max.Marks" + max_marks_value);
                            //    $scope.clear1();
                            //}
                        }
                        $scope.group_cnt = $scope.group_list_subwise.length;
                    }
                    else if (dupli_count > 0) {
                        swal("Entered Details Are Already In Below List");
                    }
                }

                console.log($scope.group_list_subwise);
                $scope.all1 = true;
                $scope.toggleAll1();
                $scope.clear1();
                $scope.index_editgroupmapping = "";
                angular.forEach($scope.exam_list, function (d) {
                    d.checked = false;
                    d.ForMaxMarkrs = d.ForMaxMarkrs_Temp;
                    d.ConvertionReqOrNot = false;
                });

            }
            else {
                $scope.submitted1 = true;
            }
        };

        //to save subsexams
        $scope.submitted2 = false;
        // var SubWise_Selected_subexms_list = [];
        $scope.SubWise_Selected_subexms_list = [];

        $scope.saveddata2 = function (exms_subs) {
            $scope.submitted2 = true;
            if ($scope.myForm2.$valid) {

                var final_count = 0;
                angular.forEach(exms_subs, function (itm) {
                    if (itm.checkedvalue) {
                        final_count += 1;
                    }
                })
                if (final_count == 0) {
                    angular.forEach($scope.subject_list, function (opt786) {
                        if ($scope.ismS_Id == opt786.ismS_Id) {
                            for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) {
                                var already_count = 0;
                                if ($scope.ismS_Id == $scope.SubWise_Selected_subexms_list[i].ISMS_Id) {
                                    already_count += 1;
                                    // $scope.SubWise_Selected_subexms_list.splice(i, 1);
                                    if (already_count > 0) {
                                        $scope.SubWise_Selected_subexms_list.splice(i, 1);
                                    }
                                }
                            }
                            opt786.EYCES_SubExamFlg = false;
                        }
                    })
                }

                else if (final_count > 0) {
                    var Selected_subexms_list = [];
                    angular.forEach(exms_subs, function (itm) {
                        var newCol = "";
                        if (itm.checkedvalue) {
                            //$scope.Selected_subsubjs_list.push({ ismS_Id: $scope.ismS_Id, sub_exam_list: itm });
                            newCol = { EMSE_Id: itm.emsE_Id, EMGR_Id: itm.EMGR_Id, EYCESSE_MaxMarks: itm.EYCESSE_MaxMarks, EYCESSE_MinMarks: itm.EYCESSE_MinMarks, EYCESSE_ExemptedFlg: itm.EYCESSE_ExemptedFlg, EYCESSE_ExemptedPer: itm.EYCESSE_ExemptedPer, EYCESSE_SubExamOrder: itm.EYCESSE_SubExamOrder }
                            Selected_subexms_list.push(newCol);
                        }
                    })

                    for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) {
                        var already_count = 0;
                        if ($scope.ismS_Id == $scope.SubWise_Selected_subexms_list[i].ISMS_Id) {
                            already_count += 1;
                            // $scope.SubWise_Selected_subexms_list.splice(i, 1);
                            if (already_count > 0) {
                                $scope.SubWise_Selected_subexms_list.splice(i, 1);
                            }
                        }
                    }

                    $scope.SubWise_Selected_subexms_list.push({ ISMS_Id: $scope.ismS_Id, sub_exam_list: Selected_subexms_list });
                }
                $('#popup5').modal('hide');
                $scope.subexam_list = [];
            }
            else {
                $scope.submitted2 = true;
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

        $scope.getOrder3 = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.emsS_Id !== 0) {
                    orderarray[key].emsS_Order = key + 1;
                    orderarray[key].EYCESSS_SubSubjectOrder = key + 1;
                    //  opt.EYCESSS_SubSubjectOrder = opt.emsS_Order;

                }
            });

        }
        $scope.getOrder5 = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.emsE_Id !== 0) {
                    orderarray[key].emsE_SubExamOrder = key + 1;
                    orderarray[key].EYCESSE_SubExamOrder = key + 1;
                    // opt.EYCESSE_SubExamOrder = opt.emsE_SubExamOrder;
                }
            });

        }

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {

            $scope.submitted = true;

            if ($scope.EMP_MarksPerFlg == 'F') {
                // swal("SElected total");
                if ($scope.myForm.$valid) {
                    var data = {
                        "EMP_Id": $scope.EMP_Id,
                        "EYC_Id": $scope.EYC_Id,
                        "EMGR_Id": $scope.EMGR_Id,
                        "EMP_PassToIndSubjectFlg": $scope.EMP_PassToIndSubjectFlg,
                        "EMP_PassToOverallFlag": $scope.EMP_PassToOverallFlag,
                        "EMP_MarksPerFlg": $scope.EMP_MarksPerFlg
                    };
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    };

                    apiService.create("PromotionSetting/savedetails", data).then(function (promise) {
                        angular.forEach(promise.promotion_details, function (itms) {
                            if (itms.emP_MarksPerFlg === 'M') {
                                itms.emP_MarksPerFlg = 'Marks';
                            }
                            else if (itms.emP_MarksPerFlg === 'P') {
                                itms.emP_MarksPerFlg = 'Percentage';
                            }
                            else if (itms.emP_MarksPerFlg === 'T') {
                                itms.emP_MarksPerFlg = 'Total';
                            }
                            else if (itms.emP_MarksPerFlg === 'F') {
                                itms.emP_MarksPerFlg = 'Final Exam';
                            }
                        });
                        $scope.gridOptions.data = promise.promotion_details;
                        $scope.promotion_details_list = promise.promotion_details;
                        if (promise.returnval === true) {
                            if (promise.emP_Id == 0 || promise.emP_Id < 0) {
                                swal('Record saved successfully');
                            }
                            else if (promise.emP_Id > 0) {
                                swal('Record updated successfully');
                            }
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.emP_Id == 0 || promise.emP_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.emP_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        //$scope.BindData();
                        $scope.clear();
                    });
                } else {
                    $scope.submitted = true;
                }
            }

            else if ($scope.EMP_MarksPerFlg === "T") {

                if ($scope.myForm.$valid) {

                    $scope.subject_list_saved = [];

                    var bestoffcount1 = 0;
                    if ($scope.EMP_BestOfApplicableFlg === true) {
                        bestoffcount1 = $scope.EMP_BestOf;
                    } else {
                        bestoffcount1 = null;
                    }

                    angular.forEach($scope.subject_list, function (opt123) {
                        if (opt123.checkedvalue) {
                            $scope.subject_list_saved.push(opt123);
                        }
                    });

                    var data = {
                        "EMP_Id": $scope.EMP_Id,
                        "EMPS_Id": $scope.EMPS_Id,
                        "EMPSG_Id": $scope.EMPSG_Id,
                        "EMPSGE_Id": $scope.EMPSGE_Id,
                        "EYC_Id": $scope.EYC_Id,
                        "EMGR_Id": $scope.EMGR_Id,
                        "EMP_PassToIndSubjectFlg": $scope.EMP_PassToIndSubjectFlg,
                        "EMP_PassToOverallFlag": $scope.EMP_PassToOverallFlag,
                        "EMP_MarksPerFlg": $scope.EMP_MarksPerFlg,
                        "EMP_BestOfApplicableFlg": $scope.EMP_BestOfApplicableFlg,
                        "EMP_BestOf": bestoffcount1,
                        pro_subjects_list: $scope.subject_list_saved
                    };

                    apiService.create("PromotionSetting/savedetails", data).then(function (promise) {
                        angular.forEach(promise.promotion_details, function (itms) {
                            if (itms.emP_MarksPerFlg === 'M') {
                                itms.emP_MarksPerFlg = 'Marks';
                            }
                            else if (itms.emP_MarksPerFlg === 'P') {
                                itms.emP_MarksPerFlg = 'Percentage';
                            }
                            else if (itms.emP_MarksPerFlg === 'T') {
                                itms.emP_MarksPerFlg = 'Total';
                            }
                            else if (itms.emP_MarksPerFlg === 'F') {
                                itms.emP_MarksPerFlg = 'Final Exam';
                            }
                        });
                        $scope.gridOptions.data = promise.promotion_details;
                        $scope.promotion_details_list = promise.promotion_details;
                        if (promise.returnval === true) {
                            if (promise.emP_Id == 0 || promise.emP_Id < 0) {
                                swal('Record saved successfully');
                            }
                            else if (promise.emP_Id > 0) {
                                swal('Record updated successfully');
                            }
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.emP_Id == 0 || promise.emP_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.emP_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        //$scope.BindData();
                        $scope.clear();
                    });


                } else {
                    $scope.submitted = true;
                }
            }

            else if ($scope.EMP_MarksPerFlg !== 'T' && $scope.EMP_MarksPerFlg !== 'F') {

                if ($scope.myForm.$valid) {
                    $scope.selected_group_list_subwise = [];
                    $scope.subject_list_saved = [];

                    var bestoffcount = 0;
                    if ($scope.EMP_BestOfApplicableFlg === true) {
                        bestoffcount = $scope.EMP_BestOf;
                    } else {
                        bestoffcount = null;
                    }

                    //angular.forEach($scope.group_list_subwise, function (opt123) {
                    //    if (opt123.checkedvalue) {
                    //        $scope.selected_group_list_subwise.push(opt123);
                    //    }
                    //});
                    var countloop = 0;
                    var subjectlist = "";
                    angular.forEach($scope.subject_list, function (opt123) {
                        if (opt123.checkedvalue) {
                            if (opt123.pro_exams_group_list === undefined || opt123.pro_exams_group_list === null || opt123.pro_exams_group_list.length === 0) {
                                countloop = 1;
                                if (subjectlist === "") {
                                    subjectlist = opt123.ismS_SubjectName;
                                } else {
                                    subjectlist = subjectlist + "," + opt123.ismS_SubjectName;
                                }
                            } else {
                                $scope.subject_list_saved.push(opt123);
                            }
                        }
                    });
                    if ($scope.subject_list_saved.length > 0 && countloop === 0) {

                        //if ($scope.selected_group_list_subwise.length > 0) {
                        var chk_percentage = 0;
                        //if ($scope.EMP_MarksPerFlg === 'P') {
                        //    angular.forEach($scope.selected_group_list_subwise, function (oops) {
                        //        chk_percentage += Number(oops.EMPSG_PercentValue);
                        //    });
                        //} else {
                        chk_percentage = 100;
                        //}
                        if (chk_percentage === 100) {
                            if ($scope.ASMAY_Id !== null && $scope.ASMAY_Id !== "" && $scope.ASMAY_Id !== undefined && $scope.EYC_Id !== null && $scope.EYC_Id !== "" && $scope.EYC_Id !== undefined && $scope.EMGR_Id !== null && $scope.EMGR_Id !== "" && $scope.EMGR_Id !== undefined) {

                                if ($scope.EMP_BestOfApplicableFlg === true && ($scope.EMP_BestOf === undefined || $scope.EMP_BestOf === null
                                    || $scope.EMP_BestOf === "")) {
                                    $scope.submitted = true;
                                    swal("Enter The Best Off Count");
                                    return;
                                } else {
                                    $scope.submitted = false;
                                }

                                var data = {
                                    "EMP_Id": $scope.EMP_Id,
                                    "EMPS_Id": $scope.EMPS_Id,
                                    "EMPSG_Id": $scope.EMPSG_Id,
                                    "EMPSGE_Id": $scope.EMPSGE_Id,
                                    "EYC_Id": $scope.EYC_Id,
                                    "EMGR_Id": $scope.EMGR_Id,
                                    "EMP_PassToIndSubjectFlg": $scope.EMP_PassToIndSubjectFlg,
                                    "EMP_PassToOverallFlag": $scope.EMP_PassToOverallFlag,
                                    "EMP_MarksPerFlg": $scope.EMP_MarksPerFlg,
                                    "EMP_BestOfApplicableFlg": $scope.EMP_BestOfApplicableFlg,
                                    "EMP_BestOf": bestoffcount,
                                    pro_subjects_list: $scope.subject_list_saved
                                    // pro_exams_group_list: $scope.selected_group_list_subwise
                                };
                                console.log(data);
                                apiService.create("PromotionSetting/savedetails", data).then(function (promise) {
                                    angular.forEach(promise.promotion_details, function (itms) {
                                        if (itms.emP_MarksPerFlg === 'M') {
                                            itms.emP_MarksPerFlg = 'Marks';
                                        }
                                        else if (itms.emP_MarksPerFlg === 'P') {
                                            itms.emP_MarksPerFlg = 'Percentage';
                                        }
                                        else if (itms.emP_MarksPerFlg === 'T') {
                                            itms.emP_MarksPerFlg = 'Total';
                                        }
                                        else if (itms.emP_MarksPerFlg === 'F') {
                                            itms.emP_MarksPerFlg = 'Final Exam';
                                        }
                                    });
                                    $scope.gridOptions.data = promise.promotion_details;
                                    $scope.promotion_details_list = promise.promotion_details;
                                    if (promise.returnval === true) {
                                        if (promise.emP_Id == 0 || promise.emP_Id < 0) {
                                            swal('Record saved successfully');
                                        }
                                        else if (promise.emP_Id > 0) {
                                            swal('Record updated successfully');
                                        }
                                    }
                                    else if (promise.returnduplicatestatus === 'Duplicate') {
                                        swal('Record already exist');
                                    }
                                    else {
                                        if (promise.emP_Id == 0 || promise.emP_Id < 0) {
                                            swal('Failed to save, please contact administrator');
                                        }
                                        else if (promise.emP_Id > 0) {
                                            swal('Failed to update, please contact administrator');
                                        }
                                    }
                                    //$scope.BindData();
                                    $scope.clear();
                                });
                            }
                            else {
                                $scope.submitted = true;
                            }
                        }
                        else {
                            swal("Total Percentage Value Exams-Group Must Be 100 !!!");
                        }
                        //}
                        //else if ($scope.selected_group_list_subwise.length === 0) {
                        //    if ($scope.group_list_subwise.length === 0) {
                        //        swal("Please Create Atleast One Exams-Group !!!");
                        //    }
                        //    else if ($scope.group_list_subwise.length > 0) {
                        //        swal("Please Select Atleast One Exams-Group !!!");
                        //    }
                        //}
                    }
                    else {
                        if (subjectlist !== "") {
                            swal("For " + subjectlist + " Subjects Groups Are Not Mapped");
                        } else {
                            swal("Please Select Subjects !!!");
                        }
                    }
                } else {
                    $scope.submitted = true;
                }
            }
        };

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

        $scope.clear = function () {
            $state.reload();
        };

        $scope.clear1 = function () {
            if ($scope.EYC_Id !== null && $scope.EYC_Id !== undefined && $scope.EYC_Id !== "") {
                angular.forEach($scope.exam_list, function (dd) {
                    dd.checked = false;
                    dd.ConvertionReqOrNot = false;
                    dd.ForMaxMarkrs = dd.ForMaxMarkrs_Temp;
                });
            }
            else {
                $scope.exam_list = $scope.tempexamlist;
                angular.forEach($scope.exam_list, function (dd) {
                    dd.checked = false;
                    dd.ConvertionReqOrNot = false;
                    dd.ForMaxMarkrs = dd.ForMaxMarkrs_Temp;
                });
            }

            $scope.editgroupflag = 0;
            $scope.index_editgroupmapping = null;
            $scope.EMPSG_MarksValue = "";
            $scope.EMPSG_PercentValue = "";
            $scope.EMPSG_GroupName = "";
            $scope.EMPSG_DisplayName = "";
            $scope.EMPSG_RoundOffFlag = false;
            $scope.EMPSG_MaxOff = 0;
            $scope.EMPSG_BestOff = "";
            $scope.EMPSG_Id = 0;
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";
            if ($scope.EMP_MarksPerFlg == 'M') {
                $scope.EMPSG_PercentValue = 0;
            }
            else if ($scope.EMP_MarksPerFlg == 'P') {
                $scope.EMPSG_MarksValue = 0;
            }
            $scope.EMPSG_Order = "";
        };

        $scope.clear2 = function () {
            //$scope.subexam_list = $scope.temp_subexam_list;
            $scope.subexam_list = $scope.tempsubexamlist;
            angular.forEach($scope.subexam_list, function (itm1) {
                itm1.EYCESSE_MaxMarks = "";
                itm1.EYCESSE_MinMarks = "";
                itm1.EMGR_Id = "";
                //itm1.EMGR_Id = $scope.EMGR_Id;
                itm1.EYCESSE_ExemptedFlg = false;
                itm1.EYCESSE_ExemptedPer = "";
            })
            $scope.all2 = true;
            $scope.toggleAll2();
            $scope.EYCESSE_Id = 0;
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
        };

        $scope.viewrecordspopup = function (employee, SweetAlert) {

            $scope.editEmployee = employee.emP_Id;
            var pageid = $scope.editEmployee;

            $scope.emP_MarksPerFlgtemp = employee.emP_MarksPerFlg;

            apiService.getURI("PromotionSetting/getalldetailsviewrecords", pageid).then(function (promise) {

                $scope.Category_Name = promise.view_prom_subjects[0].emcA_CategoryName;
                //  $scope.Exam_Name = promise.view_prom_subjects[0].emE_ExamName;
                $scope.viewrecordspopupdisplay = promise.view_prom_subjects;

            })

        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };

        $scope.viewrecordspopup_sub_grp_exms = function (employee) {
            $scope.editEmployee = employee.empsG_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("PromotionSetting/getalldetailsviewrecords_sub_grp_exms", pageid).then(function (promise) {
                $scope.sub_groupName = promise.view_exam_subjects_subgroup_exms[0].empsG_GroupName;
                $scope.viewrecordspopup_subgrps_exms = promise.view_exam_subjects_subgroup_exms;
            });
        };

        $scope.clearpopupgrid_subgrps_exms = function () {
            $scope.viewrecordspopup_subgrps_exms = "";
        };

        $scope.viewrecordspopup_subgrps = function (employee) {
            $scope.editEmployee = employee.empS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("PromotionSetting/getalldetailsviewrecords_subgrps", pageid).then(function (promise) {
                $scope.pro_Subject = promise.view_exam_subjects_subgroups[0].ismS_SubjectName;
                $scope.viewrecordspopup_subgrps_list = promise.view_exam_subjects_subgroups;
            });
        };

        $scope.clearpopupgrid_subgrps = function () {
            $scope.viewrecordspopup_subgrps_list = "";
        };

        $scope.view_exams = function (index) {
            $scope.groupName = $scope.group_list_subwise[index].EMPSG_GroupName;
            $scope.view_exam_details = $scope.group_list_subwise[index].Exm_M_Prom_Subj_Group_Exams_master;
        };

        $scope.clear_exams = function () {
            $scope.view_exam_details = "";
        };

        $scope.EMPSG_MaxOff = 0;

        $scope.clr_max_bst = function () {
            if ($scope.EYC_Id != "" && $scope.EYC_Id != null && $scope.EYC_Id != undefined) {
                //dd
            }
            else {
                swal("First Select Category !!!");
                angular.forEach($scope.exam_list, function (abc) {
                    abc.checked = false;
                    itm1.ConvertionReqOrNot = false;
                    itm1.ForMaxMarkrs = itm1.ForMaxMarkrs_Temp;
                })
            }
            $scope.EMPSG_MaxOff = 0;
            $scope.EMPSG_BestOff = 0;
            var totalmarks = 0;

            angular.forEach($scope.exam_list, function (opt123) {
                if (opt123.checked == true) {
                    $scope.EMPSG_MaxOff += 1;
                    $scope.EMPSG_BestOff += 1;
                    if (opt123.ForMaxMarkrs !== undefined && opt123.ForMaxMarkrs !== null && opt123.ForMaxMarkrs !== "") {
                        totalmarks += Number(opt123.ForMaxMarkrs)
                    } 
                    
                }
            });

            if ($scope.EMP_MarksPerFlg == 'M') {
                $scope.EMPSG_MarksValue = "";
            }

            $scope.EMPSG_MarksValue = totalmarks;

            //$scope.EMPSG_MarksValue = "";
            //$scope.EMPSG_BestOff = "";
            //$scope.EMPSG_MaxOff = "";

        };

        $scope.onchangeexammaxmarks = function () {

            var totalexammarks = 0;
            angular.forEach($scope.exam_list, function (dd) {
                if (dd.checked) {
                    if (dd.ForMaxMarkrs !== undefined && dd.ForMaxMarkrs !== null && dd.ForMaxMarkrs !== "") {
                        totalexammarks += Number(dd.ForMaxMarkrs);
                    }                 
                }
            });

            $scope.EMPSG_MarksValue =0;
            $scope.EMPSG_PercentValue = 0;

            if ($scope.EMP_MarksPerFlg === "M") {
                $scope.EMPSG_MarksValue = totalexammarks;
            }

            if ($scope.EMP_MarksPerFlg === "P") {
                $scope.EMPSG_PercentValue = totalmarks;
            }
        };

        $scope.valid_maxoff = function (maxoff) {
            var select_exm_cnt = 0;
            angular.forEach($scope.exam_list, function (opt123) {
                if (opt123.checked == true) {
                    select_exm_cnt += 1;
                }
            })
            var num_maxoff = Number(maxoff);
            if (num_maxoff > select_exm_cnt) {
                $scope.EMPSG_MaxOff = "";
            }
            $scope.EMPSG_BestOff = "";
        };

        $scope.valid_bestoff = function (bestoff) {
            if (bestoff != undefined && bestoff != null && bestoff != "") {
                if ($scope.EMPSG_MaxOff != 0) {
                    if ($scope.EMPSG_MaxOff != null && $scope.EMPSG_MaxOff != undefined && $scope.EMPSG_MaxOff != "") {
                        //var select_exm_cnt = 0;
                        //angular.forEach($scope.exam_list, function (opt123) {
                        //    if (opt123.checked == true) {
                        //        select_exm_cnt += 1;
                        //    }
                        //})

                        var num_bestoff = Number(bestoff);
                        var num_maxoff = Number($scope.EMPSG_MaxOff);
                        if (num_bestoff == 0) {
                            swal("Best Off Exams Not Should Be Zero !!!");
                            $scope.EMPSG_BestOff = "";

                        }
                        if (num_bestoff > num_maxoff) {
                            $scope.EMPSG_BestOff = "";
                        }
                        // $scope.EMPSG_BestOff = "";
                    }
                    else {
                        if (bestoff != null && bestoff != "" && bestoff != undefined) {
                            swal("First set Max Off Exams !!!");
                        }
                        //if ($scope.EMP_Id == null || $scope.EMP_Id == 0)
                        //{
                        //    swal("First set Max Off Exams !!!");
                        //}
                        //swal("First set Max Off Exams !!!");
                        $scope.EMPSG_BestOff = "";
                    }
                }
                else {
                    swal("First Select Exams !!!");
                    $scope.EMPSG_BestOff = "";
                }
            }
        };

        $scope.valid_per = function (per) {

            var num_per = Number(per);
            if (num_per > 100) {
                swal("Max Value Is 100");
                $scope.EMPSG_PercentValue = "";
            }
        };

        $scope.valid_marksval = function (marksval) {

            if (marksval != null && marksval != "" && marksval != undefined) {
                if ($scope.EYC_Id != "" && $scope.EYC_Id != null && $scope.EYC_Id != undefined) {
                    var selct_subject_count = 0;
                    var max_marks_value = 0;
                    angular.forEach($scope.subject_list, function (opte) {
                        if (opte.checkedvalue == true) {
                            selct_subject_count += 1;
                            max_marks_value = Number(opte.EMPS_MaxMarks)
                        }
                    });
                    if (selct_subject_count > 0) {
                        //if (Number(marksval) > max_marks_value) {
                        //    swal("Marks Should Be Not More Than Of Subjects Max.Marks" + marksval);
                        //    $scope.EMPSG_MarksValue = "";
                        //}
                    }
                    else {
                        swal("Select Subjects First !!!");
                        $scope.EMPSG_MarksValue = "";
                    }
                }
                else {
                    swal("First Select Category !!!");
                    $scope.EMPSG_MarksValue = "";

                }
            }
        };

        $scope.check_grp_name = function (grp_name) {
            var grp_name_dup_cnt = 0; 

            if ($scope.editgroupflag === 1) {
                angular.forEach($scope.group_list_subwise, function (t2, index) {
                    if (t2.EMPSG_GroupName.toUpperCase() == grp_name.toUpperCase() && index !== $scope.index_editgroupmapping) {
                        grp_name_dup_cnt += 1;
                    }
                });
            }
            else {
                angular.forEach($scope.group_list_subwise, function (t2, index) {
                    if (t2.EMPSG_GroupName.toUpperCase() == grp_name.toUpperCase()) {
                        grp_name_dup_cnt += 1;
                    }
                });
            }
            if(grp_name_dup_cnt>0){
                swal("Entered GroupName Already Existed,So Use New One !!!");
                $scope.EMPSG_GroupName = "";
            }
        };

        $scope.check_grp_name_order = function (grp_name_order) {
            var grp_name_order_dup_cnt = 0;

            if ($scope.editgroupflag === 1) {
                angular.forEach($scope.group_list_subwise, function (t2, index) {
                    if (Number(t2.EMPSG_Order) == Number(grp_name_order) && index !== $scope.index_editgroupmapping) {
                        grp_name_order_dup_cnt += 1;
                        $scope.EMPSG_Order = "";
                        swal("Group Order Already Entered");
                    }
                });
            }
            else {
                angular.forEach($scope.group_list_subwise, function (t2) {
                    if (Number(t2.EMPSG_Order) == Number(grp_name_order)) {
                        grp_name_order_dup_cnt += 1;
                        $scope.EMPSG_Order = "";
                        swal("Group Order Already Entered");
                    }
                });
            }
        };


        $scope.check_dis_name = function (dis_name) {
            var dis_name_dup_cnt = 0;
            //angular.forEach($scope.exm_prom_groups_list, function (t1) {
            //    if (t1.empsG_DisplayName.toUpperCase() == dis_name.toUpperCase()) {
            //        dis_name_dup_cnt += 1;
            //    }
            //    //&& t1.EMPSG_DisplayName.toUpperCase() == $scope.EMPSG_DisplayName.toUpperCase()
            //})
            angular.forEach($scope.group_list_subwise, function (t2) {
                if (t2.EMPSG_DisplayName.toUpperCase() == dis_name.toUpperCase()) {
                    dis_name_dup_cnt += 1;
                }
                //&& t1.EMPSG_DisplayName.toUpperCase() == $scope.EMPSG_DisplayName.toUpperCase()
            });
            if (dis_name_dup_cnt > 0) {
                swal("Entered DisplayName Already Existed,So Use New One !!!");
                $scope.EMPSG_DisplayName = "";
            }
        };

        $scope.select_subjs = function () {
            //  $scope.all = true;
            // $scope.toggleAll();
            if ($scope.EMP_MarksPerFlg == 'M') {
                $scope.EMPSG_PercentValue = 0;
                $scope.EMPSG_MarksValue = "";
                angular.forEach($scope.subject_list, function (ps) {
                    ps.EMPS_MaxMarks = "";
                    ps.EMPS_MinMarks = "";
                    ps.EMPS_ConvertForMarks = "";
                })
            }
            else if ($scope.EMP_MarksPerFlg == 'P') {
                $scope.EMPSG_MarksValue = 0;
                $scope.EMPSG_PercentValue = "";
            }
            $scope.group_list_subwise = [];
            $scope.group_cnt = $scope.group_list_subwise.length;
        }

        $scope.delete = function (row, index) {
            for (var x = 0; x < $scope.group_list_subwise.length; x++) {
                if (x == index) {
                    $scope.group_list_subwise.splice(x, 1);
                }
            }
            $scope.group_cnt = $scope.group_list_subwise.length;
            $scope.clear1();
        };

        $scope.AddGroupDetails = function (userdd, subjectname, subjectcode, index) {
            $scope.subjectname = subjectname + "(" + subjectcode + ")";
            $scope.subjectindex = index;
            $scope.submitted1 = false;
            $scope.EMPSG_RoundOffFlag = false;
            $scope.subjectid = userdd.ismS_Id;
            $scope.group_list_subwise = [];

            $scope.EMP_BestOfApplicableFlg = "";
            $scope.EMPSG_MarksValue = "";
            $scope.EMPSG_PercentValue = "";
            $scope.EMPSG_GroupName = "";
            $scope.EMPSG_Order = "";
            $scope.EYCE_BestOfsdasd = "";
            $scope.EMPSG_MaxOff = "";
            $scope.EMPSG_BestOff = "";
            $scope.EMPSG_DisplayName = "";

            if (userdd.EMPS_MaxMarks !== undefined && userdd.EMPS_MaxMarks !== null && userdd.EMPS_MaxMarks !== ""
                && userdd.EMPS_MinMarks !== undefined && userdd.EMPS_MinMarks !== null && userdd.EMPS_MinMarks !== ""
                && userdd.EMPS_ConvertForMarks !== undefined && userdd.EMPS_ConvertForMarks !== null && userdd.EMPS_ConvertForMarks !== "") {

                if ($scope.EMP_MarksPerFlg === "M") {

                    var data = {
                        "ISMS_Id": userdd.ismS_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "EYC_Id": $scope.EYC_Id
                    }

                    apiService.create("PromotionSetting/GetSubjectExamMaks", data).then(function (promise) {

                        if (promise !== null) {
                            $scope.GetYearlyExamSubjectMarks = promise.getYearlyExamSubjectMarks;

                            angular.forEach($scope.exam_list, function (d) {
                                d.checked = false;
                                d.EMPSGE_ForMaxMarkrs = "";
                                d.ForMaxMarkrs = "";
                                d.EMPSGE_ConvertionReqOrNot = false;

                                angular.forEach($scope.GetYearlyExamSubjectMarks, function (dd) {
                                    if (dd.emE_Id === d.emE_Id) {
                                        d.ForMaxMarkrs = dd.eyceS_MaxMarks;
                                        d.ForMaxMarkrs_Temp = dd.eyceS_MaxMarks;
                                    }
                                });
                            });
                        }
                    });
                }

                if (userdd.pro_exams_group_list !== undefined && userdd.pro_exams_group_list !== null && userdd.pro_exams_group_list.length > 0) {
                    $scope.group_list_subwise = userdd.pro_exams_group_list;
                    $scope.group_cnt = userdd.pro_exams_group_list.length;
                    $scope.all1 = true;
                    $scope.toggleAll1();
                }

                $('#map_groups').modal('show');
            } else {
                swal("Enter Max , Min And Converted Marks");
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.exam_list.some(function (options) {
                return options.checked;
            });
        };

        $scope.AddToCart = function () {
            if ($scope.EMP_MarksPerFlg === "P") {
                var totalpercentage = 0;

                angular.forEach($scope.group_list_subwise, function (dd) {
                    totalpercentage += Number(dd.EMPSG_PercentValue);
                });

                if (totalpercentage === 100) {
                    angular.forEach($scope.subject_list, function (dd) {
                        if (dd.ismS_Id === $scope.subjectid) {
                            dd.pro_exams_group_list = $scope.group_list_subwise;
                        }
                        $('#map_groups').modal('hide');
                    });
                } else {
                    swal("Group Percentage Should Be Equal To 100");
                }
            } else {
                angular.forEach($scope.subject_list, function (dd) {
                    if (dd.ismS_Id === $scope.subjectid) {
                        dd.pro_exams_group_list = $scope.group_list_subwise;
                        console.log("add&close");
                        console.log(dd);
                    }
                    $('#map_groups').modal('hide');
                });
            }
        };

        $scope.SetSubjectOrder = function (dd) {
            $scope.subject_list_temp = [];
            $scope.temp_emp_id = dd.emP_Id;
            var data = {
                "EMP_Id": dd.emP_Id
            };

            apiService.create("PromotionSetting/SetSubjectOrder", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subject_list_temp = promise.view_prom_subjects;
                }
            });
        };

        $scope.SaveSubjectOrder = function (dd) {
            var data = {
                "EMP_Id": $scope.temp_emp_id,
                "subject_list_temp": $scope.subject_list_temp
            };

            apiService.create("PromotionSetting/SaveSubjectOrder", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Order Updated Successfully");
                    } else {
                        swal("Failed To Update Record");
                    }
                    $scope.subject_list_temp = promise.view_prom_subjects;
                }
            });
        };


        $scope.EditGroupMapping = function (dd, indexedit) {

            $scope.editgroupflag = 1;
            $scope.index_editgroupmapping = indexedit;

            $scope.EMPSG_MarksValue = dd.EMPSG_MarksValue;
            $scope.EMPSG_PercentValue = dd.EMPSG_PercentValue;
            $scope.EMPSG_GroupName = dd.EMPSG_GroupName;
            $scope.EMPSG_Order = dd.EMPSG_Order;
            $scope.EMPSG_MaxOff = dd.EMPSG_MaxOff;
            $scope.EMPSG_BestOff = dd.EMPSG_BestOff;
            $scope.EMPSG_DisplayName = dd.EMPSG_DisplayName;
            $scope.EMPSG_RoundOffFlag = dd.EMPSG_RoundOffFlag === null ? false : dd.EMPSG_RoundOffFlag;

            angular.forEach($scope.exam_list, function (d) {
                d.checked = false;
                angular.forEach(dd.Exm_M_Prom_Subj_Group_Exams_master, function (ddd) {
                    if (d.emE_Id === ddd.emE_Id) {
                        d.checked = true;
                        d.ForMaxMarkrs = ddd.EMPSGE_ForMaxMarkrs;
                        d.ForMaxMarkrs_Temp = ddd.EMPSGE_ForMaxMarkrs;
                        d.ConvertionReqOrNot = ddd.EMPSGE_ConvertionReqOrNot === null ? false : ddd.EMPSGE_ConvertionReqOrNot;
                    }
                });
            });
        };

        $scope.onchangemaxmarks = function (maxmarks_temp) {
            angular.forEach($scope.subject_list, function (dd) {
                dd.EMPS_MaxMarks = maxmarks_temp;
            });
        };

        $scope.onchangeminmarks = function (minmarks_temp) {

            if (Number(minmarks_temp) < Number($scope.maxmarks)) {
                angular.forEach($scope.subject_list, function (dd) {
                    dd.EMPS_MinMarks = minmarks_temp;
                    //$scope.valid_min(dd.EMPS_MinMarks, dd);
                });
            } else {
                swal("Minimum Marks Should Be Less Than Maximum Marks");
                angular.forEach($scope.subject_list, function (dd) {
                    dd.EMPS_MinMarks = "";
                });
            }
        };

        $scope.onchangemaxentrymarks = function (maxentrymarks_temp) {
            angular.forEach($scope.subject_list, function (dd) {
                dd.EMPS_ConvertForMarks = maxentrymarks_temp;
                $scope.valid_max_convert(dd.EMPS_ConvertForMarks, dd);
            });
        };


    }
})();