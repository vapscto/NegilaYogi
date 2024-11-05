(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterAccessoriesController', MasterAccessoriesController)

    MasterAccessoriesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterAccessoriesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //=====================Load data..........
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("MasterAccessories/getdetails", pageid).then(function (promise) {

                $scope.alldata = promise.alldata;
            })
        }
        //=====================End-----Load-data----//



        //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "LMAC_Id": $scope.lmaC_Id,
                    "LMAC_AccessoriesName": $scope.lmaC_AccessoriesName,
                    "LMAC_AccessoriesDesc": $scope.lmaC_AccessoriesDesc,

                }
                apiService.create("MasterAccessories/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.lmaC_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.lmaC_Id > 0) {
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


        //=====================Edit-record....
        $scope.EditData = function (user) {
            debugger;
            $scope.lmaC_Id = user.lmaC_Id;
            $scope.lmaC_AccessoriesName = user.lmaC_AccessoriesName;
            $scope.lmaC_AccessoriesDesc = user.lmaC_AccessoriesDesc;


        }
        //====================End---edit-record....



        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmaC_Id = user.lmaC_Id;

            var dystring = "";
            if (user.lmaC_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmaC_ActiveFlg == 0) {
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
                        apiService.create("MasterAccessories/deactiveY", user).
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


        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

