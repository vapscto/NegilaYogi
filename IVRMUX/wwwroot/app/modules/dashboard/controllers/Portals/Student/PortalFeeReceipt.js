(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeReceiptController', FeeReceiptController);

    FeeReceiptController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$compile'];
    function FeeReceiptController($rootScope, $scope, $state, $location, Flash, apiService, $stateParams, $filter, superCache, $window, $compile) {

        var institutionid, automanualreceiptnotranum
        //var configsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //$scope.miid = configsettings[0].mI_Id;
        //var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        //institutionid = configsettings[0].mI_Id;
        //$scope.obj = {};
        //var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}
        //else {
        //    paginationformasters = 0;
        //}
        //$scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}
        //$scope.imgname = logopath;


        var grouporterm, autoreceipt, receiptformat, mergeinstallment, fineapplicable;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
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
            apiService.getDATA("FeeReceipt/getloaddata").
                then(function (promise) {
                    $scope.yearlst = promise.yearlist;
                    swal("If receipt is not generated,then it will be updated within 24 hrs after Successful Payment!!");
                });
        };
        //===================== Academic Year Selection

        $scope.onyearchange123 = function (asmaY_Id) {
            $scope.showdetailsreceipt = "";
            $scope.getfeereceiptlst = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("FeeReceipt/getrecdetails", data).
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
            var totnet = 0, totalbalance = 0, totpaid = 0, totcon = 0, totnetexcludingall = 0;
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
            apiService.create("FeeReceipt/getdetails", data).
                //  apiService.create("FeeStudentTransaction/printreceipt", data).
                then(function (promise) {

                    //MB for Special
                    $scope.special_head_list = promise.specialheadlist;
                    $scope.special_head_details = promise.specialheaddetails;
                    //MB for Special
                    $scope.htmldata = promise.htmldata;

                    if (promise.htmldata !== "") {

                        if (promise.srkvsdetails !== null) {
                            if (promise.srkvsdetails.length > 0) {
                                for (var i = 0; i < promise.srkvsdetails.length; i++) {
                                    totnet = Number(totnet) + Number(promise.srkvsdetails[i].fsS_TotalToBePaid);
                                    totnetexcludingall = Number(totnetexcludingall) + Number(promise.srkvsdetails[i].fsS_CurrentYrCharges);
                                    if (Number(promise.srkvsdetails[i].fsS_TotalToBePaid) > 0) {
                                        totalbalance = Number(totalbalance) + Number(promise.srkvsdetails[i].fsS_ToBePaid);
                                    }
                                    else {
                                        totalbalance = 0;
                                    }

                                    totpaid = Number(totpaid) + Number(promise.srkvsdetails[i].ftP_Paid_Amt);
                                    totcon = Number(totcon) + Number(promise.srkvsdetails[i].ftP_Concession_Amt);
                                }

                                $scope.Tnet = totnet;
                                $scope.totnetexcludingallcharges = totnetexcludingall;
                                $scope.Tcon = totcon;
                                if (Number(totnet) > Number(totpaid)) {
                                    $scope.Tbal = totalbalance;
                                }
                                else {
                                    $scope.Tbal = 0;
                                }
                                $scope.Tpai = totpaid;
                            }
                        }

                        $scope.period = promise.duration;

                        $scope.paymenrgrid = promise.currpaymentdetails;

                        if ($scope.paymenrgrid.length > 0) {
                            $scope.paymentmode = $scope.paymenrgrid[0].fyP_Bank_Or_Cash;
                        }
                        var totconpaid = 0, totchargesss = 0, totconcessionss = 0;

                        for (var r = 0; r < promise.fillstudentviewdetails.length; r++) {
                            totconpaid = promise.fillstudentviewdetails[r].ftP_Concession_Amt;
                            totchargesss = Number(totchargesss) + Number(promise.fillstudentviewdetails[r].fmA_Amount);
                            totconcessionss = Number(totconcessionss) + Number(promise.fillstudentviewdetails[r].ftP_Concession_Amt);
                        }

                        $scope.totalchargessss = Number(totchargesss);

                        $scope.atotA = promise.currpaymentdetails[0].ftP_Paid_Amt;
                        $scope.ctotA = Number(totconcessionss);
                        $scope.totchar = $scope.atotA + $scope.ctotA;
                        $scope.actotA = $scope.atotA + $scope.ctotA;
                  
                        $scope.words = $scope.amountinwords($scope.atotA);
                        $scope.narration = promise.currpaymentdetails[0].fyP_Remarks;
                        $scope.year = promise.year;

                        angular.forEach($scope.yearlst, function (op_m) {
                            if (op_m.asmaY_Id === parseInt($scope.asmaY_Id)) {
                                $scope.asmaY_Year = op_m.asmaY_Year;
                            }
                        });

                        $scope.asmaY_Year = $scope.asmaY_Year;

                        $scope.due_amount = promise.dueamount;
                        if ($scope.due_amount === 0) {
                            $scope.date = "";
                            $scope.nextduedate = false;
                        }
                        else {
                            $scope.date = promise.date;
                            //$scope.date = promise.duedetails[0].date;
                            $scope.nextduedate = true;
                        }

                        if ($scope.due_amount === 0) {
                            $scope.months = "";
                        }
                        else {
                            //$scope.months = promise.duedetails[0].month;
                            $scope.months = promise.month;

                            $scope.nextduedate = true;
                        }

                        var termname = " ";
                        if (promise.termremarks.length > 0) {
                            for (var jk = 0; jk < promise.termremarks.length; jk++) {
                                if (termname === " ") {
                                    termname = promise.termremarks[jk].termname;
                                }
                                else {
                                    termname = termname + ',' + promise.termremarks[jk].termname;
                                }
                            }
                        }

                        var feeheadname = "";
                        var validation;
                        $scope.tempreceiptarraytermexfinal = [];
                        $scope.receiptformattem = receiptformat;
                        if (receiptformat === "1" || receiptformat === "0") {
                            $scope.tempreceiptarray = [];
                            $scope.tempreceiptarrayterm = {
                            };
                            $scope.tempreceiptarraytermex = {
                            };
                            var totalamount = 0, concessionamt = 0, fineamt = 0, feecount = 0, fmH_FeeName, feetotcharges = 0;
                            var totalamountex = 0, concessionamtex = 0, fineamtex = 0, fmH_FeeNameex, feetotchargesex = 0;

                            var adjustedamt = 0; var adjustedamtex = 0;

                            if (promise.fillstudentviewdetails.length > 0) {
                                for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {
                                    var mainheadid = promise.fillstudentviewdetails[i].fmH_Id;
                                    var maininstallmentid = promise.fillstudentviewdetails[i].ftI_Id;
                                    if (promise.receiptformathead.length > 0) {
                                        for (var j = 0; j < promise.receiptformathead.length; j++) {
                                            var subheadid = promise.receiptformathead[j].fmH_Id;
                                            var subinstid = promise.receiptformathead[j].ftI_Id;

                                            if (mainheadid === subheadid && maininstallmentid === subinstid) {
                                                feecount = Number(feecount) + 1;

                                                feetotcharges = Number(feetotcharges) + Number(promise.receiptformathead[j].totalcharges);

                                                fmH_FeeName = promise.receiptformathead[j].fmH_FeeName;
                                                totalamount = Number(totalamount) + Number(promise.receiptformathead[j].ftP_Paid_Amt);
                                                concessionamt = Number(concessionamt) + Number(promise.receiptformathead[j].ftP_Concession_Amt);
                                                fineamt = Number(fineamt) + Number(promise.receiptformathead[j].ftP_Fine_Amt);
                                            }
                                        }
                                        if (feecount < 1) {

                                            fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName;
                                            if (feeheadname === "") {
                                                feetotchargesex = Number(promise.fillstudentviewdetails[i].totalcharges);
                                                totalamountex = Number(promise.fillstudentviewdetails[i].ftP_Paid_Amt);
                                                concessionamtex = Number(promise.fillstudentviewdetails[i].ftP_Concession_Amt);
                                                fineamtex = Number(promise.fillstudentviewdetails[i].ftP_Fine_Amt);
                                            }

                                            else if (fmH_FeeNameex === feeheadname) {
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
                                                    totalcharges: feetotchargesex
                                                };

                                                if ($scope.tempreceiptarraytermexfinal.length > 0) {
                                                    var aldy_cnt = 0;
                                                    angular.forEach($scope.tempreceiptarraytermexfinal, function (obj) {
                                                        if (feeheadname === obj.fmH_FeeName) {
                                                            aldy_cnt += 1;
                                                        }
                                                    }
                                                    );
                                                    if (aldy_cnt === 0) {
                                                        $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                    }
                                                    //MB
                                                    else if (aldy_cnt === 1) {
                                                        for (var q = 0; q < $scope.tempreceiptarraytermexfinal.length; q++) {
                                                            if (feeheadname === $scope.tempreceiptarraytermexfinal[q].fmH_FeeName) {
                                                                $scope.tempreceiptarraytermexfinal.splice(q, 1);
                                                            }
                                                        }
                                                        $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                    }
                                                    //MB
                                                }
                                                else if ($scope.tempreceiptarraytermexfinal.length === 0) {
                                                    $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                }

                                                fmH_FeeNameex = promise.fillstudentviewdetails[i].fmH_FeeName;
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
                                                    fsS_AdjustedAmount: adjustedamtex
                                                    //end
                                                };

                                                if ($scope.tempreceiptarraytermexfinal.length > 0) {
                                                    var aldy_cnt1 = 0;
                                                    angular.forEach($scope.tempreceiptarraytermexfinal, function (obj) {
                                                        if (fmH_FeeNameex === obj.fmH_FeeName) {
                                                            aldy_cnt1 += 1;
                                                        }
                                                    }
                                                    );
                                                    if (aldy_cnt1 === 0) {
                                                        $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                    }

                                                    //MB
                                                    else if (aldy_cnt1 === 1) {
                                                        for (var q = 0; q < $scope.tempreceiptarraytermexfinal.length; q++) {
                                                            if (feeheadname === $scope.tempreceiptarraytermexfinal[q].fmH_FeeName) {
                                                                $scope.tempreceiptarraytermexfinal.splice(q, 1);
                                                            }
                                                        }
                                                        $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                                    }
                                                    //MB

                                                }
                                                else if ($scope.tempreceiptarraytermexfinal.length === 0) {
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
                                    var feetotcharges1 = 0, totalamount1 = 0, concessionamt1 = 0, fineamt1 = 0, fmH_FeeName1 = "", adjest1 = 0;
                                    angular.forEach(promise.receiptformathead, function (sh_hd) {
                                        if (sp_hd.fmsfH_Name === sh_hd.fmH_FeeName) {
                                            count += 1;
                                            feetotcharges1 = Number(feetotcharges1) + Number(sh_hd.totalcharges);
                                            fmH_FeeName1 = sh_hd.fmH_FeeName;
                                            totalamount1 = Number(totalamount1) + Number(sh_hd.ftP_Paid_Amt);
                                            concessionamt1 = Number(concessionamt1) + Number(sh_hd.ftP_Concession_Amt);
                                            fineamt1 = Number(fineamt1) + Number(sh_hd.ftP_Fine_Amt);
                                            adjest1 = Number(adjest1) + Number(sh_hd.fsS_AdjustedAmount);
                                        }
                                    });
                                    if (count > 0) {
                                        $scope.tempreceiptarrayterm = {
                                            fmH_FeeName: fmH_FeeName1,
                                            ftP_Paid_Amt: totalamount1,
                                            ftP_Concession_Amt: concessionamt1,
                                            ftp_fine_amt: fineamt1,
                                            totalcharges: feetotcharges1,
                                            fsS_AdjustedAmount: adjest1
                                        };
                                        console.log($scope.tempreceiptarrayterm);
                                        $scope.tempreceiptarray.push($scope.tempreceiptarrayterm);
                                    }
                                });

                                //MB For Special

                                if (validation !== "add") {
                                    $scope.tempreceiptarraytermex = {
                                        fmH_FeeName: fmH_FeeNameex,
                                        ftP_Paid_Amt: totalamountex,
                                        ftP_Concession_Amt: concessionamtex,
                                        ftp_fine_amt: fineamtex,
                                        totalcharges: feetotchargesex,
                                        fsS_AdjustedAmount: adjustedamtex
                                    };

                                    $scope.tempreceiptarraytermexfinal.push($scope.tempreceiptarraytermex);
                                }

                                for (var r = 0; r < $scope.tempreceiptarraytermexfinal.length; r++) {
                                    if ($scope.tempreceiptarraytermexfinal[r].fmH_FeeName !== undefined) {
                                        $scope.tempreceiptarray.push($scope.tempreceiptarraytermexfinal[r]);
                                    }
                                }
                            }

                            $scope.showdetailsreceipt = $scope.tempreceiptarray;
                            $scope.showtotaldetails = promise.filltotaldetails;

                            //added for total
                            angular.forEach($scope.showdetailsreceipt, function (ll) {
                                if (ll.fsS_AdjustedAmount !== undefined && ll.fsS_AdjustedAmount !== undefined) {
                                    $scope.AdjustedAmounttotal += ll.fsS_AdjustedAmount;
                                }
                            });
                        }
                        else {
                            $scope.showdetailsreceipt = promise.fillstudentviewdetails;
                            $scope.showtotaldetails = promise.filltotaldetails;

                            //added for total
                            angular.forEach($scope.showdetailsreceipt, function (vv) {
                                if (vv.fsS_AdjustedAmount !== undefined && vv.fsS_AdjustedAmount !== undefined) {
                                    $scope.AdjustedAmounttotal += vv.fsS_AdjustedAmount;
                                }
                            });
                        }

                        if (promise.fillstudentviewdetails[0].amsT_FirstName !== null && promise.fillstudentviewdetails[0].amsT_MiddleName !== null && promise.fillstudentviewdetails[0].amsT_LastName !== null) {
                            $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                        }
                        else {
                            if (promise.fillstudentviewdetails[0].amsT_FirstName === null) {
                                promise.fillstudentviewdetails[0].amsT_FirstName = ' ';
                            }
                            if (promise.fillstudentviewdetails[0].amsT_MiddleName === null) {
                                promise.fillstudentviewdetails[0].amsT_MiddleName = ' ';
                            }
                            if (promise.fillstudentviewdetails[0].amsT_LastName === null) {
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
                        $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].FMCC_ConcessionName;

                        $scope.amst_mobile = promise.fillstudentviewdetails[0].amst_mobile;
                        $scope.FYP_ChallanNo = promise.fillstudentviewdetails[0].fyP_ChallanNo;

                        $scope.Paid_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");

                        if (promise.fillstudentviewdetails[0].fyp_transaction_id !== null) {
                            $scope.FYP_PaymentReference_Id = promise.fillstudentviewdetails[0].fyp_transaction_id;
                        }
                        else {
                            $scope.FYP_PaymentReference_Id = "";
                        }

                        $scope.totalpayableamt = 0;

                        angular.forEach(promise.fillstudentviewdetails, function (llll) {
                            $scope.totalpayableamt += Number(llll.totalcharges);
                        });

                        if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash === "C") {
                            $scope.bankdet = false;
                            $scope.FYP_Date = "--";
                        }

                        if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash === "O") {
                            $scope.bankdet = false;
                        }
                        
                         $scope.FYP_DD_Cheque_No = "";
                        $scope.FYP_Bank_Name = "";

                        if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash === "B" || promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash === "R") {
                            $scope.bankdet = true;
                            $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                            $scope.FYP_DD_Cheque_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                            $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                        }

                        if (promise.fillstudentviewdetails[0].fyP_Remarks !== null || promise.fillstudentviewdetails[0].fyP_Remarks !== "") {
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

                        if (promise.fillstudentviewdetails[0].amsT_FirstName !== null && promise.fillstudentviewdetails[0].amsT_MiddleName !== null && promise.fillstudentviewdetails[0].amsT_LastName !== null) {
                            $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName + ' ' + promise.fillstudentviewdetails[0].amsT_MiddleName + ' ' + promise.fillstudentviewdetails[0].amsT_LastName;
                        }
                        else {
                            if (promise.fillstudentviewdetails[0].amsT_FirstName === null) {
                                promise.fillstudentviewdetails[0].amsT_FirstName = ' ';
                            }
                            if (promise.fillstudentviewdetails[0].amsT_MiddleName === null) {
                                promise.fillstudentviewdetails[0].amsT_MiddleName = ' ';
                            }
                            if (promise.fillstudentviewdetails[0].amsT_LastName === null) {
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
                        $scope.asmaY_Year = promise.year;
                        $scope.Paid_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");
                  
                        if ($scope.FMC_RInstallmentsMergeFlag === 1) {
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
                                fmatotal = fmatotal + user.totalcharges;
                                totalpaidamount = totalpaidamount + user.ftP_Paid_Amt;
                            });
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

                        if ($scope.due_amount === 0) {
                            $scope.date = "";
                            $scope.nextduedate = false;
                        }
                        else {
                            $scope.date = promise.date;
                            $scope.nextduedate = true;
                        }

                        if ($scope.due_amount === 0) {
                            $scope.months = "";
                        }
                        else {
                            $scope.months = promise.month;
                            $scope.nextduedate = true;
                        }

                        $scope.year = promise.year;


                        if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash === "C") {
                            //$scope.FYP_Bank_Or_Cash = "Cash";
                            $scope.FYP_Bank_Name = "--";
                            $scope.FYP_DD_Cheque_No = "--";
                            $scope.FYP_Date = promise.fillstudentviewdetails[0].FYP_Date;

                        }
                        else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash === "O") {
                            //$scope.FYP_Bank_Or_Cash = "Online Payment";
                            $scope.FYP_Bank_Name = "--";
                            $scope.FYP_DD_Cheque_No = "--";
                            $scope.FYP_Date = promise.fillstudentviewdetails[0].FYP_Date;

                        }
                        else if (promise.fillstudentviewdetails[0].fyP_Bank_Or_Cash === "B") {
                            //$scope.FYP_Bank_Or_Cash = "Bank";
                            $scope.FYP_Bank_Name = promise.fillstudentviewdetails[0].fyP_Bank_Name;
                            $scope.FYP_DD_Cheque_No = promise.fillstudentviewdetails[0].fyP_DD_Cheque_No;
                            //  $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_DD_Cheque_Date, "dd-MM-yyyy");
                            $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");
                        }
                        $scope.FYP_Date = $filter('date')(promise.fillstudentviewdetails[0].fyP_Date, "dd-MM-yyyy");
                        if (promise.fillstudentviewdetails[0].fyP_Remarks !== null || promise.fillstudentviewdetails[0].fyP_Remarks !== "") {
                            $scope.feeremarks = promise.fillstudentviewdetails[0].fyP_Remarks;
                            //$scope.feeremarks = termname;
                        }
                        else {
                            $scope.feeremarks = "Remarks Not Given";
                        }
                        $scope.AMAY_RollNo = promise.fillstudentviewdetails[0].rollno;
                        $scope.curdate = $filter('date')(new Date(), "dd-MM-yyyy");
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

        };

        //================== PRINT
        //$scope.printData = function (printmodal) {
        //    html2canvas(document.getElementById('printmodal'), {
        //        onrendered: function (canvas) {
        //            var data = canvas.toDataURL();
        //            var docDefinition = {
        //                content: [{
        //                    image: data,
        //                    width: 500,
        //                }]
        //            };
        //            pdfMake.createPdf(docDefinition).download("fee.pdf");
        //        }
        //    });
        //    //var innerContents = document.getElementById("printmodal").innerHTML;
        //    //var popupWinindow = window.open('');
        //    //popupWinindow.document.open();
        //    //popupWinindow.document.write('<html><head>' +
        //    //    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
        //    //    '<link href="plugins/bootstrap/css/bootstrap.css" />' +
        //    //    '<link href="css/style.css" rel="stylesheet" />' +
        //    //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    //popupWinindow.document.close();
        //};

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
        
        //Pdf download
        $scope.download = function () {            
            html2canvas(document.getElementById('printmodal'), {
                onrendered: function (canvas) {
                    var data = canvas.toDataURL();
                    var docDefinition = {
                        content: [{
                            image: data,
                            width: 500,
                            height: 600
                        }]
                    };
                    pdfMake.createPdf(docDefinition).download("FEE RECEIPT.pdf");
                }
            });        
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
