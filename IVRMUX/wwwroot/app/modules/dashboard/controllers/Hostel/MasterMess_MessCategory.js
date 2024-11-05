(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterMess_MessCategoryController', MasterMess_MessCategoryController)
    MasterMess_MessCategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter']
    function MasterMess_MessCategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter) {
        $scope.submitted = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.Loaddata = function () {           
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("MasterMess_MessCategory/get_Mmessdata", pageid).then(function (promise) {
                $scope.get_messlistmapping = promise.get_messlistmapping;
                $scope.get_messlist = promise.master_mess;
                $scope.mess_category = promise.mess_category;

            })

        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }
        //=====================End-----Load--data----//
        //=====================saverecord....
        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.categoryselect = [];
            if ($scope.myForm.$valid) {
                var HLMMMC_Id = 0;
                if ($scope.HLMMMC_Id > 0) {
                    HLMMMC_Id = $scope.HLMMMC_Id;
                }
                if ($scope.mess_category != null && $scope.mess_category.length > 0) {
                    angular.forEach($scope.mess_category, function (itm) {
                        if (itm.selected) {
                            $scope.categoryselect.push({
                                HLMMC_Id: itm.hlmmC_Id
                            });
                        }
                    });
                    var data = {
                        "HLMM_Id": $scope.HLMM_Id,
                        "MatserMessArray": $scope.categoryselect,
                        "HLMMMC_Id": HLMMMC_Id
                    }
                    apiService.create("MasterMess_MessCategory/save_Mmessdata", data).then(function (promise) {

                        if (promise.returnval === true) {
                            if ($scope.HLMMMC_Id > 0) {
                                swal("Row  Update --" + promise.saverecord + "  duplicate " + promise.count + " ");

                            }
                            else {
                                swal("Row  Saved --" + promise.saverecord + "  duplicate " + promise.count + " ");
                            }
                            $state.reload();
                        }
                        else {
                            if (promise.returnval === false) {
                                if ($scope.HLMMMC_Id > 0) {
                                    swal('Record Not Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Not Saved Successfully!!!');
                                }
                            }
                        }

                    })
                }

               
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditData = function (user) {
            $scope.HLMMMC_Id = user.hlmmmC_Id;
            $scope.HLMM_Id = user.hlmM_Id;
            if ($scope.mess_category != null && $scope.mess_category.length > 0) {
                angular.forEach($scope.mess_category, function (itm) {
                    if (itm.hlmmC_Id == user.hlmmC_Id) {
                        itm.selected = true;
                    }
                    
                });
                
            }
            
        }

        //=================Activation/Deactivation
        $scope.deactiveY = function (user, SweetAlert) {

          //  $scope.HLMMMC_Id = user.hlmmmC_Id;

            var dystring = "";
            if (user.hlmmC_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hlmmC_ActiveFlag === false) {
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
                        apiService.create("MasterMess_MessCategory/deactiveY_Mmessdata", user).
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
        $scope.togchkbxCCCC = function () {
            $scope.HLMMC_Id = $scope.mess_category.every(function (options) {
                return options.selected;
            });
        }
        $scope.all_checkCCCC = function () {
            var HLMMC_Id = $scope.HLMMC_Id;
            angular.forEach($scope.mess_category, function (itm) {
                itm.selected = HLMMC_Id;

            });
        }
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.mess_category.some(function (options) {
                return options.selected;
            });
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.facilitylist, function (itm) {
                itm.selected = checkStatus;
            });
        }
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.facilitylist.every(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.facilitylist.some(function (options) {
                return options.selected;
            });
        }
    }
})();

