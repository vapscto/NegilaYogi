(function () {
    'use strict';
    angular
        .module('app')
        .controller('DailyFeeCollReportController', DailyFeeCollReportController)
    DailyFeeCollReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', 'exportUiGridService', 'uiGridConstants']
    function DailyFeeCollReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, exportUiGridService, uiGridConstants) {

        $scope.colarrayaggre = [];
        $scope.usrname = localStorage.getItem('username');

        $scope.FMCB_fromDATE = new Date();
        $scope.FMCB_toDATE = new Date();
        $scope.checkallhrd1 = true;
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        $scope.headwise1 = false;
        $scope.paymentwise = false;
        $scope.hrdallcheck1 = function () {
            var toggleStatus1 = $scope.checkallhrd1;
            angular.forEach($scope.arrlistchkgroup, function (itm) {
                itm.selected = toggleStatus1;
            });
        }


        $scope.colarray = [{

            title: "SLNO",
            template: "<span class='row-numberind'></span>"

        }, { name: 'Name', field: 'Name', width: '100px', title: 'Student Name' }, { name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Adm No' },

        { name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', width: '100px', title: 'Class' },
        { name: 'ASMC_SectionName', field: 'ASMC_SectionName', width: '100px', title: 'Section' },
        { name: 'MobileNo', field: 'MobileNo', width: '100px', title: 'MobileNo' },
        { name: 'EmailId', field: 'EmailId', width: '100px', title: 'EmailId' },
        { name: 'Gender', field: 'Gender', width: '100px', title: 'Gender' },
        { name: 'AMST_DOB', field: 'AMST_DOB', width: '100px', title: 'DOB' },
        { name: 'TermName', field: 'TermName', width: '100px', title: 'TermName' },

        { name: 'FYP_Receipt_No', field: 'FYP_Receipt_No', width: '100px', title: 'ReceiptNo' },
        { name: 'FYP_Bank_Name', field: 'FYP_Bank_Name', width: '100px', title: 'Bank' },
        { name: 'Date', field: 'Date', title: 'Date' },
        { name: 'FYP_Bank_Or_Cash', field: 'FYP_Bank_Or_Cash', width: '100px', title: 'Mode Of Payment' },
        { name: 'Chequedate', field: 'Chequedate', width: '100px', title: 'Cheque Date' },
        { name: 'FYP_DD_Cheque_No', field: 'FYP_DD_Cheque_No', width: '100px', title: 'Cheque/DD Details' },
        { name: 'fyp_transaction_id', field: 'fyp_transaction_id', width: '100px', title: 'Transaction Id' },
        { name: 'FYP_PaymentReference_Id', field: 'FYP_PaymentReference_Id', width: '100px', title: 'Reference Id' },
        { name: 'FYP_Remarks', field: 'FYP_Remarks', width: '100px', title: 'Remarks' },
        { name: 'AccName', field: 'AccName', width: '100px', title: 'AccName' },
        { name: 'GateWay', field: 'FYP_PayGatewayType', width: '100px', title: 'FYP_PayGatewayType' },
        { name: 'SettlementDate', field: 'Settlement_Date', width: '100px', title: 'SettlementDate' }
        ];

        $scope.colarrayall = [{

            title: "SLNO",
            template: "<span class='row-number'></span>"

        }, { name: 'Date', field: 'Date', title: 'Date' },
        {
            name: 'Receipts_Count', field: 'Receipts_Count', title: 'No.of Receipts Generated', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
            groupFooterTemplate: "Sum: #=sum#"
        },
        {
            name: 'ByBank', field: 'ByBank', title: 'ByBank', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
            groupFooterTemplate: "Sum: #=sum#"
        },
        {
            name: 'ByCash', field: 'ByCash', title: 'ByCash', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
            groupFooterTemplate: "Sum: #=sum#"
        },
        {
            name: 'ByOnline', field: 'ByOnline', title: 'ByOnline', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
            groupFooterTemplate: "Sum: #=sum#"
        },
        {
            name: 'ByCard', field: 'ByCard', title: 'ByCard', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
            groupFooterTemplate: "Sum: #=sum#"
        },
        {
            name: 'ByECS', field: 'ByECS', title: 'ByECS', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
            groupFooterTemplate: "Sum: #=sum#"
        },
        {
            name: 'ByRTGS', field: 'ByRTGS', title: 'ByRTGS', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
            groupFooterTemplate: "Sum: #=sum#"
        },
        {
            name: 'Total', field: 'Total', title: 'Total', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
            groupFooterTemplate: "Sum: #=sum#"
        }

        ];

        $scope.gridOptions = {
            enableRowSelection: true,
            enableSelectAll: true,
            showGridFooter: false,
            showColumnFooter: false,
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [10, 20, 30],
            paginationPageSize: 20,
            enableGridMenu: false,
            columnDefs: $scope.colarray,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Export all data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'selected', 'selected');
                }
            }
            ],
            getFooterValue: function () {
                return $scope.gridApi.grid.columns[2].getAggregationValue();
            }

        };

        $scope.gridOptionsall = {
            enableRowSelection: true,
            enableSelectAll: true,
            showGridFooter: false,
            showColumnFooter: true,
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [10, 20, 30],
            paginationPageSize: 20,
            enableGridMenu: false,
            columnDefs: $scope.colarrayall,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Export all data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'selected', 'selected');
                }
            }
            ],
            getFooterValue: function () {
                return $scope.gridApi.grid.columns[2].getAggregationValue();
            }

        };

        $scope.onselectyear = function () {

            var data = {
                "ASMAY_Id": $scope.ASMAY,
                // "ASMCL_Id": clsobj.asmcL_Id,s
            }
            apiService.create("DailyFeeCollReport/getdata", data).
                then(function (promise) {


                    $scope.arrlistchkgroup = promise.fillfeegroup;


                    angular.forEach($scope.arrlistchkgroup, function (tr) {
                        tr.selected = true;
                    })
                    $scope.binddatagrp();
                    //  $scope.arrlstinst1 = promise.fillinstallment;
                })
        }




        $scope.Grid_View = false;
        //  $scope.table_all = false;
        //$scope.table = false;
        $scope.tadprint = false;
        $scope.printdatatable = [];
        $scope.printdatatable1 = [];
        $scope.toggleAll1 = function () {

            var toggleStatus = $scope.all1;
            angular.forEach($scope.temp_array, function (itm) {
                itm.selected1 = toggleStatus;
                if ($scope.all1 == true) {
                    $scope.printdatatable1.push(itm);
                }
                else {
                    $scope.printdatatable1.splice(itm);
                }
            });



            //var balstd = 0;
            //for (var q = 0; q < $scope.students.length; q++) {
            //    if ($scope.students[q].stdselected == true) {
            //        balstd = balstd + $scope.students[q].totalbalance;
            //    }
            //}
            //$scope.selectedbalstd = balstd;
        }
        $scope.optionToggled1 = function (SelectedStudentRecord1, index) {

            $scope.all1 = $scope.temp_array.every(function (itm) { return itm.selected1; });
            if ($scope.printdatatable1.indexOf(SelectedStudentRecord1) === -1) {
                $scope.printdatatable1.push(SelectedStudentRecord1);
            }
            else {
                $scope.printdatatable1.splice($scope.printdatatable1.indexOf(SelectedStudentRecord1), 1);
            }


            //var balstd = 0;
            //for (var q = 0; q < $scope.students.length; q++) {
            //    if ($scope.students[q].stdselected == true) {
            //        balstd = balstd + $scope.students[q].totalbalance;
            //    }
            //}
            //$scope.selectedbalstd = balstd;

        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.all_list, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
                //if ($scope.printdatatable.length > 0) {
                //    $scope.showbutton = true;
                //}
                //else {
                //    $scope.showbutton = false;
                //}
            });
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.all_list.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            //if ($scope.printdatatable.length > 0) {
            //    $scope.showbutton = true;
            //}
            //else {
            //    $scope.showbutton = false;
            //}
        }


        $scope.checkboxchcked = [];
        $scope.sectioncheckboxchcked = [];
        // $scope.rpttyp = "All";
        $scope.Fmc_allorsudentorother = "All";
        $scope.Fmc_allorcorddorop = "All";
        $scope.individual_flag = true;
        $scope.classdt = false;
        $scope.allcc = true;
        $scope.chequedate = 0;
        $scope.chequedt = false;

        //binding the default values 
        $scope.loaddata = function () {
            $scope.acdmyr = 0;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.tabel12 = false;
            $scope.tabel123 = false;
            apiService.getURI("DailyFeeCollReport/getalldetails", pageid).
                then(function (promise) {
                    $scope.yearlst = promise.fillyear;
                    $scope.ASMAY = $scope.yearlst[0].asmaY_Id;

                    $scope.classlist = promise.fillclass;
                    $scope.seclist = promise.fillsection;
                    $scope.onclickloaddata();
                    $scope.onclickloaddatadate();
                    $scope.arrlistchkgroup = promise.fillfeegroup;

                    angular.forEach($scope.arrlistchkgroup, function (tr) {
                        tr.selected = true;
                    })
                    $scope.cheque_date();
                    $scope.columnsTest = [];
                    $scope.sort = function (keyname) {
                        $scope.sortKey = keyname;   //set the sortKey to the param passed
                        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
                    }
                    $scope.binddatagrp();
                    $scope.fromdatechange();
                    $scope.todatechange();


                })
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //$scope.onclickdata = function () {
        //    if ($scope.rpttyp == "All") {
        //        $scope.individual_flag = true;
        //    }
        //    else if ($scope.rpttyp == "Individual") {
        //        $scope.individual_flag = false;
        //       // $scope.Fmc_allorsudentorother = "Aso";
        //       // $scope.Fmc_allorcorddorop = "Ac";
        //    }
        //}

        $scope.onclickloaddatadate = function () {
            if ($scope.reportdate == "transdate") {
                $scope.details = true;
                $scope.cheque1 = true;

            }
            else if ($scope.reportdate == "settdate" || $scope.reportdate == "SettledOnline") {
                $scope.details = false;
                $scope.cheque1 = false;
                $scope.classdt = false;
            }
        };



        $scope.onclickloaddataclass = function () {
            if ($scope.reportdate == "transdate") {
                if ($scope.rpttyp == "Individual") {
                    $scope.classdt = true;
                    $scope.details = true;
                    $scope.chequedt = false;
                }
                else if ($scope.rpttyp == "All") {
                    $scope.classdt = false;
                    $scope.details = false;
                    $scope.chequedt = false;
                }
            }
            else {
                $scope.details = false;
                $scope.cheque1 = false;
                $scope.classdt = false;
            }
        };
        $scope.prsary = [];

        $scope.isOptionsRequired = function () {
            //  if ($scope.group_check == "1") {

            return !$scope.arrlistchkgroup.some(function (options) {
                return options.selected;
            });
            //  }

        }

        $scope.onclickloaddate = function () {
            if ($scope.Fmc_allorcorddorop == "B") {
                //$scope.chequedt = true;
            }
            else if (($scope.Fmc_allorcorddorop == "All") || ($scope.Fmc_allorcorddorop == "C") || ($scope.Fmc_allorcorddorop == "O") || ($scope.Fmc_allorcorddorop == "Ecs")) {

                //$scope.chequedt = false;
            }
        };






        $scope.stu_class = false;
        $scope.onclickloaddata = function () {
            if ($scope.usercheck == "1") {
                $scope.checked = false;
                $scope.stu_class = true;
            }
            else if ($scope.usercheck == "0") {
                $scope.checked = true;
                $scope.stu_class = false;
            }
            //if ($scope.grp == "1") {
            //    $scope.arrlistchkhead = false;
            //}
            //else {
            //    $scope.arrlistchkhead = true;
            //}
            //if ($scope.recp == "1") {
            //    $scope.recpdrp = false;

            //}
            //else {
            //    $scope.recpdrp = true;
            //}
            //if ($scope.betdates == "1") {

            //    $scope.frmdated = false;
            //    $scope.todated = false;
            //}
            //else {
            //    $scope.frmdated = true;
            //    $scope.todated = true;
            //}
        };
        $scope.group_check = "0";

        //$scope.load_group_check = function () {

        //    if ($scope.group_check == "1") {
        //        $scope.group_check_flag = false;
        //        $scope.binddatagrp();
        //        $scope.temp_array_total = "";
        //        //$scope.grand_total();
        //        //$scope.Grid_View = true;
        //        //$scope.table = true;
        //        //$scope.student_list = "";

        //    }
        //    else if ($scope.group_check == "0") {
        //     
        //        $scope.group_check_flag = true;
        //        $scope.binddatagrp();
        //        $scope.temp_array_total = "";
        //        // $scope.grand_total();
        //        //$scope.Grid_View = true;
        //        //$scope.table = true;
        //        //$scope.student_list = $scope.temporary_list;

        //    }
        //}

        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }

        $scope.IsHiddenup = true;
        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }
        $scope.Fmc_allorind = "indi";
        $scope.onclickloaddataindiall = function () {

            if ($scope.Fmc_allorind == "all") {

                $scope.allcc = true;
                $scope.allcc = true;


            }
            else if ($scope.Fmc_allorind == "indi") {

                $scope.allcc = false;
                $scope.allcc = false;
            }
        };

        $scope.cheque_date = function () {
            if ($scope.chequedate == "1") {
                $scope.chequedate = 1;
            }
            else {
                $scope.chequedate = 0;
            }
        };

        //$scope.acdmyr_check = function () {
        //    if ($scope.acdmyr == "1") {
        //        $scope.acdmyr = 1;
        //    }
        //    else {
        //        $scope.acdmyr = 0;
        //    }
        //};

        $scope.acdmyr_check = function () {
            $scope.headwise = false;
        };
        $scope.cheque_date = function () {
            $scope.headwise = false;
        };
        $scope.headwise_check = function () {
            $scope.acdmyr = false;
            $scope.chequedate = false;
        };


        $scope.fromdatechange = function () {
            $scope.binddatagrp($scope.arrlistchkgroup);
        }
        $scope.todatechange = function () {
            $scope.binddatagrp($scope.arrlistchkgroup);
        }
        //getting the fmg selected values
        $scope.binddatagrp = function (arrlistchkgroup) {

            $scope.arrlistchkhead = [];
            $scope.Grid_View = false;
            $scope.albumNameArray = [];
            angular.forEach($scope.arrlistchkgroup, function (role) {
                if (!!role.selected) $scope.albumNameArray.push(role);

            })
            $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
            $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();


            var data = {

                TempararyArrayList: $scope.albumNameArray,
                // Group_All: $scope.group_check,
                "ASMAY_Id": $scope.ASMAY,
                "Fromdate": $scope.from_date,
                "Todate": $scope.to_date
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("DailyFeeCollReport/getgroupmappedheads", data).
                then(function (promise) {
                    if (promise.alldata != null) {

                        // $scope.grigview1 = true;
                        $scope.arrlistchkhead = promise.alldata;

                        console.log($scope.arrlistchkhead);
                        $scope.grand_total();
                        if ($scope.temp_array_total.length > 0) {
                            $scope.total_row = true;
                        }
                        else {
                            $scope.total_row = false;
                        }
                        // $scope.temp_headlist = promise.alldata;
                    }
                    //if ($scope.group_check == "1" && (promise.alldata == null || promise.alldata == "") && $scope.albumNameArray != null && $scope.albumNameArray != "") {
                    //    // swal("No Head Is Mapped to This Group");
                    //}


                    //MB for special
                    $scope.specialfeeheads = promise.studentlist;
                    $scope.specialheadsdetails = promise.allgroupheaddata;

                    var temp_special_headers = [];
                    var remove_list = [];
                    // var headtot = 0;
                    angular.forEach($scope.specialfeeheads, function (op1) {
                        var spe_hd_list = [];
                        angular.forEach($scope.specialheadsdetails, function (op2) {
                            if (op1.fmsfH_Id == op2.fmsfH_Id) {
                                angular.forEach($scope.arrlistchkhead, function (op_m) {
                                    if (op_m.fmH_Id == op2.fmH_Id) {
                                        spe_hd_list.push(op_m);
                                        //   headtot += headtot + op2.ftP_Paid_Amt;
                                        remove_list.push(op_m);
                                    }
                                })
                            }

                        })
                        if (spe_hd_list.length > 0) {
                            temp_special_headers.push({ sp_id: op1.fmsfH_Id, sp_name: op1.fmsfH_Name, spe_hd_list: spe_hd_list, fmH_FeeName: op1.fmsfH_Name });
                        }
                    })
                    var final_headers = [];
                    angular.forEach($scope.arrlistchkhead, function (header) {
                        var not_cnt = 0;
                        angular.forEach(remove_list, function (li) {
                            if (li.fmH_Id == header.fmH_Id) {
                                not_cnt += 1;
                            }
                        })
                        if (not_cnt == 0) {
                            header.special_flag = false;
                            final_headers.push(header);
                        }
                    })
                    angular.forEach(temp_special_headers, function (sp_hd) {
                        sp_hd.special_flag = true;
                        final_headers.push(sp_hd);
                    })
                    console.log(final_headers);
                    $scope.arrlistchkhead = final_headers;
                    //MB

                })
        }

        //for dynamic gridview heads and group heads

        $scope.addcolumn = function (role) {
            $scope.albumnamearraycolumn = [];

            $scope.albumnamearraycolumn.push(role);

            var newcol = { id: role.fmH_FeeName, checked: true, value: role.fmH_Id }

            $scope.columnsTest.push(newcol)


            $scope.albumNameArrayhead = [];
            angular.forEach($scope.arrlistchkhead, function (role) {
                if (!!role.selected) $scope.albumNameArrayhead.push(role);

            })

            var data = {

                TempararyArrayheadList: $scope.albumNameArrayhead,

            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("DailyFeeCollReport/getgroupheadsid", data).
                then(function (promise) {

                })

        };

        //when checked all conditions
        $scope.chckedIndexs = [];
        $scope.checkAll = function (role) {

            if ($scope.checked) {

                $scope.checked = true;
                angular.forEach($scope.arrlistchkhead, function (privilege) {
                    privilege.Selected = $scope.checked;

                    if ($scope.chckedIndexs.indexOf(privilege) === -1) {
                        if (privilege.Selected) $scope.chckedIndexs.push(privilege);
                    }

                });

                console.log($scope.chckedIndexs);
                var data = {

                    TempararyArrayheadList: $scope.chckedIndexs,

                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("DailyFeeCollReport/getgroupheadsid", data).
                    then(function (promise) {

                    })
            } else {
                $scope.checked = false;
            }

        }






        //column total
        $scope.getTotalc = function (int) {
            var totalc = 0;
            angular.forEach($scope.feedaily, function (e) {
                totalc += e.C;

            });
            return totalc;
        };


        $scope.total_receipts = function (e) {
            var totalc = 0;
            angular.forEach($scope.all_list, function (e) {
                totalc += e.Receipts_Count;

            });
            return totalc;

        };

        $scope.total_Bybank = function (e) {
            var totalc = 0;
            angular.forEach($scope.all_list, function (e) {
                totalc += e.ByBank;

            });
            return totalc;

        };

        $scope.total_Bycash = function (e) {
            var totalc = 0;
            angular.forEach($scope.all_list, function (e) {
                totalc += e.ByCash;

            });
            return totalc;

        };

        $scope.total_Byonline = function (e) {
            var totalc = 0;
            angular.forEach($scope.all_list, function (e) {
                totalc += e.ByOnline;

            });
            return totalc;

        };

        $scope.total_Bycard = function (e) {
            var totalc = 0;
            angular.forEach($scope.all_list, function (e) {
                totalc += e.ByCard;

            });
            return totalc;

        };
        $scope.total_ByRTGS = function (e) {
            var totalc = 0;
            angular.forEach($scope.all_list, function (e) {
                totalc += e.ByRTGS;

            });
            return totalc;

        };
        $scope.total_ByECS = function (e) {
            var totalc = 0;
            angular.forEach($scope.all_list, function (e) {
                totalc += e.ByECS;

            });
            return totalc;

        };


        $scope.total_Total = function (e) {
            var totalc = 0;
            angular.forEach($scope.all_list, function (e) {
                totalc += e.Total;

            });
            return totalc;

        };



        $scope.grand_total = function () {
            $scope.temp_array_total = [];

            //$scope.iddd = $scope.arrlistchk[i].asmcL_ID;
            for (var j = 0; j < $scope.arrlistchkhead.length; j++) {

                var total_X = 0;
                angular.forEach($scope.student_list, function (e) {
                    // if(e[$scope.arrlistchkhead[j].fmH_FeeName]=$scope.arrlistchkhead[j].fmH_FeeName)
                    total_X += e[$scope.arrlistchkhead[j].fmH_FeeName];

                });
                var newcol_tot = { id: $scope.arrlistchkhead[j].fmH_Id, total: total_X, value: $scope.arrlistchkhead[j].fmH_FeeName }
                $scope.temp_array_total.push(newcol_tot);
            }

            var tottemprow = 0;
            for (var i = 0; i < $scope.temp_array_total.length; i++) {
                tottemprow = $scope.temp_array_total[i].total + tottemprow;
            }
            $scope.totrow = tottemprow;
        }





        app.filter('sumFilter', function () {
            return function (heads) {
                var taxTotals = 0;
                for (i = 0; i < heads.length; i++) {
                    taxTotal = taxTotal + heads[i].total;
                };
                return taxTotals;
            };
        });






        $scope.getTotalb = function (int) {
            var totalb = 0;
            angular.forEach($scope.student_list, function (e) {

                totalb += e['Computer Fee'];

                //  totalb += e.TutionFee;

            });
            return totalb;
        };

        $scope.table_all = false;
        $scope.submitted = false;
        //for report button click
        $scope.ShowReport = function () {
            $scope.prsary = [];
            //  $scope.kengrdtotary = [];
            $scope.colarrayaggre = [];
            $scope.kengrdtotary = [];
            $scope.colarray = [];

            if ($scope.Fmc_allorsudentorother == 'thirdparty') {
                $scope.colarray = [{

                    title: "SLNO",
                    template: "<span class='row-numberind'></span>"

                }, { name: 'Name', field: 'Name', width: '100px', title: 'Student Name' },

                { name: 'FYP_Receipt_No', field: 'FYP_Receipt_No', width: '100px', title: 'ReceiptNo' },
                { name: 'FYP_Bank_Name', field: 'FYP_Bank_Name', width: '100px', title: 'Bank' },
                { name: 'Date', field: 'Date', title: 'Date' },
                { name: 'FYP_Bank_Or_Cash', field: 'FYP_Bank_Or_Cash', width: '100px', title: 'Mode Of Payment' },
                { name: 'Chequedate', field: 'Chequedate', width: '100px', title: 'Cheque Date' },
                { name: 'FYP_DD_Cheque_No', field: 'FYP_DD_Cheque_No', width: '100px', title: 'Cheque/DD Details' },
                { name: 'fyp_transaction_id', field: 'fyp_transaction_id', width: '100px', title: 'Transaction Id' },
                { name: 'FYP_PaymentReference_Id', field: 'FYP_PaymentReference_Id', width: '100px', title: 'Reference Id' },
                { name: 'FYP_Remarks', field: 'FYP_Remarks', width: '100px', title: 'Remarks' },
                { name: 'AccName', field: 'AccName', width: '100px', title: 'AccName' },
                { name: 'GateWay', field: 'FYP_PayGatewayType', width: '100px', title: 'FYP_PayGatewayType' },
                { name: 'SettlementDate', field: 'Settlement_Date', width: '100px', title: 'SettlementDate' }

                ];
            } else {
                $scope.colarray = [{

                    title: "SLNO",
                    template: "<span class='row-numberind'></span>"

                }, { name: 'Name', field: 'Name', title: 'Student Name', width: "200px" }, { name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Adm No', width: "200px" },
                { name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class', width: "200px" },
                { name: 'ASMC_SectionName', field: 'ASMC_SectionName', title: 'Section', width: "200px" },
                { name: 'MobileNo', field: 'MobileNo', title: 'MobileNo', width: "200px" },
                { name: 'EmailId', field: 'EmailId', title: 'EmailId', width: "200px" },
                { name: 'Gender', field: 'Gender', title: 'Gender', width: "200px" },
                { name: 'AMST_DOB', field: 'AMST_DOB', title: 'DOB', width: "200px" },
                { name: 'TermName', field: 'TermName', title: 'TermName', width: "200px" },
                { name: 'FYP_Receipt_No', field: 'FYP_Receipt_No', title: 'ReceiptNo', width: "200px" },
                { name: 'FYP_Bank_Name', field: 'FYP_Bank_Name', title: 'Bank', width: "200px" },
                { name: 'Date', field: 'Date', title: 'Date', width: "200px" },
                { name: 'FYP_Bank_Or_Cash', field: 'FYP_Bank_Or_Cash', title: 'Mode Of Payment', width: "200px" },
                { name: 'Chequedate', field: 'Chequedate', title: 'Cheque Date', width: "200px" },
                { name: 'FYP_DD_Cheque_No', field: 'FYP_DD_Cheque_No', title: 'Cheque/DD Details', width: "200px" },
                { name: 'fyp_transaction_id', field: 'fyp_transaction_id', title: 'Transaction Id', width: "200px" },
                { name: 'FYP_PaymentReference_Id', field: 'FYP_PaymentReference_Id', title: 'Reference Id', width: "200px" },
                { name: 'FYP_Remarks', field: 'FYP_Remarks', title: 'Remarks', width: "200px" },
                { name: 'AccName', field: 'AccName', title: 'AccName', width: "200px" },
                { name: 'GateWay', field: 'FYP_PayGatewayType', title: 'FYP_PayGatewayType', width: "200px" },
                { name: 'SettlementDate', field: 'Settlement_Date', title: 'SettlementDate', width: "200px" }

                ];
            }

            $scope.gridOptions.data = [];
            $scope.gridOptionsall.data = [];
            if ($scope.myForm.$valid) {
                //if ($scope.rpttyp == "All")
                //{



                $scope.albumNameArraycolumn = [];
                angular.forEach($scope.arrlistchkgroup, function (role) {
                    if (!!role.selected) $scope.albumNameArraycolumn.push({
                        columnID: role.fmG_Id,
                        FMG_Id: role.fmG_Id,
                        columnName: role.fmG_GroupName
                    });
                })
                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();

                var data = {
                    "regornamedetails": $scope.report,
                    "AMAY_Id": $scope.ASMAY,
                    "Fromdate": $scope.from_date,
                    "Todate": $scope.to_date,
                    "allorindivflag": $scope.rpttyp,
                    "classflag": $scope.usercheck,
                    "groupflag": $scope.acdmyr,
                    "All_List": $scope.albumNameArraycolumn,
                    //  "TempararyArraygroupids": $scope.albumNameArraygroupids,                
                    "allorstdorothersflag": $scope.Fmc_allorsudentorother,
                    "allorcorchoronlineflag": $scope.Fmc_allorcorddorop,
                    "classid": $scope.ASMCL,
                    "cheque": $scope.chequedate,
                    "TempararyArrayListstring": $scope.reportdate,
                    "headwise": $scope.headwise,
                    "paymentwise": $scope.paymentwise

                }
                //$scope.FMCB_fromDATE = new Date(data.Fromdate, $scope.FMCB_fromDATE);

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("DailyFeeCollReport/Getreportdetails", data).
                    then(function (promise) {

                        if ($scope.headwise == true && $scope.paymentwise == false) {
                            if (promise.headwisecollection != null || promise.headwisecollection > 0) {
                                $scope.headwise1 = true;
                                $scope.Grid_View = true;
                                $scope.header_list = [];
                                $scope.headwisecollection = promise.headwisecollection;
                                angular.forEach($scope.headwisecollection, function (ff) {
                                    ff.Date = ff.Date == null ? "" : $filter('date')(ff.Date, "dd/MM/yyyy");

                                })
                                if ($scope.headwisecollection.length > 0) {
                                    for (var i = 0; i < $scope.headwisecollection.length; i++) {

                                        if (i === 0) {
                                            angular.forEach($scope.headwisecollection[i], function (key, r) {
                                                $scope.header_list.push({ colmn: r, head: key });
                                            });
                                        }
                                    }
                                }

                                //$scope.header_list.push({ colmn: 'Total_Balance', head: 0 });

                                //for (var i = 0; i < $scope.headwisecollection.length; i++) {
                                //    var cnnt1 = 0;
                                //    angular.forEach($scope.headwisecollection[i], function (key, r) {

                                //        angular.forEach($scope.header_list, function (rr) {

                                //            if (r == rr.colmn && rr.colmn != 'FYP_date') {
                                //                cnnt1 += parseInt(key);
                                //            }



                                //        });

                                //    });
                                //    $scope.headwisecollection[i].Total_Balance = cnnt1;
                                //}



                                $scope.header_list1 = [];
                                angular.forEach($scope.header_list, function (rr) {
                                    var cnnt = 0;
                                    for (var i = 0; i < $scope.headwisecollection.length; i++) {

                                        angular.forEach($scope.headwisecollection[i], function (key, r) {
                                            if (r == rr.colmn && rr.colmn != 'Date') {
                                                cnnt += parseInt(key);
                                            }



                                        });

                                    }

                                    $scope.header_list1.push({ head: rr.colmn, cntt: cnnt });

                                })
                                var abc = 0;
                                angular.forEach($scope.header_list1, function (ee) {
                                    abc += 1;
                                    ee.fld = 'id' + abc;


                                })
                                angular.forEach($scope.headwisecollection, function (key, ee) {

                                    angular.forEach(key, function (gg, ee) {

                                        angular.forEach($scope.header_list1, function (dd) {

                                            if (dd.head == ee) {

                                                if (dd.head != 'Date') {


                                                    key[dd.fld] = parseInt(gg);
                                                }
                                                else {
                                                    key[dd.fld] = gg;
                                                }
                                            }



                                        })



                                    })

                                })
                                $scope.colarrayall = [];
                                $scope.tempaggary = [];

                                $scope.colarrayall.push({
                                    title: "SLNO",
                                    template: "<span class='row-number' ></span>", width: "80px"
                                }
                                )

                                angular.forEach($scope.header_list1, function (ww) {
                                    if (ww.head != 'Date') {
                                        $scope.tempaggary.push({ field: ww.fld, name: ww.fld, aggregate: "sum" });
                                        $scope.colarrayall.push({
                                            title: ww.head, field: ww.fld, width: 200, aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        });



                                    }
                                    else if (ww.head == 'Date') {

                                        $scope.colarrayall.push({ title: ww.head, field: ww.fld, width: 200 });
                                    }

                                })

                                console.log($scope.colarrayall);
                                console.log($scope.headwisecollection);

                                $scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + " " + "," + " " + "USERNAME :" + " " + $scope.usrname + ",";
                                $scope.aaaa = [{
                                    title: $scope.txtdata,
                                    columns: $scope.colarrayall
                                }]

                                $(document).ready(function () {
                                    $('#gridheadwise').empty();
                                    $("#gridheadwise").kendoGrid({
                                        toolbar: ["excel"],

                                        dataSource: {
                                            data: $scope.headwisecollection,
                                            pageSize: 100,
                                            aggregate: $scope.tempaggary
                                        },
                                        excel: {
                                            fileName: "HeadWiseCollection.xls",
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                        },
                                        pdf: {
                                            avoidLinks: true,
                                            landscape: true,
                                            repeatHeaders: true,
                                            fileName: "HeadWiseCollection.pdf",
                                            allPages: true
                                        },



                                        sortable: true,
                                        // pageable: true,
                                        pageable: true,
                                        groupable: false,
                                        filterable: true,
                                        columnMenu: true,
                                        reorderable: true,
                                        resizable: true,
                                        selectable: true,
                                        //change: onChange,
                                        columns: $scope.colarrayall,
                                        dataBound: function (e) {
                                            var pagenum = this.dataSource.page();
                                            var pageitms = this.dataSource.pageSize();
                                            var rows = this.items();
                                            $(rows).each(function () {
                                                var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                var rowLabel = $(this).find(".row-number");
                                                $(rowLabel).html(index);
                                            });


                                        }
                                    });
                                });
                            }
                            else {
                                swal("No Record Found");

                                $scope.Grid_View = false;
                            }
                        }
                        else if ($scope.paymentwise == true) {
                            if (promise.headwisecollection != null || promise.headwisecollection > 0) {
                                $scope.headwise1 = true;
                                $scope.Grid_View = true;
                                $scope.header_list = [];
                                $scope.headwisecollection = promise.headwisecollection;
                                angular.forEach($scope.headwisecollection, function (ff) {
                                    ff.Date = ff.Date == null ? "" : $filter('date')(ff.Date, "dd/MM/yyyy");

                                })
                                if ($scope.headwisecollection.length > 0) {
                                    for (var i = 0; i < $scope.headwisecollection.length; i++) {

                                        if (i === 0) {
                                            angular.forEach($scope.headwisecollection[i], function (key, r) {
                                                $scope.header_list.push({ colmn: r, head: key });
                                            });
                                        }
                                    }
                                }


                                $scope.header_list1 = [];
                                angular.forEach($scope.header_list, function (rr) {
                                    var cnnt = 0;
                                    for (var i = 0; i < $scope.headwisecollection.length; i++) {

                                        angular.forEach($scope.headwisecollection[i], function (key, r) {
                                            if (r == rr.colmn && rr.colmn != 'Date' && rr.colmn != 'HeadName') {
                                                cnnt += parseInt(key);
                                            }



                                        });

                                    }

                                    $scope.header_list1.push({ head: rr.colmn, cntt: cnnt });

                                })
                                var abc = 0;
                                angular.forEach($scope.header_list1, function (ee) {
                                    abc += 1;
                                    ee.fld = 'id' + abc;


                                })
                                angular.forEach($scope.headwisecollection, function (key, ee) {

                                    angular.forEach(key, function (gg, ee) {

                                        angular.forEach($scope.header_list1, function (dd) {

                                            if (dd.head == ee) {

                                                if (dd.head != 'Date' && dd.head != 'HeadName') {


                                                    key[dd.fld] = parseInt(gg);
                                                }
                                                else {
                                                    key[dd.fld] = gg;
                                                }
                                            }



                                        })



                                    })

                                })
                                $scope.colarrayall = [];
                                $scope.tempaggary = [];

                                $scope.colarrayall.push({
                                    title: "SLNO",
                                    template: "<span class='row-number' ></span>", width: "80px"
                                },


                                )

                                angular.forEach($scope.header_list1, function (ww) {
                                    if (ww.head != 'Date' && ww.head != 'HeadName') {
                                        $scope.tempaggary.push({ field: ww.fld, name: ww.fld, aggregate: "sum" });
                                        $scope.colarrayall.push({
                                            title: ww.head, field: ww.fld, width: 200, aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        });



                                    }
                                    else if (ww.head == 'Date' || ww.head == 'HeadName') {

                                        $scope.colarrayall.push({ title: ww.head, field: ww.fld, width: 200 });
                                    }

                                })

                                console.log($scope.colarrayall);
                                console.log($scope.headwisecollection);

                                $scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + " " + "," + " " + "USERNAME :" + " " + $scope.usrname + ",";
                                $scope.aaaa = [{
                                    title: $scope.txtdata,
                                    columns: $scope.colarrayall
                                }]

                                $(document).ready(function () {
                                    $('#gridheadwise').empty();
                                    $("#gridheadwise").kendoGrid({
                                        toolbar: ["excel"],

                                        dataSource: {
                                            data: $scope.headwisecollection,
                                            pageSize: 100,
                                            aggregate: $scope.tempaggary
                                        },
                                        excel: {
                                            fileName: "PaymentModeCollection.xls",
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                        },



                                        sortable: true,
                                        // pageable: true,
                                        pageable: true,
                                        groupable: false,
                                        filterable: true,
                                        columnMenu: true,
                                        reorderable: true,
                                        resizable: true,
                                        selectable: true,
                                        //change: onChange,
                                        columns: $scope.colarrayall,
                                        dataBound: function (e) {
                                            var pagenum = this.dataSource.page();
                                            var pageitms = this.dataSource.pageSize();
                                            var rows = this.items();
                                            $(rows).each(function () {
                                                var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                var rowLabel = $(this).find(".row-number");
                                                $(rowLabel).html(index);
                                            });


                                        }
                                    });
                                });
                            }
                            else {
                                swal("No Record Found");
                                // $scope.table_all = false;
                                $scope.Grid_View = false;
                            }
                        }
                        else {
                            if (promise.alllist != null && promise.alllist != "") {
                                $scope.Grid_View = true;
                                if ($scope.rpttyp == "All") {
                                    $scope.all_list = promise.alllist;
                                    $scope.totreceipts = $scope.total_receipts(promise.alllist);
                                    $scope.totBybank = $scope.total_Bybank(promise.alllist);
                                    $scope.totBycash = $scope.total_Bycash(promise.alllist);
                                    $scope.totByonline = $scope.total_Byonline(promise.alllist);
                                    $scope.totBycard = $scope.total_Bycard(promise.alllist);
                                    $scope.totByECS = $scope.total_ByECS(promise.alllist);
                                    $scope.totByRTGS = $scope.total_ByRTGS(promise.alllist);
                                    $scope.totTotal = $scope.total_Total(promise.alllist);

                                    $scope.gridOptionsall.data = $scope.all_list;
                                    //$scope.gridOptionsall.data.push({
                                    //    Date: "Total", Receipts_Count: $scope.totreceipts, ByBank: $scope.totBybank,
                                    //    ByCash: $scope.totBycash, ByOnline: $scope.totByonline, ByCard: $scope.totBycard,
                                    //    ByECS: $scope.totByECS, ByRTGS: $scope.totByRTGS, Total: $scope.totTotal
                                    //});
                                    //$scope.table_all = true;
                                    //$scope.table = false;
                                    angular.forEach($scope.colarrayall, function (widobj) {
                                        widobj.width = 250;
                                    })

                                    $scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname + "," + " " + $scope.coptyright;
                                    console.log($scope.txtdata);
                                    $scope.aaaa = [{
                                        title: $scope.txtdata,
                                        columns: $scope.colarrayall
                                    }]

                                    console.log($scope.all_list);
                                    var gridall;

                                    $(document).ready(function () {
                                        initGridall();
                                    });
                                    function initGridall() {
                                        $('#grid').empty();
                                        gridall = $("#grid").kendoGrid({
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
                                                data: $scope.all_list,
                                                pageSize: 10,
                                                aggregate: [
                                                    { name: 'Receipts_Count', field: 'Receipts_Count', aggregate: "sum" },
                                                    { name: 'ByBank', field: 'ByBank', aggregate: "sum" },
                                                    { name: 'ByCash', field: 'ByCash', aggregate: "sum" },
                                                    { name: 'ByOnline', field: 'ByOnline', aggregate: "sum" },
                                                    { name: 'ByCard', field: 'ByCard', aggregate: "sum" },
                                                    { name: 'ByECS', field: 'ByECS', aggregate: "sum" },
                                                    { name: 'ByRTGS', field: 'ByRTGS', aggregate: "sum" },
                                                    { name: 'Total', field: 'Total', aggregate: "sum" }
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
                                            columns: $scope.aaaa,
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
                                if ($scope.rpttyp == "Individual" && $scope.Fmc_allorsudentorother != "multiplepaymentmode") {

                                    $scope.temporary_list = promise.alllist;
                                    $scope.student_list = promise.alllist;
                                    //MB for special
                                    // $scope.specialfeeheads = promise.studentlist;
                                    // $scope.specialheadsdetails = promise.allgroupheaddata;

                                    // var temp_special_headers = [];
                                    // var remove_list = [];
                                    //// var headtot = 0;
                                    // angular.forEach($scope.specialfeeheads, function (op1) {
                                    //     var spe_hd_list = [];
                                    //     angular.forEach($scope.specialheadsdetails, function (op2) {
                                    //         if (op1.fmsfH_Id == op2.fmsfH_Id) {
                                    //             angular.forEach($scope.arrlistchkhead, function (op_m) {
                                    //                 if (op_m.fmH_Id == op2.fmH_Id) {
                                    //                     spe_hd_list.push(op_m);
                                    //                  //   headtot += headtot + op2.ftP_Paid_Amt;
                                    //                    remove_list.push(op_m);
                                    //                 }
                                    //             })
                                    //         }

                                    //     })
                                    //     if (spe_hd_list.length > 0) {
                                    //         temp_special_headers.push({ sp_id: op1.fmsfH_Id, sp_name: op1.fmsfH_Name, spe_hd_list: spe_hd_list,fmH_FeeName: op1.fmsfH_Name });
                                    //     }
                                    // })
                                    // var final_headers = [];
                                    // angular.forEach($scope.arrlistchkhead, function (header) {
                                    //     var not_cnt = 0;
                                    //     angular.forEach(remove_list, function (li) {
                                    //         if(li.fmH_Id==header.fmH_Id)
                                    //         {
                                    //             not_cnt+=1;
                                    //         }
                                    //     })
                                    //     if(not_cnt==0)
                                    //     {
                                    //         header.special_flag = false;
                                    //         final_headers.push(header);
                                    //     }                           
                                    // })
                                    // angular.forEach(temp_special_headers, function (sp_hd) {
                                    //     sp_hd.special_flag = true;
                                    //     final_headers.push(sp_hd);
                                    // })
                                    // console.log(final_headers);
                                    angular.forEach($scope.student_list, function (stud) {
                                        angular.forEach($scope.arrlistchkhead, function (sp_hd) {
                                            if (sp_hd.special_flag) {
                                                var special_head_amoumt = 0;
                                                angular.forEach(sp_hd.spe_hd_list, function (hds) {
                                                    special_head_amoumt += stud[hds.fmH_FeeName];
                                                })
                                                //  stud[sp_hd.fmH_FeeName] = parseInt(special_head_amoumt);
                                                stud[sp_hd.fmH_FeeName] = parseInt(special_head_amoumt);
                                            }
                                        })
                                    })
                                    console.log($scope.student_list);
                                    //$scope.arrlistchkhead = final_headers;
                                    //MB




                                    $scope.table_all = false;
                                    $scope.table = true;
                                    $scope.testary = [];
                                    $scope.temp_array_total = [];



                                    // total for each row


                                    $scope.temp_array = [];

                                    angular.forEach($scope.arrlistchkhead, function (objj) {

                                        //var string = objj.fmH_FeeName;
                                        //var strstr = string.replace(/[\s]/g, '');
                                        //var string = objj.fmH_FeeName;
                                        //objj.fmH_FeeName = string.replace(/[\s]/g, '');  '["foo bar"]'
                                        var strstr = '["' + objj.fmH_FeeName + '"]';

                                        //var string = '[' + strstr + ']';
                                        $scope.colarrayaggre.push({ field: strstr, name: objj.fmH_FeeName, title: objj.fmH_FeeName, aggregate: "sum" });
                                        $scope.colarray.push({
                                            field: strstr, name: objj.fmH_FeeName, title: objj.fmH_FeeName, aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        });

                                        $scope.prsary.push({
                                            field: strstr, name: objj.fmH_FeeName, title: objj.fmH_FeeName, aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        });
                                    })
                                    $scope.all_grand_total = 0;
                                    for (var z = 0; z < $scope.temp_array_total.length; z++) {
                                        $scope.all_grand_total += $scope.temp_array_total[z].total;
                                    }
                                    $scope.colarray.push({
                                        name: 'total_side1', field: 'total_side1', title: 'Total', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    });

                                    $scope.colarrayaggre.push({ name: 'total_side1', field: 'total_side1', title: 'Total', aggregate: "sum" });
                                    angular.forEach($scope.colarray, function (widobj) {
                                        widobj.width = 100;
                                    })
                                    //angular.forEach($scope.student_list, function (index) {
                                    //    angular.forEach(index, function (v,k) {
                                    //        //delete index[k];
                                    //      k = k.replace(/[\s]/g, '');
                                    //    });
                                    //});

                                    for (var x = 0; x < $scope.student_list.length; x++) {
                                        var total_y = 0;
                                        for (var y = 0; y < $scope.arrlistchkhead.length; y++) {
                                            var column = $scope.arrlistchkhead[y].fmH_FeeName;
                                            total_y += Number($scope.student_list[x][column]);
                                            $scope.student_list[x].total_side = total_y;
                                        }
                                        // $scope.temp_array.push({ total_side: total_y, date: $scope.student_list[x].Date, array: $scope.student_list[x] });
                                        $scope.temp_array.push({ date: $scope.student_list[x].Date, array: $scope.student_list[x] });
                                        $scope.gridOptions.data.push($scope.student_list[x]);
                                        //   $scope.gridOptions.data.push({ total_side: total_y });
                                    }
                                    $scope.kengrdtotary = $scope.gridOptions.data;

                                    var obj = [];
                                    var indi_totals = [];
                                    for (var j = 0; j < $scope.arrlistchkhead.length; j++) {

                                        var total_X = 0;
                                        angular.forEach($scope.student_list, function (e) {
                                            if (e[$scope.arrlistchkhead[j].fmH_FeeName] == null) {//added for checking null values
                                                e[$scope.arrlistchkhead[j].fmH_FeeName] = 0;
                                            }
                                            total_X += e[$scope.arrlistchkhead[j].fmH_FeeName];

                                        });
                                        //obj[$scope.arrlistchkhead[j].fmH_FeeName] = total_X; //Use Bracket notation
                                        //$scope.temp_array_total.push(obj);

                                        var key = $scope.arrlistchkhead[j].fmH_FeeName;

                                        obj = {
                                            [key]: total_X
                                        };
                                        indi_totals.push(obj);
                                        console.log(obj);

                                    }
                                    $scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname + "," + " " + $scope.coptyright;
                                    console.log($scope.txtdata);
                                    $scope.aaaa1 = [{
                                        title: $scope.txtdata,
                                        columns: $scope.colarray
                                    }]
                                    console.log($scope.colarray);
                                    console.log($scope.kengrdtotary);
                                    var gridind;

                                    $(document).ready(function () {
                                        initGridind();

                                    });

                                    function initGridind() {
                                        $('#gridind').empty();
                                        gridind = $("#gridind").kendoGrid({
                                            toolbar: ["excel", "pdf"],
                                            //pdf: {
                                            //    fileName: "inddExport.pdf"
                                            //    //allPages: true
                                            //},
                                            excel: {
                                                fileName: "DailyCollection.xlsx",
                                                //allPages: true,
                                                filterable: true,
                                                allPages: true
                                            },
                                            pdf: {
                                                fileName: "DailyCollection.pdf",
                                                allPages: true,
                                                filterable: true
                                            },
                                            dataSource: {

                                                data: $scope.kengrdtotary,
                                                aggregate: $scope.colarrayaggre,
                                                pageSize: 100,
                                                schema: {

                                                    parse: function (d) {

                                                        $.each(d, function (idx, elem) {
                                                            var z = 0;
                                                            for (var j = 0; j < $scope.prsary.length; j++) {
                                                                //elem[$scope.prsary[j].name] = isNaN(elem[$scope.prsary[j].name]) ? 0 : elem[$scope.prsary[j].name];
                                                                z += elem[$scope.prsary[j].name];
                                                            }
                                                            elem.total_side1 = z;
                                                        });
                                                        return d;
                                                    }
                                                }
                                            },

                                            sortable: true,
                                            pageable: true,
                                            groupable: false,
                                            filterable: true,
                                            columnMenu: true,
                                            reorderable: true,
                                            resizable: true,
                                            columns: $scope.aaaa1,
                                            dataBound: function () {
                                                var pagenum = this.dataSource.page();
                                                var pageitms = this.dataSource.pageSize()
                                                var rows = this.items();
                                                $(rows).each(function () {
                                                    var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                    var rowLabel = $(this).find(".row-numberind");
                                                    $(rowLabel).html(index);
                                                });
                                            }

                                        }).data("kendoGrid");
                                    }


                                }

                                else if ($scope.rpttyp == "Individual" && $scope.Fmc_allorsudentorother == "multiplepaymentmode") {

                                    $scope.student_list = promise.alllist;

                                    $scope.termflg = false;
                                    $scope.stdtermflg = false;

                                    if (promise.alllist != null && promise.alllist != "") {
                                        $scope.std = true;
                                        $scope.colarrayall = [{

                                            title: "SLNO",
                                            template: "<span class='row-number'></span>"

                                        },
                                        {
                                            name: 'Name', field: 'Name', title: 'Student Name'
                                        },
                                        {
                                            name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Admission no'
                                        },
                                        {
                                            name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'ClassName'
                                        },
                                        {
                                            name: 'ASMC_SectionName', field: 'ASMC_SectionName', title: 'Section Name'
                                        },
                                        {
                                            name: 'MobileNo', field: 'MobileNo', title: 'MobileNo'
                                        },
                                        {
                                            name: 'FYP_Receipt_No', field: 'FYP_Receipt_No', title: 'Receipt_No'
                                        },
                                        {
                                            name: 'FYPPM_BankName', field: 'FYPPM_BankName', title: 'Bank Name'
                                        },
                                        {
                                            name: 'FYP_DD_Cheque_No', field: 'FYP_DD_Cheque_No', title: 'DD_Cheque_No'
                                        },
                                        {
                                            name: 'Date', field: 'Date', title: 'Date'
                                        },
                                        {
                                            name: 'Bank', field: 'Bank', title: 'Bank', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        },
                                        {
                                            name: 'Cash', field: 'Cash', title: 'Cash', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        },
                                        {
                                            name: 'Online', field: 'Online', title: 'Online', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        },
                                        {
                                            name: 'Card', field: 'Card', title: 'Card', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        },
                                        {
                                            name: 'RTGS', field: 'RTGS', title: 'RTGS', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        },
                                        {
                                            name: 'ECS', field: 'ECS', title: 'ECS', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        },

                                        {
                                            name: 'Total', field: 'Total', title: 'Total', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                        },

                                        ];
                                        $scope.students = promise.alllist;
                                        $scope.Grid_view = true;

                                        var s_total_Bank = 0;
                                        var s_total_Cash = 0;
                                        var s_total_Online = 0;
                                        var s_total_Card = 0;
                                        var s_total_RTGS = 0;
                                        var s_total_ECS = 0;
                                        var s_total = 0;

                                        angular.forEach($scope.students, function (stu) {
                                            s_total_Bank += stu.Bank;
                                            s_total_Cash += stu.Cash;
                                            s_total_Online += stu.Online;
                                            s_total_Card += stu.Card;
                                            s_total_RTGS += stu.RTGS;
                                            s_total_ECS += stu.ECS;
                                            s_total += stu.Total;
                                        })


                                        $scope.s_total_Bank = s_total_Bank;
                                        $scope.s_total_Cash = s_total_Cash;
                                        $scope.s_total_Online = s_total_Online;
                                        $scope.s_total_Card = s_total_Card;
                                        $scope.s_total_RTGS = s_total_RTGS;
                                        $scope.s_total_ECS = s_total_ECS;
                                        $scope.s_total = s_total;

                                        angular.forEach($scope.students, function (qwe) {
                                            qwe.width = 450;
                                        })

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
                                                        { name: 'Bank', field: 'Bank', aggregate: "sum" },
                                                        { name: 'Cash', field: 'Cash', aggregate: "sum" },
                                                        { name: 'Online', field: 'Online', aggregate: "sum" },
                                                        { name: 'Card', field: 'Card', aggregate: "sum" },
                                                        { name: 'RTGS', field: 'RTGS', aggregate: "sum" },
                                                        { name: 'ECS', field: 'ECS', aggregate: "sum" },
                                                        { name: 'Total', field: 'Total', aggregate: "sum" }
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



                                }
                                else {
                                    swal("No Record Found");
                                    // $scope.table_all = false;
                                    $scope.Grid_View = false;
                                }


                            }
                        }

                    })

            }
            else {
                $scope.submitted = true;
            }
        };

        //$scope.exportToExcel = function () {
        // 
        //    if ($scope.rpttyp == "Individual") {
        //        if ($scope.printdatatable1 !== null && $scope.printdatatable1.length > 0) {
        //            var exportHref = Excel.tableToExcel(table1, 'sheet name');
        //            $timeout(function () { location.href = exportHref; }, 100);
        //        }
        //        else {
        //            swal("Please select records to be Exported");
        //        }

        //    }
        //    else if ($scope.rpttyp == "All") {
        //        if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
        //            var exportHref = Excel.tableToExcel(table2, 'sheet name');
        //            $timeout(function () { location.href = exportHref; }, 100);
        //        }
        //        else {
        //            swal("Please select records to be Exported");
        //        }
        //    }


        //}
        $scope.exportExcel = function () {

            var grid = $scope.gridApi.grid;
            var rowTypes = exportUiGridService.ALL;
            var colTypes = exportUiGridService.ALL;
            exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'selected', 'selected');
        };

        //        $scope.printData = function () {


        //            if ($scope.rpttyp == "Individual") {
        //                if ($scope.kengrdtotary !== null && $scope.kengrdtotary.length > 0) {
        //                    var innerContents = document.getElementById("gridind").innerHTML;
        //                    var popupWinindow = window.open('');
        //                    popupWinindow.document.open();
        //                    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //      '<link type="text/css" media="print" rel="stylesheet" href="css/print/fees/DailyFeeCollectionPdf.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //'</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 3000);">' + innerContents + '</html>');
        //                    popupWinindow.document.close();
        //                }
        //                else {
        //                    swal("Please Select Records to be Printed");
        //                }
        //            }
        //            else if ($scope.rpttyp == "All") {
        //                if ($scope.all_list !== null && $scope.all_list.length > 0) {
        //                    var innerContents = document.getElementById("grid").innerHTML;
        //                    var popupWinindow = window.open('');
        //                    popupWinindow.document.open();
        //                    popupWinindow.document.write('<html><head>' +
        //                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
        //                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/fees/DailyFeeCollectionPdf.css" />' +

        //                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //                    popupWinindow.document.close();
        //                }
        //                else {
        //                    swal("Please Select Records to be Printed");
        //                }
        //            }



        //        }




    }
})();

angular
    .module('app').factory('exportUiGridService', exportUiGridService);

exportUiGridService.inject = ['uiGridExporterService'];
function exportUiGridService(uiGridExporterService) {
    var service = {
        exportToExcel: exportToExcel
    };

    return service;

    function Workbook() {
        if (!(this instanceof Workbook)) return new Workbook();
        this.SheetNames = [];
        this.Sheets = {};
    }

    function exportToExcel(sheetName, gridApi, rowTypes, colTypes) {
        var columns = gridApi.grid.options.showHeader ? uiGridExporterService.getColumnHeaders(gridApi.grid, colTypes) : [];
        var data = uiGridExporterService.getData(gridApi.grid, rowTypes, colTypes);
        var fileName = gridApi.grid.options.exporterExcelFilename ? gridApi.grid.options.exporterExcelFilename : 'dailyfeecolreport';
        fileName += '.xlsx';
        var wb = new Workbook(),
            ws = sheetFromArrayUiGrid(data, columns);
        wb.SheetNames.push(sheetName);
        wb.Sheets[sheetName] = ws;
        var wbout = XLSX.write(wb, {
            bookType: 'xlsx',
            bookSST: true,
            type: 'binary'
        });
        saveAs(new Blob([s2ab(wbout)], {
            type: 'application/octet-stream'
        }), fileName);
    }

    function sheetFromArrayUiGrid(data, columns) {
        var ws = {};
        var range = {
            s: {
                c: 10000000,
                r: 10000000
            },
            e: {
                c: 0,
                r: 0
            }
        };
        var C = 0;
        columns.forEach(function (c) {
            var v = c.displayName || c.value || columns[i].name;
            addCell(range, v, 0, C, ws);
            C++;
        }, this);
        var R = 1;
        data.forEach(function (ds) {
            C = 0;
            ds.forEach(function (d) {
                var v = d.value;
                addCell(range, v, R, C, ws);
                C++;
            });
            R++;
        }, this);
        if (range.s.c < 10000000) ws['!ref'] = XLSX.utils.encode_range(range);
        return ws;
    }
    /**
     * 
     * @param {*} data 
     * @param {*} columns 
     */

    function datenum(v, date1904) {
        if (date1904) v += 1462;
        var epoch = Date.parse(v);
        return (epoch - new Date(Date.UTC(1899, 11, 30))) / (24 * 60 * 60 * 1000);
    }

    function s2ab(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    function addCell(range, value, row, col, ws) {
        if (range.s.r > row) range.s.r = row;
        if (range.s.c > col) range.s.c = col;
        if (range.e.r < row) range.e.r = row;
        if (range.e.c < col) range.e.c = col;
        var cell = {
            v: value
        };
        if (cell.v == null) cell.v = '';
        var cell_ref = XLSX.utils.encode_cell({
            c: col,
            r: row
        });

        if (typeof cell.v === 'number') cell.t = 'n';
        else if (typeof cell.v === 'boolean') cell.t = 'b';
        else if (cell.v instanceof Date) {
            cell.t = 'n';
            cell.z = XLSX.SSF._table[14];
            cell.v = datenum(cell.v);
        } else cell.t = 's';

        ws[cell_ref] = cell;
    }
}
