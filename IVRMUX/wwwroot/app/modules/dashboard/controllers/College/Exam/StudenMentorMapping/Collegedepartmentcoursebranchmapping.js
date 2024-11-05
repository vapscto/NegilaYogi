(function () {
    'use strict';
    angular
        .module('app')
        .controller('Collegedepartmentcoursebranchmappingcontroller', Collegedepartmentcoursebranchmappingcontroller)

    Collegedepartmentcoursebranchmappingcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function Collegedepartmentcoursebranchmappingcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.showbtn = false;
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("Collegedepartmentcoursebranchmapping/Getdetails", id).
                then(function (promise) {
                    $scope.deptlist = promise.deptlist;
                    $scope.courselist = promise.courselist;
                    $scope.getdetails = promise.getdetails;
                    $scope.gridOptions.data = $scope.getdetails;
                });
        };


        $scope.getbranch = function () {

            $scope.AMB_Id = "";
            $scope.semesterlist = [];

            var data = {
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("Collegedepartmentcoursebranchmapping/getbranch", data).then(function (promise) {

                if (promise !== null) {
                    $scope.branchlist = promise.branchlist;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            });
        };

        $scope.getsemester = function () {

            $scope.semesterlist = [];

            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("Collegedepartmentcoursebranchmapping/getsemester", data).then(function (promise) {

                if (promise !== null) {
                    $scope.semesterlist = promise.semesterlist;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            });
        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                $scope.arraydetails = [];

                angular.forEach($scope.semesterlist, function (hi) {
                    if (hi.amseid) {
                        $scope.arraydetails.push({ AMSE_Id: hi.amsE_Id, AMSE_SEMName: hi.amsE_SEMName });
                    }
                });

                var data = {
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "HRMD_Id": $scope.HRMD_Id,
                    "semesterselecteddetails": $scope.arraydetails
                };

                apiService.create("Collegedepartmentcoursebranchmapping/savedetails", data).
                    then(function (promise) {
                        // $scope.newuser = promise.mastersubexam;

                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal('Record saved / Updated successfully');
                            } else {
                                swal('Failed To save / Update Record');
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

        //Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmD_DepartmentName', displayName: 'Department Name' },
                { name: 'amcO_CourseName', displayName: 'Course Name' },               
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" data-toggle="modal" data-target="#myModal" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;'
                        +
                        '<a ng-if="row.entity.adcO_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.adcO_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };

        // to Edit Data
        $scope.getorgvalue = function (EditRecord) {
            var data = EditRecord;
            apiService.create("Collegedepartmentcoursebranchmapping/editdeatils", data).
                then(function (promise) {
                    $scope.LPMT_Id = promise.editdetails[0].lpmT_Id;
                    $scope.LPMT_TopicName = promise.editdetails[0].lpmT_TopicName;
                    $scope.LPMT_TopicDescription = promise.editdetails[0].lpmT_TopicDescription;
                    $scope.LPMT_TopicPeriods = promise.editdetails[0].lpmT_TopicPeriods;
                });
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.adcO_ActiveFlag === true) {
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

                        apiService.create("Collegedepartmentcoursebranchmapping/deactivate", deactiveRecord).
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
                CollegeSubjectWithMasterTopicMappingTemporderDTO: orderarray
            };

            apiService.create("Collegedepartmentcoursebranchmapping/validateordernumber", data).
                then(function (promise) {
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

        $scope.isOptionsRequired = function () {
            return !$scope.semesterlist.some(function (options) {
                return options.amseid;
            });
        };

        $scope.viewrecordspopup = function (employee) {

            apiService.create("Collegedepartmentcoursebranchmapping/viewrecordspopup", employee).
                then(function (promise) {
                    $scope.viewrecordspopupdisplay = promise.getsemdetails;
                });
        };

        $scope.semesterdeactive = function (user) {

            var mgs = "";
            var confirmmgs = "";
            if (user.adcobS_ActiveFlag === true) {
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

                        apiService.create("Collegedepartmentcoursebranchmapping/semesterdeactive", user).
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
                                $scope.viewrecordspopupdisplay = promise.getsemdetails;
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };



    }

})();