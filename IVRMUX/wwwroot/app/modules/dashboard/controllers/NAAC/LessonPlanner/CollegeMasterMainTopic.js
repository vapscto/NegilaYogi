(function () {
    'use strict';
    angular.module('app').controller('CollegeMasterMainTopicController', CollegeMasterMainTopicController)
    CollegeMasterMainTopicController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function CollegeMasterMainTopicController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.editdata = false;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            $scope.ISMS_IdNew = "";
            $scope.getsubjecttopicdetails = [];
            $scope.editdata = false;
            var id = 2;
            apiService.getURI("MasterSchoolTopic/getcollegedetails", id).then(function (promise) {
                $scope.gridOptions.data = promise.getdetails;
                $scope.grouptypeListOrder = promise.getdetails;
                $scope.masterunitdetails = promise.getunitlist;
                $scope.masteryear = promise.getyear;
                $scope.masteryearnew = promise.getyear;
            });
        };

        //Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>', width: 100 },
                { name: 'asmaY_Year', displayName: 'Academic Year', width: 200, cellClass: 'textleft' },
                { name: 'amcO_CourseName', displayName: 'Course', width: 200, cellClass: 'textleft' },
                { name: 'amB_BranchName', displayName: 'Branch', width: 200, cellClass: 'textleft' },
                { name: 'amsE_SEMName', displayName: 'Semester', width: 200, cellClass: 'textleft' },
                { name: 'ismS_SubjectName', displayName: 'Subject Name', width: 200, cellClass: 'textleft', headerAttributes: { "class": "wrap-header" } },
                { name: 'lpmU_UnitName', displayName: 'Unit Name', width: 200, cellClass: 'textleft' },
                { name: 'lpmmtC_TopicName', displayName: 'Topic Name', width: 200, cellClass: 'textleft' },
                { name: 'lpmmtC_TopicDescription', displayName: 'Topic Description', width: 200, cellClass: 'textleft' },
                { name: 'lpmmtC_TotalPeriods', displayName: 'No of Periods', width: 100 },
                { name: 'lpmmtC_TotalHrs', displayName: 'No of Hours', width: 200 },
                { name: 'lpmmtC_Order', displayName: 'Order', type: 'number', width: 100 },
                {
                    field: 'id', name: '', width: 100,
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +

                        '<a ng-if="row.entity.lpmmtC_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.lpmmtC_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };

        $scope.onchangecollegeyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.subjectlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("MasterSchoolTopic/onchangecollegeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastercourse = promise.getcourse;
                }
            });
        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.subjectlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("MasterSchoolTopic/onchangecourse", data).then(function (promise) {
                if (promise !== null) {
                    $scope.masterbranch = promise.getbranch;

                }
            });
        };

        $scope.onchangebranch = function () {
            $scope.AMSE_Id = "";
            $scope.subjectlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("MasterSchoolTopic/onchangebranch", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersemester = promise.getsemester;
                }
            });
        };

        $scope.onchangesemester = function () {
            $scope.subjectlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("MasterSchoolTopic/onchangesemester", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subjectlist = promise.getsubject;
                    angular.forEach($scope.subjectlist, function (dd) {
                        dd.ismS_SubjectName = dd.ismS_SubjectName + " : " + dd.ismS_SubjectCode;
                    });
                }
            });
        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "LPMMTC_Id": $scope.LPMMTC_Id,
                    "LPMMTC_TopicName": $scope.LPMMTC_TopicName,
                    "LPMMTC_TopicDescription": $scope.LPMMTC_TopicDescription,
                    "LPMMTC_TotalPeriods": $scope.LPMMTC_TotalPeriods,
                    "LPMMTC_TotalHrs": $scope.LPMMTC_TotalHrs,
                    "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                    "LPMU_Id": $scope.LPMU_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id
                };

                apiService.create("MasterSchoolTopic/savecollegedetails", data).then(function (promise) {
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
            $scope.editdata = true;
            apiService.create("MasterSchoolTopic/editcollegedeatils", data).then(function (promise) {
                $scope.mastercourse = promise.getcourse;
                $scope.masterbranch = promise.getbranch;
                $scope.mastersemester = promise.getsemester;
                $scope.LPMMTC_Id = promise.editdetails[0].lpmmtC_Id;
                $scope.LPMMTC_TopicName = promise.editdetails[0].lpmmtC_TopicName;
                $scope.ASMAY_Id = promise.editdetails[0].asmaY_Id;
                $scope.AMCO_Id = promise.editdetails[0].amcO_Id;
                $scope.AMB_Id = promise.editdetails[0].amB_Id;
                $scope.AMSE_Id = promise.editdetails[0].amsE_Id;
                $scope.LPMMTC_TopicDescription = promise.editdetails[0].lpmmtC_TopicDescription;
                $scope.LPMMTC_TotalPeriods = promise.editdetails[0].lpmmtC_TotalPeriods;
                $scope.LPMMTC_TotalHrs = promise.editdetails[0].lpmmtC_TotalHrs;
                $scope.ISMS_Id = promise.editdetails[0];
                $scope.LPMU_Id = promise.editdetails[0].lpmU_Id;
            });
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmmtC_ActiveFlg === true) {
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

                        apiService.create("MasterSchoolTopic/collegedeactivate", deactiveRecord).then(function (promise) {
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


        $scope.onchangecollegeyearnew = function () {
            $scope.AMCO_IdNew = "";
            $scope.AMB_IdNew = "";
            $scope.AMSE_IdNew = "";
            $scope.subjectlistnew = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew
            };
            apiService.create("MasterSchoolTopic/onchangecollegeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastercoursenew = promise.getcourse;
                }
            });
        };

        $scope.onchangecoursenew = function () {
            $scope.AMB_IdNew = "";
            $scope.AMSE_IdNew = "";
            $scope.subjectlistnew = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "AMCO_Id": $scope.AMCO_IdNew
            };
            apiService.create("MasterSchoolTopic/onchangecourse", data).then(function (promise) {
                if (promise !== null) {
                    $scope.masterbranchnew = promise.getbranch;

                }
            });
        };

        $scope.onchangebranchnew = function () {
            $scope.AMSE_IdNew = "";
            $scope.subjectlistnew = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "AMCO_Id": $scope.AMCO_IdNew,
                "AMB_Id": $scope.AMB_IdNew
            };
            apiService.create("MasterSchoolTopic/onchangebranch", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersemesternew = promise.getsemester;
                }
            });
        };

        $scope.onchangesemesternew = function () {
            $scope.subjectlistnew = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "AMCO_Id": $scope.AMCO_IdNew,
                "AMB_Id": $scope.AMB_IdNew,
                "AMSE_Id": $scope.AMSE_IdNew
            };
            apiService.create("MasterSchoolTopic/onchangesemester", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subjectlistnew = promise.getsubject;
                    angular.forEach($scope.subjectlistnew, function (dd) {
                        dd.ismS_SubjectName = dd.ismS_SubjectName + " : " + dd.ismS_SubjectCode;
                    });
                }
            });
        };


        $scope.gettopicdetails = function () {
            $scope.getsubjecttopicdetails = [];
            $scope.btn = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "AMCO_Id": $scope.AMCO_IdNew,
                "AMB_Id": $scope.AMB_IdNew,
                "AMSE_Id": $scope.AMSE_IdNew,
                "ISMS_Id": $scope.ISMS_IdNew.ismS_Id
            };

            apiService.create("MasterSchoolTopic/getcollegetopicdetails", data).then(function (promise) {
                if (promise.getsubjecttopicdetails !== null) {
                    if (promise.getsubjecttopicdetails.length > 0) {
                        $scope.getsubjecttopicdetails = promise.getsubjecttopicdetails;
                        $scope.btn = true;
                    } else {
                        swal("No Data Found");
                    }

                } else {
                    swal("No Data Found");
                }
            });
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.getsubjecttopicdetails) {
                    $scope.getsubjecttopicdetails[index].lpmmT_Order = Number(index) + 1;
                }
            }
        };

        $scope.getOrder = function (orderarray) {
            var data = {
                MastercollegeTopicOrderDTO: orderarray
            };

            apiService.create("MasterSchoolTopic/validatecollegeordernumber", data).
                then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Order Updated Successfully");
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
    }
})();