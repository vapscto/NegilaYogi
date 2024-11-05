
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_PurchaseRequisitionController', INV_PurchaseRequisitionController)

    INV_PurchaseRequisitionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_PurchaseRequisitionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var date = new Date();
        $scope.invmpR_PRDate = date;
        $scope.addro = false;
        $scope.obj = {};
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

        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addprrows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i == $scope.transrows.length; i++) {
                        var id = $scope.transrows[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.removeprrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deleteprrows(data);
            }
            if ($scope.transrows.length == 0) {
            }
        };

        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag == "INVPR") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_PurchaseRequisition/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_Store = promise.get_Store;
                    $scope.get_supplier = promise.get_supplier;
                    $scope.get_item = promise.get_item;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_purchaserequisition = promise.get_purchaserequisition;
                    $scope.presentCountgrid = $scope.get_purchaserequisition.length;

                })
        };

        //====================================== On Item Change
        $scope.onitemchange = function (itemid, objid) {
            $scope.INVSTO_AvaiableStock = 0;
            var item_id = itemid.invmI_Id;
            var data = {
                "INVMI_Id": item_id
            }
            apiService.create("INV_PurchaseRequisition/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    $scope.balancestock = promise.balancestock;
                    $scope.INVSTO_AvaiableStock = $scope.balancestock[0].INVSTO_AvaiableStock;

                    angular.forEach($scope.transrows, function (obj) {
                        if (obj.itrS_Id == objid.itrS_Id) {
                            obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;
                            obj.INVSTO_PurOBQty = $scope.balancestock[0].PurOBQty;

                        }
                    })
                })
        }

        //================== Count Amount
        $scope.countitemAmt = function (probj, items) {
            var a = 0;
            $scope.purrate = parseFloat(probj.invtpR_PRUnitRate);
            $scope.qty = parseFloat(probj.invtpR_PRQty);
            probj.invtpR_ApproxAmount = $scope.purrate * $scope.qty;
            angular.forEach($scope.transrows, function (obj) {
                a += parseFloat(obj.invtpR_ApproxAmount);
            })
            var totamt = a;
            $scope.invmpR_ApproxTotAmount = totamt;
            $scope.invmpR_ApproxTotAmount = parseFloat($scope.invmpR_ApproxTotAmount);
            $scope.invmpR_ApproxTotAmount = $scope.invmpR_ApproxTotAmount.toFixed(2);
        }

        //===================================== SAVE DATA

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.arrayPR = [];
                angular.forEach($scope.transrows, function (pr) {
                    $scope.arrayPR.push({
                        invtpR_Id: pr.invtpR_Id, invmI_Id: pr.invmI_Id.invmI_Id, invmuoM_Id: pr.invmI_Id.invmuoM_Id, invtpR_PRQty: pr.invtpR_PRQty, invtpR_PRUnitRate: pr.invtpR_PRUnitRate,
                        invtpR_ApproxAmount: pr.invtpR_ApproxAmount, invtpR_Remarks: pr.invtpR_Remarks
                    });
                })
                var data = {
                    "INVMPR_Remarks": $scope.invmpR_Remarks,
                    "INVMPR_PRDate": $scope.invmpR_PRDate,
                    "INVMPR_ApproxTotAmount": $scope.invmpR_ApproxTotAmount,
                    "arrayPR": $scope.arrayPR,
                    "INVMPR_Id": $scope.invmpR_Id,

                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_PurchaseRequisition/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmpR_Id == 0 || promise.invmpR_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmpR_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmpR_Id == 0 || promise.invmpR_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmpR_Id > 0) {
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

        $scope.deactiveM = function (item, SweetAlert) {
            $scope.invmpR_Id = item.invmpR_Id;
            var dystring = "";
            if (item.invmpR_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmpR_ActiveFlg == false) {
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
                        apiService.create("INV_PurchaseRequisition/deactiveM", item).
                            then(function (promise) {
                                if (promise.returnval_1 == "Success") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval_1 == "Fail") {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval_1 == "Process") {
                                    swal("Your Requisition Already Processed!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.invtpR_Id = item.invtpR_Id;
            //    $scope.invmpR_Id = item.invmpR_Id;
            var dystring = "";
            if (item.invtpR_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invtpR_ActiveFlg == false) {
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
                        apiService.create("INV_PurchaseRequisition/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }

                                $scope.onformclick(item);
                                $scope.loaddata();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }




        $scope.edit = function (item, row) {
            $scope.transrows = [];

            $scope.invmpR_ApproxTotAmount = item.invmpR_ApproxTotAmount;
            $scope.invmpR_Remarks = item.invmpR_Remarks;
            $scope.invmpR_PRDate = new Date(item.invmpR_PRDate);
            $scope.invmpR_Id = item.invmpR_Id;
            $scope.invtpR_Id = item.invtpR_Id;
            $scope.edits = true;
            var invmpR_Id = item.invmpR_Id;
            var data = {
                "INVMPR_Id": invmpR_Id
            }
            apiService.create("INV_PurchaseRequisition/edit", data).
                then(function (promise) {
                    $scope.editPR = promise.editPR;
                    angular.forEach($scope.editPR, function (obje) {
                        $scope.transrows.push(obje);
                    })
                })
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';


        $scope.onformclick = function (itemid) {
            $scope.get_prDetail = "";
            var item_id = itemid.invmpR_Id;
            var data = {
                "INVMPR_Id": item_id
            }
            apiService.create("INV_PurchaseRequisition/get_prdetails", data).
                then(function (promise) {
                    $scope.get_prDetail = promise.get_prDetail;
                    $scope.prnum = $scope.get_prDetail[0].invmpR_PRNo;
                    $scope.reqname = $scope.get_prDetail[0].employeename;
                    $scope.get_pidataExP = promise.get_pidata;
                })
        }

    }
})();