
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagTransferController', AssetTagTransferController);
    AssetTagTransferController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AssetTagTransferController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        var date = new Date();
        $scope.invattR_CheckoutDate = date;
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

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = 10;
            $scope.itemsPerPage1 = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("AssetTagTransfer/getloaddata", pageid).
                then(function (promise) {

                    $scope.get_fromlocations = promise.get_fromlocations;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_ATTransfer = promise.get_ATTransfer;
                    $scope.presentCountgrid1 = $scope.get_ATTransfer.length;
                });
        };
        //========================================= STORE CHANGE
        $scope.onlocationchange = function () {
            $scope.get_items = "";
            $scope.get_itemtagdata = "";
            $scope.locationid = $scope.obj.invmloFrom_Id.invmloFrom_Id;
            var data = {
                "INVMLO_Id": $scope.locationid
            };
            apiService.create("AssetTagTransfer/getitems", data).
                then(function (promise) {

                    if (promise.get_items.length > 0) {
                        $scope.get_items = promise.get_items;
                        $scope.get_tolocations = promise.get_tolocations;
                    }
                    else {
                        swal("No Record Found.... !!");
                        $scope.get_items = "";
                    }
                });
        };
        $scope.onitemchange = function () {
            $scope.locationid = $scope.obj.invmloFrom_Id.invmloFrom_Id;
            $scope.itemid = $scope.obj.invmI_Id.invmI_Id;
            var data = {
                "INVMLO_Id": $scope.locationid,
                "INVMI_Id": $scope.itemid
            };
            apiService.create("AssetTagTransfer/getitemtagdata", data).
                then(function (promise) {
                    if (promise.get_itemtagdata.length > 0) {
                        $scope.get_itemtagdata = promise.get_itemtagdata;
                        $scope.presentCountgrid = $scope.get_itemtagdata.length;
                    }
                    else {
                        swal("No Record Found.... !!");
                        $scope.get_itemtagdata = "";
                    }
                });
        };

        //======================================= Grid Check box Selection
        $scope.toggleAll = function () {
            angular.forEach($scope.get_itemtagdata, function (subj) {
                subj.xyz = $scope.all;
            });
        };
        $scope.optionToggled = function () {
            $scope.all = $scope.get_itemtagdata.every(function (itm) { return itm.xyz; });
        };
        //=====================================  SAVE DATA
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.tagTransferArray = [];
                var qty = 1.00;
                angular.forEach($scope.get_itemtagdata, function (itag) {
                    if (itag.xyz) {
                        $scope.tagTransferArray.push({
                            invaaT_Id: itag.INVAAT_Id, invattR_ReceivedBy: itag.hrmE_Id.employeename, invattR_CheckOutRemarks: itag.invattR_CheckOutRemarks
                        });
                    }
                });
                if ($scope.tagTransferArray.length > 0) {
                    var data = {
                        "INVATTR_Id": $scope.invattR_Id,
                        "INVMLOFrom_Id": $scope.obj.invmloFrom_Id.invmloFrom_Id,
                        "INVMLOTo_Id": $scope.obj.invmloTo_Id.invmloTo_Id,
                        "INVMI_Id": $scope.obj.invmI_Id.invmI_Id,                     
                        "INVATTR_CheckoutDate": $scope.invattR_CheckoutDate,
                        "INVATTR_CheckOutQty": qty,
                        "tagTransferArray": $scope.tagTransferArray
                    };
                }
                else {
                    swal("Select Atleast One checkbox....!!");
                }

                apiService.create("AssetTagTransfer/savedata", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invattR_Id === 0 || promise.invattR_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invattR_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invattR_Id === 0 || promise.invattR_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invattR_Id > 0) {
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
        $scope.deactive = function (item) {
            $scope.INVATTR_Id = item.INVATTR_Id;
            var dystring = "";
            if (item.INVATTR_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.INVATTR_ActiveFlg === false) {
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
                        apiService.create("AssetTagTransfer/deactive", item).
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
        $scope.searchValue1 = '';


    }
})();