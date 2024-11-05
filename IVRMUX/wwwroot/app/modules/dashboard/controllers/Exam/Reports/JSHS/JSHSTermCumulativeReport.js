

(function () {
    'use strict';
    angular.module('app').controller('JSHSTermCumulativeReportController', JSHSTermCumulativeReportController)
    JSHSTermCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'Excel','$timeout']
    function JSHSTermCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, Excel, $timeout) {

        //$scope.asmaY_Year = "2019-2020";
        $scope.reportdata = true;
        $scope.JSHSReport = false;
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
            $scope.reportdata = true;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
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
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
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
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
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
            $scope.reportdata = true;

            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                angular.forEach($scope.termlist, function (term) {
                    if (term.ECT_Id === true) {
                        $scope.termlisttemp.push({ ECT_Id: term.ecT_Id, ECT_TermName: term.ecT_TermName });
                    }
                });

                if ($scope.checkoruncheck === true) {
                    $scope.checkoruncheckflag = 'Checked';
                } else {
                    $scope.checkoruncheckflag = 'UnChecked';
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EMGR_Id": $scope.EMGR_Id,
                    "termlist": $scope.termlisttemp,
                    "checkoruncheckflag": $scope.checkoruncheckflag
                };

                apiService.create("JSHSExamReports/get_termcumulative_reportdetails", data).then(function (promise) {

                    $scope.getstudentmarksdetails_temp = promise.getexamsubjectwisereport;

                    $scope.get_studentlist = [];

                    if ($scope.getstudentmarksdetails_temp !== null && $scope.getstudentmarksdetails_temp.length > 0) {
                        $scope.JSHSReport = true;
                        $scope.reportdata = false;
                        $scope.temp_termlist = [];

                        $scope.gettermdetails = promise.gettermdetails;

                        angular.forEach($scope.gettermdetails, function (dd) {
                            $scope.temp_termlist.push({ termid: dd.ecT_Id, termname: dd.ecT_TermName });
                        });

                        $scope.temp_termlist.push({ termid: 20000, termname: 'Total' });
                        $scope.temp_termlist.push({ termid: 20001, termname: 'Grade' });

                        angular.forEach($scope.getstudentmarksdetails_temp, function (dd) {
                            if ($scope.get_studentlist.length === 0) {
                                $scope.get_studentlist.push({ AMST_Id: dd.AMST_Id, studentname: dd.studentname, admno: dd.admno, rollno: dd.rollno, regno: dd.regno });
                            }
                            else if ($scope.get_studentlist.length > 0) {
                                var student_count = 0;
                                angular.forEach($scope.get_studentlist, function (stu) {
                                    if (stu.AMST_Id === dd.AMST_Id) {
                                        student_count += 1;
                                    }
                                });
                                if (student_count === 0) {
                                    $scope.get_studentlist.push({ AMST_Id: dd.AMST_Id, studentname: dd.studentname, admno: dd.admno, rollno: dd.rollno, regno: dd.regno });
                                }
                            }
                        });

                        // Getting student wise term details
                        $scope.studentwise_marks = [];
                        angular.forEach($scope.get_studentlist, function (stu) {
                            $scope.studentwise_marks = [];
                            angular.forEach($scope.getstudentmarksdetails_temp, function (marks) {
                                if (stu.AMST_Id === marks.AMST_Id) {
                                    if (parseInt(marks.termid) === 20001) {
                                        $scope.studentwise_marks.push({ AMST_Id: marks.AMST_Id, obtainedmarks: marks.grade, termid: marks.termid});
                                    } else {
                                        $scope.studentwise_marks.push({ AMST_Id: marks.AMST_Id, obtainedmarks: marks.marksobtained, termid: marks.termid });
                                    }
                                }
                            });
                            stu.term_markslist = $scope.studentwise_marks;
                        });

                        console.log($scope.get_studentlist);

                        $scope.colspan = $scope.temp_termlist.length + 3;

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
                console.log($scope.exm_sub_mrks_list);
                console.log($scope.total_subwise);
            } else {
                $scope.submitted = true;
            }
        };

        //to print
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