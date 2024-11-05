
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGCHADMDetailsController', CLGCHADMDetailsController)

    CLGCHADMDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'uiGridGroupingConstants']
    function CLGCHADMDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiGridGroupingConstants) {

        $scope.studentstrenthgr = false;
        $scope.totalregstudent = 0;

        $scope.totalnewstudent = 0;
        $scope.sms = 0;
        $scope.email = 0;
        $scope.regular = [];
        $scope.fields = function () {

            $scope.newadmissionstdtotal = [];
            $scope.datagraph = [];
            $scope.regularstdtotal = [];
            $scope.newadmstdgraphdta = [];


            $scope.Todaydate = new Date();
        }



        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Binddata = function () {
            $scope.fields();
            $scope.studentstrenthgr = false;

            apiService.getDATA("CLGCHStudentStrength/Getdetails1").
                then(function (promise) {

                    $scope.studentstrenth = promise.fillstudentstrenth;
                    $scope.fillnewstd = promise.fillnewstd;

                    console.log($scope.studentstrenth);
                    $scope.gridOptions.data = $scope.studentstrenth;
                    $scope.gridOptions1.data = $scope.fillnewstd;


                    if ($scope.studentstrenth.length > 0) {
                        $scope.studentstrenthgr = true;


                        $scope.yearlt = promise.yearlist;
                        $scope.regular = promise.sectionwisestrenth;

                        $scope.asmaY_Id = promise.asmaY_Id;



                        $scope.getgraphdata();
                        $scope.getgraphdata1();

                        $scope.loadcharts();

                    }
                    else {
                        swal("No Record Found")
                    }




                })

        }


        $scope.gridOptions = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: true,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'AMCO_CourseName', displayName: 'Course Name', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, width: '24%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'AMB_BranchName', displayName: 'Branch Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'AMSE_SEMName', displayName: 'Semester', grouping: { groupPriority: 2 }, sort: { priority: 2, direction: 'asc' }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'ACMS_SectionName', displayName: 'Section', grouping: { groupPriority: 3 }, sort: { priority: 0, direction: 'asc' }, width: '10%' },
                {
                    name: 'stud_count', displayName: 'Count', width: '15%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
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

        $scope.gridOptions1 = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: true,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'AMCO_CourseName', displayName: 'Course Name', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, width: '24%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'AMB_BranchName', displayName: 'Branch Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'AMSE_SEMName', displayName: 'Semester', grouping: { groupPriority: 2 }, sort: { priority: 2, direction: 'asc' }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'ACMS_SectionName', displayName: 'Section', grouping: { groupPriority: 3 }, sort: { priority: 0, direction: 'asc' }, width: '10%' },
                {
                    name: 'stud_count', displayName: 'Count', width: '15%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
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
        $scope.regularstdtotal1 = [];
        $scope.loadcharts = function () {
            var total = 0;
            var total1 = 0;
            $scope.regularstdtotal = [];
            $scope.regularstdtotal1 = [];

            if ($scope.grapharray != null) {

                for (var i = 0; i < $scope.grapharray.length; i++) {
                    $scope.regularstdtotal.push({ label: $scope.grapharray[i].AMCO_CourseName + '-' + $scope.grapharray[i].AMB_BranchName, "y": $scope.grapharray[i].stud_count })
                }
            }
            if ($scope.grapharray1 != null) {

                for (var i = 0; i < $scope.grapharray1.length; i++) {
                    $scope.regularstdtotal1.push({ label: $scope.grapharray1[i].AMCO_CourseName + '-' + $scope.grapharray1[i].AMB_BranchName, "y": $scope.grapharray1[i].stud_count })
                }
            }
            
            var chart = new CanvasJS.Chart("areachart",
                {
                    width: 1052,
                    height: 348,
                    axisX: {
                        labelFontSize: 10,
                        interval: 1,
                        labelFontColor: "black",
                        labelAngle: -20
                        //title: "Designation",
                    },
                    axisY: {
                        labelFontSize: 12,
                        // title: "No.of. Staffs",

                    },

                    data: [
                        {
                            type: "column",
                            showInLegend: true,
                            dataPoints: $scope.regularstdtotal
                        }
                    ]
                });

            chart.render();

            var chart = new CanvasJS.Chart("areachart1",
                {
                    width: 1052,
                    height: 348,
                    axisX: {
                        labelFontSize: 10,
                        interval: 1,
                        labelFontColor: "black",
                        labelAngle: -20
                        //title: "Designation",
                    },
                    axisY: {
                        labelFontSize: 12,
                        // title: "No.of. Staffs",

                    },

                    data: [
                        {
                            type: "column",
                            showInLegend: true,
                            dataPoints: $scope.regularstdtotal1
                        }
                    ]
                });

            chart.render();

        }




        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.regularstdtotal = [];
            $scope.regularstdtotal1 = [];
            $scope.studentstrenthgr = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var a = $scope.asmaY_Id;
                // alert(asmaY_Id)
                $scope.fields();
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "withtc": $scope.withtc,
                    "withdeactive": $scope.withdeactive,
                }
                apiService.create("CLGCHStudentStrength/yrchange1", data).
                    then(function (promise) {
                        $scope.studentstrenth = promise.fillstudentstrenth;
                        $scope.fillnewstd = promise.fillnewstd;
                        $scope.gridOptions.data = $scope.studentstrenth;
                        $scope.gridOptions1.data = $scope.fillnewstd;

                        if ($scope.studentstrenth.length > 0) {


                            $scope.studentstrenthgr = true;

                            // $scope.yearlt = promise.yearlist;
                            $scope.regular = promise.sectionwisestrenth;

                            $scope.asmaY_Id = promise.asmaY_Id;

                            $scope.getgraphdata();
                            $scope.getgraphdata1();

                            $scope.loadcharts();

                        }
                        else {
                            swal("No Record Found")
                        }

                    })
            }
            else {
                $scope.submitted = true;
                $scope.studentstrenthgr = false;
            }

        }

        $scope.changetc = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var a = $scope.asmaY_Id;
                // alert(asmaY_Id)
                $scope.fields();
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "withtc": $scope.withtc,
                    "withdeactive": $scope.withdeactive,
                }
                apiService.create("CLGCHStudentStrength/yrchange1", data).
                    then(function (promise) {
                        $scope.studentstrenth = promise.fillstudentstrenth;
                        $scope.fillnewstd = promise.fillnewstd;
                        $scope.gridOptions.data = $scope.studentstrenth;
                        $scope.gridOptions1.data = $scope.fillnewstd;

                        if ($scope.studentstrenth.length > 0) {


                            $scope.studentstrenthgr = true;

                            // $scope.yearlt = promise.yearlist;
                            $scope.regular = promise.sectionwisestrenth;

                            $scope.asmaY_Id = promise.asmaY_Id;

                            $scope.getgraphdata();
                            $scope.getgraphdata1();

                            $scope.loadcharts();

                        }
                        else {
                            swal("No Record Found")
                        }

                    })
            }
            else {
                $scope.submitted = true;
                $scope.studentstrenthgr = false;
            }

        }


        $scope.changetc1 = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var a = $scope.asmaY_Id;
                // alert(asmaY_Id)
                $scope.fields();
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "withtc": $scope.withtc,
                    "withdeactive": $scope.withdeactive,
                }
                apiService.create("CLGCHStudentStrength/yrchange1", data).
                    then(function (promise) {
                        $scope.studentstrenth = promise.fillstudentstrenth;
                        $scope.fillnewstd = promise.fillnewstd;
                        $scope.gridOptions.data = $scope.studentstrenth;
                        $scope.gridOptions1.data = $scope.fillnewstd;

                        if ($scope.studentstrenth.length > 0) {


                            $scope.studentstrenthgr = true;

                            // $scope.yearlt = promise.yearlist;
                            $scope.regular = promise.sectionwisestrenth;

                            $scope.asmaY_Id = promise.asmaY_Id;

                            $scope.getgraphdata();
                            $scope.getgraphdata1();

                            $scope.loadcharts();

                        }
                        else {
                            swal("No Record Found")
                        }

                    })
            }
            else {
                $scope.submitted = true;
                $scope.studentstrenthgr = false;
            }

        }
        $scope.getgraphdata = function () {
            $scope.exam_list = [];
            $scope.overalltotalmax = 0;
            angular.forEach($scope.studentstrenth, function (st) {
                if ($scope.exam_list.length == 0) {
                    $scope.exam_list.push({ AMCO_Id: st.AMCO_Id, AMCO_CourseName: st.AMCO_CourseName, AMB_Id: st.AMB_Id, AMB_BranchName: st.AMB_BranchName });
                }
                else if ($scope.exam_list.length > 0) {
                    var al_exm_cnt = 0;
                    angular.forEach($scope.exam_list, function (exm) {
                        if (exm.AMCO_Id == st.AMCO_Id && exm.AMB_Id == st.AMB_Id) {
                            al_exm_cnt += 1;
                        }
                    })
                    if (al_exm_cnt == 0) {

                        $scope.exam_list.push({ AMCO_Id: st.AMCO_Id, AMCO_CourseName: st.AMCO_CourseName, AMB_Id: st.AMB_Id, AMB_BranchName: st.AMB_BranchName });
                    }
                }
            })


            $scope.grapharray = [];
            angular.forEach($scope.exam_list, function (kk) {
                $scope.branchcnt = 0;
                angular.forEach($scope.studentstrenth, function (xx) {
                    if (kk.AMCO_Id == xx.AMCO_Id && kk.AMB_Id == xx.AMB_Id) {
                        $scope.branchcnt += xx.stud_count;
                    }

                })
                $scope.grapharray.push({ AMCO_Id: kk.AMCO_Id, AMCO_CourseName: kk.AMCO_CourseName, AMB_Id: kk.AMB_Id, AMB_BranchName: kk.AMB_BranchName, stud_count: $scope.branchcnt })
            })


        }



        $scope.getgraphdata1 = function () {
            $scope.exam_list = [];
            $scope.overalltotalmax = 0;
            angular.forEach($scope.fillnewstd, function (st) {
                if ($scope.exam_list.length == 0) {
                    $scope.exam_list.push({ AMCO_Id: st.AMCO_Id, AMCO_CourseName: st.AMCO_CourseName, AMB_Id: st.AMB_Id, AMB_BranchName: st.AMB_BranchName });
                }
                else if ($scope.exam_list.length > 0) {
                    var al_exm_cnt = 0;
                    angular.forEach($scope.exam_list, function (exm) {
                        if (exm.AMCO_Id == st.AMCO_Id && exm.AMB_Id == st.AMB_Id) {
                            al_exm_cnt += 1;
                        }
                    })
                    if (al_exm_cnt == 0) {

                        $scope.exam_list.push({ AMCO_Id: st.AMCO_Id, AMCO_CourseName: st.AMCO_CourseName, AMB_Id: st.AMB_Id, AMB_BranchName: st.AMB_BranchName });
                    }
                }
            })


            $scope.grapharray1 = [];
            angular.forEach($scope.exam_list, function (kk) {
                $scope.branchcnt = 0;
                angular.forEach($scope.fillnewstd, function (xx) {
                    if (kk.AMCO_Id == xx.AMCO_Id && kk.AMB_Id == xx.AMB_Id) {
                        $scope.branchcnt += xx.stud_count;
                    }

                })
                $scope.grapharray1.push({ AMCO_Id: kk.AMCO_Id, AMCO_CourseName: kk.AMCO_CourseName, AMB_Id: kk.AMB_Id, AMB_BranchName: kk.AMB_BranchName, stud_count: $scope.branchcnt })
            })
        }



    };
})();