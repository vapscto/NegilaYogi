
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_MasterCustomerController', INV_MasterCustomerController);
    INV_MasterCustomerController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_MasterCustomerController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



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
            apiService.getURI("INV_MasterCustomer/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_customer = promise.get_customer;
                    $scope.presentCountgrid = $scope.get_customer.length;
                })
        };
        //---------------------------------Save--------------------------------------------
        //Tax
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMC_CustomerName": $scope.invmC_CustomerName,
                    "INVMC_CustomerContactPerson": $scope.invmC_CustomerContactPerson,
                    "INVMC_CustomerContactNo": $scope.invmC_CustomerContactNo,
                    "INVMC_CustomerAddress": $scope.invmC_CustomerAddress,
                    "INVMC_Id": $scope.invmC_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_MasterCustomer/savedetails", data).then(function (promise) {

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
                        apiService.create("INV_MasterCustomer/deactive", item).
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
            $scope.invmC_CustomerName = item.invmC_CustomerName;
            $scope.invmC_CustomerContactPerson = item.invmC_CustomerContactPerson;
            $scope.invmC_CustomerContactNo = item.invmC_CustomerContactNo;
            $scope.invmC_CustomerAddress = item.invmC_CustomerAddress;
            $scope.invmC_Id = item.invmC_Id;
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

        //$scope.filterValue = function (obj) {
        //    return (angular.lowercase(obj.invmC_CustomerName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.invmC_CustomerContactPerson)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.invmC_CustomerAddress)).indexOf(angular.lowercase($scope.searchValue)) >= 0 
        //}

    }
})();