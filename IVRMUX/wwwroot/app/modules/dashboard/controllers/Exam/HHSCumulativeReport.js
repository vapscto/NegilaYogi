(function () {
    'use strict';
    angular.module('app').controller('HHSCumulativeReportController', HHSCumulativeReportController)

    HHSCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function HHSCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.repoershow = false;
        $scope.font_size = 'font25';
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.submitted = false;
        $scope.searchchkbx = "";
        $scope.applicable = true;
        $scope.nonapplicable = true;
        $scope.Left_Flag = true;
        $scope.Deactive_Flag = true;
        $scope.print = true;

        $scope.fonts = [
            { name: '10px', size: '10px ', class: 'font10' },
            { name: '11px', size: '11px ', class: 'font11' },
            { name: '12px', size: '12px ', class: 'font12' },
            { name: '13px', size: '13px ', class: 'font13' },
            { name: '14px', size: '14px ', class: 'font14' },
            { name: '15px', size: '15px', class: 'font15' },
            { name: '16px', size: '16px', class: 'font16' },
            { name: '17px', size: '17px', class: 'font17' }, { name: '18px', size: '18px', class: 'font18' }, { name: '25px', size: '25px', class: 'font25' }
        ];

        $scope.BindData = function () {
            apiService.getDATA("CumulativeReport/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
                $scope.clslist = promise.classlist;
                $scope.seclist = promise.seclist;
                $scope.amstlt = promise.amstlist;
                $scope.exsplt = promise.exmstdlist;
            });
        };

        $scope.get_cls_sections = function () {
            $scope.repoershow = false;
            $scope.print = true;
            $scope.searchchkbx = "";
            $scope.studentlistdetails = [];
            if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined && $scope.asmaY_Id != null) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id
                };

                apiService.create("ExamCalculation_SSSE/get_cls_sections", data).then(function (promise) {
                    $scope.seclist = promise.seclist;
                    $scope.asmS_Id = "";
                    $scope.emE_Id = "";
                    $scope.exsplt = [];
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

        $scope.get_classes = function (ASMAY_Id) {
            $scope.repoershow = false;
            $scope.print = true;
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("ExamCalculation_SSSE/get_classes", data).then(function (promise) {
                $scope.clslist = promise.classlist;
                $scope.asmcL_Id = "";
                $scope.asmS_Id = "";
                $scope.emE_Id = "";
                $scope.seclist = [];
                $scope.exsplt = [];
                $scope.searchchkbx = "";
                $scope.studentlistdetails = [];
                if (promise.classlist == null || promise.classlist == "") {
                    swal("Classes are Not Mapped To Selected Academic Year!!!");
                }
            });
        };

        $scope.get_exams = function (ASMAY_Id, ASMCL_Id, ASMS_Id) {
            $scope.repoershow = false;
            $scope.print = true;
            $scope.searchchkbx = "";
            $scope.studentlistdetails = [];
            if (ASMAY_Id != null && ASMAY_Id != undefined && ASMCL_Id != null && ASMCL_Id != undefined) {
                var data = {
                    "ASMS_Id": ASMS_Id,
                    "ASMCL_Id": ASMCL_Id,
                    "ASMAY_Id": ASMAY_Id,
                    "Left_Flag": $scope.Left_Flag,
                    "Deactive_Flag": $scope.Deactive_Flag
                };
                apiService.create("ExamCalculation_SSSE/get_exams", data).then(function (promise) {
                    $scope.emE_Id = "";
                    $scope.exsplt = promise.exmstdlist;

                    if (promise.exmstdlist == null || promise.exmstdlist == "") {
                        swal("Exams are Not Mapped To Selected Class And Section!!!");
                    }

                    $scope.studentlistdetails = promise.studentlist;
                    $scope.all = true;
                    angular.forEach($scope.studentlistdetails, function (dd) {
                        dd.checkedsub = true;
                    });

                });
            }
            else {
                swal("Please Select Academic Year  And Class First !!!");
                $scope.asmS_Id = "";
            }
        };

        $scope.OnChangeLeftFlag = function () {
            $scope.repoershow = false;
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
            $scope.repoershow = false;
            $scope.print = true;
        };

        $scope.saveddata = function () {
            $scope.repoershow = false;
            $scope.electivestd = [];
            $scope.electivesub = [];
            $scope.submitted = true;
            $scope.print = true;
            $scope.selectedamstids = [];

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
                    "nonapplicableflag": $scope.nonapplicableflag
                };
                apiService.create("CumulativeReport/savedetails", data).then(function (promise) {
                    $scope.masterinst = promise.configuration;
                    var count = 0;
                    if ($scope.masterinst !== null && $scope.masterinst.length > 0) {
                        if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay == true) {
                            $scope.regno = true;
                            count = count + 1;
                        } else {
                            $scope.regno = false;
                        }
                        if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay == true) {
                            $scope.admno = true;
                            count = count + 1;
                        } else {
                            $scope.admno = false;
                        }
                        if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay == true) {
                            $scope.rollno = true;
                            count = count + 1;
                        } else {
                            $scope.rollno = false;
                        }
                        if (count == 0) {
                            $scope.admno = true;
                            $scope.rollno = true;
                        }
                    } else {
                        $scope.admno = true;
                        $scope.rollno = true;
                    }

                    if ($scope.masterinst[0].exmConfig_Recordsearchtype == "Name") {
                        $scope.sortKey = "amsT_FirstName";
                    }

                    else if ($scope.masterinst[0].exmConfig_Recordsearchtype == "AdmNo") {
                        $scope.sortKey = "amsT_AdmNo";
                    }

                    else if ($scope.masterinst[0].exmConfig_Recordsearchtype == "RollNo") {
                        $scope.sortKey = "amaY_RollNo";
                    }

                    else if ($scope.masterinst[0].exmConfig_Recordsearchtype == "RegNo") {
                        $scope.sortKey = "amsT_RegistrationNo";
                    }
                    else {
                        $scope.sortKey = "amsT_FirstName";
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

                    $scope.reportname = $scope.exmmid + " Cumulative Report";

                    if (promise.savelist !== null && promise.savelist != null) {
                        $scope.repoershow = true;
                        $scope.print = false;

                        $scope.instname = promise.instname;
                        $scope.inst_name = $scope.instname !== null && $scope.instname.length > 0 ? $scope.instname[0].mI_Name : "";

                        $scope.examsubjectwise_details = promise.examsubjectwise_details;
                        $scope.studentslt = promise.savelist;
                        $scope.studentslt1 = promise.subjlist;
                        $scope.studenwise_remarks = promise.studenwise_remarks;

                        var temp_list = [];
                        for (var x = 0; x < promise.savelist.length; x++) {
                            var stu_id = promise.savelist[x].amsT_Id;
                            var stu_subj_list = [];
                            angular.forEach(promise.savelist, function (opq) {
                                if (opq.amsT_Id == stu_id) {
                                    stu_subj_list.push({
                                        amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.ismS_SubjectName,
                                        estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks,
                                        estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg
                                    });
                                }
                            });
                            if (temp_list.length == 0) {
                                temp_list.push({
                                    student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName,
                                    amsT_AdmNo: promise.savelist[x].amsT_AdmNo,
                                    amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                    amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                    classheld: promise.savelist[x].classheld,
                                    classatt: promise.savelist[x].classatt, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks,
                                    estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage,
                                    estmP_Result: promise.savelist[x].estmP_Result, estmP_SectionRank: promise.savelist[x].estmP_SectionRank,
                                    sub_list: stu_subj_list
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
                                        amsT_LastName: promise.savelist[x].amsT_LastName, amsT_AdmNo: promise.savelist[x].amsT_AdmNo,
                                        amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                        amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                        classheld: promise.savelist[x].classheld, classatt: promise.savelist[x].classatt,
                                        estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks,
                                        estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks,
                                        estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_Result: promise.savelist[x].estmP_Result,
                                        estmP_SectionRank: promise.savelist[x].estmP_SectionRank, sub_list: stu_subj_list
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
                                        angular.forEach($scope.examsubjectwise_details, function (sub_det) {
                                            if (sub_det.ismS_Id == oobj2.ismS_Id) {
                                                oobj2.eyceS_MarksDisplayFlg = sub_det.eyceS_MarksDisplayFlg;
                                                oobj2.eyceS_GradeDisplayFlg = sub_det.eyceS_GradeDisplayFlg;
                                            }
                                        });
                                        ccount += 1;
                                        if (oobj2.estmpS_PassFailFlg != 'AB') {
                                            oobj2.hema_estmpS_ObtainedMarks = oobj2.estmpS_ObtainedMarks;
                                            oobj2.hema_estmpS_ObtainedGrade = oobj2.estmpS_ObtainedGrade;
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
                            });
                            oobj.sub_list = $scope.tmparrry;
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
                                        stu_subj_list.push({
                                            amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName,
                                            estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks,
                                            estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg
                                        });
                                    }
                                })
                                if (temp_list.length == 0) {
                                    temp_list.push({
                                        student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                        amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                        amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                        amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                        classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt,
                                        estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks,
                                        estmP_TotalMaxMarks: promise.savenonsubjlist[x].estmP_TotalMaxMarks,
                                        estmP_Percentage: promise.savenonsubjlist[x].estmP_Percentage, estmP_Result: promise.savenonsubjlist[x].estmP_Result,
                                        estmP_SectionRank: promise.savenonsubjlist[x].estmP_SectionRank, sub_list: stu_subj_list
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
                                            amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                            amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                            amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                            classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt,
                                            estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks,
                                            estmP_TotalMaxMarks: promise.savenonsubjlist[x].estmP_TotalMaxMarks,
                                            estmP_Percentage: promise.savenonsubjlist[x].estmP_Percentage,
                                            estmP_Result: promise.savenonsubjlist[x].estmP_Result,
                                            estmP_SectionRank: promise.savenonsubjlist[x].estmP_SectionRank, sub_list: stu_subj_list
                                        });
                                    }
                                }

                            }
                            $scope.nonstudentslt = temp_list;
                            $scope.newarray1 = [];
                            angular.forEach($scope.studentslt, function (testobj) {
                                angular.forEach($scope.electivesub, function (testobjele) {
                                    angular.forEach($scope.nonstudentslt, function (testobj1) {
                                        if (testobj.student_id == testobj1.student_id) {
                                            angular.forEach(testobj1.sub_list, function (testobj2) {
                                                if (testobjele.ismS_Id == testobj2.ismS_Id) {
                                                    $scope.newarray1.push({
                                                        stuid: testobj.student_id, subid: testobjele.ismS_Id,
                                                        abc: testobj2.estmpS_ObtainedMarks + ' ' + testobj2.estmpS_ObtainedGrade
                                                    });
                                                }
                                            });
                                        }
                                    });
                                });
                            });
                        }
                    }
                    else if (promise.savenonsubjlist != null && promise.savenonsubjlist.length > 0) {
                        $scope.repoershow = true;
                        $scope.print = false;
                        $scope.electivestd = promise.savenonsubjlist;
                        $scope.electivesub = promise.nonsubjlist;
                        var temp_list = [];
                        for (var x = 0; x < promise.savenonsubjlist.length; x++) {
                            var stu_id = promise.savenonsubjlist[x].amsT_Id;
                            var stu_subj_list = [];
                            angular.forEach(promise.savenonsubjlist, function (opq) {
                                if (opq.amsT_Id == stu_id) {
                                    stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg });
                                }
                            });
                            if (temp_list.length == 0) {
                                temp_list.push({
                                    student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                    amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                    amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                    amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                    classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt,
                                    estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks,
                                    estmP_TotalMaxMarks: promise.savenonsubjlist[x].estmP_TotalMaxMarks,
                                    estmP_Result: promise.savenonsubjlist[x].estmP_Result, estmP_Percentage: promise.savenonsubjlist[x].estmP_Percentage,
                                    sub_list: stu_subj_list
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
                                        student_id: promise.savenonsubjlist[x].amsT_Id,
                                        amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName,
                                        amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                        amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                        amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                        classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt,
                                        estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks,
                                        estmP_TotalMaxMarks: promise.savenonsubjlist[x].estmP_TotalMaxMarks,
                                        estmP_Result: promise.savenonsubjlist[x].estmP_Result, estmP_Percentage: promise.savenonsubjlist[x].estmP_Percentage,
                                        sub_list: stu_subj_list
                                    });
                                }
                            }
                        }
                        $scope.nonstudentslt = temp_list;
                        $scope.studentslt = [];
                        angular.forEach(temp_list, function (xyz) {
                            $scope.studentslt.push({ student_id: xyz.student_id, amsT_FirstName: xyz.amsT_FirstName, amsT_LastName: xyz.amsT_LastName, classheld: xyz.classheld, classatt: xyz.classatt });
                        });
                    }
                    else if ((promise.savenonsubjlist == null || promise.savenonsubjlist.length == 0) && (promise.savelist == null || promise.savelist.length == 0)) {
                        swal('No record Found');
                    }

                    console.log($scope.studentslt);

                    angular.forEach($scope.studentslt, function (stu) {
                        angular.forEach($scope.studenwise_remarks, function (stu_rmks) {
                            if (stu.student_id === stu_rmks.amsT_Id) {
                                stu.student_remarks = stu_rmks.emeR_Remarks;
                            }
                        });
                    });
                    angular.forEach($scope.studentslt, function (oobj) {
                        angular.forEach($scope.nonstudentslt, function (oobj2) {
                            if (oobj2.student_id == oobj.student_id) {
                                $scope.tmparrry1 = [];
                                angular.forEach($scope.electivesub, function (oobj1) {
                                    var ccount1 = 0;
                                    angular.forEach(oobj2.sub_list, function (oobj3) {
                                        //ccount1 += 1;
                                        if (oobj1.ismS_Id == oobj3.ismS_Id) {
                                            angular.forEach($scope.examsubjectwise_details, function (sub_det) {
                                                if (sub_det.ismS_Id == oobj3.ismS_Id) {
                                                    oobj3.eyceS_MarksDisplayFlg = sub_det.eyceS_MarksDisplayFlg;
                                                    oobj3.eyceS_GradeDisplayFlg = sub_det.eyceS_GradeDisplayFlg;
                                                }
                                            });

                                            ccount1 += 1;
                                            if (oobj3.estmpS_PassFailFlg != 'AB' && oobj3.estmpS_PassFailFlg != 'L' && oobj3.estmpS_PassFailFlg != 'M') {
                                                oobj3.hema_estmpS_ObtainedMarks = oobj3.estmpS_ObtainedMarks;
                                                oobj3.hema_estmpS_ObtainedGrade = oobj3.estmpS_ObtainedGrade;
                                            }
                                            else if (oobj3.estmpS_PassFailFlg == 'AB' || oobj3.estmpS_PassFailFlg == 'L' || oobj3.estmpS_PassFailFlg == 'M') {
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
                                })
                                oobj.sub_list_e = $scope.tmparrry1;
                            }
                        });
                    });
                });
            }
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableIds) {
            var exportHref = Excel.tableToExcel(tableIds, $scope.reportname);
            var excelname = $scope.reportname + ".xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
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

        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
    }
})();