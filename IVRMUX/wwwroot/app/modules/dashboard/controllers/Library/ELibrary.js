(function () {
    'use strict';
    angular
.module('app')
        .controller('ELibraryController', ELibraryController)

    ELibraryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','$q']
    function ELibraryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q) {

        $scope.submitted = false;
        $scope.currentPage = 1;
        $scope.search = "";

        $scope.itemsPerPage = 15;
         //=====================Load--data.............
        $scope.Loaddata = function () {
          
           
            debugger;
            var pageid = 2;
            apiService.getURI("AddELibraryLinks/GetELibrary", pageid).then(function (promise) {
                $scope.linklist = promise.linklist;
            })
           
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
         //=====================End-----Load--data----//


        $scope.showmotherprofilepic = function (LELS_FilePath) {
            $('#preview').attr('src', LELS_FilePath);
        }
        //---upload Picture
        $scope.uploadvehiclephoto = function (input) {

            $scope.UploadMotherphoto1 = input.files;

            if (input.files && input.files[0]) {

                if ((input.files[0].type == "image/jpeg" || input.files[0].type == "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    uploadvehiclephotopic();
                }
                else if (input.files[0].type != "image/jpeg" ) {
                    swal("Please Upload the JPEG/PNG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function uploadvehiclephotopic() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadvehiclephoto.length; i++) {
                formData.append("File", $scope.UploadMotherphoto1[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Librarybooks", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    debugger;
                    $scope.LELS_FilePath = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);


        }


         //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {


                if ($scope.LELS_FilePath != null) {
                    $scope.LELS_FilePath = $scope.LELS_FilePath;
                }
                else {
                    $scope.LELS_FilePath = "";
                }
                var data = {
                    "LELS_FilePath": $scope.LELS_FilePath,
                    "LELS_Name": $scope.LELS_Name,
                    "LELS_Url": $scope.LELS_Url,
                    "LELS_BookType": $scope.LELS_BookType,
                    "LELS_Genre": $scope.LELS_Genre,
                    "LELS_PriceRange": $scope.LELS_PriceRange,
                    "LELS_Id": $scope.LELS_Id,
                }
                apiService.create("AddELibraryLinks/Savedata", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {

                                if ($scope.LELS_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Inserted Successfully!!!');
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.Floor_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Inserted Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record Already Exist!!!");
                        }
                       
                        $state.reload();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };
         //=====================End---saverecord....


        //=====================Editrecord....
    

        $scope.EditData = function (user) {
            apiService.create("AddELibraryLinks/geteditdata", user).then(function (Promise) {
                if (Promise != null) {
                   
                   
                    $scope.LELS_FilePath = Promise.editlist[0].lelS_FilePath;
                    $scope.LELS_Name = Promise.editlist[0].lelS_Name;
                    $scope.LELS_Url = Promise.editlist[0].lelS_Url;
                    $scope.LELS_BookType = Promise.editlist[0].lelS_BookType;
                    $scope.LELS_Genre = Promise.editlist[0].lelS_Genre;
                    $scope.LELS_PriceRange = Promise.editlist[0].lelS_PriceRange;
                    $scope.LELS_Id = Promise.editlist[0].lelS_Id;

                }
            })
        }
         //====================End---edit-record....


         //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            var dystring = "";
            if (user.lelS_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.lelS_ActiveFlag == 0) {
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
                    apiService.create("AddELibraryLinks/deactiveY", user).
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
                    swal("Record " + dystring + " Cancelled");
                }

            });
        }
         //================End----Activation/Deactivation--Record.........


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };



         //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

