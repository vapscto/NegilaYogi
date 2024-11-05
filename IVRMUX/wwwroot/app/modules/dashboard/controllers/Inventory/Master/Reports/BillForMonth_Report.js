
(function () {
    'use strict';
    angular
        .module('app')
        .controller('BillForMonth_ReportController', BillForMonth_ReportController);
    BillForMonth_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function BillForMonth_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {

        $scope.obj = {};
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";      
        //==========================================================
        $scope.loaddata = function () {
            var data = {
                "optionflag": $scope.optionflag
            };
            apiService.create("INV_StockSummary/getloaddata", data).
                then(function (promise) {                 
                    $scope.Select_list = promise.select_list;
                    if ($scope.ASMAY_Id === promise.yearlist[0].asmaY_Id) {
                    }
                    else {
                        $scope.academicdrp = promise.yearlist;
                        $scope.ASMAY_Id = promise.yearlist[0].asmaY_Id;
                    }
                });
        };
        $scope.drop_catlist = function () {
            $scope.Section_list = [];
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("INV_StockSummary/get_load_onchange", data).then(function
                (promise) {
                $scope.Section_list = promise.section_list;                      
            });
        };
        $scope.onrdochange = function () {
            $scope.get_report = [];
            $scope.loaddata();
        };
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_itemreportdetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            return !$scope.get_itemreportdetails.some(function (options) {
                return options.itemck;
            });          
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_itemreportdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //==================================Item Report
        $scope.submitted = false;
        //$scope.onreport = function () {
        //    $scope.submitted = true;
        //    if ($scope.myForm.$valid) {
        //        $scope.itemsArray = [];
        //        var startdate = "";
        //        var enddate = "";
        //        if ($scope.bw_dates == true) {
        //            startdate = new Date($scope.startdate).toDateString();
        //            enddate = new Date($scope.enddate).toDateString();
        //        }

        //        if ($scope.get_itemreportdetails != null && $scope.get_itemreportdetails.length > 0) {
        //            angular.forEach($scope.get_itemreportdetails, function (itm) {
        //                if (itm.itemck === true) {
        //                    $scope.itemsArray.push(itm);
        //                }
        //            });
        //        }
        //        var data = {
        //            "optionflag": $scope.optionflag,
        //            "startdate": startdate,
        //            "enddate": enddate,
        //            "itemsArray": $scope.itemsArray,
        //            "ASMAY_Id": $scope.ASMAY_Id
        //        };
        //        apiService.create("INV_StockSummary/onreportthree", data).
        //            then(function (promise) {
        //                if (promise.stock_summaryreport != null && promise.stock_summaryreport.length > 0) {
        //                    $scope.imgname = promise.logopath;
        //                    $scope.get_report = promise.stock_summaryreport;  
        //                }
        //                else {
        //                    swal("No Record Found...!!");

        //                }
        //            });
        //    }

        //};

        //Added By Praveen Gouda
        $scope.onreport = function () {
            startdate = new Date($scope.startdate).toDateString();
            enddate = new Date($scope.enddate).toDateString();

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var startdate = "";
                var enddate = "";
                //if ($scope.bw_dates == true) {
                //    startdate = new Date($scope.startdate).toDateString();
                //    enddate = new Date($scope.enddate).toDateString();
                //}   
                //var ASMCL_Id = 0;
                //if ($scope.obj.ASMCL_Id > 0) {
                //    ASMCL_Id = $scope.obj.ASMCL_Id;
                //}
                var data = {
                    "optionflag": $scope.optionflag,               
                    "ASMAY_Id": $scope.ASMAY_Id,
                    //"ASMCL_Id": ASMCL_Id,
                    "ASMCL_Id": $scope.obj.ASMCL_Id,
                    "Startdate": startdate,
                    "Enddate": enddate
                };
                apiService.create("INV_StockSummary/onreportstock", data).
                    then(function (promise) {
                        if (promise.invstockreport != null && promise.invstockreport.length > 0) {
                            $scope.stock_report = promise.invstockreport;
                            $scope.presentCountgrid = $scope.stock_report.length;
                        }
                        else {
                            swal("No Record Found...!!");
                            $state.reload();
                        }
                    });
            }

        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        //======================================Print & Export to Excel
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'printItem');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printItem").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();