
(function () {
    'use strict';
    angular.module('app').controller('ExamGraphController', ExamGraphController)

    ExamGraphController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'blockUI']
    function ExamGraphController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, blockUI) {

        $scope.BindData = function () {
            apiService.getDATA("ExamGraph/getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
            });
        };

        $scope.onselectradio = function () {
            $scope.getnewexamlist = [];
            if ($scope.radiotype === 'classwise') {
                $scope.cwise = true;
                $scope.swise = false;
            }
            else if ($scope.radiotype === 'studentwiseavg') {
                $scope.cwise = true;
                $scope.swise = false;
            }
            else {
                $scope.swise = true;
                $scope.cwise = false;
            }
            $scope.cancel1();
        };


        $scope.OnAcdyear = function () {
            var data = {
                "ASMAY_Id": $scope.obj.asmaY_Id
            };
            apiService.create("ExamGraph/OnAcdyear", data).then(function (promise) {
                $scope.clslist = promise.classlist;
            });
        };

        $scope.onclasschange = function () {
            var data = {
                "ASMAY_Id": $scope.obj.asmaY_Id,
                "ASMCL_Id": $scope.obj.asmcL_Id
            };
            apiService.create("ExamGraph/onclasschange", data).then(function (promise) {
                $scope.seclist = promise.seclist;
                $scope.exsplt = promise.exmstdlist;
                $scope.Exm_Master_Category = promise.exm_Master_Category;
            });
        };

        $scope.onsectionchange = function () {
            var data = {
                "ASMAY_Id": $scope.obj.asmaY_Id,
                "ASMCl_Id": $scope.obj.asmcL_Id,
                "ASMS_Id": $scope.obj.asmS_Id
            };
            apiService.create("ExamGraph/onsectionchange", data).then(function (promise) {
                $scope.exsplt = promise.exmstdlist;
            });
        };

        $scope.onchangeexam = function () {
            var data = {
                "ASMAY_Id": $scope.obj.asmaY_Id,
                "ASMCl_Id": $scope.obj.asmcL_Id,
                "EME_Id": $scope.obj.emE_Id
            };
            apiService.create("ExamGraph/onchangeexam", data).then(function (promise) {
                $scope.sublist = promise.sublist;
            });
        };

        $scope.onchangecategory = function () {
            var data = {
                "ASMAY_Id": $scope.obj.asmaY_Id,
                "ASMCl_Id": $scope.obj.asmcL_Id,
                "EMCA_Id": $scope.obj.emcA_Id
            };
            apiService.create("ExamGraph/onchangecategory", data).then(function (promise) {
                $scope.sublist = promise.sublist;
            });
        };

        $scope.onchangesubject = function () {
            $scope.getnewexamlist = [];
            if ($scope.radiotype === 'subwisewithallexam' && $scope.obj.ismS_Id !== undefined && $scope.obj.ismS_Id!==null) {

                var data = {
                    "ASMAY_Id": $scope.obj.asmaY_Id,
                    "ASMCl_Id": $scope.obj.asmcL_Id,
                    "EMCA_Id": $scope.obj.emcA_Id,
                    "ISMS_Id": $scope.obj.ismS_Id
                };
                apiService.create("ExamGraph/onchangesubject", data).then(function (promise) {
                    $scope.getnewexamlist = promise.getnewexamlist;
                });
            }
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getnewexamlist.some(function (options) {
                return options.Selected;
            });
        };

        $scope.submitted = false;
        $scope.obj = {};

        $scope.saveddata = function (obj) {
            $scope.studentwiseavg = [];
            $scope.get_marks_avg = [];
            $scope.submitted = true;
            $scope.tempexamlist = [];
            if ($scope.myForm.$valid) {
                if ($scope.radiotype === 'classwise') {
                    obj.ismS_Id = 0;
                    obj.emcA_Id = 0;
                }
                else if ($scope.radiotype === 'subwise') {
                    obj.asmS_Id = 0;
                    obj.emcA_Id = 0;
                }
                else if ($scope.radiotype === 'subwisewithallexam') {
                    obj.asmS_Id = 0;
                    obj.emE_Id = 0;
                    angular.forEach($scope.getnewexamlist, function (dd) {
                        if (dd.Selected) {
                            $scope.tempexamlist.push({ EME_Id: dd.emE_Id });
                        }
                    });

                }
                else if ($scope.radiotype === 'studentwiseavg') {
                    obj.emcA_Id = 0;
                    obj.ismS_Id = 0;
                }

                var data = {
                    "ASMAY_Id": obj.asmaY_Id,
                    "ASMCL_Id": obj.asmcL_Id,
                    "ASMS_Id": obj.asmS_Id,
                    "EME_Id": obj.emE_Id,
                    "ISMS_Id": obj.ismS_Id,
                    "report_type": $scope.radiotype,
                    "EMCA_Id": obj.emcA_Id,
                    "tempexamlist": $scope.tempexamlist
                };

                apiService.create("ExamGraph/onreport", data).
                    then(function (promise) {
                        $scope.main_list = promise.datareport;
                        $scope.main_list1 = promise.datareport1;

                        $scope.instname = promise.instname;
                        $scope.instnamen = $scope.instname[0].mI_Name;
                        $scope.addrees = $scope.instname[0].mI_Adress;

                        $scope.array = [];
                        if ($scope.radiotype === 'classwise') {
                            $scope.printdiv = false;

                            $scope.classteacher = promise.classteacher;
                            $scope.classteachername = $scope.classteacher[0].empname;

                            angular.forEach($scope.yearlt, function (ye) {
                                if (ye.asmaY_Id === parseInt(obj.asmaY_Id)) {
                                    $scope.yearname = ye.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.clslist, function (ye) {
                                if (ye.asmcL_Id === parseInt(obj.asmcL_Id)) {
                                    $scope.classname = ye.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.seclist, function (ye) {
                                if (ye.asmS_Id === parseInt(obj.asmS_Id)) {
                                    $scope.sectionname = ye.asmC_SectionName;
                                }
                            });

                            angular.forEach($scope.exsplt, function (ye) {
                                if (ye.emE_Id === parseInt(obj.emE_Id)) {
                                    $scope.examname = ye.emE_ExamName;
                                }
                            });

                            var chart = new CanvasJS.Chart("linechart");
                            chart.options.axisX = { labelFontSize: 12, labelAngle: -45, interval: 1, };
                            chart.options.axisY = { labelFontSize: 12 };
                            chart.options.height = 260;
                            chart.options.width = 1000;
                            $scope.array.length = 0;

                            $scope.name = "Classwise Avg";
                            for (var i = 0; i < $scope.main_list.length; i++) {
                                $scope.array.push({ y: $scope.main_list[i].estmpS_ClassAverage, label: $scope.main_list[i].ismS_SubjectName });
                            }
                            var series1 = {
                                yValueFormatString: "#,##0.##",
                                indexLabel: "{y} %",
                                indexLabelFontColor: "black",
                                indexLabelFontSize: 16,
                                indexLabelPlacement: 'inside',
                                indexLabelOrientation: 'vertical',
                                toolTipContent: "<b>{label}</b>: {y} %",
                                type: "column",
                                name: $scope.name,
                                showInLegend: true,
                                dataPoints: $scope.array,

                            };
                            chart.options.data = [];
                            chart.options.data.push(series1);
                            chart.render();

                        }
                        else if ($scope.radiotype === 'subwise') {
                            angular.forEach($scope.yearlt, function (ye) {
                                if (ye.asmaY_Id === parseInt(obj.asmaY_Id)) {
                                    $scope.yearname = ye.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.clslist, function (ye) {
                                if (ye.asmcL_Id === parseInt(obj.asmcL_Id)) {
                                    $scope.classname = ye.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.exsplt, function (ye) {
                                if (ye.emE_Id === parseInt(obj.emE_Id)) {
                                    $scope.examname = ye.emE_ExamName;
                                }
                            });

                            angular.forEach($scope.sublist, function (ye) {
                                if (ye.ismS_Id === parseInt(obj.ismS_Id)) {
                                    $scope.subjectname = ye.ismS_SubjectName;
                                }
                            });


                            var chart1 = new CanvasJS.Chart("linechart1");

                            chart1.options.axisX = { labelFontSize: 12 };
                            chart1.options.axisY = { labelFontSize: 12 };
                            chart1.options.height = 260;
                            chart1.options.width = 1000;
                            $scope.array.length = 0;
                            $scope.name = "Subjectwise Avg";
                            for (var i = 0; i < $scope.main_list1.length; i++) {
                                $scope.array.push({ y: $scope.main_list1[i].estmpS_SectionAverage, label: $scope.main_list1[i].asmC_SectionName });
                            }

                            var series1 = {
                                yValueFormatString: "#,##0.##",
                                indexLabel: "{y} %",
                                indexLabelFontColor: "black",
                                indexLabelFontSize: 16,
                                indexLabelPlacement: 'inside',
                                indexLabelOrientation: 'vertical',
                                toolTipContent: "<b>{label}</b>: {y} %",
                                type: "column",
                                name: $scope.name,
                                showInLegend: true,
                                dataPoints: $scope.array
                            };
                            chart1.options.data = [];
                            chart1.options.data.push(series1);
                            chart1.render();
                        }


                        else if ($scope.radiotype === 'subwisewithallexam') {
                            $scope.temparray = [];
                            angular.forEach($scope.yearlt, function (ye) {
                                if (ye.asmaY_Id === parseInt(obj.asmaY_Id)) {
                                    $scope.yearname = ye.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.clslist, function (ye) {
                                if (ye.asmcL_Id === parseInt(obj.asmcL_Id)) {
                                    $scope.classname = ye.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.sublist, function (ye) {
                                if (ye.ismS_Id === parseInt(obj.ismS_Id)) {
                                    $scope.subjectname = ye.ismS_SubjectName;
                                }
                            });

                            $scope.get_marks_avg = promise.get_marks_avg;

                            if ($scope.get_marks_avg.length > 0) {
                                $scope.get_exam_list = promise.get_exam_list;
                                $scope.getclasssection = promise.getclasssection;

                                angular.forEach($scope.getclasssection, function (sec) {
                                    $scope.temparray = [];
                                    angular.forEach($scope.get_marks_avg, function (get) {
                                        if (parseInt(sec.asmS_Id) === parseInt(get.asmS_Id)) {
                                            $scope.temparray.push(get);
                                        }
                                    });

                                    var sum = 0;
                                    for (var i = 0; i < $scope.temparray.length; i++) {
                                        sum += parseFloat($scope.temparray[i].estmpS_SectionAverage); //don't forget to add the base 
                                    }

                                    var testary = [];
                                    testary = $scope.temparray;
                                    for (var i = testary.length; i < $scope.get_exam_list.length; i++) {
                                        testary.push({ id: 0 });
                                    }
                                    var avg = sum / testary.length;
                                    sec.newtemp = testary;
                                    sec.averag = Number.parseFloat(avg).toFixed(2);

                                });
                                console.log($scope.getclasssection);
                            }

                            //chart

                            $scope.fnldata = [];
                            $scope.yaxiss = {};
                            for (var j = 0; j < $scope.getclasssection.length; j++) {
                                $scope.array = [];
                                for (var i = 0; i < $scope.get_exam_list.length; i++) {
                                    $scope.yaxiss = "";
                                    for (var k = 0; k < $scope.getclasssection[j].newtemp.length; k++) {
                                        if ($scope.getclasssection[j].newtemp[k].emE_Id == $scope.get_exam_list[i].emE_Id) {
                                            $scope.yaxiss = $scope.getclasssection[j].newtemp[k].estmpS_SectionAverage;
                                        }
                                    }

                                    if ($scope.yaxiss == "" || $scope.yaxiss == null) {
                                        $scope.array.push({ label: $scope.get_exam_list[i].emE_ExamName, y: 0, label: $scope.get_exam_list[i].emE_ExamName });

                                    } else {
                                        $scope.array.push({ label: $scope.get_exam_list[i].emE_ExamName, y: $scope.yaxiss, label: $scope.get_exam_list[i].emE_ExamName });
                                    }
                                }
                                $scope.array.push({ label: "Avg", y: parseFloat($scope.getclasssection[j].averag) });
                                $scope.fnldata.push({
                                    type: "line", showInLegend: true,
                                    name: $scope.getclasssection[j].asmC_SectionName,
                                    markerType: "square",

                                    dataPoints: $scope.array
                                });
                            }
                            console.log($scope.fnldata);
                            var chart = new CanvasJS.Chart("linechart121", {
                                animationEnabled: true,
                                theme: "light2",
                                height: 200,
                                width: 1000,
                                axisX: {
                                    crosshair: {
                                        enabled: true,
                                        snapToDataPoint: true
                                    }
                                },
                                axisY: {
                                    title: "Percentage",
                                    crosshair: {
                                        enabled: true
                                    }
                                },
                                toolTip: {
                                    shared: true
                                },
                                legend: {
                                    cursor: "pointer",
                                    verticalAlign: "bottom",
                                    horizontalAlign: "left",
                                    dockInsidePlotArea: false
                                },
                                data: $scope.fnldata
                            });
                            chart.render();
                        }

                        else if ($scope.radiotype === "studentwiseavg") {
                            $scope.studentwiseavg = promise.studentwiseavg;
                            if ($scope.studentwiseavg.length > 0) {
                                $scope.classteacher = promise.classteacher;
                                $scope.classteachername = $scope.classteacher[0].empname;

                                angular.forEach($scope.yearlt, function (ye) {
                                    if (ye.asmaY_Id === parseInt(obj.asmaY_Id)) {
                                        $scope.yearname = ye.asmaY_Year;
                                    }
                                });

                                angular.forEach($scope.clslist, function (ye) {
                                    if (ye.asmcL_Id === parseInt(obj.asmcL_Id)) {
                                        $scope.classname = ye.asmcL_ClassName;
                                    }
                                });

                                angular.forEach($scope.seclist, function (ye) {
                                    if (ye.asmS_Id === parseInt(obj.asmS_Id)) {
                                        $scope.sectionname = ye.asmC_SectionName;
                                    }
                                });

                                angular.forEach($scope.exsplt, function (ye) {
                                    if (ye.emE_Id === parseInt(obj.emE_Id)) {
                                        $scope.examname = ye.emE_ExamName;
                                    }
                                });


                                $scope.array = [];
                                $scope.name = "STUDENT AND EXAM WISE AVERAGE";
                                for (var i = 0; i < $scope.studentwiseavg.length; i++) {
                                    $scope.array.push({ label: $scope.studentwiseavg[i].studentname, y: $scope.studentwiseavg[i].estmP_Percentage });
                                }
                                var chart = new CanvasJS.Chart("linechart13", {
                                    animationEnabled: true,
                                    animationDuration: 3000,
                                    height: 450,
                                    width: 1000,
                                    axisX: {
                                        labelFontSize: 16,
                                        margin: 10,
                                        labelAngle: -70,
                                        interval: 1
                                    },
                                    axisY: {
                                        labelFontSize: 16,
                                        margin: 10,
                                        suffix: "%",
                                        interval: 20

                                    },
                                    toolTip: {
                                        shared: true
                                    },
                                    title: {
                                        text: "STUDENT AND EXAM WISE AVERAGE"
                                    },
                                    data: [
                                        {
                                            showInLegend: true,
                                            yValueFormatString: "#,##0.##",
                                            indexLabel: "{y} %",
                                            indexLabelFontColor: "black",
                                            indexLabelFontSize: 16,
                                            indexLabelPlacement: 'inside',
                                            indexLabelOrientation: 'vertical',
                                            toolTipContent: "<b>{label}</b>: {y} %",
                                            type: "column",
                                            dataPoints: $scope.array
                                        }
                                    ]
                                });
                                chart.render();
                            }
                        }

                        //-----------------------Print---------------------//
                        $scope.Print = function () {
                            var base64Image = "";
                            var innerContents = "";
                            var popupWinindow = "";

                            if ($scope.radiotype === 'classwise') {
                                if (promise.datareport !== null && promise.datareport.length > 0) {
                                    base64Image = chart.canvas.toDataURL();
                                    document.getElementById('linechart').style.display = 'none';
                                    document.getElementById('chartImage').src = base64Image;
                                    innerContents = document.getElementById("printSectionId").innerHTML;
                                    popupWinindow = window.open('');
                                    popupWinindow.document.open();
                                    popupWinindow.document.write('<html><head>' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                                        '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                                    popupWinindow.document.close();
                                }
                                document.getElementById('linechart').style.display = 'block';
                            }

                            else if ($scope.radiotype === 'subwise') {
                                if (promise.datareport1 !== null && promise.datareport1.length > 0) {
                                    base64Image = chart1.canvas.toDataURL();
                                    document.getElementById('linechart1').style.display = 'none';
                                    document.getElementById('chartImage1').src = base64Image;
                                    innerContents = document.getElementById("printSectionId1").innerHTML;
                                    popupWinindow = window.open('');
                                    popupWinindow.document.open();
                                    popupWinindow.document.write('<html><head>' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                                        '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                                    popupWinindow.document.close();
                                }
                                document.getElementById('linechart1').style.display = 'block';
                            }

                            else if ($scope.radiotype === 'studentwiseavg') {
                                base64Image = chart.canvas.toDataURL();
                                document.getElementById('linechart13').style.display = 'none';
                                document.getElementById('chartImage13').src = base64Image;
                                innerContents = document.getElementById("printSectionId3").innerHTML;
                                popupWinindow = window.open('');
                                popupWinindow.document.open();
                                popupWinindow.document.write('<html><head>' +
                                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                                    '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                                popupWinindow.document.close();
                                document.getElementById('linechart13').style.display = 'block';
                            }

                            else if ($scope.radiotype === 'subwisewithallexam') {
                                base64Image = chart.canvas.toDataURL();
                                document.getElementById('linechart121').style.display = 'none';
                                document.getElementById('chartImage121').src = base64Image;
                                innerContents = document.getElementById("printSectionId2").innerHTML;
                                popupWinindow = window.open('');
                                popupWinindow.document.open();
                                popupWinindow.document.write('<html><head>' +
                                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                                    '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                                popupWinindow.document.close();
                                document.getElementById('linechart121').style.display = 'block';
                            }
                        };
                        //-----------------------Print---------------------//

                    });
            }
        };

        $scope.Print1 = function () {
            var base64Image = "";
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.radiotype === 'classwise') {
                if ($scope.main_list !== null && $scope.main_list.length > 0) {
                    $scope.printdiv = true;
                    base64Image = document.getElementById('linechart').canvas.toDataURL();
                    // var base64Image = chart.canvas.toDataURL();
                    document.getElementById('linechart').style.display = 'none';
                    document.getElementById('chartImage').src = base64Image;
                    innerContents = document.getElementById("printSectionId").innerHTML;
                    popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                        '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                    popupWinindow.document.close();
                }
            }

            else if ($scope.radiotype === 'subwise') {
                if ($scope.main_list1 !== null && $scope.main_list1.length > 0) {
                    base64Image = chart1.canvas.toDataURL();
                    document.getElementById('linechart1').style.display = 'none';
                    document.getElementById('chartImage1').src = base64Image;
                    innerContents = document.getElementById("printSectionId1").innerHTML;
                    popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                        '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                    popupWinindow.document.close();
                }
            }
        };

        $scope.cancel = function () {
            $scope.cancel1();
        };

        $scope.cancel1 = function () {
            $scope.obj.asmaY_Id = "";
            $scope.obj.asmcL_Id = "";
            $scope.obj.asmS_Id = "";
            $scope.obj.ismS_Id = "";
            $scope.obj.emE_Id = "";
            $scope.obj.emcA_Id = "";
            $scope.main_list = [];
            $scope.main_list1 = [];
            $scope.submitted = false;
            $scope.studentwiseavg = [];
            $scope.get_marks_avg = [];
            $scope.main_list1 = [];
            $scope.main_list = [];
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    }

})();