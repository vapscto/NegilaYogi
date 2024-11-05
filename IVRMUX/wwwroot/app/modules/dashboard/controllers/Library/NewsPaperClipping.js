(function () {
    'use strict';
    angular
        .module('app')
        .controller('NewsPaperClippingController', NewsPaperClippingController);
    NewsPaperClippingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function NewsPaperClippingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        $scope.UploadFile = [];
        $scope.search = "";
     

        //------------for sorting.........
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }



        //----------load data into page.............
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var id = 1;
            apiService.getURI("NewsPaperClipping/Getdetails", id).
                then(function (promise) {

                    $scope.alldata = promise.alldata;

                })
        }


        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" &&
                    input.files[0].size <= 2097152) {

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

       
        //-------------search date
        //$scope.filterValue = function (obj) {
        //    return
        //    angular.lowercase(obj.intB_Title).indexOf(angular.lowercase($scope.search)) >= 0 ||
        //        angular.lowercase(obj.intB_Description).indexOf(angular.lowercase($scope.search)) >= 0 ||
        //        angular.lowercase(obj.intB_Attachment).indexOf(angular.lowercase($scope.search)) >= 0 ||
        //        ($filter('date')(obj.intB_StartDate, 'dd/MM/yyyy').indexOf($scope.search) > 0) ||
        //        ($filter('date')(obj.intB_EndDate, 'dd/MM/yyyy').indexOf($scope.search) > 0) ||
        //        ($filter('date')(obj.intB_DisplayDate, 'dd/MM/yyyy').indexOf($scope.search) > 0)
        //}
        //$scope.filterValue = function (obj) {
        //    return $filter('date')(obj.OrderDate, 'MM/dd/yyyy') == $filter('date')($scope.searchValue, 'MM/dd/yyyy')
        //}

        
        //------------save data.....  
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {

                //-------file upload

                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "LNPCL_Id": $scope.lnpcL_Id,
                        "LNPCL_ClipName": $scope.LNPCL_ClipName,
                        "LNPCL_ClipDetails": $scope.LNPCL_ClipDetails,
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
                        "LNPCL_Id": $scope.lnpcL_Id,
                        "LNPCL_ClipName": $scope.LNPCL_ClipName,
                        "LNPCL_ClipDetails": $scope.LNPCL_ClipDetails,                      
                        "LNPCL_ClipImage": $scope.file_detail,
                        "LNPCL_FilePath": att_file11,
                    }
                }



                apiService.create("NewsPaperClipping/savedetail", data).
                    then(function (promise) {

                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.lnpcL_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.lnpcL_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        //----------------edit data.......
        $scope.editnotice = function (id) {
            var data = {
                "LNPCL_Id": id.lnpcL_Id
            }
            apiService.create("NewsPaperClipping/editdetails", data).then(function (promise) {

                $scope.editdetails = [];
                if (promise.editdetails.length > 0) {
                    $scope.lnpcL_Id = promise.editdetails[0].lnpcL_Id;
                    $scope.LNPCL_ClipName = promise.editdetails[0].lnpcL_ClipName;
                    $scope.LNPCL_ClipDetails = promise.editdetails[0].lnpcL_ClipDetails;
                    $scope.file_detail = promise.editdetails[0].lnpcL_ClipImage;

                    //$scope.intB_FilePath = promise.editdetails[0].lnpcL_FilePath;
                    $scope.notice = promise.editdetails[0].lnpcL_FilePath;




                };
            })
        }; 
        //-----------------------------------


    

        //-------------for active and deactive
        $scope.deactiveY = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.lnpcL_ActiveFlg === true) {
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

                        apiService.create("NewsPaperClipping/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.clear = function () {
            $state.reload();
        };
    }
})();
