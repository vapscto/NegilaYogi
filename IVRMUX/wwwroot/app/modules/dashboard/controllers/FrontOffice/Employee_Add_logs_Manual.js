(function () {
    'use strict';
    angular
        .module('app')
        .controller('Employee_Add_logs_Manual', Employee_Add_logs_Manual)

    Employee_Add_logs_Manual.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'exportUiGridService']
    function Employee_Add_logs_Manual($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, exportUiGridService) {

        $scope.FOEST_Id = 0;
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
        $scope.morning = false;
        $scope.lunch = false;
        $scope.all = false;
        $scope.showdepartment = false;
        $scope.showdesignation = false;
        $scope.showdesignation1 = false;

        $scope.holi = {};

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
            $scope.FOMST_IIHalfLogoutTime = "";

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

            // $scope.FOMST_IHalfLoginTime = maxdata;
            //$scope.FOMST_IIHalfLogoutTime = "";
            if (maxdata >= new Date($scope.fomsT_IHalfLoginTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.FOMST_IIHalfLogoutTime = "";
            }

            // $scope.FOMST_IHalfLogoutTime = "";
        }
        $scope.validateTomintime1 = function (timedata1) {

            //  $scope.FOMST_IIHalfLoginTime = "";
            $scope.totime1 = timedata1;
            var hh = $scope.totime1.getHours();
            var mm = $scope.totime1.getMinutes();
            $scope.min1 = timedata1;

            $scope.min1.setMinutes(hh);
            $scope.min1.setMinutes(mm);
            $scope.FOMST_IIHalfLoginTime = timedata1;
        }
        $scope.validatefromtime1 = function (maxdata1) {
            // $scope.FOMST_IHalfLoginTime = maxdata;
            //$scope.FOMST_IIHalfLogoutTime = "";
            if (maxdata1 >= new Date($scope.FOMST_IHalfLogoutTime)) {
                // $scope.totimemax1 = maxdata1;
                //var hh1 = $scope.totimemax1.getHours();
                //var mm1 = $scope.totimemax1.getMinutes();
                //$scope.max1 = maxdata1;

                //$scope.max1.setMinutes(hh);
                //$scope.max1.setMinutes(mm);
                $scope.FOMST_IIHalfLoginTime = new Date($scope.FOMST_IHalfLogoutTime);
            }
            else {
                $scope.FOMST_IIHalfLoginTime = "";
            }

            // $scope.FOMST_IHalfLogoutTime = "";
        }
        //validation end
        $scope.FOEST_Date = new Date();
        $scope.fromDate = $scope.FOEST_Date.getFullYear();
        $scope.frommon = $scope.FOEST_Date.getMonth();
        $scope.fromDay = $scope.FOEST_Date.getDate();
        $scope.minDatef = new Date(
            $scope.fromDate,
            $scope.frommon,
            $scope.fromDay);

        //$scope.maxDatef = new Date(
        //      $scope.fromDate,
        //       $scope.frommon,
        //        $scope.fromDay + 365);

        //Ui Grid view rendering data from data base

        $scope.gridOptions = {
            enableColumnMenus: true,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            enableGridMenu: false,


            columnDefs: [
                { name: 'SlNo', width: 100, field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'fomS_ShiftName', width: 150, displayName: 'Shift Name' },
                { name: 'hrmE_EmployeeFirstName', width: 150, displayName: 'Employee' },
                { name: 'fohtwD_HolidayWDType', width: 150, displayName: 'Holiday Type' },
                { name: 'foesT_FDWHrMin', width: 150, displayName: 'Total Work Hour' },
                { name: 'foesT_HDWHrMin', width: 150, displayName: 'Half Day Work Hour' },
                { name: 'foesT_IHalfLoginTime', width: 150, displayName: 'Morning Login' },
                { name: 'foesT_IHalfLogoutTime', width: 150, displayName: 'Morning Logout' },
                { name: 'foesT_IIHalfLoginTime', width: 150, displayName: 'Afternoon Login' },
                { name: 'foesT_IIHalfLogoutTime', width: 150, displayName: 'Afternoon Logout' },
                { name: 'foesT_DelayPerShiftHrMin', width: 150, displayName: 'Delay Shift' },
                { name: 'foesT_EarlyPerShiftHrMin', width: 150, displayName: 'Early Shift' },
                { name: 'foesT_LunchHoursDuration', width: 150, displayName: 'Lunch Hour' },
                {
                    field: 'id', name: '', width: 120,
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);" > <i class="fa fa-pencil-square-o" ></i></a>' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);" > <i class="fa fa-trash text-danger"></i></a>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Export all data as EXCEL',
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

            //ng-click="getorgvalue(user,categorylist,classlist)"
            //ng-click="deletedata(user)"
        };
        $scope.exportExcel = function () {
            debugger;
            var grid = $scope.gridApi.grid;
            var rowTypes = exportUiGridService.ALL;
            var colTypes = exportUiGridService.ALL;
            exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
        };
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("Employee_Add_logs_Manual/getalldetails", pageid).then(
                function (promise) {

                    $scope.type = promise.stf_types;
                    $scope.department = promise.department_types;
                    $scope.selectedept = $scope.department;
                    $scope.designation = promise.designation_types;
                    $scope.selectedesg = promise.designation_types;
                    $scope.holiday = promise.holiday_types;
                    $scope.shiftname = promise.sfname;
                    $scope.employee = promise.employeelist;
                    $scope.temp_employee_arr = promise.employeelist;
                    $scope.gridOptions.data = promise.emplist;

                })
        }

        //type
        $scope.all_check_type = function (emtype) {

            var toggleStatus1 = emtype;
            angular.forEach($scope.type, function (itm) {
                itm.typ = toggleStatus1;
            });
            if ($scope.emtype == true) {
                // $scope.showdepartment = true;
                $scope.showdepartment = true;
            }
            else {
                $scope.showdepartment = false;
                $scope.showdesignation = false;
                if ($scope.showdepartment == false) {
                    $scope.dept = false;
                    angular.forEach($scope.department, function (obj) {
                        obj.dep = false;
                    })

                }
            }

        }
        $scope.addColumn1 = function () {

            $scope.emtype = $scope.type.every(function (itm) { return itm.typ; });
            $scope.get_departments();
        };

        //Department
        $scope.all_check_dept = function (dept) {

            var toggleStatus2 = dept;
            angular.forEach($scope.department, function (itm) {
                itm.dep = toggleStatus2;
            });
            if ($scope.dept == true) {
                // $scope.showdepartment = true;
                $scope.showdesignation = true;
            }
            else {
                $scope.showdesignation = false;
            }

        }
        $scope.addColumn2 = function () {
            $scope.dept = $scope.department.every(function (itm) { return itm.dep; });
            $scope.get_designation();
        };

        //Designation
        $scope.all_check_desig = function (desig) {

            var toggleStatus3 = desig;
            angular.forEach($scope.designation, function (itm) {
                itm.desg = toggleStatus3;
            });
            $scope.get_employee();
        }
        $scope.addColumn3 = function () {
            $scope.desig = $scope.designation.every(function (itm) { return itm.desg; });
            $scope.get_employee();
        };

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
        $scope.get_departments = function () {

            $scope.selectedemptypes = [];
            angular.forEach($scope.type, function (role) {
                if (role.typ) $scope.selectedemptypes.push(role);
            })
            if ($scope.selectedemptypes.length != 0) {
                $scope.showdepartment = true;
                var data = {
                    emptypes: $scope.selectedemptypes,
                }
                apiService.create("EmployeeShiftMapping/get_departments", data).
                    then(function (promise) {

                        //  $scope.showdepartment = true;
                        //  $scope.showdesignation = false;
                        $scope.department = promise.department_types;
                        $scope.count = $scope.department.length;

                        if ($scope.count == 0 && ($scope.selectedemptypes.length != 0)) {
                            swal("No Department Are Mapped with Selected Group Type !!!!");
                            $state.reload();
                        }
                    })
            }
            else if ($scope.selectedemptypes.length == 0) {
                $scope.department = $scope.selectedept;
                $scope.showdepartment = false;
                $scope.showdesignation = false;
                if ($scope.showdepartment == false) {
                    $scope.dept = false;
                }
                //  $scope.showdepartment = true;
                //  $scope.showdesignation = false;
            }
        }

        $scope.get_designation = function () {
            $scope.selecteddesgtypes = [];
            angular.forEach($scope.department, function (role) {
                if (role.dep) $scope.selecteddesgtypes.push(role);
            })
            if ($scope.selecteddesgtypes.length != 0) {
                $scope.showdesignation = true;
                var data = {
                    emptypes: $scope.selecteddesgtypes,
                }
                apiService.create("EmployeeShiftMapping/get_designation", data).
                    then(function (promise) {
                        //  $scope.showdepartment = true;
                        //  $scope.showdesignation = true;
                        $scope.designation = promise.designation_types;
                    })
            }
            else if ($scope.selecteddesgtypes.length == 0) {
                $scope.designation = $scope.selectedesg;
                $scope.showdesignation = false;
                //  $scope.showdepartment = true;
                //  $scope.showdesignation = true;
            }
        }

        $scope.get_employee = function () {

            $scope.showemployeelist = false;
            $scope.selectedemptypes = [];
            $scope.selectedempdept = [];
            $scope.selectedempdesg = [];
            $scope.selectedemptypes.length = 0;
            for (var i = 0; i < $scope.type.length; i++) {
                if ($scope.type[i].typ == true) {
                    $scope.selectedemptypes.push($scope.type[i]);
                }
            }


            $scope.selectedempdept.length = 0;
            for (var i = 0; i < $scope.department.length; i++) {
                if ($scope.department[i].dep == true) {
                    $scope.selectedempdept.push($scope.department[i]);
                }
            }

            $scope.selectedempdesg.length = 0;

            for (var i = 0; i < $scope.designation.length; i++) {
                if ($scope.designation[i].desg == true) {
                    $scope.selectedempdesg.push($scope.designation[i]);
                }
            }

            if ($scope.selectedempdesg.length != 0) {
                var data = {
                    emptypes: $scope.selectedemptypes,
                    empdept: $scope.selectedempdept,
                    empdesg: $scope.selectedempdesg
                }
                apiService.create("EmployeeShiftMapping/get_employee", data).
                    then(function (promise) {
                        if (promise.get_emp != null && promise.get_emp != '') {
                            $scope.employee = promise.get_emp;
                            $scope.showemployeelist = true;
                            $scope.countl = $scope.get_emp.length;
                        }
                        else {
                            $scope.showemployeelist = false;
                        }



                        if ($scope.countl == 0 && ($scope.selectedemptypes.length != 0)) {
                            swal("No Employee Mapped with Selected Designation !!!!");
                        }
                    })
            }
            else if ($scope.selectedempdesg.length == 0) {
                $scope.employee = $scope.temp_employee_arr;
                // $scope.countl = $scope.get_emp.length;
            }
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {

            return !$scope.type.some(function (options) {
                return options.typ;
            });
        }
        $scope.isOptionsRequired1 = function () {

            return !$scope.department.some(function (options) {
                return options.dep;
            });
        }
        $scope.isOptionsRequired2 = function () {

            return !$scope.designation.some(function (options) {
                return options.desg;
            });
        }
        $scope.isOptionsRequired3 = function () {

            return !$scope.employee.some(function (options) {
                return options.emple;
            });
        }
        $scope.isOptionsRequired4 = function () {

            return !$scope.holiday.some(function (options) {
                return options.Selected;
            });
        }


        $scope.Onshiftname = function () {
            $scope.employeelist = [];
            if ($scope.Obj.hrmE_Id.hrmE_Id > 0) {
                var data = {
                    HRME_Id: $scope.Obj.hrmE_Id.hrmE_Id,
                    empdate: $scope.FOEST_Date
                }
                apiService.create("Employee_Add_logs_Manual/empname", data).then(
                    function (promise) {
                        $scope.showdepartment1 = true;
                        if (promise.employeelist != null && promise.employeelist.length > 0) {
                            $scope.employeelist = promise.employeelist;
                            $scope.foepid = promise.employeelist[0].foeP_Id;
                            angular.forEach($scope.employeelist, function (dd) {
                                dd.foepD_PunchTime = moment(dd.foepD_PunchTime, 'H:mm').format()
                                $scope.timedis = true;
                            });
                        }
                        else {
                            swal('Add In and Out Records');
                            $scope.showdepartment1 = true;
                        }

                    })
            } else {
                $scope.morning = false;
                $scope.lunch = false;
                $scope.all = false;
            }
        }

        $scope.clearid = function () {

            $state.reload();

        }
        $scope.deletedata = function (employee) {

            var pageid = employee.foepD_Id;
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
                        apiService.getURI("Employee_Add_logs_Manual/deletedetails", pageid).
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

        $scope.getorgvalue = function (employee) {

            var pageid = employee.foesT_Id;
            $scope.FOEST_Id = pageid;
            apiService.getURI("EmployeeShiftMapping/editdetails", pageid).
                then(function (promise) {
                    if (promise.editlist.length > 0) {
                        $scope.morning = true;
                        $scope.lunch = true;
                        $scope.all = true;
                        $scope.showdepartment = true;
                        $scope.showdesignation = true;
                        $scope.fomS_Id = promise.editlist[0].fomS_Id;
                        $scope.fomsT_IHalfLoginTime = moment(promise.editlist[0].foesT_IHalfLoginTime, 'H:mm  ').format();
                        $scope.FOMST_IHalfLogoutTime = moment(promise.editlist[0].foesT_IHalfLogoutTime, 'H:mm  ').format();
                        $scope.FOMST_IIHalfLoginTime = moment(promise.editlist[0].foesT_IIHalfLoginTime, 'H:mm ').format();
                        $scope.FOMST_IIHalfLogoutTime = moment(promise.editlist[0].foesT_IIHalfLogoutTime, 'H:mm  ').format();
                        $scope.FOMST_FDWHrMin = moment(promise.editlist[0].foesT_FDWHrMin, 'H:mm  ').format();
                        $scope.FOMST_HDWHrMin = moment(promise.editlist[0].foesT_HDWHrMin, 'H:mm  ').format();
                        $scope.FOMST_DelayPerShiftHrMin = moment(promise.editlist[0].foesT_DelayPerShiftHrMin, 'H:mm  ').format();
                        $scope.FOMST_EarlyPerShiftHrMin = moment(promise.editlist[0].foesT_EarlyPerShiftHrMin, 'H:mm  ').format();
                        $scope.FOMST_LunchHoursDuration = moment(promise.editlist[0].foesT_LunchHoursDuration, 'H:mm ').format();
                        $scope.FOEST_Date = promise.editlist[0].foesT_Date;

                        angular.forEach($scope.holiday, function (role) {
                            if (role.fohwdT_Id == promise.editlist[0].fohwdT_Id) {
                                role.Selected = true;
                            }
                        })
                        $scope.selectedDayType.length = 0;
                        for (var i = 0; i < $scope.holiday.length; i++) {
                            if ($scope.holiday[i].Selected == true) {
                                $scope.selectedDayType.push($scope.holiday[i]);
                            }
                        }
                        //  $scope.fohwdT_Id = promise.editlist[0].fohwdT_Id;

                        angular.forEach($scope.employee, function (role) {
                            if (role.hrmE_Id == promise.editlist[0].hrmE_Id) {
                                role.emple = true;
                            }

                        })

                        //angular.forEach($scope.holiday, function (role) {
                        //    if (role.fohwdT_Id == promise.editlist[0].fohwdT_Id) {
                        //        $scope.holi.fohwdT_Id = true;
                        //       // role.holi(fohwdT_Id) = true;
                        //    }

                        //})



                        angular.forEach($scope.type, function (role) {
                            if (role.hrmgT_Id == promise.emplist1[0].hrmgT_Id) {
                                role.typ = true;
                            }
                        })
                        angular.forEach($scope.department, function (role) {
                            if (role.hrmD_Id == promise.emplist1[0].hrmD_Id) {
                                role.dep = true;
                            }
                        })
                        angular.forEach($scope.designation, function (role) {
                            if (role.hrmdeS_Id == promise.emplist1[0].hrmdeS_Id) {
                                role.desg = true;
                            }
                        })

                    }
                    else {
                        $scope.morning = false;
                        $scope.lunch = false;
                        $scope.all = false;

                    }

                })
        }

        $scope.selectedDayType = [];
        $scope.selectedHoliday = function (val) {
            $scope.selectedDayType.length = 0;

            for (var i = 0; i < $scope.holiday.length; i++) {
                if ($scope.holiday[i].Selected == true) {
                    $scope.selectedDayType.push($scope.holiday[i]);
                }
            }
        }
        $scope.submitted = false;
        $scope.saveddata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "InOutFlg": $scope.inout,
                    "FOEPD_PunchTime": $filter('date')($scope.timpic, "H:mm  "),
                    "FOEP_Id": $scope.foepid,
                    "FOEPD_Id": $scope.foepD_Id,
                    "HRME_Id": $scope.Obj.hrmE_Id.hrmE_Id,
                    "MI_Id": $scope.Obj.hrmE_Id.mI_Id,
                    //"FOEP_PunchDate": $filter('date')($scope.FOEST_Date, "HH:mm")
                    "FOEP_PunchDate": new Date($scope.FOEST_Date)
                }

                apiService.create("Employee_Add_logs_Manual/savedetail", data).
                    then(function (promise) {

                        if (promise.returnval == true) {
                            if (promise.foepD_Id == 0 || promise.foepD_Id < 0) {
                                swal('Record saved successfully');
                                $state.reload();
                            }
                            else if (promise.foepD_Id > 0) {
                                swal('Record updated successfully');
                                $state.reload();
                            }
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Record already exist');
                            $state.reload();
                        }
                        else {
                            if (promise.foepD_Id == 0 || promise.foepD_Id < 0) {
                                swal('Failed to save, please contact administrator');
                                $state.reload();
                            }
                            else if (promise.foepD_Id > 0) {
                                swal('Failed to update, please contact administrator');
                                $state.reload();
                            }
                        }
                        $scope.Onshiftname($scope.hrmE_Id, $scope.FOEST_Date);
                    })

            }
            else {
                //swal('At least select one class..!', 'Failed');
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

