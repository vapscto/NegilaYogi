
(function () {
    'use strict';
    angular.module('app').controller('CumulativeReportController', CumulativeReportController)
    CumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'blockUI']
    function CumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, blockUI) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.report = false;
        $scope.submitted = false;
        $scope.print = true;
        $scope.applicable = true;
        $scope.nonapplicable = true;
        $scope.Left_Flag = true;
        $scope.Deactive_Flag = true;

        $scope.BindData = function () {
            apiService.getDATA("CumulativeReport/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
                $scope.clslist = promise.classlist;
                $scope.seclist = promise.seclist;
                $scope.amstlt = promise.amstlist;
                $scope.exsplt = promise.exmstdlist;
            });
        };

        $scope.OnAcdyear = function (ASMAY_Id) {
            $scope.print = true;
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("ExamCalculation_SSSE/get_classes", data).then(function (promise) {
                $scope.clslist = promise.classlist;
                $scope.asmcL_Id = "";
                $scope.asmS_Id = "";
                $scope.emE_Id = "";
                $scope.seclist = [];
                $scope.exsplt = [];
                $scope.studentlistdetails = [];
                if (promise.classlist == null || promise.classlist == "") {
                    swal("Classes are Not Mapped To Selected Academic Year!!!");
                }

            })
        };

        $scope.onchangeclass = function () {
            $scope.studentlistdetails = [];
            $scope.seclist = [];
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = [];
            $scope.print = true;
            $scope.report = false;
            if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined && $scope.asmaY_Id != null) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id
                };

                apiService.create("ExamCalculation_SSSE/get_cls_sections", data).then(function (promise) {
                    $scope.seclist = promise.seclist;
                    if (promise.seclist == null || promise.seclist == "") {
                        swal("Sections are Not Mapped To Selected Class!!!");
                    }
                });
            }
            else {
                swal("Please Select Academic Year  First !!!");
                $scope.asmcL_Id = "";
            }
        };

        $scope.onchangesection = function (ASMAY_Id, ASMCL_Id, ASMS_Id) {
            $scope.studentlistdetails = [];
            $scope.emE_Id = "";
            $scope.exsplt = [];
            $scope.print = true;
            $scope.report = false;
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "Left_Flag": $scope.Left_Flag,
                "Deactive_Flag": $scope.Deactive_Flag
            };

            apiService.create("ExamCalculation_SSSE/get_exams", data).then(function (promise) {
                $scope.emE_Id = "";
                $scope.exsplt = promise.exmstdlist;
                $scope.studentlistdetails = promise.studentlist;

                $scope.all = true;
                angular.forEach($scope.studentlistdetails, function (dd) {
                    dd.checkedsub = true;
                });

                if (promise.exmstdlist == null || promise.exmstdlist == "") {
                    swal("Exams are Not Mapped To Selected Class And Section!!!");
                }
            });
        };

        $scope.OnChangeLeftFlag = function () {
            $scope.report = false;
            $scope.print = true;
            $scope.searchchkbx = "";
            $scope.studentlistdetails = [];
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "Left_Flag": $scope.Left_Flag,
                "Deactive_Flag": $scope.Deactive_Flag
            };

            apiService.create("ExamCalculation_SSSE/get_exams", data).then(function (promise) {
                $scope.studentlistdetails = promise.studentlist;
                $scope.all = true;
                angular.forEach($scope.studentlistdetails, function (dd) {
                    dd.checkedsub = true;
                });
            });
        };

        $scope.onselectcategory = function () {
            $scope.report = false;
            $scope.print = true;
        };

        $scope.saveddata = function () {
            $scope.selectedamstids = [];
            $scope.submitted = true;
            $scope.print = true;
            $scope.report = false;

            if ($scope.myForm.$valid) {
                angular.forEach($scope.studentlistdetails, function (dd) {
                    if (dd.checkedsub === true) {
                        $scope.selectedamstids.push(dd.amsT_Id);
                    }
                });

                var data = {
                    "EME_ID": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "AMST_Ids": $scope.selectedamstids,
                    "applicableflag": $scope.applicable,
                    "nonapplicableflag": $scope.nonapplicable,
                };

                apiService.create("CumulativeReport/savedetails", data).then(function (promise) {

                    $scope.masterinst = promise.configuration;
                    $scope.examsubjectwise_details = promise.examsubjectwise_details;
                    $scope.rollno = false;
                    $scope.regno = false;
                    $scope.admno = false;
                    var count = 0;

                    if ($scope.masterinst !== null && $scope.masterinst.length > 0) {
                        if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay == true) {
                            $scope.regno = true;
                            count = count + 1;
                        }

                        if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay == true) {
                            $scope.admno = true;
                            count = count + 1;
                        }

                        if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay == true) {
                            $scope.rollno = true;
                            count = count + 1;
                        }
                        if (count == 0) {
                            $scope.admno = true;
                            $scope.rollno = true;
                        }
                    } else {
                        $scope.admno = true;
                        $scope.rollno = true;
                    }

                    angular.forEach($scope.clslist, function (itm) {
                        if (itm.asmcL_Id == $scope.asmcL_Id) {
                            $scope.cla = itm.asmcL_ClassName;
                        }
                    });
                    angular.forEach($scope.yearlt, function (itm) {
                        if (itm.asmaY_Id == $scope.asmaY_Id) {
                            $scope.yr = itm.asmaY_Year;
                        }
                    });
                    angular.forEach($scope.seclist, function (itm) {
                        if (itm.asmS_Id == $scope.asmS_Id) {
                            $scope.sec = itm.asmC_SectionName;
                        }
                    });
                    angular.forEach($scope.exsplt, function (itm) {
                        if (itm.emE_Id == $scope.emE_Id) {
                            $scope.exmmid = itm.emE_ExamName;
                        }
                    });

                    if (promise.savelist !== null && promise.savelist.length > 0) {
                        $scope.report = true;
                        $scope.mI_name = promise.savelist[0].mI_name;
                        $scope.studentslt = promise.savelist;
                        $scope.studentslt1 = promise.subjlist;

                        var temp_list = [];
                        for (var x = 0; x < promise.savelist.length; x++) {
                            var stu_id = promise.savelist[x].amsT_Id;
                            var stu_subj_list = [];
                            angular.forEach(promise.savelist, function (opq) {
                                if (opq.amsT_Id == stu_id) {
                                    var eyceS_MarksDisplayFlg_temp = true;
                                    var eyceS_GradeDisplayFlg_temp = false;
                                    angular.forEach($scope.examsubjectwise_details, function (dd) {
                                        if (opq.ismS_Id === dd.ismS_Id) {
                                            eyceS_MarksDisplayFlg_temp = dd.eyceS_MarksDisplayFlg;
                                            eyceS_GradeDisplayFlg_temp = dd.eyceS_GradeDisplayFlg;
                                        }
                                    });
                                    stu_subj_list.push({
                                        amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id,
                                        ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks,
                                        estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade,
                                        estmpS_PassFailFlg: opq.estmpS_PassFailFlg, eyceS_SubjectOrder: opq.eyceS_SubjectOrder,
                                        eyceS_MarksDisplayFlg: eyceS_MarksDisplayFlg_temp,
                                        eyceS_GradeDisplayFlg: eyceS_GradeDisplayFlg_temp,
                                        empatY_PaperTypeName: opq.empatY_PaperTypeName,
                                        empatY_Color: opq.empatY_Color, ESTMP_Result: opq.estmP_Result
                                    });
                                }
                            });

                            if (temp_list.length == 0) {
                                temp_list.push({
                                    student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName,
                                    amsT_AdmNo: promise.savelist[x].amsT_AdmNo,
                                    amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                    amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                    estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_SectionRank: promise.savelist[x].estmP_SectionRank,
                                    classheld: promise.savelist[x].classheld, classatt: promise.savelist[x].classatt,
                                    estmP_Result: promise.savelist[x].estmP_Result, sub_list: stu_subj_list
                                });
                            }
                            else if (temp_list.length > 0) {
                                var already_cnt = 0;
                                angular.forEach(temp_list, function (opq1) {
                                    if (opq1.student_id == stu_id) {
                                        already_cnt += 1;
                                    }
                                });
                                if (already_cnt == 0) {
                                    temp_list.push({
                                        student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName,
                                        amsT_LastName: promise.savelist[x].amsT_LastName,
                                        amsT_AdmNo: promise.savelist[x].amsT_AdmNo,
                                        amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                        amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                        estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks,
                                        estmP_SectionRank: promise.savelist[x].estmP_SectionRank, classheld: promise.savelist[x].classheld,
                                        classatt: promise.savelist[x].classatt, estmP_Result: promise.savelist[x].estmP_Result, sub_list: stu_subj_list
                                    });
                                }
                            }
                        }

                        $scope.studentslt = temp_list;
                        angular.forEach($scope.studentslt, function (oobj) {
                            $scope.tmparrry = [];
                            angular.forEach($scope.studentslt1, function (oobj1) {
                                var ccount = 0;
                                angular.forEach(oobj.sub_list, function (oobj2) {
                                    if (oobj1.ismS_Id == oobj2.ismS_Id) {
                                        ccount += 1;
                                        if (oobj2.estmpS_PassFailFlg != 'AB') {
                                            if (oobj2.estmpS_PassFailFlg === 'Fail') {
                                                if ($scope.masterinst[0].exmConfig_FailBoldFlg === true) {
                                                    oobj2.styleweigh = 'bold';
                                                } else {
                                                    oobj2.styleweigh = 'normal';
                                                }

                                                if ($scope.masterinst[0].exmConfig_FailItalicFlg === true) {
                                                    oobj2.styleitalic = 'italic';
                                                } else {
                                                    oobj2.styleitalic = 'normal';
                                                }
                                                if ($scope.masterinst[0].exmConfig_FailUnderscoreFlg === true) {
                                                    oobj2.styleunderline = 'underline';
                                                } else {
                                                    oobj2.styleunderline = 'normal';
                                                }

                                                if ($scope.masterinst[0].exmConfig_FailColorFlg !== "" && $scope.masterinst[0].exmConfig_FailColorFlg !== undefined
                                                    && $scope.masterinst[0].exmConfig_FailColorFlg !== null) {
                                                    oobj2.stylecolor = $scope.masterinst[0].exmConfig_FailColorFlg;
                                                } else {
                                                    oobj2.stylecolor = 'black';
                                                }
                                                oobj2.hema_estmpS_ObtainedMarks = oobj2.estmpS_ObtainedMarks;
                                                oobj2.hema_estmpS_ObtainedGrade = oobj2.estmpS_ObtainedGrade;

                                            } else {
                                                oobj2.hema_estmpS_ObtainedMarks = oobj2.estmpS_ObtainedMarks;
                                                oobj2.hema_estmpS_ObtainedGrade = oobj2.estmpS_ObtainedGrade;
                                                oobj2.styleunderline = 'normal';
                                                oobj2.styleitalic = 'normal';
                                                oobj2.styleweigh = 'normal';
                                                oobj2.stylecolor = 'black';
                                            }
                                        }
                                        else if (oobj2.estmpS_PassFailFlg == 'AB') {
                                            oobj2.hema_estmpS_ObtainedMarks = oobj2.estmpS_PassFailFlg;
                                        }
                                        $scope.tmparrry.push(oobj2);
                                    }
                                });

                                if (ccount == 0) {
                                    var obj = {};
                                    obj.hema_estmpS_ObtainedMarks = "";
                                    obj.hema_estmpS_ObtainedGrade = "";
                                    $scope.tmparrry.push(obj);
                                }
                            })
                            oobj.sub_list = $scope.tmparrry; //oobj.sub_list[0].estmpS_ObtainedMarks
                        });

                        if (promise.savenonsubjlist != null && promise.savenonsubjlist.length > 0) {

                            $scope.electivestd = promise.savenonsubjlist;
                            $scope.electivesub = promise.nonsubjlist;

                            var temp_list = [];
                            for (var x = 0; x < promise.savenonsubjlist.length; x++) {
                                var stu_id = promise.savenonsubjlist[x].amsT_Id;
                                var stu_subj_list = [];
                                angular.forEach(promise.savenonsubjlist, function (opq) {
                                    if (opq.amsT_Id == stu_id) {

                                        var eyceS_MarksDisplayFlg_temp = true;
                                        var eyceS_GradeDisplayFlg_temp = false;
                                        angular.forEach($scope.examsubjectwise_details, function (dd) {
                                            if (opq.ismS_Id === dd.ismS_Id) {
                                                eyceS_MarksDisplayFlg_temp = dd.eyceS_MarksDisplayFlg;
                                                eyceS_GradeDisplayFlg_temp = dd.eyceS_GradeDisplayFlg;
                                            }
                                        });

                                        stu_subj_list.push({
                                            amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName,
                                            estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks,
                                            estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg,
                                            eyceS_SubjectOrder: opq.eyceS_SubjectOrder,
                                            eyceS_MarksDisplayFlg: eyceS_MarksDisplayFlg_temp,
                                            eyceS_GradeDisplayFlg: eyceS_GradeDisplayFlg_temp,
                                            empatY_PaperTypeName: opq.empatY_PaperTypeName,
                                            empatY_Color: opq.empatY_Color,
                                        });
                                    }
                                })
                                if (temp_list.length == 0) {
                                    temp_list.push({
                                        student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                        amsT_LastName: promise.savenonsubjlist[x].amsT_LastName,
                                        amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                        amaY_RollNo: promise.savenonsubjlist[x].amaY_RollNo,
                                        amsT_RegistrationNo: promise.savenonsubjlist[x].amsT_RegistrationNo,
                                        estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks,
                                        estmP_Result: promise.savelist[x].estmP_Result,
                                        estmP_SectionRank: promise.savelist[x].estmP_SectionRank, classheld: promise.savelist[x].classheld,
                                        classatt: promise.savelist[x].classatt, sub_list: stu_subj_list
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
                                            student_id: promise.savenonsubjlist[x].amsT_Id,
                                            amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                            amsT_LastName: promise.savenonsubjlist[x].amsT_LastName,
                                            amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                            amaY_RollNo: promise.savenonsubjlist[x].amaY_RollNo,
                                            amsT_RegistrationNo: promise.savenonsubjlist[x].amsT_RegistrationNo,
                                            estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks,
                                            estmP_Result: promise.savelist[x].estmP_Result,
                                            estmP_SectionRank: promise.savelist[x].estmP_SectionRank, classheld: promise.savelist[x].classheld,
                                            classatt: promise.savelist[x].classatt, sub_list: stu_subj_list
                                        });
                                    }
                                }

                            }
                            $scope.nonstudentslt = temp_list;
                        }
                    }

                    else if (promise.savenonsubjlist != null && promise.savenonsubjlist.length > 0) {
                        $scope.report = true;
                        $scope.electivestd = promise.savenonsubjlist;
                        $scope.electivesub = promise.nonsubjlist;
                        var temp_list = [];
                        for (var x = 0; x < promise.savenonsubjlist.length; x++) {
                            $scope.mI_name = promise.savenonsubjlist[0].mI_name;
                            var stu_id = promise.savenonsubjlist[x].amsT_Id;
                            var stu_subj_list = [];
                            angular.forEach(promise.savenonsubjlist, function (opq) {
                                if (opq.amsT_Id == stu_id) {
                                    var eyceS_MarksDisplayFlg_temp = true;
                                    var eyceS_GradeDisplayFlg_temp = false;
                                    angular.forEach($scope.examsubjectwise_details, function (dd) {
                                        if (opq.ismS_Id === dd.ismS_Id) {
                                            eyceS_MarksDisplayFlg_temp = dd.eyceS_MarksDisplayFlg;
                                            eyceS_GradeDisplayFlg_temp = dd.eyceS_GradeDisplayFlg;
                                        }
                                    });

                                    stu_subj_list.push({
                                        amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName,
                                        estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks,
                                        estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg,
                                        eyceS_MarksDisplayFlg: eyceS_MarksDisplayFlg_temp,
                                        eyceS_GradeDisplayFlg: eyceS_GradeDisplayFlg_temp,
                                        empatY_PaperTypeName: opq.empatY_PaperTypeName,
                                        empatY_Color: opq.empatY_Color,
                                    });
                                }
                            });

                            if (temp_list.length == 0) {
                                temp_list.push({
                                    student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt, sub_list: stu_subj_list
                                });
                            }
                            else if (temp_list.length > 0) {
                                var already_cnt = 0;
                                angular.forEach(temp_list, function (opq1) {
                                    if (opq1.student_id == stu_id) {
                                        already_cnt += 1;
                                    }
                                });
                                if (already_cnt == 0) {
                                    temp_list.push({ student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt, sub_list: stu_subj_list });
                                }
                            }
                        }
                        $scope.nonstudentslt = temp_list;
                        $scope.studentslt = [];
                        angular.forEach(temp_list, function (xyz) {
                            $scope.studentslt.push({ student_id: xyz.student_id, amsT_FirstName: xyz.amsT_FirstName, amsT_LastName: xyz.amsT_LastName, classheld: xyz.classheld, classatt: xyz.classatt });
                        });

                    }

                    else if ((promise.savenonsubjlist == 0 || promise.savenonsubjlist == null) && (promise.savelist == null || promise.savelist.length>0)) {
                        swal('No record Found');
                    }

                    console.log($scope.electivesub);
                    console.log($scope.studentslt);
                    angular.forEach($scope.studentslt, function (oobj) {
                        angular.forEach($scope.nonstudentslt, function (oobj2) {
                            if (oobj2.student_id == oobj.student_id) {
                                $scope.tmparrry1 = [];
                                angular.forEach($scope.electivesub, function (oobj1) {
                                    var ccount1 = 0;
                                    angular.forEach(oobj2.sub_list, function (oobj3) {
                                        // ccount1 += 1;
                                        if (oobj1.ismS_Id == oobj3.ismS_Id) {
                                            ccount1 += 1;
                                            if (oobj3.estmpS_PassFailFlg != 'AB') {
                                                if (oobj3.estmpS_PassFailFlg === 'Fail') {
                                                    if ($scope.masterinst[0].exmConfig_FailBoldFlg === true) {
                                                        oobj3.styleweigh1 = 'bold';
                                                    } else {
                                                        oobj3.styleweigh1 = 'normal';
                                                    }
                                                    if ($scope.masterinst[0].exmConfig_FailItalicFlg === true) {
                                                        oobj3.styleitalic1 = 'italic';
                                                    } else {
                                                        oobj3.styleitalic1 = 'normal';
                                                    }
                                                    if ($scope.masterinst[0].exmConfig_FailUnderscoreFlg === true) {
                                                        oobj3.styleunderline1 = 'underline';
                                                    } else {
                                                        oobj3.styleunderline1 = 'normal';
                                                    }
                                                    if ($scope.masterinst[0].exmConfig_FailColorFlg !== "" && $scope.masterinst[0].exmConfig_FailColorFlg !== undefined
                                                        && $scope.masterinst[0].exmConfig_FailColorFlg !== null) {
                                                        oobj2.stylecolor1 = $scope.masterinst[0].exmConfig_FailColorFlg;
                                                    } else {
                                                        oobj3.stylecolor1 = 'black';
                                                    }
                                                    oobj3.hema_estmpS_ObtainedMarks = oobj3.estmpS_ObtainedMarks;
                                                    oobj3.hema_estmpS_ObtainedGrade = oobj3.estmpS_ObtainedGrade;

                                                } else {
                                                    oobj3.hema_estmpS_ObtainedMarks = oobj3.estmpS_ObtainedMarks;
                                                    oobj3.hema_estmpS_ObtainedGrade = oobj3.estmpS_ObtainedGrade;
                                                    oobj3.styleunderline1 = 'normal';
                                                    oobj3.styleitalic1 = 'normal';
                                                    oobj3.styleweigh1 = 'normal';
                                                    oobj3.stylecolor1 = 'black';
                                                }
                                            }
                                            else if (oobj3.estmpS_PassFailFlg == 'AB') {
                                                oobj3.hema_estmpS_ObtainedMarks = oobj3.estmpS_PassFailFlg;
                                            }
                                            $scope.tmparrry1.push(oobj3);
                                        }
                                    });

                                    if (ccount1 == 0) {
                                        var obj1 = {};
                                        obj1.hema_estmpS_ObtainedMarks = "";
                                        obj1.hema_estmpS_ObtainedGrade = "";
                                        $scope.tmparrry1.push(obj1);
                                    }
                                });
                                oobj.sub_list_e = $scope.tmparrry1;
                            }
                        });
                    });
                });
            }
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.subchkbx = function (column, obj, user) {
            if (obj.abc == true) {
                angular.forEach($scope.subjectlt1, function (itm1) {
                    if (itm1.amsT_Id == user.amsT_Id) {
                        angular.forEach($scope.subjectlt, function (itm2) {
                            if (itm2.ismS_Id == column.ismS_Id) {
                                itm1.sub_list.push({ id: itm2.ismS_Id, name: itm2.ismS_SubjectName });
                            }
                        });
                    }
                });
            }
            else {
                angular.forEach($scope.subjectlt1, function (itm1) {
                    if (itm1.amsT_Id == user.amsT_Id) {
                        for (var i = 0; i < itm1.sub_list.length; i++) {
                            if (itm1.sub_list[i].id == column.ismS_Id) {
                                itm1.sub_list.splice(i, 1);
                            }
                        }
                    }
                });
            }
        };

        $scope.exportToExcel = function (tableIds) {
            var excelname = $scope.exmmid + ' - Cumulative Report - ' + $scope.yr + '-' + $scope.cla + '-' + $scope.sec + '.xls';
            var exportHref = Excel.tableToExcel(tableIds, 'Cumulative Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.OnClickAll = function () {
            angular.forEach($scope.studentlistdetails, function (dd) {
                dd.checkedsub = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.studentlistdetails.every(function (itm) { return itm.checkedsub; });
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.studentlistdetails.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
    }
})();