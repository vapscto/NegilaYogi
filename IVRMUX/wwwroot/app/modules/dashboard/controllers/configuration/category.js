(function () {
    'use strict';
    angular.module('app').controller('categoryController', categoryController)

    categoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','$q']
    function categoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache,$q) {

        $scope.sortKey = 'amC_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings!=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.getAllDetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("MasterCategory/getalldetails", pageid).then(function (promise) {
                    $scope.students = promise.categoryList;
                    $scope.institution = promise.institutionDrpdwn;
                    $scope.MI_Id = promise.institutionDrpdwn[0].mI_Id;
                    $scope.presentCountgrid = $scope.students.length;
                    $scope.typelist = [
                        { id: '1', name: "School" },
                        { id: '2', name: "College" }
                    ];
            });
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };


        $scope.edit = function (employee) {
            $scope.editEmployee = employee.amC_Id;
            var templId = $scope.editEmployee;
            $('#Logo').attr('src', "");
            $scope.MI_Logo = "";
            apiService.create("MasterCategory/getdetails", employee).then(function (promise) {
                $scope.address = promise.categoryList[0].amC_Address.split(',');
                $scope.MI_Id = promise.categoryList[0].mI_Id;
                $scope.AMC_Id = promise.categoryList[0].amC_Id;
                $scope.AMC_Name = promise.categoryList[0].amC_Name;
                $scope.Line1 = $scope.address[0];
                $scope.Line2 = $scope.address[1];
                $scope.Line3 = $scope.address[2];
                $scope.Line4 = $scope.address[3];
                $scope.AMC_RegNoPrefix = promise.categoryList[0].amC_RegNoPrefix;
                $scope.AMC_Type = promise.categoryList[0].amC_Type;
                $scope.AMC_R = promise.categoryList[0].amC_RegNoPrefix;
                $scope.AMC_Type = promise.categoryList[0].amC_Type; 
                $('#Logo').attr('src', promise.categoryList[0].amC_FilePath);
                $scope.AMC_FilePath = promise.categoryList[0].amC_FilePath;
                

            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.submitted = false;
        $scope.savetmpldata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "AMC_Id": $scope.AMC_Id,
                    "AMC_Name": $scope.AMC_Name,
                    "Line1": $scope.Line1,
                    "Line2": $scope.Line2,
                    "Line3": $scope.Line3,
                    "Line4": $scope.Line4,
                    "AMC_Type": $scope.AMC_Type,
                    "AMC_RegNoPrefix": $scope.AMC_RegNoPrefix,
                    "AMC_Logo": $scope.AMC_Logo,
                    "AMC_FilePath":$scope.AMC_FilePath
                };

                apiService.create("MasterCategory/", data).then(function (promise) {
                    if (promise.returnval == "Add") {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == "Update") {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == "false") {
                        swal('Record Not Saved/Updated successfully');
                    }
                    else if (promise.returnval == "Duplicate") {
                        swal('Category Already Exist');
                        return;
                    }
                    else {
                        swal('Operation Failed');
                        // $state.reload();
                    }
                });
            }
            else {

                $scope.submitted = true;
            }
        };



//image upload
 $scope.SelectedFileForUploadzl = [];
        $scope.selectFileforUploadzl = function (input) {

            $scope.SelectedFileForUploadzl = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#Logo')
                        .attr('src', e.target.result)
                };
                reader.readAsDataURL(input.files[0]);
                $scope.AMC_Logo = input.files[0].name;
                Uploads1();
            }
        }

        function Uploads1() {
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzl.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzl[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_GalleryImgVideos", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    //swal(d);
                    $scope.AMC_FilePath = d[0];
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
               
        }


//
        $scope.deactive = function (category) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            var mgs = "";
            if (category.amC_ActiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Category?",
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
                        apiService.create("MasterCategory/deactivate", category).then(function (promise) {
                            swal(promise.returnval);
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.amC_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.mI_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amC_Type)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };


    }

})();

