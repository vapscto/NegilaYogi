(function () {
    'use strict';
    angular
        .module('app')
        .controller('Hostel_Allotment_ReportController', Hostel_Allotment_ReportController)

    Hostel_Allotment_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout']
    function Hostel_Allotment_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = "";
        $scope.showflag = false;
        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.loaddata = function () {        
            var pageid = 2;
            apiService.getURI("Hostel_Allotment_Report/getdata", pageid).then(function (promise) {
                $scope.yearlist = promise.yearlist;
                $scope.hostellist = promise.hostellist;
            })
        }
        $scope.ExportToExcel = function () {
            var innerContents = '';
            if ($scope.type == 'Student') {
                innerContents = document.getElementById("printtable1").innerHTML;
                var exportHref = Excel.tableToExcel(printtable1, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else if ($scope.type == 'Staff') {
                innerContents = document.getElementById("printtable2").innerHTML;
                var exportHref = Excel.tableToExcel(printtable2, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else if ($scope.type == 'Guest') {
                innerContents = document.getElementById("printtable3").innerHTML;
                var exportHref = Excel.tableToExcel(printtable3, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        }
        //========================print details
        $scope.printData = function () {
            var innerContents = '';
            if ($scope.type == 'Student') {
                innerContents = document.getElementById("printtable1").innerHTML;
            }
            else if ($scope.type == 'Staff') {
                innerContents = document.getElementById("printtable2").innerHTML;
            }
            else if ($scope.type == 'Guest') {
                innerContents = document.getElementById("printtable3").innerHTML;
            }
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookCirculationReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };        
        $scope.allgrid = false;
        $scope.getreport = function () {
            
            $scope.allgrid = false;
            var frmdate1 = $scope.frmdate == null ? "" : $filter('date')($scope.frmdate, "yyyy-MM-dd");
            var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
            if ($scope.myForm.$valid) {
                var data = {
                    "type": $scope.type,
                    "frmdate": frmdate1,
                    "todate": todate1,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "HLMH_Id": $scope.HLMH_Id,                   
                }
                apiService.create("Hostel_Allotment_Report/getreport", data)
                    .then(function (promise) {
                     
                        if (promise.griddata.length > 0) {
                            $scope.griddata = promise.griddata;
                            if ($scope.type == 'Student') {
                                $scope.allgrid = true;                                
                            }
                            else if ($scope.type == 'Staff') {
                                $scope.allgrid = true;
                            }
                            else if ($scope.type == 'Guest') {
                                $scope.allgrid = true;
                            }
                        }
                            else {
                                swal('Record is not Available!!');
                                $state.reload();
                            }
                            $scope.boxfalg = true;
                            $scope.printflag = true;                        
                    });
            }
            else {
                $scope.submitted = true;
            }
        }
    }
})();

