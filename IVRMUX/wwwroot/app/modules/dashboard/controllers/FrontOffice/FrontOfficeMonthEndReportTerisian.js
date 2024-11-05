(function () {
    'use strict';

    angular
        .module('app')
        .controller('FrontOfficeMonthEndReportTerisian', FrontOfficeMonthEndReportTerisian);

    FrontOfficeMonthEndReportTerisian.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function FrontOfficeMonthEndReportTerisian($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        //===============================================

        
        $scope.ddate = new Date();
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        //======================================================

        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("FrontOffice/", id).
                then(function (promise) {
                    $scope.fillmonth = promise.fillmonth;
                    $scope.fillyear = promise.fillyear;
                });
        };

        $scope.onchageDropdownValue = function () {
            $scope.report = false;
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
            $scope.loaddata();
        };

        $scope.user_check = function () {
            if ($scope.user_check == 1) {
                $scope.userl = 1;
            }
            else {
                $scope.userl = 0;
            }
        };

        $scope.ShowReportdata = function () {
            if ($scope.myForm.$valid) {

                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();
                var data = {
                    "day": $scope.year,
                    "month": $scope.month
                };

                apiService.create("FrontOffice/getmonthreport", data).
                    then(function (promise) {

                        //if (promise.count > 0 && promise.count != null) {
                        $scope.monthmodelvalue = $scope.from_date;
                        $scope.report = true;
                        $scope.showGrafh = true;
                        $scope.FullName = $scope.IENAME;
                        $scope.totalCount = promise.count;
                        $scope.emailCount = promise.emailcount;
                        $scope.smsCount = promise.smscount;

                        $scope.workingEmployee = promise.workingEmployee;
                        $scope.missingPhoto = promise.missingPhoto;
                        $scope.missingEmailId = promise.missingEmailId;
                        $scope.missingContactNumber = promise.missingContactNumber;

                        $scope.lateinEmployeeemail = promise.onlylateinDetailsEmail;
                        $scope.lateinEmployeesms = promise.onlylateinDetailssms;
                        $scope.absentemployeeemail = promise.onlyAbsentDetailsEmail;
                        $scope.absentemployeesms = promise.onlyAbsentDetailssms;
                        $scope.earlyoutemployeeemail = promise.onlyEarlyOutDetailsEmail;
                        $scope.earlyoutemployeesms = promise.onlyEarlyOutDetailssms;

                        angular.forEach($scope.fillmonth, function (dd) {

                            if (dd.monthid == $scope.month) {
                                $scope.month = dd.monthname;
                            }
                        });

                        $scope.designation = "Implementation Engineer";
                        $scope.today = new Date();
                        $scope.report = true;

                        var chart = new CanvasJS.Chart("rangeBarChat");

                        chart.options.axisX = { interval: 1, labelFontSize: 12 };
                        chart.options.axisY = { labelFontSize: 12 };
                        chart.options.height = 260;
                        chart.options.width = 1000;

                        var series1 = { //dataSeries - first quarter
                            type: "column",
                            name: "Count",
                            showInLegend: true,
                            dataPoints: [
                                { y: parseInt($scope.workingEmployee), label: "WorkingEmployee" },
                                { y: parseInt($scope.lateinEmployeeemail), label: "LateIn Count" },
                                { y: parseInt($scope.earlyoutemployeeemail), label: "EarlyOut Count" },
                                { y: parseInt($scope.absentemployeeemail), label: "Absent Count" }
                            ]
                        };

                        chart.options.data = [];
                        chart.options.data.push(series1);
                        chart.render();

                        $scope.exportToExcel = function () {
                            if (promise.count > 0 && promise.count != null) {
                                var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                                $timeout(function () { location.href = exportHref; }, 100);
                            }
                        };

                        $scope.printData = function () {
                            var base64Image = chart.canvas.toDataURL();
                            document.getElementById('rangeBarChat').style.display = 'none';
                            document.getElementById('chartImage').src = base64Image;
                            var innerContents = document.getElementById("tablegrp").innerHTML;
                            var popupWinindow = window.open('');
                            popupWinindow.document.open();
                            popupWinindow.document.write('<html><head>' +
                                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                            popupWinindow.document.close();
                            $state.reload();
                        };
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
    }
})();

