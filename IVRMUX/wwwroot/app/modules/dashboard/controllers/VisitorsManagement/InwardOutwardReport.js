(function () {
    'use strict';
    angular
        .module('app')
        .controller('InwardOutwardReportController', InwardOutwardReportController)

    InwardOutwardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function InwardOutwardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

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


        $scope.loadgrid = function () {
            apiService.getURI("GatePassReport/loaddata/", 5).then(function (promise) {

                $scope.month_list = promise.month_list;
            });
        }



        // TO Show The Data
        $scope.submitted = false;
        $scope.report = function () {
            

            $scope.submitted = true;

            var fromdate1 = $scope.startfromdate == null ? "" : $filter('date')($scope.startfromdate, "yyyy-MM-dd");
            var todate1 = $scope.startenddate == null ? "" : $filter('date')($scope.startenddate, "yyyy-MM-dd");
          
            if ($scope.myForm.$valid) {

                var data = {
                    "radiotype": $scope.radiotype,
                    "fromdate": fromdate1,
                    "todate": todate1,
                    "all1": $scope.all1,
                    "month_id": $scope.month,
                }

                apiService.create("InwardOutwardReport/report", data).
                    then(function (promise) {

                        if (promise.viewlist.length > 0) {
                            if ($scope.radiotype == 'inward') {
                                $scope.newuser = promise.viewlist;
                                $scope.presentCountgrid = $scope.newuser.length;
                                $scope.Cumureport = true;
                                $scope.Cumureport1 = false;
                            }
                            else if ($scope.radiotype == 'outward') {
                                $scope.newuser1 = promise.viewlist;
                                $scope.presentCountgrid = $scope.newuser1.length;
                                $scope.Cumureport1 = true;
                                $scope.Cumureport = false;
                            }
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
            if ($scope.radiotype == 'inward') {
                innerContents = document.getElementById("PrintInwardData").innerHTML;
            }
            if ($scope.radiotype == 'outward') {
                innerContents = document.getElementById("PrintOutwardData").innerHTML;
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

        $scope.exportToExcel = function (table) {
            if ($scope.radiotype == 'inward') {
                var exportHref = Excel.tableToExcel(PrintInwardData, 'sheet name');
            }
            else if ($scope.radiotype == 'outward') {
                var exportHref = Excel.tableToExcel(PrintOutwardData, 'sheet name');
            }
            $timeout(function () { location.href = exportHref; }, 100);

        }
        $scope.cancel = function () {
            $scope.radiotype = "";
            $scope.Cumureport = false;
            $scope.Cumureport1 = false;
            $scope.screport = false;
            $scope.export = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";

            $scope.startfromdate = "";
            $scope.startenddate = "";
            $scope.month = "";
            $state.reload();
            
        };




        //////////////////////////////////////////////////////////................OLD CODE...................////////////////////////////////////////////////

        //$scope.onselectradio = function () {
        //    $scope.Cumureport = false;
        //    $scope.Cumureport1 = false;
        //    $scope.screport = false;
        //    $scope.export = false;
        //}

        //$scope.interacted = function (field) {
        //    return $scope.submitted;
        //};

       
        //// TO Show The Data
        //$scope.submitted = false;
        //$scope.report = function () {

        //    $scope.submitted = true;
        //    if ($scope.myForm.$valid) {

        //        var data = {
        //            "radiotype": $scope.radiotype
        //        }

        //        apiService.create("InwardOutwardReport/report", data).
        //            then(function (promise) {

        //                if (promise.viewlist.length > 0) {
        //                    if ($scope.radiotype == 'inward') {
        //                        $scope.newuser = promise.viewlist;
        //                        $scope.presentCountgrid = $scope.newuser.length;
        //                        $scope.Cumureport = true;
        //                        $scope.Cumureport1 = false;
        //                    }
        //                    else if ($scope.radiotype == 'outward') {
        //                        $scope.newuser1 = promise.viewlist;
        //                        $scope.presentCountgrid = $scope.newuser1.length;
        //                        $scope.Cumureport1 = true;
        //                        $scope.Cumureport = false;
        //                    }                          
        //                    $scope.screport = true;
        //                    $scope.export = true;
        //                }
                 
        //                else {
        //                    swal("No Records Found");
        //                }

        //            })
        //    }
        //};

        ////for print
        //$scope.Print = function () {
        //    var innerContents = '';
        //        if ($scope.radiotype == 'inward') {
        //            innerContents = document.getElementById("printSectionId").innerHTML;
        //        }
        //        if ($scope.radiotype == 'outward') {
        //            innerContents = document.getElementById("printSectionId1").innerHTML;
        //        }
        //        var popupWinindow = window.open('');
        //        popupWinindow.document.open();
        //        popupWinindow.document.write('<html><head>' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //            '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
        //            '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
        //        );
        //        popupWinindow.document.close();
        //}

        //// end for print

        //$scope.exportToExcel = function (table) {
        //    if ($scope.radiotype == 'inward') {
        //        var exportHref = Excel.tableToExcel(datatable, 'sheet name');
        //    }
        //    else if ($scope.radiotype == 'outward') {
        //        var exportHref = Excel.tableToExcel(datatable1, 'sheet name');
        //    }
        //    $timeout(function () { location.href = exportHref; }, 100);

        //}      
        //$scope.cancel = function () {
        //    $scope.radiotype = "";
        //    $scope.Cumureport = false;
        //    $scope.Cumureport1 = false;
        //    $scope.screport = false;
        //    $scope.export = false;
        //    $scope.submitted = false;
        //    $scope.myForm.$setPristine();
        //    $scope.myForm.$setUntouched();
        //    $scope.search = "";
        //};



    }

})();