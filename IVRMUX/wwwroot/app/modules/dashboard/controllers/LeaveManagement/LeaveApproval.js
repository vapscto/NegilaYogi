
(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveApprovalController', LeaveApprovalController)

    LeaveApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', '$sce']
    function LeaveApprovalController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, $sce) {

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.currentPage = 1;
        $scope.coptyright = copty;

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrelT_Status)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

        //$scope.gridleavestatus = {
        //    enableCellEditOnFocus: true,
        //    enableRowSelection: true,
        //    enableSelectAll: true,
        //    enableFiltering: true,
        //    enableCellEdit: false,
        //    paginationPageSizes: [5, 10, 15],
        //    paginationPageSize: 10,

        //    columnDefs: [
        //        { name: 'HRELAP_ApplicationID', displayName: 'App. ID', enableHiding: false},
        //        { name: 'HRME_EmployeeFirstName', displayName: 'Name', enableHiding: false},
        //        { name: 'HRML_LeaveName', displayName: 'Leave Name', enableHiding: false },
        //        { name: 'HRELAP_FromDate', displayName: 'From Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy"', enableHiding: false },
        //        { name: 'HRELAP_ToDate', displayName: 'To Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy"', enableHiding: false },
        //        { name: 'HRELAP_TotalDays', displayName: 'No. of Days', enableHiding: false },
        //        { name: 'HRELAP_ApplicationDate', displayName: 'Applied Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy HH:mm"', enableHiding: false },
        //        { name: 'HRELAP_LeaveReason', displayName: 'Reason', enableHiding: false },
        //        { name: 'HRELAP_SupportingDocument', displayName: 'Document', enableCellEdit: false, cellTemplate:
        //                '<div class="grid-action-cell">' +
        //            '<a href="javascript:void(0)" ng-if="row.entity.HRELAP_SupportingDocument != 0" ng-model="row.entity.HRELAP_SupportingDocument" ng-click="grid.appScope.viewdocument(row.entity);" title="View Document"> <i class="fa fa-eye text-blue"></i></a>' + 
        //            '<a href="javascript:void(0)" ng-if="row.entity.HRELAP_SupportingDocument == 0" ng-model="row.entity.HRELAP_SupportingDocument" ng- title="No Document"> <i class="fa fa-eye-slash text-red"></i></a>' +
        //                '</div>'
        //        }
        //    ]
        //};



        $scope.toggleAll = function () {
            var checkStatus = $scope.all;
            angular.forEach($scope.gridleavestatus, function (itm) {
                itm.selected = checkStatus;
            });
        }



        $scope.isOptionsRequired = function () {
            return !$scope.gridleavestatus.some(function (options) {
                return options.selected;
            });
        };

        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("LeaveApproval/getApprovalStatus", id).
                then(function (promise) {
                    $scope.gridleavestatus = promise.get_leavestatus;
                    $scope.temp_leave = promise.get_leavestatus;
                    $scope.activityIds = promise.activityIds;
                    if ($scope.count == 0) {
                        swal("Data not Found !!");
                        $scope.ckdept = false;
                    }
                });
        };

        //$scope.gridleavestatus.onRegisterApi = function (gridApi) {
        //    $scope.gridApi = gridApi;

        //    gridApi.selection.on.rowSelectionChanged($scope, function (row) {
        //        $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();
        //    });
        //    gridApi.selection.on.rowSelectionChangedBatch($scope, function (row) {
        //        $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();
        //    });
        //};

        $scope.get_status = function (status) {
            $scope.selected_Inst = [];
            if ($scope.myForm.$valid) {

                angular.forEach($scope.gridleavestatus, function (ty) {
                    if (ty.selected === true) {
                        $scope.selected_Inst.push({ HRELAP_Id: ty.HRELAP_Id });
                    }
                })

                var data = {
                    get_leave_status: $scope.selected_Inst,
                    "HRELAPA_Remarks": $scope.remarkstxta,
                    HRELT_Status: status
                };
                apiService.create("LeaveApproval/get_status", data).
                    then(function (promise) {
                        $scope.activityIds = promise.activityIds;

                        if (promise.activityIds.length > 0 && status == "Approved") {
                            swal("Record Approved Successfully");
                            $state.reload();
                        }
                        else if (promise.activityIds.length > 0 && status == "Rejected") {
                            swal("Record Rejected Successfully");
                            $state.reload();
                        } else {
                            swal("Record Not " + status + " Successfully");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        //$scope.reject_status = function () {
        //    if ($scope.myForm.$valid) {
        //        var data = {
        //            get_leave_status: $scope.gridApi.selection.getSelectedRows(),
        //            "HRELAPA_Remarks": $scope.remarkstxta
        //        };
        //        apiService.create("LeaveApproval/reject_status", data).
        //            then(function (promise) {
        //                $scope.gridleavestatus.data = promise.get_leavestatus;
        //                swal("Rejected...");
        //                $state.reload();
        //            });
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }
        //};

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //$scope.viewdocument = function (obj) {
        //    $scope.hrelaP_SupportingDocument = obj.hrelaP_SupportingDocument;
        //    var img = $scope.hrelaP_SupportingDocument;
        //    if (img != null) {
        //        var imagarr = img.split('.');
        //        var extention = imagarr[imagarr.length - 1];
        //        obj.filetype = extention;
        //        if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls')
        //        {
        //            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.hrelaP_SupportingDocument;
        //        }
        //        else if (extention === "jpg" || extention === "jpeg" || extention === "JPEG") {
        //            $('#preview').removeAttr('src');
        //            $('#preview').attr('src', $scope.hrelaP_SupportingDocument);
        //            $('#myModal').modal();
        //            $('#myModal').modal({ keyboard: false });
        //            $('#myModal').modal('show');
        //        }
        //        else if (extention === "pdf") {
        //            var imagedownload = $scope.hrelaP_SupportingDocument;
        //            $scope.content = "";
        //            var fileURL = "";
        //            var file = "";
        //            $http.get(imagedownload, { responseType: 'arraybuffer' })
        //                .success(function (response) {
        //                    file = new Blob([(response)], { type: 'application/pdf' });
        //                    fileURL = URL.createObjectURL(file);
        //                    $scope.content = $sce.trustAsResourceUrl(fileURL);
        //                    $('#showpdf').modal('show');
        //                });
        //        }
        //    }
        //};

        $scope.viewdocument = function (obj) {
            if (obj.HRELAP_SupportingDocument != null) {
                var img = obj.HRELAP_SupportingDocument;
                var imagarr = img.split('.');
                var extention = imagarr[imagarr.length - 1];
                obj.filetype = extention;
                if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls') {
                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + obj.HRELAP_SupportingDocument;
                }
                else if (extention === "jpg" || extention === "jpeg" || extention === "JPEG" || extention === "png") {
                    $('#preview').removeAttr('src');
                    $('#preview').attr('src', obj.HRELAP_SupportingDocument);
                    $('#myModal').modal();
                    $('#myModal').modal({ keyboard: false });
                    $('#myModal').modal('show');
                }
                else if (extention === "pdf") {
                    var imagedownload = obj.HRELAP_SupportingDocument;
                    $scope.content = "";
                    var fileURL = "";
                    var file = "";
                    $http.get(imagedownload, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            $scope.content = $sce.trustAsResourceUrl(fileURL);
                            $('#showpdf').modal('show');
                        });
                }
            }
            if (obj.HRELAP_SupportingDocument == null || obj.HRELAP_SupportingDocument == 'undefined') {
                swal("No Document to preview");
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.Viewleavebalancehistory = function (user) {

            var data = {
                "HRML_Id": user.HRML_Id,
                "HRME_Id": user.HRME_Id
            };

            apiService.create("LeaveApproval/Viewleavebalancehistory", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getemployeeleavedetails = promise.getemployeeleavedetails;
                    $scope.EmployeeName = promise.getemployeeleavedetails[0].EmpName;
                }

            });
        };
    }
})();