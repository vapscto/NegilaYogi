
(function () {
    'use strict';
    angular
        .module('app')
        .controller('PhyStock_UpdationController', PhyStock_UpdationController);
    PhyStock_UpdationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function PhyStock_UpdationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.editS = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addPSUrows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i === $scope.transrows.length; i++) {
                        var id = $scope.transrows[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.removePSUrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.DeletePSUrows(data);
            }
        };
        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("PhyStock_Updation/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_Store = promise.get_Store;
                    $scope.UOM = promise.uom;
                    $scope.get_phyStockdata = promise.get_phyStockdata;
                    $scope.presentCountgrid = $scope.get_phyStockdata.length;
                });
        };

        //===================== Get Item on store Change
        $scope.storeChange = function () {
            $scope.transgrid = true;
            var data = {
                "INVMST_Id": $scope.invmsT_Id
            };
            apiService.create("PhyStock_Updation/getitem", data).
                then(function (promise) {
                    
                        $scope.get_item = promise.get_item;
              
                   
                });
        };
        //===========Get Item deatils On item Change
        $scope.onitemchange = function (itemid, objid) {
            var data = {};
            var avstock = 0;
            data = {
                "INVMP_Id": itemid.invmP_Id,
                "INVMP_ProductPrice": itemid.invmP_ProductPrice,
                "INVMST_Id": $scope.invmsT_Id
            };
            apiService.create("PhyStock_Updation/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    $scope.availablestock = promise.availablestock;

                    angular.forEach($scope.transrows, function (obj) {
                        if (obj.itrS_Id === objid.itrS_Id) {
                            angular.forEach($scope.availablestock, function (ast) {
                                avstock += ast.AvaiableStock;
                            });
                            obj.availableitems = avstock;
                            obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;
                            obj.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_Id;


                        }
                    });
                });
        };
        //===========Radio Change
        $scope.radiochange = function (objps) {
            objps.invpsU_StockPlus = 0;
            objps.invpsU_StockMinus = 0;
        };

        //===================================== SAVE DATA
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.DCSphyStock = [];
                angular.forEach($scope.transrows, function (psu) {
                    if (psu.typeflag === 'Plus') {
                        psu.invpsU_StockPlus = psu.stockQuantity;
                    }
                    else if (psu.typeflag === 'Minus') {
                        psu.invpsU_StockMinus = psu.stockQuantity;
                    }
                    if ($scope.editS === true) {
                        $scope.DCSphyStock.push({
                            invmP_Id: psu.invmP_Id, invmP_ProductPrice: psu.invmP_ProductPrice, invpsU_StockPlus: psu.invpsU_StockPlus, invpsU_StockMinus: psu.invpsU_StockMinus, invpsU_Remarks: psu.invpsU_Remarks
                        });
                    }
                    else {
                        $scope.DCSphyStock.push({
                            invmP_Id: psu.invmP_Id.invmP_Id, invmP_ProductPrice: psu.invmP_Id.invmP_ProductPrice, invpsU_StockPlus: psu.invpsU_StockPlus, invpsU_StockMinus: psu.invpsU_StockMinus, invpsU_Remarks: psu.invpsU_Remarks
                        });
                    }
                });
                var data = {
                    "INVMST_Id": $scope.invmsT_Id,
                    "DCSPSU_Id": $scope.dcspsU_Id, 
                    "DCSphyStock": $scope.DCSphyStock,
                    "INVMUOM_Id": $scope.invmuoM_Id,
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("PhyStock_Updation/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.dcspsU_Id === 0 || promise.dcspsU_Id < 0) {
                            if (promise.returnduplicatestatus === 'Updated') {
                                swal('Record saved successfully / Stock Updated');
                            }
                            if (promise.returnduplicatestatus === 'notUpdated') {
                                swal('Record saved successfully / Failed to Update Stock');
                            }
                        }
                        else if (promise.dcspsU_Id > 0) {
                            if (promise.returnduplicatestatus === 'Updated') {
                                swal('Record updated successfully / Stock Updated');
                            }
                            if (promise.returnduplicatestatus === 'notUpdated') {
                                swal('Record updated successfully / Failed to Update Stock');
                            }
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.dcspsU_Id === 0 || promise.dcspsU_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.dcspsU_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
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
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.DCSPSU_Id = item.dcspsU_Id;
            var dystring = "";
            if (item.invpsU_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invpsU_ActiveFlg === false) {
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
                        apiService.create("PhyStock_Updation/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.edit = function (item) {
            $scope.transrows = [];
            var typflg = "";
            var stockQty = 0;
            $scope.invmsT_Id = item.invmsT_Id;
            $scope.dcspsU_Id = item.dcspsU_Id;
            $scope.invmuoM_Id = item.invmuoM_Id;
            $scope.editS = true;
            if (item.invpsU_StockPlus > 0) {
                stockQty: item.invpsU_StockPlus;
                item.typflg = "Plus";

            }
            else if (item.invpsU_StockMinus > 0) {
                stockQty: item.invpsU_StockMinus;
                item.typflg = "Minus";

            }
            $scope.transrows.push({ invmP_Id: item.invmP_Id, invmP_ProductName: item.invmP_ProductName, invmP_ProductPrice: item.invmP_ProductPrice, invmuoM_Id: item.invmuoM_Id, invmuoM_UOMName: item.invmuoM_UOMName, availableitems: item.invstO_AvaiableStock, invpsU_Remarks: item.invpsU_Remarks, stockQuantity: stockQty, typeflag: item.typflg });
            console.log($scope.transrows);
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();