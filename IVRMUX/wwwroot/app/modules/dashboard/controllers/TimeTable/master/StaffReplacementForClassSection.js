
(function () {
    'use strict';
    angular
.module('app')
.controller('StaffReplacementForClassSectionController', StaffReplacementForClassSectionController)

    StaffReplacementForClassSectionController = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function StaffReplacementForClassSectionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.grid_view = false;
        $scope.gridOptions = {};
        $scope.datareport = false;
        $scope.submitted = false;
        $scope.GetReport = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    //"HRME_Id": $scope.hrmE_Id,
                    "TTMC_Id": $scope.ttmC_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                }
                apiService.create("StaffReplacementForClassSection/getrpt", data).
                    then(function (promise) {
                        if (promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "" && promise.time_Table != null && promise.time_Table != "") {
                            $scope.grid_view = true;
                            $scope.period_list = promise.periodslst;
                            $scope.day_list = promise.gridweeks;
                            $scope.tt_list = promise.time_Table;
                            var temp_array = [];
                            $scope.table_list = [];
                            $scope.period_break_list = [];

                            for (var p = 0; p < $scope.period_list.length; p++) {
                                var break_flag = false;
                                for (var c = 0 ; c < promise.break_list.length; c++) {

                                    if ($scope.asmcL_Id == promise.break_list[c].asmcL_Id && parseFloat($scope.period_list[p].ttmP_PeriodName) == promise.break_list[c].ttmB_AfterPeriod) {

                                        $scope.period_break_list.push({ ped_id: $scope.period_list[p].ttmP_Id, ped_name: $scope.period_list[p].ttmP_PeriodName, brk_name: ' ' })
                                        $scope.period_break_list.push({ ped_id: 0, ped_name: 'Break', brk_name: promise.break_list[c].ttmB_BreakName })
                                        break_flag = true;

                                    }

                                }
                                if (break_flag == false) {

                                    $scope.period_break_list.push({ ped_id: $scope.period_list[p].ttmP_Id, ped_name: $scope.period_list[p].ttmP_PeriodName, brk_name: ' ' })
                                    break_flag = false;
                                }
                            }
                            for (var j = 0; j < $scope.day_list.length; j++) {
                                temp_array = [];
                                for (var i = 0; i < $scope.period_break_list.length; i++) {
                                    var count = 0;
                                    var newCol = "";
                                    for (var k = 0; k < $scope.tt_list.length; k++) {

                                        if ($scope.tt_list[k].ttmP_Id == $scope.period_break_list[i].ped_id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id) {

                                            if (count == 0) {
                                                newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' TO ' + $scope.tt_list[k].ismS_SubjectName, stfid: $scope.tt_list[k].hrmE_Id }
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

                                        if ($scope.period_break_list[i].ped_id == 0) {
                                            for (var x = 0; x < promise.break_list_all.length; x++) {
                                                if ($scope.day_list[j].ttmD_Id == promise.break_list_all[x].ttmD_Id && $scope.period_break_list[(i - 1)].ped_id == promise.break_list_all[x].ttmB_AfterPeriod) {

                                                    newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: promise.break_list_all[x].ttmB_BreakName, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }

                                                }
                                                if ($scope.day_list[j].ttmD_Id == promise.break_list_all[x].ttmD_Id && $scope.period_break_list[(i - 1)].ped_id != promise.break_list_all[x].ttmB_AfterPeriod) {
                                                    newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: $scope.period_break_list[i].brk_name, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                                                }
                                            }
                                        }
                                        else {
                                            newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: ' ', pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                                        }
                                    }
                                    temp_array.push(newCol);
                                    newCol = "";
                                    count = 0;
                                }
                                $scope.table_list.push(temp_array);
                            }
                            $scope.datareport = true;
                            var temp_table_list = [];
                            for (var i = 0; i < $scope.table_list.length; i++) {
                                for (var j = 0; j < $scope.table_list[i].length; j++) {
                                    if ($scope.table_list[i][j].pedid === 0) {
                                        if ($scope.table_list[i][j].value1 === 'No')
                                        {
                                            $scope.table_list[i][j].color = "black";
                                            $scope.table_list[i][j].background = "white";
                                        }
                                        else {
                                            $scope.table_list[i][j].color = "black";
                                            $scope.table_list[i][j].background = "Yellow";
                                        }
                                    }
                                }
                            }
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
            apiService.getDATA("StaffReplacementForClassSection/getalldetails").
       then(function (promise) {

           $scope.clearid();
           $scope.academic = promise.academiclist;
           $scope.categorylst = promise.catelist;
           $scope.stafflst = promise.staffDrpDwn;
           $scope.temp_classlist = promise.classlist;
           //  $scope.temp_sectionlist = promise.sectionlist;
           $scope.class_list = promise.classlist;
           $scope.section_list = promise.sectionlist;
       })
        };
        //TO clear  data
        $scope.clearid = function () {

            $scope.ttmC_Id = "";
            $scope.hrmE_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.class_list = $scope.temp_classlist;
            $scope.grid_view = false;
            $scope.submitted = false;
            $scope.datareport = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.from = "";
            $scope.to = "";


        };
        $scope.from = "";
        $scope.to = "";
        $scope.cell_click = function (dayid, periodid, day, period, hrmeId) {

            if ($scope.from != null && $scope.from != "") {

                for (var i = 0; i < $scope.table_list.length; i++) {
                    for (var j = 0; j < $scope.table_list[i].length; j++) {
                        if ($scope.table_list[i][j].pedid == periodid && $scope.table_list[i][j].dayid == dayid) {
                            if ($scope.table_list[i][j].background == "Green") {
                                if ($scope.day_from_Id === dayid && $scope.period_from_Id === periodid) {
                                    $scope.to = "";
                                    $scope.day_to_Id = "";
                                    $scope.period_to_Id = "";
                                    swal("same day same period selected !!");
                                }
                                else {
                                    $scope.to = day + "Day & " + period + " period";
                                    $scope.day_to_Id = dayid;
                                    $scope.period_to_Id = periodid;
                                }
                            }
                            else if ($scope.table_list[i][j].background == "red") {
                                if ($scope.day_from_Id === dayid && $scope.period_from_Id === periodid) {
                                    $scope.to = "";
                                    $scope.day_to_Id = "";
                                    $scope.period_to_Id = "";
                                    swal("same day same period selected !!");
                                }
                                else {
                                    $scope.to = day + "Day & " + period + " period";
                                    $scope.day_to_Id = dayid;
                                    $scope.period_to_Id = periodid;
                                }
                            }
                            else {
                                swal("Please Select Replace possibility Period !!");
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
                            $scope.table_list[i][j].color = $scope.temp_color;
                        }
                    }
                }
                var data = {
                    "TTMC_Id": $scope.ttmC_Id,
                    "TTMD_Id": dayid,
                    "TTMP_Id": periodid,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "staffSDK": $scope.staffSDK,
                    "subSDK": $scope.subSDK,
                    "conSDK": $scope.conSDK,

                }
                apiService.create("StaffReplacementForClassSection/getpossiblePeriod", data).
                then(function (promise) {
                    $scope.table_list = [];
                    $scope.table_list123 = [];
                    $scope.table_list = $scope.temp_grid;
                    $scope.table_list123 = promise.data_lst;
     
                    if ($scope.table_list123[0].ttmP_Id === 0 || $scope.table_list123[0].ttmD_Id === 0) {
                        swal("You selected UDC Period ! ", "Please Select Another Period , UDC Periods Cannot Replace !!");
                        $scope.GetReport();
                    }
                    else {
                        for (var i = 0; i < $scope.table_list.length; i++) {
                            for (var j = 0; j < $scope.table_list[i].length; j++) {
                                for (var k = 0; k < $scope.table_list123.length; k++) {
                                    $scope.temp_color = $scope.table_list[i][j].color;
                                    if ($scope.table_list[i][j].pedid == $scope.table_list123[k].ttmP_Id && $scope.table_list[i][j].dayid == $scope.table_list123[k].ttmD_Id) {
                                        $scope.table_list[i][j].color = "white";
                                        $scope.table_list[i][j].background = "Green";
                                    }
                                    else {
                                        $scope.table_list[i][j].color = $scope.temp_color;
                                    }
                                }
                            }
                        }
                    }
                })

            }
        }

        //to get class by category      
        $scope.ttmC_Id = "";
        $scope.get_class = function () {
            if ($scope.ttmC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.ttmC_Id,

                }
                apiService.create("StaffReplacementForClassSection/getclass_catg", data).
        then(function (promise) {

            $scope.class_list = promise.classlist;
            $scope.asmcL_Id = "";
            if (promise.classlist == "" || promise.classlist == null) {
                swal("No classes Are Mapped To Selected Category");
            }
        })
            }
        };

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
                apiService.create("StaffReplacementForClassSection/get_catg", data).
         then(function (promise) {

             $scope.categorylst = promise.catelist;
             $scope.asmcL_Id = "";
             if (promise.catelist === "" || promise.catelist === null) {
                 swal("No Category Are Mapped To Selected Academic Year");
             }
         })
            }
        };

        $scope.replacement_to_save = function () {

            if ($scope.from != null && $scope.from != "" && $scope.to != null && $scope.to != "") {
                var data = {
                    "TTMC_Id": Number($scope.ttmC_Id),
                    "ASMCL_Id": Number($scope.asmcL_Id),
                    "ASMS_Id": Number($scope.asmS_Id),
                    "TTMD_ID_from": Number($scope.day_from_Id),
                    "TTMP_ID_from": Number($scope.period_from_Id),
                    "TTMD_ID_to": Number($scope.day_to_Id),
                    "TTMP_ID_to": Number($scope.period_to_Id)
                }
                apiService.create("StaffReplacementForClassSection/savedetail", data).
                  then(function (promise) {
                      if (promise.returnval === true) {
                          swal("Replaced Periods sucessfully !");
                          $scope.GetReport();
                      }
                      else {
                          swal("Selected Periods Not Replaced !");
                          $scope.GetReport();
                      }
                  })
            }
            else {
                swal("Please select Replacement day & Periods !!!");
            }
        };
    }
})();