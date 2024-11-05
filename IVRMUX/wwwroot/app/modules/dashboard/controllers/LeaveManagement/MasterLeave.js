
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterLeaveController', MasterLeaveController)

    MasterLeaveController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function MasterLeaveController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {
        $scope.editEmployee = {};
        $scope.selected = {};
        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmL_LeaveName', displayName: 'LeaveName' },
                   { name: 'hrmL_LeaveCode', displayName: 'LeaveCode' },

                      {
                          field: 'id', name: '',
                          displayName: 'Actions', enableFiltering: false, enableSorting: false,
                          cellTemplate:
                           '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);" data-toggle="tooltip" title="Edit" > <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                         '<a href="javascript:void(0)" ng-click="grid.appScope.deletedataY(row.entity);" data-placement="bottom" data-toggle="tooltip" title="Delete"> <i class="fa fa-trash"></i></a> &nbsp; &nbsp;' +
                       '<a ng-if="row.entity.HRML_LeaveCreditFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                     '<span ng-if="row.entity.HRML_LeaveCreditFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                       '</div>'
                      }

               //{
              //     field: 'id', name: '',
              //     displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
              //         '<div class="grid-action-cell">' +
              //   '<a href="javascript:void(0)" ng-click="grid.appScope.Editdata(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
              //  '<a ng-if="row.entity.ttmC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              //'<span ng-if="row.entity.ttmC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
              //  '</div>'
              // }
            ]
        };

        $scope.resetLists = function () {
            $scope.configA = {
                onUpdate: function (evt) {
                    var itemEl = evt.item;
                }
            };
        };

        $scope.getorgvalue = function (employee) {            
            $scope.editEmployee = employee.hrmL_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterLeave/getdetails", pageid).
            then(function (promise) {
                $scope.HRML_Id = promise.edit_m_event[0].hrmL_Id;
                $scope.HRML_LeaveName = promise.edit_m_event[0].hrmL_LeaveName;
                $scope.HRML_LeaveCode = promise.edit_m_event[0].hrmL_LeaveCode;
                $scope.HRML_LeaveDetails = promise.edit_m_event[0].hrmL_LeaveDetails;
                $scope.ltmodel = promise.edit_m_event[0].hrmL_LeaveType;
                $scope.desig = promise.edit_m_event[0].hrmL_LeaveCreditFlg;
                $scope.lateflag = promise.edit_m_event[0].hrmL_LateDeductFlag;
                //if (promise.edit_m_event[0].hrmL_LateDeductFlag === true) {
                //    $scope.late_flag = "1";
                //}
                //else {
                //    $scope.late_flag = "0";
                //}
                //$scope.late_flag = promise.edit_m_event[0].hrmL_LateDeductFlag;
            })
        }




        $scope.deletconferm = function (employee) {
            $scope.editEmployeeY = employee;
            var id = $scope.editEmployeeY;

            var data = {
                "HRML_Id": id,
            }
            apiService.create("MasterLeave/deletepages", data).then(function (promise) {

                $scope.BindData();
                $scope.gridOptions.data = promise.master_eventlist;
                if (promise.returnval === true) {
                    swal('Record Deleted Successfully');
                }
                else {
                    swal('Mapping is Already defined');
                }

            })
        }



        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.hrmL_Id;
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
                        $scope.deletconferm(id);
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };



        $scope.deletedataY1 = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.hrmL_Id;
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
                    apiService.getURI("MasterLeave/deletepages", id).
                    then(function (promise) {
                        $scope.BindData();
                        $scope.gridOptions.data = promise.master_eventlist;
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Mapping is Already defined');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};




        //Load Page
        $scope.BindData = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("MasterLeave/GetLeave").
       then(function (promise) {
           if (promise.count == 0) {
               swal("No Records Found.....!!");
               return;
           }
           else {
               //$scope.gridOptions.data = promise.master_eventlist;
               $scope.gridOptions = promise.master_eventlist;
               $scope.vargrid = promise.master_eventlist;
               $scope.edit_m_event = promise.leaveorderlist;
           }

       })
        };

        //$scope.Editdata = function (HRML_Id) {
        //    
        //    apiService.getURI("MasterLeave/Edit/", HRML_Id).
        //    then(function (promise) {
        //        $scope.HRML_Id = promise.edit_m_event[0].HRML_Id;
        //        $scope.HRML_LeaveName = promise.edit_m_event[0].HRML_LeaveName;
        //        $scope.HRML_LeaveCode = promise.edit_m_event[0].HRML_LeaveCode;
        //    })
        //};
        //

        $scope.submitted = false;
        $scope.savedata = function () {
            
           // $scope.submitted = true;
            if ($scope.myForm.$valid)
            {
                var str = $scope.value;
                var data = {
                    "HRML_Id": $scope.HRML_Id,
                    "HRML_LeaveName": $scope.HRML_LeaveName,
                    "HRML_LeaveCode": $scope.HRML_LeaveCode,
                    "HRML_LeaveDetails": $scope.HRML_LeaveDetails,
                    "HRML_LeaveType": $scope.ltmodel,
                    "HRML_LeaveCreditFlg": $scope.desig,
                    "HRML_LateDeductFlag": $scope.lateflag
                }

                apiService.create("MasterLeave/savedetail1", data).then(function (promise) {
                    

                    $scope.gridOptions.data = promise.master_eventlist;
                    $scope.edit_m_event = promise.leaveorderlist;
                    if (promise.returnval_add == true && promise.returnval_Update == false)
                    {
                        swal('Record Saved Successfully');
                        $state.reload();

                    }
                   else if (promise.returnval_Update == true && promise.returnval_add== false)
                    {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                  else  if (promise.returnval_add == false)
                    {
                        swal('Failed to Save record');
                    }

                   else if (promise.returnval_Update == false)
                    {
                        swal('Failed to  Update record');
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
           
            $scope.HRML_LeaveName = "";
            $scope.HRML_LeaveCode = "";
            $scope.HRML_LeaveDetails = "";
            $scope.ltmodel = "";
            $scope.desig = "";            
        }

        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.hrmL_Id !== 0) {
                    orderarray[key].hrmL_LateDeductOrder = key + 1;
                }
            });
            var data = {
                MasterLeaveDTOO: orderarray,
            }
            apiService.create("MasterLeave/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.BindData();
                    }
                });
        }

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {

            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {

            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }
    }

})();