

(function () {
    'use strict';
    angular
        .module('app')
        .controller('LostBookReportController', LostBookReportController)

    LostBookReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout', 'Excel']
    function LostBookReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel) {

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
     

        $scope.search = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.BookType = 'Issue';
        $scope.BNBFlg = 'Book';
        //---------------Load --data
        $scope.Loaddata = function () {
           
            debugger;
            var pageid = 2;
            apiService.getURI("LostBookReport/getdetails", pageid).then(function (promise) {
           
                $scope.lib_list = promise.lib_list;
            })
        }
        //------------End---Load --data
      



        //---------------Get Repoet....
        $scope.get_report = function () {
            debugger;
            var fromdate = $scope.Fromdate == null ? "" : $filter('date')($scope.Fromdate, "yyyy-MM-dd");
            var todate = $scope.ToDate == null ? "" : $filter('date')($scope.ToDate, "yyyy-MM-dd");
           
          

            if ($scope.myForm.$valid) {

                var data = {
                    "BookType": $scope.BookType,
                    "BNBFlg": $scope.BNBFlg,
                    "Fromdate": fromdate,
                    "ToDate": todate,
                    "LMAL_Id": $scope.LMAL_Id,
                }
                apiService.create("LostBookReport/get_report", data).then(function (promise) {

                    if (promise.reportlist.length > 0) {
                        $scope.reportlist = promise.reportlist;

                        $scope.tablediv = true;
                        $scope.printd = true;
                        $scope.excel = true;

                    }
                    else {
                        swal("Record Not Found!");
                        $scope.tablediv = false;
                        $scope.printd = false;
                        $scope.excel = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
       //-------------End---Get Repoet....
        


     
        $scope.tablediv = false;;
        $scope.printd = false;

        //===========print===========//
        $scope.printData = function () {
            var innerContents = document.getElementById("printtable").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookTypeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        //==============End==============//

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //-----------clear-field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

