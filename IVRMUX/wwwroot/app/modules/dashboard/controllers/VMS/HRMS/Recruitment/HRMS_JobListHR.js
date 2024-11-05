(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsjobListHRController', vmsjobListHRController);

    vmsjobListHRController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'uiGridExporterService', 'uiGridExporterConstants'];
    function vmsjobListHRController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, uiGridExporterService, uiGridExporterConstants) {

        $scope.viewby = 5;
        $scope.currentPage = 4;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5; //Number of pager buttons to show
        $scope.jobedit = {};
        $scope.JobDetails = false;
        var crndate = new Date();
        $scope.todate = new Date();
        $scope.submitted = true;

        // Datatable display
        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmrfR_MRFNO', displayName: 'MRF#', enableHiding: false },
                { name: 'hrmP_Position', displayName: 'POSITION', enableHiding: false },
                { name: 'hrmrfR_JobLocation', displayName: 'LOCATION', enableHiding: false },
                { name: 'hrmP_Name', displayName: 'PRIORITY', enableHiding: false },
                { name: 'hrmrfR_NoofPosition', displayName: 'NO.OF POSITION', enableHiding: false },
                { name: 'userName', displayName: 'Created By', enableHiding: false },
                { name: 'hrmrfR_Status', displayName: 'STATUS', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true">Details</i></a>' +
                        '</div>'
                }
            ],
            exporterCsvFilename: 'MRFRequisitionList.csv',
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };
        $scope.exportCsv = function () {
            var grid = $scope.gridApi.grid;
            var rowTypes = uiGridExporterConstants.ALL;
            var colTypes = uiGridExporterConstants.ALL;
            uiGridExporterService.csvExport(grid, rowTypes, colTypes);
        };

        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;
        };

        $scope.pageChanged = function () {
            console.log('Page changed to: ' + $scope.currentPage);
        };

        $scope.setItemsPerPage = function (num) {
            $scope.itemsPerPage = num;
            $scope.currentPage = 1;
        };

        $scope.onLoadGetData = function () {
            $scope.JobDetails = false;
            var pageid = 2;
            apiService.getURI("JobListHRVMS/getalldetails", pageid).then(function (promise) {
                if (promise.vmsmrfList !== null && promise.vmsmrfList.length > 0) {
                    $scope.gridOptions.data = promise.vmsmrfList;
                }
            });
        };

        $scope.EditData = function (jobid) {
            $scope.jobedit = jobid.hrmrfR_Id;
            var pageid = $scope.jobedit;
            apiService.getURI("JobListHRVMS/editRecord", pageid).
                then(function (promise) {
                    $scope.JobDetails = true;
                    $scope.hrmrf = promise.vmsEditValue[0];
                    $scope.enable = promise.vmsEditValue[0].hrmrfR_MDFlag;
                    if (promise.vmsEditValue[0].hrmrfR_PositionFilled !== null) {
                        $scope.hrmrf.hrmrfR_PositionFilled = new Date(promise.vmsEditValue[0].hrmrfR_PositionFilled);
                    }
                    else {
                        $scope.hrmrf.hrmrfR_PositionFilled = crndate;
                    }
                });
        };

        $scope.cancel = function () {
            $scope.JobDetails = false;
            $scope.onLoadGetData();
        };

        $scope.addjob = function () {
            $state.go('app.vmsaddjob');
        };

        $scope.approvejob = function () {
            if ($scope.myForm.$valid) {
                $scope.hrmrf.hrmrfR_ManagerFlag = true;
                $scope.hrmrf.hrmrfR_HRFlag = true;
                $scope.hrmrf.hrmrfR_Status = "Approved";
                var data = $scope.hrmrf;
                apiService.create("AddJobVMS/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Submitted Successfully..");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }
                            $scope.cancel();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.rejectjob = function () {
            if ($scope.myForm.$valid) {
                $scope.hrmrf.hrmrfR_ManagerFlag = true;
                $scope.hrmrf.hrmrfR_HRFlag = false;
                $scope.hrmrf.hrmrfR_Status = "Pending";
                var data = $scope.hrmrf;
                apiService.create("AddJobVMS/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }

                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Submitted Successfully..");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            $scope.cancel();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.inprogress = function () {
            if ($scope.myForm.$valid) {
                $scope.hrmrf.hrmrfR_ManagerFlag = true;
                $scope.hrmrf.hrmrfR_HRFlag = false;
                $scope.hrmrf.hrmrfR_Status = "InProgress";
                var data = $scope.hrmrf;
                apiService.create("AddJobVMS/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Submitted Successfully..");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }
                            $scope.cancel();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }

})();