
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_PR_ReportController', INV_PR_ReportController);
    INV_PR_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_PR_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


        $scope.obj = {};
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
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
            }
            apiService.create("INV_PR_Report/getloaddata", data).
                then(function (promise) {
                    $scope.get_PRdetails = promise.get_PRdetails;
                })
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_PRdetails = "";
            $scope.get_PRreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);

        }
        //===================================PR Number Select
        $scope.togchkbxG = function () {
            $scope.prall = $scope.get_PRdetails.every(function (itm) {
                return itm.prck;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.get_PRdetails.some(function (options) {
                return options.prck;
            });
        }
        $scope.all_check = function (pral) {
            $scope.prall = pral;
            var toggleStatus = $scope.prall;
            angular.forEach($scope.get_PRdetails, function (pr) {
                pr.prck = toggleStatus;
            });
        }
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_PRdetails.every(function (itm) {
                return itm.itemck;
            });
        }
        $scope.isOptionsRequiredI = function () {
            if ($scope.individualflag == 'Item') {
                return !$scope.get_PRdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        }
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_PRdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        }

        //==================================PR Report
        $scope.submitted = false;
        $scope.onreport = function () {
            if ($scope.myForm.$valid) {
                $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                if ($scope.bw_dates == true) {
                    $scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                }
                else {
                    $scope.sDate = "";
                    $scope.eDate = "";
                }

                $scope.prArray = [];
                $scope.itemArray = [];
                if ($scope.optionflag == "All") {
                    var data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates,
                        "HRME_Id": 0
                    }
                }
                else if ($scope.optionflag == "PRno") {
                    angular.forEach($scope.get_PRdetails, function (pr) {
                        if (pr.prck == true) {
                            $scope.prArray.push(pr);
                        }
                    })
                    var data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "prArray": $scope.prArray,
                        "bwdateflag": $scope.bw_dates,
                        "HRME_Id": 0
                    }
                }
                else if ($scope.optionflag == "Item") {
                    angular.forEach($scope.get_PRdetails, function (itm) {
                        if (itm.itemck == true) {
                            $scope.itemArray.push(itm);
                        }
                    })
                    var data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "itemArray": $scope.itemArray,
                        "bwdateflag": $scope.bw_dates,
                        "HRME_Id": 0
                    }
                }
                else if ($scope.optionflag == "Requestedby") {
                    var data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "HRME_Id": $scope.obj.hrmE_Id.HRME_Id,
                        "bwdateflag": $scope.bw_dates
                    }
                }
                apiService.create("INV_PR_Report/onreport", data).
                    then(function (promise) {
                        if (promise.get_PRreport.length > 0) {
                            var totalamt = 0.00;
                            $scope.get_PRreport = promise.get_PRreport;
                            $scope.presentCountgrid = $scope.get_PRreport.length;
                            if ($scope.optionflag == "Requestedby") {
                                $scope.reqname = $scope.get_PRreport[0].requestedby;
                            }
                            angular.forEach($scope.get_PRreport, function (pr) {
                                totalamt += parseFloat(pr.INVTPR_ApproxAmount);
                                $scope.totalamt = totalamt;
                                $scope.totalamt = parseFloat($scope.totalamt);
                                $scope.totalamt = $scope.totalamt.toFixed(2);
                            })
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_PRreport = "";
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        }
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

            var innerContents = document.getElementById("printpr").innerHTML;
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