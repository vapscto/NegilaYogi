

(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeStudentConcessionReportController', FeeStudentConcessionReport123)

    FeeStudentConcessionReport123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function FeeStudentConcessionReport123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        $scope.IsHiddenup = true;
        $scope.Selectionrd = "allr";

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
        $scope.searchString = "";
        $scope.file_disable = true;
        $scope.Format_I = false;
        $scope.allcustom = true;
        $scope.allgroup = true;
        $scope.allterm = true;


        $scope.all_checktype = function () {
            var checkStatus = $scope.allcustom;
            angular.forEach($scope.custom, function (itmtype) {
                itmtype.fmgG_Id_chk = checkStatus;
            });


        }
        $scope.all_checkgroup = function () {
            var checkStatus = $scope.allgroup;
            angular.forEach($scope.group, function (itmtype) {
                itmtype.fmG_Id_chk = checkStatus;
            });


        }
        $scope.all_checkterm = function () {
            var checkStatus = $scope.allterm;
            angular.forEach($scope.groupcount, function (itmtype) {
                itmtype.fmT_Id_chk = checkStatus;
            });


        }

        $scope.get_total_student_print_con = function () {
            var totA_p = 0;
            var totB_p = 0;
            var totC_p = 0;
             var totBal_p = 0;
            angular.forEach($scope.printdatatable, function (gp) {
                totA_p += gp.Netamount;
                totB_p += gp.Concession;
                totC_p += gp.Paid;
                 totBal_p += gp.Balance;
            })
            $scope.totA_p = totA_p;
            $scope.totB_p = totB_p;
            $scope.totC_p = totC_p;
              $scope.totBal_p = totBal_p;
        }

        $scope.get_total_student_print = function () {
            var totA_p = 0;
            var totB_p = 0;
          
            if ($scope.Format_I == true) {
                for (var i = 0; i < $scope.printdatatable.length; i++)
                {
                    totA_p += $scope.printdatatable[i].totalnetc;
                    totB_p += $scope.printdatatable[i].totalbalncr;
                }
                $scope.totA_p = totA_p;
                $scope.totB_p = totB_p;
                 
            }
            else {
                angular.forEach($scope.printdatatable, function (gp) {
                    totA_p += gp.Netamount;
                    totB_p += gp.Concession;
                })
                $scope.totA_p = totA_p;
                $scope.totB_p = totB_p;
            }
            //totalbalncr = 9735
            //totalnetc = 15375
            
            


            // var totBal_p=0
            //angular.forEach($scope.printdatatable, function (gp) {
            //    totA_p += gp.Netamount;
            //    totB_p += gp.Concession;
            //     totBal_p += gp.Balance;
            //})
            //$scope.totA_p = totA_p;
            //$scope.totB_p = totB_p;
            // $scope.totBal_p = totBal_p;

        }
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings != null && configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
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



        $scope.onselectyear = function () {

            var data = {
                "ASMAY_Id": $scope.academicyr,
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



        var temp_grp_list = [];
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.rndindCLSS = false;
            $scope.HeadWisereport = false;
            $scope.custom_check_flag = true;
            $scope.group_check_flag = true;
            $scope.custom_check = "0";
            $scope.group_check = "0";
            $scope.load_group_check();
            $scope.load_custom_check();
            var data = {
                "reporttype": grouporterm,
            }
            apiService.create("FeeStudentConcessionReport/getalldetails123", data).
                then(function (promise) {

                    $scope.acayyearbind = promise.acayear;
                    //$scope.arrlistchk = promise.fgrp;
                    //$scope.groupcount = promise.fgrp;
                    $scope.clsdrpdown = promise.classlist;
                    $scope.sectiondrpre = promise.sectionlist;
                    $scope.concategory = promise.concategory;
                    //$scope.individual_drpdis1();
                    //$scope.export_flag = false;
                    //$scope.print_flag = false;
                    $scope.onclickloaddata();
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


                })
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

        $scope.checkradio = function (qq) {
            if (qq === 'Staff') {
                $scope.Selectionrd = "allr";
            }

        };

        $scope.onclickloaddata = function () {
            if ($scope.Selectionrd === "allr") {
                $scope.rndindACC = true;
                $scope.rndindSECC = false;
                $scope.rndindCLSS = false;
                $scope.rndindStff = false;
                $scope.rndindGeneral = false;
                $scope.rndindSpecialFee = false;
                $scope.rndindTotal = true;
                $scope.rndindCDAdj = false;
                $scope.individual_drdd = true;
                $scope.individual_cat = false;
                $scope.fee_group_valid = true;
                $scope.Grid_view = false;
                $scope.export_flag = false;
                $scope.print_flag = false;
                $scope.submitted = false;
                $scope.term = true;
                $scope.load_group_check();
                $scope.load_custom_check();


            }
            else if ($scope.Selectionrd === "Indi") {
                $scope.rndindACC = true;
                $scope.rndindSECC = true;
                $scope.rndindCLSS = true;
                $scope.rndindStff = false;
                $scope.rndindGeneral = false;
                $scope.rndindSpecialFee = true;
                $scope.rndindTotal = true;
                $scope.rndindCDAdj = true;
                $scope.individual_drdd = true;
                $scope.individual_cat = false;
                $scope.fee_group_valid = true;
                $scope.Grid_view = false;
                $scope.export_flag = false;
                $scope.print_flag = false;
                $scope.clsdrp = "";
                $scope.submitted = false;
                $scope.term = true;
                $scope.load_group_check();
                $scope.load_custom_check();
            }
            if ($scope.Selectionrd === "con") {
                $scope.rndindACC = true;
                $scope.rndindSECC = false;
                $scope.rndindCLSS = false;
                $scope.rndindStff = false;
                $scope.rndindGeneral = false;
                $scope.rndindSpecialFee = false;
                $scope.rndindTotal = true;
                $scope.rndindCDAdj = false;
                $scope.individual_drdd = true;
                $scope.individual_cat = false;
                $scope.fee_group_valid = true;
                $scope.Grid_view = false;
                $scope.export_flag = false;
                $scope.print_flag = false;
                $scope.submitted = false;
                $scope.term = true;
                $scope.load_group_check();
                $scope.load_custom_check();


            }
            else if ($scope.Selectionrd === "HeadWise") {
                $scope.term = false;
            }

        };
        // $scope.fee_group = true;
        $scope.individual_drdd = false;
        $scope.fee_group_valid = false;
        $scope.fmG_Individual_Flag = false;
        $scope.individual_drpdis = function () {
            debugger;
            if ($scope.individual == "1") {
                $scope.fmG_Individual_Flag = true;
                $scope.fee_group_valid = false;

            }
            else if ($scope.individual == "0") {
                $scope.fmG_Individual_Flag = false;
                $scope.fee_group_valid = true;
            }
        }
        $scope.individual1 = "0";
        $scope.individual_drpdis1 = function () {
            debugger;
            if ($scope.individual1 == "1") {
                angular.forEach($scope.concategory, function (role) {
                    role.selected = true;
                })
                $scope.fmcC_Individual_Flag = true;
                //    $scope.binddatagrp();
            }
            else if ($scope.individual1 == "0") {
                angular.forEach($scope.concategory, function (role) {
                    role.selected = false;
                })
                $scope.fmcC_Individual_Flag = false;
                //   $scope.binddatagrp();
            }
        }

        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted1;
        };
        $scope.total1 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Netamount;
            });
            return totalc;
        };
        $scope.total2 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Concession;
            });
            return totalc;
        };

        $scope.total3 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Netamount;
            });
            return totalc;
        };
        $scope.total4 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Concession;
            });
            return totalc;
        };

        $scope.total5 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Paid;
            });
            return totalc;
        };

        $scope.clear_stucon = function () {
            $state.reload();
            $scope.loaddata();
        }
        $scope.individual = "0";
        $scope.individual1 = "0";
        $scope.Grid_view = false;
        $scope.print_flag = true;
        $scope.submitted = false;

        $scope.ShowReportdata = function () {
            // if($scope.all)

            if ($scope.myForm.$valid) {
                if ($scope.reporttype == 'Student') {
                    $scope.clsdrp = $scope.clsdrp,
                        $scope.sectiondrp = $scope.sectiondrp
                }
                else {
                    $scope.clsdrp = 0,
                        $scope.sectiondrp = 0
                }




                $scope.totA_p = '';
                $scope.totB_p = '';
                $scope.totC_p = '';
                $scope.printdatatable = [];
                $scope.all = false;
                $scope.ind = false;
                $scope.cons = false;
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
                //var data = {

                //    "typeofrpt": $scope.Selectionrd,
                //    "asmyid": $scope.academicyr,
                //    "clsid": $scope.clsdrp,
                //    "setid": $scope.sectiondrp,
                //    "FMG_Id": $scope.fmG_Id,
                //    "Type": $scope.individual1,
                //    fmcC_ConcessionName: $scope.albumNameArray1,    // Fee concession array list.
                //}

                if (($scope.Selectionrd === "allr") || ($scope.Selectionrd === "con")) {

                    var data = {
                        "typeofrpt": $scope.Selectionrd,
                        "asmyid": $scope.academicyr,
                        //"clsid": $scope.clsdrp,
                        //"setid": $scope.sectiondrp,
                        "FMG_Id": $scope.fmG_Id,
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        "studenttype": $scope.status,
                        "customflag": $scope.custom_check,
                        "groupflag": $scope.group_check,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        "term_group": grouporterm,
                        "FMCC_Id": $scope.fmcC_Id,
                        "report": $scope.reporttype
                    }

                }
                else if ($scope.Selectionrd === "Indi") {

                    var data = {
                        "typeofrpt": $scope.Selectionrd,
                        "asmyid": $scope.academicyr,
                        "clsid": $scope.clsdrp,
                        "setid": $scope.sectiondrp,
                        "FMG_Id": $scope.fmG_Id,
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        "studenttype": $scope.status,
                        "customflag": $scope.custom_check,
                        "groupflag": $scope.group_check,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        "term_group": grouporterm,
                        "FMCC_Id": $scope.fmcC_Id,
                        "report": $scope.reporttype
                    };

                }

                else if ($scope.Selectionrd === "HeadWise") {

                    var data = {
                        "typeofrpt": $scope.Selectionrd,
                        "asmyid": $scope.academicyr,
                        "FMCC_Id": $scope.fmcC_Id,
                        "report": $scope.reporttype
                    }

                }

                apiService.create("FeeStudentConcessionReport/getreport", data).
                    then(function (promise) {

                        if ($scope.Selectionrd === "allr") {
                            if (promise.reportdatelist != null && promise.reportdatelist != "") {
                                $scope.Grid_view = true;
                                $scope.export_flag = true;
                                $scope.print_flag = true;
                                $scope.students123 = [];
                                $scope.students = "";
                                $scope.totcountfirst = promise.reportdatelist.length;
                                $scope.students = promise.reportdatelist;
                                $scope.totA = $scope.total1(promise.reportdatelist);
                                $scope.totB = $scope.total2(promise.reportdatelist);
                                $scope.rndindALLGrid = true;
                                $scope.file_disable = true;
                                $scope.rndindconsol = false;
                                $scope.rndindIndivisual = false;
                                $scope.IsHiddendown = true;
                                $scope.HeadWisereport = false;
                                  //123456
                                var total = 0;
                                var totaolconciaon = 0;
                                var totalbalncr = 0;
                                angular.forEach($scope.students, function (detail) {
                                    total += detail.Netamount;
                                    totaolconciaon += detail.Concession; 
                                    totalbalncr += detail.Balance;
                                    //Concession
                                })
                                $scope.totA_p = total;
                                $scope.totB_p = totaolconciaon;    
                                $scope.totBal_p = totalbalncr;
                                  //123456

                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.export_flag = false;
                                $scope.print_flag = false;
                                $scope.file_disable = true;
                            }
                        }

                        else if ($scope.Selectionrd === "Indi") {

                            if (promise.reportdatelist != null && promise.reportdatelist != "") {
                                $scope.Grid_view = true;
                                $scope.export_flag = true;
                                $scope.print_flag = true;
                                $scope.students = "";
                                $scope.students123 = "";
                                $scope.students = promise.reportdatelist;
                                $scope.totcountfirst = promise.reportdatelist.length;
                                $scope.totA = $scope.total3(promise.reportdatelist);
                                $scope.totB = $scope.total4(promise.reportdatelist);
                                $scope.rndindALLGrid = false;
                                $scope.rndindconsol = false;
                                $scope.rndindIndivisual = true;
                                $scope.IsHiddendown = true;
                                $scope.HeadWisereport = false;
                                //$scope.file_disable = false;
                               //123456
                                var total = 0;
                                var totaolconciaon = 0;
                                var Balance = 0;
                                angular.forEach($scope.students, function (detail) {
                                    total += detail.Netamount;
                                    totaolconciaon += detail.Concession;
                                    Balance += detail.Balance;
                                    //Concession
                                })
                                $scope.totA_p = total;
                                $scope.totB_p = totaolconciaon;
                                $scope.totBal_p = Balance
                                  //123456
                                $scope.employee = [];
                                $scope.employee = $scope.students[0].StudentName;
                                $scope.employeeid = [];
                                angular.forEach($scope.students, function (dev) {
                                    if ($scope.employeeid.length === 0) {
                                        $scope.employeeid.push({
                                            AMST_Id: dev.AMST_Id, StudentName: dev.StudentName, Admno: dev.Admno, Class: dev.Class, Section: dev.Section, FeeGroup: dev.FeeGroup, FeeHead: dev.FeeHead, FSC_ConcessionReason: dev.FSC_ConcessionReason, Netamount: dev.Netamount, Concession: dev.Concession

                                        });

                                    }
                                    else if ($scope.employeeid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.employeeid, function (emp) {
                                            if (emp.AMST_Id === dev.AMST_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.employeeid.push({ AMST_Id: dev.AMST_Id, StudentName: dev.StudentName, Admno: dev.Admno, Class: dev.Class, Section: dev.Section, FeeGroup: dev.FeeGroup, FeeHead: dev.FeeHead, FSC_ConcessionReason: dev.FSC_ConcessionReason, Netamount: dev.Netamount, Concession: dev.Concession });
                                        }
                                    }
                                });

                                console.log($scope.employeeid);
                                angular.forEach($scope.employeeid, function (ddd) {
                                    $scope.templist = [];
                                    var studentwisetotal = 0;
                                    var totalnetcCC = 0;
                                    angular.forEach($scope.students, function (dd) {
                                        if (dd.AMST_Id === ddd.AMST_Id) {
                                            studentwisetotal += dd.Concession;
                                            totalnetcCC += dd.Netamount;
                                            $scope.templist.push(dd);
                                        }
                                    });

                                    //totalbalncr//totalnetcCC
                                    ddd.totalbalncr = studentwisetotal;
                                    ddd.totalnetc = totalnetcCC;
                                    ddd.studentdetailstwo = $scope.templist;
                                });

                                console.log($scope.employeeid);
                                //123456
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.export_flag = false;
                                $scope.print_flag = false;
                                $scope.file_disable = true;
                            }
                        }

                        else if ($scope.Selectionrd === "con") {

                            if (promise.reportdatelist != null && promise.reportdatelist != "") {
                                $scope.Grid_view = true;
                                $scope.export_flag = true;
                                $scope.print_flag = true;
                                $scope.students = "";
                                $scope.students123 = "";
                                $scope.students = promise.reportdatelist;
                                $scope.totcountfirst = promise.reportdatelist.length;
                                $scope.totA = $scope.total3(promise.reportdatelist);
                                $scope.totB = $scope.total4(promise.reportdatelist);
                                $scope.totc = $scope.total5(promise.reportdatelist);
                                $scope.rndindALLGrid = false;
                                $scope.rndindconsol = true;
                                $scope.rndindIndivisual = false;
                                $scope.IsHiddendown = true;
                                $scope.HeadWisereport = false;
                                //$scope.file_disable = false;
                                //123456
                                var total = 0;
                                var Paid = 0;
                                var concion = 0;
                                var totalbalncr = 0;
                                angular.forEach($scope.students, function (detail) {
                                    total += detail.Netamount;
                                    Paid += detail.Paid;
                                    concion += detail.Concession;
                                    totalbalncr += detail.Balance;
                                    //Concession
                                })
                                $scope.totA_p = total;
                                $scope.totC_p = Paid;
                                $scope.totB_p = concion;
                                $scope.totBal_p = totalbalncr;
                                //123456

                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.export_flag = false;
                                $scope.print_flag = false;
                                $scope.file_disable = true;
                            }
                        }

                        else if ($scope.Selectionrd === "HeadWise") {

                            if (promise.reportdatelist !== null && promise.reportdatelist.length > 0) {
                                $scope.reportdatelist = promise.reportdatelist;
                                $scope.HeadWisereport = true;
                                $scope.Grid_view = false;
                                $scope.export_flag = true;
                                $scope.print_flag = true;
                                $scope.rndindALLGrid = false;
                                $scope.table3 = false;
                                $scope.rndindIndivisual = false;
                                $scope.IsHiddendown = true;
                                $scope.header_list = [];
                                $scope.header_listnew = [];

                                if ($scope.reportdatelist.length > 0) {
                                    for (var i = 0; i < $scope.reportdatelist.length; i++) {

                                        if (i === 0) {
                                            angular.forEach($scope.reportdatelist[i], function (key, r) {
                                                $scope.header_list.push({ colmn: r, head: key });
                                            });
                                        }
                                    }
                                }
                                $scope.header_listnew = $scope.header_list;



                                //$scope.students = "";
                                //$scope.students123 = "";
                                //$scope.students = promise.reportdatelist;
                                //$scope.totcountfirst = promise.reportdatelist.length;
                                //$scope.totA = $scope.total3(promise.reportdatelist);
                                //$scope.totB = $scope.total4(promise.reportdatelist);
                                //$scope.totc = $scope.total5(promise.reportdatelist);


                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.export_flag = false;
                                $scope.print_flag = false;
                                $scope.file_disable = true;
                                $scope.HeadWisereport = false;
                            }
                        }

                    })
            }
            else {
                $scope.submitted = true;

            }
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
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



                apiService.create("FeeStudentConcessionReport/get_groups", data).
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




        $scope.clear_feeconss = function () {

            $state.reload();


        }

        $scope.printdatatable = [];
        $scope.exportToExcel = function (table1, table4) {
            var exportHref = "";
            if ($scope.Selectionrd === "HeadWise") {
                exportHref = Excel.tableToExcel(table4, 'FeeHeadWiseExport');
                $timeout(function () { location.href = exportHref; }, 100);
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {

                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    
                    if ($scope.reporttype === 'Student') {

                        if (($scope.Selectionrd === "allr") || ($scope.Selectionrd === "Indi")) {
                            exportHref = Excel.tableToExcel(table1, 'WireWorkbenchDataExport');
                        }

                        else {
                            exportHref = Excel.tableToExcel(table3, 'WireWorkbenchDataExport');
                        }



                    }
                    else {

                        exportHref = Excel.tableToExcel(table2, 'WireWorkbenchDataExport');
                    }



                    $timeout(function () { location.href = exportHref; }, 100);
                    $timeout(function () {
                        location.href = exportHref;
                    }, 100);
                }
                else {
                    swal("Please select records to be Exported");
                }
            }
            // $state.reload();

        }



        $scope.printData = function (printSectionId, printSectionId1) {
         
            var pdss = "";
            if ($scope.Selectionrd === "HeadWise") {
                pdss = 'printSectionId1';
            }

            else {
                if ($scope.reporttype === 'Student') {

                    if (($scope.Selectionrd === "allr") || ($scope.Selectionrd === "Indi")) {
                        if ($scope.Format_I === true && $scope.Selectionrd === "Indi") {
                            pdss = 'printSectionIdDDDD';
                        }
                        else {
                            pdss = 'printSectionId';
                        }
                       
                    }

                    else {
                        pdss = 'printSectioncon';
                    }
                }
                else {
                    pdss = 'printSectionIdstf'
                }
            }

            if ($scope.Selectionrd === "HeadWise") {
             

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


        $scope.toggleAll = function () {
        
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.students1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                //else {
                //    $scope.printdatatable.splice(itm);
                //}
            });
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();

        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });

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


        $scope.toggleAllind = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.ind;
            angular.forEach($scope.students1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.ind == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print();

        }

        //123456
        $scope.toggleAllindDDD = function () {
            
            $scope.printdatatable = [];
            var toggleStatus = $scope.indDDD;
            angular.forEach($scope.employeeid, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.indDDD == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print();

        }
        //123456

        $scope.toggleAllcon = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.cons;
            angular.forEach($scope.students1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.cons == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print_con();

        }


        $scope.optionToggind = function (SelectedStudentRecord, index) {
            
            $scope.ind = $scope.students.every(function (itm) { return itm.selected; });
            // $scope.printdatatable = [];
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

        //123456
        $scope.optionToggindDDD = function (SelectedStudentRecordDD, index) {

            $scope.ind = $scope.printdatatable.every(function (itm) { return itm.selected; });
            //indDDD
            if ($scope.printdatatable.indexOf(SelectedStudentRecordDD) === -1) {
                $scope.printdatatable.push(SelectedStudentRecordDD);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecordDD), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();
        }

        //123456

        $scope.optionToggcon = function (SelectedStudentRecord, index) {
            
            $scope.ind = $scope.students.every(function (itm) { return itm.selected; });
            // $scope.printdatatable = [];
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
            $scope.get_total_student_print_con();
        }




        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }



        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

    }
})();

