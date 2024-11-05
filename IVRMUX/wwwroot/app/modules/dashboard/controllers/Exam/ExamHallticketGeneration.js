
(function () {
    'use strict';
    angular
.module('app')
.controller('ExamHallticketGeneration', ExamHallticketGeneration)

    ExamHallticketGeneration.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter']
    function ExamHallticketGeneration($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter) {

        //$scope.SubWise_Selected_subexms_list = [];
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //TO  GEt The Values iN Grid
        $scope.select_cat = false;
        $scope.BindData = function () {
            apiService.getDATA("ExamSubjectMapping/Getdetails").
       then(function (promise) {
           

           $scope.year_list = promise.yearlist;
           $scope.category_list = promise.categorylist;
           $scope.tempcategorylist = promise.categorylist;
           $scope.grade_list = promise.gradelist;
           $scope.tempexamlist = promise.examlist;
           $scope.exam_list = promise.examlist;
           angular.forEach(promise.subexamlist, function (opt) {
               opt.EYCESSE_SubExamOrder = opt.emsE_SubExamOrder;
           })
           $scope.subexam_list = promise.subexamlist;
           $scope.subject_list = promise.subjectlist;

           angular.forEach(promise.subsubjectlist, function (opt) {
               opt.EYCESSS_SubSubjectOrder = opt.emsS_Order;
           })
           $scope.subsubject_list = promise.subsubjectlist;

           $scope.tempsubsubjectlist = promise.subsubjectlist;
           $scope.tempsubexamlist = promise.subexamlist;

           //angular.forEach($scope.tempsubexamlist, function (opt1) {
           //    opt1.EYCESSE_MaxMarks = "";
           //    opt1.EYCESSE_MinMarks = "";
           //    opt1.EMGR_Id = "";
           //    opt1.EYCESSE_ExemptedFlg = false;
           //    opt1.EYCESSE_ExemptedPer="";
           //})
           angular.forEach($scope.subject_list, function (opt) {
               opt.ISMS_Id = opt.ismS_Id;
               opt.EYCES_MarksDisplayFlg = true;
               opt.EYCES_MarksGradeEntryFlg = 'M';
               // opt.EYCES_GradeDisplayFlg = false;
               opt.EYCES_AplResultFlg = true;
               //  opt.EYCES_MaxMarks = opt.ismS_Max_Marks;
               //  opt.EYCES_MinMarks = opt.ismS_Min_Marks;
               //  opt.EYCES_MarksEntryMax = opt.ismS_Max_Marks;
               opt.EYCES_SubjectOrder = opt.ismS_OrderFlag;
           })
           $scope.tempsubjectlist = promise.subjectlist;

           $scope.ASMAY_Id = promise.asmaY_Id;
           $scope.temp_year_id = promise.asmaY_Id;
           angular.forEach($scope.year_list, function (opq) {
               if (opq.asmaY_Id == $scope.ASMAY_Id) {
                   opq.Selected = true;
               }
           })
           $scope.get_category($scope.ASMAY_Id);
           $scope.all = true;
           $scope.toggleAll();
           //angular.forEach(promise.category_exams, function (itm) {
           //    itm.eycE_AttendanceFromDate = $filter('date')(new Date(itm.eycE_AttendanceFromDate), 'dd-MM-yyyy');
           //    itm.eycE_AttendanceToDate = $filter('date')(new Date(itm.eycE_AttendanceToDate), 'dd-MM-yyyy');
           //    //itm.eycE_AttendanceFromDate = $filter("date")(new Date(itm.eycE_AttendanceFromDate), 'yyyy-MM-dd');
           //    //itm.eycE_AttendanceToDate = $filter("date")(new Date(itm.eycE_AttendanceToDate), 'yyyy-MM-dd');
           //})


           $scope.gridOptions.data = promise.category_exams;

       })
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
                            //   itm.EMGR_Id = $scope.EMGR_Id;
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

            //  alert('#popup');
            if (sel_subsubjs == true) {
                $scope.subject_check = true;
                if (count == 0) {
                    //$scope.subexam_list = $scope.temp_subexam_list;
                    //   $scope.subexam_list = $scope.tempsubexamlist;
                    $scope.clear1();

                }
                $('#popup3').modal('show');

                // $scope.all1 = true;
                // $scope.toggleAll1();
            } else if (sel_subsubjs == false) {
                $scope.subject_check = false;
                swal({
                    title: "Are you sure",
                    text: "Do you want to Delete SubSubjects Mapped to " + $scope.Subject_Name + " Subject???",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete Selection!",
                    // cancelButtonColor: "#24d46a",
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
                       // swal("Deletion  Cancelled");
                       swal("Now You Can Change Selection");
                       angular.forEach($scope.subject_list, function (itm) {
                           if (itm.ismS_Id == user.ismS_Id) {
                               
                               itm.EYCES_SubSubjectFlg = true;

                               // $scope.select_subsubjs(itm.EYCES_SubSubjectFlg, user);
                               $('#popup3').modal('show');
                           }
                       })
                   }
               });
                // swal("SubSubjects  Mapping  To ''"+ $scope.Subject_Name+"'' Subject Are Deleted");
            }
        }

        //$scope.valid_max = function (max,user) {
        //    
        //    var num_max = Number(max);
        //    if (num_max > user.ismS_Max_Marks) {
        //        swal("Max.Marks  Value Not More Than " + user.ismS_Max_Marks);
        //        angular.forEach($scope.subject_list, function (itm1) {
        //            if (itm1.ismS_Id == user.ismS_Id) {
        //                //  itm1.EYCES_MaxMarks = user.ismS_Max_Marks;
        //                itm1.EYCES_MaxMarks = "";
        //            }
        //        })
        //        //$scope.EMGD_From = "";
        //    }
        //    else if (num_max < user.ismS_Min_Marks) {
        //        swal("Max.Marks  Value Not Less Than  " + user.ismS_Min_Marks);
        //        angular.forEach($scope.subject_list, function (itm1) {
        //            if (itm1.ismS_Id == user.ismS_Id) {
        //                //itm1.EYCES_MaxMarks = user.ismS_Max_Marks;
        //                itm1.EYCES_MaxMarks = "";
        //            }
        //        })
        //        //$scope.EMGD_From = "";
        //    }
        //    user.EYCES_MarksEntryMax = user.EYCES_MaxMarks;
        //}

        $scope.valid_max = function (max, user) {
            
            if (user.EYCES_MaxMarks != null && user.EYCES_MaxMarks != undefined && user.EYCES_MaxMarks != "") {
                var num_max = Number(max);
                if (num_max < 1) {
                    swal("Max. Marks  Value Not Less Than 1");
                    angular.forEach($scope.subject_list, function (itm1) {
                        if (itm1.ismS_Id == user.ismS_Id) {
                            //  itm1.EYCES_MaxMarks = user.ismS_Max_Marks;
                            itm1.EYCES_MaxMarks = "";
                        }
                    })
                }
                if (num_max > 1000) {
                    swal("Max Value Is 1000");

                    angular.forEach($scope.subject_list, function (itm1) {
                        if (itm1.ismS_Id == user.ismS_Id) {
                            //  itm1.EYCES_MaxMarks = user.ismS_Max_Marks;
                            itm1.EYCES_MaxMarks = "";
                        }
                    })
                }
            }
        }




        //$scope.valid_min = function (min,user) {
        //    
        //    if (user.EYCES_MaxMarks != null && user.EYCES_MaxMarks != undefined && user.EYCES_MaxMarks != "") {
        //        var num_min = Number(min);
        //        if (num_min > user.EYCES_MaxMarks) {
        //            swal("Min.Marks  Value Not More Than " + user.EYCES_MaxMarks);
        //            angular.forEach($scope.subject_list, function (itm1) {
        //                if (itm1.ismS_Id == user.ismS_Id) {
        //                    //  itm1.EYCES_MinMarks = user.ismS_Min_Marks;
        //                    itm1.EYCES_MinMarks = "";
        //                }
        //            })
        //            //$scope.EMGD_From = "";
        //        }
        //        else if (num_min < user.ismS_Min_Marks) {
        //            swal("Min.Marks  Value Not Less Than  " + user.ismS_Min_Marks);
        //            angular.forEach($scope.subject_list, function (itm1) {
        //                if (itm1.ismS_Id == user.ismS_Id) {
        //                    //itm1.EYCES_MinMarks = user.ismS_Min_Marks;
        //                    itm1.EYCES_MinMarks = "";
        //                }
        //            })
        //            //$scope.EMGD_From = "";
        //        }
        //    }
        //    else {
        //        swal("First Set Max.Marks !!!");
        //        user.EYCES_MinMarks = user.ismS_Min_Marks;
        //    }
        //}

        $scope.valid_min = function (min, user) {
            
            if (user.EYCES_MinMarks != null && user.EYCES_MinMarks != undefined && user.EYCES_MinMarks != "") {
                if (user.EYCES_MaxMarks != null && user.EYCES_MaxMarks != undefined && user.EYCES_MaxMarks != "") {
                    var num_min = Number(min);
                    if (num_min == 0) {
                        swal("Min.Marks  Not be Zero");
                        angular.forEach($scope.subject_list, function (itm1) {
                            if (itm1.ismS_Id == user.ismS_Id) {
                                //  itm1.EYCES_MinMarks = user.ismS_Min_Marks;
                                itm1.EYCES_MinMarks = "";
                            }
                        })
                    }
                    if (num_min > user.EYCES_MaxMarks) {
                        swal("Min.Marks  Value Not More Than Max.Marks " + user.EYCES_MaxMarks);
                        angular.forEach($scope.subject_list, function (itm1) {
                            if (itm1.ismS_Id == user.ismS_Id) {
                                //  itm1.EYCES_MinMarks = user.ismS_Min_Marks;
                                itm1.EYCES_MinMarks = "";
                            }
                        })
                        //$scope.EMGD_From = "";
                    }

                }
                else {
                    swal("First Set Max.Marks !!!");
                    //user.EYCES_MinMarks = user.ismS_Min_Marks;
                    user.EYCES_MinMarks = "";
                }
            }
        }

        $scope.valid_max_entry = function (max_entry, user) {
            
            if (user.EYCES_MarksEntryMax != null && user.EYCES_MarksEntryMax != undefined && user.EYCES_MarksEntryMax != "") {
                if (user.EYCES_MaxMarks != null && user.EYCES_MaxMarks != undefined && user.EYCES_MaxMarks != "") {
                    // if (user.EYCES_MinMarks != null && user.EYCES_MinMarks != undefined && user.EYCES_MinMarks != "") {
                    var num_max_entry = Number(max_entry);
                    if (num_max_entry > user.EYCES_MaxMarks) {
                        swal("MaxEntry.Marks  Value Not More Than Max. Marks" + user.EYCES_MaxMarks);
                        angular.forEach($scope.subject_list, function (itm1) {
                            if (itm1.ismS_Id == user.ismS_Id) {
                                //itm1.EYCES_MarksEntryMax = user.ismS_Max_Marks;
                                itm1.EYCES_MarksEntryMax = "";
                            }
                        })
                        //$scope.EMGD_From = "";
                    }
                    if (num_max_entry < 1) {
                        swal("MaxEntry.Marks  Value Not Less Than 1");
                        angular.forEach($scope.subject_list, function (itm1) {
                            if (itm1.ismS_Id == user.ismS_Id) {
                                //itm1.EYCES_MarksEntryMax = user.ismS_Max_Marks;
                                itm1.EYCES_MarksEntryMax = "";
                            }
                        })
                        //$scope.EMGD_From = "";
                    }
                    //else if (num_max_entry < user.EYCES_MinMarks) {
                    //    swal("MaxEntry.Marks  Value Not Less Than " + user.EYCES_MinMarks);
                    //    angular.forEach($scope.subject_list, function (itm1) {
                    //        if (itm1.ismS_Id == user.ismS_Id) {
                    //            // itm1.EYCES_MarksEntryMax = user.ismS_Min_Marks;
                    //            itm1.EYCES_MarksEntryMax = "";
                    //        }
                    //    })
                    //    //$scope.EMGD_From = "";
                    //}
                    //  }
                    //  else
                    //  {
                    //       swal("First Set Min.Marks");
                    //  user.EYCES_MarksEntryMax = user.EYCES_MaxMarks;
                    //     user.EYCES_MarksEntryMax = "";
                    //  }
                }
                else {
                    swal("First Set Max.Marks");
                    //user.EYCES_MarksEntryMax = user.EYCES_MaxMarks;
                    user.EYCES_MarksEntryMax = "";
                }
            }
        }

        $scope.valid_max1 = function (subsubj_max, user) {
            
            //var num_subsubj_max = Number(subsubj_max);
            //if (num_subsubj_max > 100) {
            //    swal("Max Value Is 100");

            //    user.EYCESSS_MaxMarks = "";
            //}
            //user.EYCESSS_MinMarks = "";
            var num_subsubj_max = Number(subsubj_max);
            if (num_subsubj_max > Number($scope.Subj_Max_Marks)) {
                //swal("Max Value Is " + user.EYCES_MaxMarks);
                swal("Max.Marks Max Value Is Max.Marks Of Subject " + $scope.Subj_Max_Marks);
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
        }
        $scope.valid_exmpt_per1 = function (subsubj_expt_per, user) {
            
            var num_subsubj_expt_per = Number(subsubj_expt_per);
            if (num_subsubj_expt_per > 100 || num_subsubj_expt_per < 0) {
                swal("Max Value Is 100 and Min Value Is 0");

                user.EYCESSS_ExemptedPer = "";
            }

        }

        $scope.valid_max2 = function (subexm_max, user) {
            
            //var num_subexm_max = Number(subexm_max);
            //if (num_subexm_max > 100) {
            //    swal("Max Value Is 100");

            //    user.EYCESSE_MaxMarks = "";
            //}
            //user.EYCESSE_MinMarks = "";
            var num_subexm_max = Number(subexm_max);
            if (num_subexm_max > Number($scope.Subj_Max_Marks)) {
              //  swal("Max Value Is 100");
                swal("Max.Marks Max Value Is Max.Marks Of Subject " + $scope.Subj_Max_Marks);
                user.EYCESSE_MaxMarks = "";
            }
            user.EYCESSE_MinMarks = "";
        }
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

        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subject_list, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        }

        $scope.optionToggled = function () {
            $scope.all = $scope.subject_list.every(function (itm) { return itm.checkedvalue; })
        }

        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.all1;
            angular.forEach($scope.subsubject_list, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        }

        $scope.optionToggled1 = function () {
            $scope.all1 = $scope.subsubject_list.every(function (itm) { return itm.checkedvalue; })
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
        $scope.set_order_subsubj = function () {
            
            //if ($scope.subject_check == true) {
            //    angular.forEach($scope.subject_list, function (itm) {

            //        if (itm.ismS_Id == $scope.ismS_Id) {
            //            
            //            itm.EYCES_SubSubjectFlg = false;

            //        }

            //    })

            //}
            $('#myModal3').modal('hide');
            $('#popup3').modal('show');
        }



        $scope.select_subexms = function (sel_subexms, user) {
            
            //$scope.submitted2 = true;
            $scope.subexam_list = $scope.tempsubexamlist;
            $scope.Subject_Name = user.ismS_SubjectName;
            $scope.ismS_Id = user.ismS_Id;
            $scope.Subj_Max_Marks = user.EYCES_MaxMarks;
            $scope.Subj_Min_Marks = user.EYCES_MinMarks;
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
                                //angular.forEach($scope.grade_list, function (opt123) {
                                //    if (opt123.emgR_Id == itm.EMGR_Id)
                                //    {
                                //        opt123.Selected = true;
                                //    }
                                //})
                            }

                        }
                        if (subexam_count == 0) {
                            var itm = $scope.subexam_list[b];
                            itm.checkedvalue = false;
                            itm.EYCESSE_MaxMarks = "";
                            itm.EYCESSE_MinMarks = "";
                            itm.EMGR_Id = "";
                            //itm.EMGR_Id = $scope.EMGR_Id;
                            itm.EYCESSE_ExemptedFlg = false;
                            itm.EYCESSE_ExemptedPer = "";
                            $scope.optionToggled2();

                            //angular.forEach($scope.grade_list, function (opt123) {
                            //    if (opt123.emgR_Id == itm.EMGR_Id) {
                            //        opt123.Selected = true;
                            //    }
                            //})
                        }
                    }
                }

            }

            if (sel_subexms == true) {
                $scope.exam_check = true;

                //var count = 0;

                //angular.forEach($scope.SubWise_Selected_subexms_list, function (item123) {
                //    if (item123.ismS_Id == user.ismS_Id)
                //    {
                //        count += 1;
                //        angular.forEach($scope.tempsubexamlist, function (itm) {
                //            angular.forEach(item123.sub_exam_list, function (itm1) {
                //                if (itm.emsE_Id == itm1.emsE_Id)
                //                {
                //                   // itm = itm1;  
                //                    itm.checkedvalue = true;
                //                    itm.EYCESSE_MaxMarks = itm1.EYCESSE_MaxMarks;
                //                    itm.EYCESSE_MinMarks = itm1.EYCESSE_MinMarks;
                //                    itm.EMGR_Id = itm1.EMGR_Id;
                //                    itm.EYCESSE_ExemptedFlg = itm1.EYCESSE_ExemptedFlg;
                //                    itm.EYCESSE_ExemptedPer = itm1.EYCESSE_ExemptedPer;
                //                    $scope.optionToggled2();
                //                }
                //            })
                //            //$scope.subexam_list = $scope.tempsubexamlist;
                //        })
                //         $scope.subexam_list = $scope.tempsubexamlist;


                //    }

                //})

                //for (var a = 0; a < $scope.SubWise_Selected_subexms_list.length; a++)
                //{
                //    if ($scope.SubWise_Selected_subexms_list[a].ISMS_Id == user.ismS_Id)
                //    {
                //        count += 1;
                //        for (var b = 0; b < $scope.subexam_list.length; b++) {
                //            var subexam_count = 0;
                //            for (var c = 0; c < $scope.SubWise_Selected_subexms_list[a].sub_exam_list.length; c++) {
                //               if ($scope.subexam_list[b].emsE_Id == $scope.SubWise_Selected_subexms_list[a].sub_exam_list[c].EMSE_Id)
                //               {
                //                   // itm = itm1;  
                //                   var itm = $scope.subexam_list[b];
                //                   var itm1 = $scope.SubWise_Selected_subexms_list[a].sub_exam_list[c];
                //                   subexam_count += 1;
                //                   itm.checkedvalue = true;
                //                   itm.EYCESSE_MaxMarks = itm1.EYCESSE_MaxMarks;
                //                   itm.EYCESSE_MinMarks = itm1.EYCESSE_MinMarks;
                //                   itm.EMGR_Id = itm1.EMGR_Id;
                //                   itm.EYCESSE_ExemptedFlg = itm1.EYCESSE_ExemptedFlg;
                //                   itm.EYCESSE_ExemptedPer = itm1.EYCESSE_ExemptedPer;
                //                   $scope.optionToggled2();
                //                }

                //            }
                //            if(subexam_count==0)
                //            {
                //                var itm = $scope.subexam_list[b];
                //                itm.checkedvalue = false;
                //                itm.EYCESSE_MaxMarks = "";
                //                itm.EYCESSE_MinMarks = "";
                //                itm.EMGR_Id = "";
                //                itm.EYCESSE_ExemptedFlg = false;
                //                itm.EYCESSE_ExemptedPer = "";
                //                $scope.optionToggled2();
                //            }
                //        }
                //    }

                //}



                if (count == 0) {
                    //$scope.subexam_list = $scope.temp_subexam_list;
                    //   $scope.subexam_list = $scope.tempsubexamlist;
                    $scope.clear2();

                }

                $('#popup5').modal('show');
                //$scope.all2 = true;
                //$scope.toggleAll2();
            } else if (sel_subexms == false) {
                $scope.exam_check = false;
                // $('#popup3').modal('cancel');
                // swal("SubExams Are Not Mapped To This Subject");

                // var mgs = "";
                // var confirmmgs = "";
                //if (deactiveRecord.emE_ActiveFlag == true) {
                //    mgs = "Deactive";
                //    confirmmgs = "Deactivated";

                //}
                //else {
                //    mgs = "Active";
                //    confirmmgs = "Activated";
                //}
                swal({
                    title: "Are you sure",
                    text: "Do you want to Delete SubExams Mapped to " + $scope.Subject_Name + " Subject???",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete Selection!",
                    // cancelButtonColor: "#24d46a",
                    cancelButtonText: "No, Change Selection!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
               function (isConfirm) {
                   if (isConfirm) {
                       // swal('Deleted Successfully');

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
                               //$scope.select_subexms(itm.EYCES_SubExamFlg, user);
                               $('#popup5').modal('show');
                           }
                       })
                   }
               });

                // swal("SubExams  Mapping  To ''" + $scope.Subject_Name + "'' Subject Are Deleted");
            }
        }


        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                   { name: 'SLNO', displayName: 'SL NO', width: '6%', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'asmaY_Year', width: '11%', displayName: 'Academic Year' },
              { name: 'emcA_CategoryName', displayName: 'Category Name' },
              { name: 'emE_ExamName', displayName: 'Exam Name' },
               { name: 'emgR_GradeName', width: '9%', displayName: 'Grade Name' },
              { name: 'eycE_AttendanceFromDate', width: '12%', displayName: 'Attendance From', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },//, cellFilter: 'date:\'MMMM\'' ,cellFilter: 'date:\'yyyy-MM-dd\'' 
              { name: 'eycE_AttendanceToDate', width: '10%', displayName: 'Attendance To', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },//, cellFilter: 'date:\'MMMM\'',//hema , cellFilter: 'date:"dd-MM-yyyy"'
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
        //$scope.gridOptions.onRegisterApi = function (gridApi) {
        //    $scope.gridApi = gridApi;
        //};
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
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("ExamSubjectMapping/get_category", data).
                then(function (promise) {
                    $scope.category_list = promise.categorylist;
                    $scope.EYC_Id = "";
                    if ($scope.EYCE_Id != "" && $scope.EYCE_Id != 0 && $scope.EYCE_Id != undefined) {
                        angular.forEach($scope.category_list, function (role) {
                            
                            if (role.eyC_Id == $scope.temp_category) {
                                $scope.EYC_Id = role.eyC_Id;
                                role.Selected = true;
                            }
                        })
                    }

                    if (promise.categorylist == "" || promise.categorylist == null) {
                        swal("No Categories Are Mapped To Selected Academic Year");
                    }
                })

            //  $scope.minDatemf = new Date();

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

        }

        $scope.valid_from_date = function (from_date) {
            if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != null && $scope.ASMAY_Id != undefined) {
                $scope.EYCE_AttendanceToDate = "";
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.EYCE_AttendanceFromDate = "";
            }
        }

        $scope.valid_to_date = function (to_date) {
            if ($scope.EYCE_AttendanceFromDate != "" && $scope.EYCE_AttendanceFromDate != null && $scope.EYCE_AttendanceFromDate != undefined) {
                // $scope.EYCE_AttendanceToDate = "";
            }
            else {
                swal("First Select Attendance From Date !!!");
                $scope.EYCE_AttendanceToDate = "";
            }
        }

        $scope.get_subjects = function (cat_id) {

            if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != null && $scope.ASMAY_Id != undefined) {

                var data = {
                    // "ASMAY_Id": $scope.ASMAY_Id,
                    "EYC_Id": $scope.EYC_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ExamSubjectMapping/get_subjects", data).
                    then(function (promise) {
                        $scope.subject_list = promise.subjectlist;
                        $scope.select_cat = true;
                        $scope.exam_list = promise.examlist;

                        //for (var a = 0; a < $scope.exam_list.length; a++) {
                        //    var exm_alrd_cnt = 0;
                        //    angular.forEach(promise.examlist, function (itm) {
                        //        if ($scope.exam_list[a].emE_Id == itm.emE_Id) {
                        //            exm_alrd_cnt += 1;
                        //            //$scope.SubWise_Selected_subexms_list.splice(i, 1);
                        //        }
                        //    })
                        //    if(exm_alrd_cnt==0)
                        //    {
                        //        $scope.exam_list.splice(a, 1);
                        //    }

                        //}
                        angular.forEach($scope.subject_list, function (opt) {
                            opt.ISMS_Id = opt.ismS_Id;
                            opt.EYCES_MarksGradeEntryFlg = 'M';
                            opt.EYCES_MarksDisplayFlg = true;
                            // opt.EYCES_GradeDisplayFlg = false;
                            opt.EYCES_AplResultFlg = true;
                            // opt.EYCES_MaxMarks = opt.ismS_Max_Marks;
                            //   opt.EYCES_MinMarks = opt.ismS_Min_Marks;
                            //  opt.EYCES_MarksEntryMax = opt.ismS_Max_Marks;

                            opt.EYCES_SubjectOrder = opt.ismS_OrderFlag;
                            opt.EMGR_Id = $scope.EMGR_Id;

                            for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) {
                                //var already_count = 0;
                                if (opt.ismS_Id == $scope.SubWise_Selected_subexms_list[i].ISMS_Id) {
                                    //already_count += 1;
                                    //// $scope.SubWise_Selected_subexms_list.splice(i, 1);
                                    //if (already_count > 0) {
                                    //    $scope.SubWise_Selected_subexms_list.splice(i, 1);
                                    //}
                                    opt.EYCES_SubExamFlg = true;
                                }
                            }
                            for (var i = 0; i < $scope.SubWise_Selected_subsubjs_list.length; i++) {

                                if (opt.ismS_Id == $scope.SubWise_Selected_subsubjs_list[i].ISMS_Id) {

                                    opt.EYCES_SubSubjectFlg = true;
                                }
                            }
                        })
                        if ($scope.EYCE_Id > 0) {
                            angular.forEach(temp_saved_subs, function (sy) {
                                angular.forEach($scope.subject_list, function (sy1) {
                                    if (sy.ismS_Id == sy1.ismS_Id) {
                                        sy1.ismS_OrderFlag = sy.eyceS_SubjectOrder;
                                    }
                                })
                            })

                        }
                        $scope.all = true;
                        $scope.toggleAll();

                        if ($scope.EYCE_Id != "" && $scope.EYCE_Id != 0 && $scope.EYCE_Id != undefined) {
                            if ($scope.EYC_Id == $scope.temp_category) {
                                angular.forEach($scope.tempexamlist, function (role) {
                                    

                                    if (role.emE_Id == $scope.temp_exm) {
                                        $scope.exam_list.push(role);

                                    }
                                })
                                angular.forEach($scope.exam_list, function (role) {
                                    
                                    if (role.emE_Id == $scope.temp_exm) {
                                        role.checked = true;

                                    }
                                })
                                // }

                                angular.forEach($scope.subject_list, function (role) {
                                    
                                    // $scope.all = false;
                                    var exm_subject_cnt = 0;
                                    angular.forEach($scope.selected_exm_subjects, function (itm) {
                                        if (role.ismS_Id == itm.ismS_Id) {
                                            role.checkedvalue = true;
                                            // role.ismS_Id = itm.ismS_Id;
                                            role.EYCES_MaxMarks = itm.eyceS_MaxMarks;
                                            role.EYCES_MinMarks = itm.eyceS_MinMarks;
                                            role.EYCES_MarksEntryMax = itm.eyceS_MarksEntryMax;
                                            role.EYCES_SubExamFlg = itm.eyceS_SubExamFlg;
                                            role.EYCES_SubSubjectFlg = itm.eyceS_SubSubjectFlg;
                                            // role.EYCES_MarksGradeEntryFlg = itm.eyceS_MarksGradeEntryFlg.replace(/ /g, '');
                                            role.EYCES_MarksGradeEntryFlg = itm.eyceS_MarksGradeEntryFlg;
                                            role.EYCES_MarksDisplayFlg = itm.eyceS_MarksDisplayFlg;
                                            role.EYCES_GradeDisplayFlg = itm.eyceS_GradeDisplayFlg;
                                            role.EYCES_AplResultFlg = itm.eyceS_AplResultFlg;
                                            role.EMGR_Id = itm.emgR_Id;
                                            exm_subject_cnt += 1;

                                        }
                                    })
                                    if (exm_subject_cnt == 0) {
                                        role.checkedvalue = false;
                                        $scope.optionToggled();
                                    }
                                    //if (exm_subject_cnt > 0) {
                                    //    role.checkedvalue = true;
                                    //}

                                })
                            }
                        }

                        if (promise.subjectlist == null || promise.subjectlist == "") {
                            swal("Subjects are Not Mapped To Selected Category!!!");
                            $scope.EYC_Id = "";
                            $scope.select_cat = false;
                        }
                        if (promise.examlist == null || promise.examlist == "") {
                            swal("All Exams are  Mapped To Selected Category !!!");
                            $scope.EYC_Id = "";
                        }

                    })
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.EYC_Id = "";
            }

        }

        $scope.get_gradename = function (grd) {
            
            angular.forEach($scope.subject_list, function (itm) {
                itm.EMGR_Id = grd;
            })
            //angular.forEach($scope.subsubject_list, function (itm2) {
            //    itm2.EMGR_Id = grd;
            //})
            //angular.forEach($scope.subexam_list, function (itm3) {
            //    itm3.EMGR_Id = grd;
            //})

            angular.forEach($scope.grade_list, function (itm1) {
                if (itm1.emgR_Id == grd) {
                    $scope.GroupName = itm1.emgR_GradeName;
                }
            })
        }



        $scope.isOptionsRequired = function () {
            return !$scope.exam_list.some(function (options) {
                return options.checked;
            });
        }


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
                            // swal(confirmmgs + " " + " successfully");
                            swal("Record " + mgs + " Failed");
                        }
                    }
                    //if (promise.returnval == true) {
                    //    swal(confirmmgs + " " + "successfully");
                    //}
                    //else {
                    //    swal(confirmmgs + " " + " successfully");
                    //}
                    $scope.clear();
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        $scope.deactive_sub = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
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
                            // swal(confirmmgs + " " + " successfully");
                            swal("Record " + mgs + " Failed");
                        }
                    }
                    //if (promise.returnval == true) {
                    //    swal(confirmmgs + " " + "successfully");
                    //}
                    //else {
                    //    swal(confirmmgs + " " + " successfully");
                    //}
                    //$scope.BindData();
                    $scope.clear();
                    $scope.viewrecordspopup(employee);

                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        $scope.deactive_sub_exm = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.eycessE_ActiveFlg === true) {
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

                apiService.create("ExamSubjectMapping/deactive_sub_exm", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record " + confirmmgs + " " + "successfully");
                    }
                    else {
                        // swal(confirmmgs + " " + " successfully");
                        swal("Record " + mgs + " Failed");
                    }
                    //if (promise.returnval == true) {
                    //    swal(confirmmgs + " " + "successfully");
                    //}
                    //else {
                    //    swal(confirmmgs + " " + " successfully");
                    //}
                    //$scope.BindData();
                    $scope.clear();
                    $scope.viewrecordspopup_subexms(employee);

                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }
        $scope.deactive_sub_subj = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.eycessS_ActiveFlg === true) {
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

                apiService.create("ExamSubjectMapping/deactive_sub_subj", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record " + confirmmgs + " " + "successfully");
                    }
                    else {
                        // swal(confirmmgs + " " + " successfully");
                        swal("Record " + mgs + " Failed");
                    }
                    //if (promise.returnval == true) {
                    //    swal(confirmmgs + " " + "successfully");
                    //}
                    //else {
                    //    swal(confirmmgs + " " + " successfully");
                    //}
                    //$scope.BindData();
                    $scope.clear();
                    $scope.viewrecordspopup_subsubjs(employee);

                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }



        $scope.interacted = function (field) {

            return $scope.submitted;//|| field.$dirty
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted1;//|| field.$dirty
        };
        $scope.interacted2 = function (field) {

            return $scope.submitted2;//|| field.$dirty
        };

        var temp_saved_subs = [];

        // to Edit Data
        $scope.getorgvalue = function (employee) {
            // $scope.clear();
            $scope.editEmployee = employee.eycE_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("ExamSubjectMapping/editdetails", pageid).
            then(function (promise) {
                $scope.EYCE_Id = promise.edit_cat_exm[0].eycE_Id;
                $scope.ASMAY_Id = promise.edit_cat_exm[0].asmaY_Id;
                //  $scope.selected_exm_subjects = promise.edit_cat_exm[0].edit_cat_exm_subs;
                $scope.selected_exm_subjects = promise.edit_cat_exm_subs;
                $scope.EYC_Id = promise.edit_cat_exm[0].eyC_Id;
                $scope.temp_category = promise.edit_cat_exm[0].eyC_Id;
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

                $scope.SubWise_Selected_subexms_list = [];
                $scope.SubWise_Selected_subsubjs_list = [];
                temp_saved_subs = promise.edit_cat_exm_subs;
                for (var z = 0; z < promise.edit_cat_exm_subs.length; z++) {

                    var EYCES_Id = promise.edit_cat_exm_subs[z].eyceS_Id;
                    var ISMS_Id = promise.edit_cat_exm_subs[z].ismS_Id;

                    var Selected_subexms_list = [];
                    angular.forEach(promise.edit_cat_exm_subs_sub_exms, function (itm) {
                        var newCol = "";
                        if (EYCES_Id == itm.eyceS_Id) {
                            
                            //$scope.Selected_subsubjs_list.push({ ismS_Id: $scope.ismS_Id, sub_exam_list: itm });
                            newCol = { EMSE_Id: itm.emsE_Id, EMGR_Id: itm.emgR_Id, EYCESSE_MaxMarks: itm.eycessE_MaxMarks, EYCESSE_MinMarks: itm.eycessE_MinMarks, EYCESSE_ExemptedFlg: itm.eycessE_ExemptedFlg, EYCESSE_ExemptedPer: itm.eycessE_ExemptedPer, EYCESSE_SubExamOrder: itm.eycessE_SubExamOrder }
                            Selected_subexms_list.push(newCol);
                        }
                    })

                    for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) {
                        var already_count = 0;
                        if (ISMS_Id == $scope.SubWise_Selected_subexms_list[i].ISMS_Id) {
                            already_count += 1;
                            // $scope.SubWise_Selected_subexms_list.splice(i, 1);
                            if (already_count > 0) {
                                $scope.SubWise_Selected_subexms_list.splice(i, 1);
                            }
                        }
                    }

                    $scope.SubWise_Selected_subexms_list.push({ ISMS_Id: ISMS_Id, sub_exam_list: Selected_subexms_list });


                    var Selected_subsubjs_list = [];
                    angular.forEach(promise.edit_cat_exm_subs_sub_subjs, function (itm) {
                        var newCol = "";
                        if (EYCES_Id == itm.eyceS_Id) {
                            
                            //$scope.Selected_subsubjs_list.push({ ismS_Id: $scope.ismS_Id, sub_exam_list: itm });
                            newCol = { EMSS_Id: itm.emsS_Id, EMGR_Id: itm.emgR_Id, EYCESSS_MaxMarks: itm.eycessS_MaxMarks, EYCESSS_MinMarks: itm.eycessS_MinMarks, EYCESSS_ExemptedFlg: itm.eycessS_ExemptedFlg, EYCESSS_ExemptedPer: itm.eycessS_ExemptedPer, EYCESSS_SubSubjectOrder: itm.eycessS_SubSubjectOrder }
                            Selected_subsubjs_list.push(newCol);
                        }
                    })

                    for (var i = 0; i < $scope.SubWise_Selected_subsubjs_list.length; i++) {
                        var already_count = 0;
                        if (ISMS_Id == $scope.SubWise_Selected_subsubjs_list[i].ISMS_Id) {
                            already_count += 1;
                            // $scope.SubWise_Selected_subsubjs_list.splice(i, 1);
                            if (already_count > 0) {
                                $scope.SubWise_Selected_subsubjs_list.splice(i, 1);
                            }
                        }
                    }

                    $scope.SubWise_Selected_subsubjs_list.push({ ISMS_Id: ISMS_Id, sub_subjs_list: Selected_subsubjs_list });

                }


            })
        }


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

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
                })
                if (final_count == 0) {
                    angular.forEach($scope.subject_list, function (opt786) {
                        if ($scope.ismS_Id == opt786.ismS_Id) {
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

                            opt786.EYCES_SubSubjectFlg = false;
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
                            
                            Subj_subsubjects_max_total += Number(itm.EYCESSS_MaxMarks);
                            Subj_subsubjects_min_total += Number(itm.EYCESSS_MinMarks);
                        }
                    })
                    if (Subj_subsubjects_max_total == $scope.Subj_Max_Marks && Subj_subsubjects_min_total == $scope.Subj_Min_Marks) {
                        var Selected_subsubjs_list = [];
                        angular.forEach(subjs_subs, function (itm) {
                            var newCol = "";
                            if (itm.checkedvalue) {
                                
                                //$scope.Selected_subsubjs_list.push({ ismS_Id: $scope.ismS_Id, sub_exam_list: itm });
                                newCol = { EMSS_Id: itm.emsS_Id, EMGR_Id: itm.EMGR_Id, EYCESSS_MaxMarks: itm.EYCESSS_MaxMarks, EYCESSS_MinMarks: itm.EYCESSS_MinMarks, EYCESSS_ExemptedFlg: itm.EYCESSS_ExemptedFlg, EYCESSS_ExemptedPer: itm.EYCESSS_ExemptedPer, EYCESSS_SubSubjectOrder: itm.EYCESSS_SubSubjectOrder }
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
                                
                                itm.EYCESSS_MaxMarks = "";
                                itm.EYCESSS_MinMarks = "";
                            }
                        })
                    }


                }
                //  $scope.subject_list = $scope.tempsubexamlist;
                //$scope.clear2();
                //$('#popup3').modal('hide');
                //$scope.subsubject_list = [];
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
            
            //angular.forEach(exms_subs, function (itobj) {
            //    itobj.EMGR_Id = parseInt(itobj.EMGR_Id);
            //})
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
                    $('#popup5').modal('hide');
                    $scope.subexam_list = [];
                }
                else if (final_count > 0) {
                    var Subj_subexams_max_total = 0;
                    var Subj_subexams_min_total = 0;
                    angular.forEach(exms_subs, function (itm) {
                        if (itm.checkedvalue) {
                            
                            Subj_subexams_max_total += Number(itm.EYCESSE_MaxMarks);
                            Subj_subexams_min_total += Number(itm.EYCESSE_MinMarks);
                        }
                    })
                    if (Subj_subexams_max_total == $scope.Subj_Max_Marks && Subj_subexams_min_total == $scope.Subj_Min_Marks) {

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
                        $('#popup5').modal('hide');
                        $scope.subexam_list = [];
                    }
                    else {
                        swal("Total Of Selected Sub-Exams Max And Min Marks Must Equal To Subject Max And Min Marks");
                        angular.forEach(exms_subs, function (itm) {
                            if (itm.checkedvalue) {
                                
                                itm.EYCESSE_MaxMarks = "";
                                itm.EYCESSE_MinMarks = "";
                            }
                        })
                    }
                }
                //var Selected_subexms_list = [];
                //angular.forEach(exms_subs, function (itm) {
                //    var newCol = "";
                //    if(itm.checkedvalue)
                //    {
                //        
                //        //$scope.Selected_subsubjs_list.push({ ismS_Id: $scope.ismS_Id, sub_exam_list: itm });
                //        newCol = { EMSE_Id: itm.emsE_Id, EMGR_Id: itm.EMGR_Id, EYCESSE_MaxMarks: itm.EYCESSE_MaxMarks, EYCESSE_MinMarks: itm.EYCESSE_MinMarks, EYCESSE_ExemptedFlg: itm.EYCESSE_ExemptedFlg, EYCESSE_ExemptedPer: itm.EYCESSE_ExemptedPer, EYCESSE_SubExamOrder: itm.EYCESSE_SubExamOrder }
                //       Selected_subexms_list.push(newCol);
                //    }
                //})

                //for (var i = 0; i < $scope.SubWise_Selected_subexms_list.length; i++) 
                //{
                //    var already_count = 0;
                //    if ($scope.ismS_Id == $scope.SubWise_Selected_subexms_list[i].ISMS_Id) {
                //        already_count += 1;
                //        // $scope.SubWise_Selected_subexms_list.splice(i, 1);
                //        if (already_count > 0) {
                //            $scope.SubWise_Selected_subexms_list.splice(i, 1);
                //        }
                //    }
                //}

                //$scope.SubWise_Selected_subexms_list.push({ ISMS_Id: $scope.ismS_Id, sub_exam_list: Selected_subexms_list });
                //  $scope.subject_list = $scope.tempsubexamlist;
                //$scope.clear2();

                //$('#popup5').modal('hide');
                //$scope.subexam_list = [];
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
        $scope.getOrder = function (orderarray) {
            
            angular.forEach(orderarray, function (value, key) {
                if (value.ismS_Id !== 0) {
                    orderarray[key].ismS_OrderFlag = key + 1;
                    orderarray[key].EYCES_SubjectOrder = key + 1;
                    // opt.EYCES_SubjectOrder = opt.ismS_OrderFlag;
                }
            });

        }
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
            $scope.EYCE_AttendanceFromDate = new Date($scope.EYCE_AttendanceFromDate).toDateString();
            $scope.EYCE_AttendanceToDate = new Date($scope.EYCE_AttendanceToDate).toDateString();
            $scope.exam_list_saved = [];
            $scope.subject_list_saved = [];

            angular.forEach($scope.exam_list, function (opt123) {
                if (opt123.checked) {
                    $scope.exam_list_saved.push(opt123);
                }
            })
            angular.forEach($scope.subject_list, function (opt123) {
                if (opt123.checkedvalue) {
                    $scope.subject_list_saved.push(opt123);
                }
            })
            if ($scope.myForm.$valid) {
                var data = {
                    "EYCE_Id": $scope.EYCE_Id,
                    "EYCES_Id": $scope.EYCES_Id,
                    "EYCESSS_Id": $scope.EYCESSS_Id,
                    "EYCESSE_Id": $scope.EYCESSE_Id,
                    "EYC_Id": $scope.EYC_Id,
                    //  "EME_Id": $scope.EME_Id,
                    exams_list: $scope.exam_list_saved,
                    "EMGR_Id": $scope.EMGR_Id,
                    "EYCE_AttendanceFromDate": $scope.EYCE_AttendanceFromDate,
                    "EYCE_AttendanceToDate": $scope.EYCE_AttendanceToDate,
                    "EYCE_SubExamFlg": $scope.EYCE_SubExamFlg,
                    "EYCE_SubSubjectFlg": $scope.EYCE_SubSubjectFlg,
                    exm_subjects_list: $scope.subject_list_saved,
                    exm_subject_subexams_list: $scope.SubWise_Selected_subexms_list,
                    exm_subject_subsubjects_list: $scope.SubWise_Selected_subsubjs_list,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ExamSubjectMapping/savedetails", data).
                             then(function (promise) {

                                 //angular.forEach(promise.category_exams, function (itm) {
                                 //    itm.eycE_AttendanceFromDate = $filter("date")(new Date(itm.eycE_AttendanceFromDate), 'dd-MM-yyyy');
                                 //       itm.eycE_AttendanceToDate = $filter("date")(new Date(itm.eycE_AttendanceToDate), 'dd-MM-yyyy');
                                 //    //itm.eycE_AttendanceFromDate = $filter("date")(new Date(itm.eycE_AttendanceFromDate), 'yyyy-MM-dd');
                                 //    //itm.eycE_AttendanceToDate = $filter("date")(new Date(itm.eycE_AttendanceToDate), 'yyyy-MM-dd');
                                 //})


                                 $scope.gridOptions.data = promise.category_exams;

                                 if (promise.returnval === true) {
                                     // swal('Data successfully Saved');
                                     if (promise.eycE_Id == 0 || promise.eycE_Id < 0) {
                                         swal('Record saved successfully');
                                     }
                                         // else if(promise.emcA_Id!="" && promise.emcA_Id>0 && promise.emcA_Id!=undefined)
                                     else if (promise.eycE_Id > 0) {
                                         swal('Record updated successfully');
                                     }

                                 }
                                 else if (promise.returnduplicatestatus === 'Duplicate') {
                                     //swal('Recards AlReady Exist !');
                                     swal('Record already exist');
                                 }
                                 else {
                                     //swal('Data Not Saved !');
                                     if (promise.eycE_Id == 0 || promise.eycE_Id < 0) {
                                         swal('Failed to save, please contact administrator');
                                     }
                                     else if (promise.eycE_Id > 0) {
                                         swal('Failed to update, please contact administrator');
                                     }
                                 }
                                 //if (promise.returnval === true) {
                                 //    $scope.clear();
                                 //    $scope.BindData();
                                 //    swal('Record Saved/Updated Successfully', 'success');
                                 //}
                                 //else if (promise.returnduplicatestatus === true || promise.returnval === false) {
                                 //    swal('Record is Duplicate', 'Failed');
                                 //}
                                 //else {
                                 //    $scope.clear();
                                 //    $scope.BindData();
                                 //    swal('Record Not Saved/Updated Successfully', 'Failed');
                                 //}
                                 $scope.BindData();
                                 $scope.clear();
                             })
                //$scope.BindData();
                //$scope.clear();
            }
            else {
                $scope.submitted = true;
            }
        };

        //$state.reload = function () {
        //    
        //    $window.location.href = 'http://' + HostName + '/#/app/ExamSubjectWizard/';
        //}




        $scope.valid_from = function (att_from) {
            
            var num_att_from = Number(att_from);
            if (num_att_from > 100) {
                swal("Max Value Is 100");
                //angular.forEach($scope.rows, function (itm1) {
                //    if (itm1.count == from.count) {
                //        itm1.EMGD_From = "";
                //    }
                //})
                $scope.ATT_From = "";
            }
        }
        $scope.valid_to = function (att_to) {
            
            var num_att_to = Number(att_to);
            if (num_att_to > 100) {
                swal("Max Value Is 100");
                //angular.forEach($scope.rows, function (itm1) {
                //    if (itm1.count == to.count) {
                //        itm1.EMGD_To = "";
                //    }
                //})
                $scope.ATT_To = "";
            }
        }


        $scope.clear = function () {
            
            // $scope.ASMAY_Id = "";
            $scope.ASMAY_Id = $scope.temp_year_id;

            angular.forEach($scope.year_list, function (opq) {
                if (opq.asmaY_Id == $scope.ASMAY_Id) {
                    opq.Selected = true;
                }
            })
            $scope.get_category($scope.ASMAY_Id);
            $scope.category_list = $scope.tempcategorylist;
            $scope.EYC_Id = "";
            $scope.exam_list = $scope.tempexamlist;
            $scope.select_cat = false;
            $scope.subject_list = $scope.tempsubjectlist;
            $scope.subexam_list = $scope.tempsubexamlist;
            $scope.subsubject_list = $scope.tempsubsubjectlist;
            $scope.clear1();
            $scope.clear2();
            angular.forEach($scope.exam_list, function (itm1) {
                itm1.checked = false;
            })
            $scope.EMGR_Id = "";
            $scope.EYCE_AttendanceFromDate = "";
            $scope.EYCE_AttendanceToDate = "";
            $scope.EYCE_SubExamFlg = false;
            $scope.EYCE_SubSubjectFlg = false;
            angular.forEach($scope.subject_list, function (itm1) {
                itm1.EYCES_SubExamFlg = false;
                itm1.EYCES_SubSubjectFlg = false;
                itm1.EYCES_MarksGradeEntryFlg = 'M';
                itm1.EYCES_MarksDisplayFlg = true;
                itm1.EYCES_GradeDisplayFlg = false;
                itm1.EYCES_AplResultFlg = true;
                itm1.EMGR_Id = "";
            })
            $scope.all = true;
            $scope.toggleAll();
            $scope.EYCE_Id = 0;
            $scope.EYCES_Id = 0;
            $scope.SubWise_Selected_subexms_list = [];
            $scope.SubWise_Selected_subsubjs_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            // $scope.gridOptions.clearAllFilters();
            $scope.gridApi.grid.clearAllFilters();
            $scope.search = "";
        };

        $scope.clear1 = function () {
            
            //$scope.subexam_list = $scope.temp_subexam_list;
            $scope.subsubject_list = $scope.tempsubsubjectlist;
            angular.forEach($scope.subsubject_list, function (itm1) {
                itm1.EYCESSS_MaxMarks = "";
                itm1.EYCESSS_MinMarks = "";
                itm1.EMGR_Id = "";
                // itm1.EMGR_Id = $scope.EMGR_Id;
                itm1.EYCESSS_ExemptedFlg = false;
                itm1.EYCESSS_ExemptedPer = "";
            })
            $scope.all1 = true;
            $scope.toggleAll1();
            $scope.EYCESSS_Id = 0;
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";
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
            
            $scope.editEmployee = employee.eycE_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ExamSubjectMapping/getalldetailsviewrecords", pageid).
                    then(function (promise) {
                        
                        $scope.Category_Name = promise.view_exam_subjects[0].emcA_CategoryName;
                        $scope.Exam_Name = promise.view_exam_subjects[0].emE_ExamName;
                        $scope.viewrecordspopupdisplay = promise.view_exam_subjects;

                    })

        };
        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };
        $scope.viewrecordspopup_subexms = function (employee) {
            
            $scope.editEmployee = employee.eyceS_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ExamSubjectMapping/getalldetailsviewrecords_subexms", pageid).
                    then(function (promise) {
                        

                        $scope.Exm_Subject = promise.view_exam_subjects_subexams[0].ismS_SubjectName;
                        $scope.viewrecordspopupdisplay_subexms = promise.view_exam_subjects_subexams;

                    })

        };
        $scope.clearpopupgrid_subexms = function () {
            $scope.viewrecordspopupdisplay_subexms = "";
        };
        $scope.viewrecordspopup_subsubjs = function (employee) {
            
            $scope.editEmployee = employee.eyceS_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ExamSubjectMapping/getalldetailsviewrecords_subsubjs", pageid).
                    then(function (promise) {
                        

                        $scope.Exm_Subject = promise.view_exam_subjects_subsubjects[0].ismS_SubjectName;
                        $scope.viewrecordspopupdisplay_subsubjs = promise.view_exam_subjects_subsubjects;

                    })

        };
        $scope.clearpopupgrid_subsubjs = function () {
            $scope.viewrecordspopupdisplay_subsubjs = "";
        };


    }

})();