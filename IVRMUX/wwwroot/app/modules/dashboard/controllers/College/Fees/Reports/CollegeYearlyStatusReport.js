
(function () {
    'use strict';
    angular
.module('app')
        .controller('CollegeYearlyStatusReportController', CollegeYearlyStatusReportController)

    CollegeYearlyStatusReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$interval', 'uiGridGroupingConstants', 'exportUiGridService', 'Excel','$timeout']
    function CollegeYearlyStatusReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $interval, uiGridGroupingConstants, exportUiGridService, Excel, $timeout) {



        $scope.show_btn = false;
        $scope.show_cancel = false;
        $scope.show_grid = false;
        $scope.searc_button = true;
        $scope.tabledata = false;
        // $scope.sortReverse = true;


        var paginationformasters;
        $scope.page1 = "page1";
        $scope.page2 = "page2";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else if (ivrmcofigsettings.length == 0 || ivrmcofigsettings == undefined || ivrmcofigsettings == null) {
            paginationformasters = 3;
        }

        paginationformasters = 3;
        //=========For filter char count for first table===============//
        $scope.searchValue = '';
        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }
        //====================End================================//

        //=========For filter char count for Second table===============//
        $scope.searchValue1 = "";
        $scope.search_box1 = function () {
            if ($scope.searchValue1 != "" || $scope.searchValue1 != null) {
                $scope.searc_button1 = false;
            }
            else {
                $scope.searc_button1 = true;
            }
        }
        //====================End================================//

        //=======If any Semester checkboxlist select then SHOW button Display Other wise Not=======//
        $scope.clar_sem = function () {
            
            $scope.chk_array = [];
            var chk_count = 0;
            angular.forEach($scope.semesterlist, function (itm) {
                if (itm.selected1 == true) {
                    chk_count += 1;
                    $scope.chk_array.push(itm);
                }
                if ($scope.chk_array.length > 0) {
                    $scope.show_btn = true;
                    $scope.show_cancel = true;

                }
                else {
                    $scope.show_btn = false;
                    $scope.show_cancel = false;

                    $scope.show_grid = false;
                }
            });
        }
        //====================End================================//


        // $scope.sortKey = "acysT_RollNo";    //set the sortKey to the param passed
        // $scope.reverse = true;      //if true make it false and vice versa
        $scope.search = "";
        $scope.show_flag = false;


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.sortReverse = !$scope.sortReverse; //if true make it false and vice versa
        }
        $scope.studentlist = [];


        //============Start Data Load on the Page==============//
        $scope.loaddata = function () {

            
            var pageid = 1;
            apiService.getURI("CollegeYearlyStatusReport/GetYearList", pageid).
                then(function (promise) {
                    
                    $scope.yearlist = promise.yearlist;

                    // $scope.show_flag = false;
                })
        }
        //====================End===================//

        //===========Load Student data in to the Table(grid)=============//
        $scope.msg = '';
        $scope.get_group = function () {
            $scope.studentlist = [];
            $scope.grouplst = [];
            $scope.headlst = [];
            $scope.show_flag = false;
            $scope.submitted = true;
            $scope.currentPage = 1;
            $scope.currentPage2 = 1;
            $scope.msg = '';
            $scope.itemsPerPage = paginationformasters;

            //$scope.studentlist = [];

            if ($scope.myForm.$valid) {
               
                var data = {
                    
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("CollegeYearlyStatusReport/get_group", data).then(function (promise) {

                   
                    $scope.studentlist = promise.studentlist;
                    $scope.grouplst = promise.fillmastergroup;
                    $scope.headlst = promise.fillmasterhead;
                   // $scope.installlst = promise.fillinstallment;

                  //  $scope.grouplstedit = promise.fillmastergroup;
                   // $scope.headlstedit = promise.fillmasterhead;
                    //$scope.installlstedit = promise.fillinstallment;

                    if ($scope.grouplst.length == 0) {
                        $scope.msg="No Groups"
                    }

                    console.log($scope.grouplst)
                    console.log($scope.headlst)
                    $scope.show_flag = true;
                 
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //======================End===========================//

       
        $scope.gridOptions = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: true,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'AMCO_CourseName', displayName: 'Course Name', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'AMB_BranchName', displayName: 'Branch Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>'  },
             
                { name: 'AMSE_SEMName', displayName: 'Sem Name', grouping: { groupPriority: 2 },  width: '20%' },
                {
                    name: 'AStudentCount', displayName: 'Student count', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }},
                {
                    name: 'ACollection', displayName: 'Collection', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    } },
                {
                    name: 'ACollectionAnyTime', displayName: 'collection anytime', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    } },
                {
                    name: 'ACollegeDue', displayName: 'CollegeDue', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    } },
                {
                    name: 'AFCSS_AdjustedAmount', displayName: 'AdjustedAmount', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }},
                {
                    name: 'AFCSS_ConcessionAmount', displayName: 'ConcessionAmount', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    } },
                {
                    name: 'AFCSS_CurrentYrCharges', displayName: 'CurrentYrCharges', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    } },
                {
                    name: 'AFCSS_TotalCharges', displayName: 'TotalCharges', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }},
                {
                    name: 'AFCSS_WaivedAmount', displayName: 'WaivedAmount', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }},
                {
                    name: 'AReceivable', displayName: 'Receivable', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }},
                {
                    name: 'AStudentCount', displayName: 'StudentCount', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }},
                {
                    name: 'AStudentDue', displayName: 'StudentDue', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }}
               
            ],
            exporterMenuPdf: false,
           
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'class year status report',
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
            ]
        };

        $scope.exportExcel = function (tablename) {
            var exportHref = Excel.tableToExcel(tablename, 'sheet name');

            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);

            //var grid = $scope.gridApi.grid;
            //var rowTypes = exportUiGridService.ALL;
            //var colTypes = exportUiGridService.ALL;
           // exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
        };
    



      




        //============================Save For GroupList Data=========================//
        $scope.valsgroup = [];
        $scope.valshead = [];
        $scope.valsinstallment = [];
        $scope.valstudentlst = [];
        //$scope.studentdata = promise.studentlist
        $scope.savedata = function (grouplst, headlst) {
          
            $scope.valsgroup = [];
            $scope.valshead = [];
            $scope.valsinstallment = [];
            $scope.valstudentlst = [];
            $scope.page1 = "page1";
            
            if ($scope.myForm.$valid) {
                
               
                    
                        for (var t = 0; t < grouplst.length; t++) {
                            if (grouplst[t].checkedgrplst == true) {
                                $scope.valsgroup.push(grouplst[t]);
                            }
                        }

                        for (var u = 0; u < headlst.length; u++) {
                            if (headlst[u].checkedheadlst == true) {
                                $scope.valshead.push(headlst[u]);
                            }
                        }

                        //for (var v = 0; v < installlst.length; v++) {
                        //    if (installlst[v].checkedinstallmentlst == true) {
                        //        $scope.valsinstallment.push(installlst[v]);
                        //    }
                        //}
                 
                    //for (var w = 0; w < studentlist.length; w++) {
                    //    if (studentlist[w].studchecked == true) {
                    //        $scope.valstudentlst.push(studentlist[w]);
                    //    }
                    //}

                  
                    grouplst = $scope.valsgroup;
                    headlst = $scope.valshead;
                   // installlst = $scope.valsinstallment;
                    if ($scope.valsgroup.length > 0)
                    {
                        var data = {
                            "ASMAY_Id": $scope.asmaY_Id,
                           // studentdata: studentlist,
                            savegrplst: grouplst,
                            saveheadlst: headlst,
                           // saveftilst: installlst,
                        }

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }

                        apiService.create("CollegeYearlyStatusReport/savedata", data).
                            then(function (promise) {
                            

                                $scope.show_grid = true;
                                $scope.feedetails = promise.feedetails;
                                $scope.StudentReport = promise.feedetails;
                                console.log($scope.feedetails);
                                $scope.gridOptions.data = $scope.feedetails;
                                //if (promise.feedetails != null) {
                                //    $scope.feedetails = promise.feedetails;
                                //    console.log($scope.feedetails)
                                //} else {
                                //    swal('No Record Found')
                                //}
                           

                        })
                    }
                    else {
                        swal("Select Atleast One Group!!!");
                    }
                
                
            }
            else {
                $scope.submitted = true;
            }
        };
        //======================End=======================//



        //===================Cancel========================//
        $scope.cancel = function () {
            $state.reload();
        }
        //===================End========================//


        //===========Field Validation=================//
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        //==================End===========================//


        //========Branchlist CheckBox Field Validation===========//
        $scope.isOptionsRequired_1 = function () {
            return !$scope.branchlist.some(function (options) {
                return options.selected;
            });
        }
        //==================End===========================//


        //========courselist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.courselist.some(function (options) {
                return options.selected;
            });
        }
        //==================End===========================//


        //========semesterlist CheckBox Field Validation============//
        $scope.isOptionsRequired_2 = function () {
            return !$scope.semesterlist.some(function (options) {
                return options.selected1;

            });
        }
        //==================End===========================//

        $scope.allgroupcheck = function () {
            if ($scope.allcheck == true) {
                angular.forEach($scope.grouplst, function (obj) {
                    obj.checkedgrplst = true;
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = true;
                                //angular.forEach($scope.installlst, function (obj2) {
                                //    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                //        obj2.checkedinstallmentlst = true;
                                //    }
                                //});
                            }
                        });
                   
                });
            }
            else {
                angular.forEach($scope.grouplst, function (obj) {
                    obj.checkedgrplst = false;
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = false;
                                //angular.forEach($scope.installlst, function (obj2) {
                                //    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                //        obj2.checkedinstallmentlst = false;
                                //    }
                                //});
                            }
                        });
                    
                });
            }

        }


        //==========Select for selected Grouplst,headlst,installment data for store record(for save button)================//
        $scope.firstfnc = function (vlobj) {
            
            if (vlobj.checkedgrplst == true) {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = true;
                                angular.forEach($scope.installlst, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst = true;
                                    }
                                });
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = false;
                                angular.forEach($scope.installlst, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst = false;
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        $scope.secfnc = function (vlobj1) {
            
            //if (vlobj1.checkedheadlst == true) {
            //    angular.forEach($scope.headlst, function (val) {
            //        if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
            //            angular.forEach($scope.installlst, function (val1) {
            //                if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
            //                    val1.checkedinstallmentlst = true;
            //                }
            //            });
            //        }
            //    });
            //} else {
                
            //    angular.forEach($scope.headlst, function (val) {
            //        if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
            //            angular.forEach($scope.installlst, function (val1) {
            //                if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
            //                    val1.checkedinstallmentlst = false;
            //                }
            //            });
            //        }
            //    });
            //}
            
            for (var s = 0; s < $scope.grouplst.length; s++) {
                if (vlobj1.fmG_Id == $scope.grouplst[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlst.length; t++) {
                        if (vlobj1.fmG_Id == $scope.headlst[t].fmG_Id) {
                            if ($scope.headlst[t].checkedheadlst == false) {
                                $scope.grouplst[s].checkedgrplst = false;
                            }
                            else {
                                $scope.grouplst[s].checkedgrplst = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        $scope.trdfnc = function (vlobj2, oobjj) {
            for (var u = 0; u < $scope.headlst.length; u++) {
                if (vlobj2.fmG_Id == $scope.headlst[u].fmG_Id && vlobj2.fmH_Id == $scope.headlst[u].fmH_Id) {
                    for (var v = 0; v < $scope.installlst.length; v++) {
                        if ($scope.installlst[v].fmH_Id == $scope.headlst[u].fmH_Id && $scope.installlst[v].fmG_Id == $scope.headlst[u].fmG_Id) {
                            if ($scope.installlst[v].checkedinstallmentlst == false) {
                                $scope.headlst[u].checkedheadlst = false;
                            }
                            else {
                                $scope.headlst[u].checkedheadlst = true;
                                break;
                            }
                        }
                    }
                    
                    for (var w = 0; w < $scope.grouplst.length; w++) {
                        if (vlobj2.fmG_Id == $scope.grouplst[w].fmG_Id) {
                            for (var x = 0; x < $scope.headlst.length; x++) {
                                if (vlobj2.fmG_Id == $scope.headlst[x].fmG_Id) {
                                    if ($scope.headlst[x].checkedheadlst == false) {
                                        $scope.grouplst[w].checkedgrplst = false;
                                    }
                                    else {
                                        $scope.grouplst[w].checkedgrplst = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //==================End===========================//


     


        //==============This is used for Field empty and close popup=====================//
        $scope.cllose = function () {
            
            angular.forEach($scope.grouplstedit, function (objedit) {
                objedit.checkedgrplstedit = false;
            });
            angular.forEach($scope.headlstedit, function (objedit1) {
                objedit1.checkedheadlstedit = false;
            });
            angular.forEach($scope.installlstedit, function (objedit2) {
                objedit2.checkedinstallmentlstedit = false;
            });
            angular.forEach($scope.studentlist, function (obj) {
                if (obj.amcsT_Id == $scope.AMCST_idedit) {
                    obj.studchecked = false;
                }
            });
            $scope.AMCST_idedit = 0;
           // $('#editmodal').modal('hide');
        }
        //==================End===========================//
        $scope.isOptionsRequired1 = function () {
            if ($scope.AMCST_idedit > 0) {
                return false;
            }
            else {
                return !$scope.grouplst.some(function (options) {
                    return options.checkedgrplst;
                });
            }
            
        }
        $scope.isOptionsRequirededit1 = function () {
            if ($scope.AMCST_idedit > 0)
            {
            return !$scope.grouplstedit.some(function (options) {
                    return options.checkedgrplstedit;
                });
            }
            else {
                return false;
            }
        }
    }



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
            var fileName = gridApi.grid.options.exporterExcelFilename ? gridApi.grid.options.exporterExcelFilename : 'YearlyStatusReport';
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
            if (cell.v == null) cell.v = '-';
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
})();

