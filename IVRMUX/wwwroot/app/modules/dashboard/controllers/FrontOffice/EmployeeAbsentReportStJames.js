(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeAbsentReportStJamesController', EmployeeAbsentReportStJamesController)

    EmployeeAbsentReportStJamesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function EmployeeAbsentReportStJamesController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {

        $scope.maxDatemf = new Date();

        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("EmployeeMonthlyReport/getalldetails", id).
                then(function (promise) {
                    $scope.staff_types = promise.filltypes;
                    $scope.All_Individual('All');
                    $scope.rdoPunch = "punch";
                    $scope.grid_view = false;
                });
        };

        $scope.gettodate = function () {
            $scope.minDatemf = new Date($scope.fromdate);
            $scope.maxDatemf = new Date();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.staff_types.some(function (options) {
                return options.selected;
            });
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.Department_types.some(function (options) {
                return options.selected;
            });
        };

        $scope.isOptionsRequired2 = function () {
            return !$scope.Designation_types.some(function (options) {
                return options.selected;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.All_Individual = function () {
            if ($scope.allind == 'Indi')
                $scope.disabledata = false;
            else
                $scope.disabledata = true;
        };

        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_departments();
        };

        $scope.all_checkdep = function () {
            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.Department_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_designation();
        };

        $scope.all_checkdesg = function () {
            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        };

        $scope.get_departments = function () {
            $scope.usercheck = $scope.staff_types.every(function (options) {
                return options.selected;
            });
            $scope.deptcheck = "";
            $scope.desgcheck = "";

            var groupidss;
            for (var i = 0; i < $scope.staff_types.length; i++) {
                if ($scope.staff_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.staff_types[i].hrmgT_Id;
                    else
                        groupidss = groupidss + "," + $scope.staff_types[i].hrmgT_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipletype": groupidss
                };
                apiService.create("EmployeeMonthlyReport/get_departments", data).
                    then(function (promise) {

                        $scope.Department_types = promise.filldepartment;
                        if ($scope.Department_types.length > 0) {
                            // $scope.Department_types[0].selected = true;
                            $scope.get_designation();
                        }
                    });
            }
            else {
                $scope.Department_types = "";
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        };

        $scope.get_designation = function () {
            $scope.deptcheck = $scope.Department_types.every(function (options) {
                return options.selected;
            });
            $scope.get_designationnew();
        };

        $scope.get_designationnew = function () {
            $scope.desgcheck = "";
            var groupidss;
            for (var i = 0; i < $scope.Department_types.length; i++) {
                if ($scope.Department_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.Department_types[i].hrmD_Id;
                    else
                        groupidss = groupidss + "," + $scope.Department_types[i].hrmD_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledep": groupidss
                };
                apiService.create("EmployeeMonthlyReport/get_designation", data).
                    then(function (promise) {
                        $scope.Designation_types = promise.filldesignation;
                        if ($scope.Designation_types.length > 0) {
                            //  $scope.Designation_types[0].selected = true;
                            $scope.get_employee();
                        }
                    });
            }
            else {
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        };

        $scope.get_employee = function () {
            $scope.desgcheck = $scope.Designation_types.every(function (options) {
                return options.selected;
            });
            $scope.get_employeenew();
        };

        $scope.get_employeenew = function () {
            var typeIds;
            for (var i = 0; i < $scope.staff_types.length; i++) {
                if ($scope.staff_types[i].selected == true) {
                    if (typeIds == undefined)
                        typeIds = $scope.staff_types[i].hrmgT_Id;
                    else
                        typeIds = typeIds + "," + $scope.staff_types[i].hrmgT_Id;
                }
            }

            var deptIds;
            for (var i = 0; i < $scope.Department_types.length; i++) {
                if ($scope.Department_types[i].selected == true) {
                    if (deptIds == undefined)
                        deptIds = $scope.Department_types[i].hrmD_Id;
                    else
                        deptIds = deptIds + "," + $scope.Department_types[i].hrmD_Id;
                }
            }

            var groupidss;
            for (var i = 0; i < $scope.Designation_types.length; i++) {
                if ($scope.Designation_types[i].selected == true) {
                    if (groupidss == undefined)
                        groupidss = $scope.Designation_types[i].hrmdeS_Id;
                    else
                        groupidss = groupidss + "," + $scope.Designation_types[i].hrmdeS_Id;
                }
            }

            if (groupidss != undefined) {
                var data = {
                    "multipledes": groupidss,
                    "multipletype": typeIds,
                    "multipledep": deptIds
                };
                apiService.create("EmployeeMonthlyReport/get_employee", data).
                    then(function (promise) {

                        $scope.Employeelst = promise.fillemployee;
                        //if ($scope.Employeelst.length > 0) {
                        //   // $scope.hideempse = false;
                        //   // $scope.Employeelst[0].Selected = true;
                        //    $scope.hrmE_Id = $scope.Employeelst[0].hrmE_Id;
                        //}
                    });
            }
            else {
                $scope.Employeelst = "";
            }
        };

        $scope.submitted = false;
        $scope.temp_list = [];
        $scope.GetReport = function () {

            $scope.temp_list = [];
            if ($scope.myForm.$valid) {
                var groupidss;
                if ($scope.allind == "All") {
                    for (var i = 0; i < $scope.Employeelst.length; i++) {
                        if (groupidss == undefined)
                            groupidss = $scope.Employeelst[i].hrmE_Id;
                        else
                            groupidss = groupidss + "," + $scope.Employeelst[i].hrmE_Id;
                    }
                }
                else {
                    groupidss = $scope.hrmE_Id;
                }
                if (groupidss != undefined) {

                    var fromdate1 = $filter('date')($scope.fromdate, "yyyy-MM-dd");
                    var todate1 = $filter('date')($scope.todate, "yyyy-MM-dd");
                    var data;
                    data = {
                        "fromdate": fromdate1,
                        "todate": todate1,
                        "multiplehrmeid": groupidss
                    };

                    apiService.create("EmployeeMonthlyReport/getrptStJames", data).
                        then(function (promise) {

                            if (promise.filldata.length > 0) {
                                $scope.grid_view = true;
                                $scope.absentreport = promise.filldata;
                                var totalAbsent = 1;
                                $scope.absentList = [];
                                for (var i = 0; i < $scope.absentreport.length; i++) {

                                    $scope.temp_list.push({
                                        "WorkDate": $scope.absentreport[i].WorkDate, "EmpCode": $scope.absentreport[i].EmpCode, "EmployeeName": $scope.absentreport[i].EmployeeName, "Department": $scope.absentreport[i].Department, "Designation": $scope.absentreport[i].Designation, "Status": $scope.absentreport[i].Status, "absentcount": totalAbsent
                                    });
                                    
                                    if (i < $scope.absentreport.length - 1) {
                                        if ($scope.absentreport[i].HRME_Id == $scope.absentreport[i + 1].HRME_Id) {
                                            totalAbsent = totalAbsent + 1;
                                            continue;
                                        }
                                    }
                                    totalAbsent = 1;
                                }
                                console.log($scope.temp_list);
                            }
                            else {
                                $scope.grid_view = false;
                                swal("Record not found.");
                            }
                        });
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var divToPrint = document.getElementById("table");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        };

        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("table");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "EmployeeAbsentReport.xls");
        };

        $scope.clearid = function () {
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.submitted = false;
            $scope.Department_types = "";
            $scope.Designation_types = "";
            $scope.Employeelst = "";
            $scope.hrmE_Id = "";
            $scope.usercheck = "";
            $scope.deptcheck = "";
            $scope.desgcheck = "";
            $scope.allind = "All";
            $scope.fromdate = "";
            $scope.todate = "";

            $scope.disabledata = true;
            $scope.grid_view = false;
            for (var i = 0; i < $scope.staff_types.length; i++) {
                $scope.staff_types[i].selected = false;
            }
        };
    }

})();