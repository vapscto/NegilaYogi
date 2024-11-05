

(function () {
    'use strict';
    angular
        .module('app')
        .controller('BookRegisterReportController', BookRegisterReportController)

    BookRegisterReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', 'Excel', '$timeout']
    function BookRegisterReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, Excel, $timeout) {

        $scope.submitted = false;
        $scope.tablediv = false;;
        $scope.printd = false;
        $scope.searchValue = '';

        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;


        //-------------Load-data...
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 15;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("BookRegisterReport/getdetails", pageid).then(function (promise) {

                $scope.booktype = promise.booktype;

                $scope.deptlist = promise.deptlist;

                $scope.clsslist = promise.clsslist;
            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //------------End-Load-data..


        //-------------Get-Report...
        $scope.get_report = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    "Type": $scope.Type,
                    "LMD_Id": $scope.lmD_Id
                }



                apiService.create("BookRegisterReport/get_report", data).then(function (promise) {
                    if (promise.reportlist.length > 0) {

                        $scope.reportlist = promise.reportlist;
                        $scope.tablediv = true;;
                        $scope.printd = true;
                    }
                    else {
                        swal('Record Not Available!!!');
                        $state.reload();
                    }
                })

            }
            else {
                $scope.submitted = true;
            }

        }
        //------------End-Get-Report...


        //===========print===========//
        $scope.printData = function () {
            if ($scope.filterValue1 !== null && $scope.filterValue1.length > 0) {
                var innerContents = document.getElementById("printtable").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookRegisterReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        }
        //==============End==============//


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //-----------clear-field
        $scope.Clearid = function () {
            $state.reload();
        }
        //$scope.ExportToExcel = function (tableId) {
        //    var exportHref = Excel.tableToExcel(tableId, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);
        //}
        $scope.ExportToExcel = function (tableIds) {            var excelnamemain = "Book Register Report ";            var printSectionId = tableIds;            excelnamemain = excelnamemain + '.xls';            var exportHref = Excel.tableToExcel(printSectionId, 'Book Register Report');            $timeout(function () {                var a = document.createElement('a');                a.href = exportHref;                a.download = excelnamemain;                document.body.appendChild(a);                a.click();                a.remove();            }, 100);        };

    }
})();

