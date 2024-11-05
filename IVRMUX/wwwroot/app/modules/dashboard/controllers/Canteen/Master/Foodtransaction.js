(function () {
    'use strict';
    angular
        .module('app')
        .controller('FoodtransactionController', FoodtransactionController)
    FoodtransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function FoodtransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {
        $scope.EditRecord = {};
        var paginationformasters;
        var copty;
        var modeofpaymentcode = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.obj = {};
        $scope.FoodTransction = [];
   
        $scope.date = new Date();
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addOBrows = function () {
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

        $scope.removeOBrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletegrnrows(data);
            }
        };
        $scope.onselectmodeofpayment = function (ivrmmoD_ModeOfPayment_Code) {
            if (ivrmmoD_ModeOfPayment_Code != null && ivrmmoD_ModeOfPayment_Code != "") {
                modeofpaymentcode = ivrmmoD_ModeOfPayment_Code;
            }
            

        }
        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("Foodtransaction/loaddata", pageid).
                then(function (promise) {
                    $scope.Foodcategeory = promise.foodcategeory;
                    $scope.modeofpayment = promise.modeofpayment;
                })
        }
        $scope.onitemchange = function (itemid, objid) {
            $scope.get_foodDetail = [];

            var data = {
                "CMMCA_Id": itemid.cmmcA_Id
            };
            apiService.create("Foodtransaction/FoodItem", data).
                then(function (promise) {
                    if (promise.get_foodDetail != null && promise.get_foodDetail.length > 0) {
                        $scope.get_foodDetail = promise.get_foodDetail;
                        $scope.transrows = [{ itrS_Id: 'trans1' }];
                    }

                });
        };
        $scope.onfooditemchange = function (itemid, objid) {
            $scope.get_foodtaxDetail = [];
            objid.cmmfI_UnitRate = "";
            objid.cmmfI_FoodItemName = "";
            var data = {
                "CMMFI_Id": itemid.cmmfI_Id
            };
            apiService.create("Foodtransaction/FoodItemtax", data).
                then(function (promise) {
                    if ($scope.get_foodDetail != null && $scope.get_foodDetail.length > 0) {
                        angular.forEach($scope.get_foodDetail, function (coe) {
                            if (coe.cmmfI_Id == itemid.cmmfI_Id) {
                                objid.cmmfI_UnitRate = coe.cmmfI_UnitRate;
                                objid.cmmfI_FoodItemName = coe.cmmfI_FoodItemName;
                            }

                        })
                    }
                    $scope.get_foodtaxDetail = promise.get_foodtaxDetail;
                    // $scope.get_foodDetail = promise.get_foodtaxDetail[];

                });
        };
        $scope.onfooditemtaxchange = function (itemid, objs) {
            //itemid.cmmfI_Id
            objs.cmmfiT_IdTotal = "";
            objs.invmT_Id = "";
            objs.invmT_Id = itemid.invmT_Id;
            if (objs.famount != null && objs.famount != "" && itemid.cmmfiT_TaxPercent != null && itemid.cmmfiT_TaxPercent != "") {

                objs.Taxpercent = itemid.cmmfiT_TaxPercent;

                objs.cmmfiT_TaxAmount = objs.famount * itemid.cmmfiT_TaxPercent / 100;
                objs.cmmfiT_IdTotal = objs.famount + (objs.famount * itemid.cmmfiT_TaxPercent / 100);
                //

            }
        };
        //onfooditemtaxchange
        $scope.FoodCalculate = function (objs) {
            objs.famount = "";
            if (objs.cmmmFi_Quntity != null && objs.cmmmFi_Quntity != "") {
                objs.famount = objs.cmmfI_UnitRate * objs.cmmmFi_Quntity;
            }
        }

        $scope.AddtoCart = function () {
            //FoodTransction
            if ($scope.transrows != null && $scope.transrows.length > 0) {

                if ($scope.transrows[0].cmmfiT_IdTotal > 0) {
                    angular.forEach($scope.transrows, function (coe) {
                        $scope.FoodTransction.push({
                            //cmmcA_Id: $scope.objs.cmmcA_Id.cmmcA_Id,
                            cmmfI_FoodItemName: coe.cmmfI_FoodItemName,
                            cmmmFi_Quntity: coe.cmmmFi_Quntity,
                            famount: coe.cmmfI_UnitRate,
                            cmmfiT_IdTotal: coe.cmmfiT_IdTotal,
                            Taxpercent: coe.Taxpercent,
                            cmmfiT_TaxAmount: coe.cmmfiT_TaxAmount,
                            cmmfiT_Id: coe.cmmfiT_Id.cmmfiT_Id,
                            invmT_Id: coe.invmT_Id,

                        })
                        $scope.transrows = [{ itrS_Id: 'trans1' }];
                        $scope.GrandTotal();
                    })
                }
                else {
                    swal('kindlly select the food Item')
                }
            }
           
        }

        $scope.GrandTotal = function () {
            if ($scope.FoodTransction != null && $scope.FoodTransction.length > 0) {
                $scope.Subtotal = 0; $scope.totalTax = 0;
                var Subtotal = 0;
                var totalTax = 0;
                angular.forEach($scope.FoodTransction, function (coe) {
                    Subtotal = Subtotal + (coe.famount * coe.cmmmFi_Quntity);
                    totalTax = totalTax + coe.cmmfiT_TaxAmount;

                })
                $scope.Subtotal = Subtotal;
                $scope.totalTax = totalTax;
            }
        }

        $scope.removeItemOnce = function (index) {
            $scope.FoodTransction.splice(index, 1);
        };

        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            var Subtotal = 0;
            var totalTax = 0;
            var grandtotal = 0
            $scope.CMTransactionTax = [];
            $scope.CMTransactionPaymentMode = [];
            $scope.CMTransactionItems = [];
           
                var CMMFIT_Id = 0;
                if ($scope.obj.CMMFIT_Id > 0) {
                    CMMFIT_Id = $scope.obj.CMMFIT_Id;
                }

               
                angular.forEach($scope.FoodTransction, function (coe) {
                    Subtotal = Subtotal + (coe.famount * coe.cmmmFi_Quntity);
                    totalTax = totalTax + coe.cmmfiT_TaxAmount;
                    if (coe.cmmfiT_TaxAmount > 0) {
                        $scope.CMTransactionItems.push({
                            CMMFI_Id: coe.cmmfiT_Id,
                            CMTRANS_Qty: coe.cmmmFi_Quntity,
                            CMTRANSI_UnitRate: coe.famount,
                            CMTRANSI_Amount: coe.famount * coe.cmmmFi_Quntity,
                            CMTRANSI_TaxApplicableFlg:true

                        })
                    }
                    else {
                        $scope.CMTransactionItems.push({
                            CMMFI_Id: coe.cmmfiT_Id,
                            CMTRANS_Qty: coe.cmmmFi_Quntity,
                            CMTRANSI_UnitRate: coe.famount,
                            CMTRANSI_Amount: coe.famount * coe.cmmmFi_Quntity,
                            CMTRANSI_TaxApplicableFlg: false

                        })
                    }

                   

                    $scope.CMTransactionTax.push({
                        INVMT_Id: coe.invmT_Id,
                        CMTRANST_TaxAmount: coe.cmmfiT_TaxAmount
                    })




                })
                grandtotal = Subtotal + totalTax;

                $scope.CMTransactionPaymentMode.push({
                    IVRMMOD_Id: modeofpaymentcode,
                        CMTRANSPM_Amount: grandtotal
                })
                var data = {
                    				
                    "CMTRANS_Id": $scope.CMTRANS_Id,
                    "CMTRANS_Amount": Subtotal,
                    "CMTRANS_TaxAmount": totalTax,
                    "CMTRANS_TotalAmount": grandtotal,
                  
                    "CMTRANS_Remarks": $scope.CMTRANS_Remarks,
                    "CMTRANS_PaidAmount": $scope.CMTRANS_PaidAmount,
                    "CMTRANS_PendingAmount": $scope.CMTRANS_PendingAmount,
                    "CMTRANS_KOTPrintedFlg": $scope.CMTRANS_KOTPrintedFlg,
                    "CMTRANS_NoofKOTPrints": $scope.CMTRANS_NoofKOTPrints,
                    "CMTRANS_SecurityCode": $scope.CMTRANS_SecurityCode,
                    "CMTRANS_VoidReasons": scope.CMTRANS_VoidReasons,
                    "CMTRANS_VoidKotFlg": $scope.CMTRANS_VoidKotFlg,
                    CMTransactionTax: $scope.CMTransactionTax,
                    CMTransactionPaymentMode: $scope.CMTransactionPaymentMode,
                    CMTransactionItems: $scope.CMTransactionItems
                    //"CMMFIT_TaxPercent": $scope.CMMFIT_TaxPercent,
                    //"CMMFI_Id": $scope.CMMFI_Id,
                    //"INVMT_Id": $scope.Inv_tax,
                    //"CMMFIT_Id": CMMFIT_Id,
                }
                apiService.create("Foodtransaction/savedata", data).
                    then(function (promise) {


                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !', "", "success");

                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !', "", "success");

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !', "", "error");

                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !', "", "error");
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !', "", "warning");
                        }


                        $state.reload();
                    })

           


        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.cmmfI_FoodItemName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.taxpercent)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

        $scope.deactive = function (tax) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            var mgs = "";
            if (tax.cmmfiT_ActiveFlg == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Category?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Foodtransaction/deactivate", tax).then(function (promise) {
                            swal(promise.returnval);
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });

        };
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.clear = function () {
            $state.reload();
        }
    }

})();