
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_MasterStoreController', INV_MasterStoreController);
    INV_MasterStoreController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_MasterStoreController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



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
            apiService.getURI("INV_MasterStore/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
                    $scope.empname_list = promise.empname_list;
                    $scope.presentCountgrid = $scope.get_store.length;
                })
        };
        //---------------------------------Save--------------------------------------------
        //Tax
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMS_StoreName": $scope.invmS_StoreName,
                    "INVMS_StoreLocation": $scope.invmS_StoreLocation,
                    "INVMS_ContactPerson": $scope.invmS_ContactPerson,
                    "INVMS_ContactNo": $scope.invmS_ContactNo,
                    "INVMST_Id": $scope.invmsT_Id,
                    "HRME_Id": $scope.hrmE_id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_MasterStore/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmsT_Id == 0 || promise.invmsT_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmsT_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmsT_Id == 0 || promise.invmsT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmsT_Id > 0) {
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
            $scope.INVMST_Id = item.invmsT_Id;
            var dystring = "";
            if (item.invmS_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmS_ActiveFlg == false) {
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
                        apiService.create("INV_MasterStore/deactive", item).
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
            $scope.hrmE_id = "";
            $scope.employeename = "";
            $scope.invmS_StoreName = item.invmS_StoreName;
            $scope.invmS_StoreLocation = item.invmS_StoreLocation;
            $scope.invmS_ContactPerson = item.invmS_ContactPerson;
            $scope.invmS_ContactNo = item.invmS_ContactNo;
            $scope.invmsT_Id = item.invmsT_Id;
           // $scope.hrmE_id = item.hrmE_Id;
            $scope.empname_list = $scope.empname_list; 
            angular.forEach($scope.empname_list, function (aa) {
                if (aa.hrmE_Id == item.hrmE_Id) {
                    
                    $scope.hrmE_id = aa.hrmE_Id;
                    $scope.employeename = aa.employeename;

                }
            })
            
        }


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

        //$scope.filterValue = function (obj) {
        //    return (angular.lowercase(obj.invmS_StoreName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.invmS_StoreLocation)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (JSON.stringify(obj.invmS_ContactNo)).indexOf($scope.search) >= 0 ||
        //        (angular.lowercase(obj.invmS_ContactPerson)).indexOf(angular.lowercase($scope.searchValue)) >= 0 
        //}

    }
})();