(function () {
    'use strict';
    angular.module('app').controller('BIS_IX_XII_ProgressCardReportController', BIS_IX_XII_ProgressCardReportController)
    BIS_IX_XII_ProgressCardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window']
    function BIS_IX_XII_ProgressCardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window) {

        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.termlistd = [];
        $scope.obj = {};
        $scope.searchchkbx = "";

        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();

            $scope.generateddate = new Date($scope.today);
        };

        $scope.getserverdate();

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.termlistd = [];
            $scope.studentdetails = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.class_list = [];
            $scope.termlist = [];
            $scope.getexam = [];
            $scope.AMST_Id = "";
            $scope.searchchkbx = "";
            $scope.EME_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentdetails = [];
            $scope.termlist = [];
            $scope.termlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.AMST_Id = "";
            $scope.section_list = [];
            $scope.getexam = [];
            $scope.ASMS_Id = "";
            $scope.searchchkbx = "";
            $scope.EME_Id = "";
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_sections", data).then(function (promise) {
                $scope.section_list = promise.getsectionlist;
            });
        };

        $scope.onsectionchange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentdetails = [];
            $scope.termlist = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.getexam = [];
            $scope.AMST_Id = "";
            $scope.searchchkbx = "";
            $scope.EME_Id = "";
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "flagtype": "Exam",
            };
            apiService.create("JSHSExamReports/GetStudentDetails", data).then(function (promise) {
                $scope.studentlist = promise.getstudentlist;
                $scope.all = true;
                angular.forEach($scope.studentlist, function (stu) {
                    stu.checkedsub = true;
                });

                $scope.getexam = promise.getexam;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.GetBISExamWiseProgressCardReport = function (obj) {

            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.studentdetails = [];
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];

                angular.forEach($scope.studentlist, function (dd) {
                    if (dd.checkedsub) {
                        $scope.termlisttemp.push({ AMST_Id: dd.amsT_Id })
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "Temp_AmstIds": $scope.termlisttemp
                };
                apiService.create("JSHSExamReports/GetBISExamWiseProgressCardReport", data).then(function (promise) {
                    if (promise !== null && promise.getstudentwisesubjectlist !== null && promise.getstudentwisesubjectlist.length > 0) {
                        $scope.studentdetails_temp = promise.getstudentdetails;
                        $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                        $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;
                        $scope.getgradedetails = promise.getgradedetails;
                        $scope.examwiseremarks = promise.examwiseremarks;
                        $scope.ExamWise_PaperType = promise.examWise_PaperType;
                        $scope.classteachername = promise.clstchname !== null && promise.clstchname.length > 0 ? promise.clstchname[0].hrmE_EmployeeFirstName : "";

                        $scope.tempsubject_marks = [];
                        $scope.temp_student = [];

                        angular.forEach($scope.studentdetails_temp, function (stu) {
                            $scope.tempsubject_marks = [];
                            var applytoresult_count = 0;
                            angular.forEach($scope.getstudentwisesubjectlist, function (stu_subj) {
                                if (stu.AMST_Id === stu_subj.AMST_Id) {
                                    if (stu_subj.EYCES_AplResultFlg === true) {
                                        applytoresult_count += 1;
                                    }

                                    $scope.papertype = "";
                                    $scope.papertype_color = "black";

                                    angular.forEach($scope.ExamWise_PaperType, function (stu_subj_pt) {
                                        if (stu_subj_pt.amsT_Id === stu_subj.AMST_Id && stu_subj_pt.ismS_Id === stu_subj.ISMS_Id) {
                                            $scope.papertype = "(" + stu_subj_pt.empatY_PaperTypeName + ")";
                                            $scope.papertype_color = stu_subj_pt.empatY_Color === "" ? "black" : stu_subj_pt.empatY_Color;
                                        }
                                    });


                                    $scope.tempsubject_marks.push({
                                        AMST_Id: stu_subj.AMST_Id, ISMS_Id: stu_subj.ISMS_Id, ISMS_SubjectName: stu_subj.ISMS_SubjectName,
                                        EYCES_SubjectOrder: stu_subj.EYCES_SubjectOrder,
                                        EYCES_AplResultFlg: stu_subj.EYCES_AplResultFlg,
                                        ESTMPS_MaxMarks: stu_subj.ESTMPS_MaxMarks,
                                        ESTMPS_ObtainedMarks: stu_subj.ESTMPS_ObtainedMarks,
                                        ESTMPS_ObtainedGrade: stu_subj.ESTMPS_ObtainedGrade,
                                        ESTMPS_PassFailFlg: stu_subj.ESTMPS_PassFailFlg,
                                        ESTMPS_ClassRank: stu_subj.ESTMPS_ClassRank,
                                        ESTMPS_SectionRank: stu_subj.ESTMPS_SectionRank,
                                        ESTMPS_Percentage: stu_subj.ESTMPS_Percentage,
                                        papertype: $scope.papertype,
                                        papertype_color: $scope.papertype_color,

                                    });
                                }
                            });
                            var working_days = 0;
                            var present_days = 0;
                            var att_percentage = 0;
                            var student_rmrks = "";

                            angular.forEach($scope.getstudentwiseattendancedetails, function (stu_attendance) {
                                if (stu_attendance.AMST_Id === stu.AMST_Id) {
                                    working_days = stu_attendance.TOTALWORKINGDAYS;
                                    present_days = stu_attendance.PRESENTDAYS;
                                    att_percentage = stu_attendance.ATTENDANCEPERCENTAGE;
                                }
                            });

                            angular.forEach($scope.examwiseremarks, function (stu_remarks) {
                                if (stu_remarks.amsT_Id === stu.AMST_Id) {
                                    student_rmrks = stu_remarks.emeR_Remarks.split("\n");
                                }
                            });

                            $scope.Get_ApplyToResult_Subjects = [];
                            $scope.Get_NotApplyToResult_Subjects = [];
                            angular.forEach($scope.tempsubject_marks, function (stu_subjects) {
                                if (stu_subjects.EYCES_AplResultFlg) {
                                    $scope.Get_ApplyToResult_Subjects.push(stu_subjects);
                                }
                                else if (!stu_subjects.EYCES_AplResultFlg) {
                                    $scope.Get_NotApplyToResult_Subjects.push(stu_subjects);
                                }
                            });

                            $scope.studentdetails.push({
                                AMST_Id: stu.AMST_Id, studentname: stu.studentname, admno: stu.admno, rollno: stu.rollno,
                                classname: stu.classname, sectionname: stu.sectionname, fathername: stu.fathername, mothername: stu.mothername,
                                dob: stu.dob, mobileno: stu.mobileno, ExamName: stu.ExamName, ExamNameYear: stu.ExamNameYear,
                                stu_att_percentage: att_percentage, stu_working_days: working_days, stu_present_days: present_days,
                                applytoresult: applytoresult_count, student_remarks: student_rmrks, applytoresult_subjects: $scope.Get_ApplyToResult_Subjects,
                                notapplytoresult_subjects: $scope.Get_NotApplyToResult_Subjects
                            });
                        });

                    } else {
                        swal("No Records Found");
                    }
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/NDS/BIS_ReportCardPdf.css" />' +
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


        $scope.OnClickAll = function () {
            angular.forEach($scope.studentlist, function (dd) {
                dd.checkedsub = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.studentlist.every(function (itm) { return itm.checkedsub; });
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.studentlist.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
    }
})();