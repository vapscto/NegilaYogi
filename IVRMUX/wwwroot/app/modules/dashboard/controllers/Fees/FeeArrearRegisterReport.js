

(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeArrearRegisterReportController', FeeArrearRegisterReportController123)

    FeeArrearRegisterReportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function FeeArrearRegisterReportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.IsHiddenup = true;
        $scope.print_flag = false;
        $scope.route = false;
        $scope.bus = true;
        var fromdate = "";
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length != null && admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}


        // $scope.opq = {};
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        var institutionid = configsettings[0].mI_Id;
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
        // if (admfigsettings.length > 0) {
        //     var logopath = admfigsettings[0].asC_Logo_Path;
        //}


        //Fee Group Check all
        $scope.hrdallcheckfee = function () {
            var toggleStatus = $scope.checkallhrdfee;
            angular.forEach($scope.group, function (itm) {
                itm.fmG_Id_chk = toggleStatus;
            });
        }

        $scope.get_fees = function () {
            $scope.hrdallcheckfee = $scope.group.every(function (role) {
                return role.fmG_Id_chk;
            });
        };

        //Custom Group Check All
        $scope.hrdallcheckcustom = function () {
            var toggleStatus1 = $scope.checkallhrdcustom;
            angular.forEach($scope.custom, function (itm) {
                itm.fmgG_Id_chk = toggleStatus1;
            });
        }

        $scope.get_groups = function () {
            $scope.hrdallcheckcustom = $scope.custom.every(function (role) {
                return role.fmG_Id_chk;
            });
        };

        //Term Name Check All
        $scope.hrdallcheckterm = function () {
            var toggleStatus = $scope.checkallhrdterm;
            angular.forEach($scope.groupcount, function (role) {
                role.fmT_Id_chk = toggleStatus;
            });
        };

        $scope.togchkbxC = function () {
            $scope.checkallhrdterm = $scope.groupcount.every(function (role) {
                return role.fmT_Id_chk;
            });
        };
        //Ended

        if (grouporterm == "T") {
            $scope.grouportername = "Term Name"
            $scope.term = true;
            $scope.groupterm = false;
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name"
            $scope.groupterm = true;
            $scope.term = false;
        }

        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            $scope.bus = true;
            var pageid = 1;
            var data = {
                "reporttype": grouporterm,
            }

            apiService.create("FeeArrearRegisterReport/getalldetails123", data).
                then(function (promise) {

                    $scope.arrlist6 = promise.acayear;
                    $scope.classcount = promise.classlist;
                    $scope.busroute = promise.fillroute;
                    console.log($scope.busroute);
                    //$scope.grplst = promise.grouplist;
                    //$scope.studentlst = promise.admsudentslist1;
                    //$scope.termlst = promise.fillterms;

                    if (grouporterm == "T") {
                        angular.forEach(promise.customlist, function (tr) {
                            //tr.fmgG_Id_chk = true;
                            tr.fmgG_Id_chk = false;
                        })
                    }
                    else if (grouporterm == "G") {
                        angular.forEach(promise.customlist, function (tr) {
                            //tr.fmgG_Id_chk1 = true;
                            tr.fmgG_Id_chk1 = false;
                        })
                    }

                    $scope.custom = promise.customlist;

                    if (grouporterm == "T") {
                        angular.forEach(promise.grouplist, function (tr) {
                            //tr.fmG_Id_chk = true;
                            tr.fmG_Id_chk = false;
                        })
                    }
                    else if (grouporterm == "G") {
                        angular.forEach(promise.grouplist, function (tr) {
                            //tr.fmG_Id_chk1 = true;
                            tr.fmG_Id_chk1 = false;
                        })
                    }

                    $scope.groupcount = promise.fillmastergroup;
                    $scope.group = promise.grouplist;
                    // temp_grp_list = promise.grouplist;

                })
        }

        $scope.route_id = function () {

            if ($scope.route == "1") {
                $scope.trmR_Id = true;
                $scope.bus = false;
            }
            else if ($scope.route == "0") {
                $scope.trmR_Id = false;
                $scope.bus = true;
            }
        }

        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {

            return !$scope.termlst.some(function (options) {
                return options.trmids;
            });
        }

        $scope.isRequired = function () {

            return !$scope.grplst.some(function (options) {
                return options.grpids;
            });
        }
        $scope.due_date_div = false;
        $scope.due_date_check = function () {
            if ($scope.due_date == "1") {
                $scope.due_date_div = true;
            }
            else {
                $scope.due_date_div = false;
            }
        }

        $scope.onselectclass = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            apiService.create("FeeArrearRegisterReport/getsection", data).
                then(function (promise) {
                    $scope.sectioncount = promise.sectionlist;

                })
        }

        $scope.onselectsection = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "AMSC_Id": $scope.asmC_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            apiService.create("FeeArrearRegisterReport/getstudent", data).
                then(function (promise) {
                    $scope.studlist = promise.admsudentslist;
                })
        }





        $scope.get_groups = function () {
            var FMGG_Ids = [];
            if (grouporterm == "T") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk) {
                        FMGG_Ids.push(ty.fmgG_Id);
                    }
                })
            }
            else if (grouporterm == "G") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk1) {
                        FMGG_Ids.push(ty.fmgG_Id);
                    }
                })
            }

            if (FMGG_Ids.length > 0) {
                var data = {
                    "reporttype": grouporterm,
                    FMGG_Ids: FMGG_Ids
                }

                apiService.create("FeeDefaulterReport/get_groups", data).
                    then(function (promise) {

                        //$scope.groupcount = promise.fillmastergroup;
                        //$scope.custom = promise.customlist;
                        if (grouporterm == "T") {
                            angular.forEach(promise.grouplist, function (tr) {
                                tr.fmG_Id_chk = true;
                            })
                        }
                        else if (grouporterm == "G") {
                            angular.forEach(promise.grouplist, function (tr) {
                                tr.fmG_Id_chk1 = true;
                            })
                        }
                        $scope.group = promise.grouplist;
                    });
            }
            else if (FMGG_Ids.length == 0) {
                //$scope.group = temp_grp_list;
                $scope.group = [];
            }


        }


        $scope.rndind = "All";
        //$scope.rbnsforall = true;
        $scope.individual_Name_Regno = false;
        $scope.individual_Student = false;
        $scope.print_flag = true;
        $scope.Grid_view = false;
        $scope.submitted = false;
        $scope.route_id();
        $scope.showreport = function (asmaY_Id, asmcL_Id, asmC_Id, amst_Id, termlst, due_date, asd) {

            if ($scope.myForm.$valid) {
                var cnt_trm = 0;
                $scope.albumNameArraycolumn = [];
                //angular.forEach($scope.termlst, function (termlst) {
                //    if (!!termlst.trmids) {
                //        cnt_trm += 1;
                //        $scope.albumNameArraycolumn.push({
                //            columnID: termlst.fmT_Id,
                //            FMT_ID: termlst.fmT_Id,
                //            columnName: termlst.fmT_Name
                //        });
                //    }
                //})

                //$scope.groupArray = [];
                //angular.forEach($scope.grplst,function(grplst){
                //    if (!!grplst.grpids) $scope.groupArray.push({
                //        groupid: grplst.fmgG_Id,
                //        FMG_Id: grplst.fmgG_Id,
                //        groupname: grplst.fmg_groupname

                //    })
                //})



                $scope.albumNameArraycolumn1 = [];
                angular.forEach($scope.custom, function (custom1) {
                    if (!!custom1.selected) $scope.albumNameArraycolumn1.push({
                        columnID1: custom1.fmgG_Id

                    });
                })

                $scope.albumNameArraycolumn2 = [];
                angular.forEach($scope.group, function (group) {
                    if (!!group.selected) $scope.albumNameArraycolumn2.push({
                        columnID2: group.fmG_Id

                    });
                })

                $scope.albumNameArraycolumn3 = [];
                angular.forEach($scope.groupcount, function (groupcount) {
                    if (!!groupcount.selected) $scope.albumNameArraycolumn3.push({
                        columnID3: groupcount.fmT_Id

                    });
                })

                angular.forEach($scope.arrlist6, function (ty) {
                    if (ty.asmaY_Id == $scope.asmaY_Id) {
                        $scope.year = ty.asmaY_Year;
                    }
                })



                var FMGG_Ids = [];
                var FMG_Ids = [];
                var FMT_Ids = [];
                if (grouporterm == "T") {

                    angular.forEach($scope.custom, function (ty) {
                        if (ty.fmgG_Id_chk) {
                            FMGG_Ids.push(ty.fmgG_Id);
                        }
                    })


                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                    angular.forEach($scope.groupcount, function (ty) {
                        if (ty.fmT_Id_chk) {
                            FMT_Ids.push(ty.fmT_Id);
                            cnt_trm += 1;
                        }
                    })
                }
                else if (grouporterm == "G") {
                    angular.forEach($scope.custom, function (ty) {
                        if (ty.fmgG_Id_chk1) {
                            FMGG_Ids.push(ty.fmgG_Id);
                        }
                    })


                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk1) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                }

                if ($scope.route == 1) {

                    $scope.trmR_Id = $scope.asd;
                }
                else {
                    $scope.trmR_Id = 0;
                }



                if ($scope.type == 'Ind' && $scope.route==0) {
                    var sid = $scope.AMST_Id.amst_Id
                }
                else {
                    var sid = 0;
                }





                var data = {
                    "asmay_id": $scope.asmaY_Id,
                    "fillclasflg": $scope.asmcL_Id,
                    "fillseccls": $scope.asmC_Id,
                    //"Amst_Id": $scope.studentid,
                    "Amst_Id": sid,
                    TempararyArrayhEADListnew1: $scope.albumNameArraycolumn1,
                    TempararyArrayhEADListnew2: $scope.albumNameArraycolumn2,
                    TempararyArrayhEADListnew3: $scope.albumNameArraycolumn3,
                    FMGG_Ids: FMGG_Ids,
                    FMG_Ids: FMG_Ids,
                    FMT_Ids: FMT_Ids,
                    "term_group": grouporterm,
                    "trmR_Id": $scope.trmR_Id
                }


                apiService.create("FeeArrearRegisterReport/getreport", data).then(function (promise) {

                    $scope.receiptlist = promise.headlist;
                    $scope.groupname = "";
                    $scope.userid = promise.userid;
                    //angular.forEach($scope.custom, function (opq) {
                    //    if (opq.fmgG_Id_chk1 || opq.fmgG_Id_chk == true) {
                    //        $scope.groupname += opq.fmg_groupname + " ";
                    //    }
                    //})

                    for (var q = 0; q < $scope.custom.length; q++) {
                        if (q==0) {
                           // $scope.groupname += opq.fmg_groupname;
                            $scope.groupname = $scope.custom[q].fmg_groupname;
                        }
                    }


                    //student not paid any fees list - studentlist
                    if (promise.studentlist != null && promise.studentlist != "") {
                        $scope.text_list = promise.studentlist;
                        var not_paid_list = [];

                        angular.forEach($scope.text_list, function (stuw) {
                            var n_stu_id = stuw.AMST_Id;
                            if (not_paid_list.length == 0) {
                                //var n_stu_id = stuw.AMST_Id;
                                var n_stu_cnt = 0;
                                angular.forEach($scope.text_list, function (stuw1) {
                                    if (stuw1.AMST_Id == n_stu_id) {
                                        n_stu_cnt += 1;
                                    }
                                })
                                if (cnt_trm == n_stu_cnt) {
                                    for (var q = 0; q < $scope.text_list.length; q++) {
                                        if ($scope.text_list[q].AMST_Id == n_stu_id) {
                                            not_paid_list.push($scope.text_list[q]);
                                        }
                                    }
                                }
                            }
                            else if (not_paid_list.length > 0) {
                                var alrdy_n_stu_cnt = 0;
                                angular.forEach(not_paid_list, function (N_s) {
                                    if (N_s.AMST_Id == n_stu_id) {
                                        alrdy_n_stu_cnt += 1;
                                    }
                                    // alrdy_n_stu_cnt += 1;

                                })
                                if (alrdy_n_stu_cnt == 0) {
                                    //  var n_stu_id = stuw.AMST_Id;
                                    var n_stu_cnt = 0;
                                    angular.forEach($scope.text_list, function (stuw1) {
                                        if (stuw1.AMST_Id == n_stu_id) {
                                            n_stu_cnt += 1;
                                        }
                                    })
                                    if (cnt_trm == n_stu_cnt) {
                                        for (var q = 0; q < $scope.text_list.length; q++) {
                                            if ($scope.text_list[q].AMST_Id == n_stu_id) {
                                                not_paid_list.push($scope.text_list[q]);
                                            }
                                        }
                                    }
                                }
                            }


                        })
                        promise.studentlist = not_paid_list;
                        $scope.term_list1 = [];
                        var temp_recp_list1 = promise.studentlist;
                        for (var m = 0; m < promise.studentlist.length; m++) {
                            var stu_id = promise.studentlist[m].AMST_Id;
                            var already_cnt = 0;
                            angular.forEach($scope.term_list1, function (itm1) {
                                if (itm1.stu_id == stu_id) {
                                    already_cnt += 1;
                                }
                            })
                            if (already_cnt == 0) {
                                $scope.temp_stu_rpt1 = [];
                                angular.forEach(temp_recp_list1, function (itm) {

                                    if (itm.AMST_Id == stu_id) {
                                        $scope.temp_stu_rpt1.push(itm);
                                    }

                                })
                                $scope.term_list1.push({ stu_id: stu_id, stu_rpt: $scope.temp_stu_rpt1 });
                            }

                        }
                    }
                    //else {
                    //    swal("No Record Found");
                    //    $scope.Grid_view = false;
                    //    $scope.print_flag = true;
                    //}

                    //till here -studentlist



                    //students made payment - admsudentslist1 -- kiran
                    ////$scope.groupname = "";
                    ////angular.forEach($scope.custom, function (opq) {
                    ////    if (opq.fmgG_Id_chk1 == true) {
                    ////        $scope.groupname += opq.fmg_groupname+ " " ;
                    ////    }
                    ////})


                    if (promise.admsudentslist1 != null && promise.admsudentslist1 != "") {
                        $scope.term_list1 = [];
                        var students_lists = [];
                        var std_empt = [];
                        var pend = 0;
                        for (var l = 0; l < promise.admsudentslist1.length; l++) {
                            var admsudentslist = [];
                            var st_id = promise.admsudentslist1[l].AMST_Id;
                            var st_id_cnt = 0;
                            angular.forEach(students_lists, function (itm1) {
                                if (itm1.st_id == st_id) {
                                    st_id_cnt += 1;
                                    //pend += itm1.Balance;
                                }
                            })
                            if (st_id_cnt == 0) {
                                angular.forEach(promise.admsudentslist1, function (itm) {

                                    if (itm.AMST_Id == st_id) {
                                        admsudentslist.push(itm);
                                        pend += itm.Balance;
                                    }

                                })
                                if (pend > 0) {
                                    students_lists.push({ st_id: st_id, admsudentslist: admsudentslist });
                                    pend = 0;
                                }
                                else {
                                    std_empt.push({ st_id: st_id, admsudentslist: admsudentslist });
                                }

                            }

                        }





                        for (var s = 0; s < students_lists.length; s++) {
                            $scope.due_details = promise.studentdetails;
                            var stu_id = students_lists[s].st_id;
                            $scope.term_list = [];
                            var temp_recp_list = students_lists[s].admsudentslist;
                            var totcount = 0;
                            for (var m = 0; m < students_lists[s].admsudentslist.length; m++) {
                                //  if(promise.admsudentslist[m].fyp_receipt_no!=" ")
                                //  {
                                var term_id = students_lists[s].admsudentslist[m].FMT_Id;
                                var term_cnt = 0;
                                angular.forEach(temp_recp_list, function (itm) {

                                    if (itm.FMT_Id == term_id) {
                                        term_cnt += 1;
                                    }
                                })
                                if (term_cnt == 1) {
                                    $scope.term_list.push(students_lists[s].admsudentslist[m]);
                                }
                                else if (term_cnt > 1) {
                                    if (students_lists[s].admsudentslist[m].fyp_receipt_no != " ") {

                                        var main_cnt = 0;
                                        angular.forEach($scope.term_list, function (itm) {

                                            if (itm.FMT_Id == term_id) {
                                                main_cnt += 1;
                                            }
                                        })
                                        if (main_cnt == 0) {
                                            var bal = 0;
                                            var paid = 0;
                                            var totbala = 0;
                                            var totbalb = 0;
                                            var totbal = 0;
                                            var totpaida = 0;
                                            var totpaidb = 0;
                                            var totpaid = 0;
                                            var receipt_cnt = 0;
                                            var OB = 0;
                                            var refund = 0;
                                            var excess = 0;
                                            var adjust = 0;
                                            var waived = 0;
                                            var totOBa = 0;
                                            var totOBb = 0;
                                            var totOB = 0;
                                            var totrefunda = 0;
                                            var totrefundb = 0;
                                            var totrefund = 0;
                                            var totexcessa = 0;
                                            var totexcessb = 0;
                                            var totexcess = 0;
                                            var totadjusta = 0;
                                            var totadjustb = 0;
                                            var totadjust = 0;
                                            var totwaiveda = 0;
                                            var totwaivedb = 0;
                                            var totwaived = 0;



                                            angular.forEach(temp_recp_list, function (itm) {

                                                if (itm.FMT_Id == term_id) {
                                                    if (receipt_cnt > 0) {
                                                        if (itm.fyp_receipt_no == " ") {
                                                            if (itm.Balance != 0) {
                                                                bal += itm.Balance;
                                                                paid += itm.paid;
                                                                OB += itm.opening_balance;
                                                                refund += itm.refunded;
                                                                excess += itm.Excess;
                                                                adjust += itm.adjusted;
                                                                waived += itm.Waived;
                                                            }

                                                        }
                                                        totbala += bal;
                                                        totpaida += paid;
                                                        totOBa += OB;
                                                        totrefunda += refund;
                                                        totexcessa += excess;
                                                        totadjusta += adjust;
                                                        totwaiveda += waived;

                                                    }

                                                    else if (receipt_cnt == 0) {


                                                        if ((bal != itm.Balance && paid != itm.paid) || bal == 0) {
                                                            bal += itm.Balance;
                                                            paid += itm.paid;
                                                            OB += itm.opening_balance;
                                                            refund += itm.refunded;
                                                            excess += itm.Excess;
                                                            adjust += itm.adjusted;
                                                            waived += itm.Waived;
                                                        }


                                                        if (itm.fyp_receipt_no != " " || itm.Balance == 0) {
                                                            receipt_cnt += 1;

                                                        }

                                                    }

                                                    totbalb += bal;
                                                    totpaidb += paid;
                                                    totrefundb += refund;
                                                    totexcessb += excess;
                                                    totadjustb += adjust;
                                                    totwaivedb += waived;

                                                }
                                                totbal = totbala + totbalb;
                                                totpaid = totpaida + totpaidb;
                                                totOB = totOBa + totOBb;
                                                totrefund = totrefunda + totrefundb;
                                                totexcess = totexcessa + totexcessb;
                                                totadjust = totadjusta + totadjustb;
                                                totwaived = totwaiveda + totwaivedb;

                                            })
                                            var date2 = "";
                                            var tempdate = "";
                                            //angular.forEach(temp_recp_list, function (itm) {
                                            //    if (itm.FMT_Id == term_id) {
                                            //        if (date2 == "") {
                                            //            date2 = itm.date;
                                            //        }
                                            //        else {
                                            //            if (itm.date > date2) {
                                            //                date2 = itm.date;
                                            //            }
                                            //        }
                                            //    }
                                            //})
                                            angular.forEach(temp_recp_list, function (itm) {
                                                if (itm.FMT_Id == term_id) {
                                                    if (date2 == "") {
                                                        date2 = itm.date;
                                                    }
                                                    else {
                                                        if (totcount == 0) {
                                                            if (itm.date > date2) {
                                                                date2 = date2;
                                                                tempdate = date2;
                                                                totcount += 1;
                                                            }
                                                        }
                                                        else {
                                                            date2 = itm.date;
                                                            tempdate = date2;
                                                        }

                                                    }
                                                }
                                            })
                                            var out = students_lists[s].admsudentslist[m];
                                            angular.forEach(temp_recp_list, function (itm) {
                                                if (itm.date == date2 && itm.FMT_Id == term_id) {
                                                    out = itm;
                                                }
                                            })

                                            out.Balance = bal;
                                            out.paid = paid;
                                            out.opening_balance = OB;
                                            out.refunded = refund;
                                            out.Excess = excess;
                                            out.adjusted = adjust;
                                            out.Waived = waived;


                                            $scope.term_list.push(out);
                                        }


                                    }
                                }
                                //  }

                            }
                            angular.forEach($scope.term_list, function (itm) {

                                angular.forEach(temp_recp_list, function (itm1) {
                                    if (itm.FMT_Id == itm1.FMT_Id) {
                                        main_cnt += 1;
                                    }
                                })
                            })
                            var admsudentslist_xyz = $scope.term_list;

                            var temp_list = $scope.term_list;
                            for (var x = 0; x < admsudentslist_xyz.length; x++) {
                                if (admsudentslist_xyz[x].fyp_receipt_no != " ") {
                                    var res = Number(admsudentslist_xyz[x].fyp_receipt_no);
                                    if (Number(admsudentslist_xyz[x].fyp_receipt_no) != NaN && !isNaN(admsudentslist_xyz[x].fyp_receipt_no)) {
                                        temp_list.splice(x, 1);
                                    }
                                }


                            }

                            var balance_stu = 0;
                            var paid_stu = 0;
                            var total = 0;
                            var OB_stu = 0;
                            var refund_stu = 0;
                            var excess_stu = 0;
                            var waived_stu = 0;
                            var adjusted_stu = 0;

                            angular.forEach(temp_list, function (e) {
                                balance_stu += e.Balance;
                                paid_stu += e.paid;
                                total += e.Total;
                                OB_stu += e.opening_balance;
                                refund_stu += e.refunded;
                                excess_stu += e.Excess;
                                adjusted_stu += e.adjusted;
                                waived_stu += e.Waived


                            })
                            angular.forEach(temp_list, function (e) {
                                if (e.paid == 0 && e.Balance != 0) {
                                    e.fyp_receipt_no = "";
                                    e.date = "";
                                }

                            })

                            //$scope.term_list1.push({ stu_id: stu_id, bal: balance_stu, paid: paid_stu, Total: total, stu_rpt: temp_list });
                            $scope.term_list1.push({ stu_id: stu_id, bal: balance_stu, paid: paid_stu, opening_balance: OB_stu, refunded: refund_stu, Excess: excess_stu, adjusted: adjusted_stu, Waived: waived_stu, Total: total, stu_rpt: temp_list });

                        }


                        angular.forEach($scope.term_list1, function (stu) {
                            angular.forEach(stu.stu_rpt, function (trm) {
                                angular.forEach($scope.receiptlist, function (final_trms) {
                                    if (stu.stu_id == final_trms.AMST_Id && final_trms.FMT_Id == trm.FMT_Id) {
                                        trm.fyp_receipt_no = final_trms.customflag;
                                        trm.date = final_trms.FYP_Date;
                                    }
                                })

                            })
                        })

                    }





                    if (promise.studentlist != "" || promise.studentlist != null) {

                        angular.forEach(promise.studentlist, function (s_n) {
                            var stu_n_id = s_n.AMST_Id;
                            var temp_list_n = [];
                            angular.forEach(promise.studentlist, function (s_n1) {
                                if (s_n1.AMST_Id == stu_n_id) {
                                    temp_list_n.push(s_n1);
                                }

                            })

                            var balance_stu = 0;
                            var paid_stu = 0;
                            var total = 0;
                            angular.forEach(temp_list_n, function (e) {
                                balance_stu += e.Balance;
                                paid_stu += e.paid;
                                total += e.Total;
                                OB_stu += e.opening_balance;
                                refund_stu += e.refunded;
                                excess_stu += e.Excess;
                                adjusted_stu += e.adjusted;
                                waived_stu += e.Waived

                            })
                            angular.forEach(temp_list_n, function (e) {
                                if (e.paid == 0 && e.Balance != 0) {
                                    e.fyp_receipt_no = "";
                                    e.date = "";
                                }

                            })
                            var stu_n_cnt_alrdy = 0;
                            angular.forEach($scope.term_list1, function (final) {
                                if (final.stu_id == stu_n_id) {
                                    stu_n_cnt_alrdy += 1;
                                }
                            })
                            if (stu_n_cnt_alrdy == 0) {
                                //$scope.term_list1.push({ stu_id: stu_n_id, bal: balance_stu, paid: paid_stu, Total: total, stu_rpt: temp_list_n });
                                $scope.term_list1.push({ stu_id: stu_n_id, bal: balance_stu, paid: paid_stu, opening_balance: OB_stu, refunded: refund_stu, Excess: excess_stu, adjusted: adjusted_stu, Waived: waived_stu, Total: total, stu_rpt: temp_list_n });
                            }

                        })
                    }



                    $scope.Feearrear = $scope.term_list1;
                    $scope.year1 = promise.period;
                       if (promise.reportdatelist.length > 0) {
                         $scope.date = promise.reportdatelist[0].dated;
                    }
                    $scope.months = promise.month;
                    $scope.atotA = $scope.atotalA($scope.Feearrear); //balance
                    $scope.imgname = promise.fillbusroutestudents;

                    var from = "";
                    var to = "";
                    var year = "";

                    $scope.Grid_view = true;
                    if ($scope.userid == 364) {
                        $scope.userids = true;
                        $scope.useridn = false;
                        $scope.glsuseridn = false;
                        // pdss = 'trpt'
                    }
                    else if ($scope.userid == 362) {
                        $scope.userids = false;
                        $scope.useridn = false;
                        $scope.glsuseridn = true;
                    }

                    else {
                        $scope.useridn = true;
                        $scope.userids = false;
                        $scope.glsuseridn = false;

                    }
                    $scope.print_flag = false;
                    angular.forEach($scope.Feearrear, function (value, key) {

                        $scope.FirstName = value.AMST_FirstName;
                        $scope.FromDate = new Date();
                        $scope.class = value.ASMCL_ClassName;
                        $scope.section = value.ASMC_SectionName;
                        $scope.installment = value.FMT_Name;
                        $scope.head = value.FMH_FeeName;
                        $scope.paid_amount = value.FSS_PaidAmount;
                        $scope.bal_amount = value.balance;
                        $scope.admno = value.AMST_AdmNo;
                        $scope.receipt_no = value.fyp_receipt_no;
                        if ($scope.userid == 364) {
                            $scope.route = true;
                        }


                    });
                    console.log($scope.Feearrear);



                })
            }
            else {
                $scope.submitted = true;

            }
        };

        $scope.getTotal126 = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.paidAmt;
            });
            return total;
        };


        $scope.onselectmodeof = function () {

            var VALU;
            if ($scope.BRcheck == "1") {
                VALU = $scope.CMR_Id;
            }
            else {
                VALU = 'Uncheck';
            }
            var data = {
                "filterinitialdata": $scope.filterdata,
                "fillbusroutestudents": VALU,
                "fillseccls": $scope.sectiondrp,
                "fillclasflg": $scope.clsdrp,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeArrearRegisterReport/getgroupmappedheads", data).
                then(function (promise) {

                    $scope.studentlst = promise.admsudentslist1;
                    $scope.Amst_Id = "";
                })
        };




        $scope.onclickloaddata = function () {
            if ($scope.rndind == "All") {
                //$scope.rbnsforall = true;
                $scope.individual_Name_Regno = false;
                //$scope.rbnsNameforall = true;    
                $scope.individual_Student = false;
            }
            else if ($scope.rndind == "Individual") {
                //$scope.rbnsforall = false;
                $scope.individual_Name_Regno = true;
                // $scope.rbnsNameforall = false;
                $scope.individual_Student = true;
            }
        };

        $scope.BusRoute_section = true;
        $scope.BusRoute_class = true;
        $scope.busroutedisable = true;
        $scope.Bus_Route = false;

        $scope.Clearid = function () {

            $state.reload();
            $scope.loaddata();
        }



        $scope.onclickloaddataCS = function () {


            if ($scope.BRcheck == "1") {

                $scope.BusRoute_section = false;

                $scope.BusRoute_class = false;
                $scope.busroutedisable = false;
                $scope.Bus_Route = true;

            }
            else {

                $scope.BusRoute_section = true;

                $scope.BusRoute_class = true;
                $scope.busroutedisable = true;
                $scope.Bus_Route = false;
            }

        };




        $scope.printToCart = function () {

            var pdss = "";
            if ($scope.userid == 364) {
                $scope.route = true;
                pdss = 'trpt'
            }
            else if ($scope.userid == 362) {
                $scope.route = false;
                pdss = 'glstrpt'
            }
            else {
                $scope.route = false;
                pdss = 'printrcp'
            }

            var innerContents = document.getElementById(pdss).innerHTML;

            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReport/BArrearReportPdf.css"/>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        //to check paid
        $scope.atotalB = function (e) {
            var atotalc = 0;
            angular.forEach($scope.Feearrear, function (e) {
                //atotalc += e.FSS_PaidAmount;
                atotalc += e.paid;
            });
            return atotalc;
        };
        $scope.atotalA = function (e) {
            var atotalc = 0;
            angular.forEach($scope.temp_stu_rpt, function (e) {
                //atotalc += e.FSS_ToBePaid; 
                atotalc += e.Balance;
            });
            return atotalc;
        };
        $scope.a_concession = function (e) {
            var atotalc = 0;
            angular.forEach($scope.Feearrear, function (e) {
                atotalc += e.FSS_ConcessionAmount;
            });
            return atotalc;
        };
        $scope.a_excess = function (e) {
            var atotalc = 0;
            angular.forEach($scope.Feearrear, function (e) {
                atotalc += e.FSS_ExcessPaidAmount;
            });
            return atotalc;
        };
        $scope.a_fine = function (e) {
            var atotalc = 0;
            angular.forEach($scope.Feearrear, function (e) {
                atotalc += e.FSS_FineAmount;
            });
            return atotalc;
        };




        $scope.is_optionrequired_trm_cg = function () {

            return !$scope.custom.some(function (options) {
                return options.fmgG_Id_chk;
            });
        }
        $scope.is_optionrequired_trm_grp = function () {

            return !$scope.group.some(function (options) {
                return options.fmG_Id_chk;
            });
        }
        $scope.is_optionrequired_trm_trm = function () {

            return !$scope.groupcount.some(function (options) {
                return options.fmT_Id_chk;
            });
        }
        $scope.is_optionrequired_groupterm_cg = function () {

            //if ($scope.custom_check == true) {
            return !$scope.custom.some(function (options) {
                return options.fmgG_Id_chk1;
            });
            // }

        }
        $scope.is_optionrequired_groupterm_grp = function () {

            // if ($scope.group_check==true) {
            return !$scope.group.some(function (options) {
                return options.fmG_Id_chk1;
            });
            // }

        }




    }
})();

