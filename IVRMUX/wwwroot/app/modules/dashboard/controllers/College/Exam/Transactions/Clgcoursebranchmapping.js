(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgcoursebranchmappingController', ClgcoursebranchmappingController)

    ClgcoursebranchmappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function ClgcoursebranchmappingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
        $scope.editEmployee = {};

        //TO  GEt The Values iN Grid
        $scope.loadData = function () {
            apiService.getDATA("Clgcoursebranchmapping/getalldetails").
                then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 5;
                    $scope.course_list = promise.courseslist;
                    $scope.subjectschema_list = promise.subjectshemalist;
                    $scope.subjectgrp_list = promise.subjectgrplist;
                    $scope.branch_list = promise.branchlist;
                    $scope.schmetype_list = promise.schmetypelist;
                    $scope.semisters_list = promise.semisters;
                    $scope.gridOptions.data = promise.alllist;
                });
        };

        $scope.getbranch = function () {

            $scope.AMB_Id = "";

            var data = {
                "AMCO_Id": $scope.AMCO_Id
            };

            apiService.create("Clgcoursebranchmapping/getbranch", data).then(function (promise) {
                $scope.branch_list = promise.branchlist;
            });
        };




        $scope.savecategorycoursebranchmap = function () {
            var selected_semisters = [];
            angular.forEach($scope.semisters_list, function (stu3) {
                if (stu3.amsE_Id1 === true) {
                    selected_semisters.push(stu3);
                }
            });
            var data = {
                "ECYS_Id": $scope.ECYS_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "EMG_Id": $scope.EMG_Id,
                "ACST_Id": $scope.ACST_Id,
                selected_semis: selected_semisters,
                selected_subgrps: $scope.subject_groups
            };

            apiService.create("Clgcoursebranchmapping/savedetail2", data).
                then(function (promise) {
                    $scope.gridOptions.data = promise.alllist;
                    if (promise.returnval === true) {
                        if (promise.ecyS_Id === 0 || promise.ecyS_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ecyS_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    $scope.loadData();
                    $scope.clear2();
                });
        };

        $scope.get_subjects = function () {
            if ($scope.EMG_Id !== "" && $scope.EMG_Id !== undefined) {
                var data = {
                    "EMG_Id": $scope.EMG_Id
                };
                apiService.create("Clgcoursebranchmapping/get_subjects", data).
                    then(function (promise) {
                        $scope.subject_groups = promise.subjectgroups;
                    });
            }
            else {
                swal("Please Select Group First !!!");
            }
        };

        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'amcO_CourseName', displayName: 'Course' },
                { name: 'amB_BranchName', displayName: 'Branch' },
                { name: 'acsS_SchmeName', displayName: 'Scheme' },
                { name: 'acsT_SchmeType', displayName: 'Scheme Type' },
                { name: 'amsE_SEMName', displayName: 'Semester' },
                { name: 'subgroupname', displayName: 'Subject Group' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ecyS_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ecyS_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.viewrecordspopup = function (employee, SweetAlert) {
            apiService.create("Clgcoursebranchmapping/getalldetailsviewrecords", employee).
                then(function (promise) {
                    $scope.amcO_CourseName = promise.viewrecords[0].amcO_CourseName;
                    $scope.amB_BranchName = promise.viewrecords[0].amB_BranchName;
                    $scope.acsS_SchmeName = promise.viewrecords[0].acsS_SchmeName;
                    $scope.acsT_SchmeType = promise.viewrecords[0].acsT_SchmeType;
                    $scope.amsE_SEMName = promise.viewrecords[0].amsE_SEMName;
                    $scope.viewrecordspopupdisplay = promise.viewrecords;
                });
        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };
        $scope.clear2 = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.ACSS_Id = "";
            $scope.EMG_Id = "";
            $scope.ACST_Id = "";
            $scope.ECYS_Id = 0;
            $scope.subject_groups = "";
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.gridApi2.grid.clearAllFilters();
            $scope.search = "";
        };
        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            if (employee.ecyS_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Active";
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

                        apiService.create("Clgcoursebranchmapping/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.loadData();
                            });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.semisters_list.some(function (options) {
                return options.amsE_Id1;
            });
        };

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ecyS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Clgcoursebranchmapping/editdeatils", pageid).
                then(function (promise) {
                    $scope.ECYS_Id = promise.editlist[0].ecyS_Id;
                    $scope.AMCO_Id = promise.editlist[0].amcO_Id;
                    $scope.ACSS_Id = promise.editlist[0].acsS_Id;
                    $scope.EMG_Id = promise.emG_Id;
                    $scope.AMB_Id = promise.editlist[0].amB_Id;
                    $scope.ACST_Id = promise.editlist[0].acsT_Id;
                    $scope.subject_groups = promise.editlist2;

                    angular.forEach($scope.semisters_list, function (stu4) {
                        stu4.amsE_Id1 = false;
                    });

                    angular.forEach($scope.semisters_list, function (stu3) {
                        if (stu3.amsE_Id == promise.editlist[0].amsE_Id) {
                            stu3.amsE_Id1 = true;
                        }
                    });
                });
        };
    }

})();