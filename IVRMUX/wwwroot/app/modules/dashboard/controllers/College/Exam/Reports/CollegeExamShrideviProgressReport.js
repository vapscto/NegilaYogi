
(function () {
    'use strict';
    angular.module('app').controller('CollegeExamShrideviProgressReportController', CollegeExamShrideviProgressReportController)
    CollegeExamShrideviProgressReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function CollegeExamShrideviProgressReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.searchchkbx = "";

        $scope.dateofissue = new Date();
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("ClgCumulativeReport/Getdetails").then(function (promise) {
                $scope.yearlist = promise.yearlist;
                $scope.datagriv = false;
            });
        };


        $scope.onchangeyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.searchchkbx = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ClgCumulativeReport/onchangeyear", data).then(function (promise) {
                $scope.course_list = promise.courseslist;
            });
        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.searchchkbx = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("ClgCumulativeReport/onchangecourse", data).then(function (promise) {
                $scope.branch_list = promise.branchlist;
            });
        };

        $scope.onchangebranch = function () {

            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.searchchkbx = "";
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("ClgCumulativeReport/onchangebranch", data).then(function (promise) {
                $scope.semisters_list = promise.semisters;
            });
        };

        $scope.onchangesemester = function () {
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.searchchkbx = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("ClgCumulativeReport/onchangesemester", data).then(function (promise) {
                $scope.seclist = promise.sections;
                $scope.subjectschema_list = promise.subjectshemalist;
            });
        };

        $scope.onchangesubjectscheme = function () {
            $scope.EME_Id = "";
            $scope.ACST_Id = "";
            $scope.searchchkbx = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id
            };
            apiService.create("ClgCumulativeReport/onchangesubjectscheme", data).then(function (promise) {
                $scope.schmetype_list = promise.schmetypelist;
            });
        };


        $scope.onchangeschemetype = function () {
            $scope.EME_Id = "";
            $scope.searchchkbx = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.studentlist = [];
            $scope.exam_list = [];
            $scope.student_list = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "ACST_Id": $scope.ACST_Id,
                "ACMS_Id": $scope.ACMS_Id
            };
            apiService.create("ClgCumulativeReport/onchangeschemetype", data).then(function (promise) {
                $scope.exam_list = promise.exmstdlist;
                $scope.studentlist = promise.studentlist;
                $scope.all = true;
                $timeout(function () { $scope.OnClickAll(); }, 1000);
            });
        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.Getreport = function () {
            $scope.submitted = true;
            $scope.searchchkbx = "";
            $scope.studentslt1 = [];
            $scope.student_list = [];
            $scope.student_list = [];
            $scope.examname = "";
            if ($scope.myForm.$valid) {
                $scope.studentlist_temp = [];
                angular.forEach($scope.studentlist, function (d) {
                    if (d.checkedsub) { $scope.studentlist_temp.push({ AMCST_Id: d.amcsT_Id }); }
                });
                angular.forEach($scope.exam_list, function (d) {
                    if (d.emE_Id == $scope.EME_Id) {
                        $scope.examname = d.emE_ExamName;
                    }
                });

                var data = {
                    "EME_ID": $scope.EME_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "ACSS_Id": $scope.ACSS_Id,
                    "ACST_Id": $scope.ACST_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "Studentlist_temp": $scope.studentlist_temp,
                    "graderemark": "Shridevi"
                };

                apiService.create("ClgCumulativeReport/GetProgresscardReport", data).then(function (promise) {
                    $scope.report = true;
                    if (promise.getProgressCardReportList !== null && promise.getProgressCardReportList.length > 0) {
                        $scope.student_list = promise.getProgressCardReportList;
                        if (promise.exmstdlist != null && promise.exmstdlist.length > 0) {
                            angular.forEach($scope.student_list, function (student) {
                                $scope.student_wisemarks = [];
                                angular.forEach(promise.exmstdlist, function (exam) {
                                    if (student.AMCST_Id == exam.AMCST_Id) {
                                        $scope.student_wisemarks.push(exam);
                                    }
                                    student.student_wisemarks = $scope.student_wisemarks;
                                });
                            });      
                        }
                        if (promise.subjectshemalist != null && promise.subjectshemalist.length > 0) {
                            angular.forEach($scope.student_list, function (student) {
                                $scope.subjectattenence = [];
                                angular.forEach(promise.subjectshemalist, function (exam) {
                                    if (student.AMCST_Id == exam.AMCST_Id) {
                                        $scope.subjectattenence.push(exam);
                                    }
                                    student.subjectattenence = $scope.subjectattenence;
                                });
                            });
                        }
                        $scope.subjectschema_list = [];

                        if (promise.nonsubjlist != null && promise.nonsubjlist.length > 0) {                            
                            angular.forEach($scope.student_list, function (student) {
                                $scope.student_subjectlist = [];
                                angular.forEach(promise.nonsubjlist, function (exam) {
                                    if (student.AMCST_Id == exam.AMCST_Id) {
                                        $scope.student_subjectlist.push(exam);
                                    }
                                    student.student_subjectlist = $scope.student_subjectlist;
                                });
                            });

                        }
                       
                    } else {
                        swal("No Records Found");
                    }
                });
            }
        };

        $scope.exportToExcel = function (tableIds) {
            var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.obj = {};
        $scope.studentlist = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BBIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.OnClickAll = function () {
            var checkStatus = $scope.all;
            var count = 0;
            angular.forEach($scope.studentlist, function (itm) {
                itm.checkedsub = checkStatus;
                if (itm.checkedsub == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }


        $scope.isOptionsRequired3 = function () {
            return !$scope.studentlist.some(function (options) {
                return options.checkedsub;
            });
        }

        $scope.individual = function () {

            $scope.all = $scope.studentlist.every(function (options) {
                return options.checkedsub;
            });
        }

        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.print = function () {
            //var innerContents = document.getElementById("printSectionId").innerHTML;
            //var popupWinindow = window.open('');
            //popupWinindow.document.open();
            //popupWinindow.document.write('<html><head>' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
            //    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
            //    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
            //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
         //   popupWinindow.document.close();
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BBIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.Clear_Details = function () {
            $state.reload();
        };
    }
})();