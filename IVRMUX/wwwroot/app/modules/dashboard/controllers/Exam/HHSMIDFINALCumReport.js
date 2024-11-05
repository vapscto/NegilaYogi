(function () {
    'use strict';
    angular.module('app').controller('HHSMIDFINALCumReportController', HHSMIDFINALCumReportController)
    HHSMIDFINALCumReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function HHSMIDFINALCumReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.printbtn = false;
        $scope.exclbtn = false;
        $scope.colarraytmp = [];
        $scope.repoershow = false;
        $scope.font_size = 'font25';
        $scope.size = '10px';
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
        $scope.sectionrank = true;
        $scope.displayresult = true;

        $scope.fonts = [
            { name: '10px', size: '10px', class: 'font10' },
            { name: '11px', size: '11px', class: 'font11' },
            { name: '12px', size: '12px', class: 'font12' },
            { name: '13px', size: '13px', class: 'font13' },
            { name: '14px', size: '14px', class: 'font14' },
            { name: '15px', size: '15px', class: 'font15' },
            { name: '16px', size: '16px', class: 'font16' },
            { name: '17px', size: '17px', class: 'font17' },
            { name: '18px', size: '18px', class: 'font18' },
            { name: '25px', size: '25px', class: 'font25' }
        ];

        $scope.BindData = function () {
            apiService.getDATA("HHSMIDFINALCumReport/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
                $scope.clslist = promise.classlist;
                $scope.seclist = promise.seclist;
                $scope.amstlt = promise.amstlist;
                $scope.exsplt = promise.exmstdlist;
            });
        };

        $scope.get_classes = function (ASMAY_Id) {
            $scope.repoershow = false;
            $scope.print = true;
            $scope.searchchkbx = "";
            $scope.studentlistdetails = [];
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
                if (promise.classlist === null || promise.classlist === "") {
                    swal("Classes are Not Mapped To Selected Academic Year!!!");
                }
            });
        };

        $scope.get_cls_sections = function (cls_id) {
            $scope.repoershow = false;
            $scope.print = true;
            $scope.searchchkbx = "";
            $scope.studentlistdetails = [];
            if ($scope.asmaY_Id !== "" && $scope.asmaY_Id !== undefined && $scope.asmaY_Id !== null) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id
                };

                apiService.create("ExamCalculation_SSSE/get_cls_sections", data).then(function (promise) {
                    $scope.seclist = promise.seclist;
                    $scope.asmS_Id = "";
                    $scope.emE_Id = "";
                    $scope.exsplt = [];
                    if (promise.seclist === null || promise.seclist === "") {
                        swal("Sections are Not Mapped To Selected Class!!!");
                    }
                });
            }
            else {
                swal("Please Select Academic Year  First !!!");
                $scope.asmcL_Id = "";
            }
        };

        $scope.get_exams = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
            $scope.repoershow = false;
            $scope.print = true;
            $scope.searchchkbx = "";
            $scope.studentlistdetails = [];

            if (ASMAY_Id !== null && ASMAY_Id !== undefined && ASMCL_Id !== null && ASMCL_Id !== undefined) {
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
                    if (promise.exmstdlist === null || promise.exmstdlist === "") {
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
            $scope.exmrank = [];
            $scope.printbtn = false;
            $scope.exclbtn = false;
            $scope.repoershow = false;
            $scope.electivestd = [];
            $scope.electivesub = [];
            $scope.submitted = true;
            $scope.colarrayall = [];
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
                    "AMST_Ids": $scope.selectedamstids
                };

                apiService.create("HHSMIDFINALCumReport/savedetails", data).then(function (promise) {
                    $scope.masterinst = promise.configuration;
                    var count = 0;
                    $scope.colarrayall = [{ title: "SLNO", template: "<span class='row-number'></span>", width: 100 },
                    { name: 'amsT_FirstName', field: 'amsT_FirstName', title: 'Student Name', width: 200 }];

                    if ($scope.masterinst !== null && $scope.masterinst.length > 0) {
                        if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay === true) {
                            $scope.colarrayall.push({ name: 'amsT_RegistrationNo', field: 'amsT_RegistrationNo', title: 'Reg.NO', width: 100 });
                            $scope.regno = true;
                            count = count + 1;
                        }
                        if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay === true) {
                            $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });
                            $scope.admno = true;
                            count = count + 1;
                        }
                        if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay === true) {
                            $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });
                            $scope.rollno = true;
                            count = count + 1;
                        }
                        if (count === 0) {
                            $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });
                            $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });
                            $scope.admno = true;
                            $scope.rollno = true;
                        }
                    } else {
                        $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });
                        $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });
                        $scope.admno = true;
                        $scope.rollno = true;
                    }

                    angular.forEach($scope.clslist, function (itm) {
                        if (parseInt(itm.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                            $scope.cla = itm.asmcL_ClassName;
                        }
                    });
                    angular.forEach($scope.yearlt, function (itm) {
                        if (parseInt(itm.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                            $scope.yr = itm.asmaY_Year;
                        }
                    });
                    angular.forEach($scope.seclist, function (itm) {
                        if (parseInt(itm.asmS_Id) === parseInt($scope.asmS_Id)) {
                            $scope.sec = itm.asmC_SectionName;
                        }
                    });
                    angular.forEach($scope.exsplt, function (itm) {
                        if (parseInt(itm.emE_Id) === parseInt($scope.emE_Id)) {
                            $scope.exmmid = itm.emE_ExamName;
                        }
                    });

                    $scope.instname = promise.instname;
                    $scope.inst_name = $scope.instname !== null && $scope.instname.length > 0 ? $scope.instname[0].mI_Name : "";
                    $scope.add = $scope.instname !== null && $scope.instname.length > 0 ? $scope.instname[0].mI_Address1 : "";
                    $scope.city = $scope.instname !== null && $scope.instname.length > 0 ? $scope.instname[0].ivrmmcT_Name : "";
                    $scope.pin = $scope.instname !== null && $scope.instname.length > 0 ? $scope.instname[0].mI_Pincode : "";

                    if (promise.savelist !== null && promise.savelist.length > 0) {
                        $scope.repoershow = true;
                        $scope.printbtn = true;
                        $scope.exclbtn = true;
                        $scope.studlist = promise.studlist;
                        $scope.savelist = promise.savelist;
                        $scope.subwithsubexm = promise.subwithsubexm;
                        console.log($scope.subwithsubexm);
                        $scope.electivemarks = promise.electivemarks;
                        $scope.electivestd = promise.savenonsubjlist;
                        $scope.electivesub = promise.nonsubjlist;
                        $scope.exmrank = promise.exmrank;
                        console.log($scope.exmrank);

                        $scope.details = [];
                        angular.forEach($scope.studlist, function (std) {
                            var mrklist = [];
                            var ellist = [];
                            angular.forEach($scope.subwithsubexm, function (exm) {
                                if (exm.eyceS_AplResultFlg === true) {
                                    var obtn = '';
                                    var max = '';
                                    var fff = '';
                                    var cnttt = 0;
                                    var obtngrade = '';
                                    angular.forEach($scope.savelist, function (mrk) {
                                        if (parseInt(mrk.amsT_Id) === parseInt(std.amsT_Id) && parseInt(exm.ismS_Id) === parseInt(mrk.ismS_Id)
                                            && parseInt(exm.emsE_Id) === parseInt(mrk.emsE_Id)) {
                                            cnttt += 1;
                                            obtn = mrk.estmpS_ObtainedMarks;
                                            max = mrk.estmpS_MaxMarks;
                                            fff = mrk.estmpS_PassFailFlg;
                                            obtngrade = mrk.estmpS_ObtainedGrade;
                                        }
                                    });

                                    if (cnttt > 0) {
                                        mrklist.push({
                                            ismS_Id: exm.ismS_Id, emsE_Id: exm.emsE_Id, emsE_SubExamName: exm.emsE_SubExamName,
                                            estmpS_ObtainedMarks: obtn, estmpS_MaxMarks: max, estmpS_PassFailFlg: fff, flag: true,
                                            eyceS_GradeDisplayFlg: exm.eyceS_GradeDisplayFlg, eyceS_MarksDisplayFlg: exm.eyceS_MarksDisplayFlg,
                                            estmpS_ObtainedGrade: obtngrade, eycessS_MarksFlg: exm.eycessS_MarksFlg, eycessS_GradesFlg: exm.eycessS_GradesFlg
                                        });
                                    }

                                    if (cnttt === 0) {
                                        mrklist.push({
                                            ismS_Id: exm.ismS_Id, emsE_Id: exm.emsE_Id, emsE_SubExamName: exm.emsE_SubExamName,
                                            estmpS_ObtainedMarks: obtn, estmpS_MaxMarks: max, estmpS_PassFailFlg: fff, flag: true,
                                            eyceS_GradeDisplayFlg: exm.eyceS_GradeDisplayFlg, eyceS_MarksDisplayFlg: exm.eyceS_MarksDisplayFlg,
                                            estmpS_ObtainedGrade: obtngrade, eycessS_MarksFlg: exm.eycessS_MarksFlg, eycessS_GradesFlg: exm.eycessS_GradesFlg
                                        });
                                    }
                                }
                            });
                            var bb = 0;
                            angular.forEach($scope.subwithsubexm, function (exm) {
                                if (exm.eyceS_AplResultFlg === false) {

                                    var obtn = '';
                                    var max = '';
                                    var fff = '';
                                    var cnttt = 0;
                                    var obtngrade = '';

                                    angular.forEach($scope.savelist, function (mrk) {
                                        if (parseInt(mrk.amsT_Id) === parseInt(std.amsT_Id) && parseInt(exm.ismS_Id) === parseInt(mrk.ismS_Id)
                                            && parseInt(exm.emsE_Id) === parseInt(mrk.emsE_Id)) {
                                            cnttt += 1;
                                            obtn = mrk.estmpS_ObtainedMarks;
                                            max = mrk.estmpS_MaxMarks;
                                            fff = mrk.estmpS_PassFailFlg;
                                            obtngrade = mrk.estmpS_ObtainedGrade;
                                        }
                                    });

                                    if (cnttt > 0) {
                                        mrklist.push({
                                            ismS_Id: exm.ismS_Id, emsE_Id: exm.emsE_Id, emsE_SubExamName: exm.emsE_SubExamName,
                                            estmpS_ObtainedMarks: obtn, estmpS_MaxMarks: max, estmpS_PassFailFlg: fff, flag: true,
                                            eyceS_GradeDisplayFlg: exm.eyceS_GradeDisplayFlg, eyceS_MarksDisplayFlg: exm.eyceS_MarksDisplayFlg,
                                            estmpS_ObtainedGrade: obtngrade, eycessS_MarksFlg: exm.eycessS_MarksFlg, eycessS_GradesFlg: exm.eycessS_GradesFlg
                                        });
                                    }

                                    if (cnttt === 0) {
                                        mrklist.push({
                                            ismS_Id: exm.ismS_Id, emsE_Id: exm.emsE_Id, emsE_SubExamName: exm.emsE_SubExamName,
                                            estmpS_ObtainedMarks: obtn, estmpS_MaxMarks: max, estmpS_PassFailFlg: fff, flag: true,
                                            eyceS_GradeDisplayFlg: exm.eyceS_GradeDisplayFlg, eyceS_MarksDisplayFlg: exm.eyceS_MarksDisplayFlg,
                                            estmpS_ObtainedGrade: obtngrade, eycessS_MarksFlg: exm.eycessS_MarksFlg, eycessS_GradesFlg: exm.eycessS_GradesFlg
                                        });
                                    }
                                }
                            });
                            var estmP_TotalMaxMarks = '';
                            var estmP_TotalObtMarks = '';
                            var estmP_Percentage = '';
                            var estmP_SectionRank = '';
                            var estmP_Result = '';
                            var estmP_TotalGrade = '';

                            if ($scope.exmrank.length === 0) {
                                $scope.details.push({
                                    amsT_Id: std.amsT_Id, amsT_FirstName: std.amsT_FirstName, amsT_AdmNo: std.amsT_AdmNo,
                                    amaY_RollNo: std.amaY_RollNo, amsT_RegistrationNo: std.amsT_RegistrationNo,
                                    stmrklist: mrklist, elmrklist: ellist, estmP_TotalObtMarks: estmP_TotalObtMarks, estmP_TotalMaxMarks: estmP_TotalMaxMarks, estmP_Percentage: estmP_Percentage, estmP_SectionRank: estmP_SectionRank, estmP_Result: estmP_Result, estmP_TotalGrade: estmP_TotalGrade
                                });
                            }

                            var cmm = 0;
                            angular.forEach($scope.exmrank, function (rr) {
                                if (parseInt(rr.amsT_Id) === parseInt(std.amsT_Id)) {
                                    cmm += 1;
                                    $scope.details.push({
                                        amsT_Id: rr.amsT_Id, amsT_FirstName: std.amsT_FirstName, amsT_AdmNo: std.amsT_AdmNo,
                                        amaY_RollNo: std.amaY_RollNo, amsT_RegistrationNo: std.amsT_RegistrationNo,
                                        stmrklist: mrklist, elmrklist: ellist, estmP_TotalObtMarks: rr.estmP_TotalObtMarks, estmP_TotalMaxMarks: rr.estmP_TotalMaxMarks, estmP_Percentage: rr.estmP_Percentage, estmP_SectionRank: rr.estmP_SectionRank, estmP_Result: rr.estmP_Result, estmP_TotalGrade: rr.estmP_TotalGrade
                                    });
                                }
                            });
                            if (cmm === 0) {
                                $scope.details.push({
                                    amsT_Id: std.amsT_Id, amsT_FirstName: std.amsT_FirstName, amsT_AdmNo: std.amsT_AdmNo,
                                    amaY_RollNo: std.amaY_RollNo, amsT_RegistrationNo: std.amsT_RegistrationNo,
                                    stmrklist: mrklist, elmrklist: ellist, estmP_TotalObtMarks: estmP_TotalObtMarks, estmP_TotalMaxMarks: estmP_TotalMaxMarks, estmP_Percentage: estmP_Percentage, estmP_SectionRank: estmP_SectionRank, estmP_Result: estmP_Result, estmP_TotalGrade: estmP_TotalGrade
                                });
                            }
                        });

                        //getting all subjects
                        $scope.mainsubjects = [];
                        angular.forEach($scope.subwithsubexm, function (stu2) {
                            if ($scope.mainsubjects.length === 0) {
                                $scope.mainsubjects.push({ ismS_Id: stu2.ismS_Id, ismS_SubjectName: stu2.ismS_SubjectName, eyceS_AplResultFlg: stu2.eyceS_AplResultFlg, eyceS_MarksDisplayFlg: stu2.eyceS_MarksDisplayFlg, eyceS_GradeDisplayFlg: stu2.eyceS_GradeDisplayFlg });
                            }
                            else if ($scope.mainsubjects.length > 0) {
                                var al_ct = 0;
                                angular.forEach($scope.mainsubjects, function (uf) {
                                    if (parseInt(uf.ismS_Id) === parseInt(stu2.ismS_Id)) {
                                        al_ct += 1;
                                    }
                                });
                                if (al_ct === 0) {
                                    $scope.mainsubjects.push({ ismS_Id: stu2.ismS_Id, ismS_SubjectName: stu2.ismS_SubjectName, eyceS_AplResultFlg: stu2.eyceS_AplResultFlg, eyceS_MarksDisplayFlg: stu2.eyceS_MarksDisplayFlg, eyceS_GradeDisplayFlg: stu2.eyceS_GradeDisplayFlg });
                                }
                            }
                        });
                        //getting sub exam for all subjects
                        $scope.headersub = [];
                        angular.forEach($scope.mainsubjects, function (s1) {
                            var temp1 = [];
                            angular.forEach($scope.subwithsubexm, function (e1) {
                                if (parseInt(s1.ismS_Id) === parseInt(e1.ismS_Id)) {
                                    temp1.push({ emsE_Id: e1.emsE_Id, emsE_SubExamName: e1.emsE_SubExamName, ismS_Id: e1.ismS_Id, ismS_SubjectName: e1.ismS_SubjectName, eycessS_MarksFlg: e1.eycessS_MarksFlg, eycessS_GradesFlg: e1.eycessS_GradesFlg });
                                }
                            });
                            $scope.headersub.push({ ismS_Id: s1.ismS_Id, ismS_SubjectName: s1.ismS_SubjectName, eyceS_AplResultFlg: s1.eyceS_AplResultFlg, eyceS_MarksDisplayFlg: s1.eyceS_MarksDisplayFlg, eyceS_GradeDisplayFlg: s1.eyceS_GradeDisplayFlg, subexmlist: temp1 });
                        });

                        $scope.colarraytmp = [];
                        //kendogrid start
                        angular.forEach($scope.headersub, function (obj1) {
                            if (obj1.eyceS_AplResultFlg === true) {
                                //obj.title = obj1.ismS_SubjectName;
                                $scope.colarraytmp.push({ title: obj1.ismS_SubjectName, field: obj1.ismS_Id, idd: obj1.ismS_Id, marksflg: obj1.eyceS_MarksDisplayFlg, grdflag: obj1.eyceS_GradeDisplayFlg });
                            }
                        });

                        angular.forEach($scope.colarraytmp, function (qwe) {
                            $scope.nwtmpary = [];
                            angular.forEach($scope.subwithsubexm, function (obj2) {
                                if (obj2.eyceS_AplResultFlg === true && qwe.title === obj2.ismS_SubjectName) {
                                    //if (qwe.title === obj2.ismS_SubjectName) {

                                    var subexamname = obj2.emsE_SubExamName;

                                    subexamname = subexamname.replace(".", "");
                                    subexamname = subexamname.replace("&", "");
                                    subexamname = subexamname.replace(" ", "");
                                    subexamname = subexamname.replace("/", "");
                                    subexamname = subexamname.replace("-", "");
                                    subexamname = subexamname.replace(" ", "");
                                    subexamname = subexamname.replace("(", "");
                                    subexamname = subexamname.replace(")", "");

                                    $scope.nwtmpary.push({
                                        title: obj2.emsE_SubExamName, field: "unique" + qwe.idd + obj2.emsE_Id, width: 200,
                                        submarksflag: obj2.eycessS_MarksFlg, subgradeflag: obj2.eycessS_GradesFlg
                                    });
                                }
                            });
                            qwe.columns = $scope.nwtmpary;
                        });

                        $scope.grade = [
                            { id: 1, name: 'M' },
                            { id: 2, name: 'G' }
                        ];

                        angular.forEach($scope.colarraytmp, function (cols) {
                            angular.forEach(cols.columns, function (cobs) {
                                $scope.marksgrade = [];
                                angular.forEach($scope.grade, function (grd) {
                                    //  if (cobs.title === "Total") {
                                    if (cobs.submarksflag === true) {
                                        if (grd.name === 'M') {
                                            $scope.marksgrade.push({
                                                title: grd.name, field: cobs.field + grd.name, width: 100
                                            });
                                        }
                                    }
                                    if (cobs.subgradeflag === true) {
                                        if (grd.name === 'G') {
                                            $scope.marksgrade.push({
                                                title: grd.name, field: cobs.field + grd.name, width: 100
                                            });
                                        }
                                    }

                                    if (cobs.submarksflag === false && cobs.subgradeflag === false) {
                                        if (grd.name === 'M') {
                                            $scope.marksgrade.push({
                                                title: grd.name, field: cobs.field + grd.name, width: 100
                                            });
                                        }
                                    }
                                    cobs.columns = $scope.marksgrade;
                                });
                            });
                        });

                        angular.forEach($scope.colarraytmp, function (qwe) {
                            $scope.colarrayall.push(qwe);
                        });

                        $scope.colarrayall.push({ title: 'Over All Total', name: 'estmP_TotalObtMarks', field: 'estmP_TotalObtMarks', width: 100 },
                            { title: 'Over All Grade', name: 'estmP_TotalGrade', field: 'estmP_TotalGrade', width: 100 },
                            { title: 'Max.Total', name: 'estmP_TotalMaxMarks', field: 'estmP_TotalMaxMarks', width: 100 },
                            { title: 'PER.(%)', name: 'estmP_Percentage', field: 'estmP_Percentage', width: 100 })

                        if ($scope.sectionrank === true) {
                            $scope.colarrayall.push({ title: 'Rank', name: 'estmP_SectionRank', field: 'estmP_SectionRank', width: 100 })
                        }
                        if ($scope.displayresult === true) {
                            $scope.colarrayall.push({ title: 'PASS/FAIL', name: 'estmP_Result', field: 'estmP_Result', width: 100 })
                        }

                        $scope.colarraytmpapplyresult = [];

                        angular.forEach($scope.headersub, function (obj1) {
                            if (obj1.eyceS_AplResultFlg === false) {
                                //obj.title = obj1.ismS_SubjectName;
                                $scope.colarraytmpapplyresult.push({ title: obj1.ismS_SubjectName, field: obj1.ismS_Id, idd: obj1.ismS_Id, marksflg: obj1.eyceS_MarksDisplayFlg, grdflag: obj1.eyceS_GradeDisplayFlg });
                            }
                        });

                        angular.forEach($scope.colarraytmpapplyresult, function (qwe) {
                            $scope.nwtmpary = [];
                            angular.forEach($scope.subwithsubexm, function (obj2) {
                                if (obj2.eyceS_AplResultFlg === false && qwe.title === obj2.ismS_SubjectName) {
                                    var subexamname = obj2.emsE_SubExamName;
                                    subexamname = subexamname.replace(".", "");
                                    subexamname = subexamname.replace("&", "");
                                    subexamname = subexamname.replace(" ", "");
                                    subexamname = subexamname.replace("/", "");
                                    subexamname = subexamname.replace("-", "");
                                    subexamname = subexamname.replace(" ", "");
                                    subexamname = subexamname.replace("(", "");
                                    subexamname = subexamname.replace(")", "");
                                    $scope.nwtmpary.push({
                                        title: obj2.emsE_SubExamName, field: "unique" + qwe.idd + obj2.emsE_Id, width: 200,
                                        submarksflag: obj2.eycessS_MarksFlg, subgradeflag: obj2.eycessS_GradesFlg
                                    });
                                }
                            });
                            qwe.columns = $scope.nwtmpary;
                        });

                        angular.forEach($scope.colarraytmpapplyresult, function (cols) {
                            angular.forEach(cols.columns, function (cobs) {
                                $scope.marksgrade = [];
                                angular.forEach($scope.grade, function (grd) {
                                    //  if (cobs.title === "Total") {
                                    if (cobs.submarksflag === true) {
                                        if (grd.name === 'M') {
                                            $scope.marksgrade.push({
                                                title: grd.name, field: cobs.field + grd.name, width: 100
                                            });
                                        }
                                    }
                                    if (cobs.subgradeflag === true) {
                                        if (grd.name === 'G') {
                                            $scope.marksgrade.push({
                                                title: grd.name, field: cobs.field + grd.name, width: 100
                                            });
                                        }
                                    }

                                    if (cobs.submarksflag === false && cobs.subgradeflag === false) {
                                        if (grd.name === 'M') {
                                            $scope.marksgrade.push({
                                                title: grd.name, field: cobs.field + grd.name, width: 100
                                            });
                                        }
                                    }
                                    cobs.columns = $scope.marksgrade;
                                });
                            });
                        });

                        angular.forEach($scope.colarraytmpapplyresult, function (dd) {
                            $scope.colarrayall.push(dd);
                        });

                        var abc = 'M';
                        var abc1 = 'G';

                        angular.forEach($scope.details, function (qqq) {
                            angular.forEach(qqq.stmrklist, function (qqq1) {
                                var subexamname1 = qqq1.emsE_SubExamName;
                                subexamname1 = subexamname1.replace(" ", "");
                                subexamname1 = subexamname1.replace(".", "");
                                subexamname1 = subexamname1.replace("&", "");
                                subexamname1 = subexamname1.replace(" ", "");
                                subexamname1 = subexamname1.replace("/", "");
                                subexamname1 = subexamname1.replace("-", "");
                                subexamname1 = subexamname1.replace("(", "");
                                subexamname1 = subexamname1.replace(")", "");

                                var subexamname2 = qqq1.emsE_SubExamName;
                                subexamname2 = subexamname2.replace("/", "");
                                subexamname2 = subexamname2.replace("-", "");
                                subexamname2 = subexamname2.replace(".", "");
                                subexamname2 = subexamname2.replace("&", "");
                                subexamname2 = subexamname2.replace(" ", "");
                                subexamname2 = subexamname2.replace(" ", "");
                                subexamname2 = subexamname2.replace("(", "");
                                subexamname2 = subexamname2.replace(")", "");

                                var s = "unique" + qqq1.ismS_Id + qqq1.emsE_Id;
                                var s1 = "unique" + qqq1.ismS_Id + qqq1.emsE_Id;

                                if ((qqq1.estmpS_PassFailFlg === 'AB' || qqq1.estmpS_PassFailFlg === 'M' || qqq1.estmpS_PassFailFlg === 'L')
                                    && qqq1.emsE_SubExamName !== 'Total') {
                                    if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                        if (abc === 'M') {
                                            s = s + 'M';
                                            qqq[s] = qqq1.estmpS_PassFailFlg;
                                        }
                                    }
                                    else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {
                                        if (abc1 === 'G') {
                                            s1 = s1 + 'G';
                                            qqq[s1] = qqq1.estmpS_PassFailFlg;
                                        }
                                    }
                                    else if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === true) {
                                        if (abc === 'M') {
                                            s = s + 'M';
                                            qqq[s] = qqq1.estmpS_PassFailFlg;
                                        }
                                        if (abc1 === 'G') {
                                            s1 = s1 + 'G';
                                            qqq[s1] = qqq1.estmpS_PassFailFlg;
                                        }
                                    }
                                }

                                else if ((qqq1.estmpS_PassFailFlg === 'AB' || qqq1.estmpS_PassFailFlg === 'M' || qqq1.estmpS_PassFailFlg === 'L')
                                    && qqq1.emsE_SubExamName === 'Total') {
                                    if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                        if (abc === 'M') {
                                            s = s + 'M';
                                            qqq[s] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedMarks + ")";
                                        }
                                    }
                                    else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {
                                        if (abc1 === 'G') {
                                            s1 = s1 + 'G';
                                            qqq[s1] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedGrade + ")";
                                        }
                                    }
                                    else if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === true) {
                                        if (abc === 'M') {
                                            s = s + 'M';
                                            qqq[s] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedMarks + ")";
                                        }
                                        if (abc1 === 'G') {
                                            s1 = s1 + 'G';
                                            qqq[s1] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedGrade + ")";
                                        }
                                    }
                                }

                                else if (qqq1.estmpS_PassFailFlg !== 'AB' && qqq1.estmpS_PassFailFlg !== 'M' && qqq1.estmpS_PassFailFlg !== 'L') {

                                    if (qqq1.estmpS_PassFailFlg === 'Fail') {

                                        if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                            if (abc === 'M') {
                                                s = s + 'M';
                                                qqq[s] = qqq1.estmpS_ObtainedMarks + " * ";
                                            }
                                            //if (abc1 == 'G') {
                                            //    s1 = s1 + 'G';
                                            //    qqq[s1] = qqq1.estmpS_ObtainedGrade + " * ";
                                            //}
                                        } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {
                                            //if (abc == 'M') {
                                            //    s = s + 'M';
                                            //    qqq[s] = qqq1.estmpS_ObtainedGrade + " * ";
                                            //}
                                            if (abc1 === 'G') {
                                                s1 = s1 + 'G';
                                                qqq[s1] = qqq1.estmpS_ObtainedGrade + " * ";
                                            }

                                        } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === false) {
                                            //if (abc == 'M') {
                                            //    s = s + 'M';
                                            //    qqq[s] = qqq1.estmpS_ObtainedMarks + " * ";
                                            //}
                                            //if (abc1 == 'G') {
                                            //    s1 = s1 + 'G';
                                            //    qqq[s1] = qqq1.estmpS_ObtainedMarks + " * ";
                                            //}
                                        } else {
                                            if (qqq1.estmpS_ObtainedMarks !== "" && qqq1.estmpS_ObtainedGrade !== "") {
                                                if (abc === 'M') {
                                                    s = s + 'M';
                                                    //qqq[s] = qqq1.estmpS_ObtainedMarks + " " + " | " + " " + qqq1.estmpS_ObtainedGrade + " * ";
                                                    qqq[s] = qqq1.estmpS_ObtainedMarks + " * ";
                                                }
                                                if (abc1 === 'G') {
                                                    s1 = s1 + 'G';
                                                    // qqq[s1] = qqq1.estmpS_ObtainedMarks + " " + " | " + " " + qqq1.estmpS_ObtainedGrade + " * ";
                                                    qqq[s1] = qqq1.estmpS_ObtainedGrade + " * ";
                                                }

                                            }
                                            //qqq[s] = qqq1.estmpS_ObtainedMarks + '|' +  qqq1.estmpS_ObtainedGrade + "*";
                                        }
                                    }

                                    if (qqq1.estmpS_PassFailFlg !== 'Fail') {

                                        if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                            if (abc === 'M') {
                                                s = s + 'M';
                                                qqq[s] = qqq1.estmpS_ObtainedMarks;
                                            }

                                        } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {

                                            if (abc1 === 'G') {
                                                s1 = s1 + 'G';
                                                qqq[s1] = qqq1.estmpS_ObtainedGrade;
                                            }

                                        } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === false) {
                                            //if (abc == 'M') {
                                            //    s = s + 'M';
                                            //    qqq[s] = qqq1.estmpS_ObtainedMarks;
                                            //}
                                            //if (abc1 == 'G') {
                                            //    s1 = s1 + 'G';
                                            //    qqq[s1] = qqq1.estmpS_ObtainedMarks;
                                            //}
                                        } else {
                                            if (qqq1.estmpS_ObtainedMarks !== "" && qqq1.estmpS_ObtainedGrade !== "") {
                                                if (abc === 'M') {
                                                    s = s + 'M';
                                                    qqq[s] = qqq1.estmpS_ObtainedMarks;
                                                }
                                            }

                                            if (qqq1.estmpS_ObtainedMarks !== "" && qqq1.estmpS_ObtainedGrade !== "") {
                                                if (abc1 === 'G') {
                                                    s1 = s1 + 'G';
                                                    qqq[s1] = qqq1.estmpS_ObtainedGrade;
                                                }
                                            }
                                        }
                                    }
                                }

                                else {
                                    qqq[s] = qqq1.estmpS_ObtainedMarks + "|" + qqq1.estmpS_ObtainedGrade;
                                }
                            });

                            angular.forEach(qqq.elmrklist, function (qqq1) {
                                var subexamname11 = qqq1.ismS_SubjectName;
                                subexamname11 = subexamname11.replace(".", "");
                                subexamname11 = subexamname11.replace("&", "");
                                subexamname11 = subexamname11.replace("/", "");
                                subexamname11 = subexamname11.replace("-", "");
                                subexamname11 = subexamname11.replace(" ", "");
                                subexamname11 = subexamname11.replace(" ", "");
                                subexamname11 = subexamname11.replace("(", "");
                                subexamname11 = subexamname11.replace(")", "");

                                var subexamname21 = qqq1.ismS_SubjectName;
                                subexamname21 = subexamname21.replace(".", "");
                                subexamname21 = subexamname21.replace("&", "");
                                subexamname21 = subexamname21.replace("/", "");
                                subexamname21 = subexamname21.replace("-", "");
                                subexamname21 = subexamname21.replace(" ", "");
                                subexamname21 = subexamname21.replace(" ", "");
                                subexamname21 = subexamname21.replace("(", "");
                                subexamname21 = subexamname21.replace(")", "");

                                var s1 = "unique" + qqq1.ismS_Id + subexamname11;
                                var s = "unique" + qqq1.ismS_Id + subexamname21;


                                if ((qqq1.estmpS_PassFailFlg === 'AB' || qqq1.estmpS_PassFailFlg === 'M' || qqq1.estmpS_PassFailFlg === 'L')
                                    && qqq1.emsE_SubExamName !== 'Total') {
                                    if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                        if (abc === 'M') {
                                            s = s + 'M';
                                            qqq[s] = qqq1.estmpS_PassFailFlg;
                                        }
                                    }
                                    else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {
                                        if (abc1 === 'G') {
                                            s1 = s1 + 'G';
                                            qqq[s1] = qqq1.estmpS_PassFailFlg;
                                        }
                                    }
                                    else if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === true) {
                                        if (abc === 'M') {
                                            s = s + 'M';
                                            qqq[s] = qqq1.estmpS_PassFailFlg;
                                        }
                                        if (abc1 === 'G') {
                                            s1 = s1 + 'G';
                                            qqq[s1] = qqq1.estmpS_PassFailFlg;
                                        }
                                    }
                                }

                                else if ((qqq1.estmpS_PassFailFlg === 'AB' || qqq1.estmpS_PassFailFlg === 'M' || qqq1.estmpS_PassFailFlg === 'L') && qqq1.emsE_SubExamName === 'Total') {
                                    if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                        if (abc === 'M') {
                                            s = s + 'M';
                                            qqq[s] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedMarks + ")";
                                        }
                                    }
                                    else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {
                                        if (abc1 === 'G') {
                                            s1 = s1 + 'G';
                                            qqq[s1] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedGrade + ")";
                                        }
                                    }
                                    else if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === true) {
                                        if (abc === 'M') {
                                            s = s + 'M';
                                            qqq[s] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedMarks + ")";
                                        }
                                        if (abc1 === 'G') {
                                            s1 = s1 + 'G';
                                            qqq[s1] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedGrade + ")";
                                        }
                                    }
                                }

                                else if (qqq1.estmpS_PassFailFlg !== 'AB' && qqq1.estmpS_PassFailFlg !== 'M' && qqq1.estmpS_PassFailFlg !== 'L') {

                                    if (qqq1.estmpS_PassFailFlg === 'Fail') {

                                        if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                            if (abc === 'M') {
                                                s = s + 'M';
                                                qqq[s] = qqq1.estmpS_ObtainedMarks + " * ";
                                            }
                                            //if (abc1 == 'G') {
                                            //    s1 = s1 + 'G';
                                            //    qqq[s1] = qqq1.estmpS_ObtainedGrade + " * ";
                                            //}
                                        } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {
                                            //if (abc == 'M') {
                                            //    s = s + 'M';
                                            //    qqq[s] = qqq1.estmpS_ObtainedGrade + " * ";
                                            //}
                                            if (abc1 === 'G') {
                                                s1 = s1 + 'G';
                                                qqq[s1] = qqq1.estmpS_ObtainedGrade + " * ";
                                            }

                                        } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === false) {
                                            //if (abc == 'M') {
                                            //    s = s + 'M';
                                            //    qqq[s] = qqq1.estmpS_ObtainedMarks + " * ";
                                            //}
                                            //if (abc1 == 'G') {
                                            //    s1 = s1 + 'G';
                                            //    qqq[s1] = qqq1.estmpS_ObtainedMarks + " * ";
                                            //}
                                        } else {
                                            if (qqq1.estmpS_ObtainedMarks !== "" && qqq1.estmpS_ObtainedGrade !== "") {
                                                if (abc === 'M') {
                                                    s = s + 'M';
                                                    //qqq[s] = qqq1.estmpS_ObtainedMarks + " " + " | " + " " + qqq1.estmpS_ObtainedGrade + " * ";
                                                    qqq[s] = qqq1.estmpS_ObtainedMarks + " * ";
                                                }
                                                if (abc1 === 'G') {
                                                    s1 = s1 + 'G';
                                                    // qqq[s1] = qqq1.estmpS_ObtainedMarks + " " + " | " + " " + qqq1.estmpS_ObtainedGrade + " * ";
                                                    qqq[s1] = qqq1.estmpS_ObtainedGrade + " * ";
                                                }

                                            }
                                            //qqq[s] = qqq1.estmpS_ObtainedMarks + '|' +  qqq1.estmpS_ObtainedGrade + "*";
                                        }
                                    }

                                    if (qqq1.estmpS_PassFailFlg !== 'Fail') {

                                        if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                            if (abc === 'M') {
                                                s = s + 'M';
                                                qqq[s] = qqq1.estmpS_ObtainedMarks;
                                            }

                                        } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {

                                            if (abc1 === 'G') {
                                                s1 = s1 + 'G';
                                                qqq[s1] = qqq1.estmpS_ObtainedGrade;
                                            }

                                        } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === false) {
                                            //if (abc == 'M') {
                                            //    s = s + 'M';
                                            //    qqq[s] = qqq1.estmpS_ObtainedMarks;
                                            //}
                                            //if (abc1 == 'G') {
                                            //    s1 = s1 + 'G';
                                            //    qqq[s1] = qqq1.estmpS_ObtainedMarks;
                                            //}
                                        } else {
                                            if (qqq1.estmpS_ObtainedMarks !== "" && qqq1.estmpS_ObtainedGrade !== "") {
                                                if (abc === 'M') {
                                                    s = s + 'M';
                                                    qqq[s] = qqq1.estmpS_ObtainedMarks;
                                                }
                                            }

                                            if (qqq1.estmpS_ObtainedMarks !== "" && qqq1.estmpS_ObtainedGrade !== "") {
                                                if (abc1 === 'G') {
                                                    s1 = s1 + 'G';
                                                    qqq[s1] = qqq1.estmpS_ObtainedGrade;
                                                }
                                            }
                                        }
                                    }
                                }

                                else {
                                    qqq[s] = qqq1.estmpS_ObtainedMarks + "|" + qqq1.estmpS_ObtainedGrade;
                                }
                            });
                        });

                        console.log($scope.details);
                        console.log("dfsd");
                        console.log($scope.colarrayall);

                        $(document).ready(function () {
                            $('#gridhhs').empty();
                            $("#gridhhs").kendoGrid({
                                toolbar: ["excel"],
                                excel: {
                                    fileName: "inddExport.xlsx",
                                    //allPages: true,
                                    proxyURL: "",
                                    filterable: true,
                                    allPages: true
                                },

                                dataSource: {
                                    data: $scope.details,
                                    pageSize: 100
                                },

                                sortable: true,
                                pageable: true,
                                groupable: false,
                                filterable: true,
                                columnMenu: true,
                                reorderable: true,
                                resizable: true,

                                columns: $scope.colarrayall,
                                dataBound: function () {
                                    var pagenum = this.dataSource.page();
                                    var pageitms = this.dataSource.pageSize();
                                    var rows = this.items();
                                    $(rows).each(function () {
                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                        var rowLabel = $(this).find(".row-number");
                                        $(rowLabel).html(index);
                                    });
                                }
                            });
                        });
                        //kendigrid end
                    }
                    else if (promise.savelist === null || promise.savelist.length === 0) {
                        swal('No record Found');
                    }
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
            //$scope.gridview2 = true;                
            if (obj.abc === true) {
                angular.forEach($scope.subjectlt1, function (itm1) {
                    if (parseInt(itm1.amsT_Id) === parseInt(user.amsT_Id)) {
                        angular.forEach($scope.subjectlt, function (itm2) {
                            if (parseInt(itm2.ismS_Id) === parseInt(column.ismS_Id)) {
                                itm1.sub_list.push({ id: itm2.ismS_Id, name: itm2.ismS_SubjectName });
                            }
                        });
                    }
                });
            }
            else {
                angular.forEach($scope.subjectlt1, function (itm1) {
                    if (parseInt(itm1.amsT_Id) === parseInt(user.amsT_Id)) {
                        for (var i = 0; i < itm1.sub_list.length; i++) {
                            if (parseInt(itm1.sub_list[i].id) === parseInt(column.ismS_Id)) {
                                itm1.sub_list.splice(i, 1);
                            }
                        }
                    }
                });
            }
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