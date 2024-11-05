
(function () {
    'use strict';
    angular
        .module('app')
        .controller('DisposeAssetsController', DisposeAssetsController);
    DisposeAssetsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function DisposeAssetsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
        //========================================== PAGE LOAD
        $scope.obj = {};
        $scope.invadI_DisposedDate = new Date();
        $scope.plMaxdate = new Date();

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            $scope.get_locations = "";
            $scope.get_items = "";
            $scope.get_details = "";
            apiService.getURI("DisposeAssets/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
                    $scope.get_dispose = promise.get_dispose;
                    $scope.presentCountgrid = $scope.get_dispose.length;
                });
        };
        //============================================ 
        $scope.onstorechange = function () {
            $scope.get_locations = "";
            $scope.get_items = "";
            $scope.get_details = "";
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            var data = {
                "INVMST_Id": $scope.storeid
            };
            apiService.create("DisposeAssets/getlocations", data).
                then(function (promise) {
                    $scope.get_locations = promise.get_locations;
                });
        };


        $scope.onlocationchange = function () {
            $scope.get_details = "";
            $scope.id = $scope.obj.invmlO_Id.invmlO_Id;
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            var data = {
                "INVMLO_Id": $scope.id,
                "INVMST_Id": $scope.storeid
            };
            apiService.create("DisposeAssets/getitems", data).
                then(function (promise) {
                    $scope.get_items = promise.get_items;
                    $scope.salerate = $scope.get_items[0].invstO_SalesRate;
                });
        };
        $scope.onitemchange = function () {
            var avqty = 0;
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.locationid = $scope.obj.invmlO_Id.invmlO_Id;
            $scope.itemid = $scope.obj.invmI_Id.invmI_Id;
            $scope.salerate = $scope.obj.invmI_Id.invstO_SalesRate;
            var data = {
                "INVMST_Id": $scope.storeid,
                "INVMLO_Id": $scope.locationid,
                "INVMI_Id": $scope.itemid,
                "INVSTO_SalesRate": $scope.obj.invmI_Id.invstO_SalesRate
            };
            apiService.create("CheckInAssets/getdetails", data).
                then(function (promise) {
                    $scope.get_details = promise.get_details;
                    angular.forEach($scope.get_details, function (ast) {
                        avqty += ast.invstO_AvaiableStock;
                    });
                    $scope.checkoutqty = avqty;
                });
        };

        //$scope.onitemchange = function () {
        //    $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
        //    $scope.locationid = $scope.obj.invmlO_Id.invmlO_Id;
        //    $scope.itemid = $scope.obj.invmI_Id.invmI_Id;
        //    $scope.salerate = $scope.obj.invmI_Id.invstO_SalesRate;
        //    var data = {
        //        "INVMLO_Id": $scope.locationid,
        //        "INVMST_Id": $scope.storeid,
        //        "INVMI_Id": $scope.itemid,
        //        "INVSTO_SalesRate": $scope.salerate
        //    };
        //    apiService.create("DisposeAssets/getdetails", data).
        //        then(function (promise) {
        //            $scope.get_details = promise.get_details;
        //            $scope.checkoutqty = $scope.get_details[0].invacO_CheckOutQty;
        //        });
        //};

        $scope.checkStock = function () {
            var totlqty = 0;
            var checkoutqty = 0;
            totlqty = parseInt($scope.invadI_DisposedQty);
            checkoutqty = parseInt($scope.checkoutqty);
            if (totlqty > checkoutqty) {
                swal("Please Check Check-out Quantity...!!");
                $scope.invadI_DisposedQty = "";
            }
        };

        $scope.dispose = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMST_Id": $scope.obj.invmsT_Id.invmsT_Id,
                    "INVMI_Id": $scope.obj.invmI_Id.invmI_Id,
                    "INVSTO_SalesRate": $scope.obj.invmI_Id.invstO_SalesRate,
                    "INVMLO_Id": $scope.obj.invmlO_Id.invmlO_Id,
                    "INVADI_DisposedDate": $scope.invadI_DisposedDate,
                    "INVADI_DisposedQty": $scope.invadI_DisposedQty,
                    "INVADI_DisposedRemarks": $scope.invadI_DisposedRemarks,
                    "INVADI_Id": $scope.invadI_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("DisposeAssets/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invadI_Id === 0 || promise.invadI_Id < 0) {
                            swal('Item Dispose successfully');
                        }
                        else if (promise.invadI_Id > 0) {
                            swal('Updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invadI_Id === 0 || promise.invadI_Id < 0) {
                            swal('Failed to Dispose, please contact administrator');
                        }
                        else if (promise.invadI_Id > 0) {
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

        $scope.edit = function (item) {
            $scope.editS = true;
            $scope.obj.invmsT_Id = item;
            $scope.obj.invmI_Id = item;
            $scope.obj.invmlO_Id = item;

            $scope.invadI_DisposedDate = new Date(item.invadI_DisposedDate);
            $scope.invadI_DisposedQty = item.invadI_DisposedQty;
            $scope.invadI_DisposedRemarks = item.invadI_DisposedRemarks;
            $scope.invadI_Id = item.invadI_Id;

            $scope.onitemchange();
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVADI_Id = item.invadI_Id;
            var dystring = "";
            if (item.invadI_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invadI_ActiveFlg === false) {
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
                        apiService.create("DisposeAssets/deactive", item).
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
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();