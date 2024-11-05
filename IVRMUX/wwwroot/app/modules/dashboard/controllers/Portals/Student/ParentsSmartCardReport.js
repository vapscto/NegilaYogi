
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ParentsSmartCardReportController', ParentsSmartCardReportController)

    ParentsSmartCardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', '$window', 'superCache', '$compile', '$timeout', 'Excel']
    function ParentsSmartCardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, $window, superCache, $compile,$timeout, Excel) {


        $scope.maxDatemf = new Date();
        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.FromDate);
            $scope.maxDatemf = new Date();
        };
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var copty;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
      
       



        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.showgrid = false;
        $scope.searchValue = '';
        var details = "";
        $scope.parent = "S";
        $scope.STP_FLAG = "ALL";
        $scope.imgname = logopath;
        $scope.uploadDocs_img = [];
        $scope.images_temp = [];
        var id = 1;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.loaddata = function () {
        

            apiService.getDATA("ParentsSmartCard/getreploaddata").
                then(function (promise) {

                    $scope.acamiclist = promise.acamiclist;
                   

                })
        };

        //sent Request 
        $scope.getreport = function () {
            debugger;
            $scope.submitted = false;
            if ($scope.myForm.$valid) {

                var fromdate1 = $scope.FromDate == null ? "" : $filter('date')($scope.FromDate, "yyyy-MM-dd");
                var todate1 = $scope.ToDate == null ? "" : $filter('date')($scope.ToDate, "yyyy-MM-dd");


                

                var data = {
                    "STP_FLAG": $scope.STP_FLAG,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FromDate": fromdate1,
                    "ToDate": todate1,
                }
                apiService.create("ParentsSmartCard/getreport", data).
                    then(function (promise) {
                        $scope.updatestudetailslist = promise.updatestudetailslist;
                        if (promise.updatestudetailslist != null && promise.updatestudetailslist.length>0) {
                            $scope.updatestudetailslist = promise.updatestudetailslist;
                        }
                        else {
                            swal('No Record Found')
                        }
                        
                                });

            }
            else {
                $scope.submitted = true;
            }

        }


        //end


        //===========print===========//
        $scope.printData = function () {
            var innerContents = document.getElementById("printtable").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                //'<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookTypeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
       

      

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //-------------Save Data student
        $scope.submitted = false;
      



        //-----Save Data Admin
        $scope.submitted = false;
       

      

        



        //Cancel
        $scope.cancel = function () {

            $state.reload();
            //$scope.clear();
        }

    };
})();