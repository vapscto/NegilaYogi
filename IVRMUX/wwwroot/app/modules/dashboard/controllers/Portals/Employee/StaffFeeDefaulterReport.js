

(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeDefaulterReportController', FeeDefaulterReportController)

    FeeDefaulterReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeeDefaulterReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        $scope.tadprint = false;
        $scope.class = false;
        $scope.route = false;
        $scope.Ismailsms = false;
        $scope.adlflags = false;

        //by deepak
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
        //End by 
        // $scope.IsHiddenup = true;
        $scope.printdatatable = [];
        $scope.printdatatablegrp = [];
        $scope.printdatatablehad = [];
        $scope.printdatatablecls = [];
        $scope.itemterm = [];
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        //var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));


        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }


        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
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
        $scope.search = "";
        //for total
        $scope.get_total_class_print = function () {
            var totalgrp = 0;
            var totalhad = 0;
            var totalcls = 0;
            var totalstu = 0;

            angular.forEach($scope.printdatatable, function (cls) {
                totalgrp += cls.totalbalance;
            })
            angular.forEach($scope.printdatatablegrp, function (cls) {

                totalhad += cls.totalbalance;

            })
            angular.forEach($scope.printdatatablehad, function (cls) {

                totalcls += cls.totalbalance;

            })
            angular.forEach($scope.printdatatablecls, function (cls) {

                totalstu += cls.totalbalance;

            })

            $scope.totalgrp = totalgrp;
            $scope.totalhad = totalhad;
            $scope.totalcls = totalcls;
            $scope.totalstu = totalstu;
        }

        //end

        //Custom Group Check All
        $scope.hrdallcheckcustom = function () {
            var toggleStatus1 = $scope.checkallhrdcustom;
            angular.forEach($scope.groupcount, function (itm) {
                itm.fmT_Id_chk = toggleStatus1;
            });
        }

        $scope.binddatagrp2 = function () {
            $scope.checkallhrdcustom = $scope.groupcount.every(function (role) {
                return role.fmT_Id_chk;
            });
        };
        //Ended

        $scope.toggleAllstd = function () {

            var toggleStatus = $scope.stdall;
            angular.forEach($scope.students, function (itm) {
                itm.stdselected = toggleStatus;
                if ($scope.stdall == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

            var balstd = 0;
            for (var q = 0; q < $scope.students.length; q++) {
                if ($scope.students[q].stdselected == true) {
                    balstd = balstd + $scope.students[q].totalbalance;
                }
            }
            $scope.selectedbalstd = balstd;
            $scope.get_total_class_print();
        }

        $scope.IsRequired = function () {

            return !$scope.termlst.some(function (options) {
                return options.trmids;
            });
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

            var balstd = 0;
            for (var q = 0; q < $scope.students.length; q++) {
                if ($scope.students[q].stdselected == true) {
                    balstd = balstd + $scope.students[q].totalbalance;
                }
            }
            $scope.selectedbalstd = balstd;
            $scope.get_total_class_print();
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }




        $scope.toggleAllgrp = function () {

            var toggleStatus = $scope.grpall;
            angular.forEach($scope.groups, function (itm) {
                itm.grpselected = toggleStatus;
                if ($scope.grpall == true) {
                    $scope.printdatatablegrp.push(itm);
                }
                else {
                    $scope.printdatatablegrp.splice(itm);
                }
            });
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            var balgrp = 0;
            for (var q = 0; q < $scope.groups.length; q++) {
                if ($scope.groups[q].grpselected == true) {
                    balgrp = balgrp + $scope.groups[q].totalbalance;
                }
            }
            $scope.selectedbalgrp = balgrp;
            $scope.get_total_class_print();
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

            var balgrp = 0;

            for (var q = 0; q < $scope.groups.length; q++) {
                if ($scope.groups[q].grpselected == true) {
                    balgrp = balgrp + $scope.groups[q].totalbalance;
                }
            }
            $scope.selectedbalgrp = balgrp;

            $scope.selectedbalamt = obj.selectedbal

            $scope.get_total_class_print();




        }

        $scope.toggleAllhad = function () {

            var toggleStatus = $scope.hadall;
            angular.forEach($scope.heads, function (itm) {
                itm.hadselected = toggleStatus;
                if ($scope.hadall == true) {
                    $scope.printdatatablehad.push(itm);
                }
                else {
                    $scope.printdatatablehad.splice(itm);
                }
            });
            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

            var balhad = 0;
            for (var q = 0; q < $scope.heads.length; q++) {
                if ($scope.heads[q].hadselected == true) {
                    balhad = balhad + $scope.heads[q].totalbalance;
                }
            }
            $scope.selectedbalhad = balhad;
            $scope.get_total_class_print();
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
            var balhad = 0;
            for (var q = 0; q < $scope.heads.length; q++) {
                if ($scope.heads[q].hadselected == true) {
                    balhad = balhad + $scope.heads[q].totalbalance;
                }
            }
            $scope.selectedbalhad = balhad;
            $scope.get_total_class_print();
        }
        $scope.toggleAllcls = function () {

            var toggleStatus = $scope.clsall;
            angular.forEach($scope.classes, function (itm) {
                itm.clsselected = toggleStatus;
                if ($scope.clsall == true) {
                    $scope.printdatatablecls.push(itm);
                }
                else {
                    $scope.printdatatablecls.splice(itm);
                }
            });
            if ($scope.printdatatablecls.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            var balcls = 0;
            for (var q = 0; q < $scope.classes.length; q++) {
                if ($scope.classes[q].clsselected == true) {
                    balcls = balcls + $scope.classes[q].totalbalance;
                }
            }
            $scope.selectedbalcls = balcls;
            $scope.get_total_class_print();
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
            var balcls = 0;
            for (var q = 0; q < $scope.classes.length; q++) {
                if ($scope.classes[q].clsselected == true) {
                    balcls = balcls + $scope.classes[q].totalbalance;
                }
            }
            $scope.selectedbalcls = balcls;
            $scope.get_total_class_print();
        }

        // $scope.status = "act";
        $scope.due = "duedate";
        //  var temp_grp_list = [];



        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            $scope.IsHiddenup = true;
            var pageid = 1;
            $scope.std = false;
            $scope.grp = false;
            $scope.cls = false;
            $scope.had = false;
            $scope.fmG_Class_Flag = true;
            $scope.fmG_Installment_Flag = true;
            $scope.active = true;
            //$scope.custom_check_flag = true;
            //$scope.group_check_flag = true;
            //$scope.custom_check = "0";
            //$scope.group_check = "0";
            //$scope.load_group_check();
            //$scope.load_custom_check();
            $scope.Gdate = false;
            //var data = {
            //    "reporttype": grouporterm,
            //}
            var pageid = 1;

            apiService.getURI("FeeDefaulterReport/getalldetails", pageid).
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


                    $scope.arrlist6 = promise.adcyear;
                    //$scope.groupcount = promise.fillmastergroup;
                    //$scope.termlst = promise.fillterms;
                    // $scope.classcount = promise.fillclass;
                    $scope.installmentcount = promise.fillinstallment;
                    //  temp_grp_list = promise.grouplist;
                })
        }

        $scope.getclass = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeDefaulterReport/getstaffwiseclass", data).
                then(function (promise) {

                    //$scope.staff_flag = promise.staff_flag;
                    //$scope.designationflag = "";
                    //if ($scope.staff_flag.length > 0 && $scope.staff_flag != null) {
                    //    $scope.designationflag = $scope.staff_flag[0].elP_Flg;
                    //}
                    if (promise.fillclass.length > 0 && promise.fillclass != null) {                 
                        $scope.classcount = promise.fillclass;                       
                    }
                    else {
                        swal("Class Not Mapped....!!");
                    }
                    $scope.adlflags = true;
                    $scope.getstaffterms(data);


                })
        }

        $scope.getstaffterms = function (yearid) {
            var data = {
                "reporttype": grouporterm,
                "ASMAY_Id": yearid.ASMAY_Id,
            }
            apiService.create("FeeDefaulterReport/getStaffterms", data).
                then(function (promise) {

                    $scope.groupcount = promise.fillmastergroup;
                    $scope.termlst = promise.fillterms;
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
        $scope.stu_class = false;
        $scope.class_drpdis = function () {

            if ($scope.class == "1") {
                $scope.fmG_Class_Flag = false;
                $scope.stu_class = true;
                $scope.section = true;

            }
            else if ($scope.class == "0") {
                $scope.fmG_Class_Flag = true;
                $scope.stu_class = false;
                $scope.section = false;
            }
        }

        $scope.route_id = function () {

            if ($scope.route == "1") {
                $scope.trmR_Id = true;
            }
            else if ($scope.route == "0") {
                $scope.trmR_Id = false;

            }
        }

        //====================save remark==============
        $scope.Saveremark = function () {
            $scope.remarkarray = [];
            angular.forEach($scope.students, function (qq) {
                if (qq.FSDREM_Remarks !== null && qq.FSDREM_Remarks !== undefined && qq.FSDREM_Remarks !== '') {
                    $scope.remarkarray.push(qq);
                }
            });
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                remarkarray: $scope.remarkarray
            }
            apiService.create("FeeDefaulterReport/saveremark", data).
                then(function (promise) {
                    if (promise.message === 'Add') {
                        swal('Remark Added Successfully.');
                        $state.reload();
                    }
                    else if (promise.message === 'Error') {
                        swal('Remark Not Added Successfully.');
                        $state.reload();
                    }
                    else {
                        swal('Operation Failed');
                    }
                });
        };

        //===============================================


        $scope.fee_instalmt = false;
        $scope.installment_drpdis = function () {

            if ($scope.installment == "1") {
                $scope.fmG_Installment_Flag = false;
                $scope.fee_instalmt = true;
            }
            else if ($scope.installment == "0") {
                $scope.fmG_Installment_Flag = true;
                $scope.fee_instalmt = false;
            }
        }


        //adding section 
        $scope.onselectclass = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.fmG_Class,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            apiService.create("FeeDefaulterReport/getsection", data).
                then(function (promise) {
                    $scope.sectioncount = promise.fillsection;
                    //  $scope.arrlstinst1 = promise.fillinstallment;
                })
        }


        $scope.student_install_wise = function () {


            $scope.std = false;
            $scope.grp = false;
            $scope.cls = false;
            $scope.had = false;
            $scope.Grid_view = false;
            $scope.printdatatable = [];
            $scope.printdatatablegrp = [];
            $scope.printdatatablehad = [];
            $scope.printdatatablecls = [];
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
            if ($scope.result == "FIW") {
                $scope.install_drdd = true;
                $scope.student_btns = false;
                $scope.Ismailsms = false;
                $scope.adlflags = false;
            }
            else if ($scope.result == "FSW") {
                $scope.install_drdd = false;
                $scope.student_btns = false;
                $scope.Ismailsms = false;
                $scope.adlflags = true;
            }
            else {
                $scope.install_drdd = false;
                $scope.student_btns = false;
                $scope.Ismailsms = false;
                $scope.adlflags = false;
            }

        }

        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? true : false;
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clear_feedef = function () {

            $state.reload();
            // $scope.loaddata();
            //if ($scope.rpttyp == "year") {
            //    $scope.rpttyp = "year";
            //    $scope.asmaY_Id = "";
            //    // $scope.asmC_Id = "";

            //}
            //else if ($scope.rpttyp == "date") {
            //    $scope.rpttyp == "date";
            //    $scope.fromDate = null;
            //    //$scope.todate = "";
            //    $scope.due = "duedate";
            //    //  $scope.asmC_Id = " ";
            //}
            //if ($scope.class == "1") {
            //    $scope.fmG_Class = "";
            //    $scope.section = false;
            //}
            //if ($scope.installment == "1") {
            //    $scope.fmG_Installment = "";
            //}

            ////$scope.fmG_Id = "";
            //$scope.class = false;
            //$scope.route = false;
            //$scope.trmids = "";
            //$scope.fmT_Id = "";
            //$scope.asmC_Id = "";
            //$scope.status = "act";
            //$scope.result = "FGW";
            //$scope.Grid_view = false;
            //$scope.print_flag = true;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            //// $scope.termlst = '';
            //$scope.loaddata();

        }
        $scope.Grid_view = false;
        $scope.print_flag = true;
        $scope.submitted = false;
        $scope.rpttyp = "year";
        $scope.adyr = true;
        $scope.result = "FGW";
        //$scope.status = "act";
        //$scope.due = "duedate";
        $scope.ShowReport = function (termlst, asmaY_Id, fromDate, rpttyp, result, due, asmcL_Id, trmR_Id) {

            $scope.stdall = [];
            $scope.grpall = [];
            $scope.hadall = [];
            $scope.clsall = [];
            $scope.printdatatablegrp = [];
            $scope.printdatatablehad = [];
            $scope.printdatatablecls = [];
            $scope.printdatatable = [];

            if ($scope.myForm.$valid) {






                if ($scope.result == "FSW") {
                    $scope.Ismailsms = true;
                }
                else {
                    $scope.Ismailsms = false;
                }

                angular.forEach($scope.arrlist6, function (y) {
                    if (y.asmaY_Id == $scope.asmaY_Id) {
                        $scope.acdyr = y.asmaY_Year;
                    }
                })




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


                if ($scope.rpttyp == "year") {


                    var data = {
                        "FMG_Id": $scope.fmG_Id,
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        //"studenttype": $scope.status,
                        "active": $scope.active,
                        "deactive": $scope.deactive,
                        "left": $scope.left,
                        "customflag": $scope.custom_check,
                        "groupflag": $scope.group_check,
                        "ASMCL_Id": $scope.fmG_Class,
                        "AMSC_Id": $scope.asmC_Id,
                        "TRMR_Id": $scope.trmR_Id,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        "term_group": grouporterm,
                        "Select_Button": "Student",
                    }
                }
                else if ($scope.rpttyp == "date") {
                    var data = {

                        "ASMAY_Id": $scope.asmaY_Id,
                        "From_Date": fromDate,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        // "studenttype": $scope.status,
                        "active": $scope.active,
                        "deactive": $scope.deactive,
                        "left": $scope.left,
                        "duedate": $scope.due,
                        //"customflag": $scope.custom_check,
                        //"groupflag": $scope.group_check,
                        "ASMCL_Id": $scope.fmG_Class,
                        "AMSC_Id": $scope.asmC_Id,
                        "TRMR_Id": $scope.trmR_Id,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        "term_group": grouporterm,
                        "Select_Button": "Student",
                    }
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                $scope.getTotalstd = function (int) {
                    var total = 0;
                    angular.forEach($scope.students, function (e) {
                        total += e.totalbalance;
                    });
                    return total;
                };

                $scope.getTotalgrp = function (int) {
                    var total = 0;
                    angular.forEach($scope.groups, function (e) {
                        total += e.totalbalance;
                    });
                    return total;
                };
                $scope.getTotalhd = function (int) {
                    var total = 0;
                    angular.forEach($scope.heads, function (e) {
                        total += e.totalbalance;
                    });
                    return total;
                };
                $scope.getTotalcls = function (int) {
                    var total = 0;
                    angular.forEach($scope.classes, function (e) {
                        total += e.totalbalance;
                    });
                    return total;
                };


                apiService.create("FeeDefaulterReport/radiobtndata", data).
                    then(function (promise) {


                        $scope.mI_ID = promise.mI_ID;
                        $scope.reportdetails = promise.searchstudentDetails;
                        $scope.FromDate = new Date();
                        $scope.dueduration = promise.month;
                        //angular.forEach($scope.dueduration, function (value, key) {
                        //    
                        //    $scope.Duration = value.month;
                        //    $scope.Enddate = value.enddate;
                        //});







                        if (promise.radioval == "FGW") {
                            if (promise.groupalldata != null && promise.groupalldata != "") {
                                $scope.groups = promise.groupalldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = true;
                                $scope.tot = $scope.getTotalgrp(promise.groupalldata);
                                $scope.totcountfirst = promise.groupalldata.length;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }

                        }
                        else if (promise.radioval == "FHW") {
                            if (promise.headalldata != null && promise.headalldata != "") {
                                $scope.heads = promise.headalldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = true;
                                $scope.grp = false;
                                $scope.tot = $scope.getTotalhd(promise.headalldata);
                                $scope.totcountfirst = promise.headalldata.length;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }

                        }
                        else if (promise.radioval == "FCW") {
                            if (promise.classalldata != null && promise.classalldata != "") {
                                $scope.classes = promise.classalldata;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = true;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.tot = $scope.getTotalcls(promise.classalldata);
                                $scope.totcountfirst = promise.classalldata.length;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }

                        }
                        else if (promise.radioval == "FSW") {
                            if (promise.mI_ID == 5) {
                                $scope.bdate = false;
                            }
                            else {
                                $scope.bdate = true;
                            }


                            if (promise.studentalldata != null && promise.studentalldata != "") {

                                $scope.students = promise.studentalldata;
                                //$scope.duedate = promise.date
                                if ($scope.asmcL_Id != 0) {
                                    $scope.duedate = new Date(promise.feesummlist[0].date);
                                }
                                if ($scope.route == "1") {
                                    //  $scope.routename = promise.studentalldata[0].TRMR_RouteName;

                                    angular.forEach($scope.installmentcount, function (rt) {
                                        if (rt.trmR_Id == $scope.trmR_Id) {
                                            $scope.routename = rt.trmR_RouteNo + ' : ' + rt.trmR_RouteName;
                                        }
                                    });

                                    $scope.Gdate = true;
                                }
                                else {
                                    $scope.Gdate = false;
                                }



                                if ($scope.students.length != 0) {
                                    $scope.Grid_view = true;
                                    $scope.print_flag = false;
                                    $scope.std = true;
                                    $scope.cls = false;
                                    $scope.had = false;
                                    $scope.grp = false;
                                    $scope.tot = $scope.getTotalstd(promise.studentalldata);
                                    $scope.totcountfirst = promise.studentalldata.length;
                                }
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



        // $scope.frmdt = true;
        $scope.frmdt = false;
        $scope.install_drdd = false;
        $scope.student_btns = false;
        //$scope.install_drdd = true;


        $scope.onclickloaddata = function () {

            if ($scope.rpttyp == "year") {

                $scope.frmdt = false;

                $scope.adyr = true;

                //  $scope.clear_feedef();
            }
            else if ($scope.rpttyp == "date") {

                $scope.frmdt = true;

                $scope.adyr = true;

                //  $scope.clear_feedef();

            }
        };


        //$scope.SendMSG = function (Text) {

        //    $scope.albumNameArray = [];
        //    angular.forEach($scope.reportdetails, function (user) {
        //        if (!!user.selected) $scope.albumNameArray.push(user);
        //    })

        //    var data = {
        //        "SmsMailStudentDetails": $scope.albumNameArray,
        //        "SmsMailText": Text
        //    };
        //    apiService.create("FeeDefaulterReport/SendSms", data)
        //    $scope.$apply();
        //    $scope.PostDataResponse = data;
        //    alert('SMS Sent Successfully')
        //    $scope.saved = "SMS Sent Successfully";
        //};

        //$scope.SendMAIL = function (Text) {

        //    $scope.albumNameArray = [];
        //    angular.forEach($scope.reportdetails, function (user) {
        //        if (!!user.selected) $scope.albumNameArray.push(user);
        //    })

        //    var data = {
        //        "SmsMailStudentDetails": $scope.albumNameArray,
        //        "SmsMailText": Text
        //    };
        //    apiService.create("FeeDefaulterReport/SendMail", data)
        //    $scope.$apply();
        //    $scope.PostDataResponse = data;
        //    alert('MAIL Sent Successfully')
        //    $scope.saved = "MAIL Sent Successfully";
        //};



        //$scope.printData = function () {
        //    

        //    var divToPrint = document.getElementById("table2");
        //    var newWin = window.open("");
        //    newWin.document.write(divToPrint.outerHTML);
        //    newWin.print();
        //    newWin.close();
        //}
        //$("#btnExport").click(function (e) {
        //    window.open('data:application/vnd.ms-excel,' + $('#Table').html());
        //    e.preventDefault();
        //});



        $scope.printData = function () {

            if ($scope.result == "FGW") {
                if ($scope.mI_ID == 5) {
                    $scope.bdate = false;
                }
                else {
                    $scope.bdate = true;
                }
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
                    // $state.reload();
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
                    // $state.reload();
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
                    // $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }
            }
            else if ($scope.result == "FSW") {
                var pdss = "";
                //if ($scope.mI_ID == 5 || $scope.mI_ID == 4 ) {
                //    $scope.bdate = false;
                //    pdss = 'printSectionIdstd1'
                //}
                //else if ($scope.mI_ID == 6) {
                //    $scope.bdate = true;
                //    pdss = 'printSectionIdstd1'
                //}
                //else {
                //    $scope.bdate = true;
                //    pdss = 'printSectionIdstd'
                //}
                if ($scope.route == "1") {
                    $scope.Gdate = true;
                    pdss = 'printSectionIdstd1'
                }
                else {
                    $scope.Gdate = false;
                    pdss = 'printSectionIdstd'
                }

                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var innerContents = document.getElementById(pdss).innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    // $state.reload();
                }
                else {
                    swal("Please Select Records to be Printed");
                }

            }
        }


        $scope.exportToExcel = function () {

            if ($scope.result == "FGW") {
                if ($scope.printdatatablegrp !== null && $scope.printdatatablegrp.length > 0) {
                    var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
                //  $state.reload();

            }
            else if ($scope.result == "FHW") {
                if ($scope.printdatatablehad !== null && $scope.printdatatablehad.length > 0) {
                    var exportHref = Excel.tableToExcel(tablehad, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
                // $state.reload();
            }
            else if ($scope.result == "FCW") {
                if ($scope.printdatatablecls !== null && $scope.printdatatablecls.length > 0) {
                    var exportHref = Excel.tableToExcel(tablecls, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
                //   $state.reload();
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
                //  $state.reload();
            }


        }
        $scope.selectedStudentList = [];
        $scope.send_sms = function () {
            if ($scope.students != null && $scope.students != "") {
                for (var i = 0; i < $scope.students.length; i++) {
                    if ($scope.students[i].stdselected == true) {
                        $scope.selectedStudentList.push($scope.students[i]);
                    }
                }
            }
            if ($scope.selectedStudentList.length == 0) {
                swal("Please select records to send sms");
            }
            else {
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
                var data = {
                    FMG_Ids: FMG_Ids,
                    FMT_Ids: FMT_Ids,
                    "term_group": grouporterm,
                    "selectedStudentList": $scope.selectedStudentList,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id
                };
                apiService.create("FeeDefaulterReport/SendSms", data).then(function (promise) {
                    if (promise.msg == "successSMS") {
                        swal("SMS Sent Successfully");
                        //$state.reload();
                    }
                    else if (promise.msg == "failedSMS") {
                        swal("SMS Not Sent");
                        // $state.reload();
                    }
                })
            }
        }
        $scope.selectedStudentListforemail = [];
        $scope.send_mail = function () {

            if ($scope.students != null && $scope.students != "") {
                for (var i = 0; i < $scope.students.length; i++) {
                    if ($scope.students[i].stdselected == true) {
                        $scope.selectedStudentListforemail.push($scope.students[i]);
                    }
                }
            }
            $scope.fmtdeatls = [];
            angular.forEach($scope.termlst, function (itm) {
                if (itm.trmids == true) {
                    $scope.fmtdeatls.push(itm);
                }
            });

            if ($scope.selectedStudentListforemail.length == 0) {
                swal("Please select records to send email");
            }
            else {
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
                var data = {
                    FMG_Ids: FMG_Ids,
                    FMT_Ids: FMT_Ids,
                    "term_group": grouporterm,
                    "TempTerm": $scope.fmtdeatls,
                    "selectedStudentListforemail": $scope.selectedStudentListforemail,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id

                };
                apiService.create("FeeDefaulterReport/Sendmail", data).then(function (promise) {
                    if (promise.msg == "success") {
                        swal("Email Sent Successfully");
                        //$state.reload();
                    }
                    else if (promise.msg == "failed") {
                        swal("Email Not Sent");
                        //  $state.reload();
                    }

                })
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
    }
})();