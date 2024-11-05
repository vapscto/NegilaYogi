(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveTransferController', LeaveTransferController)
    LeaveTransferController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'uiGridConstants']
    function LeaveTransferController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $uiGridConstants) {
        $scope.editEmployee = {};
        $scope.obj = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.albumNameArraycolumn = [];

        $scope.ismeridian = true;
        $scope.updateflag = false;
        $scope.all1 = false;

        $scope.gridLeaveTransfer = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmlY_LeaveYear', displayName: 'LEAVE YEAR' },
                { name: 'hrmE_EmployeeFirstName', displayName: 'EMPLOYEE NAME' },
                { name: 'hrmL_LeaveName', displayName: 'LEAVE NAME' },
                { name: 'hrelS_TotalLeaves', displayName: 'LEAVE CREDIT' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false,
                    cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.deletedataY(row.entity);" data-placement="bottom" data-toggle="tooltip" title="Delete"> <i class="fa fa-trash"></i></a> &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.HRML_LeaveCreditFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.HRML_LeaveCreditFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ]
        };


        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.hrelS_Id;
            var id = $scope.editEmployeeY;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("LeaveTransfer/deletepages", id).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                    
                                }
                                else {
                                    swal('Mapping is Already defined');
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };


        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.showdepartment = true;
        $scope.showdesignation = true;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        $scope.searchValue = '';
        $scope.searchValue1 = '';

        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        $scope.gridLeaveob = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+6}}</div>' },
                // { name: 'hrmE_Id', displayName: 'Employee Id', enableHiding: false },
                //{ name: 'hreL_ReferenceNo', displayName: 'Reference No', enableHiding: false },
                //  { name: 'hretdS_TaxDeposited', displayName: 'Loan Type', enableHiding: false },
                //{ name: 'hreL_LoanAmount', displayName: 'Loan Amount', enableHiding: false },
                { name: 'hrmE_EmployeeFirstName', displayName: 'EmployeeName', enableCellEdit: false, enableHiding: false },
                { name: 'LeaveCredit', displayName: 'Leave Credit', enableHiding: false },
                // { name: 'hretdS_DepositedDate', displayName: 'Deposited Date', enableHiding: false },


                //{
                //    field: 'id', name: '',
                //    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                //        '<div class="grid-action-cell">' +
                //        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                //        '<a ng-if="row.entity.hretdS_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                //        '<span ng-if="row.entity.hretdS_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                //        '</div>'
                //}
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };


        $scope.gridLeaveob.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };        

        $scope.search = "";
        $scope.searchValue = "";
        $scope.searchValue1 = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.Clearid1 = function (key) {
            //$state.reload();
        }



        $scope.loadData = function () {

            var id = 2;
            // $scope.all_check();
            apiService.getURI("LeaveTransfer/getLeaveOB", id).
                then(function (promise) {

                    $scope.staff_types = promise.stf_types;
                    $scope.Department_types = promise.department_types;
                    $scope.Designation_types = promise.designation_types;
                    // $scope.employee = promise.get_emp;
                    $scope.leaveyeardropdown = promise.leave_name;
                    $scope.leavedrop = promise.leavearrayyear;

                    $scope.selectedept = promise.department_types;
                    $scope.selectedesg = promise.designation_types;


                    $scope.results11 = promise.result;
                    //$scope.gridLeaveTransfer.data = promise.result;
                    $scope.presentCountgrid = $scope.results11.length;

                    //  $scope.temp_employee_arr = promise.get_emp;

                    //   $scope.temp_employee_arr = promise.get_emp;
                    //   $scope.gridLeaveob.data = promise.get_emp;
                    //   console.log(promise.get_emp);
                    $scope.count = $scope.Department_types.length;
                    $scope.count1 = $scope.Designation_types.length;
                    //$scope.countob = $scope.gridLeaveob.data;

                    $scope.temp_array = promise.leave_year_id;

                    if ($scope.count == 0) {
                        swal("Data not Found !!");
                        $scope.ckdept = false;
                    }

                })
        };

        $scope.allchangeleaveupdate = function (allchange1) {

            var toggleStatus1 = allchange1;
            angular.forEach($scope.employee, function (itm) {
                itm.hrelS_TotalLeaves = toggleStatus1;
            });
        }


        $scope.allchangeleaveuadd = function (allchange1) {

            var toggleStatus1 = allchange1;
            angular.forEach($scope.employee, function (itm) {
                itm.hrelS_CreditedLeaves = toggleStatus1;
            });
        }

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

        //leaveyear




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
                apiService.create("LeaveTransfer/get_departments", data).
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
                apiService.create("LeaveTransfer/get_designation", data).
                    then(function (promise) {
                        //  $scope.showdepartment = true;
                        //  $scope.showdesignation = true;

                        $scope.Designation_types = promise.designation_types;
                        $scope.gridLeaveob.data = promise.get_emp;
                        $scope.gridLeaveob.data = promise.get_emp;
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

                apiService.create("LeaveTransfer/get_Employe_ob", data).then(function (promise) {
                    $scope.employee = promise.get_emp;
                    $scope.gridLeaveob.data = promise.get_emp;
                    $scope.countl = $scope.get_emp.length;

                    if ($scope.countl == 0 && ($scope.selectedemptypes.length != 0)) {
                        swal("No Employee Mapped with Selected Designation !!!!");
                    }
                })
            }
            else if ($scope.selectedempdesg.length == 0) {
                $scope.employee = $scope.temp_employee_arr;

            }
        }


        $scope.onLoadGetData = function () {

            var pageid = 2;
            apiService.getURI("LeaveTransfer/getdetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leave_name = promise.leaveyeardropdown;

                }

            })
        }

        $scope.cance = function () {
            $state.reload();
        };

        $scope.savedata = function (obj) {

         
            if ($scope.myForm.$valid) {

                var data = {};
                var temp_array123 = [];
                if ($scope.updateflag == true) {
                     data = {
                        "HRELS_Id": $scope.hrelS_Id,
                        "HRELS_CBLeaves": $scope.obj.hhrelS_CBLeaves
                    }
                }
                else {
                    //angular.forEach(rows, function (obj) {
                    //    temp_array123.push({
                    //        "HRME_Id": obj.HRME_Id,
                    //        "HRELS_CreditedLeaves": obj.hrelS_TotalLeaves,
                    //    });
                    //})

                    angular.forEach($scope.employee, function (obj) {
                        if (obj.selected==true) {
                            temp_array123.push({
                                "HRME_Id": obj.hrmE_Id,
                                "HRELS_CreditedLeaves": obj.hrelS_TotalLeaves,
                            });
                        }
                       
                    })

                    data = {
                        "HRMLY_Id": $scope.HRMLY_Id,
                        "HRML_Id": $scope.HRML_Id,
                        emplist: temp_array123
                        
                    }
                }

                apiService.create("LeaveTransfer/SaveDetails", data).then(function (promise) {

                        $scope.gridOptions = promise.master_eventlist;
                        if (promise.returnval == true) {
                            swal('Record Saved/Updated Successfully');
                            $state.reload();
                        }
                        else {
                            swal('Failed to Save/Update record!! Please check record may already Exist.');
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.save = function (obj) {

            // $scope.obj = {};
            //var rows = $scope.gridApi.selection.getSelectedRows();
            //console.log(rows.hrmE_Id)

            //var d = new Date();
            //$scope.HRMLY_Id = d.getYear().toString();


            if ($scope.myForm2.$valid) {

                //var rows = $scope.gridApi.selection.getSelectedRows();
                //console.log(rows);
              
                var temp_array12 = [];
                angular.forEach($scope.employee, function (obj) {
                    if (obj.selected == true) {
                        temp_array12.push({
                            "HRME_Id": obj.hrmE_Id,
                            "HRELS_CreditedLeaves": obj.hrelS_CreditedLeaves,                        
                        });
                    }                                     
                })
                $scope.hreobL_Date = new Date().toDateString();
                var data = {                    
                    "HRMLY_Id": $scope.HRMLY_IdLeaveAdd,
                    "HRML_Id": $scope.HRML_IdLeaveAdd,
                    emplist: temp_array12,
                }
                apiService.create("LeaveTransfer/SaveDetails11", data).
                    then(function (promise) {
                        $scope.gridOptions = promise.master_eventlist;
                        if (promise.returnval == true) {
                            swal('Record Saved/Updated Successfully');
                            $scope.loadData();
                        }
                        else {
                            swal('Failed to Save/Update record!! Please check record may already Exist.');
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.selectallemployee = function (all1) {
            var toggleStatus = all1;
            if (toggleStatus == true) {
                angular.forEach($scope.employee, function (itm) {
                    itm.selected = true;
                });
            }
            else {
                angular.forEach($scope.employee, function (itm) {
                    itm.selected = false;
                });
            }

        }
        

        $scope.indiselectionemployeee = function () {
            $scope.all = $scope.employee.every(function (itm) {
                return itm.selected;
            });

        }
        $scope.indiselectionemploye = function () {
            //$scope.all1 = $scope.employee.every(function (itm) {
            //    return itm.selected;
            //});
            angular.forEach($scope.employee, function (obj) {
                if (obj.selected == false) {
                    $scope.all1 = false;
                    return;
                }

            })
        }
        $scope.indiselectionemployee = function () {
            $scope.all1 = $scope.employee.every(function (itm) {
                return itm.selected;
            });

        }


        $scope.savecarryfrd = function (obj) {

            if ($scope.myForm3.$valid) {
                var temp_array123 = [];
                angular.forEach($scope.employee, function (obj) {
                    if (obj.selected == true) {
                        temp_array123.push({
                            HRME_Id: obj.hrmE_Id,
                        });
                    }
                    
                })
                var data = {
                    "HRMLY_Id": $scope.HRMLY_Id,
                    "HRML_Id": $scope.HRML_Id,
                    emplist: temp_array123,
                }
                apiService.create("LeaveTransfer/leavecarryforward", data).then(function (promise) {
                       
                        if (promise.retrunMsg == 'Add') {
                            swal('Leave Carry Forward  Successfully');
                            $scope.loadData();
                        }
                        else {
                            swal('Failed to Save/Update record!!');
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.EditDetails = function (obj) {
            $scope.updateflag = true;

            $scope.hhrelS_TotalLeaves = obj.hrelS_TotalLeaves;
            $scope.obj.hhrelS_CBLeaves = obj.hrelS_CBLeaves;
            $scope.hrmE_EmployeeFirstName = obj.hrmE_EmployeeFirstName;
            $scope.hrmL_LeaveName = obj.hrmL_LeaveName;
            $scope.hrmlY_LeaveYear = obj.hrmlY_LeaveYear;
            $scope.hrelS_Id = obj.hrelS_Id;
            $scope.hrmlY_Id = obj.hrmlY_Id;
            $scope.hrmL_Id = obj.hrmL_Id;
        };
       

        $scope.interacted = function (field) {

            return $scope.submitted;
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


    }
})();