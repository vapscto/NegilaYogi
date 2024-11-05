(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterLeaveYearController', masterLeaveYearController)

    masterLeaveYearController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterLeaveYearController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        // form Object
        $scope.Leave = {};

        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                {
                    name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>'
                },
                {
                    name: 'hrmlY_LeaveYear', displayName: 'Leave Year', enableHiding: false
                },
                {
                    name: 'hrmlY_FromDate', displayName: 'From Date', enableHiding: false
                },
                {

                    name: 'hrmlY_ToDate', displayName: 'To Date', enableHiding: false
                },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmlY_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmlY_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };

        // Get form Details at onload 
        $scope.onloadGetData = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getURI("MasterLeaveYear/getalldetails", pageid).then(function (promise) {
                if (promise.leaveYearList !== null && promise.leaveYearList.length > 0) {
                    $scope.gridOptions.data = promise.leaveYearList;
                    $scope.gridOptions = promise.leaveYearList;
                    angular.forEach($scope.gridOptions.data, function (value, key) {
                        var fdate = value.hrmlY_FromDate.split('T');
                        value.hrmlY_FromDate = fdate[0];
                        var tdate = value.hrmlY_ToDate.split('T');
                        value.hrmlY_ToDate = tdate[0];
                    });
                }
                $scope.yearListOrder = promise.yeardetailList;
            });
        };

        $scope.seldfryr = true;
        $scope.seldtoyr = true;

        //Validate From Date by Leave Year
        $scope.validatefromdatebyleavetear = function (leaveyear) {
            if (leaveyear !== undefined && leaveyear !== "" && leaveyear !== null) {
                $scope.seldfryr = true;
                $scope.seldtoyr = true;
                $scope.Leave.hrmlY_FromDate = "";
                $scope.Leave.hrmlY_ToDate = "";
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();

                if (leaveyear.length == 4) {
                    leaveyear = new Date(leaveyear);
                    $scope.seldfryr = false;
                    $scope.FromDate = leaveyear;
                    $scope.minDatef = new Date(
                        $scope.FromDate.getFullYear(),
                        $scope.FromDate.getMonth(),
                        $scope.FromDate.getDate());

                    $scope.maxDatef = new Date(
                        $scope.FromDate.getFullYear() + 1,
                        $scope.FromDate.getMonth(),
                        $scope.FromDate.getDate());
                }
                else {
                    $scope.seldfryr = true;
                    $scope.seldtoyr = true;
                    $scope.Leave.hrmlY_FromDate = "";
                    $scope.Leave.hrmlY_ToDate = "";
                    $scope.myForm.$setPristine();
                    $scope.myForm.$setUntouched();
                }

            } else {
                $scope.seldfryr = true;
                $scope.seldtoyr = true;
                $scope.Leave.hrmlY_FromDate = "";
                $scope.Leave.hrmlY_ToDate = "";
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        }

        //validate to date
        $scope.validatetodate = function (FromDate) {
            $scope.seldtoyr = false;
            $scope.ToDate = FromDate;
            $scope.minDatet = new Date(
                $scope.ToDate.getFullYear(),
                $scope.ToDate.getMonth(),
                $scope.ToDate.getDate());

            $scope.maxDatet = new Date(
                $scope.ToDate.getFullYear() + 1,
                $scope.ToDate.getMonth(),
                $scope.ToDate.getDate());
            $scope.Leave.hrmlY_ToDate = "";
        };

        ////validate from date
        //$scope.validatefromdate = function (FromDate) {
        //        $scope.seldfryr = false;
        //        $scope.FromDate = FromDate;
        //        $scope.minDatef = new Date(
        //        $scope.FromDate.getFullYear(),
        //        $scope.toDFromDateate.getMonth(),
        //        $scope.FromDate.getDate());

        //        $scope.maxDatef = new Date(
        //        $scope.FromDate.getFullYear() - 1,
        //        $scope.FromDate.getMonth(),
        //        $scope.FromDate.getDate());
        //}

        // clear form data
        $scope.cancel = function () {
            $scope.seldfryr = true;
            $scope.seldtoyr = true;
            $scope.Leave = {};
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

                $scope.Leave.hrmlY_FromDate = new Date($scope.Leave.hrmlY_FromDate).toDateString();
                $scope.Leave.hrmlY_ToDate = new Date($scope.Leave.hrmlY_ToDate).toDateString();

                var data = $scope.Leave;

                apiService.create("MasterLeaveYear/", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                             
                            }
                            else if (promise.retrunMsg == "Year") {
                                swal("Leave Year is Already Exist ..");
                                return;
                              
                            }

                            else if (promise.retrunMsg == "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg == "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg == "Update") {
                                swal("Record Updated Successfully..");
                                $state.reload();
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            if (promise.leaveYearList !== null && promise.leaveYearList.length > 0) {

                                $scope.gridOptions.data = promise.leaveYearList;

                                angular.forEach($scope.gridOptions.data, function (value, key) {
                                    var fdate = value.hrmlY_FromDate.split('T');
                                    value.hrmlY_FromDate = fdate[0];
                                    //value.hrmlY_FromDate = new Date(value.hrmlY_FromDate).toDateString("yyyy-MM-dd");
                                    var tdate = value.hrmlY_ToDate.split('T');
                                    value.hrmlY_ToDate = tdate[0];
                                    //value.hrmlY_ToDate = new Date(value.hrmlY_ToDate).toDateString();
                                });
                            }
                            $scope.cancel();
                            $scope.onloadGetData();
                        }
                    })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        // Edit Single Record
        $scope.EditData = function (record) {
            var id = record.hrmlY_Id;
            apiService.getURI("MasterLeaveYear/editRecord", id).
                then(function (promise) {
                    if (promise.leaveYearList != null && promise.leaveYearList.length > 0) {
                        $scope.Leave = promise.leaveYearList[0];
                        $scope.Leave.hrmlY_FromDate = new Date($scope.Leave.hrmlY_FromDate);
                        $scope.Leave.hrmlY_ToDate = new Date($scope.Leave.hrmlY_ToDate);
                    }
                });
        };

        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            if (data.hrmlY_ActiveFlag == false) {
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
                        apiService.getURI("MasterLeaveYear/ActiveDeactiveRecord", data.hrmlY_Id).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                        $state.reload();
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }
                                    if (promise.leaveYearList !== null && promise.leaveYearList.length > 0) {
                                        $scope.gridOptions.data = promise.leaveYearList;
                                        angular.forEach($scope.gridOptions.data, function (value, key) {
                                            var fdate = value.hrmlY_FromDate.split('T');
                                            value.hrmlY_FromDate = fdate[0];
                                            //value.hrmlY_FromDate = new Date(value.hrmlY_FromDate).toDateString("yyyy-MM-dd");
                                            var tdate = value.hrmlY_ToDate.split('T');
                                            value.hrmlY_ToDate = tdate[0];
                                            //value.hrmlY_ToDate = new Date(value.hrmlY_ToDate).toDateString();
                                        });
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

        $scope.setYearorder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.hrmlY_Id !== 0) {
                    orderarray[key].hrmlY_LeaveYearOrder = key + 1;
                }
            });
            var data = {
                LeaveorderDTO: orderarray,
            };
            apiService.create("MasterLeaveYear/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.onLoadGetData();
                    }
                });
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.yearListOrder) {
                    $scope.yearListOrder[index].hrmlY_LeaveYearOrder = Number(index) + 1;
                }
            }
        };
    }

})();