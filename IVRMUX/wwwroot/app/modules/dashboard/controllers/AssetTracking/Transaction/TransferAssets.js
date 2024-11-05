
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TransferAssetsController', TransferAssetsController);
    TransferAssetsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function TransferAssetsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



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
        $scope.obj = {};
        $scope.invatR_CheckoutDate = new Date();

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("TransferAssets/getloaddata", pageid).
                then(function (promise) {

                    $scope.get_locations = promise.get_locations;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_transfer = promise.get_transfer;
                    $scope.presentCountgrid = $scope.get_transfer.length;
                });
        };
        //============================================        
        $scope.onlocationchange = function () {
            $scope.id = $scope.obj.invmlO_Id.invmlO_Id;
           
            var data = {
                "INVMLO_Id": $scope.id
            };
            apiService.create("TransferAssets/gettolocations", data).
                then(function (promise) {
                    $scope.get_items = promise.get_items;
                    $scope.get_tolocations = promise.get_tolocations;
                });
        };
         //============================================ TO location change  
        $scope.tolocationchange = function () {
            $scope.toid = $scope.obj.toinvmlO_Id.invmlO_Id;
            var data = {
                "INVMLO_Id": $scope.toid
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
                        $scope.get_employee = promise.get_employee;
                        $scope.employeename = $scope.get_contactperson[0].invmlO_InchargeName;
                    }
                });
        };

        $scope.onitemchange = function () {
            $scope.locationid = $scope.obj.invmlO_Id.invmlO_Id;
            $scope.itemid = $scope.obj.invmI_Id.invmI_Id;

            var data = {
                "INVMLO_Id": $scope.locationid,
                "INVMI_Id": $scope.itemid,
                "INVSTO_SalesRate": $scope.obj.invmI_Id.invstO_SalesRate
            };
            apiService.create("TransferAssets/getitemdetails", data).
                then(function (promise) {
                    $scope.get_itemdetails = promise.get_itemdetails;
                    $scope.coquantity = $scope.get_itemdetails[0].invacO_CheckOutQty;
                });
        };
            
        $scope.checkStock = function () {
            var totlqty = 0;
            var availablestok = 0;
            totlqty = parseInt($scope.invatR_CheckOutQty);
            availablestok = parseInt($scope.coquantity);
            if (totlqty > availablestok) {
                swal("Please Check Available Quantity...!!");
                $scope.invatR_CheckOutQty = "";
            }
        };


        $scope.transfer = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "INVMLOFrom_Id": $scope.obj.invmlO_Id.invmlO_Id,
                    "INVMI_Id": $scope.obj.invmI_Id.invmI_Id,
                    "INVMLOTo_Id": $scope.obj.toinvmlO_Id.invmlO_Id,
                    "INVATR_CheckoutDate": $scope.invatR_CheckoutDate,
                    "INVATR_CheckOutQty": $scope.invatR_CheckOutQty,
                    "INVATR_ReceivedBy": $scope.employeename,
                    "HRME_Id": $scope.hrmE_Id,
                    "INVATR_CheckOutRemarks": $scope.invatR_CheckOutRemarks,
                    "INVATR_Id": $scope.invatR_Id,
                    "INVSTO_SalesRate": $scope.obj.invmI_Id.invstO_SalesRate
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("TransferAssets/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invatR_Id == 0 || promise.invatR_Id < 0) {
                            swal('Item Transfer successfully');
                        }
                        else if (promise.invatR_Id > 0) {
                            swal('Updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invatR_Id == 0 || promise.invatR_Id < 0) {
                            swal('Failed to Transfer, please contact administrator');
                        }
                        else if (promise.invatR_Id > 0) {
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
            $scope.INVACO_Id = item.invacO_Id;
            var dystring = "";
            if (item.invacO_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invacO_ActiveFlg == false) {
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
                        apiService.create("TransferAssets/deactive", item).
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
            $scope.obj.invmsT_Id = item.invmsT_Id;
            $scope.obj.invmI_Id = item.invmI_Id;
            $scope.obj.invmlO_Id = item.invmlO_Id;
            $scope.obj.invmS_StoreName = item.invmS_StoreName;
            $scope.obj.invmI_ItemName = item.invmI_ItemName;
            $scope.obj.invmlO_LocationRoomName = item.invmlO_LocationRoomName;
            $scope.invacO_CheckoutDate = item.invacO_CheckoutDate;
            $scope.invacO_CheckOutQty = item.invacO_CheckOutQty;
            $scope.receivedby = item.invacO_ReceivedBy;
            $scope.empid = item.hrmE_Id;
            $scope.invacO_CheckOutRemarks = item.invacO_CheckOutRemarks;
            $scope.invacO_Id = item.invacO_Id;

        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();