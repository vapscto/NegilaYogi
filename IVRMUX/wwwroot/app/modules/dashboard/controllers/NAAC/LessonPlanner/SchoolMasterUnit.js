(function () {
    'use strict';
    angular
        .module('app')
        .controller('SchoolMasterUnitController', SchoolMasterUnitController)

    SchoolMasterUnitController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function SchoolMasterUnitController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("SchoolMasterUnit/Getdetails", id).then(function (promise) {
                $scope.gridOptions.data = promise.getdetails;
                $scope.grouptypeListOrder = promise.getdetails;
            });
        };

        //Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'lpmU_UnitName', displayName: 'Unit Name' },
                { name: 'lpmU_UnitDescription', displayName: 'Unit Description' },
                { name: 'lpmU_TotalPeriods', displayName: 'Total No of Periods' },
                { name: 'lpmU_TotalHrs', displayName: 'Total No of Hours' },
                { name: 'lpmU_Order', displayName: 'Topic Order', type: 'number' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +

                        '<a ng-if="row.entity.lpmU_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.lpmU_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "LPMU_Id": $scope.LPMU_Id,
                    "LPMU_UnitName": $scope.LPMU_UnitName,
                    "LPMU_UnitDescription": $scope.LPMU_UnitDescription,
                    "LPMU_TotalHrs": $scope.LPMU_TotalHrs === undefined || $scope.LPMU_TotalHrs === null || $scope.LPMU_TotalHrs === "" ? null : $scope.LPMU_TotalHrs,
                    "LPMU_TotalPeriods": $scope.LPMU_TotalPeriods === undefined || $scope.LPMU_TotalPeriods === null || $scope.LPMU_TotalPeriods === "" ? null : $scope.LPMU_TotalPeriods

                };

                apiService.create("SchoolMasterUnit/savedetails", data).then(function (promise) {
                    // $scope.newuser = promise.mastersubexam;

                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal('Record saved successfully');
                        } else {
                            swal('Failed To save Record');
                        }
                    } else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal('Record updated successfully');
                        } else {
                            swal('Failed To Update Record');
                        }
                    }

                    else if (promise.message === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        swal("Failed To Save / Update Record");
                    }
                    $scope.cancel();
                    //$scope.BindData();
                });
            } else {
                $scope.submitted = true;
            }

        };

        // to Edit Data
        $scope.getorgvalue = function (EditRecord) {
            var data = EditRecord;
            apiService.create("SchoolMasterUnit/editdeatils", data).then(function (promise) {
                $scope.LPMU_Id = promise.geteditdetails[0].lpmU_Id;
                $scope.LPMU_UnitName = promise.geteditdetails[0].lpmU_UnitName;
                $scope.LPMU_UnitDescription = promise.geteditdetails[0].lpmU_UnitDescription;
                $scope.LPMU_TotalHrs = promise.geteditdetails[0].lpmU_TotalHrs;
                $scope.LPMU_TotalPeriods = promise.geteditdetails[0].lpmU_TotalPeriods;
            });
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmU_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
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
                        apiService.create("SchoolMasterUnit/deactivate", deactiveRecord).then(function (promise) {
                            if (promise.message === "Exists") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.cancel();
                            // $scope.BindData();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        //fix the order drag
        //ConfigA is an Items
        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };
        $scope.init = function () {
            $scope.resetLists();
        };
        $scope.init();
        $scope.getOrder = function (orderarray) {
            var data = {
                SchoolMasterUnitTempDTO: orderarray
            };

            apiService.create("SchoolMasterUnit/validateordernumber", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal("order Updated Successfully");
                } else {
                    swal("Failed To Update order");
                }
                $scope.cancel();
                // $scope.BindData();
            });
        };


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].lpmU_Order = Number(index) + 1;
                }
            }
        };
    }
})();