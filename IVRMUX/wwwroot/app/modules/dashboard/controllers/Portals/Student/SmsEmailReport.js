(function () {
    'use strict';
    angular.module('app').controller('SmsEmailReportController', SmsEmailReportController);

    SmsEmailReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window'];
    function SmsEmailReportController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {
        $scope.currentPage = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.search = "";
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            apiService.getDATA("SmsEmailReport/getloaddata").then(function (promise) {
                $scope.studetiallist = promise.yearlist;
            });
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.sortReverse = !$scope.sortReverse;
        };       
              //=================================== Get Report
        $scope.submitted = false;
        $scope.getreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "optionflag": $scope.optionflag,
                    "FromDate": new Date($scope.startdate).toDateString(),
                    "ToDate": new Date($scope.enddate).toDateString()
                    //"ASMAY_Id": $scope.asmaY_Id
                };
                apiService.create("SmsEmailReport/getdata", data).then(function (promise) {
                    if (promise.studlist != null && promise.studlist.length !="") {
                        $scope.studlist = promise.studlist;
                    }
                    else {
                        swal("Record Not Found !");
                    }
                   
                });
            }           
        };
        $scope.radioChange = function () {
            $scope.studlist = [];
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        $scope.exportToExcel = function () {           
            var exportHref = Excel.tableToExcel(tablegrp_print, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);           
        };
        $scope.printData = function () {            
                var innerContents = document.getElementById("tablegrp_print").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                // $scope.imgdiv = true;
                $scope.headerimg = true;
            popupWinindow.document.close();
        };
    }
})();