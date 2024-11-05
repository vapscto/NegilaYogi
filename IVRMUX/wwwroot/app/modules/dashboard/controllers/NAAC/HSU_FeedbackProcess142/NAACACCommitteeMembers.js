(function () {
    'use strict';
    angular
        .module('app')
        .controller('naacaccommitteememberController', naacaccommitteememberController);

    naacaccommitteememberController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache'];
    function naacaccommitteememberController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.Committee = {};

        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ncaccomM_CommitteeName', displayName: 'Committee Name', enableHiding: false },
                { name: 'hrmE_EmployeeFirstName', displayName: 'Employee Name', enableHiding: false },
                { name: 'ncaccommM_MemberName', displayName: 'Member Name', enableHiding: false },
                { name: 'ncaccommM_MemberPhoneNo', displayName: 'Phone No', enableHiding: false },
                { name: 'ncaccommM_MemberEmailId', displayName: 'Email Id', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.ncaccommM_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.ncaccommM_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("NAACACCommitteemember/getalldetails", pageid).then(function (promise) {
                if (promise.commetteeList !== null && promise.commetteeList.length > 0) {
                    $scope.gridOptions.data = promise.commetteeList;
                }
                $scope.committeedropdownlist = promise.committeedropdownlist;
                $scope.empdropdownlist = promise.empdropdownlist;
            });
        };

        $scope.cancel = function () {
            $scope.Committee = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
        };

        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = $scope.Committee;
                apiService.create("NAACACCommitteemember/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }
                            if (promise.commetteeList !== null && promise.commetteeList.length > 0) {
                                $scope.gridOptions.data = promise.commetteeList;
                            }
                            $scope.cancel();
                        }
                    });
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        // Edit Single Record
        $scope.EditData = function (record) {
            var id = record.ncaccommM_Id;
            apiService.getURI("NAACACCommitteemember/editRecord", id).
            then(function (promise) {
                if (promise.commetteeList !== null && promise.commetteeList.length > 0) {
                    $scope.Committee = promise.commetteeList[0];
                }
            });
        };

        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.ncaccommM_ActiveFlg === false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("NAACACCommitteemember/ActiveDeactiveRecord", data.ncaccommM_Id).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }
                                    if (promise.commetteeList !== null && promise.commetteeList.length > 0) {
                                        $scope.gridOptions.data = promise.commetteeList;
                                    }
                                }

                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }

            );
        };
    }

})();