﻿

(function () {
    'use strict';
    angular.module('app').controller('MultipleExamCumulativeReportController', MultipleExamCumulativeReportController)
    MultipleExamCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window','Excel', '$timeout']
    function MultipleExamCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window, Excel, $timeout) {

        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.getexamlist = [];
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
            $scope.class_list = [];
            $scope.ASMCL_Id = "";
            $scope.section_list = [];
            $scope.ASMS_Id = "";
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            $scope.section_list = [];
            $scope.ASMS_Id = "";
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.JSHSReport = false;
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
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.JSHSReport = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("JSHSExamReports/get_Exam_grade", data).then(function (promise) {
                $scope.grade_list = promise.getallgrade;
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
            return !$scope.getexamlist.some(function (options) {
                return options.EME_Id;
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
                angular.forEach($scope.getexamlist, function (term) {
                    if (term.EME_Id === true) {
                        $scope.termlisttemp.push({ EME_Id: term.emE_Id, EME_ExamName: term.emE_ExamName });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EMGR_Id": $scope.EMGR_Id,
                    "examlist": $scope.termlisttemp
                };

                apiService.create("JSHSExamReports/getmultiple_exam_cumulative_report", data).then(function (promise) {

                    if (promise !== null) {

                        if (promise.getcumulativereportdetails !== null && promise.getcumulativereportdetails.length > 0) {
                            $scope.getcumulativereportdetails = promise.getcumulativereportdetails;
                            $scope.getsubjectslist = promise.getsubjectslist;

                            $scope.subcolmns = [];
                            $scope.JSHSReport = true;
                            angular.forEach($scope.getsubjectslist, function (dd) {
                                $scope.subcolmns.push({ subjid: dd.ISMS_Id, subjcolname: 'M' });
                                $scope.subcolmns.push({ subjid: dd.ISMS_Id, subjcolname: 'G' });

                            });

                            $scope.studentlistd = [];
                            angular.forEach($scope.getcumulativereportdetails, function (dd) {
                                if ($scope.studentlistd.length === 0) {
                                    $scope.studentlistd.push({ AMST_Id: dd.AMST_Id, studentname: dd.STUDENTNAME, admno: dd.ADMNO, regno: dd.REGNO });
                                } else if ($scope.studentlistd.length > 0) {
                                    var count = 0;
                                    angular.forEach($scope.studentlistd, function (d) {
                                        if (parseInt(d.AMST_Id) === parseInt(dd.AMST_Id)) {
                                            count += 1;
                                        }
                                    });
                                    if (count === 0) {
                                        $scope.studentlistd.push({ AMST_Id: dd.AMST_Id, studentname: dd.STUDENTNAME, admno: dd.ADMNO, regno: dd.REGNO });
                                    }
                                }
                            });

                            // STUDENT WISE MARKS DETAILS
                            $scope.studentwisesubj = [];
                            angular.forEach($scope.studentlistd, function (ddd) {
                                $scope.studentwisesubj = [];
                                angular.forEach($scope.getcumulativereportdetails, function (dd) {
                                    if (parseInt(ddd.AMST_Id) === parseInt(dd.AMST_Id)) {
                                        $scope.studentwisesubj.push(dd);
                                    }
                                });
                                ddd.markslist = $scope.studentwisesubj;
                            });
                            console.log($scope.studentlistd);
                            $scope.colnspan = 6 + $scope.subcolmns.length;

                            angular.forEach($scope.year_list, function (dd) {
                                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.yr = dd.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.class_list, function (dd) {
                                if (dd.asmcL_Id === parseInt($scope.ASMCL_Id)) {
                                    $scope.cla = dd.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.section_list, function (dd) {
                                if (dd.asmS_Id === parseInt($scope.ASMS_Id)) {
                                    $scope.sec = dd.asmC_SectionName;
                                }
                            });


                            $scope.instname = promise.getinstitution;
                            $scope.inst_name = $scope.instname[0].mI_Name;
                            $scope.add = $scope.instname[0].mI_Address1;
                            $scope.city = $scope.instname[0].ivrmmcT_Name;
                            $scope.pin = $scope.instname[0].mI_Pincode;

                        } else {
                            swal("No Records Found");
                        }
                    }

                });
            } else {
                $scope.submitted = true;
            }
        };

        //to print
        $scope.print_HHS02 = function () {
            var innerContents = document.getElementById("printidnew").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.Excel_HHS02 = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var excelname = "CONSOLIDATED MARKS SHEET REPORT.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
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