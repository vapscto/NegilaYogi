
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamPassFailConditionController', ExamPassFailConditionController)

    ExamPassFailConditionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter']
    function ExamPassFailConditionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter) {

        //$scope.SubWise_Selected_subexms_list = [];
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //TO  GEt The Values iN Grid

        $scope.d_perstatus = true;

        $scope.BindData = function () {
            apiService.getDATA("ExamPassFailCondition/Getdetails").
                then(function (promise) {
                    $scope.year_list = promise.yearlist;
                    $scope.gridOptions.data = promise.passfailrank_list;
                    $scope.examcondition_list = promise.examconditionlist;
                });
        };

        //Grid view
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 10,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', width: '6%', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', width: '9%', displayName: 'Academic Year' },
                { name: 'emE_ExamName', width: '9%', displayName: 'Exam Type' },
                { name: 'epfrC_ExamFlag', width: '9%', displayName: 'Exam Flag' },
                { name: 'emcA_CategoryName', width: '9%', displayName: 'Exam Category' },
                { name: 'ecM_ConditionName', width: '11%', displayName: 'Exam Condition' },
                { name: 'epfrC_From', displayName: 'From Number' },
                { name: 'epfrC_To', displayName: 'To Number' }, {
                    name: 'epfrC_RankFlag', width: '9%', displayName: 'Rank', cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.epfrC_RankFlag == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.epfrC_RankFlag == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                { name: 'epfrC_PassFailFlag', width: '9%', displayName: 'Pass or Fail' },
                { name: 'epfrC_OverallPercentage', width: '9%', displayName: 'Over All Percentage' },
                {
                    field: 'id', name: '', width: '10%',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.epfrC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.epfrC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        //Get Catagory -  Year Change
        $scope.get_category = function (yr_id) {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("ExamPassFailCondition/get_category", data).
                then(function (promise) {

                    $scope.category_list = promise.categorylist;

                    if (promise.categorylist === "" || promise.categorylist === null) {
                        swal("No Categories Are Mapped To Selected Academic Year");
                    }
                });
        };

        // get Exam - catagory change
        $scope.get_subjects = function () {

            if ($scope.ASMAY_Id !== "" && $scope.ASMAY_Id !== null && $scope.ASMAY_Id !== undefined) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EMCA_Id": $scope.EMCA_Id
                };


                apiService.create("ExamPassFailCondition/get_subjects", data).
                    then(function (promise) {
                        //  $scope.subject_list = promise.subjectlist;
                        $scope.exam_list = promise.examlist;                       
                        if (promise.examlist === null || promise.examlist === "") {
                            swal("All Exams are Mapped To Selected Category !!!");
                            $scope.EMCA_Id = "";
                        }
                    });
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.EMCA_Id = "";
            }
        };

        // get Exam condition - Exam change
        $scope.get_examcondition = function () {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EME_Id": $scope.EME_Id
            };
            apiService.create("ExamPassFailCondition/get_examcondition", data).
                then(function (promise) {
                    $scope.examcondition_list = promise.examconditionlist;
                });
        };

        //Exam condition change 
        $scope.exm_conditionchange = function () {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ECM_Id": $scope.ECM_Id
            };

            apiService.create("ExamPassFailCondition/get_condition", data).
                then(function (promise) {
                    $scope.condition_type = promise.conditiontype;
                    if ($scope.condition_type[0].ecM_ConditionFlag === "PO" || $scope.condition_type[0].ecM_ConditionFlag === "PF") {
                        $scope.d_perstatus = false;
                    }
                    else {
                        $scope.d_perstatus = true;
                    }
                });
        };

        //Save 
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    "EPFRC_From": $scope.fromno,
                    "EPFRC_PassFailFlag": $scope.rdo_passfail,
                    "EPFRC_ExamFlag": $scope.rdo_ipe,
                    "EPFRC_Percentage": $scope.perc,
                    "ECM_Id": $scope.ECM_Id,
                    "EMCA_Id": $scope.EMCA_Id,
                    "EPFRC_Condition": $scope.ECM_Id,
                    "EPFRC_To": $scope.tono,
                    "EPFRC_RankFlag": $scope.rdo_rank,
                    "EPFRC_Id": $scope.EPFRC_Id,
                    "EPFRC_OverallPercentage": $scope.oppercentage
                };

                apiService.create("ExamPassFailCondition/savedata", data).then(function (promise) {

                    if (promise.returnval === true) {
                        swal("Record Saved/Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval === false) {
                        swal("Failed to Save/Update");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //Edit
        $scope.getorgvalue = function (EditRecord) {
            var ID = EditRecord.epfrC_Id;
            apiService.getURI("ExamPassFailCondition/editdetails", ID).
                then(function (promise) {
                    if (promise.editlist.length > 0) {
                        $scope.editlist = promise.editlist;
                        $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;
                        $scope.EME_Id = promise.editlist[0].emE_Id;
                        $scope.fromno = promise.editlist[0].epfrC_From;
                        $scope.rdo_passfail = promise.editlist[0].epfrC_PassFailFlag;
                        $scope.rdo_ipe = promise.editlist[0].epfrC_ExamFlag;
                        $scope.ECM_Id = promise.editlist[0].ecM_Id;
                        $scope.EMCA_Id = promise.editlist[0].emcA_Id;
                        $scope.tono = promise.editlist[0].epfrC_To;
                        $scope.rdo_rank = promise.editlist[0].epfrC_RankFlag;
                        $scope.oppercentage = promise.editlist[0].epfrC_OverallPercentage;
                        $scope.perc = promise.editlist[0].epfrC_Percentage;
                        $scope.EPFRC_Id = promise.editlist[0].epfrC_Id;
                        $scope.get_category($scope.ASMAY_Id);
                        $scope.get_subjects($scope.EMCA_Id);
                        $scope.get_examcondition($scope.EME_Id);
                    } else {
                        swal('No Record Found');
                    }
                });
        };

        // Active Deactive

        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            if (employee.epfrC_ActiveFlag === true) {

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

                        apiService.create("ExamPassFailCondition/deactive", employee).
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
                                $scope.clear();
                                $scope.BindData();
                            });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        $scope.isOptionsRequired = function () {
            return !$scope.exam_list.some(function (options) {
                return options.checked;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;//|| field.$dirty
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.clear = function () {

            $state.reload();
        };
    }

})();