
(function () {
    'use strict';
    angular
        .module('app')
        .controller('RackReportController', RackReportController)

    RackReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$q', '$filter']
    function RackReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $q, $filter) {

        $scope.submitted = false;
        $scope.tablediv = false;
        $scope.printd = false;
        $scope.export = false;
        //-------------Load-data...
        $scope.loaddata = function () {
            debugger;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 15;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("RackReport/getdetails", pageid).then(function (promise) {

                $scope.floorlist = promise.floorlist;

                $scope.racklist = promise.racklist;
                $scope.lib_list = promise.lib_list;
            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
          //------------End-Load-data...


        $scope.changetable = function () {
            $scope.printbutton = false;
            $scope.reportbutton = true;
            $scope.tablegrd = false;
        }


          //-------------Get-Report...
        $scope.get_report = function () {
            debugger;
            if ($scope.myForm.$valid) {

                var data = {
                    //"LMRA_FloorName": $scope.lmrA_FloorName,
                    "LMRA_RackName": $scope.lmrA_RackName,
                    "LMAL_Id": $scope.LMAL_Id,
                }
                apiService.create("RackReport/get_report", data).then(function (promise) {
                    if (promise.reportlist.length > 0) {

                        $scope.reportlist = promise.reportlist;
                        $scope.tablediv = true;
                        $scope.printd = true;
                        $scope.export = true;
                    }
                    else {
                        $scope.printd = false;
                        $scope.export = false;
                        $scope.tablediv = false;
                        swal('Record Not Available!!!');
                        //$state.reload();

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
            if ($scope.filterValue !== null && $scope.filterValue.length > 0) {
                var innerContents = document.getElementById("printtable").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/RackReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        }

        //====================================================Excel
        $scope.exportToExcel = function (table) {
            debugger;
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }
        //==============End==============//

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //=========clear-field
        $scope.Clearid = function () {
            $scope.lmrA_FloorName = "";
            $scope.lmrA_RackName = "";
            $scope.tablediv = false;
            $scope.printd = false;
            $scope.export = false;
          //  $state.reload();
        }

        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;


      








    }
})();

