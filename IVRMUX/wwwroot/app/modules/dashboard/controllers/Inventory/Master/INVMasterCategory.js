
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INVMasterCategoryController', INVMasterCategoryController);
    INVMasterCategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INVMasterCategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.dayorderlist) {
                    $scope.dayorderlist[index].invmC_Level = Number(index) + 1;

                }


            }
        };

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

        $scope.closere = function () {
            $('#myModalreadmit').modal('hide');
            $state.reload();
        }
        $scope.getyearorder = function () {

            apiService.getDATA("INVMasterCategory/getorder").then(function (promise) {
                if (promise != null) {
                    $scope.dayorderlist = promise.categorylist
                    if (promise.categorylist != null && promise.categorylist.length > 0) {
                        $scope.dayorderlist = promise.categorylist
                        $scope.details = true;
                    } else {
                        swal("No Records Found");
                        $scope.details = false;
                    }
                } else {
                    swal("No Records Found");
                    $scope.details = false;
                }
            })
        }

        $scope.dayorderlist = [];
        $scope.saveorder = function (newuser2) {
            var data = {
                "ordeidss": $scope.dayorderlist
            }
            apiService.create("INVMasterCategory/saveorder", data).then(function (promise) {
                if (promise != null) {
                    if (promise.returnval == true) {
                        swal("Updated Successfully")
                    }
                    else {
                        swal("Failed To Update")
                    }
                    $state.reload();
                }
            })
        }





        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INVMasterCategory/getloaddata", pageid).
                then(function (promise) {
                    $scope.categorylist = promise.categorylist;
                    
                })
        };
        //---------------------------------Save--------------------------------------------
        //UOM
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMC_CategoryName": $scope.INVMC_CategoryName,
                    "INVMC_AliasName": $scope.INVMC_AliasName,
                    "INVMC_ParentId": $scope.INVMC_ParentId,
                    "INVMC_Level": $scope.INVMC_Level,
                    "INVMC_Id": $scope.INVMC_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INVMasterCategory/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmC_Id == 0 || promise.invmC_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmC_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmC_Id == 0 || promise.invmC_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmC_Id > 0) {
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
            $scope.INVMC_Id = item.invmC_Id;
            var dystring = "";
            if (item.invmC_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmC_ActiveFlg == false) {
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
                        apiService.create("INVMasterCategory/deactive", item).
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
            $scope.INVMC_CategoryName = item.invmC_CategoryName;
            $scope.INVMC_AliasName = item.invmC_AliasName;
            $scope.INVMC_ParentId = item.invmC_ParentId;
            $scope.INVMC_Level = item.invmC_Level;
            $scope.INVMC_Id = item.invmC_Id;
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