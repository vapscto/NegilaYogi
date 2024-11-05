
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckInAssetsController', CheckInAssetsController);
    CheckInAssetsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function CheckInAssetsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
        $scope.invacI_CheckInDate = new Date();

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("CheckInAssets/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_locations = promise.get_locations;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_checkin = promise.get_checkin;
                    $scope.presentCountgrid = $scope.get_checkin.length;
                });
        };
        //============================================ 
        $scope.onlocationchange = function () {
            $scope.id = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMLO_Id": $scope.id
            };
            apiService.create("CheckInAssets/getStore", data).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
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
        $scope.onstorechange = function () {
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.locationid = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMST_Id": $scope.storeid,
                "INVMLO_Id": $scope.locationid
            };
            apiService.create("CheckInAssets/getitems", data).
                then(function (promise) {
                    $scope.get_items = promise.get_items;
                    // $scope.checkoutqty = $scope.get_items[0].invacO_CheckOutQty;
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

        $scope.checkStock = function () {
            var totlqty = 0;
            var checkoutqty = 0;
            totlqty = parseInt($scope.invacI_CheckInQty);
            checkoutqty = parseInt($scope.checkoutqty);
            if (totlqty > checkoutqty) {
                swal("Please Check Check-out Quantity...!!");
                $scope.invacI_CheckInQty = "";
            }
        };


        $scope.checkIn = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMST_Id": $scope.obj.invmsT_Id.invmsT_Id,
                    "INVMI_Id": $scope.obj.invmI_Id.invmI_Id,
                    "INVSTO_SalesRate": $scope.obj.invmI_Id.invstO_SalesRate,
                    "INVMLO_Id": $scope.obj.invmlO_Id.invmlO_Id,
                    "INVACI_CheckInDate": $scope.invacI_CheckInDate,
                    "INVACI_CheckInQty": $scope.invacI_CheckInQty,
                    "INVACI_ReceivedBy": $scope.employeename,
                    "HRME_Id": $scope.hrmE_Id,
                    "INVACI_CheckInRemarks": $scope.invacI_CheckInRemarks,
                    "INVACI_Id": $scope.invacI_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("CheckInAssets/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invacI_Id === 0 || promise.invacI_Id < 0) {
                            swal('Item Check-In successfully');
                        }
                        else if (promise.invacI_Id > 0) {
                            swal('Updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invacI_Id === 0 || promise.invacI_Id < 0) {
                            swal('Failed to Check-In, please contact administrator');
                        }
                        else if (promise.invacI_Id > 0) {
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
            $scope.INVACI_Id = item.invacI_Id;
            var dystring = "";
            if (item.invacI_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invacI_ActiveFlg === false) {
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
                        apiService.create("CheckInAssets/deactive", item).
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

        //$scope.edit = function (item) {
        //    obj.invmsT_Id = item.invmsT_Id;
        //    $scope.obj.invmI_Id = item.invmI_Id;
        //    $scope.obj.invmlO_Id = item.invmlO_Id;
        //    $scope.obj.invmS_StoreName = item.invmS_StoreName;
        //    $scope.obj.invmI_ItemName = item.invmI_ItemName;
        //    $scope.obj.invmlO_LocationRoomName = item.invmlO_LocationRoomName;
        //    $scope.invacI_CheckInDate = item.invacI_CheckInDate;
        //    $scope.invacI_CheckInQty = item.invacI_CheckInQty;
        //    $scope.invacI_ReceivedBy = item.invacI_ReceivedBy;
        //    $scope.obj.hrmE_Id = item.hrmE_Id;
        //    $scope.invacO_CheckOutRemarks = item.invacO_CheckOutRemarks;
        //    $scope.invacO_Id = item.invacO_Id;

        //}
        $scope.edit = function (item) {
            $scope.editS = true;
            $scope.obj.invmsT_Id = item;
            $scope.obj.invmI_Id = item;
            $scope.obj.invmlO_Id = item;
            $scope.hrmE_Id = item.hrmE_Id;
            $scope.employeename = item.invacI_ReceivedBy;
            $scope.invacI_CheckInDate = new Date(item.invacI_CheckInDate);
            $scope.invacI_CheckInQty = item.invacI_CheckInQty;
            $scope.invacI_CheckInRemarks = item.invacI_CheckInRemarks;
            $scope.invacI_Id = item.invacI_Id;
            $scope.onitemchange();
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();