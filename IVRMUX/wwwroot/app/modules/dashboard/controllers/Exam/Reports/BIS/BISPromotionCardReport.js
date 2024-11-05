(function () {
    'use strict';
    angular.module('app').controller('BISPromotionCardReportController', BISPromotionCardReportController)
    BISPromotionCardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window', 'Excel', '$timeout']
    function BISPromotionCardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window, Excel, $timeout) {

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

        // TO Save The Data
        $scope.submitted = false;
        $scope.BISPromotionCardReport = function (obj) {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
            $scope.studentdetails = [];
            $scope.JSHSReport = false;
            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id
                };

                apiService.create("JSHSExamReports/BISPromotionCardReport", data).then(function (promise) {

                    if (promise !== null && promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {

                        $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                        $scope.JSHSReport = true;
                        $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;

                        $scope.gettermdetails = promise.gettermdetails;
                        $scope.gettermexamdetails = promise.gettermexamdetails;
                        $scope.getgroupdetails = promise.getgroupdetails;
                        $scope.getgroupexamdetails = promise.getgroupexamdetails;
                        $scope.studentdetails = promise.getstudentdetails;
                        $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                        $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;
                        $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;

                        $scope.groupwiseexamlist_temp = [];
                        $scope.groupwiseexamlist = [];

                        // Group Wise Exam List
                        angular.forEach($scope.getgroupdetails, function (dd) {
                            $scope.groupwiseexamlist_temp = [];
                            var counttotal = 0;
                            angular.forEach($scope.getgroupexamdetails, function (d) {
                                if (dd.empG_DistplayName === d.empG_DistplayName) {
                                    counttotal += d.empsgE_ForMaxMarkrs;

                                    $scope.groupwiseexamlist_temp.push({
                                        EMPG_GroupName: d.empG_GroupName, EME_Id: d.emE_Id,
                                        EME_ExamName: d.emE_ExamName, EME_ExamOrder: d.emE_ExamOrder, EMPG_DistplayName: d.empG_DistplayName,
                                        EMPSGE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs,
                                        examnamedisplay: d.emE_ExamCode
                                    });

                                    $scope.groupwiseexamlist.push({
                                        EMPG_GroupName: d.empG_GroupName, EME_Id: d.emE_Id,
                                        EME_ExamName: d.emE_ExamName, EME_ExamOrder: d.emE_ExamOrder, EMPG_DistplayName: d.empG_DistplayName,
                                        EMPSGE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs,
                                        examnamedisplay: d.emE_ExamCode
                                    });
                                }
                            });

                            $scope.groupwiseexamlist_temp.push({
                                EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                                EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000, EMPG_DistplayName: dd.empG_DistplayName,
                                EMPSGE_ForMaxMarkrs: dd.empsG_PercentValue, examnamedisplay: 'Total'
                            });

                            $scope.groupwiseexamlist_temp.push({
                                EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800001,
                                EME_ExamName: "Grade", EME_ExamOrder: 9800001, EMPG_DistplayName: dd.empG_DistplayName,
                                EMPSGE_ForMaxMarkrs: "", examnamedisplay: "Grade"
                            });

                            $scope.groupwiseexamlist.push({
                                EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                                EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000, EMPG_DistplayName: dd.empG_DistplayName,
                                EMPSGE_ForMaxMarkrs: dd.empsG_PercentValue, examnamedisplay: 'Total'
                            });

                            $scope.groupwiseexamlist.push({
                                EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800001,
                                EME_ExamName: "Grade", EME_ExamOrder: 9800001, EMPG_DistplayName: dd.empG_DistplayName,
                                EMPSGE_ForMaxMarkrs: "", examnamedisplay: "Grade"
                            });

                            dd.empsG_MarksValue = dd.empsG_PercentValue;
                            dd.groupewiseexam = $scope.groupwiseexamlist_temp;
                        });

                        console.log($scope.getgroupdetails);
                        console.log($scope.groupwiseexamlist);

                        // Student Wise Subject List
                        $scope.studenwisesubjects = [];
                        angular.forEach($scope.studentdetails, function (stu) {
                            $scope.studenwisesubjects = [];
                            angular.forEach($scope.getstudentwisesubjectlist, function (stusubj) {
                                if (stu.AMST_Id === stusubj.AMST_Id) {
                                    $scope.studenwisesubjects.push(stusubj);
                                }
                            });
                            stu.studentsubjects = $scope.studenwisesubjects;
                        });

                        //Student Wise Marks List
                        $scope.studenwisemarks = [];
                        angular.forEach($scope.studentdetails, function (stu) {
                            $scope.studenwisemarks = [];
                            angular.forEach($scope.getstudentmarksdetails, function (stusubj) {
                                if (stu.AMST_Id === stusubj.AMST_Id) {
                                    $scope.studenwisemarks.push(stusubj);
                                }
                            });
                            stu.studentmarks = $scope.studenwisemarks;
                        });

                        $scope.classteachername = "";
                        if (promise.clstchname !== null && promise.clstchname.length > 0) {
                            $scope.classteachername = promise.clstchname[0].hrmE_EmployeeFirstName;
                        }

                        $scope.getpromotionremarksdetails = promise.getpromotionremarksdetails;
                        angular.forEach($scope.studentdetails, function (stu) {
                            angular.forEach($scope.getpromotionremarksdetails, function (dd) {
                                if (stu.AMST_Id === dd.amsT_Id) {
                                    stu.remarks = dd.eprD_Remarks == null ? "" : dd.eprD_Remarks;
                                    stu.promotedclass = dd.eprD_ClassPromoted == null ? "" : dd.eprD_ClassPromoted;
                                    stu.PromotionName = dd.eprD_PromotionName == null ? "" : dd.eprD_PromotionName;
                                }
                            });
                        });

                        console.log($scope.studentdetails);
                    } else {
                        swal("No Records Found");
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("HHS02").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/NDS/ND_6_8_ReportCardPdf.css" />' +
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getexamlist.some(function (options) {
                return options.EME_Id;
            });
        };
    }
})();