(function () {
    'use strict';
    angular.module('app').controller('JSHSXIthTermReportController', JSHSXIthTermReportController)
    JSHSXIthTermReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function JSHSXIthTermReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

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
            $scope.studentlist = [];
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
            $scope.studentlist = [];
            $scope.AMST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("JSHSExamReports/get_students_category_grade", data).then(function (promise) {
                $scope.grade_list = promise.getgradelist;
                $scope.termlistd = promise.gettermlist;

                if ($scope.termlistd !== null && $scope.termlistd.length > 0) {
                    $scope.termlist = promise.gettermlist;
                } else {
                    swal("No Term Is Mapped For Selected Details");
                    $scope.termlist = [];
                }
            });
        };

        $scope.onchangeterm = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
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

                var data = {
                    "AMST_Id": 0,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EMGR_Id": $scope.EMGR_Id,
                    "ECT_Id": $scope.ECT_Id
                };

                apiService.create("JSHSExamReports/get_individualtermreport", data).then(function (promise) {

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

                    $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;

                    $scope.getstudentwisetermwisedetails = promise.getstudentwisetermwisedetails;

                    $scope.termexammarkssubheader = [];
                    $scope.countcolspan = 0;

                    angular.forEach($scope.gettermexamdetails, function (dd) {
                        $scope.termexammarkssubheader = [];
                        $scope.termexammarkssubheader.push({ ecT_Id: dd.ecT_Id, emE_Id: dd.emE_Id, columnname: 'MAX.MARKS' });
                        $scope.termexammarkssubheader.push({ ecT_Id: dd.ecT_Id, emE_Id: dd.emE_Id, columnname: 'MARKS OBTAINED' });
                        dd.subheader = $scope.termexammarkssubheader;
                    });
                    $scope.termexammarks = [];
                    angular.forEach($scope.gettermexamdetails, function (dd) {
                        $scope.termexammarks.push({ ecT_Id: dd.ecT_Id, emE_Id: dd.emE_Id, columnname: 'MAX.MARKS' });
                        $scope.termexammarks.push({ ecT_Id: dd.ecT_Id, emE_Id: dd.emE_Id, columnname: 'MARKS OBTAINED' });
                    });


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

                    //STUDENT WISE ATTENDANCE DETAILS
                    angular.forEach($scope.studentdetails, function (student) {
                        angular.forEach($scope.getstudentwiseattendancedetails, function (att) {
                            if (student.AMST_Id === att.AMST_Id) {
                                student.presentdays = att.PRESENTDAYS;
                                student.totalworkingdays = att.TOTALWORKINGDAYS;
                            }
                        });
                    });

                    // STUDENT WISE TERM WISE REMARKS DETAILS

                    angular.forEach($scope.studentdetails, function (stu) {
                        angular.forEach($scope.getstudentwisetermwisedetails, function (d) {
                            if (stu.AMST_Id === d.AMST_Id) {
                                stu.remarks = d.ECTERE_Remarks;
                            }
                        });
                    });

                    // STUDENT WISE SKILLS LIST 
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

                    console.log($scope.studentdetails);


                    angular.forEach($scope.termlist, function (dd) {
                        if (dd.ecT_Id === parseInt($scope.ECT_Id)) {
                            $scope.termname = dd.ecT_TermName.toUpperCase();
                        }
                    });

                    angular.forEach($scope.year_list, function (dd) {
                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.yearname = dd.asmaY_Year;
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSXITermReportCardPdf.css" />' +
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