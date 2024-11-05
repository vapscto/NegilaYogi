(function () {
    'use strict';
    angular
        .module('app')
        .controller('Room_Category', Room_Category)

    Room_Category.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function Room_Category($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortReverse = true;
        $scope.itemsPerPage = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        $scope.obj = {};
        //=========get data
        $scope.getAllDetail = function () {

            $scope.currentPage = 1;
            var pageid = 2;
            apiService.getURI("HS_Master/get_roomcatdata", pageid)
                .then(function (promise) {
                    $scope.get_roomcatlist = promise.griddata;
                    $scope.hostel_list = promise.hostellist;
                    $scope.feegroupe = promise.feegroupe;

                  

                })
            //$scope.HLMH_Id = ""; $scope.hlmrcA_Id = 0; $scope.HLMRCA_Description = "";
        }
        
        //=========save
        $scope.submitted = false;
        $scope.savetmpldata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var hlmrcA_SharingFlg = 0; var hlmrcA_ACFlg = 0;
                if ($scope.hlmrcA_SharingFlg == true) {
                    hlmrcA_SharingFlg = 1;
                }
                if ($scope.hlmrcA_ACFlg == true) {
                    hlmrcA_ACFlg = 1;
                }
                var data = {
                    "HLMRCA_Id": $scope.hlmrcA_Id,
                    "HLMRCA_RoomCategory": $scope.HLMRCA_RoomCategory,
                    "HLMRCA_Description": $scope.HLMRCA_Description,
                    "HLMRCA_MaxCapacity": $scope.HLMRCA_MaxCapacity,
                    "HLMH_Id": $scope.HLMH_Id,
                    "FMG_Id": $scope.FMG_Id,
                    "HLMRCA_SORate": $scope.HLMRCA_SORate,
                    "HLMRCA_RoomRate": $scope.HLMRCA_RoomRate,
                    "HLMRCA_SharingFlg": hlmrcA_SharingFlg,
                    "HLMRCA_ACFlg": hlmrcA_ACFlg

                }
                apiService.create("HS_Master/save_roomcatdata", data).
                    then(function (promise) {
                        if (promise.returnval !== null && promise.duplicate !== null) {
                            if (promise.duplicate === false) {
                                if (promise.returnval === true) {
                                    if ($scope.hlmrcA_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                        $state.reload();
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                        $state.reload();
                                    }
                                }
                                else {
                                    if (promise.returnval === false) {
                                        if ($scope.hlmrcA_Id > 0) {
                                            swal('Record Not Updated Successfully!!!');
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

        //================edit

        $scope.edit_data = function (user) {

            $scope.hlmrcA_Id = user.hlmrcA_Id;
            $scope.FMG_Id = user.fmG_Id;
            $scope.HLMRCA_RoomCategory = user.hlmrcA_RoomCategory;
            $scope.HLMRCA_Description = user.hlmrcA_Description;
            $scope.HLMRCA_MaxCapacity = user.hlmrcA_MaxCapacity;
            if (user.hlmrcA_ACFlg == true) {
                $scope.hlmrcA_ACFlg = true;
            }
            else {
                $scope.hlmrcA_ACFlg = false;
            }
            if (user.hlmrcA_SharingFlg == true) {
                $scope.hlmrcA_SharingFlg = true;
            }
            else {
                $scope.hlmrcA_SharingFlg = false;
            }
           
            $scope.HLMRCA_SORate = user.hlmrcA_SORate;
            $scope.HLMRCA_RoomRate = user.hlmrcA_RoomRate;
            $scope.HLMH_Id = user.hlmH_Id;
        };


        $scope.cancel = function () {
            $state.reload();
        }

        //================================deactive

        $scope.deactive = function (user, SweetAlert) {
            debugger;
            $scope.hlmrcA_Id = user.hlmrcA_Id;

            var dystring = "";
            if (user.hlmrcA_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hlmrcA_ActiveFlag === false) {
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
                        apiService.create("HS_Master/deactiveY_roomcatdata", user).
                            then(function (promise) {
                                if (promise.returnval === true) {
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


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



