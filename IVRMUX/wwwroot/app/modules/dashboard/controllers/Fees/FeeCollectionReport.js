(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeCollectionReportController', FeeCollectionReportController)
    FeeCollectionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeeCollectionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        $scope.tadprint = false;

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.std = false;
        $scope.adyr = true;
        $scope.frmdt = false;
        $scope.Grid_view = false;
        //$scope.frmdt = true;
        $scope.printdatatable = [];
        $scope.printdatatablegrp = [];
        $scope.printdatatablehad = [];
        $scope.printdatatablecls = [];
        $scope.printdatatableclsinscon = [];
        $scope.printdatatableclsinsconN = [];
        $scope.student_install_wise = function () {
            $scope.std = false;
            $scope.grp = false;
            $scope.cls = false;
            $scope.had = false;
            $scope.catg = false;
            $scope.Grid_view = false;
            $scope.printdatatable = [];
            $scope.printdatatablegrp = [];
            $scope.printdatatablehad = [];
            $scope.printdatatablecls = [];
            $scope.printdatatableclsinscon = [];
            $scope.printdatatableclsinsconN = [];
            $scope.printdatatablecat = [];
            $scope.stdall = false;
            angular.forEach($scope.students, function (obj) {
                obj.stdselected = false;
            });
            $scope.grpall = false;
            angular.forEach($scope.groups, function (obj) {
                obj.grpselected = false;
            });
            $scope.clsall = false;
            angular.forEach($scope.classes, function (obj) {
                obj.clsselected = false;
            });
            $scope.hadall = false;
            angular.forEach($scope.heads, function (obj) {
                obj.hadselected = false;
            });
            $scope.catall = false;
            angular.forEach($scope.category, function (obj) {
                obj.catselected = false;
            });
            //Added By Praveen
            $scope.stdmodel = false;
            angular.forEach($scope.stdcnt, function (obj) {
                obj.stdcount = false;
            });
            $scope.clsmodel = false;
            angular.forEach($scope.clscnt, function (obj) {
                obj.clscount = false;
            });
            $scope.instmodel = false;
            angular.forEach($scope.instcnt, function (obj) {
                obj.instcount = false;
            });
            //Ended
            if ($scope.printdatatablecls.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

            if ($scope.printdatatableclsinscon.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

            if ($scope.printdatatableclsinsconN.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatablecat.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            //Added By Praveen
            if ($scope.printdatatablestd.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatableclass.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatableinst.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            //Ended
        }
        $scope.get_total_student_print = function () {
            var s_total_FSS_PaidAmount_p = 0;
            var s_total_concession_p = 0;
            var s_total_balance_p = 0;
            var s_total_FSS_PayableAmount_p = 0;

            var s_total_fine_p = 0;
            var s_total_rebate_p = 0;
            var s_total_waived_p = 0;
            var s_total_adjusted_p = 0;
            angular.forEach($scope.printdatatable, function (stu) {
                s_total_FSS_PayableAmount_p += stu.totalpayable;
                s_total_FSS_PaidAmount_p += stu.FSS_PaidAmount;
                s_total_balance_p += stu.balance;
                s_total_concession_p += stu.concession;
                s_total_fine_p += stu.fine;
                s_total_rebate_p += stu.rebate;
                s_total_waived_p += stu.waived;
                s_total_adjusted_p += stu.adjusted;
            })
            $scope.s_total_FSS_PayableAmount_p = s_total_FSS_PayableAmount_p;
            $scope.s_total_FSS_PaidAmount_p = s_total_FSS_PaidAmount_p;
            $scope.s_total_balance_p = s_total_balance_p;
            $scope.s_total_concession_p = s_total_concession_p;
            $scope.s_total_fine_p = s_total_fine_p;
            $scope.s_total_rebate_p = s_total_rebate_p;
            $scope.s_total_waived_p = s_total_waived_p;
            $scope.s_total_adjusted_p = s_total_adjusted_p;
        }

        $scope.toggleAllstd = function () {

            var toggleStatus = $scope.stdall;
            $scope.printdatatable = [];
            angular.forEach($scope.students, function (itm) {
                itm.stdselected = toggleStatus;
                if ($scope.stdall == true) {
                    $scope.printdatatable.push(itm);
                }

            });
            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();
        }
        $scope.optionToggledstd = function (SelectedStudentRecord, index) {

            $scope.stdall = $scope.students.every(function (itm) { return itm.stdselected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();
        }
        $scope.get_total_group_print = function () {
            var g_total_FSS_PaidAmount_p = 0;
            var g_total_concession_p = 0;
            var g_total_balance_p = 0;
            var g_total_fine_p = 0;
            var g_total_rebate_p = 0;
            var g_total_waived_p = 0;
            var g_total_adjusted_p = 0;
            var g_total_excess_p = 0;
            var g_total_runningexcess_p = 0;
            angular.forEach($scope.printdatatablegrp, function (gp) {
                g_total_FSS_PaidAmount_p += gp.FSS_PaidAmount;
                g_total_balance_p += gp.balance;
                g_total_concession_p += gp.concession;
                g_total_fine_p += gp.fine;
                g_total_rebate_p += gp.rebate;
                g_total_waived_p += gp.waived;
                g_total_adjusted_p += gp.adjusted;
                g_total_excess_p += gp.Excess;
                g_total_runningexcess_p += gp.RunningExcess;
            })
            $scope.g_total_FSS_PaidAmount_p = g_total_FSS_PaidAmount_p;
            $scope.g_total_balance_p = g_total_balance_p;
            $scope.g_total_concession_p = g_total_concession_p;
            $scope.g_total_fine_p = g_total_fine_p;
            $scope.g_total_rebate_p = g_total_rebate_p;
            $scope.g_total_waived_p = g_total_waived_p;
            $scope.g_total_adjusted_p = g_total_adjusted_p;
            $scope.g_total_excess_p = g_total_excess_p;
            $scope.g_total_runningexcess_p = g_total_runningexcess_p;
        }
        $scope.toggleAllgrp = function () {

            $scope.printdatatablegrp = [];
            var toggleStatus = $scope.grpall;
            angular.forEach($scope.groups, function (itm) {
                itm.grpselected = toggleStatus;
                if ($scope.grpall == true) {
                    $scope.printdatatablegrp.push(itm);
                }

            });
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_group_print();
        }


        $scope.onselectyear = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                // "ASMCL_Id": clsobj.asmcL_Id,s
            }
            apiService.create("DailyFeeCollReport/getdata", data).
                then(function (promise) {
                    $scope.group = promise.fillfeegroup;
                    angular.forEach($scope.group, function (tr) {
                        tr.Selected = true;
                    })
                })
        }

        //All Check Group
        $scope.hrdallcheck1 = function () {
            var toggleStatus1 = $scope.checkallhrd1;
            angular.forEach($scope.group, function (itm) {
                itm.fmG_Id_chk = toggleStatus1;
            });
        }
        $scope.binddatagrp3 = function (toggle) {
            if ($scope.checkallhrd1 == true) {
                if (toggle == true) {
                    $scope.checkallhrd1 = true;
                }
                else {
                    $scope.checkallhrd1 = false;
                }
            }

        }    

        //All Check Term
        $scope.termallcheck1 = function () {
            var toggleStatus1 = $scope.checkallterm1;
            angular.forEach($scope.groupcount, function (itm) {
                itm.fmT_Id_chk = toggleStatus1;
            });
        }
        $scope.binddatagrp2 = function (toggle) {
            if ($scope.checkallterm1 == true) {
                if (toggle == true) {
                    $scope.checkallterm1 = true;
                }
                else {
                    $scope.checkallterm1 = false;
                }
            }

        }
        //All Check Installment
        $scope.installcheck = function () {
            var togglestatus = $scope.checkallinst;
            angular.forEach($scope.installmentname, function (inst) {
                inst.fti_Id_chk = togglestatus;
            });
        }
        $scope.get_installment = function (toggle) {
            if ($scope.checkallinst == true) {
                if (toggle == true) {
                    $scope.checkallinst = true;
                }
                else {
                    $scope.checkallinst = false;
                }
            }
        }

        $scope.optionToggledgrp = function (SelectedStudentRecord, index) {

            $scope.grpall = $scope.groups.every(function (itm) { return itm.grpselected; });

            if ($scope.printdatatablegrp.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablegrp.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablegrp.splice($scope.printdatatablegrp.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_group_print();
        }
        $scope.get_total_head_print = function () {
            var h_total_FSS_PaidAmount_p = 0;
            var h_total_concession_p = 0;
            var h_total_balance_p = 0;
            var h_total_fine_p = 0;
            var h_total_rebate_p = 0;
            var h_total_waived_p = 0;
            var h_total_adjusted_p = 0;
            var h_total_excess_p = 0;
            var h_total_runningexcess_p = 0;
            angular.forEach($scope.printdatatablehad, function (hd) {
                h_total_FSS_PaidAmount_p += hd.FSS_PaidAmount;
                h_total_balance_p += hd.balance;
                h_total_concession_p += hd.concession;
                h_total_fine_p += hd.fine;
                h_total_rebate_p += hd.rebate;
                h_total_waived_p += hd.waived;
                h_total_adjusted_p += hd.adjusted;
                h_total_excess_p += hd.Excess;
                h_total_runningexcess_p += hd.RunningExcess;
            })
            $scope.h_total_FSS_PaidAmount_p = h_total_FSS_PaidAmount_p;
            $scope.h_total_concession_p = h_total_concession_p;
            $scope.h_total_balance_p = h_total_balance_p;
            $scope.h_total_fine_p = h_total_fine_p;
            $scope.h_total_rebate_p = h_total_rebate_p;
            $scope.h_total_waived_p = h_total_waived_p;
            $scope.h_total_adjusted_p = h_total_adjusted_p;
            $scope.h_total_excess_p = h_total_excess_p;
            $scope.h_total_runningexcess_p = h_total_runningexcess_p;
        }
        $scope.toggleAllhad = function () {

            var toggleStatus = $scope.hadall;
            $scope.printdatatablehad = [];
            angular.forEach($scope.heads, function (itm) {
                itm.hadselected = toggleStatus;
                if ($scope.hadall == true) {
                    $scope.printdatatablehad.push(itm);
                }
            });
            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_head_print();
        }
        $scope.toggleAllcat = function () {

            var toggleStatus = $scope.catall;
            $scope.printdatatablecat = [];
            angular.forEach($scope.category, function (itm) {
                itm.catselected = toggleStatus;
                if ($scope.catall == true) {
                    $scope.printdatatablecat.push(itm);
                }
            });
            if ($scope.printdatatablecat.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_cat_print();
        }
        $scope.optionToggledhad = function (SelectedStudentRecord, index) {

            $scope.hadall = $scope.heads.every(function (itm) { return itm.hadselected; });
            if ($scope.printdatatablehad.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablehad.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablehad.splice($scope.printdatatablehad.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_head_print();
        }
        $scope.optionToggledcat = function (SelectedStudentRecord, index) {

            $scope.catall = $scope.category.every(function (itm) { return itm.catselected; });
            if ($scope.printdatatablecat.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablecat.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablecat.splice($scope.printdatatablecat.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatablecat.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_cat_print();
        }
        $scope.get_total_class_print = function () {
            var c_total_totalpayable = 0;
            var c_total_FSS_PaidAmount_p = 0;
            var c_total_concession_p = 0;
            var c_total_balance_p = 0;
            var c_total_fine_p = 0;
            var c_total_rebate_p = 0;
            var c_total_waived_p = 0;
            var c_total_adjusted_p = 0;
            angular.forEach($scope.printdatatablecls, function (cls) {
                c_total_totalpayable += cls.totalpayable;
                c_total_FSS_PaidAmount_p += cls.FSS_PaidAmount;
                c_total_balance_p += cls.balance;
                c_total_concession_p += cls.concession;
                c_total_fine_p += cls.fine;
                c_total_rebate_p += cls.rebate;
                c_total_waived_p += cls.waived;
                c_total_adjusted_p += cls.adjusted;
            })
            $scope.c_total_totalpayable = c_total_totalpayable;
            $scope.c_total_FSS_PaidAmount_p = c_total_FSS_PaidAmount_p;
            $scope.c_total_balance_p = c_total_balance_p;
            $scope.c_total_concession_p = c_total_concession_p;
            $scope.c_total_fine_p = c_total_fine_p;
            $scope.c_total_rebate_p = c_total_rebate_p;
            $scope.c_total_waived_p = c_total_waived_p;
            $scope.c_total_adjusted_p = c_total_adjusted_p;
        }

        $scope.toggleAllcls = function () {

            var toggleStatus = $scope.clsall;
            $scope.printdatatablecls = [];
            angular.forEach($scope.classes, function (itm) {
                itm.clsselected = toggleStatus;
                if ($scope.clsall == true) {
                    $scope.printdatatablecls.push(itm);
                }
                //else {
                //    $scope.printdatatablecls.splice(itm);
                //}
            });
            if ($scope.printdatatablecls.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_print();
        }

        $scope.toggleAllclsinscon = function () {

            var toggleStatus = $scope.clsallclscon;
            $scope.printdatatableclsinscon = [];
            angular.forEach($scope.classesinscon, function (itm) {
                itm.clsselectedclscon = toggleStatus;
                if ($scope.clsallclscon == true) {
                    $scope.printdatatableclsinscon.push(itm);
                }
                //else {
                //    $scope.printdatatablecls.splice(itm);
                //}
            });
            if ($scope.printdatatableclsinscon.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_printinscon();
        }

        $scope.toggleAllclsinsconN = function () {

            var toggleStatus = $scope.clsallclsconN;
            $scope.printdatatableclsinsconN = [];
            angular.forEach($scope.classesinsconN, function (itm) {
                itm.clsselectedclsconN = toggleStatus;
                if ($scope.clsallclsconN == true) {
                    $scope.printdatatableclsinsconN.push(itm);
                }
                //else {
                //    $scope.printdatatablecls.splice(itm);
                //}
            });
            if ($scope.printdatatableclsinsconN.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_printinsconN();
        }

        $scope.get_total_class_printinscon = function () {
            var c_TotalCharges = 0;
            var c_TotalBalance = 0;
            var c_TotalPaid = 0;
            var c_TotalConcession = 0;
            var c_TotalWaivedOff = 0;
            var c_TotalRefund = 0;
            var c_Adjustment = 0;
            var c_Excess = 0;
            var c_RunningExcess = 0;

            angular.forEach($scope.printdatatableclsinscon, function (cls) {
                c_TotalCharges += cls.TotalCharges;
                c_TotalBalance += cls.TotalBalance;
                c_TotalPaid += cls.TotalPaid;
                c_TotalConcession += cls.TotalConcession;
                c_TotalWaivedOff += cls.TotalWaivedOff;
                c_TotalRefund += cls.TotalRefund;
                c_Adjustment += cls.Adjustment;
                c_Excess += cls.Excess;
                c_RunningExcess += cls.RunningExcess;
            })
            $scope.c_TotalCharges = c_TotalCharges;
            $scope.c_TotalBalance = c_TotalBalance;
            $scope.c_TotalPaid = c_TotalPaid;
            $scope.c_TotalConcession = c_TotalConcession;
            $scope.c_TotalWaivedOff = c_TotalWaivedOff;
            $scope.c_TotalRefund = c_TotalRefund;
            $scope.c_Adjustment = c_Adjustment;
            $scope.c_Excess = c_Excess;
            $scope.c_RunningExcess = c_RunningExcess;
        }

        $scope.get_total_class_printinsconN = function () {
            var c_TotalCharges = 0;
            var c_TotalBalance = 0;
            var c_TotalPaid = 0;
            var c_TotalConcession = 0;
            var c_TotalWaivedOff = 0;
            var c_TotalRefund = 0;
            var c_Adjustment = 0;

            angular.forEach($scope.printdatatableclsinsconN, function (cls) {
                c_TotalCharges += cls.TotalCharges;
                c_TotalBalance += cls.TotalBalance;
                c_TotalPaid += cls.TotalPaid;
                c_TotalConcession += cls.TotalConcession;
                c_TotalWaivedOff += cls.TotalWaivedOff;
                c_TotalRefund += cls.TotalRefund;
                c_Adjustment += cls.Adjustment;
            })
            $scope.c_TotalCharges = c_TotalCharges;
            $scope.c_TotalBalance = c_TotalBalance;
            $scope.c_TotalPaid = c_TotalPaid;
            $scope.c_TotalConcession = c_TotalConcession;
            $scope.c_TotalWaivedOff = c_TotalWaivedOff;
            $scope.c_TotalRefund = c_TotalRefund;
            $scope.c_Adjustment = c_Adjustment;
        }

        $scope.optionToggledclsinscon = function (SelectedStudentRecord, index) {

            $scope.clsallclscon = $scope.classes.every(function (itm) { return itm.clsselectedclscon; });
            if ($scope.printdatatableclsinscon.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatableclsinscon.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatableclsinscon.splice($scope.printdatatableclsinscon.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatableclsinscon.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_printinscon();
        }

        $scope.optionToggledclsinsconN = function (SelectedStudentRecord, index) {

            $scope.clsallclsconN = $scope.classes.every(function (itm) { return itm.clsselectedclsconN; });
            if ($scope.printdatatableclsinsconN.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatableclsinsconN.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatableclsinsconN.splice($scope.printdatatableclsinsconN.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatableclsinsconN.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_printinsconN();
        }

        $scope.optionToggledcls = function (SelectedStudentRecord, index) {

            $scope.clsall = $scope.classes.every(function (itm) { return itm.clsselected; });
            if ($scope.printdatatablecls.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablecls.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablecls.splice($scope.printdatatablecls.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatablecls.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_print();
        }

         ////Added By Praveen 
        $scope.get_total_std_print = function () {
            var d_total_totalpayable_p = 0;
            var d_total_FSS_PaidAmount_p = 0;
            var d_total_concession_p = 0;
            var d_total_balance_p = 0;
            var d_total_fine_p = 0;
            var d_total_rebate_p = 0;
            var d_total_waived_p = 0;
            var d_total_adjusted_p = 0;
            angular.forEach($scope.printdatatablestd, function (cls) {
                d_total_totalpayable_p += cls.totalpayable;
                d_total_FSS_PaidAmount_p += cls.FSS_PaidAmount;
                d_total_balance_p += cls.balance;
                d_total_concession_p += cls.concession;
                d_total_fine_p += cls.fine;
                d_total_rebate_p += cls.rebate;
                d_total_waived_p += cls.waived;
                d_total_adjusted_p += cls.adjusted;
                d_total_excess_p += cls.Excess;
                d_total_adjusted_p += cls.adjusted;
            })
            $scope.d_total_totalpayable_p = d_total_totalpayable_p;
            $scope.d_total_FSS_PaidAmount_p = d_total_FSS_PaidAmount_p;
            $scope.d_total_balance_p = d_total_balance_p;
            $scope.d_total_concession_p = d_total_concession_p;
            $scope.d_total_fine_p = d_total_fine_p;
            $scope.d_total_rebate_p = d_total_rebate_p;
            $scope.d_total_waived_p = d_total_waived_p;
            $scope.d_total_adjusted_p = d_total_adjusted_p;
            $scope.d_total_excess_p = d_total_excess_p;
            $scope.d_total_runningexcess_p = d_total_runningexcess_p;
        }
        
        $scope.toggleAllstdcnt = function () {
            var toggleStatus = $scope.stdmodel;
            $scope.printdatatablestd = [];
            angular.forEach($scope.stdcnt, function (itm) {
                itm.stdcount = toggleStatus;
                if ($scope.stdmodel == true) {
                    $scope.printdatatablestd.push(itm);
                }
            });
            if ($scope.printdatatablestd.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_std_print();
        }

         //Class wise Total Paid count
        $scope.get_total_class_print = function () {
            var d_total_totalpayable_p = 0;
            var d_total_FSS_PaidAmount_p = 0;
            var d_total_concession_p = 0;
            var d_total_balance_p = 0;
            var d_total_fine_p = 0;
            var d_total_rebate_p = 0;
            var d_total_waived_p = 0;
            var d_total_adjusted_p = 0;
            angular.forEach($scope.printdatatableclass, function (cls) {
                d_total_totalpayable_p += cls.totalpayable;
                d_total_FSS_PaidAmount_p += cls.FSS_PaidAmount;
                d_total_balance_p += cls.balance;
                d_total_concession_p += cls.concession;
                d_total_fine_p += cls.fine;
                d_total_rebate_p += cls.rebate;
                d_total_waived_p += cls.waived;
                d_total_adjusted_p += cls.adjusted;
                d_total_excess_p += cls.Excess;
                d_total_adjusted_p += cls.adjusted;
            })
            $scope.d_total_totalpayable_p = d_total_totalpayable_p;
            $scope.d_total_FSS_PaidAmount_p = d_total_FSS_PaidAmount_p;
            $scope.d_total_balance_p = d_total_balance_p;
            $scope.d_total_concession_p = d_total_concession_p;
            $scope.d_total_fine_p = d_total_fine_p;
            $scope.d_total_rebate_p = d_total_rebate_p;
            $scope.d_total_waived_p = d_total_waived_p;
            $scope.d_total_adjusted_p = d_total_adjusted_p;
            $scope.d_total_excess_p = d_total_excess_p;
            $scope.d_total_runningexcess_p = d_total_runningexcess_p;
        }
      
        $scope.toggleAllclscnt = function () {

            var toggleStatus = $scope.clsmodel;
            $scope.printdatatableclass = [];
            angular.forEach($scope.clscnt, function (itm) {
                itm.clscount = toggleStatus;
                if ($scope.clsmodel == true) {
                    $scope.printdatatableclass.push(itm);
                }
            });
            if ($scope.printdatatableclass.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_print();
        }

         ////Installment wise Paid count
        $scope.get_total_inst_print = function () {
            var d_total_Payable_p = 0;
            var d_total_Paid_p = 0;
            var d_total_Balance_p = 0;
            var d_total_Concession_p = 0;
            var d_total_Arrear_p = 0;
            var d_total_Excess_p = 0;
            var d_total_Charge_p = 0;
            var d_total_adjusted_p = 0;
            angular.forEach($scope.printdatatableinst, function (inst) {
                d_total_Payable_p += inst.Payable;
                d_total_Paid_p += inst.Paid;
                d_total_Balance_p += inst.Balance;
                d_total_Concession_p += inst.Concession;
                d_total_Arrear_p += inst.Arrea;
                d_total_Excess_p += inst.Excess;
                d_total_Charge_p += inst.Charge;
                d_total_adjusted_p += inst.adjusted;
            })
            $scope.d_total_Payable_p = d_total_Payable_p;
            $scope.d_total_Paid_p = d_total_Paid_p;
            $scope.d_total_Balance_p = d_total_Balance_p;
            $scope.d_total_Concession_p = d_total_Concession_p;
            $scope.d_total_Arrear_p = d_total_Arrear_p;
            $scope.d_total_Excess_p = d_total_Excess_p;
            $scope.d_total_Charge_p = d_total_Charge_p;
            $scope.d_total_adjusted_p = d_total_adjusted_p;
        }
       
        $scope.toggleAllinstcnt = function () {

            var toggleStatus = $scope.instmodel;
            $scope.printdatatableinst = [];
            angular.forEach($scope.instcnt, function (itm) {
                itm.instcount = toggleStatus;
                if ($scope.instmodel == true) {
                    $scope.printdatatableinst.push(itm);
                }
            });
            if ($scope.printdatatableinst.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_inst_print();
        }

        //Added By Praveen
        $scope.optionToggledstdcnt = function (SelectedStudentRecord, index) {

            $scope.stdmodel = $scope.stdcnt.every(function (itm) { return itm.stdcount; });
            if ($scope.printdatatablestd.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablestd.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablestd.splice($scope.printdatatablestd.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatablestd.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_std_print();
        }

        $scope.optionToggledclscnt = function (SelectedStudentRecord, index) {

            $scope.clsmodel = $scope.clscnt.every(function (itm) { return itm.clscount; });
            if ($scope.printdatatableclass.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatableclass.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatableclass.splice($scope.printdatatableclass.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatableclass.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_print();
        }

        $scope.optionToggledinstcnt = function (SelectedStudentRecord, index) {

            $scope.instmodel = $scope.instcnt.every(function (itm) { return itm.instcount; });
            if ($scope.printdatatableinst.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatableinst.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatableinst.splice($scope.printdatatableinst.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatableinst.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_inst_print();
        }

        $scope.onclickloaddata = function () {
            $scope.Grid_view = false;
            $scope.printdatatable = [];
            $scope.printdatatablegrp = [];
            $scope.printdatatablehad = [];
            $scope.printdatatablecls = [];
            $scope.printdatatableclsinscon = [];
            $scope.printdatatableclsinsconN = [];
            $scope.printdatatablecat = [];
            $scope.printdatatablestd = [];
            $scope.printdatatableclass = [];
            $scope.printdatatableinst = [];

            $scope.stdall = false;
            angular.forEach($scope.students, function (obj) {
                obj.stdselected = false;
            });
            $scope.grpall = false;
            angular.forEach($scope.groups, function (obj) {
                obj.grpselected = false;
            });
            $scope.clsall = false;
            angular.forEach($scope.classes, function (obj) {
                obj.clsselected = false;
            });
            $scope.hadall = false;
            angular.forEach($scope.heads, function (obj) {
                obj.hadselected = false;
            });
            $scope.catall = false;
            angular.forEach($scope.category, function (obj) {
                obj.catselected = false;
            });
            //Added By Praveen
            $scope.stdmodel = false;
            angular.forEach($scope.clscnt, function (obj) {
                obj.stdcount = false;
            });
            $scope.clsmodel = false;
            angular.forEach($scope.stdcnt, function (obj) {
                obj.clscount = false;
            });
            $scope.instmodel = false;
            angular.forEach($scope.instcnt, function (obj) {
                obj.instcount = false;
            });
            //Ended
            if ($scope.printdatatableclsinscon.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatableclsinsconN.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatablecls.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            //Added By Praveen
            if ($scope.printdatatablestd.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatableclass.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            if ($scope.printdatatableinst.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            //Ended
            if ($scope.rpttyp == "year") {
                $scope.frmdt = false;
                $scope.adyr = true;
            }
            else if ($scope.rpttyp == "date") {
                $scope.frmdt = true;
                $scope.adyr = false;
            }
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };




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
            $scope.term = true;
            $scope.groupterm = false;
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name"
            $scope.groupterm = true;
            $scope.term = false;
        }


        $scope.getterms = function () {
            var data = {
                "reporttype": grouporterm,
            }
            apiService.create("FeeCollectionReport/getgrpterms", data).
                then(function (promise) {

                    $scope.groupcount = promise.fillmastergroup;
                    $scope.termlst = promise.fillterms;
                    $scope.custom = promise.customlist;

                    if (grouporterm == "T") {
                        angular.forEach(promise.grouplist, function (tr) {
                            tr.fmG_Id_chk = false;
                        })
                    }
                    else if (grouporterm == "G") {
                        angular.forEach(promise.grouplist, function (tr) {
                            tr.fmG_Id_chk1 = false;
                        })
                    }


                    $scope.group = promise.grouplist;


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


                })
        }

        var temp_grp_list = [];
        $scope.loaddata = function () {
            $scope.std = false;
            $scope.grp = false;
            $scope.cls = false;
            $scope.had = false;
            $scope.catg = false;
            $scope.custom_check_flag = true;
            $scope.group_check_flag = true;
            $scope.custom_check = "0";
            $scope.group_check = "0";
            $scope.chequedate = 0;
            $scope.load_group_check();
            $scope.load_custom_check();
            var data = {
                "reporttype": grouporterm,
            }

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;

            apiService.getURI("FeeCollectionReport/getalldetails", pageid).
                then(function (promise) {


                    $scope.configsettings = promise.accountdetails;

                    if ($scope.configsettings.length > 0) {
                        grouporterm = $scope.configsettings[0].fmC_GroupOrTermFlg;
                        autoreceipt = $scope.configsettings[0].fmC_AutoReceiptFeeGroupFlag;
                        receiptformat = $scope.configsettings[0].fmC_Receipt_Format;
                        mergeinstallment = $scope.configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
                    }

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
                    $scope.getterms();

                    $scope.arrlist6 = promise.adcyear;
                    //$scope.groupcount = promise.fillmastergroup;
                    //if (grouporterm == "T") {
                    //    angular.forEach(promise.customlist, function (tr) {
                    //        tr.fmgG_Id_chk = true;
                    //    })
                    //}
                    //else if (grouporterm == "G") {
                    //    angular.forEach(promise.customlist, function (tr) {
                    //        tr.fmgG_Id_chk1 = true;
                    //    })
                    //}

                    //$scope.custom = promise.customlist;

                    //if (grouporterm == "T") {
                    //    angular.forEach(promise.grouplist, function (tr) {
                    //        tr.fmG_Id_chk = true;
                    //    })
                    //}
                    //else if (grouporterm == "G") {
                    //    angular.forEach(promise.grouplist, function (tr) {
                    //        tr.fmG_Id_chk1 = true;
                    //    })
                    //}


                    //$scope.group = promise.grouplist;
                    //temp_grp_list = promise.grouplist;
                    $scope.classcount = promise.fillclass;
                    $scope.sectioncount = promise.fillsection;
                    $scope.installmentname = promise.installment;
                });
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.order1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }
        $scope.order2 = function (keyname) {
            $scope.sortKey2 = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        }
        $scope.order3 = function (keyname) {
            $scope.sortKey3 = keyname;   //set the sortKey to the param passed
            $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
        }
        $scope.order4 = function (keyname) {
            $scope.sortKey4 = keyname;   //set the sortKey to the param passed
            $scope.reverse4 = !$scope.reverse4; //if true make it false and vice versa
        }
        $scope.order9 = function (keyname) {
            $scope.sortKey9 = keyname;   //set the sortKey to the param passed
            $scope.reverse9 = !$scope.reverse9; //if true make it false and vice versa
        }
        $scope.order10 = function (keyname) {
            $scope.sortKey10 = keyname;   //set the sortKey to the param passed
            $scope.reverse10 = !$scope.reverse10; //if true make it false and vice versa
        }
        $scope.setTodate = function (data) {
            //  $scope.todate = false;
            console.log(data);
            $scope.ToDate = data;
            $scope.minDate = new Date(
                $scope.ToDate.getFullYear(),
                $scope.ToDate.getMonth(),
                $scope.ToDate.getDate());

        }
        $scope.load_custom_check = function () {

            if ($scope.custom_check == "1") {
                $scope.custom_check_flag = false;

            }
            else if ($scope.custom_check == "0") {
                $scope.custom_check_flag = true;

            }
        }
        $scope.load_group_check = function () {

            if ($scope.group_check == "1") {
                $scope.group_check_flag = false;
            }
            else if ($scope.group_check == "0") {
                $scope.group_check_flag = true;

            }
        }
        $scope.cheque_date = function () {
            if ($scope.chequedate == "1") {
                $scope.chequedate = 1;
            }
            else {
                $scope.chequedate = 0;
            }
        };

        $scope.clear_feecoll = function () {

            $scope.fromDate = "";
            $scope.todate = "";
            $scope.rpttyp = "year";
            $scope.result = "FGW";
            //$scope.status = "act";
            $scope.fmG_Id = "";
            $scope.fmG_Id = "";
            if ($scope.rpttyp == "year") {
                $scope.rpttyp = "year";
                $scope.asmaY_Id = "";

                // $scope.asmaY_Id = null;

            }
            else if ($scope.rpttyp == "date") {
                $scope.rpttyp == "date";
                $scope.fromDate = null;
                $scope.todate = null;

            }
            $scope.fmG_Id = "";
            $scope.status = "";
            $scope.adyr = true;
            $scope.frmdt = false;
            $scope.Grid_view = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        }
        $scope.termflg = false;
        $scope.stdtermflg = false;
        $scope.submitted = false;

        $scope.ShowReport = function () {
            $scope.termflg = false;
            $scope.stdtermflg = false;
            if ($scope.myForm.$valid) {

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



                var FMG_Ids = [];
                var FMT_Ids = [];
                var FTI_ids = [];
                if (grouporterm == "T") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                    angular.forEach($scope.groupcount, function (ty) {
                        if (ty.fmT_Id_chk) {
                            FMT_Ids.push(ty.fmT_Id);
                        }
                    })
                    ////Installment Names
                    angular.forEach($scope.installmentname, function (ty) {
                        if (ty.fti_Id_chk) {
                            FTI_ids.push(ty.ftI_Id);
                        }
                    })
                }
                else if (grouporterm == "G") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk1) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                }
                var classid = 0;
                var secid = 0;

                if (($scope.result == "FSW")) {
                    classid = $scope.fmG_Class;
                    secid = $scope.asmC_Id;
                }
                else if ($scope.result == "STRMW") {
                    classid = $scope.fmG_Class;
                    secid = $scope.asmC_Id;
                } else {

                    classid = 0;
                    secid = 0;
                }



                if ($scope.rpttyp == "year") {

                    var data = {

                        "FMG_Id": $scope.fmG_Id,
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        "active": $scope.active,
                        "deactive": $scope.deactive,
                        "left": $scope.left,
                        "customflag": $scope.custom_check,
                        "groupflag": $scope.group_check,
                        //"termflag": $scope.custom_check,
                        TempararyArrayhEADListnew1: $scope.albumNameArraycolumn1,
                        TempararyArrayhEADListnew2: $scope.albumNameArraycolumn2,
                        TempararyArrayhEADListnew3: $scope.albumNameArraycolumn3,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        FTI_ids: FTI_ids,
                        "term_group": grouporterm,
                        "FMCC_Id": $scope.chequedate,
                        "ASMCL_Id": classid,
                        "AMSC_Id": secid,
                        "From_Date": new Date().toDateString(),
                        "To_Date": new Date().toDateString(),
                    }

                }
                else if ($scope.rpttyp == "date") {
                    var data = {

                        "FMG_Id": $scope.fmG_Id,
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "From_Date": new Date($scope.fromDate).toDateString(),
                        "To_Date": new Date($scope.todate).toDateString(),
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        //"studenttype": $scope.status,
                        "active": $scope.active,
                        "deactive": $scope.deactive,
                        "left": $scope.left,
                        "customflag": $scope.custom_check,
                        "groupflag": $scope.group_check,
                        TempararyArrayhEADListnew1: $scope.albumNameArraycolumn1,
                        TempararyArrayhEADListnew2: $scope.albumNameArraycolumn2,
                        TempararyArrayhEADListnew3: $scope.albumNameArraycolumn3,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        FTI_ids: FTI_ids,
                        "term_group": grouporterm,
                        "FMCC_Id": $scope.chequedate,
                        "ASMCL_Id": classid,
                        "AMSC_Id": secid
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }



                apiService.create("FeeCollectionReport/radiobtndata", data).
                    then(function (promise) {

                        $scope.reportdetails = promise.searchstudentDetails;
                        if (promise.radioval == "FGW") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.groupalldata != null && promise.groupalldata != "") {
                                $scope.groups = promise.groupalldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = true;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.stdinsconN = false;
                                $scope.clsinsconN = false;
                                $scope.clscntinsconN = false;
                                $scope.instcntinsconN = false;
                                var g_total_FSS_PaidAmount = 0;
                                var g_total_balance = 0;
                                var g_total_concession = 0;
                                var g_total_fine = 0;
                                var g_total_rebate = 0;
                                var g_total_waived = 0;
                                var g_total_adjusted = 0;
                                var g_total_excess = 0;
                                var g_total_runningexcess = 0;
                                angular.forEach($scope.groups, function (gp) {
                                    g_total_FSS_PaidAmount += gp.FSS_PaidAmount;
                                    g_total_balance += gp.balance;
                                    g_total_concession += gp.concession;
                                    g_total_fine += gp.fine;
                                    g_total_rebate += gp.rebate;
                                    g_total_waived += gp.waived;
                                    g_total_adjusted += gp.adjusted;
                                    g_total_excess += gp.Excess;
                                    g_total_runningexcess += gp.RunningExcess;
                                })
                                $scope.g_total_FSS_PaidAmount = g_total_FSS_PaidAmount;
                                $scope.g_total_balance = g_total_balance;
                                $scope.g_total_concession = g_total_concession;
                                $scope.g_total_fine = g_total_fine;
                                $scope.g_total_rebate = g_total_rebate;
                                $scope.g_total_waived = g_total_waived;
                                $scope.g_total_adjusted = g_total_adjusted;
                                $scope.g_total_excess = g_total_excess;
                                $scope.g_total_runningexcess = g_total_runningexcess;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }
                        }
                        else if (promise.radioval == "FHW") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.headalldata != null && promise.headalldata != "") {
                                $scope.heads = promise.headalldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = true;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.stdinsconN = false;
                                $scope.clsinsconN = false;
                                $scope.clscntinsconN = false;
                                $scope.instcntinsconN = false;
                                var h_total_FSS_PaidAmount = 0;
                                var h_total_concession = 0;
                                var h_total_balance = 0;
                                var h_total_fine = 0;
                                var h_total_rebate = 0;
                                var h_total_waived = 0;
                                var h_total_adjusted = 0;
                                var h_total_excess = 0;
                                var h_total_runningexcess = 0;
                                angular.forEach($scope.heads, function (hd) {
                                    h_total_FSS_PaidAmount += hd.FSS_PaidAmount;
                                    h_total_balance += hd.balance;
                                    h_total_concession += hd.concession;
                                    h_total_fine += hd.fine;
                                    h_total_rebate += hd.rebate;
                                    h_total_waived += hd.waived;
                                    h_total_adjusted += hd.adjusted;
                                    h_total_excess += hd.Excess;
                                    h_total_runningexcess += hd.RunningExcess;
                                })
                                $scope.h_total_FSS_PaidAmount = h_total_FSS_PaidAmount;
                                $scope.h_total_concession = h_total_concession;
                                $scope.h_total_balance = h_total_balance;
                                $scope.h_total_fine = h_total_fine;
                                $scope.h_total_rebate = h_total_rebate;
                                $scope.h_total_waived = h_total_waived;
                                $scope.h_total_adjusted = h_total_adjusted;
                                $scope.h_total_excess = h_total_excess;
                                $scope.h_total_runningexcess = h_total_runningexcess;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }
                        }

                        // Added on 30 November 2020

                        else if (promise.radioval == "CTW") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.classalldatainscon != null && promise.classalldatainscon != "") {
                                $scope.classesinscon = promise.classalldatainscon;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = true;
                                $scope.clsinsconN = false;
                                $scope.stdinsconN = false;
                                $scope.clsallclscon = false;
                                $scope.clscntinsconN = false;
                                $scope.instcntinsconN = false;
                                var c_total_FSS_PaidAmount = 0;
                                var c_total_concession = 0;
                                var c_total_balance = 0;
                                var c_total_chanrges = 0;
                                var c_total_refund = 0;
                                var c_total_waived = 0;
                                var c_total_adjustment = 0;
                                var c_total_excess = 0;
                                angular.forEach($scope.classesinscon, function (cls) {
                                    c_total_FSS_PaidAmount += cls.TotalPaid;
                                    c_total_balance += cls.TotalBalance;
                                    c_total_concession += cls.TotalConcession;
                                    c_total_chanrges += cls.TotalCharges;
                                    c_total_refund += cls.TotalRefund;
                                    c_total_waived += cls.TotalWaivedOff;
                                    c_total_adjustment += cls.Adjustment;
                                    c_total_excess += cls.Excess;

                                })
                                $scope.c_total_FSS_PaidAmount = c_total_FSS_PaidAmount;
                                $scope.c_total_balance = c_total_balance;
                                $scope.c_total_concession = c_total_concession;
                                $scope.c_total_chanrges = c_total_chanrges;
                                $scope.c_total_refund = c_total_refund;
                                $scope.c_total_waived = c_total_waived;
                                $scope.c_total_adjustment = c_total_adjustment;
                                $scope.c_total_excess = c_total_excess;
                            }

                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }
                        }

                        // Added on 30 November 2020

                        //Added on 23 july 2021
                        else if (promise.radioval == "CTC") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.classalldatainsconN != null && promise.classalldatainsconN != "") {
                                $scope.classesinsconN = promise.classalldatainsconN;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.clsinsconN = true;
                                $scope.stdinsconN = false;
                                $scope.clsallclsconN = false;
                                $scope.clscntinsconN = false;
                                $scope.instcntinsconN = false;
                                var c_total_FSS_PaidAmount = 0;
                                var c_total_concession = 0;
                                var c_total_balance = 0;
                                var c_total_chanrges = 0;
                                var c_total_refund = 0;
                                var c_total_waived = 0;
                                var c_total_adjustment = 0;
                                angular.forEach($scope.classesinscon, function (cls) {
                                    c_total_FSS_PaidAmount += cls.TotalPaid;
                                    c_total_balance += cls.TotalBalance;
                                    c_total_concession += cls.TotalConcession;
                                    c_total_chanrges += cls.TotalCharges;
                                    c_total_refund += cls.TotalRefund;
                                    c_total_waived += cls.TotalWaivedOff;
                                    c_total_adjustment += cls.Adjustment;

                                })
                                $scope.c_total_FSS_PaidAmount = c_total_FSS_PaidAmount;
                                $scope.c_total_balance = c_total_balance;
                                $scope.c_total_concession = c_total_concession;
                                $scope.c_total_chanrges = c_total_chanrges;
                                $scope.c_total_refund = c_total_refund;
                                $scope.c_total_waived = c_total_waived;
                                $scope.c_total_adjustment = c_total_adjustment;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }
                        }
                        //Added on 23 July 2021

                        else if (promise.radioval == "FCW") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.classalldata != null && promise.classalldata != "") {
                                $scope.classes = promise.classalldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = true;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.stdinsconN = false;
                                $scope.clsinsconN = false;
                                $scope.clscntinsconN = false;
                                $scope.instcntinsconN = false;
                                var c_total_FSS_PaidAmount = 0;
                                var c_total_concession = 0;
                                var c_total_balance = 0;
                                var c_total_fine = 0;
                                var c_total_rebate = 0;
                                var c_total_waived = 0;
                                var c_total_adjusted = 0;
                                var c_total_totalpayable = 0;
                                var c_total_excess = 0;
                                var c_total_openingbalance = 0;
                                angular.forEach($scope.classes, function (cls) {
                                    c_total_FSS_PaidAmount += cls.FSS_PaidAmount;
                                    c_total_balance += cls.balance;
                                    c_total_concession += cls.concession;
                                    c_total_fine += cls.fine;
                                    c_total_rebate += cls.rebate;
                                    c_total_waived += cls.waived;
                                    c_total_adjusted += cls.adjusted;
                                    c_total_totalpayable += cls.totalpayable;
                                    c_total_excess += cls.Excess;
                                    c_total_openingbalance += cls.openingbalance;
                                })
                                $scope.c_total_FSS_PaidAmount = c_total_FSS_PaidAmount;
                                $scope.c_total_balance = c_total_balance;
                                $scope.c_total_concession = c_total_concession;
                                $scope.c_total_fine = c_total_fine;
                                $scope.c_total_rebate = c_total_rebate;
                                $scope.c_total_waived = c_total_waived;
                                $scope.c_total_adjusted = c_total_adjusted;
                                $scope.c_total_totalpayable = c_total_totalpayable;
                                $scope.c_total_Excess = c_total_excess;
                                $scope.c_total_openingbalance = c_total_openingbalance;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }

                        }
                        else if (promise.radioval == "FSW") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;

                            if (promise.studentalldata != null && promise.studentalldata != "") {
                                $scope.std = true;
                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>"

                                },
                                {
                                    name: 'StudentName', field: 'StudentName', title: 'Student Name'
                                },
                                {
                                    name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Admission no'
                                },
                                {
                                    name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class Name'
                                },
                                {
                                    name: 'totalpayable', field: 'totalpayable', title: 'Total Payable', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'FSS_PaidAmount', field: 'FSS_PaidAmount', title: 'Paid Amount', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'balance', field: 'balance', title: 'Balance', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'concession', field: 'concession', title: 'Concession', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'fine', field: 'fine', title: 'Fine', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'rebate', field: 'rebate', title: 'Rebate', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },

                                {
                                    name: 'waived', field: 'waived', title: 'Waived', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'adjusted', field: 'adjusted', title: 'Adjusted', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'openingbalance', field: 'openingbalance', title: 'Opening Balance', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'Excess', field: 'Excess', title: 'Excess', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                }
                                ];
                                $scope.students = promise.studentalldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.stdinsconN = false;
                                $scope.clsinsconN = false;
                                $scope.clscntinsconN = false;
                                $scope.instcntinsconN = false;                                
                                var s_total_FSS_PaidAmount = 0;
                                var s_total_balance = 0;
                                var s_total_concession = 0;
                                var s_total_fine = 0;
                                var s_total_rebate = 0;
                                var s_total_waived = 0;
                                var s_total_payable = 0;
                                var s_total_adjusted = 0;
                                var s_total_openingbalance = 0;
                                var s_total_Excess = 0;
                                angular.forEach($scope.students, function (stu) {
                                    s_total_payable += stu.totalpayable;
                                    s_total_FSS_PaidAmount += stu.FSS_PaidAmount;
                                    s_total_balance += stu.balance;
                                    s_total_concession += stu.concession;
                                    s_total_fine += stu.fine;
                                    s_total_rebate += stu.rebate;
                                    s_total_waived += stu.waived;
                                    s_total_adjusted += stu.adjusted;
                                    s_total_openingbalance += stu.openingbalance;
                                    s_total_Excess += stu.Excess;
                                })

                                $scope.s_total_FSS_PayableAmount_p = s_total_payable;

                                $scope.s_total_FSS_PaidAmount = s_total_FSS_PaidAmount;
                                $scope.s_total_concession = s_total_concession;
                                $scope.s_total_balance = s_total_balance;
                                $scope.s_total_fine = s_total_fine;
                                $scope.s_total_rebate = s_total_rebate;
                                $scope.s_total_waived = s_total_waived;
                                $scope.s_total_adjusted = s_total_adjusted;
                                $scope.s_total_openingbalance = s_total_openingbalance;
                                $scope.s_total_Excess = s_total_Excess;

                                angular.forEach($scope.students, function (qwe) {
                                    qwe.width = 250;
                                })

                                //$scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;
                                //console.log($scope.txtdata);
                                //$scope.aaaa = [{
                                //    title: $scope.txtdata,
                                //    columns: $scope.students
                                //}]


                                $(document).ready(function () {
                                    initGridall();
                                });
                                function initGridall() {
                                    $('#gridall').empty();
                                    gridall = $("#gridall").kendoGrid({
                                        toolbar: ["excel", "pdf"],
                                        excel: {
                                            fileName: "Kendo UI Grid Export.xlsx",
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                        },
                                        pdf: {
                                            fileName: "Kendo UI Grid Export.pdf",
                                            allPages: true
                                        },
                                        dataSource: {
                                            //type: "odata",
                                            //transport: {
                                            //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                            //},
                                            data: $scope.students,
                                            pageSize: 10,
                                            aggregate: [
                                                { name: 'totalpayable', field: 'totalpayable', aggregate: "sum" },
                                                { name: 'FSS_PaidAmount', field: 'FSS_PaidAmount', aggregate: "sum" },
                                                { name: 'balance', field: 'balance', aggregate: "sum" },
                                                { name: 'concession', field: 'concession', aggregate: "sum" },
                                                { name: 'fine', field: 'fine', aggregate: "sum" },
                                                { name: 'rebate', field: 'rebate', aggregate: "sum" },
                                                { name: 'waived', field: 'waived', aggregate: "sum" },
                                                { name: 'adjusted', field: 'adjusted', aggregate: "sum" },
                                                { name: 'openingbalance', field: 'openingbalance', aggregate: "sum" },
                                                { name: 'Excess', field: 'Excess', aggregate: "sum" }

                                            ]
                                        },
                                        sortable: true,
                                        //pageable: false,
                                        pageable: true,
                                        groupable: false,
                                        filterable: true,
                                        columnMenu: true,
                                        reorderable: true,
                                        resizable: true,
                                        columns: $scope.colarrayall,
                                        dataBound: function () {
                                            var pagenum = this.dataSource.page();
                                            var pageitms = this.dataSource.pageSize()
                                            var rows = this.items();
                                            $(rows).each(function () {
                                                var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                var rowLabel = $(this).find(".row-number");
                                                $(rowLabel).html(index);
                                            });
                                        }

                                    }).data("kendoGrid");
                                }


                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }
                        }

                        else if (promise.radioval == "CW") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.categorydata != null && promise.categorydata != "") {
                                $scope.category = promise.categorydata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = true;
                                $scope.clsinscon = false;
                                $scope.stdinsconN = false;
                                $scope.clsinsconN = false;
                                $scope.clscntinsconN = false;
                                $scope.instcntinsconN = false;
                                var cw_total_FSS_PaidAmount = 0;
                                var cw_total_concession = 0;
                                var cw_total_fine = 0;
                                var cw_total_rebate = 0;
                                var cw_total_waived = 0;
                                var cw_total_balance = 0;
                                angular.forEach($scope.category, function (w) {
                                    cw_total_FSS_PaidAmount += w.ByBank;
                                    cw_total_concession += w.ByCard;
                                    cw_total_fine += w.ByCash;
                                    cw_total_waived += w.ByECS;
                                    cw_total_rebate += w.ByOnline;
                                    cw_total_balance += w.ByRTGS;
                                })
                                $scope.cw_total_FSS_PaidAmount = cw_total_FSS_PaidAmount;
                                $scope.cw_total_concession = cw_total_concession;
                                $scope.cw_total_fine = cw_total_fine;
                                $scope.cw_total_rebate = cw_total_rebate;
                                $scope.cw_total_waived = cw_total_waived;
                                $scope.cw_total_balance = cw_total_balance;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }
                        }


                        else if (promise.radioval == "TRMW") {
                            $scope.termflg = true;
                            $scope.stdtermflg = false;

                            $scope.header_list = [];
                            angular.forEach($scope.groupcount, function (role) {
                                $scope.header_list.push(role);
                            })


                            if (promise.classwisedata != null && promise.classwisedata != "") {
                                $scope.Grid_view = false;
                                $scope.print_flag = true;

                                $('#grid123').empty();


                                $scope.totcountfirstnew = promise.classwisedata.length;
                                //  $scope.result = true;
                                var installmentcount = $scope.groupcount.length;


                                $scope.insarray = [{ name: "Paid" }, { name: "Balance" }, { name: "Concession" }, { name: "Arrear" }, { name: "Excess" }, { name: "Charges" }, { name: "Payable" }];
                                $scope.columns1 = [{
                                    field: "Paid", name: "Paid", title: "Total Paid", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                }, {
                                    field: "Balance", name: "Balance", title: "Total Balance", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                }, {
                                    field: "Concession", name: "Concession", title: "Total Concession", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                }, {
                                    field: "Arrear", name: "Arrear", title: "Total Arrear", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                }, {
                                    field: "Excess", name: "Excess", title: "Total Excess", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                }, {
                                    field: "Charges", name: "Charges", title: "Total Charges", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                }, {
                                    field: "Payable", name: "Payable", title: "Total Payable", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                }];

                                $scope.colagg = [
                                    { name: 'Paid', field: 'Paid', aggregate: "sum" },
                                    { name: 'Balance', field: 'Balance', aggregate: "sum" },
                                    { name: 'Concession', field: 'Concession', aggregate: "sum" },
                                    { name: 'Arrear', field: 'Arrear', aggregate: "sum" },
                                    { name: 'Excess', field: 'Excess', aggregate: "sum" },
                                    { name: 'Charges', field: 'Charges', aggregate: "sum" },
                                    { name: 'Payable', field: 'Payable', aggregate: "sum" }

                                ];
                                //$scope.insarray = [{ name: "Paid" }];
                                //$scope.columns1 = [{ field: "Paid", name: "Paid", title: "Total Paid" }, { field: "Balance", name: "Balance", title: "Total Balance" }, { field: "Concession", name: "Concession", title: "Total Concession" }, { field: "Arrear", name: "Arrear", title: "Total Arrear" }, { field: "Excess", name: "Excess", title: "Total Excess" }, { field: "Charges", name: "Charges", title: "Total Charges" }];



                                $scope.newarray = [];
                                var finalarray = 0;
                                finalarray = Number(installmentcount)
                                for (var i = 0; i < $scope.header_list.length; i++) {
                                    for (var j = 0; j < 7; j++) {
                                        $scope.newarray.push({ name: $scope.insarray[j].name, id: $scope.header_list[i].fmT_Id, name1: "hema" + $scope.header_list[i].fmT_Id + $scope.insarray[j].name });

                                    }

                                }
                                $scope.newarray1 = $scope.newarray;


                                // start
                                $scope.data = promise.classwisedata;


                                //MB
                                if (promise.classwisedata != null && promise.classwisedata != "" && promise.classwisedata.length > 0) {

                                    var stu_list_new = [];
                                    angular.forEach(promise.classwisedata, function (op1) {
                                        var stu_id = op1.FMH_Id;
                                        var list_stu = [];
                                        angular.forEach(promise.classwisedata, function (op2) {
                                            if (op2.FMH_Id == stu_id) {
                                                var coun = 0;

                                                angular.forEach($scope.header_list, function (op) {
                                                    if (op2.FMT_Id == op.fmT_Id) {
                                                        list_stu.push({ FMT_Id: op2.FMT_Id, Paid: op2.Paid, Balance: op2.Balance, Concession: op2.Concession, Arrear: op2.Arrear, Excess: op2.Excess, Charges: op2.Charges, Payable: op2.Payable })
                                                        coun += 1;
                                                    }

                                                })
                                                if (coun == 0) {
                                                    list_stu.push({ FMT_Id: op2.FMT_Id, Paid: op2.Paid, Balance: op2.Balance, Concession: op2.Concession, Arrear: op2.Arrear, Excess: op2.Excess, Charges: op2.Charges, Payable: op2.Payable });
                                                }

                                            }


                                        })
                                        if (stu_list_new.length == 0) {
                                            stu_list_new.push({ FMH_Id: stu_id, FMH_FeeName: op1.FMH_FeeName, Installment_Reports: list_stu });
                                        }
                                        else if (stu_list_new.length > 0) {
                                            var already_cnt = 0;
                                            angular.forEach(stu_list_new, function (td) {
                                                if (td.FMH_Id == stu_id) {
                                                    already_cnt += 1;
                                                }
                                            })
                                            if (already_cnt == 0) {
                                                stu_list_new.push({ FMH_Id: stu_id, FMH_FeeName: op1.FMH_FeeName, Installment_Reports: list_stu });
                                            }
                                        }

                                    })


                                    angular.forEach(stu_list_new, function (obj) {
                                        angular.forEach(obj.Installment_Reports, function (obj1) {
                                            angular.forEach($scope.newarray1, function (x) {
                                                if (x.id == obj1.FMT_Id) {
                                                    obj[x.name1] = obj1[x.name];

                                                }
                                            })

                                        })
                                    })

                                    $scope.totcountfirstnew = stu_list_new.length;
                                    $scope.data = stu_list_new;
                                    console.log($scope.data);

                                    //hema
                                    $scope.tempaggary = [];

                                    //   $scope.mainsubhdr = [];
                                    angular.forEach($scope.header_list, function (op) {
                                        op.columns = [];
                                        angular.forEach($scope.columns1, function (op123) {

                                            op.columns.push({
                                                field: "hema" + op.fmT_Id + op123.field, name: "hema" + op.fmT_Id + op123.name, title: op123.title, width: 100, aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                                groupFooterTemplate: "Sum: #=sum#"
                                            });
                                            $scope.tempaggary.push({ field: "hema" + op.fmT_Id + op123.field, name: "hema" + op.fmT_Id + op123.name, aggregate: "sum" });
                                        })
                                        op.title = op.fmT_Name;
                                    })

                                    $scope.colarrayall = [{

                                        title: "SLNO",
                                        template: "<span class='row-number'></span>", width: 75

                                    },
                                    {
                                        name: 'FMH_FeeName', field: 'FMH_FeeName', title: 'Fee Head', width: 200
                                    }

                                    ];


                                    angular.forEach($scope.header_list, function (obj) {
                                        $scope.colarrayall.push(obj);
                                    })
                                    console.log($scope.header_list);

                                    console.log($scope.colarrayall);

                                    console.log($scope.data);
                                    var gridall;

                                    $(document).ready(function () {
                                        initGridall();
                                        //$(".k-grid-toolbar").prepend('<div class="gridTitle"><h4  style="color:white;" class="titlecolor">' + "Report For" + "   " + $scope.routename + '</h4></div>');

                                    });

                                    //$scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;
                                    //console.log($scope.txtdata);
                                    //$scope.aaaa = [{
                                    //    title: $scope.txtdata,
                                    //    columns: $scope.colarrayall
                                    //}]


                                    function initGridall() {
                                        gridall = $("#grid123").kendoGrid({
                                            toolbar: ["excel", "pdf"],
                                            excel: {
                                                fileName: "Kendo UI Grid Export.xlsx",
                                                proxyURL: "",
                                                filterable: true
                                            },
                                            pdf: {
                                                fileName: "Kendo UI Grid Export.pdf"
                                            },
                                            dataSource: {
                                                //type: "odata",
                                                //transport: {
                                                //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                                //},
                                                data: $scope.data,
                                                aggregate: $scope.tempaggary
                                            },

                                            sortable: true,
                                            pageable: false,
                                            groupable: false,
                                            filterable: true,
                                            columnMenu: true,
                                            reorderable: true,
                                            resizable: true,
                                            columns: $scope.colarrayall,
                                            dataBound: function () {
                                                var rows = this.items();
                                                $(rows).each(function () {
                                                    var index = $(this).index() + 1;
                                                    var rowLabel = $(this).find(".row-number");
                                                    $(rowLabel).html(index);
                                                });
                                            }
                                        }).data("kendoGrid");
                                        gridall.setOptions({
                                            sortable: true
                                        });
                                    }
                                    //hema
                                }
                            }
                            else {
                                swal("No Record Found");

                            }
                        }

                        else if (promise.radioval == "STRMW") {
                            $scope.stdtermflg = true;

                            $scope.header_list = [];
                            angular.forEach($scope.groupcount, function (role) {
                                if (role.fmT_Id_chk) {
                                    $scope.header_list.push(role);
                                }
                            })

                            if (promise.classwisedata != null && promise.classwisedata != "") {
                                $scope.Grid_view = false;
                                $scope.print_flag = true;

                                $('#grid1234').empty();

                                $scope.totcountfirstnew = promise.classwisedata.length;
                                //  $scope.result = true;
                                var installmentcount = $scope.groupcount.length;


                                $scope.insarray = [{ name: "Charges" }, { name: "Arrear" }, { name: "Concession" }, { name: "WavedoffAmount" }, { name: "AdjustedAmount" }, { name: "Payable" }, { name: "Paid" }, { name: "Balance" }, { name: "Excess" }, { name: "RefundAmount" }];
                                $scope.columns1 = [

                                    {
                                        field: "Charges", name: "Charges", title: "Total Charges", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },
                                    {
                                        field: "Arrear", name: "Arrear", title: "Total Arrear", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },
                                    {
                                        field: "Concession", name: "Concession", title: "Total Concession", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },
                                    {
                                        field: "WavedoffAmount", name: "WavedoffAmount", title: "Total WaivedOff", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },
                                    {
                                        field: "AdjustedAmount", name: "AdjustedAmount", title: "Total AdjustedAmount", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },

                                    {
                                        field: "Payable", name: "Payable", title: "Total Payable", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },

                                    {
                                        field: "Paid", name: "Paid", title: "Total Paid", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },

                                    {
                                        field: "Balance", name: "Balance", title: "Total Balance", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },
                                    {
                                        field: "Excess", name: "Excess", title: "Total Excess", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },
                                    {
                                        field: "RefundAmount", name: "RefundAmount", title: "Total Refund", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },
                                    {
                                        field: "adjusted", name: "adjusted", title: "Total Adjusted", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    }



                                ];

                                $scope.colagg = [
                                    { name: 'Charges', field: 'Charges', aggregate: "sum" },
                                    { name: 'Arrear', field: 'Arrear', aggregate: "sum" },

                                    { name: 'Concession', field: 'Concession', aggregate: "sum" },
                                    { name: 'WavedoffAmount', field: 'WavedoffAmount', aggregate: "sum" },
                                    { name: 'AdjustedAmount', field: 'AdjustedAmount', aggregate: "sum" },

                                    { name: 'Payable', field: 'Payable', aggregate: "sum" },
                                    { name: 'Paid', field: 'Paid', aggregate: "sum" },
                                    { name: 'Balance', field: 'Balance', aggregate: "sum" },

                                    { name: 'Excess', field: 'Excess', aggregate: "sum" },
                                    { name: 'RefundAmount', field: 'RefundAmount', aggregate: "sum" },
                                    { name: 'adjusted', field: 'adjusted', aggregate: "sum" },




                                ];
                                //$scope.insarray = [{ name: "Paid" }];
                                //$scope.columns1 = [{ field: "Paid", name: "Paid", title: "Total Paid" }, { field: "Balance", name: "Balance", title: "Total Balance" }, { field: "Concession", name: "Concession", title: "Total Concession" }, { field: "Arrear", name: "Arrear", title: "Total Arrear" }, { field: "Excess", name: "Excess", title: "Total Excess" }, { field: "Charges", name: "Charges", title: "Total Charges" }];

                                $scope.newarray = [];
                                var finalarray = 0;
                                finalarray = Number(installmentcount)
                                for (var i = 0; i < $scope.header_list.length; i++) {
                                    for (var j = 0; j < 10; j++) {
                                        $scope.newarray.push({ name: $scope.insarray[j].name, id: $scope.header_list[i].fmT_Id, name1: "hema" + $scope.header_list[i].fmT_Id + $scope.insarray[j].name });

                                    }

                                }
                                $scope.newarray1 = $scope.newarray;


                                // start
                                $scope.data = promise.classwisedata;


                                //MB
                                if (promise.classwisedata != null && promise.classwisedata != "" && promise.classwisedata.length > 0) {



                                    var stu_list_new = [];
                                    angular.forEach(promise.classwisedata, function (op1) {
                                        var stu_id = op1.AMST_Id;
                                        var list_stu = [];
                                        var count1 = 0;


                                        angular.forEach(promise.classwisedata, function (op2) {
                                            if (op2.AMST_Id == stu_id) {
                                                var coun = 0;


                                                angular.forEach($scope.header_list, function (op) {
                                                    if (op2.FMT_Id == op.fmT_Id) {
                                                        //list_stu.push({ FMT_Id: op2.FMT_Id, Paid: op2.Paid, Balance: op2.Balance, Concession: op2.Concession, Arrear: op2.Arrear, Excess: op2.Excess, Charges: op2.Charges, Payable: op2.Payable })
                                                        list_stu.push({ FMT_Id: op2.FMT_Id, Charges: op2.Charges, Arrear: op2.Arrear, Concession: op2.Concession, WavedoffAmount: op2.WavedoffAmount, AdjustedAmount: op2.AdjustedAmount, Payable: op2.Payable, Paid: op2.Paid, Balance: op2.Balance, Excess: op2.Excess, RefundAmount: op2.RefundAmount })
                                                        coun += 1;
                                                    }
                                                })
                                                if (coun == 0) {
                                                    list_stu.push({ FMT_Id: op2.FMT_Id, Charges: op2.Charges, Arrear: op2.Arrear, Concession: op2.Concession, WavedoffAmount: op2.WavedoffAmount, AdjustedAmount: op2.AdjustedAmount, Payable: op2.Payable, Paid: op2.Paid, Balance: op2.Balance, Excess: op2.Excess, RefundAmount: op2.RefundAmount });
                                                }
                                            }

                                        })
                                        if (stu_list_new.length == 0) {
                                            stu_list_new.push({ AMST_Id: stu_id, StudentName: op1.StudentName, ASMCL_ClassName: op1.ASMCL_ClassName, CDNetAmount: op1.CDNetAmount, CDPaidAmount: op1.CDPaidAmount, CDBalanceAmount: op1.CDBalanceAmount, Installment_Reports: list_stu });
                                        }
                                        else if (stu_list_new.length > 0) {
                                            var already_cnt = 0;
                                            angular.forEach(stu_list_new, function (td) {
                                                if (td.AMST_Id == stu_id) {
                                                    already_cnt += 1;
                                                }
                                            })
                                            if (already_cnt == 0) {
                                                stu_list_new.push({ AMST_Id: stu_id, StudentName: op1.StudentName, ASMCL_ClassName: op1.ASMCL_ClassName, CDNetAmount: op1.CDNetAmount, CDPaidAmount: op1.CDPaidAmount, CDBalanceAmount: op1.CDBalanceAmount, Installment_Reports: list_stu });
                                            }
                                        }


                                        // list_stu.push({ FMT_Id: op2.FMT_Id, Paid: op2.Paid, Balance: op2.Balance, Concession: op2.Concession, Arrear: op2.Arrear, Excess: op2.Excess, Charges: op2.Charges, Payable: op2.Payable });




                                    })

                                    angular.forEach(stu_list_new, function (obj) {

                                        angular.forEach($scope.groupcount, function (trm) {
                                            var a = 0;
                                            var b = 0;
                                            var c = 0;
                                            var d = 0;
                                            var e = 0;
                                            var f = 0;
                                            var g = 0;
                                            var h = 0;
                                            var i = 0;
                                            var j = 0;
                                            if (trm.fmT_Id_chk == true) {
                                                angular.forEach(obj.Installment_Reports, function (obj1) {
                                                    if (trm.fmT_Id == obj1.FMT_Id) {
                                                        a += obj1.Arrear;
                                                        b += obj1.Balance;
                                                        c += obj1.Charges;
                                                        d += obj1.Concession;
                                                        e += obj1.Excess;
                                                        f += obj1.Paid;
                                                        g += obj1.Payable;

                                                        h += obj1.WavedoffAmount;
                                                        i += obj1.AdjustedAmount;
                                                        j += obj1.RefundAmount;
                                                        //k += obj1.adjusted;
                                                        //angular.forEach($scope.newarray1, function (x) {
                                                        //    if (x.id == obj1.FMT_Id) {
                                                        //        obj[x.name1] = obj1[x.name];

                                                        //    }
                                                        //})

                                                    }
                                                })
                                                angular.forEach($scope.newarray1, function (x) {
                                                    if (x.id == trm.fmT_Id) {

                                                        if (x.name == "Charges") {
                                                            obj[x.name1] = c;
                                                        }
                                                        else if (x.name == "Arrear") {
                                                            obj[x.name1] = a;
                                                        }
                                                        else if (x.name == "Concession") {
                                                            obj[x.name1] = d;
                                                        }
                                                        else if (x.name == "WavedoffAmount") {
                                                            obj[x.name1] = h;
                                                        }
                                                        else if (x.name == "AdjustedAmount") {
                                                            obj[x.name1] = i;
                                                        }
                                                        else if (x.name == "Payable") {
                                                            obj[x.name1] = g;
                                                        }
                                                        else if (x.name == "Paid") {
                                                            obj[x.name1] = f;
                                                        }
                                                        else if (x.name == "Balance") {
                                                            obj[x.name1] = b;
                                                        }
                                                        else if (x.name == "Excess") {
                                                            obj[x.name1] = e;
                                                        }
                                                        else if (x.name == "RefundAmount") {
                                                            obj[x.name1] = j;
                                                        }
                                                        else if (x.name == "adjusted") {
                                                            obj[x.name1] = k;
                                                        }

                                                    }
                                                })
                                            }

                                        })

                                        //angular.forEach($scope.newarray1, function (x) {

                                        //    if (x.name == "Paid") {
                                        //        obj[x.name1] = f;
                                        //    }
                                        //    else if (x.name == "Balance"){
                                        //        obj[x.name1] = b;
                                        //    }
                                        //    else if (x.name == "Concession") {
                                        //        obj[x.name1] = d;
                                        //    }       
                                        //    else if (x.name == "Arrear") {
                                        //        obj[x.name1] = a;
                                        //    }
                                        //    else if (x.name == "Excess") {
                                        //        obj[x.name1] = e;
                                        //    } else if (x.name == "Charges") {
                                        //        obj[x.name1] = c;
                                        //    }
                                        //    else if (x.name == "Payable") {
                                        //        obj[x.name1] = g;
                                        //    }
                                        //    })
                                        // test_list.push({ paid: tPaid, balance: tBalance });
                                    })

                                    $scope.totcountfirstnew = stu_list_new.length;
                                    $scope.data = stu_list_new;
                                    console.log($scope.data);

                                    //hema
                                    $scope.tempaggary = [];

                                    //   $scope.mainsubhdr = [];
                                    angular.forEach($scope.header_list, function (op) {
                                        op.columns = [];
                                        angular.forEach($scope.columns1, function (op123) {

                                            op.columns.push({
                                                field: "hema" + op.fmT_Id + op123.field, name: "hema" + op.fmT_Id + op123.name, title: op123.title, width: 100, aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                                groupFooterTemplate: "Sum: #=sum#"
                                            });
                                            $scope.tempaggary.push({ field: "hema" + op.fmT_Id + op123.field, name: "hema" + op.fmT_Id + op123.name, aggregate: "sum" });
                                        });

                                        op.title = op.fmT_Name;
                                    })

                                    $scope.colarrayall = [{

                                        title: "SLNO",
                                        template: "<span class='row-number'></span>", width: 75

                                    },
                                    {
                                        name: 'StudentName', field: 'StudentName', title: 'Student Name', width: 200
                                    },
                                    {
                                        name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class:Section', width: 100
                                    },
                                    {
                                        name: 'CDNetAmount', field: 'CDNetAmount', title: 'CD Net Amount', width: 100, aggregates: ["sum"]
                                    },
                                    {
                                        name: 'CDPaidAmount', field: 'CDPaidAmount', title: 'CD Paid Amount', width: 100
                                    },

                                    {
                                        name: 'CDBalanceAmount', field: 'CDBalanceAmount', title: 'CD Balance Amount', width: 100
                                    }
                                    ];



                                    angular.forEach($scope.header_list, function (obj) {
                                        $scope.colarrayall.push(obj);
                                    })
                                    console.log($scope.header_list);

                                    console.log($scope.colarrayall);


                                    //angular.forEach($scope.colarrayall, function (qwe) {
                                    //    $scope.tempaggary.push(qwe);
                                    //})
                                    console.log($scope.data);
                                    var gridall;

                                    $(document).ready(function () {
                                        initGridall();
                                        //$(".k-grid-toolbar").prepend('<div class="gridTitle"><h4  style="color:white;" class="titlecolor">' + "Report For" + "   " + $scope.routename + '</h4></div>');


                                    });
                                    function initGridall() {
                                        gridall = $("#grid1234").kendoGrid({
                                            toolbar: ["excel", "pdf"],
                                            excel: {
                                                fileName: "ConsolidatedStudentTermWise.xlsx",
                                                proxyURL: "",
                                                filterable: true
                                            },
                                            pdf: {
                                                fileName: "ConsolidatedStudentTermWise.pdf"
                                            },
                                            dataSource: {
                                                //type: "odata",
                                                //transport: {
                                                //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                                //},
                                                data: $scope.data,
                                                aggregate: $scope.tempaggary
                                            },

                                            sortable: true,
                                            pageable: false,
                                            groupable: false,
                                            filterable: true,
                                            columnMenu: true,
                                            reorderable: true,
                                            resizable: true,
                                            columns: $scope.colarrayall,
                                            dataBound: function () {
                                                var rows = this.items();
                                                $(rows).each(function () {
                                                    var index = $(this).index() + 1;
                                                    var rowLabel = $(this).find(".row-number");
                                                    $(rowLabel).html(index);
                                                });
                                            }
                                        }).data("kendoGrid");
                                        gridall.setOptions({
                                            sortable: true
                                        });
                                    }

                                    //hema

                                }

                            }
                            else {
                                swal("No Record Found");
                            }

                        }

                        else if (promise.radioval == "FPW") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            $scope.fpw1234 = true;
                            if (promise.studentalldata != null && promise.studentalldata != "") {
                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>"

                                },
                                {
                                    name: 'AMST_FatherName', field: 'AMST_FatherName', title: 'Father Name'
                                },
                                {
                                    name: 'AMST_MotherName', field: 'AMST_MotherName', title: 'Mother Name'
                                },



                                {
                                    name: 'StudentName', field: 'StudentName', title: 'Student Name'
                                },
                                {
                                    name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Admission no'
                                },
                                {
                                    name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class Name'
                                },
                                {
                                    name: 'totalpayable', field: 'totalpayable', title: 'Total Payable', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'FSS_PaidAmount', field: 'FSS_PaidAmount', title: 'Paid Amount', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'balance', field: 'balance', title: 'Balance', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'concession', field: 'concession', title: 'Concession', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'fine', field: 'fine', title: 'Fine', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'rebate', field: 'rebate', title: 'Rebate', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },

                                {
                                    name: 'waived', field: 'waived', title: 'Waived', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                },
                                {
                                    name: 'adjusted', field: 'adjusted', title: 'Adjusted', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                }
                                ];
                                $scope.students = promise.studentalldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = true;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.stdinsconN = false;
                                $scope.clsinsconN = false;
                                $scope.clscntinsconN = false;
                                $scope.instcntinsconN = false;
                                var s_total_FSS_PaidAmount = 0;
                                var s_total_balance = 0;
                                var s_total_concession = 0;
                                var s_total_fine = 0;
                                var s_total_rebate = 0;
                                var s_total_waived = 0;
                                var s_total_payable = 0;
                                var s_total_adjusted = 0;
                                angular.forEach($scope.students, function (stu) {
                                    s_total_payable += stu.totalpayable;
                                    s_total_FSS_PaidAmount += stu.FSS_PaidAmount;
                                    s_total_balance += stu.balance;
                                    s_total_concession += stu.concession;
                                    s_total_fine += stu.fine;
                                    s_total_rebate += stu.rebate;
                                    s_total_waived += stu.waived;
                                    s_total_adjusted += stu.adjusted;
                                })

                                $scope.s_total_FSS_PayableAmount_p = s_total_payable;

                                $scope.s_total_FSS_PaidAmount = s_total_FSS_PaidAmount;
                                $scope.s_total_concession = s_total_concession;
                                $scope.s_total_balance = s_total_balance;
                                $scope.s_total_fine = s_total_fine;
                                $scope.s_total_rebate = s_total_rebate;
                                $scope.s_total_waived = s_total_waived;
                                $scope.s_total_adjusted = s_total_adjusted;

                                angular.forEach($scope.students, function (qwe) {
                                    qwe.width = 250;
                                })




                                $(document).ready(function () {
                                    initGridall();
                                });
                                function initGridall() {
                                    $('#fpw1234').empty();
                                    gridall = $("#gridall").kendoGrid({
                                        toolbar: ["excel", "pdf"],
                                        excel: {
                                            fileName: "Kendo UI Grid Export.xlsx",
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                        },
                                        pdf: {
                                            fileName: "Kendo UI Grid Export.pdf",
                                            allPages: true
                                        },
                                        dataSource: {

                                            data: $scope.students,
                                            pageSize: 10,
                                            aggregate: [
                                                { name: 'totalpayable', field: 'totalpayable', aggregate: "sum" },
                                                { name: 'FSS_PaidAmount', field: 'FSS_PaidAmount', aggregate: "sum" },
                                                { name: 'balance', field: 'balance', aggregate: "sum" },
                                                { name: 'concession', field: 'concession', aggregate: "sum" },
                                                { name: 'fine', field: 'fine', aggregate: "sum" },
                                                { name: 'rebate', field: 'rebate', aggregate: "sum" },
                                                { name: 'waived', field: 'waived', aggregate: "sum" },
                                                { name: 'adjusted', field: 'adjusted', aggregate: "sum" }

                                            ]
                                        },
                                        sortable: true,
                                        //pageable: false,
                                        pageable: true,
                                        groupable: false,
                                        filterable: true,
                                        columnMenu: true,
                                        reorderable: true,
                                        resizable: true,
                                        columns: $scope.colarrayall,
                                        dataBound: function () {
                                            var pagenum = this.dataSource.page();
                                            var pageitms = this.dataSource.pageSize()
                                            var rows = this.items();
                                            $(rows).each(function () {
                                                var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                var rowLabel = $(this).find(".row-number");
                                                $(rowLabel).html(index);
                                            });
                                        }

                                    }).data("kendoGrid");
                                }


                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }
                        }

                        ////Added By Praveen gouda
                        else if (promise.radioval == "SWTPC") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.swtpCount != null && promise.swtpCount.length > 0) {
                                $scope.stdcnt = promise.swtpCount;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.clsinsconN = false;
                                $scope.stdinsconN = false;
                                $scope.stdinsconN = true;
                                $scope.clscntinsconN = false;
                                $scope.instcntinsconN = false;
                                var d_total_FSS_PaidAmount = 0;
                                var d_total_concession = 0;
                                var d_total_balance = 0;
                                var d_total_fine = 0;
                                var d_total_rebate = 0;
                                var d_total_waived = 0;
                                var d_total_adjusted = 0;
                                var d_total_totalpayable = 0;
                                var d_total_excess = 0;
                                var d_total_openingbalance = 0;
                                angular.forEach($scope.stdcnt, function (cls) {
                                    d_total_FSS_PaidAmount += cls.FSS_PaidAmount;
                                    d_total_balance += cls.balance;
                                    d_total_concession += cls.concession;
                                    d_total_fine += cls.fine;
                                    d_total_rebate += cls.rebate;
                                    d_total_waived += cls.waived;
                                    d_total_adjusted += cls.adjusted;
                                    d_total_totalpayable += cls.totalpayable;
                                    d_total_excess += cls.Excess;
                                    d_total_openingbalance += cls.openingbalance;
                                })
                                $scope.d_total_FSS_PaidAmount = d_total_FSS_PaidAmount;
                                $scope.d_total_balance = d_total_balance;
                                $scope.d_total_concession = d_total_concession;
                                $scope.d_total_fine = d_total_fine;
                                $scope.d_total_rebate = d_total_rebate;
                                $scope.d_total_waived = d_total_waived;
                                $scope.d_total_adjusted = d_total_adjusted;
                                $scope.d_total_totalpayable = d_total_totalpayable;
                                $scope.d_total_Excess = d_total_excess;
                                $scope.d_total_openingbalance = d_total_openingbalance;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }

                        }

                        ////Class wise Total Paid count
                        else if (promise.radioval == "CWTPC") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.cwtpCount != null && promise.cwtpCount.length > 0) {
                                $scope.clscnt = promise.cwtpCount;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.clsinsconN = false;
                                $scope.stdinsconN = false;
                                $scope.instcntinsconN = false;
                                $scope.clscntinsconN = true;
                                var d_total_FSS_PaidAmount = 0;
                                var d_total_concession = 0;
                                var d_total_balance = 0;
                                var d_total_fine = 0;
                                var d_total_rebate = 0;
                                var d_total_waived = 0;
                                var d_total_adjusted = 0;
                                var d_total_totalpayable = 0;
                                var d_total_excess = 0;
                                var d_total_openingbalance = 0;
                                angular.forEach($scope.clscnt, function (cls) {
                                    d_total_FSS_PaidAmount += cls.FSS_PaidAmount;
                                    d_total_balance += cls.balance;
                                    d_total_concession += cls.concession;
                                    d_total_fine += cls.fine;
                                    d_total_rebate += cls.rebate;
                                    d_total_waived += cls.waived;
                                    d_total_adjusted += cls.adjusted;
                                    d_total_totalpayable += cls.totalpayable;
                                    d_total_excess += cls.Excess;
                                    d_total_openingbalance += cls.openingbalance;
                                })
                                $scope.d_total_FSS_PaidAmount = d_total_FSS_PaidAmount;
                                $scope.d_total_balance = d_total_balance;
                                $scope.d_total_concession = d_total_concession;
                                $scope.d_total_fine = d_total_fine;
                                $scope.d_total_rebate = d_total_rebate;
                                $scope.d_total_waived = d_total_waived;
                                $scope.d_total_adjusted = d_total_adjusted;
                                $scope.d_total_totalpayable = d_total_totalpayable;
                                $scope.d_total_Excess = d_total_excess;
                                $scope.d_total_openingbalance = d_total_openingbalance;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }

                        }

                        ////Installment wise Paid count
                        else if (promise.radioval == "INMW") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.installmentwisedata != null && promise.installmentwisedata.length > 0) {
                                $scope.instcnt = promise.installmentwisedata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.clsinsconN = false;
                                $scope.stdinsconN = false;
                                $scope.clscntinsconN = false;
                                $scope.instcntinsconN = true;
                                var d_total_Payable = 0;
                                var d_total_Paid = 0;
                                var d_total_Balance = 0;
                                var d_total_Concession = 0;
                                var d_total_Arrear = 0;
                                var d_total_Excess = 0;
                                var d_total_Charge = 0;
                                var d_total_adjusted = 0;
                                angular.forEach($scope.instcnt, function (inst) {
                                    d_total_Payable += inst.Payable;
                                    d_total_Paid += inst.Paid;
                                    d_total_Balance += inst.Balance;
                                    d_total_Concession += inst.Concession;
                                    d_total_Arrear += inst.Arrear;
                                    d_total_Excess += inst.Excess;
                                    d_total_Charge += inst.Charge;
                                    d_total_adjusted += inst.adjusted;
                                })
                                $scope.d_total_Payable = d_total_Payable;
                                $scope.d_total_Paid = d_total_Paid;
                                $scope.d_total_Balance = d_total_Balance;
                                $scope.d_total_Concession = d_total_Concession;
                                $scope.d_total_Arrear = d_total_Arrear;
                                $scope.d_total_Excess = d_total_Excess;
                                $scope.d_total_Charge = d_total_Charge;
                                $scope.d_total_adjusted = d_total_adjusted;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }

                        }

                    })
            }
            else {
                $scope.submitted = true;

            }
        };
        $scope.exportToExcel = function () {

            if ($scope.result == "FGW") {
                if ($scope.printdatatablegrp !== null && $scope.printdatatablegrp.length > 0) {
                    var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }

            }
            else if ($scope.result == "FHW") {
                if ($scope.printdatatablehad !== null && $scope.printdatatablehad.length > 0) {
                    var exportHref = Excel.tableToExcel(tablehad, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            else if ($scope.result == "FCW") {
                if ($scope.printdatatablecls !== null && $scope.printdatatablecls.length > 0) {
                    var exportHref = Excel.tableToExcel(tablecls, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            else if ($scope.result == "FSW") {
                //    var table = "tablestd";
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var exportHref = Excel.tableToExcel(tablestd, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            else if ($scope.result == "CW") {
                if ($scope.printdatatablecat !== null && $scope.printdatatablecat.length > 0) {
                    var exportHref = Excel.tableToExcel(tablecat, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            else if ($scope.result == "CTW") {
                if ($scope.printdatatableclsinscon !== null && $scope.printdatatableclsinscon.length > 0) {
                    var exportHref = Excel.tableToExcel(tableclsclscon, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            else if ($scope.result == "CTC") {
                if ($scope.printdatatableclsinsconN !== null && $scope.printdatatableclsinsconN.length > 0) {
                    var exportHref = Excel.tableToExcel(tableclsclsconN, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            /////Student Wise Total paid count
            else if ($scope.result == "SWTPC") {
                if ($scope.printdatatablestd !== null && $scope.printdatatablestd.length > 0) {
                    var excelnamemain = "Student Wise Total Paid count";
                    var exportHref = Excel.tableToExcel(tablestdcnt, 'sheet name');
                    $timeout(function () {
                        var a = document.createElement('a');
                        a.href = exportHref;
                        a.download = excelnamemain;
                        document.body.appendChild(a);
                        a.click();
                        a.remove();
                    }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }

            ////Class Section wise Total paid count
            else if ($scope.result == "CWTPC") {
                if ($scope.printdatatableclass !== null && $scope.printdatatableclass.length > 0) {
                    var excelnamemain = "Class Section Wise Total Paid count";
                    var exportHref = Excel.tableToExcel(tableclscnt, 'sheet name');
                    $timeout(function () {
                        var a = document.createElement('a');
                        a.href = exportHref;
                        a.download = excelnamemain;
                        document.body.appendChild(a);
                        a.click();
                        a.remove();
                    }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }

            ////Installment Wise paid count
            else if ($scope.result == "INMW") {
                if ($scope.printdatatableinst !== null && $scope.printdatatableinst.length > 0) {
                    var excelnamemain = "Installment Wise Paid count";
                    var exportHref = Excel.tableToExcel(tableinstcnt, 'sheet name');
                    $timeout(function () {
                        var a = document.createElement('a');
                        a.href = exportHref;
                        a.download = excelnamemain;
                        document.body.appendChild(a);
                        a.click();
                        a.remove();
                    }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
        }
        $scope.printData = function () {


            if ($scope.result == "FGW") {
                if ($scope.printdatatablegrp !== null && $scope.printdatatablegrp.length > 0) {
                    var innerContents = document.getElementById("printSectionIdgrp").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }
            else if ($scope.result == "FHW") {
                if ($scope.printdatatablehad !== null && $scope.printdatatablehad.length > 0) {
                    var innerContents = document.getElementById("printSectionIdhad").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }
            else if ($scope.result == "FCW") {
                if ($scope.printdatatablecls !== null && $scope.printdatatablecls.length > 0) {
                    var innerContents = document.getElementById("printSectionIdcls").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }
            else if ($scope.result == "FSW") {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var innerContents = document.getElementById("printSectionIdstd").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }

            }
            else if ($scope.result == "CW") {
                if ($scope.printdatatablecat !== null && $scope.printdatatablecat.length > 0) {
                    var innerContents = document.getElementById("printSectionIdcat").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }

            else if ($scope.result == "CTW") {
                if ($scope.printdatatableclsinscon !== null && $scope.printdatatableclsinscon.length > 0) {
                    var innerContents = document.getElementById("printSectionIdclsinscon").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }

            else if ($scope.result == "CTC") {
                if ($scope.printdatatableclsinsconN !== null && $scope.printdatatableclsinsconN.length > 0) {
                    var innerContents = document.getElementById("printSectionIdclsinsconN").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }

            else if ($scope.result == "SWTPC") {
                if ($scope.printdatatablestd !== null && $scope.printdatatablestd.length > 0) {
                    var innerContents = document.getElementById("printSectionIdstdtotalpaidcount").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }

            else if ($scope.result == "CWTPC") {
                if ($scope.printdatatableclass !== null && $scope.printdatatableclass.length > 0) {
                    var innerContents = document.getElementById("printSectionIdclasstotalpaidcount").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }

            else if ($scope.result == "INMW") {
                if ($scope.printdatatableinst !== null && $scope.printdatatableinst.length > 0) {
                    var innerContents = document.getElementById("printSectionIdinsttotalpaidcount").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }

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

        $scope.Cancel = function () {
            $state.reload();
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



                apiService.create("FeeCollectionReport/get_groups", data).
                    then(function (promise) {


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
    }
})();
