(function () {
    'use strict';
    angular.module('app').controller('StmaryStudentPromotionReportController', StmaryStudentPromotionReportController)
    StmaryStudentPromotionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'Excel', '$timeout', '$compile']
    function StmaryStudentPromotionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, Excel, $timeout, $compile) {

        $scope.reportshowcount = 0;
        $scope.reportdata = true;
        $scope.ASA_FromDate = new Date();

        $scope.examflag = false;
        $scope.termflag = false;

        $scope.btn = true;

        $scope.BindData = function () {
            var pageid = 2;

            apiService.getURI("StudentProgressCardReport/stmarygetdetails", pageid).then(function (promise) {
                $scope.btn = false;
                $scope.getstudentdetails = promise.getstudentdetails;
                $scope.year_list = promise.getyear;
                $scope.classlist = promise.getclass;
                $scope.examorterm = promise.examorterm;

                if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                    $scope.ASMCL_Id = $scope.getstudentdetails[0].asmcL_Id;
                    $scope.asmS_Id = $scope.getstudentdetails[0].asmS_Id;
                    $scope.asmaY_Id = $scope.getstudentdetails[0].asmaY_Id;

                    if ($scope.examorterm === 'Exam') {
                        $scope.getexamtermlist = promise.getexamtermlist;
                        $scope.examflag = true;
                        $scope.termflag = false;

                        if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                            $scope.btn = true;
                            swal("Marks Are Not Published");
                        }
                    }

                    else if ($scope.examorterm === 'Promotion') {
                        $scope.getexamtermlist = promise.getexamtermlist;
                        $scope.examflag = false;
                        $scope.termflag = true;

                        if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                            $scope.btn = true;
                            swal("Marks Are Not Published");
                        }
                    }
                    else {
                        $scope.btn = true;
                        swal("Report Format Not Mapped Contact Administrator");
                    }
                } else {
                    $scope.btn = true;
                    swal("No Studnet Data Found");
                }
            });
        };

        $scope.onchangeclass = function () {
            $scope.getexamtermlist = [];
            $scope.obj.ECT_Id = "";
            $scope.obj.EME_Id = "";
            $scope.examflag = false;
            $scope.termflag = false;
            $scope.reportshowcount = 0;
            $scope.btn = false;
            if ($scope.ASMCL_Id !== undefined && $scope.ASMCL_Id !== null && $scope.ASMCL_Id !== "") {
                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "EPCFT_ExamFlag": "Promotion"
                };

                apiService.create("StudentProgressCardReport/stmaryonchangeclass", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getstudentdetails = promise.getstudentdetails;

                        if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                            $scope.examorterm = promise.examorterm;

                            if ($scope.examorterm === 'Exam') {
                                $scope.getexamtermlist = promise.getexamtermlist;
                                $scope.examflag = true;
                                $scope.termflag = false;

                                if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                                    $scope.btn = true;
                                    swal("Marks Are Not Published");
                                }
                            }

                            else if ($scope.examorterm === 'Promotion') {
                                $scope.getexamtermlist = promise.getexamtermlist;
                                $scope.examflag = false;
                                $scope.termflag = true;

                                if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                                    $scope.btn = true;
                                    swal("Marks Are Not Published");
                                }
                            }
                            else {
                                $scope.btn = true;
                                swal("Report Format Not Mapped Contact Administrator");
                            }
                        } else {
                            $scope.btn = true;
                            swal("No Studnet Data Found");
                        }
                    }
                });
            }
        };

        $scope.onchangeexam = function () {
            $scope.reportshowcount = 0;
        };

        $scope.onchangeterm = function () {
            $scope.reportshowcount = 0;
        };

        $scope.obj = {};

        $scope.saveddata = function (obj) {
            $scope.submitted = true;
            $scope.reportdata = true;
            $scope.reportshowcount = 0;
            if ($scope.myForm.$valid) {
                var emeid = 0;
                if ($scope.examorterm === "Exam") {
                    emeid = obj.EME_Id;
                } else {
                    emeid = 0;
                }
                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "EME_Id": emeid,
                    "EPCFT_ExamFlag": "Promotion"
                };
                apiService.create("StudentProgressCardReport/stmarygetreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.reportshowcount = 0;
                        $scope.generateddate = new Date();
                        $scope.spflag = promise.spflag;
                        $scope.examorterm = promise.examorterm;

                        if (promise.examorterm === "Promotion" && promise.spflag === "1") {
                            if (promise !== null) {

                                $scope.getstudentdetails = promise.getstudentdetails;
                                $scope.getsubjectlist = promise.getsubjectlist;
                                $scope.getexamlist = promise.getexamlist;
                                console.log($scope.getexamlist);
                                $scope.getsavedlist = promise.getsavedlist;
                                $scope.grade_detailslist = promise.grade_detailslist;
                                $scope.promotiondetails = promise.promotiondetails;

                                if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0 && $scope.getsavedlist !== null
                                    && $scope.getsavedlist.length > 0) {
                                    $scope.tempsubjectlist = [];
                                    $scope.subjectlistnew = [];
                                    angular.forEach($scope.getstudentdetails, function (dd) {
                                        $scope.subjectlistnew = [];
                                        angular.forEach($scope.getsubjectlist, function (stuidsub) {
                                            if (dd.amstid === stuidsub.AMST_Id) {
                                                $scope.subjectlistnew.push({ subjectid: stuidsub.ISMS_Id, subjectname: stuidsub.ISMS_SubjectName });
                                            }
                                        });

                                        dd.subjectlist = $scope.subjectlistnew;
                                    });

                                    console.log($scope.getstudentdetails);

                                    $scope.saveddetails = [];
                                    angular.forEach($scope.getstudentdetails, function (student) {
                                        angular.forEach(student.subjectlist, function (subjects) {
                                            $scope.saveddetails = [];
                                            angular.forEach($scope.getsavedlist, function (save) {
                                                if (student.amstid === save.amstid && subjects.subjectid === save.ismsid) {
                                                    $scope.saveddetails.push(save);
                                                }
                                            });

                                            subjects.marksdetails = $scope.saveddetails;
                                        });
                                    });

                                    $scope.totalmarksdetails = [];
                                    angular.forEach($scope.getstudentdetails, function (dd) {
                                        $scope.totalmarksdetails = [];
                                        angular.forEach($scope.getsavedlist, function (save) {
                                            if (dd.amstid === save.amstid && save.ismsid === 5000) {
                                                $scope.totalmarksdetails.push(save);
                                            }
                                        });
                                        dd.totalmarks = $scope.totalmarksdetails;
                                    });


                                    $scope.percentagemarksdetails = [];
                                    angular.forEach($scope.getstudentdetails, function (dd) {
                                        $scope.percentagemarksdetails = [];
                                        angular.forEach($scope.getsavedlist, function (save) {
                                            if (dd.amstid === save.amstid && save.ismsid === 5001) {
                                                $scope.percentagemarksdetails.push(save);
                                            }
                                        });
                                        dd.percentagemarks = $scope.percentagemarksdetails;
                                    });

                                    $scope.workingdaysdetails = [];
                                    angular.forEach($scope.getstudentdetails, function (dd) {
                                        $scope.workingdaysdetails = [];
                                        angular.forEach($scope.getsavedlist, function (save) {
                                            if (dd.amstid === save.amstid && save.ismsid === 5002) {
                                                $scope.workingdaysdetails.push(save);
                                            }
                                        });
                                        dd.workingdays = $scope.workingdaysdetails;
                                    });

                                    $scope.attedancedetails = [];
                                    angular.forEach($scope.getstudentdetails, function (dd) {
                                        $scope.attedancedetails = [];
                                        angular.forEach($scope.getsavedlist, function (save) {
                                            if (dd.amstid === save.amstid && save.ismsid === 5003) {
                                                $scope.attedancedetails.push(save);
                                            }
                                        });
                                        dd.attendance = $scope.attedancedetails;
                                    });

                                    $scope.attendanceper = [];
                                    angular.forEach($scope.getstudentdetails, function (dd) {
                                        $scope.attendanceper = [];
                                        angular.forEach($scope.getsavedlist, function (save) {
                                            if (dd.amstid === save.amstid && save.ismsid === 5004) {
                                                $scope.attendanceper.push(save);
                                            }
                                        });
                                        dd.attendanceperdetails = $scope.attendanceper;
                                    });

                                    $scope.rank = [];
                                    angular.forEach($scope.getstudentdetails, function (dd) {
                                        $scope.rank = [];
                                        angular.forEach($scope.getsavedlist, function (save) {
                                            if (dd.amstid === save.amstid && save.ismsid === 5005) {
                                                $scope.rank.push(save);
                                            }
                                        });
                                        dd.rankdetails = $scope.rank;
                                    });


                                    console.log("MARKS");
                                    console.log($scope.getstudentdetails);

                                    angular.forEach($scope.getstudentdetails, function (d) {
                                        angular.forEach($scope.promotiondetails, function (e) {
                                            if (d.amstid === e.amsT_Id) {
                                                d.resultpassfail = e.estmpP_TotalGrade + ' ' + e.estmpP_Result;
                                            }
                                        });
                                    });
                                    $scope.year = promise.getyear[0].asmaY_Year;
                                     
                                    $scope.report = true;
                                    $scope.printbutton = false;
                                    $scope.reportshowcount = 1;
                                } else {
                                    swal("No Records Found");
                                }
                            } else {
                                swal("No Records Found");
                            }
                        }
                        else {
                            swal("Report Format Not Mapped Contact Administrator");
                        }

                        if ($scope.reportshowcount === 1) {
                            var e1 = angular.element(document.getElementById("report"));
                            $compile(e1.html(promise.htmlstring))(($scope));
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
            }
        };

        $scope.print_HHS02 = function () {
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.examorterm === "Exam") {
                innerContents = document.getElementById("printareaId").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/ProgressCardReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            if ($scope.examorterm === "Promotion" && $scope.spflag === "1") {
                innerContents = document.getElementById("Baldwin").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/MaldaProgressReportPdf .css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

            if ($scope.examorterm === "Promotion" && $scope.spflag === "2") {
                innerContents = document.getElementById("HHS02").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
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
            apiService.create("StudentProgressCardReport/get_Exam_grade_pc", data).then(function (promise) {
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
            $scope.romnanumber = roman;
        };
    }
})();