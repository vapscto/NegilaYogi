(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeOnlinePaymentVikasaController', FeeOnlinePaymentVikasaController)

    FeeOnlinePaymentVikasaController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$window', 'FormSubmitter', 'superCache']
    function FeeOnlinePaymentVikasaController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $window, FormSubmitter, superCache) {

        $scope.students = [];
        $scope.gateway = true;
        $scope.qwe = {};
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
        // $scope.checkedheadlst = false;
        angular.forEach($scope.termlist, function (tstobj) {
            tstobj.checkedheadlst = false;
        })
        $scope.regulardisplay = true;

        var grouporterm, autoreceipt, automanualreceiptnotranum;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
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

        var institutionid = 0;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        institutionid = configsettings[0].mI_Id;

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

        $scope.firstfnc = function (vlobj) {
            debugger;
            if (vlobj.checkedgrplst == true) {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmgG_Id == obj.fmgG_Id) {
                        angular.forEach($scope.termlist, function (obj1) {
                            if (obj1.fmgG_Id == obj.fmgG_Id && obj1.termdisablechk == false) {
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
            //debugger;
            //if (vlobj1.checkedheadlst == true) {
            //    angular.forEach($scope.headlst, function (val) {
            //        if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
            //            angular.forEach($scope.installlst, function (val1) {
            //                if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
            //                    val1.checkedinstallmentlst = true;
            //                }
            //            });
            //        }
            //    });
            //} else {
            //    debugger;
            //    angular.forEach($scope.headlst, function (val) {
            //        if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
            //            angular.forEach($scope.installlst, function (val1) {
            //                if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
            //                    val1.checkedinstallmentlst = false;
            //                }
            //            });
            //        }
            //    });
            //}
            debugger;


            //for (var s = 0; s < $scope.grouplst.length; s++) {

            //    if (vlobj1.fmgG_Id == $scope.grouplst[s].fmgG_Id) {
            //        for (var t = 0; t < $scope.termlist.length; t++) {
            //            if (vlobj1.fmgG_Id == $scope.termlist[t].fmgG_Id) {
            //                if ($scope.termlist[t].checkedheadlst == false) {
            //                    $scope.grouplst[s].checkedgrplst = false;
            //                }
            //                else {
            //                    $scope.grouplst[s].checkedgrplst = true;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}

            for (var s = 0; s < $scope.grouplst.length; s++) {

                if (vlobj1.fmgG_Id == $scope.grouplst[s].fmgG_Id) {
                    var cnttt = 0;
                    var n = 0;
                    var newtemp = [];
                    for (var t = 0; t < $scope.termlist.length; t++) {
                        if (vlobj1.fmgG_Id == $scope.termlist[t].fmgG_Id) {

                            if ($scope.termlist[t].checkedheadlst == false) {
                                newtemp.push($scope.termlist[t])
                            }
                            if ($scope.termlist[t].checkedheadlst == true) {

                                cnttt += 1;
                                angular.forEach(newtemp, function (hh1) {
                                    if (hh1.termdisablechk != true) {
                                        hh1.checkedheadlst = true;
                                    }

                                })

                            }



                        }
                    }
                }
            }

            //  console.log(newtemp)

            //Praveen Added

            var grpcnt = 0;
            for (var s = 0; s < $scope.grouplst.length; s++) {

                if (vlobj1.fmgG_Id == $scope.grouplst[s].fmgG_Id) {


                    $scope.temptermlist = [];
                    var cnt = 0;
                    for (var t = 0; t < $scope.termlist.length; t++) {
                        if (vlobj1.fmgG_Id == $scope.termlist[t].fmgG_Id) {
                            if ($scope.termlist[t].checkedheadlst == false) {
                                $scope.grouplst[s].checkedgrplst = false;
                                if ($scope.termlist[t].termdisablechk != true) {
                                    $scope.temptermlist.push($scope.termlist[t])
                                }
                                angular.forEach($scope.termlist, function (xx) {
                                    if (xx.checkedheadlst == true) {
                                        cnt += 1;
                                    }
                                })
                                if (cnt > 0) {

                                    angular.forEach($scope.termlist, function (ll) {
                                        angular.forEach($scope.temptermlist, function (mm) {
                                            if (ll.fmT_Id == mm.fmT_Id && ll.termdisablechk != true) {
                                                ll.checkedheadlst = true;
                                            }
                                        })
                                    })


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

        $scope.isOptionsRequired1 = function () {
            return !$scope.grouplst.some(function (options) {
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

                for (var index = 0; index < $scope.students.length; index++) {
                    $scope.totalconcession = Number($scope.totalconcession) + Number($scope.students[index].fsS_ConcessionAmount);
                    $scope.totalfine = Number($scope.totalfine) + Number($scope.students[index].fsS_FineAmount);
                    $scope.curramount = Number($scope.curramount) + Number($scope.students[index].fsS_ToBePaid) + $scope.totalfine;
                    $scope.totalwaived = Number($scope.totalwaived) + Number($scope.students[index].fsS_WaivedAmount);

                }
                $scope.totalamt = Number($scope.curramount) - Number($scope.totalconcession) + Number($scope.totalfine);
                $scope.amount = $scope.totalamt;
                alert($scope.amount);
            }
            else {
                $scope.totalconcession = 0;
                $scope.totalfine = 0;
                $scope.curramount = 0;
                $scope.totalwaived = 0;
                $scope.totalamt = 0;
            }
        }

        $scope.amtdetails = function (userdata, students, index, totalamt) {
            $scope.totalamt = 0;
            $scope.all = $scope.students.every(function (itm) { return itm.isSelected; });

            $scope.totalconcession = 0;
            $scope.totalfine = 0;
            $scope.curramount = 0;
            $scope.totalwaived = 0;



            for (var index = 0; index < $scope.students.length; index++) {
                if (students[index].isSelected == true) {
                    $scope.totalconcession = Number($scope.totalconcession) + Number($scope.students[index].fsS_ConcessionAmount);
                    $scope.totalfine = Number($scope.totalfine) + Number($scope.students[index].fsS_FineAmount);
                    $scope.curramount = Number($scope.curramount) + Number($scope.students[index].fsS_ToBePaid) + $scope.totalfine;
                    $scope.totalwaived = Number($scope.totalwaived) + Number($scope.students[index].fsS_WaivedAmount);
                }


            }
            $scope.totalamt = Number($scope.curramount) - Number($scope.totalconcession) + Number($scope.totalfine);

            $scope.amount = $scope.totalamt;

            //for (var i = 0; i < $scope.students.length; i++) {
            //    if (students[i].isSelected == true) {
            //        $scope.totalconcession = Number($scope.totalconcession) + Number(students[i].fsS_ConcessionAmount);
            //        $scope.totalfine = Number($scope.totalfine) + Number(students[i].fsS_FineAmount);
            //        $scope.curramount = Number($scope.curramount) + Number(students[i].fsS_ToBePaid) + $scope.totalfine;
            //        $scope.totalwaived = Number($scope.totalwaived) + Number(students[i].fsS_WaivedAmount);

            //        $scope.totalamt = Number($scope.curramount) + Number($scope.totalfine);

            //        //$scope.totalnetamount = netamount;
            //        //$scope.totalconcessionn = concessionamount;
            //        //$scope.totalbalance = netamount - paidamount
            //        //$scope.totalpaidamount = paidamount

            //    }
            //    else if (students[index].isSelected == false) {
            //        $scope.totalconcession = Number(students[index].fsS_ConcessionAmount);
            //        //$scope.totalfine = Number($scope.totalfine) - Number(totalgrid[index].fsS_FineAmount);
            //        $scope.totalfine = Number(students[index].fsS_FineAmount);
            //        $scope.curramount = Number(students[index].fsS_ToBePaid) - Number(students[index].fsS_FineAmount);
            //        $scope.totalwaived = Number(students[index].fsS_WaivedAmount);

            //        $scope.totalamt = Number(totalamt) - (Number($scope.curramount) - Number($scope.totalconcession) + Number($scope.totalfine));
            //    }
            //}

            //angular.forEach($scope.students, function (yy) {
            //    if (yy.isSelected == true) {
            //        $scope.totalamt += yy.fsS_ToBePaid;
            //    }

            //})
            //$scope.totalamt = Number($scope.curramount) - Number($scope.totalconcession) + Number($scope.totalfine);
        }

        $scope.onclickloaddata = function () {
            $scope.gateway = false;
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

                apiService.create("FeeOnlinePaymentVikasa/getgrouportermdetails", data).
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

                apiService.create("FeeOnlinePaymentVikasa/getcustomgroups", data).
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

                apiService.create("FeeOnlinePaymentVikasa/getgrouportermdetails", data).
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

        $scope.optionToggledtransport = function (insid, data1, yearlst) {

            $scope.selectAll = $scope.yearlst.every(function (role) { return role.selected; })
            //$scope.showpaymentdetails = true;

            var termid = insid;
            //var studid = 400;

            var gouportermcount = "0";
            for (var i = 0; i < yearlst.length; i++) {
                name = $scope.yearlst[i].selected
                if (name == "true") {
                    gouportermcount = gouportermcount + ',' + yearlst[i].fmG_Id;
                }
            }

            if ($scope.checkboxval != 'undefined') {
                var data = {
                    "FMT_Id": termid,
                    //"Amst_Id": studid,
                    "temarray": $scope.students,
                    "ftiidss": gouportermcount,
                    "Paymenttype": $scope.checkboxval
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }


                apiService.create("FeeOnlinePaymentVikasa/getamountdetails", data).
                    then(function (promise) {

                        if (promise.fillstudentviewdetails.length > 0) {

                            $scope.showpaymentdetails1 = true;

                            if (data1.selected == true) {
                                for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {
                                    // promise.fillstudentviewdetails[i].selected = true;
                                    $scope.temptermarray.push(promise.fillstudentviewdetails[i]);
                                }

                            } else {
                                for (var j = 0; j < $scope.temptermarray.length; j++) {
                                    var name = $scope.temptermarray[j].fma_id;
                                    for (var k = 0; k < promise.fillstudentviewdetails.length; k++) {
                                        if (name == promise.fillstudentviewdetails[k].fma_id) {
                                            $scope.temptermarray.splice(j, 1);
                                            j = j - 1;
                                            break;
                                        }
                                    }
                                }
                            }


                            $scope.students = promise.fillstudentviewdetails;
                            $scope.all = true;
                            angular.forEach($scope.students, function (objec) {
                                objec.isSelected = true;
                            }
                            )

                            // $scope.students = $scope.temptermarray;

                            //JSON.stringify($scope.students);


                        }

                        else {
                            swal("Amount is Paid for the selected Term");
                            data1.selected = false;
                            if ($scope.temptermarray.length == 0) {
                                $scope.showpaymentdetails = false;
                                $state.reload();
                            }

                        }
                    })
            }
            else {
                swal("Kindly select any one Payment type")
            }
        }

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.temptermarray = [];
        var ftiids = 0;
        var remove_list = [];
        var ins_spe_list = [];
        $scope.optionToggled = function (temp_grp_ins_list, termlist) {


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
                })
            })

            //var customgroupid = "0",termid="0";
            //for (var i = 0; i < grouplst.length; i++) {
            //    name = $scope.grouplst[i].checkedgrplst
            //    if (name == "true") {
            //        customgroupid = customgroupid + ',' + grouplst[i].fmgG_Id;
            //    }
            //}


            //for (var i = 0; i < termlist.length; i++) {
            //    name = $scope.termlist[i].checkedheadlst
            //    if (name == "true") {
            //        termid = termid + ',' + termlist[i].fmT_Id;
            //    }
            //}


            if (selected_list.length > 0) {
                var data = {
                    //"FMT_Id": termid,
                    ////"Amst_Id": studid,
                    //"temarray": $scope.students,
                    //"ftiidss": gouportermcount,
                    //"Paymenttype": $scope.checkboxval,
                    //"grpidss": grpcustomgrp
                    //  "fetchcustomgrplist": customgroupid,
                    //  "fetchtermlst": termid,
                    selected_list: selected_list,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.asmcL_ID,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }


                apiService.create("FeeOnlinePaymentVikasa/getamountdetails", data).
                    then(function (promise) {

                        if (promise.fillstudentviewdetails.length > 0) {

                            $scope.showpaymentdetails1 = true;
                            $scope.paymenttest = promise.fillpaymentgateway;
                            console.log($scope.paymenttest);

                            if (promise.fillpaymentgateway.length > 1) {
                                $scope.showsingle = false;
                                $scope.showdouble = true;
                            }
                            else {
                                $scope.showsingle = true;
                                $scope.showdouble = false;
                            }

                            //if (promise.fillpaymentgateway.length > 0) {
                            //    for (var i = 0; i < promise.fillpaymentgateway.length; i++) {
                            //        if (promise.fillpaymentgateway[i].fpgD_PGName == "PAYU") {
                            //            $scope.payuvisible = true;
                            //        }

                            //        if (promise.fillpaymentgateway[i].fpgD_PGName == "BILLDESK") {
                            //            $scope.billdeskvisible = true;


                            //        }

                            //        if (promise.fillpaymentgateway[i].fpgD_PGName == "EBS") {
                            //            $scope.ebsvisible = true;

                            //        }

                            //    }
                            //}


                            if (selected_list.length > 0) {
                                for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {
                                    $scope.temptermarray.push(promise.fillstudentviewdetails[i]);
                                }

                            } else {
                                for (var j = 0; j < $scope.temptermarray.length; j++) {
                                    var name = $scope.temptermarray[j].fma_id;
                                    for (var k = 0; k < promise.fillstudentviewdetails.length; k++) {
                                        if (name == promise.fillstudentviewdetails[k].fma_id) {
                                            $scope.temptermarray.splice(j, 1);
                                            j = j - 1;
                                            break;
                                        }
                                    }
                                }
                            }


                            $scope.students = promise.fillstudentviewdetails;


                            angular.forEach(promise.finearray, function (objec) {
                                $scope.students.push(objec);
                            })




                            $scope.all = true;
                            angular.forEach($scope.students, function (objec) {
                                objec.isSelected = true;
                            }
                            )

                            // $scope.students = $scope.temptermarray;

                            //JSON.stringify($scope.students);

                            if (promise.filonlinepaymentgrid.length > 0) {
                                for (var i = 0; i < promise.filonlinepaymentgrid.length; i++) {

                                    netamount = Number(netamount) + Number(promise.filonlinepaymentgrid[i].fsS_NetAmount)
                                    concessionamount = Number(concessionamount) + Number(promise.filonlinepaymentgrid[i].fsS_ConcessionAmount)
                                    fineamount = Number(fineamount) + Number(promise.filonlinepaymentgrid[i].fsS_FineAmount)
                                    payableamountamount = Number(payableamountamount) + Number(promise.filonlinepaymentgrid[i].fsS_ToBePaid)

                                    paidamount = Number(paidamount) + Number(promise.filonlinepaymentgrid[i].fsS_PaidAmount)

                                }

                                var totpayamt = 0, totpayfine = 0;
                                for (var j = 0; j < $scope.students.length; j++) {
                                    totpayamt = Number(totpayamt) + Number($scope.students[j].fsS_ToBePaid)
                                    totpayfine = Number(totpayfine) + Number($scope.students[j].fsS_FineAmount)
                                }

                                $scope.totalamt = Number(totpayamt) + Number(totpayfine);

                                globalamount = $scope.totalamt;
                            }

                            //MB for Special
                            debugger;
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
                                            currentYrCharges += a3.fsS_CurrentYrCharges;
                                            totalCharges += a3.fsS_TotalToBePaid;
                                            concessionAmount += a3.fsS_ConcessionAmount;
                                            fineAmount += a3.fsS_FineAmount;
                                            toBePaid += a3.fsS_ToBePaid;
                                            netAmount += a3.fsS_TotalToBePaid;
                                            fsS_OBArrearAmount += a3.fsS_OBArrearAmount;
                                        })
                                        if (not_cnt == 0) {
                                            $scope.temp_Head_Instl_list.push({ fmG_Id: fmG_Id, fmG_GroupName: 'SH_' + fmG_GroupName, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, fsS_NetAmount: currentYrCharges, fsS_TotalToBePaid: totalCharges, fsS_ConcessionAmount: concessionAmount, fsS_FineAmount: fineAmount, fsS_ToBePaid: toBePaid, fsS_NetAmount: netAmount, fsS_OBArrearAmount: fsS_OBArrearAmount, Head_Flag: 'SH' });
                                        }
                                        else if (not_cnt > 0) {
                                            $scope.temp_Head_Instl_list.push({ fmG_Id: 0, fmG_GroupName: 'Special_Head', fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, fsS_NetAmount: currentYrCharges, fsS_TotalToBePaid: totalCharges, fsS_ConcessionAmount: concessionAmount, fsS_FineAmount: fineAmount, fsS_ToBePaid: toBePaid, fsS_NetAmount: netAmount, fsS_OBArrearAmount: fsS_OBArrearAmount, Head_Flag: 'SH' });
                                        }

                                    })
                                })
                                $scope.students = $scope.temp_Head_Instl_list;
                            }
                            //MB for Special

                            $scope.all = true;
                            angular.forEach($scope.students, function (objec) {
                                objec.isSelected = true;
                            })


                        }

                        else {

                            //data1.selected = false;

                            swal("Amount is Paid for the selected Term and Group");

                            angular.forEach($scope.temp_grp_ins_list, function (objr) {

                                objr.grp.checkedgrplst = false;

                                angular.forEach(objr.trm_list, function (objr1) {
                                    objr1.checkedheadlst = false;
                                })
                            })

                            $scope.showpaymentdetails1 = false;
                        }
                    })
            }
            else {
                $scope.showpaymentdetails1 = false;
                swal("Kindly select atleast one")
                $state.reload();

                //angular.forEach($scope.checkedgrplst, function (grp_trm) {
                //    grp_trm.grpdisable = false;
                //    angular.forEach(grp_trm.trm_list, function (trn) {
                //        trn.termdisablechk = false;
                //    })
                //})

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

            apiService.create("FeeOnlinePaymentVikasa/getalldetails", data).
                then(function (promise) {

                    if (promise.readterms.length > 0) {
                        if (promise.readterms[0].fmC_Online_Payment_Aca_Yr_Flag == "B") {
                            $scope.feeconfiglst = promise.readterms[0].fmC_Online_Payment_Aca_Yr_Flag;
                            $scope.acayrlst = promise.academicyrlist;

                            $scope.showbasicdetails = false;
                            $scope.showpaymentdetails = false;
                            $scope.showpaymentdetails1 = false;
                        }

                        else {
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
                            }
                            else {
                                $scope.showbasicdetails = false;
                                $scope.showpaymentdetails = false;
                                $scope.showpaymentdetails1 = false;
                                swal("Application form is not yet approved!!!!!")
                            }
                        }
                    }

                    //MB for Special
                    if (promise.specialheadlist.length > 0) {
                        $scope.special_head_list = promise.specialheadlist;
                        $scope.special_head_details = promise.specialheaddetails;
                    }
                    //MB for Special

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

                apiService.create("FeeOnlinePaymentVikasa/getalldetails", data).
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
            debugger;
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
            apiService.create("FeeOnlinePaymentVikasa/getamountdetails", data).
                then(function (promise) {


                    $scope.checktermdetails = true;

                    if (promise.fillstudentviewdetails.length > 0) {
                        $scope.showpaymentdetails = true;
                        $scope.temptermarray.push(promise.fillstudentviewdetails);

                        $scope.students = promise.fillstudentviewdetails;

                        for (var i = 0; i < promise.fillstudentviewdetails.length; i++) {

                            netamount = Number(netamount) + Number(promise.fillstudentviewdetails[i].fsS_NetAmount)
                            concessionamount = Number(concessionamount) + Number(promise.fillstudentviewdetails[i].fsS_ConcessionAmount)
                            fineamount = Number(fineamount) + Number(promise.fillstudentviewdetails[i].fsS_FineAmount)
                            payableamountamount = Number(payableamountamount) + Number(promise.fillstudentviewdetails[i].fsS_ToBePaid)

                            paidamount = Number(netamount) - +Number(promise.fillstudentviewdetails[i].fsS_PaidAmount)

                            payableamountamount = payableamountamount - concessionamount + fineamount
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

        $scope.paydetails = false;
        var goupid = "0";
        $scope.pamentsave = function (totalamt, students, checkbx, yearlst, customfeegroup, qwe) {

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

            if ($scope.amst_mobile != 0 || $scope.amst_email_id != "") {
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

                //alert(selected_list)
                console.log(selected_list);
                console.log(save_hes_insts);
                var data = {
                    //"temarray": $scope.students,
                    "temarray": save_hes_insts,
                    "topayamount": totalamt,
                    "AMST_Id": $scope.amst_Id,
                    "ASMCL_Id": $scope.asmcL_ID,
                    selected_list: selected_list,
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

                apiService.create("FeeOnlinePaymentVikasa/generatehashsequence", data).
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
                        else {
                            $scope.key = promise.paydet[0].marchanT_ID;
                            //$scope.key = promise.paydet[0].saltKey;
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

                            //radha

                            // var url = 'https://test.payu.in/_payment';

                            var payu_URL = $scope.payu_URL;
                            var url = payu_URL;
                            var method = 'POST';
                            //$scope.amount = 30;
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
                                "surl": "http://localhost:57606/api/FeeOnlinePayment/paymentresponse/",
                                "furl": "http://localhost:57606/api/FeeOnlinePayment/paymentresponse/"
                            }
                            FormSubmitter.submit(url, method, params);
                        }

                    })
            }
            else {
                swal("Email-Id / Mobile No is Missing!!!! So You cannot proceed with Payment");
            }
        }
    }

})();