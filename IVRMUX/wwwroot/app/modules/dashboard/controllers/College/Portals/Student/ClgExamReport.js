(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGExamReportController', CLGExamReportController);

    CLGExamReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window'];
    function CLGExamReportController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

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
            apiService.getDATA("ClgExamReport/getloaddata").
                then(function (promise) {
                    $scope.acayearlist = promise.studetiallist;
                    if ($scope.detailsrdo == "Halt_Ticket") {
                        $scope.acayearlist = [];
                        $scope.asmaY_Id = "";
                        $scope.EME_Id = "";
                        $scope.acayearlist = promise.examReportList;
                    }
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

            $scope.examReportList = "";
            $scope.examlist = "";
            $scope.subjectlist = "";
            $scope.acayearlist = "";
            $scope.EME_Id = "";
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.loaddata();
        };
        //================================academicyear Selection

        $scope.onyearchange = function () {

            $scope.employeeid = [];
            $scope.main_list = [];
            $scope.EME_Id = "";
            var data = {
                "Type": $scope.detailsrdo,
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("ClgExamReport/getexamdata", data).
                then(function (promise) {
                    if (promise.examlist !== null) {
                        if (promise.stuyearlist != null && promise.stuyearlist[0].FeeBalanceCounts > 0) {
                            $scope.examlist = [];
                            $scope.EME_Id = "";
                            swal("Kindly clear the Fee dues then download the Hall ticket ! ");
                        }
                        else {
                            $scope.examlist = promise.examlist;
                        }

                       
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
            $scope.employeeid = [];
            $scope.main_list = [];
        };
        $scope.getsubject = function () {
            //$scope.subjectlist = [];
            //$scope.examReportList = [];
            var data = {};
            data = {
                "Type": $scope.detailsrdo,
                "ASMAY_Id": $scope.asmaY_Id,
                "EME_Id": $scope.EME_Id
            };
            apiService.create("ClgExamReport/getSubjects", data).
                then(function (promise) {
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

            apiService.create("ClgExamReport/StudentExamDetails", data).
                then(function (promise) {

                    $scope.examReportList = promise.examReportList;
                    $scope.hide_examg = true;

                    //============================Graph
                    if ($scope.detailsrdo === "EWAS" || $scope.detailsrdo === "SWAS" || $scope.detailsrdo === "ESW") {
                        $scope.examclass = [];
                        $scope.examsection = [];
                        $scope.examobtained = [];
                        $scope.examclass1 = [];
                        $scope.examsection1 = [];
                        $scope.examobtained1 = [];

                        if ($scope.examReportList.length > 0 && $scope.examReportList !== null) {
                            if ($scope.detailsrdo === "EWAS") {
                                angular.forEach($scope.examReportList, function (objexm) {
                                    $scope.examclass.push({ label: objexm.ISMS_SubjectName, "y": objexm.ECSTMPS_SemAverage });
                                    $scope.examsection.push({ label: objexm.ISMS_SubjectName, "y": objexm.ECSTMPS_SectionAverage });

                                    $scope.examclass1.push({ label: objexm.ISMS_SubjectName, "y": objexm.ECSTMPS_SemHighest });
                                    $scope.examsection1.push({ label: objexm.ISMS_SubjectName, "y": objexm.ECSTMPS_SectionHighest });

                                    if (objexm.EYCES_MarksDisplayFlg === true) {
                                        $scope.examobtained.push({ label: objexm.ISMS_SubjectName, "y": objexm.ECSTMPS_ObtainedMarks });
                                        $scope.examobtained1.push({ label: objexm.ISMS_SubjectName, "y": objexm.ECSTMPS_ObtainedMarks });
                                    }
                                });
                            }
                            else if ($scope.detailsrdo === "SWAS") {
                                angular.forEach($scope.examReportList, function (objexm) {
                                    $scope.examclass.push({ label: objexm.EME_ExamName, "y": objexm.ECSTMPS_SemAverage });
                                    $scope.examsection.push({ label: objexm.EME_ExamName, "y": objexm.ECSTMPS_SectionAverage });


                                    $scope.examclass1.push({ label: objexm.EME_ExamName, "y": objexm.ECSTMPS_SemHighest });
                                    $scope.examsection1.push({ label: objexm.EME_ExamName, "y": objexm.ECSTMPS_SectionHighest });

                                    if (objexm.EYCES_MarksDisplayFlg === true) {
                                        $scope.examobtained.push({ label: objexm.EME_ExamName, "y": objexm.ECSTMPS_ObtainedMarks });
                                        $scope.examobtained1.push({ label: objexm.EME_ExamName, "y": objexm.ECSTMPS_ObtainedMarks });
                                    }
                                });
                            }
                            else if ($scope.detailsrdo === "ESW") {
                                angular.forEach($scope.examReportList, function (objexm) {
                                    $scope.examclass.push({ label: objexm.ASMAY_Year, "y": objexm.ECSTMPS_SemAverage });
                                    $scope.examsection.push({ label: objexm.ASMAY_Year, "y": objexm.ECSTMPS_SectionAverage });

                                    $scope.examclass1.push({ label: objexm.ASMAY_Year, "y": objexm.ECSTMPS_SemHighest });
                                    $scope.examsection1.push({ label: objexm.ASMAY_Year, "y": objexm.ECSTMPS_SectionHighest });

                                    if (objexm.EYCES_MarksDisplayFlg === true) {
                                        $scope.examobtained.push({ label: objexm.ASMAY_Year, "y": objexm.ECSTMPS_ObtainedMarks });
                                        $scope.examobtained1.push({ label: objexm.ASMAY_Year, "y": objexm.ECSTMPS_ObtainedMarks });
                                    }
                                });
                            }

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
                                    name: "SEMESTER AVERAGE",
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
                                    name: "SEMESTER HIGHEST",
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
                                $scope.examclass.push({ label: objexm.EME_ExamName + ':' + objexm.ASMAY_Year, "y": objexm.ECSTMP_SemRank });
                                $scope.examsection.push({ label: objexm.EME_ExamName + ':' + objexm.ASMAY_Year, "y": objexm.ECSTMP_SectionRank });
                                $scope.examobtained.push({ label: objexm.EME_ExamName + ':' + objexm.ASMAY_Year, "y": objexm.ECSTMPS_ObtainedMarks });
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
                                    name: "SEMESTER RANK",
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
                });
        };

        $scope.submitted = false;
        //addednew
        $scope.report = function () {
            // $scope.selected_student = [];
            $scope.examname = "";

            angular.forEach($scope.examlist, function (yyy) {
                if (yyy.emE_Id === parseInt($scope.EME_Id)) {
                    $scope.examname = yyy.emE_ExamName;
                }
            });
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "EME_Id": $scope.EME_Id,

            };

            apiService.create("HallTicketGenerationCollege/ExamReport", data).then(function (promise) {
                $scope.main_list = promise.getStudent;
                if ($scope.main_list !== null && $scope.main_list.length > 0) {
                    $scope.employeeid = [];
                    angular.forEach($scope.main_list, function (dev) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push(dev);
                        } else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.AMCST_Id === dev.AMCST_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push(dev);
                            }
                        }
                    });
                    $scope.exmyear = $scope.employeeid[0].ASMAY_Year;

                    console.log($scope.employeeid);
                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.main_list, function (dd) {
                            if (dd.AMCST_Id === ddd.AMCST_Id) {

                                $scope.templist.push(dd);
                            }
                        });
                        ddd.plannerdetails = $scope.templist;

                    });


                    console.log($scope.employeeid);
                    if ($scope.configuraion.length > 0) {
                        $scope.principal = $scope.configuraion[0].ivrmgC_PrincipalSign;
                    }
                    else {
                        $scope.principal = "";
                    }
                    var e1 = angular.element(document.getElementById("report"));
                    $scope.dynamichtml = true;
                    $compile(e1.html(promise.htmldata))(($scope));



                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.printData = function () {
            var innerContents1 = document.getElementById("printformat1").innerHTML;
            var popupWinindow1 = window.open('');
            popupWinindow1.document.open();
            popupWinindow1.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents1 + '</html>');
            popupWinindow1.document.close();
        };
        $scope.cancel = function () {
            $state.reload();
        };

    }
})();