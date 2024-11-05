
(function () {
    'use strict';
    angular
.module('app')
        .controller('ClgTimeTableStaffWiseReportController', ClgTimeTableStaffWiseReportController)

    ClgTimeTableStaffWiseReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function ClgTimeTableStaffWiseReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.staffName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.editEmployee = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.grid_view = false;
        $scope.rmmtype = false;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };


        $scope.isOptionsRequired = function () {

            return !$scope.staff_list.some(function (options) {
                return options.stf;
            });
        };

        $scope.onclickloaddata = function () {

            $scope.grid_view = false;
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.roomcheck = function () {
            $scope.getreportdata = [];
            $scope.grid_view = false;

        };
        // TO Save The Data
        $scope.submitted = false;
        $scope.getreportdata = [];

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
        $scope.GetStaffDetails = function () {
            $scope.rpttyp = "SAWC";
            var data = {
                "TYPE": $scope.rpttyp,
                "RMFLG": $scope.rmmtype
            };
            apiService.create("CLGTTStaffWiseReport/GetStaffDetails", data).
                then(function (promise) {
                    $scope.albumNameArray1 = [];
                    $scope.period_list = promise.periodslst;
                    $scope.day_list = promise.gridweeks;
                    $scope.employeedetails = promise.employeedetails[0];
                    $scope.empmobileno = promise.empmobileno;
                    $scope.empemailid = promise.empemailid;
                    $scope.getreportdata = promise.getreportdata;

                    $scope.albumNameArray1.push({hrmE_Id: $scope.employeedetails.HRME_Id, staffName: $scope.employeedetails.HRME_EmployeeFirstName});

                    if ($scope.getreportdata.length > 0) {
                        angular.forEach($scope.staff_list, function (role) {
                            if (role.stf) $scope.albumNameArray1.push(role);
                        });

                        $scope.grid_view = true;
                        $scope.mainlist = [];
                        angular.forEach($scope.albumNameArray1, function (ss) {
                            $scope.daymainlist = [];
                            angular.forEach($scope.day_list, function (dd) {
                                $scope.periodsmainlist = [];
                                angular.forEach($scope.period_list, function (pp) {
                                    angular.forEach($scope.getreportdata, function (tt) {
                                        if (ss.hrmE_Id == tt.HRME_Id && dd.ttmD_Id == tt.TTMD_Id && pp.ttmP_Id == tt.TTMP_Id) {
                                            $scope.periodsmainlist.push(tt);
                                        }
                                    });
                                });
                                $scope.daymainlist.push({ TTMD_Id: dd.ttmD_Id, TTMD_DayName: dd.ttmD_DayName, periodlst: $scope.periodsmainlist });
                            });
                            $scope.mainlist.push({ HRME_Id: ss.hrmE_Id, EMPNAME: ss.staffName, daywiselist: $scope.daymainlist });
                        });
                        console.log($scope.mainlist);
                    }
                    else {
                        swal("TimeTable is Not Generated For Selected Details !!!!");
                        $scope.grid_view = false;
                    }
                });
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.staff_list.every(function (options) {
                return options.stf;
            });
        };

        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_list, function (itm) {
                itm.stf = toggleStatus;
            });
        };

        //TO clear  data
        $scope.clearid = function () {
            $scope.asmaY_Id = "";
            $scope.ttmC_Id = "";
            $scope.hrmE_Id = "";
            $scope.usercheck = false;
            // $scope.stf = false;
            $scope.all_check();
            $scope.class_list = $scope.temp_classlist;
            $scope.grid_view = false;
            $scope.submitted = false;
            $scope.datareport = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };
    }

})();