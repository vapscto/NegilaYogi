(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeYearlyCollectionReportController', CollegeYearlyCollectionReportController)
    CollegeYearlyCollectionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', 'exportUiGridService', 'uiGridConstants']
    function CollegeYearlyCollectionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, exportUiGridService, uiGridConstants) {

        $scope.colarrayaggre = [];
        $scope.usrname = localStorage.getItem('username');

        $scope.FMCB_fromDATE = new Date();
        $scope.FMCB_toDATE = new Date();

        $scope.obj = {};
        $scope.std = false;
       

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            var grouporterm = configsettings[0].fmC_GroupOrTermFlg;
        }

        if (grouporterm == "T") {
            $scope.grouportername = "Term Name"
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name"
        }

        $scope.toggleAllSem = function () {
            var toggleStatus = $scope.semselect;
            angular.forEach($scope.semisterlistnew, function (role) { role.selected = toggleStatus; });

        }

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

        //$scope.onselectyear = function (obj) {

        //    var data = {
        //        "ASMAY_Id": obj.ASMAY,
        //        // "ASMCL_Id": clsobj.asmcL_Id,s
        //    }
        //    apiService.create("CollegeYearlyCollectionReportController/getdata", data).
        //       then(function (promise) {
        //           $scope.arrlistchkgroup = promise.fillfeegroup;
        //           $scope.binddatagrp();
        //       })
        //}

        $scope.onselectyear = function (obj) {

            var data = {
                "ASMAY_Id": obj.ASMAY,
            }

            apiService.create("CollegeYearlyCollectionReport/get_courses", data).
                then(function (promise) {

                    $scope.coursecount = promise.courselist;
                    angular.forEach($scope.coursecount, function (tr) {
                        tr.selectedcourse = true;
                    })

                    $scope.arrlistchkgroup = promise.grouplist;
                    angular.forEach($scope.arrlistchkgroup, function (tr) {
                        tr.selected = true;
                    })
                    //$scope.arrlistchkgroup = promise.grouplist;
                    $scope.binddatagrp();
                    $scope.get_branches();

                })
        };

        $scope.get_branches = function (obj) {

            var AMCO_Ids = [];
            angular.forEach($scope.coursecount, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })
            var data = {
                "ASMAY_Id": $scope.obj.ASMAY,
                AMCO_Ids: AMCO_Ids,

            }

            apiService.create("CollegeYearlyCollectionReport/get_branches", data).
                then(function (promise) {
                    $scope.branchcount = promise.branchlist;
                    angular.forEach($scope.branchcount, function (fy) {
                        fy.selectedbranch = true;
                    })
                    $scope.get_semister();
                })

        };

        $scope.get_semister = function (obj) {

            var AMB_Ids = [];
            angular.forEach($scope.branchcount, function (ty) {
                if (ty.selectedbranch) {
                    AMB_Ids.push(ty.amB_Id);
                }
            })
            var AMCO_Ids = [];
            angular.forEach($scope.coursecount, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })

            var data = {
                "ASMAY_Id": $scope.obj.ASMAY,
                AMB_Ids: AMB_Ids,
                AMCO_Ids: AMCO_Ids

            }

            apiService.create("CollegeYearlyCollectionReport/get_semisters_new", data).
                then(function (promise) {
                    $scope.semisterlistnew = promise.semisterlistnew;

                })

        };



        $scope.Grid_View = false;
       
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

        }
        $scope.optionToggled1 = function (SelectedStudentRecord1, index) {

            $scope.all1 = $scope.temp_array.every(function (itm) { return itm.selected1; });
            if ($scope.printdatatable1.indexOf(SelectedStudentRecord1) === -1) {
                $scope.printdatatable1.push(SelectedStudentRecord1);
            }
            else {
                $scope.printdatatable1.splice($scope.printdatatable1.indexOf(SelectedStudentRecord1), 1);
            }




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

        }


        $scope.checkboxchcked = [];
        $scope.sectioncheckboxchcked = [];
        $scope.rpttyp = "All";
        $scope.Fmc_allorsudentorother = "All";
        $scope.Fmc_allorcorddorop = "All";
        $scope.individual_flag = true;
        $scope.classdt = false;
        $scope.allcc = true;
        $scope.chequedate = 0;
        $scope.chequedt = false;
        //binding the default values  


        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.tabel12 = false;
            $scope.tabel123 = false;

            var data = {
                "configset": grouporterm,
            }

            apiService.create("CollegeYearlyCollectionReport/getalldetails", data).
                then(function (promise) {
                    $scope.yearlst = promise.yearlst;

                    $scope.obj.ASMAY = academicyrlst[0].asmaY_Id;

                    $scope.arrlistchkgroup = promise.grouplist;
                    angular.forEach($scope.arrlistchkgroup, function (tr) {
                        tr.selected = true;
                    })
                    $scope.seclist = promise.fillfeehead;
                    $scope.onclickloaddata();
                    $scope.load_group_check();
                    $scope.cheque_date();
                    $scope.columnsTest = [];

                    $scope.onselectyear($scope.obj);

                    $scope.sort = function (keyname) {
                        $scope.sortKey = keyname;   //set the sortKey to the param passed
                        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
                    }
                })
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.onclickloaddataclass = function () {
            if ($scope.rpttyp == "Individual") {
                $scope.classdt = true;
                $scope.details = true;
                $scope.chequedt = false;
                $scope.Grid_View = false;
            }
            else if ($scope.rpttyp == "All") {
                $scope.classdt = false;
                $scope.details = false;
                $scope.chequedt = false;
                $scope.Grid_View = false;
            }
        };

        $scope.onclickloaddatamakerchecker = function () {
            $scope.Grid_View = false;
        };


        $scope.isOptionsRequired = function () {
            if ($scope.group_check == "1") {

                return !$scope.arrlistchkgroup.some(function (options) {
                    return options.selected;
                });
            }
            else {
                return false;
            }
        }
        $scope.isOptionsRequiredsem = function () {
            if ($scope.group_check == "1") {

                return !$scope.semisterlistnew.some(function (options) {
                    return options.selected;
                });
            }
            else {
                return false;
            }
        }

        $scope.isOptionsRequiredsem = function () {
            if ($scope.group_check == "1") {

                return !$scope.selectedsection.some(function (options) {
                    return options.selectedsection;
                });
            }
            else {
                return false;
            }
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
         
        };
        $scope.group_check = "0";

        $scope.load_group_check = function () {

            if ($scope.group_check == "1") {
                $scope.group_check_flag = false;
                $scope.binddatagrp();
                $scope.temp_array_total = "";
             

            }
            else if ($scope.group_check == "0") {

                $scope.group_check_flag = true;
                $scope.binddatagrp();
                $scope.temp_array_total = "";
               

            }
        }

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


        $scope.binddatagrp = function () {

            $scope.albumNameArray = [];
            angular.forEach($scope.arrlistchkgroup, function (role) {
                if (!!role.selected) $scope.albumNameArray.push(
                    {
                        FMG_Id: role.fmG_Id,
                    });

            })
            var data = {

                TempararyArrayList: $scope.albumNameArray,
      
                "ASMAY_Id": $scope.obj.ASMAY,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeYearlyCollectionReport/getgroupmappedheads", data).
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
                    if ($scope.group_check == "1" && (promise.alldata == null || promise.alldata == "") && $scope.albumNameArray != null && $scope.albumNameArray != "") {
                        // swal("No Head Is Mapped to This Group");
                    }
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

            apiService.create("CollegeYearlyCollectionReport/getgroupheadsid", data).
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
                apiService.create("CollegeYearlyCollectionReport/getgroupheadsid", data).
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

        $scope.total_TotalApprovalPending = function (e) {
            var totalc = 0;
            angular.forEach($scope.all_list, function (e) {
                totalc += e.PendingCount;
            });
            return totalc;
        };

        $scope.total_TotalApproved = function (e) {
            var totalc = 0;
            angular.forEach($scope.all_list, function (e) {
                totalc += e.ApprovedCount;
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

            //  $scope.kengrdtotary = [];
            $scope.colarrayaggre = [];
            $scope.kengrdtotary = [];
            $scope.colarray = [];
            $scope.colarrayall = [];

            //Individual

            $scope.colarray = [{
                title: "SLNO",
                template: "<span class='row-numberind'></span>"
            }, { name: 'Name', field: 'Name', width: '100px', title: 'Student Name' }, { name: 'AMCST_AdmNo', field: 'AMCST_AdmNo', title: 'Adm No' },
            { name: 'AMCO_CourseName', field: 'AMCO_CourseName', width: '100px', title: 'Course' },
            { name: 'AMB_BranchName', field: 'AMB_BranchName', width: '100px', title: 'Branch' },
            { name: 'AMSE_SEMName', field: 'AMSE_SEMName', width: '100px', title: 'Semester' },
            { name: 'FYP_ReceiptNo', field: 'FYP_ReceiptNo', width: '100px', title: 'ReceiptNo' },
            { name: 'FYPPM_BankName', field: 'FYPPM_BankName', width: '100px', title: 'Bank' },
            { name: 'Date', field: 'Date', title: 'Date' },
            { name: 'FYP_Bank_Or_Cash', field: 'FYP_Bank_Or_Cash', width: '100px', title: 'Mode Of Payment' },
            { name: 'Chequedate', field: 'Chequedate', width: '100px', title: 'Cheque Date' },
            { name: 'FYPPM_DDChequeNo', field: 'FYPPM_DDChequeNo', width: '100px', title: 'Cheque/DD Details' },
            { name: 'AMCST_FatherName', field: 'AMCST_FatherName', width: '100px', title: 'Father Name' },
            { name: 'AMCST_MobileNo', field: 'AMCST_MobileNo', width: '100px', title: 'Mobile No' },
            { name: 'AMCST_emailId', field: 'AMCST_emailId', width: '100px', title: 'Email-Id' }
            ];
            if ($scope.rpttypmkck == true) {
                $scope.colarray.push({ name: 'Status', field: 'ApprovedFlg', width: '100px', title: 'Status' });
            }
            //Individual

            //All
            $scope.colarrayall = [{

                title: "SLNO",
                template: "<span class='row-number'></span>"

            },

            { name: 'AMSE_SEMName', field: 'AMSE_SEMName', title: 'Semester' },
            { name: 'Date', field: 'Date', title: 'Date' },

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
            }];
            if ($scope.rpttypmkck == true) {
                $scope.colarrayall.push(
                    {
                        name: 'PendingCount', field: 'PendingCount', title: 'PendingCount', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                        groupFooterTemplate: "Sum: #=sum#"
                    },
                    {
                        name: 'ApprovedCount', field: 'ApprovedCount', title: 'ApprovedCount', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                        groupFooterTemplate: "Sum: #=sum#"
                    },
                )
            }

            $scope.colarrayall.push({
                name: 'Total', field: 'Total', title: 'Total', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                groupFooterTemplate: "Sum: #=sum#"
            })

            //All

            $scope.gridOptions.data = [];
            $scope.gridOptionsall.data = [];
            if ($scope.myForm.$valid) {

                $scope.albumNameArraycolumn = [];
                var AMCO_Ids = [];
                var AMB_Ids = [];
                var FMG_Ids = [];
                var AMSE_Ids = [];

                angular.forEach($scope.arrlistchkgroup, function (ty) {
                    if (ty.selected) {
                        FMG_Ids.push(ty.fmG_Id);
                    }
                })
                angular.forEach($scope.coursecount, function (ty) {
                    if (ty.selectedcourse) {
                        AMCO_Ids.push(ty.amcO_Id);
                    }
                })

                angular.forEach($scope.branchcount, function (ty) {
                    if (ty.selectedbranch) {
                        AMB_Ids.push(ty.amB_Id);
                    }
                })

                angular.forEach($scope.semisterlistnew, function (ty) {
                    if (ty.selected) {
                        AMSE_Ids.push(ty.AMSE_Id);
                    }
                })

                $scope.Grid_View = false;
                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();
                var data = {
                    "regornamedetails": $scope.report,
                    "AMAY_Id": $scope.obj.ASMAY,
                 
                    "allorindivflag": $scope.rpttyp,
                    "classflag": $scope.usercheck,
                    "groupflag": $scope.group_check,
                    "All_List": $scope.albumNameArraycolumn,
                    //  "TempararyArraygroupids": $scope.albumNameArraygroupids,                
                    "allorstdorothersflag": $scope.Fmc_allorsudentorother,
                    "allorcorchoronlineflag": $scope.Fmc_allorcorddorop,
                    //"classid": $scope.ASMCL,
                    "ASMAY_Id": $scope.obj.ASMAY,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                    AMCO_Ids: AMCO_Ids,
                    AMSE_Ids: AMSE_Ids,
                    AMB_Ids: AMB_Ids,
                    FMG_Ids: FMG_Ids,
                    "radioval": $scope.result
                }
                //$scope.FMCB_fromDATE = new Date(data.Fromdate, $scope.FMCB_fromDATE);

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeYearlyCollectionReport/Getreportdetails", data).then(function (promise) {

                    if (promise.savedrecord != null && promise.savedrecord !== "") {
                        $scope.Grid_View = true;

                        //$scope.reportdetails = promise.searchstudentDetails;
                        if (promise.radioval == "FGW") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.savedrecord != null && promise.savedrecord != "") {
                                $scope.groups = promise.savedrecord;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                             
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = true;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.clsinsconN = false;

                                var g_total_FSS_PaidAmount = 0;
                                var g_total_balance = 0;
                                var g_total_concession = 0;
                                var g_total_fine = 0;
                                var g_total_rebate = 0;
                                var g_total_waived = 0;
                                var g_total_openingbalnce = 0;
                                angular.forEach($scope.groups, function (gp) {
                                    g_total_FSS_PaidAmount += gp.FCSS_PaidAmount;
                                    g_total_balance += gp.balance;
                                    g_total_concession += gp.concession;
                                    g_total_fine += gp.fine;
                                    g_total_rebate += gp.rebate;
                                    g_total_waived += gp.waived;
                                    g_total_openingbalnce += gp.openingbalnce;
                                })
                                $scope.g_total_FSS_PaidAmount = g_total_FSS_PaidAmount;
                                $scope.g_total_balance = g_total_balance;
                                $scope.g_total_concession = g_total_concession;
                                $scope.g_total_fine = g_total_fine;
                                $scope.g_total_rebate = g_total_rebate;
                                $scope.g_total_waived = g_total_waived;
                                $scope.g_total_openingbalnce = g_total_openingbalnce;
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
                            if (promise.savedrecord != null && promise.savedrecord != "") {
                                $scope.heads = promise.savedrecord;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = true;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.clsinsconN = false;
                                var h_total_FSS_PaidAmount = 0;
                                var h_total_concession = 0;
                                var h_total_balance = 0;
                                var h_total_fine = 0;
                                var h_total_rebate = 0;
                                var h_total_waived = 0;
                                var h_total_openingbalnce = 0;
                                angular.forEach($scope.heads, function (hd) {
                                    h_total_FSS_PaidAmount += hd.FCSS_PaidAmount;
                                    h_total_balance += hd.balance;
                                    h_total_concession += hd.concession;
                                    h_total_fine += hd.fine;
                                    h_total_rebate += hd.rebate;
                                    h_total_waived += hd.waived;
                                    h_total_openingbalnce += hd.openingbalnce;
                                })
                                $scope.h_total_FSS_PaidAmount = h_total_FSS_PaidAmount;
                                $scope.h_total_concession = h_total_concession;
                                $scope.h_total_balance = h_total_balance;
                                $scope.h_total_fine = h_total_fine;
                                $scope.h_total_rebate = h_total_rebate;
                                $scope.h_total_waived = h_total_waived;
                                $scope.h_total_openingbalnce = h_total_openingbalnce;
                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.print_flag = true;
                            }
                        }


                        else if (promise.radioval == "CTC") {
                            $scope.termflg = false;
                            $scope.stdtermflg = false;
                            if (promise.savedrecord != null && promise.savedrecord != "") {
                                $scope.classesinsconN = promise.savedrecord;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = false;
                                $scope.cls = false;
                                $scope.had = false;
                                $scope.grp = false;
                                $scope.catg = false;
                                $scope.clsinscon = false;
                                $scope.clsinsconN = true;

                                $scope.clsallclsconN = false;

                                var c_total_FSS_PaidAmount = 0;
                                var c_total_concession = 0;
                                var c_total_balance = 0;
                                var c_total_chanrges = 0;
                                var c_total_refund = 0;
                                var c_total_waived = 0;
                                var c_total_adjustment = 0;
                                var c_total_openingbalnce = 0;
                                angular.forEach($scope.classesinsconN, function (cls) {
                                    c_total_FSS_PaidAmount += cls.TotalPaid;
                                    c_total_balance += cls.TotalBalance;
                                    c_total_concession += cls.TotalConcession;
                                    c_total_chanrges += cls.TotalCharges;
                                    c_total_refund += cls.TotalRefund;
                                    c_total_waived += cls.TotalWaivedOff;
                                    c_total_adjustment += cls.Adjustment;
                                    c_total_openingbalnce += cls.openingbalnce;

                                })
                                $scope.c_total_FSS_PaidAmount = c_total_FSS_PaidAmount;
                                $scope.c_total_balance = c_total_balance;
                                $scope.c_total_concession = c_total_concession;
                                $scope.c_total_chanrges = c_total_chanrges;
                                $scope.c_total_refund = c_total_refund;
                                $scope.c_total_waived = c_total_waived;
                                $scope.c_total_adjustment = c_total_adjustment;
                                $scope.c_total_openingbalnce = c_total_openingbalnce;
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
                            $scope.std = true;
                            if (promise.savedrecord != null && promise.savedrecord != "") {
                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>"

                                },
                                {
                                    name: 'StudentName', field: 'StudentName', title: 'Student Name'
                                },
                                {
                                    name: 'AMCST_AdmNo', field: 'AMCST_AdmNo', title: 'Admission no'
                                },
                                {
                                    name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class Name'
                                },
                                {
                                    name: 'totalpayable', field: 'totalpayable', title: 'Total Payable', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                    groupFooterTemplate: "Sum: #=sum#"
                                    },
                                    {
                                        name: 'openingbalnce', field: 'openingbalnce', title: 'Opening Balance', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                        groupFooterTemplate: "Sum: #=sum#"
                                    },
                                {
                                    name: 'FCSS_PaidAmount', field: 'FCSS_PaidAmount', title: 'Paid Amount', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
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
                                    }
                                    
                                ];
                                $scope.students = promise.savedrecord;
                                $scope.Grid_view = true;
                                $scope.print_flag = false;
                                $scope.std = true;
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
                                var s_total_payable = 0;
                                var s_total_openingbalnce = 0;
                                angular.forEach($scope.students, function (stu) {
                                    s_total_payable += stu.totalpayable;
                                    s_total_FSS_PaidAmount += stu.FCSS_PaidAmount;
                                    s_total_balance += stu.balance;
                                    s_total_concession += stu.concession;
                                    s_total_fine += stu.fine;
                                    s_total_rebate += stu.rebate;
                                    s_total_waived += stu.waived;
                                    s_total_openingbalnce += stu.openingbalnce;
                                })

                                $scope.s_total_FSS_PayableAmount_p = s_total_payable;

                                $scope.s_total_FSS_PaidAmount = s_total_FSS_PaidAmount;
                                $scope.s_total_concession = s_total_concession;
                                $scope.s_total_balance = s_total_balance;
                                $scope.s_total_fine = s_total_fine;
                                $scope.s_total_rebate = s_total_rebate;
                                $scope.s_total_waived = s_total_waived;
                                $scope.s_total_openingbalnce = s_total_openingbalnce;

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
                                            data: promise.savedrecord,
                                            pageSize: 10,
                                            aggregate: [
                                                { name: 'totalpayable', field: 'totalpayable', aggregate: "sum" },
                                                { name: 'openingbalnce', field: 'openingbalnce', aggregate: "sum" },
                                                { name: 'FCSS_PaidAmount', field: 'FCSS_PaidAmount', aggregate: "sum" },
                                                { name: 'balance', field: 'balance', aggregate: "sum" },
                                                { name: 'concession', field: 'concession', aggregate: "sum" },
                                                { name: 'fine', field: 'fine', aggregate: "sum" },
                                                { name: 'rebate', field: 'rebate', aggregate: "sum" },
                                                { name: 'waived', field: 'waived', aggregate: "sum" },
                                               


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
                                        //columns: $scope.colarrayall,
                                        columns: [{

                                            title: "SLNO",
                                            template: "<span class='row-number'></span>"

                                        },
                                        {
                                            name: 'StudentName', field: 'StudentName', title: 'Student Name'
                                        },
                                        {
                                            name: 'AMCST_AdmNo', field: 'AMCST_AdmNo', title: 'Admission no'
                                        },
                                        {
                                            name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class Name'
                                        },
                                        {
                                            name: 'totalpayable', field: 'totalpayable', title: 'Total Payable', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                            groupFooterTemplate: "Sum: #=sum#"
                                            }, {
                                                name: 'openingbalnce', field: 'openingbalnce', title: 'Opening Balance', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                                                groupFooterTemplate: "Sum: #=sum#"
                                            }
                                            ,
                                        {
                                            name: 'FCSS_PaidAmount', field: 'FCSS_PaidAmount', title: 'Paid Amount', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
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
                                            }
                                        
                                        ],
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



                        else if (promise.radioval == "TRMW") {
                            $scope.termflg = true;
                            $scope.stdtermflg = false;

                            $scope.header_list = [];
                            angular.forEach($scope.groupcount, function (role) {
                                $scope.header_list.push(role);
                            })


                            if (promise.savedrecord != null && promise.savedrecord != "") {
                                $scope.Grid_view = false;
                                $scope.print_flag = true;

                                $('#grid123').empty();


                                $scope.totcountfirstnew = promise.savedrecord.length;
                                //  $scope.result = true;
                                var installmentcount = $scope.savedrecord.length;


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
                                $scope.data = promise.savedrecord;


                                //MB
                                if (promise.savedrecord != null && promise.savedrecord != "" && promise.savedrecord.length > 0) {

                                    var stu_list_new = [];
                                    angular.forEach(promise.savedrecord, function (op1) {
                                        var stu_id = op1.FMH_Id;
                                        var list_stu = [];
                                        angular.forEach(promise.savedrecord, function (op2) {
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
                                    debugger;
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
                            //angular.forEach($scope.groupcount, function (role) {
                            //    if (role.fmT_Id_chk) {
                            //        $scope.header_list.push(role);
                            //    }
                            //})

                            if (promise.savedrecord != null && promise.savedrecord != "") {
                                $scope.Grid_view = false;
                                $scope.print_flag = true;

                                $('#grid1234').empty();

                                $scope.totcountfirstnew = promise.savedrecord.length;
                              
                                //var installmentcount = $scope.groupcount.length;


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




                                ];
                          
                                $scope.newarray = [];
                                //var finalarray = 0;
                                //finalarray = Number(installmentcount)
                                //for (var i = 0; i < $scope.header_list.length; i++) {
                                //    for (var j = 0; j < 10; j++) {
                                //        $scope.newarray.push({ name: $scope.insarray[j].name, id: $scope.header_list[i].fmT_Id, name1: "hema" + $scope.header_list[i].fmT_Id + $scope.insarray[j].name });

                                //    }

                                //}
                                $scope.newarray1 = $scope.newarray;


                    
                                $scope.data = promise.savedrecord;


                          
                                if (promise.savedrecord != null && promise.savedrecord != "" && promise.savedrecord.length > 0) {



                                    var stu_list_new = [];
                                    angular.forEach(promise.savedrecord, function (op1) {
                                        var stu_id = op1.AMCST_Id;
                                        var list_stu = [];
                                        var count1 = 0;


                                        angular.forEach(promise.savedrecord, function (op2) {
                                            if (op2.AMST_Id == stu_id) {
                                                var coun = 0;


                                                angular.forEach($scope.header_list, function (op) {
                                                    if (op2.FMT_Id == op.fmT_Id) {
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
                                            stu_list_new.push({ AMCST_Id: stu_id, StudentName: op1.StudentName, ASMCL_ClassName: op1.ASMCL_ClassName, CDNetAmount: op1.CDNetAmount, CDPaidAmount: op1.CDPaidAmount, CDBalanceAmount: op1.CDBalanceAmount, Installment_Reports: list_stu });
                                        }
                                        else if (stu_list_new.length > 0) {
                                            var already_cnt = 0;
                                            angular.forEach(stu_list_new, function (td) {
                                                if (td.AMCST_Id == stu_id) {
                                                    already_cnt += 1;
                                                }
                                            })
                                            if (already_cnt == 0) {
                                                stu_list_new.push({ AMCST_Id: stu_id, StudentName: op1.StudentName, ASMCL_ClassName: op1.ASMCL_ClassName, CDNetAmount: op1.CDNetAmount, CDPaidAmount: op1.CDPaidAmount, CDBalanceAmount: op1.CDBalanceAmount, Installment_Reports: list_stu });
                                            }
                                        }




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

                                                    }
                                                })
                                            }

                                        })

                  
                                    })

                                    $scope.totcountfirstnew = stu_list_new.length;
                                    $scope.data = stu_list_new;
                                    console.log($scope.data);

                                    //hema
                                    debugger;
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
                                                data: $scope.savedrecord,
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
                    }
                    else {
                        swal("No Record Found");
                        
                        $scope.Grid_View = false;
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.exportExcel = function () {
            var grid = $scope.gridApi.grid;
            var rowTypes = exportUiGridService.ALL;
            var colTypes = exportUiGridService.ALL;
            exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'selected', 'selected');
        };

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
