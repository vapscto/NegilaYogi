(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeInOutReportController', EmployeeInOutReportController)

    EmployeeInOutReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function EmployeeInOutReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.maxDatemf = new Date();
        //loading start
        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("EmployeeInOutReport/getalldetails", id).
                then(function (promise) {

                    $scope.staff_types = promise.filltypes;
                    $scope.All_Individual();
                    $scope.grid_view = false;
                })
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
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.Department_types.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired2 = function () {
            return !$scope.Designation_types.some(function (options) {
                return options.selected;
            });
        }
        $scope.sort = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //validation end
        //all-individual start
        $scope.All_Individual = function () {

            if ($scope.allind == 'Indi')
                $scope.disabledata = false;
            else
                // $scope.hrmE_Id = "";
                $scope.disabledata = true;

        }
        //all-individual end    

        //all check button start
        $scope.all_check = function () {

            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_departments();
        }
        $scope.all_checkdep = function () {

            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.Department_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_designation();
        }
        $scope.all_checkdesg = function () {

            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        }
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
                }
                apiService.create("EmployeeInOutReport/get_departments", data).
                    then(function (promise) {

                        $scope.Department_types = promise.filldepartment;
                        if ($scope.Department_types.length > 0) {
                            // $scope.Department_types[0].selected = true;
                            $scope.get_designation();
                        }

                    })
            }
            else {
                $scope.Department_types = "";
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        }
        //fill department end
        //fill desg start
        $scope.get_designation = function () {
            $scope.deptcheck = $scope.Department_types.every(function (options) {
                return options.selected;
            });

            $scope.get_designationnew();
        }
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
                }
                apiService.create("EmployeeInOutReport/get_designation", data).
                    then(function (promise) {

                        $scope.Designation_types = promise.filldesignation;
                        if ($scope.Designation_types.length > 0) {
                            //  $scope.Designation_types[0].selected = true;
                            $scope.get_employee();
                        }
                    })
            }
            else {
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        }
        //fill desg end
        //fill employee start
        $scope.get_employee = function () {
            $scope.desgcheck = $scope.Designation_types.every(function (options) {

                return options.selected;
            });

            $scope.get_employeenew();
        }
        $scope.fromdatechange = function (fromdate) {
            $scope.tomaxDate = new Date(fromdate.getFullYear(), fromdate.getMonth(), fromdate.getDate() + 31);
            $scope.minDatemf = new Date(fromdate.getFullYear(), fromdate.getMonth(), fromdate.getDate());
        }

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
                }
                apiService.create("EmployeeInOutReport/get_employee", data).
                    then(function (promise) {

                        $scope.Employeelst = promise.fillemployee;
                        //if ($scope.Employeelst.length > 0) {
                        //   // $scope.hideempse = false;
                        //   // $scope.Employeelst[0].Selected = true;
                        //    $scope.hrmE_Id = $scope.Employeelst[0].hrmE_Id;
                        //}
                    })
            }
            else {
                $scope.Employeelst = "";
            }
        }

        //fill employee end
        //get report start
        $scope.submitted = false;
        $scope.GetReport = function () {

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
                    //var fromdate1 = $scope.fromdate == null ? "" : $filter('date')($scope.fromdate, "dd-MM-yyyy");
                    //var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "dd-MM-yyyy");

                    var fromdate1 = $scope.fromdate == null ? "" : $filter('date')($scope.fromdate, "yyyy-MM-dd");                    var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");

                    var data = {

                        "fromdate": fromdate1,
                        "todate": todate1,
                        "multiplehrmeid": groupidss
                    }
                    apiService.create("EmployeeInOutReport/getrpt", data).then(function (promise) {

                            if (promise.filldata.length > 0) {
                                $scope.grid_view = true;
                                $scope.head1 = false;
                                $scope.head2 = false;
                                if ($scope.rdopunch == "timein") {
                                    $scope.rhead = "Time In";
                                    $scope.head1 = true;
                                }
                                else if ($scope.rdopunch == "timeout") {
                                    $scope.rhead = "Time Out";
                                    $scope.head2 = true;
                                }
                                else {
                                    $scope.rhead = "In Out";
                                    $scope.head1 = true;
                                    $scope.head2 = true;
                                }

                                $scope.columnnames = promise.columnnames;
                                $scope.columnnum = $scope.columnnames.length + 5;
                                var temp_array = [];
                                var temp_array1 = [];
                                for (var i = 0; i < promise.filldata.length; i++) {

                                    var lateinearlyoutcount = $filter('filter')(promise.filldataLIEO, function (d) {
                                        var lateinearlyout = "";
                                        var intoudate = "";
                                        intoudate = promise.filldata[i].foeP_PunchDate == null ? "" : $filter('date')(promise.filldata[i].foeP_PunchDate, "yyyy-MM-dd");                                        lateinearlyout = d.punchdate == null ? "" : $filter('date')(d.punchdate, "yyyy-MM-dd");
                                        return d.HRME_Id == promise.filldata[i].hrmE_Id && lateinearlyout == intoudate && d.punchtime == promise.filldata[i].foepD_PunchTime;
                                    });

                                    var lieocount = 0
                                    if (lateinearlyoutcount.length > 0) {
                                        lieocount = 1;
                                    }
                                    else {
                                        lieocount = 0;
                                    }

                                    var newCol = { punchdate: promise.filldata[i].foeP_PunchDate, punchtime: promise.filldata[i].foepD_PunchTime, foepD_InOutFlg: promise.filldata[i].foepD_InOutFlg, lateinearlyout: lieocount }
                                    temp_array.push(newCol);
                                    if (i < promise.filldata.length - 1) {
                                        if (promise.filldata[i].hrmE_Id == promise.filldata[i + 1].hrmE_Id)
                                            continue;
                                    }
                                    var newCol1 = { pdate: temp_array, hrmE_Id: promise.filldata[i].hrmE_Id, ecode: promise.filldata[i].ecode, ename: promise.filldata[i].ename, hrmdeS_DesignationName: promise.filldata[i].hrmdeS_DesignationName }
                                    temp_array1.push(newCol1);
                                    temp_array = [];

                                }











                                //for (var i = 0; i < promise.filldata.length; i++) {
                                //    var lateinearlyout = "";
                                //    var intoudate = "";
                                //    if (promise.filldataLIEO.length > 0) {
                                //        for (var j = 0; j < promise.filldataLIEO.length; j++) {
                                //            var lateinearlyout = "";
                                //            var intoudate = "";
                                //            intoudate = promise.filldata[i].foeP_PunchDate == null ? "" : $filter('date')(promise.filldata[i].foeP_PunchDate, "yyyy-MM-dd");                                //            lateinearlyout = promise.filldataLIEO[j].punchdate == null ? "" : $filter('date')(promise.filldataLIEO[j].punchdate, "yyyy-MM-dd");
                                //            if (promise.filldataLIEO[j].HRME_Id == promise.filldata[i].hrmE_Id && intoudate == lateinearlyout && promise.filldataLIEO[j].punchtime == promise.filldata[i].foepD_PunchTime) {
                                //                var newCol = { punchdate: promise.filldata[i].foeP_PunchDate, punchtime: promise.filldata[i].foepD_PunchTime, foepD_InOutFlg: promise.filldata[i].foepD_InOutFlg, lateinearlyout: 1 }
                                //                temp_array.push(newCol);
                                //            }
                                //            else {
                                //                var newCol = { punchdate: promise.filldata[i].foeP_PunchDate, punchtime: promise.filldata[i].foepD_PunchTime, foepD_InOutFlg: promise.filldata[i].foepD_InOutFlg, lateinearlyout: 0 }
                                //                temp_array.push(newCol);
                                //            }


                                //        }
                                //    }


                                //    if (i < promise.filldata.length - 1) {
                                //        if (promise.filldata[i].hrmE_Id == promise.filldata[i + 1].hrmE_Id)
                                //            continue;
                                //    }
                                //    var newCol1 = { pdate: temp_array, hrmE_Id: promise.filldata[i].hrmE_Id, ecode: promise.filldata[i].ecode, ename: promise.filldata[i].ename, hrmdeS_DesignationName: promise.filldata[i].hrmdeS_DesignationName }
                                //    temp_array1.push(newCol1);
                                //    temp_array = [];
                                //}








                                $scope.yearlyemprep = temp_array1;
                                //$scope.columnnames = promise.columnnames;
                            }
                            else {
                                $scope.grid_view = false;
                                swal("Record not found.");

                            }
                        })
                }
            }
            else {
                $scope.submitted = true;
            }
        };
        //get report end
        //print start
        $scope.printData = function () {


            var divToPrint = document.getElementById("table");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        }
        //print end
        //export start

        $scope.exptoex = function () {
            var divToPrint = document.getElementById("table");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };
        //export end
        //TO clear  data
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
        //clear end


    }

})();