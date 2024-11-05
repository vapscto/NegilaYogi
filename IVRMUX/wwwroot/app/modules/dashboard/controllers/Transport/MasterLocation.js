
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterLocationController', MasterLocationController)

    MasterLocationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterLocationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "trmL_Id";   //set the sortKey to the param passed
        $scope.sortReverse = true; //if true make it false and vice versa



        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.listshow = false;

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MasterLocation/getdata", pageid).
        then(function (promise) {
            //$scope.getzonearea = promise.getzonearea;
            if (promise.getlocationareadata.length > 0) {
                $scope.listshow = true;
                $scope.locationdetails = promise.getlocationareadata;
                $scope.presentCountgrid = $scope.locationdetails.length;
            }
            else {
                swal("No Records Found");
                $scope.listshow = false;
            }
        })
        }

        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //---Save--//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {

                if ($scope.long == null) {
                    $scope.long = '';
                } else {
                    $scope.long = $scope.long;
                }

                if ($scope.lat == null) {
                    $scope.lat = '';
                } else {
                    $scope.lat = $scope.lat;
                }

                var data = {
                    "TRML_Id": $scope.TRML_Id,
                    "TRML_Longitude": $scope.long,
                    "TRML_Latitude": $scope.lat,
                    // "TRMA_Id": $scope.trmA_Id,
                    "TRML_LocationName": $scope.trmL_LocationName,
                }
                apiService.create("MasterLocation/savedata", data).then(function (promise) {
                    if (promise.message == "Add") {
                        if (promise.retrunval == true) {
                            swal("Record Saved Successfull");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.retrunval == true) {
                            swal("Record Updated Successfull");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message == "Mapped") {
                        swal("You Can Not Edit This Record It Already Mapped");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        //--edit--//
        $scope.edit = function (user) {
            var data = {
                "TRML_Id": user.trmL_Id
            }
            apiService.create("MasterLocation/edit", data).then(function (promise) {
                if (promise != null) {
                    if (promise.geteditdata.length > 0) {
                        $scope.long = promise.geteditdata[0].trmL_Longitude;
                        $scope.lat = promise.geteditdata[0].trmL_Latitude;
                        // $scope.trmA_Id = promise.geteditdata[0].trmA_Id;
                        $scope.trmL_LocationName = promise.geteditdata[0].trmL_LocationName;
                        $scope.arealist = true;
                        $scope.TRML_Id = promise.geteditdata[0].trmL_Id
                    }
                }
            })
        }

        //--Active Deactive--//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trmL_ActiveFlg === true) {
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
                apiService.create("MasterLocation/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }

        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return angular.lowercase(obj.trmL_Longitude).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.trmL_Latitude).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
               // angular.lowercase(obj.trmA_AreaName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.trmL_LocationName).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        $scope.cancel = function () {
            $state.reload();
        }
    };
})();


