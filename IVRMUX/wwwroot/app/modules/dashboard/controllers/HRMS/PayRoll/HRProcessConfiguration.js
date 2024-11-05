
(function () {
    'use strict';
    angular
.module('app')
.controller('HRProcessConfigurationController', HRProcessConfigurationController)

    HRProcessConfigurationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function HRProcessConfigurationController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {
        $scope.editEmployee = {};
        $scope.selected = {};
        $scope.obj = {};

        $scope.addtocartflag = false;
        $scope.TempData = [];

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;


        $scope.setItemsPerPage = function (num) {
            $scope.itemsPerPage = num;
            $scope.currentPage = 1;
        };

        $scope.loadData = function () {
            
            var id = 2;

            apiService.getURI("HRProcessConfiguration/getalldetails", id).
       then(function (promise) {
           

           $scope.privalue = [];
           $scope.GroupTypelist = promise.groupTypedropdownlist;
           $scope.Departmentlist = promise.departmentdropdownlist;
           $scope.Designationlist = promise.designationdropdownlist;
           $scope.gradelist = promise.gradedropdownlist;
           $scope.approve = promise.approveid;
           $scope.Designation_types = promise.designation_types;
           $scope.gridAuth.data = promise.gridlist;
           $scope.griddatalist = promise.gridlist;
          // console.log(promise.gridlist);
           $scope.allActivity = promise.privalue;
           // $scope.getauthdata();
           if ($scope.count == 0) {
               swal("Data not Found !!");
               $scope.typeck = false;
           }

       })


        };

        
        $scope.gridAuth = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [

                { name: 'hrmgT_NAME', displayName: 'Group Type', enableFiltering: true },
                { name: 'hrmD_NAME', displayName: 'Department', enableFiltering: true },
                { name: 'hrmdeS_NAME', displayName: 'Designation', enableFiltering: true },
                { name: 'hrpaoN_SanctionLevelNo', displayName: 'Order', enableFiltering: true },
                { name: 'ivrmstauL_UserName', displayName: 'UserName', enableFiltering: true },
                {name: 'hrlP_EmailTo', displayName: 'Email', enableFiltering: true},
                //{ name: 'IVRMUL_UserName', displayName: 'Email', enableFiltering: false },
                //{ name: 'httapproveidorder', displayName: 'Order', enableFiltering: false },
                {
                    field: 'id', name: '', displayName: 'Actions', enableFiltering: false, enableSorting: false,
                    cellTemplate: '<div class="grid-action-cell">' +
                                   '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);" data-toggle="tooltip" title="Edit" > <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                                   '<a href="javascript:void(0)" ng-click="grid.appScope.deletedataY(row.entity);" data-placement="bottom" data-toggle="tooltip" title="Delete"> <i class="fa fa-trash"></i></a> &nbsp; &nbsp;' +
                                  // '<a ng-if="row.entity.HRML_LeaveCreditFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                                //   '<span ng-if="row.entity.HRML_LeaveCreditFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                                   '</div>'
                }
            ]
        };



        $scope.SaveData = function () {
            var checkfinalflag = 0;
            angular.forEach($scope.TempData, function (obj) {
                if (obj.ApprovalFinalFlag) {
                    checkfinalflag += 1;
                }
            });

            if (checkfinalflag === 0 && $scope.hrpaoN_Id==0) {
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
            
            $scope.submitted = true;

                $scope.albumNameArraycolumn = [];
            angular.forEach($scope.allActivity, function (dto) {
                    if (!!dto.Selected) $scope.albumNameArraycolumn.push({
                        columnID:dto.hR_PR_ID,
                        columnName: dto.hR_PR_NAME
                    });
                })
                
                var dto = {
                   "HRPA_Id": 0,
                    "HRPAON_Id": $scope.hrpaoN_Id,
                    "HRMD_Id": $scope.hrhrmDd_Id,
                    "HRMDES_Id": $scope.hrchrmdeS_Id,
                    "HRMG_Id": $scope.hrcchrmG_Id,
                    "HRMGT_Id": $scope.hhhrmgT_Id,
                    "HRLP_EmailTo": $scope.hremail,
                    "HRLP_EmailCC": $scope.hremailcc,
                    //"IVRMUL_Id": $scope.id,
                   // "HRPAON_SanctionLevelNo": $scope.httapproveidorder,
                   // "HRPAON_FinalFlg": $scope.HRPAON_FinalFlg,
                   // tempactivites: $scope.albumNameArraycolumn,
                    approvaluser_array: $scope.TempData

                }

                apiService.create("HRProcessConfiguration/savedata", dto).then(function (promise) {
                    
                    if (promise.retrunMsg=="Add") {
                        swal('Record Saved Successfully');
                        $state.reload();

                    }   
                    else if (promise.retrunMsg == "NotAdded") {
                        swal('Record Not Saved Successfully');
                    }
                    else if (promise.retrunMsg == "Updated") {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.retrunMsg == "NotUpdated") {
                        swal('Record Not Updated Successfully');
                    }

                    else if (promise.retrunMsg =="duplicate") {
                        swal("Records Already Exist!!!");
                    }

                    $scope.TempData = [];
                   
                })
           

        };

     

        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = "";
            $scope.hrpA_Id = 0; $scope.hrmG_Id = 0; $scope.hrmEId = 0; $scope.ivrmuL_Id = ""; $scope.HRLAON_SanctionLevelNo = 0; $scope.hrmgT_Id = 0; $scope.hrmD_Id = 0; $scope.hrmdeS_Id = 0; $scope.HRML_Id = 0; $scope.finalflag = false; $scope.employeelist = [];
            $scope.editEmployee = employee.hrpaoN_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("HRProcessConfiguration/editRecord", pageid).then(function (promise) {
                if (promise.griddisplay != null && promise.griddisplay.length > 0) {
                    
                    $scope.hrpA_Id = promise.griddisplay[0].hrpA_Id;
                    $scope.hrcchrmG_Id = promise.griddisplay[0].hrmG_Id;
                    //$scope.HRPAON_Id = promise.griddisplay[0].ivrmuL_Id;
                    $scope.httapproveidorder = promise.griddisplay[0].hrpaoN_SanctionLevelNo;
                    $scope.hhhrmgT_Id = promise.griddisplay[0].hrmgT_Id;
                    $scope.hrhrmDd_Id = promise.griddisplay[0].hrmD_Id;
                    $scope.hrchrmdeS_Id = promise.griddisplay[0].hrmdeS_Id;
                    $scope.hremail = promise.griddisplay[0].hrlP_EmailTo;
                    $scope.hremailcc = promise.griddisplay[0].hrlP_EmailCC;
                    $scope.hrpaoN_Id = employee.hrpaoN_Id;
                    $scope.HRPAON_FinalFlg = promise.griddisplay[0].hrpaoN_FinalFlg;
                    $scope.id = promise.griddisplay[0].ivrmstauL_Id;

                    $scope.hrpA_TypeFlag = promise.griddisplay[0].hrpA_TypeFlag;
                    $scope.activitydesable = true;
                    angular.forEach($scope.allActivity, function (Activity) {
                        if (Activity.hR_PR_NAME == promise.griddisplay[0].hrpA_TypeFlag) {
                            // Activity.Selected = promise.griddisplay[0].hrpA_TypeFlag;
                            Activity.Selected = true;
                        }
                        else {
                            Activity.Selected = false;
                        }
                    });
                }
                else {
                    //$scope.hrpA_Id = 0; $scope.hrmG_Id = 0; $scope.hrmEId = 0; $scope.ivrmuL_Id = ""; $scope.HRLAON_SanctionLevelNo = 0; $scope.hrmgT_Id = 0; $scope.hrmD_Id = 0; $scope.hrmdeS_Id = 0; $scope.HRML_Id = 0; $scope.finalflag = false; $scope.employeelist = [];

                }

            });
        };


        $scope.cancel = function () {
            $state.reload();

        };

        $scope.interacted = function () {
            return $scope.submitted;
        };


        $scope.AddToCart = function () {
            
            if ($scope.myForm.$valid) {
                $scope.empname = "";
                $scope.empid = "";
                $scope.empapprovallevel = "";
                $scope.addtocartflag = true;

                angular.forEach($scope.allActivity, function (activ) {
                    if (activ.Selected == true) {
                        angular.forEach($scope.approve, function (dd) {
                            if (dd.id === Number($scope.id)) {
                                $scope.empname = dd.ivrmstauL_UserName;
                                $scope.empid = dd.id;
                            }
                        });

                        if ($scope.TempData.length === 0) {
                            $scope.TempData.push({
                                ApprovalEmpName: $scope.empname, Approval_HRME_Id: $scope.empid, ApprovalLevelNo: Number($scope.httapproveidorder), hR_PR_NAME: activ.hR_PR_NAME,
                                ApprovalFinalFlag: $scope.HRPAON_FinalFlg
                            });
                            $scope.id = "";
                           // $scope.httapproveidorder = "";
                            // $scope.HRPAON_FinalFlg = false;
                        }
                        else if ($scope.TempData.length > 0) {
                            $scope.levelcount = 0;
                            angular.forEach($scope.TempData, function (d) {
                                if (d.ApprovalLevelNo === Number($scope.httapproveidorder) && d.hR_PR_NAME == activ.hR_PR_NAME) {
                                    $scope.levelcount += 1;
                                }
                            });
                            if ($scope.levelcount === 0) {
                                $scope.empcount = 0;
                                angular.forEach($scope.TempData, function (d) {
                                    if (d.Approval_HRME_Id === Number($scope.id) && d.hR_PR_NAME == activ.hR_PR_NAME) {
                                        $scope.empcount += 1;
                                    }
                                });

                                if ($scope.empcount === 0) {
                                    $scope.TempData.push({
                                        ApprovalEmpName: $scope.empname, Approval_HRME_Id: $scope.empid, ApprovalLevelNo: Number($scope.httapproveidorder), hR_PR_NAME: activ.hR_PR_NAME,
                                        ApprovalFinalFlag: $scope.HRPAON_FinalFlg
                                    });
                                } else {
                                    swal("Approval Person Already Added");
                                }
                                $scope.id = "";
                                //$scope.httapproveidorder = "";
                               // $scope.HRPAON_FinalFlg = false;
                            } else {
                                swal("Sanction Level Order Already Added");
                            }
                        }
                    }
                });



           
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

        $scope.isOptionsRequired = function () {

            return !$scope.allActivity.some(function (options) {
                return options.Selected;
            });
        }

        $scope.deletedataY = function (employee) {
            var id = $scope.editEmployeeY;
            $scope.editEmployeeY = employee.hrpA_Id;
            
            
            var data = {
                "HRPA_Id": employee.hrpA_Id,
                "HRPAON_Id": employee.hrpaoN_Id,
                //"MI_Id": $scope.MI_Id
            }

            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HRProcessConfiguration/deleteauth", data).then(function (promise) {
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
    }
})();