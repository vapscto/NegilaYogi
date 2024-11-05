
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_T_SalesReturnController', INV_T_SalesReturnController);
    INV_T_SalesReturnController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_T_SalesReturnController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        var date = new Date();
        $scope.INVMSLRET_SalesReturnDate = date;
        $scope.INVMSLRET_CreditNoteDate = date;
        $scope.searchValue = "";


        $scope.saletrans = false;
        //  $scope.transgrid = true;
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

        //======================Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        //for (var i = 0; i < transnumconfigsettings.length; i++) {
        //    if (transnumconfigsettings.length > 0) {
        //        if (transnumconfigsettings[i].imN_Flag === "INVSALESRETURN") {
        //            $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
        //        }
        //    }
        //}
        if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings[i].imN_Flag === "INVSALESRETURN") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }

        }
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addsalesrows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.transrows.length > 1) {
                    $scope.cnt = $scope.transrows.length;
                }
                $scope.cnt = $scope.cnt + 1;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.removesalesrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);
            if (data.amstB_Id > 0) {
                $scope.Deletesalesrows(data);
            }
        };
        //==============================Page Load========================
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_T_Sales_Return/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_salesno = promise.get_salesno;
                    $scope.get_saleReturn = promise.get_saleReturn;
                    $scope.presentCountgrid = promise.get_saleReturn.length;

                });
        };


        //==================get item
        $scope.getitem1 = function (ite) {
            $scope.get_item = [];
            $scope.get_item = [];
            $scope.get_itemTax1 = [];
            $scope.salesid = ite.invmsL_Id;
            var data = {
                "INVMSL_Id": ite.invmsL_Id
            };
            apiService.create("INV_T_Sales_Return/getitem", data).
                then(function (promise) {
                    $scope.get_item = promise.get_item;
                    $scope.INVMS_StoreName = promise.get_Store[0].invmS_StoreName;
                    $scope.INVMST_Id = promise.get_Store[0].invmsT_Id;
                    $scope.transrows = [{ itrS_Id: 'trans1' }];
                    if ($scope.transrows.length === 1) {
                        $scope.cnt = 1;
                    }

                });
        };


        //===========Item Change Transcation Grid
        $scope.onitemchange = function (itemid, objid) {
            $scope.get_itemTax1 = [];
            var data = {};
            var qqty = 0;
            var dsqty = 0;
            var ttqqty = 0;
            var disqqty = 0;
            var stucnt = 0;
            var avstock = 0;

            data = {
                "INVMI_Id": itemid.INVMI_Id,
                // "INVSTO_SalesRate": itemid.INVSTO_SalesRate,
                "INVMST_Id": $scope.INVMST_Id,
                "INVMSL_Id": $scope.salesid
            };

            apiService.create("INV_T_Sales_Return/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    $scope.get_itemTax1 = promise.get_itemTax;
                    $scope.availablestock = promise.availablestock;
                    angular.forEach($scope.availablestock, function (ast) {
                        avstock += ast.AvaiableStock;
                    });
                    $scope.availableitems = avstock;

                    angular.forEach($scope.transrows, function (obj) {

                        if (obj.itrS_Id === objid.itrS_Id) {
                            obj.get_itemTax = $scope.get_itemTax1;
                            $scope.get_itemTax = $scope.get_itemTax1;
                            obj.invmuoM_UOM = $scope.get_itemDetail[0].INVMUOM_UOMName;
                            obj.invmuoM_Id = $scope.get_itemDetail[0].INVMUOM_Id;
                            obj.invtsL_BatchNo = $scope.get_itemDetail[0].INVTSL_BatchNo;
                            obj.invtsL_SalesPrice = $scope.get_itemDetail[0].INVTSL_SalesPrice;
                            obj.invtsL_SalesQty = $scope.get_itemDetail[0].INVTSL_SalesQty;
                            obj.INVTSLRET_SalesReturnQty = 0;
                            obj.INVTSLRET_SalesReturnAmount = 0.00;
                            obj.invtsL_Amount = 0.00;
                            obj.invtsL_TaxAmt = 0.00;
                            obj.invmP_Id = $scope.get_itemDetail[0].INVMP_Id;
                            //obj.INVMP_Id = 0;
                        }


                    });
                    if ($scope.invmsL_TotalAmount === 0 || $scope.invmsL_TotalAmount === "" || $scope.invmsL_TotalAmount === undefined || $scope.invmsL_TotalAmount === "NaN") {
                        $scope.invmsL_TotalAmount = 0.00;
                    }
                    if ($scope.invmsL_TotDiscount === 0 || $scope.invmsL_TotDiscount === "" || $scope.invmsL_TotDiscount === undefined || $scope.invmsL_TotDiscount === "NaN") {
                        $scope.invmsL_TotDiscount = 0.00;
                    }
                    if ($scope.invmsL_TotTaxAmt === 0 || $scope.invmsL_TotTaxAmt === "" || $scope.invmsL_TotTaxAmt === undefined || $scope.invmsL_TotTaxAmt === "NaN") {
                        $scope.invmsL_TotTaxAmt = 0.00;
                    }

                    //$scope.countAmt(objid);
                    //$scope.countDiscount(objid);
                });
        };

        //==========================Tax View and Add To Cart 
        $('#myModal').modal('hide');
        $scope.viewtxfn = function (qwe) {

            //$scope.purrate = parseFloat(qwe.invmI_Id.INVSTO_SalesRate);
            $scope.qty = parseFloat(qwe.invtsL_SalesQty);
            $scope.totdiscout = parseFloat(qwe.invtsL_DiscountAmt);
            $('#myModal').modal('show');
            $scope.get_itemTax = qwe.get_itemTax;
            var cnt = 0;
            angular.forEach($scope.get_itemTax, function (wer) {
                wer.itrS_Id = qwe.itrS_Id;
                $scope.totpurrate = $scope.purrate * $scope.qty;
                if (qwe.invtsL_DiscountAmt === undefined || qwe.invtsL_DiscountAmt === "0" || qwe.invtsL_DiscountAmt === "") {
                    $scope.finalpurrate = $scope.totpurrate;
                }
                else {
                    $scope.finalpurrate = $scope.totpurrate - $scope.totdiscout;
                }
                var taxamt = ($scope.finalpurrate * wer.invmiT_TaxValue) / 100;
                wer.invtslT_TaxAmt = taxamt;//.toFixed(2);
                wer.invtslT_TaxAmt = parseFloat(wer.invtslT_TaxAmt);
                wer.invtslT_TaxAmt = wer.invtslT_TaxAmt.toFixed(2);
                cnt = cnt + wer.invtslT_TaxAmt;
            });
        };

        //=============================Tax Amount       
        $scope.addtocart = function (itemtax) {
            $scope.arraySaletax = [];
            $scope.arraytax = [];
            var ccnt = 0;
            angular.forEach(itemtax, function (tax) {
                $scope.tsid = tax.itrS_Id;
                $scope.arraySaletax.push({ invmT_Id: tax.INVMIT_Id, invtslT_TaxPer: tax.INVMIT_TaxValue, invtslT_TaxAmt: tax.invtslT_TaxAmt });
                ccnt = ccnt + tax.invtslT_TaxAmt;
            });

            angular.forEach($scope.transrows, function (obj) {
                var tx = 0.00;
                angular.forEach(obj.get_itemTax, function (obj1) {
                    if (obj.itrS_Id === obj1.itrS_Id) {
                        tx += parseFloat(obj1.invtslT_TaxAmt);
                    }
                });
                obj.invtsL_TaxAmt = tx;
            });
            var t = 0;
            angular.forEach($scope.transrows, function (obj) {
                angular.forEach(obj.get_itemTax, function (obj1) {
                    t += parseFloat(obj1.invtslT_TaxAmt);
                });
            });
            $scope.invmsL_TotTaxAmt = t;
            $scope.invmsL_TotTaxAmt = parseFloat($scope.invmsL_TotTaxAmt);
            $scope.invmsL_TotTaxAmt = $scope.invmsL_TotTaxAmt.toFixed(2);
            angular.forEach($scope.transrows, function (stax) {
                if (stax.itrS_Id === $scope.tsid) {
                    $scope.arraytax.push({ invmT_Id: stax.INVMIT_Id, invtslT_TaxPer: stax.INVTSLT_TaxPer, invtslT_TaxAmt: stax.invtslT_TaxAmt });
                    stax.get_itemTax = itemtax;
                }
            });
            $scope.taxchange($scope.invmsL_TotTaxAmt);
        };

        ////===================Transcation Grid Count Tax,Discount,Amount   

        $scope.countAmt = function (grnobj, items) {
            if (grnobj.invtsL_SalesQty == 0) {
                swal('Sales Quantity is Zero..');
                angular.forEach($scope.transrows, function (obj) {

                    if (obj.itrS_Id === objid.itrS_Id) {
                        obj.get_itemTax = $scope.get_itemTax1;
                        $scope.get_itemTax = $scope.get_itemTax1;
                        obj.invmuoM_UOM = $scope.get_itemDetail[0].INVMUOM_UOMName;
                        obj.invmuoM_Id = $scope.get_itemDetail[0].INVMUOM_Id;
                        obj.invtsL_BatchNo = $scope.get_itemDetail[0].INVTSL_BatchNo;
                        obj.invtsL_SalesPrice = $scope.get_itemDetail[0].INVTSL_SalesPrice;
                        obj.invtsL_SalesQty = $scope.get_itemDetail[0].INVTSL_SalesQty;
                        obj.INVTSLRET_SalesReturnQty = 0;
                        obj.INVTSLRET_SalesReturnAmount = 0.00;
                        obj.invtsL_Amount = 0.00;
                        obj.invtsL_TaxAmt = 0.00;
                    }


                });

            }
            else if (grnobj.INVTSLRET_SalesReturnQty > grnobj.invtsL_SalesQty) {
                swal('Sales Quantity Less Than Return Quantity..');
                angular.forEach($scope.transrows, function (obj) {

                    if (obj.itrS_Id === objid.itrS_Id) {
                        obj.get_itemTax = $scope.get_itemTax1;
                        $scope.get_itemTax = $scope.get_itemTax1;
                        obj.invmuoM_UOM = $scope.get_itemDetail[0].INVMUOM_UOMName;
                        obj.invmuoM_Id = $scope.get_itemDetail[0].INVMUOM_Id;
                        obj.invtsL_BatchNo = $scope.get_itemDetail[0].INVTSL_BatchNo;
                        obj.invtsL_SalesPrice = $scope.get_itemDetail[0].INVTSL_SalesPrice;
                        obj.invtsL_SalesQty = $scope.get_itemDetail[0].INVTSL_SalesQty;
                        obj.INVTSLRET_SalesReturnQty = 0;
                        obj.INVTSLRET_SalesReturnAmount = 0.00;
                        obj.invtsL_Amount = 0.00;
                        obj.invtsL_TaxAmt = 0.00;
                    }


                });

            }
            else {
                var a = 0;
                var ab = 0;
                var ds = 0;
                /// parseFloat(grnobj.invtsL_SalesQty)
                
                var INVTSLRET_SalesReturnAmount = (parseFloat(grnobj.invtsL_SalesPrice) / parseFloat(grnobj.invtsL_SalesQty) * parseFloat(grnobj.INVTSLRET_SalesReturnQty));
                
                grnobj.INVTSLRET_SalesReturnAmount = INVTSLRET_SalesReturnAmount;
                $scope.purrate = parseFloat(grnobj.INVTSLRET_SalesReturnAmount);
                $scope.qty = parseFloat(grnobj.INVTSLRET_SalesReturnQty);
                $scope.totdiscout = parseFloat(grnobj.invtgrN_DiscountAmt);
                ab = $scope.purrate * $scope.qty;
                grnobj.invtsL_Amount = parseFloat(ab);
               
              //  grnobj.invtsL_Amount = grnobj.INVTSLRET_SalesReturnAmount;
                grnobj.invtsL_Amount = grnobj.invtsL_Amount.toFixed(2);
                angular.forEach($scope.transrows, function (obj) {
                    ds += parseFloat(obj.invtsL_Amount);
                    a = ds.toFixed(2);
                });
                var totamt = a;
                $scope.invmsL_TotalAmount = totamt;
                $scope.invmsL_TotalAmount = parseFloat($scope.invmsL_TotalAmount);
                $scope.invmsL_TotalAmount = $scope.invmsL_TotalAmount.toFixed(2);
            }

        };


        $scope.$watch('invmsL_TotalAmount', function (totalamoutchange) {
            $scope.invmsL_TotalAmount = parseFloat($scope.invmsL_TotalAmount);
            $scope.invmsL_TotalAmount = $scope.invmsL_TotalAmount.toFixed(2);
            $scope.invmsL_SalesValue = $scope.invmsL_TotalAmount;
            $scope.invmsL_SalesValue = parseFloat($scope.invmsL_SalesValue);
            $scope.invmsL_SalesValue = $scope.invmsL_SalesValue.toFixed(2);

            if ($scope.invmsL_TotTaxAmt != null && $scope.invmsL_TotTaxAmt != '' && $scope.invmsL_TotTaxAmt != undefined) {
                var tax = $scope.invmsL_TotTaxAmt;

                $scope.invmsL_SalesValue = parseFloat($scope.invmsL_SalesValue) + parseFloat(tax);
                $scope.invmsL_SalesValue = $scope.invmsL_SalesValue.toFixed(2);
            }



        });

        $scope.$watch('invmsL_TotDiscount', function (totalDiscountchange) {
            var tamt = parseFloat($scope.invmsL_TotalAmount);
            var tdiscout = parseFloat($scope.invmsL_TotDiscount);
            var tottx = parseFloat($scope.invmsL_TotTaxAmt);
            var purvalur = tamt - tdiscout + tottx;
            $scope.invmsL_SalesValue = purvalur;//.toFixed(2);    
            $scope.invmsL_SalesValue = parseFloat($scope.invmsL_SalesValue);
            $scope.invmsL_SalesValue = $scope.invmsL_SalesValue.toFixed(2);
        });

        $scope.taxchange = function (sobj) {
            var tamt = parseFloat($scope.invmsL_TotalAmount);
            var tdiscout = parseFloat($scope.invmsL_TotDiscount);

            var ttax = parseFloat($scope.invmsL_TotTaxAmt);
            if ($scope.invmsL_TotTaxAmt === undefined || $scope.invmsL_TotTaxAmt === "") {
                ttax = 0;
            }
            var purvalur = tamt - tdiscout + ttax;
            $scope.invmsL_SalesValue = purvalur;//.toFixed(2);             
            $scope.invmsL_SalesValue = parseFloat($scope.invmsL_SalesValue);
            $scope.invmsL_SalesValue = $scope.invmsL_SalesValue.toFixed(2);
        };

        $scope.$watch('invmsL_TotTaxAmt', function (totalTaxchange) {
            var tamt = parseFloat($scope.invmsL_TotalAmount);
            var tottx = parseFloat($scope.invmsL_TotTaxAmt);
            var tdiscout = parseFloat($scope.invmsL_TotDiscount);
            var purvalur = tamt + tottx - tdiscout;
            $scope.invmsL_SalesValue = purvalur;//.toFixed(2);    
            $scope.invmsL_SalesValue = parseFloat($scope.invmsL_SalesValue);
            $scope.invmsL_SalesValue = $scope.invmsL_SalesValue.toFixed(2);
        });



        //=======================Save Data
        $scope.objs = {};
        $scope.savedata = function (objs) {

            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                $scope.arraySale = [];

                if ($scope.arraySaletax !== undefined) {
                    angular.forEach($scope.transrows, function (sale) {
                        $scope.arraySale.push({
                            invmI_Id: sale.invmI_Id.INVMI_Id,
                            invmuoM_Id: sale.invmuoM_Id,
                            INVTSLRET_BatchNo: sale.invtsL_BatchNo,
                            INVTSLRET_SalesReturnAmount: sale.invmI_Id.INVTSLRET_SalesReturnAmount,
                            INVTSLRET_SalesReturnQty: sale.INVTSLRET_SalesReturnQty,
                            INVTSLRET_SalesReturnAmount: sale.INVTSLRET_SalesReturnAmount,
                            INVTSLRET_SalesReturnNaration: sale.invtsL_Naration,
                            saleItemTax: sale.get_itemTax, invtsL_TaxAmt: sale.invtsL_TaxAmt,
                            invmP_Id: sale.invmP_Id
                        });
                    });
                }
                else {
                    $scope.arraySaletax = [];
                    angular.forEach($scope.transrows, function (sale) {
                        $scope.arraySale.push({
                            invmI_Id: sale.invmI_Id.INVMI_Id, invmuoM_Id: sale.invmuoM_Id,
                            INVTSLRET_BatchNo: sale.invtsL_BatchNo, INVTSLRET_SalesReturnAmount: sale.invmI_Id.INVTSLRET_SalesReturnAmount,
                            INVTSLRET_SalesReturnQty: sale.INVTSLRET_SalesReturnQty, INVTSLRET_SalesReturnAmount: sale.INVTSLRET_SalesReturnAmount,
                            INVTSLRET_SalesReturnNaration: sale.invtsL_Naration, /*saleItemTax: sale.get_itemTax,*/
                            invtsL_TaxAmt: sale.invtsL_TaxAmt, invmP_Id: sale.invmP_Id
                        });
                    });
                }



                data = {
                    "INVMST_Id": $scope.INVMST_Id,
                    "INVMSL_Id": $scope.salesid,
                    "INVMSLRET_SalesReturnDate": new Date($scope.INVMSLRET_SalesReturnDate).toDateString(),
                    "INVMSLRET_TotalReturnAmount": $scope.invmsL_SalesValue,
                    "INVMSLRET_ReturnRemarks": $scope.invmsL_Remarks,
                    "INVMSLRET_CreditNoteNo": $scope.invmslreT_CreditNoteNo,
                    "INVMSLRET_CreditNoteDate": new Date($scope.INVMSLRET_CreditNoteDate).toDateString(),
                    "INVMSLRET_EWayRefNo": $scope.invmslreT_EWayRefNo,
                    "SaleItem": $scope.arraySale,
                   "trans_id": $scope.invmslreT_SalesReturnNo,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                };




                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_T_Sales_Return/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invmsL_Id === 0 || promise.invmsL_Id < 0) {
                            if (promise.returnduplicatestatus === 'Updated') {
                                swal('Record saved successfully / Stock Updated');
                            }
                            if (promise.returnduplicatestatus === 'notUpdated') {
                                swal('Record saved successfully / Failed to Update Stock');
                            }
                        }
                        else if (promise.invmsL_Id > 0) {
                            swal('Record saved/ updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmsL_Id === 0 || promise.invmsL_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmsL_Id > 0) {
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

        //==============================GRID Activate And Deactivate
        $scope.deactive = function (item, SweetAlert) {

            var dystring = "Delete";
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
                        apiService.create("INV_T_Sales_Return/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    if (promise.returnduplicatestatus === 'Updated') {
                                        swal("Record " + dystring + "d Successfully / Stock Updated");
                                    }
                                    else if (promise.returnduplicatestatus === 'notUpdated') {
                                        swal("Record " + dystring + "d Successfully / Failed to Update Stock");
                                    }
                                    else {
                                        swal("Record " + dystring + "d Successfully!!!");
                                    }
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

        $scope.view = function (itemid) {


            var data = {
                "INVMSLRET_Id": itemid.INVMSLRET_Id
              
            };

            apiService.create("INV_T_Sales_Return/viewitem", data).
                then(function (promise) {
                    if (promise.get_salereturnitemview != null && promise.get_salereturnitemview.length > 0) {
                        $scope.get_salereturnitemview = promise.get_salereturnitemview;
                        $('#myModalItemview').modal('show');
                    }
                    else
                    {
                        swal('No Recored Found...');
                    }
                  
                    $scope.get_itemTax1 = promise.get_itemTax;
                    $scope.availablestock = promise.availablestock;

                });
        };



        $scope.deactivetax = function (item, SweetAlert) {
            $scope.INVTSLT_Id = item.invtslT_Id;
            var dystring = "";
            if (item.invtslT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invtslT_ActiveFlg === false) {
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
                        apiService.create("INV_T_Sales/deactivetax", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                angular.element('#myModalTax').modal('hide');
                                //  $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };


        $scope.searchValueC = "";
        $scope.searchValueCS = "";
    }
})();