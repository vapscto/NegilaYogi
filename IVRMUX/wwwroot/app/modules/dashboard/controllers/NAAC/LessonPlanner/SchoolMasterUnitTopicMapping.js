(function () {
    'use strict';
    angular
        .module('app')
        .controller('SchoolMasterUnitTopicMappingController', SchoolMasterUnitTopicMappingController)

    SchoolMasterUnitTopicMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function SchoolMasterUnitTopicMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("SchoolMasterUnit/Getdetailsmapping", id).
                then(function (promise) {
                    $scope.gridOptions.data = promise.getgriddetails;
                    $scope.getunitdetails = promise.getunitdetails;
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
                { name: 'lpmT_TopicName', displayName: 'Topic Name' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.lpmuT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.lpmuT_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

        $scope.gettopicnames = function () {
            var data = {
                "LPMU_Id": $scope.LPMU_Id
            };
            apiService.create("SchoolMasterUnit/gettopicnames", data).then(function (promise) {
                if (promise !== null) {
                    $scope.gettopicdetails = promise.gettopicdetails;
                    if ($scope.gettopicdetails.length === 0) {
                        swal("All Topics Is Mapped For This Unit");
                    }
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };
        $scope.arraylist = [];
        // TO Save The Data
        $scope.submitted = false;

        $scope.saveddata = function () {
            $scope.submitted = true;
            $scope.arraylist = [];
            if ($scope.myForm.$valid) {
                $scope.arraylist = [];

                angular.forEach($scope.gettopicdetails, function (hi) {
                    if (hi.Selected) {
                        $scope.arraylist.push({ LPMT_Id: hi.lpmT_Id});
                    }
                });

                var data = {
                    "LPMU_Id": $scope.LPMU_Id,
                    "SchoolMasterUnitTopicMappingTempDTO": $scope.arraylist
                };

                apiService.create("SchoolMasterUnit/savemappingdetails", data).
                    then(function (promise) {
                        // $scope.newuser = promise.mastersubexam;

                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal('Record saved / Updated successfully');
                            } else {
                                swal('Failed To save / Updated Record');
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

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmuT_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("SchoolMasterUnit/deactivatemapping", deactiveRecord).
                            then(function (promise) {
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

        $scope.isOptionsRequired = function () {
            return !$scope.gettopicdetails.some(function (options) {
                return options.Selected;
            });
        };
    }

})();