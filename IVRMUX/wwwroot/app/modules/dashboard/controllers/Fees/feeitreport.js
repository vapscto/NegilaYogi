

(function () {
    'use strict';
    angular
        .module('app')
        .controller('feeitreportController', feeitreportController)

    feeitreportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI','$compile']
    function feeitreportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI, $compile) {

        $scope.indrpt = false;
        $scope.report = false;
        var institutionid, automanualreceiptnotranum
        $scope.htmldata = "";

        $scope.totalpayable = 0;
        $scope.totalconcession = 0;
        $scope.totalpaid = 0;

        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var headtot = 0;
        var contot = 0;
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transactionnumbering.length; i++) {
            if (transactionnumbering.length > 0) {
                if (transactionnumbering[i].imN_Flag == "Online") {
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
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.obj = {};
        $scope.print_flag = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;

            apiService.getURI("feeitreport/getalldetails", pageid).
                then(function (promise) {
                    $scope.onclickloaddataclass();
                    $scope.yearlist = promise.adcyear;
                    $scope.classlist = promise.fillclass;

                    $scope.fillmasterhead = promise.fillmasterhead;
                })
        }

        //adding section 
        $scope.onselectclass = function (clsobj) {

            var data = {
                "ASMAY_Id": clsobj.asmaY_Id,
                "ASMCL_Id": clsobj.asmcL_Id,
            }


            apiService.create("feeitreport/getsection", data).
                then(function (promise) {
                    $scope.sectioncount = promise.fillsection;
                    //  $scope.arrlstinst1 = promise.fillinstallment;
                })
        }


       




        $scope.onselectsection = function (secobj) {

            var data = {
                "ASMAY_Id": secobj.asmaY_Id,
                "ASMCL_Id": secobj.asmcL_Id,
                "AMSC_Id": secobj.asmC_Id,
            }


          
            apiService.create("feeitreport/getstudent", data).then(function (promise) {
                $scope.studentlst = promise.fillstudent;
                var remove_list = [];
                var ins_spe_list = [];
                $scope.totalgrid = [];
                $scope.temptermarray = [];
                $scope.special_head_list = promise.specialheadlist;
                $scope.special_head_details = promise.specialheaddetails;
                for (var i = 0; i < promise.alldata.length; i++) {
                    $scope.temptermarray.push(promise.alldata[i]);

                }
                $scope.totalgrid = $scope.temptermarray;
                $scope.temp_Head_Instl_list = [];
                angular.forEach($scope.totalgrid, function (uy) {
                    uy.returnval = 'H';
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
                            var fmG_Id = 0;
                            var fmG_GroupName = '';
                            var not_cnt = 0;
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
                            })
                            if (not_cnt == 0) {
                                $scope.temp_Head_Instl_list.push({ fmG_Id: fmG_Id, fmG_GroupName: 'SH_' + fmG_GroupName, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, returnval: 'SH' });
                            }
                            else if (not_cnt > 0) {
                                $scope.temp_Head_Instl_list.push({ fmG_Id: 0, fmG_GroupName: 'Special_Head', fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, returnval: 'SH' });
                            }

                        })
                    })
                    $scope.totalgrid = $scope.temp_Head_Instl_list;
                }

                $scope.final_Head_Instl_list = [];

              
                angular.forEach($scope.totalgrid, function (e) {

                    if ($scope.final_Head_Instl_list.length === 0) {
                        $scope.final_Head_Instl_list.push({
                            fmH_ID: e.fmH_Id, fmH_FeeName: e.fmH_FeeName,
                            returnval: e.returnval
                        });
                    }
                    else if ($scope.final_Head_Instl_list.length > 0) {
                        var count = 0;
                        angular.forEach($scope.final_Head_Instl_list, function (dd) {
                            if (dd.fmH_ID === e.fmH_Id) {
                                count += 1;
                            }
                        });
                        if (count === 0) {
                            $scope.final_Head_Instl_list.push({
                                fmH_ID: e.fmH_Id, fmH_FeeName: e.fmH_FeeName,
                                returnval: e.returnval
                            });
                        }
                    }
                });   

                $scope.fillmasterhead = $scope.final_Head_Instl_list;
            });
        };



        $scope.onselectstudent = function (std) {

            var data = {
                "ASMAY_Id": std.asmaY_Id,
                "ASMCL_Id": std.asmcL_Id,
                "AMSC_Id": std.asmC_Id,
                "AMST_Id": std.amst_Id,

            }


            apiService.create("feeitreport/getreceipt", data).
                then(function (promise) {
                    $scope.receiptnoterms = promise.fillterms;
                })
        }



        //$scope.adyr = true;
        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }



        $scope.is_optionrequired_groupterm_grp = function () {

            // if ($scope.group_check==true) {
            return !$scope.fillmasterhead.some(function (options) {
                return options.fmH_Id_chk;
            });
            // }

        }

        $scope.onclickloaddataclass = function (obj) {
            $scope.submitted = false;
            $scope.obj.asmaY_Id = "";
            $scope.obj.asmcL_Id = "";
            $scope.obj.asmC_Id = "";
            $scope.obj.amst_Id = "";
            $scope.obj.fyp_id = "";

            if ($scope.Selectionrd === 'allr') {
                $scope.head = false;
                $scope.receipt = true;
                $scope.result = false;


            }
            else if ($scope.Selectionrd === 'Indi') {
                $scope.receipt = false;
                $scope.head = true;
                $scope.result = false;
            }
            else {
                $scope.receipt = false;
                $scope.head = false;
                $scope.result = false;
            }

        };



        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.showmodaldetail = function (obj) {

            $scope.totalpayable = 0;
            $scope.totalconcession = 0;
            $scope.totalpaid = 0;

            if ($scope.myForm.$valid) {

                $scope.ASMCL_ClassName = "";
                $scope.ASMC_SectionName = "";
                $scope.AMAY_RollNo = "";
                $scope.AMST_AdmNo = "";
                $scope.AMST_FirstName = "";
                 $scope.AMST_MiddleName = "";
                $scope.AMST_LastName = "";

                $scope.curdate = "";
                $scope.asmaY_Year = "";
                $scope.AMST_FatherName = "";
                $scope.AMST_MotherName = "";
                $scope.FMCC_ConcessionName = "";

                var FMH_ID = "0";
                $scope.albumNameArray = [];
                if ($scope.Selectionrd == "Indi") {
                    angular.forEach($scope.fillmasterhead, function (ty) {
                        if (ty.fmH_Ids) {
                            $scope.albumNameArray.push(ty);
                           // FMH_ID = FMH_ID + ',' + ty.fmH_Id;
                        }
                    })
                }

                angular.forEach($scope.yearlist, function (ty) {
                    if (ty.asmaY_Id == obj.asmaY_Id) {
                        $scope.asmaY_Year = ty.asmaY_Year;
                    };
                })



                if (obj.fyp_id == "") {
                    obj.fyp_id = "0";
                }
                
                
                var data = {

                    "ASMAY_Id": obj.asmaY_Id,
                    "ASMCL_Id": obj.asmcL_Id,
                    "AMSC_Id": obj.asmC_Id,
                    "AMST_Id": obj.amst_Id,
                    "FYP_Id": obj.fyp_id,
                    "minstall": mergeinstallment,
                    "returnval": $scope.Selectionrd,
                    //  "multiplegroups": FMH_ID
                    "savetmpdata": $scope.albumNameArray,
                }

                apiService.create("feeitreport/printreceipt", data).
                    then(function (promise) {
                        $scope.tempreceiptarraytermexfinal = [];
                        $scope.tempreceiptarraytermex = {};

                        if (promise.htmldata != null) {
                            $scope.htmldata = promise.htmldata;
                            if (promise.readmissionfeeschecking != null) {
                                $scope.stthomasreceipttemplate = promise.readmissionfeeschecking;
                                if (promise.fillstudenttype != null) {
                                    $scope.stthomastermname = promise.fillstudenttype;
                                }
                            }
                            var e1 = angular.element(document.getElementById("test"));
                            $compile(e1.html(promise.htmldata))(($scope));
                            if (promise.fillstudent.length > 0) {
                                $scope.result = false;
                                $scope.arrearamount = promise.fillstudent;
                                $scope.print_flag = true;
                            }

                            if (promise.fillstudentviewdetails.length > 0) {
                                $scope.result = false;
                                $scope.tempreceiptarray = [];
                                var feeheadname = "";
                                var validation;
                                $scope.special_head_list = promise.specialheadlist;
                                $scope.print_flag = true;

                                $scope.fillstudentdetails = promise.fillstudentviewdetails;

                                $scope.specialfeehead = promise.receiptformathead;

                                $scope.Paid_Date = $filter('date')(new Date(), "dd-MM-yyyy");
                                $scope.AMST_AdmNo = promise.fillstudentviewdetails[0].admno;
                                //  $scope.asmaY_Year = promise.fillstudentviewdetails[0].ASMAY_Year;
                                $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName;
                                $scope.AMST_MiddleName = promise.fillstudentviewdetails[0].amsT_MiddleName;
                                $scope.AMST_LastName = promise.fillstudentviewdetails[0].amsT_LastName;

                                $scope.ASMCL_ClassName = promise.fillstudentviewdetails[0].classname;
                                $scope.ASMC_SectionName = promise.fillstudentviewdetails[0].ASMC_SectionName;

                                $scope.AMST_FatherName = promise.fillstudentviewdetails[0].fathername;
                                $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;

                                $scope.AMST_MotherName = promise.fillstudentviewdetails[0].mothername;


                                $scope.period = promise.duration;

                                $scope.receiptno = promise.fyP_Receipt_No;
                                $scope.FYP_Receipt_No = promise.fyP_Receipt_No;

                                $scope.showdetailsreceipt = promise.fillstudentviewdetails;

                                angular.forEach(promise.fillstudentviewdetails, function (e) {
                                    $scope.totalpayable += e.fmA_Amount;
                                    $scope.totalconcession += e.ftP_Concession_Amt;
                                    $scope.totalpaid += e.ftP_Paid_Amt;
                                });

                                $scope.totalpayable = Number($scope.totalpayable);
                                $scope.totalconcession = Number($scope.totalconcession);
                                $scope.totalpaid = Number($scope.totalpaid);
                                $scope.atotA = Number($scope.totalpaid);
            

                                $scope.words = $scope.amountinwords(Number($scope.totalpaid));

                                var feeheadname = "";
                                var validation;
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


                                    for (var r = 0; r < $scope.tempreceiptarraytermexfinal.length; r++) {
                                        if ($scope.tempreceiptarraytermexfinal[r].fmH_FeeName != undefined) {
                                            $scope.tempreceiptarray.push($scope.tempreceiptarraytermexfinal[r]);
                                        }
                                    }
                                }




                                $scope.showdetailsreceipt = $scope.tempreceiptarray;
                                $scope.showtotaldetails = promise.filltotaldetails;




                            }

                  
                        }
                        else {

                            if (promise.fillstudent.length > 0) {
                                $scope.result = true;
                                $scope.arrearamount = promise.fillstudent;
                                $scope.print_flag = true;
                            }

                            if (promise.fillstudentviewdetails.length > 0) {
                                $scope.result = true;
                                $scope.tempreceiptarray = [];
                                var feeheadname = "";
                                var validation;
                                $scope.special_head_list = promise.specialheadlist;
                                $scope.print_flag = true;

                                $scope.fillstudentdetails = promise.fillstudentviewdetails;

                                $scope.specialfeehead = promise.receiptformathead;

                                $scope.Paid_Date = $filter('date')(new Date(), "dd-MM-yyyy");
                                $scope.AMST_AdmNo = promise.fillstudentviewdetails[0].admno;
                                //  $scope.asmaY_Year = promise.fillstudentviewdetails[0].ASMAY_Year;
                                $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName;
                                $scope.AMST_MiddleName = promise.fillstudentviewdetails[0].amsT_MiddleName;
                                $scope.AMST_LastName = promise.fillstudentviewdetails[0].amsT_LastName;

                                $scope.ASMCL_ClassName = promise.fillstudentviewdetails[0].classname;
                                $scope.ASMC_SectionName = promise.fillstudentviewdetails[0].ASMC_SectionName;

                                $scope.AMST_FatherName = promise.fillstudentviewdetails[0].fathername;
                                $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;

                                $scope.AMST_MotherName = promise.fillstudentviewdetails[0].mothername;


                                $scope.period = promise.duration;
                                if (institutionid == 5) {
                                    $scope.period = $scope.asmaY_Year;
                                }

                                $scope.FYP_Receipt_No = promise.fyP_Receipt_No;

                                $scope.showdetailsreceipt = promise.fillstudentviewdetails;

                                angular.forEach(promise.fillstudentviewdetails, function (e) {
                                    $scope.totalpayable += e.fmA_Amount;
                                    $scope.totalconcession += e.ftP_Concession_Amt;
                                    $scope.totalpaid += e.ftP_Paid_Amt;
                                });

                                $scope.totalpayable = Number($scope.totalpayable);
                                $scope.totalconcession = Number($scope.totalconcession);
                                $scope.totalpaid = Number($scope.totalpaid);

                                $scope.words = $scope.amountinwords(Number($scope.totalpaid));

                                var feeheadname = "";
                                var validation;
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


                                    for (var r = 0; r < $scope.tempreceiptarraytermexfinal.length; r++) {
                                        if ($scope.tempreceiptarraytermexfinal[r].fmH_FeeName != undefined) {
                                            $scope.tempreceiptarray.push($scope.tempreceiptarraytermexfinal[r]);
                                        }
                                    }
                                }




                                $scope.showdetailsreceipt = $scope.tempreceiptarray;
                                $scope.showtotaldetails = promise.filltotaldetails;




                            }
                        }

                    })

            }
            else {
                $scope.submitted = true;
                $scope.result = false;
            }
        }

        $scope.allstudentcheck = function (obj) {

            angular.forEach($scope.fillmasterhead, function (obj1) {
                if (obj.allstdcheck == true) {
                    obj1.fmH_Ids = true;
                }
                else {
                    obj1.fmH_Ids = false;
                }
            })
        }






        $scope.ctotalC = function (int) {
            var total = 0;
            angular.forEach($scope.showdetailsreceipt, function (e) {
                total += e.totalcharges;
            });
            return total;
        };


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


        $scope.closedata = function () {
           
           
            $state.reload();
        }
        $scope.printToCart = function () {

            var pdss = "";

            pdss = 'printrcp'

            var innerContents = document.getElementById(pdss).innerHTML;
            var popupWinindow = window.open('_blank', 'padding-top=50%;');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReport/itreportpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
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


    }
})();

