(function () {
    'use strict';
    angular.module('app').controller('Hostel_Student_InOutReportController', Hostel_Student_InOutReportController)

    Hostel_Student_InOutReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel', '$q']
    function Hostel_Student_InOutReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel, $q) {
         
        $scope.data = [];
        $scope.Obj = {};

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;           
            var pageid = 2;
            apiService.getURI("Hostel_Student_InOutReport/loaddata", pageid).then(function (promise) {
                $scope.employee = promise.studentlist; 
                $scope.All_Individual();
            });
        };              

        $scope.All_Individual = function () {

            if ($scope.allind == 'individual')
                $scope.disabledata = false;
            else
                $scope.disabledata = true;

        }
        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.fromdate);
            $scope.maxDatemf = new Date();
        };

        $scope.interacted = function (field) {            return $scope.submitted;        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.exportToExceldetails = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'Student Report');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
             

        $scope.get_Report = function () {
            $scope.employee = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
                $scope.from_date = new Date($scope.fromdate).toDateString();
                $scope.to_date = new Date($scope.todate).toDateString();
             
                var data = {
                    "AMCST_Id": $scope.Obj.amcsT_Id.amcsT_Id,
                    "fromdate": $scope.from_date,
                    "todate": $scope.to_date
                }

                apiService.create("Hostel_Student_InOutReport/report", data).then(function (promise) {
                    if (promise.viewlist != null && promise.viewlist.length > 0) {

                        $scope.viewlist = promise.viewlist;


                    }

                    else {
                        swal("No Record  Found..... !!");

                    }

                });

            }

            else {
                $scope.submitted = true;
            }

        }

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };



    }
})();