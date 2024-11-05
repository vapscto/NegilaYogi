(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterCourseController', masterCourseController)

    masterCourseController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterCourseController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.Course = {};



        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                {
                    name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>'
                },
                {
                    name: 'hrmC_QulaificationName', displayName: 'Qualification Name', enableHiding: false
                },
                {
                    name: 'hrmC_QualificationDesc', displayName: 'Qualification Description', enableHiding: false
                },
                {
                    name: 'hrmC_DefaultQualFag', displayName: 'Default Qualification', enableHiding: false, cellTemplate:
                        '<div>' +
                        '<span ng-if="row.entity.hrmC_DefaultQualFag === false"> No</span>' +
                        '<span ng-if="row.entity.hrmC_DefaultQualFag === true">Yes<span>' +
                        '</div>'
                },
                {
                    name: 'hrmC_SpecialisationFlag', displayName: 'Specialization', enableHiding: false, cellTemplate:
                        '<div>' +
                        '<span ng-if="row.entity.hrmC_SpecialisationFlag === false"> No</span>' +
                        '<span ng-if="row.entity.hrmC_SpecialisationFlag === true">Yes<span>' +
                        '</div>'
                },
                {
                    name: 'hrmC_Order', displayName: 'Order', enableHiding: false
                },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmC_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };
        // Get form Details at onload 
        $scope.OnloadCourseData = function () {

            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getURI("MasterCourse/getalldetails", pageid).then(function (promise) {

                if (promise.courseList !== null && promise.courseList.length > 0) {
                    $scope.gridOptions.data = promise.courseList;
                    $scope.gridOptions = promise.courseList;
                    $scope.grouptypeListOrder = promise.courseList;
                }
            })
        }
        //check order exist or not at onchange?
        //Added by Ramesh
        //$scope.Onchangeordernumber = function () {
        //    if ($scope.Course.HRMC_Order != null && $scope.Course.HRMC_Order != "") {
        //           var data = {
        //               "HRMC_Order": $scope.Course.HRMC_Order,
        //               "HRMC_Id": $scope.Course.HRMC_Id
        //           }
        //           apiService.create("MasterCourse/validateordernumber", data).then(function (promise) {
        //               if (promise.retrunMsg != "" && promise.retrunMsg != undefined) {
        //   				$scope.Course.HRMC_Order="";
        //                   swal("Order is already exist..!!");
        //                   return;
        //               }
        //           });

        //       }

        //   }


        // clear form data
        $scope.cancel = function () {
            $scope.Course = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();

        }
        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = $scope.Course;
                apiService.create("MasterCourse/", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "AllDuplicate") {
                                swal("Record already exist..!!");
                                return;
                            } else if (promise.retrunMsg == "Duplicate") {
                                swal("Qualification Name already exist..!!");
                                return;
                            }

                            else if (promise.retrunMsg == "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg == "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg == "Update") {
                                swal("Record Updated Successfully..");
                                $state.reload();
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            if (promise.courseList !== null && promise.courseList.length > 0) {

                                $scope.gridOptions.data = promise.courseList;
                                $scope.grouptypeListOrder = promise.courseList;
                            }
                            $scope.cancel();
                        }
                    })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        // Edit Single Record
        $scope.EditData = function (record) {

            var id = record.hrmC_Id;
            apiService.getURI("MasterCourse/editRecord", id).
                then(function (promise) {

                    if (promise.courseList != null && promise.courseList.length > 0) {
                        $scope.Course = promise.courseList[0];
                    }

                })
        }
        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            if (data.hrmC_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("MasterCourse/ActiveDeactiveRecord", data.hrmC_Id).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                        $state.reload();
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }

                                    if (promise.courseList !== null && promise.courseList.length > 0) {

                                        $scope.gridOptions = promise.courseList;
                                    }
                                    $scope.OnloadCourseData();
                                }

                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }

            );
        }
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

            angular.forEach(orderarray, function (value, key) {
                if (value.hrmC_Id !== 0) {
                    orderarray[key].hrmC_Order = key + 1;
                }
            });

            var data = {
                CourseDTO: orderarray,
            }
            apiService.create("MasterCourse/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.OnloadCourseData();

                    }
                });
        }
    }

})();