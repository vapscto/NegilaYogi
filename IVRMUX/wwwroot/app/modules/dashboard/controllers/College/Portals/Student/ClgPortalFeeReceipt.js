(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgFeeReceiptController', ClgFeeReceiptController);

    ClgFeeReceiptController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$compile'];
    function ClgFeeReceiptController($rootScope, $scope, $state, $location, Flash, apiService, $stateParams, $filter, superCache, $window, $compile) {

        var institutionid;
        var grouporterm, autoreceipt, receiptformat, mergeinstallment, fineapplicable;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        institutionid = configsettings[0].mI_Id;
       
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transactionnumbering.length; i++) {
            if (transactionnumbering.length > 0) {
                if (transactionnumbering[i].imN_Flag === "Online") {
                    $scope.transnumbconfigurationsettingsassign = transactionnumbering[i];
                }
            }
        }
        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
        $scope.usrname = localStorage.getItem('username');



        //===================================================== PAGE LOAD
        $scope.bankdetails = true;
        $scope.hide_FeeReceipt = false;
        $scope.hide_FeeReceipt1 = false;

        $scope.loaddata = function () {
            var a = 0;
            apiService.getDATA("ClgFeeReceipt/getloaddata").
                then(function (promise) {

                    if (promise.feeconfiglist.length > 0) {
                        $scope.feedisplayconfig = promise.feeconfiglist[0].fmC_DetailedDisplayFlg;
                    }

                    $scope.yearlst = promise.yearlist;
                    swal("If receipt is not generated,then it will be updated within 24 hrs after Successful Payment!!");

                });
        };
        //===================== Academic Year Selection

        $scope.onyearchange123 = function (asmaY_Id) {
            $scope.showdetailsreceipt = "";
            $scope.getfeereceiptlst = "";
            $scope.htmldata = "data";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("ClgFeeReceipt/getrecdetails", data).
                then(function (promise) {

                    $scope.recnolst = promise.recnolist;
                    if ($scope.recnolst.length === "0") {
                        swal("No Record Found....");
                        $state.reload();
                        $scope.recnolst = "";
                    }
                });
        };
        //========================= Print Receipt
        $scope.bankdet = false;
        $scope.onreceiptchange = function (fypid) {
            $scope.ASMCL_ClassName = "";
            $scope.ASMC_SectionName = "";
            $scope.AMAY_RollNo = "";
            $scope.AMST_AdmNo = "";
            $scope.AMST_FirstName = "";
            $scope.curdate = "";
            $scope.asmaY_Year = "";
            $scope.AMST_FatherName = "";
            $scope.AMST_MotherName = "";
            $scope.receiptno = "";
            $scope.FMCC_ConcessionName = "";
            $scope.htmldata = "data";

            var totnet = 0, totalbalance = 0, totpaid = 0, totcon = 0;

            if (fypid !== null && fypid > 0) {
                var data = {
                    "configset": grouporterm,
                    "FYP_Id": fypid,
                    "minstall": mergeinstallment,
                    "ASMAY_ID": $scope.asmaY_Id
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                //apiService.create("ClgFeeReceipt/getdetails", data).
                apiService.create("ClgFeeReceipt/printreceipt", data).
                    //  apiService.create("FeeStudentTransaction/printreceipt", data).
                    then(function (promise) {

                        $scope.htmldata = promise.htmldata;
                        $scope.payment_mode_details = promise.paymentmode_details;
                        if (promise.htmldata != "") {
                            $scope.period = promise.duration;
                            $scope.paymenrgrid = promise.currpaymentdetails;
                            $scope.atotA = promise.currpaymentdetails[0].ftcP_PaidAmount;
                            $scope.ctotA = promise.currpaymentdetails[0].ftcP_ConcessionAmount;
                            $scope.totchar = $scope.atotA + $scope.ctotA;
                            $scope.words = $scope.amountinwords($scope.atotA);
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
                                    //MB For Special
                                    //$scope.tempreceiptarrayterm = {
                                    //    fmH_FeeName: fmH_FeeName,
                                    //    ftcP_PaidAmount: totalamount,
                                    //    ftcP_ConcessionAmount: concessionamt,
                                    //    ftcP_FineAmount: fineamt,
                                    //    fcsS_TotalCharges: feetotcharges,
                                    //};

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
                            $scope.totchags = 0;
                            angular.forEach($scope.showdetailsreceipt, function (ft) {
                                $scope.totchags += Number(ft.fcsS_TotalCharges);
                            })
                            //
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

                            $scope.TRANSAC_DATE = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");

                            $scope.asmaY_Year = promise.asmaY_Year;
                            $scope.AMCST_FatherName = promise.fillstudentviewdetails[0].amcsT_FatherName;
                            $scope.AMCST_MotherName = promise.fillstudentviewdetails[0].amcsT_MotherName;
                            $scope.receiptno = promise.fillstudentviewdetails[0].fyP_ReceiptNo;

                            $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;
                            // $scope.curdate = $filter('date')(new Date(), "dd-MM-yyyy");
                            $scope.curdate = new Date();
                            $scope.Paid_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                            if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "C") {
                                $scope.bankdet = false;
                                $scope.modeofpayment = "Cash";
                                // $scope.FYP_ReceiptDate = "--";
                            }
                            if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "O") {
                                $scope.bankdet = false;
                                $scope.modeofpayment = "Online";
                            }
                            if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "B" || promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "R") {
                                $scope.bankdet = true;
                                $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                                //$scope.FYP_DD_Cheque_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                                $scope.FYP_DD_Cheque_Date = promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date;
                                $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                                $scope.modeofpayment = "Bank/RTGS";
                            }
                            if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "S") {
                                $scope.modeofpayment = "Swipe";
                            }


                            if (promise.fillstudentviewdetails[0].fyP_Remarks != null || promise.fillstudentviewdetails[0].fyP_Remarks != "") {
                                $scope.feeremarks = termname;
                            }
                            else {
                                $scope.feeremarks = "Remarks Not Given";
                            }
                            var e1 = angular.element(document.getElementById("test"));
                            $compile(e1.html(promise.htmldata))(($scope));
                        }
                        else {
                            $scope.MI_Institute = promise.masterinstitution[0].mI_Name;
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
                                $scope.modeofpayment = "Cash";
                                $scope.FYP_Bank_Name = "--";
                                $scope.FYP_DD_Cheque_No = "--";
                                $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].FYP_ReceiptDate;
                            }
                            else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "O") {
                                $scope.modeofpayment = "Online";
                                $scope.FYP_Bank_Name = "--";
                                $scope.FYP_DD_Cheque_No = "--";
                                $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].FYP_ReceiptDate;
                            }
                            else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "B") {
                                $scope.modeofpayment = "Bank";
                                $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                                $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                                //$scope.FYP_ReceiptDate = $filter('date')(promise.fillstudentviewdetails[0].fyP_ReceiptDate, "dd-MM-yyyy");
                                $scope.FYP_ReceiptDate = promise.fillstudentviewdetails[0].fyP_ReceiptDate;
                            }
                            else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "S") {
                                $scope.modeofpayment = "Swipe";
                                $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                                $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                            }
                            else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash == "R") {
                                $scope.modeofpayment = "RTGS";
                                $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                                $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
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
                    });

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



        };

        //================== PRINT
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
        };

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
        };
        //========================= Clear
        $scope.Clear = function () {
            $state.reload();
        };

        //Number to Words
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

    };
})();