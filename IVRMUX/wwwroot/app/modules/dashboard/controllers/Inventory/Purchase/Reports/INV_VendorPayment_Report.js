
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_VendorPayment_ReportController', INV_VendorPayment_ReportController);
    INV_VendorPayment_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_VendorPayment_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


        $scope.obj = {};
        var paginationformasters;
        $scope.optionflag = "Supplier";
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
            apiService.create("INV_VendorPayment_Report/getloaddata", data).
                then(function (promise) {
                    $scope.get_VPReport_Details = promise.get_VPReport_Details;
                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_VPReport_Details = "";
            $scope.get_VPreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };

        //==================================PI Report
        $scope.submitted = false;
        $scope.onreport = function () {
            if ($scope.myForm.$valid) {
                var data = {};
                var supplier_id = 0;
                var grn_id = 0;
                if ($scope.optionflag === "Supplier") {
                    supplier_id = $scope.obj.invmS_Id.INVMS_Id;
                    grn_id = 0;
                }
                else if ($scope.optionflag === "GRN") {
                    grn_id = $scope.obj.invmgrN_Id.INVMGRN_Id;
                    supplier_id = 0;
                }
                data = {
                    "optionflag": $scope.optionflag,
                    "INVMS_Id": supplier_id,
                    "INVMGRN_Id": grn_id
                };
                apiService.create("INV_VendorPayment_Report/onreport", data).
                    then(function (promise) {
                        if (promise.get_VPreport.length > 0) {
                            $scope.get_VPreport = promise.get_VPreport;
                            $scope.presentCountgrid = $scope.get_VPreport.length;

                            angular.forEach($scope.get_VPreport, function (pi) {
                                totalamt += parseFloat(pi.INVTPI_ApproxAmount);
                                $scope.totalamt = totalamt;
                                $scope.totalamt = parseFloat($scope.totalamt);
                                $scope.totalamt = $scope.totalamt.toFixed(2);
                            });
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_VPreport = "";
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