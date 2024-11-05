(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsjobListController', vmsjobListController);

    vmsjobListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'uiGridExporterService', 'uiGridExporterConstants'];
    function vmsjobListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, uiGridExporterService, uiGridExporterConstants) {

        $scope.viewby = 5;
        $scope.currentPage = 4;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5;
        $scope.jobedit = {};
        $scope.Editable = false;

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
                { name: 'hrmrfR_Status', displayName: 'STATUS', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true">Edit</i></a>' +
                        '</div>'
                }
            ],
            exporterCsvFilename: 'JobList.csv',
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

        $scope.addjob = function () {
            $state.go('app.vmsaddjob');
            //$scope.mrfReq.hrmP_Id = "";
            //$scope.mrfReq.hrmlO_Id = "";
            //$scope.mrfReq.hrmD_Id = "";
            //$scope.mrfReq.hrmpR_Id = "";
            //$scope.mrfReq.hrmpT_Id = "";
            //$scope.mrfReq.hrmC_Id = "";
            //$scope.mrfReq.hrmrfR_MRFNO = "";
            //$scope.mrfReq.hrmrfR_NoofPosition = "";
            //$scope.mrfReq.hrmrfR_Skills = "";
            //$scope.mrfReq.hrmrfR_JobDesc = "";
            //$scope.mrfReq.hrmrfR_ExpFrom = "";
            //$scope.mrfReq.hrmrfR_ExpTo = "";
            //$scope.mrfReq.hrmrfR_Age = "";
            //$scope.mrfReq.ivrmmG_Id = "";
            //$scope.mrfReq.hrmrfR_Reason = "";
            //$scope.mrfReq.hrmrfR_Remark = "";
            //$scope.mrfReq.hrmrfR_Attachment = "";
            //$scope.mrfReq.hrmrfR_WrittenTestFlg = false;
            //$scope.mrfReq.hrmrfR_OnlineTestFlg = false;
            //$scope.mrfReq.hrmrfR_TechnicalInterviewFlg = false;
            //$scope.Editable = true;
        };

        $scope.onLoadGetData = function () {
            $scope.Editable = false;
            var pageid = 2;
            apiService.getURI("JobListVMS/getalldetails", pageid).then(function (promise) {
                if (promise.vmsmrfList !== null && promise.vmsmrfList.length > 0) {
                    $scope.gridOptions.data = promise.vmsmrfList;

                    if (promise.masterPositionList !== null && promise.masterPositionList.length > 0) {
                        $scope.masterPositionList = promise.masterPositionList;
                    }

                    if (promise.masterLocation !== null && promise.masterLocation.length > 0) {
                        $scope.masterLocation = promise.masterLocation;
                    }

                    if (promise.masterDepartmentList !== null && promise.masterDepartmentList.length > 0) {
                        $scope.masterDepartmentList = promise.masterDepartmentList;
                    }

                    if (promise.masterPriorityList !== null && promise.masterPriorityList.length > 0) {
                        $scope.masterPriorityList = promise.masterPriorityList;
                    }

                    if (promise.masterPosTypeList !== null && promise.masterPosTypeList.length > 0) {
                        $scope.masterPosTypeList = promise.masterPosTypeList;
                    }

                    if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                        $scope.masterQualification = promise.masterQualification;
                    }

                    if (promise.masterGender !== null && promise.masterGender.length > 0) {
                        $scope.masterGender = promise.masterGender;
                    }

                    $scope.mrfReq = promise.vmsmrfList[0];
                }
            });
        };

        $scope.EditData = function (jobid) {
            $scope.jobedit = jobid.hrmrfR_Id;
            var pageid = $scope.jobedit;
            apiService.getURI("JobListVMS/editRecord", pageid).
                then(function (promise) {
                    $scope.mrfReq = promise.vmsEditValue[0];
                    $scope.clientlist = promise.clientlist;
                    $scope.Editable = true;
                    //$state.go('app.vmsaddjob');

                    var clientenable = false;
                    var locationenable = false;
                    if ($scope.mrfReq.hrmrfR_JobLocation == "HO") {
                        clientenable = true;
                    }
                    else if (clientenable == false) {
                        angular.forEach($scope.clientlist, function (itm) {
                            if (itm.ismmclT_ClientName == $scope.mrfReq.hrmrfR_JobLocation) {
                                clientenable = true;
                                $scope.mrfReq.clientlocation = promise.vmsEditValue[0].hrmrfR_JobLocation;
                                $scope.mrfReq.hrmrfR_JobLocation = "Client";
                            }
                        });
                    }

                    if (clientenable == false) { locationenable = true; }
                    if (locationenable == true) {
                        $scope.mrfReq.dynamicloc = promise.vmsEditValue[0].hrmrfR_JobLocation;
                        $scope.mrfReq.hrmrfR_JobLocation = "Location";
                    }
                });
        };

        $scope.cancel = function () {
            $scope.Editable = false;
            $scope.onLoadGetData();
        };

        $scope.savejob = function () {
            if ($scope.myForm.$valid) {
                $scope.mrfReq.hrmrfR_ManagerFlag = true;
                $scope.mrfReq.hrmrfR_Status = "Pending";
                var data = $scope.mrfReq;
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
                                swal("Record Updated Successfully..");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            $scope.cancel();
                        }
                    });
            }
        };
    }

})();