(function () {
    'use strict';
    angular.module('app').controller('CertificateGeneratedReportController', CertificateGeneratedReportController)

    CertificateGeneratedReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel','$timeout']
    function CertificateGeneratedReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.pagesrecord = {};
        $scope.students = [];
        $scope.adtable = true;
        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.excel_flag = true;
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.maxdate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.coptyright = copty;
        $scope.obj = {};
        $scope.obj.type23 = 'Count';
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings != null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;

        $scope.StuAttRptDropdownList = function () {
            $scope.currentPage = 1;
            $scope.currentPage2 = 1;
            var pageid = 1;
            apiService.getURI("HHSBonafiedCertificate/CertificateGeneratedReportLoad", pageid).then(function (promise) {
                $scope.GetReportTypes = promise.getReportTypes;
            });
        };

        $scope.OnChangeYear = function () {
            $scope.excel_flag = true;
            $scope.gridflag = false;
            $scope.submitted = false;            
            $scope.GetReportDetails = [];
        };

        $scope.getDataByType = function () {
            $scope.excel_flag = true;
            $scope.gridflag = false;
            $scope.submitted = false;            
            $scope.GetReportDetails = [];
        };

        $scope.OnChangeFromDate = function () {
            $scope.excel_flag = true;
            $scope.gridflag = false;
            $scope.submitted = false;
            $scope.GetReportDetails = [];
            $scope.ToDate = null;
        };

        $scope.OnChangeToDate = function () {
            $scope.excel_flag = true;
            $scope.gridflag = false;
            $scope.submitted = false;
            $scope.GetReportDetails = [];             
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.clear = function () {
            $scope.excel_flag = true;
            $scope.gridflag = false;
            $scope.submitted = false;
            $scope.type23 = 1;
            $scope.ReportType = "";
            $scope.ToDate = null;
            $scope.FromDate = null;
            $scope.GetReportDetails = [];
        };

        $scope.GetCertificateGeneratedReport = function () {
            if ($scope.myForm.$valid) {
                $scope.GetReportDetails = [];
                var dReportType = "";
                if ($scope.obj.type23 === "Count") {
                    dReportType = "All";
                } else {
                    dReportType = $scope.ReportType;
                }
                var data = {
                    "Report_Name": dReportType,
                    "Report_Type": $scope.obj.type23,
                    "FromDate": new Date($scope.FromDate).toDateString(),
                    "ToDate": new Date($scope.ToDate).toDateString(),
                }
                apiService.create("HHSBonafiedCertificate/GetCertificateGeneratedReport", data).then(function (promise) {
                    $scope.GetReportDetails = promise.getReportDetails;
                    if ($scope.GetReportDetails !== null && $scope.GetReportDetails.length > 0) {

                        $scope.FromDate_temp = new Date($scope.FromDate);
                        $scope.ToDate_temp = new Date($scope.ToDate);
                        $scope.gridflag = true;
                        $scope.excel_flag = false;


                    } else {
                        swal("No Records Found");
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };


        $scope.printData = function () {
            var innerContents = "";
            var popupWinindow = "";

            if ($scope.obj.type23 === "Count") {
                innerContents = document.getElementById("printSectionId").innerHTML;
            }
            else {
                innerContents = document.getElementById("printSectionId2").innerHTML;
            }
            popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            var dd = "";
            if ($scope.obj.type23 === "Count") {
                dd = '#excelsection1';
            }
            else {
                dd = '#excelsection2';
            }
            var exportHref = Excel.tableToExcel(dd, 'sheet name');
            var excelname = "Certificate Generated Report.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
    }
})();