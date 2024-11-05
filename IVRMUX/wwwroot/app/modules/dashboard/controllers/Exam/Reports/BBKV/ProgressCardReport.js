(function () {
    'use strict';
    angular.module('app').controller('ProgresscardreportnewController', ProgresscardreportnewController)

    ProgresscardreportnewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ProgresscardreportnewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        $scope.amsT_Date = new Date();

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("VikasaAssessment2Report/Getdetails/", pageid).then(function (promise) {
                $scope.yearlt = promise.yearlist;
                $scope.asmcL_Id = "";
                $scope.asmS_Id = "";
                $scope.amsT_Id = "";
                $scope.emE_Id = "";
                $scope.classDropdown = "";
                $scope.sectionDropdown = "";
                $scope.exsplt = "";
            });
        };

        $scope.get_class = function () {

            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.sectionDropdown = "";
            $scope.exsplt = "";

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
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("VikasaAssessment2Report/get_exam", data).then(function (promise) {
                $scope.exsplt = promise.examList;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
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

                                if (parseInt(opq.amsT_Id) === parseInt(stu_id)) {

                                    opq.color = 'Black';

                                    stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, emgR_Id: opq.emgR_Id, ismS_SubjectName: opq.ismS_SubjectName, eyceS_MaxMarks: opq.eyceS_MaxMarks, eyceS_MinMarks: opq.eyceS_MinMarks, eyceS_AplResultFlg: opq.eyceS_AplResultFlg, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, color: opq.color, estmpS_PassFailFlg: opq.estmpS_PassFailFlg, ESTMPS_ClassAverage: opq.estmpS_ClassAverage, ESTMPS_SectionAverage: opq.estmpS_SectionAverage, ESTMPS_ClassHighest: opq.estmpS_ClassHighest, ESTMPS_SectionHighest: opq.estmpS_SectionHighest, estmP_TotalObtMarks: opq.estmP_TotalObtMarks, estmP_Percentage: opq.estmP_Percentage, estmP_TotalGrade: opq.estmP_TotalGrade, estmP_ClassRank: opq.estmP_ClassRank, estmP_SectionRank: opq.estmP_SectionRank, estmP_TotalGradeRemark: opq.estmP_TotalGradeRemark, estmP_TotalMaxMarks: opq.estmP_TotalMaxMarks });
                                }
                            });

                            if (temp_list.length === 0) {

                                temp_list.push({
                                    student_id: stude.amsT_Id, amsT_FirstName: stude.amsT_FirstName, amsT_DOB: stude.amsT_DOB, amsT_AdmNo: stude.amsT_AdmNo,
                                    amaY_RollNo: stude.amaY_RollNo, asmcL_ClassName: stude.asmcL_ClassName, asmC_SectionName: stude.asmC_SectionName,
                                    estmP_TotalMaxMarks: stude.estmP_TotalMaxMarks, estmP_TotalObtMarks: stude.estmP_TotalObtMarks,
                                    estmP_Percentage: stude.estmP_Percentage, classheld: stude.classheld, classatt: stude.classatt,
                                    estmP_TotalGrade: stude.estmP_TotalGrade, estmP_ClassRank: stude.estmP_ClassRank, estmP_SectionRank: stude.estmP_SectionRank, estmP_TotalGradeRemark: stude.estmP_TotalGradeRemark, sub_list: stu_subj_list
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
                                        student_id: stude.amsT_Id, amsT_FirstName: stude.amsT_FirstName, amsT_DOB: stude.amsT_DOB, amsT_AdmNo: stude.amsT_AdmNo,
                                        amaY_RollNo: stude.amaY_RollNo, asmcL_ClassName: stude.asmcL_ClassName,
                                        asmC_SectionName: stude.asmC_SectionName, estmP_TotalMaxMarks: stude.estmP_TotalMaxMarks,
                                        estmP_TotalObtMarks: stude.estmP_TotalObtMarks, estmP_Percentage: stude.estmP_Percentage, classheld: stude.classheld,
                                        classatt: stude.classatt, estmP_TotalGrade: stude.estmP_TotalGrade, estmP_ClassRank: stude.estmP_ClassRank, estmP_SectionRank: stude.estmP_SectionRank, estmP_TotalGradeRemark: stude.estmP_TotalGradeRemark, sub_list: stu_subj_list
                                    });
                                }
                            }
                        });

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
                            if (parseInt(dd.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.classDropdown, function (dc) {
                            if (parseInt(dc.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                                $scope.asmcL_ClassName = dc.asmcL_ClassName;
                            }
                        });

                        angular.forEach($scope.sectionDropdown, function (ds) {
                            if (parseInt(ds.asmS_Id) === parseInt($scope.asmS_Id)) {
                                $scope.asmC_SectionName = ds.asmC_SectionName;
                            }
                        });

                        $scope.issuedate = new Date($scope.amsT_Date);

                    }
                    else {
                        swal("No Records Found");
                    }
                });
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.get_totalmin = function (exm_subjs, stu_subjs) {
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

        $scope.VIKASAProgressCardReport = function () {
            var innerContents = document.getElementById("VIKASAProgressCard").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/BBKV/ProgressCard/BBKVReportCardNewPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

    }

})();
