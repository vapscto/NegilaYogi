(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsjobPostingListController', vmsjobPostingListController);

    vmsjobPostingListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'uiGridExporterService', 'uiGridExporterConstants'];
    function vmsjobPostingListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, uiGridExporterService, uiGridExporterConstants) {

        $scope.viewby = 5;
        $scope.currentPage = 4;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5;
        $scope.jobedit = {};
        $scope.Editable = false;
        $scope.submitted = true;

        // Datatable display
        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmrfR_MRFNO', displayName: 'MRF No', enableHiding: false },
                { name: 'hrmP_Position', displayName: 'Position', enableHiding: false },
                { name: 'hrmrfR_JobLocation', displayName: 'Location', enableHiding: false },
                { name: 'hrmP_Name', displayName: 'Priority', enableHiding: false },
                { name: 'hrmrfR_NoofPosition', displayName: 'No.of Position', enableHiding: false },
                { name: 'hrmrfR_Status', displayName: 'Status', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true">Edit</i></a>' +
                        '</div>'
                }
            ],
            exporterCsvFilename: 'JobPostingList.csv',
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
        };

        $scope.onLoadGetData = function () {
            $scope.Editable = false;
            var pageid = 2;
            apiService.getURI("JobPostingListVMS/getalldetails", pageid).then(function (promise) {
                if (promise.vmsmrfList !== null && promise.vmsmrfList.length > 0) {
                    $scope.gridOptions.data = promise.vmsmrfList;

                    if (promise.masterPositionList !== null && promise.masterPositionList.length > 0) {
                        $scope.masterPositionList = promise.masterPositionList;
                    }

                    if (promise.clientlist !== null && promise.clientlist.length > 0) {
                        $scope.clientlist = promise.clientlist;
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
            apiService.getURI("JobPostingListVMS/editRecord", pageid).
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

                if ($scope.mrfReq.hrmrfR_JobLocation == "Location") {
                    $scope.mrfReq.hrmrfR_JobLocation = $scope.mrfReq.dynamicloc;
                }
                if ($scope.mrfReq.hrmrfR_JobLocation === "Client") {
                    $scope.mrfReq.hrmrfR_JobLocation = $scope.mrfReq.clientlocation;
                }

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
            else {
                $scope.submitted = true;
            }
        };

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocument(document);
                }
                else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocument(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        data.hrmrfR_Attachment = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
    }

})();