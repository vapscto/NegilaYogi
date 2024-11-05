
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_EmployeeDetailsController', INV_EmployeeDetailsController);
    INV_EmployeeDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_EmployeeDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



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
            apiService.getURI("INV_MasterTax/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_tax = promise.get_tax;
                    $scope.presentCountgrid = $scope.get_tax.length;
                })
        };
        //---------------------------------Save--------------------------------------------
        //Tax
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMT_TaxName": $scope.invmT_TaxName,
                    "INVMT_TaxAliasName": $scope.invmT_TaxAliasName,
                    "INVMT_TaxAddress": $scope.invmT_TaxAddress,
                    "INVMT_TaxEmail": $scope.invmT_TaxEmail,
                    "INVMT_TaxPhoneNumber": $scope.invmT_TaxPhoneNumber,
                    "INVMT_TaxAccountNumber": $scope.invmT_AccountNumber,                   
                    "INVMT_Id": $scope.invmT_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_EmployeeDetails/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmT_Id == 0 || promise.invmT_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmT_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmT_Id == 0 || promise.invmT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmT_Id > 0) {
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
            $scope.INVMT_Id = item.invmT_Id;
            var dystring = "";
            if (item.invmT_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmT_ActiveFlg == false) {
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
                        apiService.create("INV_MasterTax/deactive", item).
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
            $scope.invmT_TaxName = item.invmT_TaxName;
            $scope.invmT_TaxAliasName = item.invmT_TaxAliasName;
            $scope.invmT_Address = item.invmT_Address;
            $scope.invmT_Email = item.invmT_Email;
            $scope.invmT_PhoneNumber = item.invmT_PhoneNumber;
            $scope.invmT_AccountNumber = item.invmT_AccountNumber;
            $scope.invmT_Id = item.invmT_Id;
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

        //$scope.filterValue = function (obj) {
        //    return (angular.lowercase(obj.invmT_TaxName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.invmT_TaxAliasName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 
        //}

    }
})();