
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_VendorPaymentController', INV_VendorPaymentController);
    INV_VendorPaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_VendorPaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var date = new Date();
        $scope.invspT_PaymentDate = date;
        $scope.editS = false;
        $scope.obj = {};
        $scope.paymenttype = "M";
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


        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_VendorPayment/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_supplier = promise.get_supplier;
                    $scope.get_paymentMode = promise.get_paymentMode;
                    $scope.get_vendorpayment = promise.get_vendorpayment;
                    $scope.presentCountgrid = $scope.get_vendorpayment.length;
                });
        };

        //===================================== PI Change
        $scope.onsupplierchange = function (supid) {
            $scope.get_indentDetail = "";
            $scope.get_SuplierGRNno = [];
            var totpurchase = 0;
            var tpaidamt = 0;
            var totbalance = 0;

            $scope.supplierid = supid;
            var data = {
                "invmS_Id": $scope.supplierid
            };
            apiService.create("INV_VendorPayment/getGRNdetail", data).
                then(function (promise) {
                    $scope.get_SuplierGRN = promise.get_SuplierGRNno;
                    if ($scope.get_SuplierGRN.length > 0) {
                        angular.forEach($scope.get_SuplierGRN, function (fod) {
                            totpurchase += parseFloat(fod.INVMGRN_PurchaseValue);
                            tpaidamt += parseFloat(fod.INVMGRN_TotalPaid);
                            totbalance += parseFloat(fod.INVMGRN_TotalBalance);
                            if (fod.INVMGRN_PurchaseValue === fod.INVMGRN_TotalPaid) {
                                $scope.paystatus = "0";
                            }
                            else if (fod.INVMGRN_PurchaseValue !== fod.INVMGRN_TotalPaid && fod.INVMGRN_TotalBalance > 0) {
                                $scope.paystatus = "1";
                            }
                            else if (fod.INVMGRN_PurchaseValue > 0 && fod.INVMGRN_TotalPaid === 0 && fod.INVMGRN_TotalBalance === 0) {
                                $scope.paystatus = "2";
                            }
                            $scope.get_SuplierGRNno.push({
                                INVMGRN_GRNNo: fod.INVMGRN_GRNNo, INVMGRN_Id: fod.INVMGRN_Id, INVMGRN_PurchaseDate: fod.INVMGRN_PurchaseDate, INVMGRN_PurchaseValue: fod.INVMGRN_PurchaseValue, INVMGRN_TotalBalance: fod.INVMGRN_TotalBalance, INVSPTGRN_Amount: fod.INVMGRN_TotalPaid, INVMS_Id: fod.INVMS_Id, paystatus: $scope.paystatus,
                                INVMGRN_TotalBalance_New: fod.INVMGRN_TotalBalance, INVSPTGRN_Amount_New: fod.INVMGRN_TotalPaid
                            });
                        });
                        $scope.payamount = tpaidamt.toFixed(2);
                        $scope.totalpurhase = totpurchase.toFixed(2);
                        $scope.totalpaid = tpaidamt.toFixed(2);
                        $scope.totalbalance = totbalance.toFixed(2);
                    }
                    else {
                        swal("No record found.... !!");
                        $scope.get_SuplierGRNno = "";
                    }
                });
        };
        //==================================== View Payment Model

        $scope.ongrnpaymentmodel = function (grnid) {
            $scope.get_GRNpayment = [];
            var totalamt = 0;
            var totalpaid = 0;
            var totalbal = 0;

            var data = {
                "INVMS_Id": grnid.INVMS_Id,
                "INVMGRN_Id": grnid.INVMGRN_Id
            };
            apiService.create("INV_VendorPayment/getGRNdetail", data).
                then(function (promise) {
                    if (promise.get_GRNpayment.length > 0) {

                        $scope.get_GRNpayment = promise.get_GRNpayment;
                        $scope.grno = $scope.get_GRNpayment[0].invmgrN_GRNNo;
                        $scope.paydate = $scope.get_GRNpayment[0].invspT_PaymentDate;
                        $scope.paymode = $scope.get_GRNpayment[0].invspT_ModeOfPayment;

                        angular.forEach($scope.get_GRNpayment, function (gp) {
                            totalamt += parseFloat(gp.invmgrN_PurchaseValue);
                            totalpaid += parseFloat(gp.invsptgrN_Amount);
                            totalbal += parseFloat(gp.invmgrN_TotalBalance);
                        });
                        $scope.tamount = totalamt.toFixed(2);
                        $scope.tpaid = totalpaid.toFixed(2);
                        $scope.tbalance = totalbal.toFixed(2);
                    }
                    else {
                        swal("No record found.... !!");
                        $scope.get_GRNpayment = "";
                    }
                });
        };

        //===================================== radio Change
        $scope.radiochange = function () {
            if ($scope.get_SuplierGRNno.length > 0) {
                angular.forEach($scope.get_SuplierGRNno, function (cleard) {
                    cleard.INVSPTGRN_Amount = 0;
                    cleard.INVMGRN_TotalBalance = 0;
                    cleard.checkedvalue = false;
                });
                $scope.totalpaid = 0;
                $scope.totalbalance = 0;
                $scope.payamount = 0;
                $scope.onsupplierchange($scope.supplierid);

            }
        };
        //================================= Checkbox Selection
        $scope.onGRNcheck = function (objck) {
            angular.forEach($scope.get_SuplierGRNno, function (grnew) {
                if (objck.invmgrN_Id === grnew.invmgrN_Id) {
                    $scope.paidnew = grnew.INVSPTGRN_Amount;
                    $scope.balancenew = grnew.INVMGRN_TotalBalance;
                }
            });
        };
        //===================================== Claculate Amount
        //$scope.countAutoAmount = function (payamt) {
        //    var totpurchase = 0;
        //    var totpaid = 0;
        //    var totbalance = 0;

        //    if (payamt !== undefined && payamt !== null && payamt !== "" && payamt !== 0) {
        //        if (parseFloat($scope.totalpaid) > parseFloat(payamt)) {
        //            swal("Amount must be greater than last paid amount....!!");
        //            payamt = 0;
        //        }
        //        var totalAmont = parseFloat(payamt);
        //        var revisedamount = 0;
        //        if ($scope.get_SuplierGRNno.length > 0) {
        //            angular.forEach($scope.get_SuplierGRNno, function (cleard) {
        //                cleard.INVSPTGRN_Amount = 0;
        //                cleard.INVMGRN_TotalBalance = 0;
        //            });                  
        //            for (var i = 0; i < $scope.get_SuplierGRNno.length; i++) {
        //                if (totalAmont <= $scope.get_SuplierGRNno[i].INVMGRN_PurchaseValue) {

        //                    revisedamount = $scope.get_SuplierGRNno[i].INVMGRN_PurchaseValue - parseFloat(totalAmont);
        //                    $scope.get_SuplierGRNno[i].INVSPTGRN_Amount = totalAmont;                         
        //                    $scope.get_SuplierGRNno[i].INVMGRN_TotalBalance = revisedamount.toFixed(2);                          

        //                    if (revisedamount >= totalAmont) {
        //                        revisedamount = 0;
        //                    } else {
        //                        totalAmont = revisedamount;
        //                    }
        //                }
        //                else if (totalAmont > $scope.get_SuplierGRNno[i].INVMGRN_PurchaseValue) {                          
        //                    $scope.get_SuplierGRNno[i].INVSPTGRN_Amount = $scope.get_SuplierGRNno[i].INVMGRN_PurchaseValue.toFixed(2);
        //                    revisedamount = totalAmont - $scope.get_SuplierGRNno[i].INVMGRN_PurchaseValue;
        //                    $scope.get_SuplierGRNno[i].INVMGRN_TotalBalance = 0;                          
        //                    totalAmont = revisedamount.toFixed(2);                            
        //                }

        //                totpurchase += parseFloat($scope.get_SuplierGRNno[i].INVMGRN_PurchaseValue);
        //                totpaid += parseFloat($scope.get_SuplierGRNno[i].INVSPTGRN_Amount);
        //                totbalance += parseFloat($scope.get_SuplierGRNno[i].INVMGRN_TotalBalance);

        //                if (revisedamount === 0) {
        //                    break;
        //                }
        //            }

        //            //if (payamt > $scope.totalpurhase) {
        //            //    swal("Amount must be Less than or Equal to Total Purchase Amount....!!");
        //            //    $scope.totalpurhase = 0;
        //            //}
        //            $scope.totalpurhase = totpurchase.toFixed(2);
        //            $scope.totalpaid = totpaid.toFixed(2);
        //            $scope.totalbalance = totbalance.toFixed(2);
        //        }
        //    }
        //};


        $scope.countManualAmount = function (objpa) {
            var totpurchase = 0;
            var totpaid = 0;
            var totbalance = 0;

            if (objpa.finalamount !== undefined && objpa.finalamount !== null && objpa.finalamount !== "" && objpa.finalamount !== 0) {
                var balanceAmount = 0;
                if ($scope.get_SuplierGRNno.length > 0) {
                    $scope.totalpaidamount = 0;
                    $scope.totalpaid = parseFloat(objpa.INVSPTGRN_Amount_New);
                    balanceAmount = parseFloat(objpa.INVMGRN_TotalBalance_New);

                    angular.forEach($scope.get_SuplierGRNno, function (objm) {
                        $scope.totalpaidamount = parseFloat(objpa.finalamount) + parseFloat($scope.totalpaid);
                        if (objpa.INVMGRN_Id === objm.INVMGRN_Id) {
                            if ($scope.totalpaidamount === objm.INVMGRN_PurchaseValue) {
                                balanceAmount = objm.INVMGRN_PurchaseValue - $scope.totalpaidamount;
                                objm.INVMGRN_TotalBalance = balanceAmount.toFixed(2);
                                objm.INVSPTGRN_Amount = $scope.totalpaidamount;
                            }
                            else if ($scope.totalpaidamount < objm.INVMGRN_PurchaseValue) {
                                balanceAmount = objm.INVMGRN_PurchaseValue - $scope.totalpaidamount;
                                objm.INVMGRN_TotalBalance = balanceAmount.toFixed(2);
                                objm.INVSPTGRN_Amount = $scope.totalpaidamount;
                            }
                            else if ($scope.totalpaidamount > objm.INVMGRN_PurchaseValue) {
                                swal("Paid Amount must be Less than or Equal to Purchase Amount....!!");
                                objm.INVSPTGRN_Amount = 0;
                            }
                        }

                        totpurchase += parseFloat(objm.INVMGRN_PurchaseValue);
                        totpaid += parseFloat(objm.INVSPTGRN_Amount);
                        totbalance += parseFloat(objm.INVMGRN_TotalBalance);

                    });
                    $scope.INVMGRN_TotalBalance = balanceAmount;
                    $scope.INVMGRN_TotalBalance = parseFloat($scope.INVMGRN_TotalBalance);
                    $scope.INVMGRN_TotalBalance = $scope.INVMGRN_TotalBalance.toFixed(2);

                    $scope.totalpurhase = totpurchase.toFixed(2);
                    $scope.totalpaid = totpaid.toFixed(2);
                    $scope.totalbalance = totbalance.toFixed(2);
                }
            }
        };

        //===================================== SAVE DATA
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {};
                $scope.paymentArray = [];
                if ($scope.paymenttype === 'A') {
                    angular.forEach($scope.get_SuplierGRNno, function (pya) {
                        $scope.paymentArray.push({ INVSPTGRN_Id: pya.INVSPTGRN_Id, INVMGRN_Id: pya.INVMGRN_Id, INVMGRN_PurchaseValue: pya.INVMGRN_PurchaseValue, INVSPTGRN_Amount: pya.finalamount, INVMGRN_TotalPaid: pya.INVSPTGRN_Amount, INVMGRN_TotalBalance: pya.INVMGRN_TotalBalance, INVSPTGRN_Remarks: pya.INVSPTGRN_Remarks });
                    });
                }
                else {
                    angular.forEach($scope.get_SuplierGRNno, function (pym) {
                        if (pym.checkedvalue) {
                            $scope.paymentArray.push({ INVSPTGRN_Id: pym.INVSPTGRN_Id, INVMGRN_Id: pym.INVMGRN_Id, INVMGRN_PurchaseValue: pym.INVMGRN_PurchaseValue, INVSPTGRN_Amount: pym.finalamount, INVMGRN_TotalPaid: pym.INVSPTGRN_Amount, INVMGRN_TotalBalance: pym.INVMGRN_TotalBalance, INVSPTGRN_Remarks: pym.INVSPTGRN_Remarks });
                        }
                    });
                }
                data = {
                    "INVSPT_Id": $scope.invspT_Id,
                    "INVMS_Id": $scope.obj.invmS_Id.invmS_Id,
                    "INVSPT_PaymentDate": $scope.invspT_PaymentDate,
                    "INVSPT_ModeOfPayment": $scope.invspT_ModeOfPayment,
                    "INVSPT_BankName": $scope.invspT_BankName,
                    "INVSPT_ChequeDDNo": $scope.invspT_ChequeDDNo,
                    "INVSPT_ChequeDDDate": $scope.invspT_ChequeDDDate,
                    "INVSPT_PaymentReference": $scope.invspT_PaymentReference,
                    "INVSPT_Amount": $scope.totalpaid,
                    "INVSPT_Remarks": $scope.invspT_Remarks,
                    "paymentArray": $scope.paymentArray
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_VendorPayment/savedetails", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.invspT_Id === 0 || promise.invspT_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invspT_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invspT_Id === 0 || promise.invspT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invspT_Id > 0) {
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

        $scope.deactive = function (pay) {
            $scope.INVSPT_Id = pay.invspT_Id;
            var dystring = "";
            if (pay.invspT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (pay.invspT_ActiveFlg === false) {
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
                        apiService.create("INV_VendorPayment/deactive", pay).
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

        $scope.deactiveGRN = function (paygrn) {
            $scope.INVSPTGRN_Id = paygrn.invsptgrN_Id;
            var dystring = "";
            if (paygrn.invsptgrN_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (paygrn.invsptgrN_ActiveFlg === false) {
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
                        apiService.create("INV_VendorPayment/deactiveGRN", paygrn).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                angular.element('#myModal').modal('hide');
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.onmodelclick = function (mdel) {
            var data = {
                "INVSPT_Id": mdel.invspT_Id
            };
            apiService.create("INV_VendorPayment/getmodeldetail", data).
                then(function (promise) {
                    $scope.get_modeldetails = promise.get_modeldetails;
                });
        };

        $scope.edit = function (edt) {
            var totpurchase = 0;
            var totpaid = 0;
            var totbalance = 0;
            $scope.edit_vendorpayment = [];
            $scope.get_SuplierGRNno = [];
            $scope.obj.invmS_Id = edt;
            $scope.invspT_ModeOfPayment = edt.invspT_ModeOfPayment;
            $scope.invspT_BankName = edt.invspT_BankName;
            $scope.invmsL_SalesDate = new Date(edt.invmsL_SalesDate);
            $scope.invspT_ChequeDDDate = new Date(edt.invspT_ChequeDDDate);
            $scope.invspT_ChequeDDNo = edt.invspT_ChequeDDNo;
            $scope.invspT_PaymentReference = edt.invspT_PaymentReference;
            $scope.payamount = edt.invspT_Amount;
            $scope.invspT_Remarks = edt.invspT_Remarks;
            $scope.invspT_Id = edt.invspT_Id;
            $scope.editS = true;
            $scope.paymenttype = "M";
            var data = {
                "INVSPT_Id": edt.invspT_Id
            };
            apiService.create("INV_VendorPayment/getmodeldetail", data).
                then(function (promise) {
                    $scope.edit_vendorpayment = promise.get_modeldetails;
                    angular.forEach($scope.edit_vendorpayment, function (objedit) {
                        $scope.get_SuplierGRNno.push({
                            INVSPTGRN_Id: objedit.invsptgrN_Id, INVMGRN_Id: objedit.invmgrN_Id, INVMGRN_GRNNo: objedit.invmgrN_GRNNo, INVMGRN_PurchaseValue: objedit.invmgrN_PurchaseValue, INVMGRN_TotalPaid: objedit.invmgrN_TotalPaid, INVMGRN_TotalBalance: objedit.invmgrN_TotalBalance,
                            INVSPTGRN_Remarks: objedit.invsptgrN_Remarks
                        });
                        totpurchase += parseFloat(objedit.invmgrN_PurchaseValue);
                        totpaid += parseFloat(objedit.invmgrN_TotalPaid);
                        totbalance += parseFloat(objedit.invmgrN_TotalBalance);
                    });
                    $scope.totalpurhase = totpurchase.toFixed(2);
                    $scope.totalpaid = totpaid.toFixed(2);
                    $scope.totalbalance = totbalance.toFixed(2);
                });
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();