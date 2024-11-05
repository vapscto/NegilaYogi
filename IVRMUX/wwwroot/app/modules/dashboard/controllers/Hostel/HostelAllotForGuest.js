(function () {
    'use strict';
    angular
        .module('app')
        .controller('HostelAllotForGuest', HostelAllotForGuest)

    HostelAllotForGuest.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter'];
    function HostelAllotForGuest($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        $scope.HLHGSTALT_AllotmentDate = new Date();
        //======================page load
        $scope.loaddata = function () {           
            var pageid = 2;
            apiService.getURI("HostelAllotForGuest/loaddata", pageid).then(function (promise) {                
                $scope.hstllist = promise.hstllist;
                $scope.categorylist = promise.categorylist;
                $scope.roomlist = promise.roomlist;
                $scope.alldata1 = promise.alldata1;              
            })
        };
        $scope.get_roomdetails = function () {        
            var data = {
                "HRMRM_Id": $scope.HRMRM_Id,
            }
            apiService.create("HostelAllotForGuest/get_roomdetails", data).then(function (promise) {               
                $scope.HLHGSTALT_NoOfBeds = promise.hlhgstalT_NoOfBeds;
            })
        }
        function Uploads(HLHGSTALT_Id) {        
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUpload.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload[i]);
            }
            for (var i = 0; i <= $scope.selectFileforUpload2.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload2[i]);
            }
            for (var i = 0; i <= $scope.selectFileforUpload3.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload3[i]);
            }
            for (var i = 0; i <= $scope.selectFileforUpload4.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload4[i]);
            }
            for (var i = 0; i <= $scope.selectFileforUpload5.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload5[i]);
            }
            for (var i = 0; i <= $scope.selectFileforUpload6.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload6[i]);
            }
            for (var i = 0; i <= $scope.selectFileforUpload7.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload7[i]);
            }
            for (var i = 0; i <= $scope.selectFileforUploadz.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadz[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", HLHGSTALT_Id);
            //var defer = $q.defer();
            $http.post("/api/ImageUpload/", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
        }  
        $scope.SelectedFileForUploadz = [];
        $scope.selectFileforUploadz = function (input) {          
            $scope.SelectedFileForUploadz = input.files;
            if (input.files && input.files[0]) {               
                if ((input.files[0].type == "image/jpeg") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();

                } else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
        function Uploadprofile() {

            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadz.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadz[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    // swal(d);
                    $scope.reg.HLHGSTALT_GuestPhoto = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }
        //======================Record Save
        $scope.search = "";
        $scope.save = function () {
            if ($scope.myForm.$valid) {        
                var data = {
                    "HLHGSTALT_Id": $scope.HLHGSTALT_Id,
                    "HLHGSTALT_GuestName": $scope.HLHGSTALT_GuestName,
                    "HLHGSTALT_GuestPhoneNo": $scope.HLHGSTALT_GuestPhoneNo,
                    "HLMH_Id": $scope.HLMH_Id,
                    "HRMRM_Id": $scope.HRMRM_Id,
                    "HLHGSTALT_NoOfBeds": $scope.HLHGSTALT_NoOfBeds,
                    "HLHGSTALT_AllotRemarks": $scope.HLHGSTALT_AllotRemarks,
                    "HLHGSTALT_AllotmentDate": $scope.HLHGSTALT_AllotmentDate,
                    "HLHGSTALT_GuestEmailId": $scope.HLHGSTALT_GuestEmailId,
                    "HLMRCA_Id": $scope.HLMRCA_Id,
                    "HLHGSTALT_AddressProof": $scope.HLHGSTALT_AddressProof,
                    "HLHGSTALT_GuestAddress": $scope.HLHGSTALT_GuestAddress,
                    "HLHGSTALT_GuestPhoto": $scope.reg.HLHGSTALT_GuestPhoto,                   
                }
                apiService.create("HostelAllotForGuest/save", data).then(function (promise) {
                    if (promise.msg == 'saved') {
                        swal("Data Saved Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data Not Saved Successfully...!!!");
                    }
                    else if (promise.msg == 'updated') {
                        swal("Data Updated Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == 'failed') {
                        swal("Data Not Updated Successfully...!!!");
                    }
                    else if (promise.returnval == true) {
                        swal("Data Already Exists");
                    }
                    else {
                        swal("Something is Wrong......");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.deactivYTab1 = function (usersem, SweetAlert) {          
            var dystring = "";
            if (usersem.hlhgstalT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.hlhgstalT_ActiveFlag == false) {
                dystring = "Activate"
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HostelAllotForGuest/deactive", usersem).
                            then(function (promise) {
                                if (promise.ret == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        ///=========clear upload field data......
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        }
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //================================edit data
        $scope.edittab1 = function (user) {           
            var data = {
                "HLHGSTALT_Id": user.hlhgstalT_Id,                
            }
            apiService.create("HostelAllotForGuest/EditData", data).then(function (promise) {
                if (promise.editlist.length > 0) { 
                    $scope.HLHGSTALT_Id = promise.editlist[0].hlhgstalT_Id;
                    $scope.HLHGSTALT_AllotmentDate = new Date(promise.editlist[0].hlhgstalT_AllotmentDate);
                    $scope.HLMH_Id = promise.editlist[0].hlmH_Id;
                    $scope.HLMRCA_Id = promise.editlist[0].hlmrcA_Id;
                    $scope.HRMRM_Id = promise.editlist[0].hrmrM_Id;
                    $scope.HLHGSTALT_GuestName = promise.editlist[0].hlhgstalT_GuestName;
                    $scope.HLHGSTALT_GuestPhoneNo = promise.editlist[0].hlhgstalT_GuestPhoneNo;
                    $scope.HLHGSTALT_GuestEmailId = promise.editlist[0].hlhgstalT_GuestEmailId;
                    $scope.HLHGSTALT_GuestAddress = promise.editlist[0].hlhgstalT_GuestAddress;
                    $scope.HLHGSTALT_AddressProof = promise.editlist[0].hlhgstalT_AddressProof;
                    $scope.HLHGSTALT_NoOfBeds = promise.editlist[0].hlhgstalT_NoOfBeds;
                    $scope.HLHGSTALT_AllotRemarks = promise.editlist[0].hlhgstalT_AllotRemarks;
                    $scope.HLHGSTALT_ActiveFlag = promise.editlist[0].hlhgstalT_ActiveFlag;
                    $scope.HLMRCA_RoomCategory = promise.editlist[0].hlmrcA_RoomCategory;
                    $scope.HRMRM_RoomNo = promise.editlist[0].hrmrM_RoomNo;
                    $scope.HLMH_Name = promise.editlist[0].hlmH_Name;
                    $('#blah').attr('src', promise.editlist[0].hlhgstalT_GuestPhoto);
                    $scope.reg.HLHGSTALT_GuestPhoto = promise.editlist[0].hlhgstalT_GuestPhoto;
                }
            })
        }
    }
})();

