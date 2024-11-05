(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGTrCountReportController', CLGTrCountReportController)

    CLGTrCountReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout', 'Excel', 'uiGridGroupingConstants', 'exportUiGridService']
    function CLGTrCountReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel, uiGridGroupingConstants, exportUiGridService) {

        $scope.submitted = false;
        $scope.tablediv = false;
        $scope.printd = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        var copty;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.Binddata = function () {
            debugger;
          
            var pageid = 2;
            apiService.getURI("LibTransactionReport/Getdetails", pageid).then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                $scope.msterliblist1 = promise.msterliblist1;


                })

        }

        $scope.statuscount = false;
        $scope.alldata = [];
       //---------------Get-Report


        $scope.gridOptions = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: true,
            enableGridMenu: true,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'AMCO_CourseName', displayName: 'Course Name', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, width: '24%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'AMB_BranchName', displayName: 'Branch Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'AMSE_SEMName', displayName: 'Semester', grouping: { groupPriority: 2 }, sort: { priority: 2, direction: 'asc' }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'ACMS_SectionName', displayName: 'Section', grouping: { groupPriority: 3 }, sort: { priority: 0, direction: 'asc' }, width: '10%' },
                {
                    name: 'STDCOUNT', displayName: 'Count', width: '15%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
            ],
            exporterMenuPdf: false,
            exporterMenuCsv: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'EXPORT ALL DATA',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ],
            getFooterValue: function () {
                return $scope.gridApi.grid.columns[4].getAggregationValue();
            }
        };

     
        $scope.get_report = function () {
            $scope.alldata = [];
            $scope.finaltotal = 0;
            if ($scope.myForm.$valid) {
                debugger;
                var fromdate1 = $scope.Issue_Datefrm == null ? "" : $filter('date')($scope.Issue_Datefrm, "yyyy-MM-dd");
                var todate1 = $scope.IssueToDateto == null ? "" : $filter('date')($scope.IssueToDateto, "yyyy-MM-dd");
                var data = {
                    "statuscount": $scope.statuscount,
                    "Issue_Date": fromdate1,
                    "IssueToDate": todate1,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "LMAL_Id": $scope.LMAL_Id,
                }

                apiService.create("LibTransactionReport/CLGget_report", data).then(function (promise) {
                    if (promise.alldata.length > 0) {
                        $scope.alldata = promise.alldata;


                        $scope.gridOptions.data = $scope.alldata;

                        angular.forEach($scope.alldata, function (dd) {

                            $scope.finaltotal += dd.seccount;
                        })





                        //if ($scope.statuscount == false) {
                        //    $scope.classlist = promise.classlist;
                           

                        //    $scope.subexam_list = [];
                        //    angular.forEach($scope.alldata, function (sme) {
                        //        if ($scope.subexam_list.length == 0) {
                        //            $scope.subexam_list.push({ ASMCL_Id: sme.ASMCL_Id, ASMCL_ClassName: sme.ASMCL_ClassName });
                        //        }
                        //        else if ($scope.subexam_list.length > 0) {
                        //            var sub_exm_cnt = 0;
                        //            angular.forEach($scope.subexam_list, function (exm) {
                        //                if (exm.ASMCL_Id == sme.ASMCL_Id ) {
                        //                    sub_exm_cnt += 1;
                        //                }
                        //            })
                        //            if (sub_exm_cnt == 0) {
                        //                $scope.subexam_list.push({ ASMCL_Id: sme.ASMCL_Id, ASMCL_ClassName: sme.ASMCL_ClassName  });
                        //            }
                        //        }

                        //    })
                        //    console.log($scope.subexam_list);


                        //    $scope.newarray = [];
                        //    angular.forEach($scope.subexam_list, function (ff) {
                        //        var clscount = 0;
                        //        angular.forEach($scope.alldata, function (gg) {
                                  
                        //            if (ff.ASMCL_Id == gg.ASMCL_Id) {
                        //                clscount += gg.seccount;
                        //                $scope.newarray.push({ ASMCL_Id: gg.ASMCL_Id, ASMCL_ClassName: gg.ASMCL_ClassName, ASMS_Id: gg.ASMS_Id, ASMC_SectionName: gg.ASMC_SectionName, seccount: gg.seccount})
                                       
                        //            }


                        //        })

                        //        $scope.newarray.push({ ASMCL_Id: ff.ASMCL_Id, ASMCL_ClassName:'TOTAL', ASMS_Id: '', ASMC_SectionName: 'TOTAL', seccount: clscount })
                        //    })
                        //}
                     
                       
                    }
                    else {
                        swal('Record Not Available!!!');
                        $state.reload();
                    }
                })

            }
            else {
                $scope.submitted = true;
            }

        }
        //-------------End--Get-Report



        //===========print===========//
        $scope.printData = function () {
            var innerContents = '';

            if ($scope.statuscount == true) {
                innerContents = document.getElementById("printareaId22").innerHTML;
            } else {
                innerContents = document.getElementById("printtable").innerHTML;
            }


            //var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }

        $scope.exportToExcel = function () {

            var tbl = '';

            if ($scope.statuscount == true) {
                tbl = '#table1111';
            } else {
                tbl = '#table11';
            }

            var exportHref = Excel.tableToExcel(tbl, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 900);
        }
        //==============End==============//

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //-----------clear-field
        $scope.Clearid = function () {
            $state.reload();
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
        var fileName = gridApi.grid.options.exporterExcelFilename ? gridApi.grid.options.exporterExcelFilename : 'TransactionCountReport';
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