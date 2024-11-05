

(function () {
    'use strict';
    angular
        .module('app')
        .controller('SubscriptionReportController', SubscriptionReportController)

    SubscriptionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', 'Excel', '$timeout']
    function SubscriptionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, Excel, $timeout) {

        $scope.submitted = false;
        $scope.tablediv = false;;
        $scope.printdoption = true;
        $scope.searchvalue = '';

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


        //---------------Load --data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("SubscriptionReport/getdetails", pageid).then(function (promise) {

                $scope.deptlist = promise.deptlist;

                $scope.lib_list = promise.lib_list;
            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //------------End---Load --data




        //---------------Get Repoet....
        $scope.get_report = function () {
            debugger;


            var fromdate = $scope.Fromdate == null ? "" : $filter('date')($scope.Fromdate, "yyyy-MM-dd");
            var todate = $scope.ToDate == null ? "" : $filter('date')($scope.ToDate, "yyyy-MM-dd");

            //if ($scope.lmD_Id == "0") {
            //    $scope.lmD_Id = "ALL"
            //}

            if ($scope.myForm.$valid) {

                var data = {
                    "LMD_Id": $scope.lmD_Id,
                    "Fromdate": fromdate,
                    "ToDate": todate,
                   // "LMAL_Id": $scope.LMAL_Id,
                }
                apiService.create("SubscriptionReport/get_report", data).then(function (promise) {

                    if (promise.reportlist.length > 0) {
                        $scope.reportlist = promise.reportlist;

                        $scope.tablediv = true;
                        $scope.printdoption = false;
                    }
                    else {
                        swal("Record Not Found!");
                        $scope.tablediv = false;
                        $scope.printd = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        //-------------End---Get Repoet....



        //===========print===========//
        $scope.printdata = function () {
            if ($scope.filterValue !== null && $scope.filterValue.length > 0) {
                var innerContents = document.getElementById("printtable").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookArrivalReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        }
        //==============End==============//


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //===========----Clear Field
        $scope.Clearid = function () {

            $state.reload();
        }

        $scope.ExportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }


    }
})();

