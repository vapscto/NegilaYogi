

(function () {
    'use strict';
    angular
        .module('app')
        .controller('Clg_HODEmpSalaryController', Clg_HODEmpSalaryController)

    Clg_HODEmpSalaryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'uiGridGroupingConstants']
    function Clg_HODEmpSalaryController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiGridGroupingConstants) {

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

            apiService.getDATA("Clg_HODEmpSalary/Getdetails").
                then(function (promise) {

                    $scope.studentstrenth = promise.fillstudentstrenth;

                    console.log($scope.studentstrenth);
                    $scope.gridOptions.data = $scope.studentstrenth;

                    $scope.yearlt = promise.yearlist;
                    if ($scope.studentstrenth.length > 0) {
                        $scope.studentstrenthgr = true;

                        $scope.regular = promise.sectionwisestrenth;

                        $scope.hrmlY_Id = promise.hrmlY_Id;



                        $scope.getgraphdata();

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

                { name: 'Name', displayName: 'Name', grouping: { groupPriority: 1 }, width: '24%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'month1', displayName: 'MONTH', grouping: { groupPriority: 0 }, width: '24%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'HRMD_DepartmentName', displayName: 'Department Name', grouping: { groupPriority: 2 }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'HRMDES_DesignationName', displayName: 'Designation Name', grouping: { groupPriority: 3 }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                {
                    name: 'netamount', displayName: 'NET SALARY', width: '15%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
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

        $scope.showsectionGrid = function (classid) {

            //alert(asmayid)
            //alert(classid)


            var data = {
                "classid": classid,
                "ASMAY_Id": $scope.asmaY_Id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Clg_HODEmpSalary/Getsectioncount", data).
                then(function (promise) {



                    $scope.fillsectioncount = promise.fillsectioncount;





                })

        }

        $scope.loadcharts = function () {
            var total = 0;
            var total1 = 0;


            if ($scope.grapharray != null) {

                for (var i = 0; i < $scope.grapharray.length; i++) {
                    $scope.regularstdtotal.push({ label: $scope.grapharray[i].month1, "y": $scope.grapharray[i].stud_count })
                }
            }





            var chart = new CanvasJS.Chart("areachart",
                {
                    width: 1070,
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



        }




        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.regularstdtotal = [];
            $scope.studentstrenthgr = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var a = $scope.asmaY_Id;
                // alert(asmaY_Id)
                $scope.fields();

                apiService.getURI("Clg_HODEmpSalary/yrchange", asmaY_Id).
                    then(function (promise) {
                        $scope.studentstrenth = promise.fillstudentstrenth;
                        $scope.gridOptions.data = $scope.studentstrenth;

                        if ($scope.studentstrenth.length > 0) {


                            $scope.studentstrenthgr = true;

                            // $scope.yearlt = promise.yearlist;
                            $scope.regular = promise.sectionwisestrenth;

                            $scope.hrmlY_Id = promise.hrmlY_Id;

                            $scope.getgraphdata();

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


            //Graph Data

            $scope.exam_list = [];
            $scope.overalltotalmax = 0;
            angular.forEach($scope.studentstrenth, function (st) {
                if ($scope.exam_list.length == 0) {
                    $scope.exam_list.push({ IVRM_Month_Id: st.IVRM_Month_Id, month1: st.month1 });
                }
                else if ($scope.exam_list.length > 0) {
                    var al_exm_cnt = 0;
                    angular.forEach($scope.exam_list, function (exm) {
                        if (exm.IVRM_Month_Id == st.IVRM_Month_Id) {
                            al_exm_cnt += 1;
                        }
                    })
                    if (al_exm_cnt == 0) {

                        $scope.exam_list.push({ IVRM_Month_Id: st.IVRM_Month_Id, month1: st.month1 });
                    }
                }
            })


            $scope.grapharray = [];
            angular.forEach($scope.exam_list, function (kk) {
                $scope.branchcnt = 0;
                angular.forEach($scope.studentstrenth, function (xx) {
                    if (kk.IVRM_Month_Id == xx.IVRM_Month_Id) {
                        $scope.branchcnt += xx.netamount;
                    }

                })
                $scope.grapharray.push({ IVRM_Month_Id: kk.IVRM_Month_Id, month1: kk.month1, stud_count: $scope.branchcnt })
            })


        }


        $scope.OnClass = function (asmcL_Id) {

            var a = $scope.asmcL_Id;
            // alert(asmaY_Id)
            $scope.fields();
            var data = {
                "classid": asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Clg_HODEmpSalary/Getsection", data).
                then(function (promise) {


                    // $scope.sectioncount = promise.fillstudentstrenth;
                    // $scope.regular = promise.sectionwisestrenth;
                    // alert($scope.regular)
                    // $scope.yearlt = promise.yearlist;
                    // $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
                    //$scope.regstdtotal = promise.fillregstd;
                    //$scope.newadmstdtotal = promise.fillnewadmstd;
                    //$scope.asmaY_Id = promise.asmaY_Id;
                    //$scope.newadmstdgraph = promise.newadmstd;
                    //$scope.year = promise.yearlist[0].asmaY_Year;

                    //
                    //$scope.classarray = promise.classarray;
                    //$scope.sectionarray = promise.sectionarray;
                    //$scope.newadmit = promise.sectionwisestrenth;

                    $scope.loadcharts();

                })


        }




    };
})();