
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_PI_ReportController', INV_PI_ReportController);
    INV_PI_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_PI_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            apiService.create("INV_PI_Report/getloaddata", data).
                then(function (promise) {
                    $scope.get_PI_details = promise.get_PI_details;
                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_PI_details = "";
            $scope.get_PIreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };
        //===================================PI Number Select
        $scope.togchkbxG = function () {
            $scope.piall = $scope.get_PI_details.every(function (itm) {
                return itm.pick;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.get_PI_details.some(function (options) {
                return options.pick;
            });
        };
        $scope.all_check = function (pial) {
            $scope.piall = pial;
            var toggleStatus = $scope.piall;
            angular.forEach($scope.get_PI_details, function (pi) {
                pi.pick = toggleStatus;
            });
        };
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_PI_details.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item') {
                return !$scope.get_PI_details.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_PI_details, function (itm) {
                itm.itemck = toggleStatus;
            });
        };

        //==================================PI Report
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
                $scope.piArray = [];
                $scope.itemArray = [];
                if ($scope.optionflag === "All") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "PIno") {
                    angular.forEach($scope.get_PI_details, function (pi) {
                        if (pi.pick === true) {
                            $scope.piArray.push(pi);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "piArray": $scope.piArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Item") {
                    angular.forEach($scope.get_PI_details, function (itm) {
                        if (itm.itemck === true) {
                            $scope.itemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "itemArray": $scope.itemArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }

                //==================mob
                else if ($scope.optionflag === "PI") {

                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,

                    };
                }

                else if ($scope.optionflag === "Itm") {

                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                    };
                }

                //================================

                apiService.create("INV_PI_Report/onreport", data).
                    then(function (promise) {
                        if (promise.get_PIreport.length > 0) {
                            var totalamt = 0.00;
                            $scope.get_PIreport = promise.get_PIreport;
                            $scope.presentCountgrid = $scope.get_PIreport.length;

                            angular.forEach($scope.get_PIreport, function (pi) {
                                totalamt += parseFloat(pi.INVTPI_ApproxAmount);
                                $scope.totalamt = totalamt;
                                $scope.totalamt = parseFloat($scope.totalamt);
                                $scope.totalamt = $scope.totalamt.toFixed(2);
                            });
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_PIreport = "";
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
        //$scope.exportToExcel = function (export_id) {

        //    var exportHref = Excel.tableToExcel(export_id, 'printpr');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printpi").innerHTML;
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