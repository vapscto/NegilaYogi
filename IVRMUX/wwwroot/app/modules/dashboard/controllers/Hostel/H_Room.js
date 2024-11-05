(function () {
    'use strict';
    angular
        .module('app')
        .controller('H_RoomController', H_RoomController)

    H_RoomController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function H_RoomController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'hrmrM_Id';
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
        $scope.facilityflg = false;
        //get data
        $scope.get_Roomloaddata = function () {
            $scope.currentPage = 1;
            var pageid = 2;
            apiService.getURI("HS_Master/get_Roomloaddata", pageid)
                .then(function (promise) {
                    $scope.hostellist = promise.hostellist;
                    $scope.floor_list = promise.floor_list;
                    $scope.room_list = promise.room_list;
                    //$scope.facilities_list = promise.facilities_list;
                   
                    $scope.get_room_list = promise.get_room_list;
                })
        }



        $scope.floor_list = [];
        $scope.Floor = function () {
            var data = {
                "HLMH_Id": $scope.hlmH_Id,
            }
            apiService.create("HS_Master/floor", data).
                then(function (promise) {
                    $scope.floor_list = promise.floor_list;
                    $scope.room_catlist = promise.room_catlist;
                });
        }

        $scope.facilities_list = [];
        $scope.Floordetails = function () {
            var data = {
                "HLMF_Id": $scope.hlmF_Id,
            }
            apiService.create("HS_Master/floordetails", data).
                then(function (promise) {
                    if (promise.facilities_list.length > 0) {
                        $scope.facilityflg = true;
                        $scope.facilities_list = promise.facilities_list;
                    }
                 
                });
        }

       
        $scope.categorydetails = function () {
            $scope.categorydetailsTT = [];
            $scope.HRMRM_ACFlg = false;
            $scope.HRMRM_SharingFlg = false;
            var data = {
                "HLMRCA_Id": $scope.hlmrcA_Id,
            }
            apiService.create("HS_Master/categorydetails", data).
                then(function (promise) {
                    $scope.categorydetailsTT = promise.categorydetails;
                    if ($scope.categorydetailsTT != null && $scope.categorydetailsTT.length > 0) {
                        if ($scope.categorydetailsTT[0].hlmrcA_ACFlg == true) {
                            $scope.HRMRM_ACFlg = true;
                        }
                        else {
                            $scope.HRMRM_ACFlg = false;
                        }
                        if ($scope.categorydetailsTT[0].hlmrcA_SharingFlg == true) {
                            $scope.HRMRM_SharingFlg = true;
                        }
                        else {
                            $scope.HRMRM_SharingFlg = false;
                        }
                    }
                    
                });
        }

        //save
        $scope.submitted = false;
        $scope.save_Roomdata = function () {

            $scope.selected_facilities = [];
            angular.forEach($scope.facilities_list, function (faci) {
                if (faci.select == true) {
                    $scope.selected_facilities.push({ HLMFTY_Id: faci.hlmftY_Id });
                }
            });
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMRM_Id": $scope.hrmrM_Id,
                    "HRMRM_RoomNo": $scope.hrmrM_RoomNo,
                    "HLMF_Id": $scope.hlmF_Id,
                    "HLMH_Id": $scope.hlmH_Id,
                    "HLMRCA_Id": $scope.hlmrcA_Id,
                    "HRMRM_ACFlg": $scope.HRMRM_ACFlg,
                    "HRMRM_BedCapacity": $scope.hrmrM_BedCapacity,
                    "HRMRM_SharingFlg": $scope.HRMRM_SharingFlg,
                    "HRMRM_RoomDesc": $scope.hrmrM_RoomDesc,
                    "HRMRM_RoomForStudentFlg": $scope.hrmrM_RoomForStudentFlg,
                    "HRMRM_RoomForStaffFlg": $scope.hrmrM_RoomForStaffFlg,
                    "HRMRM_RoomForGuestFlg": $scope.hrmrM_RoomForGuestFlg,
                    selected_facilities: $scope.selected_facilities,

                }
                apiService.create("HS_Master/save_Roomdata", data).
                    then(function (promise) {
                        if (promise.returnval !== null && promise.duplicate !== null) {
                            if (promise.duplicate === false) {
                                if (promise.returnval === true) {
                                    if ($scope.hrmrM_Id > 0) {
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
                                        if ($scope.hrmrM_Id > 0) {
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

        //===================get  Mapped  facility
        $scope.get_Mappedfacility = function (user) {

            apiService.create("HS_Master/get_MappedfacilityforRoom", user).then(function (promise) {
                $scope.get_MappedfacilityforRoom = promise.get_MappedfacilityforRoom;
                $scope.temparry = promise.get_MappedfacilityforRoom;

            });


        }



        $scope.cancel = function () {
            $state.reload();
        }

        //edit


        $scope.edit_Roomdata = function (bil) {
            var data = {
                "HRMRM_Id": bil.hrmrM_Id,
            }
            apiService.create("HS_Master/edit_Roomdata", data).
                then(function (promise) {
                    $scope.edit_Roomdatalist = promise.edit_Roomdatalist;
                    $scope.hrmrM_Id = promise.edit_Roomdatalist[0].hrmrM_Id;
                    $scope.hrmrM_RoomNo = promise.edit_Roomdatalist[0].hrmrM_RoomNo;
                    $scope.hrmrM_BedCapacity = promise.edit_Roomdatalist[0].hrmrM_BedCapacity;
                    $scope.hlmH_Id = promise.edit_Roomdatalist[0].hlmH_Id;
                    $scope.Floor();
                    $scope.hlmF_Id = promise.edit_Roomdatalist[0].hlmF_Id;
                    $scope.Floordetails();
                    $scope.hrmR_Desc = promise.edit_Roomdatalist[0].hrmR_Desc;
                    $scope.hrmR_BedCapacity = promise.edit_Roomdatalist[0].hrmR_BedCapacity;
                    $scope.HRMRM_SharingFlg = promise.edit_Roomdatalist[0].hrmrM_SharingFlg;
                    $scope.HRMRM_ACFlg = promise.edit_Roomdatalist[0].hrmrM_ACFlg;
                    $scope.hrmrM_RoomDesc = promise.edit_Roomdatalist[0].hrmrM_RoomDesc;
                    $scope.hlmrcA_Id = promise.edit_Roomdatalist[0].hlmrcA_Id;

                    if (promise.edit_Roomdatalist[0].hrmrM_RoomForStudentFlg == true) {
                        $scope.hrmrM_RoomForStudentFlg = 1;
                    }
                    if (promise.edit_Roomdatalist[0].hrmrM_RoomForStaffFlg == true) {
                        $scope.hrmrM_RoomForStaffFlg = 1;
                    }
                    if (promise.edit_Roomdatalist[0].hrmrM_RoomForGuestFlg == true) {
                        $scope.hrmrM_RoomForGuestFlg = 1;
                    }

                    angular.forEach($scope.facilities_list, function (tt) {
                        angular.forEach(promise.editfaclist, function (ss) {
                            if (tt.hlmftY_Id == ss.hlmftY_Id) {
                                tt.select = true;
                            }
                        })
                    })
                })
        };


        //====================================deactive
        $scope.deactive_Roomdata = function (user, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var dystring = "";
            if (user.hrmrM_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hrmrM_ActiveFlag === false) {
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
                        apiService.create("HS_Master/deactive_Roomdata", user).
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
        };



        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //==================== Selection Option
        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.facilities_list, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.facilities_list.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired23 = function () {
            return !$scope.facilities_list.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.hlmftY_FacilityName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }
        //======================= End


    }

})();



