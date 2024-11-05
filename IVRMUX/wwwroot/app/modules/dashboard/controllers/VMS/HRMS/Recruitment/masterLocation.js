(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterLocationController', masterLocationController);

    masterLocationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache'];
    function masterLocationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.loc = {};

        // Datatable display
        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmlO_LocationName', displayName: 'Location Name', enableHiding: false },
                { name: 'hrmlO_LocationDesc', displayName: 'Location Description', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmlO_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmlO_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("masterLocation/getalldetails", pageid).then(function (promise) {
                $scope.institutionlist = promise.institutionlist;
                
            });
        };

        $scope.getdata = function () {
            var data = { "MI_Id": $scope.loc.mI_Id };
            apiService.create("masterLocation/getdata", data).
                then(function (promise) {
                    if (promise.locationList !== null && promise.locationList.length > 0) {
                        $scope.gridOptions.data = promise.locationList;
                    }
                    else {
                        swal("No data found !!");
                        return;
                    }
                });
        };

        //// Sort table data
        //$scope.sort = function (keyname) {
        //    $scope.sortKey = keyname;   //set the sortKey to the param passed
        //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        //}

        // clear form data
        $scope.cancel = function () {
            // $scope.search = "";
            $scope.loc = {};
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
                var data = $scope.loc;

                apiService.create("masterLocation/savedetails", data).
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

                            if (promise.locationList !== null && promise.locationList.length > 0) {
                                $scope.gridOptions.data = promise.locationList;
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

            var id = record.hrmlO_Id;
            apiService.getURI("masterLocation/editRecord", id).
                then(function (promise) {
                    if (promise.locationList !== null && promise.locationList.length > 0) {
                        $scope.loc = promise.locationList[0];
                    }
                });
        };


        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmlO_ActiveFlg === false) {
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
                        apiService.DeleteURI("masterLocation/ActiveDeactiveRecord", data.hrmlO_Id).
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

                                    if (promise.locationList !== null && promise.locationList.length > 0) {
                                        $scope.gridOptions.data = promise.locationList;
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