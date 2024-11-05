
//(function () {
//    'use strict';
//    angular
//.module('app')
//.controller('BLPUMIDTERMReportController', BLPUMIDTERMReportController)

//    BLPUMIDTERMReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
//    function BLPUMIDTERMReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


//        $scope.DeleteRecord = {};
//        $scope.EditRecord = {};
//        $scope.obj = {};
//        $scope.studentlist = false;
//        $scope.currentPage = 1;
//        $scope.itemsPerPage = 5;

//        //TO  GEt The Values iN Grid

//        $scope.BindData = function () {
//            apiService.getDATA("BaldwinPUReport/Getdetails").
//       then(function (promise) {

//           $scope.yearlt = promise.yearlist;
//           $scope.clslist = promise.classlist;
//           $scope.seclist = promise.seclist;
//           $scope.amstlt = promise.amstlist;
//           $scope.exsplt = promise.exmstdlist;
//           //  $scope.gridOptions.data = promise.studmaplist;
//       })
//        };



//        $scope.sort = function (keyname) {
//            $scope.sortKey = keyname;   //set the sortKey to the param passed
//            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
//        }

//        $scope.interacted = function (field) {
//            return $scope.submitted;
//        };



//        // TO Save The Data
//        $scope.submitted = false;
//        $scope.saveddata = function () {

//            angular.forEach($scope.yearlt, function (yr) {
//                if (yr.asmaY_Id == $scope.asmaY_Id) {
//                    $scope.year = yr.asmaY_Year;
//                }
//            })
//            $scope.submitted = true;
//            if ($scope.myForm.$valid) {

//                var data = {
//                    "EME_Id": $scope.emE_Id,
//                    "ASMAY_Id": $scope.asmaY_Id,
//                    "ASMCL_Id": $scope.asmcL_Id,
//                    "ASMS_Id": $scope.asmS_Id
//                }
//                var config = {
//                    headers: {
//                        'Content-Type': 'application/json;'
//                    }
//                }
//                apiService.create("BaldwinPUReport/savedetails", data).
//                         then(function (promise) {

//                             if (promise.savelist.length > 0) {
//                                 //inst name binding
//                                 $scope.instname = promise.instname;
//                                 $scope.inst_name = $scope.instname[0].mI_Name;

//                                 var temp_list = [];
//                                 for (var x = 0; x < promise.savelist.length; x++) {
//                                     var stu_id = promise.savelist[x].amsT_Id;
//                                     var stu_subj_list = [];
//                                     angular.forEach(promise.savelist, function (opq) {
//                                         if (opq.amsT_Id == stu_id) {
//                                             stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg, ESTMPS_ClassAverage: opq.estmpS_ClassAverage, ESTMPS_SectionAverage: opq.estmpS_SectionAverage, ESTMPS_ClassHighest: opq.estmpS_ClassHighest, ESTMPS_SectionHighest: opq.estmpS_SectionHighest });
//                                         }
//                                     })
//                                     if (temp_list.length == 0) {

//                                         temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
//                                     }
//                                     else if (temp_list.length > 0) {
//                                         var already_cnt = 0;
//                                         angular.forEach(temp_list, function (opq1) {

//                                             if (opq1.student_id == stu_id) {
//                                                 already_cnt += 1;

//                                             }
//                                         })
//                                         if (already_cnt == 0) {
//                                             temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
//                                         }
//                                     }

//                                 }
//                                 $scope.exam = promise.savelist[0].emE_ExamName;
//                                 $scope.exm_sublist = promise.subjlist;
//                                 $scope.processtot = promise.savelisttot;
//                                 $scope.subj_grade_remarks = promise.grade_details;
//                                 if (promise.clstchname.length > 0) {
//                                     $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
//                                 } else {
//                                     $scope.clastechname = "";
//                                 }
//                                 //  $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
//                                 $scope.report_list = temp_list;
//                                 $scope.stud_work_attendence = promise.work_attendence;
//                                 $scope.stud_present_attendence = promise.present_attendence;

//                                 $scope.tempss = [];
//                                 var ss = 0;
//                                 //angular.forEach($scope.exm_sublist, function (bb) {
//                                 //    ss += 1;
//                                 //    if (ss==1) {
//                                 //        bb.nn = 'PART-1 LANGUAGES';
//                                 //        bb.cn = 1;
//                                 //    }
//                                 //    if (ss == 2) {
//                                 //        bb.nn = 'PART-2 OPTIONALS';
//                                 //        bb.cn = 2;
//                                 //    }
//                                 //    if (ss >2) {
//                                 //        bb.nn = '';
//                                 //    }
//                                 //})

//                                 console.log($scope.exm_sublist);


//                             } else {
//                                 swal('No record Found');
//                             }
//                         })
//            }
//        };

//        $scope.cancel = function () {
//            $scope.asmcL_Id = ""
//            $scope.emcA_Id = ""
//            $scope.asmaY_Id = ""
//            $scope.emG_Id = ""
//            $scope.asmS_Id = ""
//            $scope.subjectlt = ""
//            $scope.subjectlt1 = ""
//            $scope.studentlist = false;
//            $state.reload();
//        }

//        $scope.toggleAll = function () {

//            var toggleStatus = $scope.all;
//            angular.forEach($scope.subjectlt1, function (itm) {
//                itm.xyz = toggleStatus;

//            });
//        }

//        $scope.optionToggled = function (chk_box) {
//            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; })
//        }


//        //to print
//        $scope.printToCart = function () {
//            var innerContents = document.getElementById("Baldwin").innerHTML;
//            var popupWinindow = window.open('');
//            popupWinindow.document.open();
//            popupWinindow.document.write('<html><head>' +
//                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
//                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BBPUProgressReportPdf.css" />' +
//              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
//            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
//            popupWinindow.document.close();
//        }

//        $scope.get_totalmin = function (exm_subjs, stu_subjs) {


//            $scope.stu_grandmin_marks = 0;
//            angular.forEach(exm_subjs, function (itm) {
//                if (itm.eyceS_AplResultFlg) {
//                    angular.forEach(stu_subjs, function (itm1) {
//                        if (itm1.ismS_Id == itm.ismS_Id) {
//                            $scope.stu_grandmin_marks += itm.eyceS_MinMarks;
//                        }
//                    })
//                }
//            })
//        }
//    }
//})();

(function () {
    'use strict';
    angular
        .module('app')
        .controller('BLPUMIDTERMReportController', BLPUMIDTERMReportController)

    BLPUMIDTERMReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function BLPUMIDTERMReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.report = false;
        //TO  GEt The Values iN Grid

        $scope.arrtemp = [
            { id: 1, name: 'THEORY' },
            { id: 2, name: 'PRACTICALS' }
        ]

        $scope.BindData = function () {
            apiService.getDATA("BaldwinPUReport/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.clslist = promise.classlist;
                    $scope.seclist = promise.seclist;
                    $scope.amstlt = promise.amstlist;
                    $scope.exsplt = promise.exmstdlist;
                    //  $scope.gridOptions.data = promise.studmaplist;
                })
        };



        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {

            angular.forEach($scope.yearlt, function (yr) {
                if (yr.asmaY_Id == $scope.asmaY_Id) {
                    $scope.year = yr.asmaY_Year;
                }
            })
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("BaldwinPUReport/savedetails", data).
                    then(function (promise) {

                        if (promise.savelist.length > 0) {

                            $scope.report = true;
                            //inst name binding
                            $scope.instname = promise.instname;
                            $scope.inst_name = $scope.instname[0].mI_Name;

                            var temp_list = [];
                            for (var x = 0; x < promise.savelist.length; x++) {
                                var stu_id = promise.savelist[x].amsT_Id;
                                var stu_subj_list = [];
                                angular.forEach(promise.savelist, function (opq) {
                                    if (opq.amsT_Id == stu_id) {
                                        stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg, ESTMPS_ClassAverage: opq.estmpS_ClassAverage, ESTMPS_SectionAverage: opq.estmpS_SectionAverage, ESTMPS_ClassHighest: opq.estmpS_ClassHighest, ESTMPS_SectionHighest: opq.estmpS_SectionHighest, EMSE_SubExamName: opq.emsE_SubExamName, ESTMPSSS_ObtainedMarks: opq.estmpssS_ObtainedMarks });
                                    }
                                })
                                if (temp_list.length == 0) {

                                    temp_list.push({
                                        student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, ismS_SubjectName: promise.savelist[x].ismS_SubjectName, sub_list: stu_subj_list
                                    });
                                }
                                else if (temp_list.length > 0) {
                                    var already_cnt = 0;
                                    angular.forEach(temp_list, function (opq1) {

                                        if (opq1.student_id == stu_id) {
                                            already_cnt += 1;

                                        }
                                    })
                                    if (already_cnt == 0) {
                                        temp_list.push({
                                            student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade,
                                            ismS_SubjectName: promise.savelist[x].ismS_SubjectName, sub_list: stu_subj_list
                                        });
                                    }
                                }

                            }

                            $scope.exam = promise.savelist[0].emE_ExamName;
                            $scope.exm_sublist = promise.subjlist;
                            $scope.processtot = promise.savelisttot;
                            $scope.subj_grade_remarks = promise.grade_details;
                            if (promise.clstchname.length > 0) {
                                $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                            } else {
                                $scope.clastechname = "";
                            }
                            //  $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                            $scope.report_list = temp_list;
                            $scope.stud_work_attendence = promise.work_attendence;
                            $scope.stud_present_attendence = promise.present_attendence;
                            console.log($scope.stud_work_attendence)
                            $scope.tempss = [];
                            var ss = 0;
                            var cn = 0;


                            $scope.temparry = [];
                            $scope.temparry.push({ part: 'PART-I LANGUAGES' });
                            $scope.temparry.push({ part: 'PART-II OPTIONALS' });
                            var a1 = [];
                            var a1 = [];
                            $scope.a11 = [];
                            $scope.a22 = [];

                            console.log($scope.report_list);

                            angular.forEach($scope.report_list, function (dddd) {
                                angular.forEach(dddd.sub_list, function (ddddd) {
                                    angular.forEach($scope.exm_sublist, function (ff) {
                                        if (ddddd.ismS_Id == ff.ismS_Id) {
                                            cn += 1;
                                            if (ff.ismS_SubjectName != '') {
                                                if (ff.ismS_SubjectName.toLowerCase() == 'kannada' || ff.ismS_SubjectName.toLowerCase() == 'english' || ff.ismS_SubjectName.toLowerCase() == 'french' || ff.ismS_SubjectName.toLowerCase() == 'hindi' || ff.ismS_SubjectName.toLowerCase() == 'sanskrit' || ff.ismS_SubjectName.toLowerCase() == 'urdu') {
                                                    ff.a = 1;
                                                    console.log(ff)
                                                    $scope.a11.push(ff);
                                                }
                                                else {
                                                    $scope.a22.push(ff);
                                                }
                                            }
                                        }
                                    })
                                })
                            })


                            $scope.per_status = [];

                            angular.forEach($scope.a11, function (eps) {
                                if ($scope.per_status.length == 0) {
                                    $scope.per_status.push(eps);
                                }
                                else if ($scope.per_status.length > 0) {
                                    var al_exm_cnt = 0;
                                    angular.forEach($scope.per_status, function (exm) {
                                        if (exm.ismS_Id == eps.ismS_Id) {
                                            al_exm_cnt += 1;
                                        }
                                    })
                                    if (al_exm_cnt == 0) {
                                        $scope.per_status.push(eps);
                                    }
                                }
                            })

                            $scope.per_status1 = [];
                            angular.forEach($scope.a22, function (eps1) {

                                if ($scope.per_status1.length == 0) {
                                    $scope.per_status1.push(eps1);
                                }
                                else if ($scope.per_status1.length > 0) {
                                    var al_exm_cnt = 0;
                                    angular.forEach($scope.per_status1, function (exm) {
                                        if (exm.ismS_Id == eps1.ismS_Id) {
                                            al_exm_cnt += 1;
                                        }
                                    })
                                    if (al_exm_cnt == 0) {
                                        $scope.per_status1.push(eps1);
                                    }
                                }
                            })



                            angular.forEach($scope.report_list, function (eps1) {
                                $scope.subjectqqq = [];
                                angular.forEach(eps1.sub_list, function (sssb) {

                                    //  sssb.ismS_SubjectName = eps1.ismS_SubjectName;

                                    if ($scope.subjectqqq.length == 0) {
                                        $scope.subjectqqq.push(sssb);
                                    }

                                    else if ($scope.subjectqqq.length > 0) {
                                        var al_exm_cnt = 0;
                                        angular.forEach($scope.subjectqqq, function (exm) {
                                            if (exm.ismS_Id == sssb.ismS_Id) {
                                                al_exm_cnt += 1;
                                            }
                                        })
                                        if (al_exm_cnt == 0) {
                                            $scope.subjectqqq.push(sssb);
                                        }
                                    }
                                })

                                eps1.mainlist = $scope.subjectqqq;
                            })

                            console.log($scope.report_list);

                            var l = 0;
                            angular.forEach($scope.temparry, function (kk) {
                                if (l == 0) {
                                    l += 1;
                                    kk.sb = $scope.per_status;
                                    kk.gg = $scope.per_status.length;
                                }
                                else {
                                    kk.sb = $scope.per_status1;
                                    kk.oo = $scope.per_status1.length;
                                }
                            });

                            $scope.tmee = [];
                            angular.forEach($scope.report_list, function (kl) {

                                $scope.tempsubjectstd = [];
                                $scope.tempsubjectstdww = [];
                                $scope.tempsubjectstd1ddfsdf = [];
                                angular.forEach($scope.temparry, function (kj) {

                                    $scope.tempsubjectstd1 = [];
                                   

                                    angular.forEach(kj.sb, function (jk) {

                                        angular.forEach(kl.mainlist, function (lk) {

                                            if (lk.ismS_Id == jk.ismS_Id && lk.amsT_Id == kl.student_id) {
                                                $scope.tempsubjectstd1.push({ ismd: lk.ismS_Id, subname: lk.ismS_SubjectName, eyceS_MaxMarks: lk.estmpS_MaxMarks });
                                            }
                                        })
                                    })
                                    $scope.tempsubjectstd1ddfsdf.push({ partd: kj.part, kdjd: $scope.tempsubjectstd1 });
                                    //kj.tempsubjectstd2 = $scope.tempsubjectstd1;
                                })

                                $scope.tempsubjectstdww = $scope.tempsubjectstd1ddfsdf;
                               // kl.push({ ggg: $scope.tempsubjectstdww })
                                $scope.tmee.push({
                                    amat: kl.student_id, ggg: $scope.tempsubjectstdww, amaY_RollNo: kl.amaY_RollNo,
                                    amsT_AdmNo: kl.amsT_AdmNo, amsT_DOB: kl.amsT_DOB, amsT_FirstName:kl.amsT_FirstName,
                                    asmC_SectionName: kl.asmC_SectionName, asmcL_ClassName: kl.asmcL_ClassName, estmP_Percentage: kl.estmP_Percentage,
                                    estmP_TotalGrade: kl.estmP_TotalGrade, estmP_TotalMaxMarks: kl.estmP_TotalMaxMarks, estmP_TotalObtMarks: kl.estmP_TotalObtMarks,
                                    ismS_SubjectName: kl.ismS_SubjectName, mainlist: kl.mainlist, student_id: kl.student_id, sub_list: kl.sub_list});
                            })


                            console.log($scope.temparry);

                            console.log("dddd");
                            console.log($scope.report_list);
                            console.log("vvvmain");
                            console.log($scope.tmee);
                            console.log("pppp");
                            console.log($scope.tempsubjectstd1ddfsdf);
                            console.log("total");
                            console.log($scope.processtot);
                            
                        } else {
                            swal('No record Found');
                        }
                    })
            }
        };

        $scope.cancel = function () {
            $scope.asmcL_Id = ""
            $scope.emcA_Id = ""
            $scope.asmaY_Id = ""
            $scope.emG_Id = ""
            $scope.asmS_Id = ""
            $scope.subjectlt = ""
            $scope.subjectlt1 = ""
            $scope.studentlist = false;
            $state.reload();
        }

        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;

            });
        }

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; })
        }


        //to print
        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BBPUProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.get_totalmin = function (exm_subjs, stu_subjs) {


            $scope.stu_grandmin_marks = 0;
            angular.forEach(exm_subjs, function (itm) {
                if (itm.eyceS_AplResultFlg) {
                    angular.forEach(stu_subjs, function (itm1) {
                        if (itm1.ismS_Id == itm.ismS_Id) {
                            $scope.stu_grandmin_marks += itm.eyceS_MinMarks;
                        }
                    })
                }

            })
        };

        $scope.OnAcdyear = function () {

            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("CumulativeReport/onchangeyear", data).then(function (promise) {

                if (promise != null) {
                    $scope.clslist = promise.classlist;
                    if ($scope.clslist.length == 0) {
                        swal("No Class Is Mapped For Selected Academic Year");
                    }
                } else {
                    swal("No Class Is Mapped For Selected Academic Year");
                }

            });
        };

        $scope.onchangeclass = function () {

            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("CumulativeReport/onchangeclass", data).then(function (promise) {

                if (promise != null) {
                    $scope.seclist = promise.seclist;
                    if ($scope.seclist.length == 0) {
                        swal("No Section Is Mapped For Selected Class");
                    }

                } else {
                    swal("No Section Is Mapped For Selected Class");
                }

            });
        };

        $scope.onchangesection = function () {

            $scope.emE_Id = "";
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            };

            apiService.create("CumulativeReport/onchangesection", data).then(function (promise) {

                if (promise != null) {
                    $scope.exsplt = promise.exmstdlist;
                    if ($scope.exsplt.length == 0) {
                        swal("No Exam Is Mapped For Selected Details ");
                    }
                } else {
                    swal("No Exam Is Mapped For Selected Details");
                }

            });
        };

        $scope.onselectcategory = function () {
            $scope.report = false;
        };


    }

})();