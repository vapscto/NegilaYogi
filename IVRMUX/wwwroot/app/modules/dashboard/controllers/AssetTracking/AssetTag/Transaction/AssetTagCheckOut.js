
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagCheckOutController', AssetTagCheckOutController);
    AssetTagCheckOutController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AssetTagCheckOutController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        var date = new Date();
        $scope.invatcO_CheckoutDate = date;
        var paginationformasters;
        $scope.transflag = false;
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
            apiService.getURI("AssetTagCheckOut/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_locations = promise.get_locations;
                    $scope.get_ATcheckout = promise.get_ATcheckout;
                });
        };
        //========================================= Drop-downs  CHANGE
        $scope.onstorechange = function () {
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            var data = {
                "INVMST_Id": $scope.storeid
            };
            apiService.create("AssetTagCheckOut/getitems", data).
                then(function (promise) {

                    if (promise.get_items.length > 0) {
                        $scope.get_items = promise.get_items;
                    }
                    else {
                        swal("No Record Found.... !!");
                        $scope.get_items = "";
                        $scope.obj.invmI_Id.invmI_Id = "";
                    }
                });
        };

        $scope.onitemchange = function () {
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.itemid = $scope.obj.invmI_Id.invmI_Id;
            var data = {
                "INVMST_Id": $scope.storeid,
                "INVMI_Id": $scope.itemid
            };
            apiService.create("AssetTagCheckOut/getitemtagdata", data).
                then(function (promise) {
                    if (promise.get_itemtagdata.length > 0) {
                        $scope.get_itemtagdata = promise.get_itemtagdata;
                        $scope.presentCountgrid = $scope.get_itemtagdata.length;

                    }
                    else {
                        swal("No Record Found.... !!");
                        $scope.get_itemtagdata = "";
                        $scope.obj.invmsT_Id.invmsT_Id = "";
                        $scope.obj.invmI_Id.invmI_Id = "";
                    }
                });
        };

        $scope.onlocationchange = function () {
            $scope.id = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMLO_Id": $scope.id
            };
            apiService.create("CheckOutAssets/getcontactperson", data).
                then(function (promise) {
                    $scope.get_contactperson = promise.get_contactperson;
                    $scope.contactflag = promise.contactflag;
                    if ($scope.get_contactperson.length > 0) {
                        $scope.transflag = true;
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
                $scope.tagckoutArray = [];
                var qty = 1.00;
                angular.forEach($scope.get_itemtagdata, function (itag) {
                    if (itag.xyz) {
                        $scope.tagckoutArray.push({
                            invaaT_Id: itag.INVAAT_Id, invatcO_CheckOutRemarks: itag.invatcO_CheckOutRemarks
                        });
                    }
                });
                if ($scope.tagckoutArray.length > 0) {
                    var data = {
                        "INVATCO_Id": $scope.invatcO_Id,
                        "INVMST_Id": $scope.obj.invmsT_Id.invmsT_Id,
                        "INVMI_Id": $scope.obj.invmI_Id.invmI_Id,
                        "INVMLO_Id": $scope.obj.invmlO_Id.invmlO_Id,
                        "HRME_Id": $scope.hrmE_Id,
                        "INVATCO_ReceivedBy": $scope.employeename,
                        "INVATCO_CheckoutDate": $scope.invatcO_CheckoutDate,
                        "INVATCO_CheckOutQty": qty,
                        "tagckoutArray": $scope.tagckoutArray
                    };
                }
                else {
                    swal("Select Atleast One checkbox....!!");
                }

                apiService.create("AssetTagCheckOut/savedata", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invatcO_Id === 0 || promise.invatcO_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invatcO_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invatcO_Id === 0 || promise.invatcO_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invatcO_Id > 0) {
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
            $scope.INVATCO_Id = item.invatcO_Id;
            var dystring = "";
            if (item.invatcO_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invatcO_ActiveFlg === false) {
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
                        apiService.create("AssetTagCheckOut/deactive", item).
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

        $scope.edit = function (item) {
            $scope.obj.invmsT_Id = item;
            $scope.obj.invmI_Id = item;
            $scope.obj.invmlO_Id = item;
            $scope.invatcO_CheckoutDate = new Date(item.invatcO_CheckoutDate);
            $scope.edits = true;
            $scope.get_itemtagdata = [];
            $scope.taglist = [];
            $scope.taglist.push(item);
            angular.forEach($scope.taglist, function (ltag) {
                $scope.get_itemtagdata.push({
                    INVAAT_Id: ltag.invaaT_Id, INVAAT_AssetId: ltag.invaaT_AssetId, INVMI_Id: ltag.invmI_Id,
                    INVMI_ItemName: ltag.invmI_ItemName,
                    INVAAT_AssetDescription: ltag.invaaT_AssetDescription,
                    INVAAT_ModelNo: ltag.invaaT_ModelNo, INVAAT_SerialNo: ltag.invaaT_SerialNo,
                    // hrmE_Id: ltag.hrmE_Id, invatcO_ReceivedBy: ltag.invatcO_ReceivedBy,
                    invatcO_CheckOutRemarks: ltag.invatcO_CheckOutRemarks, xyz: true
                });

                angular.forEach($scope.get_employee, function (emp) {
                    if (ltag.hrmE_Id === emp.hrmE_Id) {
                        $scope.obj.hrmE_Id = ltag.hrmE_Id;
                        $scope.obj.invatcO_ReceivedBy = ltag.employeename;
                    }
                });
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