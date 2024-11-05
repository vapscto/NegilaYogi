//import { elementByClass } from "../../../../../../lib/sweetalert2/src/utils/dom";

(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_MasterProductController', INV_MasterProductController);
    INV_MasterProductController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_MasterProductController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
            apiService.getURI("INV_MasterProduct/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_tax = promise.get_tax;
                    $scope.get_product = promise.get_product;
                    $scope.get_item = promise.get_item;
                    $scope.get_productlist = promise.get_productlist;
                    $scope.presentCountgrid = $scope.get_productlist.length;

                    $scope.get_productItemlist = promise.get_productItemlist;
                    $scope.presentCountgrid1 = $scope.get_productItemlist.length;
                });
        };

        //-----------Tax Grid Select All
        $scope.toggleAll = function () {
            angular.forEach($scope.get_tax, function (subj) {
                subj.xyz = $scope.all;
            });
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.get_tax.every(function (itm) { return itm.xyz; });
        };

        //---------------------------------Save--------------------------------------------
        //Tax
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {};
                if ($scope.invmP_TaxApplFlg === '1') {
                    $scope.taxchk = [];
                    angular.forEach($scope.get_tax, function (tax) {
                        if (tax.xyz) {
                            $scope.taxchk.push({ invmpT_Id: tax.invmpT_Id, invmT_Id: tax.invmT_Id, invmpT_TaxValue: tax.invmpT_TaxValue });
                        }
                    });
                     data = {
                        "INVMP_ProductName": $scope.invmP_ProductName,
                        "INVMP_ProductCode": $scope.invmP_ProductCode,
                        "INVMP_ProductPrice": $scope.invmP_ProductPrice,
                        "INVMP_TaxApplFlg": $scope.invmP_TaxApplFlg,
                        "INVMP_Id": $scope.invmP_Id,
                        "product_tax": $scope.taxchk
                    };
                }
                else {
                     data = {
                        "INVMP_ProductName": $scope.invmP_ProductName,
                        "INVMP_ProductCode": $scope.invmP_ProductCode,
                        "INVMP_ProductPrice": $scope.invmP_ProductPrice,
                        "INVMP_TaxApplFlg": $scope.invmP_TaxApplFlg,
                        "INVMP_Id": $scope.invmP_Id
                    };
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_MasterProduct/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invmP_Id === 0 || promise.invmP_Id < 0) {
                            swal('Record saved successfully');
                            $scope.loaddata();
                        }
                        else if (promise.invmP_Id > 0) {
                            swal('Record updated successfully');
                            $scope.loaddata();
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmP_Id === 0 || promise.invmP_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmP_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.clear();
                    $scope.loaddata();
                    //  $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.savedata1 = function () {
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {

                var data = {
                    "INVMP_Id": $scope.invmP_Id,
                    "INVMI_Id": $scope.invmI_Id,
                    "INVMPI_ItemQty": $scope.invmpI_ItemQty,
                    "INVMPI_Id": $scope.invmpI_Id
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_MasterProduct/savedetailQty", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invmpI_Id === 0 || promise.invmpI_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmpI_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmpI_Id === 0 || promise.invmpI_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmpI_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    // $state.reload();
                    $scope.clear();
                    $scope.loaddata();
                });           
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.cancel = function () {
            $state.reload();
        };
        $scope.clear = function () {
            $scope.invmP_Id = "";
            $scope.invmI_Id = "";
            $scope.invmpI_ItemQty = "";
            $scope.submitted1 = false;
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVMP_Id = item.invmP_Id;
            var dystring = "";
            if (item.invmP_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invmP_ActiveFlg === false) {
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
                        apiService.create("INV_MasterProduct/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }

                                // $state.reload();
                                $scope.clear();
                                $scope.loaddata();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.deactiveptax = function (item, SweetAlert) {
            $scope.INVMPT_Id = item.invmpT_Id;
            var dystring = "";
            if (item.invmpT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invmpT_ActiveFlg === false) {
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
                        apiService.create("INV_MasterProduct/deactiveptax", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                // $state.reload();
                                angular.element('#myModal').modal('hide');
                                $scope.clear();
                                $scope.loaddata();

                                //  $state.reload();

                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.edit = function (item) {
            $scope.get_tax = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.invmP_ProductName = item.invmP_ProductName;
            $scope.invmP_ProductCode = item.invmP_ProductCode;
            $scope.invmP_ProductPrice = item.invmP_ProductPrice;
            $scope.invmP_TaxApplFlg = item.invmP_TaxApplFlg;
            $scope.invmP_Id = item.invmP_Id;
            $scope.invmpT = item.invmpT;
            var data = {
                "INVMP_Id": item.invmP_Id
            };
            apiService.create("INV_MasterProduct/productTax", data).
                then(function (promise) {

                    $scope.get_tax = promise.get_tax;

                    $scope.get_productTax = promise.get_productTax;
                    angular.forEach($scope.get_tax, function (atax) {
                        angular.forEach($scope.get_productTax, function (itax) {
                            if (atax.invmT_Id === itax.invmT_Id) {
                                atax.invmpT_TaxValue = itax.invmpT_TaxValue;
                                atax.invmpT_Id = itax.invmpT_Id;
                                atax.xyz = true;
                            }

                        });

                    });

                    //  $scope.get_tax = $scope.get_productTax;
                });

        };

        $scope.deactiveQty = function (item, SweetAlert) {
            $scope.INVMPI_Id = item.invmpI_Id;
            var dystring = "";
            if (item.invmpI_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invmpI_ActiveFlg === false) {
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
                        apiService.create("INV_MasterProduct/deactiveQty", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                //   $state.reload();
                                $scope.clear();
                                $scope.loaddata();
                                //$state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.editQty = function (item) {
            $scope.invmpI_Id = item.invmpI_Id;
            $scope.invmP_Id = item.invmP_Id;
            $scope.invmI_Id = item.invmI_Id;
            $scope.invmpI_ItemQty = item.invmpI_ItemQty;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };


        $scope.onformclick = function (id) {
            var data = {
                "INVMP_Id": id
            };
            apiService.create("INV_MasterProduct/productTax", data).
                then(function (promise) {
                    $scope.gridproductTax = promise.gridproductTax;
                    //$scope.get_productTax = promise.get_productTax;
                    $scope.product = $scope.gridproductTax[0].invmP_ProductName;
                });
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';

        //$scope.filterValue = function (obj) {
        //    return (angular.lowercase(obj.invmP_ProductName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        //}
        //$scope.filterValue1 = function (obj) {
        //    return (angular.lowercase(obj.invmP_ProductName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.invmI_ItemName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        //}

    }
})();