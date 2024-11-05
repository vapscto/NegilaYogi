(function () {
    'use strict';
    angular.module('app').controller('YearlyAnalysisReportController', YearlyAnalysisReportController)

    YearlyAnalysisReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function YearlyAnalysisReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#30859c",
                "#ff8c00",
                "#a9a9a9",
                "#cccc00",
                "#9dd2e1",
                "#EDCA93",
                "#696661",
                "#695A42",
                "#B6B1A8"
            ]);

        var chart = {};

        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;
        $scope.printdatatable = [];
        $scope.templist = [];
        $scope.templistnew = [];
        $scope.reportnewlist = [];
        $scope.stdgraphseries4 = [];

        $scope.studentAttendanceList = {};

        $scope.usrname = localStorage.getItem('username');

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("YearlyAnalysisReport/loaddata", pageid).then(function (promise) {
                $scope.all = true;
                $scope.yearDropdown = promise.getyearlist;
            });
        };

        $scope.getDataByType = function () {
            $scope.reportlist = [];
            $scope.gridflag = false;
            $scope.excel_flag = true;
            $scope.print_flag = true;
            $scope.templist = [];
            $scope.templistnew = [];
            $scope.reportnewlist = [];
            $scope.stdgraphseries4 = [];
        };


        $scope.report = function () {
            $scope.printdatatable = [];
            $scope.searchValue = "";
            $scope.templist = [];
            $scope.templistnew = [];
            $scope.reportnewlist = [];
            $scope.stdgraphseries4 = [];

            if ($scope.myForm.$valid) {
                $scope.reportnewlist = [];
                $scope.reportlist = [];
                $scope.excel_flag = true;
                $scope.print_flag = true;
                $scope.gridflag = false;

                var tcchecked = 0;
                var deactivechecked = 0;
                if ($scope.withtc === true) {
                    tcchecked = 1;
                } else {
                    tcchecked = 0;
                }

                if ($scope.withdeactive === true) {
                    deactivechecked = 1;
                } else {
                    deactivechecked = 0;
                }


                var data = {
                    "reporttype": $scope.type23,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "Noofyears": $scope.noofyears,
                    "tcflag": tcchecked,
                    "deactiveflag": deactivechecked
                };
                apiService.create("YearlyAnalysisReport/report", data).then(function (promise) {

                    if (promise !== null) {

                        if (promise.getreport !== null && promise.getreport.length > 0) {
                            $scope.gridflag = true;
                            $scope.excel_flag = false;
                            $scope.print_flag = false;
                            $scope.reportlist = promise.getreport;
                            $scope.noofyearslist = promise.getreportacademicyearlist;
                            $scope.classlist = promise.getclasslist;

                            $scope.templist = [];
                            $scope.templistnew = [];
                            $scope.reportnewlist = [];
                            $scope.stdgraphseries4 = [];

                            if ($scope.type23 === 'new') {
                                $scope.reportname = "New Admission";
                            } else if ($scope.type23 === 'total') {
                                if (deactivechecked === 0 && tcchecked === 0) {
                                    $scope.reportname = "Total Strength";
                                } else if (deactivechecked === 1 && tcchecked === 0) {
                                    $scope.reportname = "Total Strength Including Deactivate Students";
                                } else if (deactivechecked === 0 && tcchecked === 1) {
                                    $scope.reportname = "Total Strength Including Left Students";
                                } else if (deactivechecked === 1 && tcchecked === 1) {
                                    $scope.reportname = "Total Strength Including Left And Deactivated Students";
                                }
                            } else if ($scope.type23 === 'tc') {
                                $scope.reportname = "TC Students";
                            }

                            angular.forEach($scope.noofyearslist, function (yr) {

                                angular.forEach($scope.classlist, function (dd) {
                                    var count = 0;
                                    angular.forEach($scope.reportlist, function (temp) {
                                        if (parseInt(temp.ASMCL_Id) === dd.asmcL_Id && yr.asmaY_Id === parseInt(temp.ASMAY_Id)) {
                                            count = count + 1;
                                        }
                                    });

                                    if (count === 0) {   
                                        $scope.reportlist.push({
                                            ASMAY_Year: yr.asmaY_Year, ASMCL_ClassName: dd.asmcL_ClassName, ASMCL_Order: dd.asmcL_Order,
                                            ASMAY_Order: yr.asmaY_Order, ASMCL_Id: dd.asmcL_Id, ASMAY_Id: yr.asmaY_Id, totalcount: 0
                                        });
                                    }
                                });
                            }); 


                            angular.forEach($scope.noofyearslist, function (yr) {
                                $scope.templist = [];
                                angular.forEach($scope.classlist, function (cls) {
                                    $scope.templistnew = [];
                                    angular.forEach($scope.reportlist, function (rprt) {
                                        if (cls.asmcL_Id === parseInt(rprt.ASMCL_Id) && yr.asmaY_Id === parseInt(rprt.ASMAY_Id)) {
                                            $scope.templist.push({
                                                yearname: rprt.ASMAY_Year, classname: rprt.ASMCL_ClassName, total: rprt.totalcount, yearid: rprt.ASMAY_Id,
                                                classid: rprt.ASMCL_Id
                                            });
                                        }
                                    });
                                });                               

                                $scope.reportnewlist.push({ ASMAY_Id: yr.asmaY_Id, ASMAY_Year: yr.asmaY_Year, list: $scope.templist });
                            });

                            if (parseInt($scope.noofyears) === 1) {
                                $scope.years = "Year";
                            } else {
                                $scope.years = "Years";
                            }



                            console.log($scope.reportnewlist);
                            console.log($scope.reportlist);
                            console.log($scope.stdgraphseries4);

                            angular.forEach($scope.reportnewlist, function (dd) {
                                $scope.templistnew = [];
                                angular.forEach(dd.list, function (d) {
                                    $scope.templistnew.push({ label: d.classname, "y": d.total });
                                });

                                $scope.stdgraphseries4.push({
                                    type: 'column',
                                    name: dd.ASMAY_Year,
                                    toolTipContent: "{label}<br/> <span style='\"'color: {color};'\"'>{name}</span> : <strong>{y}</strong>",
                                    showInLegend: true,
                                    indexLabel: "{y}",
                                    indexLabelPlacement: "outside",
                                    indexLabelOrientation: "vertical",
                                    indexLabelFontColor: "black",
                                    indexLabelFontSize: 14,
                                    dataPoints: $scope.templistnew
                                });
                            });

                            chart = new CanvasJS.Chart("chartContainer", {
                                animationEnabled: true,
                                height: 500,
                                width: 1070,
                                title: {
                                    text: "Yearly Analysis Report For " + $scope.reportname + " From Last " + $scope.noofyears + " " + $scope.years
                                },
                                colorSet: "graphcolor",

                                axisX: {
                                    interval: 1,
                                    title: "Class Name",
                                    labelFontSize: 12,
                                    labelFontWeight: "bold",
                                    labelFontColor: "black",
                                    labelAngle: -45
                                },
                                axisY: {
                                    title: "Count",
                                    labelFontSize: 12,
                                    labelFontWeight: "bold",
                                    labelFontColor: "black",
                                    gridThickness: 0
                                },
                                toolTip: {
                                    shared: true
                                },
                                legend: {
                                    reversed: false
                                },
                                data: $scope.stdgraphseries4
                            });

                            chart.render();

                        } else {
                            swal("No Records Found");
                            $scope.gridflag = false;
                            $scope.excel_flag = true;
                            $scope.print_flag = true;
                        }
                    } else {
                        swal("No Records Found");
                        $scope.gridflag = false;
                        $scope.excel_flag = true;
                        $scope.print_flag = true;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.searchValue = '';

        $scope.printData = function () {

            var base64Image1 = chart.canvas.toDataURL();
            document.getElementById('chartContainer').style.display = 'none';
            document.getElementById('chartImage').src = base64Image1;
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();


            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            $scope.imgdiv = true;

            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };

        $scope.submitted = false;

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

    }
})();
