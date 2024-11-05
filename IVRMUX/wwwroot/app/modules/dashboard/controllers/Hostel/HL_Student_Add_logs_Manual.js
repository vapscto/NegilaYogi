(function () {
    'use strict';
    angular
        .module('app')
        .controller('HLStudentlogsManualController', HLStudentlogsManualController)

    HLStudentlogsManualController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'exportUiGridService']
    function HLStudentlogsManualController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, exportUiGridService) {
        $scope.HLHSTBIO_PunchDate = new Date();        
        $scope.HLHSTBIOD_Id = 0;
        $scope.HLHSTBIO_Id = 0;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.Obj = {};

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        $scope.ismeridian = false;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
       

        //validation start
        $scope.valstrtm = function (timedata) {
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setMinutes(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.HLHSTBIOD_PunchTime = "";

        }
        $scope.CheckedClassName = function (data) {
            if ($scope.checkboxchcked.indexOf(data) === -1) {
                $scope.checkboxchcked.push(data);
            }
            else {
                $scope.checkboxchcked.splice($scope.checkboxchcked.indexOf(data), 1);
            }
        }
        $scope.validatemax = function (maxdata) {     
            if (maxdata >= new Date($scope.HLHSTBIOD_PunchTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.HLHSTBIOD_PunchTime = "";
            }

        }       
       
      
       
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("Hostel_Student_InOutReport/getalldetails", pageid).then(
                function (promise) {                  
                    $scope.employee = promise.studentlist1;                   

                })
        }
      

        //Employee
        $scope.all_check_empl = function (empl) {

            var toggleStatus4 = empl;
            angular.forEach($scope.employee, function (itm) {
                itm.emple = toggleStatus4;
            });
        }
        $scope.addColumn4 = function () {
            $scope.empl = $scope.employee.every(function (itm) { return itm.emple; });
        }; 

        $scope.interacted = function (field) {

            return $scope.submitted;
        };
         
     
       

        $scope.Onshiftname = function () {
            $scope.employeelist = [];
            if ($scope.Obj.amcsT_Id.amcsT_Id > 0) {
                var data = {
                    AMCST_Id: $scope.Obj.amcsT_Id.amcsT_Id,
                    empdate: $scope.HLHSTBIO_PunchDate,
                }
                apiService.create("Hostel_Student_InOutReport/empname", data).then(
                    function (promise) {    
                            $scope.showdepartment1 = true;
                        if (promise.employeelist != null && promise.employeelist.length > 0) {
                            $scope.employeelist = promise.employeelist;
                            $scope.HLHSTBIO_Id = promise.employeelist[0].hlhstbiO_Id;
                            $scope.HLHSTBIOD_Id = promise.employeelist[0].hlhstbioD_Id;
                            angular.forEach($scope.employeelist, function (dd) {
                                dd.hlhstbioD_PunchTime = moment(dd.hlhstbioD_PunchTime, 'HH:mm').format()
                                $scope.timedis = true;
                            });
                        }
                        else {
                            swal('Add In and Out Records');
                            $scope.showdepartment1 = true;
                        }

                    })
            } 
            
        }

        $scope.clearid = function () {

            $state.reload();

        }
        $scope.deletedata = function (employee, SweetAlert) {

            var data = {
               "HLHSTBIOD_Id" : employee.hlhstbioD_Id,
                "HLHSTBIO_Id": employee.hlhstbiO_Id,
                "MI_Id": employee.MI_Id,
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Hostel_Student_InOutReport/deletedetails", data).
                            then(function (promise) {

                                if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                    $state.reload();
                                }
                                else {
                                    swal('Failed to Delete, please contact administrator.');
                                }
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }                

           $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "HLHSTBIO_Id": $scope.HLHSTBIO_Id,
                    "HLHSTBIOD_Id": $scope.HLHSTBIOD_Id,
                    "HLHSTBIOD_InOutFlg": $scope.inout,
                    "HLHSTBIOD_PunchTime": $filter('date')($scope.timpic, "HH:mm  "),                  
                    "AMCST_Id": $scope.Obj.amcsT_Id.amcsT_Id,
                    "MI_Id": $scope.Obj.amcsT_Id.mI_Id,
                    "HLHSTBIO_PunchDate": $filter('date')($scope.HLHSTBIO_PunchDate, "HH:mm")
                }

                apiService.create("Hostel_Student_InOutReport/savedetail", data).
                    then(function (promise) {

                        if (promise.returnval == true) {
                            if (promise.hlhstbiO_Id == 0 || promise.hlhstbiO_Id < 0) {
                                swal('Record saved successfully');
                                $state.reload();
                            }
                            else if (promise.hlhstbiO_Id > 0) {
                                swal('Record updated successfully');
                                $state.reload();
                            }
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Record already exist');
                            $state.reload();
                        }
                        else {
                            if (promise.hlhstbiO_Id == 0 || promise.hlhstbiO_Id < 0) {
                                swal('Failed to save, please contact administrator');
                                $state.reload();
                            }
                            else if (promise.hlhstbiO_Id > 0) {
                                swal('Failed to update, please contact administrator');
                                $state.reload();
                            }
                        }
                        $scope.Onshiftname($scope.Obj.amcsT_Id.amcsT_Id, $scope.HLHSTBIO_PunchDate);
                    })
            }
            else {
               
            }

        };

        $scope.printData = function () {


            var divToPrint = document.getElementById("grd");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
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
            var fileName = gridApi.grid.options.exporterExcelFilename ? gridApi.grid.options.exporterExcelFilename : 'employeshiftmapping';
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

