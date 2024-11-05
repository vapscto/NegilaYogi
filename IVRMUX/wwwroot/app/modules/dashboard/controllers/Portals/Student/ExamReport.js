(function () {
    'use strict';
    angular.module('app').controller('ExamReportController', ExamReportController);

    ExamReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window'];
    function ExamReportController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#3498DB",
                "#76D7C4",
                "#808B96",
                "#80DEEA",
                "#C5E1A5",
                "#AAB7B8"
            ]);

        $scope.hide_examg = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            apiService.getDATA("ExamReport/getloaddata").then(function (promise) {
                $scope.acayearlist = promise.studetiallist;
            });
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.sortReverse = !$scope.sortReverse;
        };
        //=============================== radio Change
        $scope.radioChange = function () {
            $scope.asmaY_Id = "";
            $scope.EME_Id = 0;
            //$scope.ismS_Id = "";
            //$scope.acayearlist = "";
            $scope.examlist = "";
            $scope.subjectlist = "";
            $scope.acayearlist = "";
            $scope.examReportList = "";
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.loaddata();
        };
        //================================academicyear Selection

        $scope.onyearchange = function () {
            //  $scope.examlist = [];
            //  $scope.subjectlist = [];
            // $scope.examReportList = [];
            var data = {
                "Type": $scope.detailsrdo,
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("ExamReport/getexamdata", data).then(function (promise) {
                if (promise.examlist !== null) {
                    $scope.examlist = promise.examlist;
                }
                else if (promise.subjectlist !== null) {
                    $scope.subjectlist = promise.subjectlist;
                }
                else {
                    swal("No Record Found....!!");
                }
            });
        };

        //================================Exam Selection
        $scope.onExamchange = function () {
            if ($scope.detailsrdo === "SWAE" || $scope.detailsrdo === "ESW") {
                $scope.getsubject();
            }
        };
        $scope.getsubject = function () {
            //$scope.subjectlist = [];
            // $scope.examReportList = [];
            var data = {};
            data = {
                "Type": $scope.detailsrdo,
                "ASMAY_Id": $scope.asmaY_Id,
                "EME_Id": $scope.EME_Id
            };
            apiService.create("ExamReport/getSubjects", data).then(function (promise) {
                if (promise.subjectlist.length > 0) {
                    $scope.subjectlist = promise.subjectlist;
                }
                else {
                    swal("No Record Found....!!");
                    promise.subjectlist = "";
                }
            });
        };

        //=================================== Get Report
        $scope.getreport = function () {
            var data = {};
            var chart = {};
            $scope.subject_new = [];
            $scope.examReportList = [];
            if ($scope.detailsrdo === "EWAS") {
                data = {
                    "EME_Id": $scope.EME_Id,
                    "Type": $scope.detailsrdo,
                    "ASMAY_Id": $scope.asmaY_Id
                };
            }
            else if ($scope.detailsrdo === "SWAE") {
                data = {
                    "ISMS_Id": $scope.ismS_Id,
                    "Type": $scope.detailsrdo,
                    "ASMAY_Id": $scope.asmaY_Id
                };
            }
            else if ($scope.detailsrdo === "ESW") {
                data = {
                    "EME_Id": $scope.EME_Id,
                    "ISMS_Id": $scope.ismS_Id,
                    "Type": $scope.detailsrdo,
                    "ASMAY_Id": $scope.asmaY_Id
                };
            }
            else if ($scope.detailsrdo === "OVERALL") {
                data = {
                    "Type": $scope.detailsrdo
                };
            }

            apiService.create("ExamReport/StudentExamDetails", data).then(function (promise) {
                if (promise.examReportList != null && promise.examReportList.length > 0) {
                    $scope.examReportList = promise.examReportList;
                    $scope.getexamconfig = promise.getexamconfig;

                    $scope.classrankreq = $scope.getexamconfig[0].exmConfig_ClassRankFlg;
                    $scope.sectionrankreq = $scope.getexamconfig[0].exmConfig_SecRankFlg;

                    $scope.classpositionreq = $scope.getexamconfig[0].exmConfig_ClassPositionFlg;
                    $scope.sectionpoisitonreq = $scope.getexamconfig[0].exmConfig_SectionPositionFlg;
                    $scope.feedefaulter = $scope.getexamconfig[0].exmconfig_FeeDefaulterDisplayFlg;

                    $scope.hide_examg = true;

                    //============================Graph
                    if ($scope.detailsrdo === "EWAS" || $scope.detailsrdo === "SWAE" || $scope.detailsrdo === "ESW") {
                        $scope.examclass = [];
                        $scope.examsection = [];
                        $scope.examobtained = [];
                        $scope.examclass1 = [];
                        $scope.examsection1 = [];
                        $scope.examobtained1 = [];
                        if ($scope.examReportList.length > 0 && $scope.examReportList !== null) {

                            if ($scope.detailsrdo === "EWAS") {

                                $scope.get_eme_id_details = promise.get_eme_id_details;

                                $scope.eycE_SubExamFlg = $scope.get_eme_id_details[0].eycE_SubExamFlg;
                                $scope.eycE_SubSubjectFlg = $scope.get_eme_id_details[0].eycE_SubSubjectFlg;

                                if ($scope.get_eme_id_details[0].eycE_SubExamFlg === true || $scope.get_eme_id_details[0].eycE_SubSubjectFlg === true) {
                                    $scope.subexamreportexamReportList = promise.subexamreportexamReportList;

                                    $scope.subject_new = [];

                                    angular.forEach($scope.subexamreportexamReportList, function (dd) {
                                        if ($scope.subject_new.length === 0) {
                                            $scope.subject_new.push({
                                                ISMS_Id: dd.ISMS_ID, ISMS_SUBJECTNAME: dd.ISMS_SUBJECTNAME, ESTMPS_ObtainedMarks: dd.ESTMPS_ObtainedMarks,
                                                ESTMPS_ObtainedGrade: dd.ESTMPS_ObtainedGrade, EYCES_GradeDisplayFlg: dd.EYCES_GradeDisplayFlg,
                                                EYCES_MarksDisplayFlg: dd.EYCES_MarksDisplayFlg, ESTMPS_PassFailFlg: dd.ESTMPS_PassFailFlg
                                            });
                                        } else if ($scope.subject_new.length > 0) {
                                            var count = 0;

                                            angular.forEach($scope.subject_new, function (d) {
                                                if (d.ISMS_Id === dd.ISMS_ID) {
                                                    count += 1;
                                                }
                                            });
                                            if (count === 0) {
                                                $scope.subject_new.push({
                                                    ISMS_Id: dd.ISMS_ID, ISMS_SUBJECTNAME: dd.ISMS_SUBJECTNAME, ESTMPS_ObtainedMarks: dd.ESTMPS_ObtainedMarks,
                                                    ESTMPS_ObtainedGrade: dd.ESTMPS_ObtainedGrade, EYCES_GradeDisplayFlg: dd.EYCES_GradeDisplayFlg,
                                                    EYCES_MarksDisplayFlg: dd.EYCES_MarksDisplayFlg, ESTMPS_PassFailFlg: dd.ESTMPS_PassFailFlg
                                                });
                                            }
                                        }
                                    });

                                    $scope.subexam_subsubject_marks = [];

                                    angular.forEach($scope.subject_new, function (dd) {
                                        $scope.subexam_subsubject_marks = [];
                                        angular.forEach($scope.subexamreportexamReportList, function (d) {
                                            if (dd.ISMS_Id === d.ISMS_ID) {
                                                $scope.subexam_subsubject_marks.push(d);
                                            }
                                        });
                                        dd.subexam_subsubject_marks = $scope.subexam_subsubject_marks;
                                    });

                                    console.log($scope.subject_new);
                                }


                                angular.forEach($scope.examReportList, function (objexm) {
                                    $scope.examclass.push({ label: objexm.ISMS_SubjectName, "y": objexm.ESTMPS_ClassAverage });
                                    $scope.examsection.push({ label: objexm.ISMS_SubjectName, "y": objexm.ESTMPS_SectionAverage });

                                    $scope.examclass1.push({ label: objexm.ISMS_SubjectName, "y": objexm.ESTMPS_ClassHighest });
                                    $scope.examsection1.push({ label: objexm.ISMS_SubjectName, "y": objexm.ESTMPS_SectionHighest });

                                    if (objexm.EYCES_MarksDisplayFlg === true) {
                                        $scope.examobtained.push({ label: objexm.ISMS_SubjectName, "y": objexm.ESTMPS_ObtainedMarks });
                                        $scope.examobtained1.push({ label: objexm.ISMS_SubjectName, "y": objexm.ESTMPS_ObtainedMarks });
                                    }
                                });
                            }
                            else if ($scope.detailsrdo === "SWAE") {
                                angular.forEach($scope.examReportList, function (objexm) {
                                    $scope.examclass.push({ label: objexm.EME_ExamName, "y": objexm.ESTMPS_ClassAverage });
                                    $scope.examsection.push({ label: objexm.EME_ExamName, "y": objexm.ESTMPS_SectionAverage });


                                    $scope.examclass1.push({ label: objexm.EME_ExamName, "y": objexm.ESTMPS_ClassHighest });
                                    $scope.examsection1.push({ label: objexm.EME_ExamName, "y": objexm.ESTMPS_SectionHighest });

                                    if (objexm.EYCES_MarksDisplayFlg === true) {
                                        $scope.examobtained.push({ label: objexm.EME_ExamName, "y": objexm.ESTMPS_ObtainedMarks });
                                        $scope.examobtained1.push({ label: objexm.EME_ExamName, "y": objexm.ESTMPS_ObtainedMarks });
                                    }
                                });
                            }
                            else if ($scope.detailsrdo === "ESW") {
                                angular.forEach($scope.examReportList, function (objexm) {
                                    $scope.examclass.push({ label: objexm.ASMAY_Year, "y": objexm.ESTMPS_ClassAverage });
                                    $scope.examsection.push({ label: objexm.ASMAY_Year, "y": objexm.ESTMPS_SectionAverage });

                                    $scope.examclass1.push({ label: objexm.ASMAY_Year, "y": objexm.ESTMPS_ClassHighest });
                                    $scope.examsection1.push({ label: objexm.ASMAY_Year, "y": objexm.ESTMPS_SectionHighest });

                                    if (objexm.EYCES_MarksDisplayFlg === true) {
                                        $scope.examobtained.push({ label: objexm.ASMAY_Year, "y": objexm.ESTMPS_ObtainedMarks });
                                        $scope.examobtained1.push({ label: objexm.ASMAY_Year, "y": objexm.ESTMPS_ObtainedMarks });
                                    }
                                });
                            }

                            //for (var i = 0; i < $scope.examReportList.length; i++) {
                            //    $scope.examclass.push({ label: $scope.examReportList[i].ISMS_SubjectName, "y": $scope.examReportList[i].ESTMPS_ClassAverage });
                            //    $scope.examsection.push({ label: $scope.examReportList[i].ISMS_SubjectName, "y": $scope.examReportList[i].ESTMPS_SectionAverage });
                            //    $scope.examobtained.push({ label: $scope.examReportList[i].ISMS_SubjectName, "y": $scope.examReportList[i].ESTMPS_ObtainedMarks });
                            //}
                            //=================== Average Graph
                            chart = new CanvasJS.Chart("examchart", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                width: 1050,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 12
                                },
                                axisY: {
                                    labelFontSize: 12
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "CLASS AVERAGE",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.examclass
                                },
                                {
                                    name: "SECTION AVERAGE",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.examsection
                                },
                                {
                                    name: "MARK OBTAINED",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.examobtained
                                }
                                ]
                            });
                            chart.render();
                            //========================= Highest Graph
                            chart = new CanvasJS.Chart("examchart2", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                width: 1050,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 12
                                },
                                axisY: {
                                    labelFontSize: 12
                                },
                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "CLASS HIGHEST",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.examclass1
                                },
                                {
                                    name: "SECTION HIGHEST",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.examsection1
                                },
                                {
                                    name: "MARK OBTAINED",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.examobtained1
                                }
                                ]
                            });
                            chart.render();
                        }
                        else {
                            swal("No Record Found....");
                            $scope.hide_examg = false;
                        }
                    }
                    else if ($scope.detailsrdo === "OVERALL") {

                        $scope.examclass = [];
                        $scope.examsection = [];
                        $scope.examobtained = [];
                        if ($scope.examReportList.length > 0 && $scope.examReportList !== null) {
                            angular.forEach($scope.examReportList, function (objexm) {
                                $scope.examclass.push({ label: objexm.EME_ExamName + ':' + objexm.ASMAY_Year, "y": objexm.ESTMP_ClassRank });
                                $scope.examsection.push({ label: objexm.EME_ExamName + ':' + objexm.ASMAY_Year, "y": objexm.ESTMP_SectionRank });
                                $scope.examobtained.push({ label: objexm.EME_ExamName + ':' + objexm.ASMAY_Year, "y": objexm.ESTMPS_ObtainedMarks });
                            });

                            chart = new CanvasJS.Chart("examchart", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                width: 1050,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 12
                                },
                                axisY: {
                                    labelFontSize: 12
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "CLASS RANK",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.examclass
                                },
                                {
                                    name: "SECTION RANK",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.examsection
                                },
                                {
                                    name: "TOTAL MARK OBTAINED",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.examobtained
                                }
                                ]
                            });
                            chart.render();
                        }
                        else {
                            swal("No Record Found....");
                            $scope.hide_examg = false;
                        }
                    }
                }
                else {
                    swal("Record  Is Not Found  !")
                }
                
            });
        };

        $scope.submitted = false;
    }
})();