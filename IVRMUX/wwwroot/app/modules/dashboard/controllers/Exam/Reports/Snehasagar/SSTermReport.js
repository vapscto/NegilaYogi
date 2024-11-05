(function () {
    'use strict';
    angular.module('app').controller('SSTermReportController', SSTermReportController)
    SSTermReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function SSTermReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.print = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.termlistd = [];

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
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
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
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.AMST_Id = "";
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
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.AMST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("JSHSExamReports/get_students_category_grade", data).then(function (promise) {
                $scope.termlistd = promise.gettermlist;
                $scope.getgradelist = promise.getgradetermlist;
                $scope.studentlistd = promise.getstudentlist;

                if ($scope.studentlistd !== null && $scope.studentlistd.length > 0) {
                    $scope.studentlist = promise.getstudentlist;
                } else {
                    swal("No Student List Found For Selected Details");
                    return;
                }


                if ($scope.termlistd !== null && $scope.termlistd.length > 0) {
                    $scope.termlist = promise.gettermlist;
                    angular.forEach($scope.termlist, function (d) {
                        d.ECT_Id = true;
                    });
                } else {
                    swal("No Term Is Mapped For Selected Details");
                    $scope.termlist = [];
                }
                if ($scope.getgradelist !== null && $scope.getgradelist.length > 0) {
                    $scope.gradedetails = $scope.getgradelist;
                } else {
                    swal("No Grade Found");
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.termlist.some(function (options) {
                return options.ECT_Id;
            });
        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {

            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                angular.forEach($scope.termlist, function (term) {
                    if (term.ECT_Id === true) {
                        $scope.termlisttemp.push({ ECT_Id: term.ecT_Id, ECT_TermName: term.ecT_TermName });
                    }
                });

                var data = {
                    //"AMST_Id": $scope.AMST_Id.amsT_Id,
                    "AMST_Id": 0,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "termlist": $scope.termlisttemp,
                    "EMGR_Id": $scope.EMGR_Id
                };

                apiService.create("JSHSExamReports/getss_reportdetails", data).then(function (promise) {

                    $scope.JSHSReport = true;

                    $scope.institution = promise.getinstitution;
                    if ($scope.institution !== null && $scope.institution.length > 0) {
                        $scope.inst_name = $scope.institution[0].mI_Name;
                        $scope.add = $scope.institution[0].mI_Address1;
                        $scope.city = $scope.institution[0].ivrmmcT_Name;
                        $scope.pin = $scope.institution[0].mI_Pincode;
                    }

                    $scope.gettermdetails = promise.gettermdetails;
                    $scope.gettermexamdetails = promise.gettermexamdetails;
                    $scope.studentdetails = promise.getstudentdetails;
                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                    $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                    $scope.getgradedetails = promise.getgradedetails;


                    // STUDENT WISE SUBJECT LIST
                    $scope.studentwisesubject = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.studentwisesubject = [];
                        angular.forEach($scope.getstudentwisesubjectlist, function (subject) {
                            if (student.AMST_Id === subject.AMST_Id) {
                                $scope.studentwisesubject.push(subject);
                            }
                        });
                        student.subjectlist = $scope.studentwisesubject;
                    });


                    // STUDENT WISE TERM LIST AND TERM WISE EXAM LIST
                    $scope.termlistnew = [];
                    $scope.termwiseexamlistnew = [];

                    $scope.termlistnew = [];

                    angular.forEach($scope.gettermdetails, function (term) {
                        $scope.termwiseexamlistnew = [];
                        var totalper = 0;
                        angular.forEach($scope.gettermexamdetails, function (termexam) {
                            if (termexam.ecT_Id === term.ecT_Id) {
                                totalper += termexam.ecteX_MarksPercentValue;

                                $scope.termwiseexamlistnew.push({ emE_Id: termexam.emE_Id, emE_ExamName: termexam.emE_ExamName, ecteX_MarksPercentValue: termexam.ecteX_MarksPercentValue, ecT_Id: termexam.ecT_Id });
                            }
                        });

                        $scope.termwiseexamlistnew.push({ emE_Id: 5000, emE_ExamName: "TOTAL", ecteX_MarksPercentValue: totalper, ecT_Id: term.ecT_Id });

                        $scope.termlistnew.push({
                            ecT_Id: term.ecT_Id, ecT_TermName: term.ecT_TermName, ecT_Marks: term.ecT_Marks,
                            termwiseexamlist: $scope.termwiseexamlistnew
                        });
                    });



                    console.log("Term List");
                    console.log($scope.termlistnew);
                    console.log($scope.gettermdetails);
                    $scope.termwiseexamlistnewtemp = [];
                    $scope.overalltotalper = 0;
                    var overalltotal = 0;
                    angular.forEach($scope.gettermdetails, function (term) {
                        var totalper = 0;
                        var examnamedetails = "";
                        var countexam = 0;
                        angular.forEach($scope.gettermexamdetails, function (termexam) {
                            if (termexam.ecT_Id === term.ecT_Id) {
                                totalper += termexam.ecteX_MarksPercentValue;
                                if (termexam.ecteX_NotApplToTotalFlg !== true) {
                                    overalltotal += termexam.ecteX_MarksPercentValue;
                                }
                                if (countexam === 0) {
                                    examnamedetails = termexam.emE_ExamDescription;
                                    countexam += 1;
                                } else {
                                    examnamedetails = examnamedetails + "+" + termexam.emE_ExamDescription;
                                }

                                $scope.termwiseexamlistnewtemp.push({
                                    emE_Id: termexam.emE_Id, emE_ExamName: termexam.emE_ExamName,
                                    ecteX_MarksPercentValue: termexam.ecteX_MarksPercentValue, ecT_Id: termexam.ecT_Id,
                                    emE_ExamDescription: termexam.emE_ExamDescription
                                });
                            }
                        });
                        $scope.overalltotalper = overalltotal;
                        $scope.termwiseexamlistnewtemp.push({
                            emE_Id: 5000, emE_ExamName: "TOTAL", ecteX_MarksPercentValue: totalper, ecT_Id: term.ecT_Id,
                            emE_ExamDescription: examnamedetails
                        });
                    });

                    //$scope.termwiseexamlistnewtemp.push({ emE_Id: 5001, emE_ExamName: "TOTAL", ecteX_MarksPercentValue: totalper, ecT_Id: term.ecT_Id });
                    //$scope.termwiseexamlistnewtemp.push({ emE_Id: 5001, emE_ExamName: "TOTAL", ecteX_MarksPercentValue: totalper, ecT_Id: term.ecT_Id });


                    console.log($scope.gettermexamdetails);
                    console.log("Term Wise Exam List");
                    console.log($scope.termwiseexamlistnewtemp);

                    // STUDENT WISE MARKS LIST 
                    $scope.studentwisemarksdetails = [];
                    $scope.studentwisemarkstotaldetails = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.resultflag = "";
                        $scope.studentwisemarksdetails = [];
                        $scope.studentwisemarkstotaldetails = [];
                        angular.forEach($scope.getstudentmarksdetails, function (marks) {

                            if (marks.overallresultflag === "FAIL") {
                                $scope.resultflag = "FAIL";
                            }

                            if (student.AMST_Id === marks.AMST_Id && marks.examname !== 'TOTAL GRADE' && marks.subjectname !== 'TOTAL') {
                                $scope.studentwisemarksdetails.push(marks);
                            }
                            if (student.AMST_Id === marks.AMST_Id && marks.examname !== 'TOTAL GRADE' && marks.subjectname === 'TOTAL') {
                                $scope.studentwisemarkstotaldetails.push(marks);
                            }
                        });
                        student.markslist = $scope.studentwisemarksdetails;
                        student.markstotal = $scope.studentwisemarkstotaldetails;
                        student.finalresultflag = $scope.resultflag;
                    });

                    //STUDENT WISE ATTENDANCE DETAILS 
                    $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;

                    angular.forEach($scope.studentdetails, function (student) {
                        angular.forEach($scope.getstudentwiseattendancedetails, function (attn) {
                            if (student.AMST_Id === attn.AMST_Id) {
                                student.workingdays = attn.TOTALWORKINGDAYS;
                                student.presentdays = attn.PRESENTDAYS;
                                student.attendanceper = attn.ATTENDANCEPERCENTAGE;
                            }
                        });
                    });

                    console.log("Student List");
                    console.log($scope.studentdetails);
                    $scope.print = true;
                });

                console.log($scope.exm_sub_mrks_list);
                console.log($scope.total_subwise);

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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Snehasagar/SSTermReportCardPdf.css" />' +
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
    }
})();