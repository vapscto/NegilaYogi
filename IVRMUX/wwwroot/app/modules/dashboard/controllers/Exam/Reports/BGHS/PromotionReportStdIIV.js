(function () {
    'use strict';
    angular.module('app').controller('PromotionReportStdIIVController', PromotionReportStdIIVController)
    PromotionReportStdIIVController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window', 'Excel', '$timeout']
    function PromotionReportStdIIVController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window, Excel, $timeout) {

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

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];

            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id
                };

                apiService.create("JSHSExamReports/promotionreportstdiiv", data).then(function (promise) {

                    if (promise !== null) {

                        if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {
                            $scope.JSHSReport = true;
                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                            $scope.getstudentdetails = promise.getstudentdetails;
                            $scope.getexamdetails = promise.getexamdetails;

                            $scope.getgroupexamdetails = promise.getgroupexamdetails;
                            $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;
                            $scope.getexammaxmarks = promise.getexammaxmarks;

                            $scope.groupexamdetails = [];
                            angular.forEach($scope.getexamdetails, function (dd) {
                                $scope.groupexamdetails = [];
                                angular.forEach($scope.getgroupexamdetails, function (d) {
                                    if (d.empG_GroupName === dd.empG_GroupName) {
                                        $scope.groupexamdetails.push(d);
                                    }
                                });
                                if (dd.empsG_Order !== 2) {
                                    $scope.groupexamdetails.push({
                                        empG_GroupName: dd.empG_GroupName, emE_Id: 10000, emE_ExamName: 'Total', emE_ExamOrder: 10000,
                                        empG_DistplayName: dd.empG_DistplayName
                                    });
                                    $scope.groupexamdetails.push({
                                        empG_GroupName: dd.empG_GroupName, emE_Id: 100001, emE_ExamName: 'Grade', emE_ExamOrder: 100001,
                                        empG_DistplayName: dd.empG_DistplayName
                                    });
                                }
                                dd.examgrpdetails = $scope.groupexamdetails;
                            });


                            $scope.groupexamdetailsnew = [];
                            angular.forEach($scope.getexamdetails, function (dd) {                              
                                angular.forEach($scope.getgroupexamdetails, function (d) {
                                    if (d.empG_GroupName === dd.empG_GroupName) {
                                        $scope.groupexamdetailsnew.push(d);
                                    }
                                });
                                if (dd.empsG_Order !== 2) {
                                    $scope.groupexamdetailsnew.push({
                                        empG_GroupName: dd.empG_GroupName, emE_Id: 10000, emE_ExamName: 'Total', emE_ExamOrder: 10000,
                                        empG_DistplayName: dd.empG_DistplayName
                                    });
                                    $scope.groupexamdetailsnew.push({
                                        empG_GroupName: dd.empG_GroupName, emE_Id: 100001, emE_ExamName: 'Grade', emE_ExamOrder: 100001,
                                        empG_DistplayName: dd.empG_DistplayName
                                    });
                                }
                            });

                            angular.forEach($scope.getexamdetails, function (dd) {
                                var counttotal = 0;
                                angular.forEach($scope.groupexamdetailsnew, function (d) {
                                    angular.forEach($scope.getexammaxmarks, function (ddd) {
                                        if (dd.empG_GroupName === d.empG_GroupName && d.emE_Id === ddd.emE_Id && d.emE_Id !== 10000 && d.emE_Id !== 100001) {
                                            d.maxmarks = ddd.eyceS_MaxMarks;
                                            counttotal += ddd.eyceS_MaxMarks;
                                        } else if(dd.empG_GroupName === d.empG_GroupName && d.emE_Id === 10000) {
                                            d.maxmarks = counttotal;
                                        }
                                    });                                   
                                });
                            });

                            // STUDENT WISE SUBJECT DETAILS
                            $scope.subjectdetails = [];
                            var nonapplysubject = 0;
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.subjectdetails = [];
                                angular.forEach($scope.getstudentwisesubjectlist, function (sub) {
                                    if (stu.AMST_Id === sub.AMST_Id) {
                                        $scope.subjectdetails.push(sub);
                                    }
                                    if (sub.EYCES_AplResultFlg === false) {
                                        nonapplysubject += 1;
                                    }
                                });
                                stu.subjects = $scope.subjectdetails;
                                if (nonapplysubject === 0) {
                                    stu.subjectdisplay = 0;
                                } else {
                                    stu.subjectdisplay = 1;
                                }
                            });

                            // STUDENT WISE MARKS DETAILS
                            $scope.marksdetails = [];
                            $scope.totalmarksdetails = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.marksdetails = [];
                                $scope.totalmarksdetails = [];
                                angular.forEach($scope.getstudentmarksdetails, function (sub) {
                                    if (stu.AMST_Id === sub.AMST_Id) {
                                        if (sub.ISMS_Id !== 50001) {
                                            $scope.marksdetails.push(sub);
                                        } else {
                                            $scope.totalmarksdetails.push(sub);
                                        }
                                    }
                                });
                                stu.marks = $scope.marksdetails;
                                stu.totalmarks = $scope.totalmarksdetails;
                            });

                            /* STUDENT WISE ATTENDANCE DETAILS */

                            angular.forEach($scope.getstudentdetails, function (d) {
                                angular.forEach($scope.getstudentwiseattendancedetails, function (dd) {
                                    if (d.AMST_Id === dd.AMST_Id) {
                                        d.Classheld = dd.WORKINGDAYS;
                                        d.Class_Attended = dd.PRESENTDAYS;
                                    }
                                });
                            });


                            $scope.getclassteacher = promise.getclassteacher;
                            if ($scope.getclassteacher !== null && $scope.getclassteacher.length > 0) {
                                $scope.clastechname = $scope.getclassteacher[0].classteachername;
                            }

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