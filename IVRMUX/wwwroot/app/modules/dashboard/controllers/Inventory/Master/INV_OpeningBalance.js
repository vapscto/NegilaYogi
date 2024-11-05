
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_OpeningBalanceController', INV_OpeningBalanceController);
    INV_OpeningBalanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_OpeningBalanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var date = new Date();
        $scope.invoB_PurchaseDate = date;

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
            apiService.getURI("INV_OpeningBalance/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_Store = promise.get_Store;
                    $scope.get_item = promise.get_item;
                    $scope.get_openingbalance = promise.get_openingbalance;
                    $scope.presentCountgrid = $scope.get_openingbalance.length;
                })
        };

        //===========Item Change Transcation Grid
        $scope.onitemchange = function () {
            var item_id = $scope.invmI_Id;
            var data = {
                "INVMI_Id": item_id
            }
            apiService.create("INV_OpeningBalance/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    $scope.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_UOMName;
                    $scope.invmuom_id = $scope.get_itemDetail[0].invmuoM_Id;
                })
        }
        //---------------------------------Save--------------------------------------------

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMST_Id": $scope.invmsT_Id,
                    "INVMI_Id": $scope.invmI_Id,
                    "INVMUOM_Id": $scope.invmuom_id,
                    "INVOB_BatchNo": $scope.invoB_BatchNo,
                    "INVOB_PurchaseDate": $scope.invoB_PurchaseDate,
                    "INVOB_PurchaseRate": $scope.invoB_PurchaseRate,
                    "INVOB_SaleRate": $scope.invoB_SaleRate,
                    "INVOB_DiscountAmt": $scope.invoB_DiscountAmt,
                    "INVOB_TaxAmt": $scope.invoB_TaxAmt,
                    "INVOB_Amount": $scope.invoB_Amount,
                    "INVOB_Qty": $scope.invoB_Qty,
                    "INVOB_Naration": $scope.invoB_Naration,
                    "INVOB_MfgDate": $scope.invoB_MfgDate,
                    "INVOB_ExpDate": $scope.invoB_ExpDate,
                    "INVOB_Id": $scope.invoB_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_OpeningBalance/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invoB_Id == 0 || promise.invoB_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invoB_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invoB_Id == 0 || promise.invoB_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invoB_Id > 0) {
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
            $scope.INVOB_Id = item.invoB_Id;
            var dystring = "";
            if (item.invoB_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invoB_ActiveFlg == false) {
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
                        apiService.create("INV_OpeningBalance/deactive", item).
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
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (item) {
            $scope.invmsT_Id = item.invmsT_Id;
            $scope.invmI_Id = item.invmI_Id;
            $scope.invmuom_id = item.invmuoM_Id;
            $scope.invoB_BatchNo = item.invoB_BatchNo;
            $scope.invoB_PurchaseDate = new Date(item.invoB_PurchaseDate);
            $scope.invoB_PurchaseRate = item.invoB_PurchaseRate;
            $scope.invoB_SaleRate = item.invoB_SaleRate;
            $scope.invoB_DiscountAmt = item.invoB_DiscountAmt;
            $scope.invoB_TaxAmt = item.invoB_TaxAmt;
            $scope.invoB_Amount = item.invoB_Amount;
            $scope.invoB_Qty = item.invoB_Qty;
            $scope.invoB_Naration = item.invoB_Naration;
            $scope.invoB_MfgDate = new Date(item.invoB_MfgDate);
            $scope.invoB_ExpDate = new Date(item.invoB_ExpDate);
            $scope.invoB_Id = item.invoB_Id;
            $scope.onitemchange(item.invmI_Id);
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();