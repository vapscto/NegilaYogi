(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeGroupWiseStudentReportController', CollegeGroupWiseStudentReportController)
    CollegeGroupWiseStudentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', 'exportUiGridService', 'uiGridConstants']
    function CollegeGroupWiseStudentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, exportUiGridService, uiGridConstants) {

        $scope.colarrayaggre = [];
        $scope.usrname = localStorage.getItem('username');

        $scope.FMCB_fromDATE = new Date();
        $scope.FMCB_toDATE = new Date();

        $scope.obj = {};
        $scope.print_data = false;
        $scope.print = false;

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
        $scope.checkallhrd = true;
        $scope.hrdallcheck = function () {
            var toggleStatus1 = $scope.checkallhrd;
            angular.forEach($scope.arrlistchkgroup, function (itm) {
                itm.selected = toggleStatus1;
            });
        }
        $scope.hrgsinglecheck = function () {

            $scope.checkallhrd = $scope.arrlistchkgroup.every(function (itm) {

                return itm.selected;
            });


        };

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



        $scope.onselectyear = function (obj) {

            var data = {
                "ASMAY_Id": obj.ASMAY,
            }

            apiService.create("CollegeGroupWiseStudentReport/get_courses", data).
                then(function (promise) {

                    $scope.coursecount = promise.courselist;
                    angular.forEach($scope.coursecount, function (tr) {
                        tr.selectedcourse = true;
                    })

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

            apiService.create("CollegeGroupWiseStudentReport/get_branches", data).
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

            apiService.create("CollegeGroupWiseStudentReport/get_semisters_new", data).
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

        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.tabel12 = false;
            $scope.tabel123 = false;

            var data = {
                "configset": grouporterm,
            }

            apiService.create("CollegeGroupWiseStudentReport/getalldetails", data).
                then(function (promise) {
                    $scope.yearlst = promise.yearlst;

                    $scope.obj.ASMAY = academicyrlst[0].asmaY_Id;
                    $scope.userlist = promise.userdetails;
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
                        $scope.sortKey = keyname;
                        $scope.reverse = !$scope.reverse;
                    }
                })
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
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

        $scope.userdetails = function () {
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




        //getting the fmg selected values
        $scope.binddatagrp = function () {

            $scope.albumNameArray = [];
            angular.forEach($scope.arrlistchkgroup, function (role) {
                if (!!role.selected) $scope.albumNameArray.push(
                    {
                        FMG_Id: role.fmG_Id
                    });

            })
            var data = {

                TempararyArrayList: $scope.albumNameArray,
                // Group_All: $scope.group_check,
                "ASMAY_Id": $scope.obj.ASMAY,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeGroupWiseStudentReport/getgroupmappedheads", data).
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

            apiService.create("CollegeGroupWiseStudentReport/getgroupheadsid", data).
                then(function (promise) {

                })

        };

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
                apiService.create("CollegeGroupWiseStudentReport/getgroupheadsid", data).
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
                    "Fromdate": $scope.from_date,
                    "Todate": $scope.to_date,
                    "radioval": $scope.rpttyp,
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
                  
                }
                //$scope.FMCB_fromDATE = new Date(data.Fromdate, $scope.FMCB_fromDATE);

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CollegeGroupWiseStudentReport/Getreportdetails", data).then(function (promise) {

                    if (promise.savedrecord != null && promise.savedrecord !== "") {
                        if (promise.savedrecord.length > 0) {
                            $scope.print_data = true;
                            $scope.print = true;
                            $scope.showgrid = true;
                            $scope.totcountfirst = promise.savedrecord.length;
                            if ($scope.rpttyp == "Mapped") {

                                $scope.mappedlist = promise.savedrecord;

                                $scope.mapped = true;
                                $scope.unmapped = false;
                            }

                            else if ($scope.rpttyp == "Unmapped") {


                                $scope.unmappedlist = promise.savedrecord;

                                $scope.mapped = false;
                                $scope.unmapped = true;







                            }
                        }
                        else {
                            swal("No Record Found");
                            $scope.unmappedlist = [];
                            $scope.mappedlist = [];
                            $scope.print_data = false;
                        }
                    }
                    else {
                        swal("No Record Found");
                        $scope.unmappedlist = [];
                        $scope.mappedlist = [];
                        $scope.print_data =false;
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


        $scope.printData = function () {
            if (($scope.mappedlist !== null && $scope.mappedlist.length > 0) || ($scope.unmappedlist !== null && $scope.unmappedlist.length > 0)) {
                var innerContents = document.getElementById("mappedlist").innerHTML;
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
