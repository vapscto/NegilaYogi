
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterVehicleTypeController', MasterVehicleTypeController)

    MasterVehicleTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterVehicleTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.sortKey = 'trmvT_Id';
        $scope.sortReverse = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MasterVehicleType/getdata", pageid).then(function (promise) {
                
                if (promise != null) {
                    if (promise.getmastervehicle.length > 0) {
                        
                        $scope.zoneareadetails = promise.getmastervehicle;
                        $scope.presentCountgrid = $scope.zoneareadetails.length;
                    }
                    else {
                        swal("No Records Found")
                    }
                }
                else {
                    swal("No Records Found")
                }
            })

        }

        $scope.submitted = false;


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //-------Save Data-------//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "TRMVT_Id": $scope.trmvT_Id,
                    "TRMVT_VehicleType": $scope.fuletype,
                    "TRMVT_VehicleDesc": $scope.fuleDESC,

                }
                apiService.create("MasterVehicleType/savedata", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.message == "Add") {
                            if (promise.retrval == true) {
                                swal("Record Saved Successfully");
                            }
                            else {
                                swal("Failed To Saved Record");
                            }
                        }
                        else if (promise.message == "Update") {
                            if (promise.retrval == true) {
                                swal("Record Update Successfully");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                        else if (promise.message == "Duplicate") {
                            swal("Record Already Exists");
                        }
                    }
                    else {

                    }
                    $state.reload();
                })
            } else {
                $scope.submitted = true;
            }
        }

        //--Cancel--//
        $scope.cancle = function () {
            $scope.TRMVT_Id = 0;
            $state.reload();
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.trmvT_VehicleDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmvT_VehicleType)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "TRMVT_Id": user.trmvT_Id
            }
            apiService.create("MasterVehicleType/geteditdata", data).then(function (Promise) {
                if (Promise != null) {
                    $scope.fuletype = Promise.geteditdatavehicle[0].trmvT_VehicleType;

                    $scope.fuleDESC = Promise.geteditdatavehicle[0].trmvT_VehicleDesc;

                    $scope.trmvT_Id = Promise.geteditdatavehicle[0].trmvT_Id;
                }
            })
        }

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trmvT_ActiveFlg === true) {
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
                apiService.create("MasterVehicleType/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully.");
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

    };
})();


