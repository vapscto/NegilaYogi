
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGTTStaffWiseReportController', CLGTTStaffWiseReportController)

    CLGTTStaffWiseReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function CLGTTStaffWiseReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {
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
        }

        $scope.onclickloaddata = function () {

            $scope.grid_view = false;
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.roomcheck = function () {
            $scope.getreportdata = [];
            $scope.grid_view = false;

        }
        // TO Save The Data
        $scope.submitted = false;
        $scope.getreportdata = [];
        $scope.GetReport = function () {
            $scope.getreportdata = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.albumNameArray1 = [];


                angular.forEach($scope.staff_list, function (role) {
                    if (role.stf) $scope.albumNameArray1.push(role);
                })
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TYPE": $scope.rpttyp,
                    "RMFLG": $scope.rmmtype,
                    staffarray: $scope.albumNameArray1,
                }
                apiService.create("CLGTTStaffWiseReport/getrpt", data).
                    then(function (promise) {

                        $scope.getreportdata = promise.getreportdata;

                        if ($scope.getreportdata.length>0) {
                            $scope.grid_view = true;
                            $scope.mainlist = [];
                            angular.forEach($scope.albumNameArray1, function (ss) {
                                $scope.daymainlist = [];
                                angular.forEach($scope.day_list, function (dd) {
                                    $scope.periodsmainlist = [];
                                    angular.forEach($scope.period_list, function (pp) {
                                      
                                        angular.forEach($scope.getreportdata, function (tt) {
                                            debugger;
                                            if (ss.hrmE_Id == tt.HRME_Id && dd.ttmD_Id == tt.TTMD_Id && pp.ttmP_Id == tt.TTMP_Id) {
                                                $scope.periodsmainlist.push(tt);

                                            }

                                        })



                                    })


                                    $scope.daymainlist.push({ TTMD_Id: dd.ttmD_Id, TTMD_DayName: dd.ttmD_DayName, periodlst: $scope.periodsmainlist })


                                })

                                $scope.mainlist.push({ HRME_Id: ss.hrmE_Id, EMPNAME: ss.staffName, daywiselist: $scope.daymainlist});




                            })

                            console.log($scope.mainlist);

                        }
                       else {
                          swal("TimeTable is Not Generated For Selected Details !!!!");
                            $scope.grid_view = false;
                        }


                        //if (promise.tt != null && promise.tt != "" && promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "") {
                        //    $scope.table_list_sub_wise = [];
                        //    // $scope.table_headers = [];

                        //    for (var a = 0; a < $scope.albumNameArray1.length; a++) {
                        //        $scope.grid_view = true;
                        //        $scope.period_list = promise.periodslst;
                        //        $scope.day_list = promise.gridweeks;
                        //        $scope.tt_list = promise.tt;
                        //        var temp_array = [];
                        //        $scope.table_list = [];
                        //        for (var j = 0; j < $scope.day_list.length; j++) {
                        //            temp_array = [];
                        //            for (var i = 0; i < $scope.period_list.length; i++) {
                        //                var count = 0;
                        //                var newCol = "";
                        //                for (var k = 0; k < $scope.tt_list.length; k++) {

                        //                    if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id && $scope.tt_list[k].hrmE_Id == $scope.albumNameArray1[a].hrmE_Id) {
                        //                        if (count == 0) {
                        //                            newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value2: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value_: $scope.tt_list[k].ismS_SubjectName }
                        //                            count += 1;
                        //                        }
                        //                        else if (count > 0) {
                        //                            newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                        //                            newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                        //                            newCol.value2 = newCol.value2 + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName;
                        //                            count += 1;
                        //                        }
                        //                    }

                        //                }
                        //                if (newCol == "") {
                        //                    newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: ' ', pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                        //                }
                        //                temp_array.push(newCol);
                        //                newCol = "";
                        //                count = 0;
                        //            }

                        //            $scope.table_list.push(temp_array);
                        //        }
                        //        $scope.table_list_sub_wise.push({ array: $scope.table_list, header: $scope.albumNameArray1[a].staffName, id: $scope.albumNameArray1[a].hrmE_Id });
                        //    }
                        //}
                        //else {
                        //    swal("TimeTable is Not Generated For Selected Details !!!!");
                        //    $scope.grid_view = false;
                        //}
                    })
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
        }
        $scope.exptoex = function () {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            
            $scope.rpttyp = "SAWC";
            $scope.all_check();

            apiService.getDATA("CLGTTStaffWiseReport/getdetails").
       then(function (promise) {
           $scope.year_list = promise.acayear;
           $scope.staff_list = promise.stafflist;
           $scope.period_list = promise.periodslst;
              $scope.day_list = promise.gridweeks;
          

       })
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.staff_list.every(function (options) {
                return options.stf;
            });
        }
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_list, function (itm) {
                itm.stf = toggleStatus;
            });
            //if ($scope.usercheck == "1") {
            //    angular.forEach($scope.staff_list, function (role) {
            //        role.stf = true;
            //    })
            //    $scope.stf_flag = true;
            //}
            //else if ($scope.usercheck == "0") {
            //    angular.forEach($scope.staff_list, function (role) {
            //        role.stf = false;
            //    })
            //    $scope.stf_flag = false;
            //}
        }

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