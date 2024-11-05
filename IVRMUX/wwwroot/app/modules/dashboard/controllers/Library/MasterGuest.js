(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterGuestController', MasterGuestController)

    MasterGuestController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterGuestController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;


        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("MasterGuest/getdetails", pageid).then(function (promise) {
                $scope.ccategorylist = promise.pulishlist;
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
                    "Guest_Id": $scope.Guest_Id,
                    "Guest_Name": $scope.Guest_Name,
                    "Guest_Phone_No": $scope.Guest_Phone_No,
                    "Guest_Email_Id": $scope.Guest_Email_Id,
                    "Guest_address": $scope.Guest_address
                }
                apiService.create("MasterGuest/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.Guest_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.Guest_Id > 0) {
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
            $scope.Guest_Id = user.guest_Id;
            $scope.Guest_Name = user.guest_Name;
            $scope.Guest_Phone_No = user.guest_Phone_No;
            $scope.Guest_Email_Id = user.guest_Email_Id;
            $scope.Guest_address = user.guest_address;

            
        }
        //====================End---edit-record....


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.Guest_Id = user.guest_Id;

            var dystring = "";
            if (user.guest_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.guest_ActiveFlag == 0) {
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
                        apiService.create("MasterGuest/deactiveY", user).
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

