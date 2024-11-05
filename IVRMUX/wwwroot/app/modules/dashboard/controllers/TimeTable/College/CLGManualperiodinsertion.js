
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGManualperiodinsertionController', CLGManualperiodinsertionController)

    CLGManualperiodinsertionController = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function CLGManualperiodinsertionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {


        $scope.temp_grid = [];


        $scope.get_course = function () {
            $scope.grid_view = false;
            $scope.temp_grid = [];
            $scope.tempmainarray = [];
            $scope.AMB_Id = '';
            $scope.semisterlist = [];
            if ($scope.ASMAY_Id === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                apiService.create("CLGTTCommon/getcourse_catg", data).
                    then(function (promise) {

                        $scope.courselist = promise.courselist;

                        if (promise.courselist == "" || promise.courselist == null) {
                            swal("No Course/Branch Are Mapped To Selected Category");
                        }
                    })
            }
        };
        $scope.getbranch_catg = function () {
            $scope.grid_view = false;
            $scope.temp_grid = [];
            $scope.tempmainarray = [];
            $scope.AMB_Id = '';
            $scope.semisterlist = [];
            var data = {
                "TTMC_Id": $scope.TTMC_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("CLGTTCommon/getbranch_catg", data).
                then(function (promise) {

                    $scope.branchlist = promise.branchlist;

                    if (promise.branchlist == "" || promise.branchlist == null) {
                        swal("No Branch Are Mapped To Selected Category/Course");
                    }
                })

        };
        $scope.get_semister = function () {
            $scope.grid_view = false;
            $scope.temp_grid = [];
            $scope.tempmainarray = [];
            var data = {
                "TTMC_Id": $scope.TTMC_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMB_Id": $scope.AMB_Id,
            }
            apiService.create("CLGTTCommon/get_semister", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;

                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("No Semester Are Mapped To Selected Course/Branch");
                    }
                })
        };

        $scope.onyrchange = function () {
            $scope.temp_grid = [];
            $scope.tempmainarray = [];
            $scope.TTMC_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMSE_Id = '';
            $scope.AMB_Id = '';
            $scope.ACMS_Id = '';
            $scope.grid_view = false;


        }

        $scope.grid_view = false;
        $scope.gridOptions = {};
        $scope.datareport = false;
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.temp_grid = [];
            $scope.tempmainarray = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TTMC_Id": $scope.TTMC_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                }
                apiService.create("CLGManualperiodinsertion/getrpt", data).
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

                                    if ($scope.AMCO_Id == promise.break_list[c].amcO_Id && $scope.AMB_Id == promise.break_list[c].amB_Id && $scope.AMSE_Id == promise.break_list[c].amsE_Id && parseFloat($scope.period_list[p].ttmP_PeriodName) == promise.break_list[c].ttmbC_AfterPeriod) {

                                        $scope.period_break_list.push({ ped_id: $scope.period_list[p].ttmP_Id, ped_name: $scope.period_list[p].ttmP_PeriodName, brk_name: ' ' })
                                        $scope.period_break_list.push({ ped_id: 0, ped_name: 'Break', brk_name: promise.break_list[c].ttmbC_BreakName, AF: promise.break_list[c].ttmbC_AfterPeriod })
                                        break_flag = true;

                                    }

                                }
                                if (break_flag == false) {

                                    $scope.period_break_list.push({ ped_id: $scope.period_list[p].ttmP_Id, ped_name: $scope.period_list[p].ttmP_PeriodName, brk_name: ' ' })
                                    break_flag = false;
                                }
                            }


                            $scope.distcourselist = [];

                            angular.forEach($scope.period_break_list, function (st) {

                                debugger;

                                    if ($scope.distcourselist.length == 0) {

                                        if (st.AF == null || st.AF == undefined) {
                                            $scope.distcourselist.push({ ped_id: st.ped_id, ped_name: st.ped_name, brk_name: ' ' });
                                        } else {
                                            $scope.period_break_list.push({ ped_id: 0, ped_name: 'Break', brk_name: st.brk_name, AF: st.AF })
                                        }
                                      
                                    }
                                    else if ($scope.distcourselist.length > 0) {
                                        var cntt = 0;
                                        var cntt1 = 0;
                                        angular.forEach($scope.distcourselist, function (exm) {
                                            if (st.AF != null || st.AF != undefined) {
                                            if (exm.AF == st.AF) {
                                                cntt += 1;
                                            }
                                            }
                                            else {
                                                if (exm.ped_id == st.ped_id) {
                                                    cntt1 += 1;
                                                }      
                                            }
                                        })
                                        if (cntt == 0 && cntt1 == 0) {

                                            if (st.AF == null || st.AF == undefined) {
                                                $scope.distcourselist.push({ ped_id: st.ped_id, ped_name: st.ped_name, brk_name: ' ' });
                                            } else {
                                                $scope.distcourselist.push({ ped_id: 0, ped_name: 'Break', brk_name: st.brk_name, AF: st.AF })
                                            }
                                        }
                                    }
                               
                            })

                            console.log($scope.period_break_list);
                            console.log($scope.distcourselist);

                            for (var j = 0; j < $scope.day_list.length; j++) {
                                temp_array = [];
                                for (var i = 0; i < $scope.distcourselist.length; i++) {
                                    var count = 0;
                                    var newCol = "";
                                    for (var k = 0; k < $scope.tt_list.length; k++) {

                                        if ($scope.tt_list[k].ttmP_Id == $scope.distcourselist[i].ped_id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id) {

                                            if (count == 0) {
                                                newCol = { pedid: $scope.distcourselist[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].amcO_CourseName + ' :' + $scope.tt_list[k].amB_BranchName + ' :' + $scope.tt_list[k].amsE_SEMName + ' :' + $scope.tt_list[k].acmS_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.distcourselist[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' '+ ':'+' ' + $scope.tt_list[k].ismS_SubjectName, stfid: $scope.tt_list[k].hrmE_Id}
                                                count += 1;
                                            }
                                            else if (count > 0) {
                                                newCol.value = newCol.value + ' && ' + $scope.tt_list[k].amcO_CourseName + ' :' + $scope.tt_list[k].amB_BranchName + ' :' + $scope.tt_list[k].amsE_SEMName + ' :' + $scope.tt_list[k].acmS_SectionName +' ' +' :' + $scope.tt_list[k].ismS_SubjectName;
                                                newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                count += 1;
                                            }
                                        }

                                    }

                                    if (newCol == "") {

                                        if ($scope.distcourselist[i].ped_id == 0) {
                                            for (var x = 0; x < promise.break_list_all.length; x++) {
                                                if ($scope.day_list[j].ttmD_Id == promise.break_list_all[x].ttmD_Id && $scope.distcourselist[(i - 1)].ped_id == promise.break_list_all[x].ttmbC_AfterPeriod) {

                                                    newCol = { pedid: $scope.distcourselist[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: promise.break_list_all[x].ttmbC_BreakName, pedname: $scope.distcourselist[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }

                                                }
                                                if ($scope.day_list[j].ttmD_Id == promise.break_list_all[x].ttmD_Id && $scope.distcourselist[(i - 1)].ped_id != promise.break_list_all[x].ttmbC_AfterPeriod) {
                                                    newCol = { pedid: $scope.distcourselist[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: $scope.distcourselist[i].brk_name, pedname: $scope.distcourselist[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                                                }
                                            }
                                        }
                                        else {
                                            newCol = { pedid: $scope.distcourselist[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: ' ', pedname: $scope.distcourselist[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
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
            apiService.getDATA("CLGManualperiodinsertion/getalldetails").
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
            $scope.TTMC_Id = "";
            $scope.HRME_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
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
        $scope.get_class = function () {
            if ($scope.ttmC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.ttmC_Id,

                }
                apiService.create("CLGManualperiodinsertion/getclass_catg", data).
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
                apiService.create("CLGManualperiodinsertion/get_catg", data).
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
                    "TTMC_Id": Number($scope.TTMC_Id),
                    "AMCO_Id": Number($scope.AMCO_Id),
                    "AMB_Id": Number($scope.AMB_Id),
                    "AMSE_Id": Number($scope.AMSE_Id),
                    "ACMS_Id": Number($scope.ACMS_Id),
                    "HRME_Id": Number($scope.HRME_Id.hrmE_Id),
                    "ISMS_Id": Number($scope.ISMS_Id),
                    "ASMAY_Id": $scope.ASMAY_Id,
                    periods: $scope.tempmainarray
                }
                    apiService.create("CLGManualperiodinsertion/savedetail", data).
                  then(function (promise) {
                      if (promise.returnval === true) {
                          swal("Total" + "  " + promise.sscnt + "  " + "Periods Saved  successfully !");
                          $scope.GetReport();
                          $scope.HRME_Id = "";
                         // $state.reload();
                      }
                      else {
                          swal("Total" + "  " + promise.ffcnt + " " + "Periods Not Saved !");
                          $state.reload();
                      }
                  })
            }
            }
            
        };
    }
})();