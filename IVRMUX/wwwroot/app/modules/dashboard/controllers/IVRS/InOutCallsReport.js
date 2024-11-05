
(function () {
    'use strict';
    angular
        .module('app')
        .controller('InOutCallsReportController', InOutCallsReportController)

    InOutCallsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function InOutCallsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
         
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
        }

        $scope.loaddata = function () {
            $scope.fromdate = new Date();
            $scope.todate = new Date();
        }
        $scope.search = "";

        $scope.printData = function () {
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
        $scope.printData2 = function () {
            var innerContents = document.getElementById("printSectionId2").innerHTML;
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

        $scope.exportToExcel = function (table) {


            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }

        $scope.exportToExcel2 = function (table2) {
            var exportHref = Excel.tableToExcel(table2, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.listempty1 = function() {
            $scope.datapages2 = [];
        }
        $scope.listempty2 = function () {
            $scope.datapages = [];
        }
        $scope.ShowReportdata = function () {
         
            if ($scope.myForm.$valid) {
               
                var data = {
                    "fromdate": new Date($scope.fromdate).toDateString(),
                    "todate": new Date($scope.todate).toDateString(),
                    "typeofrpt": $scope.typeofrpt,
                    "conso": $scope.conso
                }
                apiService.create("InOutCallsReport/getreport", data).
                    then(function (promise) {
                      
                        if (promise.reportdatelist.length > 0 && promise.reportdatelist !== null) {
                            if ($scope.conso=='detail') {
                                $scope.report2 = true;
                                $scope.datapages2 = promise.reportdatelist;
                            }
                            if ($scope.typeofrpt==='All') {
                                $scope.name = 'In & Out Calls Report';
                            }
                            else if ($scope.typeofrpt === 'Inbound') {
                                $scope.name = 'In Calls Report';
                            } else if ($scope.typeofrpt === 'Outbound') {
                                $scope.name = 'Out Calls Report';
                            }


                            if ($scope.conso == 'consol') {
                                $scope.report = true;
                                $scope.datapages = promise.reportdatelist;
                            }
                           
                        }
                        else {
                            //$scope.datapages.length = 0;
                            swal("Record Not Found");
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }
    }
})();

