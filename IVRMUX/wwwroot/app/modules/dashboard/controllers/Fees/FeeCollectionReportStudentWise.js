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
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.std = false;
        $scope.adyr = true;
        $scope.frmdt = false;
        $scope.Grid_view = false;      
        $scope.printdatatablestd = [];   
        $scope.printdatatable = [];

        $scope.student_install_wise = function () {
            $scope.std = false;            
            $scope.Grid_view = false;           
            $scope.printdatatablestd = [];     
            
            $scope.stdall = false;
            angular.forEach($scope.students, function (obj) {
                obj.stdselected = false;
            });

            if ($scope.printdatatablestd.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }           
        }

        $scope.get_total_student_print = function () {
            var s_total_totalpayable_p = 0;
            var s_total_FSS_PaidAmount_p = 0;
            var s_total_concession_p = 0;
            var s_total_balance_p = 0;           
            var s_total_fine_p = 0;
            var s_total_rebate_p = 0;
            var s_total_waived_p = 0;
            var s_total_adjusted_p = 0;
            var s_total_openingbalance_p = 0;
            var s_total_Excess_p = 0;
            angular.forEach($scope.printdatatablestd, function (stu) {
                s_total_totalpayable_p += stu.totalpayable;
                s_total_FSS_PaidAmount_p += stu.FSS_PaidAmount;
                s_total_balance_p += stu.balance;
                s_total_concession_p += stu.concession;
                s_total_fine_p += stu.fine;
                s_total_rebate_p += stu.rebate;
                s_total_waived_p += stu.waived;
                s_total_adjusted_p += stu.adjusted;
                s_total_openingbalance_p += stu.openingbalance;
                s_total_Excess_p += stu.Excess;
            })
            $scope.s_total_totalpayable_p = s_total_totalpayable_p;          
            $scope.s_total_FSS_PaidAmount_p = s_total_FSS_PaidAmount_p;
            $scope.s_total_balance_p = s_total_balance_p;
            $scope.s_total_concession_p = s_total_concession_p;
            $scope.s_total_fine_p = s_total_fine_p;
            $scope.s_total_rebate_p = s_total_rebate_p;
            $scope.s_total_waived_p = s_total_waived_p;
            $scope.s_total_adjusted_p = s_total_adjusted_p;
            $scope.s_total_openingbalance_p = s_total_openingbalance_p;
            $scope.s_total_Excess_p = s_total_Excess_p; 
        }

        $scope.toggleAllstd = function () {

            var toggleStatus = $scope.stdall;
            $scope.printdatatablestd = [];
            angular.forEach($scope.students, function (itm) {
                itm.stdselected = toggleStatus;
                if ($scope.stdall == true) {
                    $scope.printdatatablestd.push(itm);
                }

            });
            if ($scope.printdatatablestd.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();
        }

        $scope.optionToggledstd = function (SelectedStudentRecord, index) {
            $scope.stdall = $scope.students.every(function (itm) { return itm.stdselected; });
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
            $scope.get_total_student_print();
        }
       
        $scope.onselectyear = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,               
            }
            apiService.create("DailyFeeCollReport/getdata", data).
                then(function (promise) {
                    $scope.group = promise.fillfeegroup;
                    angular.forEach($scope.group, function (tr) {
                        tr.Selected = true;
                    })
                })
        }


        $scope.hrdallcheck1 = function () {
            var toggleStatus1 = $scope.checkallhrd1;
            angular.forEach($scope.group, function (itm) {
                itm.fmG_Id_chk = toggleStatus1;
            });
        }

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


        $scope.onclickloaddata = function () {
            $scope.Grid_view = false;        
            $scope.printdatatablestd = [];          
          
            $scope.stdall = false;
            angular.forEach($scope.students, function (obj) {
                obj.stdselected = false;
            });
                                      
            if ($scope.printdatatablestd.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

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
            $scope.result = "FSW";
            $scope.std = false;           
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
                    $scope.classcount = promise.fillclass;
                    $scope.sectioncount = promise.fillsection;

                });
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };      
        $scope.order2 = function (keyname) {
            $scope.sortKey2 = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        }
       
        $scope.setTodate = function (data) {
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
            $scope.result = "FSW";
            $scope.fmG_Id = "";
            if ($scope.rpttyp == "year") {
                $scope.rpttyp = "year";
                $scope.asmaY_Id = "";

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
                else
                {
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
                        TempararyArrayhEADListnew1: $scope.albumNameArraycolumn1,
                        TempararyArrayhEADListnew2: $scope.albumNameArraycolumn2,
                        TempararyArrayhEADListnew3: $scope.albumNameArraycolumn3,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
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
                apiService.create("FeeCollectionReport/radiobtndata", data).then(function (promise) {
                   
                        if (promise.radioval == "FSW") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;

                            if (promise.studentalldata != null && promise.studentalldata != "") {
                                $scope.std = true;
                                $scope.students = promise.studentalldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.clsinsconN = false;                              
                                var s_total_FSS_PaidAmount = 0;
                                var s_total_balance = 0;
                                var s_total_concession = 0;
                                var s_total_fine = 0;
                                var s_total_rebate = 0;
                                var s_total_waived = 0;
                                var s_total_totalpayable = 0;
                                var s_total_adjusted = 0;
                                var s_total_openingbalance = 0;
                                var s_total_Excess = 0;
                                angular.forEach($scope.students, function (stu) {
                                    s_total_totalpayable += stu.totalpayable;
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
                                $scope.s_total_totalpayable = s_total_totalpayable;
                                $scope.s_total_FSS_PaidAmount = s_total_FSS_PaidAmount;
                                $scope.s_total_concession = s_total_concession;
                                $scope.s_total_balance = s_total_balance;
                                $scope.s_total_fine = s_total_fine;
                                $scope.s_total_rebate = s_total_rebate;
                                $scope.s_total_waived = s_total_waived;
                                $scope.s_total_adjusted = s_total_adjusted;
                                $scope.s_total_openingbalance = s_total_openingbalance;
                                $scope.s_total_Excess = s_total_Excess;                           
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }
                        }
                                                           
                    })
            }
            else
            {
                $scope.submitted = true;

            }
        };

        //Export To Excel
        $scope.exportToExcel = function () {                                   
            if ($scope.result == "FSW") {
                if ($scope.printdatatablestd !== null && $scope.printdatatablestd.length > 0) {
                    var exportHref = Excel.tableToExcel(tablestd, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }           
        }

        //Print Data
        $scope.printData = function () {
                                  
            if ($scope.result == "FSW") {
                if ($scope.printdatatablestd !== null && $scope.printdatatablestd.length > 0) {
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

            return !$scope.custom.some(function (options) {
                return options.fmgG_Id_chk1;
            });
        }
        $scope.is_optionrequired_groupterm_grp = function () {
            return !$scope.group.some(function (options) {
                return options.fmG_Id_chk1;
            });
        }

        //Cancel---
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
                $scope.group = [];
            }
        }
    }
})();
