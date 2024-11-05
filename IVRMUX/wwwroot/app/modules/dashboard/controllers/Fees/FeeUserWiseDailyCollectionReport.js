(function () {
    'use strict';
    angular
        .module('app')
        .controller('DailyFeeCollReportController', DailyFeeCollReportController)
    DailyFeeCollReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', 'exportUiGridService', 'uiGridConstants']
    function DailyFeeCollReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, exportUiGridService, uiGridConstants) {

        $scope.searchString = "";
        $scope.file_disable = true;
        $scope.tadprint = false;
        $scope.usrname = localStorage.getItem('username');
        $scope.FMCB_fromDATE = new Date();
        $scope.FMCB_toDATE = new Date();
        $scope.checkallhrd1 = true;
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;

        $scope.hrdallcheck1 = function () {
            var toggleStatus1 = $scope.checkallhrd1;
            angular.forEach($scope.arrlistchkgroup, function (itm) {
                itm.selected = toggleStatus1;
            });
        }

        $scope.onselectyear = function () {

            var data = {
                "ASMAY_Id": $scope.ASMAY,
            }
            apiService.create("DailyFeeCollReport/getdata", data).
                then(function (promise) {

                    $scope.arrlistchkgroup = promise.fillfeegroup;

                    angular.forEach($scope.arrlistchkgroup, function (tr) {
                        tr.selected = true;
                    })
                    $scope.binddatagrp();
                })
        }

        $scope.loaddata = function () {
            $scope.acdmyr = 0;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            apiService.getURI("DailyFeeCollReport/getalldetails", pageid).
                then(function (promise) {
                    $scope.yearlst = promise.fillyear;
                    $scope.ASMAY = $scope.yearlst[0].asmaY_Id;

                    $scope.arrlistchkgroup = promise.fillfeegroup;

                    angular.forEach($scope.arrlistchkgroup, function (tr) {
                        tr.selected = true;
                    })
                    $scope.columnsTest = [];
                    $scope.sort = function (keyname) {
                        $scope.sortKey = keyname;   //set the sortKey to the param passed
                        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
                    }
                    //$scope.binddatagrp();
                    //$scope.fromdatechange();
                    //$scope.todatechange();


                })
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.isOptionsRequired = function () {
            return !$scope.arrlistchkgroup.some(function (options) {
                return options.selected;
            });
        }

        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }

        $scope.fromdatechange = function () {
            $scope.binddatagrp($scope.arrlistchkgroup);
        }
        $scope.todatechange = function () {
            $scope.binddatagrp($scope.arrlistchkgroup);
        }

        $scope.submitted = false;
        $scope.ShowReport = function () {

            if ($scope.myForm.$valid) {
                $scope.albumNameArraycolumn = [];
                angular.forEach($scope.arrlistchkgroup, function (role) {
                    if (!!role.selected) $scope.albumNameArraycolumn.push({
                        columnID: role.fmG_Id,
                        FMG_Id: role.fmG_Id,
                        columnName: role.fmG_GroupName
                    });
                })
                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();

                var data = {
                    "regornamedetails": $scope.result,
                    "AMAY_Id": $scope.ASMAY,
                    "Fromdate": $scope.from_date,
                    "Todate": $scope.to_date,
                    "All_List": $scope.albumNameArraycolumn,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("DailyFeeCollReport/UserWisereportdetails", data).
                    then(function (promise) {

                        if (promise.regornamedetails == "STW") {

                            if (promise.studentalldata != null && promise.studentalldata != "") {
                                $scope.Grid_view = true;
                                $scope.students = "";
                                $scope.students = promise.studentalldata;
                                $scope.totcountfirst = promise.studentalldata.length;
                                $scope.file_disable = true;
                                $scope.stu = true;
                                $scope.total = false;
                                $scope.IsHiddendown = true;
                                var total = 0;
                                angular.forEach($scope.students, function (detail) {
                                    total += detail.Amount;
                                })
                                $scope.totA_p = total;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.file_disable = true;
                            }
                        }
                        else if (promise.regornamedetails == "TCW") {

                            if (promise.totalcollection != null && promise.totalcollection != "") {
                                $scope.Grid_view = true;
                                $scope.totcollection = "";
                                $scope.totcollection = promise.totalcollection;
                                $scope.totcountfirst = promise.totalcollection.length;
                                $scope.stu = false;
                                $scope.total = true;
                                $scope.IsHiddendown = true;
                                var total = 0;
                                var receipt = 0;
                                angular.forEach($scope.totcollection, function (detail) {
                                    total += detail.totalAmount;
                                    receipt += detail.Receipt_No;
                                })
                                $scope.totA_p = total;
                                $scope.totA_receipt = receipt;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.file_disable = true;
                            }
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        //StudentWise All
        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.students1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
            });
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
        }

        //Total Collection All
        $scope.toggleAlltot = function () {
            $scope.printdatatabletot = [];
            var toggleStatus = $scope.ind;
            angular.forEach($scope.totcollection1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.ind == true) {
                    $scope.printdatatabletot.push(itm);
                }
            });
            if ($scope.printdatatabletot.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
        }

        $scope.optionToggledtot = function (SelectedStudentRecord, index) {

            $scope.ind = $scope.totcollection.every(function (itm) { return itm.selected; });
            if ($scope.printdatatabletot.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatabletot.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatabletot.splice($scope.printdatatabletot.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatabletot.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
        }

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

        //Export To Excel
        $scope.exportToExcel = function () {

            if ($scope.result == "STW") {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var exportHref = Excel.tableToExcel(student, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }

            }
            else if ($scope.result == "TCW") {
                if ($scope.printdatatabletot !== null && $scope.printdatatabletot.length > 0) {
                    var exportHref = Excel.tableToExcel(total, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
        }

        //Print Data
        $scope.printData = function () {
            if ($scope.result == "STW") {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var innerContents = document.getElementById("printSectionIdstu").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }
            else if ($scope.result == "TCW") {
                if ($scope.printdatatabletot !== null && $scope.printdatatabletot.length > 0) {
                    var innerContents = document.getElementById("printSectionIdtot").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }
        }
    }
})();

angular
    .module('app').factory('exportUiGridService', exportUiGridService);

exportUiGridService.inject = ['uiGridExporterService'];
function exportUiGridService(uiGridExporterService) {
    var service = {
        exportToExcel: exportToExcel
    };

    return service;

    function Workbook() {
        if (!(this instanceof Workbook)) return new Workbook();
        this.SheetNames = [];
        this.Sheets = {};
    }

    function exportToExcel(sheetName, gridApi, rowTypes, colTypes) {
        var columns = gridApi.grid.options.showHeader ? uiGridExporterService.getColumnHeaders(gridApi.grid, colTypes) : [];
        var data = uiGridExporterService.getData(gridApi.grid, rowTypes, colTypes);
        var fileName = gridApi.grid.options.exporterExcelFilename ? gridApi.grid.options.exporterExcelFilename : 'dailyfeecolreport';
        fileName += '.xlsx';
        var wb = new Workbook(),
            ws = sheetFromArrayUiGrid(data, columns);
        wb.SheetNames.push(sheetName);
        wb.Sheets[sheetName] = ws;
        var wbout = XLSX.write(wb, {
            bookType: 'xlsx',
            bookSST: true,
            type: 'binary'
        });
        saveAs(new Blob([s2ab(wbout)], {
            type: 'application/octet-stream'
        }), fileName);
    }

    function sheetFromArrayUiGrid(data, columns) {
        var ws = {};
        var range = {
            s: {
                c: 10000000,
                r: 10000000
            },
            e: {
                c: 0,
                r: 0
            }
        };
        var C = 0;
        columns.forEach(function (c) {
            var v = c.displayName || c.value || columns[i].name;
            addCell(range, v, 0, C, ws);
            C++;
        }, this);
        var R = 1;
        data.forEach(function (ds) {
            C = 0;
            ds.forEach(function (d) {
                var v = d.value;
                addCell(range, v, R, C, ws);
                C++;
            });
            R++;
        }, this);
        if (range.s.c < 10000000) ws['!ref'] = XLSX.utils.encode_range(range);
        return ws;
    }
    /**
     * 
     * @param {*} data 
     * @param {*} columns 
     */

    function datenum(v, date1904) {
        if (date1904) v += 1462;
        var epoch = Date.parse(v);
        return (epoch - new Date(Date.UTC(1899, 11, 30))) / (24 * 60 * 60 * 1000);
    }

    function s2ab(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    function addCell(range, value, row, col, ws) {
        if (range.s.r > row) range.s.r = row;
        if (range.s.c > col) range.s.c = col;
        if (range.e.r < row) range.e.r = row;
        if (range.e.c < col) range.e.c = col;
        var cell = {
            v: value
        };
        if (cell.v == null) cell.v = '';
        var cell_ref = XLSX.utils.encode_cell({
            c: col,
            r: row
        });

        if (typeof cell.v === 'number') cell.t = 'n';
        else if (typeof cell.v === 'boolean') cell.t = 'b';
        else if (cell.v instanceof Date) {
            cell.t = 'n';
            cell.z = XLSX.SSF._table[14];
            cell.v = datenum(cell.v);
        } else cell.t = 's';

        ws[cell_ref] = cell;
    }
}
