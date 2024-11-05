(function () {
    'use strict';

    angular
        .module('app')
        .controller('Baldwin_Final_P_ReportBGHSController', Baldwin_Final_P_ReportBGHSController);

    Baldwin_Final_P_ReportBGHSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout'];

    function Baldwin_Final_P_ReportBGHSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Baldwin_Final_P_ReportBGHS';

        activate();

        function activate() { }

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != undefined && admfigsettings != null) {
            var logopath = "";
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
            $scope.imgname = logopath;
        }

        $scope.BindData = function () {
            
            apiService.getDATA("Baldwin_Final_P_ReportBGHS/Getdetails").
       then(function (promise) {

           $scope.year_list = promise.yearlist;
           //   $scope.class_list = promise.classlist;
           //  $scope.section_list = promise.sectionlist;
           //$scope.exsplt = promise.exmstdlist;
       })
        };
        $scope.get_classes = function () {
            
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Baldwin_Final_P_ReportBGHS/get_classes", data).then(function (promise) {
                $scope.class_list = promise.classlist;
                $scope.ASMCL_Id = "";
                $scope.ASMS_Id = "";
                $scope.AMST_Id = "";
                $scope.section_list = [];
                $scope.student_list = [];
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.student_marks = [];
            })

        }
        $scope.get_sections = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("Baldwin_Final_P_ReportBGHS/get_sections", data).then(function (promise) {
                $scope.section_list = promise.sectionlist;
                $scope.ASMS_Id = "";
                $scope.AMST_Id = "";
                $scope.student_list = [];
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.student_marks = [];
            })
        }
        $scope.get_students = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            }
            apiService.create("Baldwin_Final_P_ReportBGHS/get_students", data).then(function (promise) {
                $scope.student_list = promise.studentlist;
                $scope.AMST_Id = "";
                $scope.exam_list = [];
                $scope.subject_list = [];
                $scope.student_marks = [];
            })
        }
        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        }
        $scope.get_report = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    //  "AMST_Id": $scope.AMST_Id
                }
                apiService.create("Baldwin_Final_P_ReportBGHS/get_report", data).then(function (promise) {
                    if (promise.examlist != null && promise.subjectlist != null && promise.examlist.length > 0 && promise.subjectlist.length > 0) {
                        $scope.exam_list = promise.examlist;
                        $scope.subject_list = promise.subjectlist;
                        $scope.student_marks = promise.studentmarks;
                        angular.forEach($scope.year_list, function (y) {
                            if (y.asmaY_Id == $scope.ASMAY_Id) {
                                $scope.Year_Name = y.asmaY_Year;
                            }
                        })
                        angular.forEach($scope.class_list, function (c) {
                            if (c.asmcL_Id == $scope.ASMCL_Id) {
                                $scope.Class_Name = c.asmcL_ClassName;
                            }
                        })
                        angular.forEach($scope.section_list, function (s) {
                            if (s.asmS_Id == $scope.ASMS_Id) {
                                $scope.Section_Name = s.asmC_SectionName;
                            }
                        })
                        if (promise.classteacher != null && promise.classteacher.length > 0) {
                            $scope.Class_Teacher_Name = promise.classteacher[0].hrmE_EmployeeFirstName;
                        }
                        angular.forEach($scope.exam_list, function (exm) {
                            if (exm.emE_FinalExamFlag) {
                                $scope.Exam_Name = exm.emE_ExamName;
                                $scope.Final_Exam_Id = exm.emE_Id;
                                $scope.Final_EYCE_Id = exm.eycE_Id;
                            }
                            if (exm.emE_ExamName=='PROJECT') {
                              //  $scope.Exam_Name = exm.emE_ExamName;
                                $scope.PROJECT_Exam_Id = exm.emE_Id;
                                $scope.PROJECT_EYCE_Id = exm.eycE_Id;
                            }
                        })

                        $scope.exam_subjectwise_details = promise.examsubjectwise_details;
                        $scope.exam_process_details = promise.process_examdetails;
                        //for only final exam subject details
                    //    $scope.final_exam_details = promise.final_exam_details;
                        //$scope.total_Exams = 0;
                        //angular.forEach($scope.exam_process_details, function (ttotal) {
                        //    $scope.total_Exams += ttotal.estmP_TotalObtMarks;
                        //})
                        angular.forEach($scope.exam_list, function (exm) {
                            var temp_subdetails = [];
                            angular.forEach(promise.examsubjectwise_details, function (sd) {
                                if (sd.eycE_Id == exm.eycE_Id && sd.eyceS_AplResultFlg) {
                                    temp_subdetails.push(sd);
                                }
                            })
                            var max_sub_marks = Math.max.apply(Math, temp_subdetails.map(function (item) { return item.eyceS_MaxMarks; }));
                            exm.marks = max_sub_marks;
                        })
                        $scope.total_max_sub_marks = 0;
                        angular.forEach($scope.exam_list, function (ex) {
                            $scope.total_max_sub_marks += ex.marks;
                        })
                        angular.forEach($scope.subject_list, function (sub) {
                            var temp_subdetails = [];
                            angular.forEach(promise.examsubjectwise_details, function (sd) {
                                if (sd.ismS_Id == sub.ismS_Id) {
                                    temp_subdetails.push(sd);
                                    if(sd.eycE_Id==$scope.Final_EYCE_Id)
                                    {
                                        sub.EMGR_Id = sd.emgR_Id;
                                        //for dynamic marks grade display
                                        sub.eyceS_MarksDisplayFlg = sd.eyceS_MarksDisplayFlg;
                                        sub.eyceS_GradeDisplayFlg = sd.eyceS_GradeDisplayFlg;
                                        //
                                    }
                                }
                            })
                            var applicable_flag = Math.max.apply(Math, temp_subdetails.map(function (item) { return item.eyceS_AplResultFlg; }));
                            sub.flag = applicable_flag;
                        })
                        //for student wise subjects new
                        $scope.studentwise_subjects = promise.stu_subjects;
                        angular.forEach($scope.student_list, function (stud) {
                            var subjects = [];
                            angular.forEach($scope.studentwise_subjects, function (stud_subj) {
                                if (stud_subj.amsT_Id == stud.amsT_Id) {
                                    angular.forEach($scope.subject_list, function (subj) {
                                        if (subj.ismS_Id == stud_subj.ismS_Id) {
                                            subjects.push(subj);
                                        }
                                    })
                                }

                            })
                            stud.stu_subjects = subjects;
                        })
                        angular.forEach($scope.student_list, function (stud) {
                            var subjects_new = [];
                            angular.forEach($scope.subject_list, function (subj) {
                                angular.forEach(stud.stu_subjects, function (stud_subj) {
                                    if (subj.ismS_Id == stud_subj.ismS_Id) {
                                        subjects_new.push(subj);
                                    }
                                })
                            })

                            stud.stu_subjects = subjects_new;
                        })
                        //for student wise marks new
                        angular.forEach($scope.student_list, function (stud) {
                            var stu_marks = [];
                            angular.forEach($scope.student_marks, function (stud_marks) {
                                if (stud_marks.amsT_Id == stud.amsT_Id) {
                                    stu_marks.push(stud_marks);
                                }

                            })
                            stud.stu_student_marks = stu_marks;
                            $scope.get_totalmin(stud.stu_subjects,stud);
                        })

                        $scope.stud_work_attendence = promise.work_attendence;
                        $scope.stud_present_attendence = promise.present_attendence;

                        //for grade remarks...
                        $scope.EMGR_Id = promise.emgR_Id;
                        $scope.Final_Exm_Grade_details = promise.grade_details;
                        $scope.Subjectwise_Grade_details = promise.subj_grade_details;
                    }
                    else {
                        swal("Selected Details Not Mapped with Subjects/Exams");
                        $scope.clear();
                    }


                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.Print = function () {
            var innerContents = document.getElementById("Final_Report").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' + '<style type="text/css">        @media print {    table {    page-break-inside: auto; }         tbody {   page-break-inside: avoid;     page-break-after: auto;   }  }    </style>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.clear = function () {
            $scope.ASMAY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            //  $scope.AMST_Id = "";
            $scope.class_list = [];
            $scope.section_list = [];
            $scope.student_list = [];
            $scope.exam_list = [];
            $scope.subject_list = [];
            $scope.student_marks = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        //$scope.exportToExcel = function (tableIds) {
        //    
        //    var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);

        //}

        $scope.get_totalmin = function (stu_subjs, stud) {
            //   
           var stu_grandmin_marks = 0;
            angular.forEach(stu_subjs, function (itm) {
                if (itm.flag) {
                    angular.forEach($scope.exam_subjectwise_details, function (itm1) {
                        if (itm1.ismS_Id == itm.ismS_Id && itm1.eycE_Id == $scope.Final_EYCE_Id) {
                            stu_grandmin_marks += Number(itm1.eyceS_MinMarks);
                        }
                    })
                }

            })
            angular.forEach($scope.exam_process_details, function (tot_m_e) {
                if(tot_m_e.amsT_Id==stud.amsT_Id && tot_m_e.emE_Id==$scope.Final_Exam_Id)
                {
                    tot_m_e.estmP_TotalMinMarks = stu_grandmin_marks;
                }
            })
        }
    }
})();
