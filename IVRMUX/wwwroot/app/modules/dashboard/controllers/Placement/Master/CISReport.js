(function () {
    'use strict';
    angular
        .module('app')
        .controller('CISReportController', CISReportController);
    CISReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel'];
    function CISReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel) {
        var paginationformasters;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.depdes = false;

        $scope.printstudents = [];
        $scope.searchValue = "";
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        //*************************************************load data *******************************************//
        $scope.loaddata = function () {

            var pageid = 2;

            apiService.getURI("CISReport/getdetails", pageid).
                then(function (promise) {

                  
                });
        };

      //report
        $scope.get_Report = function () {
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {

                $scope.start_Date = new Date($scope.start_Date).toDateString();
                $scope.end_Date = new Date($scope.end_Date).toDateString();

                var data = {

                    "PLCISCH_DriveFromDate": $scope.start_Date,
                    "PLCISCH_DriveToDate": $scope.end_Date,



                };
                apiService.create("CISReport/report", data).then(function (promise) {
                    if (promise.getdata.length > 0) {
                        $scope.getdata = promise.getdata;
                        $scope.presentCountgrid = $scope.getdata.length;
                    }
                    else {

                        swal("No Record Found....!!");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
      


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //******************************************* clear *******************************************//
        $scope.cleartabl1 = function () {
            $state.reload();
        }

        //************************************ export to excel *************************************//
        $scope.exportToExcel = function () {

            var blob = new Blob([document.getElementById('printP').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });

            saveAs(blob, "Campus Interview Schedule.xls");
        };


        //*******************************print *************************************//
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printP").innerHTML;
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

