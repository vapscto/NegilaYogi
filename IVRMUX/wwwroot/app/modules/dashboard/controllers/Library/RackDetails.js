(function () {
    'use strict';
    angular
        .module('app')
        .controller('RackDetailsController', RackDetailsController)

    RackDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function RackDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;


        //------------for sorting.........
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
            apiService.getURI("RackDetails/getdetails", pageid).then(function (promise) {
                //$scope.florlst = promise.floorlist;
                $scope.sublst = promise.subjectlist;
                $scope.alldata = promise.alldata;
            })
        }
          //=====================End-----Load--data----//
        

          //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "LMRA_Id": $scope.lmrA_Id,
                    "LMRA_BuildingName": $scope.lmrA_BuildingName,
                    "LMRAS_Id": $scope.lmraS_Id,
                    "LMRA_RackName": $scope.lmrA_RackName,
                    "LMRA_FloorName": $scope.lmrA_FloorName,
                    "LMRA_DisplayColour": $scope.lmrA_DisplayColour,
                    "LMRA_Location": $scope.lmrA_Location,
                    "LMS_Id": $scope.lmS_Id
                }
                apiService.create("RackDetails/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.lmrA_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.lmrA_Id > 0) {
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
            var data = {
                "LMRA_Id": user.lmrA_Id
            }
            
                apiService.create("RackDetails/EditData", data)
                    .then(function (promise)
                    {
                        if (promise.updatelist.length > 0) {
                            $scope.lmrA_Id = promise.updatelist[0].lmrA_Id;
                            $scope.lmraS_Id = promise.updatelist[0].lmraS_Id;
                            $scope.lmS_Id = promise.updatelist[0].lmS_Id;
                            $scope.lmrA_RackName = promise.updatelist[0].lmrA_RackName;
                            $scope.lmrA_FloorName = promise.updatelist[0].lmrA_FloorName;
                            $scope.lmrA_Location = promise.updatelist[0].lmrA_Location;
                            $scope.lmrA_DisplayColour = promise.updatelist[0].lmrA_DisplayColour;
                            $scope.lmrA_BuildingName = promise.updatelist[0].lmrA_BuildingName;
                        }
                    })
            
        }
          //====================End---edit-record....
        
        

        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmrA_Id = user.lmrA_Id;

            var dystring = "";
            if (user.lmrA_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmrA_ActiveFlag == 0) {
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
                        apiService.create("RackDetails/deactiveY", user).
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

