(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeFeeTransactionController', CollegeFeeTransactionController)


    CollegeFeeTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI', '$compile']
    function CollegeFeeTransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI, $compile) {
        $scope.totalconcession = 0;
        $scope.totalfine = 0;
        $scope.totalwaived = 0;

        $scope.disdddate = false;
        $scope.disddno = false;
        $scope.disbankname = false;

        $scope.studentsavedlist = true;
        $scope.printreceipt = false;
        $scope.printview = true;
        $scope.obj = {};
        //added on 09102017
        $scope.allcheck = false;
        $scope.totcountsearch = 0;
        $scope.disablefine = true;
        $scope.disableconcession = true;
        $scope.disablenetamount = true;
        $scope.disablefsS_CurrentYrCharges = true;
        $scope.disablefsS_TotalToBePaid = true;
        $scope.rolenamelist = "";
        var automanualreceiptnotranum, institutionid;
        var grouporterm, autoreceipt, receiptformat, mergeinstallment, fineapplicable;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
      
        if (transactionnumbering != null && transactionnumbering.length > 0) {
            for (var i = 0; i < transactionnumbering.length; i++) {
                if (transactionnumbering[i].imN_Flag == "Online") {
                    $scope.transnumbconfigurationsettingsassign = transactionnumbering[i];
                }
            }
        }
        $scope.Clear_Chaln_No = function () {
            $scope.FYP_ChallanNo = null;
            $scope.Clear_values_dep();
        }
        if (configsettings != null && configsettings.length > 0) {

            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            $scope.grouporterm_flag = grouporterm;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
            $scope.FMC_RInstallmentsMergeFlag = mergeinstallment;
            fineapplicable = configsettings[0].fmC_FineEnableDisable;
        }      

        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        if (grouporterm == "T") {
            $scope.grouportername = "Term Name"
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name"
        }

        $scope.reloaddata = function () {
            $scope.groupcount = [];
            $scope.grigview1 = false;
            $scope.obj.allgrouporterm = false;
            //MB for Challan
            $scope.Clear_Chaln_No();
            // }
            //MB for Challan
            $scope.alltermchk = false;
        }

        $scope.search = '';
        $scope.showreceiptno = true;
        $scope.bankdetails = true;
        $scope.optradio = true;
        $scope.cfg = {};
        $scope.formload = function () {
           
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10;
            var pageid = 1;
            apiService.getURI("CollegeFeeTransaction/getalldetails", pageid).
                then(function (promise) {
                    
                    $scope.rolenamelist = promise.rolename;

                    $scope.yearlst = promise.fillyear;

                    
                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                    $scope.loginid = promise.userId;

                    if (promise.transnumconfig.length > 0) {
                        automanualreceiptnotranum = promise.transnumconfig[0].imN_AutoManualFlag;
                    }
                    if (autoreceipt == 1 || promise.transnumconfig[0].imN_AutoManualFlag == "Auto") {
                        $scope.showreceiptno = false;
                    }
                    else {
                        $scope.showreceiptno = true;
                    }
                    $scope.receiptgrid = promise.receiparraydelete;

                    $scope.FYP_ReceiptDate = new Date();
                    $scope.FYP_FineReceiptDate = new Date();

                    $scope.FYP_DD_Cheque_Date = new Date();
                    if ($scope.FYP_Bank_Or_Cash == 'C') {
                        $scope.bankdetails = false;
                    }
                    else if ($scope.FYP_Bank_Or_Cash == 'B') {
                        $scope.bankdetails = true;
                    }
                    //configuration settings
                    if (promise.feeconfiglist.length > 0) {
                        
                        grouporterm = promise.feeconfiglist[0].fmC_GroupOrTermFlg;
                        $scope.grouporterm_flag = grouporterm;
                        mergeinstallment = promise.feeconfiglist[0].fmC_RInstallmentsMergeFlag;//added by kiran
                        $scope.FMC_RInstallmentsMergeFlag = mergeinstallment;

                        $scope.FMC_RFeeHeaderFlag = promise.feeconfiglist[0].fmC_RFeeHeaderFlag;
                        $scope.FMC_RClassFlag = promise.feeconfiglist[0].fmC_RClassFlag;
                        $scope.FMC_RSectionFlag = promise.feeconfiglist[0].fmC_RSectionFlag;
                        $scope.FMC_RUserNameFlag = promise.feeconfiglist[0].fmC_RUserNameFlag;
                        $scope.FMC_RFatherNameFlag = promise.feeconfiglist[0].fmC_RFatherNameFlag;
                        $scope.FMC_MotherNameFlag = promise.feeconfiglist[0].fmC_MotherNameFlag;
                        $scope.FMC_RHeaderTitleFlag = promise.feeconfiglist[0].fmC_RHeaderTitleFlag;
                        $scope.FMC_RPaymentDetailsFlag = promise.feeconfiglist[0].fmC_RPaymentDetailsFlag;
                        $scope.FMC_RAmountReceivedFlag = promise.feeconfiglist[0].fmC_RAmountReceivedFlag;
                        $scope.FMC_RRemarksFlag = promise.feeconfiglist[0].fmC_RRemarksFlag;
                        $scope.FMC_RCurrentDateFlag = promise.feeconfiglist[0].fmC_RCurrentDateFlag;

                        $scope.FMC_RInstallmentsFlag = promise.feeconfiglist[0].fmC_RInstallmentsFlag;
                        $scope.FMC_RInstallmentsMergeFlag = promise.feeconfiglist[0].fmC_RInstallmentsMergeFlag;
                        $scope.FMC_RFineFlag = promise.feeconfiglist[0].fmC_RFineFlag;
                        $scope.FMC_RConcessionFlag = promise.feeconfiglist[0].fmC_RConcessionFlag;
                        $scope.FMC_RWaivedFlag = promise.feeconfiglist[0].fmC_RWaivedFlag;
                        $scope.FMC_RBalanceFlag = promise.feeconfiglist[0].fmC_RBalanceFlag;
                        $scope.FMC_RAmountFlag = promise.feeconfiglist[0].fmC_RAmountFlag;
                        $scope.FMC_RBankFlag = promise.feeconfiglist[0].fmC_RBankFlag;
                        $scope.FMC_ChallanOptionFlag = promise.feeconfiglist[0].fmC_ChallanOptionFlag;
                        $scope.FMC_RDueDateFlag = promise.feeconfiglist[0].fmC_RDueDateFlag;
                        $scope.FMC_RAddressFlag = promise.feeconfiglist[0].fmC_RAddressFlag;

                        if (grouporterm == "T") {
                            $scope.grouportername = "Term Name"
                        }
                        else if (grouporterm == "G") {
                            $scope.grouportername = "Group Name"
                        }
                    }
                    $scope.getdates(promise.asmaY_Id, promise.asmaY_Year);
                    //MB for Special
                    $scope.special_head_list = promise.specialheadlist;
                    $scope.special_head_details = promise.specialheaddetails;
                    //MB for Special
                })

        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.isRowDisabled = function (item) {

            if (item.FMH_Flag == 'LS') {
                return item.disabled;
            }
           
        };

        $scope.temptermarray = [];
        $scope.temp_Fine_Amountsadv = [];
        //MB for Special
        var remove_list = [];
        var ins_spe_list = [];
        //MB for Special
        $scope.onselectgroup = function (option) {
            $scope.PayingAmount = null;
            $scope.all = false;
            $scope.curramount = 0;
            $scope.temptermarray = [];

            $scope.tempconheadarray = [];

            var groupid = "0";
            $scope.FYP_Bank_Name = ""
            $scope.FYP_DD_Cheque_No = "";
            $scope.FYP_Remarks = "";
            $scope.FYP_DD_Cheque_Date = new Date();
            //$scope.FYPPM_ClearanceDate = $scope.FYP_ReceiptDate;
            var countf = 0;
            for (var i = 0; i < $scope.groupcount.length; i++) {
                if ($scope.groupcount[i].selected == true) {
                    countf = countf + 1;
                    groupid = groupid + ',' + $scope.groupcount[i].fmG_Id;
                }
            }
            var cnt = 0;
            var selectcnt = 0;
            angular.forEach($scope.groupcount, function (obj) {
                if (obj.selected) {
                    selectcnt += 1;
                }
                if (!obj.disablepaisterms) {
                    cnt += 1;
                }
            })
            if (cnt == selectcnt) {
                //$scope.obj.allgrouporterm = option.selected;
            }
            else {
                $scope.obj.allgrouporterm = false;
            }
            if (countf > 0) {
                var data = {
                    "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                    "multiplegroups": groupid,
                    "FYP_ReceiptDate": new Date($scope.FYP_ReceiptDate).toDateString(),
                    //"FYP_ReceiptDate": new Date($scope.FYP_FineReceiptDate).toDateString(),
                    "filterinitialdata": $scope.filterdata,
                    "configset": grouporterm,
                    "minstall": mergeinstallment,
                    "ASMAY_Id": $scope.cfg.ASMAY_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CollegeFeeTransaction/getgroupmappedheadsnew", data).
                    then(function (promise) {

                        if (promise.bankname.length > 0) {
                            $scope.bankmaster = promise.bankname;
                        }
                        if (promise.getpdcdetails != null) {
                            if (promise.getpdcdetails.length > 0) {
                                $scope.studentpdcdetails = promise.getpdcdetails;
                            }
                        }

                        if (promise.fetchmodeofpayment.length > 0) {
                            $scope.modeofpayment = promise.fetchmodeofpayment;
                        }

                        if (promise.alldata.length > 0) {
                            $scope.grigview1 = true;
                            //MB for Fine
                            angular.forEach(promise.alldata, function (ed) {
                                if (ed.FMH_Flag == 'F') {
                                    $scope.temp_finehead_amt = ed.FCSS_ToBePaid;
                                    $scope.temp_finehead_amt_total = ed.FCSS_TotalCharges + ed.FCSS_FineAmount;
                                }

                            })

                            if (promise.fine_FCMAS_Ids.length > 0) {
                                $scope.temp_Fine_Amounts = promise.fine_FCMAS_Ids;
                            }
                            if (promise.fine_FCMAS_IdsAdvance.length > 0) {
                                //added  on 21/10/2021 //
                                $scope.temp_Fine_Amountsadv = promise.fine_FCMAS_IdsAdvance;
                               //added  on 21/10/2021 //
                            }
                            //MB for Fine
                            //added on 09102017
                            $scope.highestcountgid = promise.validationgroupid;

                            //hema

                            // if (option.selected == true) {

                            for (var i = 0; i < promise.alldata.length; i++) {

                                if (promise.alldata[i].FMH_Flag != "LS") {
                                    $scope.temptermarray.push(promise.alldata[i]);
                                } else {
                                    $scope.tempconheadarray.push(promise.alldata[i]);
                                }
                                

                            }
                            //hema
                            var addfinetonetamount = 0, addnetamount = 0, totalpayableamount;
                            $scope.totalgrid = $scope.temptermarray;

                            $scope.concessionheadgrid = $scope.tempconheadarray;

                            for (var i = 0; i < $scope.totalgrid.length; i++) {
                                addfinetonetamount = $scope.totalgrid[i].FCSS_FineAmount;
                                addnetamount = $scope.totalgrid[i].FCSS_TotalCharges;
                                if (Number(addfinetonetamount) > 0) {
                                    $scope.totalgrid[i].FCSS_TotalToBePaidaddfine = Number(addfinetonetamount) + Number(addnetamount);
                                }
                                else {
                                    $scope.totalgrid[i].FCSS_TotalToBePaidaddfine = Number(addnetamount);
                                }
                            }

                            var totalamt = 0;
                            var balanceamt = 0;
                            var concessionaamt = 0;
                            var fineamt = 0;

                            angular.forEach($scope.totalgrid, function (value, key) {
                                //  if (value.amcsT_Id == 0) {
                                totalamt = totalamt + value.FCSS_NetAmount;
                                balanceamt = balanceamt + value.FCSS_ToBePaid;
                                concessionaamt = concessionaamt + value.FCSS_ConcessionAmount;
                                fineamt = fineamt + value.FCSS_FineAmount;
                                // }
                            })

                            $scope.totalfee = totalamt;
                            $scope.currbalance = balanceamt;


                            //MB for Special
                        
                            $scope.installments_list = [];
                            angular.forEach(promise.instalspecial, function (intr) {
                                $scope.installments_list.push(intr);
                            })
                            $scope.temp_Head_Instl_list = [];
                            angular.forEach($scope.totalgrid, function (uy) {
                                uy.Head_Flag = 'H';
                                uy.tobepaid_M = uy.FCSS_ToBePaid;
                                $scope.temp_Head_Instl_list.push(uy);
                            })
                            remove_list = [];
                            ins_spe_list = [];
                            angular.forEach(promise.instalspecial, function (ins) {
                                var special_list = [];
                                angular.forEach($scope.special_head_list, function (op1) {
                                    var spe_ind_list = [];
                                    angular.forEach($scope.special_head_details, function (op2) {
                                        if (op1.fmsfH_Id == op2.fmsfH_Id) {
                                            angular.forEach($scope.totalgrid, function (op_m) {
                                                if (op_m.FMH_Id == op2.fmH_ID && op_m.FTI_Id == ins.ftI_Id) {
                                                    spe_ind_list.push(op_m);
                                                    remove_list.push(op_m);
                                                }
                                            })
                                        }

                                    })
                                    if (spe_ind_list.length > 0) {
                                        special_list.push({ sp_id: op1.fmsfH_Id, sp_name: op1.fmsfH_Name, sp_ind_list: spe_ind_list });
                                    }
                                })
                                if (special_list.length > 0) {
                                    ins_spe_list.push({ ftI_Id: ins.ftI_Id, ftI_Name: ins.ftI_Name, sp_list: special_list });
                                }
                            })

                            if (ins_spe_list.length > 0) {
                                angular.forEach(remove_list, function (ma1) {
                                    $scope.temp_Head_Instl_list.splice($scope.temp_Head_Instl_list.indexOf(ma1), 1);
                                })
                                angular.forEach(ins_spe_list, function (a1) {

                                    angular.forEach(a1.sp_list, function (a2) {
                                        var FCSS_CurrentYrCharges = 0;
                                        var FCSS_TotalCharges = 0;
                                        var FCSS_ConcessionAmount = 0;
                                        var FCSS_FineAmount = 0;
                                        var FCSS_ToBePaid = 0;
                                        var FCSS_TotalToBePaidaddfine = 0;
                                        var FCSS_OBArrearAmount = 0;
                                        var FMG_Id = 0;
                                        var FMG_GroupName = '';
                                        var not_cnt = 0;
                                        var totamtt = 0;
                                        angular.forEach(a2.sp_ind_list, function (a3) {
                                            if (FMG_Id == 0) {
                                                FMG_Id = a3.fmG_Id;
                                                FMG_GroupName = a3.fmG_GroupName;
                                            }
                                            else {
                                                if (FMG_Id != a3.fmG_Id) {
                                                    not_cnt += 1;
                                                }
                                            }

                                            FCSS_CurrentYrCharges += a3.FCSS_CurrentYrCharges;
                                            FCSS_TotalCharges += a3.FCSS_TotalCharges;
                                            FCSS_ConcessionAmount += a3.FCSS_ConcessionAmount;
                                            FCSS_FineAmount += a3.FCSS_FineAmount;
                                            FCSS_ToBePaid += a3.FCSS_ToBePaid;
                                            FCSS_TotalToBePaidaddfine += a3.FCSS_TotalCharges;
                                            FCSS_OBArrearAmount += a3.FCSS_OBArrearAmount;
                                        })
                                        if (not_cnt == 0) {
                                            $scope.temp_Head_Instl_list.push({ FMG_Id: FMG_Id, FMG_GroupName: 'SH_' + FMG_GroupName, FMH_Id: a2.sp_id, FMH_FeeName: a2.sp_name, FTI_Id: a1.ftI_Id, FTI_Name: a1.ftI_Name, FCSS_NetAmount: FCSS_CurrentYrCharges, FCSS_TotalCharges: FCSS_TotalCharges, FCSS_ConcessionAmount: FCSS_ConcessionAmount, FCSS_FineAmount: FCSS_FineAmount, FCSS_ToBePaid: FCSS_ToBePaid, FCSS_TotalToBePaidaddfine: FCSS_TotalToBePaidaddfine, Head_Flag: 'SH', FCSS_OBArrearAmount: FCSS_OBArrearAmount });
                                        }
                                        else if (not_cnt > 0) {
                                            $scope.temp_Head_Instl_list.push({ FMG_Id: 0, FMG_GroupName: 'Special_Head', FMH_Id: a2.sp_id, FMH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, FCSS_NetAmount: FCSS_CurrentYrCharges, FCSS_TotalCharges: FCSS_TotalCharges, FCSS_ConcessionAmount: FCSS_ConcessionAmount, FCSS_FineAmount: FCSS_FineAmount, FCSS_ToBePaid: FCSS_ToBePaid, FCSS_TotalToBePaidaddfine: FCSS_TotalToBePaidaddfine, Head_Flag: 'SH', FCSS_OBArrearAmount: FCSS_OBArrearAmount });
                                        }

                                    })
                                })
                                $scope.totalgrid = $scope.temp_Head_Instl_list;
                            }

                            //MB for Special

                            if (promise.advancefee.length > 0) {
                                $scope.totalgridadvance = promise.advancefee;
                                $scope.AMSE_SEMName = promise.advancefee[0].AMSE_SEMName;
                            }
                            else {
                                $scope.totalgridadvance = [];
                            }

                            angular.forEach($scope.totalgrid, function (user) {

                                var date2 = new Date(user.FCMAS_DueDate);
                                var date1 = new Date($scope.FYP_ReceiptDate);

                                var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                                $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));

                                user.DiffDays = $scope.dayDifference;
                            })

                            angular.forEach($scope.totalgridadvance, function (user1) {

                                var date4 = new Date(user1.FCMAS_DueDate);
                                var date3 = new Date($scope.FYP_ReceiptDate);

                                var timeDiff = Math.abs(date4.getTime() - date3.getTime());
                                $scope.dayDifference1 = Math.ceil(timeDiff / (1000 * 3600 * 24));

                                user1.DiffDays = $scope.dayDifference1;
                            })

                        }
                        else {
                            if (selectcnt > 0) {
                                swal("Student has paid amount for that Group")
                            }
                            else if (selectcnt == 0) {
                                $scope.grigview1 = false;
                                $scope.totalgrid = [];
                            }
                        }
                    })
            }

            else {
                $scope.grigview1 = false;
                $scope.totalgrid = "";
            }

        };

        $scope.onselectacademic = function (yearlst) {
            var data = {
                "filterinitialdata": $scope.filterdata,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeFeeTransaction/getacademicyear", data).
                then(function (promise) {

                    $scope.receiptgrid = promise.receiparraydelete;

                    $scope.studentlst = promise.fillstudent;
                })
        };

        $scope.tobepaidamt = function (totalgrid, index) {
            var count = 0, intertobepaidamt = 0;
            angular.forEach($scope.totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })

            var data = {
                "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                "FCMAS_Id": totalgrid[index].FCMAS_Id,
                "FCSS_ToBePaid": totalgrid[index].FCSS_ToBePaid,
                "FYP_ReceiptDate": new Date($scope.FYP_ReceiptDate).toDateString(),
                "FYP_ReceiptDate": new Date($scope.FYP_FineReceiptDate).toDateString(),
                "ASMAY_Id": $scope.cfg.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            //apiService.create("CollegeFeeTransaction/dynamicfinecalculation", data).
            //    then(function (promise) {

            //        angular.forEach($scope.totalgrid, function (user) {
            //            if (user.FMH_Flag == 'F') {
            //                user.FCSS_ToBePaid = promise.fcsS_FineAmount;
            //            }
            //        })

            //        $scope.totalfine = promise.fcsS_FineAmount;

            if (count <= 1) {
                if (Number(totalgrid[index].FCSS_TotalCharges) >= Number(totalgrid[index].FCSS_ToBePaid)) {
                    $scope.curramount = Number(totalgrid[index].FCSS_ToBePaid) + Number(totalgrid[index].FCSS_FineAmount) + Number(totalgrid[index].FCSS_OBArrearAmount);
                    //MB For Fine
                    if (totalgrid[index].fmH_Flag == 'F') {
                        $scope.totalfine = Number(totalgrid[index].FCSS_ToBePaid) + Number(totalgrid[index].FCSS_FineAmount);
                    }
                    //MB For Fine
                }
                else if ((Number(totalgrid[index].FCSS_TotalCharges) <= Number(totalgrid[index].FCSS_ToBePaid)) && Number(totalgrid[index].FCSS_TotalCharges) > 0) {
                    swal("Entered Amount is greater than Netamount");
                    totalgrid[index].FCSS_ToBePaid = Number(totalgrid[index].FCSS_TotalCharges);
                    $scope.curramount = Number(totalgrid[index].FCSS_TotalCharges) + Number(totalgrid[index].FCSS_OBArrearAmount);
                    if (totalgrid[index].fmH_Flag == 'F') {
                        $scope.totalfine = Number(totalgrid[index].FCSS_TotalCharges);
                    }
                }
                else if (Number(totalgrid[index].FCSS_TotalCharges) == 0) {
                    $scope.curramount = Number(totalgrid[index].FCSS_ToBePaid) + Number(totalgrid[index].FCSS_OBArrearAmount);
                    if (totalgrid[index].fmH_Flag == 'F') {
                        $scope.totalfine = Number(totalgrid[index].FCSS_ToBePaid);
                    }
                }
            }
            else if (count > 1) {
                if (Number(totalgrid[index].FCSS_TotalCharges) >= Number(totalgrid[index].FCSS_ToBePaid)) {
                    angular.forEach($scope.totalgrid, function (user) {
                        if (!!user.isSelected) {
                            intertobepaidamt = Number(intertobepaidamt) + Number(user.FCSS_ToBePaid) + Number(user.FCSS_OBArrearAmount);
                        }
                    })
                    $scope.curramount = intertobepaidamt;
                    if (totalgrid[index].fmH_Flag == 'F') {
                        $scope.totalfine = Number(user.FCSS_ToBePaid);
                    }
                }
                else if ((Number(totalgrid[index].FCSS_TotalCharges) <= Number(totalgrid[index].FCSS_ToBePaid)) && Number(totalgrid[index].FCSS_TotalCharges)) {
                    swal("Entered Amount is greater than Netamount");
                    totalgrid[index].FCSS_ToBePaid = Number(totalgrid[index].FCSS_TotalCharges);
                    angular.forEach($scope.totalgrid, function (user) {
                        if (!!user.isSelected) {
                            intertobepaidamt = Number(intertobepaidamt) + Number(user.FCSS_ToBePaid);
                        }
                    })
                    $scope.curramount = intertobepaidamt;
                    if (totalgrid[index].FMH_Flag == 'F') {
                        $scope.totalfine = Number(user.FCSS_ToBePaid);
                    }
                }
                else if (Number(totalgrid[index].FCSS_TotalCharges) == 0) {
                    angular.forEach($scope.totalgrid, function (user) {
                        if (!!user.isSelected) {
                            intertobepaidamt = Number(intertobepaidamt) + Number(user.FCSS_ToBePaid);
                        }
                    })
                    $scope.curramount = intertobepaidamt;
                    if (totalgrid[index].FMH_Flag == 'F') {
                        $scope.totalfine = Number(user.FCSS_ToBePaid);
                    }
                }
            }


            $scope.calculatepayableamount();
            //})
        };


        $scope.concessionamt = function (totalgrid, index) {
            var count = 0;
            angular.forEach($scope.totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })
            if (count <= 1) {
                if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) <= Number(totalgrid[index].totalamount)) {
                    $scope.totalconcession = Number(totalgrid[index].fsS_ConcessionAmount);
                }
                else if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) > Number(totalgrid[index].totalamount)) {
                    swal("Entered amount is greater than Net Amount");
                }
            }

            else if (count > 1) {
                if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) <= Number(totalgrid[index].totalamount)) {
                    var interconcessionamt = 0
                    angular.forEach($scope.totalgrid, function (user) {
                        if (!!user.isSelected) {
                            interconcessionamt = Number(interconcessionamt) + Number(user.fsS_ConcessionAmount);
                        }
                    })
                    $scope.totalconcession = interconcessionamt
                }
                else if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) > Number(totalgrid[index].totalamount)) {
                    swal("Entered amount is greater than Net Amount");
                }
            }
        };
        $scope.fineamt = function (totalgrid, index) {
            var count = 0;
            angular.forEach($scope.totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })
            if (count <= 1) {

                $scope.totalfine = Number(totalgrid[index].fsS_FineAmount);

                $scope.curramount = $scope.curramount + $scope.totalfine
            }
            else if (count > 1) {
                var interfineamt = 0;
                interfineamt = Number(interfineamt) + Number($scope.totalgrid[index].fsS_FineAmount);
                $scope.totalfine = interfineamt + $scope.totalfine;
                $scope.curramount = $scope.curramount + interfineamt;
            }
        };
        $scope.toggleAllgrouporterm = function (groupcount, obj) {
            $scope.PayingAmount = null;
            $scope.all = false;
            $scope.curramount = 0;
            var toggleStatus = obj.allgrouporterm;
            angular.forEach($scope.groupcount, function (itm) {
                if (!itm.disablepaisterms)
                    itm.selected = toggleStatus;
            });
            if (obj.allgrouporterm == true) {
                var gouportermcount = "0";
                for (var i = 0; i < groupcount.length; i++) {
                    gouportermcount = gouportermcount + ',' + groupcount[i].fmG_Id;
                }
                var data = {
                    "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                    "multiplegroups": gouportermcount,
                    "FYP_ReceiptDate": new Date($scope.FYP_ReceiptDate).toDateString(),
                    "filterinitialdata": $scope.filterdata,
                    "configset": grouporterm,
                    "ASMAY_Id": $scope.cfg.ASMAY_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CollegeFeeTransaction/getgroupmappedheadsnew", data).
                    then(function (promise) {

                        if (promise.bankname.length > 0) {
                            $scope.bankmaster = promise.bankname;
                        }
                        //MB for Challan
                        if ($scope.filterdata == 'Challan_No') {
                            $scope.grigview1 = true;
                            $scope.totalgrid = temp_alldata;
                            var addfinetonetamount = 0, addnetamount = 0;
                            for (var i = 0; i < $scope.totalgrid.length; i++) {
                                addfinetonetamount = $scope.totalgrid[i].FCSS_FineAmount;
                                addnetamount = $scope.totalgrid[i].FCSS_TotalCharges;
                                if (Number(addfinetonetamount) > 0) {
                                    $scope.totalgrid[i].FCSS_TotalToBePaidaddfine = Number(addfinetonetamount) + Number(addnetamount);
                                }
                                else {
                                    $scope.totalgrid[i].FCSS_TotalToBePaidaddfine = Number(addnetamount);
                                }
                            }
                            var totalamt = 0;
                            var balanceamt = 0;
                            var concessionaamt = 0;
                            var fineamt = 0;

                            angular.forEach($scope.totalgrid, function (value, key) {
                                if (value.amcsT_Id == 0) {
                                    totalamt = totalamt + value.totalamount;
                                    balanceamt = balanceamt + value.fsS_ToBePaid;
                                    concessionaamt = concessionaamt + value.fsS_ConcessionAmount;
                                    fineamt = fineamt + value.fsS_FineAmount;
                                }
                            })

                            $scope.totalfee = totalamt;
                            // $scope.currbalance = balanceamt;

                            $scope.all = true;
                            $scope.toggleAll($scope.all);
                        }
                        else {
                            //MB for Challan
                            if (promise.alldata.length > 0) {
                                $scope.grigview1 = true;

                                //MB for Fine
                                angular.forEach(promise.alldata, function (ed) {
                                    if (ed.fmH_Flag == 'F') {
                                        $scope.temp_finehead_amt = ed.fsS_ToBePaid;
                                        $scope.temp_finehead_amt_total = ed.fsS_TotalToBePaid + ed.fsS_FineAmount;
                                    }

                                })
                                $scope.temp_Fine_Amounts = promise.fine_FCMAS_Ids;
                                //added on 21/10/2021//
                                $scope.temp_Fine_Amountsadv = promise.fine_FCMAS_IdsAdvance;
                                        //added on 21/10/2021//
                                //MB for Fine
                                $scope.highestcountgid = promise.validationgroupid;
                                $scope.totalgrid = promise.alldata;
                                angular.forEach($scope.totalgrid, function (uy) {
                                    uy.Head_Flag = 'H';
                                    uy.tobepaid_M = uy.FCSS_ToBePaid;
                                })


                                var addfinetonetamount = 0, addnetamount = 0, totalpayableamount;

                                for (var i = 0; i < $scope.totalgrid.length; i++) {
                                    addfinetonetamount = $scope.totalgrid[i].FCSS_FineAmount;
                                    addnetamount = $scope.totalgrid[i].FCSS_TotalCharges;
                                    if (Number(addfinetonetamount) > 0) {
                                        $scope.totalgrid[i].FCSS_TotalToBePaidaddfine = Number(addfinetonetamount) + Number(addnetamount);
                                    }
                                    else {
                                        $scope.totalgrid[i].FCSS_TotalToBePaidaddfine = Number(addnetamount);
                                    }
                                }

                                var totalamt = 0;
                                var balanceamt = 0;
                                var concessionaamt = 0;
                                var fineamt = 0;

                                angular.forEach($scope.totalgrid, function (value, key) {
                                    // if (value.amcsT_Id == 0) {
                                    totalamt = totalamt + value.FCSS_NetAmount;
                                    balanceamt = balanceamt + value.FCSS_ToBePaid;
                                    concessionaamt = concessionaamt + value.fsS_ConcessionAmount;
                                    fineamt = fineamt + value.fsS_FineAmount;
                                    //  }
                                })
                                $scope.totalfee = totalamt;
                                $scope.currbalance = balanceamt;

                            }
                            else {
                                // swal("Student has not mapped with any Package / Student has paid amount for that term")
                                swal("Student has paid amount for that Group")
                            }
                            //MB for Challan
                        }

                        if (promise.fetchmodeofpayment.length > 0) {
                            $scope.modeofpayment = promise.fetchmodeofpayment;
                            $scope.diablemodeofpayment = false;
                        }
                        //MB for Challan
                    }
                    )
            }
            else {
                $scope.grigview1 = false;
                $scope.totalgrid = "";
            }
        }

        $scope.toggleAll = function (allchkdata) {
            debugger;
            $scope.disablefine = true;
            $scope.disablenetamount = true;
            $scope.disableconcession = true;
            var toggleStatus = $scope.all;
            //$scope.curramount = 0;
            //$scope.totalconcession = 0;
            //$scope.totalfine = 0;
            //$scope.totalwaived = 0;
            angular.forEach($scope.totalgrid, function (itm) {
                itm.isSelected = toggleStatus;
            });

            if (allchkdata == true) {
                for (var index = 0; index < $scope.totalgrid.length; index++) {
                    if ($scope.totalgrid[index].fmH_Flag != "F") {
                        $scope.totalconcession = Number($scope.totalconcession) + Number($scope.totalgrid[index].FCSS_ConcessionAmount);
                        $scope.totalfine = Number($scope.totalfine) + Number($scope.totalgrid[index].FCSS_FineAmount);

                        $scope.curramount = Number($scope.curramount) + Number($scope.totalgrid[index].FCSS_ToBePaid) + Number($scope.totalgrid[index].FCSS_OBArrearAmount);
                        $scope.totalwaived = Number($scope.totalwaived) + Number($scope.totalgrid[index].FCSS_WaivedAmount);
                    }
                }
            }
            else {
                //$scope.totalconcession = 0;
                //$scope.totalfine = 0;
                //$scope.curramount = 0;
                //$scope.totalwaived = 0;
            }
            for (var index = 0; index < $scope.totalgrid.length; index++) {
                if ($scope.totalgrid.isSelected == true) {
                    $scope.curramount = Number($scope.curramount) + Number($scope.totalgrid[index].FCSS_ToBePaid);
                }
            }
            //MB for Fine
            if (autoreceipt == 1) {
                $scope.get_grp_reptno();
            }
            else {
                if (fineapplicable == true) {
                    $scope.calculate_fine();
                }
            }
            //MB for Fine
            if (allchkdata == true) {
                for (var index = 0; index < $scope.totalgrid.length; index++) {
                    if ($scope.totalgrid[index].FMH_FeeName == "Fine" || $scope.totalgrid[index].FMH_Flag == "F") {
                        $scope.curramount = $scope.curramount + Number($scope.totalgrid[index].FCSS_ToBePaid)
                    }
                }
            }
            //MB
            $scope.get_modes();

            $scope.calculatepayableamount();
        }

        $scope.heads1 = [];
        $scope.get_grp_reptno = function (index) {
            
            $scope.heads1 = [];

            if ($scope.all == true) {
                angular.forEach($scope.totalgrid, function (student) {
                    $scope.heads1.push(student);
                });
            } else {
                angular.forEach($scope.totalgrid, function (student) {
                    if (student.isSelected == true) {
                        $scope.heads1.push(student);
                    }
                });
            }

            //added on 21/10/2021//

            if ($scope.adv == true) {
                angular.forEach($scope.totalgridadvance, function (student) {
                    $scope.heads1.push(student);
                });
            } else {
                angular.forEach($scope.totalgridadvance, function (student) {
                    if (student.isSelected1 == true) {
                        $scope.heads1.push(student);
                    }
                });
            }

                    //added on 21/10/2021//

            var data = {
                "auto_receipt_flag": autoreceipt,
                temp_head_list: $scope.heads1,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeFeeTransaction/get_grp_reptno", data).then(function (promise) {
                $scope.grpcount = promise.grp_count;
                $scope.FYP_ReceiptNo = promise.fyP_ReceiptNo;
                if (promise.grp_count != 0 && (promise.grp_count != 1 || promise.grp_count > 1)) {
                    swal("Can't Do Transaction For Two Fee Groups At A Time !!!!!!");
                    //MB
                    if ($scope.all == false) {
                        var totalgrid = trp[0].total_grid;
                        var index = trp[0].in_dex;
                        var userdata = trp[0].user_data;

                        angular.forEach($scope.totalgrid, function (itm) {
                            if (itm.fmH_Id == userdata.fmH_Id) {
                                itm.isSelected = false;
                            }
                        });

                        $scope.all = $scope.totalgrid.every(function (itm) {
                            return itm.isSelected;
                        });

                        if (totalgrid[index].isSelected == true) {
                            debugger;
                            $scope.totalconcession = Number($scope.totalconcession) + Number(totalgrid[index].fsS_ConcessionAmount);
                            $scope.totalfine = Number($scope.totalfine) + Number(totalgrid[index].fsS_FineAmount);
                            //$scope.totalfine =  Number(totalgrid[index].fsS_FineAmount);
                            $scope.curramount = Number($scope.curramount) + Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_FineAmount);
                            $scope.totalwaived = Number($scope.totalwaived) + Number(totalgrid[index].fsS_WaivedAmount);
                        }
                        else if (totalgrid[index].isSelected == false) {
                            debugger;
                            $scope.totalconcession = Number($scope.totalconcession) - Number(totalgrid[index].fsS_ConcessionAmount);
                            ////$scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                            $scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                            // $scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                            $scope.curramount = Number($scope.curramount) - Number(totalgrid[index].fsS_ToBePaid) - Number(totalgrid[index].fsS_FineAmount);
                            $scope.totalwaived = Number($scope.totalwaived) - Number(totalgrid[index].fsS_WaivedAmount);
                        }

                        //debugger;
                        //if ($scope.all == false) {
                        //    $scope.amtdetails(trp[0].user_data, trp[0].total_grid, trp[0].in_dex);
                        //}

                        //added on 21/10/2021//

                        var totalgridadv = trp[0].total_grid;
                        var indexadv = trp[0].in_dex;
                        var userdataadv = trp[0].user_data;
                        angular.forEach($scope.totalgridadvance, function (itm) {
                            if (itm.fmH_Id == userdata.fmH_Id) {
                                itm.isSelected1 = false;
                            }

                        });

                        $scope.all = $scope.totalgridadvance.every(function (itm) {
                            return itm.isSelected1;
                        });

                        if ($scope.totalgridadvance[index].isSelected == true) {
                            debugger;
                            $scope.totalconcession = Number($scope.totalconcession) + Number($scope.totalgridadvance[index].fsS_ConcessionAmount);
                            $scope.totalfine = Number($scope.totalfine) + Number($scope.totalgridadvance[index].fsS_FineAmount);
                            //$scope.totalfine =  Number(totalgrid[index].fsS_FineAmount);
                            $scope.curramount = Number($scope.curramount) + Number($scope.totalgridadvance[index].fsS_ToBePaid) + Number($scope.totalgridadvance[index].fsS_FineAmount);
                            $scope.totalwaived = Number($scope.totalwaived) + Number($scope.totalgridadvance[index].fsS_WaivedAmount);
                        }
                        else if ($scope.totalgridadvance[index].isSelected1 == false) {
                            debugger;
                            $scope.totalconcession = Number($scope.totalconcession) - Number($scope.totalgridadvance[index].fsS_ConcessionAmount);
                            ////$scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                            $scope.totalfine = Number($scope.totalfine) - Number($scope.totalgridadvance[index].fsS_FineAmount);
                            // $scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                            $scope.curramount = Number($scope.curramount) - Number($scope.totalgridadvance[index].fsS_ToBePaid) - Number($scope.totalgridadvance[index].fsS_FineAmount);
                            $scope.totalwaived = Number($scope.totalwaived) - Number($scope.totalgridadvance[index].fsS_WaivedAmount);
                        }

                                    //added on 21/10/2021//

                        trp = [];
                    }
                    else if ($scope.all == true) {
                        //$scope.all = false;
                        $scope.disablefine = true;
                        $scope.disablenetamount = true;
                        $scope.disableconcession = true;

                        var allchkdata = $scope.all;
                        var toggleStatus = $scope.all;

                        $scope.curramount = 0;
                        $scope.totalconcession = 0;
                        $scope.totalfine = 0;
                        $scope.totalwaived = 0;

                        //angular.forEach($scope.totalgrid, function (itm) {
                        //    itm.isSelected = toggleStatus;
                        //});

                        if (allchkdata == true) {

                            for (var index = 0; index < $scope.totalgrid.length; index++) {
                                $scope.totalconcession = Number($scope.totalconcession) + Number($scope.totalgrid[index].fsS_ConcessionAmount);
                                $scope.totalfine = Number($scope.totalfine) + Number($scope.totalgrid[index].fsS_FineAmount);
                                $scope.curramount = Number($scope.curramount) + Number($scope.totalgrid[index].fsS_ToBePaid) + $scope.totalfine;
                                $scope.totalwaived = Number($scope.totalwaived) + Number($scope.totalgrid[index].fsS_WaivedAmount);
                            }

                        }
                        else {
                            $scope.totalconcession = 0;
                            $scope.totalfine = 0;
                            $scope.curramount = 0;
                            $scope.totalwaived = 0;
                        }

                        angular.forEach($scope.totalgrid, function (itm) {
                            if (itm.fmG_Id == $scope.highestcountgid) {
                                itm.isSelected = true;
                            }
                            else {
                                itm.isSelected = false;
                            }
                        });

                        var payableamt = 0, concamt = 0, fneamt = 0;
                        angular.forEach($scope.totalgrid, function (iitm1) {
                            if (iitm1.isSelected == true) {
                                payableamt = payableamt + Number(iitm1.fsS_ToBePaid);
                                concamt = concamt + Number(iitm1.fsS_ConcessionAmount);
                                fneamt = fneamt + Number(iitm1.fsS_FineAmount);
                            }
                        });

                        $scope.curramount = Number(payableamt);
                        $scope.totalconcession = Number(concamt);
                        $scope.totalfine = Number(fneamt);


                    }
                    //MB
                }
                //MB for Fine
                if (fineapplicable == true) {
                    $scope.calculate_fine();
                }
                //MB for Fine

                $scope.calculatepayableamount();
            })
        }

        var trp = [];
        $scope.amtdetails = function (userdata, totalgrid, index) {
            trp = [];
            var newCol = "";
            newCol = {
                user_data: userdata, total_grid: totalgrid, in_dex: index
            }
            trp.push(newCol);
            $scope.disablefine = true;
            $scope.disablenetamount = true;
            $scope.all = $scope.totalgrid.every(function (itm) {
                return itm.isSelected;
            });
            if (totalgrid[index].isSelected == true) {
                $scope.totalconcession = Number($scope.totalconcession) + Number(totalgrid[index].FCSS_ConcessionAmount);
                $scope.totalfine = Number($scope.totalfine) + Number(totalgrid[index].FCSS_FineAmount);
                $scope.curramount = Number($scope.curramount) + Number(totalgrid[index].FCSS_ToBePaid) + Number(totalgrid[index].FCSS_FineAmount) + Number(totalgrid[index].FCSS_OBArrearAmount);
                $scope.totalwaived = Number($scope.totalwaived) + Number(totalgrid[index].FCSS_WaivedAmount);
                if (totalgrid[index].fmH_FeeName != "Fine") {
                    //$scope.currbalance =  $scope.curramount;
                }
            }
            else if (totalgrid[index].isSelected == false) {
                $scope.totalconcession = Number($scope.totalconcession) - Number(totalgrid[index].FCSS_ConcessionAmount);
                $scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].FCSS_FineAmount);
                $scope.curramount = Number($scope.curramount) - Number(totalgrid[index].FCSS_ToBePaid) - Number(totalgrid[index].FCSS_FineAmount) - Number(totalgrid[index].FCSS_OBArrearAmount);
                $scope.totalwaived = Number($scope.totalwaived) - Number(totalgrid[index].FCSS_WaivedAmount);
                if (totalgrid[index].fmH_FeeName != "Fine") {
                    // $scope.currbalance = $scope.curramount;
                }
            }
            //MB for Fine
            //if (fineapplicable == true) {
            //    $scope.calculate_fine();
            //}
            //MB for Fine
            if (autoreceipt == 1) {
                $scope.get_grp_reptno(index);
            }
            $scope.get_modes();

            //$scope.curramount = 0;
            //angular.forEach($scope.totalgrid, function (aa) {
            //    if (aa.isSelected == true) {
            //        $scope.curramount = aa.FCSS_ToBePaid
            //    }
            //    $scope.curramount += $scope.totalfine;
            //});

            $scope.calculatepayableamount();

        };

        $scope.onselectmodeofpayment = function (ivrmmoD_ModeOfPayment_Code) {
            var data = {
                //"modeofpayment": $scope.FYP_Bank_Or_Cash,
                "modeofpayment": ivrmmoD_ModeOfPayment_Code,
                "filterinitialdata": $scope.filterdata
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            $scope.FYP_Bank_Or_Cash = ivrmmoD_ModeOfPayment_Code;

            if ($scope.FYP_Bank_Or_Cash == 'C') {
            }
            else if ($scope.FYP_Bank_Or_Cash == 'B' || $scope.FYP_Bank_Or_Cash == 'R' || $scope.FYP_Bank_Or_Cash == 'S' || $scope.FYP_Bank_Or_Cash == 'U') {
                $scope.bankdetails = true;
            }
            else {
                $scope.groupcount = [];
                $scope.cfg.ASMAY_Id = "";
                $scope.amcsT_Id = "";
                $scope.grigview1 = false;
            }

            if ($scope.FYP_Bank_Or_Cash == 'C') {
                $scope.bankdetails = false;
            }
            else if ($scope.FYP_Bank_Or_Cash == 'B') {
                $scope.bankdetails = true;
            }
        };

        $scope.closemodel = function (userpdc) {
            $scope.FYP_DD_Cheque_Date = new Date(userpdc.fcspdC_ChequeDate);
            $scope.FYPPM_ClearanceDate = new Date(userpdc.fcspdC_ChequeDate);
            $scope.FYP_DD_Cheque_No = userpdc.fcspdC_ChequeNo;
            $scope.FMBANK_Id = userpdc.fmbanK_BankName;
            $scope.fmbanK_BankName = userpdc.fmbanK_BankName;
            $scope.FYP_Remarks = userpdc.fcspdC_Narration;
            $('#myPDCModaldetails').modal('hide');

            $scope.disdddate = true;
            $scope.disddno = true;
            $scope.disbankname = true;

        }

        $scope.route_name = "";

        $scope.onselectstudent = function (studentlst) {
            var studid = studentlst.amcsT_Id;
            $scope.grigview1 = false;
            $scope.obj.allgrouporterm = false;
            $scope.temptermarray = [];
            var data = {
                "AMCST_Id": studid,
                "filterinitialdata": $scope.filterdata,
                "configset": grouporterm,
                "autoreceiptflag": autoreceipt,
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeFeeTransaction/getstudlistgroup", data).
                then(function (promise) {

                    if (promise.getpdcdetails.length > 0) {
                        $scope.studentpdcdetails = promise.getpdcdetails;
                    }

                    if (promise.fillmastergroup.length > 0) {
                        if (promise.showstaticticsdetails.length > 0) {
                            $scope.studentviewdetails = promise.showstaticticsdetails;
                        }
                        $scope.groupcount = promise.fillmastergroup;
                        $scope.showdetails = promise.fillstudentviewdetails;
                        $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                        if (grouporterm == 'T') {
                            var termsdisable = promise.disableterms;
                            if ($scope.groupcount.length == promise.disableterms.length) {
                                for (var r = 0; r < $scope.groupcount.length; r++) {
                                    if (promise.disableterms[r].fcsS_NetAmount <= promise.disableterms[r].fcsS_PaidAmount && $scope.groupcount[r].fmG_Id == promise.disableterms[r].fmT_Id) {
                                        $scope.groupcount[r].disablepaisterms = true;

                                        $scope.groupcount[r].selected = false;
                                    }
                                    else {
                                        $scope.groupcount[r].disablepaisterms = false;
                                        $scope.groupcount[r].selected = false;
                                    }
                                }
                            }
                        }
                        else if (grouporterm == 'G') {
                            var termsdisable = promise.disableterms;
                            if ($scope.groupcount.length == promise.disableterms.length) {
                                for (var r = 0; r < $scope.groupcount.length; r++) {
                                    if (promise.disableterms[r].fcsS_NetAmount <= promise.disableterms[r].fcsS_PaidAmount && $scope.groupcount[r].fmG_Id == promise.disableterms[r].fmT_Id) {
                                        $scope.groupcount[r].disablepaisterms = true;

                                        $scope.groupcount[r].selected = false;
                                    }
                                    else {
                                        $scope.groupcount[r].disablepaisterms = false;
                                        $scope.groupcount[r].selected = false;
                                    }
                                }
                            }
                        }
                        $scope.alltermchk = $scope.groupcount.every(function (options) {
                            return options.disablepaisterms;
                        });
                        if (promise.trmR_RouteName != null) {
                            $scope.route_name = promise.trmR_RouteName;
                        }
                        else {
                            $scope.route_name = "";
                        }
                        $scope.amcsT_FirstName = promise.fillstudent[0].amcsT_FirstName;
                        $scope.amcsT_MiddleName = promise.fillstudent[0].amcsT_MiddleName;
                        $scope.amcsT_LastName = promise.fillstudent[0].amcsT_LastName;
                        $scope.amcsT_AdmNo = promise.fillstudent[0].amcsT_AdmNo;
                        $scope.amcsT_RegistrationNo = promise.fillstudent[0].amcsT_RegistrationNo;
                        $scope.acysT_RollNo = promise.fillstudent[0].acysT_RollNo;
                        $scope.amcO_CourseName = promise.fillstudent[0].amcO_CourseName;
                        $scope.amsE_SEMName = promise.fillstudent[0].amsE_SEMName;
                        $scope.amB_BranchName = promise.fillstudent[0].amB_BranchName;
                        $scope.amcsT_MobileNo = promise.fillstudent[0].amcsT_MobileNo;
                        $scope.amcsT_FatherName = promise.fillstudent[0].amcsT_FatherName;
                        $scope.amcsT_DOB = promise.fillstudent[0].amcsT_DOB;
                        $scope.fathername = promise.fillstudent[0].fathername;

                        $scope.grigview1 = false;
                        $scope.totalgrid = "";
                        if (autoreceipt == "1") {
                            $scope.recchkbox = true;
                            $scope.FYP_ReceiptNo = promise.fyP_ReceiptNo;
                        }
                        else {
                            $scope.recchkbox = false;
                        }
                        //MB for Challan
                        if ($scope.filterdata == 'Challan_No') {
                            $scope.groupcount = temp_fillmastergroup;
                            $scope.obj.allgrouporterm = true;
                            $scope.toggleAllgrouporterm($scope.groupcount, $scope.obj);
                        }
                        //MB for Challan

                        ///Praveen Added for display the amount for student selection
                        $scope.curcharge = 0;
                        $scope.curpaid = 0;
                        $scope.curoutstd = 0;

                        angular.forEach($scope.studentviewdetails, function (ll) {
                            $scope.curcharge += ll.fcsS_CurrentYrCharges;
                            $scope.curpaid += ll.fcsS_PaidAmount;
                            $scope.curoutstd += ll.fcsS_ToBePaid;

                        })

                    }
                    else {
                        swal("Kindly map student with group in student fee group Mapping form!!!")
                    }
                })
        };

        $scope.nextduedate = true;

        $scope.showpaiddetails = function () {
            var data = {
                "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                "filterinitialdata": $scope.filterdata,
                "configset": grouporterm,
                "autoreceiptflag": autoreceipt,
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeFeeTransaction/getstudlistgroup", data).
                then(function (promise) {
                    $scope.showdetails = promise.showstudetails;
                })
        };

        $scope.bankdet = false;
        $scope.showmodaldetails = function (fypid, studid) {
            $scope.AMCO_CourseName = "";
            $scope.AMB_BranchName = "";
            $scope.ACYST_RollNo = "";
            $scope.AMCST_AdmNo = "";
            $scope.AMCST_RegistrationNo = "";
            $scope.AMCST_FirstName = "";
            $scope.curdate = "";
            $scope.asmaY_Year = "";
            $scope.AMCST_FatherName = "";
            $scope.AMCST_MotherName = "";
            $scope.receiptno = "";
            $scope.FMCC_ConcessionName = "";
            var data = {
                "AMCST_Id": studid,
                "filterinitialdata": $scope.filterdata,
                "configset": grouporterm,
                "FYP_Id": fypid,
                "minstall": mergeinstallment,
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeFeeTransaction/printreceipt", data).
                then(function (promise) {
                    $scope.htmldata = promise.htmldata;
                    $scope.payment_mode_details = promise.paymentmode_details;
                    $scope.atotAadvance = 0;

                    $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                    $scope.showdetailsreceiptadvance = promise.fillstudentviewdetailsadvance;
                    angular.forEach($scope.showdetailsreceiptadvance, function (obj) {
                        $scope.atotAadvance += obj.ftcP_PaidAmount
                    })

                    $scope.totchags = 0;
                    $scope.atotA = 0;
                    angular.forEach($scope.showdetailsreceipt, function (ft) {
                        $scope.totchags += Number(ft.fcsS_TotalCharges);
                        $scope.atotA += Number(ft.ftcP_PaidAmount);

                    })

                    $scope.username = promise.username;

                    if (promise.approvedbyname != undefined) {
                        if (promise.approvedbyname.length > 0) {
                            $scope.approvedby = promise.approvedbyname[0].EmpName;
                            $scope.feerectype = "FEE"
                        }
                        else {
                            $scope.approvedby = "";
                            $scope.feerectype = "PROVISIONAL"
                        }
                    }
                    else {
                        $scope.approvedby = "";
                        $scope.feerectype = "PROVISIONAL"
                    }

                    if (promise.htmldata != "") {
                        $scope.period = promise.duration;
                        $scope.paymenrgrid = promise.currpaymentdetails;
                        //      $scope.atotA = promise.currpaymentdetails[0].ftcP_PaidAmount;
                        $scope.ctotA = promise.currpaymentdetails[0].ftcP_ConcessionAmount;


                        //  $scope.totchar = $scope.atotA + $scope.ctotA;

                        $scope.overalltot = $scope.atotA + $scope.atotAadvance;

                        if (promise.currpaymentdetails[0].fyP_TransactionTypeFlag == "B") {
                            $scope.modeofpayment = "Bank";
                        }
                        else if (promise.currpaymentdetails[0].fyP_TransactionTypeFlag == "C") {
                            $scope.modeofpayment = "Cash";
                        }
                        if (promise.currpaymentdetails[0].fyP_TransactionTypeFlag == "R") {
                            $scope.modeofpayment = "RTGS";
                        }
                        if (promise.currpaymentdetails[0].fyP_TransactionTypeFlag == "S") {
                            $scope.modeofpayment = "Swipe";
                        }
                        if (promise.currpaymentdetails[0].fyP_TransactionTypeFlag == "O") {
                            $scope.modeofpayment = "Online";
                        }

                        $scope.FYP_TransactionTypeFlag = promise.currpaymentdetails[0].fyP_TransactionTypeFlag;

                        $scope.FYP_Remarks = promise.fillstudentviewdetails[0].fyP_Remarks;
                        $scope.FYP_PaymentReference_Id = promise.currpaymentdetails[0].fyppM_PaymentReference_Id;

                        $scope.overalltot = $scope.atotA + $scope.atotAadvance;

                        $scope.words = $scope.amountinwords($scope.overalltot);
                        $scope.year = promise.year;
                        $scope.due_amount = promise.dueamount;
                        if ($scope.due_amount == 0) {
                            $scope.date = "";
                            $scope.nextduedate = false;
                        }
                        else {
                            $scope.date = promise.date;
                            $scope.nextduedate = true;
                        }

                        if ($scope.due_amount == 0) {
                            $scope.months = "";
                        }
                        else {
                            $scope.months = promise.month;

                            $scope.nextduedate = true;
                        }
                        var termname = " ";
                        if (promise.termremarks.length > 0) {
                            for (var i = 0; i < promise.termremarks.length; i++) {
                                if (termname == " ") {
                                    termname = promise.termremarks[i].fmT_Name;
                                }
                                else {
                                    termname = termname + ',' + promise.termremarks[i].fmT_Name;
                                }
                            }
                        }

                        var feeheadname = "";
                        var validation;
                        $scope.tempreceiptarraytermexfinal = [];
                        if (receiptformat == 1 || receiptformat == 0) {
                            $scope.tempreceiptarray = [];
                            $scope.tempreceiptarrayterm = {
                            };
                            $scope.tempreceiptarraytermex = {
                            };
                            var totalamount = 0, concessionamt = 0, fineamt = 0, feecount = 0, fmH_FeeName, feetotcharges = 0;
                            var totalamountex = 0, concessionamtex = 0, fineamtex = 0, fmH_FeeNameex, feetotchargesex = 0;
                            if (promise.fillstudentviewdetails.length > 0) {
                                for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {
                                    var mainheadid = promise.fillstudentviewdetails[i].fmH_Id;
                                    var maininstallmentid = promise.fillstudentviewdetails[i].ftI_Id;
                                    if (promise.receiptformathead.length > 0) {
                                        for (var j = 0; j < promise.receiptformathead.length; j++) {
                                            var subheadid = promise.receiptformathead[j].fmH_Id;
                                            var subinstid = promise.receiptformathead[j].ftI_Id;

                                            if (mainheadid == subheadid && maininstallmentid == subinstid) {
                                                feecount = Number(feecount) + 1;

                                                feetotcharges = Number(feetotcharges) + Number(promise.receiptformathead[j].fcsS_TotalCharges);

                                                fmH_FeeName = promise.receiptformathead[j].fmH_FeeName;
                                                totalamount = Number(totalamount) + Number(promise.receiptformathead[j].ftcP_PaidAmount);
                                                concessionamt = Number(concessionamt) + Number(promise.receiptformathead[j].ftcP_ConcessionAmount);
                                                fineamt = Number(fineamt) + Number(promise.receiptformathead[j].ftcP_FineAmount);
                                            }
                                        }
                                        if (feecount < 1) {

                                            fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName
                                            if (feeheadname == "") {
                                                feetotchargesex = Number(promise.fillstudentviewdetails[i].fcsS_TotalCharges);
                                                totalamountex = Number(promise.fillstudentviewdetails[i].ftcP_PaidAmount);
                                                concessionamtex = Number(promise.fillstudentviewdetails[i].ftcP_ConcessionAmount);
                                                fineamtex = Number(promise.fillstudentviewdetails[i].ftcP_FineAmount);
                                            }

                                            else if (fmH_FeeNameex === feeheadname) {
                                                feetotchargesex = Number(feetotchargesex) + Number(promise.fillstudentviewdetails[i].fcsS_TotalCharges);
                                                totalamountex = Number(totalamountex) + Number(promise.fillstudentviewdetails[i].ftcP_PaidAmount);
                                                concessionamtex = Number(concessionamtex) + Number(promise.fillstudentviewdetails[i].ftcP_ConcessionAmount);
                                                fineamtex = Number(fineamtex) + Number(promise.fillstudentviewdetails[i].ftcP_FineAmount);
                                            }
                                            else {
                                                validation = "add";
                                                $scope.tempreceiptarraytermex = {
                                                    fmH_FeeName: feeheadname,
                                                    ftcP_PaidAmount: totalamountex,
                                                    ftcP_ConcessionAmount: concessionamtex,
                                                    ftcP_FineAmount: fineamtex,
                                                    fcsS_TotalCharges: feetotchargesex,
                                                };
                                                if ($scope.tempreceiptarraytermexfinal.length > 0) {
                                                    var aldy_cnt = 0;
                                                    angular.forEach($scope.tempreceiptarraytermexfinal, function (obj) {
                                                        if (feeheadname == obj.fmH_FeeName) {
                                                            aldy_cnt += 1;
                                                        }
                                                    }
                                                    )
                                                    if (aldy_cnt == 0) {
                                                        $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                    }
                                                }
                                                else if ($scope.tempreceiptarraytermexfinal.length == 0) {
                                                    $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                }
                                                fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName
                                                totalamountex = Number(promise.fillstudentviewdetails[i].ftcP_PaidAmount);
                                                concessionamtex = Number(promise.fillstudentviewdetails[i].ftcP_ConcessionAmount);
                                                fineamtex = Number(promise.fillstudentviewdetails[i].ftcP_FineAmount);
                                                feetotchargesex = Number(promise.fillstudentviewdetails[i].fcsS_TotalCharges);
                                                $scope.tempreceiptarraytermex = {
                                                    fmH_FeeName: fmH_FeeNameex,
                                                    ftcP_PaidAmount: totalamountex,
                                                    ftcP_ConcessionAmount: concessionamtex,
                                                    ftcP_FineAmount: fineamtex,
                                                    fcsS_TotalCharges: feetotchargesex,
                                                };
                                                if ($scope.tempreceiptarraytermexfinal.length > 0) {
                                                    var aldy_cnt = 0;
                                                    angular.forEach($scope.tempreceiptarraytermexfinal, function (obj) {
                                                        if (fmH_FeeNameex == obj.fmH_FeeName) {
                                                            aldy_cnt += 1;
                                                        }
                                                    }
                                                    )
                                                    if (aldy_cnt == 0) {
                                                        $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                    }
                                                }
                                                else if ($scope.tempreceiptarraytermexfinal.length == 0) {
                                                    $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                }
                                            }
                                            feeheadname = fmH_FeeNameex;
                                        }
                                        feecount = 0;
                                    }
                                    else {
                                        $scope.tempreceiptarray.push(promise.fillstudentviewdetails[i]);
                                    }
                                }
                            }
                            if (promise.receiptformathead.length > 0) {
                                var temlength = $scope.tempreceiptarray.length;

                                angular.forEach($scope.special_head_list, function (sp_hd) {
                                    var count = 0;
                                    var feetotcharges1 = 0, totalamount1 = 0, concessionamt1 = 0, fineamt1 = 0, fmH_FeeName1 = "";
                                    angular.forEach(promise.receiptformathead, function (sh_hd) {
                                        if (sp_hd.fmsfH_Name == sh_hd.fmH_FeeName) {
                                            count += 1;
                                            feetotcharges1 = Number(feetotcharges1) + Number(sh_hd.fcsS_TotalCharges);
                                            fmH_FeeName1 = sh_hd.fmH_FeeName;
                                            totalamount1 = Number(totalamount1) + Number(sh_hd.ftcP_Paid_Amt);
                                            concessionamt1 = Number(concessionamt1) + Number(sh_hd.ftcP_ConcessionAmount);
                                            fineamt1 = Number(fineamt1) + Number(sh_hd.ftP_Fine_Amt);
                                        }
                                    })
                                    if (count > 0) {
                                        $scope.tempreceiptarrayterm = {
                                            fmH_FeeName: fmH_FeeName1,
                                            ftcP_PaidAmount: totalamount1,
                                            ftcP_ConcessionAmount: concessionamt1,
                                            ftcP_FineAmount: fineamt1,
                                            fcsS_TotalCharges: feetotcharges1,
                                        };
                                        console.log($scope.tempreceiptarrayterm);
                                        $scope.tempreceiptarray.push($scope.tempreceiptarrayterm);
                                    }
                                })

                                //MB For Special
                                if (validation != "add") {
                                    $scope.tempreceiptarraytermex = {
                                        fmH_FeeName: fmH_FeeNameex,
                                        ftcP_PaidAmount: totalamountex,
                                        ftcP_ConcessionAmount: concessionamtex,
                                        ftcP_FineAmount: fineamtex,
                                        fcsS_TotalCharges: feetotchargesex,
                                    };
                                    $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                }
                                //MB For Special
                                // $scope.tempreceiptarray.push($scope.tempreceiptarrayterm);
                                //MB For Special
                                for (var r = 0; r < $scope.tempreceiptarraytermexfinal.length; r++) {
                                    if ($scope.tempreceiptarraytermexfinal[r].fmH_FeeName != undefined) {
                                        $scope.tempreceiptarray.push($scope.tempreceiptarraytermexfinal[r]);
                                    }
                                }
                            }
                            $scope.showdetailsreceipt = $scope.tempreceiptarray;
                            $scope.showtotaldetails = promise.filltotaldetails;

                        }
                        else {
                            $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                            $scope.showtotaldetails = promise.filltotaldetails;
                        }
                        //for total charges MB


                        if (promise.fillstudentviewdetails[0].amcsT_FirstName != null && promise.fillstudentviewdetails[0].amcsT_MiddleName != null && promise.fillstudentviewdetails[0].amcsT_LastName != null) {
                            $scope.AMCST_FirstName = promise.fillstudentviewdetails[0].amcsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amcsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amcsT_LastName;
                        }
                        else {
                            if (promise.fillstudentviewdetails[0].AMCST_FirstName == null) {
                                promise.fillstudentviewdetails[0].AMCST_FirstName = ' ';
                            }
                            if (promise.fillstudentviewdetails[0].AMCST_MiddleName == null) {
                                promise.fillstudentviewdetails[0].AMCST_MiddleName = ' ';
                            }
                            if (promise.fillstudentviewdetails[0].AMCST_LastName == null) {
                                promise.fillstudentviewdetails[0].AMCST_LastName = ' ';
                            }
                            $scope.AMCST_FirstName = promise.fillstudentviewdetails[0].amcsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amcsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amcsT_LastName;
                        }
                        $scope.AMCO_CourseName = promise.fillstudentviewdetails[0].amcO_CourseName;
                        $scope.AMB_BranchName = promise.fillstudentviewdetails[0].amB_BranchName;
                        $scope.AMSE_SEMName = promise.fillstudentviewdetails[0].amsE_SEMName;
                        $scope.ACYST_RollNo = promise.fillstudentviewdetails[0].ACYST_RollNo;
                        $scope.AMCST_AdmNo = promise.fillstudentviewdetails[0].amcsT_AdmNo;
                        $scope.AMCST_RegistrationNo = promise.fillstudentviewdetails[0].amcsT_RegistrationNo;
                        $scope.NEET_RollNo = promise.fillstudentviewdetails[0].amcsT_NEETRN;
                        $scope.TRANSAC_DATE = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");


                        //$scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                        for (var i = 0; i < $scope.yearlst.length; i++) {

                            if ($scope.yearlst[i].asmaY_Id == promise.asmaY_Id) {
                                $scope.asmaY_Year = $scope.yearlst[i].asmaY_Year; 
                            }
                        }
                       // $scope.asmaY_Year = promise.asmaY_Year;
                        $scope.AMCST_FatherName = promise.fillstudentviewdetails[0].amcsT_FatherName;
                        $scope.AMCST_MotherName = promise.fillstudentviewdetails[0].amcsT_MotherName;
                        $scope.receiptno = promise.fillstudentviewdetails[0].fyP_ReceiptNo;

                        $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;
                        // $scope.curdate = $filter('date')(new Date(), "dd-MM-yyyy");
                        $scope.curdate = new Date();
                        $scope.Paid_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                        if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "C") {
                            $scope.bankdet = false;
                            // $scope.FYP_ReceiptDate = "--";
                        }
                        if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "O") {
                            $scope.bankdet = false;
                        }
                        if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "B" || promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "R") {
                            $scope.bankdet = true;
                            $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                            //$scope.FYP_DD_Cheque_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                            $scope.FYP_DD_Cheque_Date = promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date;
                            $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                        }

                        var e1 = angular.element(document.getElementById("test"));
                       $compile(e1.html(promise.htmldata))(($scope));
                    }
                    else {
                        $scope.MI_Address1 = promise.masterinstitution[0].mI_Address1;
                        $scope.MI_Address2 = promise.masterinstitution[0].mI_Address2;
                        $scope.MI_Address3 = promise.masterinstitution[0].mI_Address3;
                        $scope.MI_Pincode = promise.masterinstitution[0].mI_Pincode;
                        $scope.receiptno = promise.fillstudentviewdetails[0].fyP_ReceiptNo;
                        //$scope.FYP_ReceiptDate = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                        $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].fyP_ReceiptDate;
                        $scope.AMCST_AdmNo = promise.fillstudentviewdetails[0].amcsT_AdmNo;
                        $scope.AMCST_RegistrationNo = promise.fillstudentviewdetails[0].amcsT_RegistrationNo;
                        $scope.asmaY_Year = promise.asmaY_Year;
                        if (promise.fillstudentviewdetails[0].amcsT_FirstName != null && promise.fillstudentviewdetails[0].amcsT_MiddleName != null && promise.fillstudentviewdetails[0].amcsT_LastName != null) {
                            $scope.AMCST_FirstName = promise.fillstudentviewdetails[0].amcsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amcsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amcsT_LastName;
                        }
                        else {
                            if (promise.fillstudentviewdetails[0].amcsT_FirstName == null) {
                                promise.fillstudentviewdetails[0].amcsT_FirstName = ' ';
                            }
                            if (promise.fillstudentviewdetails[0].amcsT_MiddleName == null) {
                                promise.fillstudentviewdetails[0].amcsT_MiddleName = ' ';
                            }
                            if (promise.fillstudentviewdetails[0].amcsT_LastName == null) {
                                promise.fillstudentviewdetails[0].amcsT_LastName = ' ';
                            }
                            $scope.AMCST_FirstName = promise.fillstudentviewdetails[0].amcsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amcsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amcsT_LastName;
                        }
                        $scope.AMCO_CourseName = promise.fillstudentviewdetails[0].amcO_CourseName;
                        $scope.AMB_BranchName = promise.fillstudentviewdetails[0].amB_BRanchName;
                        $scope.AMCST_FatherName = promise.fillstudentviewdetails[0].amcsT_FatherName;
                        $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;
                        $scope.AMCST_MotherName = promise.fillstudentviewdetails[0].amcsT_MotherName;
                        $scope.period = promise.duration;
                        if ($scope.FMC_RInstallmentsMergeFlag == 1) {
                            $scope.FMC_RInstallmentsFlag = 0;

                            $scope.showdetailsreceipt = promise.filltotaldetails;
                        }
                        else {
                            $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                        }
                        if (promise.fillstudentviewdetails.length > 0) {
                            var fmatotal = 0;
                            var totalpaidamount = 0;
                            angular.forEach(promise.fillstudentviewdetails, function (user) {
                                fmatotal = fmatotal + user.fcsS_TotalCharges;
                                totalpaidamount = totalpaidamount + user.ftcP_PaidAmount;
                            })
                        }
                        $scope.totalfma = fmatotal;
                        if (fmatotal > totalpaidamount)
                            $scope.btotA = fmatotal - totalpaidamount;
                        else
                            $scope.btotA = 0;
                        $scope.atotA = promise.currpaymentdetails[0].ftcP_PaidAmount;
                        $scope.ctotA = promise.currpaymentdetails[0].ftcP_ConcessionAmount;
                        $scope.ftotA = promise.currpaymentdetails[0].ftcP_FineAmount;
                        $scope.wtotA = promise.currpaymentdetails[0].ftP_Waived_Amt;
                        $scope.paymenrgrid = promise.currpaymentdetails;

                        $scope.FYPPM_BankName = $scope.paymenrgrid[0].fyppM_BankName;
                        $scope.FYPPM_DDChequeDate = $scope.paymenrgrid[0].fyppM_DDChequeDate;
                        $scope.FYPPM_DDChequeNo = $scope.paymenrgrid[0].fyppM_DDChequeNo;
                        $scope.FYP_TransactionTypeFlag = $scope.paymenrgrid[0].fyP_TransactionTypeFlag;
                        $scope.FYPPM_Transaction_Id = $scope.paymenrgrid[0].fyppM_Transaction_Id;
                        $scope.FYPPM_PaymentReference_Id = $scope.paymenrgrid[0].fyppM_PaymentReference_Id;

                        $scope.words = $scope.amountinwords(overalltot);
                        $scope.due_amount = promise.dueamount;
                        if ($scope.due_amount == 0) {
                            $scope.date = "";
                            $scope.nextduedate = false;
                        }
                        else {
                            $scope.date = promise.date;
                            $scope.nextduedate = true;
                        }

                        if ($scope.due_amount == 0) {
                            $scope.months = "";
                        }
                        else {
                            $scope.months = promise.month;
                            $scope.nextduedate = true;
                        }
                        $scope.year = promise.year;
                        if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "C") {
                            $scope.FYP_Bank_Name = "--";
                            $scope.FYP_DD_Cheque_No = "--";
                            $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].FYP_ReceiptDate;
                        }
                        else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "O") {
                            $scope.FYP_Bank_Name = "--";
                            $scope.FYP_DD_Cheque_No = "--";
                            $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].FYP_ReceiptDate;
                        }
                        else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "B") {
                            $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                            $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                            //$scope.FYP_ReceiptDate = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                            $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].fyP_ReceiptDate;
                        }
                        // $scope.FYP_ReceiptDate = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                        $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].fyP_ReceiptDate;
                        if (promise.fillstudentviewdetails[0].fyP_Remarks != null || promise.fillstudentviewdetails[0].fyP_Remarks != "") {
                            $scope.feeremarks = promise.fillstudentviewdetails[0].fyP_Remarks;
                        }
                        else {
                            $scope.feeremarks = "Remarks Not Given";
                        }
                        $scope.ACYST_RollNo = promise.fillstudentviewdetails[0].ACYST_RollNo;
                        // $scope.curdate = $filter('date')(new Date(), "dd-MM-yyyy");
                    }
                })
            $scope.atotalA = function (e) {
                var atotalc = 0;
                angular.forEach($scope.showdetailsreceipt, function (e) {
                    atotalc += e.ftcP_PaidAmount;
                });
                return atotalc;
            };
            $scope.ctotalA = function (e) {
                var atotalc = 0;
                angular.forEach($scope.showdetailsreceipt, function (e) {
                    atotalc += e.ftcP_ConcessionAmount;
                });
                return atotalc;
            };
        }
         $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }

        //$scope.DeletRecord = function (employee, SweetAlert) {
        //    $scope.editEmployee = employee.fyghM_Id;
        //    var orgid = $scope.editEmployee;
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do You Want To Delete Record?",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
        //        cancelButtonText: "Cancel",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //   function (isConfirm) {
        //       if (isConfirm) {
        //           apiService.DeleteURI("CollegeFeeTransaction/Deletedetails", orgid).
        //           then(function (promise) {
        //               $scope.thirdgrid = promise.alldata;
        //               if (promise.returnval == "true") {
        //                   swal('Record Deleted Successfully');
        //               }
        //               else {
        //                   swal('Record Not Deleted Successfully');
        //               }
        //               $state.reload();
        //           })
        //       }
        //       else {
        //           swal("Record Deletion Cancelled");
        //       }
        //   });
        //}

        $scope.cleardata = function () {
            $state.reload();
        }

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;
            apiService.getURI("CollegeFeeTransaction/Editdetails", orgid).
                then(function (promise) {
                    $scope.addnewbtn = false;
                    $scope.FMG_Id = promise.alldata[0].fmG_Id;
                    $scope.totalgrid.headcount.selected.FMH_Id = promise.alldata[0].fmH_Id;
                    $scope.totalgrid.installmentcount.FMI_Id = promise.alldata[0].fmI_Id;
                    if (promise.alldata[0].fyghM_FineApplicableFlag = 0) {
                        $scope.totalgrid.FYGHM_FineApplicableFlag = true;
                    }
                    if (promise.alldata[0].fyghM_Common_AmountFlag = 0) {
                        $scope.totalgrid.FYGHM_Common_AmountFlag = true;
                    }
                    if (promise.alldata[0].fyghM_ActiveFlag = 0) {
                        $scope.totalgrid.FYGHM_ActiveFlag = true;
                    }
                })
        }
        $scope.printData = function (printmodal) {
            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link href="plugins/bootstrap/css/bootstrap.css" />' +
                '<link href="css/style.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }   

        $scope.printData1 = function (printmodal) {
            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                        ' <link href="css/print/baldwin/BWMC/BGMCStudycertificatePdf.css" media="print" rel="stylesheet" />' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                        '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');;
            popupWinindow.document.close();
        } 

        $scope.submitted = false;
        $scope.savedatatrans = [];
        $scope.savedata = function (totalgrid, groupcount) {
            $scope.savedatatrans = [];
            $scope.savedataadvance = [];
            if ($scope.myForm.$valid) {
                //MB for Challan
                if ($scope.filterdata != 'Challan_No') {

                    //advance collection
                    if ($scope.totalgridadvance)
                        angular.forEach($scope.totalgridadvance, function (opq) {
                            if (opq.isSelected1) {
                                count += 1;
                                $scope.savedataadvance.push(opq);
                            }
                        })
                    //advance collection

                    var count = 0;
                    if (ins_spe_list.length == 0 && remove_list.length == 0) {
                        angular.forEach($scope.totalgrid, function (opq) {
                            if (opq.isSelected) {
                                count += 1;
                                $scope.savedatatrans.push(opq);
                            }
                        })
                    }
                    else if (ins_spe_list.length > 0 && remove_list.length > 0) {
                        angular.forEach($scope.totalgrid, function (opq) {
                            if (opq.isSelected) {
                                if (opq.Head_Flag == 'H') {
                                    count += 1;
                                    $scope.savedatatrans.push(opq);
                                }
                                else if (opq.Head_Flag == 'SH') {
                                    angular.forEach(ins_spe_list, function (s) {
                                        if (s.ftI_Id == opq.ftI_Id) {
                                            angular.forEach(s.sp_list, function (s1) {
                                                if (s1.sp_id == opq.fmH_Id) {
                                                    var toBePaid = 0;
                                                    angular.forEach(s1.sp_ind_list, function (s2) {
                                                        toBePaid += Number(s2.FCSS_ToBePaid);
                                                    })
                                                    if (toBePaid == Number(opq.FCSS_ToBePaid)) {
                                                        angular.forEach(s1.sp_ind_list, function (s2) {
                                                            count += 1;
                                                            $scope.savedatatrans.push(s2);
                                                        })
                                                    }
                                                    else if (toBePaid > Number(opq.FCSS_ToBePaid)) {

                                                        var keepGoing = true;
                                                        angular.forEach(s1.sp_ind_list, function (s2) {
                                                            if (keepGoing) {
                                                                if (Number(opq.FCSS_ToBePaid) >= Number(s2.FCSS_ToBePaid)) {
                                                                    count += 1;
                                                                    $scope.savedatatrans.push(s2);
                                                                    opq.FCSS_ToBePaid = (Number(opq.FCSS_ToBePaid) - Number(s2.FCSS_ToBePaid));
                                                                }
                                                                else if (Number(opq.FCSS_ToBePaid) < Number(s2.FCSS_ToBePaid)) {
                                                                    s2.FCSS_ToBePaid = Number(opq.FCSS_ToBePaid);
                                                                    count += 1;
                                                                    $scope.savedatatrans.push(s2);
                                                                    opq.FCSS_ToBePaid = (Number(opq.FCSS_ToBePaid) - Number(s2.FCSS_ToBePaid));
                                                                }
                                                                if (Number(opq.FCSS_ToBePaid) == 0) {
                                                                    keepGoing = false;
                                                                }
                                                            }

                                                        })
                                                    }

                                                }

                                            })
                                        }

                                    })
                                }
                            }
                        })
                    }
                    //MB For Special

                    if ($scope.curramount > 0) {
                        var pay_modes = [];
                        var amount = 0;
                        if ($scope.FYP_PayModeType == 'Multiple') {
                            if ($scope.Cash_Multiple) {
                                amount += Number($scope.Cash_Amount);
                                pay_modes.push({ FYPPM_TransactionTypeFlag: 'C', FYPPM_TotalPaidAmount: Number($scope.Cash_Amount), FYPPM_BankName: '', FYPPM_DDChequeNo: '', FYPPM_DDChequeDate: new Date().toDateString() });
                            }
                            if ($scope.Bank_Multiple) {
                                amount += Number($scope.Bank_Amount);
                                pay_modes.push({ FYPPM_TransactionTypeFlag: 'B', FYPPM_TotalPaidAmount: Number($scope.Bank_Amount), FYPPM_BankName: $scope.Bank_Name, FYPPM_DDChequeNo: $scope.Bank_No, FYPPM_DDChequeDate: new Date($scope.Bank_Date).toDateString() });
                            }
                            if ($scope.Card_Multiple) {
                                amount += Number($scope.Card_Amount);
                                pay_modes.push({ FYPPM_TransactionTypeFlag: 'S', FYPPM_TotalPaidAmount: Number($scope.Card_Amount), FYPPM_BankName: $scope.Card_Name, FYPPM_DDChequeNo: $scope.Card_No, FYPPM_DDChequeDate: new Date($scope.Card_Date).toDateString() });
                            }
                            if ($scope.R_N_Multiple) {
                                amount += Number($scope.R_N_Amount);
                                pay_modes.push({ FYPPM_TransactionTypeFlag: 'R', FYPPM_TotalPaidAmount: Number($scope.R_N_Amount), FYPPM_BankName: $scope.R_N_Name, FYPPM_DDChequeNo: $scope.R_N_No, FYPPM_DDChequeDate: new Date($scope.R_N_Date).toDateString() });
                            }
                        }
                        else {
                            amount = Number($scope.curramount);
                        }

                        if (Number($scope.curramount) == Number(amount)) {
                            //if ($scope.FYP_Id > 0) {
                            //    var disfun = "Update";
                            //    var data = {
                            //        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                            //        "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                            //        savetmpdata: $scope.savedatatrans,
                            //        "FYP_ReceiptNo": $scope.FYP_ReceiptNo,
                            //        "FYP_ReceiptDate": new Date($scope.FYP_ReceiptDate).toDateString(),
                            //        "FYP_PayModeType":$scope.FYP_PayModeType,
                            //        "FYP_Remarks": $scope.FYP_Remarks,
                            //        "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                            //        "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                            //        "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                            //        "FYP_TotalPaidAmount": $scope.curramount,
                            //        "FYP_Tot_Concession_Amt": $scope.totalconcession,
                            //        "FYP_TotalFineAmount": $scope.totalfine,
                            //        "FYP_Tot_Waived_Amt": $scope.totalwaived,
                            //        "FYP_Bank_Name": $scope.FYP_Bank_Name,
                            //        "filterinitialdata": $scope.filterdata,
                            //        "auto_receipt_flag": autoreceipt,
                            //        "automanualreceiptno": automanualreceiptnotranum,
                            //        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                            //        "FYP_Id": $scope.FYP_Id
                            //    }
                            //}
                            //else {

                            var FYPPM_ClearanceDate = null;
                            if ($scope.FYPPM_ClearanceDate != null && $scope.FYPPM_ClearanceDate != undefined) {
                                FYPPM_ClearanceDate = new Date($scope.FYPPM_ClearanceDate).toDateString();
                            }
                            else {
                                $scope.FYPPM_ClearanceDate = new Date().toDateString();
                                FYPPM_ClearanceDate = $scope.FYPPM_ClearanceDate;
                            }

                            var disfun = "Save";
                            var data = {
                                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                                "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                                savetmpdata: $scope.savedatatrans,
                                "FYP_ReceiptNo": "",
                                "FYP_ReceiptDate": new Date($scope.FYP_ReceiptDate).toDateString(),
                                "FYPPM_ClearanceDate": new Date($scope.FYPPM_ClearanceDate).toDateString(),
                                "FYP_PayModeType": $scope.FYP_PayModeType,
                                "FYP_Remarks": $scope.FYP_Remarks,
                                "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                                "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                                "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                                "FYP_TotalPaidAmount": $scope.curramount,
                                "FYP_Tot_Concession_Amt": $scope.totalconcession,
                                "FYP_TotalFineAmount": $scope.totalfine,
                                "FYP_Tot_Waived_Amt": $scope.totalwaived,
                                "FYP_Tot_Waived_Amt": 0,
                                //"FYP_Bank_Name": $scope.FYP_Bank_Name,
                                "FYP_Bank_Name": $scope.FMBANK_Id,
                                "filterinitialdata": $scope.filterdata,
                                "auto_receipt_flag": autoreceipt,
                                "automanualreceiptno": automanualreceiptnotranum,
                                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                                Modes: pay_modes,
                                saveadvancedata: $scope.savedataadvance
                            }
                            // }
                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            }
                            swal({
                                title: "Are you sure?",
                                text: "Do You Want To " + disfun + " Record? ",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes," + disfun + " it",
                                cancelButtonText: "Cancel",
                                closeOnConfirm: false,
                                closeOnCancel: false,
                                showLoaderOnConfirm: true,
                            },
                                function (isConfirm) {
                                    if (isConfirm) {
                                        apiService.create("CollegeFeeTransaction/", data).
                                            then(function (promise) {
                                                if (promise.returnval == "true") {
                                                    //if ($scope.cfg.ASMAY_Id === promise.fillyear[0].asmaY_Id) {
                                                    //}
                                                    //else {
                                                    //    $scope.yearlst = promise.fillyear;
                                                    //    $scope.cfg.ASMAY_Id = promise.fillyear[0].asmaY_Id;
                                                    //}
                                                    //if (autoreceipt == 1) {
                                                    //    $scope.showreceiptno = false;
                                                    //}
                                                    //else {
                                                    //    $scope.showreceiptno = true;
                                                    //}
                                                    //$scope.receiptgrid = promise.receiparraydelete;
                                                    //$scope.FYP_ReceiptDate = new Date();
                                                    //$scope.FYP_DD_Cheque_Date = new Date();
                                                    //if ($scope.FYP_Bank_Or_Cash == 'C') {
                                                    //    $scope.bankdetails = false;
                                                    //}
                                                    //else if ($scope.FYP_Bank_Or_Cash == 'B') {
                                                    //    $scope.bankdetails = true;
                                                    //}
                                                    //configuration settings
                                                    //if (promise.feeconfiglist.length > 0) {
                                                    //    grouporterm = promise.feeconfiglist[0].fmC_GroupOrTermFlg;
                                                    //    if (grouporterm == "T") {
                                                    //        $scope.grouportername = "Term Name"
                                                    //    }
                                                    //    else if (grouporterm == "G") {
                                                    //        $scope.grouportername = "Group Name"
                                                    //    }
                                                    //}
                                                    //$scope.addnewbtn = true;
                                                    //if (promise.displaymessage == null) {
                                                    //    promise.displaymessage = "Saved/Updated";
                                                    //}
                                                    //$scope.grigview1 = false;
                                                    //$scope.submitted = false;
                                                    //$scope.FYP_ReceiptDate = new Date();
                                                    //$scope.FYP_DD_Cheque_Date = new Date();
                                                    // $state.reload();
                                                    //  swal("Record " + promise.displaymessage + " Successfully");
                                                    if ($scope.FYP_Id > 0) {
                                                        swal("Record Updated Successfully");
                                                    }
                                                    else {
                                                        swal("Record Saved Successfully");
                                                    }
                                                }
                                                else if (promise.returnval == "false") {
                                                    if ($scope.FYP_Id > 0) {
                                                        swal("Record Not Updated Successfully");
                                                    }
                                                    else {
                                                        swal("Record Not Saved Successfully");
                                                    }
                                                }
                                                else {
                                                    swal(promise.returnval);
                                                }
                                                $state.reload();
                                            })
                                    }
                                    else {
                                        swal("Record saved Failed", "Failed");
                                    }
                                });
                        }
                        else {
                            swal("Sum Of Amounts Of By Mode Of Payments Must Match Now Paying Amount");
                        }

                    }
                    else {
                        var count_head = 0;
                        angular.forEach(totalgrid, function (obje) {
                            if (obje.isSelected) {
                                count_head += 1;
                            }
                        })
                        if (count_head == 0) {
                            swal("Atleast one head has to be checked to save the Transaction!!!");
                        }
                        else {
                            swal("Transaction cannot be done for Zero amount!!!")
                        }
                    }
                    //MB for Challan
                }
                else if ($scope.filterdata == 'Challan_No') {
                    $scope.savedatatrans = [];
                    var count_head = 0;
                    angular.forEach($scope.totalgrid, function (user) {
                        if (!!user.isSelected) {
                            $scope.savedatatrans.push(user);
                            count_head += 1;
                        }
                    })
                    if ($scope.curramount > 0) {
                        var pay_modes = [];
                        var amount = 0;
                        if ($scope.FYP_PayModeType == 'Multiple') {
                            if ($scope.Cash_Multiple) {
                                amount += Number($scope.Cash_Amount);
                                pay_modes.push({ FYPPM_TransactionTypeFlag: 'C', FYPPM_TotalPaidAmount: Number($scope.Cash_Amount), FYPPM_BankName: '', FYPPM_DDChequeNo: '', FYPPM_DDChequeDate: new Date().toDateString() });
                            }
                            if ($scope.Bank_Multiple) {
                                amount += Number($scope.Bank_Amount);
                                pay_modes.push({ FYPPM_TransactionTypeFlag: 'B', FYPPM_TotalPaidAmount: Number($scope.Bank_Amount), FYPPM_BankName: $scope.Bank_Name, FYPPM_DDChequeNo: $scope.Bank_No, FYPPM_DDChequeDate: new Date($scope.Bank_Date).toDateString() });
                            }
                            if ($scope.Card_Multiple) {
                                amount += Number($scope.Card_Amount);
                                pay_modes.push({ FYPPM_TransactionTypeFlag: 'S', FYPPM_TotalPaidAmount: Number($scope.Card_Amount), FYPPM_BankName: $scope.Card_Name, FYPPM_DDChequeNo: $scope.Card_No, FYPPM_DDChequeDate: new Date($scope.Card_Date).toDateString() });
                            }
                            if ($scope.R_N_Multiple) {
                                amount += Number($scope.R_N_Amount);
                                pay_modes.push({ FYPPM_TransactionTypeFlag: 'R', FYPPM_TotalPaidAmount: Number($scope.R_N_Amount), FYPPM_BankName: $scope.R_N_Name, FYPPM_DDChequeNo: $scope.R_N_No, FYPPM_DDChequeDate: new Date($scope.R_N_Date).toDateString() });
                            }
                        }
                        else {
                            amount = Number($scope.curramount);
                        }
                        if (Number($scope.curramount) == Number(amount)) {
                            var data = {
                                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                                "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                                savetmpdata: $scope.savedatatrans,
                                "FYP_ReceiptNo": $scope.FYP_ReceiptNo,
                                "FYP_ReceiptDate": new Date($scope.FYP_ReceiptDate).toDateString(),
                                "FYP_Remarks": $scope.FYP_Remarks,
                                "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                                "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                                "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                                "FYP_TotalPaidAmount": $scope.curramount,
                                "FYP_Tot_Concession_Amt": $scope.totalconcession,
                                "FYP_TotalFineAmount": $scope.totalfine,
                                //"FYP_Tot_Waived_Amt": $scope.totalwaived,
                                "FYP_Tot_Waived_Amt": 0,
                                "FYP_Bank_Name": $scope.FMBANK_Id,
                                //"FMBANK_Id": $scope.FMBANK_Id,
                                "filterinitialdata": $scope.filterdata,
                                "auto_receipt_flag": autoreceipt,
                                "automanualreceiptno": automanualreceiptnotranum,
                                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                                "FYP_ChallanNo": $scope.FYP_ChallanNo,
                                "FYP_Id": temp_FYP_Id,
                            }
                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            }
                            swal({
                                title: "Are you sure?",
                                text: "Do You Want To Make Transaction? ",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Make it",
                                cancelButtonText: "Cancel",
                                closeOnConfirm: false,
                                closeOnCancel: false,
                                showLoaderOnConfirm: true,

                            },
                                function (isConfirm) {
                                    if (isConfirm) {
                                        apiService.create("CollegeFeeTransaction/Save_Chaln_No", data).
                                            then(function (promise) {
                                                if (promise.returnval == "Cancel") {
                                                    swal("Transaction Failed");
                                                }
                                                else if (promise.returnval == "Save") {
                                                    swal('Transaction Done Successfully For Selected Challan No');
                                                }
                                                else if (promise.returnval == "Error") {
                                                    swal('Kindly contact Administrator ');
                                                }
                                                else {
                                                    swal(promise.returnval);
                                                }
                                                $state.reload();
                                            })
                                    }
                                    else {
                                        swal("Transaction Cancelled");
                                    }

                                });
                        }
                        else {
                            swal("Sum Of Amounts Of By Mode Of Payments Must Match Now Paying Amount");
                        }
                    }
                    else {
                        if (count_head == 0) {
                            swal("Atleast one head has to be checked to save the Transaction!!!");
                        }
                        else {
                            swal("Transaction cannot be done for Zero amount!!!")
                        }
                    }
                }
                //MB for Challan
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.closedata = function () {
            $scope.FYP_DD_Cheque_Date = new Date();
            $scope.FYP_ReceiptDate = new Date();
            $scope.FYP_DD_Cheque_No = "";
            $scope.FYP_Bank_Name = "";
            $scope.FYP_Remarks = "";
        }

        //$scope.DeletRecord = function (paymentid, studentid) {
        //    var data = {
        //        "FYP_Id": paymentid,
        //        "AMCST_Id": studentid
        //    }
        //    var config = {
        //        headers: {
        //            'Content-Type': 'application/json;'
        //        }
        //    }
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do You Want To Delete Record?",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
        //        cancelButtonText: "Cancel",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //        function (isConfirm) {
        //            if (isConfirm) {
        //                apiService.create("CollegeFeeTransaction/Deletedetails", data).
        //                    then(function (promise) {
        //                        if (promise.returnval == "true") {
        //                            swal('Record Deleted Successfully');
        //                            //$state.reload();
        //                        }
        //                        else {
        //                            swal('Transaction is not Processed.Kindly contact Administrator!!!!!');
        //                        }
        //                        $state.reload();
        //                    })
        //            }
        //            else {
        //                swal("Record Deletion Cancelled");
        //            }
        //        });
        //}

        $scope.DeletRecord = function (paymentid, studentid) {
            swal({
                title: "Cancel Receipt!",
                text: "Kindly enter Remarks for Cancel Receipt!!!",
                type: "input",
                showCancelButton: true,
                closeOnConfirm: false,
                inputPlaceholder: "Enter Remarks"
            }, function (inputValue) {
                if (inputValue === false) return false;
                if (inputValue === "") {
                    swal.showInputError("Kindly Enter Remarks!");
                    return false
                }
                if (inputValue != "") {

                    var data = {
                        "FYP_Id": paymentid,
                        "AMCST_Id": studentid,
                        "FYP_Remarks": inputValue
                    }

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("CollegeFeeTransaction/Deletedetails", data).
                        then(function (promise) {
                            if (promise.returnval == "true") {

                                swal('Record Deleted Successfully');
                                $state.reload();
                            }
                            else {
                                swal('Transaction is not Processed.Kindly contact Administrator!!!!!');
                            }
                        })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            },
            )
        }


        $scope.checkforduplicates = function (receiptno) {
            var data = {
                "FYP_Id": $scope.FYP_Id,
                "FYP_ReceiptNo": receiptno,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeFeeTransaction/feereceiptduplicate", data).
                then(function (promise) {
                    if (promise.duplicatereceipt.length > 0) {
                        swal("Duplicate Receipt No")
                        if ($scope.FYP_Id > 0) {
                            $scope.FYP_ReceiptNo = "";
                            $scope.FYP_ReceiptNo_E = "";
                        }
                        else {
                            $scope.FYP_ReceiptNo = receiptno;
                        }
                    }
                })
        }
        $scope.studentlst = [];
        $scope.searchfilter = function (objj, radioobj) {
            if (institutionid == '5' || institutionid == '4' || institutionid == '3' || institutionid == '6' || institutionid == '8') {
                if (objj.search.length >= '3' && radioobj == 'regular') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;

                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                            //MB For Challan
                            if ($scope.filterdata == 'Challan_No') {
                                angular.forEach($scope.studentlst, function (ob) {
                                    if (ob.amcsT_Id == temp_amcsT_Id) {
                                        ob.Selected = true;
                                        $scope.AMCST_Id = ob;
                                    }
                                })
                                $scope.onselectstudent($scope.AMCST_Id);
                            }
                            //MB For Challan
                        })
                }

                if (objj.search.length >= '4' && radioobj == 'AdmNo') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
                else if (objj.search.length >= '6' && radioobj == 'regno') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
                else if (objj.search.length >= '3' && radioobj == 'NameAdmno') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
                else if (objj.search.length >= '4' && radioobj == 'Admnoname') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
                //radha
                else if (objj.search.length >= '3' && radioobj == 'NameRegNo') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
                else if (objj.search.length >= '4' && radioobj == 'RegNoName') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
                //radha
            }
            else {
                if (objj.search.length >= '1' && radioobj == 'regular') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
                if (objj.search.length >= '2' && radioobj == 'AdmNo') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
                else if (objj.search.length >= '2' && radioobj == 'regno') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
                else if (objj.search.length >= '2' && radioobj == 'NameAdmno') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
                else if (objj.search.length >= '2' && radioobj == 'Admnoname') {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("CollegeFeeTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amcsT_FirstName.length > 0) {
                                    var string = objectt.amcsT_FirstName;
                                    objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }
            }
        };
        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;

                if ($scope.search123 == "5") {
                    $scope.txt = false;
                    $scope.numbr = true;
                    $scope.dat = false;
                }
                else if ($scope.search123 == "4") {
                    $scope.txt = false;
                    $scope.numbr = false;
                    $scope.dat = true;
                }
                else {
                    $scope.txt = true;
                    $scope.numbr = false;
                    $scope.dat = false;
                }
                $scope.searchtxt = "";
                $scope.searchdat = "";
                $scope.searchnumbr = "";
            }
        }

        $scope.ShowSearch_Report = function () {
        
            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
                if ($scope.search123 == "5") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr,
                        "ASMAY_Id": Number($scope.cfg.ASMAY_Id)

                    }
                }
                else if ($scope.search123 == "4") {
                    debugger;
                    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123,
                        "searchdate": date,
                        "ASMAY_Id": Number($scope.cfg.ASMAY_Id)

                    }
                }
                else {
                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt,
                        "ASMAY_Id": Number($scope.cfg.ASMAY_Id)

                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CollegeFeeTransaction/searching", data).
                    then(function (promise) {
                        $scope.receiptgrid = promise.searcharray;
                        $scope.totcountsearch = promise.searcharray.length;

                        if (promise.searcharray == null || promise.searcharray == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }
        $scope.clearsearch = function () {
            $state.reload();
        }
        $scope.alltermchk = false;
        $scope.updateshowlabel = false;
        $scope.showstudentname = true;
        $scope.diablemodeofpayment = false;
        //$scope.edittransaction = function (fypid, amcsT_Id) {
        //    $scope.FYP_Id = fypid;
        //    $scope.AMCST_Id = amcsT_Id;
        //    var data = {
        //        "FYP_Id": fypid,
        //        "AMCST_Id": amcsT_Id
        //    }
        //    apiService.create("CollegeFeeTransaction/edittransaction", data).
        //   then(function (promise) {
        //       $scope.alltermchk = true;
        //       $scope.allcheck = true;
        //       $scope.grigview1 = true;
        //       $scope.updateshowlabel = true;
        //       $scope.showstudentname = false;
        //       $scope.diablemodeofpayment = true;
        //       $scope.isSelected = true;
        //       $scope.totalgrid = promise.receiparraydeleteall;
        //       //MB for Special
        //       debugger;
        //       $scope.temp_Head_Instl_list = [];
        //       angular.forEach($scope.totalgrid, function (uy) {
        //           uy.Head_Flag = 'H';
        //           $scope.temp_Head_Instl_list.push(uy);
        //       })
        //       remove_list = [];
        //       ins_spe_list = [];
        //       angular.forEach(promise.instalspecial, function (ins) {
        //           var special_list = [];
        //           angular.forEach($scope.special_head_list, function (op1) {
        //               var spe_ind_list = [];
        //               angular.forEach($scope.special_head_details, function (op2) {
        //                   if (op1.fmsfH_Id == op2.fmsfH_Id) {
        //                       angular.forEach($scope.totalgrid, function (op_m) {
        //                           if (op_m.fmH_Id == op2.fmH_ID && op_m.ftI_Id == ins.ftI_Id) {
        //                               spe_ind_list.push(op_m);
        //                               remove_list.push(op_m);
        //                           }
        //                       })
        //                   }
        //               })
        //               if (spe_ind_list.length > 0) {
        //                   special_list.push({ sp_id: op1.fmsfH_Id, sp_name: op1.fmsfH_Name, sp_ind_list: spe_ind_list });
        //               }
        //           })
        //           if (special_list.length > 0) {
        //               ins_spe_list.push({ ftI_Id: ins.ftI_Id, ftI_Name: ins.ftI_Name, sp_list: special_list });
        //           }
        //       })
        //       if (ins_spe_list.length > 0) {
        //           angular.forEach(remove_list, function (ma1) {
        //               $scope.temp_Head_Instl_list.splice($scope.temp_Head_Instl_list.indexOf(ma1), 1);
        //           })
        //           angular.forEach(ins_spe_list, function (a1) {
        //               angular.forEach(a1.sp_list, function (a2) {
        //                   var fsS_CurrentYrCharges = 0;
        //                   var fsS_TotalToBePaid = 0;
        //                   var fsS_ConcessionAmount = 0;
        //                   var fsS_FineAmount = 0;
        //                   var fsS_ToBePaid = 0;
        //                   var fsS_TotalToBePaidaddfine = 0;
        //                   var fmG_Id = 0;
        //                   var fmG_GroupName = '';
        //                   var not_cnt = 0;
        //                   var totamtt = 0;
        //                   angular.forEach(a2.sp_ind_list, function (a3) {
        //                       if (fmG_Id == 0) {
        //                           fmG_Id = a3.fmG_Id;
        //                           fmG_GroupName = a3.fmG_GroupName;
        //                       }
        //                       else {
        //                           if (fmG_Id != a3.fmG_Id) {
        //                               not_cnt += 1;
        //                           }
        //                       }
        //                       fsS_CurrentYrCharges += a3.fsS_CurrentYrCharges;
        //                       fsS_TotalToBePaid += a3.fsS_TotalToBePaid;
        //                       fsS_ConcessionAmount += a3.fsS_ConcessionAmount;
        //                       fsS_FineAmount += a3.fsS_FineAmount;
        //                       fsS_ToBePaid += a3.fsS_ToBePaid;
        //                       fsS_TotalToBePaidaddfine += 0;
        //                   })
        //                   if (not_cnt == 0) {
        //                       $scope.temp_Head_Instl_list.push({ fmG_Id: fmG_Id, fmG_GroupName: 'SH_' + fmG_GroupName, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, totalamount: fsS_CurrentYrCharges, fsS_TotalToBePaid: fsS_TotalToBePaid, fsS_ConcessionAmount: fsS_ConcessionAmount, fsS_FineAmount: fsS_FineAmount, fsS_ToBePaid: fsS_ToBePaid, fsS_TotalToBePaidaddfine: fsS_TotalToBePaidaddfine, Head_Flag: 'SH' });
        //                   }
        //                   else if (not_cnt > 0) {
        //                       $scope.temp_Head_Instl_list.push({ fmG_Id: 0, fmG_GroupName: 'Special_Head', fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, totalamount: fsS_CurrentYrCharges, fsS_TotalToBePaid: fsS_TotalToBePaid, fsS_ConcessionAmount: fsS_ConcessionAmount, fsS_FineAmount: fsS_FineAmount, fsS_ToBePaid: fsS_ToBePaid, fsS_TotalToBePaidaddfine: fsS_TotalToBePaidaddfine, Head_Flag: 'SH' });
        //                   }
        //               })
        //           })
        //           $scope.totalgrid = $scope.temp_Head_Instl_list;
        //       }
        //       //MB for Special
        //       for (var s = 0; s < $scope.totalgrid.length; s++) {
        //           $scope.totalgrid[s].isSelected = true;
        //       }
        //       //changed radha
        //       $scope.filltermsaved = [];
        //       $scope.groupcount = promise.fillmastergroup;
        //       for (var g = 0; g < $scope.groupcount.length; g++) {
        //           for (var h = 0; h < promise.disableterms.length; h++) {
        //               if ($scope.groupcount[g].fmG_Id == promise.disableterms[h].fmG_Id)
        //                   $scope.filltermsaved.push($scope.groupcount[g]);
        //           }
        //       }
        //       $scope.groupcount = $scope.filltermsaved;
        //       //changed radha
        //       for (var r = 0; r < $scope.groupcount.length; r++) {
        //           $scope.groupcount[r].disablepaisterms = true;
        //       }
        //       for (var p = 0; p < promise.disableterms.length; p++) {
        //           for (var q = 0; q < $scope.groupcount.length; q++) {
        //               if ($scope.groupcount[q].fmG_Id == promise.disableterms[p].fmG_Id) {
        //                   $scope.groupcount[q].selected = true;
        //               }
        //           }
        //       }
        //       $scope.studentlst = promise.receiparraydelete
        //       $scope.amcsT_FirstName = promise.receiparraydelete[0].amcsT_FirstName + ' ' + promise.receiparraydelete[0].amcsT_MiddleName + ' ' +
        //           promise.receiparraydelete[0].amcsT_LastName;
        //       $scope.updateshowlabel = true;
        //       $scope.updatename = $scope.amcsT_FirstName;
        //       $scope.studidd = promise.receiparraydelete[0].amcsT_Id;
        //       $scope.amcsT_AdmNo = promise.receiparraydelete[0].amcsT_AdmNo;
        //       $scope.amcsT_RegistrationNo = promise.receiparraydelete[0].amcsT_RegistrationNo;
        //       $scope.acysT_RollNo = promise.receiparraydelete[0].acysT_RollNo;
        //       $scope.amcO_CourseName = promise.receiparraydelete[0].amcO_CourseName;
        //       $scope.amB_BranchName = promise.receiparraydelete[0].amB_BranchName;
        //       $scope.amcsT_MobileNo = promise.receiparraydelete[0].amcsT_MobileNo;
        //       $scope.amcsT_FatherName = promise.receiparraydelete[0].amcsT_FatherName;
        //       $scope.amcsT_DOB = promise.receiparraydelete[0].amcsT_DOB;
        //       $scope.FYP_ReceiptDate = new Date(promise.receiparraydelete[0].fyP_ReceiptDate);
        //       if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'C') {
        //           $scope.FYP_Bank_Or_Cash = 'C';
        //           $scope.bankdetails = false;
        //       }
        //       else if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'B') {
        //           $scope.FYP_Bank_Or_Cash = 'B';
        //           $scope.bankdetails = true;
        //       }
        //       else if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'S') {
        //           $scope.FYP_Bank_Or_Cash = 'S';
        //           $scope.bankdetails = true;
        //       }
        //       else if (promise.receiparraydelete[0].fyP_Bank_Or_Cash == 'R') {
        //           $scope.FYP_Bank_Or_Cash = 'R';
        //           $scope.bankdetails = true;
        //       }
        //       $scope.FYP_DD_Cheque_Date = new Date(promise.receiparraydelete[0].fyP_DD_Cheque_Date);
        //       $scope.FYP_DD_Cheque_No = promise.receiparraydelete[0].fyP_DD_Cheque_No;
        //       $scope.FYP_Bank_Name = promise.receiparraydelete[0].fyP_Bank_Name;
        //       $scope.totalfee = promise.receiparraydelete[0].fyP_TotalPaidAmount;
        //       $scope.curramount = promise.receiparraydelete[0].fyP_TotalPaidAmount;
        //       if (autoreceipt == 1) {
        //           $scope.showreceiptno = false;
        //       }
        //       else {
        //           $scope.showreceiptno = true;
        //           $scope.FYP_ReceiptNo = promise.receiparraydelete[0].fyP_ReceiptNo;
        //       }
        //   }
        //   )
        //}


        $scope.edittransaction = function (fypid, amcsT_Id, user1) {
            $scope.FYP_Id = fypid;
            $scope.AMCST_Id = amcsT_Id;
            var data = {
                "FYP_Id": fypid,
                "AMCST_Id": amcsT_Id
            }
            apiService.create("CollegeFeeTransaction/edittransaction", data).
                then(function (promise) {
                    $scope.Stu_Name = user1.amcsT_FirstName + ' ' + user1.amcsT_MiddleName + ' ' + user1.amcsT_LastName;
                    $scope.FYP_PayModeType_E = promise.fyP_PayModeType;
                    $scope.FYP_ReceiptDate_E = new Date(promise.fyP_ReceiptDate);
                    $scope.FYP_ReceiptNo_E = promise.fyP_ReceiptNo;
                    $scope.FYP_Remarks_E = promise.fyP_Remarks;
                    if ($scope.FYP_PayModeType_E == 'Single') {
                        $scope.FYP_Bank_Or_Cash_E = promise.currpaymentdetails[0].fyppM_TransactionTypeFlag;
                        $scope.FYP_DD_Cheque_Date_E = new Date(promise.currpaymentdetails[0].fyppM_DDChequeDate);
                        $scope.FYP_DD_Cheque_No_E = promise.currpaymentdetails[0].fyppM_DDChequeNo;
                        $scope.FYPPM_ClearanceDate_E = promise.currpaymentdetails[0].fyppM_ClearanceDate;
                        $scope.FYP_Bank_Name_E = promise.currpaymentdetails[0].fyppM_BankName;
                    }
                    else if ($scope.FYP_PayModeType_E == 'Multiple') {
                        angular.forEach(promise.currpaymentdetails, function (pay_d) {
                            if (pay_d.fyppM_TransactionTypeFlag == 'C') {
                                $scope.Cash_Multiple_E = true;
                                $scope.Cash_Amount_E = pay_d.fyppM_TotalPaidAmount;
                            }
                            else if (pay_d.fyppM_TransactionTypeFlag == 'B') {
                                $scope.Bank_Multiple_E = true;
                                $scope.Bank_Date_E = new Date(pay_d.fyppM_DDChequeDate);
                                $scope.Clearance_Date_E = new Date(pay_d.fyppM_DDChequeDate);
                                $scope.Bank_No_E = pay_d.fyppM_DDChequeNo;
                                $scope.Bank_Name_E = pay_d.fyppM_BankName;
                                $scope.Bank_Amount_E = pay_d.fyppM_TotalPaidAmount;
                            }
                            else if (pay_d.fyppM_TransactionTypeFlag == 'S') {
                                $scope.Card_Multiple_E = true;
                                $scope.Card_Date_E = new Date(pay_d.fyppM_DDChequeDate);
                                $scope.Card_No_E = pay_d.fyppM_DDChequeNo;
                                $scope.Card_Name_E = pay_d.fyppM_BankName;
                                $scope.Card_Amount_E = pay_d.fyppM_TotalPaidAmount;
                            }
                            else if (pay_d.fyppM_TransactionTypeFlag == 'R') {
                                $scope.R_N_Multiple_E = true;
                                $scope.R_N_Date_E = new Date(pay_d.fyppM_DDChequeDate);
                                $scope.R_N_No_E = pay_d.fyppM_DDChequeNo;
                                $scope.R_N_Name_E = pay_d.fyppM_BankName;
                                $scope.R_N_Amount_E = pay_d.fyppM_TotalPaidAmount;
                            }
                            else if (pay_d.fyppM_TransactionTypeFlag == 'U') {
                                $scope.R_N_Multiple_E = true;
                                $scope.R_N_Date_E = new Date(pay_d.fyppM_DDChequeDate);
                                $scope.R_N_No_E = pay_d.fyppM_DDChequeNo;
                                $scope.R_N_Name_E = pay_d.fyppM_BankName;
                                $scope.R_N_Amount_E = pay_d.fyppM_TotalPaidAmount;
                            }

                        })

                    }
                })
        }


        $scope.getdates = function (yr_id, data) {
            if (data != null) {
               
                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                $scope.minDatemf = new Date(
                    $scope.fromDate,
                    $scope.frommon,
                    $scope.fromDay + 1);

                $scope.maxDatemf = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);
                $scope.today = new Date();
            }
        }
        $scope.showmodaldetailsnew = function (fypid, studid) {
            $scope.AMCO_CourseName = "";
            $scope.AMB_BranchName = "";
            $scope.ACYST_RollNo = "";
            $scope.AMST_AdmNo = "";
            $scope.AMCST_FirstName = "";
            $scope.curdate = "";
            $scope.asmaY_Year = "";
            $scope.AMST_FatherName = "";
            $scope.AMST_MotherName = "";
            $scope.receiptno = "";
            $scope.FMCC_ConcessionName = "";
            var data = {
                "AMCST_Id": studid,
                "filterinitialdata": $scope.filterdata,
                "configset": grouporterm,
                "FYP_Id": fypid,
                "minstall": mergeinstallment
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeFeeTransaction/printreceiptnew", data).
                then(function (promise) {
                    debugger;
                    $scope.MI_Address1 = promise.masterinstitution[0].mI_Address1;
                    $scope.MI_Address2 = promise.masterinstitution[0].mI_Address2;
                    $scope.MI_Address3 = promise.masterinstitution[0].mI_Address3;
                    $scope.MI_Pincode = promise.masterinstitution[0].mI_Pincode;
                    $scope.receiptno = promise.fillstudentviewdetails[0].fyP_ReceiptNo;
                    //$scope.FYP_ReceiptDate = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                    $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].fyP_ReceiptDate;
                    $scope.AMST_AdmNo = promise.fillstudentviewdetails[0].admno;
                    $scope.asmaY_Year = promise.asmaY_Year;
                    if (promise.fillstudentviewdetails[0].amcsT_FirstName != null && promise.fillstudentviewdetails[0].amcsT_MiddleName != null && promise.fillstudentviewdetails[0].amcsT_LastName != null) {
                        $scope.AMCST_FirstName = promise.fillstudentviewdetails[0].amcsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amcsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amcsT_LastName;
                    }
                    else {
                        if (promise.fillstudentviewdetails[0].amcsT_FirstName == null) {
                            promise.fillstudentviewdetails[0].amcsT_FirstName = ' ';
                        }
                        if (promise.fillstudentviewdetails[0].amcsT_MiddleName == null) {
                            promise.fillstudentviewdetails[0].amcsT_MiddleName = ' ';
                        }
                        if (promise.fillstudentviewdetails[0].amcsT_LastName == null) {
                            promise.fillstudentviewdetails[0].amcsT_LastName = ' ';
                        }
                        $scope.AMCST_FirstName = promise.fillstudentviewdetails[0].amcsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amcsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amcsT_LastName;
                    }
                    $scope.AMCO_CourseName = promise.fillstudentviewdetails[0].amcO_CourseName;
                    $scope.AMB_BranchName = promise.fillstudentviewdetails[0].amB_BranchName;
                    $scope.AMCST_FatherName = promise.fillstudentviewdetails[0].amcsT_FatherName;
                    $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;
                    $scope.AMCST_MotherName = promise.fillstudentviewdetails[0].AMCST_MotherName;
                    $scope.period = promise.duration;
                    if ($scope.FMC_RInstallmentsMergeFlag == 1) {
                        $scope.FMC_RInstallmentsFlag = 0;

                        $scope.showdetailsreceipt = promise.filltotaldetails;
                    }
                    else {
                        $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                    }
                    if (promise.fillstudentviewdetails.length > 0) {
                        var fmatotal = 0;
                        var totalpaidamount = 0;
                        angular.forEach(promise.fillstudentviewdetails, function (user) {
                            fmatotal = fmatotal + user.fcsS_TotalCharges;
                            totalpaidamount = totalpaidamount + user.ftcP_PaidAmount;
                        })
                    }
                    $scope.totalfma = fmatotal;
                    if (fmatotal > totalpaidamount)
                        $scope.btotA = fmatotal - totalpaidamount;
                    else
                        $scope.btotA = 0;
                    $scope.atotA = promise.currpaymentdetails[0].ftcP_PaidAmount;
                    $scope.ctotA = promise.currpaymentdetails[0].ftcP_ConcessionAmount;
                    $scope.ftotA = promise.currpaymentdetails[0].ftcP_FineAmount;
                    $scope.wtotA = promise.currpaymentdetails[0].ftP_Waived_Amt;
                    $scope.paymenrgrid = promise.currpaymentdetails;
                    $scope.words = $scope.amountinwords($scope.atotA);
                    $scope.due_amount = promise.dueamount;
                    if ($scope.due_amount == 0) {
                        $scope.date = "";
                        $scope.nextduedate = false;
                    }
                    else {
                        $scope.date = promise.date;
                        $scope.nextduedate = true;
                    }

                    if ($scope.due_amount == 0) {
                        $scope.months = "";
                    }
                    else {
                        $scope.months = promise.month;
                        $scope.nextduedate = true;
                    }
                    $scope.year = promise.year;
                    if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "C") {
                        $scope.FYP_Bank_Name = "--";
                        $scope.FYP_DD_Cheque_No = "--";
                        $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].FYP_ReceiptDate;
                    }
                    else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "O") {
                        $scope.FYP_Bank_Name = "--";
                        $scope.FYP_DD_Cheque_No = "--";
                        $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].FYP_ReceiptDate;
                    }
                    else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "B") {
                        $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                        $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                        // $scope.FYP_ReceiptDate = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                        $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].fyP_ReceiptDate;
                    }
                    // $scope.FYP_ReceiptDate = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                    $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].fyP_ReceiptDate;
                    if (promise.fillstudentviewdetails[0].fyP_Remarks != null || promise.fillstudentviewdetails[0].fyP_Remarks != "") {
                        $scope.feeremarks = promise.fillstudentviewdetails[0].fyP_Remarks;
                    }
                    else {
                        $scope.feeremarks = "Remarks Not Given";
                    }
                    $scope.ACYST_RollNo = promise.fillstudentviewdetails[0].ACYST_RollNo;
                    // $scope.curdate = $filter('date')(new Date(), "dd-MM-yyyy");
                    $scope.curdate = new Date();
                })
        }

        $scope.printDatanew = function (printmodal) {
            var innerContents = document.getElementById("printmodalnew").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link href="plugins/bootstrap/css/bootstrap.css" />' +
                '<link href="css/style.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        //MB For Challan & Fine
        $scope.Clear_Chaln_No = function () {
            $scope.FYP_ChallanNo = null;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.Clear_values_dep();
        }
        $scope.Clear_values_dep = function () {
            $scope.totalgrid = [];
            $scope.AMCST_Id = {};
            $scope.groupcount = [];
            $scope.amcsT_FirstName = "";
            $scope.amcsT_MiddleName = "";
            $scope.amcsT_LastName = "";
            $scope.amcsT_AdmNo = "";
            $scope.amcsT_RegistrationNo = "";
            $scope.acysT_RollNo = "";
            $scope.amcsT_MobileNo = "";
            $scope.amcO_CourseName = "";
            $scope.amB_BranchName = "";
            $scope.amcsT_FatherName = "";
            $scope.amcsT_DOB = "";
            $scope.amsT_fullanme = "";
            $scope.grigview1 = false;
            $scope.totalfee = 0;
            $scope.totalconcession = 0;
            $scope.totalfine = 0;
            $scope.curramount = 0;
            $scope.currbalance = 0;
            $scope.obj.allgrouporterm = false;
            $scope.all = false;
        }

        var temp_amcsT_Id = "";
        var temp_fillmastergroup = [];
        var temp_alldata = [];
        var temp_FYP_Id = "";
        $scope.Search_Chaln_No = function () {
            debugger;
            var data = {
                "configset": grouporterm,
                "FYP_ChallanNo": $scope.FYP_ChallanNo,
                "FYP_ReceiptDate": new Date($scope.FYP_ReceiptDate).toDateString(),
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeFeeTransaction/Search_Chaln_No", data).
                then(function (promise) {
                    if (promise.challan_Flag == true) {
                        if (promise.returnval == 'Pay') {
                            temp_fillmastergroup = promise.fillmastergroup;
                            temp_alldata = promise.alldata;
                            angular.forEach(promise.alldata, function (ed) {
                                if (ed.fmH_Flag == 'F') {
                                    $scope.temp_finehead_amt = ed.fsS_ToBePaid;
                                    $scope.temp_finehead_amt_total = ed.fsS_TotalToBePaid + ed.fsS_FineAmount;
                                }

                            })
                            var objj = {};
                            objj.search = promise.amcsT_FirstName;
                            var radioobj = 'regular';
                            temp_amcsT_Id = promise.amcsT_Id;
                            temp_FYP_Id = promise.fyP_Id;
                            $scope.searchfilter(objj, radioobj);
                        }
                        else if (promise.returnval == 'Paid') {
                            swal("Searched Challa No Paid Amount");
                            $scope.Clear_Chaln_No();
                        }
                        else {
                            swal("Kindly Contact Administrator");
                            $scope.Clear_Chaln_No();
                        }
                    }
                    else if (promise.challan_Flag == false) {
                        swal("Challan No Not Found");
                        $scope.Clear_Chaln_No();
                    }
                })
        }

        //$scope.calculate_fine = function () {
        //    var fine_total = 0;
        //    $scope.totalfine = 0;
        //    $scope.totalfineadv = 0;
        //    angular.forEach($scope.totalgrid, function (nt) {
        //        if (nt.isSelected) {
        //            angular.forEach($scope.temp_Fine_Amounts, function (fn) {
        //                if (fn.fcmaS_Id == nt.FCMAS_Id) {
        //                    fine_total += fn.fine_Amt;
        //                }
        //            })
        //        }
        //    })

        //    angular.forEach($scope.totalgrid, function (nt) {
        //        if (nt.FMH_Flag == 'F') {

        //            if (fine_total == 0) {
        //                nt.isSelected = false;
        //            }

        //            if (nt.isSelected == undefined || nt.isSelected == false) {
        //                if (fine_total > 0) {
        //                    nt.isSelected = true;
        //                }
        //            }
        //            nt.FCSS_ToBePaid = fine_total + $scope.temp_finehead_amt;
        //            //commented on 27 sep 2021
        //            nt.fsS_TotalToBePaidaddfine = fine_total + $scope.temp_finehead_amt_total;
        //            if ($scope.curramount > 0 && nt.isSelected != undefined) {
        //                //$scope.curramount -= Number(nt.FCSS_ToBePaid);
        //            }

        //            if (nt.isSelected == true) {
        //                $scope.totalfine = nt.FCSS_ToBePaid;
        //                $scope.curramount = $scope.curramount + Number(nt.FCSS_ToBePaid);
        //            }
        //            else if (nt.isSelected == false) {
        //                if ($scope.totalfine <= Number(nt.FCSS_ToBePaid)) {
        //                    //$scope.curramount = Number(nt.FCSS_ToBePaid) - $scope.totalfine;
        //                    $scope.totalfine = 0;
        //                }
        //                else {
        //                    $scope.curramount = Number(nt.FCSS_ToBePaid) - $scope.totalfine;
        //                }

        //            }
        //        }
        //    })
        //    var fine_totaladv = 0;
        //    angular.forEach($scope.totalgridadvance, function (nt) {
        //        if (nt.isSelected1) {
        //            angular.forEach($scope.temp_Fine_Amountsadv, function (fn) {
        //                if (fn.fcmaS_Id == nt.FCMAS_Id) {
        //                    fine_totaladv += fn.fine_Amt;
        //                }
        //            })
        //        }
        //    })

        //    angular.forEach($scope.totalgridadvance, function (nt) {
        //        if (nt.FMH_Flag == 'F') {

        //            if (fine_totaladv == 0) {
        //                nt.isSelected1 = false;
        //            }

        //            if (nt.isSelected1 == undefined || nt.isSelected1 == false) {
        //                if (fine_totaladv > 0) {
        //                    nt.isSelected1 = true;
        //                }
        //            }
        //            nt.FCSS_ToBePaid = fine_totaladv + $scope.temp_finehead_amt;
        //            //commented on 27 sep 2021
        //            nt.fsS_TotalToBePaidaddfine = fine_totaladv + $scope.temp_finehead_amt_total;
        //            if ($scope.curramount > 0 && nt.isSelected1 != undefined) {
        //                //$scope.curramount -= Number(nt.FCSS_ToBePaid);
        //            }

        //            if (nt.isSelected1 == true) {
        //                $scope.totalfineadv = nt.FCSS_ToBePaid;
        //                $scope.curramount = $scope.curramount + Number(nt.FCSS_ToBePaid);
        //            }
        //            else if (nt.isSelected1 == false) {
        //                if ($scope.totalfineadv <= Number(nt.FCSS_ToBePaid)) {
        //                    //$scope.curramount = Number(nt.FCSS_ToBePaid) - $scope.totalfineadv;
        //                    $scope.totalfineadv = 0;
        //                }
        //                else {
        //                    $scope.curramount = Number(nt.FCSS_ToBePaid) - $scope.totalfineadv;
        //                }

        //            }
        //        }
        //    })
        //}

        $scope.get_modes = function () {
            $scope.FYP_Bank_Or_Cash = 'B';
            $scope.bankdetails = true;
            //$scope.FYP_DD_Cheque_Date = new Date();
            $scope.FYP_DD_Cheque_No = "";
            $scope.FYP_Bank_Name = "";
            $scope.Cash_Multiple = false;
            $scope.Cash_Amount = "";
            $scope.Bank_Multiple = false;
            $scope.Bank_Date = new Date();
            $scope.Bank_No = "";
            $scope.Bank_Name = "";
            $scope.Bank_Amount = "";
            $scope.Card_Multiple = false;
            $scope.Card_Date = new Date();
            $scope.Card_No = "";
            $scope.Card_Name = "";
            $scope.Card_Amount = "";
            $scope.R_N_Multiple = false;
            $scope.R_N_Date = new Date();
            $scope.R_N_No = "";
            $scope.R_N_Name = "";
            $scope.R_N_Amount = "";
        }

        $scope.chk_amount = function (type, amount_pay) {

            if (amount_pay != "" && amount_pay != null && amount_pay != undefined) {
                if (Number(amount_pay) == 0) {
                    swal("Amount Is Must Be Greater Than Zero");
                    if (type == 'Cash') {
                        $scope.Cash_Amount = "";
                    }
                    else if (type == 'Bank') {
                        $scope.Bank_Amount = "";
                    }
                    else if (type == 'Card') {
                        $scope.Card_Amount = "";
                    }
                    else if (type == 'R_N') {
                        $scope.R_N_Amount = "";
                    }
                }
                else if (Number(amount_pay) > 0) {
                    var amount = 0;
                    if ($scope.Cash_Multiple) {
                        amount += Number($scope.Cash_Amount);
                    }
                    if ($scope.Bank_Multiple) {
                        amount += Number($scope.Bank_Amount);
                    }
                    if ($scope.Card_Multiple) {
                        amount += Number($scope.Card_Amount);
                    }
                    if ($scope.R_N_Multiple) {
                        amount += Number($scope.R_N_Amount);
                    }
                    if (Number($scope.curramount) < Number(amount)) {
                        swal("Sum Of Amounts Must Match Now Paying Amount");
                        if (type == 'Cash') {
                            $scope.Cash_Amount = "";
                        }
                        else if (type == 'Bank') {
                            $scope.Bank_Amount = "";
                        }
                        else if (type == 'Card') {
                            $scope.Card_Amount = "";
                        }
                        else if (type == 'R_N') {
                            $scope.R_N_Amount = "";
                        }
                    }
                }
            }
        };

        $scope.chk_selectmode = function () {
            if ($scope.FYP_PayModeType == 'Single') {
                return false;
            }
            else if ($scope.FYP_PayModeType == 'Multiple') {
                if ($scope.Cash_Multiple) {
                    return false;
                }
                else if ($scope.Bank_Multiple) {
                    return false;
                }
                else if ($scope.Card_Multiple) {
                    return false;
                }
                else if ($scope.R_N_Multiple) {
                    return false;
                }
                else {
                    return true;
                }
            }
        };
        $scope.submitted1 = false;
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.clear_edit = function () {

            $scope.FYP_Id = 0;
            $scope.Stu_Name = "";
            $scope.FYP_PayModeType_E = 'Single';
            $scope.FYP_ReceiptDate_E = new Date();
            $scope.FYP_ReceiptNo_E = "";
            $scope.FYP_Remarks_E = "";
            $scope.FYP_Bank_Or_Cash_E = 'B';
            $scope.FYP_DD_Cheque_Date_E = new Date();
            $scope.FYP_DD_Cheque_No_E = "";
            $scope.FYP_Bank_Name_E = "";

            $scope.Cash_Multiple_E = false;
            $scope.Cash_Amount_E = "";


            $scope.Bank_Multiple_E = false;
            $scope.Bank_Date_E = new Date();
            $scope.Bank_No_E = "";
            $scope.Bank_Name_E = "";
            $scope.Bank_Amount_E = "";

            $scope.Card_Multiple_E = false;
            $scope.Card_Date_E = new Date();
            $scope.Card_No_E = "";
            $scope.Card_Name_E = "";
            $scope.Card_Amount_E = "";

            $scope.R_N_Multiple_E = false;
            $scope.R_N_Date_E = new Date();
            $scope.R_N_No_E = "";
            $scope.R_N_Name_E = "";
            $scope.R_N_Amount_E = "";

        }
        $scope.saveddata1 = function () {
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                var pay_modes = [];
                if ($scope.FYP_PayModeType_E == 'Multiple') {
                    if ($scope.Cash_Multiple_E) {
                        pay_modes.push({ FYPPM_TransactionTypeFlag: 'C', FYPPM_TotalPaidAmount: Number($scope.Cash_Amount_E), FYPPM_BankName: '', FYPPM_DDChequeNo: '', FYPPM_DDChequeDate: new Date().toDateString() });
                    }
                    if ($scope.Bank_Multiple_E) {
                        pay_modes.push({ FYPPM_TransactionTypeFlag: 'B', FYPPM_TotalPaidAmount: Number($scope.Bank_Amount_E), FYPPM_BankName: $scope.Bank_Name_E, FYPPM_DDChequeNo: $scope.Bank_No_E, FYPPM_DDChequeDate: new Date($scope.Bank_Date_E).toDateString() });
                    }
                    if ($scope.Card_Multiple_E) {
                        pay_modes.push({ FYPPM_TransactionTypeFlag: 'S', FYPPM_TotalPaidAmount: Number($scope.Card_Amount_E), FYPPM_BankName: $scope.Card_Name_E, FYPPM_DDChequeNo: $scope.Card_No_E, FYPPM_DDChequeDate: new Date($scope.Card_Date_E).toDateString() });
                    }
                    if ($scope.R_N_Multiple_E) {
                        pay_modes.push({ FYPPM_TransactionTypeFlag: 'R', FYPPM_TotalPaidAmount: Number($scope.R_N_Amount_E), FYPPM_BankName: $scope.R_N_Name_E, FYPPM_DDChequeNo: $scope.R_N_No_E, FYPPM_DDChequeDate: new Date($scope.R_N_Date_E).toDateString() });
                    }
                }
                var data = {
                    "FYP_ReceiptNo": $scope.FYP_ReceiptNo_E,
                    "FYP_ReceiptDate": new Date($scope.FYP_ReceiptDate_E).toDateString(),
                    "FYP_PayModeType": $scope.FYP_PayModeType_E,
                    "FYP_Remarks": $scope.FYP_Remarks_E,
                    "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash_E,
                    "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date_E).toDateString(),
                    "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No_E,
                    "FYP_Bank_Name": $scope.FYP_Bank_Name_E,
                    "auto_receipt_flag": autoreceipt,
                    Modes: pay_modes,
                    "FYP_Id": $scope.FYP_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Update Record? ",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false,
                    showLoaderOnConfirm: true,
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("CollegeFeeTransaction/", data).
                                then(function (promise) {
                                    if (promise.returnval == "true") {
                                        if ($scope.FYP_Id > 0) {
                                            swal("Record Updated Successfully");
                                        }
                                        else {
                                            swal("Record Saved Successfully");
                                        }
                                    }
                                    else if (promise.returnval == "false") {
                                        if ($scope.FYP_Id > 0) {
                                            swal("Record Not Updated Successfully");
                                        }
                                        else {
                                            swal("Record Not Saved Successfully");
                                        }
                                    }
                                    else {
                                        swal(promise.returnval);
                                    }
                                    $state.reload();
                                })
                        }
                        else {
                            swal("Record Update Failed", "Failed");
                        }
                    });
            }

        }
        $scope.total_heads_amount = 0;
        $scope.Distribute_Amount = function (amt) {
            if (Number(amt) > 0) {
                // if ($scope.total_heads_amount == 0) {
                $scope.total_heads_amount = 0;
                angular.forEach($scope.totalgrid, function (hed) {
                    $scope.total_heads_amount += Number(hed.tobepaid_M);
                })
                //  }
                if (Number(amt) > $scope.total_heads_amount) {
                    $scope.all = false;
                    swal("Entered Amount Greater Than Of Total" + $scope.total_heads_amount);
                    $scope.PayingAmount = null;
                    angular.forEach($scope.totalgrid, function (hed) {
                        hed.isSelected = false;
                        hed.FCSS_ToBePaid = hed.tobepaid_M;
                    })
                }
                else {
                    angular.forEach($scope.totalgrid, function (hed) {
                        hed.isSelected = false;
                        hed.FCSS_ToBePaid = hed.tobepaid_M;
                    })
                    var keepGoing1 = true;
                    angular.forEach($scope.totalgrid, function (hd_ins) {
                        if (hd_ins.Head_Flag == 'SH') {
                            if (keepGoing1) {
                                if (Number(amt) >= Number(hd_ins.tobepaid_M)) {
                                    hd_ins.isSelected = true;
                                    hd_ins.FCSS_ToBePaid = Number(hd_ins.tobepaid_M);
                                    amt = (Number(amt) - Number(hd_ins.tobepaid_M));
                                }
                                else if (Number(amt) < Number(hd_ins.tobepaid_M)) {
                                    hd_ins.FCSS_ToBePaid = Number(amt);
                                    hd_ins.isSelected = true;
                                    amt = (Number(amt) - Number(hd_ins.FCSS_ToBePaid));
                                }
                                if (Number(amt) == 0) {
                                    keepGoing1 = false;
                                }
                            }
                        }
                    })
                    if (keepGoing1) {
                        angular.forEach($scope.installments_list, function (instal) {
                            angular.forEach($scope.groupcount, function (grp) {
                                if (grp.selected) {
                                    angular.forEach($scope.totalgrid, function (hd_ins) {
                                        if (hd_ins.FMG_Id == grp.fmG_Id && hd_ins.FTI_Id == instal.ftI_Id) {
                                            if (hd_ins.Head_Flag == 'H') {
                                                if (keepGoing1) {
                                                    if (Number(amt) >= Number(hd_ins.tobepaid_M)) {
                                                        hd_ins.isSelected = true;
                                                        hd_ins.FCSS_ToBePaid = Number(hd_ins.tobepaid_M);
                                                        amt = (Number(amt) - Number(hd_ins.tobepaid_M));
                                                    }
                                                    else if (Number(amt) < Number(hd_ins.tobepaid_M)) {
                                                        hd_ins.FCSS_ToBePaid = Number(amt);
                                                        hd_ins.isSelected = true;
                                                        amt = (Number(amt) - Number(hd_ins.FCSS_ToBePaid));
                                                    }
                                                    if (Number(amt) == 0) {
                                                        keepGoing1 = false;
                                                    }
                                                }
                                            }
                                        }
                                    })
                                }
                            })
                        })
                    }
                }

                $scope.totalconcession = 0;
                $scope.totalfine = 0;
                $scope.curramount = 0;
                $scope.totalwaived = 0;

                for (var u = 0; u < $scope.totalgrid.length; u++) {
                    if ($scope.totalgrid[u].isSelected) {
                        $scope.amtdetails($scope.totalgrid[u], $scope.totalgrid, u);
                    }
                }
            }
        }

        //advance collection
        $scope.toggleAllAdvance = function (allchkdata1) {

            $scope.disablefine = true;
            $scope.disablenetamount = true;
            $scope.disableconcession = true;
            var toggleStatus = $scope.adv;
            //$scope.curramount = 0;
            //$scope.totalconcession = 0;
            //$scope.totalfine = 0;
            //$scope.totalwaived = 0;

            angular.forEach($scope.totalgridadvance, function (itm) {
                itm.isSelected1 = toggleStatus;
            });

            if (allchkdata1 == true) {

                for (var index = 0; index < $scope.totalgridadvance.length; index++) {
                    if ($scope.totalgridadvance[index].FMH_Flag != "F") {
                        $scope.totalconcession = Number($scope.totalconcession) + Number($scope.totalgridadvance[index].FCSS_ConcessionAmount);
                        $scope.totalfine = Number($scope.totalfine) + Number($scope.totalgrid[index].FCSS_FineAmount);
                        $scope.curramount = Number($scope.curramount) + Number($scope.totalgridadvance[index].FCSS_ToBePaid) + Number($scope.totalgridadvance[index].FCSS_OBArrearAmount);
                        $scope.totalwaived = Number($scope.totalwaived) + Number($scope.totalgridadvance[index].FCSS_WaivedAmount);
                    }
                }
            }
            else {
                //$scope.totalconcession = 0;
                //$scope.totalfine = 0;
                //$scope.curramount = 0;
                //$scope.totalwaived = 0;

                for (var index = 0; index < $scope.totalgridadvance.length; index++) {
                    if ($scope.totalgridadvance[index].FMH_Flag != "F") {
                        $scope.totalconcession = Number($scope.totalconcession) - Number($scope.totalgridadvance[index].FCSS_ConcessionAmount);
                        $scope.totalfine = Number($scope.totalfine) - Number($scope.totalgrid[index].FCSS_FineAmount);
                        $scope.curramount = Number($scope.curramount) - Number($scope.totalgridadvance[index].FCSS_ToBePaid) + Number($scope.totalgridadvance[index].FCSS_OBArrearAmount);
                        $scope.totalwaived = Number($scope.totalwaived) - Number($scope.totalgridadvance[index].FCSS_WaivedAmount);
                    }
                }

            }
            for (var index = 0; index < $scope.totalgridadvance.length; index++) {
                if ($scope.totalgridadvance.isSelected1 == true) {

                    $scope.curramount = Number($scope.curramount) + Number($scope.totalgridadvance[index].FCSS_ToBePaid);

                }
            }

            //MB for Fine
            if (autoreceipt == 1) {
                $scope.get_grp_reptno();
            }
            else {
                if (fineapplicable == true) {
                    $scope.calculate_fine();
                }
            }
            //MB for Fine
            if (allchkdata1 == true) {
                for (var index = 0; index < $scope.totalgridadvance.length; index++) {
                    if ($scope.totalgridadvance[index].FMH_FeeName == "Fine" || $scope.totalgridadvance[index].FMH_Flag == "F") {
                        $scope.curramount = $scope.curramount + Number($scope.totalgridadvance[index].FCSS_ToBePaid)
                    }
                }
            }
            //MB
            $scope.get_modes();

            $scope.calculatepayableamount();
        }


        $scope.tobepaidamtadvance = function (totalgridadvance, index) {
            var count = 0, intertobepaidamt = 0;
            angular.forEach($scope.totalgridadvance, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })
            if (count <= 1) {
                if (Number(totalgridadvance[index].FCSS_TotalCharges) >= Number(totalgridadvance[index].FCSS_ToBePaid)) {
                    $scope.curramount = Number(totalgridadvance[index].FCSS_ToBePaid) + Number(totalgridadvance[index].FCSS_FineAmount) + Number(totalgridadvance[index].FCSS_OBArrearAmount);
                    //MB For Fine
                    if (totalgridadvance[index].FMH_Flag == 'F') {
                        $scope.totalfine = Number(totalgridadvance[index].FCSS_ToBePaid) + Number(totalgridadvance[index].FCSS_FineAmount);
                    }
                    //MB For Fine
                }
                else if ((Number(totalgridadvance[index].FCSS_TotalCharges) <= Number(totalgridadvance[index].FCSS_ToBePaid)) && Number(totalgridadvance[index].FCSS_TotalCharges) > 0) {
                    swal("Entered Amount is greater than Netamount");
                    totalgridadvance[index].FCSS_ToBePaid = Number(totalgridadvance[index].FCSS_TotalCharges);
                    $scope.curramount = Number(totalgridadvance[index].FCSS_TotalCharges) + Number(totalgridadvance[index].FCSS_OBArrearAmount);
                    if (totalgridadvance[index].FMH_Flag == 'F') {
                        $scope.totalfine = Number(totalgridadvance[index].FCSS_TotalCharges);
                    }
                }
                else if (Number(totalgridadvance[index].FCSS_TotalCharges) == 0) {
                    $scope.curramount = Number(totalgridadvance[index].FCSS_ToBePaid) + Number(totalgridadvance[index].FCSS_OBArrearAmount);
                    if (totalgridadvance[index].FMH_Flag == 'F') {
                        $scope.totalfine = Number(totalgridadvance[index].FCSS_ToBePaid);
                    }
                }
            }
            else if (count > 1) {
                if (Number(totalgrid[index].FCSS_TotalCharges) >= Number(totalgridadvance[index].FCSS_ToBePaid)) {
                    angular.totalgridadvance($scope.totalgridadvance, function (user) {
                        if (!!user.isSelected) {
                            intertobepaidamt = Number(intertobepaidamt) + Number(user.FCSS_ToBePaid) + Number(user.FCSS_OBArrearAmount);
                        }
                    })
                    $scope.curramount = intertobepaidamt;
                    if (user.FMH_Flag == 'F') {
                        $scope.totalfine = Number(user.FCSS_ToBePaid);
                    }
                }
                else if ((Number(totalgridadvance[index].FCSS_TotalCharges) <= Number(totalgridadvance[index].FCSS_ToBePaid)) && Number(totalgridadvance[index].FCSS_TotalCharges)) {
                    swal("Entered Amount is greater than Netamount");
                    totalgridadvance[index].FCSS_ToBePaid = Number(totalgridadvance[index].FCSS_TotalCharges);
                    angular.forEach($scope.totalgridadvance, function (user) {
                        if (!!user.isSelected) {
                            intertobepaidamt = Number(intertobepaidamt) + Number(user.FCSS_ToBePaid);
                        }
                    })
                    $scope.curramount = intertobepaidamt;
                    if (user.FMH_Flag == 'F') {
                        $scope.totalfine = Number(user.FCSS_ToBePaid);
                    }
                }
                else if (Number(totalgridadvance[index].FCSS_TotalCharges) == 0) {
                    angular.forEach($scope.totalgridadvance, function (user) {
                        if (!!user.isSelected) {
                            intertobepaidamt = Number(intertobepaidamt) + Number(user.FCSS_ToBePaid);
                        }
                    })
                    $scope.curramount = intertobepaidamt;
                    if (user.FMH_Flag == 'F') {
                        $scope.totalfine = Number(user.FCSS_ToBePaid);
                    }
                }
            }

            $scope.calculatepayableamount();
        };

        $scope.advancedetails = function (userdata, totalgridadvance, index) {
            trp = [];
            var newCol = "";
            newCol = {
                user_data: userdata, total_grid: totalgridadvance, in_dex: index
            }
            trp.push(newCol);
            $scope.disablefine = true;
            $scope.disablenetamount = true;

            if (totalgridadvance[index].isSelected1 == true) {
                $scope.totalconcession = Number($scope.totalconcession) + Number(totalgridadvance[index].FCSS_ConcessionAmount);
                $scope.totalfine = Number($scope.totalfine) + Number(totalgridadvance[index].FCSS_FineAmount);
                $scope.curramount = Number($scope.curramount) + Number(totalgridadvance[index].FCSS_ToBePaid) + Number(totalgridadvance[index].FCSS_FineAmount) + Number(totalgridadvance[index].FCSS_OBArrearAmount);
                $scope.totalwaived = Number($scope.totalwaived) + Number(totalgridadvance[index].FCSS_WaivedAmount);
                if (totalgridadvance[index].fmH_FeeName != "Fine") {

                }
            }
            else if (totalgridadvance[index].isSelected1 == false) {
                $scope.adv = false;
                $scope.totalconcession = Number($scope.totalconcession) - Number(totalgridadvance[index].FCSS_ConcessionAmount);
                $scope.totalfine = Number($scope.totalfine) - Number(totalgridadvance[index].FCSS_FineAmount);
                $scope.curramount = Number($scope.curramount) - Number(totalgridadvance[index].FCSS_ToBePaid) - Number(totalgridadvance[index].FCSS_FineAmount) - Number(totalgridadvance[index].FCSS_OBArrearAmount);
                $scope.totalwaived = Number($scope.totalwaived) - Number(totalgridadvance[index].FCSS_WaivedAmount);
                if (totalgridadvance[index].fmH_FeeName != "Fine") {
                }
            }

            if (fineapplicable == true) {
                $scope.calculate_fine();
            }
            $scope.get_modes();

            $scope.calculatepayableamount();
        };

        //advance collection

        $scope.calculatefine = function (date) {
            $scope.FYP_ReceiptDate = date;
            $scope.onselectgroup();

            angular.forEach($scope.totalgrid, function (user) {
                user.isSelected = false;
            })
            $scope.all = false;

        }


        $scope.calculatepayableamount = function () {
            $scope.tobeapid_amount = 0;
            $scope.fineamt_amount = 0;
            //for regular
            for (var index = 0; index < $scope.totalgrid.length; index++) {
                if ($scope.totalgrid[index].isSelected === true) {
                        $scope.tobeapid_amount += Number($scope.totalgrid[index].FCSS_ToBePaid);
                        //$scope.totalconcession = Number($scope.totalconcession) + Number($scope.totalgrid[index].FCSS_ConcessionAmount);
                        //$scope.totalfine = Number($scope.totalfine) + Number($scope.totalgrid[index].FCSS_FineAmount);
                        //$scope.curramount = Number($scope.curramount) + Number($scope.totalgrid[index].FCSS_ToBePaid) + Number($scope.totalgrid[index].FCSS_OBArrearAmount);
                        //$scope.totalwaived = Number($scope.totalwaived) + Number($scope.totalgrid[index].FCSS_WaivedAmount);
                }
            }
            //For Advance Fee
            if ($scope.totalgridadvance != null) {
                for (var index = 0; index < $scope.totalgridadvance.length; index++) {
                    if ($scope.totalgridadvance[index].isSelected1 === true) {
                        //$scope.totalconcession = Number($scope.totalconcession) + Number($scope.totalgridadvance[index].FCSS_ConcessionAmount);
                        //$scope.totalfine = Number($scope.totalfine) + Number($scope.totalgrid[index].FCSS_FineAmount);
                        //$scope.curramount = Number($scope.curramount) + Number($scope.totalgridadvance[index].FCSS_ToBePaid) + Number($scope.totalgridadvance[index].FCSS_OBArrearAmount);
                        //$scope.totalwaived = Number($scope.totalwaived) + Number($scope.totalgridadvance[index].FCSS_WaivedAmount);
                        $scope.tobeapid_amount += Number($scope.totalgridadvance[index].FCSS_ToBePaid);
                    }
                }
            }
            //Current Tobe Paid Amount
            $scope.curramount = $scope.tobeapid_amount;


            for (var index = 0; index < $scope.totalgrid.length; index++) {
                if ($scope.totalgrid[index].isSelected === true) {
                    if ($scope.totalgrid[index].FMH_Flag == "F") {
                        $scope.fineamt_amount +=  Number($scope.totalgrid[index].FCSS_ToBePaid);
                    }
                }
            }
            for (var index = 0; index < $scope.totalgridadvance.length; index++) {
                if ($scope.totalgridadvance[index].isSelected1 === true) {
                    if ($scope.totalgridadvance[index].FMH_Flag == "F") {
                        $scope.fineamt_amount += Number($scope.totalgridadvance[index].FCSS_ToBePaid);
                    }
                }
            }
            $scope.totalfine = $scope.fineamt_amount;
        }

        $scope.calculate_fine = function () {
            var fine_total = 0;
            $scope.totalfine = 0;
            $scope.totalfineadv = 0;
            $scope.finedetails = [];
            angular.forEach($scope.totalgrid, function (nt) {
                if (nt.isSelected) {
                    angular.forEach($scope.temp_Fine_Amounts, function (fn) {
                        if (fn.fcmaS_Id == nt.FCMAS_Id) {

                            if (nt.FMH_Flag != 'F') {
                                if ($scope.finedetails.length === 0) {

                                    $scope.finedetails.push({
                                        fine_total: fn.fine_Amt,
                                        fmgid: nt.FMG_Id,
                                        fcmasid: fn.fcmaS_Id
                                    })
                                }
                                else if ($scope.finedetails.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.finedetails, function (emp) {
                                        if (emp.fmgid === nt.FMG_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.finedetails.push({
                                            fine_total: fn.fine_Amt,
                                            fmgid: nt.FMG_Id,
                                            fcmasid: fn.fcmaS_Id
                                        })
                                    }
                                }
                            }
                        }
                    })
                }
            })

            angular.forEach($scope.totalgrid, function (nt) {
                var count = 0;
                if (nt.FMH_Flag == 'F') {

                    count = $scope.finedetails.filter((obj) => obj.fmgid === nt.FMG_Id).length;
                    if ($scope.finedetails.length == 0) {
                        nt.isSelected = false;
                    } else {
                        angular.forEach($scope.finedetails, function (fd) {
                            if (fd.fine_total == 0) {
                                nt.isSelected = false;
                            }
                            if (nt.isSelected == true && count == 0) {
                                nt.isSelected = false;
                            }
                            else if ((nt.isSelected == undefined || nt.isSelected == false) && nt.FMG_Id == fd.fmgid) {
                                nt.isSelected = true;
                                nt.FCSS_ToBePaid = fd.fine_total + $scope.temp_finehead_amt;
                                nt.fsS_TotalToBePaidaddfine = fd.fine_total + $scope.temp_finehead_amt_total;
                            }
                        });
                    }

                    if ($scope.curramount > 0 && nt.isSelected != undefined) {

                    }
                    if (nt.isSelected == true) {
                        $scope.totalfine = nt.FCSS_ToBePaid;
                        $scope.curramount = $scope.curramount + Number(nt.FCSS_ToBePaid);
                    }
                    else if (nt.isSelected == false) {
                        if ($scope.totalfine <= Number(nt.FCSS_ToBePaid)) {
                            $scope.totalfine = 0;
                        }
                        else {
                            $scope.curramount = Number(nt.FCSS_ToBePaid) - $scope.totalfine;
                        }

                    }
                }
            })
            var fine_totaladv = 0;
            $scope.finedetailsadvance = [];
            angular.forEach($scope.totalgridadvance, function (nt) {
                if (nt.isSelected1) {
                    angular.forEach($scope.temp_Fine_Amountsadv, function (fn) {
                        if (fn.fcmaS_Id == nt.FCMAS_Id) {


                            if (nt.FMH_Flag != 'F') {
                                if ($scope.finedetailsadvance.length === 0) {

                                    $scope.finedetailsadvance.push({
                                        fine_totaladv: fn.fine_Amt,
                                        fmgidadv: nt.FMG_Id,
                                        fcmasidadv: fn.fcmaS_Id,
                                        semname: nt.AMSE_SEMName,
                                    })
                                }
                                else if ($scope.finedetailsadvance.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.finedetailsadvance, function (emp) {
                                        if (emp.fmgidadv === nt.FMG_Id && emp.semname === nt.AMSE_SEMName) {
                                            intcount += 1;


                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.finedetailsadvance.push({
                                            fine_totaladv: fn.fine_Amt,
                                            fmgidadv: nt.FMG_Id,
                                            fcmasidadv: fn.fcmaS_Id,
                                            semname: nt.AMSE_SEMName,
                                        })
                                    }
                                }
                            }
                        }
                    })
                }
            })

            angular.forEach($scope.totalgridadvance, function (nt) {
                var countadv = 0;
                if (nt.FMH_Flag == 'F') {

                    countadv = $scope.finedetailsadvance.filter((obj) => obj.fmgidadv === nt.FMG_Id).length;
                    if ($scope.finedetailsadvance.length == 0) {
                        nt.isSelected1 = false;
                    }
                    else {
                        angular.forEach($scope.finedetailsadvance, function (fd) {
                            if (fd.fine_totaladv == 0) {
                                nt.isSelected1 = false;
                            }
                            if (nt.isSelected1 == true && countadv == 0) {
                                nt.isSelected1 = false;
                            }
                            else if ((nt.isSelected1 == undefined || nt.isSelected1 == false) && nt.FMG_Id == fd.fmgidadv && nt.AMSE_SEMName == fd.semname) {

                                nt.isSelected1 = true;
                                nt.FCSS_ToBePaid = fd.fine_totaladv + $scope.temp_finehead_amt;
                                nt.fsS_TotalToBePaidaddfine = fd.fine_totaladv + $scope.temp_finehead_amt_total;

                            }

                        });
                    }


                    if ($scope.curramount > 0 && nt.isSelected1 != undefined) {

                    }
                    if (nt.isSelected1 == true) {
                        $scope.totalfineadv = nt.FCSS_ToBePaid;
                        $scope.curramount = $scope.curramount + Number(nt.FCSS_ToBePaid);
                    }
                    else if (nt.isSelected1 == false) {
                        if ($scope.totalfineadv <= Number(nt.FCSS_ToBePaid)) {
                            $scope.totalfineadv = 0;
                        }
                        else {
                            $scope.curramount = Number(nt.FCSS_ToBePaid) - $scope.totalfineadv;
                        }

                    }
                }
            })
        }


        $scope.viewstatus = function (fypid) {
            var data = {
                "FYP_Id": fypid,
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CollegeFeeTransaction/viewstatus", data).
                then(function (promise) {

                    if (promise.translogresults.length > 0) {
                        $scope.responsestatuslogs = promise.translogresults[0].responsestatuslogs;
                        $scope.fyp_transaction_Id = promise.translogresults[0].order_id;
                        $scope.fyP_PaymentReference_Id = promise.translogresults[0].payment_id;

                        if (promise.razorpaytransactionlogs != null) {
                            if (promise.razorpaytransactionlogs.length > 0) {
                                if (promise.razorpaytransactionlogs.FMOT_PayGatewayType == "RAZORPAY") {
                                    $scope.fyppsT_Settlement_Date = promise.razorpaytransactionlogs[0].fyppsT_Settlement_Date;
                                }


                            }
                            else {
                                $scope.fyppsT_Settlement_Date = promise.translogresults[0].fyP_Date;
                            }

                        }
                        else {
                            $scope.fyppsT_Settlement_Date = promise.translogresults[0].fyP_Date;
                        }

                    }

                })
        }

    }
})();