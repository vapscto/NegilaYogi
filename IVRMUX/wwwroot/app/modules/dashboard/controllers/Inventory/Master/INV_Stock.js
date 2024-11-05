
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_StockController', INV_StockController);
    INV_StockController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_StockController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length == 1) {
            $scope.cnt = 1;
        }
        $scope.addstockrows = function () {
            if ($scope.transrows.length > 1) {
                $scope.cnt = $scope.transrows.length;
            }
            $scope.cnt = $scope.cnt + 1;
            var newItemNo = $scope.cnt;
            $scope.transrows.push({ 'objg': + newItemNo });
        };

        $scope.removestockrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletestockrows(data);
            }
            if ($scope.transrows.length == 0) {
            }
        };
        //===============================Load Page

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_Stock/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
                    $scope.get_item = promise.get_item;
                    $scope.get_stock = promise.get_stock;
                    $scope.presentCountgrid = $scope.get_stock.length;
                })
        };
        //================================Save-
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.arrayStock = [];
                angular.forEach($scope.transrows, function (stok) {
                    $scope.arrayStock.push({ invmsT_Id: stok.invmsT_Id, invmI_Id: stok.invmI_Id.invmI_Id, invstO_BatchNo: stok.invstO_BatchNo, invstO_PurchaseDate: stok.invstO_PurchaseDate, invstO_PurchaseRate: stok.invstO_PurchaseRate, invstO_ItemConQty: stok.invstO_ItemConQty, invstO_PurOBQty: stok.invstO_PurOBQty, invstO_PurRetQty: stok.invstO_PurRetQty, invstO_SalesRate: stok.invstO_SalesRate, invstO_SalesQty: stok.invstO_SalesQty, invstO_SalesRetQty: stok.invstO_SalesRetQty, invstO_MatIssPlusQty: stok.invstO_MatIssPlusQty, invstO_MatIssMinusQty: stok.invstO_MatIssMinusQty, invstO_PhyPlusQty: stok.invstO_PhyPlusQty, invstO_PhyMinQty: stok.invstO_PhyMinQty, invstO_AvaiableStock: stok.invstO_AvaiableStock, invstO_Id: stok.invstO_Id });
                })
                var data = {
                    "stocklist": $scope.arrayStock,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_Stock/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invstO_Id == 0 || promise.invstO_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invstO_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invstO_Id == 0 || promise.invstO_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invstO_Id > 0) {
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

        $scope.edit = function (id) {
            var invstO_Id = id;
            var data = {
                "INVSTO_Id": invstO_Id
            }
            apiService.create("INV_Stock/editStock", data).
                then(function (promise) {
                    $scope.get_editstock = promise.get_editstock;
                    $scope.transrows = $scope.get_editstock;
                    angular.forEach($scope.transrows, function (obj) {
                        obj.invmsT_Id = $scope.get_editstock[0].invmsT_Id;
                        obj.invmI_Id = $scope.get_editstock[0].invmI_Id;
                        obj.invstO_Id = $scope.get_editstock[0].invstO_Id;
                        obj.invstO_BatchNo = $scope.get_editstock[0].invstO_BatchNo;
                        obj.invstO_PurchaseDate = new date($scope.get_editstock[0].invstO_PurchaseDate);
                        obj.invstO_PurchaseRate = $scope.get_editstock[0].invstO_PurchaseRate;
                        obj.invstO_SalesRate = $scope.get_editstock[0].invstO_SalesRate;
                        obj.invstO_ItemConQty = $scope.get_editstock[0].invstO_ItemConQty;
                        obj.invstO_PurOBQty = $scope.get_editstock[0].invstO_PurOBQty;
                        obj.invstO_PurRetQty = $scope.get_editstock[0].invstO_PurRetQty;
                        obj.invstO_SalesQty = $scope.get_editstock[0].invstO_SalesQty;
                        obj.invstO_SalesRetQty = $scope.get_editstock[0].invstO_SalesRetQty;
                        obj.invstO_MatIssPlusQty = $scope.get_editstock[0].invstO_MatIssPlusQty;
                        obj.invstO_MatIssMinusQty = $scope.get_editstock[0].invstO_MatIssMinusQty;
                        obj.invstO_PhyPlusQty = $scope.get_editstock[0].invstO_PhyPlusQty;
                        obj.invstO_PhyMinQty = $scope.get_editstock[0].invstO_PhyMinQty;
                        obj.invstO_AvaiableStock = $scope.get_editstock[0].invstO_AvaiableStock;
                    })
                })
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();