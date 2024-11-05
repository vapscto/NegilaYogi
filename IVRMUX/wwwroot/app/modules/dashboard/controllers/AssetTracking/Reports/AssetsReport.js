
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetsReportController', AssetsReportController);
    AssetsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout'];
    function AssetsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {


        $scope.disposeQFlg = false;
        $scope.locationflg = false;
        $scope.selectionflag = "year";
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
        //-------------------------------------------------------------------
        $scope.obj = {};
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var data = {
                "selectionflag": $scope.selectionflag
            };  
            apiService.create("AssetsReport/getloaddata", data).
                then(function (promise) {
                    $scope.get_AssetsReportdetails = promise.get_AssetsReportdetails;
                });
        };
        $scope.onyearChange = function () {
            //$scope.get_locations = [];
            $scope.getreport();
        };

        $scope.onlocationChange = function () {
            $scope.locationarray = [];
            if ($scope.invmlO_Id > 0 || $scope.invmlO_Id !== undefined || $scope.invmlO_Id !== '') {
                $scope.locationarray.push({ INVMLO_Id: $scope.invmlO_Id });
            }
        };
        $scope.getreportClick = function () {
            $scope.selectionflag = "Location";
            $scope.getreport();
        };
        //   ========================================== Assets Report
        $scope.getreport = function () {
            $scope.submitted = true;
            $scope.get_AssetsReport = "";
            if ($scope.myForm.$valid) {
                var data = {};


                data = {
                    "selectionflag": $scope.selectionflag,
                    "itemarray": "",
                    //"IMFY_FinancialYear": $scope.coYear,
                    "IMFY_Id": $scope.coYear,
                    "locationarray": $scope.locationarray
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("AssetsReport/getreport", data).then(function (promise) {
                    if ($scope.selectionflag === "year") {
                        $scope.get_locations = promise.get_locations;                      
                    }
                    else {
                        if (promise.get_AssetsReport.length > 0) {
                            $scope.imfY_FinancialYear1 = promise.financial_year[0].imfY_FinancialYear;
                            $scope.get_AssetsReport = promise.get_AssetsReport;
                            $scope.get_locationDetails = promise.get_locationDetails;
                            $scope.location = $scope.get_locationDetails[0].INVMLO_LocationRoomName;
                            $scope.incharge = $scope.get_locationDetails[0].INVMLO_InchargeName;
                            //angular.forEach($scope.get_AssetsReport, function (stk) {
                            //    if ($scope.selectionflag === "Location") {
                            //        $scope.location = stk.INVMLO_LocationRoomName;
                            //        $scope.incharge = stk.INVMLO_InchargeName;
                            //    }
                            //});
                        }
                        else {
                            swal("No record found.... !");
                        }
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
            var exportHref = "";
            exportHref = Excel.tableToExcel(export_id, 'printARL');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printARL").innerHTML;
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