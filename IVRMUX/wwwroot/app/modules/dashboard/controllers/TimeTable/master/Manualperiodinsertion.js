
(function () {
    'use strict';
    angular
.module('app')
        .controller('ManualperiodinsertionController', ManualperiodinsertionController)

    ManualperiodinsertionController = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function ManualperiodinsertionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.grid_view = false;
        $scope.gridOptions = {};
        $scope.datareport = false;
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.tempmainarray = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TTMC_Id": $scope.ttmC_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                   

                }
                apiService.create("Manualperiodinsertion/getrpt", data).
                    then(function (promise) {
                        debugger;
                        if (promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "") {
                            /*&& promise.time_Table != null && promise.time_Table != ""*/
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
                                                newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' '+ ':'+' ' + $scope.tt_list[k].ismS_SubjectName, stfid: $scope.tt_list[k].hrmE_Id}
                                                count += 1;
                                            }
                                            else if (count > 0) {
                                                newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName +' ' +' :' + $scope.tt_list[k].ismS_SubjectName;
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


                            console.log($scope.table_list)

                            $scope.period_list = promise.periodslst;
                            $scope.day_list = promise.gridweeks;
                            $scope.tt_list = promise.time_Table;
                            $scope.dayperiodlist = [];
                            var idd = 0;
                            angular.forEach($scope.day_list, function (mm) {

                                angular.forEach($scope.period_list, function (nn) {
                                    idd += 1;
                                    $scope.dayperiodlist.push({ id: idd, TTMD_Id: mm.ttmD_Id, TTMP_Id: nn.ttmP_Id })
                                })

                            })

                            $scope.sublist = promise.subjectlist;

                            //console.log($scope.dayperiodlist);

                        }
                        else {
                            swal("No. of Periods and Days are not mapped for Selected Details !!!!");
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
            apiService.getDATA("Manualperiodinsertion/getalldetails").
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

            $scope.ASMAY_Id = "";
            $scope.ttmC_Id = "";
            $scope.HRME_Id = "";
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
        $scope.tempmainarray = [];
        $scope.cell_click = function (dayid, periodid, day, period, hrmeId) {
            debugger;
            if (periodid == 0 || periodid == undefined) {
                swal("Select valid periods");
            }
            else {
                $scope.from = day + "Day & " + period + " period";
                $scope.day_from_Id = dayid;
                $scope.period_from_Id = periodid;
                $scope.table_list = $scope.temp_grid;
                for (var i = 0; i < $scope.table_list.length; i++) {
                    for (var j = 0; j < $scope.table_list[i].length; j++) {
                        if ($scope.table_list[i][j].pedid == periodid && $scope.table_list[i][j].dayid == dayid) {
                            if ($scope.table_list[i][j].color == "white" && $scope.table_list[i][j].background == "Green") {
                                $scope.table_list[i][j].color = "black";
                                $scope.table_list[i][j].background = "white";
                                //cntt += 1;

                            }
                            else {
                                $scope.table_list[i][j].color = "white";
                                $scope.table_list[i][j].background = "Green";

                            }
                        }
                    }

                }

               
                if ($scope.tempmainarray.length == 0) {

                    angular.forEach($scope.dayperiodlist, function (hhh) {

                        if (hhh.TTMD_Id == dayid && hhh.TTMP_Id == periodid) {
                            $scope.tempmainarray.push({ ID:hhh.id,TTMD_Id: dayid, TTMP_Id: periodid });

                        }

                    })

                    //console.log($scope.tempmainarray);
                    //$scope.tempmainarray.push({ dayid1: dayid, periodid1: periodid });
                }
                else {
                    var cnt = 0;
                    angular.forEach($scope.tempmainarray, function (rr) {

                        if (rr.TTMD_Id == dayid && rr.TTMP_Id == periodid) {
                            cnt += 1;

                        }
                    })
                    if (cnt == 0) {
                        angular.forEach($scope.dayperiodlist, function (hhh) {

                            if (hhh.TTMD_Id == dayid && hhh.TTMP_Id == periodid) {
                                $scope.tempmainarray.push({ ID: hhh.id, TTMD_Id: dayid, TTMP_Id: periodid });

                            }

                        })
                    }
                    if (cnt > 0) {
                        $scope.tempmainarray1 = [];

                        angular.forEach($scope.dayperiodlist, function (hhh) {

                            if (hhh.TTMD_Id == dayid && hhh.TTMP_Id == periodid) {
                                $scope.tempmainarray1.push({ ID: hhh.id, TTMD_Id: dayid, TTMP_Id: periodid });

                            }

                        })

                        $scope.tempmainarray2 = [];

                        angular.forEach($scope.tempmainarray, function (uu) {

                            angular.forEach($scope.tempmainarray1, function (zz) {

                                if (uu.ID !=zz.ID) {
                                    $scope.tempmainarray2.push(uu);
                                }

                            })
                        })
                       
                        $scope.tempmainarray = $scope.tempmainarray2;

                    }

                }
               


                console.log($scope.tempmainarray);


            }
        
        };

        //to get class by category      
        $scope.ttmC_Id = "";
        $scope.asmaY_Id = "";
        $scope.get_class = function () {
           if ($scope.ttmC_Id != "" && $scope.asmaY_Id != "") {
                var data = {
                    "TTMC_Id": $scope.ttmC_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("Manualperiodinsertion/getclass_catg", data).
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

            return $scope.submitted;
        };

        $scope.get_category = function () {

            if ($scope.asmaY_Id === "") {
                swal("Please Select the Academic Year !");
            }
            else {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("Manualperiodinsertion/get_catg", data).
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
            debugger;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.HRME_Id == "" || $scope.HRME_Id == undefined) {
                    swal("Select Staff Name");
                }
                else {
                var data = {
                    "TTMC_Id": Number($scope.ttmC_Id),
                    "ASMCL_Id": Number($scope.asmcL_Id),
                    "ASMS_Id": Number($scope.asmS_Id),
                    "HRME_Id": Number($scope.HRME_Id.hrmE_Id),
                    "ISMS_Id": Number($scope.ISMS_Id),
                    "ASMAY_Id": $scope.ASMAY_Id,
                    Time_Table: $scope.tempmainarray
                }
                apiService.create("Manualperiodinsertion/savedetail", data).
                  then(function (promise) {
                      if (promise.returnval === true) {
                          swal("Total" + "  " + promise.sscnt + "  " + "Periods Saved  successfully !");
                          $scope.GetReport();
                          $scope.HRME_Id = "";
                         // $state.reload();
                      }
                      else {
                          swal("Total" + "  " + promise.ffcnt + " " + "Periods Not Saved !");
                       //   swal("Same staff cannot able to insert in particular Period and Day !");
                          $state.reload();
                      }
                  })
            }
            }
            
        };
    }
})();