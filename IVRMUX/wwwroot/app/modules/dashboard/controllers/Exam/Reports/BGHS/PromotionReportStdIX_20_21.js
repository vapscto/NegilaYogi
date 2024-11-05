(function () {
    'use strict';
    angular.module('app').controller('PromotionReportStdIXController', PromotionReportStdIXController)
    PromotionReportStdIXController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function PromotionReportStdIXController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.print = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.termlistd = [];
        $scope.romnanumber = "";

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
        $scope.BGHS_IX_20202021 = function () {

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
                    "ASMS_Id": $scope.ASMS_Id
                    //"termlist": $scope.termlisttemp,
                    //"EMGR_Id": $scope.EMGR_Id
                };

                apiService.create("JSHSExamReports/BGHS_IX_20202021", data).then(function (promise) {

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


                    // GET STUDENT WISE SUBJECT GROUP DETAILS
                    $scope.subjectgroupdetails = [];
                    var countsubgrp = 0;
                    var subjgrpindex = 0;
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.subjectgroupdetails_temp = [];
                        countsubgrp = 0;
                        subjgrpindex = 0;
                        angular.forEach(student.subjectlist, function (sub) {
                            if ($scope.subjectgroupdetails_temp.length === 0) {
                                subjgrpindex += 1;
                                $scope.subjectgroupdetails_temp.push({
                                    ESG_Id: sub.ESG_Id, ESG_SubjectGroupName: sub.ESG_SubjectGroupName,
                                    ESG_CompulsoryFlag: sub.ESG_CompulsoryFlag , grpindex: subjgrpindex
                                });
                            } else if ($scope.subjectgroupdetails_temp.length > 0) {
                                countsubgrp = 0;
                                angular.forEach($scope.subjectgroupdetails_temp, function (subgrp) {
                                    if (subgrp.ESG_Id === sub.ESG_Id) {
                                        countsubgrp += 1;
                                    }
                                });
                                if (countsubgrp === 0) {
                                    subjgrpindex += 1;
                                    $scope.subjectgroupdetails_temp.push({
                                        ESG_Id: sub.ESG_Id, ESG_SubjectGroupName: sub.ESG_SubjectGroupName,
                                        ESG_CompulsoryFlag: sub.ESG_CompulsoryFlag, grpindex: subjgrpindex
                                    });
                                }
                            }
                        });
                        student.subjectgroup = $scope.subjectgroupdetails_temp;
                    });


                    // STUDENT WISE SUBJECT GROUP WITH SUBJECTS 
                    var subjectromnan = 0;
                    $scope.groupsubjectlist = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.groupsubjectlist = [];
                        subjectromnan = 0;
                        angular.forEach(student.subjectgroup, function (subgrp) {
                            $scope.groupsubjectlist = [];
                            subjectromnan = 0;
                            angular.forEach(student.subjectlist, function (subjlist) {
                                if (subgrp.ESG_Id === subjlist.ESG_Id) {
                                    subjectromnan += 1;
                                    $scope.romanfunction(subjectromnan);
                                    $scope.groupsubjectlist.push({
                                        AMST_Id: subjlist.AMST_Id, ISMS_Id: subjlist.ISMS_Id, ISMS_SubjectName: subjlist.ISMS_SubjectName,
                                        EYCES_SubjectOrder: subjlist.EYCES_SubjectOrder, EYCES_AplResultFlg: subjlist.EYCES_AplResultFlg,
                                        ESG_Id: subjlist.ESG_Id, ESG_SubjectGroupName: subjlist.ESG_SubjectGroupName, romannumbersubj: $scope.romnanumber
                                    });
                                }
                            });
                            subgrp.subgrplist = $scope.groupsubjectlist;
                        });
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

                                $scope.termwiseexamlistnew.push({
                                    emE_Id: termexam.emE_Id, emE_ExamName: termexam.emE_ExamName,
                                    ecteX_MarksPercentValue: '(' + termexam.ecteX_MarksPercentValue + ')', ecT_Id: termexam.ecT_Id
                                });
                            }
                        });

                        $scope.termwiseexamlistnew.push({ emE_Id: 5000, emE_ExamName: "TOTAL", ecteX_MarksPercentValue: '(' + totalper + ')', ecT_Id: term.ecT_Id });
                        $scope.termwiseexamlistnew.push({ emE_Id: 5001, emE_ExamName: "GRADE", ecteX_MarksPercentValue: "", ecT_Id: term.ecT_Id });

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
                    angular.forEach($scope.gettermdetails, function (term) {
                        var totalper = 0;
                        var examnamedetails = "";
                        var countexam = 0;
                        angular.forEach($scope.gettermexamdetails, function (termexam) {
                            if (termexam.ecT_Id === term.ecT_Id) {
                                totalper += termexam.ecteX_MarksPercentValue;

                                $scope.termwiseexamlistnewtemp.push({
                                    emE_Id: termexam.emE_Id, emE_ExamName: termexam.emE_ExamName,
                                    ecteX_MarksPercentValue: '(' + termexam.ecteX_MarksPercentValue + ')', ecT_Id: termexam.ecT_Id,
                                    emE_ExamDescription: termexam.emE_ExamDescription
                                });
                            }
                        });
                        $scope.overalltotalper = $scope.overalltotalper + totalper;

                        $scope.termwiseexamlistnewtemp.push({
                            emE_Id: 5000, emE_ExamName: "TOTAL", ecteX_MarksPercentValue: '(' + totalper + ')', ecT_Id: term.ecT_Id,
                            emE_ExamDescription: examnamedetails
                        });

                        $scope.termwiseexamlistnewtemp.push({
                            emE_Id: 5001, emE_ExamName: "GRADE", ecteX_MarksPercentValue: "", ecT_Id: term.ecT_Id,
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
                        $scope.studentwisemarksdetails = [];
                        $scope.studentwisemarkstotaldetails = [];
                        angular.forEach($scope.getstudentmarksdetails, function (marks) {
                            if (student.AMST_Id === marks.AMST_Id && marks.examname !== 'TOTAL GRADE' && marks.subjectname !== 'GRAND TOTAL') {
                                $scope.studentwisemarksdetails.push(marks);
                            }
                            if (student.AMST_Id === marks.AMST_Id && marks.examname !== 'TOTAL GRADE' && marks.subjectname === 'GRAND TOTAL') {
                                $scope.studentwisemarkstotaldetails.push(marks);
                            }
                        });

                        student.markslist = $scope.studentwisemarksdetails;
                        student.markstotal = $scope.studentwisemarkstotaldetails;
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


                    // STUDENT WISE SUBJECT GROUP WISE MARKS
                    angular.forEach($scope.studentdetails, function (student) {
                        angular.forEach(student.subjectgroup, function (subgrp) {
                            angular.forEach(student.markslist, function (marks) {
                                if (subgrp.ESG_Id === marks.subjectid && marks.termid === 9500) {
                                    subgrp.maxmarks = marks.maxmiummarks;
                                    subgrp.obtmarks = marks.marksobtained;
                                    subgrp.obtgrade = marks.grade;
                                }
                            });
                        });
                    });

                    // STUDENT WISE GRAND TOTAL MARKS

                    angular.forEach($scope.studentdetails, function (student) {
                        angular.forEach(student.markslist, function (marks) {
                            if (marks.termid === 9501) {
                                student.maxmarks = marks.maxmiummarks;
                                student.obtmarks = marks.marksobtained;
                                student.obtgrade = marks.grade;
                            }
                        });

                    });

                    console.log("Student List");
                    console.log($scope.studentdetails);
                    $scope.clstchname = promise.clstchname;
                    if ($scope.clstchname !== null && $scope.clstchname.length > 0) {
                        $scope.clastechname = $scope.clstchname[0].hrmE_EmployeeFirstName;
                    }
                    $scope.print = true;

                    angular.forEach($scope.year_list, function (d) {
                        if (d.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.yearname = d.asmaY_Year;
                        }
                    });
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSTermReportCardPdf.css" />' +
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


        $scope.romanfunction = function (num) {
            var lookup = { M: 1000, CM: 900, D: 500, CD: 400, C: 100, XC: 90, l: 50, xl: 40, x: 10, ix: 9, v: 5, iv: 4, i: 1 },
                roman = '',
                i;
            for (i in lookup) {
                while (num >= lookup[i]) {
                    roman += i;
                    num -= lookup[i];
                }
            }
            $scope.romnanumber= roman;
        };
    }
})();