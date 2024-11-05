(function () {
    'use strict';
    angular
        .module('app')
        .controller('FiancialAccountingVoucherController', FiancialAccountingVoucherController)

    FiancialAccountingVoucherController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function FiancialAccountingVoucherController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag == "OnlineRegular") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        $scope.crledgerdetails = [];
        $scope.drledgerdetails = [];
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.newdate = new Date();
        $scope.FAMVOU_VoucherDate = new Date();
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        $scope.gridshow = false;

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("FiancialAccountingVoucher/getalldetails", pageid).then(function (promise) {
                $scope.companyname = promise.companyname;
                $scope.fyear = promise.fyear;
                $scope.getreport = promise.getreport;
            })
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            $scope.vouchert = [];
            $scope.temps = [];
            if ($scope.myForm.$valid) {
                var FAMVOU_Id = 0;
             
                if ($scope.FAMVOU_Suffix == true) {
                    FAMVOU_Suffix = 1;
                }
                if ($scope.FAMVOU_BillwiseFlg == true) {
                    FAMVOU_BillwiseFlg = 1;
                }
                if ($scope.FAMVOU_Prefix == true) {
                    FAMVOU_Prefix = 1;
                }
                if ($scope.FAMLED_BillwiseFlg == true) {
                    FAMLED_BillwiseFlg = 1;
                }
                if ($scope.FAMVOU_Id > 0) {
                    FAMVOU_Id = $scope.FAMVOU_Id;
                }
                
                $scope.temps.push({
                    IVN_VoucherName: $scope.Vochertype
                })
                for (var i = 0; i < $scope.transrows.length; i++) {
                    if ($scope.transrows[i].receiptvoucherflag == 'CR') {

                        $scope.vouchert.push({
                            FAMLED_Id: $scope.transrows[i].famleD_Id,
                            FATVOU_Amount: $scope.transrows[i].creditAmount,
                            FATVOU_CRDRFlg: $scope.transrows[i].receiptvoucherflag

                        })
                    }
                    else {
                        $scope.vouchert.push({
                            FAMLED_Id: $scope.transrows[i].famleD_Id,
                            FATVOU_Amount: $scope.transrows[i].debitamount,
                            FATVOU_CRDRFlg: $scope.transrows[i].receiptvoucherflag

                        })
                    }
                   

                }



                var data = {
                    "FAMVOU_Id": FAMVOU_Id,
                    "FAMCOMP_Id": $scope.FAMCOMP_Id,
                   
                    "FAMVOU_VoucherType": $scope.Vochertype,
                    "FAMVOU_VoucherDate": $scope.FAMVOU_VoucherDate,
            
                    "FA_T_Voucher": $scope.vouchert,
                  //  "transnumconfigsettings": $scope.temps
                }
                apiService.create("FiancialAccountingVoucher/savedata", data).
                    then(function (promise) {

                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');

                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records save / Updated Successfully !');

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not save /Updated  !');

                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                        $state.reload();
                    })
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    
        $scope.Deletedata = function (item, SweetAlert) {
            var data = {
                "FAMVOU_Id": item.FAMVOU_Id
            }
            var dystring = "";
            if (item.famvoU_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.famvoU_ActiveFlg == false) {
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
                        apiService.create("FiancialAccountingVoucher/deactive", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not  Active / Deactive  !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (user) {
            //editledger
            var data = {
                "FAMVOU_Id": user.famvoU_Id
            }
            apiService.create("FiancialAccountingVoucher/edit", data).
                then(function (promise) {
                    $scope.FAMCOMP_Id = promise.editMvoucher[0].famcomP_Id;
                    $scope.IMFY_Id = promise.editMvoucher[0].imfY_Id;
                    $scope.FAMVOU_VoucherType = promise.editMvoucher[0].famvoU_VoucherType;
                    $scope.FAMVOU_VoucherNo = promise.editMvoucher[0].famvoU_VoucherNo;
                    $scope.FAMVOU_VoucherDate = new Date(promise.editMvoucher[0].famvoU_VoucherDate);
                    $scope.FAMVOU_Narration = promise.editMvoucher[0].famvoU_Narration;
                    $scope.FAMVOU_UserVoucherType = promise.editMvoucher[0].famvoU_UserVoucherType;
                    $scope.FAMVOU_Id = promise.editMvoucher[0].famvoU_Id;
                    if (promise.editMvoucher[0].famvoU_Suffix == true) {
                        $scope.FAMVOU_Suffix = true;
                    }
                    else {
                        $scope.FAMVOU_Suffix = false;
                    }
                    if (promise.editMvoucher[0].famvoU_Prefix == true) {
                        $scope.FAMVOU_Prefix = true;
                    }
                    else {
                        $scope.FAMVOU_Prefix = false;
                    }
                    if (promise.editMvoucher[0].famvoU_BillwiseFlg == true) {
                        $scope.FAMVOU_BillwiseFlg = true;
                    }
                    else {
                        $scope.FAMVOU_BillwiseFlg = false;
                    }
                    $scope.ledgerChange();
                    if (promise.editTvoucher != null && promise.editTvoucher.length > 0) {
                        $scope.transrows = promise.editTvoucher;
                    }
                    else {  
                        swal("Please Contact Adminstarotor !");
                    }

                   
                })

        };
        $scope.clear = function () {
            $state.reload();
        };
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
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
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

        };

        $scope.ledgerChange = function () {
            $scope.ledgerdetails = [];
            var data = {
                "FAMCOMP_Id": $scope.FAMCOMP_Id
            }
            apiService.create("FiancialAccountingVoucher/savedatatwo", data).
                then(function (promise) {

                    if (promise.ledgerdetails != null && promise.ledgerdetails.length > 0) {
                        $scope.ledgerdetails = promise.ledgerdetails;

                        for (var k = 0; k < promise.ledgerdetails.length; k++) {
                            if (promise.ledgerdetails[k].famledD_OBCRDRFlg == 'DR') {
                                $scope.drledgerdetails.push({
                                    FAMLED_Id: promise.ledgerdetails[k].famleD_Id,
                                    FAMLED_LedgerName: promise.ledgerdetails[k].famleD_LedgerName

                                })
                            }
                            else {
                                $scope.crledgerdetails.push({
                                    FAMLED_Id: promise.ledgerdetails[k].famleD_Id,
                                    FAMLED_LedgerName: promise.ledgerdetails[k].famleD_LedgerName

                                })
                            }
                        }

                    }
                    else if (promise.returnval == "admin") {
                        swal('Please Contact  Administrator  !');
                    }
                    else {
                        swal('Record Not Found  !');
                    }


                })

        };


        $scope.changedrcr = function () {
            if ($scope.Vochertype == 'Journal Voucher' || $scope.Vochertype == 'Contra Voucher' || $scope.Vochertype == 'Purchase Return Voucher' || $scope.Vochertype == 'Sales Return Voucher' ){
                angular.forEach($scope.transrows, function (objg) {

                    if (objg.receiptvoucherflag === "CR") {

                        objg.creditamountflg = false;
                        objg.debitamountflg = true;
                    }
                    else {
                        objg.debitamountflg = false;
                        objg.creditamountflg = true;
                    }

                })
            }
        }

        $scope.VoucherChange = function () {
            $scope.ledgerdetails = [];
            $scope.drledgerdetails = [];
            $scope.crledgerdetails = [];
            $scope.transrows = [];
          
            var data = {
                "FAMCOMP_Id": $scope.FAMCOMP_Id
            }
            apiService.create("FiancialAccountingVoucher/savedatatwo", data).
                then(function (promise) {

                    $scope.gridshow = true;
                    if (promise.ledgerdetails != null && promise.ledgerdetails.length > 0) {
                        
                        for (var k = 0; k < promise.ledgerdetails.length; k++) {
                            if (promise.ledgerdetails[k].famledD_OBCRDRFlg == 'DR') {
                                $scope.drledgerdetails.push({
                                    famleD_Id: promise.ledgerdetails[k].famleD_Id,
                                    famleD_LedgerName: promise.ledgerdetails[k].famleD_LedgerName

                                })
                            }
                            else {
                                $scope.crledgerdetails.push({
                                    famleD_Id: promise.ledgerdetails[k].famleD_Id,
                                    famleD_LedgerName: promise.ledgerdetails[k].famleD_LedgerName

                                })
                            }
                        }

                     
                        if ($scope.Vochertype == 'Receipt Voucher') {
                            $scope.transrows.push({
                                receiptvoucherflag: "CR",
                                debitamount: 0,
                                creditAmount: 0,
                                debitamountflg: true,
                                creditamountflg: false,
                                //ledgerdetails: promise.ledgerdetails
                                ledgerdetails: $scope.crledgerdetails
                            })

                        }
                        else if ($scope.Vochertype == 'Payment Voucher') {
                          

                            $scope.transrows.push({
                                receiptvoucherflag: "DR",
                                debitamount: 0,
                                creditAmount: 0,
                                debitamountflg: false,
                                creditamountflg: true,
                                //ledgerdetails: promise.ledgerdetails
                                ledgerdetails: $scope.drledgerdetails
                            })

                        }
                        else if ($scope.Vochertype == 'Journal Voucher') {
                            $scope.transrows.push({
                                receiptvoucherflag: "DR",
                                debitamount: 0,
                                creditAmount: 0,
                                debitamountflg: false,
                                creditamountflg: true,
                                //ledgerdetails: promise.ledgerdetails
                                ledgerdetails: $scope.crledgerdetails
                            })
                        }
                        else if ($scope.Vochertype == 'Contra Voucher') {
                            $scope.transrows.push({
                                receiptvoucherflag: "CR",
                                debitamount: 0,
                                creditAmount: 0,
                                debitamountflg: true,
                                creditamountflg: false,
                                //ledgerdetails: promise.ledgerdetails
                                ledgerdetails: $scope.crledgerdetails
                            })
                        }
                        else if ($scope.Vochertype == 'Purchase Return Voucher') {
                            $scope.transrows.push({
                                receiptvoucherflag: "CR",
                                debitamount: 0,
                                creditAmount: 0,
                                debitamountflg: true,
                                creditamountflg: false,
                                //ledgerdetails: promise.ledgerdetails
                                ledgerdetails: $scope.crledgerdetails
                            })
                        }
                        else if ($scope.Vochertype == 'Sales Return Voucher') {
                            $scope.transrows.push({
                                receiptvoucherflag: "DR",
                                debitamount: 0,
                                creditAmount: 0,
                                debitamountflg: false,
                                creditamountflg: true,
                                //ledgerdetails: promise.ledgerdetails
                                ledgerdetails: $scope.drledgerdetails
                            })
                        }


                    }
                    else if (promise.returnval == "admin") {
                        swal('Please Contact  Administrator  !');
                    }
                    else {
                        swal('Record Not Found  !');
                    }


                })

        };

        $scope.creditchange = function (){
            if ($scope.Vochertype == 'Receipt Voucher' || $scope.Vochertype == 'Purchase Return Voucher') {
                var totalCR = 0;
                var totalDR = 0;
                for (var i = 0; i < $scope.transrows.length; i++) {

                    if ($scope.transrows[i].receiptvoucherflag == "CR") {
                        totalCR = totalCR + Number($scope.transrows[i].creditAmount);
                    }
                    else {
                        totalDR = totalDR + Number($scope.transrows[i].debitamount);
                    }

                }

                if (totalCR == totalDR) {

                }
                else if (totalDR > totalCR) {

                    swal("Total debit amount is greater than total credit amount");
                }
                else {
                   $scope.transrows.push({
                    receiptvoucherflag: "DR",
                    debitamount: 0,
                    creditAmount: 0,
                    debitamountflg: false,
                    creditamountflg: true,
                    ledgerdetails: $scope.drledgerdetails
                })
                }
            }

            if ($scope.Vochertype == 'Payment Voucher' || $scope.Vochertype == 'Journal Voucher' || $scope.Vochertype == 'Contra Voucher' || $scope.Vochertype == 'Sales Return Voucher') {
                var totalCR = 0;
                var totalDR = 0;
                for (var i = 0; i < $scope.transrows.length; i++) {

                    if ($scope.transrows[i].receiptvoucherflag == "DR") {
                        totalDR = totalDR + Number($scope.transrows[i].debitamount);

                    }
                    else {
                        totalCR = totalCR + Number($scope.transrows[i].creditAmount);
                    }

                }

                if (totalCR == totalDR) {

                }
                else if (totalCR > totalDR) {

                    swal("Total debit amount is greater than total credit amount");
                }
                else {
                    $scope.transrows.push({
                        receiptvoucherflag: "CR",
                        debitamount: 0,
                        creditAmount: 0,
                        debitamountflg: true,
                        creditamountflg: false,
                        ledgerdetails: $scope.crledgerdetails
                    })
                }



            }


        }

        $scope.debitchange = function () {


            if ($scope.Vochertype == 'Receipt Voucher' || $scope.Vochertype == 'Purchase Return Voucher') {
                var totalCR = 0;
                var totalDR = 0;
                for (var i = 0; i < $scope.transrows.length; i++) {

                    if ($scope.transrows[i].receiptvoucherflag == "CR") {
                        totalCR = totalCR + Number($scope.transrows[i].creditAmount);
                    }
                    else {
                        totalDR = totalDR + Number($scope.transrows[i].debitamount);
                    }

                }

                if (totalCR == totalDR) {

                }
                else if (totalDR > totalCR) {

                    swal("Total debit amount is greater than total credit amount");
                }
                else {
                    $scope.transrows.push({
                        receiptvoucherflag: "DR",
                        debitamount: 0,
                        creditAmount: 0,
                        debitamountflg: false,
                        creditamountflg: true,
                        ledgerdetails: $scope.drledgerdetails
                    })
                }


           
            }

            if ($scope.Vochertype == 'Payment Voucher' || $scope.Vochertype == 'Journal Voucher' || $scope.Vochertype == 'Contra Voucher' || $scope.Vochertype == 'Sales Return Voucher') {
                var totalCR = 0;
                var totalDR = 0;
                for (var i = 0; i < $scope.transrows.length; i++) {

                    if ($scope.transrows[i].receiptvoucherflag == "DR") {
                        totalDR = totalDR + Number($scope.transrows[i].debitamount);
                       
                    }
                    else {
                        totalCR = totalCR + Number($scope.transrows[i].creditAmount);
                    }

                }

                if (totalCR == totalDR) {

                }
                else if (totalCR>totalDR) {

                    swal("Total debit amount is greater than total credit amount");
                }
                else {
                    $scope.transrows.push({
                        receiptvoucherflag: "CR",
                        debitamount: 0,
                        creditAmount: 0,
                        debitamountflg: true,
                        creditamountflg: false,
                        ledgerdetails: $scope.crledgerdetails
                    })
                }



            }


        }

       
    }

})();