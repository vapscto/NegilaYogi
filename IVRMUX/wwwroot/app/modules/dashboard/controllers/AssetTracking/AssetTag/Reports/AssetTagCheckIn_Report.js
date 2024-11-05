
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagCheckIn_ReportController', AssetTagCheckIn_ReportController);
    AssetTagCheckIn_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function AssetTagCheckIn_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            apiService.create("AssetTagCheckIn_Report/getloaddata", data).
                then(function (promise) {
                    $scope.get_ckIndetails = promise.get_ckIndetails;
                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_ckIndetails = "";
            $scope.get_ckInreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };


        //========================Tag
        $scope.togchkbxtg = function () {
            $scope.tgall = $scope.get_ckIndetails.every(function (tg) {
                return tg.tgck;
            });
        };
        $scope.isOptionsRequiredtg = function () {
            if ($scope.optionflag === 'Tag') {
                return !$scope.get_ckIndetails.some(function (options) {
                    return options.tgck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checktg = function (tg) {
            $scope.tgall = tg;
            var toggleStatus = $scope.tgall;
            angular.forEach($scope.get_ckIndetails, function (tg) {
                tg.tgck = toggleStatus;
            });
        };
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_ckIndetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item') {
                return !$scope.get_ckIndetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_ckIndetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //========================Store
        $scope.togchkbxST = function () {
            $scope.stall = $scope.get_ckIndetails.every(function (str) {
                return str.stck;
            });
        };
        $scope.isOptionsRequiredST = function () {
            if ($scope.optionflag === 'Store') {
                return !$scope.get_ckIndetails.some(function (options) {
                    return options.stck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkST = function (stri) {
            $scope.stall = stri;
            var toggleStatus = $scope.stall;
            angular.forEach($scope.get_ckIndetails, function (st) {
                st.stck = toggleStatus;
            });
        };
        //======================== Location
        $scope.togchkbxlo = function () {
            $scope.loall = $scope.get_ckIndetails.every(function (loc) {
                return loc.lock;
            });
        };
        $scope.isOptionsRequiredlo = function () {
            if ($scope.optionflag === 'Location') {
                return !$scope.get_ckIndetails.some(function (options) {
                    return options.lock;
                });
            } else {
                return false;
            }
        };
        $scope.all_checklo = function (loct) {
            $scope.loall = loct;
            var toggleStatus = $scope.loall;
            angular.forEach($scope.get_ckIndetails, function (loca) {
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

                $scope.ciItemArray = [];
                $scope.ciStoreArray = [];
                $scope.ciLocationArray = [];
                $scope.ciTagArray = [];
                if ($scope.optionflag === "All") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Item") {
                    angular.forEach($scope.get_ckIndetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.ciItemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "ciItemArray": $scope.ciItemArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Store") {
                    angular.forEach($scope.get_ckIndetails, function (st) {
                        if (st.stck === true) {
                            $scope.ciStoreArray.push(st);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "ciStoreArray": $scope.ciStoreArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Location") {
                    angular.forEach($scope.get_ckIndetails, function (lo) {
                        if (lo.lock === true) {
                            $scope.ciLocationArray.push(lo);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "ciLocationArray": $scope.ciLocationArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Tag") {
                    angular.forEach($scope.get_ckIndetails, function (tg) {
                        if (tg.tgck === true) {
                            $scope.ciTagArray.push(tg);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "ciTagArray": $scope.ciTagArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                apiService.create("AssetTagCheckIn_Report/onreport", data).
                    then(function (promise) {
                        if (promise.get_ckInreport.length > 0) {
                            $scope.get_ckInreport = promise.get_ckInreport;
                            $scope.presentCountgrid = $scope.get_ckInreport.length;
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_ckInreport = "";
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

            var innerContents = document.getElementById("printagci").innerHTML;
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