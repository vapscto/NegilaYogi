(function () {
    'use strict';
    angular
        .module('app')
        .controller('DueDateReportController', DueDateReportController)
    DueDateReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', 'exportUiGridService', 'uiGridConstants']
    function DueDateReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, exportUiGridService, uiGridConstants) {

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

        $scope.loaddata = function () {
            apiService.get("FeeDueDateReport/getinitialfeedata/").then(function (promise) {

                if (promise.yearList != null && promise.yearList.length > 0) {
                    $scope.yearlist = promise.yearList;                       
                }
            });
        }
       

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
          
        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }

        $scope.IsHiddenup = true;
        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }


        $scope.ShowReport = function () {
            if ($scope.myForm.$valid) {

                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();

                $scope.closeing_bal = 0;

                var data = {
                    "AMAY_Id": $scope.asmaY_Id,
                    "Fromdate": $scope.from_date
                    //"Todate": $scope.to_date,
                }
                apiService.create("FeeDueDateReport/getreport", data).then(function (promise) {
                    $scope.termflg = false;
                    $scope.stdtermflg = false;
                    if ((promise.incomereport != null && promise.incomereport.length > 0) || (promise.expensereport != null && promise.expensereport.length > 0)) {

                        $scope.incomereport = promise.incomereport;
                        $scope.income_cnt = 0;
                        angular.forEach($scope.incomereport, function (td) {                            
                            $scope.income_cnt += td.FYP_Tot_Amount;                            
                        })

                        $scope.expensereport = promise.expensereport;
                        $scope.expense_cnt = 0;
                        angular.forEach($scope.expensereport, function (td) {
                            $scope.expense_cnt += td.PCREQTNDET_Amount;
                        })

                        $scope.closeing_bal = $scope.income_cnt - $scope.expense_cnt;

                        $scope.Grid_view = true;
                        $scope.clscntinsconN = true;        
                    }                   
                    else {
                        swal("No Record Found");
                        $scope.Grid_view = false;
                        $scope.print_flag = true;
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        //$scope.optionToggledclscnt = function (SelectedStudentRecord, index) {
        //    $scope.daily = $scope.incomereport.every(function (itm) { return itm.dailycount; });
        //    if ($scope.printdatatabledaily.indexOf(SelectedStudentRecord) === -1) {
        //        $scope.printdatatabledaily.push(SelectedStudentRecord);
        //    }
        //    else {
        //        $scope.printdatatabledaily.splice($scope.printdatatabledaily.indexOf(SelectedStudentRecord), 1);
        //    }
        //    if ($scope.printdatatabledaily.length > 0) {
        //        $scope.showbutton = true;
        //    }
        //    else {
        //        $scope.showbutton = false;
        //    }
        //    $scope.get_total_class_print();
        //}


        //$scope.toggleAllclscnt = function () {

        //    var toggleStatus = $scope.daily;
        //    $scope.printdatatabledaily = [];
        //    angular.forEach($scope.incomereport, function (itm) {
        //        itm.dailycount = toggleStatus;
        //        if ($scope.daily == true) {
        //            $scope.printdatatabledaily.push(itm);
        //        }
        //    });
        //    if ($scope.printdatatabledaily.length > 0) {
        //        $scope.showbutton = true;
        //    }
        //    else {
        //        $scope.showbutton = false;
        //    }
        //    $scope.get_total_daily_print();
        //}
     

        //Export To Excel
        $scope.exportToExcel = function () {          

            if (($scope.incomereport !== null && $scope.incomereport.length > 0) || ($scope.expensereport !== null && $scope.expensereport.length > 0)) {
                var exportHref = Excel.tableToExcel(tablecldaily, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }
        }

        //Print
        $scope.printData = function () {
            if (($scope.incomereport !== null && $scope.incomereport.length > 0) || ($scope.expensereport !== null && $scope.expensereport.length > 0)) {
                var innerContents = document.getElementById("printdailycollectionreport").innerHTML;
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
