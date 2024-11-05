
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_StockSummaryReportController', INV_StockSummaryReportController);
    INV_StockSummaryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_StockSummaryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {

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
        $scope.search = "";
        //==========================================================

        $scope.loaddata = function () {            
            var data = {
                "optionflag": $scope.optionflag
            };
            apiService.create("INV_StockSummary/getloaddata", data).
                then(function (promise) {
                    $scope.get_itemreportdetails = promise.get_report; 
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
           // if ($scope.optionflag === 'Item') {
               
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
            $scope.submitted = true;
            if ($scope.myForm.$valid) {                            
                $scope.itemsArray = [];
                var startdate = "";
                var enddate = "";
                if ($scope.bw_dates == true) {
                    startdate = new Date($scope.startdate).toDateString();
                    enddate = new Date($scope.enddate).toDateString();
                }

                if ($scope.get_itemreportdetails != null && $scope.get_itemreportdetails.length > 0) {
                    angular.forEach($scope.get_itemreportdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.itemsArray.push(itm);
                        }
                    });
                }                           
                var data = {
                    "optionflag": $scope.optionflag,
                    "startdate": startdate ,
                    "enddate": enddate ,                 
                    "itemsArray": $scope.itemsArray
                };
                apiService.create("INV_StockSummary/onreport", data).
                    then(function (promise) {
                        if (promise.stock_summaryreport != null && promise.stock_summaryreport.length > 0) {
                             $scope.imgname=promise.logopath;
                            $scope.get_report = promise.stock_summaryreport;
                           
                             var ds=0;
                              $scope.employee = [];
                            $scope.employee = $scope.get_report[0].INVMI_ItemName;
                            $scope.employeeid = [];
                            angular.forEach($scope.get_report, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                      INVMI_Id:dev.INVMI_Id,
                                        INVMI_ItemCode:dev.INVMI_ItemCode,
                                   INVMI_ItemName:dev.INVMI_ItemName,
                                 INVSTO_BatchNo:dev.INVSTO_BatchNo,
                                  INVSTO_SalesRate:dev.INVSTO_SalesRate,
                                       SalesQty:dev.SalesQty,
                                   StockValue:dev.StockValue,
                                 
                                    });
                                }
                                else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.INVMI_Id === dev.INVMI_Id) {
                                            intcount += 1;
                                           
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            INVMI_Id:dev.INVMI_Id,
                                        INVMI_ItemCode:dev.INVMI_ItemCode,
                                   INVMI_ItemName:dev.INVMI_ItemName,
                                 INVSTO_BatchNo:dev.INVSTO_BatchNo,
                                  INVSTO_SalesRate:dev.INVSTO_SalesRate,
                                       SalesQty:dev.SalesQty,
                                   StockValue:dev.StockValue,
                                    
                                        });
                                    }
                                }
                            });
                            console.log($scope.employeeid);
                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                 
                                angular.forEach($scope.get_report, function (dd) {
                                    if (dd.INVMI_Id === ddd.INVMI_Id) {
                                        $scope.templist.push(dd);
                                       

                                    }
                                });
                                ddd.plannerdetails = $scope.templist;
                            
                            });
                            console.log($scope.employeeid);
                            $scope.presentCountgrid = $scope.employeeid.length;
                        }
                        else {
                            swal("No Record Found...!!");
                          
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