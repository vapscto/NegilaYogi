(function () {
    'use strict';
    angular
        .module('app')
        .controller('Schoolstudentmentormappingcontroller', Schoolstudentmentormappingcontroller)

    Schoolstudentmentormappingcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function Schoolstudentmentormappingcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.showbtn = false;
        $scope.studentdata = false;
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("Schoolstudentmentormapping/Getdetails", id).
                then(function (promise) {
                    $scope.yearlist = promise.getyear;
                    $scope.getdetails = promise.getdetails;
                    $scope.gridOptions.data = $scope.getdetails;
                });
        };


        $scope.onchangeyear = function () {
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.HRME_Id = "";
            $scope.employeedetails = [];
            $scope.templist = [];
            $scope.getstudentlist = [];
            $scope.getsavedstudentlist = [];
            $scope.studentdata = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("Schoolstudentmentormapping/onchangeyear", data).then(function (promise) {

                if (promise !== null) {
                    $scope.classlist = promise.getclass;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            });
        };

        $scope.getsection = function () {
            $scope.ASMS_Id = "";
            $scope.HRME_Id = "";
            $scope.employeedetails = [];
            $scope.templist = [];
            $scope.getstudentlist = [];
            $scope.getsavedstudentlist = [];
            $scope.studentdata = false;
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
            };
            apiService.create("Schoolstudentmentormapping/getsection", data).then(function (promise) {

                if (promise !== null) {
                    $scope.sectionlist = promise.getsection;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            });
        };

        $scope.getemployee = function () {
            $scope.employeedetails = [];
            $scope.HRME_Id = "";
            $scope.templist = [];
            $scope.getstudentlist = [];
            $scope.getsavedstudentlist = [];
            $scope.studentdata = false;
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("Schoolstudentmentormapping/getemployee", data).then(function (promise) {

                if (promise !== null) {
                    $scope.employeedetails = promise.getemployee;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            });
        };

        $scope.getstudentdata = function () {
            $scope.templist = [];
            $scope.getstudentlist = [];
            $scope.getsavedstudentlist = [];
            $scope.studentdata = false;

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id
                };
                apiService.create("Schoolstudentmentormapping/getstudentdata", data).then(function (promise) {

                    if (promise !== null) {
                        $scope.templist = [];

                        $scope.getstudentlist = promise.getstudentdetails;
                        $scope.getsavedstudentlist = promise.getsaveddetails;

                        if ($scope.getstudentlist !== null) {

                            if ($scope.getstudentlist.length > 0) {
                                $scope.studentdata = true;
                                if ($scope.getsavedstudentlist !== null) {
                                    if ($scope.getsavedstudentlist.length > 0) {
                                        $scope.AMME_Id = $scope.getsavedstudentlist[0].ammE_Id;

                                        angular.forEach($scope.getsavedstudentlist, function (dd) {

                                            angular.forEach($scope.getstudentlist, function (ddd) {

                                                if (dd.amsT_Id === ddd.amsT_Id) {
                                                    ddd.Selected = true;
                                                }
                                            });
                                        });
                                    }
                                }

                            } else {
                                swal("No Records Found");
                            }
                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                        }

                        //else {
                        //    angular.forEach($scope.getstudentlist, function (dd) {
                        //        dd.Selected = false;
                        //    });
                        //}
                    } else {
                        swal("Something Went Wrong Kindly Contact Administrator");
                    }

                });
            } else {
                $scope.submitted = true;
            }
        };


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.getstudentlist, function (itm) {
                itm.Selected = toggleStatus;
            });
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.getstudentlist.every(function (itm) { return itm.Selected; });
        };


        // Save data
        $scope.savedata = function () {
            $scope.submitted1 = false;

            if ($scope.myForm1.$valid) {
                $scope.temparry = [];
                angular.forEach($scope.getstudentlist, function (dd) {
                    if (dd.Selected) {
                        $scope.temparry.push({ AMST_Id: dd.amsT_Id });
                    }
                });

                console.log($scope.temparry);


                var obj = {
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "AMME_Id": $scope.AMME_Id,
                    "SchoolstudentmentormappingtempDTO": $scope.temparry
                };

                apiService.create("Schoolstudentmentormapping/savedata", obj).then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Record Saved/Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval === false) {
                        swal("Failed to save /Update record");
                        $state.reload();
                    }
                    else {
                        swal("Sorry...something went wrong");
                        $state.reload();
                    }
                });
            }
            else {

                $scope.submitted1 = true;
            }
        };


        $scope.isOptionsRequired = function () {

            return !$scope.getstudentlist.some(function (options) {
                return options.Selected;
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
                { name: 'asmaY_Year', displayName: 'Academic Year' },                
                { name: 'asmcL_ClassName', displayName: 'Class' },
                { name: 'asmC_SectionName', displayName: 'Section' },
                { name: 'employeename', displayName: 'Employee Name' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" data-toggle="modal" data-target="#myModal" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;'
                        +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };



        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.acdcbM_ActiveFlag === true) {
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

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.cancel = function () {
            $state.reload();
        };


        $scope.viewrecordspopup = function (employee) {
            apiService.create("Schoolstudentmentormapping/viewrecordspopup", employee).
                then(function (promise) {
                    $scope.viewrecordspopupdisplay = promise.getstudentdata;
                });
        };

        $scope.Deletedata = function (user) {

            swal({
                title: "Are you sure",
                text: "Do You Want To Delete This record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,  Delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("Schoolstudentmentormapping/Deletedata", user).
                            then(function (promise) {

                                if (promise.returnval === true) {
                                    swal("Record Deleted successfully");
                                }
                                else {
                                    swal("Record Deletion Failed");
                                }
                                $scope.viewrecordspopupdisplay = promise.getstudentdata;

                            });
                    }
                    else {
                        swal("Record Deleted Cancelled");
                    }
                });
        };
    }

})();