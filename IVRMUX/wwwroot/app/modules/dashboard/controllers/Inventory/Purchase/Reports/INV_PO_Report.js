
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_PO_ReportController', INV_PO_ReportController);
    INV_PO_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_PO_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            apiService.create("INV_PO_Report/getloaddata", data).
                then(function (promise) {
                    $scope.get_POdetails = promise.get_POdetails;
                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_POdetails = "";
            $scope.get_POreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };
        //===================================PO Number Select
        $scope.togchkbx = function () {
            $scope.poall = $scope.get_POdetails.every(function (itm) {
                return itm.pock;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.get_POdetails.some(function (options) {
                return options.pock;
            });
        };
        $scope.all_check = function (poal) {
            $scope.poall = poal;
            var toggleStatus = $scope.poall;
            angular.forEach($scope.get_POdetails, function (po) {
                po.pock = toggleStatus;
            });
        };
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_POdetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item') {
                return !$scope.get_POdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_POdetails, function (itm) {
                itm.itemck = toggleStatus;
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
                $scope.poArray = [];
                $scope.poitemArray = [];
                $scope.posuplierArray = [];
                if ($scope.optionflag === "All") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "PONo") {
                    angular.forEach($scope.get_POdetails, function (po) {
                        if (po.pock === true) {
                            $scope.poArray.push(po);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "poArray": $scope.poArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Item") {
                    angular.forEach($scope.get_POdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.poitemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "poitemArray": $scope.poitemArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Supplier") {
                    angular.forEach($scope.get_POdetails, function (sp) {
                        if (sp.spck === true) {
                            $scope.posuplierArray.push(sp);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "posuplierArray": $scope.posuplierArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                    //==================mob
                else if ($scope.optionflag === "PO") {
                   
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

                apiService.create("INV_PO_Report/onreport", data).
                    then(function (promise) {
                        if (promise.get_POreport.length > 0) {
                            var totalrate = 0.00;
                            var totaltax = 0.00;
                            var totalamt = 0.00;                         
                            $scope.get_POreport = promise.get_POreport;
                            $scope.presentCountgrid = $scope.get_POreport.length;

                            angular.forEach($scope.get_POreport, function (po) {
                                totalrate += parseFloat(po.INVTPO_RatePerUnit);
                                $scope.totalrate = totalrate;
                                $scope.totalrate = parseFloat($scope.totalrate);
                                $scope.totalrate = $scope.totalrate.toFixed(2);

                                totaltax += parseFloat(po.INVTPO_TaxAmount);
                                $scope.totaltax = totaltax;
                                $scope.totaltax = parseFloat($scope.totaltax);
                                $scope.totaltax = $scope.totaltax.toFixed(2);

                                totalamt += parseFloat(po.INVTPO_Amount);
                                $scope.totalamt = totalamt;
                                $scope.totalamt = parseFloat($scope.totalamt);
                                $scope.totalamt = $scope.totalamt.toFixed(2);
                            });
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_POreport = "";
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

            var innerContents = document.getElementById("printpo").innerHTML;
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