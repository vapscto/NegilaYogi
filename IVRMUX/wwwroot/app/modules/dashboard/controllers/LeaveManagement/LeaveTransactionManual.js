(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveTransactionManualController', LeaveTransactionManualController)
    LeaveTransactionManualController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function LeaveTransactionManualController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {
        $scope.editEmployee = {};
        $scope.obj = {};
        $scope.divflg = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.albumNameArraycolumn = [];
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.yearchg = function (obj) {

            var year_name = {};
            angular.forEach($scope.get_year, function (z) {
                if (z.hrmlY_Id == obj) {
                    year_name = z.hrmlY_LeaveYear;
                }
            })
            $scope.maxDateyr = new Date(year_name);


            $scope.minDatefrom = new Date(
                $scope.maxDateyr.getFullYear(),
                $scope.maxDateyr.getMonth(),
                $scope.maxDateyr.getDate()
            );
            $scope.maxDatefrom = new Date(
                $scope.maxDateyr.getFullYear() + 1,
                $scope.maxDateyr.getMonth(),
                $scope.maxDateyr.getDate() - 1
            );
        }
        $scope.showdepartment = true;
        $scope.showdesignation = true;
        $scope.txtlop = true;
        $scope.txtlophd = true;
        //$scope.gridLeavetranslops = {
        //    enableFiltering: true,
        //    enableColumnMenus: false,
        //    paginationPageSizes: [5, 10, 15],
        //    paginationPageSize: 10,
        //    columnDefs: [
        //        { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableHiding: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
        //        { name: 'hrmL_LeaveName', displayName: 'Leave Name', enableHiding: false },
        //        { name: 'hreltD_FromDate', displayName: 'From Date', enableFiltering: false },
        //        { name: 'hreltD_ToDate', displayName: 'To Date', enableFiltering: false },
        //        { name: 'hreltD_TotDays', displayName: 'Availed', enableFiltering: false },
        //        {
        //            field: 'id', name: '',
        //            displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
        //                '<div class="grid-action-cell">' +

        //                '<a ng-if="row.entity.hrelT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
        //                '<span ng-if="row.entity.hrelT_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
        //                '</div>'
        //        }
        //    ],
        //    onRegisterApi: function (gridApi) {
        //        $scope.gridApi = gridApi;
        //        // $scope.gridApi.core.refresh($scope.gridOptions.data);
        //    }

        //};

        $scope.onlyWeekendsPredicate = function (date) {
            var day = date.getDay();
            return day === 1 || day === 2 || day === 3 || day === 4 || day === 5 || day === 6;
        };




        //$scope.gridLeavetranslops = {

        //    columnDefs: [

        //        { name: 'hrmL_LeaveName', displayName: 'Leave Name', enableFiltering: false },
        //        { name: 'hreltD_FromDate', displayName: 'From Date', cellFilter: 'date:\'dd-MM-yyyy\'', enableFiltering: false },
        //        { name: 'hreltD_ToDate', displayName: 'To Date', cellFilter: 'date:\'dd-MM-yyyy\'', enableFiltering: false },
        //        { name: 'hreltD_TotDays', displayName: 'Availed', enableFiltering: false },
        //        { name: 'hreld_ActiveFlag', displayName: 'Action', enableFiltering: false }

        //    ]
        //};

        $scope.loadData = function () {

            var id = 2;
            // $scope.all_check();
            apiService.getURI("LeaveTransactionManual/getLeavetransm", id).
                then(function (promise) {


                    $scope.staff_types = promise.stf_types;
                    $scope.Department_types = promise.department_types;
                    $scope.Designation_types = promise.designation_types;
                    $scope.get_year = promise.get_year;
                    // $scope.employee = promise.get_emp;
                    //$scope.gridLeaveob.data = promise.leave_name;

                    $scope.selectedept = promise.department_types;
                    $scope.selectedesg = promise.designation_types;
                    $scope.temp_employee_arr = promise.get_emp;
                    $scope.temp_emp_lop = promise.get_emp_lop;
                    //  $scope.temp_emp_other = promise.get_emp_other;
                    // $scope.temp_leave_obarr = promise.leave_name;                 
                    $scope.count = $scope.Department_types;
                    $scope.count1 = $scope.Designation_types;
                    $scope.counttr = $scope.get_emp_lop;
                   // $scope.gridLeavetranslops.data = promise.get_emp_lop;
                    $scope.gridLeavetranslops = promise.get_emp_lop;
                    //$scope.gridLeavetranslops.data = promise.get_emp_lop;
                    $scope.divflg = true;

                })
        };

        $scope.all_check_type = function (emtype) {

            var toggleStatus1 = emtype;
            angular.forEach($scope.staff_types, function (itm) {
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
                    $scope.desig = false;
                    angular.forEach($scope.Department_types, function (obj) {
                        obj.dep = false;
                    })
                    angular.forEach($scope.Designation_types, function (obj) {
                        obj.desg = false;
                    })
                }
            }

            //   $scope.showdepartment = true;
            //   $scope.showdesignation = false;
        }

        $scope.addColumn1 = function () {

            $scope.emtype = $scope.staff_types.every(function (itm) { return itm.typ; });
            $scope.get_departments();
        };

        //Department
        $scope.all_check_dept = function (dept) {

            var toggleStatus2 = dept;
            angular.forEach($scope.Department_types, function (itm) {
                itm.dep = toggleStatus2;
            });
            if ($scope.dept == true) {
                // $scope.showdepartment = true;
                $scope.showdesignation = true;
            }
            else {
                $scope.showdesignation = false;
                if ($scope.showdesignation == false) {
                    $scope.desig = false;
                    angular.forEach($scope.Designation_types, function (obj) {
                        obj.desg = false;
                    })

                }
            }


        }
        $scope.addColumn2 = function () {
            $scope.dept = $scope.Department_types.every(function (itm) { return itm.dep; });
            $scope.get_designation();
        };

        //Designation
        $scope.all_check_desig = function (desig) {

            var toggleStatus3 = desig;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.desg = toggleStatus3;
            });
            $scope.get_employee();
        }
        $scope.addColumn3 = function () {
            $scope.desig = $scope.Designation_types.every(function (itm) { return itm.desg; });
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

        //Validation Code
        $scope.isOptionsRequired = function () {

            return !$scope.staff_types.some(function (options) {
                return options.typ;
            });
        }

        $scope.isOptionsRequired1 = function () {

            return !$scope.Department_types.some(function (options) {
                return options.dep;
            });
        }
        $scope.isOptionsRequired2 = function () {

            return !$scope.Designation_types.some(function (options) {
                return options.desg;
            });
        }


        $scope.get_departments = function () {

            $scope.selectedemptypes = [];
            angular.forEach($scope.staff_types, function (role) {
                if (role.typ) $scope.selectedemptypes.push(role);
            })
            if ($scope.selectedemptypes.length != 0) {
                $scope.desig = false;
                $scope.dept = false;
                angular.forEach($scope.Designation_types, function (obj) {
                    obj.desg = false;
                })

                var data = {
                    emptypes: $scope.selectedemptypes,
                }
                apiService.create("LeaveTransactionManual/get_departments", data).
                    then(function (promise) {

                        //  $scope.showdepartment = true;
                        //  $scope.showdesignation = false;
                        $scope.Department_types = promise.department_types;
                        $scope.count = $scope.Department_types.length;

                        if ($scope.count == 0 && ($scope.selectedemptypes.length != 0)) {
                            swal("No Department Are Mapped with Selected Group Type !!!!");

                        }
                    })
            }
            else if ($scope.selectedemptypes.length == 0) {
                $scope.Department_types = $scope.selectedept;
                $scope.showdepartment = false;
                $scope.showdesignation = false;
                if ($scope.showdepartment == false) {
                    $scope.dept = false;
                    $scope.desig = false;
                    angular.forEach($scope.Designation_types, function (obj) {
                        obj.desg = false;
                    })
                    angular.forEach($scope.Department_types, function (obj) {
                        obj.dep = false;
                    })
                }
                //  $scope.showdepartment = true;
                //  $scope.showdesignation = false;
            }
        }

        $scope.get_designation = function () {
            $scope.selecteddesgtypes = [];
            angular.forEach($scope.Department_types, function (role) {
                if (role.dep) $scope.selecteddesgtypes.push(role);
            })
            if ($scope.selecteddesgtypes.length != 0) {
                $scope.showdesignation = true;
                var data = {
                    emptypes: $scope.selecteddesgtypes,
                }
                apiService.create("LeaveTransactionManual/get_designation", data).
                    then(function (promise) {
                        //  $scope.showdepartment = true;
                        //  $scope.showdesignation = true;
                        $scope.Designation_types = promise.designation_types;
                    })
            }
            else if ($scope.selecteddesgtypes.length == 0) {
                $scope.Designation_types = $scope.selectedesg;
                $scope.showdesignation = false;
                $scope.desig = false;
                angular.forEach($scope.Designation_types, function (obj) {
                    obj.desg = false;
                })
                //  $scope.showdepartment = true;
                //  $scope.showdesignation = true;
            }
        }

        $scope.get_employee = function () {

            $scope.selectedemptypes = [];
            $scope.selectedempdept = [];
            $scope.selectedempdesg = [];
            angular.forEach($scope.staff_types, function (role) {
                if (role.typ) $scope.selectedemptypes.push(role);
            })
            angular.forEach($scope.Department_types, function (role) {
                if (role.dep) $scope.selectedempdept.push(role);
            })

            angular.forEach($scope.Designation_types, function (role) {
                if (role.desg) $scope.selectedempdesg.push(role);
            })
            if ($scope.selectedempdesg.length != 0) {
                var data = {
                    emptypes: $scope.selectedemptypes,
                    empdept: $scope.selectedempdept,
                    empdesg: $scope.selectedempdesg
                }
                apiService.create("LeaveTransactionManual/get_employee", data).
                    then(function (promise) {

                        $scope.employee = promise.get_emp;
                        //$scope.countl = $scope.get_emp.length;

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

        $scope.emp_lop = function (obj1) {

            $scope.selectedemplop = [];
            //angular.forEach($scope.employee, function (role)
            //{
            //    if (role.lop) $scope.selectedemplop.push(role);
            //})
            $scope.selectedemplop.push({ HRME_Id: obj1.hrmE_Id});
            if ($scope.selectedemplop.length != 0) {

                var data1 = {
                    emptypes: $scope.selectedemplop,
                }
                apiService.create("LeaveTransactionManual/get_Emp_lop", data1).
                    then(function (promise) {

                        if (promise.get_emp_lop.length > 0) {
                            $scope.gridLeavetranslops = promise.get_emp_lop;

                            angular.forEach($scope.gridLeavetranslops, function (value, key) {
                                var fdate = value.hreltD_FromDate.split('T');
                                value.hreltD_FromDate = fdate[0];
                                var tdate = value.hreltD_ToDate.split('T');
                                value.hreltD_ToDate = tdate[0];                              
                            });
                            $scope.divflg = false;
                        }
                        else {
                            swal("No Record Found with Selected Employee !!!!");
                            $scope.divflg = true;
                        }
                    })
            }
            else if ($scope.selectedemplop.length == 0) {
                $scope.gridLeavetranslops = $scope.temp_emp_lop;
            }


        }
        //$scope.frmdate = function () {
        //    
        //    var fdate = $filter('date')($scope.fromDate, "yyyy-MM-dd");
        //    if ($scope.gridLeavetranslops.data != null) {
        //        for (var i = 0; i < $scope.gridLeavetranslops.data.length; i++) {
        //            if (fdate == $scope.gridLeavetranslops.data[i].hreltD_FromDate && $scope.gridLeavetranslops.data[i].hrelT_ActiveFlag == true) {
        //                $scope.fromDate = "";
        //                swal("Entry already exist for selected date");
        //            }
        //        }
        //    }

        //}

        $scope.frmdate = function () {

            var fdate = $filter('date')($scope.fromDate, "yyyy-MM-dd");
            //var tdate = $filter('date')($scope.toDate, "yyyy-MM-dd");
            //$scope.fftmpdate = new Date($scope.fromDate);
            //$scope.tttmpdate = new Date($scope.fromDate);

            // var ffdate = $filter('date')($scope.fftmpdate, "yyyy-MM-dd");
            //if ($scope.fftmpdate <= $scope.tttmpdate) {             
            //}
            if ($scope.gridLeavetranslops != null) {

                for (var i = 0; i < $scope.gridLeavetranslops.length; i++) {
                    if (fdate == $scope.gridLeavetranslops[i].hreltD_FromDate && $scope.gridLeavetranslops[i].hrelT_ActiveFlag == true && $scope.selectedemplop[0].HRME_Id == $scope.gridLeavetranslops[i].hrmE_Id ) {
                        $scope.fromDate = "";
                        swal("Entry already exist for selected date");
                    }
                    else if ($scope.gridLeavetranslops[i].hreltD_FromDate < fdate) {
                        if (fdate <= $scope.gridLeavetranslops[i].hreltD_ToDate && $scope.selectedemplop[0].HRME_Id == $scope.gridLeavetranslops[i].hrmE_Id ) {
                            $scope.fromDate = "";
                            swal("Entry already exist for selected date");
                        }

                    }
                }
            }
        }

        //Total days
        $scope.todate = function () {

            var a = moment($scope.fromDate);
            var b = moment($scope.toDate);
            //var days = (b - a) / 1000 / 60 / 60 / 24;
            var days = (b - a) / (1000 * 3600 * 24);
            var totald = days + 1;

            if (totald > 0) {
                $scope.totaldays = totald;
                $scope.nooflop = totald;
                // $scope.nooflophd = $scope.totaldays - 0.5;
                if ($scope.cklop == "lopdata") {
                    $scope.txtlop = false;
                    $scope.nooflop = $scope.totaldays;
                }
                else if ($scope.cklop == "lopdatahd") {
                    $scope.txtlop = false;

                    $scope.nooflop = $scope.totaldays - 0.5;
                }
            }
            else {
                swal("To Date must be greater then From selected Date !!!!");
                $scope.totaldays = 0;
            }
        };

        $scope.activelop = function () {
            if ($scope.cklop == "lopdata") {
                $scope.txtlop = false;
                $scope.nooflop = $scope.totaldays + 0.5;
            }
            else if ($scope.cklop == "lopdatahd") {
                $scope.txtlop = false;

                $scope.nooflop = $scope.totaldays - 0.5;
            }
            if ($scope.nooflop <= $scope.totaldays) {
                return true;
            }
            else {
                $scope.nooflop = $scope.totaldays;
            }
        };
        
        $scope.activeloponefour = function () {
            if ($scope.cklop === "lopdata") {
                $scope.txtlop = false;
                $scope.nooflop = $scope.totaldays;
            }
            else if ($scope.cklop === "loponefour") {
                $scope.txtlop = false;
                $scope.nooflop = $scope.totaldays - 0.25;
            }
            if ($scope.nooflop <= $scope.totaldays) {
                return true;
            }
            else {
                $scope.nooflop = $scope.totaldays;
            }
        };

        $scope.activelopthrfour = function () {
            if ($scope.cklop === "lopdata") {
                $scope.txtlop = false;
                $scope.nooflop = $scope.totaldays;
            }
            else if ($scope.cklop === "lopthrfour") {
                $scope.txtlop = false;
                $scope.nooflop = $scope.totaldays - 0.75;
            }
            if ($scope.nooflop <= $scope.totaldays) {
                return true;
            }
            else {
                $scope.nooflop = $scope.totaldays;
            }
        };

        $scope.savedata = function () {

            // $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.fromDate = new Date($scope.fromDate).toDateString();
                $scope.toDate = new Date($scope.toDate).toDateString();
                var str = $scope.value;
                //   $scope.hrmL_Id = 4;
                $scope.hrmlleavetype = "LWP";
                var data1 = {
                    //"HRELT_Id": $scope.HRELT_Id,
                    "HRELTD_Id": $scope.hreltD_Id,
                    "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id,
                    "HRMLY_Id": $scope.hrmlY_Id,
                    "HRELT_Id": $scope.hrelT_Id,
                    "HRML_Id": $scope.hrmL_Id,
                    "HRML_LeaveType": $scope.hrmlleavetype,
                    //"HRELTD_FromDate": $scope.fromDate,
                    //"HRELTD_ToDate": $scope.toDate,
                    "HRELT_FromDate": $scope.fromDate,
                    "HRELT_ToDate": $scope.toDate,
                    "HRELT_TotDays": $scope.nooflop,
                    "HRELTD_LWPFlag": 1,
                }

                apiService.create("LeaveTransactionManual/saveDATA", data1).then(function (promise) {
                    $scope.gridLeavetranslops = promise.master_loplist;
                    if (promise.returnduplicatestatus !== "") {

                        if (promise.returnduplicatestatus == "Add") {
                            swal('Record Saved Successfully', 'Please Regenerate Salary');
                            $state.reload();

                        } else if (promise.returnduplicatestatus == "Update") {
                            swal('Record Updated Successfully');
                            $state.reload();
                        } else if (promise.returnduplicatestatus == "false") {
                            swal('Already Record Available');
                        } else if (promise.returnduplicatestatus == "error") {
                            swal('Something Went Wrong', 'Try Again');

                        } else if (promise.returnduplicatestatus == "LWPNotSet") {
                            swal('LWP Flag Not Set In Master Leave');
                            $state.reload();

                        }

                       
                    }
                    else {
                        swal('Failed to Save/Update record');
                    }
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

        $scope.DeletRecord = function (employee) {

            //$scope.editEmployee = employee.hrelT_Id;
            //var orgaid = $scope.editEmployee
            var data =
            {
                "HRELT_Id": employee.hrelT_Id,
            }
            var mgs = "";
            var confirmmgs = "";
            if (employee.hrelT_ActiveFlag == 0) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else if (employee.hrelT_ActiveFlag == 1) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }

            swal({
                title: "Are you sure?",
                //text: "Do you want to  " + mgs + " " + "the" + " " + employee.ismS_SubjectName + " " + "Subject ?",
                text: "Do you want to  " + mgs + " " + "the Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,  " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("LeaveTransactionManual/Deletedetails", data).
                            then(function (promise) {
                           
                                if (promise.returnval == true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                    $state.reload();
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                    $state.reload();
                                }
                               
                                $scope.emp_lop($scope.selectedemplop[0]);
                            })
                    }
                    else {
                        if (mgs == "Activate") {
                            mgs = "Activation";
                        }
                        if (mgs == "Deactivate") {
                            mgs = "De-Activation";
                        }
                        swal("Record " + mgs + " Cancelled");
                    }
                });



        }

    }
})();