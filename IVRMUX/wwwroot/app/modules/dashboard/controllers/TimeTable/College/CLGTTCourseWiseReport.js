
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGTTCourseWiseReportController', CLGTTCourseWiseReportController)

    CLGTTCourseWiseReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', 'Excel', '$timeout', '$http', '$q', '$stateParams', '$filter']
    function CLGTTCourseWiseReportController($rootScope, $scope, $state, $location, apiService, Flash, Excel, $timeout, $http, $q, $stateParams, $filter) {

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsE_SEMName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.searchchkbx1 = "";
        $scope.filterchkbx1 = function (obj) {
            return angular.lowercase(obj.acmS_SectionName).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
        }


        $scope.editEmployee = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.grid_view = false;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.semisterlist.every(function (options) {
                return options.class;
            });
        }
        $scope.togchkbx1 = function () {
            $scope.usercheck1 = $scope.section_list.every(function (options) {
                return options.sec;
            });
        }
        $scope.all_check = function () {
            
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.semisterlist, function (itm) {
                itm.class = toggleStatus;
            });
        }



        $scope.all_check1 = function () {
            var toggleStatus = $scope.usercheck1;
            angular.forEach($scope.section_list, function (itm) {
                itm.sec = toggleStatus;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.semisterlist.some(function (options) {
                return options.class;
            });

        }

        $scope.isOptionsRequired1 = function () {
            return !$scope.section_list.some(function (options) {
                return options.sec;
            });

        }

        $scope.interacted1 = function (field) {

            return $scope.submitted || field.$dirty;
        };


        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.getcourse_catg = function () {
            $scope.semisterlist = [];
            $scope.branchlist = [];
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.semisterlist = [];
            $scope.usercheck = false;
            if ($scope.ASMAY_Id == "" && $scope.ASMAY_Id == undefined) {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != undefined && $scope.TTMC_Id != "") {
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
            $scope.semisterlist = [];
            $scope.AMB_Id = '';
            $scope.semisterlist = [];
            $scope.usercheck = false;
            $scope.branchlist = [];
            if ($scope.ASMAY_Id == "" && $scope.ASMAY_Id == undefined) {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != undefined && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                }
                apiService.create("CLGTTCommon/getbranch_catg", data).
                    then(function (promise) {

                        $scope.branchlist = promise.branchlist;

                        if (promise.branchlist == "" || promise.branchlist == null) {
                            swal("No Branch Are Mapped To Selected Category/Course");
                        }
                    })
            }
        };

        $scope.get_semister = function () {
            $scope.semisterlist = [];
            if ($scope.ASMAY_Id == "" && $scope.ASMAY_Id == undefined) {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != undefined && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                }
                apiService.create("CLGTTCommon/get_semister", data).
                    then(function (promise) {

                        $scope.semisterlist = promise.semisterlist;

                        if (promise.semisterlist == "" || promise.semisterlist == null) {
                            swal("No Semesters Are Mapped To Selected Category/Course");
                        }
                    })
            }
        };


        $scope.get_category = function () {

            $scope.TTMC_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.semisterlist = [];
            $scope.usercheck = false;

        }
        $scope.time_Table = [];
        $scope.TT_Break_list_all = [];
        $scope.allday = [];
        // TO Save The Data
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];

                angular.forEach($scope.semisterlist, function (role) {
                    if (role.class) $scope.albumNameArray1.push(role);
                })
                angular.forEach($scope.section_list, function (role) {
                    if (role.sec) $scope.albumNameArray2.push(role);
                })
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TTMC_Id": $scope.TTMC_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    classarray: $scope.albumNameArray1,
                    sectionarray: $scope.albumNameArray2,
                }
                apiService.create("CLGTTCourseWiseReport/getrpt", data).
                    then(function (promise) {
                      
                        $scope.time_Table = promise.time_Table;
                        $scope.TT_Break_list_all = promise.tT_Break_list_all;
                        $scope.allday = promise.allday;
                        $scope.dayBreak_list_all = promise.dayBreak_list_all;

                       

                        if ($scope.time_Table.length > 0 && $scope.allday.length > 0) {
                            $scope.grid_view = true;

                            $scope.distcourselist = [];

                            angular.forEach($scope.time_Table, function (st) {
                                if ($scope.distcourselist.length == 0) {

                                    $scope.distcourselist.push({ AMCO_Id: st.AMCO_Id, AMB_Id: st.AMB_Id, AMSE_Id: st.AMSE_Id, ACMS_Id: st.ACMS_Id, CRSDETAILS: st.CRSDETAILS });
                                }
                                else if ($scope.distcourselist.length > 0) {
                                    var cntt = 0;
                                    angular.forEach($scope.distcourselist, function (exm) {
                                        if (exm.AMCO_Id == st.AMCO_Id && exm.AMB_Id == st.AMB_Id && exm.AMSE_Id == st.AMSE_Id && exm.ACMS_Id == st.ACMS_Id) {
                                            cntt += 1;
                                        }
                                    })
                                    if (cntt == 0) {

                                        $scope.distcourselist.push({ AMCO_Id: st.AMCO_Id, AMB_Id: st.AMB_Id, AMSE_Id: st.AMSE_Id, ACMS_Id: st.ACMS_Id, CRSDETAILS: st.CRSDETAILS });
                                    }
                                }
                            })

                            $scope.temparray = [];
                            angular.forEach($scope.distcourselist, function (yy) {
                                $scope.mainperiodlist = [];
                                var ct = 0;
                                angular.forEach($scope.periodslst, function (xx) {
                                    if ($scope.TT_Break_list_all.length > 0) {
                                        var cnt = 0;
                                        angular.forEach($scope.TT_Break_list_all, function (zz) {
                                            if (yy.AMCO_Id == zz.amcO_Id && yy.AMB_Id == zz.amB_Id && yy.AMSE_Id == zz.amsE_Id) {
                                                ct += 1;
                                                if (xx.ttmP_PeriodName == zz.ttmbC_AfterPeriod) {
                                                    cnt += 1;
                                                    $scope.mainperiodlist.push({ TTMP_Id: xx.ttmP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.ttmP_PeriodName })
                                                    $scope.mainperiodlist.push({ TTMP_Id: zz.ttmbC_BreakName, TTMP_PeriodName: 'BREAK', AF: zz.ttmbC_AfterPeriod})
                                                }
                                                else {

                                                }
                                            }
                                            //else {
                                            //    $scope.mainperiodlist.push({ TTMP_Id: xx.ttmP_Id, TTMP_PeriodName: xx.ttmP_PeriodName })
                                            //}

                                        })
                                        if (cnt == 0 && ct > 0) {
                                            $scope.mainperiodlist.push({ TTMP_Id: xx.ttmP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.ttmP_PeriodName })
                                        }
                                        if (ct == 0) {
                                            $scope.mainperiodlist.push({ TTMP_Id: xx.ttmP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.ttmP_PeriodName })
                                        }

                                    }
                                    else {
                                        $scope.mainperiodlist.push({ TTMP_Id: xx.ttmP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.ttmP_PeriodName })
                                    }


                                })

                                ///START
                                $scope.distprdlist = [];

                                angular.forEach($scope.mainperiodlist, function (st) {

                                    debugger;

                                    if ($scope.distprdlist.length == 0) {

                                        if (st.AF == null || st.AF == undefined) {
                                            $scope.distprdlist.push({ TTMP_Id: st.TTMP_Id, TTMP_PeriodName: st.TTMP_PeriodName });
                                        } else {
                                            $scope.distprdlist.push({ TTMP_Id: st.TTMP_Id, TTMP_PeriodName: st.TTMP_PeriodName,  AF: st.AF })
                                        }

                                    }
                                    else if ($scope.distprdlist.length > 0) {
                                        var cntt = 0;
                                        var cntt1 = 0;
                                        angular.forEach($scope.distprdlist, function (exm) {
                                            if (st.AF != null || st.AF != undefined) {
                                                if (exm.AF == st.AF) {
                                                    cntt += 1;
                                                }
                                            }
                                            else {
                                                if (exm.TTMP_Id == st.TTMP_Id) {
                                                    cntt1 += 1;
                                                }
                                            }
                                        })
                                        if (cntt == 0 && cntt1 == 0) {

                                            if (st.AF == null || st.AF == undefined) {
                                                $scope.distprdlist.push({ TTMP_Id: st.TTMP_Id, TTMP_PeriodName: st.TTMP_PeriodName });
                                            } else {
                                                $scope.distprdlist.push({ TTMP_Id: st.TTMP_Id, TTMP_PeriodName: st.TTMP_PeriodName, AF: st.AF })
                                            }
                                        }
                                    }

                                })


                                //END

                                console.log($scope.distprdlist);

                                yy.perlist = $scope.mainperiodlist;
                                yy.perlist1 = $scope.distprdlist;

                            })

                            angular.forEach($scope.distcourselist, function (ww) {
                                $scope.daylst = [];
                                angular.forEach($scope.allday, function (dd) {
                                    $scope.periodlst = [];
                                    if (ww.AMCO_Id == dd.amcO_Id && ww.AMB_Id == dd.amB_Id && ww.AMSE_Id == dd.amsE_Id) {
                                        angular.forEach($scope.time_Table, function (tt) {
                                            if (ww.AMCO_Id == tt.AMCO_Id && ww.AMB_Id == tt.AMB_Id && ww.AMSE_Id == tt.AMSE_Id && ww.ACMS_Id == tt.ACMS_Id && dd.ttmD_Id == tt.TTMD_Id) {
                                                $scope.periodlst.push(tt);
                                            }

                                        })


                                        $scope.daybr = [];
                                        angular.forEach($scope.dayBreak_list_all, function (vv) {
                                            if (ww.AMCO_Id == vv.amcO_Id && ww.AMB_Id == vv.amB_Id && ww.AMSE_Id == vv.amsE_Id  && dd.ttmD_Id == vv.ttmD_Id) {
                                                $scope.daybr.push(vv);
                                            }

                                        })

                                        $scope.daylst.push({ TTMD_Id: dd.ttmD_Id, TTMD_DayName: dd.ttmD_DayName, PERIODS: $scope.periodlst, break: $scope.daybr})
                                    }


                                })

                                ww.daylist = $scope.daylst;
                            })

                            console.log($scope.distcourselist)
                        }

 else {
                            swal("TimeTable is Not Generated For Selected Details !!!!");
                            $scope.grid_view = false;
                        }

                        //if (promise.tt != null && promise.tt != "" && promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "") {
                        //    $scope.table_list_cls_sec_wise = [];
                        //    for (var a = 0; a < $scope.albumNameArray1.length; a++) {
                        //        $scope.period_break_list = [];
                        //        $scope.period_list = promise.periodslst;
                        //        for (var p = 0; p < $scope.period_list.length; p++) {
                        //            var break_flag = false;
                        //            for (var c = 0 ; c < promise.tT_Break_list.length; c++) {
                        //                if ($scope.albumNameArray1[a].asmcL_Id == promise.tT_Break_list[c].asmcL_Id && parseFloat($scope.period_list[p].ttmP_PeriodName) == promise.tT_Break_list[c].ttmB_AfterPeriod) {
                        //                    $scope.period_break_list.push({ ped_id: $scope.period_list[p].ttmP_Id, ped_name: $scope.period_list[p].ttmP_PeriodName })
                        //                    $scope.period_break_list.push({ ped_id: 0, ped_name: 'Break' })
                        //                    break_flag = true;
                        //                }
                        //            }
                        //            if (break_flag == false) {
                        //                $scope.period_break_list.push({ ped_id: $scope.period_list[p].ttmP_Id, ped_name: $scope.period_list[p].ttmP_PeriodName, brk_name: ' ' })
                        //                break_flag = false;
                        //            }
                        //        }
                        //        for (var b = 0; b < $scope.albumNameArray2.length; b++) {
                        //            $scope.grid_view = true;
                        //            $scope.day_list = promise.gridweeks;
                        //            $scope.tt_list = promise.tt;
                        //            var temp_array = [];
                        //            $scope.table_list = [];
                        //            for (var j = 0; j < $scope.day_list.length; j++) {
                        //                temp_array = [];
                        //                for (var i = 0; i < $scope.period_break_list.length; i++) {
                        //                    var count = 0;
                        //                    var newCol = "";
                        //                    for (var k = 0; k < $scope.tt_list.length; k++) {

                        //                        if ($scope.tt_list[k].ttmP_Id == $scope.period_break_list[i].ped_id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id && $scope.tt_list[k].asmcL_Id == $scope.albumNameArray1[a].asmcL_Id && $scope.tt_list[k].asmS_Id == $scope.albumNameArray2[b].asmS_Id) {
                        //                            if (count == 0) {
                        //                                newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value2: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName }
                        //                                count += 1;
                        //                            }
                        //                            else if (count > 0) {
                        //                                newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                        //                                newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                        //                                newCol.value2 = newCol.value2 + '&& ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName;
                        //                                count += 1;
                        //                            }
                        //                        }
                        //                    }
                        //                    if (newCol == "") {
                        //                        if ($scope.period_break_list[i].ped_id == 0) {
                        //                            for (var x = 0; x < promise.tT_Break_list_all.length; x++) {
                        //                                if ($scope.day_list[j].ttmD_Id == promise.tT_Break_list_all[x].ttmD_Id && promise.tT_Break_list_all[x].asmcL_Id == $scope.albumNameArray1[a].asmcL_Id && $scope.period_break_list[(i - 1)].ped_id == promise.tT_Break_list_all[x].ttmB_AfterPeriod) {
                        //                                    newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: promise.tT_Break_list_all[x].ttmB_BreakName, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                        //                                }
                        //                                if ($scope.day_list[j].ttmD_Id == promise.tT_Break_list_all[x].ttmD_Id && promise.tT_Break_list_all[x].asmcL_Id == $scope.albumNameArray1[a].asmcL_Id && $scope.period_break_list[(i - 1)].ped_id != promise.tT_Break_list_all[x].ttmB_AfterPeriod) {
                        //                                    newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: 'No', pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                        //                                }
                        //                            }
                        //                        }
                        //                        else {
                        //                            newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: ' ', pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                        //                        }
                        //                    }
                        //                    temp_array.push(newCol);
                        //                    newCol = "";
                        //                    count = 0;
                        //                }
                        //                $scope.table_list.push(temp_array);
                        //            }
                        //                $scope.table_list_cls_sec_wise.push({ array: $scope.table_list, ped_list: $scope.period_break_list, header: "Class : " + $scope.albumNameArray1[a].asmcL_ClassName + " & Section : " + $scope.albumNameArray2[b].asmC_SectionName, id: $scope.albumNameArray1[a].asmcL_Id + ':' + $scope.albumNameArray2[b].asmS_Id });
                                 
                        //        }
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

            $scope.all_check();
            $scope.all_check1();
            apiService.getDATA("CLGTTCourseWiseReport/getdetails").
       then(function (promise) {
           $scope.year_list = promise.acayear;
           $scope.categorylst = promise.categorylist;
           $scope.section_list = promise.sectionlist;
           $scope.periodslst = promise.periodslst;

       })
        };

        //to get class by category      
        $scope.ttmC_Id = "";
        $scope.asmaY_Id = "";
     


        //TO clear  data
        $scope.clearid = function () {

            $state.reload();
        };


    }

})();