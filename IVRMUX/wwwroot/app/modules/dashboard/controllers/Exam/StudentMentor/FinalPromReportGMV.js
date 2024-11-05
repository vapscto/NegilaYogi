(function () {
    'use strict';
    angular.module('app').controller('FinalPromReportGMVController', FinalPromReportLESController)
    FinalPromReportLESController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window', 'Excel', '$timeout']
    function FinalPromReportLESController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window, Excel, $timeout) {

        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.getexamlist = [];
        $scope.obj = {};
        $scope.generateddate = new Date();
        $scope.imgname = "";
        $scope.OverAlltotal = false;
        $scope.OverAlClass = false;
        $scope.ECTEX_NotApplToTotalFlg = false;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            $scope.imgname = admfigsettings[0].asC_Logo_Path;
        }
        else {
            $scope.imgname = "";
        }
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.class_list = [];
            $scope.ASMCL_Id = "";
            $scope.section_list = [];
            $scope.ASMS_Id = "";
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            $scope.section_list = [];
            $scope.ASMS_Id = "";
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_sections", data).then(function (promise) {
                $scope.section_list = promise.getsectionlist;
            });
        };

        //-----------section Selection
        $scope.onsectionchange = function () {
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("JSHSExamReports/get_Exam_grade", data).then(function (promise) {
                $scope.grade_list = promise.getgradelist;
                $scope.getexamlist = promise.getexam;

                if ($scope.getexamlist !== null && $scope.getexamlist.length > 0) {
                    $scope.examlist = $scope.getexamlist;
                } else {
                    swal("No Term Is Mapped For Selected Details");
                    $scope.termlist = [];
                }
            });
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.BBHS_IX_20202021 = function (obj) {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];

            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                   // "ECTEX_NotApplToTotalFlg": $scope.ECTEX_NotApplToTotalFlg
                };

                apiService.create("JSHSExamReports/BBHS_IX_20202021", data).then(function (promise) {

                    if (promise !== null && promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {

                        $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                        $scope.JSHSReport = true;
                        $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;

                        $scope.getgroupdetails = promise.getgroupdetails;
                        $scope.getgroupexamdetails = promise.getgroupexamdetails;
                        $scope.studentdetails = promise.getstudentdetails;
                        $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                        $scope.groupwiseexamlist_temp = [];
                        $scope.groupwiseexamlist = [];
                        $scope.examwiseremarks = [];
                        $scope.examwiseremarks = promise.examwiseremarks;
                        // Group Wise Exam List
                        angular.forEach($scope.getgroupdetails, function (dd) {
                            $scope.groupwiseexamlist_temp = [];
                            var counttotal = 0;
                            angular.forEach($scope.getgroupexamdetails, function (d) {
                                if (dd.empG_DistplayName === d.empG_DistplayName) {
                                    counttotal += d.empsgE_ForMaxMarkrs;

                                    $scope.groupwiseexamlist_temp.push({
                                        EMPG_GroupName: d.empG_GroupName, EME_Id: d.emE_Id,
                                        EME_ExamName: d.emE_ExamName, EME_ExamOrder: d.emE_ExamOrder, EMPG_DistplayName: d.empG_DistplayName,
                                        EMPSGE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs,
                                        examnamedisplay: d.emE_ExamCode
                                    });

                                    $scope.groupwiseexamlist.push({
                                        EMPG_GroupName: d.empG_GroupName, EME_Id: d.emE_Id,
                                        EME_ExamName: d.emE_ExamName, EME_ExamOrder: d.emE_ExamOrder, EMPG_DistplayName: d.empG_DistplayName,
                                        EMPSGE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs,
                                        examnamedisplay: d.emE_ExamCode
                                    });
                                }
                            });

                            $scope.groupwiseexamlist_temp.push({
                                EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                                EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000, EMPG_DistplayName: dd.empG_DistplayName,
                                EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: 'Total'
                            });
                            $scope.groupwiseexamlist_temp.push({
                                EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800001,
                                EME_ExamName: "Grade", EME_ExamOrder: 9800001, EMPG_DistplayName: dd.empG_DistplayName,
                                EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: "Grade"
                            });
                            $scope.groupwiseexamlist.push({
                                EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                                EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000, EMPG_DistplayName: dd.empG_DistplayName,
                                EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: 'Total'
                            });

                            $scope.groupwiseexamlist.push({
                                EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800001,
                                EME_ExamName: "Grade", EME_ExamOrder: 9800001,
                                EMPG_DistplayName: dd.empG_DistplayName,
                                EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: "Grade"
                            });
                            dd.empsG_MarksValue = counttotal;
                            dd.groupewiseexam = $scope.groupwiseexamlist_temp;
                        });

                        console.log($scope.getgroupdetails);
                        console.log($scope.groupwiseexamlist);
                        $scope.sportsdetails = [];
                        if (promise.getstudentwisesportsdetails != null && promise.getstudentwisesportsdetails.length > 0) {
                            $scope.sportsdetails = promise.getstudentwisesportsdetails;
                        }
                            
                        
                        // Student Wise Subject List
                        $scope.studenwisesubjects = [];
                        angular.forEach($scope.studentdetails, function (stu) {
                            $scope.studenwisesubjects = [];
                            angular.forEach($scope.getstudentwisesubjectlist, function (stusubj) {
                                if (stu.AMST_Id === stusubj.AMST_Id) {
                                    $scope.studenwisesubjects.push(stusubj);
                                }
                            });
                            stu.studentsubjects = $scope.studenwisesubjects;
                        });

                        //Student Wise Marks List
                        $scope.studenwisemarks = [];
                        angular.forEach($scope.studentdetails, function (stu) {
                            $scope.studenwisemarks = [];
                            angular.forEach($scope.getstudentmarksdetails, function (stusubj) {
                                if (stu.AMST_Id === stusubj.AMST_Id) {
                                    $scope.studenwisemarks.push(stusubj);
                                }
                            });
                            stu.studentmarks = $scope.studenwisemarks;
                        });

                        // Student Wise Exam_Groupwise total
                        $scope.getstudent_examwisemarks = promise.getstudent_examwisemarks;
                        $scope.student_wisemarks = [];
                        angular.forEach($scope.studentdetails, function (stu) {
                            $scope.student_wisemarks = [];
                            angular.forEach($scope.getstudent_examwisemarks, function (stusubj) {
                                if (stu.AMST_Id === stusubj.AMST_Id) {
                                    $scope.student_wisemarks.push(stusubj);
                                }
                            });
                            stu.student_marks = $scope.student_wisemarks;
                        });

                        $scope.stud_work_attendence = promise.work_attendence;
                        $scope.stud_present_attendence = promise.present_attendence;

                        $scope.getpromotionmarksdetails = promise.getpromotionmarksdetails;
                        angular.forEach($scope.studentdetails, function (stu) {
                            angular.forEach($scope.getpromotionmarksdetails, function (stusubj) {
                                if (stu.AMST_Id === stusubj.amsT_Id) {
                                    stu.TotalMarks = stusubj.estmpP_TotalObtMarks;
                                    stu.TotalMaxMarks = stusubj.estmpP_TotalMaxMarks;
                                    stu.TotalGrade = stusubj.estmpP_TotalGrade;
                                    stu.TotalPercentage = stusubj.estmpP_Percentage;
                                    stu.SectionRank = stusubj.estmpP_SectionRank;
                                    stu.ClassRank = stusubj.estmpP_ClassRank;
                                }
                            });
                        });


                        angular.forEach($scope.year_list, function (dd) {
                            if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });

                        if (promise.clstchname !== null && promise.clstchname.length > 0) {
                            $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                        }
                        $scope.getpromotionremarksdetails = [];
                        $scope.getpromotionremarksdetails = promise.getpromotionremarksdetails;
                        angular.forEach($scope.studentdetails, function (stu) {
                            angular.forEach($scope.getpromotionremarksdetails, function (dd) {
                                if (stu.AMST_Id === dd.amsT_Id) {
                                    stu.remarks = dd.eprD_Remarks;
                                    stu.promotedclass = dd.eprD_ClassPromoted;
                                }
                            });
                        });
                        //added
                        angular.forEach($scope.studentdetails, function (stu) {
                            var TotalMarks = 0;
                            angular.forEach(stu.student_marks, function (dd) {
                                if (dd.EME_Id == 9800000) {
                                    TotalMarks = (Number(TotalMarks) + (Number(dd.ObtainedMarks)));
                                }
                            });
                            stu.TotalMarks = TotalMarks;
                        });
                        //added skills
                        $scope.skilllist_temp = [];
                        if (promise.getstudentwiseskillslist != null && promise.getstudentwiseskillslist.length > 0) {
                            $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;
                            angular.forEach($scope.studentdetails, function (stu) {
                                $scope.skilllist_temp = [];
                                var count = 0;
                                angular.forEach($scope.getstudentwiseskillslist, function (dd) {
                                    if (stu.AMST_Id === dd.AMST_Id) {
                                        if ($scope.skilllist_temp.length === 0) {
                                            $scope.skilllist_temp.push({ ECS_SkillName: dd.ECS_SkillName, ECS_Id: dd.ECS_Id, ECSA_Id: dd.ECSA_Id });
                                        } else if ($scope.skilllist_temp.length > 0) {
                                            count = 0;
                                            angular.forEach($scope.skilllist_temp, function (ddd) {
                                                if (ddd.ECS_SkillName === dd.ECS_SkillName) {
                                                    count += 1;
                                                }
                                            });
                                            if (count === 0) {
                                                $scope.skilllist_temp.push({ ECS_SkillName: dd.ECS_SkillName, ECS_Id: dd.ECS_Id, ECSA_Id: dd.ECSA_Id });
                                            }
                                        }
                                    }
                                });
                                stu.skill_main_list = $scope.skilllist_temp;
                            });
                        }
                        $scope.gettermdetails = [];
                        if (promise.gettermdetails != null && promise.gettermdetails.length > 0) {
                            $scope.gettermdetails = promise.gettermdetails;
                        }
                       // 
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            //popupWinindow.document.write('<html><head>' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BBIIReportPdf.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            //popupWinindow.document.close();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.Excel_HHS02 = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var excelname = "CONSOLIDATED MARKS SHEET REPORT.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getexamlist.some(function (options) {
                return options.EME_Id;
            });
        };
    }
})();