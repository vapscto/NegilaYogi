(function () {
    'use strict';
    angular.module('app').controller('JSHSIXthTermReportController', JSHSIXthTermReportController)
    JSHSIXthTermReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window']
    function JSHSIXthTermReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window) {

        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.termlistd = [];
        $scope.obj = {};
        $scope.generateddate = new Date();

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
            $scope.studentlist = [];
            $scope.termlist = [];
            $scope.AMST_Id = "";
            $scope.class_list = [];
            $scope.section_list = [];
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
            $scope.termlist = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.AMST_Id = "";
            $scope.section_list = [];
            $scope.ASMS_Id = "";

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
            $scope.termlist = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.AMST_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("JSHSExamReports/get_students_category_grade", data).then(function (promise) {
                $scope.studentlistd = promise.getstudentlist;
                $scope.grade_list = promise.getgradelist;
                $scope.termlistd = promise.gettermlist;

                if ($scope.studentlistd !== null && $scope.studentlistd.length > 0) {
                    $scope.studentlist = promise.getstudentlist;
                } else {
                    swal("No Student List Found For Selected Details");
                    return;
                }

                if ($scope.termlistd !== null && $scope.termlistd.length > 0) {
                    $scope.termlist = promise.gettermlist;
                } else {
                    swal("No Term Is Mapped For Selected Details");
                    $scope.termlist = [];
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
        $scope.saveddata = function (obj) {

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
                    "AMST_Id": 0,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EMGR_Id": $scope.EMGR_Id,
                    "ECT_Id": $scope.ECT_Id
                };

                apiService.create("JSHSExamReports/getixtermreport", data).then(function (promise) {

                    $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                    $scope.JSHSReport = true;
                    $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;
                    $scope.getgradedetails = promise.getgradedetails;
                    $scope.gettermdetails = promise.gettermdetails;
                    $scope.gettermexamdetails = promise.gettermexamdetails;

                    $scope.getgroupdetails = promise.getgroupdetails;

                    $scope.subcolumnsexamdetails = [];

                    angular.forEach($scope.getgroupdetails, function (dd) {
                        $scope.subcolumnsexamdetails.push({
                            columdisplay: dd.empG_DistplayName, columgroup: dd.empG_GroupName, colummarks: '(' + dd.empsG_PercentValue + ')'
                        });
                    });

                    $scope.subcolumnsexamdetails.push({ columnid: 500000, columdisplay: 'MARKS OBTAINED', columgroup: 'Subject Total', colummarks: '(100)' });
                    $scope.subcolumnsexamdetails.push({ columnid: 500001, columdisplay: 'Grade', columgroup: 'Subject Grade', colummarks: '' });

                    $scope.studentdetails = promise.getstudentdetails;

                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;

                    $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;

                    $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;

                    $scope.getstudentwisesportsdetails = promise.getstudentwisesportsdetails;

                    $scope.getpromotionmarksdetails = promise.getpromotionmarksdetails;


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


                    // STUDENT WISE SKILL  LIST 
                    $scope.studentwiseskillslist = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.studentwiseskillslist = [];
                        angular.forEach($scope.getstudentwiseskillslist, function (skills) {
                            if (student.AMST_Id === skills.AMST_Id) {
                                $scope.studentwiseskillslist.push(skills);
                            }
                        });
                        student.skillslist = $scope.studentwiseskillslist;
                    });


                    // STUDENT WISE Activites LIST 
                    $scope.studentwiseactiviteslist = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.studentwiseactiviteslist = [];
                        angular.forEach($scope.getstudentwiseactiviteslist, function (activites) {
                            if (student.AMST_Id === activites.AMST_Id) {
                                $scope.studentwiseactiviteslist.push(activites);
                            }
                        });
                        student.activiteslist = $scope.studentwiseactiviteslist;
                    });


                    // STUDENT WISE Sports LIST 
                    $scope.studentwisesportslist = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        angular.forEach($scope.getstudentwisesportsdetails, function (sport) {
                            if (student.AMST_Id === sport.AMST_Id) {
                                student.height = sport.SPCCSHW_Height;
                                student.weight = sport.SPCCSHW_Weight;
                            }
                        });
                    });

                    $scope.colspan_sport = $scope.termlisttemp.length * 2;

                    $scope.colspan_sport_head = $scope.colspan_sport * 2;

                    // STUDENT WISE MARKS LIST 

                    $scope.studentwisemarksdetails = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.studentwisemarksdetails = [];
                        angular.forEach($scope.getstudentmarksdetails, function (marks) {
                            if (student.AMST_Id === marks.AMST_Id) {
                                $scope.studentwisemarksdetails.push(marks);
                            }
                        });
                        student.markslist = $scope.studentwisemarksdetails;
                    });

                    $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;

                    angular.forEach($scope.studentdetails, function (dd) {
                        angular.forEach($scope.getstudentwiseattendancedetails, function (att) {
                            if (att.AMST_Id === dd.AMST_Id) {
                                dd.presentdays = att.PRESENTDAYS;
                                dd.totalworkingdays = att.TOTALWORKINGDAYS;
                                dd.totalpercentage = att.ATTENDANCEPERCENTAGE;
                            }
                        });
                    });

                    $scope.getstudentwisetermwisedetails = promise.getstudentwisetermwisedetails;

                    angular.forEach($scope.studentdetails, function (dd) {
                        angular.forEach($scope.getstudentwisetermwisedetails, function (d) {
                            if (dd.AMST_Id === d.AMST_Id) {
                                dd.remarks = d.ECTERE_Remarks;
                            }
                        });
                    });

                    angular.forEach($scope.studentdetails, function (dd) {
                        angular.forEach($scope.getpromotionmarksdetails, function (ddd) {

                            if (dd.AMST_Id === ddd.amsT_Id) {
                                dd.overallmaxmarks = ddd.estmpP_TotalMaxMarks;
                                dd.overallobtainedmarks = Math.round(ddd.estmpP_TotalObtMarks);
                                dd.overallgrade = ddd.estmpP_TotalGrade;
                            }
                        });
                    });

                    console.log($scope.studentdetails);

                    angular.forEach($scope.year_list, function (dd) {
                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.yearname = dd.asmaY_Year;
                        }
                    });

                    angular.forEach($scope.termlist, function (dd) {
                        if (dd.ecT_Id === parseInt($scope.ECT_Id)) {
                            $scope.termname = dd.ecT_TermName;
                        }
                    });

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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSTermReportCardPdf.css" />' +
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

        $scope.saveddatadata = function (obj) {

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
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EMGR_Id": $scope.EMGR_Id,
                    "termlist": $scope.termlisttemp
                };

                apiService.create("JSHSExamReports/get_reportdetails", data).then(function (promise) {

                    $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                    $scope.JSHSReport = true;
                    $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;
                    $scope.getgradedetails = promise.getgradedetails;
                    $scope.gettermdetails = promise.gettermdetails;
                    $scope.gettermexamdetails = promise.gettermexamdetails;

                    $scope.studentdetails = promise.getstudentdetails;

                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;

                    $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;

                    $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;

                    $scope.getstudentwisesportsdetails = promise.getstudentwisesportsdetails;


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

                    // STUDENT WISE SKILLS LIST 
                    $scope.studentwiseskillslist = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.studentwisesubject = [];
                        angular.forEach($scope.getstudentwiseskillslist, function (skills) {
                            if (student.AMST_Id === skills.AMST_Id) {
                                $scope.studentwiseskillslist.push(skills);
                            }
                        });
                        student.skillslist = $scope.studentwiseskillslist;
                    });

                    // STUDENT WISE Activites LIST 
                    $scope.studentwiseactiviteslist = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.studentwisesubject = [];
                        angular.forEach($scope.getstudentwiseactiviteslist, function (activites) {
                            if (student.AMST_Id === activites.AMST_Id) {
                                $scope.studentwiseactiviteslist.push(activites);
                            }
                        });
                        student.activiteslist = $scope.studentwiseactiviteslist;
                    });

                    // STUDENT WISE Sports LIST 
                    $scope.studentwisesportslist = [];
                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.studentwisesubject = [];
                        angular.forEach($scope.getstudentwisesportsdetails, function (sport) {
                            if (student.AMST_Id === sport.AMST_Id) {
                                student.height = sport.SPCCSHW_Height;
                                student.weight = sport.SPCCSHW_Weight;
                            }
                        });

                    });


                    // STUDENT WISE MARKS LIST 

                    $scope.studentwisemarksdetails = [];

                    angular.forEach($scope.studentdetails, function (student) {
                        $scope.studentwisesubject = [];
                        angular.forEach($scope.getstudentmarksdetails, function (marks) {
                            if (student.AMST_Id === marks.AMST_Id) {
                                $scope.studentwisemarksdetails.push(marks);
                            }
                        });
                        student.markslist = $scope.studentwisemarksdetails;
                    });

                    console.log($scope.studentdetails);
                });
                console.log($scope.exm_sub_mrks_list);
                console.log($scope.total_subwise);
            } else {
                $scope.submitted = true;
            }
        };
    }
})();