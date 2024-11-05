
(function () {
    'use strict';
    angular.module('app').controller('LeaveAuthorizationController', LeaveAuthorizationController);
    LeaveAuthorizationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function LeaveAuthorizationController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {
        $scope.editEmployee = {};
        $scope.selected = {};
        $scope.finalflag = false;
        $scope.TempData = [];
        $scope.addtocartflag = false;
        $scope.loadData = function () {
            var id = 2;
            $scope.employeelist = [];
            $scope.get_emp = [];
            $scope.grade_name = [];
            $scope.leave_name = [];
            $scope.staff_types = [];
            $scope.Department_types = [];
            $scope.Designation_types = [];
            $scope.gridAuth = [];
            $scope.searchValue = "";

            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;
            var data = {
                "onchangeoronload": "OnLoad"
            };

            apiService.create("LeaveAuthorization/getAuthLeave", data).then(function (promise) {
                $scope.get_emp = promise.get_emp;
                $scope.grade_name = promise.grade_name;
                $scope.leave_name = promise.leave_name;
                $scope.staff_types = promise.stf_types;
                $scope.Department_types = promise.department_types;
                $scope.Designation_types = promise.designation_types;
                //$scope.gridAuth.data = promise.get_auth;
                $scope.gridAuth = promise.get_auth;
                $scope.employeelist = promise.employeelist;
                $scope.get_institution = promise.get_institution;
                $scope.MI_Id = promise.mI_Id;
                if ($scope.count == 0) {
                    swal("Data not Found !!");
                    $scope.typeck = false;
                }
            });
        };

        $scope.OnChangeInstitution = function () {
            $scope.get_emp = [];
            $scope.grade_name = [];
            $scope.leave_name = [];
            $scope.staff_types = [];
            $scope.Department_types = [];
            $scope.Designation_types = [];
            $scope.gridAuth = [];
            $scope.employeelist = [];
            $scope.finalflag = false;
            $scope.TempData = [];
            $scope.addtocartflag = false;

            $scope.hrmG_Id = "";
            $scope.hrmgT_Id = "";
            $scope.hrmD_Id = "";
            $scope.hrmdeS_Id = "";
            $scope.hrmdeS_Id = "";
            $scope.HRML_Id = "";
            $scope.ivrmuL_Id = "";
            $scope.HRLAON_SanctionLevelNo = "";
            $scope.searchValue = "";

            var data = {
                "MI_Id": $scope.MI_Id,
                "onchangeoronload": "OnChange"
            };

            apiService.create("LeaveAuthorization/getAuthLeave", data).then(function (promise) {
                $scope.get_emp = promise.get_emp;
                $scope.grade_name = promise.grade_name;
                $scope.leave_name = promise.leave_name;
                $scope.staff_types = promise.stf_types;
                $scope.Department_types = promise.department_types;
                $scope.Designation_types = promise.designation_types;                
                $scope.gridAuth = promise.get_auth;
                $scope.employeelist = promise.employeelist;                
                if ($scope.count == 0) {
                    swal("Data not Found !!");
                    $scope.typeck = false;
                }
            });

        };

        $scope.getemployeelist = function () {
            var data = {
                "HRMG_Id": $scope.hrmG_Id,
                "HRMGT_Id": $scope.hrmgT_Id,
                "HRMD_Id": $scope.hrmD_Id,
                "HRMDES_Id": $scope.hrmdeS_Id,
                "MI_Id": $scope.MI_Id
            };

            if ($scope.hrmG_Id > 0 && $scope.hrmgT_Id > 0 && $scope.hrmD_Id > 0 && $scope.hrmdeS_Id > 0) {
                apiService.create("LeaveAuthorization/getemployeelist", data).then(function (promise) {
                    $scope.employeelist = promise.employeelist;
                });
            }
        };

        $scope.all_check_type = function () {
            var toggleStatus = $scope.typeck;
            angular.forEach($scope.leave_name, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.togchkbx = function () {
            $scope.typeck = $scope.leave_name.every(function (type) {
                return type.typs;
            });
        };

        $scope.emptogchkbx = function () {
            $scope.employeeck = $scope.employeelist.every(function (types) {
                return types.hrmEId;
            });
        };

        $scope.all_check_Emp = function () {
            var toggleStatuss = $scope.employeeck;
            angular.forEach($scope.employeelist, function (itm) {
                itm.selected = toggleStatuss;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.employeelist.some(function (options) {
                return options.selected;
            });
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.leave_name.some(function (options) {
                return options.selected;
            });
        };


        $scope.AddToCart = function () {
            if ($scope.myForm.$valid) {

                $scope.empname = "";
                $scope.empid = "";
                $scope.empapprovallevel = "";
                $scope.addtocartflag = true;
                angular.forEach($scope.get_emp, function (dd) {
                    if (dd.id === Number($scope.ivrmuL_Id)) {
                        $scope.empname = dd.userName;
                        $scope.empid = dd.id;
                    }
                });

                if ($scope.TempData.length === 0) {
                    $scope.TempData.push({
                        ApprovalEmpName: $scope.empname, Approval_HRME_Id: $scope.empid, ApprovalLevelNo: Number($scope.HRLAON_SanctionLevelNo),
                        ApprovalFinalFlag: $scope.finalflag
                    });
                    $scope.ivrmuL_Id = "";
                    $scope.HRLAON_SanctionLevelNo = "";
                    $scope.finalflag = false;
                }
                else if ($scope.TempData.length > 0) {
                    $scope.levelcount = 0;
                    angular.forEach($scope.TempData, function (d) {
                        if (d.ApprovalLevelNo === Number($scope.HRLAON_SanctionLevelNo)) {
                            $scope.levelcount += 1;
                        }
                    });
                    if ($scope.levelcount === 0) {
                        $scope.empcount = 0;
                        angular.forEach($scope.TempData, function (d) {
                            if (d.Approval_HRME_Id === Number($scope.ivrmuL_Id)) {
                                $scope.empcount += 1;
                            }
                        });

                        if ($scope.empcount === 0) {
                            $scope.TempData.push({
                                ApprovalEmpName: $scope.empname, Approval_HRME_Id: $scope.empid, ApprovalLevelNo: Number($scope.HRLAON_SanctionLevelNo),
                                ApprovalFinalFlag: $scope.finalflag
                            });
                        } else {
                            swal("Approval Person Already Added");
                        }
                        $scope.ivrmuL_Id = "";
                        $scope.HRLAON_SanctionLevelNo = "";
                        $scope.finalflag = false;
                    } else {
                        swal("Sanction Level Order Already Added");
                    }
                }
            } else {
                $scope.submitted = true;
            }
        };

        $scope.DeleteTempdata = function (dd, index) {
            $scope.TempData.splice(index, 1);

            if ($scope.TempData.length === 0) {
                $scope.addtocartflag = false;
            }
        };

        $scope.SaveData = function () {
            var checkfinalflag = 0;
            angular.forEach($scope.TempData, function (obj) {
                if (obj.ApprovalFinalFlag) {
                    checkfinalflag += 1;
                }
            });

            if (checkfinalflag === 0) {
                swal({
                    title: "Final Approval User Not Mapped For This Details",
                    text: "Do you want continue",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            $scope.saveauthdata();
                        }
                        else {
                            swal("Cancelled");
                        }
                    });
            } else {
                $scope.saveauthdata();
            }
        };

        $scope.saveauthdata = function () {
            //$scope.submitted = true;
            //if ($scope.myForm.$valid) {
            $scope.albumNameArray = [];

            //$scope.employeeArray = [];
            //angular.forEach($scope.employeelist, function (emp) {
            //    if (emp.selected) $scope.employeeArray.push(emp);
            //});

            if ($scope.hrlA_Id == undefined) { $scope.hrlA_Id = 0; }

            var data = {
                "HRLA_Id": $scope.hrlA_Id,
                "HRML_Id": $scope.HRML_Id,
                "HRMG_Id": $scope.hrmG_Id,               
                "HRMGT_Id": $scope.hrmgT_Id,
                "HRMD_Id": $scope.hrmD_Id,
                "HRMDES_Id": $scope.hrmdeS_Id,
                emp_array: $scope.employeeArray,
                approvaluser_array: $scope.TempData,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("LeaveAuthorization/saveauthdata", data).then(function (promise) {
                if (promise.returnval == true) {
                    swal('Record Saved/Updated Successfully');
                }
                else if (promise.returnduplicatestatus == "Duplicate") {
                    swal("Records Already Exist!!!");
                }
                else {
                    swal('Failed to Save/Update record');
                }

                
                $scope.finalflag = false;
                $scope.TempData = [];
                $scope.gridAuth = [];
                $scope.HRML_Id = "";
                $scope.gridAuth = promise.get_auth;

            });
            //}
            //else {
            //    $scope.submitted = true;
            //}
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.gridAuth = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'hrmG_GradeName', displayName: 'Grade Name', enableFiltering: true },
                { name: 'hrmE_EmployeeFirstName', displayName: 'Employee Name', enableFiltering: true },
                { name: 'hrmL_LeaveType', displayName: 'Leave Type', enableFiltering: true },
                { name: 'hrmE_EmailId', displayName: 'Email', enableFiltering: true },
                { name: 'leaveauthorizeusername', displayName: 'Sanctioned User ', enableFiltering: true },
                { name: 'hrlaoN_SanctionLevelNo', displayName: 'Order', enableFiltering: false },
                {
                    field: 'id', name: '', displayName: 'Actions', enableFiltering: false, enableSorting: false,
                    cellTemplate: '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);" data-toggle="tooltip" title="Edit" > <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.deletedataY(row.entity);" data-placement="bottom" data-toggle="tooltip" title="Delete"> <i class="fa fa-trash"></i></a> &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.HRML_LeaveCreditFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.HRML_LeaveCreditFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ]
        };

        //Edit and Update
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = "";
            $scope.hrlA_Id = 0; $scope.hrmG_Id = 0; $scope.hrmEId = 0; $scope.ivrmuL_Id = ""; $scope.HRLAON_SanctionLevelNo = 0; $scope.hrmgT_Id = 0; $scope.hrmD_Id = 0; $scope.hrmdeS_Id = 0; $scope.HRML_Id = 0; $scope.finalflag = false; $scope.employeelist = []; 
            $scope.editEmployee = employee.HRLAON_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("LeaveAuthorization/editdetails", pageid).then(function (promise) {
                if (promise.edit_auth != null && promise.edit_auth.length > 0) {
                    $scope.hrlA_Id = promise.edit_auth[0].hrlA_Id;
                    $scope.hrmG_Id = promise.edit_auth[0].hrmG_Id;
                    $scope.hrmEId = promise.edit_auth[0].hrmE_Id;
                    $scope.ivrmuL_Id = promise.edit_auth[0].ivrmuL_Id;
                    $scope.HRLAON_SanctionLevelNo = promise.edit_auth[0].hrlaoN_SanctionLevelNo;
                    $scope.hrmgT_Id = promise.edit_auth[0].hrmgT_Id;
                    $scope.hrmD_Id = promise.edit_auth[0].hrmD_Id;
                    $scope.hrmdeS_Id = promise.edit_auth[0].hrmdeS_Id;
                    $scope.HRML_Id = promise.edit_auth[0].hrmL_Id;
                    $scope.finalflag = promise.edit_auth[0].hrlaoN_FinalFlg;
                    angular.forEach($scope.leave_name, function (role) {
                        if (role.hrmL_Id == promise.edit_auth[0].hrmL_Id) {
                            role.selected = true;
                        }
                    });

                    //$scope.employeelist = promise.employeelist;

                    //angular.forEach($scope.employeelist, function (emp) {
                    //    if (emp.hrmE_Id == promise.edit_auth[0].hrmE_Id) {
                    //        emp.selected = true;
                    //    }
                    //});
                }
                else {
                    $scope.hrlA_Id = 0; $scope.hrmG_Id = 0; $scope.hrmEId = 0; $scope.ivrmuL_Id = ""; $scope.HRLAON_SanctionLevelNo = 0; $scope.hrmgT_Id = 0; $scope.hrmD_Id = 0; $scope.hrmdeS_Id = 0; $scope.HRML_Id = 0; $scope.finalflag = false; $scope.employeelist = [];

                }
               
                //angular.forEach(promise.edit_auth, function (role1) {
                //    angular.forEach($scope.leave_name, function (role) {
                //        if (role.hrmL_Id == role1.hrmL_Id) {
                //            role.typs = true;
                //        }
                //    });
                //});
            });
        };

        //Delete 
        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.hrlA_Id;
            var id = $scope.editEmployeeY;

            var data = {
                "HRLA_Id": employee.hrlA_Id,
                "HRLAON_Id": employee.HRLAON_Id,
                "MI_Id": $scope.MI_Id
            }

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
                        apiService.create("LeaveAuthorization/deleteauth", data).then(function (promise) {
                            $scope.cancel();
                            //$scope.gridAuth.data = promise.get_auth;
                            if (promise.returnval === true) {
                                swal('Record Deleted Successfully');
                            }
                            else {
                                swal('Record Not Deleted Successfully!');
                            }
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
    }
})();