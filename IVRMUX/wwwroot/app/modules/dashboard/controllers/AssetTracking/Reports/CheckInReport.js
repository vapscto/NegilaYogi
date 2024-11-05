
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckInReportController', CheckInReportController);
    CheckInReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function CheckInReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout,$filter) {

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
            var pageid = 2;
            apiService.getURI("CheckInReport/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_Financialyear = promise.get_Financialyear;
                    $scope.get_store = promise.get_store;
                    $scope.get_items = promise.get_items;
                    $scope.get_locations = promise.get_locations;
                    $scope.academicyearlist = promise.academicyearlist;
                });
        };

        //==========================================================================
        //============================================== Item Selection
        $scope.item_click = function () {
            $scope.allitem = $scope.get_items.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isRequireditem = function () {
            if ($scope.selectionflag === "Item") {
                return !$scope.get_items.some(function (options) {
                    return options.itemck;
                });
            }
        };
        $scope.all_item = function (ac) {
            $scope.allitem = ac;
            var toggleStatus = $scope.allitem;
            angular.forEach($scope.get_items, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //============================================== Location Selection
        $scope.location_click = function () {
            $scope.alllocation = $scope.get_locations.every(function (lc) {
                return lc.locationck;
            });
        };
        $scope.isRequiredlocation = function () {
            if ($scope.selectionflag === "Location") {
                return !$scope.get_locations.some(function (options) {
                    return options.locationck;
                });
            }
        };
        $scope.all_location = function (lca) {
            $scope.alllocation = lca;
            var toggleStatus = $scope.alllocation;
            angular.forEach($scope.get_locations, function (loca) {
                loca.locationck = toggleStatus;
            });
        };

        $scope.getreport = function () {
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");

                if ($scope.datefilter === "BD") {
                    $scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                }
                else {
                    $scope.sDate = "";
                    $scope.eDate = "";
                }
                if ($scope.datefilter === "Year") {
                    $scope.financialyear = $scope.imfY_Id;
                    $scope.acyear = 0;
                }
                else {
                    $scope.financialyear = 0;
                }
                if ($scope.datefilter === "AYear") {
                    $scope.acyear = $scope.ASMAY_Id;
                    $scope.financialyear = 0;
                }
                else {
                    $scope.acyear = 0;
                }

                if ($scope.selectionflag === "All") {
                    data = {
                        "selectionflag": $scope.selectionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "IMFY_Id": $scope.financialyear,
                        "ASMAY_Id": $scope.acyear,
                        "itemarray": "",
                        "locationarray": ""
                    };
                }
                else if ($scope.selectionflag === "Item") {
                    $scope.itemarray = [];
                    angular.forEach($scope.get_items, function (itm) {
                        if (itm.itemck === true) {
                            $scope.itemarray.push(itm);
                        }
                    });

                    data = {
                        "selectionflag": $scope.selectionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "IMFY_Id": $scope.financialyear,
                        "ASMAY_Id": $scope.acyear,
                        "itemarray": $scope.itemarray,
                        "locationarray": ""
                    };
                }
                else if ($scope.selectionflag === "Location") {
                    $scope.locationarray = [];
                    angular.forEach($scope.get_locations, function (loc) {
                        if (loc.locationck === true) {
                            $scope.locationarray.push(loc);
                        }
                    });
                    data = {
                        "selectionflag": $scope.selectionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "IMFY_Id": $scope.financialyear,
                        "ASMAY_Id": $scope.acyear,
                        "itemarray": "",
                        "locationarray": $scope.locationarray
                    };
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("CheckInReport/getreport", data).then(function (promise) {
                    if (promise.get_checkInreport.length > 0) {
                        $scope.get_checkInreport = promise.get_checkInreport;
                        $scope.presentCountgrid = $scope.get_checkInreport.length;
                    }
                    else {
                        $scope.get_checkInreport = "";
                        swal("No Record Found....!!");
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

            var exportHref = Excel.tableToExcel(export_id, 'printcin');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
        $scope.printData = function () {
            var innerContents = document.getElementById("printcin").innerHTML;
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