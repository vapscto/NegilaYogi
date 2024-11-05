(function () {
    'use strict';
    angular
        .module('app')
        .controller('H_BedController', H_BedController)
    H_BedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function H_BedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'HLMF_Id';
        $scope.sortReverse = true;
        $scope.obj = {};

        //var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        ////if (ivrmcofigsettings.length > 0) {
        ////    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        ////}
        ////else {
        ////    paginationformasters = 0
        ////}

        //$scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //get data
        $scope.getAllDetail = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("Training_Master/getdata_F", pageid).then(function (promise) {
                $scope.floor_lists = promise.floor_lists;
                $scope.room = promise.room;
                $scope.hostel_list = promise.hostel_list;
                $scope.floor = promise.floor;            
          
            })
        };
        $scope.floor = function () {
            $scope.floor_list = [];
            var data = {
                "HLMH_Id": $scope.hlmH_Id,
            }
            apiService.create("HostelAllotForCLGStudent/floor", data).then(function (promise) {
                if (promise.floor_list != null && promise.floor_list.length > 0) {
                    $scope.floor = promise.floor_list;
                } else {
                    swal('Record Not Available!');
                }
            });
        };
        $scope.room = function () {
            $scope.room_Details = [];
            $scope.HRMRM_RoomNoONE = "";
            $scope.HRMRM_BedCapacity = "";
            $scope.AllotedCount = "";
            $scope.AvailableBedCapacity = "";
            $scope.obj.HRMRM_Id = "";
            var data = {
                "HLMF_Id": $scope.hlmF_Id
            }
            apiService.create("HostelAllotForCLGStudent/roomForVacateReport", data).then(function (promise) {

                if (promise.room_list != null && promise.room_list.length > 0) {
                    $scope.room = promise.room_list;
                }
            });
        }
        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    //"HRMF_FName": $scope.Floor,
                    //"HRMB_Id": $scope.hrmB_Id,
                    //"HRMF_Id": $scope.Id
                    "HLMB_Id": $scope.hlmB_Id,
                    "HLMH_Id": $scope.hlmH_Id,
                    "HLMB_BedName": $scope.hlmB_BedName,
                    "HLMF_Id": $scope.hlmF_Id,
                    "HRMRM_Id": $scope.hrmrM_Id,
                    "HLMB_MattressFlg": $scope.hlmB_MattressFlg,
                    "HLMB_BedSheetFlg": $scope.hlmB_BedSheetFlg,
                    "HLMB_PillowFlg": $scope.hlmB_PillowFlg,
                    "HLMB_StudyTableFlg": $scope.hlmB_StudyTableFlg,
                    "HLMB_LampFlg": $scope.hlmB_LampFlg,
                }
                apiService.create("Training_Master/SaveEdit_F", data).
                    then(function (promise) {
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
                    })

            }
            else {

                $scope.submitted = true;
            }

        };



        //===================get  Mapped  facility
        $scope.get_Mappedfacility = function (user) {
            apiService.create("Training_Master/get_Mappedfacility", user).then(function (promise) {
                $scope.get_MappedfacilityforRoom = promise.get_MappedfacilityforRoom;
                $scope.temparry = promise.get_MappedfacilityforRoom;
                $scope.mainarray = [];
                forEach($scope.get_MappedfacilityforRoom, function (tt) {
                    var subarry = [];
                    forEach($scope.temparry, function (ss) {
                        if (tt.hrmrM_Id == ss.hrmrM_Id) {
                            subarry.push(ss);
                        }
                    })
                })
            });
        }

        $scope.edit = function (record) {
            var data = {
                "HLMB_Id": record.hlmB_Id,
                "HLMH_Id": record.hlmH_Id,
            }

            apiService.create("Training_Master/details_F", data).
                then(function (promise) {
                    $scope.hlmH_Id = promise.floor_lists[0].hlmH_Id;
                    $scope.hlmB_BedName = promise.floor_lists[0].hlmB_BedName;
                    $scope.hlmF_Id = promise.floor_lists[0].hlmF_Id;
                    $scope.hlmB_Id = promise.floor_lists[0].hlmB_Id;
                    $scope.hrmrM_Id = promise.floor_lists[0].hrmrM_Id;
                    $scope.hrmF_FloorName = promise.floor_lists[0].hrmF_FloorName;
                    $scope.hrmrM_RoomNo = promise.floor_lists[0].hrmrM_RoomNo; 
                    $scope.hlmB_MattressFlg = promise.floor_lists[0].hlmB_MattressFlg;
                    $scope.hlmB_BedSheetFlg = promise.floor_lists[0].hlmB_BedSheetFlg;
                    $scope.hlmB_PillowFlg = promise.floor_lists[0].hlmB_PillowFlg;
                    $scope.hlmB_StudyTableFlg = promise.floor_lists[0].hlmB_StudyTableFlg;
                    $scope.hlmB_LampFlg = promise.floor_lists[0].hlmB_LampFlg;
                    $scope.getAllDetail();
                    //getAllDetail
                })
        };




        $scope.deactive_Roomdata = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmrM_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Training_Master/deactive_Roomdata", data.hrmrM_Id).
                            then(function (promise) {


                                if (promise.retrunMsg !== "") {

                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }

                                    $scope.getAllDetail();
                                }

                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }

            );
        }


        $scope.cancel = function () {
            $state.reload();
        }


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();






































