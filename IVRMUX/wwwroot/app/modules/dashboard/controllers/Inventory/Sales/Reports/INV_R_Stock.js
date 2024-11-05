﻿
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_R_StockController', INV_R_StockController);
    INV_R_StockController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_R_StockController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


        $scope.obj = {};
        $scope.checkoutflag = false;
        $scope.optionflag = "All";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings!=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings!=null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        //==========================================================

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var ckout = 0;
            var data = {
                "optionflag": $scope.optionflag
            };
            apiService.create("INV_R_Stock/getloaddata", data).
                then(function (promise) {
                    $scope.get_stockdetails = promise.get_stockdetails;

                    angular.forEach($scope.get_stockdetails, function (stk) {
                        ckout += stk.INVSTO_CheckedOutQty;
                    });
                    if (ckout > 0) {
                        $scope.checkoutflag = true;
                    }
                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_stockdetails = "";
            $scope.get_StockReport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };

        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_stockdetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item') {
                return !$scope.get_stockdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_stockdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //========================ITEM
        $scope.togchkbxST = function () {
            $scope.stall = $scope.get_stockdetails.every(function (str) {
                return str.stck;
            });
        };
        $scope.isOptionsRequiredST = function () {
            if ($scope.optionflag === 'Store') {
                return !$scope.get_stockdetails.some(function (options) {
                    return options.stck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkST = function (stri) {
            $scope.stall = stri;
            var toggleStatus = $scope.stall;
            angular.forEach($scope.get_stockdetails, function (st) {
                st.stck = toggleStatus;
            });
        };

        //==================================Stock Report
        $scope.submitted = false;
        $scope.onreport = function () {

            if ($scope.myForm.$valid) {
                var data = {};
                var icCount = 0;
                var ckoutCount = 0;
                var stockPcount = 0;
                var stockMcount = 0;
                var cstk = 0;

                if ($scope.overallflg === true) {
                    $scope.overallflag = "Overall";
                }
                else {
                    $scope.overallflag = "";
                }
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

                if ($scope.optionflag === "All") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": 0,
                        "INVMST_Id": 0,
                        "INVMG_Id": 0,
                        "bwdateflag": $scope.bw_dates,
                        "overallflag": $scope.overallflag
                    };
                }
                else if ($scope.optionflag === "Item") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": $scope.obj.INVMI_Id.INVMI_Id,
                        "INVMST_Id": 0,
                        "INVMG_Id": 0,
                        "bwdateflag": $scope.bw_dates,
                        "overallflag": $scope.overallflag
                    };
                }
                else if ($scope.optionflag === "Store") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "tagStoreArray": $scope.tagStoreArray,
                        "INVMI_Id": 0,
                        "INVMST_Id": $scope.obj.INVMST_Id.INVMST_Id,
                        "INVMG_Id": 0,
                        "bwdateflag": $scope.bw_dates,
                        "overallflag": $scope.overallflag
                    };
                }
                else if ($scope.optionflag === "Group") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": 0,
                        "INVMST_Id": 0,
                        "INVMG_Id": $scope.obj.INVMG_Id.INVMG_Id,
                        "bwdateflag": $scope.bw_dates,
                        "overallflag": $scope.overallflag
                    };
                }
                apiService.create("INV_R_Stock/onreport", data).
                    then(function (promise) {
                        if (promise.get_StockReport.length > 0) {
                            $scope.get_StockReport = promise.get_StockReport;
                            $scope.presentCountgrid = $scope.get_StockReport.length;
                            angular.forEach($scope.get_StockReport, function (stk) {
                                if (stk.INVSTO_AvaiableStock !== null) {
                                    cstk += stk.INVSTO_AvaiableStock;
                                }
                                if (stk.INVSTO_ItemConQty !== null) {
                                    icCount += stk.INVSTO_ItemConQty;
                                }
                                if (stk.INVSTO_CheckedOutQty !== null) {
                                    ckoutCount += stk.INVSTO_CheckedOutQty;
                                }
                                if (stk.INVSTO_PhyPlusQty !== null) {
                                    stockPcount += stk.INVSTO_PhyPlusQty;
                                }
                                if (stk.INVSTO_PhyMinQty !== null) {
                                    stockMcount += stk.INVSTO_PhyMinQty;
                                }
                            });
                            $scope.clstoc = cstk;
                            if (icCount > 0) {
                                $scope.viewIC = true;
                            } else {
                                $scope.viewIC = false;
                            }
                            if (ckoutCount > 0) {
                                $scope.viewCKout = true;
                            } else {
                                $scope.viewCKout = false;
                            }
                            if (stockPcount > 0) {
                                $scope.viewSP = true;
                            } else {
                                $scope.viewSP = false;
                            }
                            if (stockMcount > 0) {
                                $scope.viewSM = true;
                            } else {
                                $scope.viewSM = false;
                            }
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_StockReport = "";
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
            var excelname = "";
            var exportHref = "";
            var excelsheetname = "";
            var reportname = "";
            if ($scope.overallflg == true && $scope.optionflag == "All") {
                reportname = "#printStock1";
                excelsheetname = "OVER ALL STOCK";
            }
            else if ($scope.overallflg == "" && ($scope.optionflag == "Item" || $scope.optionflag == "Store" || $scope.optionflag == "Group")) {
                reportname = "#printStock2";
                excelsheetname = "STOCK";
            }

            else if ($scope.overallflg == true && ($scope.optionflag == "Item" || $scope.optionflag == "Store" || $scope.optionflag == "Group")) {
                reportname = "#printStock1";
                excelsheetname = "OVER ALL STOCK";
            }
            else if ($scope.optionflag == "All" || $scope.optionflag == "Item" || $scope.optionflag == "Store" || $scope.optionflag == "Group") {
                reportname = "#printStock2";
                excelsheetname = "STOCK";
            }
            
            excelname = excelsheetname + ' REPORT ';
            excelname = excelname.toUpperCase() + '.xls';
            exportHref = Excel.tableToExcel(reportname, excelsheetname);
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        };


        $scope.printData = function () {
            var innerContents = "";
            var name = "";
            if ($scope.overallflg == true && $scope.optionflag == "All") {
                name = "printStock1";
            }
            else if ($scope.overallflg == "" && ($scope.optionflag == "Item" || $scope.optionflag == "Store" || $scope.optionflag == "Group")) {
                name = "printStock2";
            }
            else if ($scope.overallflg == true && ($scope.optionflag == "Item" || $scope.optionflag == "Store" || $scope.optionflag == "Group")) {
                name = "printStock1";
            }
            else if ($scope.optionflag == "All" || $scope.optionflag == "Item" || $scope.optionflag == "Store" || $scope.optionflag == "Group") {
                name = "printStock2";
            }
            innerContents = document.getElementById(name).innerHTML;
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