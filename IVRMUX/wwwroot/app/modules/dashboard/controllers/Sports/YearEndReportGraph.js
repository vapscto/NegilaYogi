(function () {
    'use strict';

    angular
        .module('app')
        .controller('YearEndReportGraph', YearEndReportGraph);

    YearEndReportGraph.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache'];

    function YearEndReportGraph($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {


        $scope.submitted = false;
        $scope.table_flag = false;
        $scope.showdataGrafh = false;
        $scope.searchValue = '';

        $scope.Onload = function () {
            apiService.getURI("YearEndReport/loadDrpDwn/", 5).then(function (promise) {
                $scope.academicYear = promise.academicYear;
            });
        }

        $scope.cancel = function () {
            //$scope.ASMAY_Id = "";
            //$scope.radioption = "";
            //$scope.yearEndReport = false;
            //$scope.showdataGrafh = false;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();

            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.OnchangeRadioVal = function () {
            $scope.yearEndReport = "";
            $scope.array = "";
            $scope.showdataGrafh = false;

        }

        $scope.getReport = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "radioVal": $scope.radioption,
                }

                apiService.create("YearEndReport/getReportGraph/", data).
                    then(function (promise) {

                        if (promise.count > 0) {

                            angular.forEach($scope.academicYear, function (aa) {
                                if (aa.asmaY_Id == $scope.ASMAY_Id) {
                                    $scope.yearname = aa.yearName;
                                }
                            })

                            $scope.yearEndReport = promise.yearEndReport;
                            $scope.presentCountgrid = $scope.yearEndReport.length;
                            $scope.showdataGrafh = true;
                            var chart = new CanvasJS.Chart("rangeBarChat");

                            chart.options.axisX = {
                                labelFontSize: 12,
                                labelAngle: -20,
                                interval: 1,
                                labelFontColor: "black",
                                labelFontWeight: "bold",
                            };
                            chart.options.axisY = { labelFontSize: 12 };
                            chart.options.height = 300;
                            chart.options.width = 750;

                            $scope.array = [];
                            $scope.newttl = 0;
                            if ($scope.radioption == 'divts') {
                                $scope.array.length = 0;
                                $scope.name = "Division Name";
                                for (var i = 0; i < $scope.yearEndReport.length; i++) {
                                    $scope.array.push({ y: parseInt($scope.yearEndReport[i].totalNo), label: $scope.yearEndReport[i].divisionName });
                                    $scope.newttl = $scope.newttl + $scope.yearEndReport[i].totalNo;
                                }
                            }
                            else {
                                $scope.array.length = 0;
                                $scope.name = "House Name";
                                for (var i = 0; i < $scope.yearEndReport.length; i++) {
                                    $scope.array.push({ y: parseInt($scope.yearEndReport[i].totalNo), label: $scope.yearEndReport[i].houseName });
                                    $scope.newttl = $scope.newttl + $scope.yearEndReport[i].totalNo;
                                }
                            }


                            var series1 = {
                                type: "column",
                                name: $scope.name,
                                showInLegend: true,
                                dataPoints: $scope.array
                            };
                            chart.options.data = [];
                            chart.options.data.push(series1);
                            chart.render();

                            $scope.Print = function () {

                                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                                    var base64Image = chart.canvas.toDataURL();
                                    document.getElementById('rangeBarChat').style.display = 'none';
                                    document.getElementById('chartImage').src = base64Image;
                                    var innerContents = document.getElementById("printSectionId").innerHTML;
                                    var popupWinindow = window.open('');
                                    popupWinindow.document.open();
                                    popupWinindow.document.write('<html><head>' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                        '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
                                        '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                                    );
                                    popupWinindow.document.close();
                                }
                                else {
                                    swal("Please Select Records to be Printed");
                                }
                            }

                        }
                        else {
                            swal("No Records Found");
                            $scope.yearEndReport = "";
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        }
        //===========================ALL SELECTION OPTION FOR PRINT.
        $scope.printdatatable = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.yearEndReport, function (itm) {
                itm.checked = toggleStatus;
                if ($scope.checkall == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }
        //======================SINGLE SELECTION OPTION FOR PRINT.
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.checkall = $scope.yearEndReport.every(function (itm) { return itm.checked; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }


        }


        $scope.ExportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        }
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname1 = logopath;

    }
})();
