

(function () {
    'use strict';
    angular
        .module('app')
        .controller('MaldaProgressReportExamController', MaldaProgressReportExamController)

    MaldaProgressReportExamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function MaldaProgressReportExamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.printbutton = true;
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.report = false;
        //TO  GEt The Values iN Grid
        var pageid = 2;
        $scope.BindData = function () {
            apiService.getURI("MaldaProgressReportExam/Getdetails", pageid).then(function (promise) {
                $scope.yearlt = promise.yearlist;
                $scope.clslist = promise.classlist;
                $scope.seclist = promise.seclist;
                $scope.amstlt = promise.amstlist;
                $scope.exsplt = promise.exmstdlist;
                $scope.grade_list = promise.grade_list;
            });
        };



        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            angular.forEach($scope.yearlt, function (yr) {
                if (parseInt(yr.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                    $scope.year = yr.asmaY_Year;
                }
            });

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("MaldaProgressReportExam/savedetails", data).then(function (promise) {
                    $scope.gradation = [];
                    if (promise.savelist.length > 0) {

                        $scope.report = true;
                        $scope.printbutton = false;

                        //inst name binding
                        $scope.instname = promise.instname;
                        $scope.inst_name = $scope.instname[0].mI_Name;

                        $scope.countStudent = promise.countStudent.length;

                        var temp_list = [];
                        for (var x = 0; x < promise.savelist.length; x++) {
                            var stu_id = promise.savelist[x].amsT_Id;
                            var stu_subj_list = [];

                            angular.forEach(promise.savelist, function (opq) {
                                if (parseInt(opq.amsT_Id) === parseInt(stu_id)) {
                                    stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, emgR_Id: opq.emgR_Id, ismS_SubjectName: opq.ismS_SubjectName, eyceS_MaxMarks: opq.eyceS_MaxMarks, eyceS_MinMarks: opq.eyceS_MinMarks, eyceS_AplResultFlg: opq.eyceS_AplResultFlg, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_PassFailFlg == "" ? "NA" : opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_PassFailFlg == "" ? "NA" : opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg, ESTMPS_ClassAverage: opq.estmpS_ClassAverage, ESTMPS_SectionAverage: opq.estmpS_SectionAverage, ESTMPS_ClassHighest: opq.estmpS_PassFailFlg == "" ? "NA" :  opq.estmpS_ClassHighest, ESTMPS_SectionHighest: opq.estmpS_SectionHighest, estmP_TotalObtMarks: opq.estmP_TotalObtMarks, estmP_Percentage: opq.estmP_Percentage, estmP_TotalGrade: opq.estmP_TotalGrade, estmP_ClassRank: opq.estmP_ClassRank, estmP_SectionRank: opq.estmP_SectionRank, estmP_TotalGradeRemark: opq.estmP_TotalGradeRemark, estmP_TotalMaxMarks: opq.estmP_TotalMaxMarks, eyceS_MarksDisplayFlg: opq.eyceS_MarksDisplayFlg, eyceS_GradeDisplayFlg: opq.eyceS_GradeDisplayFlg });
                                }
                            });
                            if (temp_list.length === 0) {
                                temp_list.push({
                                    student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName,
                                    amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                    asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName,
                                    estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks,
                                    estmP_Percentage: promise.savelist[x].estmP_Percentage, classheld: promise.savelist[x].classheld,
                                    classatt: promise.savelist[x].classatt, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade,
                                    estmP_ClassRank: promise.savelist[x].estmP_ClassRank, estmP_SectionRank: promise.savelist[x].estmP_SectionRank,
                                    estmP_TotalGradeRemark: promise.savelist[x].estmP_TotalGradeRemark, estmP_Result: promise.savelist[x].estmP_Result,
                                    sub_list: stu_subj_list
                                });
                            }
                            else if (temp_list.length > 0) {
                                var already_cnt = 0;
                                angular.forEach(temp_list, function (opq1) {

                                    if (parseInt(opq1.student_id) === parseInt(stu_id)) {
                                        already_cnt += 1;
                                    }
                                });

                                if (already_cnt === 0) {
                                    temp_list.push({
                                        student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, classheld: promise.savelist[x].classheld, classatt: promise.savelist[x].classatt, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade,estmP_ClassRank: promise.savelist[x].estmP_ClassRank, estmP_SectionRank: promise.savelist[x].estmP_SectionRank, estmP_TotalGradeRemark: promise.savelist[x].estmP_TotalGradeRemark, estmP_Result: promise.savelist[x].estmP_Result, sub_list: stu_subj_list
                                    });
                                }
                            }
                        }

                        $scope.exam = promise.savelist[0].emE_ExamName;
                        $scope.exm_sublist = promise.subjlist;
                        $scope.subj_grade_remarks = promise.grade_details;
                        if (promise.clstchname.length > 0) {
                            $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                        } else {
                            $scope.clastechname = "";
                        }

                        $scope.report_list = temp_list;
                        $scope.getstudentwiseremarks = promise.getstudentwiseremarks;

                        if ($scope.getstudentwiseremarks !== null && $scope.getstudentwiseremarks.length > 0) {
                            angular.forEach($scope.report_list, function (stu) {
                                angular.forEach($scope.getstudentwiseremarks, function (re) {
                                    if (parseInt(stu.student_id) === parseInt(re.amsT_Id)) {
                                        stu.remarks = re.emeR_Remarks;
                                    }
                                });
                            });
                        }

                        $scope.grade_detailslist = promise.grade_detailslist;
                        $scope.gradationlength = $scope.grade_detailslist.length;

                    } else {
                        swal('No record Found');
                    }
                });
            }
        };

        $scope.cancel = function () {
            $scope.asmcL_Id = "";
            $scope.emcA_Id = "";
            $scope.asmaY_Id = "";
            $scope.emG_Id = "";
            $scope.asmS_Id = "";
            $scope.subjectlt = "";
            $scope.subjectlt1 = "";
            $scope.studentlist = false;
            $state.reload();
        };



        //to print
        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/MaldaProgressReportPdf .css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.get_totalmin = function (exm_subjs, stu_subjs) {
            //   
            $scope.stu_grandmin_marks = 0;
            angular.forEach(exm_subjs, function (itm) {
                if (itm.eyceS_AplResultFlg) {
                    angular.forEach(stu_subjs, function (itm1) {
                        if (parseInt(itm1.ismS_Id) === parseInt(itm.ismS_Id)) {
                            $scope.stu_grandmin_marks += itm.eyceS_MinMarks;
                        }
                    });
                }
            });
        };

        $scope.OnAcdyear = function () {
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("MaldaProgressReportExam/onchangeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.clslist = promise.classlist;
                    if ($scope.clslist.length === 0) {
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

            apiService.create("MaldaProgressReportExam/onchangeclass", data).then(function (promise) {

                if (promise !== null) {
                    $scope.seclist = promise.seclist;
                    if ($scope.seclist.length === 0) {
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
            apiService.create("MaldaProgressReportExam/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.exsplt = promise.exmstdlist;
                    if ($scope.exsplt.length === 0) {
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

        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
    }

})();