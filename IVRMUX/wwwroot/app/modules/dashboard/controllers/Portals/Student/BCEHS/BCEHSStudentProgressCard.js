(function () {
    'use strict';
    angular.module('app').controller('BCEHSStudentProgressCardController', BCEHSStudentProgressCardController)

    BCEHSStudentProgressCardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$compile']
    function BCEHSStudentProgressCardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $compile) {

        $scope.percounttotal = 0;
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.HHS_I_IV_grid = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.grandfinaltotal = 0;
        $scope.grandfinalmaxtotal = 0;
        $scope.grandtotalperc = 0;
        //TO  GEt The Values iN Grid
        $scope.grandavgtotal = 0;
        $scope.exm_sub_mrks_list = [];

        $scope.halfyearatt = [];
        $scope.fullyearatt = [];
        $scope.readmit = false;

        $scope.BindData = function () {
            var pageid = 2;

            apiService.getURI("StudentProgressCardReport/stmarygetdetails", pageid).then(function (promise) {
                $scope.btn = false;
                $scope.getstudentdetails = promise.getstudentdetails;
                $scope.year_list = promise.getyear;
                $scope.classlist = promise.getclass;
                $scope.examorterm = promise.examorterm;

                if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                    $scope.ASMCL_Id = $scope.getstudentdetails[0].asmcL_Id;
                    $scope.asmS_Id = $scope.getstudentdetails[0].asmS_Id;
                    $scope.asmaY_Id = $scope.getstudentdetails[0].asmaY_Id;
                    $scope.getexamtermlist = [];

                    if ($scope.examorterm === 'Exam' || $scope.examorterm === 'Promotion') {
                        $scope.getexamtermlist = promise.getexamtermlist;
                        if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                            swal("Marks Not Published");
                        }
                        $scope.examflag = false;
                        $scope.termflag = false;
                    }
                    else {
                        $scope.btn = true;
                        swal("Report Format Not Mapped Contact Administrator");
                    }
                } else {
                    $scope.btn = true;
                    swal("No Studnet Data Found");
                }
            });
        };

        $scope.onchangeclass = function () {
            $scope.getexamtermlist = [];
            $scope.obj.ECT_Id = "";
            $scope.obj.EME_Id = "";
            $scope.EME_Id = "";
            $scope.examflag = false;
            $scope.HHS_I_IV_grid = false;
            $scope.termflag = false;
            $scope.reportshowcount = 0;
            $scope.btn = false;
            if ($scope.ASMCL_Id !== undefined && $scope.ASMCL_Id !== null && $scope.ASMCL_Id !== "") {
                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id
                };

                apiService.create("StudentProgressCardReport/stmaryonchangeclass", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getstudentdetails = promise.getstudentdetails;

                        if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                            $scope.examorterm = promise.examorterm;

                            if ($scope.examorterm === 'Exam' || $scope.examorterm === 'Promotion') {
                                if ($scope.getexamtermlist === null && $scope.getexamtermlist.length === 0) {
                                    swal("Marks Not Published");
                                }
                                $scope.getexamtermlist = promise.getexamtermlist;
                                $scope.examflag = false;
                                $scope.termflag = false;
                            }
                            else {
                                $scope.btn = true;
                                swal("Report Format Not Mapped Contact Administrator");
                            }
                        } else {
                            $scope.btn = true;
                            swal("No Studnet Data Found");
                        }
                    }
                });
            }
        };

        $scope.onchangeexam = function () {             
            $scope.HHS_I_IV_grid = false;
            $scope.submitted = true;
            $scope.getstudentmarksdetails_temp = [];
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
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

        $scope.Get_Stjames_Progresscard_Report = function () {
            $scope.HHS_I_IV_grid = false;
            $scope.submitted = true;
            $scope.getstudentmarksdetails_temp = [];
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
            if ($scope.myForm.$valid) {
                $scope.grandavgtotal = 0;

                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "EME_Id": $scope.EME_Id
                };
                apiService.create("StudentProgressCardReport/Get_BCEHS_Progresscard_Report", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.spflag = promise.spflag;
                        $scope.examflag = promise.examflag;

                        if (promise.getpublishmarks !== null && promise.getpublishmarks.length > 0) {
                            if (promise.savelist !== null && promise.savelist.length > 0) {
                                $scope.report = true;
                                $scope.HHS_I_IV_grid = true;
                                $scope.instname = promise.instname;
                                $scope.inst_name = $scope.instname[0].mI_Name;
                                var temp_list = [];
                                for (var x = 0; x < promise.savelist.length; x++) {
                                    var stu_id = promise.savelist[x].amsT_Id;
                                    var stu_subj_list = [];
                                    angular.forEach(promise.savelist, function (opq) {
                                        if (opq.amsT_Id == stu_id) {
                                            stu_subj_list.push({
                                                amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg, ESTMPS_ClassAverage: opq.estmpS_ClassAverage, ESTMPS_SectionAverage: opq.estmpS_SectionAverage, ESTMPS_ClassHighest: opq.estmpS_ClassHighest, ESTMPS_SectionHighest: opq.estmpS_SectionHighest,
                                                EYCES_MarksDisplayFlg: opq.eyceS_MarksDisplayFlg, EYCES_GradeDisplayFlg: opq.eyceS_GradeDisplayFlg
                                            });
                                        }
                                    });
                                    if (temp_list.length == 0) {

                                        temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
                                    }
                                    else if (temp_list.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(temp_list, function (opq1) {

                                            if (opq1.student_id == stu_id) {
                                                already_cnt += 1;

                                            }
                                        });
                                        if (already_cnt == 0) {
                                            temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
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
                                $scope.report_list = temp_list;
                                $scope.stud_work_attendence = promise.work_attendence;
                                $scope.stud_present_attendence = promise.present_attendence;

                                var e1 = angular.element(document.getElementById("report"));
                                $compile(e1.html(promise.htmlstring))(($scope));


                            } else {
                                swal('No record Found');
                            }

                            $scope.year = promise.asmaY_Year;                             

                            angular.forEach($scope.getexamlist, function (dd) {
                                if (dd.emE_Id === parseInt($scope.EME_Id)) {
                                    $scope.exam = dd.emE_ExamName;
                                }
                            });
                        } else {
                            swal("Marks Not Yet Published");
                        }
                    }
                });
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;
            });
        };

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; });
        };

        //to print
        $scope.printToCart = function () {
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.examflag === 1) {
                innerContents = document.getElementById("Baldwin").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print"  rel="stylesheet" href="css/print/baldwin/ProgressReport/BCOEDIReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            } else if ($scope.examflag === 2) {
                innerContents = document.getElementById("Baldwin").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BCOEDIIReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        };         
    }
})();