(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegePreAdmOnlinePaymentController', CollegePreAdmOnlinePaymentController)

    CollegePreAdmOnlinePaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$window', 'FormSubmitter', 'superCache']
    function CollegePreAdmOnlinePaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $window, FormSubmitter, superCache) {

        var netamount = 0
        var concessionamount = 0
        var fineamount = 0
        var payableamountamount = 0
        var paidamount = 0
        var globalamount;
        $scope.qwe = {};
        $scope.disablestu = false;
        $scope.showsingle = false;
        $scope.showdouble = false;
        $scope.gateway = true;
        var autoreceipt, automanualreceiptnotranum

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        if (configsettings.length > 0) {
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
        }
        $scope.collapsed = true;
        $scope.firstfnc = function (vlobj) {

            if (vlobj.checkedgrplst == true) {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmgG_Id == obj.fmgG_Id) {
                        angular.forEach($scope.termlist, function (obj1) {
                            if (obj1.fmgG_Id == obj.fmgG_Id) {
                                obj1.checkedheadlst = true;
                                //angular.forEach($scope.installlst, function (obj2) {
                                //    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                //        obj2.checkedinstallmentlst = true;
                                //    }
                                //});
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmgG_Id == obj.fmgG_Id) {
                        angular.forEach($scope.termlist, function (obj1) {
                            if (obj1.fmgG_Id == obj.fmgG_Id) {
                                obj1.checkedheadlst = false;
                                //angular.forEach($scope.installlst, function (obj2) {
                                //    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                //        obj2.checkedinstallmentlst = false;
                                //    }
                                //});
                            }
                        });
                    }
                });
            }
        }


        $scope.secfnc = function (vlobj1) {
            if (vlobj1.cantuncheck === undefined || vlobj1.cantuncheck === false) {
                for (var s = 0; s < $scope.grouplst.length; s++) {
                    if (vlobj1.fmgG_Id === $scope.grouplst[s].fmgG_Id) {
                        var cnttt = 0; var n = 0;
                        var newtemp = [];
                        for (var t = 0; t < $scope.termlist.length; t++) {
                            if (vlobj1.fmgG_Id == $scope.termlist[t].fmgG_Id) {
                                if ($scope.termlist[t].checkedheadlst === false) {
                                    newtemp.push($scope.termlist[t]);
                                }
                                if ($scope.termlist[t].checkedheadlst === true) {
                                    cnttt += 1;
                                    angular.forEach(newtemp, function (hh1) {
                                        if (hh1.termdisablechk != true) {
                                            hh1.checkedheadlst = true;
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
                //Praveen Added
                var grpcnt = 0;
                for (var s = 0; s < $scope.grouplst.length; s++) {
                    if (vlobj1.fmgG_Id === $scope.grouplst[s].fmgG_Id) {
                        $scope.temptermlist = [];
                        var cnt = 0;
                        for (var t = 0; t < $scope.termlist.length; t++) {
                            if (vlobj1.fmgG_Id === $scope.termlist[t].fmgG_Id) {
                                if ($scope.termlist[t].checkedheadlst === false) {
                                    $scope.grouplst[s].checkedgrplst = false;
                                    if ($scope.termlist[t].termdisablechk !== true) {
                                        $scope.temptermlist.push($scope.termlist[t]);
                                    }
                                    angular.forEach($scope.termlist, function (xx) {
                                        if (xx.checkedheadlst === true) {
                                            cnt += 1;
                                        }
                                    });
                                    if (cnt > 0) {
                                        angular.forEach($scope.termlist, function (ll) {
                                            angular.forEach($scope.temptermlist, function (mm) {
                                                if (ll.fmT_Id === mm.fmT_Id && ll.termdisablechk !== true) {
                                                    ll.checkedheadlst = true;
                                                }
                                            });
                                        });
                                    }
                                }
                                else {
                                    grpcnt += 1;
                                    $scope.grouplst[s].checkedgrplst = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else {
                angular.forEach($scope.temp_grp_ins_list, function (obj1) {
                    if (obj1.grp.fmgG_Id == vlobj1.fmgG_Id) {
                        angular.forEach(obj1.trm_list, function (obj3) {
                            if (obj3.fmT_Id == vlobj1.fmT_Id && obj3.termdisablechk == false) {
                                obj3.checkedheadlst = true;
                                obj3.cantuncheck = true;
                            }
                        });
                    }
                });
            }
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

        $scope.showalldetails = false;

        var searchObject = $location.search();

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

        var grouporterm;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
        }

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag == "PreOnlineFull") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

        $scope.showstu = false;
        var termid;
        $scope.showpaymentdetails1 = false;
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 50

            var data = {
                "configset": grouporterm
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegePreAdmOnlinePayment/getstudentdetails", data).
                then(function (promise) {

                    if (promise.institutiondet.length > 0) {
                        for (var i = 0; i < promise.institutiondet.length; i++) {
                            $scope.institutioname = promise.institutiondet[0].inT_NAME;
                            $scope.institulogo = promise.institutiondet[0].institutioN_LOGO;
                        }
                    }

                    $scope.studentlist = promise.registrationList;

                    if (promise.transnumconfig.length > 0) {
                        automanualreceiptnotranum = promise.transnumconfig[0].imN_AutoManualFlag;
                    }

                    if ($scope.studentlist.length == 1) {
                        $scope.studentlist = promise.registrationList;
                        $scope.pacA_Id = $scope.studentlist[0].pacA_Id;
                        $scope.selectstudent();
                        $scope.showstu = false;
                        // swal(promise.dismessage);
                    }
                    else if ($scope.studentlist.length == 0) {
                        swal(promise.dismessage);
                    }
                    else {
                        $scope.showstu = true;
                        $scope.showalldetails = false;
                    }


                    //MB for Special
                    if (promise.specialheadlist.length > 0) {
                        $scope.special_head_list = promise.specialheadlist;
                        $scope.special_head_details = promise.specialheaddetails;
                    }
                    //MB for Special

                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

        $scope.onclickloaddata = function (totalamt, students, checkbx, yearlst, customfeegroup, qwe) {
            $scope.gateway = false;

            if (qwe.paygtw === "RAZORPAY") {
                $scope.pamentsave(totalamt, students, qwe);
            }
        }

        $scope.showpaymentdetails1 = false;
        $scope.selectstudent = function () {

            if ($scope.pacA_Id != "") {
                var data = {
                    "configset": grouporterm,
                    "PACA_Id": $scope.pacA_Id,
                    "multiplegroups": termid,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegePreAdmOnlinePayment/getalldetails", data).
                    then(function (promise) {

                        var partialorfullpay;
                        if (promise.partialorfullpayment.length > 0) {
                            partialorfullpay = promise.partialorfullpayment[0].ispaC_FullPaymentCompFlg
                        }

                        //if (partialorfullpay == "1") {

                        //}

                        $scope.showalldetails = true;
                        $scope.showpaymentdetails = true;
                        $scope.grouplst = promise.customgrplist;
                        $scope.termlist = promise.termlst;

                        angular.forEach($scope.grouplst, function (trn) {
                            trn.checkedgrplst = false;
                        })

                        angular.forEach($scope.termlist, function (trn) {
                            trn.checkedheadlst = false;
                            })

                        angular.forEach($scope.termlist, function (trn) {
                            if (trn.preAdmFlag == true) {
                                trn.checkedheadlst = true;

                            }
                        })



                        $scope.temp_grp_ins_list = [];
                        angular.forEach($scope.grouplst, function (grp) {
                            var term_list = [];
                            angular.forEach($scope.termlist, function (trn) {
                                if (trn.fmgG_Id == grp.fmgG_Id) {
                                    if (trn.preAdmFlag == true) {
                                        trn.checkedheadlst = true;
                                        grp.checkedgrplst = true;
                                        grp.grpdisable = true;
                                    }
                                    else {
                                        trn.checkedheadlst = false;
                                    }
                                    term_list.push(trn);
                                }
                            })

                            $scope.temp_grp_ins_list.push({ grp: grp, trm_list: term_list });
                        })

                        var totalamou = "0";
                        angular.forEach(promise.filonlinepaymentgrid, function (onjjj) {
                            totalamou = Number(totalamou) + onjjj.fsS_NetAmount;
                        })

                        $scope.totalnetamount = totalamou;

                        if (promise.fineheadfmaidsaved != null || promise.fineheadfmaidsaved != "" || promise.fineheadfmaidsaved != 0) {
                            $scope.totalbalance = totalamou - Number(promise.fineheadfmaidsaved);
                        }
                        else {
                            $scope.totalbalance = totalamou;
                        }


                        //for (var i = 0; i < promise.disableterms.length; i++) {
                        //    for (var j = 0; j < $scope.temp_grp_ins_list.length; j++) {

                        //        for (var k = 0; k < $scope.temp_grp_ins_list[j].trm_list.length; k++) {

                        //            if (promise.disableterms[i].FMGG_Id == $scope.temp_grp_ins_list[j].trm_list[k].fmgG_Id && promise.disableterms[i].fmt_id == $scope.temp_grp_ins_list[j].trm_list[k].fmT_Id) {
                        //                if (promise.disableterms[i].payable <= promise.disableterms[i].paid) {
                        //                    $scope.temp_grp_ins_list[j].trm_list[k].termdisablechk = true;
                        //                    //$scope.temp_grp_ins_list[j].trm_list[k].checkedheadlst = true;
                        //                }
                        //                else {
                        //                    $scope.temp_grp_ins_list[j].trm_list[k].termdisablechk = false;
                        //                }
                        //            }
                        //        }

                        //    }

                        //}

                        angular.forEach($scope.temp_grp_ins_list, function (obj) {
                            obj.grpdisable = obj.trm_list.every(function (options) {
                                return options.termdisablechk;
                            })
                        })


                        $scope.pasR_FirstName = promise.fillstudent[0].pacA_FirstName
                        $scope.pasR_MiddleName = promise.fillstudent[0].pacA_MiddleName;
                        $scope.pasR_LastName = promise.fillstudent[0].pacA_LastName

                        $scope.pasR_RegistrationNo = promise.fillstudent[0].pacA_RegistrationNo
                        $scope.classname = promise.fillstudent[0].amcO_CourseName;
                        $scope.branch = promise.fillstudent[0].amB_BranchName;
                        $scope.Semester = promise.fillstudent[0].amsE_SEMName;
                        $scope.pasR_MobileNo = promise.fillstudent[0].pacA_MobileNo;
                        $scope.pasR_emailId = promise.fillstudent[0].pacA_emailId;
                        $scope.pacA_StudentPhoto = promise.fillstudent[0].pacA_StudentPhoto;

                  
                    })
            }
            else {
                swal(" Kindly select Student ")
            }

        };

        $scope.cancel = function () {
            $state.reload();
        }


        $scope.temptermarray = [];
        var ftiids = 0;

        var remove_list = [];
        var ins_spe_list = [];
        $scope.paymenttest = [];
        $scope.qwe.paygtw = false;

        $scope.optionToggled = function (temp_grp_ins_list, termlist) {

            payableamountamount = 0;
            var selected_list = [];
            angular.forEach($scope.temp_grp_ins_list, function (grp_trm) {
                if (grp_trm.grp.checkedgrplst) {
                    var term_list = [];
                    angular.forEach(grp_trm.trm_list, function (trn) {
                        if (trn.checkedheadlst) {
                            term_list.push(trn);
                        }
                    })
                    selected_list.push({ grp: grp_trm.grp, trm_list: term_list });
                }

            })

            angular.forEach($scope.temp_grp_ins_list, function (grp_trm) {
                grp_trm.grpdisable = true;
                angular.forEach(grp_trm.trm_list, function (trn) {
                    trn.termdisablechk = true;
                    trn.preAdmFlag = true;
                })
            })

            if (selected_list.length > 0) {
                var data = {
                    grouplist: selected_list,
                    "PACA_Id": $scope.pacA_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }


                apiService.create("CollegePreAdmOnlinePayment/getamountdetails", data).
                    then(function (promise) {

                        $scope.disablestu = true;

                        $scope.paymenttest = promise.fillpaymentgateway;

                        if (promise.fillpaymentgateway.length == 1) {
                            $scope.qwe.paygtw = promise.fillpaymentgateway[0].fpgD_PGName;
                            $scope.gateway = false;
                        }

                        if (promise.fillpaymentgateway.length > 1) {
                            $scope.showsingle = false;
                            $scope.showdouble = true;
                        }
                        else {
                            $scope.showsingle = true;
                            $scope.showdouble = false;
                        }

                        if (promise.fillstudentviewdetails.length > 0) {

                            $scope.showpaymentdetails1 = true;

                            if (selected_list.length > 0) {
                                for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {
                                    $scope.temptermarray.push(promise.fillstudentviewdetails[i]);
                                }

                            } else {
                                for (var j = 0; j < $scope.temptermarray.length; j++) {
                                    var name = $scope.temptermarray[j].fcmaS_Id;
                                    for (var k = 0; k < promise.fillstudentviewdetails.length; k++) {
                                        if (name == promise.fillstudentviewdetails[k].fcmaS_Id) {
                                            $scope.temptermarray.splice(j, 1);
                                            j = j - 1;
                                            break;
                                        }
                                    }
                                }
                            }

                            $scope.students = promise.fillstudentviewdetails;

                            //angular.forEach(promise.finearray, function (objec) {
                            //    $scope.students.push(objec);
                            //})

                            $scope.all = true;
                            angular.forEach($scope.students, function (objec) {
                                objec.isSelected = true;
                            }
                            )

                            if (promise.fillstudentviewdetails.length > 0) {
                                for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {

                                    // netamount = Number(netamount) + Number(promise.fillstudentviewdetails[i].fsS_NetAmount)
                                    //concessionamount = Number(concessionamount) + Number(promise.filonlinepaymentgrid[i].fsS_ConcessionAmount)
                                    // fineamount = Number(fineamount) + Number(promise.filonlinepaymentgrid[i].fsS_FineAmount)
                                    payableamountamount = Number(payableamountamount) + Number(promise.fillstudentviewdetails[i].fcsS_ToBePaid)

                                    // paidamount = Number(paidamount) + Number(promise.filonlinepaymentgrid[i].fsS_PaidAmount)
                                }

                                var totpayamt = 0, totpayfine = 0;
                                for (var j = 0; j < $scope.students.length; j++) {
                                    //totpayamt = Number(totpayamt) + Number($scope.students[j].fsS_ToBePaid)
                                    //totpayfine = Number(totpayfine) + Number($scope.students[j].fsS_FineAmount)

                                    totpayamt = Number(payableamountamount);
                                }

                                // $scope.totalamt = Number(totpayamt) + Number(totpayfine);
                                $scope.totalamt = Number(payableamountamount)
                                globalamount = $scope.totalamt;
                            }


                            //MB for Special
                         
                            $scope.temp_Head_Instl_list = [];
                            angular.forEach($scope.students, function (uy) {
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
                                            angular.forEach($scope.students, function (op_m) {
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
                                        var currentYrCharges = 0;
                                        var totalCharges = 0;
                                        var concessionAmount = 0;
                                        var fineAmount = 0;
                                        var toBePaid = 0;
                                        var netAmount = 0;
                                        var fmG_Id = 0;
                                        var fmG_GroupName = '';
                                        var fmG_GroupName = '';
                                        var fsS_OBArrearAmount = 0;

                                        var not_cnt = 0;
                                        angular.forEach(a2.sp_ind_list, function (a3) {
                                            if (fmG_Id == 0) {
                                                fmG_Id = a3.fmG_Id;
                                                fmG_GroupName = "Baldwin Girls School Fee";
                                            }
                                            else {
                                                if (fmG_Id != a3.fmG_Id) {
                                                    not_cnt += 1;
                                                }
                                            }
                                            currentYrCharges += a3.fcsS_CurrentYrCharges;
                                            totalCharges += a3.fcsS_TotalToBePaid;
                                            concessionAmount += a3.fcsS_ConcessionAmount;
                                            fineAmount += a3.fcsS_FineAmount;
                                            toBePaid += a3.fcsS_ToBePaid;
                                            netAmount += a3.fcsS_TotalToBePaid;
                                            fsS_OBArrearAmount += a3.fcsS_OBArrearAmount;
                                        })
                                        if (not_cnt == 0) {
                                            $scope.temp_Head_Instl_list.push({ fmG_Id: fmG_Id, fmG_GroupName: 'SH_' + fmG_GroupName, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, fsS_NetAmount: currentYrCharges, fcsS_TotalToBePaid: totalCharges, fcsS_ConcessionAmount: concessionAmount, fcsS_FineAmount: fineAmount, fcsS_ToBePaid: toBePaid, fcsS_NetAmount: netAmount, fcsS_OBArrearAmount: fcsS_OBArrearAmount, Head_Flag: 'SH' });
                                        }
                                        else if (not_cnt > 0) {
                                            $scope.temp_Head_Instl_list.push({ fmG_Id: 0, fmG_GroupName: 'Special_Head', fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, fsS_NetAmount: currentYrCharges, fcsS_TotalToBePaid: totalCharges, fcsS_ConcessionAmount: concessionAmount, fcsS_FineAmount: fineAmount, fcsS_ToBePaid: toBePaid, fcsS_NetAmount: netAmount, fcsS_OBArrearAmount: fcsS_OBArrearAmount, Head_Flag: 'SH' });
                                        }

                                    })
                                })
                                $scope.students = $scope.temp_Head_Instl_list;
                            }
                            //MB for Special

                            if (promise.fillpaymentgateway.length == 1) {
                                $scope.onclickloaddata($scope.totalamt, $scope.students, 1, 1, 1, $scope.qwe);
                            }

                        }

                        else {

                            swal("Amount is Paid for the selected Term");

                            angular.forEach($scope.temp_grp_ins_list, function (objr) {

                                objr.grp.checkedgrplst = false;

                                angular.forEach(objr.trm_list, function (objr1) {
                                    objr1.checkedheadlst = false;
                                })
                            })

                            $scope.showpaymentdetails1 = false;

                            $state.reload();

                        }
                    })
            }
            else {
                $scope.showpaymentdetails1 = false;
                swal("Kindly select atleast one Term")

                angular.forEach($scope.temp_grp_ins_list, function (grp_trm) {
                    grp_trm.grpdisable = false;
                    angular.forEach(grp_trm.trm_list, function (trn) {
                        trn.termdisablechk = false;
                    })
                })
                //$state.reload();
            }
        }


        $scope.interacted = function (field) {

            return $scope.submitted;
        };


        $scope.pamentsave = function (totalamt, students, qwe) {

            if ($scope.myForm.$valid) {

                //MB For Special
                var count = 0;
                var save_hes_insts = [];
                if (ins_spe_list.length == 0 && remove_list.length == 0) {
                    angular.forEach(students, function (opq) {
                        if (opq.isSelected) {
                            count += 1;
                            save_hes_insts.push(opq);
                        }
                    })
                }
                else if (ins_spe_list.length > 0 && remove_list.length > 0) {
                    angular.forEach(students, function (opq) {
                        if (opq.isSelected) {
                            if (opq.Head_Flag == 'H') {
                                count += 1;
                                save_hes_insts.push(opq);
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
                                                        save_hes_insts.push(s2);
                                                    })
                                                }
                                                else if (toBePaid > Number(opq.fsS_ToBePaid)) {
                                                    var keepGoing = true;
                                                    angular.forEach(s1.sp_ind_list, function (s2) {
                                                        if (keepGoing) {
                                                            if (Number(opq.fsS_ToBePaid) >= Number(s2.fsS_ToBePaid)) {
                                                                count += 1;
                                                                save_hes_insts.push(s2);
                                                                opq.fsS_ToBePaid = (Number(opq.fsS_ToBePaid) - Number(s2.toBePaid));
                                                            }
                                                            else if (Number(opq.fsS_ToBePaid) < Number(s2.fsS_ToBePaid)) {
                                                                s2.toBePaid = Number(opq.fsS_ToBePaid);
                                                                count += 1;
                                                                save_hes_insts.push(s2);
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


                var selected_list = [];
                angular.forEach($scope.temp_grp_ins_list, function (grp_trm) {
                    if (grp_trm.grp.checkedgrplst) {
                        var term_list = [];
                        angular.forEach(grp_trm.trm_list, function (trn) {
                            if (trn.checkedheadlst) {
                                term_list.push(trn);
                            }
                        })
                        selected_list.push({ grp: grp_trm.grp, trm_list: term_list });
                    }

                })


                var data = {
                    grouplist: selected_list,
                    "temarray": $scope.students,
                    "topayamount": totalamt,
                    "PACA_Id": $scope.pacA_Id,
                    selected_list: $scope.students,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                    "auto_receipt_flag": autoreceipt,
                    "automanualreceiptno": automanualreceiptnotranum,
                    "onlinepaygteway": qwe.paygtw
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegePreAdmOnlinePayment/generatehashsequence", data).
                    then(function (promise) {


                        if (qwe.paygtw == "BILLDESK") {
                            if (promise.pipeSepMsg != null && promise.pipeSepMsg != '') {
                                //var url "https://pgi.billdesk.com/pgidsk/PGIMerchantPayment";
                                var url = promise.url;
                                var method = 'POST';
                                var params = {
                                    "msg": promise.pipeSepMsg
                                }
                                FormSubmitter.submit(url, method, params);
                            }
                            else {
                                swal("Sorry Something Went Wrong");
                            }
                        }

                        else if (qwe.paygtw == "PAYTM") {
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

                            // for staging 
                            //string transactionURL = "https://securegw-stage.paytm.in/theia/processTransaction";
                            // for production 
                            // string transactionURL = "https://securegw.paytm.in/theia/processTransaction";

                            //payu_URL = "https://securegw-stage.paytm.in/theia/processTransaction";
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
                                //"CALLBACK_URL": "https://secure.paytm.in/oltp-web/invoiceResponse",
                                "CALLBACK_URL": "http://localhost:57606/api/CollegePreAdmOnlinePayment/paymentresponsePAYTM/",
                            }

                            FormSubmitter.submit(url, method, params);

                        }
                        else if (qwe.paygtw == "PAYU") {
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
                                "surl": $scope.surl,
                                "furl": $scope.furl,
                                "udf1": $scope.udf1,
                                "udf2": $scope.udf2,
                                "udf3": $scope.udf3,
                                "udf4": $scope.udf4,
                                "udf5": $scope.udf5,
                                "udf6": $scope.udf6,
                                "service_provider": $scope.service_provider,
                                "hash": promise.paydet[0].hash,
                                "surl": "http://localhost:57606/api/CollegePreAdmOnlinePayment/paymentresponse/",
                                "furl": "http://localhost:57606/api/CollegePreAdmOnlinePayment/paymentresponse/"
                            }
                            FormSubmitter.submit(url, method, params);
                        }

                        else if (qwe.paygtw == "RAZORPAY") {

                            $scope.txnamt = Number(promise.fmA_Amount) * 100;
                            $scope.SaltKey = promise.fpgD_AuthorisationKey;
                            $scope.orderid = promise.trans_id;

                            $scope.institutioname = $scope.institutioname;
                            $scope.institulogo = $scope.institulogo;

                            $scope.stuname = promise.fillstudent[0].pasR_FirstName;
                            $scope.stuemailid = promise.fillstudent[0].pasR_emailId;
                            $scope.stuaddress = promise.fillstudent[0].amsT_PerCity;
                            $scope.stumobileno = promise.fillstudent[0].pasR_MobileNo;
                            $scope.stuadmno = promise.fillstudent[0].pasR_RegistrationNo;
                            $scope.splitpayinfor = promise.payinfo;

                            $scope.mI_Id = promise.mI_Id;
                            $scope.asmaY_Id = promise.asmaY_Id;
                            $scope.amst_Id = promise.amst_Id;
                        }

                    })
            }
            else {
                // swal("Please enter Valid date")
                $scope.submitted = true;
            }

        }
    }

})();
