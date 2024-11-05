(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeStudent_TTController', CollegeStudent_TTController);

    CollegeStudent_TTController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window'];
    function CollegeStudent_TTController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


        $scope.grid_view = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CollegeStudent_TT/getloaddata", pageid).
                then(function (promise) {
                    $scope.getyear = promise.getyear;
                });
        };
        //=================================== GENERATE TT
        $scope.onyearchange = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id
                };
                apiService.create("CollegeStudent_TT/getStudentTT", data).
                    then(function (promise) {

                        if (promise.tt !== null && promise.tt !== "" && promise.periodslst !== null && promise.periodslst !== "" && promise.gridweeks !== null && promise.gridweeks !== "") {
                            $scope.albumNameArray1 = promise.alldata;
                            $scope.albumNameArray2 = promise.alldata;
                            $scope.table_list_cls_sec_wise = [];
                            for (var a = 0; a < $scope.albumNameArray1.length; a++) {
                                $scope.period_break_list = [];
                                $scope.period_list = promise.periodslst;
                                for (var p = 0; p < $scope.period_list.length; p++) {
                                    var break_flag = false;
                                    for (var c = 0; c < promise.tT_Break_list.length; c++) {
                                        if ($scope.albumNameArray1[a].asmcL_Id === promise.tT_Break_list[c].asmcL_Id && parseFloat($scope.period_list[p].ttmP_PeriodName) === promise.tT_Break_list[c].ttmbC_AfterPeriod) {
                                            $scope.period_break_list.push({ ped_id: $scope.period_list[p].ttmP_Id, ped_name: $scope.period_list[p].ttmP_PeriodName });
                                            $scope.period_break_list.push({ ped_id: 0, ped_name: 'Break', brk_name: promise.tT_Break_list[c].type });
                                            break_flag = true;
                                        }
                                    }
                                    if (break_flag === false) {
                                        $scope.period_break_list.push({ ped_id: $scope.period_list[p].ttmP_Id, ped_name: $scope.period_list[p].ttmP_PeriodName, brk_name: ' ' });
                                        break_flag = false;
                                    }
                                }
                                for (var b = 0; b < $scope.albumNameArray2.length; b++) {
                                    $scope.grid_view = true;
                                    $scope.day_list = promise.gridweeks;
                                    $scope.tt_list = promise.tt;
                                    var temp_array = [];
                                    $scope.table_list = [];
                                    for (var j = 0; j < $scope.day_list.length; j++) {
                                        temp_array = [];
                                        for (var i = 0; i < $scope.period_break_list.length; i++) {
                                            var count = 0;
                                            var newCol = "";
                                            for (var k = 0; k < $scope.tt_list.length; k++) {

                                                if ($scope.tt_list[k].ttmP_Id === $scope.period_break_list[i].ped_id && $scope.tt_list[k].ttmD_Id === $scope.day_list[j].ttmD_Id && $scope.tt_list[k].asmcL_Id === $scope.albumNameArray1[a].asmcL_Id && $scope.tt_list[k].asmS_Id === $scope.albumNameArray2[b].asmS_Id) {
                                                    if (count === 0) {
                                                        newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value2: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName };
                                                        count += 1;
                                                    }
                                                    else if (count > 0) {
                                                        newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                        newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                        newCol.value2 = newCol.value2 + '&& ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName;
                                                        count += 1;
                                                    }
                                                }
                                            }
                                            if (newCol === "") {
                                                if ($scope.period_break_list[i].ped_id === 0) {
                                                    for (var x = 0; x < promise.tT_Break_list_all.length; x++) {
                                                        if ($scope.day_list[j].ttmD_Id === promise.tT_Break_list_all[x].ttmD_Id && promise.tT_Break_list_all[x].amB_Id === $scope.albumNameArray1[a].asmcL_Id && $scope.period_break_list[(i - 1)].ped_id === promise.tT_Break_list_all[x].ttmbC_AfterPeriod) {
                                                            newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: promise.tT_Break_list_all[x].ttmbC_BreakName, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' };
                                                        }
                                                        if ($scope.day_list[j].ttmD_Id === promise.tT_Break_list_all[x].ttmD_Id && promise.tT_Break_list_all[x].amB_Id === $scope.albumNameArray1[a].asmcL_Id && $scope.period_break_list[(i - 1)].ped_id !== promise.tT_Break_list_all[x].ttmbC_AfterPeriod) {
                                                            newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: $scope.period_break_list[i].brk_name, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' };
                                                        }
                                                    }
                                                }
                                                else {
                                                    newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: ' ', pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' };
                                                }
                                            }
                                            temp_array.push(newCol);
                                            newCol = "";
                                            count = 0;
                                        }
                                        $scope.table_list.push(temp_array);
                                    }
                                    $scope.table_list_cls_sec_wise.push({ array: $scope.table_list, ped_list: $scope.period_break_list, header: "Class : " + $scope.albumNameArray1[a].asmcL_ClassName + " & Section : " + $scope.albumNameArray2[b].asmC_SectionName, id: $scope.albumNameArray1[a].asmcL_Id + ':' + $scope.albumNameArray2[b].asmS_Id });
                                }
                            }
                        }
                        else {
                            swal("TimeTable is Not Generated For Selected Details !!!!");
                            $scope.grid_view = false;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

    }
})();