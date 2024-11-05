(function () {
    'use strict';
    angular.module('app').controller('MaldaIXTermReportController', MaldaIXTermReportController)
    MaldaIXTermReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function MaldaIXTermReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

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
            apiService.getURI("MaldaProgressReportExam/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.yearlist;
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.studentdetails = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.AMST_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("MaldaProgressReportExam/onchangeyear", data).then(function (promise) {
                $scope.class_list = promise.classlist;
            });
        };

        $scope.onclasschange = function () {
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.AMST_Id = "";
            $scope.ASMS_Id = "";

            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("MaldaProgressReportExam/onchangeclass", data).then(function (promise) {
                $scope.section_list = promise.seclist;
            });
        };

        //-----------section Selection
        $scope.onsectionchange = function () {
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
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

            apiService.create("MaldaProgressReportExam/onchangesection", data).then(function (promise) {
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
            $scope.studentdetails = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;

            if ($scope.myForm.$valid) {               
                var data = {
                    "AMST_Id": 0,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id
                };

                apiService.create("MaldaProgressReportExam/ixpromotionreport", data).then(function (promise) {

                    $scope.JSHSReport = true;

                    $scope.institution = promise.instname;
                    if ($scope.institution !== null && $scope.institution.length > 0) {
                        $scope.inst_name = $scope.institution[0].mI_Name;
                        $scope.add = $scope.institution[0].mI_Address1;
                        $scope.city = $scope.institution[0].ivrmmcT_Name;
                        $scope.pin = $scope.institution[0].mI_Pincode;
                    }

                    $scope.getgrouplist = promise.getgrouplist;
                    $scope.studentdetails = promise.getstudentdetails;
                    $scope.getstudentwisesubjectlist = promise.getsubjectlist;
                    $scope.getstudentmarksdetails = promise.getsavedlist;
                    $scope.getattendanceranklist = promise.getattendanceranklist;

                    $scope.getgradedetails = promise.grade_detailslist;


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
                    var snocount = 0;
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.subjectgroupdetails_temp = [];
                        countsubgrp = 0;
                        snocount = 0;
                        angular.forEach(student.subjectlist, function (sub) {
                            if ($scope.subjectgroupdetails_temp.length === 0) {
                                snocount += 1;
                                $scope.subjectgroupdetails_temp.push({ ESG_Id: sub.ESG_Id, ESG_SubjectGroupName: sub.ESG_SubjectGroupName, snocountd: snocount });
                            } else if ($scope.subjectgroupdetails_temp.length > 0) {
                                countsubgrp = 0;
                                angular.forEach($scope.subjectgroupdetails_temp, function (subgrp) {
                                    if (subgrp.ESG_Id === sub.ESG_Id) {
                                        countsubgrp += 1;
                                    }
                                });
                                if (countsubgrp === 0) {
                                    snocount += 1;
                                    $scope.subjectgroupdetails_temp.push({ ESG_Id: sub.ESG_Id, ESG_SubjectGroupName: sub.ESG_SubjectGroupName, snocountd: snocount });
                                }
                            }
                        });
                        student.subjectgroup = $scope.subjectgroupdetails_temp;
                    });


                    // STUDENT WISE SUBJECT GROUP WITH SUBJECTS 
                    $scope.groupsubjectlist = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.groupsubjectlist = [];
                        angular.forEach(student.subjectgroup, function (subgrp) {
                            $scope.groupsubjectlist = [];
                            angular.forEach(student.subjectlist, function (subjlist) {
                                if (subgrp.ESG_Id === subjlist.ESG_Id) {
                                    $scope.groupsubjectlist.push(subjlist);
                                }
                            });
                            subgrp.subgrplist = $scope.groupsubjectlist;
                        });
                    });


                    // STUDENT WISE EXAM LIST
                    $scope.subexamheader = [];
                    $scope.subexamheader.push({ colmid: 1, colmhead: "HIGHEST MARKS" , rowspan:6 });
                    $scope.subexamheader.push({ colmid: 2, colmhead: "OBTAINED MARKS", rowspan: 1 });

                    $scope.examwisecolmheads = [];
                    angular.forEach($scope.getgrouplist, function (d) {
                        $scope.examwisecolmheadstemp = [];
                        angular.forEach($scope.subexamheader, function (dd) {
                            $scope.examwisecolmheads.push({
                                EMPSG_GroupName: d.EMPSG_GroupName, EMPSG_DisplayName: d.EMPSG_DisplayName,
                                colmid: dd.colmid, colmhead: dd.colmhead, colmrowspan: dd.rowspan
                            });
                            $scope.examwisecolmheadstemp.push({
                                EMPSG_GroupName: d.EMPSG_GroupName, EMPSG_DisplayName: d.EMPSG_DisplayName,
                                colmid: dd.colmid, colmhead: dd.colmhead, colmrowspan: dd.rowspan
                            });
                        });
                        d.headecolm = $scope.examwisecolmheadstemp;
                    });

                    console.log($scope.examwisecolmheads);

                    // STUDENT WISE MARKS LIST 
                    $scope.studentwisemarksdetails = [];
                    $scope.studentwisemarkstotaldetails = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.studentwisemarksdetails = [];
                        angular.forEach($scope.getstudentmarksdetails, function (dd) {
                            if (student.AMST_Id === dd.AMST_Id && dd.EMPSG_GroupName !== 'Final Cumulative') {
                                $scope.studentwisemarksdetails.push(dd);
                            }
                        });
                        student.markslist = $scope.studentwisemarksdetails;
                    });

                    /* STUDENT GROUP WISE TOTAL MARKS */
                    angular.forEach($scope.studentdetails, function (dd) {
                        angular.forEach(dd.subjectgroup, function (ddd) {
                            angular.forEach($scope.getstudentmarksdetails, function (d) {
                                if (dd.AMST_Id === d.AMST_Id && ddd.ESG_SubjectGroupName === d.ISMS_SubjectName && d.EMPSG_GroupName === 'Final Cumulative') {
                                    ddd.overalltotalmark = d.ESTMPPSG_GroupObtMarks + "/" + d.ESTMPPSG_GroupMaxMarks;
                                    ddd.totalfinalmarks = d.PER;
                                    ddd.totalgrade = d.GroupSectionMaxGrade;

                                }
                            });
                        });                       
                    });

                    /*STUDENT WISE ATTENDANCE AND RANK DETAILS */
                    $scope.attnrank = [];
                    angular.forEach($scope.studentdetails, function (dd) {
                        $scope.attnrank = [];
                        angular.forEach($scope.getattendanceranklist, function (d) {
                            if (d.AMST_Id === dd.AMST_Id) {
                                $scope.attnrank.push(d);
                            }
                        });
                        dd.attendancerank = $scope.attnrank;
                    });


                    /*STUDENT WISE FINAL TOTAL*/
                    angular.forEach($scope.studentdetails, function (dd) {                        
                        angular.forEach($scope.getstudentmarksdetails, function (d) {
                            if (d.AMST_Id === dd.AMST_Id && d.ISMS_SubjectName === 'Overall Final') {
                                dd.totalmaxmarks = d.ESTMPPSG_GroupMaxMarks;
                                dd.totalobtainedmarks = d.ESTMPPSG_GroupObtMarks;
                                dd.totalgradeobtained = d.GroupSectionMaxGrade;
                                dd.totalpercentage = d.PER;
                                dd.total = d.ESTMPPSG_GroupObtMarks + "/" + d.ESTMPPSG_GroupMaxMarks;                                
                            }
                        });                         
                    });

                    $scope.getoveralldetails = promise.getoveralldetails;

                    angular.forEach($scope.studentdetails, function (dd) {
                        angular.forEach($scope.getoveralldetails, function (d) {
                            if (d.amsT_Id === dd.AMST_Id) {
                                dd.totalsectionrank = d.estmpP_SectionRank;                              
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/MaldaProgressIXReportPdf.css" />' +
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