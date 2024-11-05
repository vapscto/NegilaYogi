
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_Quotation_ReportController', INV_Quotation_ReportController);
    INV_Quotation_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_Quotation_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            apiService.create("INV_Quotation_Report/getloaddata", data).
                then(function (promise) {
                    $scope.get_Quotedetails = promise.get_Quotedetails;
                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_Quotedetails = "";
            $scope.get_Quotationreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };
        //===================================Quotation Number 
        $scope.togchkbxQ = function () {
            $scope.qtall = $scope.get_Quotedetails.every(function (itm) {
                return itm.qtck;
            });
        };
        $scope.isOptionsRequiredQ = function () {
            if ($scope.optionflag === 'QuoteNo') {
                return !$scope.get_Quotedetails.some(function (options) {
                    return options.qtck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkQ = function (itmi) {
            $scope.qtall = itmi;
            var toggleStatus = $scope.qtall;
            angular.forEach($scope.get_Quotedetails, function (itm) {
                itm.qtck = toggleStatus;
            });
        };

        //===================================PI Number 
        $scope.togchkbx = function () {
            $scope.piall = $scope.get_Quotedetails.every(function (pi) {
                return pi.pick;
            });
        };
        $scope.isOptionsRequired = function () {
            if ($scope.optionflag === 'PINo') {
                return !$scope.get_Quotedetails.some(function (options) {
                    return options.pick;
                });
            }
            else {
                return false;
            }
        };
        $scope.all_check = function (pial) {
            $scope.piall = pial;
            var toggleStatus = $scope.piall;
            angular.forEach($scope.get_Quotedetails, function (pi) {
                pi.pick = toggleStatus;
            });
        };
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_Quotedetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item') {
                return !$scope.get_Quotedetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.spall = itmi;
            var toggleStatus = $scope.spall;
            angular.forEach($scope.get_Quotedetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //========================Supplier
        $scope.togchkbxSP = function () {
            $scope.spall = $scope.get_Quotedetails.every(function (itm) {
                return itm.spck;
            });
        };
        $scope.isOptionsRequiredSP = function () {
            if ($scope.optionflag === 'Supplier') {
                return !$scope.get_Quotedetails.some(function (options) {
                    return options.spck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkSP = function (itmi) {
            $scope.spall = itmi;
            var toggleStatus = $scope.spall;
            angular.forEach($scope.get_Quotedetails, function (itm) {
                itm.spck = toggleStatus;
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
                $scope.quoteArray = [];
                $scope.pIArray = [];
                $scope.itemArray = [];
                $scope.suplierArray = [];
                if ($scope.optionflag === "All") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "QuoteNo") {
                    angular.forEach($scope.get_Quotedetails, function (qt) {
                        if (qt.qtck === true) {
                            $scope.quoteArray.push(qt);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "quoteArray": $scope.quoteArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "PINo") {
                    angular.forEach($scope.get_Quotedetails, function (pi) {
                        if (pi.pick === true) {
                            $scope.pIArray.push(pi);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "pIArray": $scope.pIArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Item") {
                    angular.forEach($scope.get_Quotedetails, function (itm) {
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
                else if ($scope.optionflag === "Supplier") {
                    angular.forEach($scope.get_Quotedetails, function (sp) {
                        if (sp.spck === true) {
                            $scope.suplierArray.push(sp);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "suplierArray": $scope.suplierArray,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                apiService.create("INV_Quotation_Report/onreport", data).
                    then(function (promise) {
                        if (promise.get_Quotationreport.length > 0) {
                            var totalQamt = 0.00;
                            var totalNamt = 0.00;
                            $scope.get_Quotationreport = promise.get_Quotationreport;
                            $scope.presentCountgrid = $scope.get_Quotationreport.length;

                            angular.forEach($scope.get_Quotationreport, function (qt) {
                                totalQamt += parseFloat(qt.INVTSQ_QuotedRate);
                                $scope.totalQamt = totalQamt;
                                $scope.totalQamt = parseFloat($scope.totalQamt);
                                $scope.totalQamt = $scope.totalQamt.toFixed(2);

                                totalNamt += parseFloat(qt.INVTSQ_NegotiatedRate);
                                $scope.totalNamt = totalNamt;
                                $scope.totalNamt = parseFloat($scope.totalNamt);
                                $scope.totalNamt = $scope.totalNamt.toFixed(2);
                            });
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_Quotationreport = "";
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

            var innerContents = document.getElementById("printQt").innerHTML;
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