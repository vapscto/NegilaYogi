(function () {
    'use strict';
    angular
.module('app')
        .controller('MasterLanguageController', MasterLanguageController)

    MasterLanguageController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterLanguageController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;


        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("MasterLanguage/getdetails", pageid).then(function (promise) {
                $scope.ccategorylist = promise.langlist;
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
                    "LML_Id": $scope.lmL_Id,
                    "LML_LanguageName": $scope.lmL_LanguageName
                }
                apiService.create("MasterLanguage/Savedata", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.lmL_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Inserted Successfully!!!');
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.lmL_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Inserted Successfully!!!');
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
            $scope.lmL_Id = user.lmL_Id;
            $scope.lmL_LanguageName = user.lmL_LanguageName;
        }
        //====================End---edit-record....



         //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmL_Id = user.lmL_Id;

            var dystring = "";
            if (user.lmL_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmL_ActiveFlg == 0) {
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
                    apiService.create("MasterLanguage/deactiveY", user).
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

