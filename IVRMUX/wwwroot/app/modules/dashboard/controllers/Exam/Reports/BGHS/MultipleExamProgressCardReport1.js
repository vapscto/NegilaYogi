(function () {
    'use strict';
    angular.module('app').controller('MultipleExamProgressCardReportNew1Controller', MultipleExamProgressCardReportNew1Controller)
    MultipleExamProgressCardReportNew1Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window', 'Excel', '$timeout']
    function MultipleExamProgressCardReportNew1Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window, Excel, $timeout) {

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

                apiService.create("JSHSExamReports/BGHS_GetMultiple_Exam_Progress_Report", data).then(function (promise) {

                    if (promise !== null) {

                        if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {
                            $scope.JSHSReport = true;
                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                            $scope.getstudentdetails = promise.getstudentdetails;
                            $scope.getexamdetails = promise.getexamdetails;

                            var totalcount = 0;

                            angular.forEach($scope.getexamdetails, function (dd) {
                                totalcount += dd.eyceS_MaxMarks;
                            });

                            $scope.getexamdetails.push({ emE_Id: 150001, emE_ExamName: 'Total', eyceS_MaxMarks: totalcount });
                            $scope.getexamdetails.push({ emE_Id: 150002, emE_ExamName: 'Grade', eyceS_MaxMarks: '' });


                            $scope.getexamsubjectwisemarksdetails = promise.getexamsubjectwisemarksdetails;
                            $scope.getexamwisetotaldetails = promise.getexamwisetotaldetails;

                            $scope.subjectdetails = [];

                            var nonapplicablecount = 0;
                            var applicablecount = 0;
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.subjectdetails = [];
                                nonapplicablecount = 0;
                                applicablecount = 0;
                                angular.forEach($scope.getstudentwisesubjectlist, function (sub) {
                                    if (stu.AMST_Id === sub.AMST_Id) {
                                        if (sub.EYCES_AplResultFlg) {
                                            applicablecount += 1;
                                        }
                                        if (!sub.EYCES_AplResultFlg) {
                                            nonapplicablecount += 1;
                                        }
                                        $scope.subjectdetails.push(sub);
                                    }
                                });
                                stu.nonapplicablecount = nonapplicablecount;
                                stu.applicablecount = applicablecount;
                                stu.subjects = $scope.subjectdetails;
                            });


                            // Marks
                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.totalgrade = [];
                            angular.forEach($scope.getstudentdetails, function (st) {
                                $scope.totalgrade = [];
                                angular.forEach($scope.getstudentmarksdetails, function (su) {
                                    if (st.AMST_Id === su.AMST_Id) {
                                        $scope.totalgrade.push(su);
                                    }
                                });
                                st.marks = $scope.totalgrade;
                            });

                            //$scope.clastechname = $scope.getstudentmarksdetails[0].classteachername;


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