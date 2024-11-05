
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_ItemReportController', INV_ItemReportController);
    INV_ItemReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_ItemReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {

        $scope.obj = {};
        $scope.optionflag = "Item";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        //==========================================================

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var data = {
                "optionflag": $scope.optionflag
            };
            apiService.create("INV_ItemReport/getloaddata", data).
                then(function (promise) {
                    $scope.get_itemreportdetails = promise.get_itemreportdetails;
                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_itemreportdetails = "";
            $scope.get_itemreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
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
            //if ($scope.optionflag === 'Item') {
               
            //} else {
            //    return false;
            //}
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
        $scope.onreport = function () {
            if ($scope.myForm.$valid) {
               
                $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                if ($scope.bw_dates === true) {
                    $scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                }
                else {
                    $scope.sDate = "";
                    $scope.eDate = "";
                }


                $scope.itemsArray = [];
                if ($scope.optionflag === null || $scope.optionflag === undefined) {
                    $scope.optionflag = "";
                }
                var data = {};
                if ($scope.optionflag === "Item") {
                    angular.forEach($scope.get_itemreportdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.itemsArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "itemsArray": $scope.itemsArray,
                        "INVMG_Id": 0,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMG_Id": $scope.obj.INVMG_Id.INVMG_Id,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                apiService.create("INV_ItemReport/onreport", data).
                    then(function (promise) {
                        if (promise.get_itemreport.length > 0) {
                            $scope.get_itemreport = promise.get_itemreport;
                            $scope.presentCountgrid = $scope.get_itemreport.length;
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_itemreport = "";
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