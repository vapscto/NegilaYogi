(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterDonorController', MasterDonorController)

    MasterDonorController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterDonorController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

         //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("MasterDonor/getdetails", pageid).then(function (promise) {
                $scope.ccategorylist = promise.donorlist;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
            //=====================End-----Load--data----//


        //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "Donor_Id": $scope.Donor_Id,
                    "Donor_Name": $scope.Donor_Name,
                    "Donor_Address": $scope.Donor_Address,
                }
                apiService.create("MasterDonor/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.Donor_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.Donor_Id > 0) {
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


        //=====================Edit--record....
        $scope.EditData = function (user) {
            debugger;
            $scope.Donor_Id = user.donor_Id;
            $scope.Donor_Name = user.donor_Name;
            $scope.Donor_Address = user.donor_Address;

        }
         //====================End---edit-record....
      
        

         //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.Donor_Id = user.donor_Id;

            var dystring = "";
            if (user.donor_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.donor_ActiveFlag == 0) {
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
                        apiService.create("MasterDonor/deactiveY", user).
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

