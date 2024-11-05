(function () {
    'use strict';
    angular
        .module('app')
        .controller('V_AppointmentApprovalReport', V_AppointmentApprovalReport)

    V_AppointmentApprovalReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function V_AppointmentApprovalReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

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

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.searchValue2 = "";

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey2 == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey2 = key;
        }

        $scope.onselectradio = function () {
            $scope.Cumureport = false;
            $scope.Cumureport1 = false;
            $scope.screport = false;
            $scope.export = false;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.radiotype = "Approved";
        //===============loaddata 
        $scope.loadgrid = function () {
            apiService.getURI("V_AppointmentApprovalReport/loaddata/", 5).then(function (promise) {
                $scope.institutionlist = promise.institutionlist;
                $scope.month_list = promise.month_list;
            });
        }



        // ======================to get Report
        $scope.submitted = false;
        $scope.report = function () {
            $scope.griddata = [];
            $scope.submitted = true;
            $scope.selected_Inst = [];
         

            var fromdate1 = $scope.startfromdate == null ? "" : $filter('date')($scope.startfromdate, "yyyy-MM-dd");
            var todate1 = $scope.startenddate == null ? "" : $filter('date')($scope.startenddate, "yyyy-MM-dd");

            if ($scope.myForm.$valid) {

                var data = {
                    "fromdate": fromdate1,
                    "todate": todate1,
                    "all1": $scope.all1,
                    "month_id": $scope.month,
                    "radiotype": $scope.radiotype,
                }

                apiService.create("V_AppointmentApprovalReport/report", data).
                    then(function (promise) {

                        if (promise.viewlist.length > 0) {

                            $scope.griddata = promise.viewlist;
                            $scope.Cumureport = true;
                            $scope.Cumureport1 = false;

                            $scope.screport = true;
                            $scope.export = true;
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

        //for print
        $scope.Print = function () {
            var innerContents = '';           
            innerContents = document.getElementById("printSectionId").innerHTML;        
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

        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');

            $timeout(function () { location.href = exportHref; }, 100);
        }
     
        $scope.cancel = function () {          
            $state.reload();

        };

        $scope.togchkbx = function () {
            $scope.institutionlist.every(function (options) {
                return options.select;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.institutionlist.some(function (options) {
                return options.select;
            });
        }
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.institutionlist, function (itm) {
                itm.select = checkStatus;
            });
        }


    }

})();