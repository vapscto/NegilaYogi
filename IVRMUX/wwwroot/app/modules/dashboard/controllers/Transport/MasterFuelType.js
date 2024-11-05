
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterFuelController', MasterFuelController)

    MasterFuelController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterFuelController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.sortKey = 'trmfT_Id';
        $scope.sortReverse = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        $scope.loaddata = function () {
            
            var pageid = 2;
            apiService.getURI("MasterFuel/getdata", pageid).then(function (promise) {
                if (promise != null) {
                    if (promise.getmasterfuel.length > 0) {
                        $scope.zoneareadetails = promise.getmasterfuel;
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
                    "TRMFT_Id": $scope.trmft_Id,
                    "TRMFT_FuelType": $scope.fuletype
                }
                apiService.create("MasterFuel/savedata", data).then(function (promise) {
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
            $scope.trmft_Id = 0;
            $state.reload();
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.trmfT_FuelType)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "TRMFT_Id": user.trmfT_Id
            }
            apiService.create("MasterFuel/geteditdata", data).then(function (Promise) {
                if (Promise != null) {
                    $scope.fuletype = Promise.geteditdatafuel[0].trmfT_FuelType;

                    $scope.trmft_Id = Promise.geteditdatafuel[0].trmfT_Id;
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
            if (user.trmfT_ActiveFlg === true) {
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
                apiService.create("MasterFuel/activedeactive/", user).
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


