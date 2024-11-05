(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeFeeOnlinePaymentController', CollegeFeeOnlinePaymentController)

    CollegeFeeOnlinePaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$window', 'FormSubmitter', 'superCache','$compile']
    function CollegeFeeOnlinePaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $window, FormSubmitter, superCache, $compile) {

        $scope.students = [];
        $scope.gateway = true;

        $scope.qwe = {}; 
        $scope.qweteresian = {};

        $scope.disableall = true;
        $scope.disableindividual = true;

        $scope.SHOWTOTALPAYABL = false;

        $scope.showsingle = false;
        $scope.showdouble = false;
        $scope.payuvisible = false;
        $scope.billdeskvisible = false;
        $scope.ebsvisible = false;
        $scope.collapsed = true;
        var netamount = 0
        var concessionamount = 0
        var fineamount = 0
        var payableamountamount = 0
        var paidamount = 0
        var fine_total = 0;
        $scope.totalfine = 0;
        $scope.totalfineadv = 0;
        $scope.advopeningamt = true;
        $scope.advnetamt = true;
        $scope.advconamt = true;
        $scope.advtotamt = true;
        $scope.advpaidamt = true;
        $scope.advtobepaidamt = true;
        $scope.FMC_EnablePartialPaymentFlg = 0;
        var advance = 0;
        // $scope.checkedheadlst = false;
        angular.forEach($scope.termlist, function (tstobj) {
            tstobj.checkedheadlst = false;
        })
        $scope.regulardisplay = true;

      


        var grouporterm, autoreceipt, automanualreceiptnotranum, FMC_DetailedDisplayFlg;
        var institutionid = 0;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        if (configsettings.length > 0) {

            institutionid = configsettings[0].mI_Id;

            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            FMC_DetailedDisplayFlg = configsettings[0].fmC_DetailedDisplayFlg;
            $scope.FMC_EnablePartialPaymentFlg = configsettings[0].fmC_EnablePartialPaymentFlg
        }

        //if ($scope.FMC_EnablePartialPaymentFlg == 0) {
        //    $scope.totalamtdisable = true;
        //}
        //else {
        //    $scope.totalamtdisable = false;
        //}

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

        if (institutionid == "4") {
            $scope.linkname = "https://bdcampusstrg.blob.core.windows.net/files/18-19 TRANS FEE.pdf"
        }
        else if (institutionid == "6") {
            $scope.linkname = "https://bdcampusstrg.blob.core.windows.net/files/BBHSFeeStructure.jpg"
        }


        $scope.disablecheckbox = true;
        $scope.disablechild = true;

        var globalamount;
        var searchObject = $location.search();
        $scope.prospectusno = false;

        $scope.onlinepayment = true;

        $scope.showbasicdetails = true;
        $scope.showpaymentdetails = true;
        $scope.temp_Fine_Amountsadv = [];
        // swal(searchObject.status);
        if (searchObject.status == "failure") {
            swal("Payment Unsuccessfull");
            //  Request.QueryString.Remove("status");
            //$location.url($location.path)
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "success") {
            swal("Payment Successfull");
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Networkfailure") {
            swal('Network failure..!!', 'Try again after some time');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }


        //checkbox

        //$scope.firstfnc = function (vlobj) {

        //    if (vlobj.checkedgrplst == true) {
        //        angular.forEach($scope.grouplst, function (obj) {
        //            if (vlobj.fmgG_Id == obj.fmgG_Id) {
        //                angular.forEach($scope.termlist, function (obj1) {
        //                    if (obj1.fmgG_Id == obj.fmgG_Id && obj1.termdisablechk == false) {
        //                        obj1.checkedheadlst = true;
        //                        //angular.forEach($scope.installlst, function (obj2) {
        //                        //    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
        //                        //        obj2.checkedinstallmentlst = true;
        //                        //    }
        //                        //});
        //                    }
        //                });
        //            }
        //        });
        //    } else {
        //        angular.forEach($scope.grouplst, function (obj) {
        //            if (vlobj.fmgG_Id == obj.fmgG_Id) {
        //                angular.forEach($scope.termlist, function (obj1) {
        //                    if (obj1.fmgG_Id == obj.fmgG_Id) {
        //                        obj1.checkedheadlst = false;
        //                        //angular.forEach($scope.installlst, function (obj2) {
        //                        //    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
        //                        //        obj2.checkedinstallmentlst = false;
        //                        //    }
        //                        //});
        //                    }
        //                });
        //            }
        //        });
        //    }
        //}

        //$scope.secfnc = function (vlobj1) {
        //    //
        //    //if (vlobj1.checkedheadlst == true) {
        //    //    angular.forEach($scope.headlst, function (val) {
        //    //        if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
        //    //            angular.forEach($scope.installlst, function (val1) {
        //    //                if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
        //    //                    val1.checkedinstallmentlst = true;
        //    //                }
        //    //            });
        //    //        }
        //    //    });
        //    //} else {
        //    //    
        //    //    angular.forEach($scope.headlst, function (val) {
        //    //        if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
        //    //            angular.forEach($scope.installlst, function (val1) {
        //    //                if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
        //    //                    val1.checkedinstallmentlst = false;
        //    //                }
        //    //            });
        //    //        }
        //    //    });
        //    //}

        //    for (var s = 0; s < $scope.grouplst.length; s++) {

        //        if (vlobj1.fmgG_Id == $scope.grouplst[s].fmgG_Id) {
        //            for (var t = 0; t < $scope.termlist.length; t++) {
        //                if (vlobj1.fmgG_Id == $scope.termlist[t].fmgG_Id) {
        //                    if ($scope.termlist[t].checkedheadlst == false) {
        //                        $scope.grouplst[s].checkedgrplst = false;
        //                    }
        //                    else {
        //                        $scope.grouplst[s].checkedgrplst = true;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        $scope.isOptionsRequired1 = function () {
            return !$scope.grouplist.some(function (options) {
                return options.checkedgrplst;
            });
        }

        //checkbox


        $scope.toggleAll = function (allchkdata) {
            $scope.disablefine = true;
            $scope.disablenetamount = true;

            var toggleStatus = $scope.all;

            $scope.curramount = 0;
            $scope.totalconcession = 0;
            $scope.totalfine = 0;
            $scope.totalwaived = 0;

            angular.forEach($scope.students, function (itm) {
                itm.isSelected = toggleStatus;
            });

            if (allchkdata == true) {

                $scope.SHOWTOTALPAYABL = true;

                for (var index = 0; index < $scope.students.length; index++) {
                    $scope.totalconcession = Number($scope.totalconcession) + Number($scope.students[index].fcsS_ConcessionAmount);
                    $scope.totalfine = Number($scope.totalfine) + Number($scope.students[index].fcsS_FineAmount);
                    $scope.curramount = Number($scope.curramount) + Number($scope.students[index].fcsS_ToBePaid) + $scope.totalfine;
                    $scope.totalwaived = Number($scope.totalwaived) + Number($scope.students[index].fcsS_WaivedAmount);

                }
                $scope.totalamt = Number($scope.curramount)+ Number($scope.totalfine);
            }
            else {

                $scope.SHOWTOTALPAYABL = false;

                $scope.totalconcession = 0;
                $scope.totalfine = 0;
                $scope.curramount = 0;
                $scope.totalwaived = 0;
                $scope.totalamt = 0;
            }
        }


        $scope.curramount = 0;
        $scope.amtdetails = function (userdata, students, index, totalamt) {

            $scope.disablefine = true;
            $scope.disablenetamount = true;

            $scope.all = $scope.students.every(function (itm) {
                return itm.isSelected;
            });

            if (students[index].isSelected == true) {

                $scope.SHOWTOTALPAYABL = true;

                $scope.totalconcession = Number($scope.totalconcession) + Number(students[index].FCSS_ConcessionAmount);
                $scope.totalfine = Number($scope.totalfine) + Number(students[index].FCSS_FineAmount);
                $scope.curramount = Number($scope.curramount) + Number(students[index].FCSS_ToBePaid) + Number(students[index].FCSS_FineAmount);
                $scope.totalwaived = Number($scope.totalwaived) + Number(students[index].FCSS_WaivedAmount);
                if (students[index].fmH_FeeName != "Fine") {
                }
                $scope.totalamt = $scope.curramount;
            }
            else if (students[index].isSelected == false) {
                $scope.totalconcession = Number($scope.totalconcession) - Number(students[index].FCSS_ConcessionAmount);
                $scope.totalfine = Number($scope.totalfine) - Number(students[index].FCSS_FineAmount);
                $scope.curramount = Number($scope.curramount) - Number(students[index].FCSS_ToBePaid) - Number(students[index].FCSS_FineAmount);
                $scope.totalwaived = Number($scope.totalwaived) - Number(students[index].FCSS_WaivedAmount);

                if (students[index].fmH_FeeName != "Fine") {
                }
                $scope.totalamt = $scope.curramount;
            }
        };


        $scope.onclickloaddata = function (totalamt, students, checkbx, yearlst, customfeegroup, qwe, totalgridadvance) {

       // $scope.onclickloaddata = function (totalamt, students, checkbx, yearlst, customfeegroup, qwe) {
            $scope.gateway = false;

            $scope.disableall = true;
            $scope.disableindividual = true;

            if (qwe.paygtw === "RAZORPAY") {
               // $scope.pamentsave(totalamt, students, checkbx, yearlst, customfeegroup, qwe);
                $scope.pamentsave(totalamt, students, checkbx, yearlst, customfeegroup, qwe, totalgridadvance);

            }

        }

        $scope.onclickloaddataregular = function (paymenttype) {
            //var studid = 400;

            if (paymenttype == true) {
                var regularval = 'R';
                var data = {
                    "Paymenttype": regularval,
                    // "Amst_Id": studid,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeOnlinePayment/getgrouportermdetails", data).
                    then(function (promise) {

                        $scope.yearlst = promise.fillinstallment
                        $scope.showpaymentdetails = true;

                    })
            }

        }

        $scope.getcustomgroups = function (termid, yearlst) {

            var gouportermcount1 = "0";
            for (var i = 0; i < yearlst.length; i++) {
                name = $scope.yearlst[i].selected
                if (name == "true") {
                    gouportermcount1 = gouportermcount1 + ',' + yearlst[i].fmG_Id;
                }
            }

            if ($scope.checkboxval != 'undefined') {
                var data = {
                    "ftiidss": gouportermcount1,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeOnlinePayment/getcustomgroups", data).
                    then(function (promise) {

                        $scope.customdisplay = true;
                        $scope.customfeegroup = promise.customgroup;

                    })
            }
        }

        $scope.onclickloaddatatransport = function (paymenttype) {
            //var studid = 400;

            if (paymenttype == true) {
                var transportval = 'T';

                var data = {
                    "Paymenttype": transportval,
                    // "Amst_Id": studid,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeOnlinePayment/getgrouportermdetails", data).
                    then(function (promise) {

                        $scope.translabel = "Transport ";

                        $scope.yearlst1 = promise.transportopted
                        $scope.showpaymentdetails = true;

                    })
            }
            else {
                $scope.transportdisplay = false;
            }
        }


        $scope.cancel = function () {
            $state.reload();
        }

        $scope.temptermarray = [];
        var ftiids = 0;
        var remove_list = [];
        var ins_spe_list = [];
        $scope.optionToggled = function (grouplist) {

            var groupfmgids = 0;
            var checkfmgid = 0;
            angular.forEach(grouplist, function (grp_trm) {
                if (grp_trm.checkedgrplst) {
                    groupfmgids = groupfmgids + ',' + grp_trm.fmG_Id
                    checkfmgid = checkfmgid + 1;
                }
            })



            var selected_list = [];
            angular.forEach($scope.grp_ins_list, function (grp_trm) {
                if (grp_trm.grp.checkedgrplst) {
                    var term_list = [];
                    angular.forEach(grp_trm.installmentlist, function (trn) {
                        if (trn.checkedheadlst) {
                            term_list.push(trn);
                        }
                    })
                    selected_list.push({ grp: grp_trm.grp, trm_list: term_list });
                }

            })

            angular.forEach($scope.grp_ins_list, function (grp_trm) {
                grp_trm.grpdisable = true;
                angular.forEach(grp_trm.installmentlist, function (trn) {
                    trn.termdisablechk = true;
                })
            })



            if (checkfmgid >= 1) {
                var data = {
                    //"ASMAY_Id": $scope.ASMAY_Id,
                    "groupfmgidss": groupfmgids,
                    "FYP_ReceiptDate": new Date().toDateString(),
                    "AMCO_Id": $scope.amcO_Id,
                    selected_listgroup: selected_list,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeFeeOnlinePayment/getheaddetails", data).
                    then(function (promise) {

                        $scope.configFeesetting=promise.feeconfiglist;

                        if ($scope.configFeesetting[0].fmC_EnablePartialPaymentFlg == 0 || $scope.configFeesetting[0].fmC_EnablePartialPaymentFlg == null ) {
                            $scope.totalamtdisable = true;
                        }
                        else if ($scope.configFeesetting[0].fmC_EnablePartialPaymentFlg == 1) {
                            $scope.totalamtdisable = false;
                        }
                        else if ($scope.configFeesetting[0].fmC_EnablePartialPaymentFlg == 2) {
                            if (promise.studwisepartialpayment.length != null && promise.studwisepartialpayment.length > 0) {
                                $scope.totalamtdisable = false;
                            }
                            else {
                                $scope.totalamtdisable = true;
                            }

                            
                        }

                        
                     
                        $scope.SHOWTOTALPAYABL = true;

                        $scope.paymenttest = promise.fillpaymentgateway;

                        if (promise.alldata.length > 0) {
                            $scope.showpaymentdetails1 = true;
                            $scope.students = promise.alldata;

                            angular.forEach(promise.finearray, function (objec) {
                                $scope.students.push(objec);
                            })
                        }

                        else {
                            $scope.showpaymentdetails1 = false;
                            $scope.students = {};
                            swal("Amount is Paid for the selected Term and Group");
                        }

                        //added//
                        if (promise.advancefee != null) {
                            if (promise.advancefee.length > 0) {
                                $scope.totalgridadvance = promise.advancefee;

                            }
                            else {
                                $scope.totalgridadvance = [];
                            }

                        }
                        else {
                            $scope.totalgridadvance = [];
                        }

                        if (promise.fine_FCMAS_IdsAdvance.length > 0) {

                            $scope.temp_Fine_Amountsadv = promise.fine_FCMAS_IdsAdvance;

                        }

                        //added//

                        angular.forEach(grouplist, function (trn) {
                            trn.grpdisable = true;
                        })

                        var chkdt = true;
                        $scope.all = true;
                        $scope.toggleAll(chkdt);

                    })
            }
            else {
                $scope.showpaymentdetails1 = false;
                swal("Kindly select atleast one");
                $state.reload();
            }
        }

        $scope.showpaymentdetails1 = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5

            var data = {
                "configset": grouporterm,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeFeeOnlinePayment/getloaddata", data).
                then(function (promise) {

                    if (promise.filonlinepaymentgrid.length > 0) {

                        $scope.grouplist = promise.fillgroup;
                        $scope.fillinstallment = promise.fillinstallmentnew;

                        $scope.grp_ins_list = [];
                       
                        angular.forEach($scope.grouplist, function (grp) {
                            var installmentlist = [];
                            var groupg = [];
                            console.log(grp.fmG_CompulsoryFlag);
                            angular.forEach($scope.fillinstallment, function (trn) {
                                if (trn.fmG_Id == grp.fmG_Id) {
                                    installmentlist.push(trn);
                                }
                            })
                           
                            $scope.grp_ins_list.push({ grp: grp, installmentlist: installmentlist });
                        })

                        for (var i = 0; i < promise.disableterms.length; i++) {
                            for (var j = 0; j < $scope.grp_ins_list.length; j++) {

                                for (var k = 0; k < $scope.grp_ins_list[j].installmentlist.length; k++) {

                                    if (promise.disableterms[i].fmT_Id == $scope.grp_ins_list[j].installmentlist[k].fmG_Id && promise.disableterms[i].ftI_Id == $scope.grp_ins_list[j].installmentlist[k].ftI_Id) {
                                        //
                                        if ($scope.grp_ins_list[j].grp.fmG_CompulsoryFlag != 'C') {
                                            if (promise.disableterms[i].fcsS_ToBePaid > 0) {
                                                $scope.grp_ins_list[j].installmentlist[k].termdisablechk = false;
                                            }
                                            else {
                                                $scope.grp_ins_list[j].installmentlist[k].termdisablechk = true;
                                            }
                                        }
                                        else {

                                            if (promise.disableterms[i].fcsS_ToBePaid > 0) {
                                                $scope.grp_ins_list[j].grp.checkedgrplst = true;
                                                $scope.grp_ins_list[j].grpdisable = true;
                                                
                                                $scope.grp_ins_list[j].installmentlist[k].checkedheadlst = true;
                                                $scope.grp_ins_list[j].installmentlist[k].termdisablechk = true;

                                            }

                                        }
                                    }
                                }
                            }
                        }

                        angular.forEach($scope.grp_ins_list, function (obj) {
                            obj.grpdisable = obj.installmentlist.every(function (options) {
                                return options.termdisablechk;
                            });
                        })



                        $scope.paymenttestt = promise.fillpaymentgateway;
                        $scope.paymenttesttteresian = promise.fillpaymentgateway;

                        if (promise.fillstudent.length > 0) {
                            $scope.amcsT_FirstName = promise.fillstudent[0].amcsT_FirstName
                            $scope.amcsT_MiddleName = promise.fillstudent[0].amcsT_MiddleName;
                            $scope.amcsT_LastName = promise.fillstudent[0].amcsT_LastName

                            $scope.amsT_fullanme = $scope.AMCST_FirstName
                            $scope.amcsT_AdmNo = promise.fillstudent[0].amcsT_AdmNo
                            $scope.amcsT_RegistrationNo = promise.fillstudent[0].amcsT_RegistrationNo
                            $scope.amaY_RollNo = promise.fillstudent[0].acysT_RollNo;
                            $scope.coursename = promise.fillstudent[0].amcO_CourseName;
                            $scope.branchname = promise.fillstudent[0].amB_BranchName;
                            $scope.semestername = promise.fillstudent[0].amsE_SEMName;
                            $scope.sectionname = promise.fillstudent[0].acmS_SectionName;

                            $scope.amcst_mobile = promise.fillstudent[0].amcsT_MobileNo;
                            $scope.amcst_email_id = promise.fillstudent[0].amcsT_emailId;

                            $scope.amcsT_Id = promise.fillstudent[0].amcsT_Id;
                            $scope.amcO_Id = promise.fillstudent[0].amcO_Id;
                            $scope.amB_Id = promise.fillstudent[0].amB_Id;
                            $scope.amsE_Id = promise.fillstudent[0].amsE_Id;
                            $scope.acmS_Id = promise.fillstudent[0].acmS_Id;
                            $scope.amcsT_StudentPhoto = promise.fillstudent[0].amcsT_StudentPhoto;

                            // $scope.feedisplayconfig = "ND";
                            $scope.feedisplayconfig = promise.feeconfiglist[0].fmC_DetailedDisplayFlg;
                        }

                        if (promise.filonlinepaymentgrid.length > 0) {
                            for (var i = 0; i < promise.filonlinepaymentgrid.length; i++) {

                                netamount = Number(netamount) + Number(promise.filonlinepaymentgrid[i].fcsS_NetAmount)
                                concessionamount = Number(concessionamount) + Number(promise.filonlinepaymentgrid[i].fcsS_ConcessionAmount)
                                fineamount = Number(fineamount) + Number(promise.filonlinepaymentgrid[i].fcsS_FineAmount)
                                payableamountamount = Number(payableamountamount) + Number(promise.filonlinepaymentgrid[i].fcsS_ToBePaid)

                                paidamount = Number(paidamount) + Number(promise.filonlinepaymentgrid[i].fcsS_PaidAmount)
                            }

                            $scope.totalnetamount = netamount;
                            $scope.totalconcessionn = concessionamount;
                            $scope.totalbalance = payableamountamount;
                            $scope.totalpaidamount = paidamount;
                        }
                    }
                    else {
                        $scope.showbasicdetails = false;
                        $scope.showpaymentdetails = false;
                        $scope.showpaymentdetails1 = false;
                        swal("Contact Administrator!!!!!")
                    }

                    if (promise.institutiondet.length > 0) {
                        for (var i = 0; i < promise.institutiondet.length; i++) {
                            $scope.institutioname = promise.institutiondet[0].inT_NAME;
                            $scope.institulogo = promise.institutiondet[0].institutioN_LOGO;
                        }
                    }

                    if (grouporterm == 'T') {
                        var termsdisable = promise.disableterms;
                        if ($scope.grouplist.length == promise.disableterms.length) {
                            for (var r = 0; r < $scope.grouplist.length; r++) {
                                if (promise.grouplist[r].fcsS_NetAmount <= promise.disableterms[r].fcsS_PaidAmount && $scope.grouplist[r].fmG_Id == promise.disableterms[r].fmT_Id) {
                                    $scope.grouplist[r].grpdisable = true;

                                    $scope.grouplist[r].checkedgrplst = false;
                                }
                                else {
                                    $scope.grouplist[r].grpdisable = false;
                                    $scope.grouplist[r].checkedgrplst = false;
                                }
                            }
                        }
                    }
                    else if (grouporterm == 'G') {
                        var termsdisable = promise.disableterms;
                        if ($scope.grouplist.length == promise.disableterms.length) {
                            for (var r = 0; r < $scope.grouplist.length; r++) {
                                if (promise.grouplist[r].fcsS_NetAmount <= promise.disableterms[r].fcsS_PaidAmount && $scope.grouplist[r].fmG_Id == promise.disableterms[r].fmT_Id) {
                                    $scope.grouplist[r].grpdisable = true;

                                    $scope.grouplist[r].checkedgrplst = false;
                                }
                                else {
                                    $scope.grouplist[r].grpdisable = false;
                                    $scope.grouplist[r].checkedgrplst = false;
                                }
                            }
                        }
                    }
                })

            $scope.sort = function (keyname) {
                // $scope.sortKey = keyname;   //set the sortKey to the param passed
                //  $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };


        $scope.onselectacademic = function () {

            netamount = 0
            concessionamount = 0
            fineamount = 0
            payableamountamount = 0
            paidamount = 0

            if ($scope.ASMAY_Id != undefined) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "configset": grouporterm
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeOnlinePayment/getalldetails", data).
                    then(function (promise) {

                        if (promise.customgrplist.length > 0) {
                            if (promise.transnumconfig.length > 0) {
                                automanualreceiptnotranum = promise.transnumconfig[0].imN_AutoManualFlag;
                            }

                            $scope.grouplst = promise.customgrplist;
                            $scope.termlist = promise.termlst;

                            angular.forEach($scope.grouplst, function (trn) {
                                trn.checkedgrplst = false;
                            })

                            angular.forEach($scope.termlist, function (trn) {
                                trn.checkedheadlst = false;
                            })

                            $scope.termsdisable = promise.disableterms;

                            $scope.temp_grp_ins_list = [];
                            angular.forEach($scope.grouplst, function (grp) {
                                var term_list = [];
                                angular.forEach($scope.termlist, function (trn) {
                                    if (trn.fmgG_Id == grp.fmgG_Id) {
                                        term_list.push(trn);
                                    }
                                })
                                $scope.temp_grp_ins_list.push({ grp: grp, trm_list: term_list });
                            })

                            for (var i = 0; i < promise.disableterms.length; i++) {
                                for (var j = 0; j < $scope.temp_grp_ins_list.length; j++) {

                                    for (var k = 0; k < $scope.temp_grp_ins_list[j].trm_list.length; k++) {

                                        if (promise.disableterms[i].FMGG_Id == $scope.temp_grp_ins_list[j].trm_list[k].fmgG_Id && promise.disableterms[i].fmt_id == $scope.temp_grp_ins_list[j].trm_list[k].fmT_Id) {
                                            if (promise.disableterms[i].payable <= promise.disableterms[i].paid) {
                                                $scope.temp_grp_ins_list[j].trm_list[k].termdisablechk = true;
                                            }
                                            else {
                                                $scope.temp_grp_ins_list[j].trm_list[k].termdisablechk = false;
                                            }
                                        }
                                    }
                                }

                            }

                            angular.forEach($scope.temp_grp_ins_list, function (obj) {
                                obj.grpdisable = obj.trm_list.every(function (options) {
                                    return options.termdisablechk;
                                });
                            })

                            $scope.showpaymentdetails = true
                            $scope.checkboxvalregular = true;
                            $scope.yearlst = promise.fillinstallment;

                            if (promise.fillstudent.length > 0) {
                                $scope.amsT_FirstName = promise.fillstudent[0].amsT_FirstName
                                $scope.amsT_MiddleName = promise.fillstudent[0].amsT_MiddleName;
                                $scope.amsT_LastName = promise.fillstudent[0].amsT_LastName

                                $scope.amsT_fullanme = $scope.AMST_FirstName
                                $scope.amsT_AdmNo = promise.fillstudent[0].amsT_AdmNo
                                $scope.amsT_RegistrationNo = promise.fillstudent[0].amsT_RegistrationNo
                                $scope.amaY_RollNo = promise.fillstudent[0].amaY_RollNo;
                                $scope.classname = promise.fillstudent[0].classname;
                                $scope.sectionname = promise.fillstudent[0].sectionname;

                                $scope.amst_mobile = promise.fillstudent[0].amst_mobile;
                                $scope.amst_email_id = promise.fillstudent[0].amst_email_id;

                                $scope.amst_Id = promise.fillstudent[0].amst_Id;
                                $scope.asmcL_ID = promise.fillstudent[0].asmcL_ID;
                            }

                            if (promise.filonlinepaymentgrid.length > 0) {
                                for (var i = 0; i < promise.filonlinepaymentgrid.length; i++) {

                                    netamount = Number(netamount) + Number(promise.filonlinepaymentgrid[i].fsS_NetAmount)
                                    concessionamount = Number(concessionamount) + Number(promise.filonlinepaymentgrid[i].fsS_ConcessionAmount)
                                    fineamount = Number(fineamount) + Number(promise.filonlinepaymentgrid[i].fsS_FineAmount)
                                    payableamountamount = Number(payableamountamount) + Number(promise.filonlinepaymentgrid[i].fsS_ToBePaid)

                                    paidamount = Number(paidamount) + Number(promise.filonlinepaymentgrid[i].fsS_PaidAmount)
                                }

                                $scope.totalnetamount = netamount;
                                $scope.totalconcessionn = concessionamount;
                                $scope.totalbalance = payableamountamount;
                                $scope.totalpaidamount = paidamount;
                            }

                            $scope.showbasicdetails = true;
                            $scope.showpaymentdetails = true;
                            $scope.showpaymentdetails1 = false;
                        }
                        else {
                            $scope.showbasicdetails = false;
                            $scope.showpaymentdetails = false;
                            $scope.showpaymentdetails1 = false;
                            swal("For Selected Academic Year there is no Pending Payment!!Select Another Academic Year")
                        }
                    })
            }
            else {
                $scope.showbasicdetails = false;
                $scope.showpaymentdetails = false;
                $scope.showpaymentdetails1 = false;
                swal("Kindly Select Academic Year!!!!")

            }
        }

        $scope.checktermdetails = false;
        $scope.temptermarray = [];
        $scope.selectterm = function (insid) {

            $scope.showpaymentdetails = true;

            var termid = insid;
            var data = {
                "FMT_Id": termid,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var netamount = 0
            var concessionamount = 0
            var fineamount = 0
            var payableamountamount = 0
            var paidamount = 0
            apiService.create("FeeOnlinePayment/getamountdetails", data).
                then(function (promise) {


                    $scope.checktermdetails = true;

                    if (promise.fillstudentviewdetails.length > 0) {
                        $scope.showpaymentdetails = true;
                        $scope.temptermarray.push(promise.fillstudentviewdetails);

                        $scope.students = promise.fillstudentviewdetails;

                        for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {

                            netamount = Number(netamount) + Number(promise.fillstudentviewdetails[i].fsS_NetAmount);
                            concessionamount = Number(concessionamount) + Number(promise.fillstudentviewdetails[i].fsS_ConcessionAmount);
                            fineamount = Number(fineamount) + Number(promise.fillstudentviewdetails[i].fsS_FineAmount);
                            payableamountamount = Number(payableamountamount) + Number(promise.fillstudentviewdetails[i].fsS_ToBePaid);

                            paidamount = Number(netamount) - +Number(promise.fillstudentviewdetails[i].fsS_PaidAmount);

                            payableamountamount = payableamountamount - concessionamount + fineamount;
                        }

                        $scope.totalamt = Number(payableamountamount) + Number(fineamount);

                        $scope.key = promise.paydet[0].marchanT_ID;
                        $scope.txnid = promise.paydet[0].trans_id;
                        $scope.amount = promise.paydet[0].amount;


                        $scope.productinfo = promise.paydet[0].productinfo;
                        $scope.firstname = promise.paydet[0].firstname;
                        $scope.email = promise.paydet[0].email;
                        $scope.phone = promise.paydet[0].phone;
                        $scope.surl = promise.paydet[0].surl;
                        $scope.furl = promise.paydet[0].furl;
                        $scope.hash = promise.paydet[0].hash;
                        $scope.udf1 = promise.paydet[0].udf1;
                        $scope.udf2 = promise.paydet[0].udf2;
                        $scope.udf3 = promise.paydet[0].udf3;
                        $scope.udf4 = promise.paydet[0].udf4;
                        $scope.udf5 = promise.paydet[0].udf5;
                        $scope.udf6 = promise.paydet[0].udf6;
                        $scope.service_provider = promise.paydet[0].service_provider;

                        $scope.hash_string = promise.paydet[0].hash_string;
                        $scope.payu_URL = promise.paydet[0].payu_URL;


                    }
                    else {
                        swal("Amount is already Paid for selected Term");
                        $scope.showpaymentdetails = false;
                    }
                })
        };

        $scope.clickimage = function (radioimage) {
            angular.forEach($scope.paymenttest, function (objectt) {
                if (objectt.fpgD_Image == radioimage) {
                    $scope.qwe.paygtw = objectt.fpgD_PGName;
                    //$scope.onclickloaddata = function (totalamt, students, checkbx, yearlst, customfeegroup, qwe) {
                    $scope.onclickloaddata($scope.totalamt, $scope.students, 1, 1, 1, $scope.qwe);
                    return;
                }
            });
        };

        $scope.clickimagedisplay = function (radioimage, totalamtdisplay) {
            angular.forEach($scope.paymenttestt, function (objectt) {
                if (objectt.fpgD_Image == radioimage) {
                    $scope.qwe.paygtw = objectt.fpgD_PGName;
                    //$scope.onclickloaddata = function (totalamt, students, checkbx, yearlst, customfeegroup, qwe) {
                    $scope.onclickloaddatadisplay(totalamtdisplay, $scope.students, 1, $scope.qwe);
                    return;
                }
            });
        };

        $scope.onclickloaddatadisplay = function (totalamtdisplay, students, yearlst, qweteresian) {
            $scope.gateway = false;

            $scope.disableall = true;
            $scope.disableindividual = true;

            if (qweteresian.paygtwteresian === "RAZORPAY") {
              //  $scope.pamentsaveDisplay(totalamtdisplay, students, yearlst, qweteresian);
                $scope.pamentsaveDisplay(totalamtdisplay, students, yearlst, qweteresian, totalgridadvance);

            }
        }
        

       // $scope.pamentsaveDisplay = function (totalamtdisplay, students, yearlst, qweteresian) {
        $scope.pamentsaveDisplay = function (totalamtdisplay, students, yearlst, qweteresian, totalgridadvance) {

            if (totalamtdisplay != undefined) {
                if (totalamtdisplay != "") {
                    if ($scope.amst_mobile != 0 || $scope.amst_email_id != "") {

                        if (payableamountamount >= totalamtdisplay) {

                            var data = {
                                "topayamount": totalamtdisplay,
                                "AMCST_Id": $scope.amcsT_Id,
                                "onlinepaygteway": qweteresian.paygtwteresian,
                                "AMCO_Id": $scope.amcO_Id,
                                "AMB_Id": $scope.amB_Id,
                                "AMSE_Id": $scope.amsE_Id,
                                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,

                            }

                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            }


                          

                                apiService.create("CollegeFeeOnlinePayment/generatehashsequencedisplay", data).
                                    then(function (promise) {
                                        if (qweteresian.paygtwteresian === "PAYU") {

                                            $scope.key = promise.paydet[0].marchanT_ID;
                                            $scope.txnid = promise.paydet[0].trans_id;
                                            $scope.amount = promise.paydet[0].amount;

                                            $scope.productinfo = promise.paydet[0].productinfo;
                                            $scope.firstname = promise.paydet[0].firstname;
                                            $scope.email = promise.paydet[0].email;
                                            $scope.phone = promise.paydet[0].phone;
                                            $scope.surl = promise.paydet[0].surl;
                                            $scope.furl = promise.paydet[0].furl;
                                            $scope.hash = promise.paydet[0].hash;
                                            $scope.udf1 = promise.paydet[0].udf1;
                                            $scope.udf2 = promise.paydet[0].udf2;
                                            $scope.udf3 = promise.paydet[0].udf3;
                                            $scope.udf4 = promise.paydet[0].udf4;
                                            $scope.udf5 = promise.paydet[0].udf5;
                                            $scope.udf6 = promise.paydet[0].udf6;
                                            $scope.service_provider = promise.paydet[0].service_provider;

                                            $scope.hash_string = promise.paydet[0].hash_string;
                                            $scope.payu_URL = promise.paydet[0].payu_URL;

                                            var payu_URL = $scope.payu_URL;
                                            var url = payu_URL;
                                            var method = 'POST';
                                            var params = {
                                                "key": $scope.key,
                                                "txnid": $scope.txnid,
                                                "amount": $scope.amount,
                                                "productinfo": $scope.productinfo,
                                                "firstname": $scope.firstname,
                                                "email": $scope.email,
                                                "phone": $scope.phone,
                                                "udf1": $scope.udf1,
                                                "udf2": $scope.udf2,
                                                "udf3": $scope.udf3,
                                                "udf4": $scope.udf4,
                                                "udf5": $scope.udf5,
                                                "udf6": $scope.udf6,
                                                "service_provider": $scope.service_provider,
                                                "hash": promise.paydet[0].hash,
                                                "surl": "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponse/",
                                                "furl": "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponse/"
                                            }
                                            FormSubmitter.submit(url, method, params);
                                        }
                                        else if (qweteresian.paygtwteresian === "PAYTM") {

                                            $scope.MID = promise.paydet[0].mid;
                                            $scope.ORDER_ID = promise.paydet[0].ordeR_ID;
                                            $scope.CUST_ID = promise.paydet[0].cusT_ID;
                                            $scope.TXN_AMOUNT = promise.paydet[0].txN_AMOUNT;
                                            $scope.CHANNEL_ID = promise.paydet[0].channeL_ID;
                                            $scope.EMAIL = promise.paydet[0].email;
                                            $scope.MOBILE_NO = promise.paydet[0].mobilE_NO;
                                            $scope.INDUSTRY_TYPE_ID = promise.paydet[0].industrY_TYPE_ID;
                                            $scope.WEBSITE = promise.paydet[0].website;
                                            $scope.CHECKSUMHASH = promise.paydet[0].checksumhash;
                                            $scope.MERC_UNQ_REF = promise.paydet[0].merC_UNQ_REF;

                                            $scope.payu_URL = promise.paydet[0].payu_URL;
                                            var payu_URL = $scope.payu_URL;

                                            var url = payu_URL;
                                            var method = 'POST';
                                            var params = {
                                                "MID": $scope.MID,
                                                "ORDER_ID": $scope.ORDER_ID,
                                                "CUST_ID": $scope.CUST_ID,
                                                "TXN_AMOUNT": $scope.TXN_AMOUNT,
                                                "CHANNEL_ID": $scope.CHANNEL_ID,
                                                "EMAIL": $scope.EMAIL,
                                                "MOBILE_NO": $scope.MOBILE_NO,
                                                "INDUSTRY_TYPE_ID": $scope.INDUSTRY_TYPE_ID,
                                                "WEBSITE": $scope.WEBSITE,
                                                "CHECKSUMHASH": $scope.CHECKSUMHASH,
                                                "MERC_UNQ_REF": $scope.MERC_UNQ_REF,
                                                "CALLBACK_URL": "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponsePAYTM/",
                                            }
                                            FormSubmitter.submit(url, method, params);
                                        }

                                        else if (qweteresian.paygtwteresian == "RAZORPAY") {

                                            $scope.txnamt = Number(totalamtdisplay) * 100;
                                            $scope.SaltKey = promise.merchantkey;
                                            $scope.orderid = promise.trans_id;

                                            $scope.institutioname = $scope.institutioname;
                                            $scope.institulogo = $scope.institulogo; /*"https://dcampusstrg.blob.core.windows.net/mobileimage/TRSN/logo.png";*/

                                            $scope.stuname = promise.fillstudent[0].amcsT_FirstName;
                                            $scope.stuemailid = promise.fillstudent[0].amcsT_emailId;
                                            $scope.stuaddress = promise.fillstudent[0].amsE_SEMName;
                                            $scope.stumobileno = promise.fillstudent[0].amcsT_MobileNo;
                                            $scope.stuadmno = promise.fillstudent[0].amcsT_AdmNo;
                                            $scope.splitpayinfor = promise.splitpayinformation;

                                            $scope.mI_Id = promise.mI_Id;
                                            $scope.asmaY_Id = promise.asmaY_Id;
                                            $scope.amcst_Id = promise.amcsT_Id;

                                        }

                                        else if (qweteresian.paygtwteresian == "EASEBUZZ") {
                                            var strdata = promise.strForm;
                                            $scope.htmldata = promise.strForm;
                                            var e1 = angular.element(document.getElementById("test"));
                                            $compile(e1.html($scope.htmldata))(($scope));
                                        }

                                    });
                           

                        }
                        else {
                            swal("Entered Amount is greater than Payable amount");
                        }
                    }
                    else {
                        swal("Email-Id / Mobile No is Missing!!!! So You cannot proceed with Payment");
                    }
                }
                else {
                    swal("Kindly Enter Amount");
                }

            }
            else {
                swal("Kindly Enter Amount");
            }
        }

        $scope.paydetails = false;
        var goupid = "0";
     //   $scope.pamentsave = function (totalamt, students, checkbx, yearlst, customfeegroup, qwe) {
        $scope.pamentsave = function (totalamt, students, checkbx, yearlst, customfeegroup, qwe, totalgridadvance) {

            if (totalamt != undefined || totalamt != 0) {
                if ($scope.amst_mobile != 0 || $scope.amst_email_id != "") {
                    var selected_list = [];
                    var selected_listadvance = [];
                    angular.forEach($scope.students, function (grp_trm) {
                        if (grp_trm.isSelected) {
                            selected_list.push(grp_trm);
                        }
                    })
                    angular.forEach($scope.totalgridadvance, function (grp_trm) {
                        if (grp_trm.isSelected1) {
                            selected_listadvance.push(grp_trm);
                        }
                    })

                    angular.forEach(selected_list, function (obj) {
                        obj.fcmaS_DueDate = new Date();


                    })
                    angular.forEach(selected_listadvance, function (obj) {
                        obj.fcmaS_DueDate = new Date();


                    })


                    var selected_listgroup = [];
                    angular.forEach($scope.grp_ins_list, function (grp_trm) {
                        if (grp_trm.grp.checkedgrplst) {
                            var term_list = [];
                            angular.forEach(grp_trm.installmentlist, function (trn) {
                                if (trn.checkedheadlst) {
                                    term_list.push(trn);
                                }
                            })
                            selected_listgroup.push({ grp: grp_trm.grp, trm_list: term_list });
                        }

                    })

                    angular.forEach($scope.grp_ins_list, function (grp_trm) {
                        grp_trm.grpdisable = true;
                        angular.forEach(grp_trm.installmentlist, function (trn) {
                            trn.termdisablechk = true;
                        })
                    })


                    var data = {
                        "topayamount": totalamt,
                        "AMCST_Id": $scope.amcsT_Id,
                        selected_list: selected_list,
                        "onlinepaygteway": qwe.paygtw,
                        "AMCO_Id": $scope.amcO_Id,
                        "AMB_Id": $scope.amB_Id,
                        "AMSE_Id": $scope.amsE_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                        advancedata: selected_listadvance,
                        selected_listgroup: selected_listgroup
                    }

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("CollegeFeeOnlinePayment/generatehashsequence", data).
                        then(function (promise) {
                            if (qwe.paygtw === "PAYU") {

                                $scope.key = promise.paydet[0].marchanT_ID;
                                $scope.txnid = promise.paydet[0].trans_id;
                                $scope.amount = promise.paydet[0].amount;

                                $scope.productinfo = promise.paydet[0].productinfo;
                                $scope.firstname = promise.paydet[0].firstname;
                                $scope.email = promise.paydet[0].email;
                                $scope.phone = promise.paydet[0].phone;
                                $scope.surl = promise.paydet[0].surl;
                                $scope.furl = promise.paydet[0].furl;
                                $scope.hash = promise.paydet[0].hash;
                                $scope.udf1 = promise.paydet[0].udf1;
                                $scope.udf2 = promise.paydet[0].udf2;
                                $scope.udf3 = promise.paydet[0].udf3;
                                $scope.udf4 = promise.paydet[0].udf4;
                                $scope.udf5 = promise.paydet[0].udf5;
                                $scope.udf6 = promise.paydet[0].udf6;
                                $scope.service_provider = promise.paydet[0].service_provider;

                                $scope.hash_string = promise.paydet[0].hash_string;
                                $scope.payu_URL = promise.paydet[0].payu_URL;

                                var payu_URL = $scope.payu_URL;
                                var url = payu_URL;
                                var method = 'POST';
                                var params = {
                                    "key": $scope.key,
                                    "txnid": $scope.txnid,
                                    "amount": $scope.amount,
                                    "productinfo": $scope.productinfo,
                                    "firstname": $scope.firstname,
                                    "email": $scope.email,
                                    "phone": $scope.phone,
                                    "udf1": $scope.udf1,
                                    "udf2": $scope.udf2,
                                    "udf3": $scope.udf3,
                                    "udf4": $scope.udf4,
                                    "udf5": $scope.udf5,
                                    "udf6": $scope.udf6,
                                    "service_provider": $scope.service_provider,
                                    "hash": promise.paydet[0].hash,
                                    "surl": "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponse/",
                                    "furl": "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponse/"
                                }
                                FormSubmitter.submit(url, method, params);
                            }
                            else if (qwe.paygtw === "PAYTM") {

                                $scope.MID = promise.paydet[0].mid;
                                $scope.ORDER_ID = promise.paydet[0].ordeR_ID;
                                $scope.CUST_ID = promise.paydet[0].cusT_ID;
                                $scope.TXN_AMOUNT = promise.paydet[0].txN_AMOUNT;
                                $scope.CHANNEL_ID = promise.paydet[0].channeL_ID;
                                $scope.EMAIL = promise.paydet[0].email;
                                $scope.MOBILE_NO = promise.paydet[0].mobilE_NO;
                                $scope.INDUSTRY_TYPE_ID = promise.paydet[0].industrY_TYPE_ID;
                                $scope.WEBSITE = promise.paydet[0].website;
                                $scope.CHECKSUMHASH = promise.paydet[0].checksumhash;
                                $scope.MERC_UNQ_REF = promise.paydet[0].merC_UNQ_REF;

                                $scope.payu_URL = promise.paydet[0].payu_URL;
                                var payu_URL = $scope.payu_URL;

                                var url = payu_URL;
                                var method = 'POST';
                                var params = {
                                    "MID": $scope.MID,
                                    "ORDER_ID": $scope.ORDER_ID,
                                    "CUST_ID": $scope.CUST_ID,
                                    "TXN_AMOUNT": $scope.TXN_AMOUNT,
                                    "CHANNEL_ID": $scope.CHANNEL_ID,
                                    "EMAIL": $scope.EMAIL,
                                    "MOBILE_NO": $scope.MOBILE_NO,
                                    "INDUSTRY_TYPE_ID": $scope.INDUSTRY_TYPE_ID,
                                    "WEBSITE": $scope.WEBSITE,
                                    "CHECKSUMHASH": $scope.CHECKSUMHASH,
                                    "MERC_UNQ_REF": $scope.MERC_UNQ_REF,
                                    "CALLBACK_URL": "http://localhost:57606/api/CollegeFeeOnlinePayment/paymentresponsePAYTM/",
                                }
                                FormSubmitter.submit(url, method, params);
                            }

                            else if (qwe.paygtw == "RAZORPAY") {

                                $scope.txnamt = Number(promise.fmA_Amount) * 100;
                                $scope.SaltKey = promise.merchantkey;
                                $scope.orderid = promise.trans_id;

                                $scope.institutioname = $scope.institutioname;
                                $scope.institulogo = "https://dcampusstrg.blob.core.windows.net/mobileimage/TRSN/teresianmobilelogo.png";

                                $scope.stuname = promise.fillstudent[0].amcsT_FirstName;
                                $scope.stuemailid = promise.fillstudent[0].amcsT_emailId;
                                $scope.stuaddress = promise.fillstudent[0].amsE_SEMName;
                                $scope.stumobileno = promise.fillstudent[0].amcsT_MobileNo;
                                $scope.stuadmno = promise.fillstudent[0].amcsT_AdmNo;
                                $scope.splitpayinfor = promise.splitpayinformation;

                                $scope.mI_Id = promise.mI_Id;
                                $scope.asmaY_Id = promise.asmaY_Id;
                                $scope.amcst_Id = promise.amcsT_Id;

                            }
                            else if (qwe.paygtw  == "EASEBUZZ") {
                                var strdata = promise.strForm;
                                $scope.htmldata = promise.strForm;
                                var e1 = angular.element(document.getElementById("test"));
                                $compile(e1.html($scope.htmldata))(($scope));
                            }

                        })


                }
                else {
                    swal("Email-Id / Mobile No is Missing!!!! So You cannot proceed with Payment");
                }
            }
            else {
                swal("Kindly select any one head");
            }


        }
        //added//

        $scope.toggleAllAdvance = function (allchkdata1) {

            var toggleStatus = $scope.adv;
            angular.forEach($scope.totalgridadvance, function (itm) {
                if (toggleStatus == true) {
                    if (itm.FMH_Flag != 'F') {
                        itm.isSelected1 = toggleStatus;
                    }

                }
                else {
                    itm.isSelected1 = toggleStatus;
                }

                
            });

            if (allchkdata1 == true) {

                for (var index = 0; index < $scope.totalgridadvance.length; index++) {
                    // if ($scope.totalgridadvance[index].FMH_Flag != "F") {
                    $scope.totalconcession = Number($scope.totalconcession) + Number($scope.totalgridadvance[index].FCSS_ConcessionAmount);
                    // $scope.totalfine = Number($scope.totalfine) + Number($scope.totalgrid[index].FCSS_FineAmount);
                    $scope.curramount = Number($scope.curramount) + Number($scope.totalgridadvance[index].FCSS_ToBePaid) + Number($scope.totalgridadvance[index].FCSS_OBArrearAmount);
                    $scope.totalwaived = Number($scope.totalwaived) + Number($scope.totalgridadvance[index].FCSS_WaivedAmount);
                    //}
                    advance = 0;

                }
                $scope.calculate_fine();
            }
            else {


                for (var index = 0; index < $scope.totalgridadvance.length; index++) {
                    if ($scope.totalgridadvance[index].FMH_Flag != "F") {
                        $scope.totalconcession = Number($scope.totalconcession) - Number($scope.totalgridadvance[index].FCSS_ConcessionAmount);
                        // $scope.totalfine = Number($scope.totalfine) - Number($scope.totalgrid[index].FCSS_FineAmount);
                        $scope.curramount = Number($scope.curramount) - Number($scope.totalgridadvance[index].FCSS_ToBePaid) + Number($scope.totalgridadvance[index].FCSS_OBArrearAmount);
                        $scope.totalwaived = Number($scope.totalwaived) - Number($scope.totalgridadvance[index].FCSS_WaivedAmount);

                    }

                }
                $scope.totalamt = $scope.curramount;

            }


            //if (allchkdata1 == true) {
            //    for (var index = 0; index < $scope.totalgridadvance.length; index++) {
            //        if ($scope.totalgridadvance[index].FMH_FeeName == "Fine" || $scope.totalgridadvance[index].FMH_Flag == "F") {
            //            $scope.curramount = $scope.curramount + Number($scope.totalgridadvance[index].FCSS_ToBePaid)
            //        }
            //    }
            //}
            $scope.totalamt = 0;
            var total = 0;
            angular.forEach($scope.totalgridadvance, function (nt) {
                if (nt.isSelected1 == true) {
                    total += nt.FCSS_ToBePaid;
                }


            })
            angular.forEach($scope.students, function (nt) {
                if (nt.isSelected == true) {
                    total += nt.fcsS_ToBePaid;
                }


            })
            $scope.totalamt = total;

        }

        $scope.advancedetails = function (userdata, totalgridadvance, index, totalamt) {

            $scope.disablefine = true;
            $scope.disablenetamount = true;

            //$scope.all = $scope.totalgridadvance.every(function (itm) {
            //    return itm.isSelected;
            //});


            var cnt = 0;
            if (cnt == 0) {
                
                angular.forEach($scope.totalgridadvance, function (ty) {
                    var keep_go = false;
                    if (userdata.AMSE_Id <= ty.AMSE_Id ) {
                        keep_go = true;
                    }
                    if (keep_go==false) {
                        //if ((ty.disablepaisterms == false || ty.disablepaisterms == undefined)) {
                        if (!ty.isSelected1) {
                              //  swal("You have to Select Semesters in Order");
                            $scope.totalgridadvance[index].isSelected1 = false;
                            }
                       // }
                    }
                })
            }
           
            $scope.calculate_fine();


        };



        $scope.calculate_fine = function () {
            var fine_totaladv = 0;
            var advance = 0;
            $scope.finedetailsadvance = [];
            angular.forEach($scope.totalgridadvance, function (nt) {
                if (nt.isSelected1) {
                    angular.forEach($scope.temp_Fine_Amountsadv, function (fn) {
                        if (fn.fcmaS_Id == nt.FCMAS_Id) {
                            advance += fn.fine_Amt;

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
         
                if (nt.FMH_Flag == 'F') {

                    var countadv = 0;

                    countadv = $scope.finedetailsadvance.filter((obj) => obj.fmgidadv === nt.FMG_Id).length;
                    if ($scope.finedetailsadvance.length == 0) {
                        nt.isSelected1 = false;
                    }
                    //else if ($scope.finedetailsadvance.length == 0 && nt.isSelected1 == true && nt.FCSS_ToBePaid > 0) {
                    //    nt.isSelected = true;
                    //}

                    else {
                        angular.forEach($scope.finedetailsadvance, function (fd) {
                            if (fd.fine_totaladv == 0) {
                                nt.isSelected1 = false;
                                nt.FCSS_ToBePaid = 0;
                                nt.fsS_TotalToBePaidaddfine = 0;
                            }
                            else if (nt.isSelected == true && countadv == 0 && nt.FCSS_ToBePaid > 0) {
                                nt.isSelected = true;
                                nt.FCSS_ToBePaid = fd.fine_totaladv ;
                                nt.fsS_TotalToBePaidaddfine = fd.fine_totaladv;


                            }

                            //if (nt.isSelected1 == true && countadv == 0) {
                            //    nt.isSelected1 = false;
                            //    nt.FCSS_ToBePaid = 0;
                            //    nt.fsS_TotalToBePaidaddfine = 0;
                            //}
                            else if (nt.isSelected1 == false && countadv > 0 && nt.FCSS_ToBePaid > 0 && nt.fsS_TotalToBePaidaddfine > 0) {
                                nt.isSelected1 = false;
                                nt.FCSS_ToBePaid = 0;
                                nt.fsS_TotalToBePaidaddfine = 0;
                            }
                            else if ((nt.isSelected1 == undefined || nt.isSelected1 == false) && nt.FMG_Id == fd.fmgidadv && nt.AMSE_SEMName == fd.semname) {

                                nt.isSelected1 = true;
                                nt.FCSS_ToBePaid = fd.fine_totaladv;
                                nt.fsS_TotalToBePaidaddfine = fd.fine_totaladv ;

                            }

                        });
                    }


                    if ($scope.curramount > 0 && nt.isSelected1 != undefined) {

                    }


                    if (nt.isSelected1 == true) {
                    $scope.totalfineadv = nt.FCSS_ToBePaid;
                    $scope.totalamt = $scope.curramount + Number(nt.FCSS_ToBePaid);
                }
                    else if (nt.isSelected1 == false) {
                        if ($scope.totalfineadv <= Number(nt.FCSS_ToBePaid) || countadv>0) {

                        $scope.totalamt = $scope.curramount;


                    }
                    else {
                    
                        $scope.totalamt = $scope.curramount - advance;
                    }

                }
                }

            })
            $scope.totalamt = 0;
            var total = 0;
            angular.forEach($scope.totalgridadvance, function (nt) {
                if (nt.isSelected1 == true) {
                    total += nt.FCSS_ToBePaid;
                }


            })
            angular.forEach($scope.students, function (nt) {
                if (nt.isSelected == true) {
                    total += nt.fcsS_ToBePaid;
                }


            })
            $scope.totalamt = total;
           


        }


        $scope.firstfnc = function (vlobj) {
            if (vlobj.cantuncheck === undefined || vlobj.cantuncheck === false) {
                if (vlobj.checkedgrplst === true) {
                    angular.forEach($scope.grouplist, function (obj) {
                        if (vlobj.fmG_Id == obj.fmG_Id) {
                            angular.forEach($scope.fillinstallment, function (obj1) {
                                if (obj1.fmG_Id == obj.fmG_Id && (obj1.termdisablechk == false || obj1.termdisablechk == undefined)) {
                                    obj1.checkedheadlst = true;
                                   
                                }
                            });
                        }
                    });
                } else {
                    angular.forEach($scope.grouplist, function (obj) {
                        if (vlobj.fmG_Id == obj.fmG_Id) {
                            angular.forEach($scope.fillinstallment, function (obj1) {
                                if (obj1.fmG_Id == obj.fmG_Id) {
                                    obj1.checkedheadlst = false;
                                  
                                }
                            });
                        }
                    });
                }
            }
            else {
                angular.forEach($scope.grp_ins_list, function (obj1) {
                    if (obj1.grp.fmG_Id === vlobj.fmG_Id) {
                        obj1.grp.checkedgrplst = true;
                        obj1.grp.cantuncheck = true;
                    }
                });
            }
        };

        $scope.secfnc = function (vlobj1) {
            if (vlobj1.cantuncheck === undefined || vlobj1.cantuncheck === false) {
                for (var s = 0; s < $scope.grouplist.length; s++) {
                    if (vlobj1.fmG_Id === $scope.grouplist[s].fmG_Id) {
                        var cnttt = 0; var n = 0;
                        var newtemp = [];
                        for (var t = 0; t < $scope.fillinstallment.length; t++) {
                            if (vlobj1.fmG_Id == $scope.fillinstallment[t].fmG_Id) {
                                if ($scope.fillinstallment[t].checkedheadlst === false || $scope.fillinstallment[t].checkedheadlst === undefined) {
                                    newtemp.push($scope.fillinstallment[t]);
                                }
                                if ($scope.fillinstallment[t].checkedheadlst === true) {
                                    cnttt += 1;
                                    if (newtemp.length > 0) {
                                        angular.forEach(newtemp, function (hh1) {
                                            if (hh1.termdisablechk != true) {
                                                hh1.checkedheadlst = true;
                                            }
                                        });
                                    }
                                    else {
                                        $scope.grouplist[s].checkedgrplst = false;
                                    }
                                }
                            }
                        }
                    }
                }
                
                var grpcnt = 0;
                for (var s = 0; s < $scope.grouplist.length; s++) {
                    if (vlobj1.fmG_Id === $scope.grouplist[s].fmG_Id) {
                        $scope.temptermlist = [];
                        var cnt = 0;
                        for (var t = 0; t < $scope.fillinstallment.length; t++) {
                            if (vlobj1.fmG_Id === $scope.fillinstallment[t].fmG_Id) {
                                if ($scope.fillinstallment[t].checkedheadlst === false || $scope.fillinstallment[t].checkedheadlst === undefined) {
                                    $scope.grouplist[s].checkedgrplst = false;
                                    if ($scope.fillinstallment[t].termdisablechk !== true) {
                                        $scope.temptermlist.push($scope.fillinstallment[t]);
                                    }
                                    angular.forEach($scope.fillinstallment, function (xx) {
                                        if (xx.checkedheadlst === true) {
                                            cnt += 1;
                                        }
                                    });
                                    if (cnt > 0) {
                                        angular.forEach($scope.fillinstallment, function (ll) {
                                            angular.forEach($scope.temptermlist, function (mm) {
                                                if (ll.ftI_Id === mm.ftI_Id && ll.termdisablechk !== true) {
                                                    ll.checkedheadlst = true;
                                                }
                                            });
                                        });
                                    }
                                }
                                else {
                                    grpcnt += 1;
                                    $scope.grouplist[s].checkedgrplst = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else {
                angular.forEach($scope.grp_ins_list, function (obj1) {
                    if (obj1.grp.fmG_Id == vlobj1.fmG_Id) {
                        angular.forEach(obj1.fillinstallment, function (obj3) {
                            if (obj3.ftI_Id == vlobj1.ftI_Id && obj3.termdisablechk == false) {
                                obj3.checkedheadlst = true;
                                obj3.cantuncheck = true;
                            }
                        });
                    }
                });
            }


          
        };


        $scope.totalamtchange = function (amt) {
            var finalamt = Number(amt);
            var compulasaryamt = 0;
            $scope.totalamt = 0;
            if (Number(amt) > 0) {
                for (var z = 0; z < $scope.students.length; z++) {
                    if ($scope.students[z].FMG_CompulsoryFlag != 'C') {
                        if (Number(amt) >= $scope.students[z].fcsS_ToBePaid) {
                            $scope.students[z].fcsS_ToBePaid = $scope.students[z].fcsS_ToBePaid;
                            amt = Number(amt) - $scope.students[z].fcsS_ToBePaid;
                        }
                        else if (Number(amt) == 0) {
                            $scope.students[z].fcsS_ToBePaid = 0;
                        }
                        else if (Number(amt) <= $scope.students[z].fcsS_ToBePaid) {
                            $scope.students[z].fcsS_ToBePaid = Number(amt);
                            amt = 0;
                        }
                    }
                    else {
                        compulasaryamt += $scope.students[z].fcsS_ToBePaid;
                    }

                
                }
               // $scope.totalamt = Number(finalamt) + Number(compulasaryamt);
                $scope.totalamt = Number(finalamt) + Number(compulasaryamt);
                
                $scope.totalamtdisable = true;



            } else {
                swal("Amount should be grether than 0");
            }

        }
    }

})();