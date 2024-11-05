
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_MonthEndReportController', INV_MonthEndReportController);
    INV_MonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_MonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgdiv = false;
        $scope.imgdivMA = false;
        $scope.imgdivK = false;
        $scope.imgname = logopath;
        $scope.headerimg = false;
        var temp = [];
        var year = "";
        //===============================================================================

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("INV_MonthEndReport/getloaddata", pageid).
                then(function (promise) {
                    $scope.acayyearbind = promise.acayear;
                    $scope.monthlist = promise.month_array;
                });
        };
        //============================================Academic Year 
        $scope.onselectYear = function () {
            temp = [];
            angular.forEach($scope.acayyearbind, function (itm) {
                if (itm.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                    year = itm.asmaY_Year;
                }
            });
            var s1 = year.substring(0, 4);
            var s2 = year.substring(year.length, 5);
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
            $scope.years = temp;
        };
        $scope.get_monthendreport = '';
        $scope.submitted = false;
        //=====================================Monthend report
        $scope.showReportdata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "month": $scope.IVRM_Month_Id,
                    "year": $scope.yearmodel                  
                };

                apiService.create("INV_MonthEndReport/getmonthreport", data).
                    then(function (promise) {
                        if (promise.get_monthendreport.length > 0) {

                            $scope.report = true;
                            $scope.get_monthendreport = promise.get_monthendreport;
                            $scope.grnCount = promise.get_monthendreport[0].grnCount;
                            $scope.salesCount = promise.get_monthendreport[0].salesCount;
                            $scope.itemCount = promise.get_monthendreport[0].itemCount;
                            $scope.emailCount = promise.get_monthendreport[0].email;
                            $scope.smsCount = promise.get_monthendreport[0].sms;
                            $scope.designation = "Implementation Engineer";
                            $scope.today = new Date();
                            angular.forEach($scope.monthlist, function (itm) {
                                if (itm.ivrM_Month_Id === parseInt($scope.IVRM_Month_Id)) {
                                    $scope.monthmodelvalue = itm.ivrM_Month_Name;
                                }
                            });
                            angular.forEach($scope.acayyearbind, function (itm) {
                                if (itm.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.acayearnow = itm.asmaY_Year;
                                }
                            });

                            $scope.report = true;
                            var chart = new CanvasJS.Chart("rangeBarChat");
                            chart.options.axisX = { interval: 1, labelFontSize: 12 };
                            chart.options.axisY = { labelFontSize: 12 };
                            chart.options.height = 260;
                            chart.options.width = 1000;
                            var series1 = {
                                type: "column",
                                name: "COUNT",
                                showInLegend: true,
                                dataPoints: [
                                    { y: parseInt($scope.grnCount), label: "GRN COUNT" },
                                    { y: parseInt($scope.salesCount), label: "SALES COUNT" },
                                    { y: parseInt($scope.itemCount), label: "ITEM COUNT" },
                                    { y: parseInt($scope.smsCount), label: "SMS COUNT" },
                                    { y: parseInt($scope.emailCount), label: "EMAIL COUNT" }
                                ]
                            };
                            chart.options.data = [];
                            chart.options.data.push(series1);
                            chart.render();

                            $scope.exportToExcel = function () {
                                if (promise.get_monthendreport.length > 0) {
                                    var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                                    $timeout(function () { location.href = exportHref; }, 100);
                                }
                            };
                            $scope.printData = function () {

                                if (promise.get_monthendreport.length > 0) {
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
                                    $scope.imgdiv = true;
                                    $scope.headerimg = true;
                                    popupWinindow.document.close();
                                }
                            };


                        }
                        else {
                            swal("Record Not Found....!!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };


    }
})();