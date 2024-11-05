(function () {
    'use strict';
    angular.module('app').controller('MultipleExamProgressCardReport1Controller', MultipleExamProgressCardReport1Controller)
    MultipleExamProgressCardReport1Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window', 'Excel', '$timeout']
    function MultipleExamProgressCardReport1Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window, Excel, $timeout) {

        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.displayattendance = true;
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
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];

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
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
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
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
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
                    swal("No Exam Is Mapped For Selected Details");
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
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails =[];
            $scope.getexamwisetotaldetails = [];

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
                    "examlist": $scope.termlisttemp
                };

                apiService.create("JSHSExamReports/getmultiple_exam_progress_report", data).then(function (promise) {

                    if (promise !== null) {

                        if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {
                            $scope.JSHSReport = true;
                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                            $scope.getstudentdetails = promise.getstudentdetails;
                            $scope.getexamdetails = promise.getexamdetails;

                            $scope.getexamsubjectwisemarksdetails = promise.getexamsubjectwisemarksdetails;
                            $scope.getexamwisetotaldetails = promise.getexamwisetotaldetails;

                            $scope.subjectdetails = [];

                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.subjectdetails = [];
                                angular.forEach($scope.getstudentwisesubjectlist, function (sub) {
                                    if (stu.AMST_Id === sub.AMST_Id) {
                                        $scope.subjectdetails.push(sub);
                                    }
                                });
                                stu.subjects = $scope.subjectdetails;
                            });

                            $scope.marksdetails = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.marksdetails = [];
                                angular.forEach($scope.getexamsubjectwisemarksdetails, function (sub) {
                                    if (stu.AMST_Id === sub.amsT_Id) {
                                        $scope.marksdetails.push(sub);
                                    }
                                });
                                stu.marks = $scope.marksdetails;
                            });

                            $scope.totalmarksdetails = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.totalmarksdetails = [];
                                angular.forEach($scope.getexamwisetotaldetails, function (sub) {
                                    if (stu.AMST_Id === sub.amsT_Id) {
                                        $scope.totalmarksdetails.push(sub);
                                    }
                                });
                                stu.totalmarks = $scope.totalmarksdetails;
                            });

                            // Subjet Wise Total
                            $scope.getstudentmarksindidetails = promise.getstudentmarksindidetails;
                            $scope.totalgradeindi = [];
                            angular.forEach($scope.getstudentdetails, function (st) {
                                $scope.totalgradeindi = [];
                                angular.forEach($scope.getstudentmarksindidetails, function (su) {
                                    if (st.AMST_Id === su.AMST_Id) {
                                        $scope.totalgradeindi.push(su);
                                    }
                                });
                                st.inidsubmarkstotal = $scope.totalgradeindi;
                            });

                            // Over All Total
                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.totalgrade = [];
                            angular.forEach($scope.getstudentdetails, function (st) {
                                $scope.totalgrade = [];
                                angular.forEach($scope.getstudentmarksdetails, function (su) {
                                    if (st.AMST_Id === su.AMST_Id) {
                                        $scope.totalgrade.push(su);
                                    }
                                });
                                st.markstotal = $scope.totalgrade;
                            });

                            $scope.clastechname = $scope.getstudentmarksdetails[0].classteachername;

                            console.log($scope.getstudentdetails);

                            angular.forEach($scope.year_list, function (dd) {
                                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.year = dd.asmaY_Year;
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

                        } else {
                            swal("No Records Found");
                        }
                    }

                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
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