(function () {
    'use strict';
    angular
        .module('app')
        .controller('MessMenuController', MessMenuController)

    MessMenuController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter']
    function MessMenuController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter) {

        $scope.submitted = false;
        //$scope.radio_flag = "Boys";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //=====================Load--data.............\\

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("HS_Master/get_MessMenudata", pageid).then(function (promise) {
                $scope.get_messCategorylist = promise.get_messCategorylist;

                $scope.get_messlist = promise.get_messlist;
                $scope.griddata = promise.griddata;

            });            
        }
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.ismeridian = true;
        //=====================End-----Load--data----//


        //=====================saverecord....
        $scope.savedata = function () {
         
            if ($scope.myForm.$valid) {                
                var data = {
                    "HLMMN_Id": $scope.hlmmN_Id,
                    "HLMMC_Id": $scope.HLMMC_Id,
                    "HLMM_Id": $scope.HLMM_Id,
                    "HLMMN_MenuName": $scope.HLMMN_MenuName,
                    "HLMMN_MenuDesc": $scope.HLMMN_MenuDesc,                   
                }

                apiService.create("HS_Master/save_MessMenudata", data).then(function (promise) {

                    if (promise.returnval !== null && promise.duplicate !== null) {
                        if (promise.duplicate === false) {
                            if (promise.returnval === true) {
                                if ($scope.hlmmN_Id > 0) {
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
                                    if ($scope.hlmmN_Id > 0) {
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
                        //$state.reload();
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

            var data = {
                "HLMMN_Id": user.hlmmN_Id
            }
            apiService.create("HS_Master/edit_MessMenudata", data).then(function (promise) {
                $scope.edit_MessMenulist = promise.edit_MessMenulist;

                $scope.hlmmN_Id = promise.edit_MessMenulist[0].hlmmN_Id;
                $scope.HLMM_Id = promise.edit_MessMenulist[0].hlmM_Id;
                $scope.HLMMC_Id = promise.edit_MessMenulist[0].hlmmC_Id;
                $scope.HLMMN_MenuName = promise.edit_MessMenulist[0].hlmmN_MenuName;
                $scope.HLMMN_MenuDesc = promise.edit_MessMenulist[0].hlmmN_MenuDesc;  
                
            });
        }
        //====================End
        

        //=================Activation/Deactivation
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.hlmmN_Id = user.hlmmN_Id;

            var dystring = "";
            if (user.hlmmN_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hlmmN_ActiveFlag === false) {
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
                        apiService.create("HS_Master/deactiveY_MessMenudata", user).
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
        }
        //================End----Activation/Deactivation--Record.........


        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }     
     


    }
})();

