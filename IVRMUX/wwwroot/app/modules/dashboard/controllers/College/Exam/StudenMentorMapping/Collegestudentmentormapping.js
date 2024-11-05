﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('Collegestudentmentormappingcontroller', Collegestudentmentormappingcontroller)

    Collegestudentmentormappingcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function Collegestudentmentormappingcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.showbtn = false;
        $scope.studentdata = false;
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("Collegestudentmentormapping/Getdetails", id).
                then(function (promise) {
                    $scope.yearlist = promise.getyear;
                    $scope.getdetails = promise.getdetails;
                    $scope.gridOptions.data = $scope.getdetails;
                });
        };


        $scope.onchangeyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.HRME_Id = "";
            $scope.employeedetails = [];
            $scope.templist = [];
            $scope.getstudentlist = [];
            $scope.getsavedstudentlist = [];
            $scope.studentdata = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("Collegestudentmentormapping/onchangeyear", data).then(function (promise) {

                if (promise !== null) {
                    $scope.courselist = promise.getcourse;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            });
        };

        $scope.getbranch = function () {

            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.HRME_Id = "";
            $scope.ACMS_Id = "";
            $scope.employeedetails = [];
            $scope.templist = [];
            $scope.getstudentlist = [];
            $scope.getsavedstudentlist = [];
            $scope.studentdata = false;
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("Collegestudentmentormapping/getbranch", data).then(function (promise) {

                if (promise !== null) {
                    $scope.branchlist = promise.getbranch;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            });
        };

        $scope.getsemester = function () {

            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.HRME_Id = "";
            $scope.employeedetails = [];
            $scope.templist = [];
            $scope.getstudentlist = [];
            $scope.getsavedstudentlist = [];
            $scope.studentdata = false;
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("Collegestudentmentormapping/getsemester", data).then(function (promise) {

                if (promise !== null) {
                    $scope.semesterlist = promise.getsemester;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            });
        };

        $scope.getsection = function () {
            $scope.ACMS_Id = "";
            $scope.HRME_Id = "";
            $scope.employeedetails = [];
            $scope.templist = [];
            $scope.getstudentlist = [];
            $scope.getsavedstudentlist = [];
            $scope.studentdata = false;
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("Collegestudentmentormapping/getsection", data).then(function (promise) {

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
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id
            };
            apiService.create("Collegestudentmentormapping/getemployee", data).then(function (promise) {

                if (promise !== null) {
                    $scope.employeedetails = promise.getemployeedetails;
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
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id
                };
                apiService.create("Collegestudentmentormapping/getstudentdata", data).then(function (promise) {

                    if (promise !== null) {
                        $scope.templist = [];

                        $scope.getstudentlist = promise.getstudentlist;
                        $scope.getsavedstudentlist = promise.getsavedstudentlist;

                        if ($scope.getstudentlist !== null) {

                            if ($scope.getstudentlist.length > 0) {
                                $scope.studentdata = true;
                                if ($scope.getsavedstudentlist !== null) {
                                    if ($scope.getsavedstudentlist.length > 0) {
                                        $scope.AMMEC_Id = $scope.getsavedstudentlist[0].ammeC_Id;

                                        angular.forEach($scope.getsavedstudentlist, function (dd) {

                                            angular.forEach($scope.getstudentlist, function (ddd) {

                                                if (dd.amcsT_Id === ddd.amcsT_Id) {
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
                        $scope.temparry.push({ AMCST_Id: dd.amcsT_Id, studentname: dd.studentname });
                    }
                });

                console.log($scope.temparry);


                var obj = {
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "AMMEC_Id": $scope.AMMEC_Id,
                    "CollegestudentmentormappingtempDTO": $scope.temparry
                };

                apiService.create("Collegestudentmentormapping/savedata", obj).then(function (promise) {
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
                { name: 'yearname', displayName: 'Academic Year' },
                { name: 'employeename', displayName: 'Employee Name' },
                { name: 'coursename', displayName: 'Course' },
                { name: 'branchname', displayName: 'Branch' },
                { name: 'semestername', displayName: 'Semester' },
                { name: 'sectioname', displayName: 'Section' },
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
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.cancel = function () {
            $state.reload();
        };



        $scope.viewrecordspopup = function (employee) {
            apiService.create("Collegestudentmentormapping/viewrecordspopup", employee).
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

                        apiService.create("Collegestudentmentormapping/Deletedata", user).
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