(function () {
    'use strict';
    angular.module('app').controller('BISFinalPromotionCardReportController', BISFinalPromotionCardReportController)
    BISFinalPromotionCardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window', 'Excel', '$timeout']
    function BISFinalPromotionCardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window, Excel, $timeout) {

        $scope.JSHSReport = false;
        $scope.sub_checked = false;
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

                apiService.create("JSHSExamReports/BISFianlPromotionCardReport", data).then(function (promise) {

                    if (promise !== null && promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {

                        $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                        $scope.JSHSReport = true;
                        $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;
                        $scope.getgroupdetails = promise.getgroupdetails;
                        $scope.getgroupexamdetails = promise.getgroupexamdetails;
                        $scope.studentdetails = promise.getstudentdetails;
                        $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;

                        $scope.groupwiseexamlist_temp = [];
                        $scope.groupwiseexamlist = [];

                        angular.forEach($scope.getgroupdetails, function (grp) {
                            var TotalMarks = 0;
                            angular.forEach($scope.getgroupexamdetails, function (grpexam) {
                                if (grp.empG_GroupName === grpexam.empG_GroupName) {
                                    TotalMarks += grpexam.empsgE_ForMaxMarkrs;

                                    $scope.groupwiseexamlist.push({
                                        EME_Id: grpexam.emE_Id, EME_ExamName: grpexam.emE_ExamName,
                                        EME_ExamCode: grpexam.emE_ExamCode, EMPSGE_ForMaxMarkrs: grpexam.empsgE_ForMaxMarkrs,
                                        EMPG_GroupName: grpexam.empG_GroupName, EMPG_DistplayName: grpexam.empG_DistplayName
                                    });

                                    $scope.groupwiseexamlist_temp.push({
                                        EME_Id: grpexam.emE_Id, EME_ExamName: grpexam.emE_ExamName,
                                        EME_ExamCode: grpexam.emE_ExamCode, EMPSGE_ForMaxMarkrs: grpexam.empsgE_ForMaxMarkrs,
                                        EMPG_GroupName: grpexam.empG_GroupName, EMPG_DistplayName: grpexam.empG_DistplayName
                                    });
                                }
                            });

                            $scope.groupwiseexamlist.push({
                                EME_Id: 980000, EME_ExamName: 'Total',
                                EME_ExamCode: 'Total', EMPSGE_ForMaxMarkrs: TotalMarks,
                                EMPG_GroupName: grp.empG_GroupName, EMPG_DistplayName: grp.empG_DistplayName
                            });

                            $scope.groupwiseexamlist.push({
                                EME_Id: 980002, EME_ExamName: 'Percentage',
                                EME_ExamCode: 'Percentage', EMPSGE_ForMaxMarkrs: '100%',
                                EMPG_GroupName: grp.empG_GroupName, EMPG_DistplayName: grp.empG_DistplayName
                            });

                            $scope.groupwiseexamlist.push({
                                EME_Id: 980001, EME_ExamName: 'Grade',
                                EME_ExamCode: 'Grade', EMPSGE_ForMaxMarkrs: '',
                                EMPG_GroupName: grp.empG_GroupName, EMPG_DistplayName: grp.empG_DistplayName
                            });
                        });


                        angular.forEach($scope.getgroupdetails, function (grp) {
                            $scope.groupwiseexamlist.push({
                                EME_Id: 9800000, EME_ExamName: grp.empG_DistplayName,
                                EME_ExamCode: grp.empG_DistplayName, EMPSGE_ForMaxMarkrs: grp.empsG_PercentValue,
                                EMPG_GroupName: grp.empG_GroupName, EMPG_DistplayName: grp.empG_DistplayName
                            });
                        });


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

                        $scope.stud_work_attendence = promise.work_attendence;
                        $scope.stud_present_attendence = promise.present_attendence;

                        $scope.nonapplicablesubject_examwisemarks = promise.nonapplicablesubject_examwisemarks;

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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCEHSTermReportCardPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();