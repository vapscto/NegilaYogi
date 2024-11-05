(function () {
    'use strict';
    angular
        .module('app')
        .controller('FAUserComapanyMappingController', FAUserComapanyMappingController)
    FAUserComapanyMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function FAUserComapanyMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.itemsPerPage = 10;

            var id = 1;
            apiService.getURI("FAMasterCompany/GetCompany", id).
                then(function (promise) {
                    $scope.companies = promise.fillcompany;

                 
                    $scope.userName = promise.fillfinacialUser;

                    $scope.UserCompanyDetails = promise.userCompanyDetails;
                });
        };

    


        $scope.delete = function (det, SweetAlert) {
            var data = {
                "FAUCM_Id": det.faucM_Id
            }
            var dystring = "";
            if (det.faucM_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (det.faucM_ActiveFlg == false) {
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
                        apiService.create("FAMasterCompany/deleteUserDetails", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not Active / Deactive !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact Administrator !');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }
        $scope.edit = function (det) {
            //  $scope.famcomP_Id = det.famcomP_Id;
            var id = det.faucM_Id;


            apiService.getURI("FAMasterCompany/editUserDetails/", id).
                then(function (promise) {

                    $scope.famcomP_Id = promise.userCompanyDetails[0].famcomP_Id;
                    $scope.faucM_Password = promise.userCompanyDetails[0].faucM_Password;

                    $scope.faucM_Id = promise.userCompanyDetails[0].faucM_Id;
                    $scope.muser_Id = promise.userCompanyDetails[0].user_Id;
                  

                })
        }
        $scope.submitted = false;
        $scope.saveDetails = function () {
            if ($scope.myForm.$valid) {
                var faucM_Id = 0;
                if ($scope.faucM_Id > 0) {
                    faucM_Id = $scope.faucM_Id;
                }
               

                var data = {
                    "FAUCM_Id": faucM_Id,


          
                    "FAMCOMP_Id": $scope.famcomP_Id,

                    "FAUCM_Password": $scope.faucM_Password,
                    "muser_Id": $scope.muser_Id
                   


                }
                apiService.create("FAMasterCompany/saveUserDetails", data)
                    .then(function (promise) {



                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated !');
                        }
                        else if (promise.returnval == "Duplicate") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact Administrator !');
                        }
                        $state.reload();



                    })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.clearid = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }
})();
