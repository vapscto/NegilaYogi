
(function () {
    'use strict';
    angular
.module('app')
        .controller('ClgStudentTimeTableReportController', ClgStudentTimeTableReportController)

    ClgStudentTimeTableReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', 'Excel', '$timeout', '$http', '$q', '$stateParams', '$filter']
    function ClgStudentTimeTableReportController($rootScope, $scope, $state, $location, apiService, Flash, Excel, $timeout, $http, $q, $stateParams, $filter) {

        $scope.GetStudentDetails = function () {
            var pageid = 2;
            apiService.getURI("CLGTTCourseWiseReport/GetStudentDetails", pageid).then(function (promise) {
                $scope.studentdetails = promise.studentdetails[0];
                $scope.time_Table = promise.time_Table;
                $scope.TT_Break_list_all = promise.tT_Break_list_all;
                $scope.allday = promise.allday;
                $scope.periodslst = promise.periodslst;

                if ($scope.time_Table.length > 0 && $scope.allday.length > 0) {
                    $scope.grid_view = true;

                    $scope.distcourselist = [];
                    angular.forEach($scope.time_Table, function (st) {
                        if ($scope.distcourselist.length == 0) {
                            $scope.distcourselist.push({ AMCO_Id: st.AMCO_Id, AMB_Id: st.AMB_Id, AMSE_Id: st.AMSE_Id, ACMS_Id: st.ACMS_Id, CRSDETAILS: st.CRSDETAILS });
                        }
                        else if ($scope.distcourselist.length > 0) {
                            var cntt = 0;
                            angular.forEach($scope.distcourselist, function (exm) {
                                if (exm.AMCO_Id == st.AMCO_Id && exm.AMB_Id == st.AMB_Id && exm.AMSE_Id == st.AMSE_Id && exm.ACMS_Id == st.ACMS_Id) {
                                    cntt += 1;
                                }
                            });
                            if (cntt == 0) {
                                $scope.distcourselist.push({ AMCO_Id: st.AMCO_Id, AMB_Id: st.AMB_Id, AMSE_Id: st.AMSE_Id, ACMS_Id: st.ACMS_Id, CRSDETAILS: st.CRSDETAILS });
                            }
                        }
                    });

                    $scope.temparray = [];
                    angular.forEach($scope.distcourselist, function (yy) {
                        $scope.mainperiodlist = [];
                        var ct = 0;
                        angular.forEach($scope.periodslst, function (xx) {
                            if ($scope.TT_Break_list_all.length > 0) {
                                var cnt = 0;
                                angular.forEach($scope.TT_Break_list_all, function (zz) {
                                    if (yy.AMCO_Id == zz.amcO_Id && yy.AMB_Id == zz.amB_Id && yy.AMSE_Id == zz.amsE_Id) {
                                        ct += 1;
                                        if (xx.ttmP_PeriodName == zz.ttmbC_AfterPeriod) {
                                            cnt += 1;
                                            $scope.mainperiodlist.push({ TTMP_Id: xx.ttmP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.ttmP_PeriodName });
                                            $scope.mainperiodlist.push({ TTMP_Id: zz.ttmbC_BreakName, TTMP_PeriodName: 'BREAK' });
                                        }
                                        else {

                                        }
                                    }
                                });
                                if (cnt == 0 && ct > 0) {
                                    $scope.mainperiodlist.push({ TTMP_Id: xx.ttmP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.ttmP_PeriodName });
                                }
                                if (ct == 0) {
                                    $scope.mainperiodlist.push({ TTMP_Id: xx.ttmP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.ttmP_PeriodName });
                                }
                            }
                            else {
                                $scope.mainperiodlist.push({ TTMP_Id: xx.ttmP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.ttmP_PeriodName });
                            }
                        });
                        yy.perlist = $scope.mainperiodlist;
                    });

                    angular.forEach($scope.distcourselist, function (ww) {
                        $scope.daylst = [];
                        angular.forEach($scope.allday, function (dd) {
                            $scope.periodlst = [];
                            if (ww.AMCO_Id == dd.amcO_Id && ww.AMB_Id == dd.amB_Id && ww.AMSE_Id == dd.amsE_Id) {
                                angular.forEach($scope.time_Table, function (tt) {
                                    if (ww.AMCO_Id == tt.AMCO_Id && ww.AMB_Id == tt.AMB_Id && ww.AMSE_Id == tt.AMSE_Id && ww.ACMS_Id == tt.ACMS_Id && dd.ttmD_Id == tt.TTMD_Id) {
                                        $scope.periodlst.push(tt);
                                    }
                                });
                                $scope.daylst.push({ TTMD_Id: dd.ttmD_Id, TTMD_DayName: dd.ttmD_DayName, PERIODS: $scope.periodlst });
                            }
                        });
                        ww.daylist = $scope.daylst;
                    });
                    console.log($scope.distcourselist);
                }
                else {
                    swal("TimeTable is Not Generated For Selected Details !!!!");
                    $scope.grid_view = false;
                }
            });
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsE_SEMName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.searchchkbx1 = "";
        $scope.filterchkbx1 = function (obj) {
            return angular.lowercase(obj.acmS_SectionName).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
        };

        $scope.editEmployee = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.grid_view = false;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.semisterlist.every(function (options) {
                return options.class;
            });
        };

        $scope.togchkbx1 = function () {
            $scope.usercheck1 = $scope.section_list.every(function (options) {
                return options.sec;
            });
        };

        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.semisterlist, function (itm) {
                itm.class = toggleStatus;
            });
        };

        $scope.all_check1 = function () {
            var toggleStatus = $scope.usercheck1;
            angular.forEach($scope.section_list, function (itm) {
                itm.sec = toggleStatus;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.semisterlist.some(function (options) {
                return options.class;
            });
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.section_list.some(function (options) {
                return options.sec;
            });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.time_Table = [];
        $scope.TT_Break_list_all = [];
        $scope.allday = [];
        $scope.submitted = false;
    
        $scope.printData = function () {
            var divToPrint = document.getElementById("table");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        };

        $scope.exptoex = function () {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        };
        //TO  GEt The Values iN Grid

        //to get class by category      
        $scope.ttmC_Id = "";
        $scope.asmaY_Id = "";

        //TO clear  data
        $scope.clearid = function () {
            $state.reload();
        };
    }

})();