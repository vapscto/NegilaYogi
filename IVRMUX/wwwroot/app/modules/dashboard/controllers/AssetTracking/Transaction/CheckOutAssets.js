
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckOutAssetsController', CheckOutAssetsController);
    CheckOutAssetsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function CheckOutAssetsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        $scope.editS = false;
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
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.obj = {};
        $scope.invacO_CheckoutDate = new Date();

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("CheckOutAssets/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
                    console.log($scope.get_store);
                    $scope.get_locations = promise.get_locations;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_checkout = promise.get_checkout;
                    $scope.presentCountgrid = $scope.get_checkout.length;
                });
        };
        //============================================ 

        //===================== Get Item on store Change
        $scope.onstorechange = function () {
            $scope.id = $scope.obj.invmsT_Id.invmsT_Id;
            var data = {
                "INVMST_Id": $scope.id
            };
            apiService.create("INV_T_Sales/getitem", data).
                then(function (promise) {

                    if (promise.get_item.length > 0) {
                        $scope.get_items = promise.get_item;
                    }
                    else {
                        swal('For Selected Store, No Item found..!!');
                        $scope.get_items.length = 0;
                    }
                });
        };
        //$scope.onstorechange = function () {
        //    $scope.id = $scope.obj.invmsT_Id.invmsT_Id;
        //    var data = {
        //        "INVMST_Id": $scope.id
        //    };
        //    apiService.create("CheckOutAssets/getitems", data).
        //        then(function (promise) {
        //            $scope.get_items = promise.get_items;
        //        });
        //};
        $scope.onlocationchange = function () {
            $scope.id = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMLO_Id": $scope.id
            };
            apiService.create("CheckOutAssets/getcontactperson", data).
                then(function (promise) {
                    $scope.get_contactperson = promise.get_contactperson;
                    $scope.contactflag = promise.contactflag;
                    if ($scope.contactflag === "E") {
                        $scope.hrmE_Id = $scope.get_contactperson[0].hrmE_Id;
                        $scope.employeename = $scope.get_contactperson[0].employeename;
                    }
                    else {
                        $scope.employeename = $scope.get_contactperson[0].invmlO_InchargeName;
                    }
                });
        };
        $scope.onitemchange = function () {
            var data = {};
            var avstock = 0;
            if ($scope.editS != true) {
                $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            }
            else {
                $scope.storeid = $scope.invmsT_Id
            }
          
            $scope.itemid = $scope.obj.INVMI_Id.INVMI_Id;
            $scope.salerate = $scope.obj.INVMI_Id.INVSTO_SalesRate;
            data = {
                "INVMI_Id": $scope.itemid,
                "INVSTO_SalesRate": $scope.salerate,
                "INVMST_Id": $scope.storeid
            };
            apiService.create("INV_T_Sales/getitemDetail", data).
                then(function (promise) {
                    $scope.availablestock = promise.availablestock;
                    angular.forEach($scope.availablestock, function (ast) {
                        avstock += ast.AvaiableStock;
                    });
                    $scope.avstock = avstock;
                });
        };
        //$scope.onitemchange = function () {
        //    $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
        //    $scope.itemid = $scope.obj.INVMI_Id.INVMI_Id;
        //    $scope.salerate = $scope.obj.INVMI_Id.INVSTO_SalesRate;
        //    var data = {
        //        "INVMST_Id": $scope.storeid,
        //        "INVMI_Id": $scope.itemid,
        //        "INVSTO_SalesRate": $scope.salerate
        //    };
        //    apiService.create("CheckOutAssets/get_avaiablestock", data).
        //        then(function (promise) {
        //            $scope.availablestock = promise.availablestock;
        //            $scope.avstock = $scope.availablestock[0].invstO_AvaiableStock;
        //        });
        //};

        $scope.checkStock = function () {
            var totlqty = 0;
            var availablestok = 0;
            totlqty = parseInt($scope.invacO_CheckOutQty);
            availablestok = parseInt($scope.avstock);
            if (totlqty > availablestok) {
                swal("Please Check Available Stock...!!");
                $scope.invacO_CheckOutQty = "";
            }
        };

        $scope.checkOut = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {};
                if ($scope.invacO_CheckOutQty <= 0) {
                    swal("Check-Out Quantity must be greater than zero....!!");
                    return;
                }
                if ($scope.editS === true) {
                    data = {
                        "INVMST_Id": $scope.invmsT_Id,
                        "INVMI_Id": $scope.invmI_Id,
                        "INVMLO_Id": $scope.obj.invmlO_Id.invmlO_Id,
                        "INVACO_CheckoutDate": $scope.invacO_CheckoutDate,
                        "INVACO_CheckOutQty": $scope.invacO_CheckOutQty,
                        "HRME_Id": $scope.hrmE_Id,
                        "INVACO_CheckOutRemarks": $scope.invacO_CheckOutRemarks,
                        "INVACO_Id": $scope.invacO_Id,
                        "INVACO_ReceivedBy": $scope.employeename,
                    };
                }
                else {
                    data = {
                        "INVMST_Id": $scope.obj.invmsT_Id.invmsT_Id,
                        "INVMI_Id": $scope.obj.INVMI_Id.INVMI_Id,
                        "INVSTO_SalesRate": $scope.obj.INVMI_Id.INVSTO_SalesRate,
                        "INVMLO_Id": $scope.obj.invmlO_Id.invmlO_Id,
                        "INVACO_CheckoutDate": $scope.invacO_CheckoutDate,
                        "INVACO_CheckOutQty": $scope.invacO_CheckOutQty,
                        "INVACO_ReceivedBy": $scope.employeename,
                        "HRME_Id": $scope.hrmE_Id,
                        "INVACO_CheckOutRemarks": $scope.invacO_CheckOutRemarks,
                        "INVACO_Id": $scope.invacO_Id
                    };
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("CheckOutAssets/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invacO_Id === 0 || promise.invacO_Id < 0) {
                            swal('Item Check-Out successfully');
                        }
                        else if (promise.invacO_Id > 0) {
                            swal('Updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invacO_Id === 0 || promise.invacO_Id < 0) {
                            swal('Failed to Check-Out, please contact administrator');
                        }
                        else if (promise.invacO_Id > 0) {
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

        $scope.edit = function (item, salerate) {
            $scope.editS = true;
            $scope.invmsT_Id = item.invmsT_Id;
            $scope.invmS_StoreName = item.invmS_StoreName;
            $scope.invmI_Id = item.invmI_Id;
            $scope.invmI_ItemName = item.invmI_ItemName;
            $scope.invstO_SalesRate = item.invstO_SalesRate;
            $scope.obj.invmlO_Id = item;
            $scope.contactflag = "E";
            $scope.hrmE_Id = item.hrmE_Id;
            $scope.employeename = item.invacO_ReceivedBy;
            $scope.invacO_CheckoutDate = new Date(item.invacO_CheckoutDate);
            $scope.invacO_CheckOutQty = item.invacO_CheckOutQty;
            $scope.invacO_CheckOutRemarks = item.invacO_CheckOutRemarks;
            $scope.invacO_Id = item.invacO_Id;
           
            var avstock = 0;
            var data = {
                "INVMI_Id": $scope.invmI_Id,
                "INVSTO_SalesRate": $scope.invstO_SalesRate,
                "INVMST_Id": $scope.invmsT_Id
            };
            apiService.create("INV_T_Sales/getitemDetail", data).
                then(function (promise) {
                    $scope.availablestock = promise.availablestock;
                    angular.forEach($scope.availablestock, function (ast) {
                        avstock += ast.AvaiableStock;
                    });
                    $scope.avstock = avstock;
                });

                //$scope.onitemchange();
           
           
            debugger;
            if ($scope.obj.invmlO_Id.invmlO_Id != null && $scope.obj.invmlO_Id.invmlO_Id != undefined) {
                debugger;
                $scope.onlocationchange();
            }
           

        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVACO_Id = item.invacO_Id;
            var dystring = "";
            if (item.invacO_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invacO_ActiveFlg === false) {
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
                        apiService.create("CheckOutAssets/deactive", item).
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