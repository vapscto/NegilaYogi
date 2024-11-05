
(function () {
    'use strict';
    angular
.module('app')
.controller('StaffReplacementInUnallocatedPeriodController', StaffReplacementInUnallocatedPeriodController)

    StaffReplacementInUnallocatedPeriodController = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function StaffReplacementInUnallocatedPeriodController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.grid_view = false;
        $scope.gridOptions = {};
        $scope.datareport = false;
        $scope.submitted = false;
        $scope.GetReport = function () {


            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "HRME_Id": $scope.hrmE_Id,
                    // "TTMC_Id": $scope.ttmC_Id,
                }
                apiService.create("StaffReplacementInUnallocatedPeriod/getrpt", data).
                    then(function (promise) {
                        if (promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "" && promise.time_Table != null && promise.time_Table != "") {
                            $scope.grid_view = true;
                            $scope.period_list = promise.periodslst;
                            $scope.day_list = promise.gridweeks;
                            $scope.tt_list = promise.time_Table;
                            var temp_array = [];
                            $scope.table_list = [];
                            for (var j = 0; j < $scope.day_list.length; j++) {
                                temp_array = [];
                                for (var i = 0; i < $scope.period_list.length; i++) {

                                    var count = 0;
                                    var newCol = "";
                                    for (var k = 0; k < $scope.tt_list.length; k++) {

                                        if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id) {
                                            if (count == 0) {
                                                newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName }
                                                count += 1;
                                            }
                                            else if (count > 0) {
                                                newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                count += 1;
                                            }
                                        }
                                    }
                                    if (newCol == "") {
                                        newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: 'Empty', pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'white' }
                                    }
                                    temp_array.push(newCol);
                                    newCol = "";
                                    count = 0;
                                }
                                $scope.table_list.push(temp_array);
                            }
                            $scope.datareport = true;
                            var temp_table_list = [];
                            temp_table_list = $scope.table_list;
                            $scope.temp_grid = temp_table_list;
                        }
                        else {
                            swal("TimeTable is Not Generated For Selected Details !!!!");
                            $scope.grid_view = false;
                            $scope.datareport = false;
                        }
                    })
            }
            $scope.staffSDK = "0";
            $scope.subSDK = "0";
            $scope.conSDK = "0";
            $scope.from = "";
            $scope.to = "";
            $scope.day_to_Id = "";
            $scope.period_to_Id = "";
            $scope.day_from_Id = "";
            $scope.period_from_Id = "";
        };
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("StaffReplacementInUnallocatedPeriod/getalldetails").
       then(function (promise) {

           $scope.clearid();
           $scope.academic = promise.academiclist;
           $scope.categorylst = promise.catelist;
           $scope.stafflst = promise.staffDrpDwn;
       })
        };
        //TO clear  data
        $scope.clearid = function () {

            $scope.ttmC_Id = "";
            $scope.hrmE_Id = "";
            $scope.grid_view = false;
            $scope.submitted = false;
            $scope.datareport = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };
        $scope.from = "";
        $scope.to = "";
        $scope.cell_click = function (dayid, periodid, day, period) {
            $scope.table_list = $scope.temp_grid;
            if ($scope.from != null && $scope.from != "") {

                for (var i = 0; i < $scope.table_list.length; i++) {
                    for (var j = 0; j < $scope.table_list[i].length; j++) {
                        if ($scope.table_list[i][j].pedid == periodid && $scope.table_list[i][j].dayid == dayid) {
                            if ($scope.table_list[i][j].background == "green") {
                                if ($scope.day_from_Id === dayid && $scope.period_from_Id === periodid) {
                                    $scope.to = "";
                                    $scope.day_to_Id = "";
                                    $scope.period_to_Id = "";
                                    swal("same day same period selected !!");
                                }
                                else {
                                    $scope.table_list[i][j].background="yellow"
                                    $scope.to = day + "Day & " + period + " period";
                                    $scope.day_to_Id = dayid;
                                    $scope.period_to_Id = periodid;
                                }
                            }
                            else  if ($scope.table_list[i][j].background == "red")
                            {
                                if ($scope.day_from_Id === dayid && $scope.period_from_Id === periodid) {
                                    $scope.to = "";
                                    $scope.day_to_Id = "";
                                    $scope.period_to_Id = "";
                                    swal("same day same period selected !!");
                                }
                                else {
                                    $scope.table_list[i][j].background = "yellow"
                                    $scope.to = day + "Day & " + period + " period";
                                    $scope.day_to_Id = dayid;
                                    $scope.period_to_Id = periodid;
                                }
                            }
                        }
                    }
                }
            }
            else {
                $scope.from = day + "Day & " + period + " period";
                $scope.day_from_Id = dayid;
                $scope.period_from_Id = periodid;
                $scope.table_list = $scope.temp_grid;
                for (var i = 0; i < $scope.table_list.length; i++) {
                    for (var j = 0; j < $scope.table_list[i].length; j++) {
                        $scope.temp_color = $scope.table_list[i][j].color;
                        if ($scope.table_list[i][j].pedid == periodid && $scope.table_list[i][j].dayid == dayid) {
                            $scope.table_list[i][j].color = "white";
                            $scope.table_list[i][j].background = "red";
                        }
                        else {
                            if ($scope.table_list[i][j].value == "Empty")
                            {
                                $scope.table_list[i][j].color = "green";
                                $scope.table_list[i][j].background = "green";
                            }
                            else {
                                $scope.table_list[i][j].color = $scope.temp_color;
                            }
                        }
                    }
                }
            }
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.get_category = function () {

            if ($scope.asmaY_Id === "") {
                swal("Please Select the Academic Year !");
            }
            else {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("StaffReplacementInUnallocatedPeriod/get_catg", data).
         then(function (promise) {

             $scope.categorylst = promise.catelist;
             if (promise.catelist === "" || promise.catelist === null) {
                 swal("No Category Are Mapped To Selected Academic Year");
             }
         })
            }
        };

        $scope.replacement_to_save = function () {

            if ($scope.from != null && $scope.from != "" && $scope.to != null && $scope.to != "") {
                var data = {
                    "HRME_Id":$scope.hrmE_Id,
                    "TTMD_ID_from": Number($scope.day_from_Id),
                    "TTMP_ID_from": Number($scope.period_from_Id),
                    "TTMD_ID_to": Number($scope.day_to_Id),
                    "TTMP_ID_to": Number($scope.period_to_Id),
                    "staffSDK": $scope.staffSDK,
                    "subSDK": $scope.subSDK,
                    "conSDK": $scope.conSDK,
                }
                apiService.create("StaffReplacementInUnallocatedPeriod/savedetail", data).
                  then(function (promise) {
                      if (promise.returnval_cls_notfree === true)
                      {
                          swal("Replacment Day Period Class & Section Not free !");
                      }
                      else
                      {
                          if (promise.returnval === true) {
                              swal("Replaced Periods sucessfully !");
                              $scope.GetReport();
                          }
                          else {
                              swal("Selected Periods Not Replaced !");
                              $scope.GetReport();
                          }
                      }
                   
                  })
            }
            else {
                swal("Please select Replacement day & Periods !!!");
            }
        };


    }
})();