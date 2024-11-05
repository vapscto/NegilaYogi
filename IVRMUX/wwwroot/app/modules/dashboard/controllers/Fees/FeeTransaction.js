(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeStudentTransactionController', FeeStudentTransactionController)

    FeeStudentTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI', '$compile']
    function FeeStudentTransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI, $compile) {

        $scope.studentsavedlist = true;
        //$scope.disbankname = false;
        //$scope.disbankname = true;
        $scope.printreceipt = false;
        $scope.searchby = 0;
        $scope.printview = true;
        $scope.termwiseamount = [];
        $scope.groupwiseamount = [];
        var studentphtoto = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";

        $scope.asmaY_Year = "";
        $scope.portalusername = "";

        $scope.disablwmodes = false;
        $scope.disableamtchk = false;
        $scope.classnamechange = false;
        $scope.sectionchange = false;
        $scope.filterdata3 == false;
        $scope.filterdata4 = false;

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

        var institutionid, automanualreceiptnotranum;

        $scope.obj.allsms = "true";
        $scope.obj.allemail = false;

        var grouporterm, autoreceipt, receiptformat, mergeinstallment, fineapplicable;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        institutionid = configsettings[0].mI_Id;
        if (configsettings[0].fmC_FeeSearchNoOfDigits != null) {
            $scope.searchby = configsettings[0].fmC_FeeSearchNoOfDigits;
        } else {
            $scope.searchby = 3;
        }
      
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transactionnumbering.length; i++) {
            if (transactionnumbering.length > 0) {
                if (transactionnumbering[i].imN_Flag == "Online") {
                    $scope.transnumbconfigurationsettingsassign = transactionnumbering[i];
                }
            }
        }

        $scope.Clear_Chaln_No = function () {
            $scope.FYP_ChallanNo = null;
            $scope.Clear_values_dep();
        }
        $scope.onselectclass = function () {

        }

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
            fineapplicable = configsettings[0].fmC_FineEnableDisable;

            //added Praveen
            if (configsettings[0].fmC_AutoRecieptPrintFlag != null && configsettings[0].fmC_AutoRecieptPrintFlag != undefined) {
                $scope.fmC_AutoRecieptPrintFlag = configsettings[0].fmC_AutoRecieptPrintFlag;
            }
            //end
        }

        if (admfigsettings.length > 0) {
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
            //$scope.showstudentname = true;
            studentphtoto = null;
            $('#blah').attr('src', studentphtoto);
            //MB for Challan
            $scope.Clear_Chaln_No();

            if ($scope.filterdata3 == true) {


                $scope.classnamechange = true;

            }
            else {
                $scope.classnamechange = false;
            }
            if ($scope.filterdata4 == true) {


                $scope.sectionchange = true;
                $scope.filterdata3 == true

            }
            else {
                $scope.sectionchange = false;
            }

            // }
            //MB for Challan
        }

        $scope.reloaddataleft = function () {

            if ($scope.filterdata2 == true || $scope.filterdata2 == undefined) {
                $scope.filterdata2 = false;
                $scope.filterdata1 = true;
                $scope.filterdata = "NameAdmno";
            }


            $scope.groupcount = [];
            $scope.grigview1 = false;
            $scope.obj.allgrouporterm = false;
            //$scope.showstudentname = true;
            studentphtoto = null;
            $('#blah').attr('src', studentphtoto);
            //MB for Challan
            $scope.Clear_Chaln_No();
            // }
            //MB for Challan
        }
        $scope.reloaddataclasswise = function () {

            if ($scope.filterdata3 == true || $scope.filterdata3 == undefined) {
                $scope.classnamechange = true;
           }
            else {
                $scope.classnamechange = false;
            }
        }


        $scope.reloaddatasectionwise = function () {

            if ($scope.filterdata4 == true || $scope.filterdata4 == undefined) {
                $scope.sectionchange = true;
                $scope.filterdata3 = true;
                $scope.classnamechange = true;
            }
            else {
                $scope.sectionchange = false;
                $scope.filterdata3 = false;
                $scope.classnamechange = false;
            }
        }


        $scope.reloaddatadeactive = function () {

            if ($scope.filterdata1 == true || $scope.filterdata1 == undefined) {
                $scope.filterdata1 = false;
                $scope.filterdata2 = true;
                $scope.filterdata = "NameAdmno";
            }


            $scope.groupcount = [];
            $scope.grigview1 = false;
            $scope.obj.allgrouporterm = false;
            //$scope.showstudentname = true;
            studentphtoto = null;
            $('#blah').attr('src', studentphtoto);
            //MB for Challan
            $scope.Clear_Chaln_No();
            // }
            //MB for Challan
        }

        $scope.searchbyclick = function (abc) {
            $scope.searchby = abc;
        };
        $scope.search = '';
        $scope.filterOnLocation = function (user1) {
            return angular.lowercase(user1.amsT_FirstName + ' ' + user1.amsT_MiddleName + ' ' + user1.amsT_LastName).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.classname).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.sectionname).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.amsT_AdmNo).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user1.fyP_Receipt_No).indexOf(angular.lowercase($scope.search)) >= 0 || JSON.stringify(user1.fyP_Tot_Amount).indexOf($scope.search) >= 0 || ($filter('date')(user1.fyP_Date, 'dd-MM-yyyy').indexOf($scope.search) >= 0);

        };


        $scope.fypdate_change = function () {
            var selectcnt = 0;
            angular.forEach($scope.groupcount, function (obj) {
                if (obj.selected) {
                    selectcnt += 1;
                }
              

            })

            if (selectcnt > 0) {
                $scope.totalgrid = [];
                $scope.onselectgroupdatechange();
            }

        }
        $scope.showreceiptno = true;
        $scope.bankdetails = false;

        $scope.totalfee = 0;
        $scope.totalconcession = 0;
        $scope.totalfine = 0;
        $scope.curramount = 0;
        $scope.currbalance = 0;
        $scope.totalwaived = 0;
        $scope.optradio = true;
        $scope.cfg = {};
        $scope.rebateflag = false;
        $scope.rebateamount = 0;
        $scope.formload = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10

            var pageid = 1;

            apiService.getURI("FeeStudentTransaction/getalldetails", pageid).
                then(function (promise) {

                    //$scope.cfg.ASMAY_Id = promise.asmaY_Id;
                    //$scope.asmaY_Year = promise.asmaY_Year;

                    $scope.rolenamelist = promise.rolename;

                    $scope.yearlst = promise.yearlist;
                    $scope.Classlist = promise.classlist;
                    $scope.Sectionlist = promise.sectionlist;

                    //$scope.yearlst = academicyrlst;
                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                    //$scope.studentlst = promise.fillstudent;
                    //$scope.yearlst = promise.fillyear;

                    $scope.loginid = promise.userid;
                    if (promise.transnumconfig != null) {
                        if (promise.transnumconfig.length > 0) {

                            automanualreceiptnotranum = promise.transnumconfig[0].imN_AutoManualFlag;

                        }
                    }

                    $scope.receiptgrid = promise.receiparraydelete;

                    $scope.FYP_Date = new Date();
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

                        mergeinstallment = promise.feeconfiglist[0].fmC_RInstallmentsMergeFlag;//added by kiran

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
                        $scope.rebateflag = promise.feeconfiglist[0].fmC_RebateAplicableFlg;

                        if ($scope.rebateflag == null || $scope.rebateflag == undefined || $scope.rebateflag == false) {
                            $scope.rebateflag = false;
                        }
                        else {
                            $scope.rebateflag = true;
                        }
                        receiptformat = promise.feeconfiglist[0].fmC_Receipt_Format;

                        autoreceipt = promise.feeconfiglist[0].fmC_AutoReceiptFeeGroupFlag;

                        if (grouporterm == "T") {
                            $scope.grouportername = "Term Name"
                        }
                        else if (grouporterm == "G") {
                            $scope.grouportername = "Group Name"
                        }
                    }

                    if (promise.feeconfiglist[0].fmC_AutoReceiptFeeGroupFlag == 1 || promise.transnumconfig[0].imN_AutoManualFlag == "Auto") {
                        $scope.showreceiptno = false;
                    }
                    else {
                        $scope.showreceiptno = true;
                    }

                    $scope.getdates(promise.asmaY_Id, promise.asmaY_Year);

                    //MB for Special
                    $scope.special_head_list = promise.specialheadlist;
                    $scope.special_head_details = promise.specialheaddetails;
                    //MB for Special


                    $scope.loadmodulefeedback();
                })



            $scope.loadmodulefeedback = function () {


                var data = {
                    "Flag": "Fees"

                };
                apiService.create("FeedbackTransaction/loadfeedbackquestion", data).then(function (promise) {

                    if (promise.feedbackquestion !== undefined && promise.feedbackquestion !== null && promise.feedbackquestion.length > 0) {

                        $scope.feedbackquestion = promise.feedbackquestion;
                        $scope.feedbackoption = promise.feedbackoption;

                        $scope.TempGetFeedbackOption = [];

                        angular.forEach($scope.feedbackquestion, function (fqe) {
                            $scope.TempGetFeedbackOption = [];
                            angular.forEach($scope.feedbackoption, function (fop) {
                                if (fqe.fmtY_Id == fop.fmtY_Id && fqe.fmqE_Id == fop.fmqE_Id) {
                                    $scope.TempGetFeedbackOption.push(fop)
                                }
                            });

                            fqe.feedbackoptiondata = $scope.TempGetFeedbackOption;

                        })
                        $("#feedback").modal('show');
                    }
                });

            };




            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }


        // Feedback Save 17-02-2024
        $scope.submitted1 = false;

        $scope.Savefeedback = function () {
            if ($scope.myForm1.$valid) {
                //fp.fmoP_Id > 0 && 
                $scope.temp = [];
                angular.forEach($scope.feedbackquestion, function (fop) {
                    angular.forEach(fop.feedbackoptiondata, function (fp) {
                        if (fop.name == fp.fmoP_Id && fop.fmqE_Id == fp.fmqE_Id && fp.fmoP_FeedbackOptions != "") {
                            $scope.temp.push({
                                name: fp.fmoP_Id,
                                FMOP_FeedbackOptions: fp.fmoP_FeedbackOptions,
                                FMOP_OptionsValue: fp.fmoP_OptionsValue,
                                FMTY_Id: fop.fmtY_Id,
                                FMQE_Id: fp.fmqE_Id,
                                FMQE_FeedbackQuestions: fop.fmqE_FeedbackQuestions,
                                FMQE_FeedbackQRemarks: fop.fmqE_FeedbackQRemarks,
                                FMTY_FeedbackTypeName: fop.fmtY_FeedbackTypeName,
                                FSTTR_FeedBack: fop.fsttR_FeedBack

                            })
                        }

                    });
                });



                var data = {
                    "savemodulefeedback": $scope.temp

                };
                apiService.create("FeedbackTransaction/Savefeedback", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        }
                        else {
                            swal("Failed Saved/Updated Record");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };


        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.clear_first_tab = function () {
            $state.reload();
        };



        $scope.temptermarray = [];
        $scope.groupidss = {};

        //MB for Special
        var remove_list = [];
        var ins_spe_list = [];
        //MB for Special


        $scope.onselectgroupdatechange = function () {

            if ($scope.studentviewdetails.length > 0) {
                $scope.mtotalfee = $scope.studentviewdetails[0].fsS_CurrentYrCharges;
                $scope.mtotcon = $scope.studentviewdetails[0].fsS_ConcessionAmount;
                $scope.mtotwaived = $scope.studentviewdetails[0].fsS_WaivedAmount;
                $scope.mtotbalance = $scope.studentviewdetails[0].fsS_ToBePaid;
            }

          

            if ($scope.FYP_Date == '--') {
                $scope.FYP_Date = new Date();
            }

            $scope.all = false;

            $scope.curramount = 0;

            $scope.temptermarray = [];
            var groupid = "0";

            $scope.FYP_Bank_Name = ""
            $scope.FYP_DD_Cheque_No = "";
            $scope.FYP_Remarks = "";
            $scope.FYP_DD_Cheque_Date = new Date();
            // var groupid = option.fmG_Id;

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
                $scope.obj.allgrouporterm = true;
            }
            else {
                $scope.obj.allgrouporterm = false;
            }

            if (countf > 0) {
                var data = {
                    "Amst_Id": $scope.Amst_Id.amst_Id,
                    "multiplegroups": groupid,
                    //"modeofpayment": $scope.FYP_Bank_Or_Cash,
                    "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                    "filterinitialdata": $scope.filterdata1,
                    "configset": grouporterm,
                    "minstall": mergeinstallment, //added by kiran
                    "ASMAY_ID": $scope.cfg.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeStudentTransaction/getgroupmappedheadsnew", data).
                    then(function (promise) {
                        if (grouporterm == "T") {
                            $scope.termwiseamount = promise.termwiseamount;
                        }
                        if (grouporterm == "G") {
                            $scope.groupwiseamount = promise.groupwiseamount;
                        }
                        if (promise.narrationlist != null) {
                            if (promise.narrationlist.length > 0) {
                                $scope.narrationlist = promise.narrationlist;
                            }
                        }

                        //MULTIMODE
                        if (promise.fetchmodeofpayment.length > 0) {
                            angular.forEach(promise.fetchmodeofpayment, function (ed) {
                                if (ed.ivrmmoD_ModeOfPayment == 'Bank') {
                                    ed.rdo_Bank_Multiple = 'singlee';
                                    //ed.Bank_Date = new Date();
                                }
                                else {
                                    ed.rdo_Bank_Multiple = 'null';
                                }
                                if (ed.ivrmmoD_ModeOfPayment == 'Bank') {
                                    ed.chk_Bank_Multiple = true;
                                    //ed.Bank_Date = new Date();
                                }
                                else {
                                    ed.chk_Bank_Multiple = 'null';
                                }
                                ed.Bank_Date = new Date();
                                //ed.chk_Bank_Multiple = false;
                                //ed.rdo_Bank_Multiple = false;
                            })
                            $scope.paymodeee = promise.fetchmodeofpayment;
                        }
                        //MULTIMODE

                        if (promise.alldata.length > 0) {
                            $scope.grigview1 = true;
                            //MB for Fine
                            angular.forEach(promise.alldata, function (ed) {
                                if (ed.fmH_Flag == 'F') {
                                    $scope.temp_finehead_amt = ed.fsS_ToBePaid;
                                    $scope.temp_finehead_amt_total = ed.fsS_TotalToBePaid + ed.fsS_FineAmount;
                                }

                            })
                            $scope.temp_Fine_Amounts = promise.fine_FMA_Ids;
                            //MB for Fine
                            //added on 09102017
                            $scope.highestcountgid = promise.validationgroupid;

                            //hema

                            // if (option.selected == true) {

                            for (var i = 0; i < promise.alldata.length; i++) {
                                $scope.temptermarray.push(promise.alldata[i]);

                            }
                           
                            var addfinetonetamount, addnetamount, totalpayableamount;
                            $scope.totalgrid = $scope.temptermarray;

                            for (var i = 0; i < $scope.totalgrid.length; i++) {
                                addfinetonetamount = $scope.totalgrid[i].fsS_FineAmount
                                addnetamount = $scope.totalgrid[i].fsS_TotalToBePaid
                                if (Number(addfinetonetamount) > 0) {
                                    $scope.totalgrid[i].fsS_TotalToBePaidaddfine = Number(addfinetonetamount) + Number(addnetamount);
                                }
                                else {
                                    $scope.totalgrid[i].fsS_TotalToBePaidaddfine = Number(addnetamount);
                                }
                            }

                            var totalamt = 0;
                            var balanceamt = 0;
                            var concessionaamt = 0;
                            var fineamt = 0;

                            angular.forEach($scope.totalgrid, function (value, key) {
                                if (value.amst_Id == 0) {
                                    totalamt = totalamt + value.totalamount;
                                    balanceamt = balanceamt + value.fsS_ToBePaid;
                                    concessionaamt = concessionaamt + value.fsS_ConcessionAmount;
                                    fineamt = fineamt + value.fsS_FineAmount;
                                }
                            })

                            $scope.totalfee = totalamt;
                            $scope.currbalance = balanceamt;


                            //MB for Special

                            $scope.temp_Head_Instl_list = [];
                            angular.forEach($scope.totalgrid, function (uy) {
                                uy.Head_Flag = 'H';
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
                                                if (op_m.fmH_Id == op2.fmH_ID && op_m.ftI_Id == ins.ftI_Id) {
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
                                        var fsS_CurrentYrCharges = 0;
                                        var fsS_TotalToBePaid = 0;
                                        var fsS_ConcessionAmount = 0;
                                        var fsS_FineAmount = 0;
                                        var fsS_ToBePaid = 0;
                                        var fsS_TotalToBePaidaddfine = 0;
                                        var fmG_Id = 0;
                                        var fmG_GroupName = '';
                                        var not_cnt = 0;
                                        var totamtt = 0;
                                        var fsS_OBArrearAmount = 0;
                                        angular.forEach(a2.sp_ind_list, function (a3) {
                                            if (fmG_Id == 0) {
                                                fmG_Id = a3.fmG_Id;
                                                fmG_GroupName = a3.fmG_GroupName;
                                            }
                                            else {
                                                if (fmG_Id != a3.fmG_Id) {
                                                    not_cnt += 1;
                                                }
                                            }

                                            fsS_CurrentYrCharges += a3.fsS_CurrentYrCharges;
                                            fsS_TotalToBePaid += a3.fsS_TotalToBePaid;
                                            fsS_ConcessionAmount += a3.fsS_ConcessionAmount;
                                            fsS_FineAmount += a3.fsS_FineAmount;
                                            fsS_ToBePaid += a3.fsS_ToBePaid;
                                            fsS_TotalToBePaidaddfine += a3.fsS_TotalToBePaid;
                                            fsS_OBArrearAmount += a3.fsS_OBArrearAmount;
                                        })
                                        if (not_cnt == 0) {
                                            $scope.temp_Head_Instl_list.push({ fmG_Id: fmG_Id, fmG_GroupName: 'SH_' + fmG_GroupName, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, totalamount: fsS_CurrentYrCharges, fsS_TotalToBePaid: fsS_TotalToBePaid, fsS_ConcessionAmount: fsS_ConcessionAmount, fsS_FineAmount: fsS_FineAmount, fsS_ToBePaid: fsS_ToBePaid, fsS_TotalToBePaidaddfine: fsS_TotalToBePaidaddfine, fsS_OBArrearAmount: fsS_OBArrearAmount, Head_Flag: 'SH' });
                                        }
                                        else if (not_cnt > 0) {
                                            $scope.temp_Head_Instl_list.push({ fmG_Id: 0, fmG_GroupName: 'Special_Head', fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, totalamount: fsS_CurrentYrCharges, fsS_TotalToBePaid: fsS_TotalToBePaid, fsS_ConcessionAmount: fsS_ConcessionAmount, fsS_FineAmount: fsS_FineAmount, fsS_ToBePaid: fsS_ToBePaid, fsS_TotalToBePaidaddfine: fsS_TotalToBePaidaddfine, fsS_OBArrearAmount: fsS_OBArrearAmount, Head_Flag: 'SH' });
                                        }

                                    })
                                })
                                $scope.totalgrid = $scope.temp_Head_Instl_list;
                            }
                            //MB for Special


                        }
                        else {
                            if (selectcnt > 0) {
                                swal("Student has paid amount for that Group")
                            }
                            else if (selectcnt == 0) {
                                $scope.grigview1 = false;
                                $scope.totalgrid = [];
                            }

                            //swal(promise.validationvalue);
                            // swal("Student has not mapped with any Package / Student has paid amount for that term")

                        }
                    })
            }

            else {
                $scope.grigview1 = false;
                $scope.totalgrid = "";
            }

        };


        $scope.onselectgroup = function (option, index) {

            if ($scope.studentviewdetails.length > 0) {
                $scope.mtotalfee = $scope.studentviewdetails[0].fsS_CurrentYrCharges;
                $scope.mtotcon = $scope.studentviewdetails[0].fsS_ConcessionAmount;
                $scope.mtotwaived = $scope.studentviewdetails[0].fsS_WaivedAmount;
                $scope.mtotbalance = $scope.studentviewdetails[0].fsS_ToBePaid;
            }

            if ($scope.grouportername === "Term Name") {
                var cnt = 0;
                if (cnt == 0) {
                    var keep_go = true;
                    angular.forEach($scope.groupcount, function (ty) {
                        if (ty.fmG_Id == option.fmG_Id) {
                            keep_go = false;
                        }
                        if (keep_go) {
                            if ((ty.disablepaisterms == false || ty.disablepaisterms == undefined)) {
                                if (!ty.selected) {
                                    swal("You have to select Terms in Order");
                                    $scope.groupcount[index].selected = false;
                                }
                            }
                        }
                    })
                }
            }


            if ($scope.FYP_Date == '--') {
                $scope.FYP_Date = new Date();
            }

            $scope.all = false;

            $scope.curramount = 0;

            $scope.temptermarray = [];
            var groupid = "0";

            $scope.FYP_Bank_Name = ""
            $scope.FYP_DD_Cheque_No = "";
            $scope.FYP_Remarks = "";
            $scope.FYP_DD_Cheque_Date = new Date();
            // var groupid = option.fmG_Id;

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
                $scope.obj.allgrouporterm = option.selected;
            }
            else {
                $scope.obj.allgrouporterm = false;
            }

            if (countf > 0) {
                var data = {
                    "Amst_Id": $scope.Amst_Id.amst_Id,
                    "multiplegroups": groupid,
                    //"modeofpayment": $scope.FYP_Bank_Or_Cash,
                    "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                    "filterinitialdata": $scope.filterdata1,
                    "configset": grouporterm,
                    "minstall": mergeinstallment, //added by kiran
                    "ASMAY_ID": $scope.cfg.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeStudentTransaction/getgroupmappedheadsnew", data).
                    then(function (promise) {
                        if (promise.bankname.length > 0) {
                            $scope.bankmaster = promise.bankname;
                        }
                        if (grouporterm == "T") {
                            $scope.termwiseamount = promise.termwiseamount;
                        }
                        if (grouporterm == "G") {
                            $scope.groupwiseamount = promise.groupwiseamount;
                        }
                        if (promise.narrationlist != null) {
                            if (promise.narrationlist.length > 0) {
                                $scope.narrationlist = promise.narrationlist;
                            }
                        }

                        //MULTIMODE
                        if (promise.fetchmodeofpayment.length > 0) {
                            angular.forEach(promise.fetchmodeofpayment, function (ed) {
                                if (ed.ivrmmoD_ModeOfPayment == 'Bank') {
                                    ed.rdo_Bank_Multiple = 'singlee';
                                    //ed.Bank_Date = new Date();
                                }
                                else {
                                    ed.rdo_Bank_Multiple = 'null';
                                }
                                if (ed.ivrmmoD_ModeOfPayment == 'Bank') {
                                    ed.chk_Bank_Multiple = true;
                                    //ed.Bank_Date = new Date();
                                }
                                else {
                                    ed.chk_Bank_Multiple = 'null';
                                }
                                ed.Bank_Date = new Date();
                                //ed.chk_Bank_Multiple = false;
                                //ed.rdo_Bank_Multiple = false;
                            })
                            $scope.paymodeee = promise.fetchmodeofpayment;
                        }
                        //MULTIMODE

                        if (promise.alldata.length > 0) {
                            $scope.grigview1 = true;
                            //MB for Fine
                            angular.forEach(promise.alldata, function (ed) {
                                if (ed.fmH_Flag == 'F') {
                                    $scope.temp_finehead_amt = ed.fsS_ToBePaid;
                                    $scope.temp_finehead_amt_total = ed.fsS_TotalToBePaid + ed.fsS_FineAmount;
                                }

                            })
                            $scope.temp_Fine_Amounts = promise.fine_FMA_Ids;
                            //MB for Fine
                            //added on 09102017
                            $scope.highestcountgid = promise.validationgroupid;

                            //hema

                            // if (option.selected == true) {

                            for (var i = 0; i < promise.alldata.length; i++) {
                                $scope.temptermarray.push(promise.alldata[i]);

                            }
                            //    }

                            //    } else {
                            //                for (var j = 0; j < $scope.temptermarray.length; j++) {
                            //                    var name = $scope.temptermarray[j].fmA_Id;
                            //                    for (var k = 0; k < promise.alldata.length; k++) {
                            //                        if (name == promise.alldata[k].fmA_Id) {
                            //                        $scope.temptermarray.splice(j, 1);
                            //                        j = j -1;
                            //                        break;
                            //                    }
                            //                }
                            //    }
                            //}

                            //hema
                            var addfinetonetamount, addnetamount, totalpayableamount;
                            $scope.totalgrid = $scope.temptermarray;

                            for (var i = 0; i < $scope.totalgrid.length; i++) {
                                addfinetonetamount = $scope.totalgrid[i].fsS_FineAmount
                                addnetamount = $scope.totalgrid[i].fsS_TotalToBePaid
                                if (Number(addfinetonetamount) > 0) {
                                    $scope.totalgrid[i].fsS_TotalToBePaidaddfine = Number(addfinetonetamount) + Number(addnetamount);
                                }
                                else {
                                    $scope.totalgrid[i].fsS_TotalToBePaidaddfine = Number(addnetamount);
                                }
                            }

                            var totalamt = 0;
                            var balanceamt = 0;
                            var concessionaamt = 0;
                            var fineamt = 0;

                            angular.forEach($scope.totalgrid, function (value, key) {
                                if (value.amst_Id == 0) {
                                    totalamt = totalamt + value.totalamount;
                                    balanceamt = balanceamt + value.fsS_ToBePaid;
                                    concessionaamt = concessionaamt + value.fsS_ConcessionAmount;
                                    fineamt = fineamt + value.fsS_FineAmount;
                                }
                            })

                            $scope.totalfee = totalamt;
                            $scope.currbalance = balanceamt;


                            //MB for Special

                            $scope.temp_Head_Instl_list = [];
                            angular.forEach($scope.totalgrid, function (uy) {
                                uy.Head_Flag = 'H';
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
                                                if (op_m.fmH_Id == op2.fmH_ID && op_m.ftI_Id == ins.ftI_Id) {
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
                                //$scope.totalgrid = $scope.temp_Head_Instl_list;
                                //angular.forEach($scope.temp_Head_Instl_list, function (r) {
                                //    r.Head_Flag = 'H';
                                //})
                                angular.forEach(ins_spe_list, function (a1) {

                                    angular.forEach(a1.sp_list, function (a2) {
                                        var fsS_CurrentYrCharges = 0;
                                        var fsS_TotalToBePaid = 0;
                                        var fsS_ConcessionAmount = 0;
                                        var fsS_FineAmount = 0;
                                        var fsS_ToBePaid = 0;
                                        var fsS_TotalToBePaidaddfine = 0;
                                        var fmG_Id = 0;
                                        var fmG_GroupName = '';
                                        var not_cnt = 0;
                                        var totamtt = 0;
                                        var fsS_OBArrearAmount = 0;
                                        angular.forEach(a2.sp_ind_list, function (a3) {
                                            if (fmG_Id == 0) {
                                                fmG_Id = a3.fmG_Id;
                                                fmG_GroupName = a3.fmG_GroupName;
                                            }
                                            else {
                                                if (fmG_Id != a3.fmG_Id) {
                                                    not_cnt += 1;
                                                }
                                            }

                                            fsS_CurrentYrCharges += a3.fsS_CurrentYrCharges;
                                            fsS_TotalToBePaid += a3.fsS_TotalToBePaid;
                                            fsS_ConcessionAmount += a3.fsS_ConcessionAmount;
                                            fsS_FineAmount += a3.fsS_FineAmount;
                                            fsS_ToBePaid += a3.fsS_ToBePaid;
                                            fsS_TotalToBePaidaddfine += a3.fsS_TotalToBePaid;
                                            fsS_OBArrearAmount += a3.fsS_OBArrearAmount;
                                        })
                                        if (not_cnt == 0) {
                                            $scope.temp_Head_Instl_list.push({ fmG_Id: fmG_Id, fmG_GroupName: 'SH_' + fmG_GroupName, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, totalamount: fsS_CurrentYrCharges, fsS_TotalToBePaid: fsS_TotalToBePaid, fsS_ConcessionAmount: fsS_ConcessionAmount, fsS_FineAmount: fsS_FineAmount, fsS_ToBePaid: fsS_ToBePaid, fsS_TotalToBePaidaddfine: fsS_TotalToBePaidaddfine, fsS_OBArrearAmount: fsS_OBArrearAmount, Head_Flag: 'SH' });
                                        }
                                        else if (not_cnt > 0) {
                                            $scope.temp_Head_Instl_list.push({ fmG_Id: 0, fmG_GroupName: 'Special_Head', fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, totalamount: fsS_CurrentYrCharges, fsS_TotalToBePaid: fsS_TotalToBePaid, fsS_ConcessionAmount: fsS_ConcessionAmount, fsS_FineAmount: fsS_FineAmount, fsS_ToBePaid: fsS_ToBePaid, fsS_TotalToBePaidaddfine: fsS_TotalToBePaidaddfine, fsS_OBArrearAmount: fsS_OBArrearAmount, Head_Flag: 'SH' });
                                        }

                                    })
                                })
                                $scope.totalgrid = $scope.temp_Head_Instl_list;
                            }
                            //MB for Special


                        }
                        else {
                            if (selectcnt > 0) {
                                swal("Student has paid amount for that Group")
                            }
                            else if (selectcnt == 0) {
                                $scope.grigview1 = false;
                                $scope.totalgrid = [];
                            }

                            //swal(promise.validationvalue);
                            // swal("Student has not mapped with any Package / Student has paid amount for that term")

                        }
                    })
            }

            else {
                $scope.grigview1 = false;
                $scope.totalgrid = "";
            }

        };

        $scope.onselectacademic = function (yearlst) {

            angular.forEach(yearlst, function (op_m) {
                if (op_m.asmaY_Id == $scope.cfg.ASMAY_Id) {
                    $scope.asmaY_Year = op_m.asmaY_Year
                }
            })

            var data = {
                "filterinitialdata": $scope.filterdata,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            //var radioselctionlst = $scope.filterdata
            //var academicyearid = $scope.ASMAY_Id;

            apiService.create("FeeStudentTransaction/getacademicyear", data).
                then(function (promise) {

                    //$scope.studentlst = promise.fillstudent;

                    $scope.receiptgrid = promise.receiparraydelete;

                    $scope.grigview1 = false;
                    $scope.groupcount = {};

                    $scope.amsT_FirstName = "";
                    $scope.amsT_MiddleName = "";
                    $scope.amsT_LastName = "";
                    $scope.amsT_AdmNo = "";
                    $scope.amsT_RegistrationNo = "";
                    $scope.amaY_RollNo = "";
                    $scope.amsT_mobile = "";
                    $scope.classname = "";
                    $scope.sectionname = "";
                    $scope.fathername = "";
                    $scope.studentdob = "";
                    $scope.route_name = "";

                    $scope.getdates(promise.asmaY_Id, $scope.asmaY_Year);

                })
        };


        //$scope.tobepaidamt = function (totalgrid, index) {
        //    var currentamt, totalpaidamt;

        //    currentamt = Number(totalgrid[index].fsS_ToBePaid);
        //    totalpaidamt = Number(totalgrid[index].fsS_TotalToBePaid);

        //    if(Number(totalpaidamt)< Number(currentamt))
        //    {
        //        swal("Entered amount is greater than Net Amount");
        //        totalgrid[index].fsS_ToBePaid = totalpaidamt;
        //    }
        //};


        $scope.tobepaidamt = function (totalgrid, index) {
            var count = 0, intertobepaidamt = 0;

            angular.forEach($scope.totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })

            if (count <= 1) {

                if (Number(totalgrid[index].fsS_TotalToBePaid) >= Number(totalgrid[index].fsS_ToBePaid)) {
                    $scope.curramount = Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_FineAmount);

                    //MB For Fine
                    if (totalgrid[index].fmH_Flag == 'F') {
                        $scope.totalfine = Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_FineAmount);
                    }
                    //MB For Fine
                }
                else if ((Number(totalgrid[index].fsS_TotalToBePaid) <= Number(totalgrid[index].fsS_ToBePaid)) && Number(totalgrid[index].fsS_TotalToBePaid) > 0) {
                    swal("Entered Amount is greater than Netamount");
                    totalgrid[index].fsS_ToBePaid = Number(totalgrid[index].fsS_TotalToBePaid);
                    $scope.curramount = Number(totalgrid[index].fsS_TotalToBePaid);

                    if (totalgrid[index].fmH_Flag == 'F') {
                        $scope.totalfine = Number(totalgrid[index].fsS_TotalToBePaid);
                    }

                }
                else if (Number(totalgrid[index].fsS_TotalToBePaid) == 0) {
                    $scope.curramount = Number(totalgrid[index].fsS_ToBePaid);

                    if (totalgrid[index].fmH_Flag == 'F') {
                        $scope.totalfine = Number(totalgrid[index].fsS_ToBePaid);
                    }

                }

                //if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount)  + Number(totalgrid[index].fsS_WaivedAmount) <= Number(totalgrid[index].totalamount))
                //{
                //    $scope.curramount = Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_FineAmount);
                //}

                //else if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) > Number(totalgrid[index].totalamount))
                //{
                //    swal("Entered amount is greater than Net Amount");
                //}
            }
            else if (count > 1) {

                if (Number(totalgrid[index].fsS_TotalToBePaid) >= Number(totalgrid[index].fsS_ToBePaid)) {

                    angular.forEach($scope.totalgrid, function (user) {
                        if (!!user.isSelected) {

                            intertobepaidamt = Number(intertobepaidamt) + Number(user.fsS_ToBePaid);
                        }
                    })

                    $scope.curramount = intertobepaidamt;

                    if (totalgrid[index].fmH_Flag == 'F') {
                        $scope.totalfine = Number(totalgrid[index].fsS_ToBePaid);
                    }

                }

                else if ((Number(totalgrid[index].fsS_TotalToBePaid) <= Number(totalgrid[index].fsS_ToBePaid)) && Number(totalgrid[index].fsS_TotalToBePaid)) {

                    swal("Entered Amount is greater than Netamount");

                    totalgrid[index].fsS_ToBePaid = Number(totalgrid[index].fsS_TotalToBePaid);

                    angular.forEach($scope.totalgrid, function (user) {
                        if (!!user.isSelected) {

                            intertobepaidamt = Number(intertobepaidamt) + Number(user.fsS_ToBePaid);
                        }
                    })

                    $scope.curramount = intertobepaidamt;

                    if (totalgrid[index].fmH_Flag == 'F') {
                        $scope.totalfine = Number(totalgrid[index].fsS_ToBePaid);
                    }

                }

                else if (Number(totalgrid[index].fsS_TotalToBePaid) == 0) {

                    angular.forEach($scope.totalgrid, function (user) {
                        if (!!user.isSelected) {

                            intertobepaidamt = Number(intertobepaidamt) + Number(user.fsS_ToBePaid);
                        }
                    })

                    $scope.curramount = intertobepaidamt;


                    if (totalgrid[index].fmH_Flag == 'F') {
                        $scope.totalfine = Number(totalgrid[index].fsS_ToBePaid);
                    }

                }
            }

            //MULTIMODE
            if ($scope.FYP_PayModeType === 'Single') {
                angular.forEach($scope.paymodeee, function (ed) {
                    if (ed.rdo_Bank_Multiple === 'singlee') {
                        ed.Bank_Amount = $scope.curramount;
                    }
                })
            }
            if ($scope.FYP_PayModeType === 'Multiple') {
                angular.forEach($scope.paymodeee, function (ed) {
                    if (ed.chk_Bank_Multiple === 'multii') {
                        ed.Bank_Amount = $scope.curramount;
                    }
                })
            }
            //MULTIMODE
        };



        $scope.concessionamt = function (totalgrid, index) {
            var count = 0;
            angular.forEach($scope.totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })

            //  var tobepaidamount = $scope.totalgrid[index].fsS_ToBePaid;
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

                    //$scope.curramount = Number($scope.curramount) - Number(totalgrid[index].fsS_ToBePaid);
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
                //$scope.totalconcession = Number($scope.totalconcession) + Number(totalgrid[index].fsS_ConcessionAmount)

            }
        };

        $scope.fineamt = function (totalgrid, index) {
            var count = 0;
            angular.forEach($scope.totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })

            //  var tobepaidamount = $scope.totalgrid[index].fsS_ToBePaid;
            if (count <= 1) {

                $scope.totalfine = Number(totalgrid[index].fsS_FineAmount);

                $scope.curramount = $scope.curramount + $scope.totalfine
            }
            else if (count > 1) {

                var interfineamt = 0
                // angular.forEach($scope.totalgrid, function (user) {
                //if (!!user.isSelected) {
                //    interfineamt = Number(interfineamt) + Number($scope.totalgrid[index].fsS_FineAmount);
                //}
                //interfineamt = Number(interfineamt) + Number($scope.totalgrid[index].fsS_FineAmount);
                // })

                interfineamt = Number(interfineamt) + Number($scope.totalgrid[index].fsS_FineAmount);

                $scope.totalfine = interfineamt + $scope.totalfine

                $scope.curramount = $scope.curramount + interfineamt
                //$scope.totalfine = Number($scope.totalfine) + Number(user.ftp_fine_amt)

            }
        };

        $scope.waivedoffamt = function (totalgrid, index) {
            var count = 0;
            angular.forEach($scope.totalgrid, function (user) {
                if (!!user.isSelected) {
                    count = count + 1
                }
            })

            //  var tobepaidamount = $scope.totalgrid[index].fsS_ToBePaid;
            if (count <= 1) {
                if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) <= Number(totalgrid[index].totalamount)) {
                    $scope.totalwaived = Number(totalgrid[index].fsS_WaivedAmount);
                }
                else if (Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_ConcessionAmount) + Number(totalgrid[index].fsS_WaivedAmount) > Number(totalgrid[index].totalamount)) {
                    swal("Entered amount is greater than Net Amount");
                }
            }
            else if (count > 1) {

                $scope.totalwaived = Number($scope.totalwaived) + Number(user.fsS_WaivedAmount)

            }
        };

        // $scope.amtdetails = function (totalgrid, index) {

        //    // $scope.totalfee = $scope.totalfee + totalgrid[index].fsS_ToBePaid;
        //     $scope.totalconcession = $scope.totalconcession + totalgrid[index].fsS_ConcessionAmount
        //     $scope.totalfine = $scope.totalfine + totalgrid[index].ftp_fine_amt;
        //     $scope.curramount = $scope.curramount + totalgrid[index].fsS_ToBePaid;
        //    // $scope.currbalance = $scope.currbalance + (totalgrid[index].fsS_ToBePaid - totalgrid[index].fsS_ToBePaid);
        //     $scope.totalwaived = $scope.totalwaived + totalgrid[index].fsS_WaivedAmount;

        ////     apiService.getURI("FeeStudentTransaction/getacademicyear", academicyearid).
        ////then(function (promise) {
        ////    $scope.studentlst = promise.fillstudent;
        ////})
        // };

        //$scope.fsS_ToBePaid = true;
        //$scope.fsS_ConcessionAmount = true;
        //$scope.ftp_fine_amt = true;
        //$scope.fsS_WaivedAmount = true;
        //$scope.ftp_rebate_amt = true;

        //  $scope.checked = {};
        //  $scope.checkedgrid = {};

        $scope.toggleAllgrouporterm = function (groupcount, obj) {

            if ($scope.studentviewdetails.length > 0) {
                $scope.mtotalfee = 0;
                $scope.mtotcon = 0;
                $scope.mtotwaived = 0;
                $scope.mtotbalance = 0;

                for (var r = 0; r < $scope.studentviewdetails.length; r++) {
                    $scope.mtotalfee = $scope.mtotalfee + $scope.studentviewdetails[r].fsS_CurrentYrCharges;
                    $scope.mtotcon = $scope.mtotcon + $scope.studentviewdetails[r].fsS_ConcessionAmount;
                    $scope.mtotwaived = $scope.mtotwaived + $scope.studentviewdetails[r].fsS_WaivedAmount;
                    $scope.mtotbalance = $scope.mtotbalance + $scope.studentviewdetails[r].fsS_ToBePaid;
                }
            }

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
                    "Amst_Id": $scope.Amst_Id.amst_Id,
                    "multiplegroups": gouportermcount,
                    // "modeofpayment": $scope.FYP_Bank_Or_Cash,
                    "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                    "filterinitialdata": $scope.filterdata,
                    "configset": grouporterm,
                    "ASMAY_ID": $scope.cfg.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeStudentTransaction/getgroupmappedheadsnew", data).
                    then(function (promise) {
                        if (promise.bankname.length > 0) {
                            $scope.bankmaster = promise.bankname;
                        }
                        if (grouporterm == "T") {
                            $scope.termwiseamount = promise.termwiseamount;
                        }
                        if (grouporterm == "G") {
                            $scope.groupwiseamount = promise.groupwiseamount;
                        }

                        if (promise.narrationlist != null) {
                            if (promise.narrationlist.length > 0) {
                                $scope.narrationlist = promise.narrationlist;
                            }
                        }


                        if (promise.fetchmodeofpayment.length > 0) {
                            angular.forEach(promise.fetchmodeofpayment, function (ed) {
                                if (ed.ivrmmoD_ModeOfPayment == 'Bank') {
                                    ed.rdo_Bank_Multiple = 'singlee';
                                }
                                else {
                                    ed.rdo_Bank_Multiple = 'null';
                                }
                                if (ed.ivrmmoD_ModeOfPayment == 'Bank') {
                                    ed.chk_Bank_Multiple = true;
                                }
                                else {
                                    ed.chk_Bank_Multiple = 'null';
                                }
                                ed.Bank_Date = new Date();
                            })
                            $scope.paymodeee = promise.fetchmodeofpayment;
                        }
                        //MULTIMODE

                        //MB for Challan
                        if ($scope.filterdata == 'Challan_No') {
                            $scope.grigview1 = true;
                            $scope.totalgrid = temp_alldata;
                            for (var i = 0; i < $scope.totalgrid.length; i++) {
                                addfinetonetamount = $scope.totalgrid[i].fsS_FineAmount
                                addnetamount = $scope.totalgrid[i].fsS_TotalToBePaid
                                if (Number(addfinetonetamount) > 0) {
                                    $scope.totalgrid[i].fsS_TotalToBePaidaddfine = Number(addfinetonetamount) + Number(addnetamount);
                                }
                                else {
                                    $scope.totalgrid[i].fsS_TotalToBePaidaddfine = Number(addnetamount);
                                }
                            }
                            var totalamt = 0;
                            var balanceamt = 0;
                            var concessionaamt = 0;
                            var fineamt = 0;

                            angular.forEach($scope.totalgrid, function (value, key) {
                                if (value.amst_Id == 0) {
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
                                $scope.temp_Fine_Amounts = promise.fine_FMA_Ids;
                                //MB for Fine
                                $scope.highestcountgid = promise.validationgroupid;
                                $scope.totalgrid = promise.alldata;

                                var addfinetonetamount, addnetamount, totalpayableamount;

                                for (var i = 0; i < $scope.totalgrid.length; i++) {
                                    addfinetonetamount = $scope.totalgrid[i].fsS_FineAmount
                                    addnetamount = $scope.totalgrid[i].fsS_TotalToBePaid
                                    if (Number(addfinetonetamount) > 0) {
                                        $scope.totalgrid[i].fsS_TotalToBePaidaddfine = Number(addfinetonetamount) + Number(addnetamount);
                                    }
                                    else {
                                        $scope.totalgrid[i].fsS_TotalToBePaidaddfine = Number(addnetamount);
                                    }
                                }

                                var totalamt = 0;
                                var balanceamt = 0;
                                var concessionaamt = 0;
                                var fineamt = 0;

                                angular.forEach($scope.totalgrid, function (value, key) {
                                    if (value.amst_Id == 0) {
                                        totalamt = totalamt + value.totalamount;
                                        balanceamt = balanceamt + value.fsS_ToBePaid;
                                        concessionaamt = concessionaamt + value.fsS_ConcessionAmount;
                                        fineamt = fineamt + value.fsS_FineAmount;
                                    }
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
                        //MB for Challan


                        //radha

                        //MB for Special

                        $scope.temp_Head_Instl_list = [];
                        angular.forEach($scope.totalgrid, function (uy) {
                            uy.Head_Flag = 'H';
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
                                            if (op_m.fmH_Id == op2.fmH_ID && op_m.ftI_Id == ins.ftI_Id) {
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
                            //$scope.totalgrid = $scope.temp_Head_Instl_list;
                            //angular.forEach($scope.temp_Head_Instl_list, function (r) {
                            //    r.Head_Flag = 'H';
                            //})
                            angular.forEach(ins_spe_list, function (a1) {

                                angular.forEach(a1.sp_list, function (a2) {
                                    var fsS_CurrentYrCharges = 0;
                                    var fsS_TotalToBePaid = 0;
                                    var fsS_ConcessionAmount = 0;
                                    var fsS_FineAmount = 0;
                                    var fsS_ToBePaid = 0;
                                    var fsS_TotalToBePaidaddfine = 0;
                                    var fmG_Id = 0;
                                    var fmG_GroupName = '';
                                    var not_cnt = 0;
                                    var totamtt = 0;
                                    var fsS_OBArrearAmount = 0;
                                    angular.forEach(a2.sp_ind_list, function (a3) {
                                        if (fmG_Id == 0) {
                                            fmG_Id = a3.fmG_Id;
                                            fmG_GroupName = a3.fmG_GroupName;
                                        }
                                        else {
                                            if (fmG_Id != a3.fmG_Id) {
                                                not_cnt += 1;
                                            }
                                        }

                                        fsS_CurrentYrCharges += a3.fsS_CurrentYrCharges;
                                        fsS_TotalToBePaid += a3.fsS_TotalToBePaid;
                                        fsS_ConcessionAmount += a3.fsS_ConcessionAmount;
                                        fsS_FineAmount += a3.fsS_FineAmount;
                                        fsS_ToBePaid += a3.fsS_ToBePaid;
                                        fsS_TotalToBePaidaddfine += a3.fsS_TotalToBePaid;
                                        fsS_OBArrearAmount += a3.fsS_OBArrearAmount;
                                    })
                                    if (not_cnt == 0) {
                                        $scope.temp_Head_Instl_list.push({ fmG_Id: fmG_Id, fmG_GroupName: 'SH_' + fmG_GroupName, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, totalamount: fsS_CurrentYrCharges, fsS_TotalToBePaid: fsS_TotalToBePaid, fsS_ConcessionAmount: fsS_ConcessionAmount, fsS_FineAmount: fsS_FineAmount, fsS_ToBePaid: fsS_ToBePaid, fsS_TotalToBePaidaddfine: fsS_TotalToBePaidaddfine, fsS_OBArrearAmount: fsS_OBArrearAmount, Head_Flag: 'SH' });
                                    }
                                    else if (not_cnt > 0) {
                                        $scope.temp_Head_Instl_list.push({ fmG_Id: 0, fmG_GroupName: 'Special_Head', fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, totalamount: fsS_CurrentYrCharges, fsS_TotalToBePaid: fsS_TotalToBePaid, fsS_ConcessionAmount: fsS_ConcessionAmount, fsS_FineAmount: fsS_FineAmount, fsS_ToBePaid: fsS_ToBePaid, fsS_TotalToBePaidaddfine: fsS_TotalToBePaidaddfine, fsS_OBArrearAmount: fsS_OBArrearAmount, Head_Flag: 'SH' });
                                    }

                                })
                            })
                            $scope.totalgrid = $scope.temp_Head_Instl_list;
                        }
                        //MB for Special

                        //radha


                    }
                    )
            }
            else {
                $scope.grigview1 = false;
                $scope.totalgrid = "";
            }
        }

        $scope.narrationchange = function () {

            for (var i = 0; i < $scope.narrationlist.length; i++) {
                if ($scope.FMNAR_Id == $scope.narrationlist[i].fmnaR_Id) {

                    $scope.FYP_Remarks = $scope.narrationlist[i].fmnaR_Narration;
                }

            }

        


        }

        $scope.toggleAll = function (allchkdata) {


            $scope.disablefine = true;
            $scope.disablenetamount = true;
            $scope.disableconcession = true;


            var toggleStatus = $scope.all;

            $scope.curramount = 0;
            $scope.totalconcession = 0;
            $scope.totalfine = 0;
            $scope.totalwaived = 0;

            angular.forEach($scope.totalgrid, function (itm) {
                itm.isSelected = toggleStatus;
            });


            if (allchkdata == true) {

                for (var index = 0; index < $scope.totalgrid.length; index++) {
                   
                        if ($scope.totalgrid[index].fmH_Flag != "F") {
                            $scope.totalconcession = Number($scope.totalconcession) + Number($scope.totalgrid[index].fsS_ConcessionAmount);
                            $scope.totalfine = Number($scope.totalfine) + Number($scope.totalgrid[index].fsS_FineAmount);

                            $scope.curramount = Number($scope.curramount) + Number($scope.totalgrid[index].fsS_ToBePaid);
                            $scope.totalwaived = Number($scope.totalwaived) + Number($scope.totalgrid[index].fsS_WaivedAmount);
                        }
                    

                }

            }
            else {
                $scope.totalconcession = 0;
                $scope.totalfine = 0;
                $scope.curramount = 0;
                $scope.totalwaived = 0;
            }

            for (var index = 0; index < $scope.totalgrid.length; index++) {
                if ($scope.totalgrid.isSelected == true) {

                    $scope.curramount = Number($scope.curramount) + Number($scope.totalgrid[index].fsS_ToBePaid);

                }
            }
            //MB for Fine

            if (autoreceipt == 1) {
                $scope.get_grp_reptno();
            }
            // else {
            if ($scope.filterdata != 'Challan_No') {
                if (fineapplicable == true) {
                    $scope.calculate_fine();
                }
            }
                
                if ($scope.FYP_PayModeType === 'Single') {
                    angular.forEach($scope.paymodeee, function (ed) {
                        if (ed.rdo_Bank_Multiple === 'singlee') {
                            ed.Bank_Amount = $scope.curramount - rebateamount;
                        }
                    })
                }
                if ($scope.FYP_PayModeType === 'Multiple') {
                    angular.forEach($scope.paymodeee, function (ed) {
                        if (ed.chk_Bank_Multiple === 'multii') {
                            ed.Bank_Amount = $scope.curramount - rebateamount;
                        }
                    })
                }
         //   }
            //MB for Fine

            //for (var index = 0; index < $scope.totalgrid.length; index++) {
            //    if ($scope.totalgrid.isSelected == true) {
            //        if ($scope.totalgrid[index].fmH_FeeName != "Fine") {
            //           // $scope.currbalance = $scope.curramount;
            //        }
            //    }
            //}
            //if (allchkdata == true) {
            //    for (var index = 0; index < $scope.totalgrid.length; index++) {
            //        if ($scope.totalgrid[index].fmH_FeeName == "Fine" || $scope.totalgrid[index].fmH_Flag == "F") {
            //            $scope.curramount = $scope.curramount + Number($scope.totalgrid[index].fsS_ToBePaid)
            //        }
            //    }
            //}

            //MULTIMODE

            //MULTIMODE

            //dont uncheck
            //$scope.currbalance =  $scope.curramount;
            //MB

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
            var data = {
                "auto_receipt_flag": autoreceipt,
                temp_head_list: $scope.heads1,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeStudentTransaction/get_grp_reptno", data).
                then(function (promise) {
                    $scope.grpcount = promise.grp_count;
                    $scope.FYP_Receipt_No = promise.auto_FYP_Receipt_No;



                    if (promise.grp_count != 0 && (promise.grp_count != 1 || promise.grp_count > 1)) {
                        swal("Can't Do Transaction For Two Fee Groups At A Time !!!!!!");

                        //if (temp_123 != undefined)
                        //{
                        //    angular.forEach($scope.totalgrid, function (itm) {
                        //        if (temp_123[0].fmH_Id == itm.fmH_Id)
                        //            itm.isSelected = false;
                        //    });
                        //}
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

                                $scope.totalconcession = Number($scope.totalconcession) + Number(totalgrid[index].fsS_ConcessionAmount);
                                $scope.totalfine = Number($scope.totalfine) + Number(totalgrid[index].fsS_FineAmount);
                                //$scope.totalfine =  Number(totalgrid[index].fsS_FineAmount);
                                $scope.curramount = Number($scope.curramount) + Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_FineAmount);
                                $scope.totalwaived = Number($scope.totalwaived) + Number(totalgrid[index].fsS_WaivedAmount);
                            }
                            else if (totalgrid[index].isSelected == false) {

                                $scope.totalconcession = Number($scope.totalconcession) - Number(totalgrid[index].fsS_ConcessionAmount);
                                ////$scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                                $scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                                // $scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                                $scope.curramount = Number($scope.curramount) - Number(totalgrid[index].fsS_ToBePaid) - Number(totalgrid[index].fsS_FineAmount);
                                $scope.totalwaived = Number($scope.totalwaived) - Number(totalgrid[index].fsS_WaivedAmount);
                            }

                            //
                            //if ($scope.all == false) {
                            //    $scope.amtdetails(trp[0].user_data, trp[0].total_grid, trp[0].in_dex);
                            //}
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

                    if ($scope.FYP_PayModeType === 'Single') {
                        angular.forEach($scope.paymodeee, function (ed) {
                            if (ed.rdo_Bank_Multiple === 'singlee') {
                                ed.Bank_Amount = $scope.curramount;
                            }
                        })
                    }
                    if ($scope.FYP_PayModeType === 'Multiple') {
                        angular.forEach($scope.paymodeee, function (ed) {
                            if (ed.chk_Bank_Multiple === 'multii') {
                                ed.Bank_Amount = $scope.curramount;
                            }
                        })
                    }
                    //MB for Fine
                    //if (fineapplicable == true) {
                    //    $scope.calculate_fine();

                    //    if ($scope.FYP_PayModeType === 'Single') {
                    //        angular.forEach($scope.paymodeee, function (ed) {
                    //            if (ed.rdo_Bank_Multiple === 'singlee') {
                    //                ed.Bank_Amount = $scope.curramount;
                    //            }
                    //        })
                    //    }
                    //    if ($scope.FYP_PayModeType === 'Multiple') {
                    //        angular.forEach($scope.paymodeee, function (ed) {
                    //            if (ed.chk_Bank_Multiple === 'multii') {
                    //                ed.Bank_Amount = $scope.curramount;
                    //            }
                    //        })
                    //    }
                    //}

                    //MB for Fine

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

            //trp.push(userdata);

            $scope.disablefine = true;
            $scope.disablenetamount = true;

            $scope.all = $scope.totalgrid.every(function (itm) {
                return itm.isSelected;
            });

            if (totalgrid[index].isSelected == true) {
                $scope.totalconcession = Number($scope.totalconcession) + Number(totalgrid[index].fsS_ConcessionAmount);
                $scope.totalfine = Number($scope.totalfine) + Number(totalgrid[index].fsS_FineAmount);
              //  $scope.curramount = Number($scope.curramount) + Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_FineAmount);
                $scope.curramount = Number($scope.curramount) + Number(totalgrid[index].fsS_ToBePaid) + Number(totalgrid[index].fsS_FineAmount) - Number($scope.rebateamount);

                $scope.totalwaived = Number($scope.totalwaived) + Number(totalgrid[index].fsS_WaivedAmount);

                if (totalgrid[index].fmH_FeeName != "Fine") {
                    //$scope.currbalance =  $scope.curramount;
                }

            }
            else if (totalgrid[index].isSelected == false) {
                $scope.totalconcession = Number($scope.totalconcession) - Number(totalgrid[index].fsS_ConcessionAmount);
                //$scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
                $scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
               // $scope.curramount = Number($scope.curramount) - Number(totalgrid[index].fsS_ToBePaid) - Number(totalgrid[index].fsS_FineAmount);
                $scope.curramount = Number($scope.curramount) - Number(totalgrid[index].fsS_ToBePaid) - Number(totalgrid[index].fsS_FineAmount) - Number($scope.rebateamount);

                $scope.totalwaived = Number($scope.totalwaived) - Number(totalgrid[index].fsS_WaivedAmount);

                if (totalgrid[index].fmH_FeeName != "Fine") {
                    // $scope.currbalance = $scope.curramount;
                }

            }



            if (autoreceipt == 1) {
                $scope.get_grp_reptno(index);
            }
            //else {
            if (fineapplicable == true) {
                $scope.calculate_fine();
                if ($scope.FYP_PayModeType === 'Single') {
                    angular.forEach($scope.paymodeee, function (ed) {
                        if (ed.rdo_Bank_Multiple === 'singlee') {
                            ed.Bank_Amount = $scope.curramount;
                        }
                    })
                }
                if ($scope.FYP_PayModeType === 'Multiple') {
                    angular.forEach($scope.paymodeee, function (ed) {
                        if (ed.chk_Bank_Multiple === 'multii') {
                            ed.Bank_Amount = $scope.curramount;
                        }
                    })
                }

            }
            //}




            //MULTIMODE


        };



        $scope.onselectmodeofpayment = function () {

            var data = {
                "modeofpayment": $scope.FYP_Bank_Or_Cash,
                "filterinitialdata": $scope.filterdata,
                "ASMAY_ID": $scope.cfg.ASMAY_Id,
                "Amst_Id": $scope.Amst_Id,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            if ($scope.FYP_Bank_Or_Cash == 'C') {
                //getElementByID("chequedddate").disabled;
                //getElementByID("chequeno").disabled;

            }
            else if ($scope.FYP_Bank_Or_Cash == 'B' || $scope.FYP_Bank_Or_Cash == 'R' || $scope.FYP_Bank_Or_Cash == 'S') {
                //getElementByID("chequedddate").enabled;
                //getElementByID("chequeno").enabled;
                $scope.bankdetails = true;
            }
            else {

                $scope.groupcount = [];
                $scope.cfg.ASMAY_Id = "";
                $scope.amst_Id = "";
                $scope.grigview1 = false;
                //getElementByID("chequedddate").enabled;
                //getElementByID("chequeno").enabled;
            }

            if ($scope.FYP_Bank_Or_Cash == 'C') {
                $scope.bankdetails = false;
            }
            else if ($scope.FYP_Bank_Or_Cash == 'B') {
                $scope.bankdetails = true;
            }

            //     apiService.create("FeeStudentTransaction/getgroupmappedheads", data).
            //then(function (promise) {

            //    if ($scope.FYP_Bank_Or_Cash == 'C') {
            //        $scope.bankdetails = false;
            //    }
            //    else if ($scope.FYP_Bank_Or_Cash == 'B') {
            //        $scope.bankdetails = true;
            //    }

            //    if (promise.filterinitialdata == 'AdmNo' || promise.filterinitialdata == 'Preadmission' || promise.filterinitialdata == 'Admnoname' || promise.filterinitialdata == 'Regular' || promise.filterinitialdata == 'YearLoss' || promise.filterinitialdata == 'InActive' || promise.filterinitialdata == 'regno' || promise.filterinitialdata == 'NameAdmno' || promise.filterinitialdata == 'NameRegNo' || promise.filterinitialdata == 'RegNoName') {
            //        $scope.studentlst = promise.fillstudent;
            //    }
            //})

        };

        // $scope.disablepaisterms = false;


        $scope.route_name = "";

        $scope.onselectstudent = function (studentlst) {

            $scope.routedetails = [];
            $scope.pickuprt = '';
            $scope.droptr = '';

            //var studid = $scope.Amst_Id.amst_Id;

            var studid = studentlst.amst_Id;

            $scope.grigview1 = false;

            $scope.obj.allgrouporterm = false;

            $scope.temptermarray = [];

            var data = {
                "Amst_Id": studid,
                "filterinitialdata": $scope.filterdata,
                "configset": grouporterm,
                "autoreceiptflag": autoreceipt,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeStudentTransaction/getstudlistgroup", data).
                then(function (promise) {

                    if (promise.fillmastergroup.length > 0) {

                        if (promise.showstaticticsdetails.length > 0) {
                            $scope.studentviewdetails = promise.showstaticticsdetails;
                        }

                        //MULTIMODE
                        $scope.grpcountamt = promise.fillmastergroupforamount;
                        //MULTIMODE

                        $scope.groupcount = promise.fillmastergroup;
                        $scope.showdetails = promise.fillstudentviewdetails;

                        $scope.showdetailsreceipt = promise.fillstudentviewdetails;

                        if (grouporterm == 'T') {
                            var termsdisable = promise.disableterms;

                            if ($scope.groupcount.length == promise.disableterms.length) {
                                for (var r = 0; r < $scope.groupcount.length; r++) {
                                    if (promise.disableterms[r].netamount <= promise.disableterms[r].paid && $scope.groupcount[r].fmT_Id == promise.disableterms[r].FMT_Id) {
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

                        $scope.routedetails = promise.routedetails;
                        if (promise.routedetails != null) {
                            $scope.pickuprt = $scope.routedetails[0].trmR_PickRouteName;
                            $scope.droptr = $scope.routedetails[0].trmR_DropRouteName;
                        }

                        $scope.alltermchk = $scope.groupcount.every(function (options) {
                            // return options.disablepaisterms;
                        });

                        //if (grouporterm == 'T')
                        //{
                        //    var termsdisable = promise.disableterms;

                        //    for (var r = 0; r < $scope.groupcount.length; r++) {
                        //        if (promise.disableterms[r].netamount == promise.disableterms[r].paid && $scope.groupcount[r].FMT_Id == promise.disableterms[r].FMT_Id) {
                        //            $scope.groupcount[r].disablepaisterms = true;

                        //        }
                        //    }
                        //}

                        //$scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + " " + promise.fillstudentviewdetails[0].amsT_MiddleName + " " + promise.fillstudentviewdetails[0].amsT_LastName;

                        //$scope.amsT_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName
                        //$scope.amsT_MiddleName = promise.fillstudentviewdetails[0].amsT_MiddleName;
                        //$scope.amsT_LastName = promise.fillstudentviewdetails[0].amsT_LastName

                        //$scope.amsT_fullanme = $scope.AMST_FirstName
                        //$scope.amsT_AdmNo = promise.fillstudentviewdetails[0].admno
                        //$scope.amsT_RegistrationNo = promise.fillstudentviewdetails[0].amsT_RegistrationNo
                        //$scope.amaY_RollNo = promise.fillstudentviewdetails[0].rollno;


                        //if (promise.trmR_RouteName != null) {
                        //    $scope.route_name = promise.trmR_RouteName;
                        //}
                        //else {
                        //    $scope.route_name = "";
                        //}

                        $scope.amsT_FirstName = promise.fillstudent[0].amsT_FirstName
                        $scope.amsT_MiddleName = promise.fillstudent[0].amsT_MiddleName;
                        $scope.amsT_LastName = promise.fillstudent[0].amsT_LastName

                        $scope.amsT_fullanme = $scope.AMST_FirstName
                        $scope.amsT_AdmNo = promise.fillstudent[0].amsT_AdmNo
                        $scope.amsT_RegistrationNo = promise.fillstudent[0].amsT_RegistrationNo
                        $scope.amaY_RollNo = promise.fillstudent[0].amaY_RollNo;

                        $scope.classname = promise.fillstudent[0].classname
                        $scope.sectionname = promise.fillstudent[0].sectionname;

                        $scope.amsT_mobile = promise.fillstudent[0].amst_mobile;

                        $scope.fathername = promise.fillstudent[0].fathername;
                        $scope.studentdob = promise.fillstudent[0].studentdob;

                        //$('#blah').attr('src', promise.fillstudent[0].amsT_Photoname);

                        if (promise.fillstudent[0].amsT_Photoname != null) {
                            studentphtoto = promise.fillstudent[0].amsT_Photoname;
                        }
                        else {
                            studentphtoto = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";
                        }

                        $('#blah').attr('src', studentphtoto);

                        if (promise.filusername.length > 0) {
                            $scope.portalusername = promise.filusername[0].portalusername;
                        }


                        $scope.grigview1 = false;
                        $scope.totalgrid = "";

                        if (autoreceipt == "1") {
                            $scope.recchkbox = true;
                            $scope.FYP_Receipt_No = promise.fyP_Receipt_No
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
                    }
                    else {
                        swal("Kindly map student with group in student fee group Mapping form!!!")
                        $scope.studentviewdetails = {};
                    }


                })
        };

        $scope.nextduedate = true;

        $scope.showpaiddetails = function () {
            // alert('skdjfh,kd');
            var data = {
                "AMST_ID": $scope.Amst_Id.amst_Id,
                "filterinitialdata": $scope.filterdata,
                "configset": grouporterm,
                "autoreceiptflag": autoreceipt,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeStudentTransaction/getstudlistgroup", data).

                then(function (promise) {
                    $scope.showdetails = promise.showstudetails;
                })
        }

        $scope.bankdet = false;
        $scope.showmodaldetails = function (fypid, studid) {

            $scope.ASMCL_ClassName = "";
            $scope.ASMC_SectionName = "";
            $scope.AMAY_RollNo = "";
            $scope.AMST_AdmNo = "";
            $scope.AMST_FirstName = "";
            //$scope.curdate = new Date().getDate() + '/' + new Date().getMonth() + '/' + new Date().getFullYear();
            $scope.curdate = "";
            $scope.asmaY_Year = "";
            $scope.AMST_FatherName = "";
            $scope.AMST_MotherName = "";
            $scope.receiptno = "";
            $scope.FMCC_ConcessionName = "";

            var totnet = 0, totalbalance = 0, totpaid = 0, totcon = 0;
            $scope.studid = studid;

            var data = {
                "AMST_ID": studid,
                "filterinitialdata": $scope.filterdata,
                "configset": grouporterm,
                "FYP_Id": fypid,
                "minstall": mergeinstallment,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeStudentTransaction/printreceipt", data).
                then(function (promise) {
                    var totalamt = 0;
                    var totalrebateamt = 0;

                    $scope.htmldata = promise.htmldata;

                    if (promise.fillstudentviewdetails != null) {
                        if (promise.htmldata != "") {

                            $scope.groupwisebal = 0;
                            if (promise.groupwiseBalance != null) {
                                if (promise.groupwiseBalance.length > 0) {
                                    $scope.groupwisebal = promise.groupwiseBalance[0].FSS_ToBePaid;
                                }
                            }
                            //stthomas
                            if (promise.readmissionfeeschecking != null) {
                                $scope.stthomasreceipttemplate = promise.readmissionfeeschecking;
                                if (promise.fillstudenttype != null) {
                                    $scope.stthomastermname = promise.fillstudenttype;
                                }
                            }
                            //stthomas

                            if (promise.srkvsdetails != null) {
                                if (promise.srkvsdetails.length > 0) {
                                    for (var i = 0; i < promise.srkvsdetails.length; i++) {
                                        totnet = Number(totnet) + Number(promise.srkvsdetails[i].fsS_TotalToBePaid)
                                        if (Number(promise.srkvsdetails[i].fsS_TotalToBePaid) > 0) {
                                            totalbalance = Number(totalbalance) + Number(promise.srkvsdetails[i].fsS_ToBePaid)
                                        }
                                        else {
                                            totalbalance = 0;
                                        }

                                        totpaid = Number(totpaid) + Number(promise.srkvsdetails[i].ftP_Paid_Amt)
                                        //totcon = Number(totcon) + Number(promise.srkvsdetails[i].fsS_ConcessionAmount)
                                    }

                                    $scope.Tnet = totnet;
                                    if (Number(totnet) > Number(totpaid)) {
                                        //$scope.Tbal = Number(totnet) - Number(totpaid);
                                        $scope.Tbal = totalbalance;
                                    }
                                    else {
                                        $scope.Tbal = 0;
                                    }
                                    $scope.Tpai = totpaid;
                                    // $scope.Tcon = totcon;
                                }
                            }

                            if (promise.getfeeheaddetails != null) {
                                $scope.getfeeheaddetails = promise.getfeeheaddetails


                            }

                            if (promise.streamdetails != null) {
                                if (promise.streamdetails.length != null && promise.streamdetails.length > 0) {
                                    $scope.ASMST_StreamName = promise.streamdetails[0].asmsT_StreamName;
                                }
                            }

                            $scope.period = promise.duration;

                            // $scope.period = $scope.period.split("/")[0];

                            $scope.paymenrgrid = promise.currpaymentdetails;

                            $scope.fyppM_TransactionId = promise.currpaymentdetails[0].fyppM_TransactionId;
                            $scope.fyppM_PaymentReferenceId = promise.currpaymentdetails[0].fyppM_PaymentReferenceId;

                            if ($scope.paymenrgrid.length > 0) {
                                $scope.paymentmode = $scope.paymenrgrid[0].fyP_Bank_Or_Cash;
                            }

                            // $scope.atotA = $scope.atotalA(promise.currpaymentdetails);
                            // $scope.ctotA = $scope.ctotalA(promise.currpaymentdetails);

                            var totconpaid = 0, totchargesss = 0, totconcessionss = 0, totbalance = 0, Opening_Balance_amount = 0;
                            if (promise.fillstudentviewdetails != null) {
                                for (var r = 0; r < promise.fillstudentviewdetails.length; r++) {
                                    totconpaid = promise.fillstudentviewdetails[r].ftP_Concession_Amt;
                                    totchargesss = Number(totchargesss) + Number(promise.fillstudentviewdetails[r].fmA_Amount);
                                    totconcessionss = Number(totconcessionss) + Number(promise.fillstudentviewdetails[r].ftP_Concession_Amt);
                                    if (promise.fillstudentviewdetails[r].fsS_OBArrearAmount > 0) {
                                        totbalance = Number(totbalance) + Number(promise.fillstudentviewdetails[r].fsS_OBArrearAmount) - Number(promise.fillstudentviewdetails[r].fsS_ToBePaid);
                                    } else {
                                        totbalance = Number(totbalance) + Number(promise.fillstudentviewdetails[r].fsS_ToBePaid);
                                    }
                                    Opening_Balance_amount = Number(Opening_Balance_amount) + Number(promise.fillstudentviewdetails[r].fsS_OBArrearAmount);
                                    totalrebateamt += Number(promise.fillstudentviewdetails[r].Rebate_Amount);

                                }
                            }

                            $scope.totalchargessss = Number(totchargesss);

                            $scope.Opening_Balance_amount = Number(Opening_Balance_amount);


                            if (promise.currpaymentdetails.length > 0) {
                                for (var i = 0; i < promise.currpaymentdetails.length; i++) {
                                    totalamt += promise.currpaymentdetails[i].ftP_Paid_Amt;
                                }
                            }
                            $scope.atotA = totalamt;
                            $scope.rtotA = totalrebateamt;
                            //$scope.atotA = promise.currpaymentdetails[0].ftP_Paid_Amt;
                            //$scope.ctotA = promise.currpaymentdetails[0].ftP_Concession_Amt;
                            $scope.ctotA = Number(totconcessionss);
                            $scope.totchar = Number(totchargesss);
                            $scope.totbal = Number(totbalance);
                            //$scope.totchar = $scope.atotA + $scope.ctotA;

                            $scope.words = $scope.amountinwords($scope.atotA);
                            $scope.narration = promise.currpaymentdetails[0].fyP_Remarks;
                            $scope.year = promise.year;

                            angular.forEach($scope.yearlst, function (op_m) {
                                if (op_m.asmaY_Id == $scope.cfg.ASMAY_Id) {
                                    $scope.asmaY_Year = op_m.asmaY_Year
                                }
                            })

                            $scope.asmaY_Year = $scope.asmaY_Year;

                            $scope.due_amount = promise.dueamount;
                            if ($scope.due_amount == 0) {
                                $scope.date = "";
                                $scope.nextduedate = false;
                            }
                            else {
                                $scope.date = promise.date;
                                //$scope.date = promise.duedetails[0].date;
                                $scope.nextduedate = true;
                            }

                            if ($scope.due_amount == 0) {
                                $scope.months = "";
                            }
                            else {
                                //$scope.months = promise.duedetails[0].month;
                                $scope.months = promise.month;

                                $scope.nextduedate = true;
                            }

                            var termname = " ";
                            if (promise.termremarks != null) {
                                if (promise.termremarks.length > 0) {
                                    for (var i = 0; i < promise.termremarks.length; i++) {
                                        if (termname == " ") {
                                            termname = promise.termremarks[i].termname
                                        }
                                        else {
                                            termname = termname + ',' + promise.termremarks[i].termname
                                        }
                                    }
                                }
                            }

                            $scope.Refund_amount = 0;
                            if (promise.fillstudentviewdetails != null) {
                                if (promise.fillstudentviewdetails.length > 0) {
                                    for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {
                                        $scope.Refund_amount = Number($scope.Refund_amount) + Number(promise.fillstudentviewdetails[i].fsS_RefundAmount);
                                    }
                                }
                                else {
                                    $scope.Refund_amount = 0;
                                }
                            }

                            var feeheadname = "";
                            var validation;
                            $scope.tempreceiptarraytermexfinal = [];
                            $scope.receiptformattem = receiptformat;
                            if (receiptformat == 1 || receiptformat == 0) {
                                $scope.tempreceiptarray = [];
                                $scope.tempreceiptarrayterm = {
                                };
                                $scope.tempreceiptarraytermex = {
                                };
                                var totalamount = 0, concessionamt = 0, fineamt = 0, feecount = 0, fmH_FeeName, feetotcharges = 0;
                                var totalamountex = 0, concessionamtex = 0, fineamtex = 0, fmH_FeeNameex, feetotchargesex = 0;

                                //praveen added
                                var adjustedamt = 0; var adjustedamtex = 0;
                                //end
                                if (promise.fillstudentviewdetails != null) {
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

                                                        feetotcharges = Number(feetotcharges) + Number(promise.receiptformathead[j].totalcharges);

                                                        fmH_FeeName = promise.receiptformathead[j].fmH_FeeName
                                                        totalamount = Number(totalamount) + Number(promise.receiptformathead[j].ftP_Paid_Amt);
                                                        concessionamt = Number(concessionamt) + Number(promise.receiptformathead[j].ftP_Concession_Amt);
                                                        fineamt = Number(fineamt) + Number(promise.receiptformathead[j].ftP_Fine_Amt);
                                                    }
                                                }
                                                if (feecount < 1) {

                                                    fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName
                                                    if (feeheadname == "") {
                                                        //fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName

                                                        feetotchargesex = Number(promise.fillstudentviewdetails[i].totalcharges);

                                                        totalamountex = Number(promise.fillstudentviewdetails[i].ftP_Paid_Amt);
                                                        concessionamtex = Number(promise.fillstudentviewdetails[i].ftP_Concession_Amt);
                                                        fineamtex = Number(promise.fillstudentviewdetails[i].ftP_Fine_Amt);
                                                    }

                                                    else if (fmH_FeeNameex === feeheadname) {
                                                        //fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName
                                                        feetotchargesex = Number(feetotchargesex) + Number(promise.fillstudentviewdetails[i].totalcharges);

                                                        totalamountex = Number(totalamountex) + Number(promise.fillstudentviewdetails[i].ftP_Paid_Amt);
                                                        concessionamtex = Number(concessionamtex) + Number(promise.fillstudentviewdetails[i].ftP_Concession_Amt);
                                                        fineamtex = Number(fineamtex) + Number(promise.fillstudentviewdetails[i].ftP_Fine_Amt);
                                                    }
                                                    else {

                                                        validation = "add";

                                                        $scope.tempreceiptarraytermex = {
                                                            fmH_FeeName: feeheadname,
                                                            ftP_Paid_Amt: totalamountex,
                                                            ftP_Concession_Amt: concessionamtex,
                                                            ftp_fine_amt: fineamtex,
                                                            totalcharges: feetotchargesex,
                                                        };

                                                        // $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);

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

                                                            //MB
                                                            else if (aldy_cnt == 1) {
                                                                for (var q = 0; q < $scope.tempreceiptarraytermexfinal.length; q++) {
                                                                    if (feeheadname == $scope.tempreceiptarraytermexfinal[q].fmH_FeeName) {
                                                                        $scope.tempreceiptarraytermexfinal.splice(q, 1);
                                                                    }
                                                                }
                                                                $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                            }
                                                            //MB


                                                        }
                                                        else if ($scope.tempreceiptarraytermexfinal.length == 0) {
                                                            $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                        }



                                                        fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName
                                                        totalamountex = Number(promise.fillstudentviewdetails[i].ftP_Paid_Amt);
                                                        concessionamtex = Number(promise.fillstudentviewdetails[i].ftP_Concession_Amt);
                                                        fineamtex = Number(promise.fillstudentviewdetails[i].ftP_Fine_Amt);

                                                        feetotchargesex = Number(promise.fillstudentviewdetails[i].totalcharges);

                                                        //Praveen
                                                        adjustedamtex = Number(promise.fillstudentviewdetails[i].fsS_AdjustedAmount);
                                                        //end

                                                        $scope.tempreceiptarraytermex = {
                                                            fmH_FeeName: fmH_FeeNameex,
                                                            ftP_Paid_Amt: totalamountex,
                                                            ftP_Concession_Amt: concessionamtex,
                                                            ftp_fine_amt: fineamtex,
                                                            totalcharges: feetotchargesex,
                                                            //praveen
                                                            fsS_AdjustedAmount: adjustedamtex,
                                                            //end
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

                                                            //MB
                                                            else if (aldy_cnt == 1) {
                                                                for (var q = 0; q < $scope.tempreceiptarraytermexfinal.length; q++) {
                                                                    if (feeheadname == $scope.tempreceiptarraytermexfinal[q].fmH_FeeName) {
                                                                        $scope.tempreceiptarraytermexfinal.splice(q, 1);
                                                                    }
                                                                }
                                                                $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                            }
                                                            //MB

                                                        }
                                                        else if ($scope.tempreceiptarraytermexfinal.length == 0) {
                                                            $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                        }
                                                    }

                                                    feeheadname = fmH_FeeNameex

                                                    //$scope.tempreceiptarraytermex = {
                                                    //    fmH_FeeName: fmH_FeeNameex,
                                                    //    ftP_Paid_Amt: totalamountex,
                                                    //    ftP_Concession_Amt: concessionamtex,
                                                    //    ftp_fine_amt: fineamtex,
                                                    //};

                                                    //$scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);

                                                    // $scope.tempreceiptarraytermex.push(promise.fillstudentviewdetails[i]);
                                                }
                                                feecount = 0;
                                            }

                                            else if ($scope.FMC_RInstallmentsMergeFlag === 0) {

                                                $scope.tempreceiptarray = promise.showstudetails;
                                            }
                                            else {
                                                $scope.tempreceiptarray.push(promise.fillstudentviewdetails[i]);
                                            }

                                            //else {
                                            //    $scope.tempreceiptarray.push(promise.fillstudentviewdetails[i]);
                                            //}
                                        }

                                    }
                                }
                                if (promise.receiptformathead.length > 0) {
                                    var temlength = $scope.tempreceiptarray.length;

                                    //MB For Special
                                    //$scope.tempreceiptarrayterm = {
                                    //    fmH_FeeName: fmH_FeeName,
                                    //    ftP_Paid_Amt: totalamount,
                                    //    ftP_Concession_Amt: concessionamt,
                                    //    ftp_fine_amt: fineamt,
                                    //    totalcharges: feetotcharges,
                                    //};

                                    angular.forEach($scope.special_head_list, function (sp_hd) {
                                        var count = 0;
                                        var feetotcharges1 = 0, totalamount1 = 0, concessionamt1 = 0, fineamt1 = 0, fmH_FeeName1 = "", adjest1 = 0;
                                        angular.forEach(promise.receiptformathead, function (sh_hd) {
                                            if (sp_hd.fmsfH_Name == sh_hd.fmH_FeeName) {
                                                count += 1;
                                                feetotcharges1 = Number(feetotcharges1) + Number(sh_hd.totalcharges);
                                                fmH_FeeName1 = sh_hd.fmH_FeeName;
                                                totalamount1 = Number(totalamount1) + Number(sh_hd.ftP_Paid_Amt);
                                                concessionamt1 = Number(concessionamt1) + Number(sh_hd.ftP_Concession_Amt);
                                                fineamt1 = Number(fineamt1) + Number(sh_hd.ftP_Fine_Amt);
                                                adjest1 = Number(adjest1) + Number(sh_hd.fsS_AdjustedAmount);
                                            }
                                        })
                                        if (count > 0) {
                                            $scope.tempreceiptarrayterm = {
                                                fmH_FeeName: fmH_FeeName1,
                                                ftP_Paid_Amt: totalamount1,
                                                ftP_Concession_Amt: concessionamt1,
                                                ftp_fine_amt: fineamt1,
                                                totalcharges: feetotcharges1,
                                                fsS_AdjustedAmount: adjest1,
                                            };
                                            console.log($scope.tempreceiptarrayterm);
                                            $scope.tempreceiptarray.push($scope.tempreceiptarrayterm);
                                        }
                                    })

                                    //MB For Special

                                    if (validation != "add") {
                                        $scope.tempreceiptarraytermex = {
                                            fmH_FeeName: fmH_FeeNameex,
                                            ftP_Paid_Amt: totalamountex,
                                            ftP_Concession_Amt: concessionamtex,
                                            ftp_fine_amt: fineamtex,
                                            totalcharges: feetotchargesex,
                                            fsS_AdjustedAmount: adjustedamtex,
                                        };

                                        $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                    }
                                    //$scope.tempreceiptarraytermex = {
                                    //    fmH_FeeName: fmH_FeeNameex,
                                    //    ftP_Paid_Amt: totalamountex,
                                    //    ftP_Concession_Amt: concessionamtex,
                                    //    ftp_fine_amt: fineamtex,
                                    //};

                                    //MB For Special
                                    // $scope.tempreceiptarray.push($scope.tempreceiptarrayterm);
                                    //MB For Special
                                    //$scope.tempreceiptarray[temlength].fmH_FeeName = fmH_FeeName;

                                    //$scope.tempreceiptarray.push($scope.tempreceiptarraytermex);
                                    //$scope.tempreceiptarray[temlength].fmH_FeeName = fmH_FeeName;

                                    for (var r = 0; r < $scope.tempreceiptarraytermexfinal.length; r++) {
                                        if ($scope.tempreceiptarraytermexfinal[r].fmH_FeeName != undefined) {
                                            $scope.tempreceiptarray.push($scope.tempreceiptarraytermexfinal[r]);
                                        }
                                    }
                                }

                                $scope.showdetailsreceipt = $scope.tempreceiptarray;
                                $scope.showtotaldetails = promise.filltotaldetails;
                                $scope.showdetailsreceipt = promise.fillstudentviewdetails;

                                //added for total
                                angular.forEach($scope.showdetailsreceipt, function (ll) {
                                    if (ll.fsS_AdjustedAmount != undefined && ll.fsS_AdjustedAmount != undefined) {
                                        $scope.AdjustedAmounttotal += ll.fsS_AdjustedAmount;
                                    }

                                })

                            }
                            else {
                                $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                                $scope.showtotaldetails = promise.filltotaldetails;


                                //added for total
                                angular.forEach($scope.showdetailsreceipt, function (vv) {
                                    if (vv.fsS_AdjustedAmount != undefined && vv.fsS_AdjustedAmount != undefined) {
                                        $scope.AdjustedAmounttotal += vv.fsS_AdjustedAmount;
                                    }

                                })

                                //$scope.atotA = $scope.atotalA(promise.showdetailsreceipt);
                                //$scope.ctotA = $scope.ctotalA(promise.showdetailsreceipt);

                                //$scope.words = $scope.amountinwords($scope.atotA);
                                //$scope.due_amount = promise.duedetails[0].dueamount;
                                //$scope.months = promise.duedetails[0].month;
                                //// $scope.monthes = $scope.getmontnames($scope.months)
                                //$scope.date = promise.duedetails[0].date;

                            }

                            //$scope.showdetails = promise.fillstudentviewdetails;

                            //if (promise.fillstudentviewdetails[0].amsT_FirstName != null)
                            //{
                            //    $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName;
                            //}
                            //else
                            //{
                            //    $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName;
                            //}


                            $scope.multiplereceiptarr = [];


                            if (promise.fillstudentviewdetails.length > 0 && promise.fillstudentviewdetails != null) {

                                $scope.fillstudentviewdetailsarr = promise.fillstudentviewdetails;

                                $scope.getfeegroupdetails = promise.getfeeheaddetails;


                                angular.forEach($scope.getfeegroupdetails, function (grp) {

                                    var totalamt = 0;

                                    $scope.multiplereceiptarr = [];

                                    angular.forEach($scope.fillstudentviewdetailsarr, function (grphead) {

                                        if (grp.fmG_Id == grphead.fmG_Id) {

                                            $scope.multiplereceiptarr.push(grphead)

                                            totalamt = totalamt + grphead.ftP_Paid_Amt;
                                        }

                                        grp.fillstudentviewdetailsa = $scope.multiplereceiptarr;

                                        grp.amountwords = $scope.amountinwords(totalamt);
                                        grp.totamount = totalamt;
                                    })

                                })


                            }



                            $scope.endusername = promise.portalusername;

                            if (promise.fillacclst.length > 0 && promise.fillacclst != null) {
                                $scope.housename = promise.fillacclst[0].SPCCMH_HouseName;
                                $scope.routeno = promise.fillacclst[0].TRMR_RouteNo;
                            }


                            if (promise.fillstudentviewdetails[0].amsT_FirstName != null && promise.fillstudentviewdetails[0].amsT_MiddleName != null && promise.fillstudentviewdetails[0].amsT_LastName != null) {
                                $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                            }
                            else {
                                if (promise.fillstudentviewdetails[0].amsT_FirstName == null) {
                                    promise.fillstudentviewdetails[0].amsT_FirstName = ' ';
                                }
                                if (promise.fillstudentviewdetails[0].amsT_MiddleName == null) {
                                    promise.fillstudentviewdetails[0].amsT_MiddleName = ' ';
                                }
                                if (promise.fillstudentviewdetails[0].amsT_LastName == null) {
                                    promise.fillstudentviewdetails[0].amsT_LastName = ' ';
                                }
                                $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                            }


                            $scope.ASMCL_ClassName = promise.fillstudentviewdetails[0].classname;
                            $scope.ASMC_SectionName = promise.fillstudentviewdetails[0].sectionname;
                            $scope.AMAY_RollNo = promise.fillstudentviewdetails[0].rollno;
                            $scope.AMST_AdmNo = promise.fillstudentviewdetails[0].admno;
                            //$scope.curdate = new Date().getDate() + '/' + new Date().getMonth() + '/' + new Date().getFullYear();
                            $scope.curdate = $filter('date')(new Date(), "dd-MM-yyyy");
                            // $scope.asmaY_Year = promise.asmaY_Year;
                            $scope.AMST_FatherName = promise.fillstudentviewdetails[0].fathername;
                            $scope.AMST_MotherName = promise.fillstudentviewdetails[0].mothername;
                            $scope.receiptno = promise.fillstudentviewdetails[0].fyP_Receipt_No;
                            $scope.pendingamount = promise.pendingamount;
                            $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;
                            $scope.fyP_Remarks = promise.fillstudentviewdetails[0].fyP_Remarks;

                            $scope.amst_mobile = promise.fillstudentviewdetails[0].amst_mobile;
                            $scope.FYP_ChallanNo = promise.fillstudentviewdetails[0].fyP_ChallanNo;

                            $scope.FYP_PaymentReference_Id = promise.fillstudentviewdetails[0].fyP_PaymentReference_Id;
                            $scope.fyp_transaction_Id = promise.fillstudentviewdetails[0].fyp_transaction_Id;

                            $scope.Paid_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");


                            $scope.totalpayableamt = 0;

                            angular.forEach(promise.fillstudentviewdetails, function (llll) {
                                $scope.totalpayableamt += Number(llll.totalcharges)
                            })


                            //$scope.FYP_Date = $filter('date') (promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");
                            //$scope.fyp_date = new Date(promise.fillstudentviewdetails[0].fyp_date).getDate() + '/' + new Date(promise.fillstudentviewdetails[0].fyp_date).getMonth() + '/' + new Date(promise.fillstudentviewdetails[0].fyp_date).getFullYear();

                            if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "C") {
                                $scope.bankdet = false;
                                $scope.FYP_Date = "--";
                                $scope.modeofpayment = "Cash";
                                $scope.FYP_DD_Cheque_No = "--";
                                $scope.FYP_DD_Cheque_Date = "--";
                                $scope.FYP_Bank_Name = "--";
                                // $scope.FYP_Bank_Or_Cash = "Cash";
                            }

                            if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "O") {
                                $scope.bankdet = false;
                                $scope.modeofpayment = "Online";
                                $scope.FYP_DD_Cheque_No = "--";
                                $scope.FYP_DD_Cheque_Date = "--";
                                $scope.FYP_Bank_Name = "--";
                                // $scope.FYP_Bank_Or_Cash = "Online Payment";
                            }

                            if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "E") {
                                $scope.bankdet = false;
                                $scope.modeofpayment = "ECS";
                                $scope.FYP_DD_Cheque_No = "--";
                                $scope.FYP_DD_Cheque_Date = "--";
                                $scope.FYP_Bank_Name = "--";
                                // $scope.FYP_Bank_Or_Cash = "Online Payment";
                            }

                            if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "B") {
                                $scope.bankdet = true;
                                $scope.modeofpayment = "BANK";
                                // $scope.FYP_Bank_Or_Cash = "Bank";
                                $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                                $scope.FYP_DD_Cheque_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                                // $scope.FYP_DD_Cheque_Date = new Date(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date).getDate() + '/' + new Date(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date).getMonth() + '/' + new Date(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date).getYear();
                                $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                            }

                            if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "R") {
                                $scope.bankdet = true;
                                $scope.modeofpayment = "RTGS/NEFT";
                                $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                                $scope.FYP_DD_Cheque_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                                $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                            }

                            if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "S") {
                                $scope.bankdet = true;
                                $scope.modeofpayment = "CARD";
                                $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                                $scope.FYP_DD_Cheque_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                                $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                            }

                            if (promise.fillstudentviewdetails[0].fyP_Remarks != null || promise.fillstudentviewdetails[0].fyP_Remarks != "") {
                                //$scope.feeremarks = promise.fillstudentviewdetails[0].fyP_Remarks; 
                                $scope.feeremarks = termname;
                            }
                            else {
                                $scope.feeremarks = "Remarks Not Given";
                            }

                            var e1 = angular.element(document.getElementById("test"));
                            $compile(e1.html(promise.htmldata))(($scope));

                        }

                        else {
                            $scope.MI_Address1 = promise.masterinstitution[0].mI_Address1;
                            $scope.MI_Address2 = promise.masterinstitution[0].mI_Address2;
                            $scope.MI_Address3 = promise.masterinstitution[0].mI_Address3;
                            $scope.MI_Pincode = promise.masterinstitution[0].mI_Pincode;
                            $scope.pendingamount = promise.pendingamount;
                            $scope.receiptno = promise.fillstudentviewdetails[0].fyP_Receipt_No;
                            $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");
                            $scope.AMST_AdmNo = promise.fillstudentviewdetails[0].admno;
                            $scope.asmaY_Year = promise.asmaY_Year;

                            if (promise.fillstudentviewdetails[0].amsT_FirstName != null && promise.fillstudentviewdetails[0].amsT_MiddleName != null && promise.fillstudentviewdetails[0].amsT_LastName != null) {
                                $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                            }
                            else {
                                if (promise.fillstudentviewdetails[0].amsT_FirstName == null) {
                                    promise.fillstudentviewdetails[0].amsT_FirstName = ' ';
                                }
                                if (promise.fillstudentviewdetails[0].amsT_MiddleName == null) {
                                    promise.fillstudentviewdetails[0].amsT_MiddleName = ' ';
                                }
                                if (promise.fillstudentviewdetails[0].amsT_LastName == null) {
                                    promise.fillstudentviewdetails[0].amsT_LastName = ' ';
                                }
                                $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                            }

                            $scope.ASMCL_ClassName = promise.fillstudentviewdetails[0].classname;
                            $scope.ASMC_SectionName = promise.fillstudentviewdetails[0].sectionname;
                            $scope.AMST_FatherName = promise.fillstudentviewdetails[0].fathername;
                            $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;
                            $scope.AMST_MotherName = promise.fillstudentviewdetails[0].mothername;
                            $scope.period = promise.duration;
                            if ($scope.FMC_RInstallmentsMergeFlag == 1) {
                                $scope.FMC_RInstallmentsFlag = 0;

                                $scope.showdetailsreceipt = promise.filltotaldetails;
                            }
                            else {
                                $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                            }

                            //$scope.showtotaldetails = promise.filltotaldetails;

                            if (promise.fillstudentviewdetails.length > 0) {
                                var fmatotal = 0;
                                var totalpaidamount = 0;
                                angular.forEach(promise.fillstudentviewdetails, function (user) {
                                    fmatotal = fmatotal + user.totalcharges;
                                    totalpaidamount = totalpaidamount + user.ftP_Paid_Amt;
                                })
                            }
                            $scope.totalfma = fmatotal;
                            if (fmatotal > totalpaidamount)
                                $scope.btotA = fmatotal - totalpaidamount;
                            else
                                $scope.btotA = 0;

                            $scope.atotA = promise.currpaymentdetails[0].ftP_Paid_Amt;
                            $scope.ctotA = promise.currpaymentdetails[0].ftP_Concession_Amt;
                            $scope.ftotA = promise.currpaymentdetails[0].ftP_Fine_Amt;
                            $scope.wtotA = promise.currpaymentdetails[0].ftP_Waived_Amt;
                            $scope.paymenrgrid = promise.currpaymentdetails;
                            $scope.words = $scope.amountinwords($scope.atotA);
                            $scope.narration = promise.currpaymentdetails[0].fyP_Remarks;
                            $scope.due_amount = promise.dueamount;

                            if ($scope.paymenrgrid.length > 0) {
                                $scope.paymentmode = $scope.paymenrgrid[0].fyP_Bank_Or_Cash;
                            }

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
                                //$scope.FYP_Bank_Or_Cash = "Cash";
                                $scope.FYP_Bank_Name = "--";
                                $scope.FYP_DD_Cheque_No = "--";
                                $scope.FYP_Date = promise.fillstudentviewdetails[0].FYP_Date;

                            }
                            else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "O") {
                                //$scope.FYP_Bank_Or_Cash = "Online Payment";
                                $scope.FYP_Bank_Name = "--";
                                $scope.FYP_DD_Cheque_No = "--";
                                $scope.FYP_Date = promise.fillstudentviewdetails[0].FYP_Date;

                            }
                            else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "B") {
                                //$scope.FYP_Bank_Or_Cash = "Bank";
                                $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                                $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                                //  $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                                $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");
                            }
                            $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");
                            if (promise.fillstudentviewdetails[0].fyP_Remarks != null || promise.fillstudentviewdetails[0].fyP_Remarks != "") {
                                $scope.feeremarks = promise.fillstudentviewdetails[0].fyP_Remarks;
                                //$scope.feeremarks = termname;
                            }
                            else {
                                $scope.feeremarks = "Remarks Not Given";
                            }
                            $scope.AMAY_RollNo = promise.fillstudentviewdetails[0].rollno;
                            $scope.curdate = $filter('date')(new Date(), "dd-MM-yyyy");
                        }
                    }
                    else {
                        swal("Network Failure! kindly Contact Administrator")
                    }


                })

            //$scope.atotalA = function (e) {
            //    var atotalc = 0;
            //    angular.forEach($scope.showdetailsreceipt, function (e) {
            //        atotalc += e.totalcharges;
            //    });
            //    return atotalc;
            //};

            $scope.atotalA = function (e) {
                var atotalc = 0;
                angular.forEach($scope.showdetailsreceipt, function (e) {
                    atotalc += e.ftP_Paid_Amt;
                });
                return atotalc;
            };
            $scope.ctotalA = function (e) {
                var atotalc = 0;
                angular.forEach($scope.showdetailsreceipt, function (e) {
                    atotalc += e.ftP_Concession_Amt;
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

        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;

            swal({
                title: "An input!",
                text: "Write something interesting:",
                type: "input",
                showCancelButton: true,
                closeOnConfirm: false,
                inputPlaceholder: "Write something"
            }, function (inputValue) {
                if (inputValue === false) return false;
                if (inputValue === "") {
                    swal.showInputError("You need to write something!");
                    return false
                }
                swal("Nice!", "You wrote: " + inputValue, "success");
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("FeeStudentTransaction/Deletedetails", orgid).
                            then(function (promise) {

                                $scope.thirdgrid = promise.alldata;


                                if (promise.returnval == true) {

                                    //$scope.masterse = promise.masterSectionData;

                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                }

                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });


            //})
        }

        $scope.cleardata = function () {

            //$scope.FYP_Date = "";
            //$scope.ASMAY_Id = "";
            //$scope.Amst_Id = "";
            //$scope.FYP_Receipt_No = "";
            //$scope.groupcount = "";
            //$scope.amtadjustment = "";
            //$scope.FYP_Remarks = "";
            //$scope.FYP_Bank_Or_Cash = "";
            //$scope.L_Code = "";
            //$scope.FYP_DD_Cheque_Date = "";
            //$scope.FYP_DD_Cheque_No = "";
            //$scope.FYP_Bank_Name = "";
            //$scope.totalfee = "";
            //$scope.totalconcession = "";
            //$scope.totalfine = "";
            //$scope.curramount = "";
            //$scope.currbalance = "";
            //$scope.totalwaived = "";
            //$scope.grigview1 = false;

            $state.reload();

        }

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("FeeStudentTransaction/Editdetails", orgid).
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

        $scope.filterby = function () {
            var entereddata = $scope.search;

            var data = {
                "FMG_GroupName": $scope.searchthird,
                "FMH_FeeName": $scope.typethird,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeStudentTransaction/1", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    swal("searched Successfully");
                })
        }

        //$scope.printData = function (printmodal) {
        //    var innerContents = document.getElementById("printmodal").innerHTML;
        //    var popupWinindow = window.open('');
        //    // var popupWinindow = window.open('','_top');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
        //     '<link href="plugins/bootstrap/css/bootstrap.css" />' +
        //     '<link href="css/style.css" rel="stylesheet" />' +
        //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.onunload(); }, 100);" >' + innerContents + '</html><script> window.onunload = refreshParent; function refreshParent() {window.opener.location.reload();window.close();}          </script>');
        //    popupWinindow.document.close();
        //}

        //$scope.printData = function (printmodal) {
        //    var innerContents = document.getElementById("printmodal").innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/printJanaSevaFeeReceipt.css" />' +
        //     '<link href="plugins/bootstrap/css/bootstrap.css" />' +
        //     '<link href="css/style.css" rel="stylesheet" />' +
        //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close();
        //}

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

        //$scope.BGHSAPP = function () {
        //    var innerContents = document.getElementById("BGHSAPP").innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +

        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSAPPPdf.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

        //        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close();
        //}

        $scope.printDatahhs = function (printmodal) {
            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print"  rel="stylesheet" href="css/print/hutchings/Challan/HHSRECEIPTPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.submitted = false;
        $scope.savedatatrans = [];
        var waivedoff = 0;

        $scope.savedata = function (totalgrid, groupcount, paymodeee) {

            $scope.savedatatrans = [];
            var count = 0;
            //var countflag="deselect";

            //for (var i = 0; i < groupcount.length; i++) {
            //    name = groupcount[i].FMG_Id
            //    if (name == true) {
            //        countflag = "selected";
            //    }
            //}

            if ($scope.myForm.$valid) {
                //MB for Challan
                if ($scope.filterdata != 'Challan_No') {
                    //MB for Challan
                    //angular.forEach($scope.totalgrid, function (user) {
                    //    if (!!user.isSelected) {
                    //        $scope.savedatatrans.push(user);
                    //    }
                    //})

                    //MB For Special
                    if (ins_spe_list.length == 0 && remove_list.length == 0) {
                        angular.forEach($scope.totalgrid, function (opq) {
                            if (opq.isSelected) {
                                count += 1;
                                //  opq.netAmount = Number(opq.netAmount);
                                $scope.savedatatrans.push(opq);
                            }
                        })
                    }
                    else if (ins_spe_list.length > 0 && remove_list.length > 0) {
                        angular.forEach($scope.totalgrid, function (opq) {
                            if (opq.isSelected) {
                                if (opq.Head_Flag == 'H') {
                                    count += 1;
                                    //  opq.netAmount = Number(opq.netAmount);
                                    $scope.savedatatrans.push(opq);
                                }
                                else if (opq.Head_Flag == 'SH') {
                                    angular.forEach(ins_spe_list, function (s) {
                                        if (s.ftI_Id == opq.ftI_Id) {
                                            angular.forEach(s.sp_list, function (s1) {
                                                if (s1.sp_id == opq.fmH_Id) {
                                                    var toBePaid = 0;
                                                    angular.forEach(s1.sp_ind_list, function (s2) {
                                                        toBePaid += Number(s2.fsS_ToBePaid);
                                                    })
                                                    if (toBePaid == Number(opq.fsS_ToBePaid)) {
                                                        angular.forEach(s1.sp_ind_list, function (s2) {
                                                            count += 1;
                                                            $scope.savedatatrans.push(s2);
                                                        })
                                                    }
                                                    else if (toBePaid > Number(opq.fsS_ToBePaid)) {

                                                        var keepGoing = true;
                                                        angular.forEach(s1.sp_ind_list, function (s2) {
                                                            if (keepGoing) {
                                                                if (Number(opq.fsS_ToBePaid) >= Number(s2.fsS_ToBePaid)) {
                                                                    count += 1;
                                                                    $scope.savedatatrans.push(s2);
                                                                    opq.fsS_ToBePaid = (Number(opq.fsS_ToBePaid) - Number(s2.fsS_ToBePaid));
                                                                }
                                                                else if (Number(opq.fsS_ToBePaid) < Number(s2.fsS_ToBePaid)) {
                                                                    s2.fsS_ToBePaid = Number(opq.fsS_ToBePaid);
                                                                    count += 1;
                                                                    $scope.savedatatrans.push(s2);
                                                                    opq.fsS_ToBePaid = (Number(opq.fsS_ToBePaid) - Number(s2.fsS_ToBePaid));
                                                                }
                                                                if (Number(opq.fsS_ToBePaid) == 0) {
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
                        if ($scope.FYP_PayModeType === 'Multiple' || $scope.FYP_PayModeType === 'Single') {
                            angular.forEach(paymodeee, function (ed) {
                                if (ed.chk_Bank_Multiple != 'null' || ed.rdo_Bank_Multiple != 'null') {
                                    if (ed.ivrmmoD_Flag === 'C') {
                                        amount += Number(ed.Bank_Amount);
                                        pay_modes.push({ FYPPM_TransactionTypeFlag: 'C', FYPPM_TotalPaidAmount: Number(ed.Bank_Amount), FYPPM_BankName: '', FYPPM_DDChequeNo: '', FYPPM_DDChequeDate: new Date().toDateString(), IVRMMOD_ModeOfPayment: 'Cash', IVRMMOD_ModeOfPayment_Code: ed.ivrmmoD_ModeOfPayment_Code, FYPPM_Id: ed.fyppM_Id });
                                    }
                                    if (ed.ivrmmoD_Flag === 'B') {
                                        amount += Number(ed.Bank_Amount);
                                        pay_modes.push({ FYPPM_TransactionTypeFlag: 'B', FYPPM_TotalPaidAmount: Number(ed.Bank_Amount), FYPPM_BankName: ed.Bank_Name, FYPPM_DDChequeNo: ed.Bank_No, FYPPM_DDChequeDate: new Date(ed.Bank_Date).toDateString(), IVRMMOD_ModeOfPayment: ed.ivrmmoD_ModeOfPayment, IVRMMOD_ModeOfPayment_Code: ed.ivrmmoD_ModeOfPayment_Code, FYPPM_Id: ed.fyppM_Id });
                                    }
                                }
                            })
                        }
                        else {
                            amount = Number($scope.curramount);
                        }
                        if (Number($scope.curramount) === Number(amount)) {


                            if ($scope.FYP_Id > 0) {
                                var disfun = "Update";
                                var data = {
                                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                                    "Amst_Id": $scope.Amst_Id,
                                    savetmpdata: $scope.savedatatrans,
                                    "FYP_Receipt_No": $scope.FYP_Receipt_No,
                                    "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                                    "FYPcurr_Date": new Date($scope.FYP_Date).toDateString(),
                                    "FYP_Remarks": $scope.FYP_Remarks,
                                    "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                                    // "L_Code": $scope.L_Code,
                                    "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                                    "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                                    "FYP_Tot_Amount": $scope.curramount,
                                    "FYP_Tot_Concession_Amt": $scope.totalconcession,
                                    "FYP_Tot_Fine_Amt": $scope.totalfine,
                                    "FYP_Tot_Waived_Amt": waivedoff,
                                    "FYP_Bank_Name": $scope.FYP_Bank_Name,
                                    "filterinitialdata": $scope.filterdata,
                                    "auto_receipt_flag": autoreceipt,
                                    "automanualreceiptno": automanualreceiptnotranum,
                                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                                    "FYP_Id": $scope.FYP_Id,

                                    Modes: pay_modes,
                                    "FYP_PayModeType": $scope.FYP_PayModeType,
                                    "smsconfig": $scope.obj.allsms,
                                    "emailconfig": $scope.obj.allemail
                                }
                            }
                            else {

                                var disfun = "Save";

                                var data = {
                                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                                    "Amst_Id": $scope.Amst_Id.amst_Id,
                                    savetmpdata: $scope.savedatatrans,
                                    "FYP_Receipt_No": $scope.FYP_Receipt_No,
                                    "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                                    "FYPcurr_Date": new Date($scope.FYP_Date).toDateString(),
                                    "FYP_Remarks": $scope.FYP_Remarks,
                                    "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                                    // "L_Code": $scope.L_Code,
                                    "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                                    "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                                    "FYP_Tot_Amount": $scope.curramount,
                                    "FYP_Tot_Concession_Amt": $scope.totalconcession,
                                    "FYP_Tot_Fine_Amt": $scope.totalfine,
                                    "FYP_Tot_Waived_Amt": waivedoff,
                                    "FYP_Bank_Name": $scope.FYP_Bank_Name,
                                    "filterinitialdata": $scope.filterdata,
                                    "auto_receipt_flag": autoreceipt,
                                    "automanualreceiptno": automanualreceiptnotranum,
                                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,

                                    Modes: pay_modes,
                                    "FYP_PayModeType": $scope.FYP_PayModeType,
                                    "smsconfig": $scope.obj.allsms,
                                    "emailconfig": $scope.obj.allemail
                                }
                            }


                            //var data = {
                            //    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                            //    "Amst_Id": $scope.Amst_Id.amst_Id,
                            //        savetmpdata: $scope.savedatatrans,
                            //    "FYP_Receipt_No": $scope.FYP_Receipt_No,
                            //    "FYP_Date": new Date($scope.FYP_Date),
                            //    "FYP_Remarks": $scope.FYP_Remarks,
                            //    "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                            //    // "L_Code": $scope.L_Code,
                            //    "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                            //    "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                            //    "FYP_Tot_Amount": $scope.curramount,
                            //    "FYP_Tot_Concession_Amt": $scope.totalconcession,
                            //    "FYP_Tot_Fine_Amt": $scope.totalfine,
                            //    "FYP_Tot_Waived_Amt": $scope.totalwaived,
                            //    "FYP_Bank_Name": $scope.FYP_Bank_Name,
                            //    "filterinitialdata": $scope.filterdata,
                            //    "auto_receipt_flag": autoreceipt,
                            //    "automanualreceiptno": automanualreceiptnotranum,
                            //    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                            //        }

                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            }

                            //receipt

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


                                        apiService.create("FeeStudentTransaction/", data).
                                            then(function (promise) {

                                                if (promise.returnval == "true") {
                                                    //reload

                                                    if ($scope.cfg.ASMAY_Id === promise.fillyear[0].asmaY_Id) {
                                                    }
                                                    else {
                                                        $scope.yearlst = promise.fillyear;
                                                        $scope.cfg.ASMAY_Id = promise.fillyear[0].asmaY_Id;
                                                    }

                                                    if (autoreceipt == 1) {
                                                        $scope.showreceiptno = false;
                                                    }
                                                    else {
                                                        $scope.showreceiptno = true;
                                                    }

                                                    $scope.receiptgrid = promise.receiparraydelete;

                                                    $scope.FYP_Date = new Date();
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

                                                        if (grouporterm == "T") {
                                                            $scope.grouportername = "Term Name"
                                                        }
                                                        else if (grouporterm == "G") {
                                                            $scope.grouportername = "Group Name"
                                                        }
                                                    }

                                                    $scope.addnewbtn = true;

                                                    if (promise.displaymessage == null) {
                                                        promise.displaymessage = "Saved/Updated";
                                                    }

                                                    $scope.grigview1 = false;

                                                    $scope.submitted = false;

                                                    $scope.FYP_Date = new Date();
                                                    $scope.FYP_DD_Cheque_Date = new Date();

                                                    //$state.reload();
                                                    //swal("Record " + promise.displaymessage + " Successfully")

                                                    if ($scope.fmC_AutoRecieptPrintFlag == false) {
                                                        swal("Record " + promise.displaymessage + " Successfully")
                                                        $state.reload();
                                                    }
                                                    else {
                                                        $scope.temppyp = promise.fyP_Id;
                                                        $scope.printauto($scope.temppyp, $scope.Amst_Id);
                                                    }

                                                }

                                                else if (promise.returnval == "false") {
                                                    swal("Record " + promise.displaymessage + " Successfully");
                                                    //swal("Kindly contact Administrator");
                                                }
                                                else {
                                                    swal(promise.returnval);
                                                }

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

                    var pay_modes = [];
                    var amount = 0;
                    if ($scope.FYP_PayModeType === 'Multiple' || $scope.FYP_PayModeType === 'Single') {
                        angular.forEach(paymodeee, function (ed) {
                            if (ed.chk_Bank_Multiple != 'null' || ed.rdo_Bank_Multiple != 'null') {
                                if (ed.ivrmmoD_Flag === 'C') {
                                    amount += Number(ed.Bank_Amount);
                                    pay_modes.push({ FYPPM_TransactionTypeFlag: 'C', FYPPM_TotalPaidAmount: Number(ed.Bank_Amount), FYPPM_BankName: '', FYPPM_DDChequeNo: '', FYPPM_DDChequeDate: new Date().toDateString(), IVRMMOD_ModeOfPayment: 'Cash', IVRMMOD_ModeOfPayment_Code: ed.ivrmmoD_ModeOfPayment_Code });
                                }
                                if (ed.ivrmmoD_Flag === 'B') {
                                    amount += Number(ed.Bank_Amount);
                                    pay_modes.push({ FYPPM_TransactionTypeFlag: 'B', FYPPM_TotalPaidAmount: Number(ed.Bank_Amount), FYPPM_BankName: ed.Bank_Name, FYPPM_DDChequeNo: ed.Bank_No, FYPPM_DDChequeDate: new Date(ed.Bank_Date).toDateString(), IVRMMOD_ModeOfPayment: ed.ivrmmoD_ModeOfPayment, IVRMMOD_ModeOfPayment_Code: ed.ivrmmoD_ModeOfPayment_Code });
                                }
                            }
                        })
                    }
                    else {
                        amount = Number($scope.curramount);
                    }


                    if ($scope.curramount > 0) {
                        //  var disfun = "Save";
                        var data = {
                            "ASMAY_Id": $scope.cfg.ASMAY_Id,
                            "Amst_Id": $scope.Amst_Id.amst_Id,
                            savetmpdata: $scope.savedatatrans,
                            "FYP_Receipt_No": $scope.FYP_Receipt_No,
                            "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                            "FYP_Remarks": $scope.FYP_Remarks,
                            "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                            // "L_Code": $scope.L_Code,
                            "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                            "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                            "FYP_Tot_Amount": $scope.curramount,
                            "FYP_Tot_Concession_Amt": $scope.totalconcession,
                            "FYP_Tot_Fine_Amt": $scope.totalfine,
                            "FYP_Tot_Waived_Amt": $scope.totalwaived,
                            "FYP_Bank_Name": $scope.FYP_Bank_Name,
                            "filterinitialdata": $scope.filterdata,
                            "auto_receipt_flag": autoreceipt,
                            "automanualreceiptno": automanualreceiptnotranum,
                            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,

                            Modes: pay_modes,

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

                                    apiService.create("FeeStudentTransaction/Save_Chaln_No", data).
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



        $scope.Applyrebate = function (totalgrid, groupcount, paymodeee) {

            $scope.savedatatrans = [];
            $scope.termwisetot = [];
            var count = 0;
            var type = 0;

            if ($scope.myForm.$valid) {

                angular.forEach($scope.totalgrid, function (opq) {
                    if (opq.isSelected) {
                        count += 1;

                        $scope.savedatatrans.push(opq);
                    }
                })


                if ($scope.savedatatrans.length > 0) {
                    if (grouporterm == "T") {



                        angular.forEach($scope.totalgrid, function (t3) {
                            var al_cnt = 0;
                            angular.forEach($scope.savedatatrans, function (rt) {
                                if (parseInt(rt.fmT_Id) === parseInt(t3.fmT_Id)) {

                                    al_cnt += Number(rt.fsS_ToBePaid);


                                }
                            });

                            if ($scope.termwisetot.length == 0) {
                                $scope.termwisetot.push({
                                    FMT_Id: t3.fmT_Id,
                                    FMT_Total: al_cnt

                                });
                            }
                            else {
                                var intcount = 0;
                                angular.forEach($scope.termwisetot, function (emp) {
                                    if (emp.FMT_Id === t3.fmT_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.termwisetot.push({

                                        FMT_Id: t3.fmT_Id,
                                        FMT_Total: al_cnt

                                    });
                                }
                            }
                        });

                        $scope.termwisetotalamt = [];


                        angular.forEach($scope.termwiseamount, function (t3) {
                            angular.forEach($scope.termwisetot, function (rt) {
                                if (rt.FMT_Id === t3.FMT_Id && rt.FMT_Total >= t3.total) {
                                    $scope.termwisetotalamt.push({

                                        Fmtidnew: t3.FMT_Id,
                                        FMT_Total: rt.FMT_Total

                                    });
                                }

                            });
                        });

                        if ($scope.termwiseamount.length == $scope.termwisetot.length) {
                            type = 0;
                        }
                        else {
                            type = 1;
                        }
                    }
                    if (grouporterm == "G") {

                        angular.forEach($scope.totalgrid, function (t3) {
                            var al_cnt = 0;
                            angular.forEach($scope.savedatatrans, function (rt) {
                                if (parseInt(rt.ftI_Id) === parseInt(t3.ftI_Id) && parseInt(rt.fmG_Id) === parseInt(t3.fmG_Id)) {

                                    al_cnt += Number(rt.fsS_ToBePaid);


                                }
                            });

                            if ($scope.termwisetot.length == 0) {
                                $scope.termwisetot.push({
                                    FTI_Id: t3.ftI_Id,
                                    FMT_Total: al_cnt,
                                    FMG_Id: t3.fmG_Id

                                });
                            }
                            else {
                                var intcount = 0;
                                angular.forEach($scope.termwisetot, function (emp) {
                                    if (emp.FTI_Id === t3.ftI_Id && emp.FMG_Id === t3.fmG_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.termwisetot.push({

                                        FTI_Id: t3.ftI_Id,
                                        FMT_Total: al_cnt,
                                        FMG_Id: t3.fmG_Id

                                    });
                                }
                            }
                        });

                        $scope.termwisetotalamt = [];


                        angular.forEach($scope.groupwiseamount, function (t3) {
                            angular.forEach($scope.termwisetot, function (rt) {
                                if (rt.FTI_Id === t3.FTI_Id && rt.FMG_Id === t3.FMG_Id && rt.FMT_Total >= t3.total) {
                                    $scope.termwisetotalamt.push({

                                        Fmtidnew: t3.FTI_Id,
                                        FMT_Total: rt.FMT_Total,
                                        FMG_Id: t3.FMG_Id

                                    });
                                }

                            });
                        });

                        if ($scope.termwiseamount.length == $scope.termwisetot.length) {
                            type = 0;
                        }
                        else {
                            type = 1;
                        }

                    }





                    var data = {
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "Amst_Id": $scope.Amst_Id.amst_Id,
                        savetmpdata: $scope.savedatatrans,
                        FMTtotal: $scope.termwisetotalamt,

                        "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                        "transtype": type,
                        "configset": grouporterm


                    }





                    apiService.create("FeeStudentTransaction/rebateamountcalc/", data).
                        then(function (promise) {

                            $scope.rebateamount = promise.rebateamount;
                            $scope.curramount = $scope.curramount - $scope.rebateamount;

                            if ($scope.FYP_PayModeType === 'Single') {
                                angular.forEach($scope.paymodeee, function (ed) {
                                    if (ed.rdo_Bank_Multiple === 'singlee') {
                                        ed.Bank_Amount = $scope.curramount;
                                    }
                                })
                            }
                            if ($scope.FYP_PayModeType === 'Multiple') {
                                angular.forEach($scope.paymodeee, function (ed) {
                                    if (ed.chk_Bank_Multiple === 'multii') {
                                        ed.Bank_Amount = $scope.curramount;
                                    }
                                })
                            }

                            $scope.Rebateapplyandsave(totalgrid, groupcount, paymodeee);




                        })


                }

                else {
                    swal("Select atleast one check box");
                }



            }

            else {
                $scope.submitted = true;
            }

        };


        $scope.Rebateapplyandsave = function (totalgrid, groupcount, paymodeee) {

            $scope.savedatatrans = [];
            var count = 0;

            if ($scope.myForm.$valid) {

                if ($scope.filterdata != 'Challan_No') {

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
                                                        toBePaid += Number(s2.fsS_ToBePaid);
                                                    })
                                                    if (toBePaid == Number(opq.fsS_ToBePaid)) {
                                                        angular.forEach(s1.sp_ind_list, function (s2) {
                                                            count += 1;
                                                            $scope.savedatatrans.push(s2);
                                                        })
                                                    }
                                                    else if (toBePaid > Number(opq.fsS_ToBePaid)) {

                                                        var keepGoing = true;
                                                        angular.forEach(s1.sp_ind_list, function (s2) {
                                                            if (keepGoing) {
                                                                if (Number(opq.fsS_ToBePaid) >= Number(s2.fsS_ToBePaid)) {
                                                                    count += 1;
                                                                    $scope.savedatatrans.push(s2);
                                                                    opq.fsS_ToBePaid = (Number(opq.fsS_ToBePaid) - Number(s2.fsS_ToBePaid));
                                                                }
                                                                else if (Number(opq.fsS_ToBePaid) < Number(s2.fsS_ToBePaid)) {
                                                                    s2.fsS_ToBePaid = Number(opq.fsS_ToBePaid);
                                                                    count += 1;
                                                                    $scope.savedatatrans.push(s2);
                                                                    opq.fsS_ToBePaid = (Number(opq.fsS_ToBePaid) - Number(s2.fsS_ToBePaid));
                                                                }
                                                                if (Number(opq.fsS_ToBePaid) == 0) {
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

                    if ($scope.curramount > 0) {

                        var pay_modes = [];
                        var amount = 0;
                        if ($scope.FYP_PayModeType === 'Multiple' || $scope.FYP_PayModeType === 'Single') {
                            angular.forEach(paymodeee, function (ed) {
                                if (ed.chk_Bank_Multiple != 'null' || ed.rdo_Bank_Multiple != 'null') {
                                    if (ed.ivrmmoD_Flag === 'C') {
                                        amount += Number(ed.Bank_Amount);
                                        pay_modes.push({ FYPPM_TransactionTypeFlag: 'C', FYPPM_TotalPaidAmount: Number(ed.Bank_Amount), FYPPM_BankName: '', FYPPM_DDChequeNo: '', FYPPM_DDChequeDate: new Date().toDateString(), IVRMMOD_ModeOfPayment: 'Cash', IVRMMOD_ModeOfPayment_Code: ed.ivrmmoD_ModeOfPayment_Code, FYPPM_Id: ed.fyppM_Id });
                                    }
                                    if (ed.ivrmmoD_Flag === 'B') {
                                        amount += Number(ed.Bank_Amount);
                                        pay_modes.push({ FYPPM_TransactionTypeFlag: 'B', FYPPM_TotalPaidAmount: Number(ed.Bank_Amount), FYPPM_BankName: ed.Bank_Name, FYPPM_DDChequeNo: ed.Bank_No, FYPPM_DDChequeDate: new Date(ed.Bank_Date).toDateString(), IVRMMOD_ModeOfPayment: ed.ivrmmoD_ModeOfPayment, IVRMMOD_ModeOfPayment_Code: ed.ivrmmoD_ModeOfPayment_Code, FYPPM_Id: ed.fyppM_Id });
                                    }
                                }
                            })
                        }
                        else {
                            amount = Number($scope.curramount);
                        }
                        if (Number($scope.curramount) === Number(amount)) {


                            if ($scope.FYP_Id > 0) {
                                var disfun = "Update";
                                var data = {
                                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                                    "Amst_Id": $scope.Amst_Id,
                                    savetmpdata: $scope.savedatatrans,
                                    "FYP_Receipt_No": $scope.FYP_Receipt_No,
                                    "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                                    "FYPcurr_Date": new Date($scope.FYP_Date).toDateString(),
                                    "FYP_Remarks": $scope.FYP_Remarks,
                                    "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,

                                    "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                                    "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                                    "FYP_Tot_Amount": $scope.curramount,
                                    "FYP_Tot_Concession_Amt": $scope.totalconcession,
                                    "FYP_Tot_Fine_Amt": $scope.totalfine,
                                    "FYP_Tot_Waived_Amt": waivedoff,
                                    "FYP_Bank_Name": $scope.FYP_Bank_Name,
                                    "filterinitialdata": $scope.filterdata,
                                    "auto_receipt_flag": autoreceipt,
                                    "automanualreceiptno": automanualreceiptnotranum,
                                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                                    "FYP_Id": $scope.FYP_Id,

                                    Modes: pay_modes,
                                    "FYP_PayModeType": $scope.FYP_PayModeType,
                                    "smsconfig": $scope.obj.allsms,
                                    "emailconfig": $scope.obj.allemail,
                                    "FSS_RebateAmount": $scope.rebateamount
                                }
                            }
                            else {

                                var disfun = "Save";

                                var data = {
                                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                                    "Amst_Id": $scope.Amst_Id.amst_Id,
                                    savetmpdata: $scope.savedatatrans,
                                    "FYP_Receipt_No": $scope.FYP_Receipt_No,
                                    "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                                    "FYPcurr_Date": new Date($scope.FYP_Date).toDateString(),
                                    "FYP_Remarks": $scope.FYP_Remarks,
                                    "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                                    // "L_Code": $scope.L_Code,
                                    "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                                    "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                                    "FYP_Tot_Amount": $scope.curramount,
                                    "FYP_Tot_Concession_Amt": $scope.totalconcession,
                                    "FYP_Tot_Fine_Amt": $scope.totalfine,
                                    "FYP_Tot_Waived_Amt": waivedoff,
                                    "FYP_Bank_Name": $scope.FYP_Bank_Name,
                                    "filterinitialdata": $scope.filterdata,
                                    "auto_receipt_flag": autoreceipt,
                                    "automanualreceiptno": automanualreceiptnotranum,
                                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,

                                    Modes: pay_modes,
                                    "FYP_PayModeType": $scope.FYP_PayModeType,
                                    "smsconfig": $scope.obj.allsms,
                                    "emailconfig": $scope.obj.allemail,
                                    "FSS_RebateAmount": $scope.rebateamount
                                }
                            }



                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            }



                            swal({
                                title: "Are you sure?",
                                text: "Payable Amount " + $scope.curramount + "  \n Rebate Amount " + $scope.rebateamount + " \n Do You Want To " + disfun + " Record? ",
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


                                        apiService.create("FeeStudentTransaction/Rebateapplyandsave/", data).
                                            then(function (promise) {

                                                if (promise.returnval == "true") {
                                                    //reload

                                                    if ($scope.cfg.ASMAY_Id === promise.fillyear[0].asmaY_Id) {
                                                    }
                                                    else {
                                                        $scope.yearlst = promise.fillyear;
                                                        $scope.cfg.ASMAY_Id = promise.fillyear[0].asmaY_Id;
                                                    }

                                                    if (autoreceipt == 1) {
                                                        $scope.showreceiptno = false;
                                                    }
                                                    else {
                                                        $scope.showreceiptno = true;
                                                    }

                                                    $scope.receiptgrid = promise.receiparraydelete;

                                                    $scope.FYP_Date = new Date();
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

                                                        if (grouporterm == "T") {
                                                            $scope.grouportername = "Term Name"
                                                        }
                                                        else if (grouporterm == "G") {
                                                            $scope.grouportername = "Group Name"
                                                        }
                                                    }

                                                    $scope.addnewbtn = true;

                                                    if (promise.displaymessage == null) {
                                                        promise.displaymessage = "Saved/Updated";
                                                    }

                                                    $scope.grigview1 = false;

                                                    $scope.submitted = false;

                                                    $scope.FYP_Date = new Date();
                                                    $scope.FYP_DD_Cheque_Date = new Date();

                                                    //$state.reload();
                                                    //swal("Record " + promise.displaymessage + " Successfully")

                                                    if ($scope.fmC_AutoRecieptPrintFlag == false) {
                                                        swal("Record " + promise.displaymessage + " Successfully")
                                                        $state.reload();
                                                    }
                                                    else {
                                                        $scope.temppyp = promise.fyP_Id;
                                                        $scope.printauto($scope.temppyp, $scope.Amst_Id);
                                                    }

                                                }

                                                else if (promise.returnval == "false") {
                                                    swal("Record " + promise.displaymessage + " Successfully");
                                                    //swal("Kindly contact Administrator");
                                                }
                                                else {
                                                    swal(promise.returnval);
                                                }

                                            })
                                    }
                                    else {



                                        swal("Record saved Failed", "Failed");
                                        $scope.curramount = $scope.curramount + $scope.rebateamount;
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

                    var pay_modes = [];
                    var amount = 0;
                    if ($scope.FYP_PayModeType === 'Multiple' || $scope.FYP_PayModeType === 'Single') {
                        angular.forEach(paymodeee, function (ed) {
                            if (ed.chk_Bank_Multiple != 'null' || ed.rdo_Bank_Multiple != 'null') {
                                if (ed.ivrmmoD_Flag === 'C') {
                                    amount += Number(ed.Bank_Amount);
                                    pay_modes.push({ FYPPM_TransactionTypeFlag: 'C', FYPPM_TotalPaidAmount: Number(ed.Bank_Amount), FYPPM_BankName: '', FYPPM_DDChequeNo: '', FYPPM_DDChequeDate: new Date().toDateString(), IVRMMOD_ModeOfPayment: 'Cash', IVRMMOD_ModeOfPayment_Code: ed.ivrmmoD_ModeOfPayment_Code });
                                }
                                if (ed.ivrmmoD_Flag === 'B') {
                                    amount += Number(ed.Bank_Amount);
                                    pay_modes.push({ FYPPM_TransactionTypeFlag: 'B', FYPPM_TotalPaidAmount: Number(ed.Bank_Amount), FYPPM_BankName: ed.Bank_Name, FYPPM_DDChequeNo: ed.Bank_No, FYPPM_DDChequeDate: new Date(ed.Bank_Date).toDateString(), IVRMMOD_ModeOfPayment: ed.ivrmmoD_ModeOfPayment, IVRMMOD_ModeOfPayment_Code: ed.ivrmmoD_ModeOfPayment_Code });
                                }
                            }
                        })
                    }
                    else {
                        amount = Number($scope.curramount);
                    }


                    if ($scope.curramount > 0) {
                        //  var disfun = "Save";
                        var data = {
                            "ASMAY_Id": $scope.cfg.ASMAY_Id,
                            "Amst_Id": $scope.Amst_Id.amst_Id,
                            savetmpdata: $scope.savedatatrans,
                            "FYP_Receipt_No": $scope.FYP_Receipt_No,
                            "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                            "FYP_Remarks": $scope.FYP_Remarks,
                            "FYP_Bank_Or_Cash": $scope.FYP_Bank_Or_Cash,
                            // "L_Code": $scope.L_Code,
                            "FYP_DD_Cheque_Date": new Date($scope.FYP_DD_Cheque_Date).toDateString(),
                            "FYP_DD_Cheque_No": $scope.FYP_DD_Cheque_No,
                            "FYP_Tot_Amount": $scope.curramount,
                            "FYP_Tot_Concession_Amt": $scope.totalconcession,
                            "FYP_Tot_Fine_Amt": $scope.totalfine,
                            "FYP_Tot_Waived_Amt": $scope.totalwaived,
                            "FYP_Bank_Name": $scope.FYP_Bank_Name,
                            "filterinitialdata": $scope.filterdata,
                            "auto_receipt_flag": autoreceipt,
                            "automanualreceiptno": automanualreceiptnotranum,
                            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,

                            Modes: pay_modes,

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

                                    apiService.create("FeeStudentTransaction/Save_Chaln_No", data).
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


        //added






        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.closedata = function () {
            $scope.FYP_DD_Cheque_Date = new Date();
            $scope.FYP_Date = new Date();
            $scope.FYP_DD_Cheque_No = "";
            $scope.FYP_Bank_Name = "";
            $scope.FYP_Remarks = "";
            $scope.formload();
            //$state.reload();
        }

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
                        "Amst_Id": studentid,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "FYP_Remarks": inputValue
                    }

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("FeeStudentTransaction/Deletedetails", data).
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
                "FYP_Receipt_No": receiptno,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeStudentTransaction/feereceiptduplicate", data).
                then(function (promise) {
                    if (promise.duplicatereceipt.length > 0) {
                        swal("Duplicate Receipt No")
                        if ($scope.FYP_Id > 0) {
                            $scope.FYP_Receipt_No = "";
                        }
                        else {
                            $scope.FYP_Receipt_No = receiptno;
                        }
                    }
                })
        }

        $scope.studentlst = [];

        $scope.searchfilter = function (objj, radioobj) {

          //  if (institutionid == '5' || institutionid == '4' || institutionid == '3' || institutionid == '6' || institutionid == '8' || institutionid == '17' || institutionid == '10001' || institutionid == '22' || institutionid == '7') {
                if (objj.search.length >= $scope.searchby && radioobj == 'regular') {
                    // $scope.studentlst = {};
                    if ($scope.filterdata1 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL":"L"
                        }

                    }
                    else if ($scope.filterdata2 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "D"
                        }

                    }
                    else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        }

                    }
                    else if ($scope.filterdata4 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                            "ASMS_Id": $scope.cfg.ASMS_Id,
                        }

                    }

                    else {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S"
                        }
                    }
                    
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    /// $scope.studentlst = promise.fillstudent;
                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;

                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                            //MB For Challan
                            if ($scope.filterdata == 'Challan_No') {
                                // $scope.Amst_Id = {};
                                //$scope.Amst_Id.amst_Id = temp_amst_Id;
                                angular.forEach($scope.studentlst, function (ob) {
                                    if (ob.amst_Id == temp_amst_Id) {
                                        ob.Selected = true;
                                        $scope.Amst_Id = ob;
                                    }
                                })

                                $scope.onselectstudent($scope.Amst_Id);


                            }
                            //MB For Challan
                        })
                }

                if (objj.search.length >= $scope.searchby && radioobj == 'AdmNo') {
                    //$scope.studentlst = {};
                    //var data = {
                    //    "filterinitialdata": radioobj,
                    //    "searchfilter": objj.search,
                    //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                    //}
                    if ($scope.filterdata1 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "L"
                        }

                    }
                    else if ($scope.filterdata2 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "D"
                        }

                    }
                    else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        }

                    }

                    else if ($scope.filterdata4 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                            "ASMS_Id": $scope.cfg.ASMS_Id
                        }

                    }
                    else {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S"
                        }
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    /// $scope.studentlst = promise.fillstudent;
                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;

                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })

                        })
                }

                else if (objj.search.length >= $scope.searchby && radioobj == 'regno') {
                    //$scope.studentlst = {};
                    //var data = {
                    //    "filterinitialdata": radioobj,
                    //    "searchfilter": objj.search,
                    //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                    //}
                    if ($scope.filterdata1 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "L"
                        }

                    }
                    else if ($scope.filterdata2 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "D"
                        }

                    }
                    else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        }

                    }
                    else if ($scope.filterdata4 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                            "ASMS_Id": $scope.cfg.ASMS_Id,
                        }

                    }
                    else {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S"
                        }
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    /// $scope.studentlst = promise.fillstudent;
                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;

                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })

                        })
                }

                else if (objj.search.length >= $scope.searchby && radioobj == 'NameAdmno') {
                    // $scope.studentlst = {};
                    //var data = {
                    //    "filterinitialdata": radioobj,
                    //    "searchfilter": objj.search,
                    //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                    //}
                    if ($scope.filterdata1 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "L"
                        }

                    }
                    else if ($scope.filterdata2 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "D"
                        }

                    }
                    else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        }

                    }
                    else if ($scope.filterdata4 == true && $scope.filterdata3 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                            "ASMS_Id": $scope.cfg.ASMS_Id,
                        }

                    }
                    else {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S"
                        }
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    /// $scope.studentlst = promise.fillstudent;
                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }


                else if (objj.search.length >= $scope.searchby && radioobj == 'Admnoname') {
                    //$scope.studentlst = {};
                    //var data = {
                    //    "filterinitialdata": radioobj,
                    //    "searchfilter": objj.search,
                    //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                    //}
                    if ($scope.filterdata1 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "L"
                        }

                    }
                    else if ($scope.filterdata2 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "D"
                        }

                    }
                    else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        }

                    }

                    else if ($scope.filterdata4 == true ) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                            "ASMS_Id": $scope.cfg.ASMS_Id
                        }

                    }
                    else {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S"
                        }
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }


                //radha

                else if (objj.search.length >= $scope.searchby && radioobj == 'NameRegNo') {

                    //var data = {
                    //    "filterinitialdata": radioobj,
                    //    "searchfilter": objj.search,
                    //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                    //}
                    if ($scope.filterdata1 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "L"
                        }

                    }
                    else if ($scope.filterdata2 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "D"
                        }

                    }
                    else if ($scope.filterdata3 == true &&( $scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        }

                    }

                    else if ($scope.filterdata4 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                            "ASMS_Id": $scope.cfg.ASMS_Id,
                        }

                    }
                    else {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id
                        }
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }


                else if (objj.search.length >= $scope.searchby && radioobj == 'RegNoName') {

                    //var data = {
                    //    "filterinitialdata": radioobj,
                    //    "searchfilter": objj.search,
                    //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                    //}
                    if ($scope.filterdata1 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "L"
                        }

                    }
                    else if ($scope.filterdata2 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "D"
                        }

                    }
                    else if ($scope.filterdata3 == true && ($scope.filterdata4 == false ||  $scope.filterdata4 == undefined)) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        }

                    }
                    else if ($scope.filterdata4 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                            "ASMS_Id": $scope.cfg.ASMS_Id,
                        }

                    }
                    else {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S"
                        }
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })

                }

                else if (objj.search.length >= $scope.searchby && radioobj == 'left') {
                    // $scope.studentlst = {};
                    //var data = {
                    //    "filterinitialdata": radioobj,
                    //    "searchfilter": objj.search,
                    //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                    //}
                    if ($scope.filterdata1 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "L"
                        }

                    }
                    else if ($scope.filterdata2 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "D"
                        }

                    }
                    else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        }

                    }
                    else if ($scope.filterdata4 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                            "ASMS_Id": $scope.cfg.ASMS_Id,
                        }

                    }
                    else {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S"
                        }
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    /// $scope.studentlst = promise.fillstudent;
                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
                }

                else if (objj.search.length >= $scope.searchby && radioobj == 'inactive') {
                    // $scope.studentlst = {};
                    //var data = {
                    //    "filterinitialdata": radioobj,
                    //    "searchfilter": objj.search,
                    //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                    //}
                    if ($scope.filterdata1 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "L"
                        }

                    }
                    else if ($scope.filterdata2 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "D"
                        }

                    }
                    else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        }

                    }
                    else if ($scope.filterdata4 == true) {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S",
                            "ASMCL_ID": $scope.cfg.ASMCL_Id,
                            "ASMS_Id": $scope.cfg.ASMS_Id
                        }

                    }
                    else {
                        var data = {
                            "filterinitialdata": radioobj,
                            "searchfilter": objj.search,
                            "ASMAY_ID": $scope.cfg.ASMAY_Id,
                            "AMST_SOL": "S"
                        }
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    /// $scope.studentlst = promise.fillstudent;
                    apiService.create("FeeStudentTransaction/searchfilter", data).
                        then(function (promise) {
                            $scope.studentlst = promise.fillstudent;
                            angular.forEach($scope.studentlst, function (objectt) {
                                if (objectt.amsT_FirstName.length > 0) {
                                    var string = objectt.amsT_FirstName;
                                    objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                                }
                            })
                        })
            }

                else if (objj.search.length >= $scope.searchby && radioobj == 'Mothername') {

                //var data = {
                //    "filterinitialdata": radioobj,
                //    "searchfilter": objj.search,
                //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                //}
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }
                else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S",
                        "ASMCL_ID": $scope.cfg.ASMCL_Id,
                    }

                }
                else if ($scope.filterdata4 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S",
                        "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        "ASMS_Id": $scope.cfg.ASMS_Id,
                    }

                }
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeStudentTransaction/searchfilter", data).
                    then(function (promise) {
                        $scope.studentlst = promise.fillstudent;
                        angular.forEach($scope.studentlst, function (objectt) {
                            if (objectt.amsT_FirstName.length > 0) {
                                var string = objectt.amsT_FirstName;
                                objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })

            }

                else if (objj.search.length >= $scope.searchby && radioobj == 'Fathername') {

                //var data = {
                //    "filterinitialdata": radioobj,
                //    "searchfilter": objj.search,
                //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                //}
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }
                else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S",
                        "ASMCL_ID": $scope.cfg.ASMCL_Id,
                    }

                }
                else if ($scope.filterdata4 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S",
                        "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        "ASMS_Id": $scope.cfg.ASMS_Id,
                    }

                }
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeStudentTransaction/searchfilter", data).
                    then(function (promise) {
                        $scope.studentlst = promise.fillstudent;
                        angular.forEach($scope.studentlst, function (objectt) {
                            if (objectt.amsT_FirstName.length > 0) {
                                var string = objectt.amsT_FirstName;
                                objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })

            }
                else if (objj.search.length >= $scope.searchby && radioobj == 'MobileNo') {

                //var data = {
                //    "filterinitialdata": radioobj,
                //    "searchfilter": objj.search,
                //    "ASMAY_ID": $scope.cfg.ASMAY_Id
                //}
                if ($scope.filterdata1 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "L"
                    }

                }
                else if ($scope.filterdata2 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "D"
                    }

                }
                else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S",
                        "ASMCL_ID": $scope.cfg.ASMCL_Id,
                    }

                }
                else if ($scope.filterdata4 == true) {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S",
                        "ASMCL_ID": $scope.cfg.ASMCL_Id,
                        "ASMS_Id": $scope.cfg.ASMS_Id,
                    }

                }
                else {
                    var data = {
                        "filterinitialdata": radioobj,
                        "searchfilter": objj.search,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id,
                        "AMST_SOL": "S"
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeStudentTransaction/searchfilter", data).
                    then(function (promise) {
                        $scope.studentlst = promise.fillstudent;
                        angular.forEach($scope.studentlst, function (objectt) {
                            if (objectt.amsT_FirstName.length > 0) {
                                var string = objectt.amsT_FirstName;
                                objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })

            }



                //radha

            //}
            //else {

            //    if (objj.search.length >= '1' && radioobj == 'regular') {
            //        // $scope.studentlst = "";
            //        //var data = {
            //        //    "filterinitialdata": radioobj,
            //        //    "searchfilter": objj.search,
            //        //    "ASMAY_ID": $scope.cfg.ASMAY_Id
            //        //}
            //        if ($scope.filterdata1 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "L"
            //            }

            //        }
            //        else if ($scope.filterdata2 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "D"
            //            }

            //        }
            //        else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //            }

            //        }
            //        else if ($scope.filterdata4 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //                "ASMS_Id": $scope.cfg.ASMS_Id,
            //            }

            //        }
            //        else {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S"
            //            }
            //        }
            //        var config = {
            //            headers: {
            //                'Content-Type': 'application/json;'
            //            }
            //        }
            //        /// $scope.studentlst = promise.fillstudent;
            //        apiService.create("FeeStudentTransaction/searchfilter", data).
            //            then(function (promise) {
            //                $scope.studentlst = promise.fillstudent;
            //                angular.forEach($scope.studentlst, function (objectt) {
            //                    if (objectt.amsT_FirstName.length > 0) {
            //                        var string = objectt.amsT_FirstName;
            //                        objectt.amsT_FirstName = string.replace(/  +/g, ' ');
            //                    }
            //                })

            //                //MB For Challan
            //                if ($scope.filterdata == 'Challan_No') {
            //                    // $scope.Amst_Id = {};
            //                    //$scope.Amst_Id.amst_Id = temp_amst_Id;
            //                    angular.forEach($scope.studentlst, function (ob) {
            //                        if (ob.amst_Id == temp_amst_Id) {
            //                            ob.Selected = true;
            //                            $scope.Amst_Id = ob;
            //                        }
            //                    })

            //                    $scope.onselectstudent($scope.Amst_Id);


            //                }
            //                //MB For Challan
            //            })
            //    }

            //    if (objj.search.length >= '2' && radioobj == 'AdmNo') {
            //        // $scope.studentlst = "";
            //        //var data = {
            //        //    "filterinitialdata": radioobj,
            //        //    "searchfilter": objj.search,
            //        //    "ASMAY_ID": $scope.cfg.ASMAY_Id
            //        //}
            //        if ($scope.filterdata1 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "L"
            //            }

            //        }
            //        else if ($scope.filterdata2 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "D"
            //            }

            //        }
            //        else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //            }

            //        }
            //        else if ($scope.filterdata4 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //                "ASMS_Id": $scope.cfg.ASMS_Id,
            //            }

            //        }
            //        else {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S"
            //            }
            //        }

            //        var config = {
            //            headers: {
            //                'Content-Type': 'application/json;'
            //            }
            //        }
            //        /// $scope.studentlst = promise.fillstudent;
            //        apiService.create("FeeStudentTransaction/searchfilter", data).
            //            then(function (promise) {
            //                $scope.studentlst = promise.fillstudent;
            //                angular.forEach($scope.studentlst, function (objectt) {
            //                    if (objectt.amsT_FirstName.length > 0) {
            //                        var string = objectt.amsT_FirstName;
            //                        objectt.amsT_FirstName = string.replace(/  +/g, ' ');
            //                    }
            //                })
            //            })
            //    }

            //    else if (objj.search.length >= '2' && radioobj == 'regno') {
            //        // $scope.studentlst = "";
            //        //var data = {
            //        //    "filterinitialdata": radioobj,
            //        //    "searchfilter": objj.search,
            //        //    "ASMAY_ID": $scope.cfg.ASMAY_Id
            //        //}
            //        if ($scope.filterdata1 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "L"
            //            }

            //        }
            //        else if ($scope.filterdata2 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "D"
            //            }

            //        }
            //        else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //            }

            //        }

            //        else if ($scope.filterdata4 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //                "ASMS_Id": $scope.cfg.ASMS_Id,
            //            }

            //        }
            //        else {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S"
            //            }
            //        }
            //        var config = {
            //            headers: {
            //                'Content-Type': 'application/json;'
            //            }
            //        }
            //        /// $scope.studentlst = promise.fillstudent;
            //        apiService.create("FeeStudentTransaction/searchfilter", data).
            //            then(function (promise) {
            //                $scope.studentlst = promise.fillstudent;
            //                angular.forEach($scope.studentlst, function (objectt) {
            //                    if (objectt.amsT_FirstName.length > 0) {
            //                        var string = objectt.amsT_FirstName;
            //                        objectt.amsT_FirstName = string.replace(/  +/g, ' ');
            //                    }
            //                })
            //            })
            //    }

            //    else if (objj.search.length >= '2' && radioobj == 'NameAdmno') {
            //        //   $scope.studentlst = "";
            //        //var data = {
            //        //    "filterinitialdata": radioobj,
            //        //    "searchfilter": objj.search,
            //        //    "ASMAY_ID": $scope.cfg.ASMAY_Id
            //        //}
            //        if ($scope.filterdata1 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "L"
            //            }

            //        }
            //        else if ($scope.filterdata2 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "D"
            //            }

            //        }
            //        else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //            }

            //        }

            //        else if ($scope.filterdata4 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //                "ASMS_Id": $scope.cfg.ASMS_Id,
            //            }

            //        }
            //        else {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S"
            //            }
            //        }
            //        var config = {
            //            headers: {
            //                'Content-Type': 'application/json;'
            //            }
            //        }
            //        /// $scope.studentlst = promise.fillstudent;
            //        apiService.create("FeeStudentTransaction/searchfilter", data).
            //            then(function (promise) {
            //                $scope.studentlst = promise.fillstudent;
            //                angular.forEach($scope.studentlst, function (objectt) {
            //                    if (objectt.amsT_FirstName.length > 0) {
            //                        var string = objectt.amsT_FirstName;
            //                        objectt.amsT_FirstName = string.replace(/  +/g, ' ');
            //                    }
            //                })
            //            })
            //    }


            //    else if (objj.search.length >= '2' && radioobj == 'Admnoname') {
            //        // $scope.studentlst = "";
            //        //var data = {
            //        //    "filterinitialdata": radioobj,
            //        //    "searchfilter": objj.search,
            //        //    "ASMAY_ID": $scope.cfg.ASMAY_Id
            //        //}
            //        if ($scope.filterdata1 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "L"
            //            }

            //        }
            //        else if ($scope.filterdata2 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "D"
            //            }

            //        }
            //        else if ($scope.filterdata3 == true && ($scope.filterdata4 == false || $scope.filterdata4 == undefined)) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //            }

            //        }
            //        else if ($scope.filterdata4 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //                "ASMS_Id": $scope.cfg.ASMS_Id
            //            }

            //        }
            //        else {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S"
            //            }
            //        }
            //        var config = {
            //            headers: {
            //                'Content-Type': 'application/json;'
            //            }
            //        }

            //        apiService.create("FeeStudentTransaction/searchfilter", data).
            //            then(function (promise) {
            //                $scope.studentlst = promise.fillstudent;
            //                angular.forEach($scope.studentlst, function (objectt) {
            //                    if (objectt.amsT_FirstName.length > 0) {
            //                        var string = objectt.amsT_FirstName;
            //                        objectt.amsT_FirstName = string.replace(/  +/g, ' ');
            //                    }
            //                })
            //            })
            //    }


            //    else if (objj.search.length >= '2' && radioobj == 'inactive') {
            //        // $scope.studentlst = {};
            //        //var data = {
            //        //    "filterinitialdata": radioobj,
            //        //    "searchfilter": objj.search,
            //        //    "ASMAY_ID": $scope.cfg.ASMAY_Id
            //        //}
            //        if ($scope.filterdata1 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "L"
            //            }

            //        }
            //        else if ($scope.filterdata2 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "D"
            //            }

            //        }
            //        else if ($scope.filterdata3 == true && ($scope.filterdata4 == true || $scope.filterdata4 == undefined)) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //            }

            //        }

            //        else if ($scope.filterdata4 == true) {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S",
            //                "ASMCL_ID": $scope.cfg.ASMCL_Id,
            //                "ASMS_Id": $scope.cfg.ASMS_Id,
            //            }

            //        }
            //        else {
            //            var data = {
            //                "filterinitialdata": radioobj,
            //                "searchfilter": objj.search,
            //                "ASMAY_ID": $scope.cfg.ASMAY_Id,
            //                "AMST_SOL": "S"
            //            }
            //        }
            //        var config = {
            //            headers: {
            //                'Content-Type': 'application/json;'
            //            }
            //        }
            //        /// $scope.studentlst = promise.fillstudent;
            //        apiService.create("FeeStudentTransaction/searchfilter", data).
            //            then(function (promise) {
            //                $scope.studentlst = promise.fillstudent;
            //                angular.forEach($scope.studentlst, function (objectt) {
            //                    if (objectt.amsT_FirstName.length > 0) {
            //                        var string = objectt.amsT_FirstName;
            //                        objectt.amsT_FirstName = string.replace(/  +/g, ' ');
            //                    }
            //                })
            //            })
            //    }



            //}

            // }                
        };



        //$scope.search_flag = false;
        //$scope.search123 = "";
        //var search_s = "";
        //var list_s = [];
        //$scope.onselectsearch = function () {
        //    search_s = $scope.search123;
        //    list_s = $scope.receiptgrid;
        //    if (search_s == "" || search_s == undefined) {
        //        swal("Select Any Field For Search");
        //        $scope.search_flag = false;
        //    }
        //    else {
        //        $scope.search_flag = true;

        //    }

        //}
        //var search_list = [];
        //$scope.searchtext = "";
        //$scope.ShowSearchReport = function () {
        //    
        //    search_list = [];
        //    if ($scope.searchtext == "") {
        //        swal("Text Is Needed For Search ");
        //    }
        //    else {
        //        var substring = $scope.searchtext.toLowerCase();
        //        if ($scope.search123 == "0") {
        //            
        //            // var substring = $scope.searchtext.toLowerCase();
        //            angular.forEach(list_s, function (itm_s) {
        //                var string1 = itm_s.amsT_FirstName.toLowerCase();
        //                var string2 = itm_s.amsT_MiddleName.toLowerCase();
        //                var string3 = itm_s.amsT_LastName.toLowerCase();
        //                //   substring = "oo";
        //                //string.indexOf($scope.searchtext) !== -1
        //                // if (string.indexOf($scope.searchtext) !== -1)
        //                if (string1.includes(substring) || string2.includes(substring) || string3.includes(substring)) {
        //                    search_list.push(itm_s);
        //                }
        //            });
        //            $scope.receiptgrid = search_list;
        //        }
        //        if ($scope.search123 == "1") {
        //            
        //            angular.forEach(list_s, function (itm_s) {
        //                var string1 = itm_s.classname.toLowerCase();

        //                if (string1.startsWith(substring)) {
        //                    search_list.push(itm_s);
        //                }
        //            });
        //            $scope.receiptgrid = search_list;
        //        }
        //        if ($scope.search123 == "2") {
        //            
        //            angular.forEach(list_s, function (itm_s) {
        //                var string1 = itm_s.amsT_AdmNo.toLowerCase();

        //                if (string1.includes(substring)) {
        //                    search_list.push(itm_s);
        //                }
        //            });
        //            $scope.receiptgrid = search_list;
        //        }
        //        if ($scope.search123 == "3") {
        //            
        //            angular.forEach(list_s, function (itm_s) {
        //                var string1 = itm_s.fyP_Receipt_No.toLowerCase();

        //                if (string1.includes(substring)) {
        //                    search_list.push(itm_s);
        //                }
        //            });
        //            $scope.receiptgrid = search_list;
        //        }
        //        if ($scope.search123 == "4") {
        //            
        //            angular.forEach(list_s, function (itm_s) {
        //                var string1 = $filter('date')(itm_s.fyP_Date, 'dd-MM-yyyy');

        //                if (string1.includes(substring)) {
        //                    search_list.push(itm_s);
        //                }
        //            });
        //            $scope.receiptgrid = search_list;
        //        }
        //        if ($scope.search123 == "5") {
        //            
        //            angular.forEach(list_s, function (itm_s) {
        //                var string1 = itm_s.fyP_Tot_Amount.toString().toLowerCase();


        //                if (string1.includes(substring)) {
        //                    search_list.push(itm_s);
        //                }
        //            });
        //            $scope.receiptgrid = search_list;
        //        }
        //        if ($scope.search123 == "6") {
        //            
        //            angular.forEach(list_s, function (itm_s) {
        //                var string1 = itm_s.fyP_Bank_Or_Cash.toLowerCase();

        //                if (string1.includes(substring)) {
        //                    search_list.push(itm_s);
        //                }
        //            });
        //            $scope.receiptgrid = search_list;
        //        }

        //    }
        //}


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
                    // var sub = Number($scope.searchtext);
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

            // var entereddata = $scope.search;
            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
                if ($scope.search123 == "5") {
                    // var sub = Number($scope.searchtext);
                    //$scope.txt = false;
                    //$scope.numbr = true;
                    //$scope.dat = false;
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                        // "searchtext": substring,
                    }
                }
                else if ($scope.search123 == "4") {

                    //$scope.txt = false;
                    //$scope.numbr = false;
                    //$scope.dat = true;
                    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123,
                        "searchdate": date,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                        // "searchtext": substring,
                    }
                }
                else {
                    //$scope.txt = true;
                    //$scope.numbr = false;
                    //$scope.dat = false;

                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                        // "searchtext": substring,
                    }

                }


                // var substring = $scope.searchtext.toLowerCase();
                //var data = {
                //    "searchType": $scope.search123,
                //    "searchtext": $scope.searchtext,
                //   // "searchtext": substring,
                //}

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }



                apiService.create("FeeStudentTransaction/searching", data).
                    then(function (promise) {
                        $scope.receiptgrid = promise.searcharray;
                        $scope.totcountsearch = promise.searcharray.length;

                        if (promise.searcharray == null || promise.searcharray == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }
                        //swal("searched Successfully");
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }

        }


        $scope.clearsearch = function () {

            //$scope.search123 = "";
            //$scope.search_flag = false;
            //$scope.searchtxt = "";
            //$scope.searchnumbr = "";
            //$scope.searchdat = "";
            //$scope.totcountsearch = 0;

            $state.reload();

        }


        $scope.alltermchk = false;

        $scope.updateshowlabel = false;
        $scope.showstudentname = true;

        $scope.diablemodeofpayment = false;

        $scope.edittransaction = function (fypid, amstid, fyP_Bank_Or_Cash) {
            $scope.routedetails = [];
            $scope.FYP_Id = fypid;
            $scope.Amst_Id = amstid;
            $scope.fyP_Bank_Or_Cash = fyP_Bank_Or_Cash;
            var data = {
                "FYP_Id": fypid,
                "Amst_Id": amstid,
                "ASMAY_ID": $scope.cfg.ASMAY_Id,
                "modeofpayment": $scope.fyP_Bank_Or_Cash
            }

            apiService.create("FeeStudentTransaction/edittransaction", data).
                then(function (promise) {
                    if (promise.bankname.length > 0) {
                        $scope.bankmaster = promise.bankname;
                    }

                    if (promise.showstaticticsdetails.length > 0) {
                        $scope.mtotalfee = promise.showstaticticsdetails[0].fsS_CurrentYrCharges;
                        $scope.mtotcon = promise.showstaticticsdetails[0].fsS_ConcessionAmount;
                        $scope.mtotwaived = promise.showstaticticsdetails[0].fsS_WaivedAmount;
                        $scope.mtotbalance = promise.showstaticticsdetails[0].fsS_ToBePaid;
                    }
                    if (promise.narrationlist != null) {
                        if (promise.narrationlist.length > 0) {
                            $scope.narrationlist = promise.narrationlist;
                        }
                    }


                    $scope.disableacademic = true;
                    $scope.alltermchk = true;

                    $scope.allcheck = true;
                    $scope.grigview1 = true;

                    $scope.updateshowlabel = true;
                    $scope.showstudentname = false;

                    $scope.diablemodeofpayment = true;

                    $scope.isSelected = true;

                    $scope.totalgrid = promise.receiparraydeleteall;

                    //MB for Special

                    $scope.temp_Head_Instl_list = [];
                    angular.forEach($scope.totalgrid, function (uy) {
                        uy.Head_Flag = 'H';
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
                                        if (op_m.fmH_Id == op2.fmH_ID && op_m.ftI_Id == ins.ftI_Id) {
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
                        //$scope.totalgrid = $scope.temp_Head_Instl_list;
                        //angular.forEach($scope.temp_Head_Instl_list, function (r) {
                        //    r.Head_Flag = 'H';
                        //})
                        angular.forEach(ins_spe_list, function (a1) {

                            angular.forEach(a1.sp_list, function (a2) {
                                var fsS_CurrentYrCharges = 0;
                                var fsS_TotalToBePaid = 0;
                                var fsS_ConcessionAmount = 0;
                                var fsS_FineAmount = 0;
                                var fsS_ToBePaid = 0;
                                var fsS_TotalToBePaidaddfine = 0;
                                var fmG_Id = 0;
                                var fmG_GroupName = '';
                                var not_cnt = 0;
                                var totamtt = 0;
                                var fsS_OBArrearAmount = 0;
                                angular.forEach(a2.sp_ind_list, function (a3) {
                                    if (fmG_Id == 0) {
                                        fmG_Id = a3.fmG_Id;
                                        fmG_GroupName = a3.fmG_GroupName;
                                    }
                                    else {
                                        if (fmG_Id != a3.fmG_Id) {
                                            not_cnt += 1;
                                        }
                                    }

                                    fsS_CurrentYrCharges += a3.fsS_CurrentYrCharges;
                                    fsS_TotalToBePaid += a3.fsS_TotalToBePaid;
                                    fsS_ConcessionAmount += a3.fsS_ConcessionAmount;
                                    fsS_FineAmount += a3.fsS_FineAmount;
                                    fsS_ToBePaid += a3.fsS_ToBePaid;
                                    fsS_TotalToBePaidaddfine += 0;
                                    fsS_OBArrearAmount += 0;
                                })
                                if (not_cnt == 0) {
                                    $scope.temp_Head_Instl_list.push({ fmG_Id: fmG_Id, fmG_GroupName: 'SH_' + fmG_GroupName, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, totalamount: fsS_CurrentYrCharges, fsS_TotalToBePaid: fsS_TotalToBePaid, fsS_ConcessionAmount: fsS_ConcessionAmount, fsS_FineAmount: fsS_FineAmount, fsS_ToBePaid: fsS_ToBePaid, fsS_TotalToBePaidaddfine: fsS_TotalToBePaidaddfine, fsS_OBArrearAmount: fsS_OBArrearAmount, Head_Flag: 'SH' });
                                }
                                else if (not_cnt > 0) {
                                    $scope.temp_Head_Instl_list.push({ fmG_Id: 0, fmG_GroupName: 'Special_Head', fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, totalamount: fsS_CurrentYrCharges, fsS_TotalToBePaid: fsS_TotalToBePaid, fsS_ConcessionAmount: fsS_ConcessionAmount, fsS_FineAmount: fsS_FineAmount, fsS_ToBePaid: fsS_ToBePaid, fsS_TotalToBePaidaddfine: fsS_TotalToBePaidaddfine, fsS_OBArrearAmount: fsS_OBArrearAmount, Head_Flag: 'SH' });
                                }

                            })
                        })
                        $scope.totalgrid = $scope.temp_Head_Instl_list;
                    }
                    //MB for Special

                    for (var s = 0; s < $scope.totalgrid.length; s++) {
                        $scope.totalgrid[s].isSelected = true;
                        $scope.totalgrid[s].fmH_Flag = '';
                    }

                    //changed radha

                    $scope.filltermsaved = [];
                    $scope.groupcount = promise.fillmastergroup;

                    for (var g = 0; g < $scope.groupcount.length; g++) {
                        for (var h = 0; h < promise.disableterms.length; h++) {
                            if ($scope.groupcount[g].fmG_Id == promise.disableterms[h].fmG_Id)
                                $scope.filltermsaved.push($scope.groupcount[g]);
                        }
                    }

                    $scope.groupcount = $scope.filltermsaved;

                    //changed radha

                    for (var r = 0; r < $scope.groupcount.length; r++) {
                        $scope.groupcount[r].disablepaisterms = true;
                    }

                    for (var p = 0; p < promise.disableterms.length; p++) {
                        for (var q = 0; q < $scope.groupcount.length; q++) {
                            if ($scope.groupcount[q].fmG_Id == promise.disableterms[p].fmG_Id) {
                                $scope.groupcount[q].selected = true;
                            }
                        }
                    }

                    $scope.studentlst = promise.receiparraydelete
                    $scope.amsT_FirstName = promise.receiparraydelete[0].amsT_FirstName + ' ' + promise.receiparraydelete[0].amsT_MiddleName + ' ' +
                        promise.receiparraydelete[0].amsT_LastName

                    $scope.updateshowlabel = true;
                    $scope.updatename = $scope.amsT_FirstName

                    $scope.studidd = promise.receiparraydelete[0].amst_Id

                    if (promise.filusername.length > 0 && promise.filusername != null) {
                        $scope.portalusername = promise.filusername[0].portalusername;
                    }

                    //$scope.amsT_MiddleName = promise.receiparraydelete[0].amsT_MiddleName;
                    // $scope.amsT_LastName = promise.receiparraydelete[0].amsT_LastName

                    $scope.amsT_AdmNo = promise.receiparraydelete[0].amsT_AdmNo
                    $scope.amsT_RegistrationNo = promise.receiparraydelete[0].amsT_RegistrationNo
                    $scope.amaY_RollNo = promise.receiparraydelete[0].rollno;

                    $scope.classname = promise.receiparraydelete[0].classname
                    $scope.sectionname = promise.receiparraydelete[0].sectionname;

                    $scope.amsT_mobile = promise.receiparraydelete[0].amst_mobile;

                    $scope.fathername = promise.receiparraydelete[0].fathername;
                    $scope.studentdob = promise.receiparraydelete[0].studentdob;

                    $scope.FYP_Date = new Date(promise.receiparraydelete[0].fyP_Date);

                    if (promise.filusername[0].amsT_Photoname != null) {
                        studentphtoto = promise.filusername[0].amsT_Photoname;
                    }
                    else {
                        studentphtoto = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";
                    }
                    $('#blah').attr('src', studentphtoto);

                    $scope.fypdatedisabled = true;

                    angular.forEach(promise.fetchmodeofpayment, function (a3) {
                        if (a3.fyppM_TransactionTypeFlag === 'C') {
                            a3.ivrmmoD_ModeOfPayment = "Cash";
                            a3.Bank_Amount = a3.bank_Amount;
                            a3.amountdesiable = true;
                        }
                        else if (a3.fyppM_TransactionTypeFlag === 'B') {
                            a3.ivrmmoD_ModeOfPayment = "Bank";
                            a3.Bank_Date = new Date(a3.bank_Date);
                            a3.Bank_No = a3.bank_No;
                            a3.Bank_Name = a3.bank_Name;
                            a3.Bank_Amount = a3.bank_Amount;
                            a3.ivrmmoD_Flag = "B";
                            a3.amountdesiable = true;
                            a3.dddatedisable = true;

                        }
                        else if (a3.fyppM_TransactionTypeFlag === 'R') {
                            a3.ivrmmoD_ModeOfPayment = "RTGS/NEFT";

                            a3.Bank_Date = new Date(a3.bank_Date);
                            a3.Bank_No = a3.bank_No;
                            a3.Bank_Name = a3.bank_Name;
                            a3.Bank_Amount = a3.bank_Amount;
                            a3.ivrmmoD_Flag = "B";
                            a3.amountdesiable = true;
                            a3.dddatedisable = true;

                        }
                        else if (a3.fyppM_TransactionTypeFlag === 'S') {
                            a3.ivrmmoD_ModeOfPayment = "Card";

                            $scope.Bank_Date = new Date(a3.bank_Date);
                            $scope.Bank_No = a3.bank_No;
                            $scope.Bank_Name = a3.bank_Name;
                            $scope.Bank_Amount = a3.bank_Amount;
                            a3.ivrmmoD_Flag = "B";
                            a3.amountdesiable = true;
                            a3.dddatedisable = true;
                        }
                    });

                    $scope.paymodeee = promise.fetchmodeofpayment;
                    //$scope.FYP_PayModeType = promise.receiparraydelete[0].fyP_PayModeType;
                    $scope.FYP_PayModeType = promise.receiparraydelete[0].fyP_DeviceFlag;
                    $scope.disablwmodes = true;
                    $scope.amount_check = false;
                    $scope.disableamtchk = true;

                    //MULTIMODE

                    $scope.FYP_Remarks = promise.receiparraydelete[0].fyP_Remarks

                    var totalconcesionammttt = 0;
                    angular.forEach($scope.totalgrid, function (conamt) {
                        totalconcesionammttt = Number(totalconcesionammttt) + conamt.fsS_ConcessionAmount
                    })

                    $scope.totalconcession = totalconcesionammttt

                    $scope.totalfee = promise.receiparraydelete[0].fyP_Tot_Amount
                    $scope.curramount = promise.receiparraydelete[0].fyP_Tot_Amount

                    if (autoreceipt == 1) {
                        $scope.showreceiptno = false;
                    }
                    else {
                        $scope.showreceiptno = true;
                        $scope.FYP_Receipt_No = promise.receiparraydelete[0].fyP_Receipt_No
                           $scope.PayingAmount = promise.receiparraydelete[0].fyP_Tot_Amount
                    }
                }
                )
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
                // For Academic From Date
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

            $scope.ASMCL_ClassName = "";
            $scope.ASMC_SectionName = "";
            $scope.AMAY_RollNo = "";
            $scope.AMST_AdmNo = "";
            $scope.AMST_FirstName = "";
            //$scope.curdate = new Date().getDate() + '/' + new Date().getMonth() + '/' + new Date().getFullYear();
            $scope.curdate = "";
            $scope.asmaY_Year = "";
            $scope.AMST_FatherName = "";
            $scope.AMST_MotherName = "";
            $scope.receiptno = "";
            $scope.FMCC_ConcessionName = "";

            var data = {
                "AMST_ID": studid,
                "filterinitialdata": $scope.filterdata,
                "configset": grouporterm,
                "FYP_Id": fypid,
                "minstall": mergeinstallment,
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeStudentTransaction/printreceiptnew", data).
                then(function (promise) {


                    $scope.MI_Address1 = promise.masterinstitution[0].mI_Address1;
                    $scope.MI_Address2 = promise.masterinstitution[0].mI_Address2;
                    $scope.MI_Address3 = promise.masterinstitution[0].mI_Address3;
                    $scope.MI_Pincode = promise.masterinstitution[0].mI_Pincode;
                    $scope.pendingamount = promise.pendingamount;
                    $scope.receiptno = promise.fillstudentviewdetails[0].fyP_Receipt_No;
                    $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");
                    $scope.AMST_AdmNo = promise.fillstudentviewdetails[0].admno;
                    $scope.asmaY_Year = promise.asmaY_Year;

                    if (promise.fillstudentviewdetails[0].amsT_FirstName != null && promise.fillstudentviewdetails[0].amsT_MiddleName != null && promise.fillstudentviewdetails[0].amsT_LastName != null) {
                        $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                    }
                    else {
                        if (promise.fillstudentviewdetails[0].amsT_FirstName == null) {
                            promise.fillstudentviewdetails[0].amsT_FirstName = ' ';
                        }
                        if (promise.fillstudentviewdetails[0].amsT_MiddleName == null) {
                            promise.fillstudentviewdetails[0].amsT_MiddleName = ' ';
                        }
                        if (promise.fillstudentviewdetails[0].amsT_LastName == null) {
                            promise.fillstudentviewdetails[0].amsT_LastName = ' ';
                        }
                        $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                    }

                    $scope.ASMCL_ClassName = promise.fillstudentviewdetails[0].classname;
                    $scope.ASMC_SectionName = promise.fillstudentviewdetails[0].sectionname;
                    $scope.AMST_FatherName = promise.fillstudentviewdetails[0].fathername;
                    $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;
                    $scope.AMST_MotherName = promise.fillstudentviewdetails[0].mothername;
                    $scope.period = promise.duration;
                    if ($scope.FMC_RInstallmentsMergeFlag == 1) {
                        $scope.FMC_RInstallmentsFlag = 0;

                        $scope.showdetailsreceipt = promise.filltotaldetails;
                    }
                    else {
                        $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                    }

                    //$scope.showtotaldetails = promise.filltotaldetails;

                    if (promise.fillstudentviewdetails.length > 0) {
                        var fmatotal = 0;
                        var totalpaidamount = 0;
                        angular.forEach(promise.fillstudentviewdetails, function (user) {
                            fmatotal = fmatotal + user.totalcharges;
                            totalpaidamount = totalpaidamount + user.ftP_Paid_Amt;
                        })
                    }
                    $scope.totalfma = fmatotal;
                    if (fmatotal > totalpaidamount)
                        $scope.btotA = fmatotal - totalpaidamount;
                    else
                        $scope.btotA = 0;

                    $scope.atotA = promise.currpaymentdetails[0].ftP_Paid_Amt;
                    $scope.ctotA = promise.currpaymentdetails[0].ftP_Concession_Amt;
                    $scope.ftotA = promise.currpaymentdetails[0].ftP_Fine_Amt;
                    $scope.wtotA = promise.currpaymentdetails[0].ftP_Waived_Amt;
                    $scope.paymenrgrid = promise.currpaymentdetails;
                    $scope.words = $scope.amountinwords($scope.atotA);
                    $scope.narration = promise.currpaymentdetails[0].fyP_Remarks;
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
                        //$scope.FYP_Bank_Or_Cash = "Cash";
                        $scope.FYP_Bank_Name = "--";
                        $scope.FYP_DD_Cheque_No = "--";
                        $scope.FYP_Date = promise.fillstudentviewdetails[0].FYP_Date;

                    }
                    else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "O") {
                        //$scope.FYP_Bank_Or_Cash = "Online Payment";
                        $scope.FYP_Bank_Name = "--";
                        $scope.FYP_DD_Cheque_No = "--";
                        $scope.FYP_Date = promise.fillstudentviewdetails[0].FYP_Date;

                    }
                    else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "B") {
                        //$scope.FYP_Bank_Or_Cash = "Bank";
                        $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                        $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                        //  $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                        $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");
                    }
                    $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");
                    if (promise.fillstudentviewdetails[0].fyP_Remarks != null || promise.fillstudentviewdetails[0].fyP_Remarks != "") {
                        $scope.feeremarks = promise.fillstudentviewdetails[0].fyP_Remarks;
                        //$scope.feeremarks = termname;
                    }
                    else {
                        $scope.feeremarks = "Remarks Not Given";
                    }
                    $scope.AMAY_RollNo = promise.fillstudentviewdetails[0].rollno;
                    $scope.curdate = $filter('date')(new Date(), "dd-MM-yyyy");
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
            $scope.Amst_Id = {};
            $scope.groupcount = [];

            $scope.amsT_FirstName = "";
            $scope.amsT_MiddleName = "";
            $scope.amsT_LastName = "";
            $scope.amsT_AdmNo = "";
            $scope.amsT_RegistrationNo = "";
            $scope.amaY_RollNo = "";
            $scope.amsT_mobile = "";
            $scope.classname = "";
            $scope.sectionname = "";
            $scope.fathername = "";
            $scope.studentdob = "";

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

        var temp_amst_Id = "";
        var temp_fillmastergroup = [];
        var temp_alldata = [];
        var temp_FYP_Id = "";
        $scope.Search_Chaln_No = function () {


            var data = {
                "configset": grouporterm,
                "FYP_ChallanNo": $scope.FYP_ChallanNo,
                "FYP_Date": new Date($scope.FYP_Date).toDateString(),
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeStudentTransaction/Search_Chaln_No", data).
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
                            // $scope.temp_Fine_Amounts = promise.fine_FMA_Ids;
                            var objj = {};
                            //objj.search = promise.amsT_AdmNo;
                            // var radioobj = 'AdmNo';
                            objj.search = promise.amsT_FirstName;
                            var radioobj = 'regular';
                            //var radioobj = 'NameAdmno';
                            temp_amst_Id = promise.amst_Id;
                            temp_FYP_Id = promise.fyP_Id;
                            $scope.searchfilter(objj, radioobj);

                            //$scope.Amst_Id.amst_Id = promise.amst_Id;
                            //angular.forEach($scope.studentlst, function (ast) {
                            //    if(ast.amst_Id== promise.amst_Id)
                            //    {
                            //        ast.Selected = true;
                            //    }
                            //})
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
        //    angular.forEach($scope.totalgrid, function (nt) {
        //        if (nt.isSelected) {
        //            angular.forEach($scope.temp_Fine_Amounts, function (fn) {
        //                if (fn.fmA_Id == nt.fmA_Id) {
        //                    fine_total += fn.fine_Amt;

        //                }
        //            })
        //        }

        //    })
        //    angular.forEach($scope.totalgrid, function (nt) {
        //        if (nt.fmH_Flag == 'F') {


        //            nt.fsS_ToBePaid = fine_total + $scope.temp_finehead_amt;
        //            //nt.fsS_TotalToBePaidaddfine = fine_total + $scope.temp_finehead_amt_total;
        //            // nt.fsS_TotalToBePaid = nt.fsS_TotalToBePaidaddfine;
        //            //if ($scope.curramount > 0 && nt.isSelected != undefined) {
        //            //    $scope.curramount -= Number(nt.fsS_ToBePaid);
        //            //}

        //            if (nt.isSelected == false || nt.isSelected == undefined) {
        //                if (fine_total > 0) {
        //                    nt.isSelected = true;
        //                }

        //            }
        //            if (nt.isSelected == true) {
        //                if (fine_total == 0) {
        //                    nt.isSelected = false;
        //                }
        //            }



        //            if (nt.isSelected == true) {

        //                $scope.totalfine = nt.fsS_ToBePaid;
        //                $scope.curramount += Number(nt.fsS_ToBePaid);
        //            }
        //            else if (nt.isSelected == false) {
        //                if (nt.fsS_ToBePaid != 0) {
        //                    $scope.curramount -= Number(nt.fsS_ToBePaid);
        //                }
        //                else if ($scope.totalfine != 0) {
        //                    $scope.curramount -= Number($scope.totalfine);

        //                }


        //                $scope.totalfine = 0;
        //            }


        //        }
        //    })



        //}



        $scope.calculate_fine = function () {
            var fineheadcount = 0;
            var fine_total = 0;

            angular.forEach($scope.totalgrid, function (nt) {
                if (nt.fmH_Flag == 'F') {
                    fineheadcount += 1;

                }

            })

            if (fineheadcount > 1) {
            
                angular.forEach($scope.temp_Fine_Amounts, function (fn) {
                    var finetotalnew = 0;
                    var fmtid = 0;
                    if (fn.fine_Amt > 0) {
                        angular.forEach($scope.totalgrid, function (nt) {

                            if (nt.isSelected) {


                                if (fn.fmA_Id == nt.fmA_Id) {
                                    finetotalnew += fn.fine_Amt;
                                    fmtid = nt.fmT_Id;

                                }


                            }

                            if (nt.fmH_Flag == 'F' && nt.fmT_Id == fmtid) {
                                nt.fsS_ToBePaid = finetotalnew;
                                $scope.totalfine += finetotalnew
                                $scope.curramount += Number(nt.fsS_ToBePaid);

                            }


                        })
                    }
            })
            }
            else {


                angular.forEach($scope.totalgrid, function (nt) {
                if (nt.isSelected) {
                    angular.forEach($scope.temp_Fine_Amounts, function (fn) {
                        if (fn.fmA_Id == nt.fmA_Id) {
                            fine_total += fn.fine_Amt;

                        }
                    })
                }

            })
            angular.forEach($scope.totalgrid, function (nt) {
                if (nt.fmH_Flag == 'F') {


                    nt.fsS_ToBePaid = fine_total + $scope.temp_finehead_amt;
                    
                    if (nt.isSelected == false || nt.isSelected == undefined) {
                        if (fine_total > 0) {
                            nt.isSelected = true;
                        }

                    }
                    if (nt.isSelected == true) {
                        if (fine_total == 0) {
                            nt.isSelected = false;
                        }
                    }



                    if (nt.isSelected == true) {

                        $scope.totalfine = nt.fsS_ToBePaid;
                        $scope.curramount += Number(nt.fsS_ToBePaid);
                    }
                    else if (nt.isSelected == false) {
                        if (nt.fsS_ToBePaid != 0) {
                            $scope.curramount -= Number(nt.fsS_ToBePaid);
                        }
                        else if ($scope.totalfine != 0) {
                            $scope.curramount -= Number($scope.totalfine);

                        }


                        $scope.totalfine = 0;
                    }


                }
            })

            }
            
         
          



        }




        $scope.printauto = function (fypid, studentid) {

            var fid = fypid;
            //var stid = studentid.amst_Id
            var stid = studentid.amst_Id

            var stid1 = studentid;

            var mgs = "Print";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            swal({
                title: "Record Saved Successfully",
                text: "Do You Want To " + mgs + " Receipt?",
                type: "success",
                //icon: "success",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {

                        if (stid != undefined) {
                            $scope.showmodaldetails(fid, stid)
                        }
                        else {
                            $scope.showmodaldetails(fid, stid1)
                        }

                        $("#myModal1").modal();
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                        // $scope.loaddata();
                        $state.reload();
                    }
                });
        }



        $scope.Mail = function () {
            $scope.tmplt = [];
            var Template = document.getElementById("printmodalnew1").innerHTML;
            $scope.tmplt.push({
                TemplateString: '<html><head>' + '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link href="plugins/bootstrap/css/bootstrap.css" />' +
                    '<link href="css/style.css" rel="stylesheet" />' +
                    '</head><body>' + Template + '</body></html>'
            });
            $scope.EmailSMS = "Email";

            var data =
            {
                "EmailSMS": $scope.EmailSMS,
                "Amst_Id": $scope.studid,
                Template: $scope.tmplt

            };

            apiService.create("FeeStudentTransaction/SendEmail", data).
                then(function (promise) {
                    if (promise.returnval == "success") {

                        swal("Email Sent..!!!");

                    }
                    else if (promise.returnval == "notFound") {

                        swal("Email Not sent..!!!", 'Default Email-Id is Not Found.. !!!');
                    }
                    else if (promise.returnval == "Error") {
                        swal("Something went wrong", 'Try After some time..!!');
                    } else {
                        swal("Something went wrong", 'Try After some time..!!');
                    }
                });
        };


        //MULTIMODE OF PAYMENT
        $scope.get_modes = function (FYP_PayModeType, paymodeee) {

            angular.forEach(paymodeee, function (ed) {
                if (ed.ivrmmoD_ModeOfPayment == 'Bank') {
                    ed.rdo_Bank_Multiple = 'singlee';
                    //ed.Bank_Date = new Date();
                }
                else {
                    ed.rdo_Bank_Multiple = 'null';
                }

                if (ed.ivrmmoD_ModeOfPayment == 'Bank') {
                    ed.chk_Bank_Multiple = true;
                    //ed.Bank_Date = new Date();
                }
                else {
                    ed.chk_Bank_Multiple = 'null';
                }
                ed.Bank_Date = new Date();
                //ed.chk_Bank_Multiple = false;
                //ed.rdo_Bank_Multiple = false;
            })

            //$scope.paymodeee = promise.fetchmodeofpayment;

            if (FYP_PayModeType === "Multiple") {
                angular.forEach(paymodeee, function (ed) {
                    //ed.chk_Bank_Multiple = false;
                    ed.Bank_Date = new Date();
                    ed.Bank_No = "";
                    ed.Bank_Name = "";
                    ed.Bank_Amount = "";
                })
            }
            else if (FYP_PayModeType === "Single") {
                angular.forEach(paymodeee, function (ed) {
                    //ed.chk_Bank_Multiple = false;
                    ed.Bank_Date = new Date();
                    ed.Bank_No = "";
                    ed.Bank_Name = "";
                    ed.Bank_Amount = "";
                });
            }

            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

            $scope.toggleAll($scope.all);

        };


        $scope.checkchange = function (radiovalue, radoid, abccheck) {
            angular.forEach(radiovalue, function (ed) {
                if (ed.ivrmmoD_Id == radoid) {
                    if (abccheck == true) {
                        ed.chk_Bank_Multiple = true;
                        //ed.Bank_Date = new Date();
                    }
                    else {
                        ed.chk_Bank_Multiple = 'null';
                        ed.rdo_Bank_Multiple = 'null';

                        ed.Bank_Date = "";
                        ed.Bank_No = "";
                        ed.Bank_Name = "";
                        ed.Bank_Amount = "";
                    }

                }
                //else {
                //    ed.rdo_Bank_Multiple = 'null';
                //    ed.chk_Bank_Multiple = 'null';
                //}

                //if (ed.chk_Bank_Multiple != 'multii') {
                //    ed.chk_Bank_Multiple = false;
                //}
                ed.Bank_Date = new Date();
            })
            $scope.paymodeee = radiovalue;

            $scope.toggleAll($scope.all);

        };

        $scope.radiochange = function (radiovalue, radoid) {
            angular.forEach(radiovalue, function (ed) {
                if (ed.ivrmmoD_Id == radoid) {
                    ed.rdo_Bank_Multiple = 'singlee';
                    ed.chk_Bank_Multiple = true;
                    ed.Bank_Amount = $scope.curramount;
                    //ed.Bank_Date = new Date();
                }
                else {
                    ed.rdo_Bank_Multiple = 'null';
                    ed.chk_Bank_Multiple = 'null';

                    ed.Bank_Date = "";
                    ed.Bank_No = "";
                    ed.Bank_Name = "";
                    ed.Bank_Amount = "";
                }

                ed.Bank_Date = new Date();
                //if (ed.chk_Bank_Multiple != 'multii') {
                //    ed.chk_Bank_Multiple = false;
                //}

            })
            $scope.paymodeee = radiovalue;
        };


        $scope.chk_Bank_Multiple = false;
        $scope.rdo_Bank_Multiple = false;
        $scope.chk_amount = function (type, amount_pay, paymodeee, Bank_Multiple) {

            if (Bank_Multiple === true) {
                angular.forEach(paymodeee, function (ed) {
                    if (ed.chk_Bank_Multiple === true) {
                        $scope.enabletext = true;
                    }
                    else {
                        $scope.enabletext = false;
                    }
                })
            }

            if (Bank_Multiple === "singlee") {
                angular.forEach(paymodeee, function (ed) {
                    if (ed.rdo_Bank_Multiple === "singlee") {
                        $scope.enabletext = true;
                    }
                    else {
                        $scope.enabletext = false;
                    }
                })
            }

            if (amount_pay !== "" && amount_pay !== null && amount_pay !== undefined) {
                if (Number(amount_pay) === 0) {
                    swal("Amount Is Must Be Greater Than Zero");
                    if (type === 'C' || type === 'B') {
                        $scope.Bank_Amount = "";
                    }
                }
                else if (Number(amount_pay) > 0) {
                    var amount = 0;

                    angular.forEach(paymodeee, function (ed) {
                        if (ed.chk_Bank_Multiple === true) {
                            amount += Number(ed.Bank_Amount);
                        }
                        if (Number($scope.curramount) < Number(amount)) {

                            swal("Sum Of Amounts Must Match Now Paying Amount");
                            ed.Bank_Amount = "";

                        }
                    })
                }

            }
        };


        $scope.reverseall = function (checkedor) {
            var termsel = 0;
            angular.forEach($scope.groupcount, function (ed) {
                if (ed.selected == true) {
                    termsel = Number(termsel) + 1;
                }
            })

            if (termsel >= 1) {
                if (checkedor === false) {
                    $scope.PayingAmount = "";
                    $scope.onselectstudent($scope.Amst_Id);
                }
                else if (checkedor === true) {
                    angular.forEach($scope.totalgrid, function (ed) {
                        ed.isSelected = false;
                    })
                    $scope.all = false;
                }

                angular.forEach($scope.groupcount, function (ed) {
                    ed.disablepaisterms = true;
                })

                $scope.alltermchk = true;
            }
            else {
                swal("Kindly select atleast any one term");
                $scope.amount_check = false;
            }
        };

        $scope.total_heads_amount = 0;
        $scope.Distribute_Amount = function (amt) {
            if (Number(amt) > 0) {
                // if ($scope.total_heads_amount === 0) {
                $scope.total_heads_amount = 0;
                angular.forEach($scope.totalgrid, function (hed) {
                    $scope.total_heads_amount += Number(hed.fsS_ToBePaid);
                })
                //  }
                if (Number(amt) > $scope.total_heads_amount) {
                    $scope.all = false;
                    swal("Entered Amount Greater Than Of Total" + $scope.total_heads_amount);
                    $scope.PayingAmount = null;
                    angular.forEach($scope.totalgrid, function (hed) {
                        hed.isSelected = false;
                        hed.fsS_ToBePaid = hed.fsS_ToBePaid;
                    })
                }
                else {
                    angular.forEach($scope.totalgrid, function (hed) {
                        hed.isSelected = false;
                        hed.fsS_ToBePaid = hed.fsS_ToBePaid;
                    })
                    var keepGoing1 = true;
                    angular.forEach($scope.totalgrid, function (hd_ins) {
                        if (hd_ins.Head_Flag === 'SH') {
                            if (keepGoing1) {
                                if (Number(amt) >= Number(hd_ins.fsS_ToBePaid)) {
                                    hd_ins.isSelected = true;
                                    hd_ins.fsS_ToBePaid = Number(hd_ins.fsS_ToBePaid);
                                    amt = (Number(amt) - Number(hd_ins.fsS_ToBePaid));
                                }
                                else if (Number(amt) < Number(hd_ins.fsS_ToBePaid)) {
                                    hd_ins.fsS_ToBePaid = Number(amt);
                                    hd_ins.isSelected = true;
                                    amt = (Number(amt) - Number(hd_ins.fsS_ToBePaid));
                                }
                                if (Number(amt) === 0) {
                                    keepGoing1 = false;
                                }
                            }
                        }
                    })
                    if (keepGoing1) {
                        angular.forEach($scope.temp_Head_Instl_list, function (instal) {
                            angular.forEach($scope.groupcount, function (grp) {
                                if (grp.selected) {
                                    angular.forEach($scope.totalgrid, function (hd_ins) {
                                        if (hd_ins.ftI_Id === grp.fmG_Id && hd_ins.ftI_Id === instal.ftI_Id) {
                                            if (hd_ins.Head_Flag === 'H') {
                                                if (keepGoing1) {
                                                    if (Number(amt) >= Number(hd_ins.fsS_ToBePaid)) {
                                                        hd_ins.isSelected = true;
                                                        hd_ins.fsS_ToBePaid = Number(hd_ins.fsS_ToBePaid);
                                                        amt = (Number(amt) - Number(hd_ins.fsS_ToBePaid));
                                                    }
                                                    else if (Number(amt) < Number(hd_ins.fsS_ToBePaid)) {
                                                        hd_ins.fsS_ToBePaid = Number(amt);
                                                        hd_ins.isSelected = true;
                                                        amt = (Number(amt) - Number(hd_ins.fsS_ToBePaid));
                                                    }
                                                    if (Number(amt) === 0) {
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
            else {
                swal("Entered Amount Should be greater than 0");
                angular.forEach($scope.totalgrid, function (hed) {
                    hed.isSelected = false;
                })

                $scope.curramount = 0;
                angular.forEach($scope.paymodeee, function (ed) {
                    ed.Bank_Amount = "0";
                })

            }
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
            apiService.create("FeeStudentTransaction/viewstatus", data).
                then(function (promise) {

                    if (promise.translogresults.length > 0) {
                        $scope.responsestatuslogs = promise.translogresults[0].responsestatuslogs;
                        $scope.fyp_transaction_Id = promise.translogresults[0].order_id;
                        $scope.fyP_PaymentReference_Id = promise.translogresults[0].payment_id;

                        
                        if (promise.razorpaytransactionlogs.length > 0) {
                            if (promise.razorpaytransactionlogs.FMOT_PayGatewayType == "RAZORPAY") {
                                $scope.fyppsT_Settlement_Date = promise.razorpaytransactionlogs[0].fyppsT_Settlement_Date;
                            }
                            
                            
                        }
                        else {
                            $scope.fyppsT_Settlement_Date = promise.translogresults[0].fyP_Date;
                        }
                        
                    }
                    
                })
        }

        $scope.viewpaydetails = function (fypid) {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeStudentTransaction/viewpaydetails", data).
                then(function (promise) {

                    if (promise.getgroupwisepaymentdetails.length > 0) {
                        $scope.paydetails = promise.getgroupwisepaymentdetails;

                        var t_FSS_CurrentYrCharges = 0;
                        var t_FSS_PaidAmount = 0;
                        var t_FSS_ToBePaid = 0;
                        var t_FSS_ConcessionAmount = 0;
                        var t_FSS_WaivedAmount = 0;
                        var t_FSS_RunningExcessAmount = 0;
                        angular.forEach($scope.paydetails, function (gp) {
                            t_FSS_CurrentYrCharges += gp.FSS_CurrentYrCharges;
                            t_FSS_PaidAmount += gp.FSS_PaidAmount;
                            t_FSS_ToBePaid += gp.FSS_ToBePaid;
                            t_FSS_ConcessionAmount += gp.FSS_ConcessionAmount;
                            t_FSS_WaivedAmount += gp.FSS_WaivedAmount;
                            t_FSS_RunningExcessAmount += gp.FSS_RunningExcessAmount;
                        })

                        $scope.t_FSS_CurrentYrCharges = t_FSS_CurrentYrCharges;
                        $scope.t_FSS_PaidAmount = t_FSS_PaidAmount;
                        $scope.t_FSS_ToBePaid = t_FSS_ToBePaid;
                        $scope.t_FSS_ConcessionAmount = t_FSS_ConcessionAmount;
                        $scope.t_FSS_WaivedAmount = t_FSS_WaivedAmount;
                        $scope.t_FSS_RunningExcessAmount = t_FSS_RunningExcessAmount;

                    }
                })
        }


        $scope.viewpayexcessdetails = function (fypid) {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeStudentTransaction/viewpayexcessdetails", data).
                then(function (promise) {
                    $scope.payexcessdetails = promise.getexcesspaidstudents;

                    var t_FSS_RunningExcessAmount = 0;
                    angular.forEach($scope.payexcessdetails, function (gp) {
                        t_FSS_RunningExcessAmount += gp.FSS_RunningExcessAmount;
                    })

                    $scope.t_FSS_RunningExcessAmount = t_FSS_RunningExcessAmount;

                })
        }

        // View Student Profile

        $scope.ViewStudentProfile = function () {
            $scope.myTabIndex2 = 0;
            $scope.year_name = "";
            $scope.viewstudentexamsubjectdetails = [];
            $scope.viewstudentwiseexamdetails = [];
            $scope.viewstudentattendancetails = [];
            $scope.viewstudentsubjectdetails = [];
            $scope.viewstudentfeedetails = [];
            $scope.studentdivlist = [];

            var data = {
                "AMST_Id": $scope.Amst_Id.amst_Id,
                //"student_staffflag": 'admin'
            };

            apiService.create("StudentDashboard/ViewStudentProfile", data).then(function (promise) {

                if (promise !== null) {
                    $scope.viewstudentjoineddetails = promise.viewstudentjoineddetails;

                    if ($scope.viewstudentjoineddetails !== null && $scope.viewstudentjoineddetails.length > 0) {
                        $scope.studentname_view = $scope.viewstudentjoineddetails[0].studentname;
                        $scope.amstadmno_view = $scope.viewstudentjoineddetails[0].amsT_AdmNo;
                        $scope.amstregno_view = $scope.viewstudentjoineddetails[0].amsT_RegistrationNo;
                        $scope.year_view = $scope.viewstudentjoineddetails[0].asmaY_Year;
                        $scope.class_view = $scope.viewstudentjoineddetails[0].asmcL_ClassName;
                        $scope.photo_view = $scope.viewstudentjoineddetails[0].amsT_Photoname;
                        $scope.gender_view = $scope.viewstudentjoineddetails[0].amsT_Sex;
                        $scope.status_view = $scope.viewstudentjoineddetails[0].amsT_SOL;
                        $scope.doa_view = new Date($scope.viewstudentjoineddetails[0].amsT_Date);
                        $scope.dob_view = new Date($scope.viewstudentjoineddetails[0].amsT_DOB);

                        $scope.viewstudentdetails = promise.viewstudentdetails;

                        if ($scope.viewstudentdetails !== null && $scope.viewstudentdetails.length > 0) {
                            $scope.mobile_view = $scope.viewstudentdetails[0].amsT_MobileNo;
                            $scope.email_view = $scope.viewstudentdetails[0].amsT_emailId;
                            $scope.stutpin = $scope.viewstudentdetails[0].amsT_Tpin;
                            //Father Details
                            $scope.FatherName = $scope.viewstudentdetails[0].amsT_FatherName;
                            $scope.FatherSurName = $scope.viewstudentdetails[0].amsT_FatherSurname === null
                                || $scope.viewstudentdetails[0].amsT_FatherSurname === "" ? "" : $scope.viewstudentdetails[0].amsT_FatherSurname;
                            $scope.Father_MobileNo = $scope.viewstudentdetails[0].amsT_FatherMobleNo;
                            $scope.Father_EmailId = $scope.viewstudentdetails[0].amsT_FatheremailId;
                            $scope.Father_photo = $scope.viewstudentdetails[0].ansT_FatherPhoto;


                            //Mother Details
                            $scope.MotherName = $scope.viewstudentdetails[0].amsT_MotherName;
                            $scope.MotherNamesurname = $scope.viewstudentdetails[0].amsT_MotherSurname === null || $scope.viewstudentdetails[0].amsT_MotherSurname === "" || $scope.viewstudentdetails[0].amsT_MotherSurname === "0" ? "" : ' ' + $scope.viewstudentdetails[0].amsT_MotherSurname;
                            $scope.MotherName = $scope.MotherName + ' ' + $scope.MotherNamesurname;
                            $scope.Mother_MobileNo = $scope.viewstudentdetails[0].amsT_MotherMobileNo;
                            $scope.Mother_EmailId = $scope.viewstudentdetails[0].amsT_MotherEmailId;
                            $scope.Mother_photo = $scope.viewstudentdetails[0].ansT_MotherPhoto;

                        }

                        if (promise.viewstudentacademicyeardetails !== null && promise.viewstudentacademicyeardetails.length > 0) {
                            $scope.viewstudentacademicyeardetails = promise.viewstudentacademicyeardetails;
                        }

                        if (promise.viewstudentguardiandetails !== null && promise.viewstudentguardiandetails.length > 0) {
                            $scope.viewstudentguardiandetails = promise.viewstudentguardiandetails;
                        }

                        //Over All Attendance Details
                        $scope.att_workingdays = [];
                        $scope.att_presentdays = [];
                        $scope.att_percentage = [];
                        if (promise.viewstudentattendancetails !== null && promise.viewstudentattendancetails.length > 0) {
                            $scope.viewstudentattendancetails = promise.viewstudentattendancetails;

                            angular.forEach($scope.viewstudentattendancetails, function (d) {
                                $scope.att_workingdays.push({ label: d.ASMAY_Year + '-' + d.ASMCL_ClassName + '-' + d.ASMC_SectionName, "y": d.WORKINGDAYS });
                                $scope.att_presentdays.push({ label: d.ASMAY_Year + '-' + d.ASMCL_ClassName + '-' + d.ASMC_SectionName, "y": d.PRESENTDAYS });
                                $scope.att_percentage.push({ label: d.ASMAY_Year + '-' + d.ASMCL_ClassName + '-' + d.ASMC_SectionName, "y": d.PERCENTAGE });
                            });

                            var chart = new CanvasJS.Chart("att_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Working Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_workingdays
                                },
                                {
                                    name: "Present Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_presentdays
                                },
                                {
                                    name: "Percentage",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_percentage
                                }]
                            });
                            chart.render();
                        }

                        // Year Month Wise Attendance Details
                        $scope.viewstudentattendanceMonthdetails = [];
                        $scope.att_Month_workingdays = [];
                        $scope.att_Month_presentdays = [];
                        if (promise.viewstudentattendanceMonthdetails !== null && promise.viewstudentattendanceMonthdetails.length > 0) {
                            $scope.viewstudentattendanceMonthdetails = promise.viewstudentattendanceMonthdetails;

                            angular.forEach($scope.viewstudentattendanceMonthdetails, function (d) {
                                $scope.att_Month_workingdays.push({ label: d.Months + '-' + d.Years, "y": d.WorkingCount });
                                $scope.att_Month_presentdays.push({ label: d.Months + '-' + d.Years, "y": d.PresentCount });
                            });

                            var chart = new CanvasJS.Chart("att_Month_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Working Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_Month_workingdays
                                },
                                {
                                    name: "Present Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_Month_presentdays
                                }]
                            });
                            chart.render();
                        }


                        //Subject Details For Current Year
                        if (promise.viewstudentsubjectdetails !== null && promise.viewstudentsubjectdetails.length > 0) {
                            $scope.viewstudentsubjectdetails = promise.viewstudentsubjectdetails;
                        }

                        //Exam Details
                        if (promise.viewstudentwiseexamdetails !== null && promise.viewstudentwiseexamdetails.length > 0) {
                            $scope.viewstudentwiseexamdetails = promise.viewstudentwiseexamdetails;
                        }


                        // Fee Details
                        $scope.fee_yearlycharges = [];
                        $scope.fee_Concession = [];
                        $scope.fee_Payable = [];
                        $scope.fee_PaidAmount = [];
                        $scope.fee_Outstanding = [];
                        if (promise.viewstudentfeedetails !== null && promise.viewstudentfeedetails.length > 0) {
                            $scope.viewstudentfeedetails = promise.viewstudentfeedetails;

                            angular.forEach($scope.viewstudentfeedetails, function (d) {
                                $scope.fee_yearlycharges.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.YearlyCharges });
                                $scope.fee_Concession.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Concession });
                                $scope.fee_Payable.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Payable });
                                $scope.fee_PaidAmount.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.PaidAmount });
                                $scope.fee_Outstanding.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Outstanding });
                            });

                            var chart = new CanvasJS.Chart("fee_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Yearly Charges",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_yearlycharges
                                },
                                {
                                    name: "Concession",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_Concession
                                },
                                {
                                    name: "Payable",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_Payable
                                },
                                {
                                    name: "Paid Amount",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_PaidAmount
                                },
                                {
                                    name: "Outstanding",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_Outstanding
                                }]
                            });
                            chart.render();

                        }

                        // Fee Yearly Paid Details
                        $scope.viewstudenfeeyeardetails = [];
                        if (promise.viewstudenfeeyeardetails !== null && promise.viewstudenfeeyeardetails.length > 0) {
                            $scope.viewstudenfeeyeardetails = promise.viewstudenfeeyeardetails;
                        }

                        //Compliants list
                        if (promise.studentdivlist !== null && promise.studentdivlist.length > 0) {
                            $scope.studentdivlist = promise.studentdivlist;

                            angular.forEach($scope.studentdivlist, function (dd) {
                                if (dd.ASCOMP_FilePath !== null && dd.ASCOMP_FilePath !== "") {
                                    var img = dd.ASCOMP_FilePath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    dd.filetype = lastelement;
                                    console.log("data.filetype : " + dd.filetype);
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ASCOMP_FilePath;
                                    }
                                }
                            });
                        }
                        $scope.OnChangeLIBYear("LIB");
                        $('#mymodalviewdetais').modal('show');
                    }
                }
            });

            //Praveen Gouda Added
            $scope.OnChangeLIBYear = function (flag) {
                $scope.librarydetails = [];
                var data = {
                    "flag": flag,
                    "ASMAY_Id": $scope.LIBYearId,
                    "AMST_IdTwo": student_id,
                    "OnClickOrOnChange": 'OnChange',
                };
                apiService.create("StudentDashboard/onclick_LIB", data).then(function (promise) {
                    if (promise.librarydetails !== null && promise.librarydetails.length > 0) {
                        $scope.librarydetails = promise.librarydetails;
                        $('#myModalLibrary').modal('show');
                    }
                    //else {
                    //    swal('No Data Found..!!');
                    //}
                });
            };

        };


    }

})();
