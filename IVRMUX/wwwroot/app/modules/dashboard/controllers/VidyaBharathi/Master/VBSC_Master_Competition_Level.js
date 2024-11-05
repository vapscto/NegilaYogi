
(function () {
    'use strict';
    angular
        .module('app')
        .controller('VBSC_Master_Competition_LevelController', VBSC_Master_Competition_LevelController);
    VBSC_Master_Competition_LevelController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function VBSC_Master_Competition_LevelController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }
        $scope.levelwise = false;
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.obj = {};
        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("VBSC_Master_UOM/getloaddatalevel", pageid).
                then(function (promise) {
                    $scope.Master_trust = promise.master_trust;

                    $scope.get_levl = promise.get_levl;
                    $scope.presentCountgrid = $scope.get_levl.length;

                })
        };
        
        //---------------------------------Save--------------------------------------------
     //Competition level
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "VBSCMCL_SportsCCFlg": $scope.VBSCMCL_SportsCCFlg,
                    "MT_Id": $scope.obj.MT_Id,
                    "VBSCMCL_LevelFlg": $scope.vbscmcL_LevelFlg,
                    "VBSCMCL_CompetitionLevel": $scope.vbscmcL_CompetitionLevel,
                    "VBSCMCL_CLDesc": $scope.vbscmcL_CLDesc,

                }
                apiService.create("VBSC_Master_UOM/savedetailslevel", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.vbscmcL_Id == 0 || promise.vbscmcL_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.vbscmcL_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.vbscmcL_Id == 0 || promise.vbscmcL_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.vbscmcL_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
              $state.reload();
            
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.vbscmcL_Id = item.vbscmcL_Id;
            var dystring = "";
            if (item.vbscmcL_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.vbscmcL_ActiveFlag == false) {
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
                        apiService.create("VBSC_Master_UOM/deactivelevel", item).
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
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }



        $scope.edit = function (user) {
            $scope.vbscmcL_Id = user.vbscmcL_Id;
            $scope.VBSCMCL_SportsCCFlg = user.vbscmcL_SportsCCFlg;
            $scope.obj.MT_Id = user.mT_Id;
            $scope.vbscmcL_LevelFlg = user.vbscmcL_LevelFlg;
            $scope.vbscmcL_CompetitionLevel = user.vbscmcL_CompetitionLevel;
            $scope.vbscmcL_CLDesc = user.vbscmcL_CLDesc;
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;

        };
        $scope.searchValue = '';



    }
})();