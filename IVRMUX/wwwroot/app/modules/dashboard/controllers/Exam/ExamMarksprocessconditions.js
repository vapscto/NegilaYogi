
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamamrksprocessconditionController', ExamamrksprocessconditionController)

    ExamamrksprocessconditionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter']
    function ExamamrksprocessconditionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter) {

        //$scope.SubWise_Selected_subexms_list = [];
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //TO  GEt The Values iN Grid
        $scope.select_cat = false;

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', width: '6%', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', width: '11%', displayName: 'Academic Year' },
                { name: 'emcA_CategoryName', width: '12%', displayName: 'Category Name' },
                { name: 'emE_ExamName', width: '15%', displayName: 'Exam Name' },
                { name: 'eycE_ExamStartDate', width: '12%', displayName: 'Exam Start Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'eycE_ExamEndDate', width: '12%', displayName: 'Exam End Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'eycE_MarksEntryLastDate', width: '15%', displayName: 'Marks Entry Last Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'eycE_MarksProcessLastDate', width: '15%', displayName: 'Marks Process Last Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'eycE_MarksPublishDate', width: '15%', displayName: 'Marks Publish Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' }
                //{
                //    field: 'id', name: '', width: '10%',
                //    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                //        '<div class="grid-action-cell">' +
                //        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;'
                //}
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;

            }

        };



        //========== Load Data 
        $scope.BindData = function () {
            apiService.getDATA("ExamMarksprocesscondition/Getdetails").
                then(function (promise) {
                    $scope.year_list = promise.yearlist;
                    $scope.gridOptions.data = promise.category_exams;
                });
        };


        $scope.valid_from_date = function (from_date) {
            if ($scope.ASMAY_Id !== "" && $scope.ASMAY_Id !== null && $scope.ASMAY_Id !== undefined) {
                $scope.EYCE_AttendanceToDate = "";
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.EYCE_AttendanceFromDate = "";
            }
        };

        $scope.valid_to_date = function (to_date) {
            if ($scope.EYCE_AttendanceFromDate !== "" && $scope.EYCE_AttendanceFromDate !== null && $scope.EYCE_AttendanceFromDate !== undefined) {
                // $scope.EYCE_AttendanceToDate = "";
            }
            else {
                swal("First Select Attendance From Date !!!");
                $scope.EYCE_AttendanceToDate = "";
            }
        };

        $scope.get_category = function (yr_id) {

            $scope.EYCE_ExamStartDate = "";
            $scope.EYCE_ExamEndDate = "";
            $scope.EYCE_MarksEntryLastDate = "";
            $scope.EYCE_MarksProcessLastDate = "";
            $scope.EYCE_MarksPublishDate = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ExamMarksprocesscondition/get_category", data).
                then(function (promise) {
                    $scope.category_list = promise.categorylist;

                    angular.forEach($scope.year_list, function (yea) {

                        if (parseInt(yea.asmaY_Id) === parseInt($scope.ASMAY_Id)) {
                            $scope.mindate = new Date(yea.asmaY_From_Date);
                            $scope.maxdt = new Date(yea.asmaY_To_Date);
                        }
                    });

                    if (promise.categorylist === null || promise.categorylist === '') {
                        swal("No Categories Are Mapped To Selected Academic Year");
                        $scope.ASMAY_Id = "";
                    }
                });
        };

        $scope.get_subjects = function (cat_id) {
            if ($scope.ASMAY_Id !== "" && $scope.ASMAY_Id !== null && $scope.ASMAY_Id !== undefined) {

                var data = {
                    "EYC_Id": $scope.EYC_Id
                };

                apiService.create("ExamMarksprocesscondition/get_subjects", data).
                    then(function (promise) {
                        $scope.subject_list = promise.subjectlist;
                        $scope.select_cat = true;
                        $scope.exam_list = promise.examlist;
                    });
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.EYC_Id = "";
            }
        };


        $scope.interacted = function (field) {
            return $scope.submitted;//|| field.$dirty
        };

        var temp_saved_subs = [];

        $scope.setexamenddate = function () {

            $scope.EYCE_ExamEndDate = "";
            $scope.EYCE_MarksEntryLastDate = "";
            $scope.EYCE_MarksProcessLastDate = "";
            $scope.EYCE_MarksPublishDate = "";

            $scope.examstartdate = new Date($scope.EYCE_ExamStartDate);
            $scope.exammindate = new Date(
                $scope.examstartdate.getFullYear(),
                $scope.examstartdate.getMonth(),
                $scope.examstartdate.getDate() + 1);
        };

        $scope.setentrylastdate = function () {
            $scope.EYCE_MarksEntryLastDate = "";
            $scope.EYCE_MarksProcessLastDate = "";
            $scope.EYCE_MarksPublishDate = "";

            $scope.examenddate = new Date($scope.EYCE_ExamEndDate);
            $scope.exammarksentry = new Date(
                $scope.examenddate.getFullYear(),
                $scope.examenddate.getMonth(),
                $scope.examenddate.getDate() + 1);
        };

        $scope.setprocesslastdate = function () {
            $scope.EYCE_MarksProcessLastDate = "";
            $scope.EYCE_MarksPublishDate = "";

            $scope.examprocesslast = new Date($scope.EYCE_MarksEntryLastDate);
            $scope.examprocesslastdate = new Date(
                $scope.examprocesslast.getFullYear(),
                $scope.examprocesslast.getMonth(),
                $scope.examprocesslast.getDate() + 1);
        };

        $scope.setpublishdate = function () {
            $scope.EYCE_MarksPublishDate = "";
            $scope.examsetpublish = new Date($scope.EYCE_MarksProcessLastDate);
            $scope.examsetpublishdated = new Date(
                $scope.examsetpublish.getFullYear(),
                $scope.examsetpublish.getMonth(),
                $scope.examsetpublish.getDate() + 1);
        };




        // to Edit Data
        $scope.getorgvalue = function (employee) {
            apiService.create("ExamMarksprocesscondition/editdetails", employee).
                then(function (promise) {
                    $scope.EYCE_Id = promise.edit_cat_exm[0].eycE_Id;
                    $scope.selected_exm_subjects = promise.edit_cat_exm_subs;
                    $scope.EYC_Id = promise.edit_cat_exm[0].eyC_Id;
                    $scope.temp_category = promise.edit_cat_exm[0].eyC_Id;

                    $scope.ASMAY_Id = promise.asmaY_Id;
                    if ($scope.ASMAY_Id !== "") {
                        $scope.get_category($scope.ASMAY_Id);
                    }
                    $scope.exam_list = promise.examlist;
                    $scope.EME_Id = $scope.exam_list[0].emE_Id;

                    if (promise.edit_cat_exm[0].eycE_MarksPublishDate !== null && promise.edit_cat_exm[0].eycE_MarksPublishDate !== undefined) {
                        $scope.EYCE_MarksPublishDate = new Date(promise.edit_cat_exm[0].eycE_MarksPublishDate);
                    }
                    if (promise.edit_cat_exm[0].eycE_ExamStartDate !== null && promise.edit_cat_exm[0].eycE_ExamStartDate !== undefined) {
                        $scope.EYCE_ExamStartDate = new Date(promise.edit_cat_exm[0].eycE_ExamStartDate);
                    }
                    if (promise.edit_cat_exm[0].eycE_ExamEndDate !== null && promise.edit_cat_exm[0].eycE_ExamEndDate !== undefined) {
                        $scope.EYCE_ExamEndDate = new Date(promise.edit_cat_exm[0].eycE_ExamEndDate);
                    }

                    if (promise.edit_cat_exm[0].eycE_MarksEntryLastDate !== null && promise.edit_cat_exm[0].eycE_MarksEntryLastDate !== undefined) {
                        $scope.EYCE_MarksEntryLastDate = new Date(promise.edit_cat_exm[0].eycE_MarksEntryLastDate);
                    }
                    if (promise.edit_cat_exm[0].eycE_MarksProcessLastDate !== null && promise.edit_cat_exm[0].eycE_MarksProcessLastDate !== undefined) {
                        $scope.EYCE_MarksProcessLastDate = new Date(promise.edit_cat_exm[0].eycE_MarksProcessLastDate);
                    }
                    if (promise.edit_cat_exm[0].eycE_MarksPublishDate !== null && promise.edit_cat_exm[0].eycE_MarksPublishDate !== undefined) {
                        $scope.EYCE_MarksPublishDate = new Date(promise.edit_cat_exm[0].eycE_MarksPublishDate);
                    }
                });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {

            //$scope.submitted = true;
            var EYCE_MarksPublishDate = $scope.EYCE_MarksPublishDate === null ? "" : $filter('date')($scope.EYCE_MarksPublishDate, "yyyy-MM-dd");
            var EYCE_ExamStartDate = $scope.EYCE_ExamStartDate === null ? "" : $filter('date')($scope.EYCE_ExamStartDate, "yyyy-MM-dd");
            var EYCE_ExamEndDate = $scope.EYCE_ExamEndDate === null ? "" : $filter('date')($scope.EYCE_ExamEndDate, "yyyy-MM-dd");
            var EYCE_MarksEntryLastDate = $scope.EYCE_MarksEntryLastDate === null ? "" : $filter('date')($scope.EYCE_MarksEntryLastDate, "yyyy-MM-dd");
            var EYCE_MarksProcessLastDate = $scope.EYCE_MarksProcessLastDate === null ? "" : $filter('date')($scope.EYCE_MarksProcessLastDate, "yyyy-MM-dd");

            if ($scope.myForm.$valid) {
                var data = {
                    "EYCE_Id": $scope.EYCE_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EYC_Id": $scope.EYC_Id,
                    "EME_Id": $scope.EME_Id,
                    "EMCA_Id": $scope.EMCA_Id,
                    "EYCE_MarksPublishDate": new Date($scope.EYCE_MarksPublishDate).toDateString(),
                    "EYCE_ExamStartDate": new Date($scope.EYCE_ExamStartDate).toDateString(),
                    "EYCE_ExamEndDate": new Date($scope.EYCE_ExamEndDate).toDateString(),
                    "EYCE_MarksEntryLastDate": new Date($scope.EYCE_MarksEntryLastDate).toDateString(),
                    "EYCE_MarksProcessLastDate": new Date($scope.EYCE_MarksProcessLastDate).toDateString(),

                };

                apiService.create("ExamMarksprocesscondition/savedetails", data).
                    then(function (promise) {

                        $scope.gridOptions.data = promise.category_exams;

                        if (promise.returnval === true) {
                            if (promise.eycE_Id === 0 || promise.eycE_Id < 0) {
                                swal('Record saved successfully');
                            }
                            else if (promise.eycE_Id > 0) {
                                swal('Record updated successfully');
                            }
                        }

                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.eycE_Id === 0 || promise.eycE_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.eycE_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $scope.BindData();
                        $scope.clear();
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        // Clear Function

        $scope.clear = function () {
            $state.reload();
        };

        // End Of Clear Function

        $scope.get_exm_details = function (obj) {
            var data = {
                "EME_Id": $scope.EME_Id,
                "EYC_Id": $scope.EYC_Id
            };

            apiService.create("ExamMarksprocesscondition/get_exm_details", data).then(function (promise) {

                if (promise.examlistdetails.length > 0) {
                    $scope.EYCE_Id = promise.examlistdetails[0].eycE_Id;
                    if (promise.examlistdetails[0].eycE_MarksPublishDate !== null && promise.examlistdetails[0].eycE_MarksPublishDate !== undefined) {
                        $scope.EYCE_MarksPublishDate = new Date(promise.examlistdetails[0].eycE_MarksPublishDate);
                    }
                    if (promise.examlistdetails[0].eycE_ExamStartDate !== null && promise.examlistdetails[0].eycE_ExamStartDate !== undefined) {
                        $scope.EYCE_ExamStartDate = new Date(promise.examlistdetails[0].eycE_ExamStartDate);
                    }
                    if (promise.examlistdetails[0].eycE_ExamEndDate !== null && promise.examlistdetails[0].eycE_ExamEndDate !== undefined) {
                        $scope.EYCE_ExamEndDate = new Date(promise.examlistdetails[0].eycE_ExamEndDate);
                    }

                    if (promise.examlistdetails[0].eycE_MarksEntryLastDate !== null && promise.examlistdetails[0].eycE_MarksEntryLastDate !== undefined) {
                        $scope.EYCE_MarksEntryLastDate = new Date(promise.examlistdetails[0].eycE_MarksEntryLastDate);
                    }
                    if (promise.examlistdetails[0].eycE_MarksProcessLastDate !== null && promise.examlistdetails[0].eycE_MarksProcessLastDate !== undefined) {
                        $scope.EYCE_MarksProcessLastDate = new Date(promise.examlistdetails[0].eycE_MarksProcessLastDate);
                    }
                    if (promise.examlistdetails[0].eycE_MarksPublishDate !== null && promise.examlistdetails[0].eycE_MarksPublishDate !== undefined) {
                        $scope.EYCE_MarksPublishDate = new Date(promise.examlistdetails[0].eycE_MarksPublishDate);
                    }
                }
            });
        };
    }

})();