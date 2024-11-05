
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeStudentLedgerController', CollegeStudentLedgerController)

    CollegeStudentLedgerController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$interval', 'uiGridGroupingConstants', 'Excel','$timeout']
    function CollegeStudentLedgerController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $interval, uiGridGroupingConstants, Excel, $timeout) {
        $scope.show_btn = false;
        $scope.show_cancel = false;
        $scope.show_grid = false;
        $scope.searc_button = true;
        $scope.tabledata = false;
        var paginationformasters;
        $scope.page1 = "page1";
        $scope.page2 = "page2";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else if (ivrmcofigsettings.length == 0 || ivrmcofigsettings == undefined || ivrmcofigsettings == null) {
            paginationformasters = 3;
        }

        paginationformasters = 3;
        //=========For filter char count for first table===============//
        $scope.searchValue = '';
        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }

        //(function () {
        //    'use strict';
        //    angular
        //.module('app')
        //        .controller('CollegeStudentLedgerController', CollegeStudentLedgerController)

        //    //CollegeStudentLedgerController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', , '$interval', 'uiGridGroupingConstants', 'exportUiGridService']
        //    CollegeStudentLedgerController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$interval', 'uiGridGroupingConstants', 'exportUiGridService']
        //    function CollegeStudentLedgerController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $interval, uiGridGroupingConstants, exportUiGridService) {
        //        $scope.show_btn = false;
        //        $scope.show_cancel = false;
        //        $scope.show_grid = false;
        //        $scope.searc_button = true;

        // $scope.sortReverse = true;
        //$scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}

        //$scope.imgname = logopath;

        ////$scope.acmS_Id = 'ALL';
        //var paginationformasters;
        //$scope.page1 = "page1";
        //$scope.page2 = "page2";
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}
        //else if (ivrmcofigsettings.length == 0 || ivrmcofigsettings == undefined || ivrmcofigsettings == null) {
        //    paginationformasters = 10;
        //}

        //paginationformasters = 10;
        ////=========For filter char count for first table===============//
        //$scope.searchValue = '';
        //$scope.search_box = function () {
        //    if ($scope.searchValue != "" || $scope.searchValue != null) {
        //        $scope.searc_button = false;
        //    }
        //    else {
        //        $scope.searc_button = true;
        //    }
        //}
        //====================End================================//
        //=========GET THE STUDENT DROPDOWN===============//

        $scope.get_student = function () {
            debugger;
            $scope.studentlist = [];
            $scope.submitted = false;
            if ($scope.myForm.$valid) {

                if ($scope.acmS_Id == 'ALL') {
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "AMCO_Id": $scope.amcO_Id,
                        "AMB_Id": $scope.amB_Id,
                        "AMSE_Id": $scope.amsE_Id,
                    }
                }
                else {
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "AMCO_Id": $scope.amcO_Id,
                        "AMB_Id": $scope.amB_Id,
                        "AMSE_Id": $scope.amsE_Id,
                        "ACMS_Id": $scope.acmS_Id,
                    }
                }

                apiService.create("CollegeStudentLedger/get_student", data).
                    then(function (promise) {
                        $scope.studentlist = promise.studentlist;
                        // $scope.amsE_Id = "";
                        if ($scope.studentlist.length == 0 || $scope.studentlist == null) {
                            swal('No Student!!!');
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }


        //====================End================================//


        //=========For filter char count for Second table===============//
        $scope.searchValue1 = "";
        $scope.search_box1 = function () {
            if ($scope.searchValue1 != "" || $scope.searchValue1 != null) {
                $scope.searc_button1 = false;
            }
            else {
                $scope.searc_button1 = true;
            }
        }
        //====================End================================//

        //=======If any Semester checkboxlist select then SHOW button Display Other wise Not=======//
        $scope.clar_sem = function () {

            $scope.chk_array = [];
            var chk_count = 0;
            angular.forEach($scope.semesterlist, function (itm) {
                if (itm.selected1 == true) {
                    chk_count += 1;
                    $scope.chk_array.push(itm);
                }
                if ($scope.chk_array.length > 0) {
                    $scope.show_btn = true;
                    $scope.show_cancel = true;

                }
                else {
                    $scope.show_btn = false;
                    $scope.show_cancel = false;

                    $scope.show_grid = false;
                }
            });
        }
        //====================End================================//


        $scope.exportToExcel = function (exporttable) {


            var exportHref = Excel.tableToExcel(exporttable, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }

        // $scope.sortKey = "acysT_RollNo";    //set the sortKey to the param passed
        // $scope.reverse = true;      //if true make it false and vice versa
        $scope.search = "";
        $scope.show_flag = false;


        $scope.sort1 = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.sortReverse = !$scope.sortReverse; //if true make it false and vice versa
        }
        $scope.studentlist = [];


        //============Start Data Load on the Page==============//
        $scope.loaddata = function () {


            var pageid = 1;
            apiService.getURI("CollegeStudentLedger/GetYearList", pageid).
                then(function (promise) {

                    $scope.yearlist = promise.yearlist;
                    $scope.sectionlist = promise.sectionlist;
                    $scope.quotalist = promise.quotalist;
                    // $scope.show_flag = false;
                })
        }
        //====================End===================//
        $scope.msg = '';
        //===========Load ALL Courses data in to the CheckboxList===============//
        $scope.get_courses = function () {
            $scope.msg = '';

            $scope.amcO_Id = "";
            $scope.courselist = [];
            $scope.amB_Id = ''
            $scope.branchlist = [];
            $scope.semesterlist = [];
            $scope.amsE_Id = '';
            $scope.show_flag = false;
            if ($scope.asmaY_Id != undefined && $scope.asmaY_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id
                }
                apiService.create("CollegeStudentLedger/get_courses", data).then(function (promise) {
                    $scope.courselist = promise.courselist;

                    $scope.grouplst = promise.fillmastergroup;
                    $scope.headlst = promise.fillmasterhead;
                    if ($scope.grouplst.length == 0) {
                        $scope.msg = "No Groups"
                    }
                    // $scope.amcO_Id = "";
                    if ($scope.courselist.length == 0 || $scope.courselist == null) {
                        swal('For Selected Year Courses Are Not Available!!!');

                    }
                })
            }
            else {
                $scope.courselist = [];
                $scope.amcO_Id = "";
            }
            $scope.show_btn = false;
            $scope.show_cancel = false;

            $scope.show_grid = false;
        };
        //====================End===================//

        //================Check all groups=====================//
        $scope.allgroupcheck = function () {
            if ($scope.allcheck == true) {
                angular.forEach($scope.grouplst, function (obj) {
                    obj.checkedgrplst = true;
                    angular.forEach($scope.headlst, function (obj1) {
                        if (obj1.fmG_Id == obj.fmG_Id) {
                            obj1.checkedheadlst = true;
                            //angular.forEach($scope.installlst, function (obj2) {
                            //    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                            //        obj2.checkedinstallmentlst = true;
                            //    }
                            //});
                        }
                    });

                });
            }
            else {
                angular.forEach($scope.grouplst, function (obj) {
                    obj.checkedgrplst = false;
                    angular.forEach($scope.headlst, function (obj1) {
                        if (obj1.fmG_Id == obj.fmG_Id) {
                            obj1.checkedheadlst = false;
                            //angular.forEach($scope.installlst, function (obj2) {
                            //    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                            //        obj2.checkedinstallmentlst = false;
                            //    }
                            //});
                        }
                    });

                });
            }

        }

        //====================End===================//

        //================Load Branches data in to the CheckboxList=====================//
        $scope.get_branches = function () {

            $scope.amB_Id = ''
            $scope.branchlist = [];
            $scope.semesterlist = [];
            $scope.amsE_Id = '';
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id
            }
            apiService.create("CollegeStudentLedger/get_branches", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
                // $scope.amB_Id = "";

                if ($scope.branchlist.length == 0 || $scope.branchlist == null) {
                    swal('For Selected Course Branches Are Not Available!!!');
                    $scope.show_btn = false;
                    $scope.show_cancel = false;
                    $scope.show_grid = false;
                }
            })
        }
        //========================END==================================//


        //============Load Semester data in to the CheckboxList==================//
        $scope.get_semisters = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id
            }
            apiService.create("CollegeStudentLedger/get_semisters", data).
                then(function (promise) {
                    $scope.semesterlist = promise.semesterlist;
                    // $scope.amsE_Id = "";
                    if ($scope.semesterlist.length == 0 || $scope.semesterlist == null) {
                        swal('For Selected Branch Semesters Are Not Available!!!');
                        $scope.show_btn = false;
                        $scope.show_cancel = false;
                        $scope.show_grid = false;
                    }
                })
        };
        //==========================END============================//



        //====================Delete Record===================//
        $scope.DeleteRecord = function (user) {

            //var id = user.fcmsgH_Id
            //alert(id)
            var data = {
                "FCMSGH_Id": user.fcmsgH_Id
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {

                    if (isConfirm) {

                        apiService.create("CollegeStudentLedger/DeleteRecord", data).
                            then(function (promise) {
                                if (promise.returnval == "true") {
                                    swal('Record Deleted Successfully');
                                }
                                else if (promise.returnval == "false") {
                                    swal('Record Not Deleted');
                                }
                                else if (promise.returnval == "Depend") {
                                    swal('Record cannot be Deleted.Transaction has already  been done for this group');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  Cancelled");
                    }
                });
        }
        //====================End===================//


        $scope.currentPage = 1;
        $scope.currentPage2 = 1;

        $scope.itemsPerPage = paginationformasters;

        //===========Load Student data in to the Table(grid)=============//
        $scope.get_report = function () {



            //$scope.studentlist = [];

            if ($scope.myForm.$valid) {

                var idc = [];
                var idb = [];
                var idse = [];
                angular.forEach($scope.courselist, function (crs) {
                    if (crs.selected) {
                        idc.push(crs.amcO_Id);
                    }
                })
                angular.forEach($scope.branchlist, function (brc) {
                    if (brc.selected) {
                        idb.push(brc.amB_Id);
                    }
                })
                angular.forEach($scope.semesterlist, function (sem) {
                    if (sem.selected1) {
                        idse.push(sem.amsE_Id);
                    }
                })

                var data = {
                    //  "MI_Id":$scope.MI_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    AMCO_Ids: idc,
                    AMB_Ids: idb,
                    AMSE_Ids: idse
                }
                apiService.create("CollegeStudentLedger/get_report", data).then(function (promise) {

                    $scope.show_grid = true;
                    $scope.studentlist = promise.studentlist;
                    $scope.grouplst = promise.fillmastergroup;
                    $scope.headlst = promise.fillmasterhead;
                    $scope.installlst = promise.fillinstallment;

                    $scope.grouplstedit = promise.fillmastergroup;
                    $scope.headlstedit = promise.fillmasterhead;
                    $scope.installlstedit = promise.fillinstallment;


                   

                    $scope.show_flag = true;
                    if (promise.studentlist != null) {
                        $scope.Recordlength = promise.studentlist.length;
                    }
                    if (promise.studentreport != null) {
                        $scope.Recordlength2 = promise.studentreport.length;
                    }
                    //$scope.amcsT_Id = "";
                    if ($scope.studentlist.length == 0 || $scope.studentlist == null) {
                        swal('For Selected details students are not Available!!');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //======================End===========================//



        //======================Edit Record=========================//
        var eeditstudentsdata = [];
        $scope.edit = function (role) {

            var data = {

                "AMCST_Id": role.amcsT_Id
            }
            apiService.create("CollegeStudentLedger/editdata", data).
                then(function (promise) {
                    role.studchecked = true;
                    $scope.AMCST_idedit = role.amcsT_Id;
                    $scope.eeditstudentsdata = promise.editdatalist;
                    if ($scope.eeditstudentsdata.length > 0) {
                        // $scope.ACMST_idedit = promise.amcsT_Id;
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            objedit.disablegrp = false;
                        });
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            objedit1.disablehead = false;
                        });
                        angular.forEach($scope.installlstedit, function (objedit2) {
                            objedit2.disableins = false;
                        });

                        angular.forEach($scope.eeditstudentsdata, function (grpeditt) {

                            angular.forEach($scope.grouplstedit, function (objedit) {
                                if (grpeditt.fmG_Id == objedit.fmG_Id) {
                                    objedit.checkedgrplstedit = true;
                                    //if (grpeditt.fsS_PaidAmount > 0) {
                                    //    objedit.disablegrp = true;
                                    //}

                                    //objedit.disablegrp = true;
                                    angular.forEach($scope.headlstedit, function (objedit1) {
                                        if (grpeditt.fmG_Id == objedit1.fmG_Id && objedit1.fmH_Id == grpeditt.fmH_Id) {
                                            objedit1.checkedheadlstedit = true;
                                            //if (grpeditt.fsS_PaidAmount > 0) {
                                            //    objedit1.disablehead = true;
                                            //}

                                            angular.forEach($scope.installlstedit, function (objedit2) {
                                                if (grpeditt.fmG_Id == objedit2.fmG_Id && grpeditt.fmH_Id == objedit2.fmH_Id && grpeditt.ftI_Id == objedit2.ftI_Id) {
                                                    objedit2.checkedinstallmentlstedit = true;
                                                    if (grpeditt.fcsS_PaidAmount > 0) {
                                                        objedit2.disableins = true;
                                                        objedit1.disablehead = true;
                                                        objedit.disablegrp = true;
                                                    }
                                                }
                                            });
                                        }

                                    });
                                }

                            });
                        });
                    }
                    else {
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            objedit.checkedgrplst = false;
                            objedit.disablegrp = false;
                        });
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            objedit1.checkedheadlst = false;
                            objedit1.disablehead = false;
                        });
                        angular.forEach($scope.installlstedit, function (objedit2) {
                            objedit2.checkedinstallmentlst = false;
                            objedit2.disableins = false;
                        });
                        swal("Student has not mapped with any of the group!")
                        $('#editmodal').modal('hide');
                        role.studchecked = false;
                        $scope.AMCST_idedit = 0;
                    }
                    $scope.isOptionsRequirededit1();
                })
        }
        //======================End===========================//
        //naveen work


        //$scope.gridOptions = {
        //    showGridFooter: true,
        //    showColumnFooter: true,
        //    enableFiltering: true,
        //    enableGridMenu: false,
        //    enableColumnMenus: false,
        //    treeRowHeaderAlwaysVisible: false,
        //    columnDefs: [
        //        { name: 'ASMAY_Year', displayName: 'ASMAY_Year', grouping: { groupPriority: 0 }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>'},
        //        {
        //            name: 'FMG_GroupName', displayName: 'FeeGroup', grouping: { groupPriority: 1 }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>'
        //        },
        //        { name: 'FMH_FeeName', displayName: 'Fee Head', grouping: { groupPriority: 2 }, sort: { priority: 1, direction: 'asc' }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

        //        { name: 'Receipt', displayName: 'Recepit', grouping: { groupPriority: 2 }, sort: { priority: 1, direction: 'asc' }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' 
        //        },
        //        {
        //            name: 'PaidAmount', displayName: 'PaidAmount',width: '25%'
        //        },

        //        {
        //            name: 'ConcessionAmount', displayName: 'ConcessionAmount', width: '25%'
        //        },
        //        {
        //            name: 'StudentFeesDue', displayName: 'StudentFeesDue ', width: '25%'
        //        },
        //        {
        //            name: 'AdjustedAmount', displayName: 'AdjustedAmount', width: '25%'
        //        },
        //        {
        //            name: 'ExcessAmountAdjusted', displayName: 'ExcessAmountAdjusted', width: '25%'
        //        }

        //    ],
        //    exporterMenuPdf: false,
        //    onRegisterApi: function (gridApi) {
        //        $scope.gridApi = gridApi;
        //    },
        //    gridMenuCustomItems: [{
        //        title: 'student Ledger report',
        //        action: function ($event) {
        //            exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
        //        },
        //        order: 110
        //    },
        //    {
        //        title: 'Export visible data as EXCEL',
        //        action: function ($event) {
        //            exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
        //        },
        //        order: 111
        //    }
        //    ]
        //};

        //$scope.exportExcel = function () {
        //    var grid = $scope.gridApi.grid;
        //    var rowTypes = exportUiGridService.ALL;
        //    var colTypes = exportUiGridService.ALL;
        //    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
        //};


        $scope.gridOptions = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: true,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'ASMAY_Year', displayName: 'ASMAY_Year', grouping: { groupPriority: 0 }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'FMG_GroupName', displayName: 'FMG_GroupName', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'FMH_FeeName', displayName: 'FMH_FeeName', grouping: { groupPriority: 2 }, sort: { priority: 1, direction: 'asc' }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'Receipt', displayName: 'Receipt', grouping: { groupPriority: 3 }, sort: { priority: 1, direction: 'asc' }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                {
                    name: 'TotalCharges', displayName: 'TotalCharges', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'PaidAmount', displayName: 'PaidAmount', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'ConcessionAmount', displayName: 'ConcessionAmount', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'StudentFeesDue', displayName: 'StudentFeesDue', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'ConcessionAmount', displayName: 'ConcessionAmount', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'AdjustedAmount', displayName: 'AdjustedAmount', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'ExcessAmountAdjusted', displayName: 'ExcessAmountAdjusted', width: '25%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                }
            ],
            exporterMenuPdf: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'class year status report',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ]
        };


        $scope.exportExcel = function (exporttable) {
            //var grid = $scope.gridApi.grid;
            //var rowTypes = exportUiGridService.ALL;
            //var colTypes = exportUiGridService.ALL;
            //exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
            var exportHref = Excel.tableToExcel(exporttable, 'sheet name');

            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };



        //naveen work

        //============================Save For GroupList Data=========================//
        $scope.valsgroup = [];
        $scope.valshead = [];
        $scope.valsinstallment = [];
        $scope.valstudentlst = [];
        $scope.feedetails = [];
        $scope.show_grid = false;

        $scope.lastyrtotal = 0;
        $scope.curryrtotal = 0;
        $scope.rectotal = 0;
        $scope.paidtotal = 0;
        $scope.finertotal = 0;
        $scope.constotal = 0;
        $scope.lastyrtotal1 = 0;
        $scope.curryrtotal1 = 0;
        $scope.rectotal1 = 0;
        $scope.paidtotal1 = 0;
        $scope.finertotal1 = 0;
        $scope.constotal1 = 0;
        //$scope.studentdata = promise.studentlist
        $scope.savedata = function (grouplst, headlst) {
           
            $scope.feedetails = [];
            $scope.valsgroup = [];
            $scope.valshead = [];
            $scope.valsinstallment = [];
            $scope.valstudentlst = [];
            $scope.sec_list = [];
            // $scope.page1 = "page1";
            $scope.show_flag = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                for (var t = 0; t < grouplst.length; t++) {
                    if (grouplst[t].checkedgrplst == true) {
                        $scope.valsgroup.push(grouplst[t]);
                    }
                }

                for (var u = 0; u < headlst.length; u++) {
                    if (headlst[u].checkedheadlst == true) {
                        $scope.valshead.push(headlst[u]);
                    }
                }

                grouplst = $scope.valsgroup;
                headlst = $scope.valshead;
                //   installlst = $scope.valsinstallment;
                if ($scope.valsgroup.length > 0) {
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "AMCST_Id": $scope.amcsT_Id,
                        savegrplst: grouplst,
                        saveheadlst: headlst,

                    }

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("CollegeStudentLedger/savedata", data).
                        then(function (promise) {
                            $scope.show_grid = true;
                            $scope.feedetails = promise.feedetails;
                            console.log($scope.feedetails)
                            $scope.gridOptions.data = $scope.feedetails;
                            $scope.StudentReport = promise.feedetails;
                            //if ($scope.feedetails.length!= 0) {
                            //    $scope.show_grid = true;
                        })
                }
                else {
                    swal("Select Atleast One Group!!!");
                }


            }
            else {
                $scope.submitted = true;
            }
        };
        //======================End=======================//
        $scope.printdatatablegrp = [];
        $scope.optionToggledgrp = function (SelectedStudentRecord, index) {

            $scope.lastyrtotal1 = 0;
            $scope.curryrtotal1 = 0;
            $scope.rectotal1 = 0;
            $scope.paidtotal1 = 0;
            $scope.finertotal1 = 0;
            $scope.constotal1 = 0;

            $scope.grpall = $scope.feedetails.every(function (itm) { return itm.grpselected; });

            if ($scope.printdatatablegrp.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatablegrp.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatablegrp.splice($scope.printdatatablegrp.indexOf(SelectedStudentRecord), 1);
            }


            if ($scope.printdatatablegrp.length > 0) {
                angular.forEach($scope.printdatatablegrp, function (ff) {
                    $scope.lastyrtotal1 = $scope.lastyrtotal1 + ff.StudentDue;
                    $scope.curryrtotal1 = $scope.curryrtotal1 + ff.BFCSS_CurrentYrCharges;
                    $scope.rectotal1 = $scope.rectotal1 + ff.Receivable;
                    $scope.paidtotal1 = $scope.paidtotal1 + ff.Collection;
                    $scope.finertotal1 = $scope.finertotal1 + ff.CollectionAnyTime;
                    $scope.constotal1 = $scope.constotal1 + ff.BFCSS_ConcessionAmount;
                })

                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

        }

        $scope.toggleAllgrp = function () {
            $scope.lastyrtotal1 = 0;
            $scope.curryrtotal1 = 0;
            $scope.rectotal1 = 0;
            $scope.paidtotal1 = 0;
            $scope.finertotal1 = 0;
            $scope.constotal1 = 0;
            // $scope.printdatatablegrp = [];
            var toggleStatus = $scope.grpall;
            angular.forEach($scope.feedetails, function (itm) {
                itm.grpselected = toggleStatus;
                if ($scope.grpall == true) {
                    $scope.printdatatablegrp.push(itm);
                }
                //else {
                //    $scope.printdatatablegrp.splice(itm);
                //}
            });

            angular.forEach($scope.printdatatablegrp, function (ff) {
                $scope.lastyrtotal1 = $scope.lastyrtotal1 + ff.StudentDue;
                $scope.curryrtotal1 = $scope.curryrtotal1 + ff.BFCSS_CurrentYrCharges;
                $scope.rectotal1 = $scope.rectotal1 + ff.Receivable;
                $scope.paidtotal1 = $scope.paidtotal1 + ff.Collection;
                $scope.finertotal1 = $scope.finertotal1 + ff.CollectionAnyTime;
                $scope.constotal1 = $scope.constotal1 + ff.BFCSS_ConcessionAmount;
            })
            if ($scope.printdatatablegrp.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

        }


        //===================Cancel========================//
        $scope.cancel = function () {
            $state.reload();
        }
        //===================End========================//


        //===========Field Validation=================//
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        //==================End===========================//


        //========Branchlist CheckBox Field Validation===========//
        $scope.isOptionsRequired_1 = function () {
            return !$scope.branchlist.some(function (options) {
                return options.selected;
            });
        }
        //==================End===========================//


        //========courselist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.courselist.some(function (options) {
                return options.selected;
            });
        }
        //==================End===========================//


        //========semesterlist CheckBox Field Validation============//
        $scope.isOptionsRequired_2 = function () {
            return !$scope.semesterlist.some(function (options) {
                return options.selected1;

            });
        }
        //==================End===========================//

        //==========Select for selected Grouplst,headlst,installment data for store record(for save button)================//
        $scope.firstfnc = function (vlobj) {

            if (vlobj.checkedgrplst == true) {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = true;
                                angular.forEach($scope.installlst, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst = true;
                                    }
                                });
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = false;
                                angular.forEach($scope.installlst, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst = false;
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        $scope.secfnc = function (vlobj1) {

            if (vlobj1.checkedheadlst == true) {
                angular.forEach($scope.headlst, function (val) {
                    if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
                        angular.forEach($scope.installlst, function (val1) {
                            if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
                                val1.checkedinstallmentlst = true;
                            }
                        });
                    }
                });
            } else {

                angular.forEach($scope.headlst, function (val) {
                    if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
                        angular.forEach($scope.installlst, function (val1) {
                            if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
                                val1.checkedinstallmentlst = false;
                            }
                        });
                    }
                });
            }

            for (var s = 0; s < $scope.grouplst.length; s++) {
                if (vlobj1.fmG_Id == $scope.grouplst[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlst.length; t++) {
                        if (vlobj1.fmG_Id == $scope.headlst[t].fmG_Id) {
                            if ($scope.headlst[t].checkedheadlst == false) {
                                $scope.grouplst[s].checkedgrplst = false;
                            }
                            else {
                                $scope.grouplst[s].checkedgrplst = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        $scope.trdfnc = function (vlobj2, oobjj) {
            for (var u = 0; u < $scope.headlst.length; u++) {
                if (vlobj2.fmG_Id == $scope.headlst[u].fmG_Id && vlobj2.fmH_Id == $scope.headlst[u].fmH_Id) {
                    for (var v = 0; v < $scope.installlst.length; v++) {
                        if ($scope.installlst[v].fmH_Id == $scope.headlst[u].fmH_Id && $scope.installlst[v].fmG_Id == $scope.headlst[u].fmG_Id) {
                            if ($scope.installlst[v].checkedinstallmentlst == false) {
                                $scope.headlst[u].checkedheadlst = false;
                            }
                            else {
                                $scope.headlst[u].checkedheadlst = true;
                                break;
                            }
                        }
                    }

                    for (var w = 0; w < $scope.grouplst.length; w++) {
                        if (vlobj2.fmG_Id == $scope.grouplst[w].fmG_Id) {
                            for (var x = 0; x < $scope.headlst.length; x++) {
                                if (vlobj2.fmG_Id == $scope.headlst[x].fmG_Id) {
                                    if ($scope.headlst[x].checkedheadlst == false) {
                                        $scope.grouplst[w].checkedgrplst = false;
                                    }
                                    else {
                                        $scope.grouplst[w].checkedgrplst = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //==================End===========================//


        //==========Select for selected Grouplst,headlst,installment data for store record(for Edit)================//
        $scope.firstfncedit = function (vlobjedit) {

            if (vlobjedit.checkedgrplstedit == true) {
                angular.forEach($scope.grouplstedit, function (objedit) {
                    if (vlobjedit.fmG_Id == objedit.fmG_Id) {
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            if (objedit1.fmG_Id == objedit.fmG_Id) {
                                objedit1.checkedheadlstedit = true;
                                angular.forEach($scope.installlstedit, function (objedit2) {
                                    if (objedit.fmG_Id == objedit2.fmG_Id && objedit1.fmH_Id == objedit2.fmH_Id) {
                                        objedit2.checkedinstallmentlstedit = true;
                                    }
                                });
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplstedit, function (objedit) {
                    if (vlobjedit.fmG_Id == objedit.fmG_Id) {
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            if (objedit1.fmG_Id == objedit.fmG_Id) {
                                objedit1.checkedheadlstedit = false;
                                angular.forEach($scope.installlstedit, function (objedit2) {
                                    if (objedit.fmG_Id == objedit2.fmG_Id && objedit1.fmH_Id == objedit2.fmH_Id) {
                                        objedit2.checkedinstallmentlstedit = false;
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        $scope.secfncedit = function (vlobjedit1) {

            if (vlobjedit1.checkedheadlstedit == true) {
                //angular.forEach($scope.grouplst, function (val) {
                //    if (vlobj1.fmG_Id == val.fmG_Id) {
                //        angular.forEach($scope.headlst, function (val1) {
                //            if (val1.fmG_Id == val.fmG_Id) {
                //                val1.checkedheadlst = true;
                //                angular.forEach($scope.installlst, function (val2) {
                //                    if (val.fmG_Id == val2.fmG_Id && val1.fmH_Id == val2.fmH_Id) {
                //                        val2.checkedinstallmentlst = true;
                //                    }
                //                });
                //            }
                //        });
                //    }
                //});

                angular.forEach($scope.headlstedit, function (valedit) {
                    if (vlobjedit1.fmG_Id == valedit.fmG_Id && vlobjedit1.fmH_Id == valedit.fmH_Id) {
                        angular.forEach($scope.installlstedit, function (valedit1) {

                            if (valedit1.fmH_Id == valedit.fmH_Id && valedit1.fmG_Id == valedit.fmG_Id) {
                                valedit1.checkedinstallmentlstedit = true;
                            }

                        });
                    }
                });
            } else {
                angular.forEach($scope.headlstedit, function (valedit) {
                    if (vlobjedit1.fmG_Id == valedit.fmG_Id && vlobjedit1.fmH_Id == valedit.fmH_Id) {
                        angular.forEach($scope.installlstedit, function (valedit1) {
                            if (valedit1.fmH_Id == valedit.fmH_Id && valedit1.fmG_Id == valedit.fmG_Id) {
                                valedit1.checkedinstallmentlstedit = false;
                            }
                        });
                    }
                });
            }

            for (var s = 0; s < $scope.grouplstedit.length; s++) {
                if (vlobjedit1.fmG_Id == $scope.grouplstedit[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlstedit.length; t++) {
                        if (vlobjedit1.fmG_Id == $scope.headlstedit[t].fmG_Id) {
                            if ($scope.headlstedit[t].checkedheadlstedit == false) {
                                $scope.grouplstedit[s].checkedgrplstedit = false;
                            }
                            else {
                                $scope.grouplstedit[s].checkedgrplstedit = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        $scope.trdfncedit = function (vlobjedit2) {

            for (var u = 0; u < $scope.headlstedit.length; u++) {
                if (vlobjedit2.fmG_Id == $scope.headlstedit[u].fmG_Id && vlobjedit2.fmH_Id == $scope.headlstedit[u].fmH_Id) {
                    for (var v = 0; v < $scope.installlstedit.length; v++) {
                        if ($scope.installlstedit[v].fmH_Id == $scope.headlstedit[u].fmH_Id && $scope.installlstedit[v].fmG_Id == $scope.headlstedit[u].fmG_Id) {
                            if ($scope.installlstedit[v].checkedinstallmentlstedit == false) {
                                $scope.headlstedit[u].checkedheadlstedit = false;
                            }
                            else {
                                $scope.headlstedit[u].checkedheadlstedit = true;
                                break;
                            }
                        }
                    }

                    for (var w = 0; w < $scope.grouplstedit.length; w++) {
                        if (vlobjedit2.fmG_Id == $scope.grouplstedit[w].fmG_Id) {
                            for (var x = 0; x < $scope.headlstedit.length; x++) {
                                if (vlobjedit2.fmG_Id == $scope.headlstedit[x].fmG_Id) {
                                    if ($scope.headlstedit[x].checkedheadlstedit == false) {
                                        $scope.grouplstedit[w].checkedgrplstedit = false;
                                    }
                                    else {
                                        $scope.grouplstedit[w].checkedgrplstedit = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //==================End===========================//
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}
        //$scope.imgname = logopath;

        $scope.printData = function () {
            if ($scope.printdatatablegrp !== null && $scope.printdatatablegrp.length > 0) {
                var innerContents = document.getElementById("printgrdgrp").innerHTML;
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


        //==============This is used for Field empty and close popup=====================//
        $scope.cllose = function () {

            angular.forEach($scope.grouplstedit, function (objedit) {
                objedit.checkedgrplstedit = false;
            });
            angular.forEach($scope.headlstedit, function (objedit1) {
                objedit1.checkedheadlstedit = false;
            });
            angular.forEach($scope.installlstedit, function (objedit2) {
                objedit2.checkedinstallmentlstedit = false;
            });
            angular.forEach($scope.studentlist, function (obj) {
                if (obj.amcsT_Id == $scope.AMCST_idedit) {
                    obj.studchecked = false;
                }
            });
            $scope.AMCST_idedit = 0;
            // $('#editmodal').modal('hide');
        }
        //==================End===========================//
        $scope.isOptionsRequired1 = function () {
            if ($scope.AMCST_idedit > 0) {
                return false;
            }
            else {
                return !$scope.grouplst.some(function (options) {
                    return options.checkedgrplst;
                });
            }

        }
        $scope.isOptionsRequirededit1 = function () {
            if ($scope.AMCST_idedit > 0) {
                return !$scope.grouplstedit.some(function (options) {
                    return options.checkedgrplstedit;
                });
            }
            else {
                return false;
            }
        }
    }
})();

