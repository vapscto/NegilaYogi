
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_OpeningBalanceController', INV_OpeningBalanceController);
    INV_OpeningBalanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_OpeningBalanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var date = new Date();
        // $scope.invoB_PurchaseDate = date;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null &&  admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addOBrows = function () {
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

        $scope.removeOBrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletegrnrows(data);
            }
        };
        //====================================== Page Load
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
                    $scope.year_id = promise.imfY_Id;
                    if ($scope.get_openingbalance != null && $scope.get_openingbalance.length > 0) {
                        for (var i = 0; i < $scope.get_openingbalance.length; i++) {
                           
                            $scope.get_openingbalance[i].invoB_TAmount = (Number($scope.get_openingbalance[i].invoB_Qty) * ($scope.get_openingbalance[i].invoB_PurchaseRate));
                        }
                    }
                });
        };

        //===========Get Item deatils On item Change
        $scope.onitemchange = function (itemid, objid) {
            $scope.get_itemTax1 = [];
            var data = {
                "INVMI_Id": itemid.invmI_Id
            };
            apiService.create("INV_T_GRN/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    angular.forEach($scope.transrows, function (obj) {
                        if (obj.itrS_Id === objid.itrS_Id) {
                            obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;
                            obj.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_Id;
                            var date = new Date();
                            obj.invoB_PurchaseDate = date;
                            obj.invoB_BatchNo = "0";
                            obj.invoB_Qty = 0.00;
                            obj.invstO_PurchaseRate = 0.00;
                            obj.invstO_SalesRate = 0.00;
                        }
                    });

                });
        };

        //===================================== SAVE DATA

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.arrayOB = [];
                if ($scope.editS === true) {
                    angular.forEach($scope.transrows, function (ob) {
                        $scope.arrayOB.push({
                            INVMI_Id: ob.invmI_Id, INVOB_PurchaseDate: new Date(ob.invoB_PurchaseDate).toDateString(), INVMUOM_Id: ob.invmuoM_Id, INVOB_BatchNo: ob.invoB_BatchNo,
                            INVOB_PurchaseRate: ob.invstO_PurchaseRate, INVOB_SaleRate: ob.invstO_SalesRate, INVOB_Qty: ob.invoB_Qty,
                            INVOB_Naration: ob.invoB_Naration
                        });
                    });
                }
                else {
                    angular.forEach($scope.transrows, function (ob) {
                        $scope.arrayOB.push({
                            INVMI_Id: ob.invmI_Id.invmI_Id, INVOB_PurchaseDate: new Date(ob.invoB_PurchaseDate), INVMUOM_Id: ob.invmuoM_Id, INVOB_BatchNo: ob.invoB_BatchNo, INVOB_MfgDate: ob.invoB_MfgDate,
                            INVOB_ExpDate: ob.invoB_ExpDate, INVOB_PurchaseRate: ob.invstO_PurchaseRate, INVOB_SaleRate: ob.invstO_SalesRate, INVOB_Qty: ob.invoB_Qty,
                            INVOB_Naration: ob.invoB_Naration
                        });
                    });
                }
                var data = {
                    "INVMST_Id": $scope.invmsT_Id,
                    "INVOB_Id": $scope.invoB_Id,
                    "OBItem": $scope.arrayOB,
                    "IMFY_Id": $scope.year_id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_OpeningBalance/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        swal("Record Updated Successfully.")
                    }
                    else {
                        swal("Record Not Updated Successfully !!!.")
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
            $scope.INVOB_Id = item.invoB_Id;
            var dystring = "";
            if (item.invoB_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invoB_ActiveFlg === false) {
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
            $scope.get_obdetails = [];
            $scope.transrows = [];

            $scope.invmsT_Id = item.invmsT_Id;
            $scope.invoB_Id = item.invoB_Id;
            $scope.editS = true;
            var invoB_Id = item.invoB_Id;
            var data = {
                "INVOB_Id": invoB_Id
            };
            apiService.create("INV_OpeningBalance/getobdetails", data).
                then(function (promise) {

                    $scope.get_obdetails = promise.get_obdetails;
                    angular.forEach($scope.get_obdetails, function (objr) {
                        $scope.transrows.push({
                            invmI_Id: objr.invmI_Id,
                            invmI_ItemName: objr.invmI_ItemName,
                            invmuoM_Id: objr.invmuoM_Id,
                            invmuoM_UOMName: objr.invmuoM_UOMName,
                            invoB_BatchNo: objr.invoB_BatchNo,
                            invoB_PurchaseDate: new Date(objr.invoB_PurchaseDate),
                            invoB_Qty: objr.invoB_Qty,
                            invoB_MfgDate: objr.invoB_MfgDate,
                            invstO_PurchaseRate: objr.invoB_PurchaseRate,
                            invoB_ExpDate: objr.invoB_ExpDate,
                            invstO_SalesRate: objr.invoB_SaleRate,
                            invoB_Naration: objr.invoB_Naration,
                            invoB_TotalAmount: objr.invoB_PurchaseRate * objr.qty
                           // invoB_TotalAmount: objr.invoB_TotalAmount
                        });
                    });

                });
        };

        //================== Count Amount
        $scope.countitemAmt = function (probj, items) {
            var a = 0;
            $scope.purrate = parseFloat(probj.invstO_PurchaseRate);
            $scope.qty = parseFloat(probj.invoB_Qty);
            probj.invoB_TotalAmount = $scope.purrate * $scope.qty;
            angular.forEach($scope.transrows, function (obj) {
                a += parseFloat(obj.invoB_TotalAmount);
            })
            var totamt = a;
            $scope.invoB_TotalAmount = totamt;
            $scope.invoB_TotalAmount = parseFloat($scope.invoB_TotalAmount);
            $scope.invoB_TotalAmount = $scope.invoB_TotalAmount.toFixed(2);
        }



        //$scope.move_to_stock = function (item) {
        //    $scope.get_obdetails = [];
        //    $scope.transrows = [];

        //    $scope.invmsT_Id = item.invmsT_Id;
        //    $scope.invoB_Id = item.invoB_Id;
        //    $scope.editS = true;
        //    var invoB_Id = item.invoB_Id;
        //    var data = {
        //        "INVOB_Id": invoB_Id
        //    };
        //    apiService.create("INV_OpeningBalance/move_to_stock", data).
        //        then(function (promise) {
        //            if (returnduplicatestatus === "no") {
        //                swal("No Record Found")
        //            }
        //            else {
        //                if (promise.returnval === true) {
        //                    swal("OB moved to stock Successfully.");
        //                }
        //                else {
        //                    swal("OB not moved to stock Successfully!!!");
        //                }
        //                $state.reload();
        //            }
                    
        //        });
        //};


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();