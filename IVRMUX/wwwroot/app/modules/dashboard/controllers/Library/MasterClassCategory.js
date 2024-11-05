(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterClassCategoryController', MasterClassCategoryController)

    MasterClassCategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterClassCategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
            apiService.getURI("MasterClassCategory/getdetails", pageid).then(function (promise) {
              
                $scope.alldata = promise.alldata;
                $scope.stafflist = promise.stafflist;
            })
        }
        //=====================End-----Load-data----//



        //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "LMCC_Id": $scope.lmcC_Id,
                    "LMCC_CategoryName": $scope.lmcC_CategoryName,
                    "IVRMUL_Id": $scope.ivrmuL_Id,
                   
                }
                apiService.create("MasterClassCategory/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {                                
                                    if (promise.returnval == true) {
                                        if ($scope.lmcC_Id > 0) {
                                            swal('Record Updated Successfully!!!');
                                        }
                                        else {
                                            swal('Record Saved Successfully!!!');
                                        }

                                    }
                                    else {
                                        if (promise.returnval == false) {
                                            if ($scope.lmcC_Id > 0) {
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
            $scope.lmcC_Id = user.lmcC_Id;
            $scope.lmcC_CategoryName = user.lmcC_CategoryName;
            $scope.ivrmuL_Id = user.ivrmuL_Id;
            

        }
        //====================End---edit-record....



        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmcC_Id = user.lmcC_Id;

            var dystring = "";
            if (user.lmcC_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmcC_ActiveFlag == 0) {
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
                        apiService.create("MasterClassCategory/deactiveY", user).
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

