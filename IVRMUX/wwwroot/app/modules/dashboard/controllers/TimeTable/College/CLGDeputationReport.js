﻿
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGDeputationReportController', CLGDeputationReportController)

    CLGDeputationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function CLGDeputationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
      
     
        $scope.tadprint = false;
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 15;
        }
       
        $scope.currentPage = 1;
        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.flag = 'D';
        $scope.stdsearchValue = '';
      

        $scope.loaddata = function () {
            $scope.report = false;
            apiService.getDATA("CLGDeputationReport/getdata").
                then(function (promise) {
                    $scope.acayyearbind = promise.acdlist;
                })
        }     

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
            $scope.loaddata();
        }
        //for print
        $scope.printData = function () {

            var innerContents = '';
            if ($scope.flag=='C') {
                innerContents = document.getElementById("STDPRNT1").innerHTML;
            }
            else if ($scope.flag == 'D') {
                innerContents = document.getElementById("STDPRNT").innerHTML;
            }

           
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
        $scope.exportToExcel = function () {
            var table = '';
            if ($scope.flag == 'C') {
                table = '#STDEXCEL1'
            }
            else if ($scope.flag == 'D') {
                table = '#STDEXCEL'
            }

            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }

        $scope.onclickloaddata = function () {
            $scope.datapages = [];
            $scope.report = false;
            $scope.report1 = false;

        }
        $scope.datapages = [];
        $scope.ShowReportdata = function () {
            $scope.datapages = [];

            var TTSD_Datefrm = new Date($scope.fromdate).toDateString();
            var TTSD_Dateto = new Date($scope.todate).toDateString();
            if ($scope.myForm.$valid) {
                var data = {
                    "fromdate": TTSD_Datefrm,
                    "todate": TTSD_Dateto,
                    "ASMAY_ID": $scope.academicyr,
                    "flag": $scope.flag
                }
                apiService.create("CLGDeputationReport/getreport", data).
                    then(function (promise) {
                        if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {
                            if ($scope.flag=='C') {
                                $scope.report = true;
                                $scope.report1 = false;
                            }
                            else if ($scope.flag == 'D') {
                                $scope.report1 = true;
                                $scope.report = false;
                            }
                           
                            $scope.datapages = promise.reportdatelist;
                        }
                        else {
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
