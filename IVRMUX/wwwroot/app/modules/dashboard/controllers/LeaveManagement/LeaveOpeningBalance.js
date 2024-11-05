
(function () {
    'use strict';
    angular
.module('app')
.controller('LeaveOpeningBalanceController', LeaveOpeningBalanceController)
    LeaveOpeningBalanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'uiGridConstants']
    function LeaveOpeningBalanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter,$uiGridConstants) {
        $scope.editEmployee = {};
        $scope.obj = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.albumNameArraycolumn = [];
        $scope.ismeridian = true;

        $scope.gridLeaveOpening = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmE_EmployeeFirstName', displayName: 'EMPLOYEE NAME' },
                { name: 'hrmL_LeaveName', displayName: 'LEAVE NAME' },
                { name: 'hreobL_OBLeaves', displayName: 'LEAVE BALANCE' },
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

        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.hreobL_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("LeaveOpeningBalance/getdetails", pageid).
                then(function (promise) {
                    $scope.HRML_Id = promise.edit_lvblnce[0].hrmL_Id;
                    $scope.HRME_EmployeeFirstName = promise.edit_lvblnce[0].hrmE_EmployeeFirstName;
                    $scope.HRML_LeaveName = promise.edit_lvblnce[0].hrmL_LeaveName;
                    $scope.HREOBL_OBLeaves = promise.edit_lvblnce[0].hreobL_OBLeaves;
                });
        };


        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.hreobL_Id;
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
                        apiService.DeleteURI("LeaveOpeningBalance/deletepages", id).
                            then(function (promise) {
                                
                                //$scope.BindData();
                                //$scope.gridLeaveOpening.data = promise.edit_lvblnce; 
                                if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Mapping is Already defined');
                                }
                                $scope.loadData();
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

        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }


        //$scope.gridLeaveob = { enableCellEditOnFocus: true };
        $scope.gridLeaveob = {
            enableCellEditOnFocus: true,
            enableRowSelection: true,
            enableSelectAll: true,
            columnDefs: [

                  { name: 'SlNo', field: 'name', enableCellEdit: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },                 
                { name: 'hrmL_LeaveName', displayName: 'LeaveName', enableCellEdit: false, enableFiltering: false, },
                //{name:'hrml_id',displayName:'leaveId',enableCellEdit:false,enableFiltering:false},
                { name: 'hreobL_OBLeaves', displayName: 'Leave(Opening Balance)', enableFiltering: false, },
            ]
          

        };
        $scope.gridLeaveob.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };
        //$scope.gridApi.selection.getSelectedRows();


     
       
        $scope.loadData = function () {
            
            var id = 2;
            // $scope.all_check();
            apiService.getURI("LeaveOpeningBalance/getLeaveOB", id).
                then(function (promise) {

                    $scope.staff_types = promise.stf_types;
                    $scope.Department_types = promise.department_types;
                    $scope.Designation_types = promise.designation_types;
                    // $scope.employee = promise.get_emp;
                    $scope.gridLeaveob.data = promise.leave_name;

                    $scope.results11 = promise.result;
                    $scope.gridLeaveOpening.data = promise.result;
                    $scope.presentCountgrid = $scope.results11.length;

                    $scope.selectedept = promise.department_types;
                    $scope.selectedesg = promise.designation_types;
                    $scope.temp_employee_arr = promise.get_emp;
                    // $scope.temp_leave_obarr = promise.leave_name;


                    $scope.count = $scope.Department_types.length;
                    $scope.count1 = $scope.Designation_types.length;
                    //$scope.countob = $scope.gridLeaveob.data;

                    $scope.temp_array = promise.get_year;

                    if ($scope.count == 0) {
                        swal("Data not Found !!");
                        $scope.ckdept = false;
                    }

                });
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
            
            $scope.emtype = $scope.staff_types.every(function (itm)
            { return itm.typ; });
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
            $scope.dept = $scope.Department_types.every(function (itm)
            { return itm.dep; });
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
            $scope.desig = $scope.Designation_types.every(function (itm)
            { return itm.desg; });
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
            $scope.empl = $scope.employee.every(function (itm)
            { return itm.emple; });
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
                apiService.create("LeaveOpeningBalance/get_departments", data).
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
                apiService.create("LeaveOpeningBalance/get_designation", data).
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
             
                apiService.create("LeaveOpeningBalance/get_Employe_ob", data).
                       then(function (promise) {

                           $scope.employee = promise.get_emp;
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

        $scope.savedata = function (obj) {
            
            // $scope.obj = {};
            if ($scope.myForm.$valid) {
            var rows = $scope.gridApi.selection.getSelectedRows();
           
            //$scope.submitted = true;
            //var d = new Date();
            //$scope.HRMLY_Id = d.getYear().toString();


            //if ($scope.myForm.$valid) {

                //$scope.hreobL_Date = new Date().toDateString();
                $scope.hreobL_Date = new Date().toDateString();
                var data = {
                    //"HREOBL_Id":$scope.HREOBL_Id,
                   //"HRME_Id": $scope.obj,
                    //"HRML_Id":$scope.hrmL_Id,    
                    //"HREOBL_Date": new Date($scope.HREOBL_Date).toDateString(),
                    //"HREOBL_OBLeaves": $scope.hreobL_OBLeaves,
                    "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id,
                    "HREOBL_Date": new Date($scope.hreobL_Date).toDateString(),
                    temp_table_data: rows,                    
                    "HRMLY_Id": $scope.temp_array[0].hrmlY_Id,
                
                }
                
                apiService.create("LeaveOpeningBalance/SaveDetails", data).
                    then(function (promise) {
                        $scope.gridOptions = promise.master_eventlist;
                        if (promise.returnval == true) {
                            swal('Record Saved/Updated Successfully');
                            $scope.loadData();
                        }
                        else {
                            swal('Failed to Save/Update record');
                        }
                    })
            }
            else
            {
                $scope.submitted = true;
            }
           
        };
        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.cancel = function () {
          
            $state.reload();
        }
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