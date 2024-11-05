﻿
(function () {
    'use strict';
    angular.module('app').controller('VikasaInterProgressReportExamController', VikasaInterProgressReportExamController)
    VikasaInterProgressReportExamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function VikasaInterProgressReportExamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        $scope.amsT_Date = new Date();        

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("VikasaAssessment2Report/Getdetails/", pageid).then(function (promise) {
                $scope.yearlt = promise.yearlist;             
            });
        };

        $scope.onselectradio = function () {
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.asmaY_Id = "";
            $scope.temp = [];
            $scope.studentlist = [];
        };


        $scope.get_class = function () {

            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.sectionDropdown = "";
            $scope.exsplt = "";
            $scope.temp = [];
            $scope.studentlist = [];

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("VikasaAssessment2Report/get_class", data).then(function (promise) {
                $scope.classDropdown = promise.classlist;
            });
        };

        $scope.get_section = function () {
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = "";
            $scope.temp = [];
            $scope.studentlist = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("VikasaAssessment2Report/get_section", data).then(function (promise) {
                $scope.sectionDropdown = promise.sectionList;
            });
        };

        $scope.get_Exam = function () {

            $scope.emE_Id = "";
            $scope.temp = [];
            $scope.studentlist = [];

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "radiotype": $scope.radiotype,
            };

            apiService.create("VikasaAssessment2Report/get_exam", data).then(function (promise) {
                $scope.exsplt = promise.examList;
                $scope.studentlist = promise.studentlist;

                $scope.all = true;
                angular.forEach($scope.studentlist, function (dd) {
                    dd.checkedsub = true;
                });
            });
        };


        $scope.OnClickAll = function () {
            angular.forEach($scope.studentlist, function (dd) {
                dd.checkedsub = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.studentlist.every(function (itm) { return itm.checkedsub; });
        };

        $scope.OnChangeExam = function () {
            $scope.temp = [];
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.studentlist.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
       

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.student_temp = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                angular.forEach($scope.studentlist, function (dd) {
                    if (dd.checkedsub) {
                        $scope.student_temp.push({ AMST_Id: dd.amsT_Id });
                    }
                });
                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "student_temp": $scope.student_temp
                };

                var temp_list = [];
                apiService.create("VikasaProgressReportExam/savedetails", data).then(function (promise) {

                    $scope.temp = promise.savelist;
                    if ($scope.temp.length > 0) {

                        $scope.instname = promise.instname;
                        $scope.inst_name = $scope.instname[0].mI_Name;

                        var temp_list = [];
                        angular.forEach($scope.temp, function (stude) {

                            var stu_id = stude.amsT_Id;
                            var stu_subj_list = [];

                            angular.forEach(promise.savelist, function (opq) {

                                if (opq.amsT_Id == stu_id) {

                                    if (opq.estmpS_ObtainedGrade == "A") {
                                        opq.color = 'Green';
                                    } else if (opq.estmpS_ObtainedGrade == "A1") {
                                        opq.color = '#0000A0';
                                    } else if (opq.estmpS_ObtainedGrade == "A2") {
                                        opq.color = '#0000FF';
                                    } else if (opq.estmpS_ObtainedGrade == "B1") {
                                        opq.color = '#006400';
                                    } else if (opq.estmpS_ObtainedGrade == "B2") {
                                        opq.color = '#008000';
                                    } else if (opq.estmpS_ObtainedGrade == "C1") {
                                        opq.color = '#9400D3';
                                    } else if (opq.estmpS_ObtainedGrade == "C2") {
                                        opq.color = '#EE82EE';
                                    } else if (opq.estmpS_ObtainedGrade == "A+") {
                                        opq.color = 'Purple';
                                    } else if (opq.estmpS_ObtainedGrade == "A+") {
                                        opq.color = 'Purple';
                                    } else if (opq.estmpS_ObtainedGrade == "B") {
                                        opq.color = 'forestgreen';
                                    } else if (opq.estmpS_ObtainedGrade == "C") {
                                        opq.color = 'Brown';
                                    } else if (opq.estmpS_ObtainedGrade == "D") {
                                        opq.color = 'Orange';
                                    } else if (opq.estmpS_ObtainedGrade == "E") {
                                        opq.color = 'Red';
                                    } else if (opq.estmpS_ObtainedGrade == "F") {
                                        opq.color = 'Orange';
                                    } else if (opq.estmpS_ObtainedGrade == "G") {
                                        opq.color = 'RosyBrown';
                                    } else if (opq.estmpS_ObtainedGrade == "U") {
                                        opq.color = 'Brown';
                                    } else {
                                        opq.color = 'Black';
                                    }

                                    stu_subj_list.push({
                                        amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, emgR_Id: opq.emgR_Id,
                                        ismS_SubjectName: opq.ismS_SubjectName,
                                        eyceS_MaxMarks: opq.eyceS_MaxMarks, eyceS_MinMarks: opq.eyceS_MinMarks, eyceS_AplResultFlg: opq.eyceS_AplResultFlg,
                                        estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks,
                                        estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, color: opq.color,
                                        estmpS_PassFailFlg: opq.estmpS_PassFailFlg, ESTMPS_ClassAverage: opq.estmpS_ClassAverage,
                                        ESTMPS_SectionAverage: opq.estmpS_SectionAverage, ESTMPS_ClassHighest: opq.estmpS_ClassHighest,
                                        ESTMPS_SectionHighest: opq.estmpS_SectionHighest, estmP_TotalObtMarks: opq.estmP_TotalObtMarks,
                                        estmP_Percentage: opq.estmP_Percentage, estmP_TotalGrade: opq.estmP_TotalGrade, estmP_ClassRank: opq.estmP_ClassRank,
                                        estmP_SectionRank: opq.estmP_SectionRank, estmP_TotalGradeRemark: opq.estmP_TotalGradeRemark,
                                        estmP_TotalMaxMarks: opq.estmP_TotalMaxMarks, estmpS_SectionHighest: opq.estmpS_SectionHighest
                                    });
                                }
                            })

                            if (temp_list.length == 0) {

                                temp_list.push({
                                    student_id: stude.amsT_Id, amsT_FirstName: stude.amsT_FirstName, amsT_DOB: stude.amsT_DOB,
                                    amsT_AdmNo: stude.amsT_AdmNo, amaY_RollNo: stude.amaY_RollNo, asmcL_ClassName: stude.asmcL_ClassName,
                                    asmC_SectionName: stude.asmC_SectionName, estmP_TotalMaxMarks: stude.estmP_TotalMaxMarks,
                                    estmP_TotalObtMarks: stude.estmP_TotalObtMarks, estmP_Percentage: stude.estmP_Percentage,
                                    classheld: stude.classheld, classatt: stude.classatt, estmP_TotalGrade: stude.estmP_TotalGrade,
                                    estmP_TotalObtMarks: stude.estmP_TotalObtMarks, estmP_Percentage: stude.estmP_Percentage,
                                    estmP_TotalGrade: stude.estmP_TotalGrade, estmP_ClassRank: stude.estmP_ClassRank, estmP_SectionRank: stude.estmP_SectionRank,
                                    estmP_TotalGradeRemark: stude.estmP_TotalGradeRemark, estmP_TotalMaxMarks: stude.estmP_TotalMaxMarks, sub_list: stu_subj_list
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
                                        student_id: stude.amsT_Id, amsT_FirstName: stude.amsT_FirstName, amsT_DOB: stude.amsT_DOB,
                                        amsT_AdmNo: stude.amsT_AdmNo, amaY_RollNo: stude.amaY_RollNo, asmcL_ClassName: stude.asmcL_ClassName,
                                        asmC_SectionName: stude.asmC_SectionName, estmP_TotalMaxMarks: stude.estmP_TotalMaxMarks,
                                        estmP_TotalObtMarks: stude.estmP_TotalObtMarks, estmP_Percentage: stude.estmP_Percentage,
                                        classheld: stude.classheld, classatt: stude.classatt, estmP_TotalGrade: stude.estmP_TotalGrade,
                                        estmP_TotalObtMarks: stude.estmP_TotalObtMarks, estmP_Percentage: stude.estmP_Percentage,
                                        estmP_TotalGrade: stude.estmP_TotalGrade, estmP_ClassRank: stude.estmP_ClassRank,
                                        estmP_SectionRank: stude.estmP_SectionRank, estmP_TotalGradeRemark: stude.estmP_TotalGradeRemark,
                                        estmP_TotalMaxMarks: stude.estmP_TotalMaxMarks, sub_list: stu_subj_list
                                    });
                                }
                            }
                        })

                        $scope.exam = promise.savelist[0].emE_ExamName;
                        $scope.exm_sublist = promise.subjlist;
                        $scope.processtot = promise.savelisttot;
                        $scope.subj_grade_remarks = promise.grade_details;

                        if (promise.clstchname.length > 0) {
                            $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                        } else {
                            $scope.clastechname = "";
                        }


                        $scope.report_list = temp_list;
                        console.log($scope.report_list);

                        var stu_subj_list_remaks = [];
                        $scope.remarks = promise.remarks;


                        $scope.stud_work_attendence = promise.work_attendence;
                        $scope.stud_present_attendence = promise.present_attendence;

                        angular.forEach($scope.yearlt, function (dd) {
                            if (dd.asmaY_Id == $scope.asmaY_Id) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.classDropdown, function (dc) {
                            if (dc.asmcL_Id == $scope.asmcL_Id) {
                                $scope.asmcL_ClassName = dc.asmcL_ClassName;
                            }
                        });

                        angular.forEach($scope.sectionDropdown, function (ds) {
                            if (ds.asmS_Id == $scope.asmS_Id) {
                                $scope.asmC_SectionName = ds.asmC_SectionName;
                            }
                        });

                        $scope.issuedate = new Date($scope.amsT_Date);

                        if ($scope.dailybtedates === "format1") {
                            $scope.imaged = "/images/clients/Vikasa/HeaderSawyerpuram.jpg";
                            $scope.imagedd = "images/clients/Vikasa/images/pencils.png";
                            $scope.grade = "/images/clients/Vikasa/grade.png";
                            $scope.style = "width: 60%; height: 90px;";
                            $scope.schooladdress = "";
                        } else {
                            $scope.imaged = "/images/clients/Vikasa/HeaderSawyerpuramformat1.jpg";
                            $scope.imagedd = "images/clients/Vikasa/images/pencils.png";
                            $scope.grade = "/images/clients/Vikasa/images/grade.png";
                            $scope.schooladdress = " Affiliated to Cambridge International Examinations U.K.";
                            $scope.style == "";
                        }

                    }
                    else {
                        swal("No Records Found");
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        //for print

        // end for print

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
        }

        $scope.VIKASAProgressCardReport = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/ProgressCardReport/VikasaintProgressCardReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

    }

})();