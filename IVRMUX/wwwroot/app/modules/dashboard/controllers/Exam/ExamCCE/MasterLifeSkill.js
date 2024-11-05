(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterLifeSkillController', MasterLifeSkillController);

    MasterLifeSkillController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http'];

    function MasterLifeSkillController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.BindData = function () {
            apiService.getDATA("MasterLifeSkill/Getdetails").then(function (promise) {
                $scope.gridOptions.data = promise.filldata;
                $scope.gridOptions1.data = promise.getskilldata;
                $scope.grouptypeListOrder = promise.getskilldata;
                $scope.gridOptions3.data = promise.filldatamapping;
                $scope.fillskill = promise.fillskill;
                $scope.fillskillarea = promise.fillskillarea;
                $scope.fillMastergrade = promise.fillMastergrade;
                $scope.yearlist = promise.getyear;
            });
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ecS_SkillName', displayName: 'Skill Name' },
                { name: 'ecS_SkillCode', displayName: 'Skill Code' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ecS_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ecS_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.Savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ECS_Id": $scope.ecS_Id,
                    "ECS_SkillName": $scope.ecS_SkillName,
                    "ECS_SkillCode": $scope.ecS_SkillCode
                };
                apiService.create("MasterLifeSkill/savedata", data).
                    then(function (promise) {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal('Record saved successfully');
                            }
                            else {
                                swal('Failed to save, please contact administrator');
                            }
                        }
                        else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal('Record updated successfully');
                            }
                            else {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        else if (promise.message === 'Duplicate') {
                            swal('Record already exist');
                        } else {
                            swal('Failed to save /Upadte Record');
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
            $scope.ecS_SkillName = "";
            $scope.ecS_SkillCode = "";
            $scope.ecS_Id = 0;
        };

        $scope.Editexammasterdata = function (EditRecord) {

            var data = {
                "ECS_Id": EditRecord.ecS_Id
            };

            apiService.create("MasterLifeSkill/editdetails/", data).
                then(function (promise) {

                    $scope.ecS_Id = promise.editlist[0].ecS_Id;
                    $scope.ecS_SkillName = promise.editlist[0].ecS_SkillName;
                    $scope.ecS_SkillCode = promise.editlist[0].ecS_SkillCode;
                });
        };

        //to deactive the data
        $scope.deactive = function (deactiveRecord) {

            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.ecS_ActiveFlag === true) {

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
                        apiService.create("MasterLifeSkill/deactivate", deactiveRecord).
                            then(function (promise) {
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
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $scope.cancel();
                    $scope.BindData();
                });

        };


        //Master Life Skill Area Starts 

        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ecsA_SkillArea', displayName: 'Skill Area' },
                { name: 'ecsA_SkillOrder', displayName: 'Skill Area Order' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdataarea(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ecsA_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactivearea(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ecsA_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactivearea(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1 || field.$dirty;
        };


        $scope.Savedataarea = function () {
            if ($scope.myForm2.$valid) {
                var data = {
                    "ECSA_Id": $scope.ecsA_Id,
                    "ECSA_SkillArea": $scope.ecsA_SkillArea,
                    "ECSA_SkillOrder": $scope.ecsA_SkillOrder
                };

                apiService.create("MasterLifeSkill/Savedataarea", data).
                    then(function (promise) {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal('Record saved successfully');
                            }
                            else {
                                swal('Failed to save, please contact administrator');
                            }
                        }
                        else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal('Record updated successfully');
                            }
                            else {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        else if (promise.message === 'Duplicate') {
                            swal('Record already exist');
                        } else {
                            swal('Failed to save /Upadte Record');
                        }
                        $scope.cancelarea();
                        $scope.BindData();
                        // $state.reload();
                    });

            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.cancelarea = function () {
            $scope.ecsA_SkillArea = "";
            $scope.ecsA_SkillOrder = "";
            $scope.ecsA_Id = 0;
        };

        $scope.Editexammasterdataarea = function (EditRecord) {

            var data = {
                "ECSA_Id": EditRecord.ecsA_Id
            };
            apiService.create("MasterLifeSkill/editdetailsarea/", data).then(function (promise) {
                $scope.ecsA_Id = promise.editlist[0].ecsA_Id;
                $scope.ecsA_SkillArea = promise.editlist[0].ecsA_SkillArea;
                $scope.ecsA_SkillOrder = promise.editlist[0].ecsA_SkillOrder;
                $scope.edit = true;
            });
        };

        $scope.deactivearea = function (deactiveRecord) {

            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.ccE_MLSA_ActiveFlag === true) {
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
                        apiService.create("MasterLifeSkill/deactivatearea", deactiveRecord).
                            then(function (promise) {
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
                                $scope.cancelarea();
                                $scope.BindData();
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $scope.cancelarea();
                    $scope.BindData();
                });

        };

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

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].ecsA_SkillOrder = Number(index) + 1;

                }
            }
        };

        $scope.getOrder = function (orderarray) {
            var data = {
                "subexamDTO": orderarray
            };
            apiService.create("MasterLifeSkill/validateordernumber", data).
                then(function (promise) {
                    swal(promise.message);
                    $state.reload();
                });
        };
        //Master Life Skill Area End 


        //Skill Area Mapping Start
        $scope.interacted2 = function (field) {
            return $scope.submitted2 || field.$dirty;
        };


        $scope.gridOptions3 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Year' },
                { name: 'ecS_SkillName', displayName: 'Life Skill Name' },
                { name: 'ecsA_SkillArea', displayName: 'Life Skill Area' },
                { name: 'ecsaM_IndicatorDescription', displayName: 'Description' },
                { name: 'emgR_NAME', displayName: 'Grade Name' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdataareamapping(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ecsaM_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactiveareamapping(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ecsaM_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactiveareamapping(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };


        $scope.MasterGradeselect = function (emgR_Id) {
            var data = {
                "EMGR_Id": $scope.emgR_Id
            };
            apiService.create("MasterLifeSkill/getgrade/", data).
                then(function (promise) {
                    $scope.fillgradename = promise.fillgradename;
                });
        };

        $scope.Savedataareamapping = function () {
            if ($scope.myForm3.$valid) {
                var data = {
                    "ECSAM_Id": $scope.ECSAM_Id,
                    "ECS_Id": $scope.ECS_Id,
                    "ECSA_Id": $scope.ECSA_Id,
                    "ECSAM_IndicatorDescription": $scope.ECSAM_IndicatorDescription,
                    "EMGD_Id": $scope.emgD_Id,
                    "EMGR_Id": $scope.emgR_Id,
                    "ASMAY_Id": $scope.ASMAY_Id
                };

                apiService.create("MasterLifeSkill/Savedataareamapping", data).
                    then(function (promise) {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal('Record saved successfully');
                            }
                            else {
                                swal('Failed to save, please contact administrator');
                            }
                        }
                        else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal('Record updated successfully');
                            }
                            else {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        else if (promise.message === 'Duplicate') {
                            swal('Record already exist');
                        } else {
                            swal('Failed to save /Upadte Record');
                        }
                        $scope.edit1 = false;
                        $scope.cancelareamapping();
                        $scope.BindData();
                    });

            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.cancelareamapping = function () {
            $scope.ECS_Id = "";
            $scope.ECSA_Id = "";
            $scope.emgR_Id = "";
            $scope.ECSAM_IndicatorDescription = "";
            $scope.ECSAM_Id = 0;
            $scope.emgD_Id = 0;

        };

        $scope.Editexammasterdataareamapping = function (EditRecord) {

            var data = {
                "ECSAM_Id": EditRecord.ecsaM_Id
            };

            apiService.create("MasterLifeSkill/editdetailsareamapping/", data).
                then(function (promise) {

                    $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;
                    $scope.ECSA_Id = promise.editlist[0].ecsA_Id;
                    $scope.ECS_Id = promise.editlist[0].ecS_Id;
                    $scope.ECSAM_Id = promise.editlist[0].ecsaM_Id;
                    $scope.ecsA_Id = promise.editlist[0].ecsA_Id;
                    $scope.emgR_Id = promise.editlist[0].emgR_Id;

                    $scope.ECSAM_IndicatorDescription = promise.editlist[0].ecsaM_IndicatorDescription;
                    $scope.MasterGradeselect($scope.emgR_Id);
                    $scope.emgD_Id = promise.editlist[0].emgD_Id;
                    $scope.edit1 = true;
                });
        };

        $scope.deactiveareamapping = function (deactiveRecord) {

            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.ccE_MLSA_ActiveFlag === true) {
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
                        apiService.create("MasterLifeSkill/deactivateareamapping", deactiveRecord).
                            then(function (promise) {
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
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $scope.cancelareamapping();
                    $scope.BindData();
                });
        };

        //Skill Area Mapping End

    }
})();
