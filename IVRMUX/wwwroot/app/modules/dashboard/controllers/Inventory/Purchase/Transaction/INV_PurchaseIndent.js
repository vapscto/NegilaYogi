
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_PurchaseIndentController', INV_PurchaseIndentController);
    INV_PurchaseIndentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_PurchaseIndentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.searchValue2 = '';
        var date = new Date();
        $scope.invmpI_PIDate = date;
        $scope.apflag = false;
        $scope.editS = false;
        $scope.printbill = false;
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
        $scope.ind_Mi_id = ivrmcofigsettings[0].mI_Id;

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
        $scope.addpirows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i === $scope.transrows.length; i++) {
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
        $scope.removepirows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletepirows(data);
            }

        };

        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "INVPI") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_PurchaseIndent/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_prNo = promise.get_prNo;
                    $scope.get_item = promise.get_item;
                    $scope.get_purchaseindent = promise.get_purchaseindent;
                    $scope.presentCountgrid = $scope.get_purchaseindent.length;
                    if (promise.prflag === true) {
                        $scope.prflag = true;
                    }
                    else {
                        $scope.prflag = false;
                    }
                    //$scope.ap_list = [];
                    //angular.forEach($scope.get_purchaseindent, function (pino) {
                    //    angular.forEach(promise.get_prNo, function (prno) {
                    //        if (pino.invmpR_Id == prno.invmpR_Id) {
                    //            $scope.apflag = true;
                    //        }
                    //        else {
                    //            $scope.apflag = false;
                    //        } 
                    //        $scope.ap_list.push({
                    //            'invmpR_Id': prno.invmpR_Id, 'hrmE_Id': prno.hrmE_Id, 'employeename': prno.employeename, 'invmpR_ApproxTotAmount': prno.invmpR_ApproxTotAmount, 'invmpR_PRDate': prno.invmpR_PRDate, 'apflag': $scope.apflag
                    //        });
                    //    })
                    //})

                    //$scope.ap_list = [];
                    //angular.forEach(promise.get_prNo, function (prno) {
                    //    angular.forEach($scope.get_purchaseindent, function (pino) {

                    //        if (pino.invmpR_Id == prno.invmpR_Id) {
                    //            $scope.apflag = true;
                    //        }
                    //        else {
                    //            $scope.apflag = false;
                    //        }
                    //        $scope.ap_list.push({
                    //            'invmpR_Id': prno.invmpR_Id, 'hrmE_Id': prno.hrmE_Id, 'employeename': prno.employeename, 'invmpR_ApproxTotAmount': prno.invmpR_ApproxTotAmount, 'invmpR_PRDate': prno.invmpR_PRDate, 'apflag': $scope.apflag
                    //        });

                    //    })
                    //})




                });
        };


        //======================================== View PR
        $('#myModal').modal('hide');
        $scope.viewtx = function (objc) {
            var invmprid = objc.invmpR_Id;
            var data = {
                "INVMPR_Id": invmprid
            };
            apiService.create("INV_PurchaseIndent/getprDetail", data).
                then(function (promise) {
                    $scope.get_indentDetail = promise.get_indentDetail;
                    $scope.invmpI_ApproxTotAmount = $scope.get_indentDetail[0].invmpR_ApproxTotAmount;
                    $scope.prno = $scope.get_indentDetail[0].invmpR_PRNo;
                    $scope.indent_list = [];
                    angular.forEach($scope.get_indentDetail, function (idn) {
                        $scope.indent_list.push({
                            'invmpR_Id': idn.invmpR_Id, 'invtpR_Id': idn.invtpR_Id, 'invmI_Id': idn.invmI_Id, 'invmuoM_Id': idn.invmuoM_Id, invmpR_PRNo: idn.invmpR_PRNo,
                            'hrmE_Id': idn.hrmE_Id, 'invmI_ItemName': idn.invmI_ItemName, 'invmI_ItemCode': idn.invmI_ItemCode, 'invmuoM_UOMName': idn.invmuoM_UOMName,
                            'employeename': idn.employeename, 'invtpR_PRQty': idn.invtpR_PRQty, 'invtpI_PIQty': idn.invtpR_PRQty, 'invtpR_PRUnitRate': idn.invtpR_PRUnitRate, 'invtpI_PIUnitRate': idn.invtpR_PRUnitRate, 'invtpR_ApproxAmount': idn.invtpR_ApproxAmount, 'invmpR_ApproxTotAmount': idn.invmpR_ApproxTotAmount, 'invtpI_ApproxAmount': idn.invtpR_ApproxAmount, 'invtpR_ApprovedQty': idn.invtpR_ApprovedQty, 'invmpR_PRDate': idn.invmpR_PRDate, 'invmpR_Remarks': idn.invmpR_Remarks, 'invmpI_ApproxTotAmount': idn.invmpR_ApproxTotAmount, 'invtpR_ActiveFlg': idn.invtpR_ActiveFlg, 'invmpR_PICreatedFlg': idn.invmpR_PICreatedFlg
                        });
                    });
                    $('#myModal').modal('show');
                });
        };
        ////====================================== Add To Cart

        $scope.finalindent_list = [];
        $scope.newindent_list = [];
        $scope.addtocart = function (objtx) {
            var a = 0;
            //  angular.forEach(objtx, function (objx) {
            angular.forEach($scope.indent_list, function (oobj) {
                //  if (objx.invtpR_Id == oobj.invtpR_Id) {
                $scope.newindent_list.push(oobj);
                //  }
                // })
            });
            angular.forEach($scope.newindent_list, function (obj) {
                a += parseFloat(obj.invtpI_ApproxAmount);
            });
            var totamt = a;
            $scope.invmpI_ApproxTotAmount = totamt;
            $scope.invmpI_ApproxTotAmount = parseFloat($scope.invmpI_ApproxTotAmount);
            $scope.invmpI_ApproxTotAmount = $scope.invmpI_ApproxTotAmount.toFixed(2);
        };


        //====================================== On Item Change
        $scope.onitemchange = function (itemid, objid) {
            var item_id = itemid.invmI_Id;
            var data = {
                "INVMI_Id": item_id
            };
            apiService.create("INV_PurchaseRequisition/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    angular.forEach($scope.transrows, function (obj) {
                        if (obj.itrS_Id === objid.itrS_Id) {
                            obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;
                            obj.invstO_AvaiableStock = $scope.get_itemDetail[0].invstO_AvaiableStock;
                        }
                    });
                });
        };


        //================== Count Amount
        $scope.countitemAmt = function (rowobj) {
            var a = 0;
            $scope.purrate = parseFloat(rowobj.invtpI_PIUnitRate);
            $scope.qty = parseFloat(rowobj.invtpI_PIQty);
            rowobj.invtpI_ApproxAmount = $scope.purrate * $scope.qty;
            if ($scope.editS === true) {
                angular.forEach($scope.transrowsedit, function (obj) {
                    a += parseFloat(obj.invtpI_ApproxAmount);
                });
            }
            else {
                angular.forEach($scope.transrows, function (obj) {
                    a += parseFloat(obj.invtpI_ApproxAmount);
                });
            }
            var totamt = a;
            $scope.invmpI_ApproxTotAmount = totamt;
            $scope.invmpI_ApproxTotAmount = parseFloat($scope.invmpI_ApproxTotAmount);
            $scope.invmpI_ApproxTotAmount = $scope.invmpI_ApproxTotAmount.toFixed(2);
        };

        //================== Count Amount rate
        $scope.countitemAmtQ = function (rowobj) {
            var a = 0;
            $scope.purrate = parseFloat(rowobj.invtpI_PIUnitRate);
            $scope.qty = parseFloat(rowobj.invtpI_PIQty);
            rowobj.invtpI_ApproxAmount = $scope.purrate * $scope.qty;
            if ($scope.editS === true) {
                angular.forEach($scope.transrowsedit, function (obj) {
                    a += parseFloat(obj.invtpI_ApproxAmount);
                });
            }
            else {
                angular.forEach($scope.transrows, function (obj) {
                    a += parseFloat(obj.invtpI_ApproxAmount);
                });
            }
            var totamt = a;
            $scope.invmpI_ApproxTotAmount = totamt;
            $scope.invmpI_ApproxTotAmount = parseFloat($scope.invmpI_ApproxTotAmount);
            $scope.invmpI_ApproxTotAmount = $scope.invmpI_ApproxTotAmount.toFixed(2);
        };

        //===================================== PR Change
        //$scope.onprchange = function (itemid) {
        //    $scope.get_indentDetail = "";
        //    var item_id = itemid.invmpR_Id;
        //    var data = {
        //        "INVMPR_Id": item_id
        //    }
        //    apiService.create("INV_PurchaseIndent/getprDetail", data).
        //        then(function (promise) {
        //            $scope.get_indentDetail = promise.get_indentDetail;
        //            $scope.invmpI_ApproxTotAmount = $scope.get_indentDetail[0].invmpR_ApproxTotAmount;
        //            $scope.indent_list = [];
        //            angular.forEach($scope.get_indentDetail, function (idn) {
        //                $scope.indent_list.push({
        //                    invmpR_Id: idn.invmpR_Id, invtpR_Id: idn.invtpR_Id, invmI_Id: idn.invmI_Id, invmuoM_Id: idn.invmuoM_Id,
        //                    hrmE_Id: idn.hrmE_Id, invmI_ItemName: idn.invmI_ItemName, invmI_ItemCode: idn.invmI_ItemCode, invmuoM_UOMName: idn.invmuoM_UOMName,
        //                    employeename: idn.employeename, invtpR_PRQty: idn.invtpR_PRQty, invtpI_PIQty: idn.invtpR_PRQty, invtpR_PRUnitRate: idn.invtpR_PRUnitRate, invtpI_PIUnitRate: idn.invtpR_PRUnitRate, invtpR_ApproxAmount: idn.invtpR_ApproxAmount, invmpR_ApproxTotAmount: idn.invmpR_ApproxTotAmount, invtpI_ApproxAmount: idn.invtpR_ApproxAmount, invtpR_ApprovedQty: idn.invtpR_ApprovedQty, invmpR_PRDate: idn.invmpR_PRDate, invmpR_Remarks: idn.invmpR_Remarks, invmpI_ApproxTotAmount: idn.invmpR_ApproxTotAmount, invtpR_ActiveFlg: idn.invtpR_ActiveFlg, invmpR_PICreatedFlg: idn.invmpR_PICreatedFlg
        //                });
        //            })
        //        })
        //}

        //================== Count Amount
        $scope.countAQAmt = function (piobj) {
            var a = 0;
            $scope.purrate = parseFloat(piobj.invtpI_PIUnitRate);
            $scope.qty = parseFloat(piobj.invtpI_PIQty);
            piobj.invtpI_ApproxAmount = $scope.purrate * $scope.qty;
            angular.forEach($scope.indent_list, function (obj) {
                a += parseFloat(obj.invtpI_ApproxAmount);
            });
            var totamt = a;
            $scope.invmpI_ApproxTotAmount = totamt;
            $scope.invmpI_ApproxTotAmount = parseFloat($scope.invmpI_ApproxTotAmount);
            $scope.invmpI_ApproxTotAmount = $scope.invmpI_ApproxTotAmount.toFixed(2);
        };

        //===================================== SAVE DATA

        $scope.savedata = function () {
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                $scope.arrayPI = [];
                if ($scope.prflag === true) {
                    angular.forEach($scope.newindent_list, function (li) {
                        $scope.arrayPI.push({
                            invmpR_Id: li.invmpR_Id, invtpR_Id: li.invtpR_Id, invmI_Id: li.invmI_Id, invmuoM_Id: li.invmuoM_Id,
                            invtpI_PRQty: li.invtpR_PRQty, invtpI_PIQty: li.invtpI_PIQty, invtpI_PIUnitRate: li.invtpI_PIUnitRate,
                            invtpI_ApproxAmount: li.invtpI_ApproxAmount, invtpI_Remarks: li.invtpI_Remarks
                        });
                    });
                    data = {
                        "INVMPI_ReferenceNo": $scope.invmpI_ReferenceNo,
                        "INVMPI_PIDate": $scope.invmpI_PIDate,
                        "INVMPI_Remarks": $scope.invmpI_Remarks,
                        "INVMPI_ApproxTotAmount": $scope.invmpI_ApproxTotAmount,
                        "arrayPI": $scope.arrayPI,
                        "prflag": $scope.prflag,
                        "INVMPI_Id": $scope.invmpI_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
                }
                else if ($scope.prflag === true && $scope.editS === true || $scope.transrowsedit > 0) {
                    angular.forEach($scope.transrowsedit, function (tr) {
                        $scope.arrayPI.push(tr);
                        //$scope.arrayPI.push({
                        //    invmpI_Id: tr.invmpI_Id, invmI_Id: tr.invmI_Id, invmI_ItemName: tr.invmI_ItemName, invmuoM_Id: tr.invmuoM_Id,
                        //    invmuoM_UOMName: tr.invmuoM_UOMName, invtpI_PRQty: tr.invtpI_PRQty, invtpI_PIQty: tr.invtpI_PIQty,
                        //    invtpI_PIUnitRate: tr.invtpI_PIUnitRate, invtpI_ApproxAmount: tr.invtpI_ApproxAmount,
                        //    invtpI_Remarks: tr.invtpI_Remarks
                        //});
                    });
                    data = {
                        "INVMPI_ReferenceNo": $scope.invmpI_ReferenceNo,
                        "INVMPI_PIDate": $scope.invmpI_PIDate,
                        "INVMPI_Remarks": $scope.invmpI_Remarks,
                        "INVMPI_ApproxTotAmount": $scope.invmpI_ApproxTotAmount,
                        "arrayPI": $scope.arrayPI,
                        "prflag": $scope.prflag,
                        "INVMPI_Id": $scope.invmpI_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
                }
                else {
                    angular.forEach($scope.transrows, function (tr) {
                        $scope.arrayPI.push({
                            invmI_Id: tr.invmI_Id.invmI_Id, invmuoM_Id: tr.invmI_Id.invmuoM_Id, invtpI_PIQty: tr.invtpI_PIQty,
                            invtpI_PIUnitRate: tr.invtpI_PIUnitRate, invtpI_ApproxAmount: tr.invtpI_ApproxAmount, invtpI_Remarks: tr.invtpI_Remarks
                        });
                    });
                    data = {
                        "INVMPI_ReferenceNo": $scope.invmpI_ReferenceNo,
                        "INVMPI_PIDate": $scope.invmpI_PIDate,
                        "INVMPI_Remarks": $scope.invmpI_Remarks,
                        "INVMPI_ApproxTotAmount": $scope.invmpI_ApproxTotAmount,
                        "arrayPI": $scope.arrayPI,
                        "prflag": $scope.prflag,
                        "INVMPI_Id": $scope.invmpI_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_PurchaseIndent/savedetails", data).then(function (promise) {

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
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.savedata = function () {
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                $scope.arrayPI = [];
                if ($scope.prflag === true) {
                    angular.forEach($scope.newindent_list, function (li) {
                        $scope.arrayPI.push({
                            invmpR_Id: li.invmpR_Id, invtpR_Id: li.invtpR_Id, invmI_Id: li.invmI_Id, invmuoM_Id: li.invmuoM_Id,
                            invtpI_PRQty: li.invtpR_PRQty, invtpI_PIQty: li.invtpI_PIQty, invtpI_PIUnitRate: li.invtpI_PIUnitRate,
                            invtpI_ApproxAmount: li.invtpI_ApproxAmount, invtpI_Remarks: li.invtpI_Remarks
                        });
                    });
                    data = {
                        "INVMPI_ReferenceNo": $scope.invmpI_ReferenceNo,
                        "INVMPI_PIDate": $scope.invmpI_PIDate,
                        "INVMPI_Remarks": $scope.invmpI_Remarks,
                        "INVMPI_ApproxTotAmount": $scope.invmpI_ApproxTotAmount,
                        "arrayPI": $scope.arrayPI,
                        "prflag": $scope.prflag,
                        "INVMPI_Id": $scope.invmpI_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
                }
                else if ($scope.prflag === true && $scope.editS === true || $scope.transrowsedit.length > 0) {
                    angular.forEach($scope.transrowsedit, function (tr) {
                        $scope.arrayPI.push(tr);
                        //$scope.arrayPI.push({
                        //    invmpI_Id: tr.invmpI_Id, invmI_Id: tr.invmI_Id, invmI_ItemName: tr.invmI_ItemName, invmuoM_Id: tr.invmuoM_Id,
                        //    invmuoM_UOMName: tr.invmuoM_UOMName, invtpI_PRQty: tr.invtpI_PRQty, invtpI_PIQty: tr.invtpI_PIQty,
                        //    invtpI_PIUnitRate: tr.invtpI_PIUnitRate, invtpI_ApproxAmount: tr.invtpI_ApproxAmount,
                        //    invtpI_Remarks: tr.invtpI_Remarks
                        //});
                    });
                    data = {
                        "INVMPI_ReferenceNo": $scope.invmpI_ReferenceNo,
                        "INVMPI_PIDate": $scope.invmpI_PIDate,
                        "INVMPI_Remarks": $scope.invmpI_Remarks,
                        "INVMPI_ApproxTotAmount": $scope.invmpI_ApproxTotAmount,
                        "arrayPI": $scope.arrayPI,
                        "prflag": $scope.prflag,
                        "INVMPI_Id": $scope.invmpI_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
                }
                else {
                    angular.forEach($scope.transrows, function (tr) {
                        $scope.arrayPI.push({
                            invmI_Id: tr.invmI_Id.invmI_Id, invmuoM_Id: tr.invmI_Id.invmuoM_Id, invtpI_PIQty: tr.invtpI_PIQty,
                            invtpI_PIUnitRate: tr.invtpI_PIUnitRate, invtpI_ApproxAmount: tr.invtpI_ApproxAmount, invtpI_Remarks: tr.invtpI_Remarks
                        });
                    });
                    data = {
                        "INVMPI_ReferenceNo": $scope.invmpI_ReferenceNo,
                        "INVMPI_PIDate": $scope.invmpI_PIDate,
                        "INVMPI_Remarks": $scope.invmpI_Remarks,
                        "INVMPI_ApproxTotAmount": $scope.invmpI_ApproxTotAmount,
                        "arrayPI": $scope.arrayPI,
                        "prflag": $scope.prflag,
                        "INVMPI_Id": $scope.invmpI_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_PurchaseIndent/savedetails", data).then(function (promise) {

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


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        //============================================ Print Receipt
        
       

        $scope.genrateReceipt = function () {
            $scope.checkArray = [];
            debugger;
            $scope.result_new = [];
            
            angular.forEach($scope.get_purchaseindent, function (aa) {
                if (aa.checkedvalue === true) {
                    $scope.result_new.push(aa);
                }
            })


            if ($scope.result_new.length > 0) {
                $scope.printbill = true;
                var data = {
                    piArray: $scope.result_new
                };

                apiService.create("INV_PurchaseIndent/genrateReceipt", data).then(function (promise) {

                    $scope.get_PIReceipt = promise.get_purchaseindent;
                    $scope.get_printreceipt = [];

                    $scope.pi_Receipt = [];
                    var totqty = 0;
                    var totamt = 0;

                    angular.forEach($scope.get_PIReceipt, function (pick) {

                        $scope.pi_Receipt.push(pick);
                        totqty += pick.invtpI_PIQty;
                        totamt += pick.invtpI_ApproxAmount;


                    });

                    $scope.get_printreceipt.push({
                        result: $scope.pi_Receipt, tqty: totqty, tamt: totamt
                    });

                });
            }
            else {
                swal('Select a Record....');
                $scope.printbill = false;
            }
            
          
                
                  
        };

    
        $scope.toggleAll = function (pial) {
            $scope.piall = pial;
           
          $scope.result_new = [];
          
            var toggleStatus = $scope.piall;
            angular.forEach($scope.get_purchaseindent, function (pialll) {
                pialll.checkedvalue = toggleStatus;
               
            });
        };
        
    
        $scope.optionToggled = function () {
           
            $scope.all = $scope.get_purchaseindent.every(function (options) {
                return options.checkedvalue;
            });
         
        }

        $scope.printReceipt = function () {
            var innerContents = document.getElementById("printPIReceipt").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
            $scope.printbill = false;
        };


        $scope.deactiveM = function (item, SweetAlert) {
            $scope.invmpI_Id = item.invmpI_Id;
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
                        apiService.create("INV_PurchaseIndent/deactiveM", item).
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
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.invtpI_Id = item.invtpI_Id;
            var dystring = "";
            if (item.invtpI_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invtpI_ActiveFlg === false) {
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
                        apiService.create("INV_PurchaseIndent/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $scope.onformgrid(item);
                                $scope.loaddata();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };



        $scope.edit = function (editid) {

            $scope.transrowsedit = [];
            $scope.invmpI_Id = editid.invmpI_Id;
            $scope.invmpI_PIDate = new Date(editid.invmpI_PIDate);
            $scope.invmpI_ReferenceNo = editid.invmpI_ReferenceNo;
            $scope.invmpI_Remarks = editid.invmpI_Remarks;
            $scope.invmpI_ApproxTotAmount = editid.invmpI_ApproxTotAmount;
            $scope.prflag = false;
            $scope.editS = true;
            var pi_id = editid.invmpI_Id;
            var data = {
                "INVMPI_Id": pi_id

            };
            apiService.create("INV_PurchaseIndent/getpidetails", data).
                then(function (promise) {
                    $scope.edit_pimodel = promise.get_pimodel;
                    angular.forEach($scope.edit_pimodel, function (editli) {
                        $scope.transrowsedit.push({
                            invmpI_Id: editli.invmpI_Id, invtpI_Id: editli.invtpI_Id, invmI_Id: editli.invmI_Id, invmI_ItemName: editli.invmI_ItemName, invmuoM_Id: editli.invmuoM_Id,
                            invmuoM_UOMName: editli.invmuoM_UOMName, invtpI_PRQty: editli.invtpI_PRQty, invtpI_PIQty: editli.invtpI_PIQty,
                            invtpI_PIUnitRate: editli.invtpI_PIUnitRate, invtpI_ApproxAmount: editli.invtpI_ApproxAmount,
                            invtpI_Remarks: editli.invtpI_Remarks
                        });
                    });

                });
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';


        $scope.onformdetails = function (itemid) {
            $scope.get_prDetail = "";

            var item_id = itemid.invmpR_Id;
            var data = {
                "INVMPR_Id": item_id
            };
            apiService.create("INV_PurchaseRequisition/get_prdetails", data).
                then(function (promise) {
                    $scope.get_prDetail = promise.get_prDetail;
                    $scope.prnum = $scope.get_prDetail[0].invmpR_PRNo;
                    $scope.reqname = $scope.get_prDetail[0].employeename;
                });
        };

        $scope.onformgrid = function (itemid) {
            $scope.get_pimodel = "";
            var item_id = itemid.invmpI_Id;
            var data = {
                "INVMPI_Id": item_id
            };
            apiService.create("INV_PurchaseIndent/getpidetails", data).
                then(function (promise) {
                    $scope.get_pimodel = promise.get_pimodel;
                    $scope.pinum = $scope.get_pimodel[0].invmpI_PINo;
                });
            $('#myModalgrid').modal('show');
        };

    }
})();