
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_MasterUOMController', INV_MasterUOMController);
    INV_MasterUOMController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_MasterUOMController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_MasterUOM/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_uom = promise.get_uom;
                    $scope.presentCountgrid = $scope.get_uom.length;
                })
        };
        //---------------------------------Save--------------------------------------------
        //UOM
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMUOM_UOMName": $scope.invmuoM_UOMName,
                    "INVMUOM_UOMAliasName": $scope.invmuoM_UOMAliasName,
                    "INVMUOM_Qty": $scope.invmuoM_Qty,
                    "INVMUOM_Id": $scope.invmuoM_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_MasterUOM/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmuoM_Id == 0 || promise.invmuoM_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmuoM_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmuoM_Id == 0 || promise.invmuoM_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmuoM_Id > 0) {
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
            $scope.INVMUOM_Id = item.invmuoM_Id;
            var dystring = "";
            if (item.invmuoM_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmuoM_ActiveFlg == false) {
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
                        apiService.create("INV_MasterUOM/deactive", item).
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

        $scope.edit = function (item) {
            $scope.invmuoM_UOMName = item.invmuoM_UOMName;
            $scope.invmuoM_UOMAliasName = item.invmuoM_UOMAliasName;
            $scope.invmuoM_Qty = item.invmuoM_Qty;
            $scope.invmuoM_Id = item.invmuoM_Id;
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

        //$scope.filterValue = function (obj) {
        //    return (angular.lowercase(obj.invmuoM_UOMName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.invmuoM_UOMAliasName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.invmuoM_Qty)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        //}

    }
})();