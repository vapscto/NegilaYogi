(function () {
    'use strict';

    angular.module('app').controller('CoScholasticActivityController', CoScholasticActivityController);

    CoScholasticActivityController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function CoScholasticActivityController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};

        $scope.tab2click = function () {
            $scope.termdtl = false;
        };

        $scope.tab1click = function () {
            $scope.termdtl = false;
            $scope.BindData();
        };

        $scope.termdtl = false;

        $scope.Toggle_header = function () {

            var toggleStatus = $scope.exmall;
            angular.forEach($scope.examnamelist, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.Toggle_field = function (chk_box) {
            $scope.exmall = $scope.examnamelist.every(function (itm) { return itm.selected; });
        };



        $scope.BindData = function () {
            apiService.getDATA("CoScholasticActivity/Getdetails").then(function (promise) {

                $scope.gridOptions.data = promise.gridlist;
                $scope.gridOptions1.data = promise.gridlist1;
                $scope.grouptypeListOrder = promise.gridlist1;

                $scope.gridOptions2.data = promise.getactivitesareamappinglist;
                $scope.fillactivity = promise.getactiviteslist;
                $scope.fillactivityarea = promise.getactivitesarealist;
                $scope.fillMastergrade = promise.fillMastergrade;

                $scope.yearlist = promise.getyear;

            });
        };

        //---------------------------------Co-Scholastic Activities       
        $scope.savedata = function () {
            $scope.submitted = false;

            if ($scope.myForm.$valid) {
                var data = {
                    "ECACT_SkillName": $scope.ecacT_SkillName,
                    "ECACT_SkillCode": $scope.ecacT_SkillCode,
                    "ECACT_Id": $scope.ecacT_Id
                };

                apiService.create("CoScholasticActivity/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.ecacT_Id === 0 || promise.ecacT_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ecacT_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ecacT_Id === 0 || promise.ecacT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.ecacT_Id > 0) {
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

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $scope.ecacT_SkillName = "";
            $scope.ecacT_SkillCode = "";
            $state.reload();
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'Termname', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ecacT_SkillName', displayName: 'Activities' },
                { name: 'ecacT_SkillCode', displayName: 'Activity Code', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ecacT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ecacT_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.deactive = function (deactiveRecord) {

            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.ecacT_ActiveFlag === true) {
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
                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        };
                        apiService.create("CoScholasticActivity/deactivate", deactiveRecord).
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
                                $scope.cancel();
                                $scope.BindData();
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.Editexammasterdata = function (EditRecord) {

            var MEditId = EditRecord.ecacT_Id;
            apiService.getURI("CoScholasticActivity/editdetails/", MEditId).
                then(function (promise) {
                    $scope.ecacT_Id = promise.editlist[0].ecacT_Id;
                    $scope.ecacT_SkillName = promise.editlist[0].ecacT_SkillName;
                    $scope.ecacT_SkillCode = promise.editlist[0].ecacT_SkillCode;
                });
        };

        //-----------------------------------

        //---------------------------------Co-Scholastic Activities    

        $scope.savedata1 = function () {
            $scope.submitted1 = false;

            if ($scope.myForm1.$valid) {
                var data = {
                    "ECACTA_SkillArea": $scope.ecactA_SkillArea,
                    "ECACTA_SkillOrder": $scope.ecactA_SkillOrder,
                    "ECACTA_Id": $scope.ecactA_Id
                };

                apiService.create("CoScholasticActivity/savedetails1", data).
                    then(function (promise) {

                        if (promise.returnval === true) {
                            if (promise.ecactA_Id === 0 || promise.ecactA_Id < 0) {
                                swal('Record saved successfully');
                            }
                            else if (promise.ecactA_Id > 0) {
                                swal('Record updated successfully');
                            }
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.ecactA_Id === 0 || promise.ecactA_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.ecactA_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $scope.cancel1();
                        $scope.BindData();
                    });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1 || field.$dirty;
        };

        $scope.cancel1 = function () {
            $scope.ecactA_SkillArea = "";
            $scope.ecactA_SkillOrder = "";
        };

        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'Termname', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ecactA_SkillArea', displayName: 'Activities Area' },
                { name: 'ecactA_SkillOrder', displayName: 'Activity Order' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata1(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ecactA_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ecactA_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].ecactA_SkillOrder = Number(index) + 1;

                }
            }
        };

        $scope.deactive1 = function (deactiveRecord) {

            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.ecactA_ActiveFlag === true) {
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
                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        };
                        apiService.create("CoScholasticActivity/deactivate1", deactiveRecord).
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
                                $scope.cancel1();
                                $scope.BindData();
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.Editexammasterdata1 = function (EditRecord) {

            var MEditId = EditRecord.ecactA_Id;
            apiService.getURI("CoScholasticActivity/editdetails1/", MEditId).
                then(function (promise) {
                    $scope.ecactA_Id = promise.editlist1[0].ecactA_Id;
                    $scope.ecactA_SkillArea = promise.editlist1[0].ecactA_SkillArea;
                    $scope.ecactA_SkillOrder = promise.editlist1[0].ecactA_SkillOrder;
                });
        };

        $scope.getOrder = function (orderarray) {
            var data = {
                "temp_activiteSkillArea": orderarray
            };
            apiService.create("CoScholasticActivity/validateordernumber", data).
                then(function (promise) {
                    swal(promise.message);
                    $state.reload();
                });
        };

        //-----------------------------------


        //----------- Activites Area Mapping ---------//

        $scope.cancel2 = function () {
            $scope.ecactA_SkillArea = "";
            $scope.ecactA_SkillOrder = "";
        };

        $scope.gridOptions2 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'Termname', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Year' },
                { name: 'ecacT_SkillName', displayName: 'Activities ' },
                { name: 'ecactA_SkillArea', displayName: 'Activities Area' },
                { name: 'ecactaM_IndicatorDescription', displayName: 'Description' },
                { name: 'emgR_GradeName', displayName: 'Grade Name' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editmapping(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ecactaM_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactivemapping(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ecactaM_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactivemapping(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };


        $scope.savedata2 = function () {
            $scope.submitted2 = false;
            if ($scope.myForm3.$valid) {
                var data = {
                    "ECACT_Id": $scope.ECACT_Id,
                    "ECACTA_Id": $scope.ECACTA_Id,
                    "EMGR_Id": $scope.EMGR_Id,
                    "ECACTAM_IndicatorDescription": $scope.ECACTAM_IndicatorDescription,
                    "ECACTAM_Id": $scope.ECACTAM_Id,
                    "ASMAY_Id": $scope.ASMAY_Id
                };

                apiService.create("CoScholasticActivity/savedetail2", data).then(function (promise) {
                    if (promise.returnduplicatestatus === "Add") {
                        if (promise.returnval === true) {
                            swal('Record saved successfully');
                        } else {
                            swal('Failed to save, please contact administrator');
                        }
                    } else if (promise.returnduplicatestatus === "Update") {
                        if (promise.returnval === true) {
                            swal('Record updated successfully');
                        } else {
                            swal('Failed to update, please contact administrator');
                        }
                    } else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        swal('Something Went Wrong please contact administrator');
                    }
                    $scope.cancel3();
                    $scope.BindData();
                });
            }
            else {
                $scope.submitted2 = true;
            }
        };


        $scope.cancel3 = function () {
            $scope.ECACT_Id = "";
            $scope.ECACTA_Id = "";
            $scope.EMGR_Id = "";
            $scope.ECACTAM_IndicatorDescription = "";
        };


        $scope.Editmapping = function (EditRecord) {
            var MEditId = EditRecord.ecactaM_Id;
            apiService.getURI("CoScholasticActivity/editdetails2/", MEditId).then(function (promise) {
                $scope.ASMAY_Id = promise.editlist1[0].asmaY_Id;
                $scope.ECACT_Id = promise.editlist1[0].ecacT_Id;
                $scope.ECACTA_Id = promise.editlist1[0].ecactA_Id;
                $scope.EMGR_Id = promise.editlist1[0].emgR_Id;
                $scope.ECACTAM_Id = promise.editlist1[0].ecactaM_Id;
                $scope.ECACTAM_IndicatorDescription = promise.editlist1[0].ecactaM_IndicatorDescription;
            });
        };

        $scope.get_exam = function () {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "EYC_Id": $scope.eyC_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("CoScholasticActivity/get_exam", data).then(function (promise) {
                $scope.examnamelist = promise.examnamelist;
                if ($scope.ECTMP_Id > 0) {
                    angular.forEach($scope.examnamelist, function (exm) {
                        angular.forEach($scope.editexmlist, function (exm1) {
                            if (parseInt(exm1.emE_Id) === parseInt(exm.emE_Id)) {
                                exm.selected = true;
                            }
                        });
                    });
                }
                if ($scope.examnamelist === null || $scope.examnamelist === "") {
                    swal("Exam are Not Mapped To Selected Category!!!");
                    // $scope.emE_Id = "";
                }
            });
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };


        $scope.viewrecordspopup = function (employee) {
            $scope.editEmployee = employee.ectmP_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("CoScholasticActivity/getexampopup", pageid).then(function (promise) {
                $scope.viewrecordspopupdisplay = promise.exampopup;
            });
        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
    }
})();
