
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_MasterItemController', INV_MasterItemController);
    INV_MasterItemController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_MasterItemController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_MasterItem/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_tax = promise.get_tax;
                    $scope.get_itemgroup = promise.get_itemgroup;
                    $scope.get_UOM = promise.get_UOM;

                    $scope.get_item = promise.get_item;
                    $scope.presentCountgrid = $scope.get_item.length;
                })
        };

        //-----------Tax Grid Select All
        $scope.toggleAll = function () {
            angular.forEach($scope.get_tax, function (subj) {
                subj.xyz = $scope.all;
            })
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.get_tax.every(function (itm) { return itm.xyz; });
        };


        //---------------------------------Save--------------------------------------------
        //Tax
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.invmI_TaxAplFlg == '1') {
                    $scope.taxchk = [];
                    angular.forEach($scope.get_tax, function (tax) {
                        if (tax.xyz) {
                            $scope.taxchk.push({ invmiT_Id: tax.invmiT_Id, invmT_Id: tax.invmT_Id, invmiT_TaxValue: tax.invmiT_TaxValue });
                        }
                    })
                    var data = {
                        "INVMG_Id": $scope.invmG_Id,
                        "INVMI_ItemName": $scope.invmI_ItemName,
                        "INVMI_MaxStock": $scope.invmI_MaxStock,
                        "INVMI_TaxAplFlg": $scope.invmI_TaxAplFlg,
                        "INVMUOM_Id": $scope.invmuoM_Id,
                        "INVMI_ItemCode": $scope.invmI_ItemCode,
                        "INVMI_ReorderStock": $scope.invmI_ReorderStock,
                        "INVMI_HSNCode": $scope.invmI_HSNCode,
                        "INVMI_RawMatFlg": $scope.invmI_RawMatFlg,
                        "INVMI_ForSaleFlg": $scope.invmI_ForSaleFlg,
                        "INVMI_MaintenanceAplFlg": $scope.invmI_MaintenanceAplFlg,
                        "INVMI_Id": $scope.invmI_Id,
                        "tax_Applicable": $scope.taxchk,
                    }
                }
                else {
                    var data = {
                        "INVMG_Id": $scope.invmG_Id,
                        "INVMI_ItemName": $scope.invmI_ItemName,
                        "INVMI_MaxStock": $scope.invmI_MaxStock,
                        "INVMI_TaxAplFlg": $scope.invmI_TaxAplFlg,
                        "INVMUOM_Id": $scope.invmuoM_Id,
                        "INVMI_ItemCode": $scope.invmI_ItemCode,
                        "INVMI_ReorderStock": $scope.invmI_ReorderStock,
                        "INVMI_HSNCode": $scope.invmI_HSNCode,
                        "INVMI_RawMatFlg": $scope.invmI_RawMatFlg,
                        "INVMI_ForSaleFlg": $scope.invmI_ForSaleFlg,
                        "INVMI_MaintenanceAplFlg": $scope.invmI_MaintenanceAplFlg,
                        "INVMI_Id": $scope.invmI_Id,
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_MasterItem/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmI_Id == 0 || promise.invmI_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmI_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmI_Id == 0 || promise.invmI_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmI_Id > 0) {
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
            $scope.INVMI_Id = item.invmI_Id;
            var dystring = "";
            if (item.invmI_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmI_ActiveFlg == false) {
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
                        apiService.create("INV_MasterItem/deactive", item).
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

        $scope.deactiveitax = function (item, SweetAlert) {
            $scope.INVMIT_Id = item.invmiT_Id;
            var dystring = "";
            if (item.invmiT_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmiT_ActiveFlg == false) {
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
                        apiService.create("INV_MasterItem/deactiveitax", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                angular.element('#myModal').modal('hide');
                                //$state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (item) {
            $scope.get_tax = "";
            $scope.get_itemTax = "";
            $scope.invmG_Id = item.invmG_Id;
            $scope.invmG_GroupName = item.invmG_GroupName;
            $scope.invmI_ItemName = item.invmI_ItemName;
            $scope.invmI_MaxStock = item.invmI_MaxStock;
            $scope.invmI_TaxAplFlg = item.invmI_TaxAplFlg;
            $scope.invmuoM_Id = item.invmuoM_Id;
            $scope.invmI_ItemCode = item.invmI_ItemCode;
            $scope.invmI_ReorderStock = item.invmI_ReorderStock;
            $scope.invmI_HSNCode = item.invmI_HSNCode;
            $scope.invmI_RawMatFlg = item.invmI_RawMatFlg;
            $scope.invmI_ForSaleFlg = item.invmI_ForSaleFlg;
            $scope.invmI_MaintenanceAplFlg = item.invmI_MaintenanceAplFlg;
            $scope.invmI_Id = item.invmI_Id;
            $scope.invmiT_Id = item.invmiT_Id;
            $scope.invmiT_TaxValue = item.invmiT_TaxValue;
            var data = {
                "INVMI_Id": item.invmI_Id
            }
            apiService.create("INV_MasterItem/itemTax", data).
                then(function (promise) {
                    $scope.get_tax = promise.get_tax;
                    $scope.get_itemTax = promise.get_itemTax;
                    angular.forEach($scope.get_tax, function (atax) {
                        angular.forEach($scope.get_itemTax, function (itax) {
                            if (atax.invmT_Id == itax.invmT_Id) {
                                atax.invmiT_TaxValue = itax.invmiT_TaxValue;
                                atax.invmiT_Id = itax.invmiT_Id;
                                atax.xyz = true;
                                //   $scope.get_tax.push({ invmiT_Id: atax.invmiT_Id, invmT_Id: atax.invmT_Id, invmiT_TaxValue: itax.invmiT_TaxValue });
                            }

                        })

                    })
                    console.log($scope.get_tax);
                    console.log($scope.get_itemTax);

                    //  $scope.get_tax = $scope.get_itemTax;
                })


        }

        $scope.onformclick = function (id) {
            var data = {
                "INVMI_Id": id
            }
            apiService.create("INV_MasterItem/itemTax", data).
                then(function (promise) {
                    $scope.griditemTax = promise.griditemTax;
                    $scope.item = $scope.griditemTax[0].invmI_ItemName;
                })
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

        //$scope.filterValue = function (obj) {
        //    return (angular.lowercase(obj.invmG_GroupName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.invmI_ItemName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.invmuoM_UOMName)).indexOf(angular.lowercase($scope.searchValue)) >= 0

        //}

    }
})();