(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckList_MasterController', checkList_MasterController)
    checkList_MasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$filter', '$stateParams', 'Excel', '$timeout']
    function checkList_MasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $filter, $stateParams, Excel, $timeout) {

        $scope.sortKey = 'ismresgmrE_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

        //===================Load data=======================
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Exit_Employee/Get_Checklist", pageid).then(function (promise) {
                $scope.department_lisd_dd = promise.department_lisd_dd;
                $scope.designation_list_dd = promise.designation_list_dd;
                $scope.check_list = promise.check_list;
                $scope.presentCountgrid = $scope.check_list.length;

            });
        };
        //================Remove image

        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        };
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" &&
                    input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
                $scope.photo = $scope.UploadFile[0].name;
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //=============Save/Edit========================

        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.notice1 = "";
            if ($scope.notice != undefined && $scope.notice.length > 0) {
                $scope.notice1 = $scope.notice[0];
            }

            if ($scope.myForm.$valid) {

                var data = {
                    "ISMRESGMCL_Id": $scope.id,
                    "HRMD_Id": $scope.HRMD_Id,
                    // "HRMDES_Id": $scope.HRMDES_Id,
                    "ISMRESGMCL_Remarks": $scope.ISMRESGMCL_Remarks,
                    "ISMRESGMCL_CheckListName": $scope.ISMRESGMCL_CheckListName,
                    "ISMRESGMCL_Template": $scope.notice1
                };
                apiService.create("Exit_Employee/Save_Edit_Checklist", data).then(function (primise) {
                    if (primise.returnval === "Add") {
                        swal('Record Save Successfully');
                    }
                    else if (primise.returnval === "Update") {
                        swal('Record Update Successfully');
                    }
                    else {
                        swal('Operation Failed');
                    }
                    $state.reload();
                });
            }
            else {

                $scope.submitted = true;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        //============================getdetailsReasonmaster================================
        $scope.edit = {};
        $scope.edit = function (ss) {
            var pageid = ss.ismresgmcL_Id;
            apiService.getURI("Exit_Employee/get_details_checklist", pageid).then(function (promise) {
                $scope.ISMRESGMCL_CheckListName = promise.get_details_check_list[0].ismresgmcL_CheckListName;
                $scope.hrmD_DepartmentName = promise.get_details_check_list[0].hrmD_DepartmentName;
                $scope.HRMD_Id = promise.get_details_check_list[0].hrmD_Id;
                // $scope.hrmdeS_DesignationName = promise.get_details_check_list[0].hrmdeS_DesignationName;
                // $scope.hrmdeS_Id = promise.get_details_check_list[0].hrmdeS_Id;
                $scope.ISMRESGMCL_Remarks = promise.get_details_check_list[0].ismresgmcL_Remarks;
                $scope.file_detail = promise.get_details_check_list[0].ismresgmcL_Template;
                if (promise.get_details_check_list[0].ismresgmcL_Template != null && promise.get_details_check_list[0].ismresgmcL_Template != "") {
                    var nameArray = $scope.file_detail.split('.');
                    $scope.extention = nameArray[nameArray.length - 1];
                    if ($scope.extention === 'doc' || $scope.extention === 'docx' || $scope.extention === 'ppt' || $scope.extention === 'pptx' || $scope.extention === 'xlsx' || $scope.extention === 'xls') {
                        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.file_detail;
                    }
                }
                $scope.id = promise.get_details_check_list[0].ismresgmcL_Id;
            });
        };

        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.ismresgmcL_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Exit_Employee/active_deactive_checklist", flr).
                            then(function (promise) {

                                if (promise.returnval === "false") {
                                    swal('Master Check list Deactivated Successfully');
                                }

                                else if (promise.returnval === "true") {
                                    swal('Master Check list Activated Successfully');
                                }
                                else {
                                    swal("Cancelled");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.showmodaldetails = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg") {
                $('#preview').attr('src', data);
            }
            else if (extention == "doc" || extention == "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention == "pdf") {
                var imagedownload = data;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        var pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        var embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');

                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };
    }

})();
