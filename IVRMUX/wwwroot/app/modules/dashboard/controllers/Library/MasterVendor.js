(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterVendorController', MasterVendorController)

    MasterVendorController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterVendorController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;


        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("MasterVendor/getdetails", pageid).then(function (promise) {
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
                    "LMV_Id": $scope.lmV_Id,
                    "LMV_VendorName": $scope.lmV_VendorName,
                    "LMV_MobileNo": $scope.lmV_MobileNo,
                    "LMV_EMailId": $scope.lmV_EMailId,
                    "LMV_PhoneNo": $scope.lmV_PhoneNo,
                    "LMV_Address": $scope.lmV_Address
                }
                apiService.create("MasterVendor/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.lmV_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.lmV_Id > 0) {
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
            $scope.lmV_Id = user.lmV_Id;
            $scope.lmV_VendorName = user.lmV_VendorName;
            $scope.lmV_MobileNo = user.lmV_MobileNo;
            $scope.lmV_EMailId = user.lmV_EMailId;
            $scope.lmV_PhoneNo = user.lmV_PhoneNo;
            $scope.lmV_Address = user.lmV_Address;
            
        }
        //====================End---edit-record....
       


         //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmV_Id = user.lmV_Id;

            var dystring = "";
            if (user.lmV_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmV_ActiveFlg == 0) {
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
                        apiService.create("MasterVendor/deactiveY", user).
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

