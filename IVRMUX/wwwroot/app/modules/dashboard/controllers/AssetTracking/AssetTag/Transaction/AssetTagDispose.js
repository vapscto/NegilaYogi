
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagDisposeController', AssetTagDisposeController);
    AssetTagDisposeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AssetTagDisposeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        var date = new Date();
        $scope.invatdI_DisposedDate = date;
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
            apiService.getURI("AssetTagDispose/getloaddata", pageid).
                then(function (promise) {    
                    $scope.get_store = promise.get_store;
                 
                    $scope.get_ATcheckout = promise.get_ATcheckout;
                });
        };
        //========================================= Location CHANGE
        $scope.onstorechange = function () {          
            $scope.get_locations = "";    
            $scope.get_items = "";
            $scope.get_itemtagdata = "";
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            var data = {
                "INVMST_Id": $scope.storeid
            };
            apiService.create("AssetTagDispose/getlocation", data).
                then(function (promise) {                 
                    if (promise.get_locations.length > 0) {
                        $scope.get_locations = promise.get_locations;
                    }
                    else {
                        swal("No Record Found.... !!");
                        $scope.get_locations = "";
                    }
                });
        };
        //========================================= STORE CHANGE
        $scope.onlocationchange = function () {
            $scope.get_locations = "";
            $scope.get_items = "";
            $scope.get_itemtagdata = "";
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.locationid = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMST_Id": $scope.storeid,
                "INVMLO_Id": $scope.locationid
            };
            apiService.create("AssetTagDispose/getitems", data).
                then(function (promise) {                 
                    if (promise.get_items.length > 0) {
                        $scope.get_items = promise.get_items;
                    }
                    else {
                        swal("No Record Found.... !!");
                        $scope.get_items = "";
                    }
                });
        };

        $scope.onitemchange = function () {
            $scope.locationid = $scope.obj.invmlO_Id.invmlO_Id;
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.itemid = $scope.obj.invmI_Id.invmI_Id;
            var data = {
                "INVMLO_Id": $scope.locationid,
                "INVMST_Id": $scope.storeid,
                "INVMI_Id": $scope.itemid
            };
            apiService.create("AssetTagDispose/getitemtagdata", data).
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
                $scope.tagDisposeArray = [];
                var qty = 1.00;
                angular.forEach($scope.get_itemtagdata, function (dtag) {
                    if (dtag.xyz) {
                        $scope.tagDisposeArray.push({
                            invaaT_Id: dtag.invaaT_Id, invatdI_DisposedRemarks: dtag.invatdI_DisposedRemarks
                        });
                    }
                });
                if ($scope.tagDisposeArray.length > 0) {
                    var data = {
                        "INVATDI_Id": $scope.invatdI_Id,
                        "INVMST_Id": $scope.obj.invmsT_Id.invmsT_Id,
                        "INVMI_Id": $scope.obj.invmI_Id.invmI_Id,
                        "INVMLO_Id": $scope.obj.invmlO_Id.invmlO_Id,
                        "INVATDI_DisposedDate": $scope.invatdI_DisposedDate,
                        "INVATDI_DisposedQty": qty,
                        "tagDisposeArray": $scope.tagDisposeArray
                    };
                }
                else {
                    swal("Select Atleast One checkbox....!!");
                }

                apiService.create("AssetTagDispose/savedata", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invatdI_Id === 0 || promise.invatdI_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invatdI_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invatdI_Id === 0 || promise.invatdI_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invatdI_Id > 0) {
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
            $scope.INVATDI_Id = item.invatdI_Id;
            var dystring = "";
            if (item.invatdI_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invatdI_ActiveFlg === false) {
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
                        apiService.create("AssetTagDispose/deactive", item).
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