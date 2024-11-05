(function () {
    'use strict';
    angular
        .module('app')
        .controller('Room_Tariff', Room_Tariff)

    Room_Tariff.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function Room_Tariff($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


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
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            var pageid = 2;
            apiService.getURI("Room_Tariff/loaddata", pageid)
                .then(function (promise) {
                    $scope.yeralist = promise.yeralist;
                    $scope.room_list = promise.room_list;
                    $scope.gridlistdata = promise.gridlistdata;

                })
        }

        //save
        $scope.submitted = false;
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "HLMRTF_Id": $scope.HLMRTF_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "HRMRM_Id": $scope.HRMRM_Id,
                    "HLMRTF_SORate": $scope.HLMRTF_SORate,
                    "HLMRTF_RoomRate": $scope.HLMRTF_RoomRate,

                }
                apiService.create("Room_Tariff/savedata", data).
                    then(function (promise) {
                        if (promise.returnval !== null && promise.duplicate !== null) {
                            if (promise.duplicate === false) {
                                if (promise.returnval === true) {
                                    if ($scope.HLMRTF_Id > 0) {
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
                                        if ($scope.HLMRTF_Id > 0) {
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


        $scope.cancel = function () {
            $state.reload();
        }

        //edit


        $scope.edit = function (bil) {
            var data = {
                "HLMRTF_Id": bil.hlmrtF_Id,
            }
            apiService.create("Room_Tariff/editdata", data).
                then(function (promise) {
                    $scope.editlist = promise.editlist;

                    $scope.HLMRTF_Id = promise.editlist[0].hlmrtF_Id;
                    $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;
                    $scope.HRMRM_Id = promise.editlist[0].hrmrM_Id;
                    $scope.HLMRTF_SORate = promise.editlist[0].hlmrtF_SORate;
                    $scope.HLMRTF_RoomRate = promise.editlist[0].hlmrtF_RoomRate;

                });
        };


        //====================================deactive
        $scope.Ydeactive = function (user, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var dystring = "";
            if (user.hlmrtF_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hlmrtF_ActiveFlag === false) {
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
                        apiService.create("Room_Tariff/Ydeactive", user).
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


        $scope.singlebedflag = true;

        $scope.bedflag_enable = function () {
            if ($scope.HRMRM_Id != null || $scope.HRMRM_Id != undefined || $scope.HRMRM_Id != '') {
                $scope.singlebedflag = false;
            }
        }


        $scope.get_totalamount = function () {
           
            var data = {
                "HRMRM_Id": $scope.HRMRM_Id,
                "HLMRTF_SORate": $scope.HLMRTF_SORate,
            }

            apiService.create("Room_Tariff/get_bedcount", data).then(function (promise) {
               
                $scope.HRMRM_BedCapacity = promise.hrmrM_BedCapacity;
                if ($scope.HRMRM_BedCapacity != null || $scope.HRMRM_BedCapacity != undefined) {
                    var price = 0;
                    var price = $scope.HLMRTF_SORate * $scope.HRMRM_BedCapacity;
                    $scope.HLMRTF_RoomRate = price;

                }

            });
        }



    }

})();



