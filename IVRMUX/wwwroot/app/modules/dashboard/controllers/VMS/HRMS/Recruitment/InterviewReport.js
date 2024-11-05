(function () {
    'use strict';
    angular
        .module('app')
        .controller('InterviewReportController', InterviewReportController);

    InterviewReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout'];
    function InterviewReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, Excel, $timeout) {

        $scope.maxDatemf = new Date();

        $scope.hrcisC_InterviewRounds = "0";
        $scope.hrcmG_Id = "0";
        //loading start
        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("AddCandidateInterviewVMS/getallgrade", id).
                then(function (promise) {
                    if (promise.gradelist !== null && promise.gradelist.length > 0) {
                        $scope.gradelist = promise.gradelist;
                    }
                });
        };
        //loading end
        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.fromdate);
            $scope.maxDatemf = new Date();
        };
        //validation start
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
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
        //validation end
        //all-individual start
        $scope.All_Individual = function () {

            if ($scope.allind == 'Indi')
                $scope.disabledata = false;
            else
                // $scope.hrmE_Id = "";
                $scope.disabledata = true;

        };
        //all-individual end    

        //all check button start
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
        //all-check button end
        // fill dep start
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
                    "multipletype": groupidss,
                };
                apiService.create("EmployeeInOutReport/get_departments", data).
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
        //fill department end
        //fill desg start
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
                    "multipledep": groupidss,
                };
                apiService.create("EmployeeInOutReport/get_designation", data).
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
        //fill desg end
        //fill employee start
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
                apiService.create("EmployeeInOutReport/get_employee", data).
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

        $scope.viewfunction = function () {
            $scope.grid_view = false;
            $scope.upcomingintvw = [];
            $scope.inprogressintvw = [];
            $scope.completedintvw = [];
        };

        //fill employee end
        //get report start
        $scope.grid_view = false;
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.upcomingintvw = [];
            if ($scope.myForm.$valid) {
                if ($scope.fromdate != undefined && $scope.fromdate != "") {
                    $scope.fromdate = new Date($scope.fromdate).toDateString();
                }
                else {
                    $scope.fromdate = "";
                }

                if ($scope.todate != undefined && $scope.todate != "") {
                    $scope.todate = new Date($scope.todate).toDateString();
                }
                else {
                    $scope.todate = "";
                }

                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "rdotype": $scope.rdopunch,
                    "HRCISC_InterviewRounds": $scope.hrcisC_InterviewRounds,
                    "HRCMG_Id": $scope.hrcmG_Id
                };
                apiService.create("AddCandidateInterviewVMS/getrpt", data).
                    then(function (promise) {                        
                            if (promise.upcomingintvw.length !== 0 && promise.upcomingintvw !== null) {
                                $scope.upcomingintvw = promise.upcomingintvw;
                                $scope.grid_view = true;
                            }
                            else {
                                swal("Record not found.");
                                $scope.grid_view = false;
                            }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        //get report end
        //print start
        $scope.printData = function () {
            var id ="table";           
            var innerContents = document.getElementById(id).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        //print end
        //export start

        $scope.exptoex = function () {
            var divToPrint = document.getElementById("table");         

            var exportHref = Excel.tableToExcel(divToPrint, divToPrint);
            $timeout(function () {
                location.href = exportHref;
            }, 100);

            //var divToPrint = document.getElementById("table");
            //var blob = new Blob([divToPrint.outerHTML], {
            //    type: "application/vnd.ms-excel;charset=utf-8"
            //});
            //saveAs(blob, "InterviewReport.xls");
        };
        //export end
        //TO clear  data
        $scope.clearid = function () {
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            //$scope.submitted = false;
            //$scope.Department_types = "";
            //$scope.Designation_types = "";
            //$scope.Employeelst = "";
            //$scope.hrmE_Id = "";
            //$scope.usercheck = "";
            //$scope.deptcheck = "";
            //$scope.desgcheck = "";
            //$scope.allind = "All";
            //$scope.fromdate = "";
            //$scope.todate = "";

            //$scope.disabledata = true;
            //$scope.grid_view = false;
            //for (var i = 0; i < $scope.staff_types.length; i++) {
            //    $scope.staff_types[i].selected = false;
            //}
            $state.reload();
        };
        //clear end


    }

})();