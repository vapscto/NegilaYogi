
(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterFacilityontroller', MasterFacilityontroller);

    MasterFacilityontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter'];

    function MasterFacilityontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter) {


        //======================for pagination
       

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = "";


        ///======================================Load Data.
        $scope.loaddata = function () {
            var id = 2;
            apiService.getURI("HS_Master/get_facilitydata", id).then(function (promise) {
               
                $scope.get_facilitylist = promise.get_facilitylist;
            })
        }

        //=====================Sorting
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //==============================save data For Tab1

        $scope.savedatatab1 = function () {
            debugger;
            
            if ($scope.myForm.$valid) {

                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "HLMFTY_Id": $scope.hlmftY_Id,
                        "HLMFTY_FacilityName": $scope.hlmftY_FacilityName,
                        "HLMFTY_FacilityDesc": $scope.hlmftY_FacilityDesc,
                    }
                }
                else {
                    var att_file = "";
                    $scope.filedoc = [];
                    $scope.filedoc = [$scope.notice];
                    if ($scope.filedoc.length > 0) {
                        for (var i = 0; i < $scope.filedoc.length; i++) {
                            att_file = $scope.filedoc[0];
                        }
                    }
                    var att_file11 = att_file.toString();

                    var data = {
                        "HLMFTY_Id": $scope.hlmftY_Id,
                        "HLMFTY_FacilityName": $scope.hlmftY_FacilityName,
                        "HLMFTY_FacilityDesc": $scope.hlmftY_FacilityDesc,
                        "HLMFTY_FacilityFileName": $scope.file_detail,
                        "HLMFTY_FacilityFilePath": att_file11,
                    }
                }

                apiService.create("HS_Master/save_facilitydata", data).then(function (promise) {

                    if (promise.duplicate != null && promise.returnval != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.hlmftY_Id > 0) {
                                    swal('Data Updated Successfully!')
                                }
                                else {
                                    swal('Data Saved Successfully!')
                                }
                                $state.reload();
                            }
                            else {
                                if ($scope.hlmftY_Id > 0) {
                                    swal('Data Not Updated Successfully!')
                                }
                                else {
                                    swal('Data Not Saved Successfully!')
                                }
                            }
                        }
                        else {
                            swal('Record already Exist!')
                        }
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        //==========================edit data for tab1
        $scope.edit_facilitydata = function (data) {
            debugger;
            apiService.create("HS_Master/edit_faclitydata", data).then(function (promise) {

                $scope.edit_facilitylist = promise.edit_facilitylist;
                
                $scope.hlmftY_Id = promise.edit_facilitylist[0].hlmftY_Id;
                $scope.hlmftY_FacilityName = promise.edit_facilitylist[0].hlmftY_FacilityName;
                $scope.hlmftY_FacilityDesc = promise.edit_facilitylist[0].hlmftY_FacilityDesc;
               
                $scope.file_detail = promise.edit_facilitylist[0].hlmftY_FacilityFileName;
                $scope.att_file11 = promise.edit_facilitylist[0].hlmftY_FacilityFilePath;
                $scope.notice = promise.edit_facilitylist[0].hlmftY_FacilityFilePath;

            })
        }


        //===========deactive and active for Tab1
        $scope.deactiv_facilitydata = function (usersem, SweetAlert) {
            $scope.hlmftY_Id = usersem.hlmftY_Id
            var dystring = "";
            if (usersem.hlmftY_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.hlmftY_ActiveFlag == false) {
                dystring = "Activate";
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
                        apiService.create("HS_Master/deactiveY_faclitydata", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        //For Cancel data record 
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //==================filter Name
        $scope.searchValue = "";
        $scope.filterValue123 = function (obj) {

            return (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ncacsP123_AddOnProgramName)).indexOf(angular.lowercase($scope.searchValue)) >= 0

        }

        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {
            debugger;
            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
        function Uploadprofile() {
            debugger;
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
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
        //----------End..........!


        ///=========clear upload field data......
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        }



    }
})();
