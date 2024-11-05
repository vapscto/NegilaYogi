(function () {
    'use strict';

    angular
        .module('app')
        .controller('NotationController', NotationController);

    NotationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']

    function NotationController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {
      
        $scope.UploadStudentProfilePic = [];
        $scope.images_temp = [];
        var id = 1;
        $scope.load = function () {
            
            apiService.getURI("Notice/Getdetails", id).
      then(function (promise) {
          if (promise.returnval === true) {
              
              $scope.viewrecordspopupdisplay1 = promise.notice_data;
                  // $scope.notice_description= promise.notice_description,
                  // $scope.notice_EStartDate= promise.notice_EStartDate,
                  // $scope.notice_EndDate= promise.notice_EndDate,
                  //$scope.display_date= promise.display_date
          }
          else {
              swal('No Records Found');
          }
      })
            }

        $scope.uploadStudentProfilePic = function (input, document) {
            
            $scope.UploadStudentProfilePic = input.files;

            for (var a = 0; a < $scope.UploadStudentProfilePic.length; a++) {

                if (input.files && input.files[a]) {

                    if ((input.files[a].type === "image/jpeg" || input.files[a].type === "image/png") && input.files[a].size <= 2097152)  // 2097152 bytes = 2MB || input.files[a].type == "image/png"   video/mp4,video/3gpp
                    {
                        var count = 0;
                        angular.forEach($scope.images_temp, function (imgt123) {
                            if (imgt123.name == input.files[a].name) {
                                count += 1;
                            }
                        });
                        if (count == 0) {
                            $scope.images_temp.push(input.files[a]);
                        }
                        var reader = new FileReader();
                        // var id = input.files[a].name;
                        reader.onload = function (e) {
                            
                            $('#blah')
                             .attr('src', e.target.result)
                        };
                        reader.readAsDataURL(input.files[a]);
                        //$scope.images_temp.push(input.files[a]);
                    }
                    else if (input.files[a].type != "image/jpeg") {
                        swal("Please Upload the JPEG file");
                        return;
                    } else if (input.files[a].size > 2097152) {
                        swal("Image size should be less than 2MB");
                        return;
                    }
                }

            }
            $scope.notice_title = $scope.notice_title;
        }

        $scope.remove_img = function (sel_img_del) {
            
            for (var i = 0; i < $scope.images_temp.length; i++) {
                
                var imgt123 = $scope.images_temp[i];
                if (imgt123.name == sel_img_del.name) {
                    
                    $scope.images_temp.splice(i, 1);
                }
            }
           
        }
        $scope.selectedImages = [];
        $scope.submitted = false;
        $scope.savedata = function () {

            
            if ($scope.myForm1.$valid) {

                if ($scope.images_paths != null) {
                    $scope.selectedImages.length = 0;
                    for (var i = 0; i < $scope.images_paths.length; i++) {
                        $scope.selectedImages.push( {"imagePath": $scope.images_paths[i]});
                    }
                }
               
               // Uploadprofile1();
                var data = {
                    "INTB_Title": $scope.notice_title,
                    "INTB_Description": $scope.notice_description,
                    "INTB_StartDate": new Date($scope.notice_EStartDate).toDateString(),
                    "INTB_EndDate": new Date($scope.notice_EndDate).toDateString(),
                   "INTB_DisplayDate": new Date($scope.display_date).toDateString(),
                   images_list: $scope.selectedImages,
                }
                
                apiService.create("Notice/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Data successfully Saved');
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records AlReady Exist !');
                        }
                        else if (promise.returnval === false) {
                            swal('Data Not Saved !');
                        }
                        $scope.gridOptions1.data = promise.master_eventlist;
                    });
              
                $scope.clear1();
               
            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.clear1 = function () {
            $state.reload();
        };


        function Uploadprofile1() {
            
            var formData = new FormData();

            for (var i = 0; i <= $scope.images_temp.length; i++) {
                formData.append("File", $scope.images_temp[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_imgs_vids", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
            .success(function (d) {

                defer.resolve(d);
                swal(d);
                $scope.images_paths = d;
              
                $scope.savedata();
            })
            .error(function () {
                defer.reject("File Upload Failed!");
            });
          
        }

   
        $scope.submitted = false;
        $scope.submit = function () {
            
            if ($scope.myForm1.$valid) {
                if ($scope.images_temp.length == 0) {
                    
                    $scope.savedata();
                }
                else if ($scope.images_temp.length > 0) {

                    
                    Uploadprofile1();
                }
            }
            else {
                $scope.submitted = true;

            }

        }

        $scope.interacted2 = function (field) {

            return $scope.submitted || field.$dirty;
        };



        $scope.Deactivate = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.intB_ActiveFlag === true) {
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

                apiService.create("Notice/deactivate", employee).
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

    }
})();
