(function () {
    'use strict';
    angular
        .module('app')
        .controller('H_FloorController', H_FloorController)

    H_FloorController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function H_FloorController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


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

        //get data
        $scope.getAllDetail = function () {

            $scope.currentPage = 1;
            var pageid = 2;
            apiService.getURI("HS_Master/get_floordata", pageid)
                .then(function (promise) {
                    $scope.grid_Alldataforfloor = promise.grid_Alldataforfloor;
                    $scope.houstel_list = promise.houstel_list;
                    $scope.facilities_list = promise.facilities_list;
                })
        }

        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {
            debugger;
            $scope.selected_facilities = [];
            angular.forEach($scope.facilities_list, function (faci) {
                if (faci.select == true) {
                    $scope.selected_facilities.push({ HLMFTY_Id: faci.hlmftY_Id });
                }
            });

            if ($scope.myForm.$valid) {
                var data = {
                    "HLMF_Id": $scope.hlmF_Id,
                    "HLMH_Id": $scope.hlmH_Id,
                    "HRMF_FloorName": $scope.hrmF_FloorName,
                    "HRMF_TotalRooms": $scope.hrmF_TotalRooms,
                    "HRMF_FloorDesc": $scope.hrmF_FloorDesc,
                  

                    selected_facilities: $scope.selected_facilities,
                }
                apiService.create("HS_Master/save_Floordata", data).
                    then(function (promise) {
                        if (promise.returnval !== null && promise.duplicate !== null) {
                            if (promise.duplicate === false) {
                                if (promise.returnval === true) {
                                    if ($scope.hlmF_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                        $state.reload();
                                    }
                                    else {
                                        swal('Record Inserted Successfully!!!');
                                        $state.reload();
                                    }
                                }
                                else {
                                    if (promise.returnval === false) {
                                        if ($scope.hlmF_Id > 0) {
                                            swal('Record Not Updated Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Inserted Successfully!!!');
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

            apiService.create("HS_Master/get_Mappedfacility", user).then(function (promise) {
                $scope.mappedfacilitylist = promise.mappedfacilitylist;
                $scope.temparry = promise.mappedfacilitylist;

                $scope.mainarray = [];
                forEach($scope.mappedfacilitylist, function (tt) {
                    var subarry = [];
                    forEach($scope.temparry, function (ss) {
                        if (tt.hlmF_Id == ss.hlmF_Id) {
                            subarry.push(ss);
                        }
                    })     
                })
                if (subarry.length > 0) {
                    $scope.mainarray.push({
                        hlmF_Id: tt.hlmF_Id,
                        hrmF_FloorName: tt.hrmF_FloorName,
                        hlmH_Name: tt.hlmH_Name,
                        hrmF_TotalRooms: tt.hrmF_TotalRooms,
                        subarry: subarry
                    })
                }
                console.log($scope.mainarray);
            });

           
        }



        //================edit
       
        $scope.edit_floordata = function (bil) {
            
            var pageid = { "HLMF_Id": bil.hlmF_Id }
            apiService.create("HS_Master/edit_floordata", pageid).
                then(function (promise) {
                    $scope.edit_floorlist = promise.edit_floorlist;
                    $scope.facilty_list =promise.facilty_list;
                     
                    $scope.hlmF_Id = promise.edit_floorlist[0].hlmF_Id;
                    $scope.hlmH_Id = promise.edit_floorlist[0].hlmH_Id;
                    $scope.hrmF_FloorName = promise.edit_floorlist[0].hrmF_FloorName;
                    $scope.hrmF_TotalRooms = promise.edit_floorlist[0].hrmF_TotalRooms;
                    $scope.hrmF_FloorDesc = promise.edit_floorlist[0].hrmF_FloorDesc;

                    $scope.hrmF_FName = promise.edit_floorlist[0].hrmF_FName;
                    $scope.hrmF_TotalRoom = promise.edit_floorlist[0].hrmF_TotalRoom;
                    $scope.hrmF_Desc = promise.edit_floorlist[0].hrmF_Desc;
                    

                    angular.forEach($scope.facilities_list, function (tt) {
                        angular.forEach($scope.facilty_list, function (ss) {
                            if (tt.hlmftY_Id == ss.hlmftY_Id) {
                                tt.select = true;
                            }
                        })
                    })
                    
                })
        };


        $scope.cancel = function () {
            $state.reload();
        }

        //================================deactive

        $scope.deactive = function (user, SweetAlert) {
            debugger;
            $scope.hrmF_Id = user.hrmF_Id;

            var dystring = "";
            if (user.hrmF_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hrmF_ActiveFlag === false) {
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
                        apiService.create("HS_Master/deactivate_Floordata", user).
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
            return $scope.submitted;
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



