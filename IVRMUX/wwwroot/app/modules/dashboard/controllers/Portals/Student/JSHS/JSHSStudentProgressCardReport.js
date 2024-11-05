(function () {
    'use strict';
    angular.module('app').controller('JSHSStudentProgressCardReportController', jSHSStudentProgressCardReportController)
    jSHSStudentProgressCardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'Excel', '$timeout']
    function jSHSStudentProgressCardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, Excel, $timeout) {

        $scope.reportdata = true;
        $scope.ASA_FromDate = new Date();

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSPortal_StudentReports/Getdetails_IT", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };       

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.reportdata = true;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSPortal_StudentReports/get_Exam_grade_pc", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
                $scope.getexamlist = promise.getexam;
                if ($scope.getexamlist !== null && $scope.getexamlist.length > 0) {
                    $scope.examlist = $scope.getexamlist;
                } else {
                    swal("No Exam Is Mapped For Selected Details");
                    $scope.termlist = [];
                }
            });
        };

       


       
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.saveddata = function () {
            $scope.submitted = true;
            $scope.reportdata = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                   "EME_Id": $scope.EME_Id
                };
                apiService.create("JSHSPortal_StudentReports/saveddata_pc", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.savelist !== null && promise.savelist.length > 0) {
                            $scope.bb = promise.savelist;
                            $scope.CurrentDate = $scope.ASA_FromDate;
                            $scope.studenttemparray = [];
                            $scope.reportdata = false;
                            angular.forEach($scope.bb, function (rr) {
                                if ($scope.studenttemparray.length === 0) {
                                    $scope.studenttemparray.push({
                                        amst_id: rr.amsT_Id, student_name: rr.amsT_FirstName, class_name: rr.asmcL_ClassName, section_name: rr.asmC_SectionName, rollno: rr.amaY_RollNo, fathersname: rr.amsT_FatherName, mothername: rr.amsT_MotherName, dob: rr.amsT_DOB,
                                        estmP_TotalObtMarks: rr.estmP_TotalObtMarks, estmP_TotalGrade: rr.estmP_TotalGrade,
                                        estmP_TotalMaxMarks: rr.estmP_TotalMaxMarks, admno: rr.amsT_AdmNo
                                    });
                                }
                                else if ($scope.studenttemparray.length > 0) {
                                    var count = 0;
                                    angular.forEach($scope.studenttemparray, function (yy) {
                                        if (yy.amst_id === rr.amsT_Id) {
                                            count += 1;
                                        }
                                    });
                                    if (count === 0) {
                                        $scope.studenttemparray.push({
                                            amst_id: rr.amsT_Id, student_name: rr.amsT_FirstName, class_name: rr.asmcL_ClassName, section_name: rr.asmC_SectionName,
                                            rollno: rr.amaY_RollNo, fathersname: rr.amsT_FatherName, mothername: rr.amsT_MotherName, dob: rr.amsT_DOB,
                                            estmP_TotalObtMarks: rr.estmP_TotalObtMarks, estmP_TotalGrade: rr.estmP_TotalGrade,
                                            estmP_TotalMaxMarks: rr.estmP_TotalMaxMarks, admno: rr.amsT_AdmNo
                                        });
                                    }
                                }
                            });

                            $scope.subjecttemparray = [];

                            angular.forEach($scope.studenttemparray, function (ww) {
                                $scope.subjecttemparray = [];
                                angular.forEach($scope.bb, function (tt) {
                                    if (ww.amst_id === tt.amsT_Id) {
                                        $scope.subjecttemparray.push({
                                            subj_id: tt.ismS_Id, sub_name: tt.ismS_SubjectName, estmpS_MaxMarks: tt.estmpS_MaxMarks, estmpS_ObtainedMarks: tt.estmpS_ObtainedMarks, estmpS_ObtainedGrade: tt.estmpS_ObtainedGrade, eyceS_AplResultFlg: tt.eyceS_AplResultFlg
                                        });
                                    }
                                });
                                ww.subjectlist = $scope.subjecttemparray;
                            });

                            $scope.Presentattendence = promise.present_attendence;
                            angular.forEach($scope.studenttemparray, function (dd) {
                                angular.forEach($scope.Presentattendence, function (att) {
                                    if (dd.amst_id === att.AMST_Id) {
                                        dd.presentdays = att.classattended;
                                        dd.workingdays = att.classheld;
                                    }
                                });
                            });

                            $scope.examwiseremarks = promise.examwiseremarks;

                            angular.forEach($scope.studenttemparray, function (d) {
                                angular.forEach($scope.examwiseremarks, function (at) {
                                    if (d.amst_id === at.amsT_Id) {
                                        d.remarksd = at.emeR_Remarks;                                       
                                    }
                                });
                            });

                            $scope.getstudentdetails = promise.getstudentdetails;

                            angular.forEach($scope.studenttemparray, function (dd) {
                                angular.forEach($scope.getstudentdetails, function (d) {
                                    if (dd.amst_id === d.amsT_Id) {
                                        dd.photoname = d.photoname;
                                    }
                                });
                            });

                            angular.forEach($scope.year_list, function (e) {
                                if (e.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.yearname = e.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.examlist, function (e) {
                                if (e.emE_Id === parseInt($scope.EME_Id)) {
                                    $scope.examname = e.emE_ExamName.toUpperCase();
                                }
                            });


                        } else {
                            swal("No Records Found1");
                        }
                        
                    } else {
                        swal("No Records Found2");
                    }
                   
                });
            }
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/ProgressCardReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };     
    }
})();