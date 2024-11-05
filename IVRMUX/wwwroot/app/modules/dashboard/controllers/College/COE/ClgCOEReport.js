
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgCOEReportController', ClgCOEReportController)

    ClgCOEReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', 'Excel', '$timeout', '$stateParams', '$filter', '$sce']
    function ClgCOEReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, Excel, $timeout, $stateParams, $filter, $sce) {

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.printsection = false;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.grid_view = false;
        //$scope.gridOptions = {

        //    enableColumnMenus: false,
        //    enableFiltering: true,
        //    paginationPageSizes: [5, 10, 15],
        //    paginationPageSize: 5,

        //    columnDefs: [
        //        { name: 'SNO', displayName: 'SL NO', width: '7%', enableFiltering: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
        //        { name: 'eventName', width: '15%', displayName: 'Event Name' },
        //        { name: 'eventDesc', width: '50%', displayName: 'Event Description' },
        //        { name: 'coeE_EStartDate', width: '14%', displayName: 'Event Start Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
        //        { name: 'coeE_EEndDate', width: '14%', displayName: 'Event End Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },],
        //    onRegisterApi: function (gridApi) {
        //        $scope.gridApi = gridApi;
        //    },
        //};
        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.printToCart = function () {

            var innerContents = document.getElementById("printrcp").outerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        }
        $scope.labeldisable = true;

        $scope.onradiobuttonSelect = function (type) {
            $scope.loaddata();
            $scope.labeldisable = false;
        }

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var id = 2;
            apiService.getURI("ClgCOEReport/", id).then(function (promise) {

                $scope.year = promise.fillyear;
                $scope.course_rp = promise.course_rp;
            });
        }

        $scope.clear = function () {
            $state.reload();
        }
        //===================================Select All 
        $scope.togchkbxC = function () {
            $scope.itemall = $scope.course_rp.every(function (itm) {
                return itm.cors;
            });
        }
        $scope.isOptionsRequiredC = function () {
            return !$scope.course_rp.some(function (options) {
                return options.cors;
            });
        }
        $scope.all_checkC = function (itemal) {
            $scope.itemall = itemal;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.course_rp, function (item) {
                item.cors = toggleStatus;
            });
        }
        //===================================Get Report
        $scope.getReport = function () {
            if ($scope.myForm.$valid) {
                $scope.arraycourse = [];
                if ($scope.all1 == "C") {
                    angular.forEach($scope.course_rp, function (cr) {
                        if (cr.cors == true) {
                            $scope.arraycourse.push(cr);
                        }
                    })
                    var data = {
                        "typeflag": $scope.all1,
                        "ASMAY_Id": $scope.Year,
                        "monthid": $scope.month,
                        "AMCO_Ids": $scope.arraycourse,
                    }
                }
                else {
                    var data = {
                        "typeflag": $scope.all1,
                        "ASMAY_Id": $scope.Year,
                        "monthid": $scope.month,
                        "AMCO_Ids": "",
                    }
                }
                apiService.create("ClgCOEReport", data).then(function (promise) {

                    if (promise.coereport.length > 0) {
                        $scope.coereport_grd = promise.coereport;
                        $scope.presentCountgrid = $scope.coereport_grd.length;
                        $scope.imgname = $scope.coereport_grd[0].logo;
                    }
                    else {
                        swal("No Record Found");
                        $scope.coereport_grd = "";
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.isOptionsRequiredS = function () {
            return !$scope.course_rp.some(function (options) {
                return options.cors;
            });
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();