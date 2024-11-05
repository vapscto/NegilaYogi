(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterLocationController', MasterLocationController)

    MasterLocationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterLocationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
            apiService.getURI("MasterLocation/getdetails", pageid).then(function (promise) {
           
                $scope.getdata = promise.getdata;
               
            })
        }
        //=====================End-----Load--data----//


        //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "VMML_Location": $scope.vmmL_Location,
                    "VMML_LocationDescription": $scope.vmmL_LocationDescription,
                    "VMML_LocationFacilities": $scope.vmmL_LocationFacilities,
                    "VMML_Id": $scope.vmmL_Id,
                   
                }
                apiService.create("MasterLocation/saveRecorddata", data).then(function (promise) {
                        if (promise.returnval != null /*&& promise.duplicate != null*/) {
                            //if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.vmmL_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.vmmL_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            //}
                            //else {
                            //    swal("Record already exist");
                            //}
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
                "VMML_Id": user.vmmL_Id
            }

            apiService.create("MasterLocation/editrecord", data)
                .then(function (promise) {
                    if (promise.editlist.length > 0) {
                        $scope.vmmL_Id = promise.editlist[0].vmmL_Id;
                        $scope.vmmL_Location = promise.editlist[0].vmmL_Location;
                        $scope.vmmL_LocationDescription = promise.editlist[0].vmmL_LocationDescription;
                        $scope.vmmL_LocationFacilities = promise.editlist[0].vmmL_LocationFacilities;
                      
                    }
                })

        }
        //====================End---edit-record....



        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.vmmL_Id = user.vmmL_Id;

            var dystring = "";
            if (user.vmmL_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.vmmL_ActiveFlg == 0) {
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
                        apiService.create("MasterLocation/deactiveY", user).
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

