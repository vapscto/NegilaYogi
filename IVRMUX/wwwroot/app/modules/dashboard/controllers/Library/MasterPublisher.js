(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterPublisherController', MasterPublisherController)

    MasterPublisherController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterPublisherController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;


        //=====================Loaddata.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("MasterPublisher/getdetails", pageid).then(function (promise) {
                $scope.ccategorylist = promise.pulishlist;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
         //=====================End-----Loaddata----//



         //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "LMP_Id": $scope.lmP_Id,
                    "LMP_PublisherName": $scope.lmP_PublisherName,
                    "LMP_PhoneNo": $scope.lmP_PhoneNo,
                    "LMP_MobileNo": $scope.lmP_MobileNo,
                    "LMP_EMailId": $scope.lmP_EMailId,
                    "LMP_Address": $scope.lmP_Address
                }
                apiService.create("MasterPublisher/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.lmP_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.lmP_Id > 0) {
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
            $scope.lmP_Id = user.lmP_Id;
            $scope.lmP_PublisherName = user.lmP_PublisherName;           
            $scope.lmP_PhoneNo = user.lmP_PhoneNo;
            $scope.lmP_MobileNo = user.lmP_MobileNo;
            $scope.lmP_EMailId = user.lmP_EMailId;
            $scope.lmP_Address = user.lmP_Address;
        }
         //====================End---editrecord....


       

        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmP_Id = user.lmP_Id;

            var dystring = "";
            if (user.lmP_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmP_ActiveFlg == 0) {
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
                        apiService.create("MasterPublisher/deactiveY", user).
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
            return $scope.submitted;
        };

          //===========----Clear Field.........
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

