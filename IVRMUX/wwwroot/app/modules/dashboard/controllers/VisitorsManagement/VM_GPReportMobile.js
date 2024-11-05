(function () {
    'use strict';
    angular
        .module('app')
        .controller('VM_GPReportMobileController', VM_GPReportMobileController)

    VM_GPReportMobileController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function VM_GPReportMobileController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {


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



        $scope.exportbutton = true;
        $scope.screportbutton = true;

        $scope.onselectradio = function () {
            $scope.Cumureport = false;
            $scope.Cumureport1 = false;
            $scope.screportbutton = true;
            $scope.exportbutton = true;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //==========================Load Data.
        $scope.loaddata = function () {
            apiService.getURI("GatePassReport/loaddata/", 5).then(function (promise) {
                $scope.month_list = promise.month_list;
            });
        }



        ///===============================================================

        // TO Show The Data
        $scope.submitted = false;
        $scope.showreport = function () {
            debugger;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var fromdate1 = $scope.startfromdate == null ? "" : $filter('date')($scope.startfromdate, "yyyy-MM-dd");
              

                var data = {
                    "radiotype": $scope.radiotype,
                    "fromdate": fromdate1,
                }

                apiService.create("GatePassReport/reportforMobile", data).
                    then(function (promise) {

                        $scope.yearname = promise.yarname[0].asmaY_Year;

                        if (promise.viewlist.length > 0) {
                            if ($scope.radiotype == 'std') {
                                $scope.newuser = promise.viewlist;
                                $scope.presentCountgrid = $scope.newuser.length;
                                $scope.Cumureport = true;
                                $scope.Cumureport1 = false;

                            }
                            else if ($scope.radiotype == 'emp') {
                                $scope.newuser1 = promise.viewlist;
                                $scope.presentCountgrid = $scope.newuser1.length;
                                $scope.Cumureport1 = true;
                                $scope.Cumureport = false;

                            }
                            $scope.screportbutton = false;
                            $scope.exportbutton = false;
                        }

                        else {
                            swal("No Records Found");

                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            $scope.Cumureport1 = false;
                        }

                    })
            }
        };
        //=================================================================

      

        //for print
        $scope.Print = function () {
            var innerContents = '';
            if ($scope.radiotype == 'std') {
                innerContents = document.getElementById("printSectionId").innerHTML;
            }
            if ($scope.radiotype == 'emp') {
                innerContents = document.getElementById("printSectionId1").innerHTML;
            }
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/Visitor_Management/InwardReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }

        // end for print

        $scope.exportToExcel = function () {
            var innerContents = '';
            if ($scope.radiotype == 'std') {
                innerContents = document.getElementById("printSectionId").innerHTML;
                var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');

            }
            else if ($scope.radiotype == 'emp') {
                innerContents = document.getElementById("printSectionId1").innerHTML;
                var exportHref = Excel.tableToExcel(printSectionId1, 'sheet name');

            }
            $timeout(function () { location.href = exportHref; }, 100);

        }

        $scope.cancel = function () {
            $scope.radiotype = "";
            $scope.Cumureport = false;
            $scope.Cumureport1 = false;
            $scope.screportbutton = true;
            $scope.exportbutton = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.startfromdate = ""; 

            $state.reload();
        };


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



        $scope.searchValue = "";
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.sortReverse = !$scope.sortReverse; //if true make it false and vice versa
        }

        $scope.searchValue2 = "";
        $scope.itemsPerPage2 = 10;
        $scope.currentPage2 = 1;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.sortReverse2 = !$scope.sortReverse2; //if true make it false and vice versa
        }

        

    }

})();