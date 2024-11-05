(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterTimeSlabController', MasterTimeSlabController)

    MasterTimeSlabController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterTimeSlabController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;


        //--------for sorting....
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("MasterTimeSlab/getdetails", pageid).then(function (promise) {

                
                $scope.alldata = promise.alldata;

            })
        }
         //=====================End-----Load--data----//


         //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {

                if ($scope.Slab_FineType == 1) {
                    $scope.Slab_FineType ='p';
                }
                else {
                    $scope.Slab_FineType = ' ';
                }
                var data = {
                    "LFSE_Id": $scope.lfsE_Id,
                    "LFSE_UserType": $scope.lfsE_UserType,
                    "LFSE_PerDayFlg": $scope.lfsE_PerDayFlg,
                    "LFSE_FromDay": $scope.lfsE_FromDay,
                    "LFSE_ToDay": $scope.lfsE_ToDay,
                    "LFSE_Amount": $scope.lfsE_Amount,
                    "LFSE_SlabTypeFlg": $scope.lfsE_SlabTypeFlg,
                 
                }
                apiService.create("MasterTimeSlab/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.lfsE_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.lfsE_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
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
            debugger;
            $scope.lfsE_Id = user.lfsE_Id,
            $scope.lfsE_UserType = user.lfsE_UserType;
            $scope.lfsE_PerDayFlg = user.lfsE_PerDayFlg;
            $scope.lfsE_FromDay = user.lfsE_FromDay;
            $scope.lfsE_ToDay = user.lfsE_ToDay;
            $scope.lfsE_Amount = user.lfsE_Amount;
            $scope.lfsE_SlabTypeFlg = user.lfsE_SlabTypeFlg;

            if (user.lfsE_PerDayFlg == true) {
                $scope.lfsE_PerDayFlg = 1;
            }
            else {
                $scope.lfsE_PerDayFlg = 0;
            }
         
        }
        //====================End---edit-record....



         //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lfsE_Id = user.lfsE_Id;

            var dystring = "";
            if (user.lfsE_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.lfsE_ActiveFlg == 0) {
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
                        apiService.create("MasterTimeSlab/deactiveY", user).
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

