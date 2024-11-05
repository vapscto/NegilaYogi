
(function () {
    'use strict';
    angular
        .module('app')
        .controller('SiteLocationsReportController', SiteLocationsReportController);
    SiteLocationsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function SiteLocationsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

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
        //-------------------------------------------------------------------
        $scope.obj = {};
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("SiteLocationsReport/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_sites = promise.get_sites;
                })
        };

        //==========================================================================
        $scope.togchkbx = function () {
            $scope.allcheck = $scope.get_sites.every(function (itm) {
                return itm.siteck;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.get_sites.some(function (options) {
                return options.siteck;
            });
        }
        $scope.all_check = function (ac) {
            $scope.allcheck = ac;
            var toggleStatus = $scope.allcheck;
            angular.forEach($scope.get_sites, function (site) {
                site.siteck = toggleStatus;
            });
        }

        $scope.getreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.sitearray = [];
                angular.forEach($scope.get_sites, function (st) {
                    if (st.siteck == true) {
                        $scope.sitearray.push(st);
                    }

                });
                var data = {
                    "sitearray": $scope.sitearray
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("SiteLocationsReport/getreport", data).then(function (promise) {
                    $scope.get_sitereport = promise.get_sitereport;
                    $scope.presentCountgrid = $scope.get_sitereport.length;
                    $scope.sitename = $scope.get_sitereport[0].invmsI_SiteBuildingName
                    $scope.rowspn = $scope.get_sitereport.length;
                    $scope.site_list = [];
                    angular.forEach($scope.get_sitereport, function (st) {
                        if ($scope.site_list.length == 0) {
                            $scope.site_list.push({ invmsI_Id: st.invmsI_Id, invmsI_SiteBuildingName: st.invmsI_SiteBuildingName });
                        }
                        else if ($scope.site_list.length > 0) {
                            var al_site_cnt = 0;
                            angular.forEach($scope.site_list, function (site) {
                                if (site.invmsI_Id == st.invmsI_Id) {
                                    al_site_cnt += 1;
                                }
                            })
                            if (al_site_cnt > 0) {
                                $scope.site_list.push({ invmsI_Id: st.invmsI_Id, invmsI_SiteBuildingName: st.invmsI_SiteBuildingName });
                            }
                        }
                    })
                    // $scope.get_sitereport.push($scope.site_list);
                })
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
        }

        //======================================Print & Export to Excel
        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'printSLLL');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printSL").innerHTML;
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