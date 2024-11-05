
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterClgFeeConfigController', feeMController)

    feeMController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function feeMController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.debitcrdchr = false;
        $scope.creditcrdchr = false;

        $scope.showcarddetails = function (checkedval) {
            if (checkedval == true) {
                $scope.debitcrdchr = true;
                $scope.creditcrdchr = true;
            }
            if (checkedval == false) {
                $scope.debitcrdchr = false;
                $scope.creditcrdchr = false;
            }
        }


        $scope.objmaster = {};

        $scope.savedata = function () {
            
            if ($scope.FMC_GroupOrTermFlg === "T") {
                $scope.FMC_GroupOrTermFlg = "T";
            }
            else {
                $scope.FMC_GroupOrTermFlg = "G";
            }



            if ($scope.FMC_RPaperSizeFlag === "1") {
                $scope.FMC_RPaperSizeFlag = 1;
            }
            else if ($scope.FMC_RPaperSizeFlag === "2") {
                $scope.FMC_RPaperSizeFlag = 2;

            }
            else if ($scope.FMC_RPaperSizeFlag === "3") {
                $scope.FMC_RPaperSizeFlag = 3;
            }
            else {
                $scope.FMC_RPaperSizeFlag = 4;
            }

            if ($scope.FMC_StudentwiseJVFlag === "S") {
                $scope.FMC_StudentwiseJVFlag = "S";
            }
            else if ($scope.FMC_StudentwiseJVFlag = "H") {
                $scope.FMC_StudentwiseJVFlag = "H";
            }

            if ($scope.cardchargesflag == false) {
                $scope.debitcardcharges = "0";
                $scope.creditcardcharges = "0";
            }
            if ($scope.FMC_No_Receipt === "" || $scope.FMC_No_Receipt == 0) {
                $scope.FMC_No_Receipt = 1;
            }

            var data = {
                "FMC_Id": $scope.fmC_Id,
                // "MI_Id": 2,
                "FMC_GroupOrTermFlg": $scope.FMC_GroupOrTermFlg,
                "FMC_Areawise_FeeFlg": $scope.FMC_Areawise_FeeFlg,
                "FMC_TransportFeeAreaFlag": $scope.FMC_TransportFeeAreaFlag,
                "FMC_TransportFeeZoneFlag": $scope.FMC_TransportFeeZoneFlag,
                "FMC_DOACheckFlag": $scope.FMC_DOACheckFlag,
                //   "FMC_Default_Currency": $scope.FMC_Default_Currency,
                "FMC_Default_Currency": 1,
                "FMC_ArrearColumn": $scope.FMC_ArrearColumn,
                "FMC_Fine_Column": $scope.FMC_Fine_Column,
                "FMC_ArrearLedgerFlag": $scope.FMC_ArrearLedgerFlag,
                "FMC_Fine_LedgerFlag": $scope.FMC_Fine_LedgerFlag,
                "FMC_ArrearAfterFlag": $scope.FMC_ArrearAfterFlag,
                "FMC_RebateTypeFlag": $scope.FMC_RebateTypeFlag,
                "FMC_Receipt_Signatory": $scope.FMC_Receipt_Signatory,
                "FMC_No_Receipt": $scope.FMC_No_Receipt,
                "FMC_Receipt_SignatoryImage": "",
                "FMC_ChallanOptionFlag": $scope.FMC_ChallanOptionFlag,
                "FMC_AutoReceiptFeeGroupFlag": $scope.FMC_AutoReceiptFeeGroupFlag,
                "FMC_GroupRemarksFlag": $scope.FMC_GroupRemarksFlag,
                "FMC_RInstallmentsFlag": $scope.FMC_RInstallmentsFlag,
                "FMC_RInstallmentsMergeFlag": $scope.FMC_RInstallmentsMergeFlag,
                "FMC_RFineFlag": $scope.FMC_RFineFlag,
                "FMC_RConcessionFlag": $scope.FMC_RConcessionFlag,
                "FMC_RWaivedFlag": $scope.FMC_RWaivedFlag,
                "FMC_RBalanceFlag": $scope.FMC_RBalanceFlag,
                "FMC_RAmountFlag": $scope.FMC_RAmountFlag,
                "FMC_RBankFlag": $scope.FMC_RBankFlag,
                "FMC_RDueDateFlag": $scope.FMC_RDueDateFlag,
                "FMC_RAddressFlag": $scope.FMC_RAddressFlag,
                "FMC_RPaperSizeFlag": $scope.FMC_RPaperSizeFlag,
                "FMC_RFeeGroupFeeHeadFlag": $scope.FMC_RFeeGroupFeeHeadFlag,
                "FMC_RSplFeeHeadFlag": $scope.FMC_RSplFeeHeadFlag,
                "FMC_RHeaderTitleFlag": $scope.FMC_RHeaderTitleFlag,
                "FMC_RClassFlag": $scope.FMC_RClassFlag,
                //    "RClassFlag ": $scope.RClassFlag,
                //  "test ": $scope.test,
                "FMC_RSectionFlag": $scope.FMC_RSectionFlag,
                "FMC_RUserNameFlag": $scope.FMC_RUserNameFlag,
                "FMC_RFatherNameFlag": $scope.FMC_RFatherNameFlag,
                "FMC_MotherNameFlag": $scope.FMC_MotherNameFlag,
                "FMC_RFeeHeaderFlag": $scope.FMC_RFeeHeaderFlag,
                "FMC_RPaymentDetailsFlag": $scope.FMC_RPaymentDetailsFlag,
                "FMC_RAmountReceivedFlag": $scope.FMC_RAmountReceivedFlag,
                "FMC_RRemarksFlag": $scope.FMC_RRemarksFlag,
                "FMC_RCurrentDateFlag": $scope.FMC_RCurrentDateFlag,
                "FMC_StudentwiseJVFlag": $scope.FMC_StudentwiseJVFlag,
                "FMC_Receipt_Format": $scope.FMC_Receipt_Format,
                "cardchargesflag": $scope.cardchargesflag,
                "debitcardcharges": $scope.debitcardcharges,
                "creditcardcharges": $scope.creditcardcharges,

                "FMC_EableStaffTrans": $scope.FMC_EableStaffTrans,
                "FMC_EableOtherStudentTrans": $scope.FMC_EableOtherStudentTrans,

                "fineenabledisable": $scope.FMC_FineEnableDisable,

                //"FMC_BtachwiseFeeGlg": $scope.FMC_BtachwiseFeeGlg,
                "FMC_MakerCheckerReqdFlg": $scope.FMC_MakerCheckerReqdFlg,

                "FMC_RoomwiseHostelFeeFlg": $scope.FMC_RoomwiseHostelFeeFlg,
                "FMC_CommonHostelFeeFlg": $scope.FMC_CommonHostelFeeFlg,
                "FMC_CommonTransportLocationFeeFlg": $scope.FMC_CommonTransportLocationFeeFlg,
                "FMC_TransportFeeLocationFlag": $scope.FMC_TransportFeeLocationFlag,

                "FMC_FineMapping": $scope.FMC_FineMapping,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("MasterClgFeeConfig/savedetail", data).
            then(function (promise) {
                //swal('Record Saved/Updated Successfully', 'success');
                //$scope.loaddata();

                
                if (promise.returnduplicatestatus === "Save") {
                    swal('Record Saved Successfully');
                    $scope.loaddata();

                }
                else if (promise.returnduplicatestatus === "NotSave") {
                    swal('Record Not Saved');
                    $scope.loaddata();

                }
                else if (promise.returnduplicatestatus === "Update") {
                    swal('Record Updated Successfully');
                    $scope.loaddata();

                }
                else if (promise.returnduplicatestatus === "NotUpdate") {
                    swal('Record Not Updated');
                    $scope.loaddata();

                }
            })
        }

        //$scope.clearDis = false;
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10

            var pageid = 2;
            apiService.getURI("MasterClgFeeConfig/getalldetails", pageid).
        then(function (promise) {
            
            $scope.fmC_Id = promise.masterdata[0].fmC_Id;
            $scope.students = promise.feeconfiglist;

            if (rolenme != "Admin") {
                if (promise.masterdata[0].fmC_Areawise_FeeFlg === 1) {
                    $scope.FMC_Areawise_FeeFlg = 1;
                }
                else {
                    $scope.FMC_Areawise_FeeFlg = 0;
                }
                if (promise.masterdata[0].fmC_ArrearAfterFlag === 1) {
                    $scope.FMC_ArrearAfterFlag = 1;
                }
                else {
                    $scope.FMC_ArrearAfterFlag = 0;
                }
                if (promise.masterdata[0].fmC_ArrearColumn === 1) {
                    $scope.FMC_ArrearColumn = 1;
                }
                else {
                    $scope.FMC_ArrearColumn = 0;
                }
                if (promise.masterdata[0].fmC_ArrearLedgerFlag === 1) {
                    $scope.FMC_ArrearLedgerFlag = 1;
                }
                else {
                    $scope.FMC_ArrearLedgerFlag = 0;
                }
                if (promise.masterdata[0].fmC_AutoReceiptFeeGroupFlag === 1) {
                    $scope.FMC_AutoReceiptFeeGroupFlag = 1;
                }
                else {
                    $scope.FMC_AutoReceiptFeeGroupFlag = 0;
                }
                if (promise.masterdata[0].fmC_ChallanOptionFlag === 1) {
                    $scope.FMC_ChallanOptionFlag = 1;
                }
                else {
                    $scope.FMC_ChallanOptionFlag = 0;
                }
                if (promise.masterdata[0].fmC_DOACheckFlag === 1) {
                    $scope.FMC_DOACheckFlag = 1;
                }
                else {
                    $scope.FMC_DOACheckFlag = 0;
                }
                $scope.FMC_Default_Currency = promise.masterdata[0].fmC_Default_Currency;
                if (promise.masterdata[0].fmC_Fine_Column === 1) {
                    $scope.FMC_Fine_Column = 1;
                }
                else {
                    $scope.FMC_Fine_Column = 0;
                }
                if (promise.masterdata[0].fmC_Fine_LedgerFlag === 1) {
                    $scope.FMC_Fine_LedgerFlag = 1;
                }
                else {
                    $scope.FMC_Fine_LedgerFlag = 0;
                }
                if (promise.masterdata[0].fmC_GroupOrTermFlg === "T") {
                    $scope.FMC_GroupOrTermFlg = "T";
                }
                else {
                    $scope.FMC_GroupOrTermFlg = "G";
                }
                if (promise.masterdata[0].fmC_GroupRemarksFlag === 1) {
                    $scope.FMC_GroupRemarksFlag = 1;
                }
                else {
                    $scope.FMC_GroupRemarksFlag = 0;
                }
                if (promise.masterdata[0].fmC_MotherNameFlag === 1) {
                    $scope.FMC_MotherNameFlag = 1;
                }
                else {
                    $scope.FMC_MotherNameFlag = 0;
                }
                if (promise.masterdata[0].fmC_RAddressFlag === 1) {
                    $scope.FMC_RAddressFlag = 1;
                }
                else {
                    $scope.FMC_RAddressFlag = 0;
                }
                if (promise.masterdata[0].fmC_RAmountFlag === 1) {
                    $scope.FMC_RAmountFlag = 1;
                }
                else {
                    $scope.FMC_RAmountFlag = 0;
                }
                if (promise.masterdata[0].fmC_RAmountReceivedFlag === 1) {
                    $scope.FMC_RAmountReceivedFlag = 1;
                }
                else {
                    $scope.FMC_RAmountReceivedFlag = 0;
                }
                if (promise.masterdata[0].fmC_RBalanceFlag === 1) {
                    $scope.FMC_RBalanceFlag = 1;
                }
                else {
                    $scope.FMC_RBalanceFlag = 0;
                }
                if (promise.masterdata[0].fmC_RBankFlag === 1) {
                    $scope.FMC_RBankFlag = 1;
                }
                else {
                    $scope.FMC_RBankFlag = 0;
                }
                if (promise.masterdata[0].fmC_RClassFlag === 1) {
                    $scope.FMC_RClassFlag = 1;
                }
                else {
                    $scope.FMC_RClassFlag = 0;
                }
                if (promise.masterdata[0].fmC_RConcessionFlag === 1) {
                    $scope.FMC_RConcessionFlag = 1;
                }
                else {
                    $scope.FMC_RConcessionFlag = 0;
                }
                if (promise.masterdata[0].fmC_RCurrentDateFlag === 1) {
                    $scope.FMC_RCurrentDateFlag = 1;
                }
                else {
                    $scope.FMC_RCurrentDateFlag = 0;
                }
                if (promise.masterdata[0].fmC_RDueDateFlag === 1) {
                    $scope.FMC_RDueDateFlag = 1;
                }
                else {
                    $scope.FMC_RDueDateFlag = 0;
                }
                if (promise.masterdata[0].fmC_RFatherNameFlag === 1) {
                    $scope.FMC_RFatherNameFlag = 1;
                }
                else {
                    $scope.FMC_RFatherNameFlag = 0;
                }
                if (promise.masterdata[0].fmC_RFeeGroupFeeHeadFlag === 1) {
                    $scope.FMC_RFeeGroupFeeHeadFlag = 1;
                }
                else {
                    $scope.FMC_RFeeGroupFeeHeadFlag = 0;
                }
                if (promise.masterdata[0].fmC_RFeeHeaderFlag === 1) {
                    $scope.FMC_RFeeHeaderFlag = 1;
                }
                else {
                    $scope.FMC_RFeeHeaderFlag = 0;
                }
                if (promise.masterdata[0].fmC_RFineFlag === 1) {
                    $scope.FMC_RFineFlag = 1;
                }
                else {
                    $scope.FMC_RFineFlag = 0;
                }
                if (promise.masterdata[0].fmC_RHeaderTitleFlag === 1) {
                    $scope.FMC_RHeaderTitleFlag = 1;
                }
                else {
                    $scope.FMC_RHeaderTitleFlag = 0;
                }
                if (promise.masterdata[0].fmC_RInstallmentsFlag === 1) {
                    $scope.FMC_RInstallmentsFlag = 1;
                }
                else {
                    $scope.FMC_RInstallmentsFlag = 0;
                }
                if (promise.masterdata[0].fmC_RInstallmentsMergeFlag === 1) {
                    $scope.FMC_RInstallmentsMergeFlag = 1;
                }
                else {
                    $scope.FMC_RInstallmentsMergeFlag = 0;
                }
                if (promise.masterdata[0].fmC_RPaperSizeFlag === 1) {
                    $scope.FMC_RPaperSizeFlag = 1;
                }
                else if (promise.masterdata[0].fmC_RPaperSizeFlag === 2) {
                    $scope.FMC_RPaperSizeFlag = 2;

                }
                else if (promise.masterdata[0].fmC_RPaperSizeFlag === 3) {
                    $scope.FMC_RPaperSizeFlag = 3;
                }
                else if (promise.masterdata[0].fmC_RPaperSizeFlag === 4) {
                    $scope.FMC_RPaperSizeFlag = 4;
                }
                else {
                    $scope.FMC_RPaperSizeFlag = false;
                }
                if (promise.masterdata[0].fmC_RPaymentDetailsFlag === 1) {
                    $scope.FMC_RPaymentDetailsFlag = 1;
                }
                else {
                    $scope.FMC_RPaymentDetailsFlag = 0;
                }
                if (promise.masterdata[0].fmC_RRemarksFlag === 1) {
                    $scope.FMC_RRemarksFlag = 1;
                }
                else {
                    $scope.FMC_RRemarksFlag = 0;
                }
                if (promise.masterdata[0].fmC_RSectionFlag === 1) {
                    $scope.FMC_RSectionFlag = 1;
                }
                else {
                    $scope.FMC_RSectionFlag = 0;
                }
                if (promise.masterdata[0].fmC_RSplFeeHeadFlag === 1) {
                    $scope.FMC_RSplFeeHeadFlag = 1;
                }
                else {
                    $scope.FMC_RSplFeeHeadFlag = 0;
                }
                if (promise.masterdata[0].fmC_RUserNameFlag === 1) {
                    $scope.FMC_RUserNameFlag = 1;
                }
                else {
                    $scope.FMC_RUserNameFlag = 0;
                }
                if (promise.masterdata[0].fmC_RWaivedFlag === 1) {
                    $scope.FMC_RWaivedFlag = 1;
                }
                else {
                    $scope.FMC_RWaivedFlag = 0;
                }


                if (promise.masterdata[0].fmC_EableStaffTrans === 1) {
                    $scope.FMC_EableStaffTrans = 1;
                }
                else {
                    $scope.FMC_EableStaffTrans = 0;
                }


                if (promise.masterdata[0].fmC_EableOtherStudentTrans === 1) {
                    $scope.FMC_EableOtherStudentTrans = 1;
                }
                else {
                    $scope.FMC_EableOtherStudentTrans = 0;
                }


                //if (promise.masterdata[0].fmC_BtachwiseFeeGlg === true) {
                //    $scope.FMC_BtachwiseFeeGlg = 1;
                //}
                //else {
                //    $scope.FMC_BtachwiseFeeGlg = 0;
                //}

                if (promise.masterdata[0].fmC_MakerCheckerReqdFlg === true) {
                    $scope.FMC_MakerCheckerReqdFlg = 1;
                }
                else {
                    $scope.FMC_MakerCheckerReqdFlg = 0;
                }

                if (promise.masterdata[0].fmC_RoomwiseHostelFeeFlg === true) {
                    $scope.FMC_RoomwiseHostelFeeFlg = 1;
                }
                else {
                    $scope.FMC_RoomwiseHostelFeeFlg = 0;
                }
                if (promise.masterdata[0].fmC_CommonHostelFeeFlg === true) {
                    $scope.FMC_CommonHostelFeeFlg = 1;
                }
                else {
                    $scope.FMC_CommonHostelFeeFlg = 0;
                }
                if (promise.masterdata[0].fmC_CommonTransportLocationFeeFlg === true) {
                    $scope.FMC_CommonTransportLocationFeeFlg = 1;
                }
                else {
                    $scope.FMC_CommonTransportLocationFeeFlg = 0;
                }

                if (promise.masterdata[0].fmC_CommonTransportAreaFeeFlg === true) {
                    $scope.FMC_CommonTransportAreaFeeFlg = 1;
                }
                else {
                    $scope.FMC_CommonTransportAreaFeeFlg = 0;
                }


                if (promise.masterdata[0].fmC_TransportFeeLocationFlag === true) {
                    $scope.FMC_TransportFeeLocationFlag = 1;
                }
                else {
                    $scope.FMC_TransportFeeLocationFlag = 0;
                }

                if (promise.masterdata[0].fmC_FineMapping === true) {
                    $scope.FMC_FineMapping = 1;
                }
                else {
                    $scope.FMC_FineMapping = 0;
                }

                $scope.FMC_RebateTypeFlag = promise.masterdata[0].fmC_RebateTypeFlag;
                $scope.FMC_Receipt_Signatory = promise.masterdata[0].fmC_Receipt_Signatory;
                $scope.FMC_No_Receipt = promise.masterdata[0].fmC_No_Receipt;
                $scope.FMC_Receipt_SignatoryImage = promise.masterdata[0].fmC_Receipt_SignatoryImage;
                if (promise.masterdata[0].fmC_StudentwiseJVFlag === "S") {
                    $scope.FMC_StudentwiseJVFlag = "S";
                }
                else if (promise.masterdata[0].fmC_StudentwiseJVFlag === "H") {
                    $scope.FMC_StudentwiseJVFlag = "H";
                }
                if (promise.masterdata[0].fmC_TransportFeeAreaFlag === 1) {
                    $scope.FMC_TransportFeeAreaFlag = 1;
                }
                else {
                    $scope.FMC_TransportFeeAreaFlag = 0;
                }
                if (promise.masterdata[0].fmC_TransportFeeZoneFlag === 1) {
                    $scope.FMC_TransportFeeZoneFlag = 1;
                }
                else {
                    $scope.FMC_TransportFeeZoneFlag = 0;
                }

                if (promise.masterdata[0].fmC_FineEnableDisable === true) {
                    $scope.FMC_FineEnableDisable = 1;
                }
                else {
                    $scope.FMC_FineEnableDisable = 0;
                }


                if (promise.masterdata[0].fmC_Receipt_Format === 1) {
                    $scope.fmC_Receipt_Format = 1;
                }
                else {
                    $scope.fmC_Receipt_Format = 0;
                }

                if (promise.masterdata[0].fmC_MakerCheckerReqdFlg === true) {
                    $scope.FMC_MakerCheckerReqdFlg = 1;
                }
                else {
                    $scope.FMC_MakerCheckerReqdFlg = 0;
                }

                if (promise.masterdata[0].fmC_RoomwiseHostelFeeFlg === true) {
                    $scope.FMC_RoomwiseHostelFeeFlg = 1;
                }
                else {
                    $scope.FMC_RoomwiseHostelFeeFlg = 0;
                }

                if (promise.masterdata[0].fmC_CommonHostelFeeFlg === true) {
                    $scope.FMC_CommonHostelFeeFlg = 1;
                }
                else {
                    $scope.FMC_CommonHostelFeeFlg = 0;
                }

                if (promise.masterdata[0].fmC_CommonTransportLocationFeeFlg === true) {
                    $scope.FMC_CommonTransportLocationFeeFlg = 1;
                }
                else {
                    $scope.FMC_CommonTransportLocationFeeFlg = 0;
                }

                if (promise.masterdata[0].fmC_TransportFeeLocationFlag === true) {
                    $scope.FMC_TransportFeeLocationFlag = 1;
                }
                else {
                    $scope.FMC_TransportFeeLocationFlag = 0;
                }
            }
        })
        }

        $scope.Cleardata = function () {
            $state.reload();
        }

        $scope.edittransaction = function (fmcid) {

            var data = {
                "FMC_Id": fmcid
            }

            apiService.create("MasterClgFeeConfig/editdetails", data).
                then(function (promise) {

                    $scope.fmC_Id = promise.masterdata[0].fmC_Id;
                    //$scope.students = promise.masterdata;


                    if (promise.masterdata[0].fmC_Areawise_FeeFlg === 1) {
                        $scope.FMC_Areawise_FeeFlg = 1;
                    }
                    else {
                        $scope.FMC_Areawise_FeeFlg = 0;
                    }
                    if (promise.masterdata[0].fmC_ArrearAfterFlag === 1) {
                        $scope.FMC_ArrearAfterFlag = 1;
                    }
                    else {
                        $scope.FMC_ArrearAfterFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_ArrearColumn === 1) {
                        $scope.FMC_ArrearColumn = 1;
                    }
                    else {
                        $scope.FMC_ArrearColumn = 0;
                    }
                    if (promise.masterdata[0].fmC_ArrearLedgerFlag === 1) {
                        $scope.FMC_ArrearLedgerFlag = 1;
                    }
                    else {
                        $scope.FMC_ArrearLedgerFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_AutoReceiptFeeGroupFlag === 1) {
                        $scope.FMC_AutoReceiptFeeGroupFlag = 1;
                    }
                    else {
                        $scope.FMC_AutoReceiptFeeGroupFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_ChallanOptionFlag === 1) {
                        $scope.FMC_ChallanOptionFlag = 1;
                    }
                    else {
                        $scope.FMC_ChallanOptionFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_DOACheckFlag === 1) {
                        $scope.FMC_DOACheckFlag = 1;
                    }
                    else {
                        $scope.FMC_DOACheckFlag = 0;
                    }
                    $scope.FMC_Default_Currency = promise.masterdata[0].fmC_Default_Currency;
                    if (promise.masterdata[0].fmC_Fine_Column === 1) {
                        $scope.FMC_Fine_Column = 1;
                    }
                    else {
                        $scope.FMC_Fine_Column = 0;
                    }
                    if (promise.masterdata[0].fmC_Fine_LedgerFlag === 1) {
                        $scope.FMC_Fine_LedgerFlag = 1;
                    }
                    else {
                        $scope.FMC_Fine_LedgerFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_GroupOrTermFlg === "T") {
                        $scope.FMC_GroupOrTermFlg = "T";
                    }
                    else {
                        $scope.FMC_GroupOrTermFlg = "G";
                    }
                    if (promise.masterdata[0].fmC_GroupRemarksFlag === 1) {
                        $scope.FMC_GroupRemarksFlag = 1;
                    }
                    else {
                        $scope.FMC_GroupRemarksFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_MotherNameFlag === 1) {
                        $scope.FMC_MotherNameFlag = 1;
                    }
                    else {
                        $scope.FMC_MotherNameFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RAddressFlag === 1) {
                        $scope.FMC_RAddressFlag = 1;
                    }
                    else {
                        $scope.FMC_RAddressFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RAmountFlag === 1) {
                        $scope.FMC_RAmountFlag = 1;
                    }
                    else {
                        $scope.FMC_RAmountFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RAmountReceivedFlag === 1) {
                        $scope.FMC_RAmountReceivedFlag = 1;
                    }
                    else {
                        $scope.FMC_RAmountReceivedFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RBalanceFlag === 1) {
                        $scope.FMC_RBalanceFlag = 1;
                    }
                    else {
                        $scope.FMC_RBalanceFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RBankFlag === 1) {
                        $scope.FMC_RBankFlag = 1;
                    }
                    else {
                        $scope.FMC_RBankFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RClassFlag === 1) {
                        $scope.FMC_RClassFlag = 1;
                    }
                    else {
                        $scope.FMC_RClassFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RConcessionFlag === 1) {
                        $scope.FMC_RConcessionFlag = 1;
                    }
                    else {
                        $scope.FMC_RConcessionFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RCurrentDateFlag === 1) {
                        $scope.FMC_RCurrentDateFlag = 1;
                    }
                    else {
                        $scope.FMC_RCurrentDateFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RDueDateFlag === 1) {
                        $scope.FMC_RDueDateFlag = 1;
                    }
                    else {
                        $scope.FMC_RDueDateFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RFatherNameFlag === 1) {
                        $scope.FMC_RFatherNameFlag = 1;
                    }
                    else {
                        $scope.FMC_RFatherNameFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RFeeGroupFeeHeadFlag === 1) {
                        $scope.FMC_RFeeGroupFeeHeadFlag = 1;
                    }
                    else {
                        $scope.FMC_RFeeGroupFeeHeadFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RFeeHeaderFlag === 1) {
                        $scope.FMC_RFeeHeaderFlag = 1;
                    }
                    else {
                        $scope.FMC_RFeeHeaderFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RFineFlag === 1) {
                        $scope.FMC_RFineFlag = 1;
                    }
                    else {
                        $scope.FMC_RFineFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RHeaderTitleFlag === 1) {
                        $scope.FMC_RHeaderTitleFlag = 1;
                    }
                    else {
                        $scope.FMC_RHeaderTitleFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RInstallmentsFlag === 1) {
                        $scope.FMC_RInstallmentsFlag = 1;
                    }
                    else {
                        $scope.FMC_RInstallmentsFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RInstallmentsMergeFlag === 1) {
                        $scope.FMC_RInstallmentsMergeFlag = 1;
                    }
                    else {
                        $scope.FMC_RInstallmentsMergeFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RPaperSizeFlag === 1) {
                        $scope.FMC_RPaperSizeFlag = 1;
                    }
                    else if (promise.masterdata[0].fmC_RPaperSizeFlag === 2) {
                        $scope.FMC_RPaperSizeFlag = 2;

                    }
                    else if (promise.masterdata[0].fmC_RPaperSizeFlag === 3) {
                        $scope.FMC_RPaperSizeFlag = 3;
                    }
                    else if (promise.masterdata[0].fmC_RPaperSizeFlag === 4) {
                        $scope.FMC_RPaperSizeFlag = 4;
                    }
                    else {
                        $scope.FMC_RPaperSizeFlag = false;
                    }
                    if (promise.masterdata[0].fmC_RPaymentDetailsFlag === 1) {
                        $scope.FMC_RPaymentDetailsFlag = 1;
                    }
                    else {
                        $scope.FMC_RPaymentDetailsFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RRemarksFlag === 1) {
                        $scope.FMC_RRemarksFlag = 1;
                    }
                    else {
                        $scope.FMC_RRemarksFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RSectionFlag === 1) {
                        $scope.FMC_RSectionFlag = 1;
                    }
                    else {
                        $scope.FMC_RSectionFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RSplFeeHeadFlag === 1) {
                        $scope.FMC_RSplFeeHeadFlag = 1;
                    }
                    else {
                        $scope.FMC_RSplFeeHeadFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RUserNameFlag === 1) {
                        $scope.FMC_RUserNameFlag = 1;
                    }
                    else {
                        $scope.FMC_RUserNameFlag = 0;
                    }
                    if (promise.masterdata[0].fmC_RWaivedFlag === 1) {
                        $scope.FMC_RWaivedFlag = 1;
                    }
                    else {
                        $scope.FMC_RWaivedFlag = 0;
                    }


                    if (promise.masterdata[0].fmC_EableStaffTrans === 1) {
                        $scope.FMC_EableStaffTrans = 1;
                    }
                    else {
                        $scope.FMC_EableStaffTrans = 0;
                    }


                    if (promise.masterdata[0].fmC_EableOtherStudentTrans === 1) {
                        $scope.FMC_EableOtherStudentTrans = 1;
                    }
                    else {
                        $scope.FMC_EableOtherStudentTrans = 0;
                    }


                    if (promise.masterdata[0].fmC_USER_PREVILEDGE === 1) {
                        $scope.FMC_USER_PREVILEDGE = 1;
                    }
                    else {
                        $scope.FMC_USER_PREVILEDGE = 0;
                    }

                    if (promise.masterdata[0].fmC_StaffConcessionCheck != null && promise.masterdata[0].fmC_StaffConcessionCheck != undefined) {
                        $scope.fmC_StaffConcessionCheck = promise.masterdata[0].fmC_StaffConcessionCheck;
                    }
                    else {
                        $scope.fmC_StaffConcessionCheck = false;
                    }


                    if (promise.masterdata[0].fmC_ShowPreviousFeeFisrtFlg != null && promise.masterdata[0].fmC_ShowPreviousFeeFisrtFlg != undefined) {
                        $scope.fmC_ShowPreviousFeeFisrtFlg = promise.masterdata[0].fmC_ShowPreviousFeeFisrtFlg;
                    }
                    else {
                        $scope.fmC_ShowPreviousFeeFisrtFlg = false;
                    }

                    $scope.FMC_RebateTypeFlag = promise.masterdata[0].fmC_RebateTypeFlag;
                    $scope.FMC_Receipt_Signatory = promise.masterdata[0].fmC_Receipt_Signatory;
                    $scope.FMC_No_Receipt = promise.masterdata[0].fmC_No_Receipt;
                    $scope.FMC_Receipt_SignatoryImage = promise.masterdata[0].fmC_Receipt_SignatoryImage;
                    if (promise.masterdata[0].fmC_StudentwiseJVFlag === "S") {
                        $scope.FMC_StudentwiseJVFlag = "S";
                    }
                    else if (promise.masterdata[0].fmC_StudentwiseJVFlag === "H") {
                        $scope.FMC_StudentwiseJVFlag = "H";
                    }
                    if (promise.masterdata[0].fmC_TransportFeeAreaFlag === 1) {
                        $scope.FMC_TransportFeeAreaFlag = 1;
                    }
                    else {
                        $scope.FMC_TransportFeeAreaFlag = 0;
                    }

                    if (promise.masterdata[0].fmC_TransportFeeZoneFlag === 1) {
                        $scope.FMC_TransportFeeZoneFlag = 1;
                    }
                    else {
                        $scope.FMC_TransportFeeZoneFlag = 0;
                    }

                    if (promise.masterdata[0].fmC_FineEnableDisable === true) {
                        $scope.FMC_FineEnableDisable = 1;
                    }
                    else {
                        $scope.FMC_FineEnableDisable = 0;
                    }

                    if (promise.masterdata[0].fmC_FineMapping === true) {
                        $scope.FMC_FineMapping = 1;
                    }
                    else {
                        $scope.FMC_FineMapping = 0;
                    }

                    if (promise.masterdata[0].fmC_Receipt_Format === 1) {
                        $scope.fmC_Receipt_Format = 1;
                    }
                    else {
                        $scope.fmC_Receipt_Format = 0;
                    }
                    //Praveen Added
                    if (promise.masterdata[0].fmC_AutoRecieptPrintFlag != null && promise.masterdata[0].fmC_AutoRecieptPrintFlag != undefined) {
                        $scope.fmC_AutoRecieptPrintFlag = promise.masterdata[0].fmC_AutoRecieptPrintFlag
                    }
                    else {
                        $scope.fmC_AutoRecieptPrintFlag = false;
                    }


                    if (promise.masterdata[0].fmC_MakerCheckerReqdFlg === true) {
                        $scope.FMC_MakerCheckerReqdFlg = 1;
                    }
                    else {
                        $scope.FMC_MakerCheckerReqdFlg = 0;
                    }

                    if (promise.masterdata[0].fmC_RoomwiseHostelFeeFlg === true) {
                        $scope.FMC_RoomwiseHostelFeeFlg = 1;
                    }
                    else {
                        $scope.FMC_RoomwiseHostelFeeFlg = 0;
                    }

                    if (promise.masterdata[0].fmC_CommonHostelFeeFlg === true) {
                        $scope.FMC_CommonHostelFeeFlg = 1;
                    }
                    else {
                        $scope.FMC_CommonHostelFeeFlg = 0;
                    }

                    if (promise.masterdata[0].fmC_CommonTransportLocationFeeFlg === true) {
                        $scope.FMC_CommonTransportLocationFeeFlg = 1;
                    }
                    else {
                        $scope.FMC_CommonTransportLocationFeeFlg = 0;
                    }

                    if (promise.masterdata[0].fmC_TransportFeeLocationFlag === true) {
                        $scope.FMC_TransportFeeLocationFlag = 1;
                    }
                    else {
                        $scope.FMC_TransportFeeLocationFlag = 0;
                    }

                })
        }

    }
})();
