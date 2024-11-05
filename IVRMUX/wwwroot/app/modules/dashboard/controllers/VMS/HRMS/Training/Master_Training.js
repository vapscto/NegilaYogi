(function () {
    'use strict';
    angular
        .module('app')
        .controller('TrainingController', trainingController)

    trainingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter']
    function trainingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter) {

        $scope.sortKey = 'Id';
        $scope.sortReverse = true;
        $scope.donce = false;
      



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        //-----------upload file/photo.............
       
        $scope.uploadFile = [];

        $scope.uploadFile = function (input, document) {

            $scope.uploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blah').attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();

                }
                else if (input.files[0].type !== "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };

        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {
                formData.append("File", $scope.uploadFile[i]);
                $scope.file_detail = $scope.uploadFile[0].name;
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
                    $scope.obj.image = d;
                    $scope.notice = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }
        //----------End..........!



        ///=========clear upload field data......
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        }
        //get data 
        $scope.getAllDetail = function () {
            $scope.currentPage = 1;
            // $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Master/getdata_T", pageid)
                .then(function (promise) {
                    $scope.quali = promise.qualification_list;
                    $scope.gender = promise.gender_list;
                    $scope.training_lists = promise.training_list;
                    $scope.presentCountgrid = $scope.training_lists.length;

                })
        }

        //save
        $scope.submitted = false;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 10;
        $scope.savetmpldata = function () {
            $scope.notice1 = "";
          
            if ($scope.notice != null) {
                $scope.notice1 = $scope.notice;
            }
            else {
                $scope.notice1 =$scope.image;
            }

            $scope.submitted = true;
            var DATEBIRTH = $scope.date_of_birth === null ? "" : $filter('date')($scope.date_of_birth, "yyyy-MM-dd");
          //  if ($scope.myForm.$valid) {
                var data = {
                    "HRMETR_Id": $scope.id,
                    "HRMETR_Name": $scope.trainer_Name,
                    "HRMETR_Image_Name": $scope.file_detail,
                    "HRMETR_ImagePath": $scope.notice1,
                    "IVRMMG_Id": $scope.ivrmmG_Id,
                    "HRMETR_DOB": DATEBIRTH,
                    "HRMETR_Address": $scope.address,
                    "HRMETR_City": $scope.city,
                    "HRMETR_Pincode": $scope.pin_code,
                    "HRMETR_EmailId": $scope.Email,
                    "HRMETR_ContactNo": $scope.contact_no,
                    "HRMETR_DomainExp": $scope.Domain_exp,
                    "HRMETR_TrainingExp": $scope.Training_Exp,
                    "HRMETR_Skills": $scope.skills,
                    "HRMEQ_Id": $scope.hrmeQ_Id,
                    "HRMETR_ParttimeORFullTimeFlg": $scope.hiringType,
                    "HRMETR_Remarks": $scope.Remark
                };
                apiService.create("Training_Master/SaveEdit_T", data).then(function (promise) {

                    if (promise.returnvalue !== "") {

                        if (promise.returnvalue === "Update") {
                            swal('Record Updated Successfully');
                            $state.reload();
                            return;
                        }
                        else if (promise.returnvalue === "Add") {
                            swal('Record Saved Successfully');
                            $state.reload();
                            return;
                        }
                        else if (promise.returnvalue === "false") {
                            swal('Record Not Saved/Updated successfully');
                            return;
                        }
                        else if (promise.returnvalue === "Duplicate") {
                            swal('Category Already Exist');
                            $state.reload();
                            return;
                        }
                    }

                    else {
                        swal('Operation Failed');
                        $state.reload();
                    }
                });

            //}
            //else {
            //    $scope.submitted = true;
            //}
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.hrmetR_ActiveFlag === false) {
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
                        apiService.create("Training_Master/deactivate_T", flr).
                            then(function (promise) {

                                if (promise.hrmetR_ActiveFlag === true) {
                                    if (promise.returnval === false) {
                                        swal('Master Training Deactivated Successfully');
                                    }
                                }
                                else if (promise.hrmetR_ActiveFlag === false) {
                                    swal('Master Training Activated Successfully');
                                }
                                //   }

                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };
        //cancel
        $scope.cancel = function () {
            $state.reload();
        }

        //edit
        var DATEBIRTH = $scope.date_of_birth === null ? "" : $filter('date')($scope.date_of_birth, "yyyy-MM-dd");
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrmetR_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Master/details_T", pageid).
                then(function (promise) {

                    $scope.id = promise.train_list[0].hrmetR_Id;
                    $scope.trainer_Name = promise.train_list[0].hrmetR_Name;
                    $scope.address = promise.train_list[0].hrmetR_Address;
                    $scope.date_of_birth = new Date(promise.train_list[0].hrmetR_DOB);
                    $scope.city = promise.train_list[0].hrmetR_City;
                    $scope.pin_code = promise.train_list[0].hrmetR_Pincode;
                    $scope.Email = promise.train_list[0].hrmetR_EmailId;
                    $scope.contact_no = promise.train_list[0].hrmetR_ContactNo;
                    $scope.Domain_exp = promise.train_list[0].hrmetR_DomainExp;
                    $scope.Training_Exp = promise.train_list[0].hrmetR_TrainingExp;
                    $scope.skills = promise.train_list[0].hrmetR_Skills;
                    $scope.hiringType = promise.train_list[0].hrmetR_ParttimeORFullTimeFlg;
                    $scope.Remark = promise.train_list[0].hrmetR_Remarks;
                    $scope.file_detail = promise.train_list[0].hrmetR_Image_Name;
                    $scope.ivrmmG_Id = promise.train_list[0].ivrmmG_Id;
                    $scope.hrmeQ_Id = promise.train_list[0].hrmeQ_Id;
                    $scope.ivrmmG_GenderName = promise.train_list[0].ivrmmG_GenderName;
                    $scope.hrmE_QualificationName = promise.train_list[0].hrmE_QualificationName;
                    $('#blah').attr('src', promise.train_list[0].hrmetR_ImagePath);
                    $scope.obj.image = promise.train_list[0].hrmetR_ImagePath;




                })
        };

        ///view
        $scope.Report = function () {

        };


    }

})();



