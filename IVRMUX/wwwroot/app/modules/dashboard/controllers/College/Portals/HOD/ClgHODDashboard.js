
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgHODDashboardController', ClgHODDashboardController)

    ClgHODDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'uiGridGroupingConstants']
    function ClgHODDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiGridGroupingConstants) {


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






        $scope.tempcldrlst = [];
        $scope.loaddata = function () {
            $scope.fields();
            apiService.getDATA("ClgHODDashboard/Getdetails").then(function (promise) {

                //===================================== STUDENT Details
                $scope.acayrlst = promise.yearlist;
                $scope.staffdetails = promise.studentdetails;
                //if ($scope.staffdetails.length > 0) {
                //    $scope.staffname = $scope.staffdetails[0].hrmE_EmployeeFirstName;
                //    $scope.department = $scope.staffdetails[0].hrmD_DepartmentName;
                //    $scope.email = $scope.staffdetails[0].amst_email_id;
                //    $scope.mobileno = $scope.staffdetails[0].amst_mobile;
                //    $scope.empcode = $scope.staffdetails[0].hrmE_EmployeeCode;
                  
                //}
              
                $scope.studentstrenth = promise.fillstudentstrenth; 
                $scope.gridOptions.data = $scope.studentstrenth;
                if ($scope.studentstrenth.length > 0) {
                    $scope.studentstrenthgr = true;

                    $scope.regular = promise.sectionwisestrenth;

                    $scope.hrmlY_Id = promise.hrmlY_Id;
                    
                    $scope.getgraphdata();

                    $scope.loadcharts();

                }
                //else {
                //    swal("No Record Found")
                //}

                $scope.stdabsentlist = promise.saved_hods_stf;
                if (promise.filldepartment.length > 0) {
                    $scope.empDetails = promise.filldepartment;
                    $scope.HRME_EmployeeFirstName = $scope.empDetails[0].hrmE_EmployeeFirstName;
                    $scope.HRME_DOJ = $scope.empDetails[0].hrmE_DOJ;
                    $scope.HRMD_DepartmentName = $scope.empDetails[0].hrmD_DepartmentName;
                    $scope.HRMDES_DesignationName = $scope.empDetails[0].hrmdeS_DesignationName;
                    $scope.HRME_EmployeeCode = $scope.empDetails[0].hrmE_EmployeeCode;
                    $scope.HRME_DOB = $scope.empDetails[0].hrmE_DOB;
                    $scope.photo = $scope.empDetails[0].hrmE_PhotoNo;
                    $scope.coedata = promise.coedata;
                    $scope.studentcount = promise.studentcount;
                }
                if (promise.mobile.length > 0) {
                    $scope.HRME_MobileNo = promise.mobile[0].hrmE_MobileNo;
                }
                if (promise.email.length > 0) {
                    $scope.HRME_EmailId = promise.email[0].hrmE_EmailId;
                }

                if ($scope.coedata != "" && $scope.coedata != null) {
                    if ($scope.coedata.length > 0) {
                        $scope.hidecoe = true;
                    }
                }
                else {
                    $scope.hidecoe = false;
                }
                //for salary count
                var totalsalary = 0;
                if (promise.salarylist.length > 0) {
                    $scope.salary_list = promise.salarylist;
                    for (var i = 0; i < $scope.salary_list.length; i++) {
                        totalsalary += $scope.salary_list[i].salary;
                    }

                }

                $scope.SalaryC = totalsalary;







            });
        }


        var HostName = location.host;
        $scope.showStudent = function () {

            $window.location.href = 'http://' + HostName + '/#/app/StudentGeneralRegister/4';

        };




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


        $scope.gridOptions = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: true,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'month1', displayName: 'MONTH', grouping: { groupPriority: 0 }, width: '24%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'HRMD_DepartmentName', displayName: 'Department Name', grouping: { groupPriority: 1 }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'HRMDES_DesignationName', displayName: 'Designation Name', grouping: { groupPriority: 2 }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

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

      
    };
})();

