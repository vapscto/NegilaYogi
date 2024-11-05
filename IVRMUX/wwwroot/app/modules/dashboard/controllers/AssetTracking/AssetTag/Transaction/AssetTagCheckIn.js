
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagCheckInController', AssetTagCheckInController);
    AssetTagCheckInController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AssetTagCheckInController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        var date = new Date();
        $scope.invatcI_CheckInDate = date;
        $scope.transflag = false;
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
            apiService.getURI("AssetTagCheckIn/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_employee = promise.get_employee;
                    $scope.get_locations = promise.get_locations;
                    $scope.get_ATcheckout = promise.get_ATcheckout;
                });
        };
        //========================================= Location CHANGE
        $scope.onlocationchange = function () {
            $scope.get_store = "";
            $scope.get_items = "";
            $scope.get_itemtagdata = "";
            $scope.locationid = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMLO_Id": $scope.locationid
            };
            apiService.create("AssetTagCheckIn/getstore", data).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
                    $scope.get_contactperson = promise.get_contactperson;
                    $scope.contactflag = promise.contactflag;
                    if ($scope.get_contactperson.length > 0 || $scope.get_store.length > 0) {
                        if ($scope.contactflag === "E") {
                            $scope.hrmE_Id = $scope.get_contactperson[0].hrmE_Id;
                            $scope.employeename = $scope.get_contactperson[0].employeename;
                        }
                        else {
                            $scope.get_employee = promise.get_employee;
                            $scope.employeename = $scope.get_contactperson[0].invmlO_InchargeName;
                        }
                    }
                    else {
                        swal("No Record Found.... !!");
                        $scope.get_store = "";
                    }
                });
        };
        //========================================= STORE CHANGE
        $scope.onstorechange = function () {
            $scope.get_store = "";
            $scope.get_items = "";
            $scope.get_itemtagdata = "";
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.locationid = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMST_Id": $scope.storeid,
                "INVMLO_Id": $scope.locationid
            };
            apiService.create("AssetTagCheckIn/getitems", data).
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
            apiService.create("AssetTagCheckIn/getitemtagdata", data).
                then(function (promise) {
                    if (promise.get_itemtagdata.length > 0) {
                        $scope.transflag = true;
                        $scope.get_itemtagdata = promise.get_itemtagdata;
                        $scope.presentCountgrid = $scope.get_itemtagdata.length;
                    }
                    else {
                        $scope.transflag = false;
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
                $scope.tagckInArray = [];
                var qty = 1.00;
                angular.forEach($scope.get_itemtagdata, function (itag) {
                    if (itag.xyz) {
                        $scope.tagckInArray.push({
                            invaaT_Id: itag.invaaT_Id, invatcI_CheckInRemarks: itag.invatcI_CheckInRemarks
                        });
                    }
                });
                if ($scope.tagckInArray.length > 0) {
                    var data = {
                        "INVATCI_Id": $scope.invatcI_Id,
                        "INVMST_Id": $scope.obj.invmsT_Id.invmsT_Id,
                        "INVMI_Id": $scope.obj.invmI_Id.invmI_Id,
                        "INVMLO_Id": $scope.obj.invmlO_Id.invmlO_Id,
                        "HRME_Id": $scope.hrmE_Id,
                        "INVATCI_ReceivedBy": $scope.employeename,
                        "INVATCI_CheckInDate": $scope.invatcI_CheckInDate,
                        "INVATCI_CheckInQty": qty,
                        "tagckInArray": $scope.tagckInArray
                    };
                }
                else {
                    swal("Select Atleast One checkbox....!!");
                }

                apiService.create("AssetTagCheckIn/savedata", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invatcI_Id === 0 || promise.invatcI_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invatcI_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invatcI_Id === 0 || promise.invatcI_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invatcI_Id > 0) {
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
            $scope.INVATCI_Id = item.invatcI_Id;
            var dystring = "";
            if (item.invatcI_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invatcI_ActiveFlg === false) {
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
                        apiService.create("AssetTagCheckIn/deactive", item).
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