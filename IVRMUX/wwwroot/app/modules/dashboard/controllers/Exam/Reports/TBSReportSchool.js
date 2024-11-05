(function () {
    'use strict';
    angular.module('app').controller('TBSReportSchoolController', TBSReportSchoolController)
    TBSReportSchoolController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window']
    function TBSReportSchoolController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window) {

        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.getstudentlist = [];
        $scope.grade_list = [];
        $scope.termlistd = [];
        $scope.obj = {};
        $scope.imgname = "";
        $scope.grandeTotal = true;
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
            $scope.getstudentmarksdetails = [];
            $scope.termlisttemp = [];
            $scope.termlistd = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.getstudentlist = [];
            $scope.class_list = [];
            $scope.termlist = [];
            $scope.AMST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.termlist = [];
            $scope.termlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.AMST_Id = "";
            $scope.section_list = [];
            $scope.getstudentlist = [];
            $scope.ASMS_Id = "";
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_sections", data).then(function (promise) {
                $scope.section_list = promise.getsectionlist;
            });
        };

        $scope.onsectionchange = function () {

            $scope.getstudentmarksdetails = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.termlist = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.getstudentlist = [];
            $scope.grade_list = [];
            $scope.AMST_Id = "";
            $scope.EMGR_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "flagtype": "Exam",

            };
            apiService.create("JSHSExamReports/GetStudentDetails", data).then(function (promise) {
                $scope.getstudentlist = promise.getstudentlist;
                $scope.grade_list = promise.getgradelist;

                $scope.all = true;
                angular.forEach($scope.getstudentlist, function (dd) {
                    dd.AMST_Ids = true;
                });
                $scope.getexamlist = promise.getexam;
                if ($scope.getexamlist !== null && $scope.getexamlist.length > 0) {
                    $scope.examlist = $scope.getexamlist;
                } else {
                    swal("No Term Is Mapped For Selected Details");
                    $scope.termlist = [];
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };




        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {
            $scope.getstudentmarksdetails = [];
            $scope.submitted = true;
            $scope.getstudentwiseattendancedetails = [];
            $scope.examwiseremarks = [];
            $scope.getstudentwisetermwisedetails = [];
            $scope.groupwiseexamlist = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.Year = "";
            $scope.getsubjects = [];
            $scope.YearlySkillAreaStudentWise = [];
            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                $scope.Temp_AmstId = [];
                $scope.termlisttemp = [];
                $scope.groupwiseexamlist = [];
                $scope.examlistgrade = [];
                angular.forEach($scope.getexamlist, function (term) {
                    if (term.EME_Id === true) {
                        $scope.termlisttemp.push({ EME_Id: term.emE_Id, EME_ExamName: term.emE_ExamName });
                    }
                });
                angular.forEach($scope.getstudentlist, function (dd) {
                    if (dd.AMST_Ids) {
                        $scope.Temp_AmstId.push({ AMST_Id: dd.amsT_Id });
                    }
                });
                //
                angular.forEach($scope.year_list, function (dd) {
                    if (dd.asmaY_Id == $scope.ASMAY_Id) {
                        $scope.Year = dd.asmaY_Year;
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "Temp_AmstIds": $scope.Temp_AmstId,
                    "examlist": $scope.termlisttemp,
                    "EMGR_Id": $scope.EMGR_Id,
                    "EMPG_GroupName": ""
                };

                apiService.create("JSHSExamReports/TBSReportSchool", data).then(function (promise) {

                    $scope.studentdetails = promise.getstudentdetails;
                    $scope.getpromotionremarksdetails = [];
                    $scope.getpromotionremarksdetails = promise.getpromotionremarksdetails;
                    $scope.groupwiseexamlist = promise.getgroupexamdetails;
                    $scope.examwiseremarks = promise.examwiseremarks;
                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                    $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                    $scope.getstudentwisetermwisedetails = promise.getstudentwisetermwisedetails;
                    $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;
                    $scope.getsubjects = promise.getsubjects;
                    $scope.YearlySkillAreaStudentWise = promise.yearlySkillAreaStudentWise;
                    $scope.examlistgradetwo = [];
                    if ($scope.groupwiseexamlist != null && $scope.groupwiseexamlist.length > 0) {
                        angular.forEach($scope.groupwiseexamlist, function (d) {
                            $scope.examlistgrade.push({
                                empsgE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs,
                                EMPG_GroupName: d.empG_GroupName, EME_Id: d.emE_Id,
                                EME_ExamName: d.emE_ExamName, EME_ExamOrder: d.emE_ExamOrder,
                                EMPG_DistplayName: d.empG_DistplayName,
                                examnamedisplay: d.emE_ExamCode,
                                colspan: null,
                                GradeExam: "Exam",
                                Gradespan: null
                            });
                            $scope.examlistgrade.push({
                                empsgE_ForMaxMarkrs: "Grade",
                                EMPG_GroupName: d.empG_GroupName, EME_Id: d.emE_Id,
                                EME_ExamName: d.emE_ExamName, EME_ExamOrder: d.emE_ExamOrder,
                                EMPG_DistplayName: d.empG_DistplayName,
                                examnamedisplay: d.emE_ExamCode,
                                colspan: null,
                                GradeExam: "Grade",
                                Gradespan: 2

                            });
                            $scope.examlistgrade.push({
                                empsgE_ForMaxMarkrs: "Grade",
                                EMPG_GroupName: d.empG_GroupName, EME_Id: d.emE_Id,
                                EME_ExamName: d.emE_ExamName, EME_ExamOrder: d.emE_ExamOrder,
                                EMPG_DistplayName: d.empG_DistplayName,
                                examnamedisplay: d.emE_ExamCode,
                                colspan: null,
                                GradeExam: "Gradess",
                                Gradespan: 2

                            });

                            //tempsubjects 

                        });
                        //Graded Subjects 

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
                        //student_marsk
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
                        //marksUnder 

                        $scope.getstudent_examwisemarks = [];
                        $scope.getstudent_examwisemarks = promise.getstudent_examwisemarks;
                        angular.forEach($scope.studentdetails, function (stu) {
                            $scope.student_wisemarks = [];
                            angular.forEach($scope.getstudent_examwisemarks, function (stusubj) {
                                if (stu.AMST_Id === stusubj.AMST_Id) {
                                    $scope.student_wisemarks.push(stusubj);
                                }
                            });
                            stu.student_marks = $scope.student_wisemarks;
                        });
                        //exams

                    }
                    else {
                        swal("Record  Not Found !");
                    }

                    //test

                });
            } else {
                $scope.submitted = true;
            }
        };

        //to print
        $scope.print_HHS02 = function () {
            var innerContents = document.getElementById("HHS02").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.isOptionsRequired2 = function () {
            return !$scope.getexamlist.some(function (options) {
                return options.EME_Id;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.getstudentlist.some(function (options) {
                return options.AMST_Ids;
            });
        };

        $scope.OnClickAll = function () {
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
            angular.forEach($scope.getstudentlist, function (dd) {
                dd.AMST_Ids = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.getstudentlist.every(function (itm) { return itm.AMST_Ids; });
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
    }
})();