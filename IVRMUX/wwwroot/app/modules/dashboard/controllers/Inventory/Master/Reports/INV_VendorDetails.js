
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_VendorDetailsController', INV_VendorDetailsController);
    INV_VendorDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_VendorDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



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
            apiService.getURI("INV_MasterSupplier/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_supplier = promise.get_supplier;
                    $scope.presentCountgrid = $scope.get_supplier.length;
                })
        };
        //---------------------------------Save--------------------------------------------
        //Tax
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMS_SupplierName": $scope.invmS_SupplierName,
                    "INVMS_SupplierCode": $scope.invmS_SupplierCode,
                    "INVMS_SupplierConatctPerson": $scope.invmS_SupplierConatctPerson,
                    "INVMS_SupplierConatctNo": $scope.invmS_SupplierConatctNo,
                    "INVMS_EmailId": $scope.invmS_EmailId,
                    "INVMS_GSTNo": $scope.invmS_GSTNo,
                    "INVMS_SupplierAddress": $scope.invmS_SupplierAddress,
                    "INVMS_Id": $scope.invmS_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_MasterSupplier/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmS_Id == 0 || promise.invmS_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmS_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmS_Id == 0 || promise.invmS_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmS_Id > 0) {
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
            $scope.INVMS_Id = item.invmS_Id;
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
                        apiService.create("INV_MasterSupplier/deactive", item).
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
            $scope.invmS_SupplierName = item.invmS_SupplierName;
            $scope.invmS_SupplierCode = item.invmS_SupplierCode;
            $scope.invmS_SupplierConatctPerson = item.invmS_SupplierConatctPerson;
            $scope.invmS_SupplierConatctNo = item.invmS_SupplierConatctNo;
            $scope.invmS_EmailId = item.invmS_EmailId;
            $scope.invmS_GSTNo = item.invmS_GSTNo;
            $scope.invmS_SupplierAddress = item.invmS_SupplierAddress;
            $scope.invmS_Id = item.invmS_Id;
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';



    }
})();