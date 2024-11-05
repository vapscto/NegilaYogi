
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamMasterPersonalityController', ExamMasterPersonalityController)

    ExamMasterPersonalityController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamMasterPersonalityController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //TO  GEt The Values iN Grid
        // var temp_exam_list = [];
        $scope.EME_FinalExamFlag = false;
        $scope.EME_ActiveFlag = true;
        $scope.BindData = function () {
            apiService.getDATA("exammasterPersonality/Getdetails").then(function (promise) {
                $scope.gridOptions.data = promise.exammasterpersonalityname;
                $scope.exammasterpersonalityname = promise.exammasterpersonalityname;
                $scope.grouptypeListOrder = promise.personalityorder;
            });
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'eP_PersonlaityName', displayName: 'Personality Name' },
                { name: 'eP_PersonlaityCode', displayName: 'Personality Code' },
                { name: 'eP_PersonlaityOrder', displayName: 'Personality Order', type: 'number' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.eP_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.eP_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

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
            angular.forEach(orderarray, function (value, key) {
                if (value.per_Id !== 0) {
                    orderarray[key].eP_PersonlaityOrder = key + 1;
                }
            });
            var data = {
                examPersonlityDTO: orderarray
            };

            apiService.create("exammasterPersonality/validateordernumber", data).then(function (promise) {
                if (promise.retrunMsg !== "" && promise.retrunMsg !== undefined && promise.retrunMsg !== null) {
                    swal(promise.retrunMsg);
                }
                $scope.cancel();
                $scope.BindData();
            });
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].eP_PersonlaityOrder = Number(index) + 1;
                }
            }
        };

        //to deactive the data

        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.eP_ActiveFlag === true) {
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
                        apiService.create("exammasterPersonality/deactivate", deactiveRecord).then(function (promise) {
                            if (promise.already_cnt === true) {
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
                            $scope.BindData();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });

        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        };

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        };

        // to Edit Data
        $scope.Editexammasterdata = function (EditRecord) {
            var MEditId = EditRecord.eP_Id;
            apiService.getURI("exammasterPersonality/editdetails/", MEditId).then(function (promise) {
                $scope.per_Id = promise.exammpersonalityname[0].eP_Id;
                $scope.per_Code = promise.exammpersonalityname[0].eP_PersonlaityCode;
                $scope.per_Name = promise.exammpersonalityname[0].eP_PersonlaityName;
                $scope.per_ActiveFlag = promise.exammpersonalityname[0].eP_ActiveFlag;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "EP_Id": $scope.per_Id,
                    "EP_PersonlaityCode": $scope.per_Code,
                    "EP_PersonlaityName": $scope.per_Name
                };

                apiService.create("exammasterPersonality/savedetails", data).then(function (promise) {
                    $scope.newuser = promise.exammastername;

                    if (promise.returnval === true) {
                        if (promise.per_Id === 0 || promise.per_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.per_Id > 0) {
                            swal('Record updated successfully');
                        }

                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.per_Id === 0 || promise.per_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.per_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.cancel();
                    $scope.BindData();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $scope.per_Id = 0;
            $scope.per_Name = "";
            $scope.per_Code = "";
            $scope.per_ActiveFlag = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $scope.search = "";
        };

        $scope.get_older = function () {
            $scope.BindData();
        };
    }

})();