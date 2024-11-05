
(function () {
    'use strict';
    angular
.module('app')
.controller('FeeSummarizedReportController', FeeSummarizedReportController)
    FeeSummarizedReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile']
    function FeeSummarizedReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {

        $scope.obj = {};
        $scope.hide_anuual = true;
        $scope.hide_others = true;
        $scope.hide_others_bbkv = true;
        $scope.hide_anuual_baldwin = true;
        $scope.hide_others_bladwin = true;
        $scope.checkallhrd1 = true;
        $scope.checkallhrd2 = true;
        $scope.checkallhrd3 = true;

        $scope.groupterm = false;
        $scope.term = false;

        $scope.obj = {};
        $scope.usr = {};
        $scope.obj.allgrouporterm = true;

        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        if (grouporterm == "T") {
            $scope.grouportername = "Term Name"
            $scope.groupterm = false;
            $scope.term = true;
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name"
            $scope.groupterm = true;
            $scope.term = false;
        }

        $scope.evens = ["2", "4", "6", "8", "10", "12", "14"];
        $scope.odds = ["1", "3", "5", "7", "9", "11", "13"];


        $scope.hrdallcheck1 = function () {
            var toggleStatus1 = $scope.checkallhrd1;
            angular.forEach($scope.custom, function (itm) {
                itm.selected = toggleStatus1;
            });
        }
        $scope.hrdallcheck2 = function () {
            var toggleStatus = $scope.checkallhrd2;
            angular.forEach($scope.groupcount, function (itm) {
                itm.selected2 = toggleStatus;
            });
        }
        $scope.hrdallcheck3 = function () {
            var toggleStatus2 = $scope.checkallhrd3;
            angular.forEach($scope.grouplist, function (itm) {
                itm.selected3 = toggleStatus2;
            });
        }
        //-------------PageLaod 
        $scope.loaddata = function () {
          
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            $scope.groupterm = false;
            $scope.term = false;
            $scope.hideallindi = false;
            $scope.hidestu = true;
            var pageid = 1;
            // $scope.rpttyp = "Annual";
            //  $scope.onclickloaddataclass();
            var data = {
                "reporttype": grouporterm,
            }
            apiService.create("FeeSummarizedReport/getalldetails", data).
        then(function (promise) {
            $scope.arrlist6 = promise.adcyear;
            $scope.classcount = promise.fillclass;
            $scope.groupcount = promise.fillmastergroup;
            $scope.headlst = promise.fillmasterhead;
            //  $scope.termlst = promise.fillterms;
            $scope.termlst = promise.fillmastergroup;
            if (grouporterm == "T") {
                angular.forEach(promise.customlist, function (tr) {
                    tr.fmgG_Id_chk = true;
                })
            }
            else if (grouporterm == "G") {
                angular.forEach(promise.customlist, function (tr) {
                    tr.fmgG_Id_chk1 = true;
                })
            }
            $scope.custom = promise.customlist;
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
            $scope.groupcount = promise.grouplist;
            $scope.grouplist = promise.grouplist;

            angular.forEach($scope.grouplist, function (itm) {
                itm.fmG_Id_chk = true;
            });
            angular.forEach($scope.headlst, function (itm) {
                itm.checkedheadlst = true;
            });

        })
        }

        $scope.toggleAllgrouporterm = function (grouplist, headlst, obj) {
            var toggleStatus = obj.allgrouporterm;
            if (obj.allgrouporterm == true) {
                angular.forEach($scope.grouplist, function (itm) {
                    itm.fmG_Id_chk = toggleStatus;
                });
                angular.forEach($scope.headlst, function (itm) {
                    itm.checkedheadlst = toggleStatus;
                });
            }
            else if (obj.allgrouporterm == false) {
                angular.forEach($scope.grouplist, function (itm) {
                    itm.fmG_Id_chk = toggleStatus;
                });
                angular.forEach($scope.headlst, function (itm) {
                    itm.checkedheadlst = toggleStatus;
                });
            }
        }

        //---------------PageLaod  END
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }

        //----------------Class & Section Change 
        $scope.onselectclass = function (asmcL_Id) {
            
            $scope.asmS_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
           
            apiService.create("FeeSummarizedReport/getsection", data).
               then(function (promise) {
                   $scope.sectioncount = promise.fillsection;
                   for (var i = 0; i < $scope.classcount.length; i++) {
                       if (asmcL_Id == $scope.classcount[i].asmcL_Id) {
                           $scope.seledcls = $scope.classcount[i].asmcL_ClassName;
                       }
                   }
                   //  $scope.arrlstinst1 = promise.fillinstallment;
               })
        }

        $scope.onselectsection = function (amsC_Id) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "AMSC_Id": $scope.asmS_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
          
            apiService.create("FeeSummarizedReport/getstudent", data).
               then(function (promise) {
                   $scope.studentlst = promise.fillstudent;
                   //  $scope.arrlstinst1 = promise.fillinstallment;
                   for (var i = 0; i < $scope.sectioncount.length; i++) {
                       if (amsC_Id == $scope.sectioncount[i].amsC_Id) {
                           $scope.seledsect = $scope.sectioncount[i].asmc_sectionname;
                       }
                   }
               })
        }


        $scope.isOptionsRequired1 = function () {
            return !$scope.grouplist.some(function (options) {
                    return options.checkedgrplst;
                });
        }

      



        $scope.firstfnc = function (vlobj) {
            debugger;
            if (vlobj.checkedgrplst == true) {
                angular.forEach($scope.grouplist, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = true;
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplist, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = false;
                            }
                        });
                    }
                });
            }
        }

        $scope.secfnc = function (vlobj1) {
          
            debugger;
            for (var s = 0; s < $scope.grouplist.length; s++) {
                if (vlobj1.fmG_Id == $scope.grouplist[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlst.length; t++) {
                        if (vlobj1.fmG_Id == $scope.headlst[t].fmG_Id) {
                            if ($scope.headlst[t].checkedheadlst == false) {
                                $scope.grouplist[s].checkedgrplst = false;
                            }
                            else {
                                $scope.grouplist[s].checkedgrplst = true;
                                break;
                            }
                        }
                    }
                }
            }
        }




        //---------------Annual & Term Wise Selection Change
        $scope.onclickloaddataclass = function () {
            if ($scope.rpttyp == "Annual") {

                $scope.hideallindi = false;
                $scope.submitted = false;
                $scope.groupterm = false;
                $scope.term = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            else if ($scope.rpttyp == "Others") {
                if (grouporterm == "T") {
                    $scope.groupterm = false;
                    $scope.term = true;
                }
                else if (grouporterm == "G") {
                    $scope.groupterm = true;
                    $scope.term = false;
                }

                $scope.hideallindi = true;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.obj.studentid = "";
            $scope.fmT_Id = [];
            $scope.sectioncount = [];
            $scope.hide_anuual = true;
            $scope.hide_others = true;
            $scope.hide_anuual_baldwin = true;
            $scope.hide_others_bladwin = true;
            // $scope.Feesummarized.length = 0;
        };
        //----------------all & indi Selection Change
        $scope.onclickallindi = function () {
            if ($scope.rpttypai == "all") {
                $scope.hidestu = true;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            else if ($scope.rpttypai == "Ind") {
                $scope.hidestu = false;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.obj.studentid = "";
            $scope.fmT_Id = [];
            $scope.sectioncount = [];
            $scope.hide_anuual = true;
            $scope.hide_others = true;
            $scope.hide_anuual_baldwin = true;
            $scope.hide_others_bladwin = true;

            //  $scope.Feesummarized.length = 0;
        }

        $scope.hide_anuual = true;
        $scope.hide_others = true;
        $scope.submitted = false;
        //-------------------Get Custom and Group Fee
        $scope.get_groups = function () {
            debugger;
            var FMGG_Idss = [];
            if (grouporterm == "T") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk) {
                        FMGG_Idss.push({ "fmgG_Id": ty.fmgG_Id });
                    }
                })
            }
            else if (grouporterm == "G") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk) {
                        FMGG_Idss.push({ "fmgG_Id": ty.fmgG_Id });
                    }
                })
            }

            if (FMGG_Idss.length > 0) {
                var data = {
                    "reporttype": grouporterm,
                    FMGG_Idss: FMGG_Idss
                }
                apiService.create("FeeSummarizedReport/get_groups", data).
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
                             tr.fmG_Id_chk = true;
                         })
                     }
                     $scope.group = promise.grouplist;
                 });

            }
            else if (FMGG_Idss.length == 0) {
                //$scope.group = temp_grp_list;
                $scope.group = [];
            }
        }


        //-------------------Reports
        $scope.showreport = function (asmaY_Id, asmcL_Id, asmC_Id, amst_Id, termlst, chkbxfinal, headlst, grouplist) {
         

            $scope.checkboxchcked = [];
            $scope.valsgroup = [];
            $scope.valshead = [];
            var FMH_Ids = [];
            //----get Custom,Group & Term data

            //for (var t = 0; t < grouplist.length; t++) {
            //    if (grouplist[t].checkedgrplst == true) {
            //        $scope.valsgroup.push(grouplist[t]);
            //    }
            //}

            //for (var u = 0; u < $scope.headlst.length; u++) {
            //        if ($scope.headlst[u].checkedheadlst == true) {
            //            $scope.valshead.push($scope.headlst[u]);
            //            FMGG_Idss.push({ "fmH_Id": ty.fmH_Id });
            //    }
            //}

            $scope.groupname = "";
            angular.forEach($scope.groupcount, function (opq) {
                if (opq.fmG_Id_chk || opq.fmG_Id_chk == true) {
                    $scope.groupname += opq.fmG_GroupName + " ";
                }
            })

            angular.forEach($scope.headlst, function (ty) {
                if (ty.checkedheadlst) {
                    FMH_Ids.push(ty.fmH_Id);
                }
            })


            $scope.albumNameArraycolumn1 = [];
            angular.forEach($scope.custom, function (custom1) {
                if (!!custom1.fmgG_Id_chk) $scope.albumNameArraycolumn1.push({
                    columnID1: custom1.fmgG_Id
                });
            })
            $scope.albumNameArraycolumn2 = [];
            angular.forEach($scope.groupcount, function (group) {
                if (!!group.fmG_Id_chk) $scope.albumNameArraycolumn2.push({
                    columnID2: group.fmG_Id

                });
            })
            $scope.albumNameArraycolumn3 = [];
            angular.forEach($scope.termlst, function (termlst) {

                if (!!termlst.fmT_Id) $scope.albumNameArraycolumn3.push({
                    columnID3: termlst.fmT_Id

                });
            })
          
            var FMGG_Idss = [];
            var FMG_Idss = [];
            var FMT_Idss = [];
            if (grouporterm == "T") {
                FMGG_Idss.length = 0;
                FMG_Idss.length = 0;
                FMT_Idss.length = 0;
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk) {
                        FMGG_Idss.push({ "fmgG_Id": ty.fmgG_Id });
                    }
                })

                angular.forEach($scope.groupcount, function (ty) {
                    if (ty.fmG_Id_chk) {
                        FMG_Idss.push({ "fmG_Id": ty.fmG_Id });
                    }
                })
                angular.forEach($scope.termlst, function (ty) {
                    if (ty.fmT_Id_check) {
                        FMT_Idss.push({ "fmT_Id": ty.fmT_Id });
                    }
                })
            }
            else if (grouporterm == "G") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk) {
                        FMGG_Idss.push({ "fmgG_Id": ty.fmgG_Id });
                    }
                })
                angular.forEach($scope.groupcount, function (ty) {
                    if (ty.fmG_Id_chk1) {
                        FMG_Idss.push({ "fmG_Id": ty.fmG_Id });
                    }
                })
            }

            if ($scope.myForm.$valid) {
                if ($scope.rpttyp == "Annual" && $scope.rpttypai == "Ind") {

                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "AMSC_Id": $scope.asmS_Id,
                        "Amst_Id": $scope.obj.studentid,
                        "temparrayinst1": "",
                        "stype": $scope.rpttyp,
                        "allindi": $scope.rpttypai,
                       // savegrplst: grouplist,
                        "FMH_Ids": FMH_Ids,

                    }
                }
                else if ($scope.rpttyp == "Annual" && $scope.rpttypai == "all") {

                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "AMSC_Id": $scope.asmS_Id,
                        "temparrayinst1": "",
                        "stype": $scope.rpttyp,
                        "allindi": $scope.rpttypai,
                        //savegrplst: grouplist,
                        "FMH_Ids": FMH_Ids,


                    }
                }
                else if ($scope.rpttyp == "Others" && $scope.rpttypai == "Ind") {
                    //$scope.checkboxchcked = [];
                    //for (var j = 0; j < chkbxfinal.length; j++) {
                    //    for (var i = 0; i < termlst.length; i++) {
                    //        if (chkbxfinal[j] == termlst[i].fmT_Id) {
                    //            $scope.checkboxchcked.push(termlst[i]);
                    //        }
                    //    }
                    //}                  

                    console.log($scope.checkboxchcked);
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "AMSC_Id": $scope.asmS_Id,
                        "Amst_Id": $scope.obj.studentid,
                        // "temparrayinst1": $scope.checkboxchcked,
                        "stype": $scope.rpttyp,
                        "allindi": $scope.rpttypai,
                        //"FMG_Id": $scope.fmG_Id,
                        FMG_Idss: FMG_Idss,
                        FMT_Idss: FMT_Idss,
                        FMGG_Idss: FMGG_Idss,
                        FMH_Ids: FMH_Ids,
                    }
                }
                else if ($scope.rpttyp == "Others" && $scope.rpttypai == "all") {
                    //$scope.checkboxchcked = [];
                    //for (var j = 0; j < chkbxfinal.length; j++) {
                    //    for (var i = 0; i < termlst.length; i++) {
                    //        if (chkbxfinal[j] == termlst[i].fmT_Id) {
                    //            $scope.checkboxchcked.push(termlst[i]);
                    //        }
                    //    }
                    //}
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "AMSC_Id": $scope.asmS_Id,
                        // "temparrayinst1": $scope.checkboxchcked,
                        "stype": $scope.rpttyp,
                        "allindi": $scope.rpttypai,
                        //    "FMG_Id": $scope.fmG_Id,
                        FMG_Idss: FMG_Idss,
                        FMT_Idss: FMT_Idss,
                        FMGG_Idss: FMGG_Idss,
                        FMH_Ids: FMH_Ids,
                    }
                }
                apiService.create("FeeSummarizedReport/getreport", data).then(function (promise) {

                    if (promise.feesummlist != null && promise.feesummlist != "") {
                        var stu_id = "";
                        $scope.mi_id = promise.miid;
                        $scope.report_flag = promise.ftp_remarks;
                        if ($scope.report_flag == "15") {
                            $scope.hide_anuual_baldwin = true;
                            $scope.hide_others_bladwin = true;
                            $scope.hide_others_bbkv = true;
                            if ($scope.rpttyp == "Annual") {
                                $scope.hide_anuual = false;
                                $scope.hide_others = true;
                                $scope.hide_others_bbkv = true;
                            }
                            else {
                                $scope.hide_anuual = true;
                                $scope.hide_others_bbkv = true;
                                $scope.hide_others = false;
                            }
                        }
                        else if ($scope.report_flag == "5") {
                            $scope.hide_anuual = true;
                            $scope.hide_others = true;
                            $scope.hide_others_bbkv = true;
                            if ($scope.rpttyp == "Annual") {
                                $scope.hide_anuual_baldwin = false;
                                $scope.hide_others_bbkv = true;
                                $scope.hide_others_bladwin = true;
                            }
                            else {
                                $scope.hide_anuual_baldwin = true;
                                $scope.hide_others_bbkv = true;
                                $scope.hide_others_bladwin = false;
                            }
                        }
                        else if ($scope.mi_id == "14") {
                            $scope.hide_anuual = true;
                            $scope.hide_others = true;
                            $scope.hide_others_bbkv = false;
                            $scope.hide_anuual_baldwin = true;
                            $scope.hide_others_bladwin = true;
                        }
                        else {
                            $scope.hide_anuual = true;
                            $scope.hide_others = true;
                            $scope.hide_others_bbkv = false;
                            $scope.hide_anuual_baldwin = true;
                            $scope.hide_others_bladwin = true;
                        }
                        //------------------------------report Data 
                        var temp_final_report = [];
                        for (var a = 0; a < promise.feesummlist.length; a++) {
                            var temp_stu_report = [];
                            stu_id = promise.feesummlist[a].AMST_Id;
                            var already_cnt = 0;
                            if (temp_final_report.length > 0) {
                                angular.forEach(temp_final_report, function (rt) {
                                    if (rt.AMST_Id == stu_id) {
                                        already_cnt += 1;
                                    }
                                })
                            }
                            if (already_cnt == 0) {
                                for (var b = 0; b < promise.feesummlist.length; b++) {
                                    if (stu_id == promise.feesummlist[b].AMST_Id) {

                                        temp_stu_report.push({ FSS_ToBePaid: promise.feesummlist[b].FSS_ToBePaid, FMH_Id: promise.feesummlist[b].FMH_Id, FMH_FeeName: promise.feesummlist[b].FMH_FeeName, FTI_Name: promise.feesummlist[b].FTI_Name })
                                    }
                                }
                                //
                                //debugger;
                                //var newArr = [];
                                //angular.forEach(temp_stu_report, function(value, key) {
                                //    var exists = false;
                                //    angular.forEach(newArr, function(val2, key) {
                                //        if(angular.equals(value.FMH_FeeName, val2.FMH_FeeName)){ exists = true }; 
                                //    });
                                //    if(exists == false && value.FMH_FeeName != "") { newArr.push({"FMH_FeeName":value})}; 
                                //});
                                //

                                var stu_Total = 0;
                                var fine_total = 0;
                                var total_fee = 0;

                                angular.forEach(temp_stu_report, function (ew) {
                                    stu_Total += ew.FSS_ToBePaid;
                                    if (ew.FSS_FineAmount != undefined && ew.FSS_FineAmount != null) {
                                        fine_total += ew.FSS_FineAmount;
                                    }
                                    else {
                                        fine_total = 0;
                                    }
                                    total_fee += stu_Total + fine_total;
                                })



                                //  $scope.words = $scope.amountinwords(stu_Total);
                                if (fine_total == "NaN") {
                                    $scope.fine_total = 0;
                                }
                                temp_final_report.push({
                                    AMST_Id: stu_id, Name: promise.feesummlist[a].Name,
                                    AMST_AdmNo: promise.feesummlist[a].AMST_AdmNo,
                                    AMAY_RollNo: promise.feesummlist[a].AMAY_RollNo,
                                    ASMCL_ClassName: promise.feesummlist[a].ASMCL_ClassName,
                                    ASMC_SectionName: promise.feesummlist[a].ASMC_SectionName,
                                    AMST_FatherName: promise.feesummlist[a].AMST_FatherName,
                                    stu_report: temp_stu_report,
                                    Total: stu_Total,
                                    totalInWords: $scope.amountinwords(stu_Total),
                                    fee_m: fine_total,
                                    fine: total_fee,
                                    words: $scope.amountinwords(total_fee),
                                    FTI_Name: promise.feesummlist[a].FTI_Name,
                                    FMH_FeeName: promise.feesummlist[a].FMH_FeeName,
                                    FSS_NetAmount: promise.feesummlist[a].FSS_NetAmount,
                                    FSS_ToBePaid: promise.feesummlist[a].FSS_ToBePaid,
                                    FSS_FineAmount: promise.feesummlist[a].FSS_FineAmount
                                })

                            }
                        }
                        $scope.Feesummarized = temp_final_report;
                        console.log($scope.Feesummarized);
                        $scope.remarks = promise.ftp_remarks;
                        $scope.account = promise.accountdetails;
                        angular.forEach($scope.account, function (value, key) {
                            debugger;
                            $scope.accountno = value.acc_No;
                            $scope.bankname = value.bank_Name;
                            $scope.SchoolName = value.mI_Name;
                            // $scope.ifsc_code = value.ifsc;
                        });
                        var date = new Date();
                        $scope.FromDate = ('0' + date.getDate()).slice(-2) + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

                        angular.forEach($scope.Feesummarized, function (value, key) {
                            $scope.FirstName = value.Name;
                            $scope.class = value.ASMCL_ClassName;
                            $scope.section = value.ASMC_SectionName;
                            $scope.installment = value.FTI_Name;
                            $scope.head = value.FMH_FeeName;
                            $scope.amount = value.FSS_NetAmount;
                            $scope.admno = value.AMST_AdmNo;
                            //$scope.section = value.ASMC_SectionName;
                            $scope.totB = $scope.total(promise.classalldata);
                            // $scope.words = $scope.amountinwords($scope.FSS_ToBePaid);
                            //if ($scope.fine_total == null) {
                            //    $scope.words = "";
                            //}
                            //else {
                            //    $scope.words = $scope.amountinwords(fine_total);                              
                            //}                           
                        });
                       
                        var e1 = angular.element(document.getElementById("test"));
                        $compile(e1.html(promise.htmldata))(($scope));
                    }
                    else {
                        swal("No records Found");
                        $scope.hide_anuual = true;
                        $scope.hide_others = true;
                        $scope.hide_anuual_baldwin = true;
                        $scope.hide_others_bladwin = true;
                        // $scope.print_flag = true;
                        //$('#MaldaReceipt').modal('hide');
                    }
                })
            }
            else {
                $scope.submitted = true;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        };
        //----------------------------------------------------------Report END------------------------------------------

        //$scope.printToCart = function () {
        //    debugger;
        //    var innerContents = document.getElementById("BBKVChallan").innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //   '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //                '<link type="text/css" media="print" rel="stylesheet" href="css/print/BBKV/BBKVChallan/BKVChallanPdf.css"/>' +
        //      '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close();
        //}

        $scope.printToCart = function () {
            debugger;
            if ($scope.mi_id == 5) {
                var innerContents = document.getElementById("BBKVChallan").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
               //'<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
               //  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BBHS/BankReceipt/BBHSBankReceiptPdf.css" />' +
               //   '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
               // '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/bbkv/bbkvchallan/bkvchallanpdf.css"/>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            } 
            else if ($scope.mi_id == 14) {
                var innerContents = document.getElementById("BBKVChallan").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
               // popupWinindow.document.write('<html><head>' +
               //'<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
               //             '<link type="text/css" media="print" rel="stylesheet" href="css/print/bbkv/bbkvchallan/bkvchallanpdf.css"/>' +
               //   '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
               //     '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/bbkv/bbkvchallan/BBKVChallanMultiple.css"/>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');


                popupWinindow.document.close();
            }
            else if ($scope.mi_id == 15) {
                var innerContents = document.getElementById("MaldaReceipt").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                       '<link type="text/css" media="print" rel="stylesheet" href="css/print/StMaryMalda/ReceiptPdf.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else if ($scope.mi_id == 30){

                var innerContents = document.getElementById("test").innerHTML;
                var popupWinindow = window.open('');
                //  popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/StMaryMalda/ReceiptPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        }

        $scope.total = function (e) {
            
            var totalc = 0;
            angular.forEach($scope.Feesummarized, function (e) {
                //totalc += e.FSS_NetAmount;
                totalc += e.FSS_ToBePaid;
            });
            return totalc;
        };
        //Get Current Date
        $scope.tdate = new Date();
        //Selected Academic Year
        $scope.getyear = function (asmaY_Id) {
            for (var i = 0; i < $scope.arrlist6.length; i++) {
                if (asmaY_Id == $scope.arrlist6[i].asmaY_Id) {
                    $scope.cyear = $scope.arrlist6[i].asmaY_Year;
                }
            }
        }
        $scope.onselectstudent = function (amst_Id) {
           
            if ($scope.studentlst != null) {
                for (var i = 0; i < $scope.studentlst.length; i++) {
                    if (amst_Id == $scope.studentlst[i].amst_Id) {
                        $scope.selectstudent = $scope.studentlst[i].amsT_FirstName + ' ' + $scope.studentlst[i].amsT_MiddleName + ' ' + $scope.studentlst[i].amsT_LastName;
                    }
                }
            }

        }
        $scope.amountinwords = function convertNumberToWords(totalc) {
           
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
            totalc = totalc.toString();
            var atemp = totalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++, j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++, j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                totalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        totalc = n_array[i] * 10;
                    } else {
                        totalc = n_array[i];
                    }
                    if (totalc != 0) {
                        words_string += words[totalc] + " ";
                    }
                    if ((i == 1 && totalc != 0) || (i == 0 && totalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && totalc != 0) || (i == 2 && totalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && totalc != 0) || (i == 4 && totalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && totalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && totalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }

        $scope.isOptionsRequired = function () {
            //debugger;
            return !$scope.termlst.some(function (options) {
                return options.fmT_Id;
            });
        }


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

            return !$scope.termlst.some(function (options) {
                return options.fmT_Id_check;
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




