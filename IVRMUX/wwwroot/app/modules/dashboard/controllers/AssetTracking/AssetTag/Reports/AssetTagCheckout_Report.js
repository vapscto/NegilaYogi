
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagCheckout_ReportController', AssetTagCheckout_ReportController);
    AssetTagCheckout_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function AssetTagCheckout_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


        $scope.obj = {};
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
            apiService.create("AssetTagCheckout_Report/getloaddata", data).
                then(function (promise) {
                    $scope.get_ckoutdetails = promise.get_ckoutdetails;
                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_ckoutdetails = "";
            $scope.get_ckoutreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };
        //========================Tag
        $scope.togchkbxtg = function () {
            $scope.tgall = $scope.get_ckoutdetails.every(function (tg) {
                return tg.tgck;
            });
        };
        $scope.isOptionsRequiredtg = function () {
            if ($scope.optionflag === 'Tag') {
                return !$scope.get_ckoutdetails.some(function (options) {
                    return options.tgck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checktg = function (tg) {
            $scope.tgall = tg;
            var toggleStatus = $scope.tgall;
            angular.forEach($scope.get_ckoutdetails, function (tg) {
                tg.tgck = toggleStatus;
            });
        };
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_ckoutdetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item') {
                return !$scope.get_ckoutdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_ckoutdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //========================Store
        $scope.togchkbxST = function () {
            $scope.stall = $scope.get_ckoutdetails.every(function (str) {
                return str.stck;
            });
        };
        $scope.isOptionsRequiredST = function () {
            if ($scope.optionflag === 'Store') {
                return !$scope.get_ckoutdetails.some(function (options) {
                    return options.stck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkST = function (stri) {
            $scope.stall = stri;
            var toggleStatus = $scope.stall;
            angular.forEach($scope.get_ckoutdetails, function (st) {
                st.stck = toggleStatus;
            });
        };
        //======================== Location
        $scope.togchkbxlo = function () {
            $scope.loall = $scope.get_ckoutdetails.every(function (loc) {
                return loc.lock;
            });
        };
        $scope.isOptionsRequiredlo = function () {
            if ($scope.optionflag === 'Location') {
                return !$scope.get_ckoutdetails.some(function (options) {
                    return options.lock;
                });
            } else {
                return false;
            }
        };
        $scope.all_checklo = function (loct) {
            $scope.loall = loct;
            var toggleStatus = $scope.loall;
            angular.forEach($scope.get_ckoutdetails, function (loca) {
                loca.lock = toggleStatus;
            });
        };

        //==================================PO Report
        $scope.submitted = false;
        $scope.onreport = function () {
            if ($scope.myForm.$valid) {
                var data = {};
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

                $scope.coItemArray = [];
                $scope.coStoreArray = [];
                $scope.coLocationArray = [];
                $scope.coTagArray = [];
                if ($scope.optionflag === "All") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Item") {
                    angular.forEach($scope.get_ckoutdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.coItemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "coItemArray": $scope.coItemArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Store") {
                    angular.forEach($scope.get_ckoutdetails, function (st) {
                        if (st.stck === true) {
                            $scope.coStoreArray.push(st);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "coStoreArray": $scope.coStoreArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Location") {
                    angular.forEach($scope.get_ckoutdetails, function (lo) {
                        if (lo.lock === true) {
                            $scope.coLocationArray.push(lo);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "coLocationArray": $scope.coLocationArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Tag") {
                    angular.forEach($scope.get_ckoutdetails, function (tg) {
                        if (tg.tgck === true) {
                            $scope.coTagArray.push(tg);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "coTagArray": $scope.coTagArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                apiService.create("AssetTagCheckout_Report/onreport", data).
                    then(function (promise) {
                        if (promise.get_ckoutreport.length > 0) {
                            $scope.get_ckoutreport = promise.get_ckoutreport;
                            $scope.presentCountgrid = $scope.get_ckoutreport.length;
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_ckoutreport = "";
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

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printagco").innerHTML;
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