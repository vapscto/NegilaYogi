(function () {
    'use strict';
    angular.module('app').controller('SchoolStaffperiodmappingController', SchoolStaffperiodmappingController)
    SchoolStaffperiodmappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function SchoolStaffperiodmappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.showbtn = false;
        $scope.details = false;
        //TO  GEt The Values iN Grid
        $scope.currentPage = 1;
        // $scope.itemsPerPage = 1;

        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("SchoolStaffperiodmapping/Getdetails", id).then(function (promise) {
                $scope.masteryear = promise.masteryear;
                $scope.employeedetails = promise.employeedetails;
            });
        };

        $scope.obj = {};

        $scope.getemployeedetails = function () {
            $scope.ASMS_Id = "";
            $scope.ISMS_Id = "";
            $scope.getalldates = [];
            $scope.details = false;
            var data = {
                "HRME_Id": $scope.HRME_Id.hrmE_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("SchoolStaffperiodmapping/getemployeedetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.masterclass = promise.masterclass;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onchangeclass = function () {
            $scope.ISMS_Id = "";
            $scope.getalldates = [];
            $scope.details = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "HRME_Id": $scope.HRME_Id.hrmE_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("SchoolStaffperiodmapping/onchangeclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersection = promise.mastersection;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };


        $scope.onchangesection = function () {
            $scope.ISMS_Id = "";
            $scope.getalldates = [];
            $scope.details = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "HRME_Id": $scope.HRME_Id.hrmE_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("SchoolStaffperiodmapping/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersubjects = promise.mastersubjects;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.getsearchdetails = function () {

            $scope.submitted = true;
            $scope.getalldates = [];
            $scope.details = false;

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ISMS_Id": $scope.ISMS_Id
                };
                apiService.create("SchoolStaffperiodmapping/getsearchdetails", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getalldates = promise.getalldates;

                        $scope.gettopicdetails = promise.gettopicdetails;

                        if ($scope.gettopicdetails.length === 0) {
                            swal("No Topic Is Mapped For This Selection Details");
                            return;
                        }

                        $scope.getsavedetails = promise.getsavedetails;

                        angular.forEach($scope.getalldates, function (dd) {

                            angular.forEach($scope.getsavedetails, function (ddd) {
                                if (dd.alldates === ddd.lplP_LPDate) {
                                    dd.lpmT_Id = ddd.lpmT_Id;
                                    dd.Selected = true;
                                    dd.lplP_Id = ddd.lplP_Id;
                                    dd.lplP_ClassTakenFlg = ddd.lplP_ClassTakenFlg;
                                }
                            });
                        });

                        console.log($scope.getalldates);

                        $scope.details = true;

                    } else {
                        swal("Something Went Wrong Kindly Contact Administrator");
                    }
                });

            } else {
                $scope.details = false;
                $scope.submitted = true;
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.topicdetails.some(function (options) {
                return options.Selected;
            });
        };

        // TO Save The Data
        $scope.submitted = false;

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.arraydetails = [];
                angular.forEach($scope.getalldates, function (hi) {
                    if (hi.Selected) {
                        $scope.arraydetails.push({
                            LPLP_LPDate: new Date(hi.alldates).toDateString(),
                            LPMT_Id: hi.lpmT_Id, LPLP_Id: hi.lplP_Id
                        });
                    }
                });

                console.log($scope.arraydetails);

                if ($scope.arraydetails.length > 0) {
                    var data = {
                        "ISMS_Id": $scope.ISMS_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id,
                        "HRME_Id": $scope.HRME_Id.hrmE_Id,
                        "SchoolStaffPeriodMappingTempDTO": $scope.arraydetails
                    };

                    apiService.create("SchoolStaffperiodmapping/savedata", data).then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record saved / Updated successfully');
                        } else {
                            swal('Failed To save / Update Record');
                        }
                        $scope.cancel();
                    });
                } else {
                    swal("Kindly Select The Dates To Save The Details");
                }               
            } else {
                $scope.submitted = true;
            }

        };

        // to Edit Data
        $scope.getorgvalue = function (EditRecord) {
            var data = EditRecord;
            apiService.create("CollegeSubjetMasterTopicMapping/editdeatils", data).then(function (promise) {
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
            if (deactiveRecord.lpsmtM_ActiveFlag === true) {
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
                        apiService.create("CollegeSubjetMasterTopicMapping/deactivate", deactiveRecord).then(function (promise) {
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

            apiService.create("CollegeSubjetMasterTopicMapping/validateordernumber", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal("order Updated Successfully");
                } else {
                    swal("Failed To Update order");
                }
                $scope.cancel();
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

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return JSON.stringify(obj.alldates).indexOf($scope.searchValue) >= 0;
        };

        //Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ismS_SubjectName', displayName: 'Subject Name' },
                { name: 'lpmT_TopicName', displayName: 'Topic Name' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.lpsmtM_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.lpsmtM_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };
    }

})();