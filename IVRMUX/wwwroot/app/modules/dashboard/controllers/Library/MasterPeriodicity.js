(function () {
    'use strict';
    angular
.module('app')
        .controller('MasterPeriodicityController', MasterPeriodicityController)

    MasterPeriodicityController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterPeriodicityController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;
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
            apiService.getURI("MasterPeriodicity/getdetails", pageid).then(function (promise) {
                $scope.ccategorylist = promise.periodlist;
            })
          
        }
         //=====================End-----Load--data----//


         //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "LMPE_Id": $scope.lmpE_Id,
                    "LMPE_PeriodicityName": $scope.LMPE_PeriodicityName
                }
                apiService.create("MasterPeriodicity/Savedata", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.lmpE_Id > 0) {
                                    swal('Record Updated Successfully!!!','Update');
                                }
                                else {
                                    swal('Record Saved Successfully!!!','Save');
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.lmpE_Id > 0) {
                                        swal('Record Not Updated Successfully!!!', 'Not Update');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!', 'Not Save');
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
            $scope.lmpE_Id = user.lmpE_Id;
            $scope.LMPE_PeriodicityName = user.lmpE_PeriodicityName;
        }
        //====================End---edit-record....
        


         //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.LMPE_Id = user.LMPE_Id;

            var dystring = "";
            if (user.lmpE_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmpE_ActiveFlg == 0) {
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
                    apiService.create("MasterPeriodicity/deactiveY", user).
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

