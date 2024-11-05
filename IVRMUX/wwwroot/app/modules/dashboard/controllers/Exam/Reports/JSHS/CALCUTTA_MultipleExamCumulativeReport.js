(function () {
    'use strict';
    angular.module('app').controller('CALCUTTA_MultipleExamCumulativeReportController', JSHSMultipleExamCumulativeReportController)
    JSHSMultipleExamCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'Excel','$timeout']
    function JSHSMultipleExamCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, Excel, $timeout) {

        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.getmultipleexamcumulativereport_temp = [];
        $scope.examdetails = [];
        $scope.examlisttemp = [];
        $scope.obj = {};
        $scope.reportdata = true;

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.reportdata = true;
            $scope.getmultipleexamcumulativereport_temp = [];
            $scope.examdetails = [];
            $scope.examlisttemp = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            $scope.JSHSReport = false;
            $scope.reportdata = true;
            $scope.getmultipleexamcumulativereport_temp = [];
            $scope.examdetails = [];
            $scope.examlisttemp = [];
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
            $scope.reportdata = true;
            $scope.examdetails = [];
            $scope.getmultipleexamcumulativereport_temp = [];
            $scope.examlisttemp = [];
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.examlist.some(function (options) {
                return options.EME_Id;
            });
        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {
            $scope.reportdata = true;
            $scope.JSHSReport = false;
            $scope.getmultipleexamcumulativereport_temp = [];
            $scope.examdetails = [];
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.examlisttemp = [];
                angular.forEach($scope.examlist, function (term) {
                    if (term.EME_Id === true) {
                        $scope.examlisttemp.push({ EME_Id: term.emE_Id, EME_ExamName: term.emE_ExamName });
                    }
                });
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EMGR_Id": $scope.EMGR_Id,
                    "examlist": $scope.examlisttemp
                };

                apiService.create("JSHSExamReports/get_multipleexam_reportdetails", data).then(function (promise) {

                    $scope.getmultipleexamcumulativereport_temp = promise.getmultipleexamcumulativereport;
                    if ($scope.getmultipleexamcumulativereport_temp !== null && $scope.getmultipleexamcumulativereport_temp.length > 0) {
                        $scope.reportdata = false;
                        $scope.JSHSReport = true;
                        $scope.getstudentmarksdetails = $scope.getmultipleexamcumulativereport_temp;

                        $scope.getgradedetails = promise.getgradedetails;
                        $scope.getexamdetails = promise.getexamdetails;

                        $scope.examdetails = [];

                        angular.forEach($scope.getexamdetails, function (ex) {
                            $scope.examdetails.push({ EME_Id: ex.emE_Id, EME_ExamName: ex.emE_ExamName });
                        });

                        $scope.examdetails.push({ EME_Id: 50000, EME_ExamName: 'TOTAL' });
                       // $scope.examdetails.push({ EME_Id: 50001, EME_ExamName: 'GRADE' });
                        //$scope.examdetails.push({ EME_Id: 50000, EME_ExamName: 'percentage' });


                        $scope.studentdetails = [];

                        angular.forEach($scope.getstudentmarksdetails, function (dd) {
                            if ($scope.studentdetails.length === 0) {
                                $scope.studentdetails.push({ AMST_Id: dd.AMST_Id, studentname: dd.studentname, admno: dd.admno, regno: dd.regno, rollno: dd.rollno, Attendence: dd.Attendence });
                            } else if ($scope.studentdetails.length > 0) {
                                var count = 0;
                                angular.forEach($scope.studentdetails, function (d) {
                                    if (d.AMST_Id === dd.AMST_Id) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.studentdetails.push({ AMST_Id: dd.AMST_Id, studentname: dd.studentname, admno: dd.admno, regno: dd.regno, rollno: dd.rollno, Attendence: dd.Attendence });
                                }
                            }
                        });
                        $scope.temp_markslist = [];

                        angular.forEach($scope.studentdetails, function (stu) {
                            $scope.temp_markslist = [];
                            angular.forEach($scope.examdetails, function (exm) {
                                angular.forEach($scope.getstudentmarksdetails, function (marks) {
                                    if (stu.AMST_Id === marks.AMST_Id && exm.EME_Id === marks.examid) {
                                        if (marks.examid !== 50001) {
                                            $scope.temp_markslist.push({ EME_Id: marks.examid, marksobtained: marks.marksobtained, maxmarks: marks.maxmiummarks, percentage: marks.percentage });
                                        } else {
                                            $scope.temp_markslist.push({ EME_Id: marks.examid, marksobtained: marks.grade, maxmarks: marks.maxmiummarks });
                                        }                                       
                                    }
                                });
                            });
                            stu.student_marks = $scope.temp_markslist;
                        });
                        console.log($scope.studentdetails);
                        $scope.colspan = $scope.examdetails.length + 3;

                        $scope.getinstitution = promise.getinstitution;

                        $scope.inst_name = $scope.getinstitution[0].mI_Name;

                        $scope.yearname = "";
                        angular.forEach($scope.year_list, function (yr) {
                            if (yr.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.yearname = yr.asmaY_Year;
                            }
                        });

                        $scope.classname = "";
                        angular.forEach($scope.class_list, function (cls) {
                            if (cls.asmcL_Id === parseInt($scope.ASMCL_Id)) {
                                $scope.classname = cls.asmcL_ClassName;
                            }
                        });

                        $scope.sectionname = "";
                        angular.forEach($scope.section_list, function (sec) {
                            if (sec.asmS_Id === parseInt($scope.ASMS_Id)) {
                                $scope.sectionname = sec.asmC_SectionName;
                            }
                        });

                    } else {
                        swal("No Records Found");
                    }
                });

            } else {
                $scope.submitted = true;
            }
        };

        $scope.print_HHS02 = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        //to excel
        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
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