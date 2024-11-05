(function () {
    'use strict';
    angular
.module('app')
.controller('FeeConcessionReportController', FeeConcessionReportController)
    FeeConcessionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeeConcessionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
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

        $scope.adyr = true;
        $scope.frmdt = false;
        $scope.Grid_view = false;
        //  $scope.frmdt = true;
        $scope.printdatatable = [];
        $scope.printdatatablegrp = [];
        $scope.printdatatablehad = [];
        $scope.printdatatablecls = [];


        $scope.usercheckcustGrp = true;
        $scope.usercheckGrp = true;
        $scope.usercheckC = false;


        $scope.all_checkcustGrp = function () {
            var checkStatus = $scope.usercheckcustGrp;
            angular.forEach($scope.custom, function (itmtype) {
                itmtype.fmgG_Id_chk = checkStatus;
            });


        }
        $scope.all_checkGrp = function () {
            var checkStatus = $scope.usercheckGrp;
            angular.forEach($scope.group, function (itmtype) {
                itmtype.fmG_Id_chk = checkStatus;
            });


        }
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            angular.forEach($scope.groupcount, function (itmtype) {
                itmtype.fmT_Id_chk = checkStatus;
            });


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
           

        }

        $scope.get_total_class_print = function () {
            var c_total_concession_p = 0;
            var h_total_concession_p = 0;
            var g_total_concession_p = 0;
            var s_total_concession_p = 0;
           
            angular.forEach($scope.printdatatable, function (cls) {
               
               
                s_total_concession_p += cls.concession;
            })
            angular.forEach($scope.printdatatablegrp, function (cls) {

                g_total_concession_p += cls.concession;
                
            })
            angular.forEach($scope.printdatatablehad, function (cls) {

                h_total_concession_p += cls.concession;

            })
            angular.forEach($scope.printdatatablecls, function (cls) {

                c_total_concession_p += cls.concession;

            })
           
            $scope.c_total_concession_p = c_total_concession_p;
            $scope.h_total_concession_p = h_total_concession_p;
            $scope.g_total_concession_p = g_total_concession_p;
            $scope.s_total_concession_p = s_total_concession_p;
        }
        $scope.toggleAllstd = function () {
            
            var toggleStatus = $scope.stdall;
            $scope.printdatatable = [];
            angular.forEach($scope.students, function (itm) {
                itm.stdselected = toggleStatus;
                if ($scope.stdall == true) {
                    $scope.printdatatable.push(itm);
                }
                //else {
                //    $scope.printdatatable.splice(itm);
                //}
                
            });
            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_print();
        }
        $scope.optionToggledstd = function (SelectedStudentRecord, index) {
            
            $scope.stdall = $scope.students.every(function (itm)
            { return itm.stdselected; });
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
            $scope.get_total_group_print();
        }
        //$scope.get_total_group_print = function () {
        //    var g_total_FSS_PaidAmount_p = 0;
        //    var g_total_concession_p = 0;
        //    var g_total_fine_p = 0;
        //    var g_total_rebate_p = 0;
        //    var g_total_waived_p = 0;
        //    angular.forEach($scope.printdatatablegrp, function (gp) {
        //        g_total_FSS_PaidAmount_p += gp.FSS_PaidAmount;
        //        g_total_concession_p += gp.concession;
        //        g_total_fine_p += gp.fine;
        //        g_total_rebate_p += gp.rebate;
        //        g_total_waived_p += gp.waived;
        //    })
        //    $scope.g_total_FSS_PaidAmount_p = g_total_FSS_PaidAmount_p;
        //    $scope.g_total_concession_p = g_total_concession_p;
        //    $scope.g_total_fine_p = g_total_fine_p;
        //    $scope.g_total_rebate_p = g_total_rebate_p;
        //    $scope.g_total_waived_p = g_total_waived_p;
        //}
        $scope.toggleAllgrp = function () {
            
            $scope.printdatatablegrp = [];
            var toggleStatus = $scope.grpall;
            angular.forEach($scope.groups, function (itm) {
                itm.grpselected = toggleStatus;
                if ($scope.grpall == true) {
                    $scope.printdatatablegrp.push(itm);
                }
                //else {
                //    $scope.printdatatablegrp.splice(itm);
                //}
            });
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_print();
        }
        $scope.optionToggledgrp = function (SelectedStudentRecord, index) {
            
            $scope.grpall = $scope.groups.every(function (itm)
            { return itm.grpselected; });

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
            $scope.get_total_class_print();
        }
     
        $scope.toggleAllhad = function () {
            
            var toggleStatus = $scope.hadall;
            $scope.printdatatablehad = [];
            angular.forEach($scope.heads, function (itm) {
                itm.hadselected = toggleStatus;
                if ($scope.hadall == true) {
                    $scope.printdatatablehad.push(itm);
                }
                //else {
                //    $scope.printdatatablehad.splice(itm);
                //}
            });
            if ($scope.printdatatablehad.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_class_print();
        }
        $scope.optionToggledhad = function (SelectedStudentRecord, index) {
            
            $scope.hadall = $scope.heads.every(function (itm)
            { return itm.hadselected; });
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
            $scope.get_total_class_print();
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
        $scope.optionToggledcls = function (SelectedStudentRecord, index) {
            
            $scope.clsall = $scope.classes.every(function (itm)
            { return itm.clsselected; });
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

        $scope.onclickloaddata = function () {
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



        var temp_grp_list = [];






        $scope.loaddata = function () {
            $scope.std = false;
            $scope.grp = false;
            $scope.cls = false;
            $scope.had = false;
            $scope.custom_check_flag = true;
            $scope.group_check_flag = true;
            $scope.custom_check = "0";
            $scope.group_check = "0";
            $scope.load_group_check();
            $scope.load_custom_check();
            var data = {
                "reporttype": grouporterm,
            }
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            apiService.create("FeeConcessionReport/getalldetails", data).
             then(function (promise) {
                 $scope.arrlist6 = promise.adcyear;
                 $scope.groupcount = promise.fillmastergroup;
                 $scope.arrlist6 = promise.adcyear;
                 $scope.groupcount = promise.fillmastergroup;
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


                 $scope.group = promise.grouplist;
                 temp_grp_list = promise.grouplist;




             });
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
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

        $scope.clear_feecon = function () {

            $state.reload();
           
        }
        $scope.submitted = false;
        $scope.ShowReport = function () {
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

                if ($scope.rpttyp == "year") {

                    var data = {

                        "FMG_Id": $scope.fmG_Id,
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        "studenttype": $scope.status,
                        "customflag": $scope.custom_check,
                        "groupflag": $scope.group_check,
                        "active": $scope.active,
                        "deactive": $scope.deactive,
                        "left": $scope.left,
                        //"termflag": $scope.custom_check,
                        TempararyArrayhEADListnew1: $scope.albumNameArraycolumn1,
                        TempararyArrayhEADListnew2: $scope.albumNameArraycolumn2,
                        TempararyArrayhEADListnew3: $scope.albumNameArraycolumn3,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        "term_group": grouporterm,
                    }

                }
                else if ($scope.rpttyp == "date") {
                    var data = {

                        "FMG_Id": $scope.fmG_Id,
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id,
                        "active": $scope.active,
                        "deactive": $scope.deactive,
                        "left": $scope.left,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "From_Date": new Date($scope.fromDate).toDateString(),
                        "To_Date": new Date($scope.todate).toDateString(),
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        "studenttype": $scope.status,
                        "customflag": $scope.custom_check,
                        "groupflag": $scope.group_check,
                        TempararyArrayhEADListnew1: $scope.albumNameArraycolumn1,
                        TempararyArrayhEADListnew2: $scope.albumNameArraycolumn2,
                        TempararyArrayhEADListnew3: $scope.albumNameArraycolumn3,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        "term_group": grouporterm,
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }


                
                apiService.create("FeeConcessionReport/radiobtndata", data).
               then(function (promise) {
                   
                   $scope.reportdetails = promise.searchstudentDetails;
                   if (promise.radioval == "FGW") {
                       if (promise.groupalldata != null && promise.groupalldata != "") {
                           $scope.groups = promise.groupalldata;
                           $scope.Grid_view = true;
                           $scope.print_flag = false;
                           $scope.std = false;
                           $scope.cls = false;
                           $scope.had = false;
                           $scope.grp = true;
                           var g_total_FSS_PaidAmount = 0;
                           var g_total_concession = 0;
                           var g_total_fine = 0;
                           var g_total_rebate = 0;
                           var g_total_waived = 0;
                           angular.forEach($scope.groups, function (gp) {
                               g_total_FSS_PaidAmount += gp.FSS_PaidAmount;
                               g_total_concession += gp.concession;
                              
                           })
                           $scope.g_total_FSS_PaidAmount = g_total_FSS_PaidAmount;
                           $scope.g_total_concession = g_total_concession;
                          
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
                           var h_total_FSS_PaidAmount = 0;
                           var h_total_concession = 0;
                           var h_total_fine = 0;
                           var h_total_rebate = 0;
                           var h_total_waived = 0;
                           angular.forEach($scope.heads, function (hd) {
                               h_total_FSS_PaidAmount += hd.FSS_PaidAmount;
                               h_total_concession += hd.concession;
                             
                           })
                           $scope.h_total_FSS_PaidAmount = h_total_FSS_PaidAmount;
                           $scope.h_total_concession = h_total_concession;
                         
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
                           var c_total_FSS_PaidAmount = 0;
                           var c_total_concession = 0;
                           var c_total_fine = 0;
                           var c_total_rebate = 0;
                           var c_total_waived = 0;
                           angular.forEach($scope.classes, function (cls) {
                               c_total_FSS_PaidAmount += cls.FSS_PaidAmount;
                               c_total_concession += cls.concession;
                              
                           })
                           $scope.c_total_FSS_PaidAmount = c_total_FSS_PaidAmount;
                           $scope.c_total_concession = c_total_concession;
                          
                       }
                       else {
                           swal("No Record Found");
                           $scope.Grid_view = false;
                           $scope.print_flag = true;
                       }

                   }
                   else if (promise.radioval == "FSW") {
                       if (promise.studentalldata != null && promise.studentalldata != "") {
                           $scope.students = promise.studentalldata;
                           $scope.Grid_view = true;
                           $scope.print_flag = false;
                           $scope.std = true;
                           $scope.cls = false;
                           $scope.had = false;
                           $scope.grp = false;
                           var s_total_FSS_PaidAmount = 0;
                           var s_total_concession = 0;
                           var s_total_fine = 0;
                           var s_total_rebate = 0;
                           var s_total_waived = 0;
                           angular.forEach($scope.students, function (stu) {
                               s_total_FSS_PaidAmount += stu.FSS_PaidAmount;
                               s_total_concession += stu.concession;
                              
                           })
                           $scope.s_total_FSS_PaidAmount = s_total_FSS_PaidAmount;
                           $scope.s_total_concession = s_total_concession;
                          
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
