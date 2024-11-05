(function () {
    'use strict';
    angular
.module('app')
.controller('StaffMaxMinDaySettingController', StaffMaxMinDaySettingController)

    StaffMaxMinDaySettingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function StaffMaxMinDaySettingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.editEmployee = {};

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SlNo', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                   { name: 'academicyr', displayName: 'Academic Year' },
                   { name: 'catgname', displayName: 'Category Name' },
                   { name: 'stafname', displayName: 'Staff Name' },
                   { name: 'period', displayName: 'Period' },
                   { name: 'maxday', displayName: 'Max Day' },
                   { name: 'minday', displayName: 'Min Day' },

                  {
                      field: 'id', name: '',
                      displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                    '<div class="grid-action-cell">' +
                    '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                   '<a ng-if="row.entity.ttpmmD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttpmmD_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                    '</div>'
                  }
            ]

        };


        $scope.BindData = function () {
            apiService.getDATA("StaffMaxMinDaySetting/getdetails").
            then(function (promise) {

                $scope.acdlist = promise.acdlist;
                $scope.ctlist = promise.ctlist;
                $scope.stafflist = promise.stafflist;
                $scope.periodlist = promise.periodlist;
                $scope.gridOptions.data = promise.daylistdetail;
                $scope.MinDay = 1;
                $scope.MaxDay = promise.count - 1;
            })
        };



        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.submitted = false;
        $scope.save = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "TTPMMD_Id": $scope.TTPMMD_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TTMP_Id": $scope.TTMP_Id,
                    "TTMC_Id": $scope.TTMC_ID,
                    "HRME_Id": $scope.Staff_Id,
                    "TTPMMD_MaxDay": $scope.MaxDay,
                    "TTPMMD_MinDay": $scope.MinDay

                }
                apiService.create("StaffMaxMinDaySetting/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.cancel();
                            $scope.BindData();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');

                        }
                        else {
                            swal('Data Not Saved !');

                        }
                    })
            }
            else {
                $scope.submitted = true;

            }

        };
        $scope.cancel = function () {
            $scope.TTPMMD_Id = 0;
            $scope.ASMAY_Id = "";
            $scope.TTMP_Id = "";
            $scope.TTMC_ID = "";
            $scope.Staff_Id = "";
            $scope.MaxDay = "";
            $scope.MinDay = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttpmmD_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("StaffMaxMinDaySetting/getdetail", pageid).
            then(function (promise) {
                $scope.TTPMMD_Id = promise.detailedit[0].ttpmmD_Id;
                $scope.ASMAY_Id = promise.detailedit[0].asmaY_Id;
                $scope.TTMP_Id = promise.detailedit[0].ttmP_Id;
                $scope.TTMC_ID = promise.detailedit[0].ttmC_Id;
                $scope.Staff_Id = promise.detailedit[0].hrmE_Id;
                $scope.MaxDay = promise.detailedit[0].ttpmmD_MaxDay;
                $scope.MinDay = promise.detailedit[0].ttpmmD_MinDay;

            })
            $scope.cancel();
        };


        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttpmmD_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("StaffMaxMinDaySetting/deactive", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        };


    }

})();